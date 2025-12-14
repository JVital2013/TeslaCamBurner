using System.Diagnostics;
using TeslaCamBurner;

namespace Tesla_Cam_Burner_GUI
{
    public partial class MainWindow : Form
    {
        private VideoBurner videoBurner;
        private Parser fileParser;
        private string loadedFile = "";
        private string invalidFileText = "No file loaded";
        private bool validInput = false;
        private uint numFrames = 0;
        private uint seiFrames = 0;

        private readonly DataReceivedEventHandler progressHandler;
        private readonly EventHandler ffmpegExited;

        public MainWindow()
        {
            InitializeComponent();

            progressHandler = new((object obj, DataReceivedEventArgs args) =>
            {
                if (args.Data == null) return;
                string[] valueParts = args.Data.Split('=');
                if (valueParts[0] != "frame") return;
                this.BeginInvoke(new Action(() => { progressBar.Value = (int)((double.Parse(valueParts[1]) / (double)numFrames) * 100); }));
            });

            ffmpegExited = new((object? obj, EventArgs args) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    settingsButton.Enabled = true;
                    saveButton.Enabled = true;
                    inputFileBox.Enabled = true;
                    inputBrowse.Enabled = true;

                    if (obj == null)
                    {
                        progressBar.Value = 0;
                        MessageBox.Show("Unable to retrieve conversion status", "Tesla Cam Burner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if(((Process)obj).ExitCode == 0)
                    {
                        MessageBox.Show("Exported video file!", "Tesla Cam Burner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        progressBar.Value = 100;
                    }
                    else
                    {
                        MessageBox.Show("An Error occured exporting the video", "Tesla Cam Burner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }));
            });

            fileParser = new();
            videoBurner = new(
                ffmpegExited,
                progressHandler,
                Properties.Settings.Default.ffmpeg_path,
                Properties.Settings.Default.font_path,
                Properties.Settings.Default.show_latlon
            );
        }

        private void inputBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Tesla Dashcam mp4";
            openFileDialog.Filter = "Tesla MP4 (*.mp4)|*.mp4";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                inputFileBox.Text = openFileDialog.FileName;
                LoadFile();
                if (!validInput)
                {
                    MessageBox.Show(invalidFileText, "Tesla Cam Burner", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            Settings settingsDialog = new();
            if (settingsDialog.ShowDialog() == DialogResult.OK)
            {
                videoBurner = new(
                    ffmpegExited,
                    progressHandler,
                    Properties.Settings.Default.ffmpeg_path,
                    Properties.Settings.Default.font_path,
                    Properties.Settings.Default.show_latlon
                );
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (inputFileBox.Text != loadedFile) LoadFile();
            if (!validInput)
            {
                MessageBox.Show(invalidFileText, "Tesla Cam Burner", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save video with info";
            saveFileDialog.Filter = "Standard MP4 (*.mp4)|*.mp4";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            settingsButton.Enabled = false;
            saveButton.Enabled = false;
            inputBrowse.Enabled = false;
            inputFileBox.Enabled = false;
            videoBurner.CreateVideo(inputFileBox.Text, saveFileDialog.FileName);
        }

        private void LoadFile()
        {
            loadedFile = inputFileBox.Text;
            SeiMetadata?[] seiMetadata = [];
            ContainerInfo containerInfo;

            try
            {
                seiMetadata = fileParser.Parse(loadedFile);
                containerInfo = new ContainerInfo(loadedFile);
            }
            catch (Exception ex)
            {
                invalidFileText = ex.Message;
                metadataVersionDisplay.Text = "--";
                imageFramesDisplay.Text = "--";
                seiFramesDisplay.Text = "--";
                widthDisplay.Text = "--";
                heightDisplay.Text = "--";
                validInput = false;
                return;
            }

            numFrames = (uint)containerInfo.durations.Count;
            seiFrames = 0;

            bool gotMetadataVersion = false;
            foreach (SeiMetadata? thisData in seiMetadata)
            {
                if (thisData != null)
                {
                    seiFrames++;
                    if (!gotMetadataVersion)
                    {
                        metadataVersionDisplay.Text = thisData.Version.ToString();
                        gotMetadataVersion = true;
                    }
                }
            }

            seiFramesDisplay.Text = seiFrames.ToString();
            imageFramesDisplay.Text = containerInfo.durations.Count.ToString();
            widthDisplay.Text = containerInfo.width.ToString();
            heightDisplay.Text = containerInfo.height.ToString();

            // Do nothing if no sei data
            if (seiFrames == 0)
            {
                invalidFileText = "No SEI metadata! Either the car was not running, or this is not a Tesla MP4";
                validInput = false;
                return;
            }

            // Allow encoding - consider the input valid - but warn user
            if (metadataVersionDisplay.Text != "1")
            {
                MessageBox.Show($"This program only supports Tesla metadata version 1. This file uses version {metadataVersionDisplay.Text}. There may be unexpected results!", "Tesla Cam Burner", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Warn if resolution is different than expected (need samples from HW4?)
            if (containerInfo.width != 1280 || containerInfo.height != 960)
            {
                MessageBox.Show($"This program expects a resolution of 1280x960, but this file is {containerInfo.width}x{containerInfo.height}. Please open an issue on GitHub with a sample file so this video size can be supported. There may be unexpected results!", "Tesla Cam Burner", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            validInput = true;
        }
    }
}
