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

        private string DbColumnSave { get { return DBName((materials) ? "MaterialsGrid" : "CommoditiesGrid" ,  "DGVCol"); } }

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

            if (materials)
            {
                dataGridViewMC.Columns.Remove(dataGridViewMC.Columns[5]);       // to give name,shortname abv,category,type,number
                labelItems1.Text = "Data".Tx(this);
                labelItems2.Text = "Mats".Tx(this);
            }
            else
            {
                dataGridViewMC.Columns.Remove(dataGridViewMC.Columns[1]);       //shortname
                dataGridViewMC.Columns.Remove(dataGridViewMC.Columns[1]);       //then category to give name,type,number, avg price
                labelItems1.Text = "Total".Tx(this);
                textBoxItems2.Visible = labelItems2.Visible = false;
            }

            SetCheckBoxes();
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
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        private void Display(HistoryEntry he, HistoryList hl)
        {
            //if ( he != null ) System.Diagnostics.Debug.WriteLine("Hash displayed" + he.MaterialCommodity.DataHash());

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

            if (mc.Count > 0)
            {
                labelNoItems.Visible = false;

                foreach (MaterialCommodities m in mc)
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

                    int idx = dataGridViewMC.Rows.Add(rowobj);
                    //dataGridViewMC.Rows[idx].Tag = m;
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
