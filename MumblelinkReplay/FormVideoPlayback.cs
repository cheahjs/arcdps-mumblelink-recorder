using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MumblelinkReplay.Record;

namespace MumblelinkReplay
{
    public partial class FormVideoPlayback : Form
    {
        private string RecordingPath;
        private string VideoPath;
        private SharedMemory SharedMemory = new SharedMemory();
        private Record.Record Record;
        private Timer RecordTimer = new Timer();
        private Timer RealtimeTimer = new Timer();
        private ulong LastUpdateTime;

        public FormVideoPlayback(string recordingPath, string videoPath)
        {
            RecordingPath = recordingPath;
            VideoPath = videoPath;
            Record = new Record.Record(RecordingPath);
            SharedMemory.Update(Record.GetPrevLinkedMem());
            InitializeComponent();
            LastUpdateTime = Record.LogStartMonotonicMicros;
            RealtimeTimer.Interval = 500;
            RealtimeTimer.Tick += RealtimeTick;
            RecordTimer.Interval = 1000 / 2;
            RecordTimer.Tick += btnAdvance_Click;
        }

        private void FormVideoPlayback_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = Record.GetPrevLinkedMem();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        private void RealtimeTick(object sender, EventArgs e)
        {
            var newUpdate = Record.NextUpdate();
            if (newUpdate == null)
            {
                RealtimeTimer.Stop();
                return;
            }
            propertyGrid1.SelectedObject = newUpdate.Value.LinkedMem;
            SharedMemory.Update(newUpdate.Value.LinkedMem);
            RealtimeTimer.Interval = (int)((newUpdate.Value.TimestampMicro - LastUpdateTime) / 1000);
            LastUpdateTime = newUpdate.Value.TimestampMicro;
        }

        private void btnAdvance_Click(object sender, EventArgs e)
        {
            var newUpdate = Record.NextUpdate();
            if (newUpdate == null) { return; }

            propertyGrid1.SelectedObject = newUpdate.Value.LinkedMem;
            SharedMemory.Update(newUpdate.Value.LinkedMem);
            LastUpdateTime = newUpdate.Value.TimestampMicro;
        }

        private void btnToggleTick_Click(object sender, EventArgs e)
        {
            RecordTimer.Enabled = !RecordTimer.Enabled;
        }

        private void btnToggleRealtime_Click(object sender, EventArgs e)
        {
            RealtimeTimer.Enabled = !RealtimeTimer.Enabled;
        }
    }
}
