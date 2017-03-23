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
using EDDiscovery2.DB;
using EDDiscovery.EliteDangerous;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMaterialCommodities : UserControlCommonBase
    {
        private TravelHistoryControl travelhistorycontrol;
        private EDDiscoveryForm discoveryform;

        public bool materials = false;
        private int displaynumber = 0;
        List<MaterialCommodities> last_mc = null;

        private string DbColumnSave { get { return ((materials) ? "MaterialsGrid" : "CommoditiesGrid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }

        #region Init

        public UserControlMaterialCommodities()
        {
            InitializeComponent();
        }

        public override void Init( EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            travelhistorycontrol = ed.TravelControl;
            displaynumber = vn;

            dataGridViewMC.MakeDoubleBuffered();
            dataGridViewMC.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewMC.RowTemplate.Height = 26;

            if (materials)
                dataGridViewMC.Columns.Remove(dataGridViewMC.Columns[5]);       // to give name,shortname abv,category,type,number
            else
            {
                dataGridViewMC.Columns.Remove(dataGridViewMC.Columns[1]);       //shortname
                dataGridViewMC.Columns.Remove(dataGridViewMC.Columns[1]);       //then category to give name,type,number, avg price
            }

            travelhistorycontrol.OnTravelSelectionChanged += Display;

            SetCheckBoxes();
        }

        void SetCheckBoxes()
        {
            checkBoxClear.Enabled = false;
            checkBoxClear.Checked = (materials) ? EDDConfig.Instance.ClearMaterials : EDDConfig.Instance.ClearCommodities;
            checkBoxClear.Enabled = true;
        }

        #endregion

        #region Display

        public override void Display(HistoryEntry he, HistoryList hl)
        {
            Display(he?.MaterialCommodity.Sort(!materials));
        }

        public void Display(List<MaterialCommodities> mc)
        {
            if (mc == null)
            {
                dataGridViewMC.Rows.Clear();
                return;
            }

            last_mc = mc;

            dataGridViewMC.Rows.Clear();

            if (mc.Count > 0)
            {
                labelNoItems.Visible = false;

                foreach (MaterialCommodities m in mc)
                {
                    object[] rowobj;

                    if (materials)
                    {
                        rowobj = new[] { m.name, m.shortname, m.category, m.type, m.count.ToString() };
                    }
                    else
                    {
                        rowobj = new[] { m.name, m.type, m.count.ToString(), m.price.ToString("0.#") };
                    }

                    int idx = dataGridViewMC.Rows.Add(rowobj);
                    //dataGridViewMC.Rows[idx].Tag = m;
                }

                if (dataGridViewMC.SortedColumn != null && dataGridViewMC.SortOrder != SortOrder.None)
                {
                    dataGridViewMC.Sort(dataGridViewMC.SortedColumn, dataGridViewMC.SortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending);
                }
            }
            else
            {
                labelNoItems.Visible = true;
            }
        }

        #endregion

        #region Layout

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewMC, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewMC, DbColumnSave);

            travelhistorycontrol.OnTravelSelectionChanged -= Display;
        }

        #endregion

        private void checkBoxClear_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBoxClear.Enabled)
            {
                if (materials)
                    EDDConfig.Instance.ClearMaterials = checkBoxClear.Checked;
                else
                    EDDConfig.Instance.ClearCommodities = checkBoxClear.Checked;

                discoveryform.RecalculateHistoryDBs();
            }
        }

        private void dataGridViewMC_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if ((materials && e.Column.Index == 4) || (!materials && (e.Column.Index == 2 || e.Column.Index == 3)))
            {
                double v1;
                double v2;
                bool v1hasval = Double.TryParse(e.CellValue1?.ToString(), out v1);
                bool v2hasval = Double.TryParse(e.CellValue2?.ToString(), out v2);

                if (v1hasval || v2hasval)
                {
                    if (!v1hasval)
                    {
                        e.SortResult = 1;
                    }
                    else if (!v2hasval)
                    {
                        e.SortResult = -1;
                    }
                    else
                    {
                        e.SortResult = v1.CompareTo(v2);
                    }

                    e.Handled = true;
                }
            }
        }
    }

    public class UserControlMaterials : UserControlMaterialCommodities
    {
        public UserControlMaterials()
        {
            materials = true;
            Name = "Materials";
        }
    }

    public class UserControlCommodities : UserControlMaterialCommodities
    {
        public UserControlCommodities()
        {
            materials = false;
            Name = "Commodities";
        }
    }
}
