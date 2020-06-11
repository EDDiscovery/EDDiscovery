/*
 * Copyright © 2017 EDDiscovery development team
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
using EliteDangerousCore.DB;
using EliteDangerousCore;
using EDDiscovery.Forms;

namespace EDDiscovery.UserControls
{
    // scale to window width - split list in two if too long - maybe the text thing could do this
    // option to hide during dock
    // option to be verbose or terse

    public partial class UserControlMissionOverlay :   UserControlCommonBase
    {
        private  HistoryEntry currentHE;
        private string DBSelections { get { return DBName("MissionOverlay", "Sel"); } }

        [Flags]
        enum SelectionBits
        {
            None = 0,
            StartDate = 1,
            MissionDescription = 2,
            RewardInfo = 4,
            FactionInfo = 8,
            MissionName = 16,
            EndDate = 32,
            Default = MissionName | StartDate | EndDate | MissionDescription | RewardInfo | FactionInfo
        }

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            Display(currentHE);
        }

        bool debug_followcursor = false;        // only for debugging, normally locked to last entry

        #region Init

        public UserControlMissionOverlay()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            BaseUtils.Translator.Instance.Translate(this);

            SelectionBits sel = (SelectionBits)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DBSelections, (int)SelectionBits.Default);

            missionDescriptionToolStripMenuItem.Checked = (sel & SelectionBits.MissionName) != SelectionBits.None;
            startDateToolStripMenuItem.Checked = (sel & SelectionBits.StartDate) != SelectionBits.None;
            endDateToolStripMenuItem.Checked = (sel & SelectionBits.StartDate) != SelectionBits.None;
            factionInformationToolStripMenuItem.Checked = (sel & SelectionBits.FactionInfo) != SelectionBits.None;
            missionDescriptionToolStripMenuItem.Checked = (sel & SelectionBits.MissionDescription) != SelectionBits.None;
            rewardToolStripMenuItem.Checked = (sel & SelectionBits.RewardInfo) != SelectionBits.None;

            // TAGS relate controls to selection bits
            missionNameToolStripMenuItem.Tag = SelectionBits.MissionName;
            startDateToolStripMenuItem.Tag = SelectionBits.StartDate;
            endDateToolStripMenuItem.Tag = SelectionBits.EndDate;
            factionInformationToolStripMenuItem.Tag = SelectionBits.FactionInfo;
            missionDescriptionToolStripMenuItem.Tag = SelectionBits.MissionDescription;
            rewardToolStripMenuItem.Tag = SelectionBits.RewardInfo;

            missionNameToolStripMenuItem.Click += new System.EventHandler(this.Selection_Click);
            startDateToolStripMenuItem.Click += new System.EventHandler(this.Selection_Click);
            endDateToolStripMenuItem.Click += new System.EventHandler(this.Selection_Click);
            factionInformationToolStripMenuItem.Click += new System.EventHandler(this.Selection_Click);
            missionDescriptionToolStripMenuItem.Click += new System.EventHandler(this.Selection_Click);
            rewardToolStripMenuItem.Click += new System.EventHandler(this.Selection_Click);

            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
        }

        public override void LoadLayout()
        {
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            if (debug_followcursor)
                uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged; //DEBUG

            Resize += UserControlMissionOverlay_Resize;
        }

        private void Discoveryform_OnHistoryChange(HistoryList hl)
        {
            Display(hl.GetLast);
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            Display(he);
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            Display(he);
        }

        private void UserControlMissionOverlay_Resize(object sender, EventArgs e)
        {
            Display(currentHE);
        }

        public override void Closing()
        {
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            if (debug_followcursor)
                uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
        }

        #endregion

        #region Implementation

        public override void InitialDisplay()
        {
            Display(discoveryform.history.GetLast);
        }

        private void Display(HistoryEntry he)
        {
            currentHE = he;

            MissionList ml = currentHE?.MissionList;

            pictureBox.ClearImageList();

            if ( ml != null )
            {
                Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                Color backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                DateTime hetime = currentHE.EventTimeUTC;

                List<MissionState> mcurrent = ml.GetAllCurrentMissions(hetime);

                int vpos = 4;
                Font displayfont = discoveryform.theme.GetFont;

                foreach (MissionState ms in mcurrent)
                {
                    string text = "";

                    if (missionNameToolStripMenuItem.Checked)
                        text += JournalFieldNaming.ShortenMissionName(ms.Mission.Name);

                    if (missionDescriptionToolStripMenuItem.Checked && ms.Mission.LocalisedName.HasChars() )
                        text = text.AppendPrePad(ms.Mission.LocalisedName, ", ");

                    if (startDateToolStripMenuItem.Checked)
                        text = text.AppendPrePad( EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(ms.Mission.EventTimeUTC).ToString(), ", ");

                    if (endDateToolStripMenuItem.Checked)
                        text = text.AppendPrePad(EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(ms.Mission.Expiry).ToString(), startDateToolStripMenuItem.Checked ? "-" : ", ");

                    string mainpart = BaseUtils.FieldBuilder.Build(
                                        "< ", ms.DestinationSystemStation(),
                                        " ", factionInformationToolStripMenuItem.Checked ? ms.Mission.TargetFaction : null,
                                        "< ", ms.Mission.TargetLocalised,
                                        "< ", ms.Mission.KillCount?.ToString("N") ?? null,
                                        " ", ms.Mission.CommodityLocalised,
                                        "< ", ms.Mission.Count,
                                        " Left ".T(EDTx.UserControlMissionOverlay_IL), ms.CargoDepot?.ItemsToGo
                                        );

                    text = text.AppendPrePad(mainpart, ", ");

                    if (rewardToolStripMenuItem.Checked && ms.Mission.Reward.HasValue && ms.Mission.Reward > 0 )
                        text = text.AppendPrePad( BaseUtils.FieldBuilder.Build(" ;cr", ms.Mission.Reward.GetValueOrDefault().ToString("N0")) , ", ");

                    StringFormat frmt = new StringFormat();

                    var ie = pictureBox.AddTextAutoSize(new Point(10, vpos), new Size(this.Width-20, this.Height), text, displayfont, textcolour, backcolour, 1.0F, frmt:frmt);
                    vpos = ie.Location.Bottom + displayfont.ScalePixels(4);
                }
            }

            pictureBox.Render();
        }
        #endregion

        #region UI


        private void Selection_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            SelectionBits sel = (SelectionBits)(tsmi.Tag);     // tag contains bit number

            SelectionBits cur = (SelectionBits)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DBSelections, (int)SelectionBits.Default);
            cur = (cur & ~sel);
            if ( tsmi.Checked )
                cur |= sel;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DBSelections, (int)cur);

            System.Diagnostics.Debug.WriteLine("Mission overal sel code " + cur);
            Display(currentHE);
        }

        #endregion
    }
}
