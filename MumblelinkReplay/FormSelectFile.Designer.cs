using System.Windows.Forms;

namespace MumblelinkReplay
{
    partial class FormSelectFile
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
            this.txtMumbleFile = new System.Windows.Forms.TextBox();
            this.txtVideoFile = new System.Windows.Forms.TextBox();
            this.btnSelectMumble = new System.Windows.Forms.Button();
            this.btnSelectVideo = new System.Windows.Forms.Button();
            this.btnReplay = new System.Windows.Forms.Button();
            this.dlgMumble = new System.Windows.Forms.OpenFileDialog();
            this.dlgVideo = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // txtMumbleFile
            // 
            this.txtMumbleFile.Location = new System.Drawing.Point(12, 12);
            this.txtMumbleFile.Name = "txtMumbleFile";
            this.txtMumbleFile.Size = new System.Drawing.Size(335, 23);
            this.txtMumbleFile.TabIndex = 0;
            // 
            // txtVideoFile
            // 
            this.txtVideoFile.Location = new System.Drawing.Point(12, 41);
            this.txtVideoFile.Name = "txtVideoFile";
            this.txtVideoFile.Size = new System.Drawing.Size(335, 23);
            this.txtVideoFile.TabIndex = 1;
            // 
            // btnSelectMumble
            // 
            this.btnSelectMumble.Location = new System.Drawing.Point(353, 12);
            this.btnSelectMumble.Name = "btnSelectMumble";
            this.btnSelectMumble.Size = new System.Drawing.Size(147, 23);
            this.btnSelectMumble.TabIndex = 2;
            this.btnSelectMumble.Text = "Select Mumble Record";
            this.btnSelectMumble.UseVisualStyleBackColor = true;
            this.btnSelectMumble.Click += new System.EventHandler(this.btnSelectMumble_Click);
            // 
            // btnSelectVideo
            // 
            this.btnSelectVideo.Location = new System.Drawing.Point(353, 41);
            this.btnSelectVideo.Name = "btnSelectVideo";
            this.btnSelectVideo.Size = new System.Drawing.Size(147, 23);
            this.btnSelectVideo.TabIndex = 3;
            this.btnSelectVideo.Text = "Select Video File";
            this.btnSelectVideo.UseVisualStyleBackColor = true;
            this.btnSelectVideo.Click += new System.EventHandler(this.btnSelectVideo_Click);
            // 
            // btnReplay
            // 
            this.btnReplay.Location = new System.Drawing.Point(404, 78);
            this.btnReplay.Name = "btnReplay";
            this.btnReplay.Size = new System.Drawing.Size(96, 23);
            this.btnReplay.TabIndex = 4;
            this.btnReplay.Text = "Start Replay";
            this.btnReplay.UseVisualStyleBackColor = true;
            this.btnReplay.Click += new System.EventHandler(this.btnReplay_Click);
            // 
            // dlgMumble
            // 
            this.dlgMumble.FileName = "mumbleFile";
            this.dlgMumble.Filter = "Recordings|*.mlrec";
            // 
            // dlgVideo
            // 
            this.dlgVideo.FileName = "videoFile";
            // 
            // FormSelectFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 113);
            this.Controls.Add(this.btnReplay);
            this.Controls.Add(this.btnSelectVideo);
            this.Controls.Add(this.btnSelectMumble);
            this.Controls.Add(this.txtVideoFile);
            this.Controls.Add(this.txtMumbleFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormSelectFile";
            this.Text = "Mumblelink Replay";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtMumbleFile;
        private TextBox txtVideoFile;
        private Button btnSelectMumble;
        private Button btnSelectVideo;
        private Button btnReplay;
        private OpenFileDialog dlgMumble;
        private OpenFileDialog dlgVideo;
    }
}