using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MumblelinkReplay
{
    public partial class FormVideoPlayback : Form
    {
        private string RecordingPath;
        private string VideoPath;

        public FormVideoPlayback(string recordingPath, string videoPath)
        {
            RecordingPath = recordingPath;
            VideoPath = videoPath;
            InitializeComponent();
        }

        private void FormVideoPlayback_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
