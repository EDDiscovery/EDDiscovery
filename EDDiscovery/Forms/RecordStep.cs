using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2
{
    public partial class RecordStep : Form
    {
        public long Elapsed = 0;

        public Vector3 Pos;
        public long FlyTime = 0;

        public Vector3 Dir;
        public long PanTime = 0;

        public float Zoom;
        public long ZoomTime = 0;

        public string Msg = "";
        public long MsgTime = 0;

        public bool WaitForComplete = false;
        public bool DisplayMessageWhenComplete = false;

        private Vector3 initpos;
        private Vector3 initdir;
        private float initzoom;

        public RecordStep()
        {
            InitializeComponent();
        }

        public void Init( Vector3 pos, Vector3 dir , float zoom )
        {
            initpos = pos;
            initdir = dir;
            initzoom = zoom;

            textBoxWait.Text = "100";
            textBoxPanTime.Text = "0";
            textBoxFlyTime.Text = "0";
            textBoxZoomTime.Text = "0";
            textBoxMsgTime.Text = "0";
            checkBoxPan.Checked = true;
            checkBoxPos.Checked = true;
            checkBoxChangeZoom.Checked = true;
            ValidateData();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ValidateData()
        {
            Msg = textBoxMessage.Text;

            bool okay = true;

            textBoxWait.ReadOnly = checkBoxWaitForSlew.Checked;
            textBoxWait.Enabled = !checkBoxWaitForSlew.Checked;

            if (checkBoxWaitForSlew.Checked)
                Elapsed = EDDiscovery2._3DMap.MapRecorder.FlightEntry.WaitForComplete;
            else
                okay = okay && long.TryParse(textBoxWait.Text, out Elapsed);

            textBoxFlyTime.ReadOnly = !checkBoxPos.Checked;
            textBoxFlyTime.Enabled = checkBoxPos.Checked;
            textBoxPos.Text = (checkBoxPos.Checked) ? String.Format("{0},{1},{2}", initpos.X, initpos.Y, initpos.Z) : "N/A";

            if (checkBoxPos.Checked)
            {
                Pos = initpos;
                okay = okay && long.TryParse(textBoxFlyTime.Text, out FlyTime);
            }
            else
            {
                Pos = EDDiscovery2._3DMap.MapRecorder.FlightEntry.NullVector;
                FlyTime = 0;
            }

            textBoxPanTime.ReadOnly = !checkBoxPan.Checked;
            textBoxPanTime.Enabled = checkBoxPan.Checked;
            textBoxDir.Text = (checkBoxPan.Checked) ? String.Format("{0},{1},{2}", initdir.X, initdir.Y, initdir.Z) : "N/A";

            if (checkBoxPan.Checked)
            {
                Dir = initdir;
                okay = okay && long.TryParse(textBoxPanTime.Text, out PanTime);
            }
            else
            {
                Dir = EDDiscovery2._3DMap.MapRecorder.FlightEntry.NullVector;
                PanTime = 0;
            }

            textBoxZoomTime.ReadOnly = !checkBoxChangeZoom.Checked;
            textBoxZoomTime.Enabled = checkBoxChangeZoom.Checked;
            textBoxZoom.Text = (checkBoxChangeZoom.Checked) ? String.Format("{0}", initzoom) : "N/A";

            if (checkBoxChangeZoom.Checked)
            {
                Zoom = initzoom;
                okay = okay && long.TryParse(textBoxZoomTime.Text, out ZoomTime);
            }
            else
            {
                Zoom = 0;
                ZoomTime = 0;
            }

            okay = okay && long.TryParse(textBoxMsgTime.Text, out MsgTime);

            WaitForComplete = checkBoxWaitComplete.Checked;
            checkBoxDisplayMessageWhenComplete.Enabled = WaitForComplete;

            DisplayMessageWhenComplete = (WaitForComplete) ? checkBoxDisplayMessageWhenComplete.Checked : false;

            buttonOK.Enabled = okay;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void checkBoxWaitForSlew_CheckedChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void checkBoxGoTo_CheckedChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void checkBoxChangeDir_CheckedChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void checkBoxChangeZoom_CheckedChanged(object sender, EventArgs e)
        {
            ValidateData();
        }
    }
}
