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
        public long Pan = 0;
        public long Fly = 0;
        public long Zoom = 0;
        public long HoldHere = 0;
        public string Msg = "";
        public long MsgTime = 0;

        public RecordStep()
        {
            InitializeComponent();
        }

        public void Init( Vector3 pos, Vector3 dir , float zoom )
        {
            textBoxLoc.Text = String.Format("{0},{1},{2}", pos.X, pos.Y, pos.Z);
            textBoxDir.Text = String.Format("{0},{1},{2}", dir.X, dir.Y, dir.Z);
            textBoxZoom.Text = String.Format("{0}", zoom);
            textBoxDelta.Text = "100";
            textBoxPan.Text = "0";
            textBoxFly.Text = "0";
            textBoxZoomTime.Text = "0";
            textBoxPauseHere.Text = "0";
            textBoxMsgTime.Text = "3000";
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

        private bool ValidateData()
        {
            Msg = textBoxMessage.Text;
            bool okay = long.TryParse(textBoxDelta.Text, out Elapsed) &&
                        long.TryParse(textBoxPan.Text, out Pan) && long.TryParse(textBoxFly.Text, out Fly) &&
                        long.TryParse(textBoxZoomTime.Text, out Zoom) &&
                        long.TryParse(textBoxMsgTime.Text, out MsgTime);

            if ( okay )
            {
                if (textBoxPauseHere.Text.Length == 0)
                    HoldHere = 0;
                else
                {
                    okay = long.TryParse(textBoxPauseHere.Text, out HoldHere);
                    if (okay && HoldHere == 0)
                    {
                        HoldHere = (long)Math.Max(Math.Max(Pan, Fly), Zoom);
                        if (HoldHere>0)
                            HoldHere += 50;
                    }
                }
            }

            return okay;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = ValidateData();
        }
    }
}
