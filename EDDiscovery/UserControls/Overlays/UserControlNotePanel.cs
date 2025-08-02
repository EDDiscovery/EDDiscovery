/*
 * Copyright © 2016 - 2022 EDDiscovery development team
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
 */

using EliteDangerousCore;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlNotePanel : UserControlCommonBase
    {
        HistoryEntry lastHE;

        private Font displayfont;
        [Flags]
        enum Configuration
        {
            showSystemNotes = 1,
            showGMPNotes = 2,

        };

        Configuration config = (Configuration)(Configuration.showSystemNotes | Configuration.showGMPNotes);

        bool Config(Configuration c) { return (config & c) != 0; }

        public UserControlNotePanel()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateToolstrip(contextMenuStrip);
        }

        protected override void Init()
        {
            DBBaseName = "NotePanel";

            config = (Configuration)GetSetting("Config", (int)config);

            displayfont = ExtendedControls.Theme.Current.GetFont;

            DiscoveryForm.OnHistoryChange += OnHistoryChange;
            DiscoveryForm.OnNoteChanged += OnNoteChange;

        }

        protected override void LoadLayout()
        {
        }

        protected override void Closing()
        {
            DiscoveryForm.OnHistoryChange -= OnHistoryChange;
            DiscoveryForm.OnNoteChanged -= OnNoteChange;
            PutSetting("Config", (int)config);
        }

        public override bool SupportTransparency { get { return true; } }
        protected override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            Display(lastHE);
        }

        protected override void TransparencyModeChanged(bool on)
        {
            Display(lastHE);
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            Display(he);
        }

        private void OnNoteChange(Object sender, HistoryEntry he)
        {
            Display(lastHE);
        }

        protected override void InitialDisplay()
        {
            Display(DiscoveryForm.History.GetLast);
        }

        private void OnHistoryChange()            // when user clicks around..  HE may be null here
        {
            Display(DiscoveryForm.History.GetLast);
        }

        void Display(HistoryEntry he)
        {
            pictureBox.ClearImageList();

            lastHE = he;

            if (he != null)
            {
                HistoryEntry hefsd = DiscoveryForm.History.GetLastHistoryEntry(x => x.IsFSDCarrierJump, he);

                if (hefsd != null)
                {
                    Color textcolour = IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
                    Color backcolour = IsTransparentModeOn ? Color.Transparent : this.BackColor;

                    string botline = "";

                    if (Config(Configuration.showSystemNotes))
                    {
                        for (int pos = hefsd.Index; pos < DiscoveryForm.History.Count && (pos==hefsd.Index || !DiscoveryForm.History[pos].IsFSDCarrierJump); pos++)
                        {
                            HistoryEntry cur = DiscoveryForm.History[pos];
                            string notetext = cur.GetNoteText();
                            if ( notetext.HasChars())
                                botline += notetext.WordWrap(60) + Environment.NewLine;
                        }
                    }

                    if (Config(Configuration.showGMPNotes))
                    {
                        var gmo = DiscoveryForm.GalacticMapping.FindSystem(hefsd.System.Name);
                        if (gmo != null)
                            botline = ("GMP: " + gmo.Description).WordWrap(60) + Environment.NewLine;
                    }

                    pictureBox.AddTextAutoSize(
                        new Point(0, 5),
                        new Size(10000, 10000),
                        botline,
                        displayfont,
                        textcolour,
                        backcolour,
                        1.0F);
                }
            }

            pictureBox.Render();
        }

        private void miSystemNotes_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showSystemNotes, ((ToolStripMenuItem)sender).Checked, true);
            Display(lastHE);
        }

        private void miGMPNotes_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showGMPNotes, ((ToolStripMenuItem)sender).Checked, true);
            Display(lastHE);
        }

        void FlipConfig(Configuration item, bool ch, bool redisplay = false)
        {
            if (ch)
                config = (Configuration)((int)config | (int)item);
            else
                config = (Configuration)((int)config & ~(int)item);
        }


    }
}
