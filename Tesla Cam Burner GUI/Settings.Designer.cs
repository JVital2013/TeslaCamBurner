namespace Tesla_Cam_Burner_GUI
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ffmpegPathLabel = new Label();
            ffmpegPathBox = new TextBox();
            ffmpegBrowse = new Button();
            saveButton = new Button();
            fontBrowse = new Button();
            fontBox = new TextBox();
            fontLabel = new Label();
            includeLocation = new CheckBox();
            SuspendLayout();
            // 
            // ffmpegPathLabel
            // 
            ffmpegPathLabel.AutoSize = true;
            ffmpegPathLabel.Location = new Point(8, 15);
            ffmpegPathLabel.Name = "ffmpegPathLabel";
            ffmpegPathLabel.Size = new Size(50, 15);
            ffmpegPathLabel.TabIndex = 0;
            ffmpegPathLabel.Text = "FFmpeg";
            // 
            // ffmpegPathBox
            // 
            ffmpegPathBox.Location = new Point(64, 12);
            ffmpegPathBox.Name = "ffmpegPathBox";
            ffmpegPathBox.Size = new Size(198, 23);
            ffmpegPathBox.TabIndex = 1;
            // 
            // ffmpegBrowse
            // 
            ffmpegBrowse.Location = new Point(268, 12);
            ffmpegBrowse.Name = "ffmpegBrowse";
            ffmpegBrowse.Size = new Size(75, 23);
            ffmpegBrowse.TabIndex = 2;
            ffmpegBrowse.Text = "Browse...";
            ffmpegBrowse.UseVisualStyleBackColor = true;
            ffmpegBrowse.Click += ffmpegButton_Click;
            // 
            // saveButton
            // 
            saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            saveButton.DialogResult = DialogResult.OK;
            saveButton.Location = new Point(138, 98);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 3;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // fontBrowse
            // 
            fontBrowse.Location = new Point(268, 40);
            fontBrowse.Name = "fontBrowse";
            fontBrowse.Size = new Size(75, 23);
            fontBrowse.TabIndex = 6;
            fontBrowse.Text = "Browse...";
            fontBrowse.UseVisualStyleBackColor = true;
            fontBrowse.Click += fontBrowse_Click;
            // 
            // fontBox
            // 
            fontBox.Location = new Point(64, 40);
            fontBox.Name = "fontBox";
            fontBox.Size = new Size(198, 23);
            fontBox.TabIndex = 5;
            // 
            // fontLabel
            // 
            fontLabel.AutoSize = true;
            fontLabel.Location = new Point(27, 43);
            fontLabel.Name = "fontLabel";
            fontLabel.Size = new Size(31, 15);
            fontLabel.TabIndex = 4;
            fontLabel.Text = "Font";
            // 
            // includeLocation
            // 
            includeLocation.AutoSize = true;
            includeLocation.Location = new Point(121, 73);
            includeLocation.Name = "includeLocation";
            includeLocation.Size = new Size(114, 19);
            includeLocation.TabIndex = 7;
            includeLocation.Text = "Include Location";
            includeLocation.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(353, 133);
            Controls.Add(includeLocation);
            Controls.Add(fontBrowse);
            Controls.Add(fontBox);
            Controls.Add(fontLabel);
            Controls.Add(saveButton);
            Controls.Add(ffmpegBrowse);
            Controls.Add(ffmpegPathBox);
            Controls.Add(ffmpegPathLabel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Settings";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label ffmpegPathLabel;
        private TextBox ffmpegPathBox;
        private Button ffmpegBrowse;
        private Button saveButton;
        private Button fontBrowse;
        private TextBox fontBox;
        private Label fontLabel;
        private CheckBox includeLocation;
    }
}