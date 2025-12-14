namespace Tesla_Cam_Burner_GUI
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            ffmpegPathBox.Text = Properties.Settings.Default.ffmpeg_path;
            fontBox.Text = Properties.Settings.Default.font_path;
            includeLocation.Checked = Properties.Settings.Default.show_latlon;
            this.AcceptButton = saveButton;
        }

        private void ffmpegButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select ffmpeg executable";
            openFileDialog.Filter = "Application (*.exe)|*.exe";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the path of the selected file
                ffmpegPathBox.Text = openFileDialog.FileName;
            }
        }

        private void fontBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select font for overlay";
            openFileDialog.Filter = "TrueType Font (*.ttf)|*.ttf";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the path of the selected file
                fontBox.Text = openFileDialog.FileName;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ffmpeg_path = ffmpegPathBox.Text;
            Properties.Settings.Default.font_path = fontBox.Text;
            Properties.Settings.Default.show_latlon = includeLocation.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
