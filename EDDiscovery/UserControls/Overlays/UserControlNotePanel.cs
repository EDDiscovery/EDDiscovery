/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlNotePanel : UserControlCommonBase
    {
        private string DbSave { get { return DBName("NotePanel" ); } }

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
        }

        public override void Init()
        {
            config = (Configuration)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbSave + "Config", (int)config);

            displayfont = discoveryform.theme.GetFont;

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += NewEntry;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += DisplaySelected;
        }

        public override void InitialDisplay()
        {
            DisplaySelected(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= DisplaySelected;
            uctg = thc;
            uctg.OnTravelSelectionChanged += DisplaySelected;
        }

        private void DisplaySelected(HistoryEntry he, HistoryList hl) =>
            DisplaySelected(he, hl, true);

        private void DisplaySelected(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (he != null && he.IsFSDCarrierJump )
                Display(he);
            else
                Display(hl);
        }

        public override void Closing()
        {
            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= NewEntry;
            uctg.OnTravelSelectionChanged -= DisplaySelected;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DbSave + "Config", (int)config);
        }

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made..
        {
            if (he != null && he.IsFSDCarrierJump)
                Display(he);
            else
                Display(hl);
        }

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            Display(lastHE);
        }

        private void Display( HistoryList hl)            // when user clicks around..  HE may be null here
        {
            Display(hl.GetLastFSDCarrierJump);
        }

        void FlipConfig(Configuration item, bool ch, bool redisplay = false)
        {
            if (ch)
                config = (Configuration)((int)config | (int)item);
            else
                config = (Configuration)((int)config & ~(int)item);
        }

        void Display(HistoryEntry he)
        {
            pictureBox.ClearImageList();

            lastHE = he;

            if (he != null)
            {
                Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                Color backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                discoveryform.history.FillEDSM(he); // Fill in any EDSM info we have, force it to try again.. in case system db updated
                string botline = "";
                if (Config(Configuration.showGMPNotes))
                {
                    var gmo = discoveryform.galacticMapping.Find(he.System.Name);
                    if (gmo != null)
                        botline = ("GMP: " + gmo.description).WordWrap(60) + Environment.NewLine;
                }

                if (Config(Configuration.showSystemNotes))
                {
                    if (he.snc != null)
                        botline += (he.snc.Note).WordWrap(60) + Environment.NewLine;
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
    }
}
