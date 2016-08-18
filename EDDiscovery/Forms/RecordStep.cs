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
        public long HoldHere = 0;
        public string Msg = "";

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
            textBoxPauseHere.Text = "0";
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

        private void textBoxFly_TextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = ValidateData();
        }

        private void textBoxPan_TextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = ValidateData();
        }

        private void textBoxDelta_TextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = ValidateData();
        }

        private void textBoxMessage_TextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = ValidateData();
        }

        private bool ValidateData()
        {
            Msg = textBoxMessage.Text;
            bool okay = long.TryParse(textBoxDelta.Text, out Elapsed) && long.TryParse(textBoxPan.Text, out Pan) && long.TryParse(textBoxFly.Text, out Fly);

            if ( okay )
            {
                if (textBoxPauseHere.Text.Length == 0)
                    HoldHere = 0;
                else
                {
                    okay = long.TryParse(textBoxPauseHere.Text, out HoldHere);
                    if (okay && HoldHere == 0)
                        HoldHere = (long)Math.Max(Pan, Fly);
                }
            }

            return okay;
        }
    }
}
