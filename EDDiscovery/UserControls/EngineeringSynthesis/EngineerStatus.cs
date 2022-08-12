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

using EDDiscovery.Controls;
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class EngineerStatusPanel : UserControl
    {
        public ItemData.EngineeringInfo EngineerInfo { get; private set; }      // may be null if entry not associated with an engineer
        public List<HistoryEntry> Crafts { get; private set; }  // may be null, no crafting, not an engineer, not updated yet

        public int RecipesCount { get { return dataGridViewEngineering.RowCount; } }
        public int[] WantedPerRecipe { get; private set; }

        public Action Redisplay { get; set; }

        public Action<EngineerStatusPanel> ColumnSetupChanged { get; set; }

        public void SaveDGV(string root)
        {
            dataGridViewEngineering.SaveColumnSettings(root, (a, b) => EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(a, b),
                            (c, d) => EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(c, d));
        }
        public void LoadDGV(string root)
        {
            inchange = true;
            dataGridViewEngineering.LoadColumnSettings(root, (a) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(a, int.MinValue),
                            (b) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(b, double.MinValue));
            inchange = false;
        }

        public int GetVSize(bool fullinfo)
        {
            //System.Diagnostics.Debug.Write($"VSize {pictureBoxLockImage.Size} {Font.Height}");
            return Font.ScalePixels(fullinfo ? 450 : 300);
        }

        private bool inchange = true;

        private Timer delaytime = new Timer() { Interval = 500 };       // we get a storm of column widths changing, so using a timer we reduce them to one

        public EngineerStatusPanel()
        {
            InitializeComponent();
        }

        public void Init(string name, ItemData.EngineeringInfo ei, string wantedsettings, string colsetting)
        {
            this.Name = name;
            EngineerInfo = ei;

            ExtendedControls.Theme.Current?.ApplyStd(this);
            var enumlist = new Enum[] { EDTx.UserControlEngineering_UpgradeCol, EDTx.UserControlEngineering_ModuleCol, EDTx.UserControlEngineering_LevelCol,
                            EDTx.UserControlEngineering_MaxCol, EDTx.UserControlEngineering_WantedCol, EDTx.UserControlEngineering_CraftedCol,
                            EDTx.UserControlEngineering_NotesCol, EDTx.UserControlEngineering_RecipeCol, EDTx.UserControlEngineering_EngineersCol,
                        };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, null, "UserControlEngineering");    // share IDs with Engineering panel./

            dataGridViewEngineering.LoadColumnSettings(colsetting, (a) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(a, int.MinValue),
                            (b) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(b, double.MinValue));

            labelEngineerName.Text = name;
            labelEngineerStatus.Text = "";
            engineerImage.Image = BaseUtils.Icons.IconSet.GetIcon("Engineers." + name);
            labelEngineerStarSystem.Text = ei?.StarSystem ?? "";
            labelEngineerPlanet.Text = ei?.Planet ?? "";
            labelEngineerBaseName.Text = ei?.BaseName ?? "";
            labelDiscovery.Text = ei?.DiscoveryRequirements ?? "";
            labelMeeting.Text = ei?.MeetingRequirements ?? "";
            labelUnlock.Text = ei?.UnlockRequirements ?? "";
            labelEngineerDistance.Text = "";
            labelCrafts.Text = "";
            pictureBoxLockImage.Visible = ei != null && ei.PermitRequired;

            if (name == "Weapon" || name == "Suit")        // these are not currently recorded in EngineerList
                CraftedCol.HeaderText = "-";

            dataGridViewEngineering.MakeDoubleBuffered();

            dataViewScrollerPanel.Suspend();

            for (int i = 0; i < Recipes.EngineeringRecipes.Count; i++)
            {
                if (Recipes.EngineeringRecipes[i].engineers.Contains(name))
                {
                    Recipes.EngineeringRecipe r = Recipes.EngineeringRecipes[i];

                    var row = dataGridViewEngineering.Rows[dataGridViewEngineering.Rows.Add()];
                    row.Tag = r;
                    row.Cells[UpgradeCol.Index].Value = r.Name; 
                    row.Cells[ModuleCol.Index].Value = r.modulesstring;
                    row.Cells[LevelCol.Index].Value = r.level;
                    row.Cells[EngineersCol.Index].Tag = r.engineers;        // keep list in tag
                    row.Cells[EngineersCol.Index].Value = string.Join(Environment.NewLine, r.engineers);
                    row.Cells[RecipeCol.Index].ToolTipText = r.IngredientsStringLong;
                }
            }

            WantedPerRecipe = wantedsettings.RestoreArrayFromString(0, RecipesCount);

            for(int i = 0; i < RecipesCount; i++)
            {
                dataGridViewEngineering.Rows[i].Cells[WantedCol.Index].Value = WantedPerRecipe[i].ToString();
            }

            dataViewScrollerPanel.Resume();

            //System.Diagnostics.Debug.WriteLine($"Engineer {name} Recipes {dataGridViewEngineering.RowCount}");

            delaytime.Tick += Delaytime_Tick;
        }

        private void Delaytime_Tick(object sender, EventArgs e)
        {
            delaytime.Stop();
            ColumnSetupChanged?.Invoke(this);
        }

        public void InstallColumnEvents()
        {
            dataGridViewEngineering.ColumnStateChanged += DataGridViewEngineering_ColumnStateChanged;
            dataGridViewEngineering.ColumnFillWeightChanged += DataGridViewEngineering_ColumnFillWeightChanged;
            dataGridViewEngineering.ColumnDisplayIndexChanged += DataGridViewEngineering_ColumnDisplayIndexChanged;
            inchange = false;
        }

        private void DataGridViewEngineering_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (delaytime.Enabled == false && !inchange)
                delaytime.Start();
        }

        private void DataGridViewEngineering_ColumnFillWeightChanged(object sender, DataGridViewColumnEventArgs e, bool firsttime)
        {
            if (delaytime.Enabled == false && !inchange && !firsttime)
                delaytime.Start();
        }

        private void DataGridViewEngineering_ColumnStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.Visible && delaytime.Enabled == false && !inchange)
                delaytime.Start();
        }

        public void UnInstallEvents()
        {
            dataGridViewEngineering.ColumnStateChanged -= DataGridViewEngineering_ColumnStateChanged;
            dataGridViewEngineering.ColumnFillWeightChanged -= DataGridViewEngineering_ColumnFillWeightChanged;
            dataGridViewEngineering.ColumnDisplayIndexChanged -= DataGridViewEngineering_ColumnDisplayIndexChanged;
        }

        // mcllist may be null, cursystem may be null as may crafts

        public void UpdateStatus(string status, ISystem cursystem, List<MaterialCommodityMicroResource> mcllist, List<HistoryEntry> crafts)
        {
            labelEngineerStatus.Text = status;
            string dist = "";
            if (cursystem != null && EngineerInfo != null)
            {
                var d = cursystem.Distance(EngineerInfo.X, EngineerInfo.Y, EngineerInfo.Z);
                dist = d.ToString("0.#") + " ly";
            }

            labelEngineerDistance.Text = dist;

            if (mcllist != null )        // may be null
            {
                var totals = MaterialCommoditiesRecipe.TotalList(mcllist);                  // start with totals present

                dataViewScrollerPanel.Suspend();
                dataGridViewEngineering.SuspendLayout();

                var wwmode = dataGridViewEngineering.DefaultCellStyle.WrapMode;
                dataGridViewEngineering.DefaultCellStyle.WrapMode = DataGridViewTriState.False;     // seems to make it a tad faster

                foreach (DataGridViewRow row in dataGridViewEngineering.Rows)
                {
                    row.Visible = true;     // if we hide stuff, we just make it invisible, we do not remove it

                    Recipes.EngineeringRecipe r = row.Tag as Recipes.EngineeringRecipe;

                    string tooltip = "";
                    int craftcount = 0;

                    foreach (var c in crafts.EmptyIfNull())     // all crafts
                    {
                        var cb = c.journalEntry as EliteDangerousCore.JournalEvents.JournalEngineerCraftBase;
                        if (cb != null)     // if an craft base type, check name and level
                        {
                            if (cb.Engineering.BlueprintName.Equals(r.fdname, StringComparison.InvariantCultureIgnoreCase) && cb.Engineering.Level == r.LevelInt)
                            {
                                tooltip = tooltip.AppendPrePad(c.EventTimeUTC.ToString() + " " + (c.ShipInformation?.Name ?? "?") + " " + (cb.Engineering.ExperimentalEffect_Localised ?? ""), Environment.NewLine);
                                craftcount++;
                            }
                        }
                        else if (c.EntryType == JournalTypeEnum.TechnologyBroker)       // if tech broker, check name
                        {
                            var tb = c.journalEntry as EliteDangerousCore.JournalEvents.JournalTechnologyBroker;
                            //string unl = string.Join(",", tb.ItemsUnlocked.Select(x=>x.Name));  System.Diagnostics.Debug.WriteLine($"{unl} {r.fdname}");
                            if (tb.ItemsUnlocked != null && Array.FindIndex(tb.ItemsUnlocked, x => x.Name.Equals(r.fdname, StringComparison.InvariantCultureIgnoreCase)) >= 0)
                            {
                                tooltip = tooltip.AppendPrePad(c.EventTimeUTC.ToString() + " " + (tb.BrokerType ?? "") + " " + string.Join(",", tb.ItemsUnlocked.Select(x => x.Name_Localised)));
                                craftcount++;
                            }
                        }
                    }

                    row.Cells[CraftedCol.Index].ToolTipText = tooltip;

                    var res = MaterialCommoditiesRecipe.HowManyLeft(mcllist, totals, r, WantedPerRecipe[row.Index], reducetotals: false);    // recipes not chained not in order

                    row.Cells[MaxCol.Index].Value = res.Item1.ToString();
                    row.Cells[CraftedCol.Index].Value = CraftedCol.HeaderText == "-" ? "" : craftcount.ToString();
                    row.Cells[PercentageCol.Index].Value = res.Item5.ToString("N0");
                    row.Cells[NotesCol.Index].Value = res.Item3;
                    row.Cells[NotesCol.Index].ToolTipText = res.Item4;
                    row.Cells[RecipeCol.Index].Value = r.IngredientsStringvsCurrent(mcllist);
                    row.DefaultCellStyle.BackColor = (res.Item5 >= 100.0) ? ExtendedControls.Theme.Current.GridHighlightBack : ExtendedControls.Theme.Current.GridCellBack;
                }

                dataGridViewEngineering.DefaultCellStyle.WrapMode = wwmode;
                dataGridViewEngineering.ResumeLayout();
                dataViewScrollerPanel.Resume();
            }

            labelCrafts.Text = crafts != null ? (crafts.Count + " crafts") : "";
        }


        public void UpdateWordWrap(bool ww)
        {
            dataGridViewEngineering.SetWordWrap(ww);
            dataViewScrollerPanel.UpdateScroll();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            pictureBoxLockImage.Location = new Point(labelEngineerStarSystem.Right + 4, (labelEngineerStarSystem.Top+labelEngineerStarSystem.Bottom)/2 - pictureBoxLockImage.Height/2);
            int maxlright = new int[] {
                labelEngineerName.Right, pictureBoxLockImage.Right,labelEngineerDistance.Right, labelEngineerStatus.Right, labelEngineerPlanet.Right,
                labelEngineerBaseName.Right, labelEngineerStatus.Right, labelCrafts.Right}.Max();
            int normalproportion = Width * 6 / 16;
            dataViewScrollerPanel.Size = new Size(ClientRectangle.Width - Math.Max(maxlright,normalproportion) - 8, Height);
        }

        private void dataGridViewEngineering_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == WantedCol.Index)
            {
                string v = (string)dataGridViewEngineering[WantedCol.Index, e.RowIndex].Value;

                if (v.InvariantParse(out int iv))
                {
                    //System.Diagnostics.Debug.WriteLine("Set wanted {0} to {1}", rno, iv);
                    WantedPerRecipe[e.RowIndex] = iv;
                    Redisplay?.Invoke();
                }
                else
                    dataGridViewEngineering[WantedCol.Index, e.RowIndex].Value = WantedPerRecipe[e.RowIndex].ToString();
            }
        }

        private void dataGridViewEngineering_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if ( e.Column.Index >= 2 && e.Column.Index <= 6)
                e.SortDataGridViewColumnNumeric();
        }

        private void dataGridViewEngineering_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                DataGridViewRow row = dataGridViewEngineering.Rows[e.RowIndex];
                Recipes.EngineeringRecipe r = (Recipes.EngineeringRecipe)row.Tag;

                if (e.ColumnIndex == RecipeCol.Index)
                {
                    dataGridViewEngineering.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = r.IngredientsStringLong;
                }
            }

        }
    }
}


