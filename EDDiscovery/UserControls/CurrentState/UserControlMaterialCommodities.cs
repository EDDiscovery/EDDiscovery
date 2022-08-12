/*
 * Copyright © 2016 - 2021 EDDiscovery development team
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using EDDiscovery.Controls;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMaterialCommodities : UserControlCommonBase
    {
        public string dbWrapText = "WrapText";
        public string dbFilter = "Filter2";
        public string dbClearZero = "ClearZero";

        public enum PanelType { Materials, Commodities, MicroResources};
        public PanelType PanelMode;

        JournalFilterSelector cfs;

        uint? last_mcl = null;

        #region Init

        public UserControlMaterialCommodities()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = PanelMode == PanelType.Materials ? "MaterialsGrid" : PanelMode == PanelType.Commodities ? "CommoditiesGrid" : "MicroResourcesGrid";

            dataGridViewMC.MakeDoubleBuffered();
            extCheckBoxWordWrap.Checked = GetSetting(dbWrapText, false);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            var enumlist = new Enum[] { EDTx.UserControlMaterialCommodities_NameCol, EDTx.UserControlMaterialCommodities_ShortName, EDTx.UserControlMaterialCommodities_Category, EDTx.UserControlMaterialCommodities_Type, EDTx.UserControlMaterialCommodities_Number, EDTx.UserControlMaterialCommodities_Price };
            var enumlisttt = new Enum[] { EDTx.UserControlMaterialCommodities_buttonFilter_ToolTip, EDTx.UserControlMaterialCommodities_textBoxItems1_ToolTip, EDTx.UserControlMaterialCommodities_textBoxItems2_ToolTip, EDTx.UserControlMaterialCommodities_checkBoxShowZeros_ToolTip, EDTx.UserControlMaterialCommodities_extCheckBoxWordWrap_ToolTip };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, null, "UserControlMaterialCommodities");
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this, "UserControlMaterialCommodities");

            cfs = new JournalFilterSelector();
            cfs.AddAllNone();

            MaterialCommodityMicroResourceType[] items;
            Tuple<MaterialCommodityMicroResourceType.ItemType, string>[] types;

            Price.Tag = Number.Tag = "Num";     // these tell the sorter to do numeric sorting

            if (PanelMode == PanelType.Materials)
            {
                dataGridViewMC.Columns[5].HeaderText = "Recipes".T(EDTx.UserControlMaterialCommodities_Recipes);
                labelItems1.Text = "Data".T(EDTx.UserControlMaterialCommodities_Data);
                labelItems2.Text = "Mats".T(EDTx.UserControlMaterialCommodities_Mats);

                items = MaterialCommodityMicroResourceType.GetMaterials(true);
                types = MaterialCommodityMicroResourceType.GetTypes((x) => x.IsMaterial, true);

                var cats = MaterialCommodityMicroResourceType.GetCategories((x) => x.IsMaterial, true);

                foreach (var t in cats)
                {
                    string[] members = MaterialCommodityMicroResourceType.GetFDNameMembersOfCategory(t.Item1, true);
                    cfs.AddGroupOption(String.Join(";", members) + ";", t.Item2);
                }
            }
            else if (PanelMode == PanelType.MicroResources)
            {
                dataGridViewMC.ContextMenuStrip = null;
                dataGridViewMC.Columns.Remove(Type);
                Number.HeaderText = "Ship Locker".T(EDTx.UserControlMaterialCommodities_ShipLocker);
                Price.HeaderText = "BackPack".T(EDTx.UserControlMaterialCommodities_BackPack);
                labelItems1.Text = "Total".T(EDTx.UserControlMaterialCommodities_Total);
                textBoxItems2.Visible = labelItems2.Visible = false;

                items = MaterialCommodityMicroResourceType.GetMicroResources(true);
                types = MaterialCommodityMicroResourceType.GetTypes((x) => x.IsMicroResources, true);

                var cats = MaterialCommodityMicroResourceType.GetCategories((x) => x.IsMicroResources, true);

                foreach (var t in cats)
                {
                    string[] members = MaterialCommodityMicroResourceType.GetFDNameMembersOfCategory(t.Item1, true);
                    cfs.AddGroupOption(String.Join(";", members) + ";", t.Item2);
                }
            }
            else
            {
                dataGridViewMC.Columns.Remove(dataGridViewMC.Columns[1]);       //remove cat

                DataGridViewColumn c = dataGridViewMC.Columns[1];       // reassign column 1 to end and call recipes
                c.HeaderText = "Recipes".T(EDTx.UserControlMaterialCommodities_Recipes);
                c.DisplayIndex = 4; // need to change its display pos
                dataGridViewMC.Columns.Remove(c);   // and to place it at the end otherwise it does not fill in the right order
                dataGridViewMC.Columns.Add(c);

                labelItems1.Text = "Total".T(EDTx.UserControlMaterialCommodities_Total);
                textBoxItems2.Visible = labelItems2.Visible = false;
                checkBoxShowZeros.Location = new Point(textBoxItems1.Right + 8, checkBoxShowZeros.Top);

                items = MaterialCommodityMicroResourceType.GetCommodities(true);
                types = MaterialCommodityMicroResourceType.GetTypes((x) => x.IsCommodity, true);

                MaterialCommodityMicroResourceType[] rare = items.Where(x => x.IsRareCommodity).ToArray();
                cfs.AddGroupOption(String.Join(";", rare.Select(x => x.FDName).ToArray()) + ";", "Rare".T(EDTx.UserControlMaterialCommodities_Rare));
            }

            foreach (var t in types)
            {
                string[] members = MaterialCommodityMicroResourceType.GetFDNameMembersOfType(t.Item1, true);
                cfs.AddGroupOption(String.Join(";", members) + ";", t.Item2);
            }

            foreach (var x in items)
                cfs.AddStandardOption(x.FDName,x.Name);

            checkBoxShowZeros.Checked = !GetSetting(dbClearZero, true); // used to be clear zeros, now its show zeros, invert
            checkBoxShowZeros.CheckedChanged += CheckBoxClear_CheckedChanged;

            cfs.SaveSettings += FilterChanged;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= CallBackDisplayWithCheck;
            uctg = thc;
            uctg.OnTravelSelectionChanged += CallBackDisplayWithCheck;
        }

        public override void LoadLayout()
        {
            dataGridViewMC.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            uctg.OnTravelSelectionChanged += CallBackDisplayWithCheck;
            DGVLoadColumnLayout(dataGridViewMC);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewMC);

            uctg.OnTravelSelectionChanged -= CallBackDisplayWithCheck;
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            Display(uctg?.GetCurrentHistoryEntry?.MaterialCommodity);
        }

        private void CallBackDisplayWithCheck(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            uint? mcl = he?.MaterialCommodity;
            if ( mcl != last_mcl )
                Display(mcl);
        }

        private void Display(uint? mcl)       // update display. mcl can be null
        {
            last_mcl = mcl;

            DataGridViewColumn sortcolprev = dataGridViewMC.SortedColumn != null ? dataGridViewMC.SortedColumn : dataGridViewMC.Columns[0];
            SortOrder sortorderprev = dataGridViewMC.SortedColumn != null ? dataGridViewMC.SortOrder : SortOrder.Ascending;
            int firstline = dataGridViewMC.SafeFirstDisplayedScrollingRowIndex();

            dataGridViewMC.Rows.Clear();

            textBoxItems1.Text = textBoxItems2.Text = "";

            if (mcl == null)
                return;

            //System.Diagnostics.Debug.WriteLine("Display mcl " + mcl.GetHashCode());

            string filters = GetSetting(dbFilter, "All");
            //System.Diagnostics.Debug.WriteLine("Filter is " + filters);
            string[] filter = filters.SplitNoEmptyStartFinish(';');
            bool all = filter.Length > 0 && filter[0] == "All";
            bool showzeros = checkBoxShowZeros.Checked;

            dataViewScrollerPanel.SuspendLayout();

            MaterialCommodityMicroResourceType[] allitems = PanelMode == PanelType.Materials ? MaterialCommodityMicroResourceType.GetMaterials(true) : PanelMode == PanelType.MicroResources ? MaterialCommodityMicroResourceType.GetMicroResources(true) : MaterialCommodityMicroResourceType.GetCommodities(true);

            foreach ( MaterialCommodityMicroResourceType mcd in allitems)        // we go thru all items..
            {
                if (all || filter.Contains(mcd.FDName) )      // and see if they are in the filter
                {
                    object[] rowobj;

                    MaterialCommodityMicroResource m = discoveryform.history.MaterialCommoditiesMicroResources.Get(mcl.Value, mcd.FDName);      // at generation mcl, find fdname.

                    if (showzeros || (m != null && m.NonZero))       // if display zero, or we have some..
                    {
                        string s = "";

                        if (PanelMode == PanelType.Materials)
                        {
                            s = Recipes.UsedInRecipesByFDName(mcd.FDName, Environment.NewLine);

                            int limit = mcd.MaterialLimit() ?? 0;

                            rowobj = new[] { mcd.Name, mcd.Shortname, mcd.TranslatedCategory,
                                                mcd.TranslatedType + ( limit>0 ? " (" + limit.ToString() + ")" : "") ,
                                                m != null ? m.Count.ToString() : "0",  s
                            };
                        }
                        else if (PanelMode == PanelType.MicroResources)
                        {
                            rowobj = new[] { mcd.Name, mcd.Shortname, mcd.TranslatedCategory,
                                                m != null ? m.Counts[0].ToString() : "0", m != null ? m.Counts[1].ToString() : "0"
                            };
                        }
                        else
                        {
                            s = Recipes.UsedInRecipesByFDName(mcd.FDName, Environment.NewLine);

                            rowobj = new[] { mcd.Name, mcd.TranslatedType,
                                                m != null ? m.Count.ToString() : "0",
                                                m != null ? m.Price.ToString("0.#") : "-", s
                            };
                        }

                        dataGridViewMC.Rows.Add(rowobj);
                        dataGridViewMC.Rows[dataGridViewMC.RowCount - 1].Cells[dataGridViewMC.ColumnCount-1].ToolTipText = s;
                        dataGridViewMC.Rows[dataGridViewMC.RowCount - 1].Tag = s;
                    }
                }
            }

            dataViewScrollerPanel.ResumeLayout();

            dataGridViewMC.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewMC.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;
            if (firstline >= 0 && firstline < dataGridViewMC.RowCount)
                dataGridViewMC.SafeFirstDisplayedScrollingRowIndex( firstline);

            var mcllist = discoveryform.history.MaterialCommoditiesMicroResources.Get(mcl.Value);
            var counts = MaterialCommoditiesMicroResourceList.Count(mcllist);

            if (PanelMode == PanelType.Materials)
            {
                textBoxItems1.Text = counts[(int)MaterialCommodityMicroResourceType.CatType.Encoded].ToString();
                textBoxItems2.Text = (counts[(int)MaterialCommodityMicroResourceType.CatType.Raw] + counts[(int)MaterialCommodityMicroResourceType.CatType.Manufactured]).ToString();
            }
            else if (PanelMode == PanelType.MicroResources)
            {
                textBoxItems1.Text = (counts[(int)MaterialCommodityMicroResourceType.CatType.Data] + counts[(int)MaterialCommodityMicroResourceType.CatType.Component] +
                                       counts[(int)MaterialCommodityMicroResourceType.CatType.Item] + counts[(int)MaterialCommodityMicroResourceType.CatType.Consumable]).ToString();
            }
            else
            {
                textBoxItems1.Text = counts[(int)MaterialCommodityMicroResourceType.CatType.Commodity].ToString();
            }
        }

        #endregion

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            cfs.Open(GetSetting(dbFilter,"All"), b, this.FindForm());
        }

        private void FilterChanged(string newset, Object e)
        {
            string filters = GetSetting(dbFilter, "All");
            if ( filters != newset )
            {
                PutSetting(dbFilter, newset);
                Display(last_mcl);
            }
        }

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            PutSetting(dbWrapText, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }

        private void UpdateWordWrap()
        {
            dataGridViewMC.SetWordWrap(extCheckBoxWordWrap.Checked);
            dataViewScrollerPanel.UpdateScroll();
        }

        private void CheckBoxClear_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting(dbClearZero, !checkBoxShowZeros.Checked);    // negative because we changed button sense
            Display(last_mcl);
        }

        private void dataGridViewMC_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            object tag = e.Column.Tag;
            if ( tag != null)
                e.SortDataGridViewColumnNumeric();
        }

        private void dataGridViewMC_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rcell = dataGridViewMC.ColumnCount - 1;
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewMC.Rows.Count && e.ColumnIndex == rcell)
            {
                DataGridViewRow row = dataGridViewMC.Rows[e.RowIndex];

                if (row.Height > dataGridViewMC.RowTemplate.Height)
                {
                    row.Height = dataGridViewMC.RowTemplate.Height;
                }
                else
                {

                    using (Graphics g = Parent.CreateGraphics())
                    {
                        using (StringFormat f = new StringFormat())
                        {
                            string ms = (string)row.Cells[rcell].Value + ".";
                            var sz = g.MeasureString(ms, dataGridViewMC.Font, new SizeF(dataGridViewMC.Columns[rcell].Width - 4, 1000), f);
                            sz.Height *= 63.0f / 56.0f; // it underestimates of course, scale it a bit
                            row.Height = (int)sz.Height;
                            //System.Diagnostics.Debug.WriteLine("Measured h" + sz);
                        }

                    }
                }

                dataViewScrollerPanel.UpdateScroll();
            }

        }

        #region Right click

        private void openRecipeInWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewMC.RightClickRowValid)
            {
                string mats = (string)dataGridViewMC.Rows[dataGridViewMC.RightClickRow].Tag;
                if (mats != null)   // sheer paranoia.
                {
                    mats = mats.Replace(": ", Environment.NewLine + "      ");
                    ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                    info.Info(dataGridViewMC.Rows[dataGridViewMC.RightClickRow].Cells[0].Value as string, FindForm().Icon, mats);
                    info.Size = new Size(800, 600);
                    info.StartPosition = FormStartPosition.CenterParent;
                    info.ShowDialog(FindForm());
                }
            }
        }

        #endregion

    }

    public class UserControlMaterials : UserControlMaterialCommodities
    {
        public UserControlMaterials()
        {
            PanelMode = UserControlMaterialCommodities.PanelType.Materials;
        }
    }

    public class UserControlCommodities : UserControlMaterialCommodities
    {
        public UserControlCommodities()
        {
            PanelMode = UserControlMaterialCommodities.PanelType.Commodities;
        }
    }

    public class UserControlMicroResources : UserControlMaterialCommodities
    {
        public UserControlMicroResources()
        {
            PanelMode = UserControlMaterialCommodities.PanelType.MicroResources;
        }
    }
}
