namespace MumblelinkReplay
{
    partial class FormVideoPlayback
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVideoPlayback));
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.btnAdvance = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.btnToggleTick = new System.Windows.Forms.Button();
            this.btnToggleRealtime = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.SuspendLayout();
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(0, 0);
            this.axWindowsMediaPlayer1.Margin = new System.Windows.Forms.Padding(0);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(686, 390);
            this.axWindowsMediaPlayer1.TabIndex = 0;
            // 
            // btnAdvance
            // 
            this.btnAdvance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdvance.Location = new System.Drawing.Point(599, 355);
            this.btnAdvance.Name = "btnAdvance";
            this.btnAdvance.Size = new System.Drawing.Size(75, 23);
            this.btnAdvance.TabIndex = 1;
            this.btnAdvance.Text = "Advance";
            this.btnAdvance.UseVisualStyleBackColor = true;
            this.btnAdvance.Click += new System.EventHandler(this.btnAdvance_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.propertyGrid1.Location = new System.Drawing.Point(12, 12);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(415, 366);
            this.propertyGrid1.TabIndex = 2;
            // 
            // btnToggleTick
            // 
            this.btnToggleTick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggleTick.Location = new System.Drawing.Point(518, 355);
            this.btnToggleTick.Name = "btnToggleTick";
            this.btnToggleTick.Size = new System.Drawing.Size(75, 23);
            this.btnToggleTick.TabIndex = 3;
            this.btnToggleTick.Text = "Toggle Tick";
            this.btnToggleTick.UseVisualStyleBackColor = true;
            this.btnToggleTick.Click += new System.EventHandler(this.btnToggleTick_Click);
            // 
            // btnToggleRealtime
            // 
            this.btnToggleRealtime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggleRealtime.Location = new System.Drawing.Point(437, 355);
            this.btnToggleRealtime.Name = "btnToggleRealtime";
            this.btnToggleRealtime.Size = new System.Drawing.Size(75, 23);
            this.btnToggleRealtime.TabIndex = 4;
            this.btnToggleRealtime.Text = "Toggle Real";
            this.btnToggleRealtime.UseVisualStyleBackColor = true;
            this.btnToggleRealtime.Click += new System.EventHandler(this.btnToggleRealtime_Click);
            // 
            // FormVideoPlayback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 390);
            this.Controls.Add(this.btnToggleRealtime);
            this.Controls.Add(this.btnToggleTick);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.btnAdvance);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Name = "FormVideoPlayback";
            this.Text = "FormVideoPlayback";
            this.Load += new System.EventHandler(this.FormVideoPlayback_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.Button btnAdvance;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button btnToggleTick;
        private System.Windows.Forms.Button btnToggleRealtime;
    }
}