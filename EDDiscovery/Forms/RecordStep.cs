/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using OpenTK;
using System;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class RecordStep : ExtendedControls.DraggableForm
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

            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            bool winborder = theme.ApplyToFormStandardFontSize(this);
            panelTop.Visible = panelTop.Enabled = !winborder;

            BaseUtils.Translator.Instance.Translate(this);
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
                Elapsed = EDDiscovery._3DMap.MapRecorder.FlightEntry.WaitForComplete;
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
                Pos = EDDiscovery._3DMap.MapRecorder.FlightEntry.NullVector;
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
                Dir = EDDiscovery._3DMap.MapRecorder.FlightEntry.NullVector;
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

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label_index_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }
    }
}
