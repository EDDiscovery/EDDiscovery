/*
 * Copyright © 2022 - 2023 EDDiscovery development team
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

        protected override void Init()
        {
            isHistoric = GetSetting(dbHistoricMatsSave, false);
            chkNotHistoric.Checked = !isHistoric;
            this.chkNotHistoric.CheckedChanged += new System.EventHandler(this.chkNotHistoric_CheckedChanged);

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, false);
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;     // install after setup
            extCheckBoxMoreInfo.Checked = GetSetting(dbMoreInfo, false);
            extCheckBoxMoreInfo.Click += extCheckBoxMoreInfo_Click;

            List<string> engineers = Recipes.EngineeringRecipes.SelectMany(r => r.Engineers).Distinct().ToList();
            engineers.Sort();
            efs = new RecipeFilterSelector(engineers);
            efs.UC.AddGroupItem(string.Join(";",ItemData.ShipEngineers()), "Ship Engineers");
            efs.UC.AddGroupItem(string.Join(";", ItemData.OnFootEngineers()), "On Foot Engineers");
            efs.UC.AddGroupItem("Guardian;Guardian Weapons;Human;Special Effect;Suit;Weapon;", "Other Enginnering");
            efs.SaveSettings += (newvalue, e) => {
                string prevsetting = GetSetting(dbEngFilterSave,"All");
                if (prevsetting != newvalue)
                {
                    PutSetting(dbEngFilterSave, newvalue);
                    SetupDisplay();
                    UpdateDisplay();
                }
            };

            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip,this);

            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
            DiscoveryForm.OnHistoryChange += RefreshData;
        }

        protected override void Closing()
        {
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnHistoryChange -= RefreshData;

            PutSetting(dbHistoricMatsSave, isHistoric);
        }

        protected override void InitialDisplay()
        {
            SetupDisplay();
            RefreshData();
        }

        private void RefreshData()
        {
            if (isHistoric)
            {
                RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());
            }
            else
            {
                last_he = DiscoveryForm.History.GetLast;
                UpdateDisplay();
            }
        }

        // travel history cursor moved, either by user or via RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());
        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            if (isHistoric)
            {
                last_he = he;
                UpdateDisplay();
            }
        }

        // new entry
        private void Discoveryform_OnNewEntry(HistoryEntry he)
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

            List<string> engineers = Recipes.EngineeringRecipes.SelectMany(r => r.Engineers).Distinct().ToList();
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
                    ep.SaveSettings += () => { PutSetting(dbWSave + "_" + name, ep.WantedPerRecipe.ToString(",")); };
                    ep.AskForRedisplay += () => { UpdateDisplay(); };

                    ep.ColumnSetupChanged += (panel) =>
                    {
                        //System.Diagnostics.Debug.WriteLine($"Panel {panel.Name} changed");
                        panel.SaveDGV(colsetting);      

                        foreach (var p in engineerpanels)
                        {
                            if (p != panel)
                            {
                                p.LoadDGV(colsetting);
                            }
                        }
                    };

                    panelEngineers.Controls.Add(ep);

                    int panelvspacing = ep.GetVSize(extCheckBoxMoreInfo.Checked);

                    // need to set bounds after adding, for some reason
                    ep.Bounds = new Rectangle(0, vpos, panelEngineers.Width - panelEngineers.ScrollBarWidth - 4, panelvspacing);

                    engineerpanels.Add(ep);

                    ep.InstallColumnEvents();

                    vpos += panelvspacing + 4;
                    //       System.Diagnostics.Debug.WriteLine($"Made {name} Complete {BaseUtils.AppTicks.TickCountLap("s1")}");
                }
            }

            panelEngineers.ResumeLayout();
            //System.Diagnostics.Debug.WriteLine($"Setup Complete {BaseUtils.AppTicks.TickCountLap("s1")}");

        }

        // last_he is the position, may be nul
        public void UpdateDisplay()
        {
            //System.Diagnostics.Debug.WriteLine($"Update {BaseUtils.AppTicks.TickCountLap("s2", true)}");

            var lastengprog = DiscoveryForm.History.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.EngineerProgress, last_he); // may be null
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

                var mcllist = last_he != null ? DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(last_he.MaterialCommodity) : null;

                List<HistoryEntry> crafts = null;

                if ( last_he != null)
                {
                    if (ep.Name.Contains("Guardian") || ep.Name.Equals("Human"))
                        crafts = DiscoveryForm.History.Engineering.Get(last_he.Engineering, EngineerCrafting.TechBrokerID);
                    else
                        crafts = DiscoveryForm.History.Engineering.Get(last_he.Engineering, ep.Name);
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

        private void chkNotHistoric_CheckedChanged(object sender, EventArgs e)
        {
            isHistoric = !chkNotHistoric.Checked;
            RefreshData();
        }

        private void extButtonPushResources_Click(object sender, EventArgs e)
        {
            Dictionary<MaterialCommodityMicroResourceType, int> resourcelist = new Dictionary<MaterialCommodityMicroResourceType, int>();
            foreach (var p in engineerpanels)
            {
                foreach( var kvp in p.NeededResources.EmptyIfNull() )
                {
                    if (resourcelist.TryGetValue(kvp.Key, out int value))
                    {
                        resourcelist[kvp.Key] = value + kvp.Value;
                    }
                    else
                        resourcelist[kvp.Key] = kvp.Value;
                }
            }

            if (resourcelist.Count > 0)
            {
                var req = new UserControlCommonBase.PushResourceWantedList() { Resources = resourcelist };
                RequestPanelOperationOpen(PanelInformation.PanelIDs.Resources, req);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            foreach (var x in engineerpanels)
            {
                x.Clear();
            }

            UpdateDisplay();
        }

        #endregion

    }
}
