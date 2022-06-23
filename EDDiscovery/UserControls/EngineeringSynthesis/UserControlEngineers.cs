/*
 * Copyright © 2022 - 2022 EDDiscovery development team
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
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlEngineers : UserControlCommonBase
    {
        private List<EngineerStatusPanel> engineerpanels;
        private bool isHistoric = false;
        private HistoryEntry last_he = null;
        RecipeFilterSelector efs;

        private string dbHistoricMatsSave = "HistoricMaterials";
        private string dbWordWrap = "WordWrap";
        private string dbEngFilterSave = "EngineerFilter";
        private string dbWSave = "Wanted";
        private string dbMoreInfo = "MoreInfo";

        public UserControlEngineers()
        {
            InitializeComponent();
            DBBaseName = "Engineers";
        }

        public override void Init()
        {
            isHistoric = GetSetting(dbHistoricMatsSave, false);

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, false);
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;     // install after setup
            extCheckBoxMoreInfo.Checked = GetSetting(dbMoreInfo, false);
            extCheckBoxMoreInfo.Click += extCheckBoxMoreInfo_Click;

            List<string> engineers = Recipes.EngineeringRecipes.SelectMany(r => r.engineers).Distinct().ToList();
            engineers.Sort();
            efs = new RecipeFilterSelector(engineers);
            efs.AddGroupOption(string.Join(";",ItemData.ShipEngineers()), "Ship Engineers");
            efs.AddGroupOption(string.Join(";", ItemData.OnFootEngineers()), "On Foot Engineers");
            efs.AddGroupOption("Guardian;Guardian Weapons;Human;Special Effect;Suit;Weapon;", "Other Enginnering");
            efs.SaveSettings += (newvalue, e) => {
                string prevsetting = GetSetting(dbEngFilterSave,"All");
                if (prevsetting != newvalue)
                {
                    PutSetting(dbEngFilterSave, newvalue);
                    SetupDisplay();
                    UpdateDisplay();
                }
            };

            var enumlisttt = new Enum[] { EDTx.UserControlEngineers_buttonFilterEngineer_ToolTip, EDTx.UserControlEngineers_extCheckBoxWordWrap_ToolTip, EDTx.UserControlEngineers_extCheckBoxMoreInfo_ToolTip };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= UCTGChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += UCTGChanged;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += UCTGChanged;
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= UCTGChanged;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;

            PutSetting(dbHistoricMatsSave, isHistoric);
        }

        internal void SetHistoric(bool newVal)
        {
            isHistoric = newVal;
            if (isHistoric)
            {
                last_he = uctg.GetCurrentHistoryEntry;
            }
            else
            {
                last_he = discoveryform.history.GetLast;
            }
            UpdateDisplay();
        }

        public override void InitialDisplay()
        {
            SetupDisplay();
            last_he = isHistoric ? uctg.GetCurrentHistoryEntry : discoveryform.history.GetLast;
            UpdateDisplay();
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            last_he = isHistoric ? uctg.GetCurrentHistoryEntry : discoveryform.history.GetLast;
            UpdateDisplay();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            if (!isHistoric)        // only track new items if not historic
            {
                last_he = he;
                if (he.journalEntry is ICommodityJournalEntry || he.journalEntry is IMaterialJournalEntry)
                {
                    UpdateDisplay();
                }
            }
        }

        private void UCTGChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (isHistoric || last_he == null)
            {
                last_he = he;
                UpdateDisplay();
            }
        }

        public void SetupDisplay()
        {
            //System.Diagnostics.Debug.WriteLine($"Setup {BaseUtils.AppTicks.TickCountLap("s1", true)}");

            if ( engineerpanels!=null)
            {
                foreach (var ep in engineerpanels)
                    ep.UnInstallEvents();

                panelEngineers.ClearControls();
                engineerpanels = null;
            }

            //System.Diagnostics.Debug.WriteLine($"Cleaned {BaseUtils.AppTicks.TickCountLap("s1")}");

            List<string> engineers = Recipes.EngineeringRecipes.SelectMany(r => r.engineers).Distinct().ToList();
            engineers.Sort();
            engineerpanels = new List<EngineerStatusPanel>();

            string engineerssetting = GetSetting(dbEngFilterSave, "All");
            string colsetting = DGVSaveName();

            panelEngineers.SuspendLayout();

            int vpos = 0;

            foreach (var name in engineers)
            {
                if (engineerssetting == "All" || engineerssetting.Contains(name))
                {
                    var ep = new EngineerStatusPanel();
                    ItemData.EngineeringInfo ei = ItemData.GetEngineerInfo(name);

                    ep.Init(name, ei, GetSetting(dbWSave + "_" + name, ""), colsetting);
                    ep.UpdateWordWrap(extCheckBoxWordWrap.Checked);

                    ep.Redisplay += () =>
                    {
                        PutSetting(dbWSave + "_" + name, ep.WantedPerRecipe.ToString(","));
                        UpdateDisplay();
                    };

                    ep.ColumnSetupChanged += (panel) =>
                    {
                        System.Diagnostics.Debug.WriteLine($"Panel {panel.Name} changed");
                        panel.SaveDGV(colsetting);      

                        foreach (var p in engineerpanels)
                        {
                            if (p != panel)
                            {
                                p.LoadDGV(colsetting);
                            }
                        }
                    };

                    System.Diagnostics.Debug.WriteLine($"Init {name}");

                    panelEngineers.Controls.Add(ep);

                    System.Diagnostics.Debug.WriteLine($"Added {name}");

                    int panelvspacing = ep.GetVSize(extCheckBoxMoreInfo.Checked);

                    // need to set bounds after adding, for some reason
                    ep.Bounds = new Rectangle(0, vpos, panelEngineers.Width - panelEngineers.ScrollBarWidth - 4, panelvspacing);

                    System.Diagnostics.Debug.WriteLine($"Bounds {name}");

                    engineerpanels.Add(ep);

                    ep.InstallColumnEvents();

                    System.Diagnostics.Debug.WriteLine($"Columns {name}");

                    vpos += panelvspacing + 4;
                    //       System.Diagnostics.Debug.WriteLine($"Made {name} Complete {BaseUtils.AppTicks.TickCountLap("s1")}");
                }
            }

            System.Diagnostics.Debug.WriteLine($"Finished Eng");

            panelEngineers.ResumeLayout();
            //System.Diagnostics.Debug.WriteLine($"Setup Complete {BaseUtils.AppTicks.TickCountLap("s1")}");

            System.Diagnostics.Debug.WriteLine($"Exit setup");
        }

        // last_he is the position, may be nul
        public void UpdateDisplay()
        {
            //System.Diagnostics.Debug.WriteLine($"Update {BaseUtils.AppTicks.TickCountLap("s2", true)}");

            var lastengprog = discoveryform.history.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.EngineerProgress, last_he); // may be null
            var system = last_he?.System;       // may be null

            for (int i = 0; i < engineerpanels.Count; i++)
            {
                var ep = engineerpanels[i];

                string engineer = ep.Name;

                string status = "";

                if (lastengprog != null && engineerpanels[i].EngineerInfo != null)      // if we have progress, and its an engineer
                {
                    var state = (lastengprog.journalEntry as EliteDangerousCore.JournalEvents.JournalEngineerProgress).Progress(engineer);
                    if (state == EliteDangerousCore.JournalEvents.JournalEngineerProgress.InviteState.UnknownEngineer)
                        state = EliteDangerousCore.JournalEvents.JournalEngineerProgress.InviteState.None;      // frontier are not telling, presume none
                    status = state.ToString();
                }

                var mcllist = last_he != null ? discoveryform.history.MaterialCommoditiesMicroResources.Get(last_he.MaterialCommodity) : null;

                List<HistoryEntry> crafts = null;

                if ( last_he != null)
                {
                    if (ep.Name.Contains("Guardian") || ep.Name.Equals("Human"))
                        crafts = discoveryform.history.Engineering.Get(last_he.Engineering, EngineeringList.TechBrokerID);
                    else
                        crafts = discoveryform.history.Engineering.Get(last_he.Engineering, ep.Name);
                }
                
                ep.UpdateStatus(status, system, mcllist,crafts);
            }

            //System.Diagnostics.Debug.WriteLine($"Update Complete {BaseUtils.AppTicks.TickCountLap("s2")}");
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if ( engineerpanels != null )
            {
                for (int i = 0; i < engineerpanels.Count; i++)
                {
                    engineerpanels[i].Width = panelEngineers.Width - panelEngineers.ScrollBarWidth - 4;
                }
            }
        }

        #region UI

        private void buttonFilterEngineer_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            efs.Open(GetSetting(dbEngFilterSave, "All"), b, this.FindForm());
        }

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
            foreach ( var p in engineerpanels.DefaultIfEmpty())
                p.UpdateWordWrap(extCheckBoxWordWrap.Checked);
        }

        private void extCheckBoxMoreInfo_Click(object sender, EventArgs e)
        {
            PutSetting(dbMoreInfo, extCheckBoxMoreInfo.Checked);
            int vpos = 0;

            panelEngineers.SuspendLayout();

            for (int i = 0; i < engineerpanels.Count; i++)
            {
                var ep = engineerpanels[i];
                int panelvspacing = ep.GetVSize(extCheckBoxMoreInfo.Checked);

                // need to set bounds after adding, for some reason
                ep.Bounds = new Rectangle(0, vpos, panelEngineers.Width - panelEngineers.ScrollBarWidth - 4, panelvspacing);
                vpos += panelvspacing + 4;
            }
            panelEngineers.ResumeLayout();
        }



            #endregion
        }
}
