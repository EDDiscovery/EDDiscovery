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
        public bool materials = false;

        FilterSelector cfs;
        private string DbColumnSave { get { return DBName((materials) ? "MaterialsGrid" : "CommoditiesGrid",  "DGVCol"); } }
        private string DbFilterSave { get { return DBName((materials) ? "MaterialsGrid" : "CommoditiesGrid", "Filter2"); } }
        private string DbClearZeroSave { get { return DBName((materials) ? "MaterialsGrid" : "CommoditiesGrid", "ClearZero"); } }
        private string DbTruncateText { get { return DBName((materials) ? "MaterialsGrid" : "CommoditiesGrid", "WrapText"); } }

        MaterialCommoditiesList last_mcl;

        #region Init

        public UserControlMaterialCommodities()
        {
            InitializeComponent();
            var corner = dataGridViewMC.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewMC.MakeDoubleBuffered();

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            cfs = new FilterSelector(DbFilterSave);

            MaterialCommodityData[] items;
            Tuple<string, string>[] types;

            cfs.AddAllNone();

            if (materials)
            {
                dataGridViewMC.Columns[5].HeaderText = "Recipes".T(EDTx.UserControlMaterialCommodities_Recipes);
                labelItems1.Text = "Data".T(EDTx.UserControlMaterialCommodities_Data);
                labelItems2.Text = "Mats".T(EDTx.UserControlMaterialCommodities_Mats);

                items = MaterialCommodityData.GetMaterials(true);
                types = MaterialCommodityData.GetTypes((x) => !x.IsCommodity, true);

                Tuple<string, string>[] cats = MaterialCommodityData.GetCategories((x) => !x.IsCommodity, true);

                foreach (var t in cats)
                {
                    string[] members = MaterialCommodityData.GetFDNameMembersOfCategory(t.Item1, true);
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
                checkBoxClear.Location = new Point(textBoxItems1.Right + 8, checkBoxClear.Top);

                items = MaterialCommodityData.GetCommodities(true);
                types = MaterialCommodityData.GetTypes((x) => x.IsCommodity, true);

                MaterialCommodityData[] rare = items.Where(x => x.IsRareCommodity).ToArray();
                cfs.AddGroupOption(String.Join(";", rare.Select(x => x.FDName).ToArray()) + ";", "Rare".T(EDTx.UserControlMaterialCommodities_Rare));
            }

            foreach (var t in types)
            {
                string[] members = MaterialCommodityData.GetFDNameMembersOfType(t.Item1, true);
                cfs.AddGroupOption(String.Join(";", members) + ";", t.Item2);
            }

            foreach (var x in items)
                cfs.AddStandardOption(x.FDName,x.Name);

            checkBoxClear.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbClearZeroSave, true);
            checkBoxClear.CheckedChanged += CheckBoxClear_CheckedChanged;

            extCheckBoxTruncateText.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbTruncateText, true);
            extCheckBoxTruncateText.CheckedChanged += TruncateText_CheckedChanged;

            dataGridViewMC.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewMC.AutoSizeRowsMode = extCheckBoxTruncateText.Checked ? DataGridViewAutoSizeRowsMode.None : DataGridViewAutoSizeRowsMode.AllCells;

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
            DGVLoadColumnLayout(dataGridViewMC, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewMC, DbColumnSave);

            uctg.OnTravelSelectionChanged -= CallBackDisplayWithCheck;
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            MaterialCommoditiesList mcl = uctg?.GetCurrentHistoryEntry?.MaterialCommodity;
            Display(mcl);
        }

        private void CallBackDisplayWithCheck(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            MaterialCommoditiesList mcl = he?.MaterialCommodity;
            if ( mcl != last_mcl )
                Display(mcl);
        }

        private void Display(MaterialCommoditiesList mcl)       // update display. mcl can be null
        {
            last_mcl = mcl;

            DataGridViewColumn sortcolprev = dataGridViewMC.SortedColumn != null ? dataGridViewMC.SortedColumn : dataGridViewMC.Columns[0];
            SortOrder sortorderprev = dataGridViewMC.SortedColumn != null ? dataGridViewMC.SortOrder : SortOrder.Ascending;
            int firstline = dataGridViewMC.FirstDisplayedScrollingRowIndex;

            dataGridViewMC.Rows.Clear();

            textBoxItems1.Text = textBoxItems2.Text = "";

            if (mcl == null)
                return;
            
            //System.Diagnostics.Debug.WriteLine("Display mcl " + mcl.GetHashCode());

            string filters = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbFilterSave, "All");
            //System.Diagnostics.Debug.WriteLine("Filter is " + filters);
            string[] filter = filters.SplitNoEmptyStartFinish(';');
            bool all = filter.Length > 0 && filter[0] == "All";
            bool clearzero = checkBoxClear.Checked;

            MaterialCommodityData[] allitems = materials ? MaterialCommodityData.GetMaterials(true) : MaterialCommodityData.GetCommodities(true);

            foreach ( MaterialCommodityData mcd in allitems)        // we go thru all items..
            {
                if (all || filter.Contains(mcd.FDName) )      // and see if they are in the filter
                {
                    object[] rowobj;

                    MaterialCommodities m = mcl.List.Find(x => x.Details.Name == mcd.Name);     // and we see if we actually have some at this time

                    if (!clearzero || (m != null && m.Count > 0))       // if display zero, or we have some..
                    {

                        string s = Recipes.UsedInSythesisByFDName(mcd.FDName, Environment.NewLine);
                        string e = Recipes.UsedInEngineeringByFDName(mcd.FDName, Environment.NewLine);
                        s = s.AppendPrePad(e, Environment.NewLine);
                        string b = Recipes.UsedInTechBrokerUnlocksByFDName(mcd.FDName, Environment.NewLine);
                        s = s.AppendPrePad(b, Environment.NewLine);
                        string se = Recipes.UsedInSpecialEffectsyFDName(mcd.FDName, Environment.NewLine);
                        s = s.AppendPrePad(se, Environment.NewLine);

                        if (materials)
                        {
                            int limit = mcd.MaterialLimit() ?? 0;

                            rowobj = new[] { mcd.Name, mcd.Shortname, mcd.TranslatedCategory,
                                                mcd.TranslatedType + ( limit>0 ? " (" + limit.ToString() + ")" : "") ,
                                                m != null ? m.Count.ToString() : "0",  s
                            };
                        }
                        else
                        {
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

            dataGridViewMC.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewMC.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;
            if (firstline >= 0 && firstline < dataGridViewMC.RowCount)
                dataGridViewMC.FirstDisplayedScrollingRowIndex = firstline;

            if (materials)
            {
                textBoxItems1.Text = mcl.DataCount.ToString();
                textBoxItems2.Text = mcl.MaterialsCount.ToString();
            }
            else
            {
                textBoxItems1.Text = mcl.CargoCount.ToString();
            }
        }

        #endregion

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            cfs.Filter(b, this.FindForm());
        }

        private void FilterChanged(object sender, bool same, Object e)
        {
            if (!same)
                Display(last_mcl);
        }

        private void CheckBoxClear_CheckedChanged(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbClearZeroSave, checkBoxClear.Checked);
            Display(last_mcl);
        }

        private void TruncateText_CheckedChanged(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbTruncateText, checkBoxClear.Checked);
            dataGridViewMC.AutoSizeRowsMode = extCheckBoxTruncateText.Checked ? DataGridViewAutoSizeRowsMode.None : DataGridViewAutoSizeRowsMode.AllCells;
            Display(last_mcl);
        }

        private void dataGridViewMC_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if ((materials && e.Column.Index == 4) || (!materials && (e.Column.Index == 3 || e.Column.Index ==2)))
            {
                e.SortDataGridViewColumnNumeric();
            }
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
            }

        }

        #region Right click

        int rightclickrow = -1;

        private void dataGridViewMC_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                rightclickrow = -1;
            }

            if (dataGridViewMC.SelectedCells.Count < 2 || dataGridViewMC.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
            {
                DataGridView.HitTestInfo hti = dataGridViewMC.HitTest(e.X, e.Y);
                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    dataGridViewMC.ClearSelection();                // select row under cursor.
                    dataGridViewMC.Rows[hti.RowIndex].Selected = true;

                    if (e.Button == MouseButtons.Right)         // right click on MC map, get in before the context menu
                    {
                        rightclickrow = hti.RowIndex;
                    }
                }
            }
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
        }

        private void openRecipeInWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickrow >= 0 && rightclickrow < dataGridViewMC.RowCount)
            {
                string mats = (string)dataGridViewMC.Rows[rightclickrow].Tag;
                if (mats != null)   // sheer paranoia.
                {
                    mats = mats.Replace(": ", Environment.NewLine + "      ");
                    ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                    info.Info(dataGridViewMC.Rows[rightclickrow].Cells[0].Value as string, FindForm().Icon, mats);
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
            materials = true;
        }
    }

    public class UserControlCommodities : UserControlMaterialCommodities
    {
        public UserControlCommodities()
        {
            materials = false;
        }
    }
}
