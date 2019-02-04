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
        private string DbFilterSave { get { return DBName((materials) ? "MaterialsGrid" : "CommoditiesGrid", "Filter"); } }

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
            string[] types;

            if (materials)
            {
                dataGridViewMC.Columns.Remove(dataGridViewMC.Columns[5]);       // to give name,shortname abv,category,type,number
                labelItems1.Text = "Data".Tx(this);
                labelItems2.Text = "Mats".Tx(this);

                items = MaterialCommodityData.GetMaterials(true);
                types = MaterialCommodityData.GetTypes((x) => !x.IsCommodity, true);

                MaterialCommodityData[] raw = items.Where(x => x.IsRaw).ToArray();
                cfs.AddGroupOption("Raw", String.Join(";", raw.Select(x => x.Name).ToArray()) + ";");
                MaterialCommodityData[] enc = items.Where(x => x.IsEncoded).ToArray();
                cfs.AddGroupOption("Encoded", String.Join(";", enc.Select(x => x.Name).ToArray()) + ";");
                MaterialCommodityData[] manu = items.Where(x => x.IsManufactured).ToArray();
                cfs.AddGroupOption("Manufactured", String.Join(";", manu.Select(x => x.Name).ToArray()) + ";");
            }
            else
            {
                items = MaterialCommodityData.GetCommodities(true);
                types = MaterialCommodityData.GetTypes((x) => x.IsCommodity, true);

                MaterialCommodityData[] rare = items.Where(x => x.IsRareCommodity).ToArray();
                cfs.AddGroupOption("Rare", String.Join(";", rare.Select(x => x.Name).ToArray()) + ";");

                dataGridViewMC.Columns.Remove(dataGridViewMC.Columns[1]);       //shortname
                dataGridViewMC.Columns.Remove(dataGridViewMC.Columns[1]);       //then category to give name,type,number, avg price
                labelItems1.Text = "Total".Tx(this);
                textBoxItems2.Visible = labelItems2.Visible = false;
            }

            foreach (string t in types)
            {
                string[] members = MaterialCommodityData.GetMembersOfType(t, true);
                cfs.AddGroupOption(t, String.Join(";", members) + ";");
            }

            foreach (var x in items)
                cfs.AddStandardOption(x.Name);

            SetCheckBoxes();

            cfs.Changed += FilterChanged;
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

        void SetCheckBoxes()
        {
            checkBoxClear.Enabled = false;
            checkBoxClear.Checked = (materials) ? EDDiscoveryForm.EDDConfig.ClearMaterials : EDDiscoveryForm.EDDConfig.ClearCommodities;
            checkBoxClear.Enabled = true;
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

            System.Diagnostics.Trace.WriteLine(BaseUtils.AppTicks.TickCountLap(this,true) + " MC " + displaynumber + " Begin Display");

            List<MaterialCommodities> mc = mcl.Sort(!materials);

            string filter = EliteDangerousCore.DB.SQLiteDBClass.GetSettingString(DbFilterSave, "All");

            if (mc.Count > 0)
            {
                labelNoItems.Visible = false;

                foreach (MaterialCommodities m in mc)
                {
                    if (filter == "All" || filter.Contains(m.Details.Name + ";"))
                    {
                        object[] rowobj;

                        if (materials)
                        {
                            rowobj = new[] { m.Details.Name, m.Details.Shortname, m.Details.TranslatedCategory, m.Details.TranslatedType + " (" + (m.Details.MaterialLimit()??0).ToString() + ")" ,
                                m.Count.ToString() };
                        }
                        else
                        {
                            rowobj = new[] { m.Details.Name, m.Details.TranslatedType, m.Count.ToString(), m.Price.ToString("0.#") };
                        }

                        dataGridViewMC.Rows.Add(rowobj);
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
                    textBoxItems1.Text = mcl.CargoCount.ToStringInvariant();
            }
            else
            {
                labelNoItems.Visible = true;
            }

            System.Diagnostics.Trace.WriteLine(BaseUtils.AppTicks.TickCountLap(this) + " MC " + displaynumber + " Load Finished");
        }

        #endregion

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            cfs.Filter(b, this.FindForm());
        }

        private void FilterChanged(object sender, Object e)
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history, true);
        }

        private void checkBoxClear_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBoxClear.Enabled)
            {
                if (materials)
                    EDDiscoveryForm.EDDConfig.ClearMaterials = checkBoxClear.Checked;
                else
                    EDDiscoveryForm.EDDConfig.ClearCommodities = checkBoxClear.Checked;

                discoveryform.RecalculateHistoryDBs();
            }
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
