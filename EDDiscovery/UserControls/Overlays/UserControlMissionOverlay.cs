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
    public partial class UserControlMissionOverlay :   UserControlCommonBase
    {
        private  HistoryEntry currentHE;

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            Display(currentHE);
        }

        #region Init

        public UserControlMissionOverlay()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            BaseUtils.Translator.Instance.Translate(this);
            //BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Display;
        }

        #endregion

        #region Implementation

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry);
        }

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            Display(he);
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
                    string text = BaseUtils.FieldBuilder.Build(
                                    "", JournalFieldNaming.ShortenMissionName(ms.Mission.Name),
                                    "< (;-", (EDDiscoveryForm.EDDConfig.DisplayUTC ? ms.Mission.EventTimeUTC : ms.Mission.EventTimeLocal),
                                    "<;)", (EDDiscoveryForm.EDDConfig.DisplayUTC ? ms.Mission.Expiry : ms.Mission.Expiry.ToLocalTime()),
                                    "< ", ms.DestinationSystemStation(),
                                    " ", ms.Mission.TargetFaction,
                                    "< ", ms.Mission.TargetLocalised,
                                    "< ", ms.Mission.KillCount?.ToString("N") ?? null,
                                    " ", ms.Mission.CommodityLocalised,
                                    "< ", ms.Mission.Count,
                                    " ", ms.Mission.Reward.GetValueOrDefault().ToString("N0")
                                    );

                    var ie = pictureBox.AddTextAutoSize(new Point(10, vpos), new Size(1200, 35), text, displayfont, textcolour, backcolour, 1.0F);
                    vpos = ie.pos.Bottom + 4;
                }
            }

            pictureBox.Render();
        }
        #endregion

        #region UI


        #endregion

    }
}
