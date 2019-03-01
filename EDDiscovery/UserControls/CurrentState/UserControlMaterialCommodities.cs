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

        #region Init

        public UserControlMaterialCommodities()
        {
            InitializeComponent();
            var corner = dataGridViewMC.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewMC.MakeDoubleBuffered();
            dataGridViewMC.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewMC.RowTemplate.Height = 26;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            cfs = new FilterSelector(DbFilterSave);

            MaterialCommodityData[] items;
            Tuple<string, string>[] types;

            cfs.AddAllNone();

            if (materials)
            {
                dataGridViewMC.Columns.Remove(dataGridViewMC.Columns[5]);       // to give name,shortname abv,category,type,number
                labelItems1.Text = "Data".Tx(this);
                labelItems2.Text = "Mats".Tx(this);

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
                dataGridViewMC.Columns.Remove(dataGridViewMC.Columns[1]);       //shortname
                dataGridViewMC.Columns.Remove(dataGridViewMC.Columns[1]);       //then category to give name,type,number, avg price
                labelItems1.Text = "Total".Tx(this);
                textBoxItems2.Visible = labelItems2.Visible = false;
                checkBoxClear.Location = new Point(textBoxItems1.Right + 8, checkBoxClear.Top);

                items = MaterialCommodityData.GetCommodities(true);
                types = MaterialCommodityData.GetTypes((x) => x.IsCommodity, true);

                MaterialCommodityData[] rare = items.Where(x => x.IsRareCommodity).ToArray();
                cfs.AddGroupOption(String.Join(";", rare.Select(x => x.FDName).ToArray()) + ";", "Rare".Tx(this));
            }

            foreach (var t in types)
            {
                string[] members = MaterialCommodityData.GetFDNameMembersOfType(t.Item1, true);
                cfs.AddGroupOption(String.Join(";", members) + ";", t.Item2);
            }

            foreach (var x in items)
                cfs.AddStandardOption(x.FDName,x.Name);

            checkBoxClear.Checked = EliteDangerousCore.DB.SQLiteDBClass.GetSettingBool(DbClearZeroSave, true);
            checkBoxClear.CheckedChanged += CheckBoxClear_CheckedChanged;

            cfs.Closing += FilterChanged;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
            DGVLoadColumnLayout(dataGridViewMC, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewMC, DbColumnSave);

            uctg.OnTravelSelectionChanged -= Display;
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history, true);
        }

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            Display(he?.MaterialCommodity);
        }

        private void Display(MaterialCommoditiesList mcl)
        {
            dataGridViewMC.Rows.Clear();
            textBoxItems1.Text = textBoxItems2.Text = "";

            if (mcl == null)
                return;

            string[] filter = EliteDangerousCore.DB.SQLiteDBClass.GetSettingString(DbFilterSave, "All").SplitNoEmptyStartFinish(';');
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
                        if (materials)
                        {
                            int limit = mcd.MaterialLimit() ?? 0;

                            rowobj = new[] { mcd.Name, mcd.Shortname, mcd.TranslatedCategory,
                                                mcd.TranslatedType + ( limit>0 ? " (" + limit.ToString() + ")" : "") ,
                                                m != null ? m.Count.ToString() : "0"
                            };
                        }
                        else
                        {
                            rowobj = new[] { mcd.Name, mcd.TranslatedType,
                                                m != null ? m.Count.ToString() : "0",
                                                m != null ? m.Price.ToString("0.#") : "-" };
                        }

                        dataGridViewMC.Rows.Add(rowobj);
                    }
                }
            }

            if (dataGridViewMC.SortedColumn != null && dataGridViewMC.SortOrder != SortOrder.None)
            {
                dataGridViewMC.Sort(dataGridViewMC.SortedColumn, dataGridViewMC.SortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending);
            }

            if (materials)
            {
                textBoxItems1.Text = mcl.DataCount.ToStringInvariant();
                textBoxItems2.Text = mcl.MaterialsCount.ToStringInvariant();
            }
            else
            {
                textBoxItems1.Text = mcl.CargoCount.ToStringInvariant();
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
                Display(uctg.GetCurrentHistoryEntry, discoveryform.history, true);
        }

        private void CheckBoxClear_CheckedChanged(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.SQLiteDBClass.PutSettingBool(DbClearZeroSave, checkBoxClear.Checked);
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history, true);
        }

        private void dataGridViewMC_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if ((materials && e.Column.Index == 4) || (!materials && e.Column.Index == 3))
            {
                e.SortDataGridViewColumnNumeric();
            }
        }

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
