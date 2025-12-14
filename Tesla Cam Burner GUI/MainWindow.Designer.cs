namespace Tesla_Cam_Burner_GUI
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            inputFileBox = new TextBox();
            inputFileLabel = new Label();
            inputBrowse = new Button();
            settingsButton = new Button();
            copyrightLabel = new Label();
            detailsGroup = new GroupBox();
            seiFramesDisplay = new Label();
            imageFramesDisplay = new Label();
            metadataVersionDisplay = new Label();
            seiFramesLabel = new Label();
            imageFramesLabel = new Label();
            metadataVersionLabel = new Label();
            saveButton = new Button();
            progressBar = new ProgressBar();
            widthLabel = new Label();
            widthDisplay = new Label();
            heightDisplay = new Label();
            heightLabel = new Label();
            detailsGroup.SuspendLayout();
            SuspendLayout();
            // 
            // inputFileBox
            // 
            inputFileBox.Location = new Point(74, 12);
            inputFileBox.Name = "inputFileBox";
            inputFileBox.Size = new Size(253, 23);
            inputFileBox.TabIndex = 0;
            // 
            // inputFileLabel
            // 
            inputFileLabel.AutoSize = true;
            inputFileLabel.Location = new Point(12, 15);
            inputFileLabel.Name = "inputFileLabel";
            inputFileLabel.Size = new Size(56, 15);
            inputFileLabel.TabIndex = 1;
            inputFileLabel.Text = "Input File";
            // 
            // inputBrowse
            // 
            inputBrowse.Location = new Point(333, 12);
            inputBrowse.Name = "inputBrowse";
            inputBrowse.Size = new Size(75, 23);
            inputBrowse.TabIndex = 2;
            inputBrowse.Text = "Browse...";
            inputBrowse.UseVisualStyleBackColor = true;
            inputBrowse.Click += inputBrowse_Click;
            // 
            // settingsButton
            // 
            settingsButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            settingsButton.Location = new Point(334, 159);
            settingsButton.Name = "settingsButton";
            settingsButton.Size = new Size(75, 23);
            settingsButton.TabIndex = 3;
            settingsButton.Text = "Settings";
            settingsButton.UseVisualStyleBackColor = true;
            settingsButton.Click += settingsButton_Click;
            // 
            // copyrightLabel
            // 
            copyrightLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            copyrightLabel.AutoSize = true;
            copyrightLabel.ForeColor = SystemColors.GrayText;
            copyrightLabel.Location = new Point(12, 163);
            copyrightLabel.Name = "copyrightLabel";
            copyrightLabel.Size = new Size(211, 15);
            copyrightLabel.TabIndex = 4;
            copyrightLabel.Text = "2025 Jamie Vital. Licensed under GPLv3";
            // 
            // detailsGroup
            // 
            detailsGroup.Controls.Add(heightDisplay);
            detailsGroup.Controls.Add(heightLabel);
            detailsGroup.Controls.Add(widthDisplay);
            detailsGroup.Controls.Add(widthLabel);
            detailsGroup.Controls.Add(seiFramesDisplay);
            detailsGroup.Controls.Add(imageFramesDisplay);
            detailsGroup.Controls.Add(metadataVersionDisplay);
            detailsGroup.Controls.Add(seiFramesLabel);
            detailsGroup.Controls.Add(imageFramesLabel);
            detailsGroup.Controls.Add(metadataVersionLabel);
            detailsGroup.Location = new Point(12, 41);
            detailsGroup.Name = "detailsGroup";
            detailsGroup.Size = new Size(315, 80);
            detailsGroup.TabIndex = 5;
            detailsGroup.TabStop = false;
            detailsGroup.Text = "Details";
            // 
            // seiFramesDisplay
            // 
            seiFramesDisplay.AutoSize = true;
            seiFramesDisplay.Location = new Point(265, 37);
            seiFramesDisplay.Name = "seiFramesDisplay";
            seiFramesDisplay.Size = new Size(17, 15);
            seiFramesDisplay.TabIndex = 5;
            seiFramesDisplay.Text = "--";
            // 
            // imageFramesDisplay
            // 
            imageFramesDisplay.AutoSize = true;
            imageFramesDisplay.Location = new Point(265, 19);
            imageFramesDisplay.Name = "imageFramesDisplay";
            imageFramesDisplay.Size = new Size(17, 15);
            imageFramesDisplay.TabIndex = 4;
            imageFramesDisplay.Text = "--";
            // 
            // metadataVersionDisplay
            // 
            metadataVersionDisplay.AutoSize = true;
            metadataVersionDisplay.Location = new Point(113, 19);
            metadataVersionDisplay.Name = "metadataVersionDisplay";
            metadataVersionDisplay.Size = new Size(17, 15);
            metadataVersionDisplay.TabIndex = 3;
            metadataVersionDisplay.Text = "--";
            // 
            // seiFramesLabel
            // 
            seiFramesLabel.AutoSize = true;
            seiFramesLabel.Location = new Point(193, 37);
            seiFramesLabel.Name = "seiFramesLabel";
            seiFramesLabel.Size = new Size(66, 15);
            seiFramesLabel.TabIndex = 2;
            seiFramesLabel.Text = "SEI Frames:";
            // 
            // imageFramesLabel
            // 
            imageFramesLabel.AutoSize = true;
            imageFramesLabel.Location = new Point(175, 19);
            imageFramesLabel.Name = "imageFramesLabel";
            imageFramesLabel.Size = new Size(84, 15);
            imageFramesLabel.TabIndex = 1;
            imageFramesLabel.Text = "Image Frames:";
            // 
            // metadataVersionLabel
            // 
            metadataVersionLabel.AutoSize = true;
            metadataVersionLabel.Location = new Point(6, 19);
            metadataVersionLabel.Name = "metadataVersionLabel";
            metadataVersionLabel.Size = new Size(101, 15);
            metadataVersionLabel.TabIndex = 0;
            metadataVersionLabel.Text = "Metadata Version:";
            // 
            // saveButton
            // 
            saveButton.Location = new Point(333, 47);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 74);
            saveButton.TabIndex = 6;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            progressBar.Location = new Point(12, 128);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(397, 23);
            progressBar.TabIndex = 7;
            // 
            // widthLabel
            // 
            widthLabel.AutoSize = true;
            widthLabel.Location = new Point(65, 37);
            widthLabel.Name = "widthLabel";
            widthLabel.Size = new Size(42, 15);
            widthLabel.TabIndex = 6;
            widthLabel.Text = "Width:";
            // 
            // widthDisplay
            // 
            widthDisplay.AutoSize = true;
            widthDisplay.Location = new Point(113, 37);
            widthDisplay.Name = "widthDisplay";
            widthDisplay.Size = new Size(17, 15);
            widthDisplay.TabIndex = 7;
            widthDisplay.Text = "--";
            // 
            // heightDisplay
            // 
            heightDisplay.AutoSize = true;
            heightDisplay.Location = new Point(113, 55);
            heightDisplay.Name = "heightDisplay";
            heightDisplay.Size = new Size(17, 15);
            heightDisplay.TabIndex = 9;
            heightDisplay.Text = "--";
            // 
            // heightLabel
            // 
            heightLabel.AutoSize = true;
            heightLabel.Location = new Point(61, 55);
            heightLabel.Name = "heightLabel";
            heightLabel.Size = new Size(46, 15);
            heightLabel.TabIndex = 8;
            heightLabel.Text = "Height:";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(421, 187);
            Controls.Add(progressBar);
            Controls.Add(saveButton);
            Controls.Add(detailsGroup);
            Controls.Add(copyrightLabel);
            Controls.Add(settingsButton);
            Controls.Add(inputBrowse);
            Controls.Add(inputFileLabel);
            Controls.Add(inputFileBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tesla Cam Burner";
            detailsGroup.ResumeLayout(false);
            detailsGroup.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox inputFileBox;
        private Label inputFileLabel;
        private Button inputBrowse;
        private Button settingsButton;
        private Label copyrightLabel;
        private GroupBox detailsGroup;
        private Button saveButton;
        private ProgressBar progressBar;
        private Label metadataVersionLabel;
        private Label seiFramesLabel;
        private Label imageFramesLabel;
        private Label seiFramesDisplay;
        private Label imageFramesDisplay;
        private Label metadataVersionDisplay;
        private Label widthDisplay;
        private Label widthLabel;
        private Label heightDisplay;
        private Label heightLabel;
    }
}
