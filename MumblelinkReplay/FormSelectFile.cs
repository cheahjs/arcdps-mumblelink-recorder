using System;
using System.Windows.Forms;

namespace MumblelinkReplay
{
    public partial class FormSelectFile : Form
    {
        public FormSelectFile()
        {
            InitializeComponent();
        }

        private void btnSelectMumble_Click(object sender, EventArgs e)
        {
            if (dlgMumble.ShowDialog() == DialogResult.OK)
            {
                txtMumbleFile.Text = dlgMumble.FileName;
            }
        }

        private void btnSelectVideo_Click(object sender, EventArgs e)
        {
            if (dlgVideo.ShowDialog() == DialogResult.OK)
            {
                txtVideoFile.Text = dlgVideo.FileName;
            }
        }

        private void btnReplay_Click(object sender, EventArgs e)
        {
            Hide();
            var playbackForm = new FormVideoPlayback(txtMumbleFile.Text, txtVideoFile.Text);
            playbackForm.ShowDialog();
            Close();
        }
    }
}