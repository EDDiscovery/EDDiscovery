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
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class EngineerStatusPanel : UserControl
    {
        public ItemData.EngineeringInfo EngineerInfo { get; private set; }
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

        private bool inchange = true;

        private Timer delaytime = new Timer() { Interval = 500 };       // we get a storm of column widths changing, so using a timer we reduce them to one

        public EngineerStatusPanel()
        {
            InitializeComponent();
        }

        public void Init(string name, string starsystem, string planet, string basename, ItemData.EngineeringInfo ei, string wantedsettings, string colsetting)
        {
            EngineerInfo = ei;

            ExtendedControls.Theme.Current?.ApplyStd(this);
            var enumlist = new Enum[] { EDTx.UserControlEngineering_UpgradeCol, EDTx.UserControlEngineering_ModuleCol, EDTx.UserControlEngineering_LevelCol,
                            EDTx.UserControlEngineering_MaxCol, EDTx.UserControlEngineering_WantedCol, 
                            EDTx.UserControlEngineering_NotesCol, EDTx.UserControlEngineering_RecipeCol, EDTx.UserControlEngineering_EngineersCol,
                        };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, null, "UserControlEngineering");    // share IDs with Engineering panel./

            dataGridViewEngineering.LoadColumnSettings(colsetting, (a) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(a, int.MinValue),
                            (b) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(b, double.MinValue));

            labelEngineerName.Text = name;
            labelEngineerStatus.Text = "";
            engineerImage.Image = BaseUtils.Icons.IconSet.GetIcon("Engineers." + name);
            labelEngineerStarSystem.Text = starsystem;
            labelPlanet.Text = planet;
            labelBaseName.Text = basename;
            labelEngineerDistance.Text = "";

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
            dataGridViewEngineering.ColumnWidthChanged += DataGridViewEngineering_ColumnWidthChanged;
            dataGridViewEngineering.ColumnDisplayIndexChanged += DataGridViewEngineering_ColumnDisplayIndexChanged;
            inchange = false;
        }

        private void DataGridViewEngineering_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
             if (delaytime.Enabled == false && !inchange)
                delaytime.Start();
        }

        private void DataGridViewEngineering_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (delaytime.Enabled == false && !inchange)
                delaytime.Start();
         }

        private void DataGridViewEngineering_ColumnStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.Visible && delaytime.Enabled == false && !inchange) 
            {
                delaytime.Start();
            }
        }

        public void UnInstallEvents()
        {
            dataGridViewEngineering.ColumnStateChanged -= DataGridViewEngineering_ColumnStateChanged;
            dataGridViewEngineering.ColumnWidthChanged -= DataGridViewEngineering_ColumnWidthChanged;
            dataGridViewEngineering.ColumnDisplayIndexChanged -= DataGridViewEngineering_ColumnDisplayIndexChanged;
        }

        public void UpdateStatus(string status, ISystem cursystem, List<MaterialCommodityMicroResource> mcllist)
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

                    var res = MaterialCommoditiesRecipe.HowManyLeft(mcllist, totals, r, WantedPerRecipe[row.Index], reducetotals: false);    // recipes not chained not in order

                    row.Cells[MaxCol.Index].Value = res.Item1.ToString();
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
        }

        public void UpdateWordWrap(bool ww)
        {
            dataGridViewEngineering.DefaultCellStyle.WrapMode = ww ? DataGridViewTriState.True : DataGridViewTriState.False;
            dataGridViewEngineering.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataViewScrollerPanel.UpdateScroll();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            dataViewScrollerPanel.Width = ClientRectangle.Width - labelEngineerStarSystem.Left - 200;
//            System.Diagnostics.Debug.WriteLine($"width of {Name} {ClientRectangle} {dataViewScrollerPanel.Width}");
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
    }
}


