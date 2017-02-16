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
 * EDDiscovery is not affiliated with Fronter Developments plc.
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

namespace EDDiscovery.UserControls
{
    public partial class UserControlMaterialCommodities : UserControlCommonBase
    {
        private TravelHistoryControl travelhistorycontrol;
        private EDDiscoveryForm discoveryform;

        public bool materials = false;
        private int displaynumber = 0;
        private int namecol, abvcol, catcol, typecol, numcol, pricecol;
        List<MaterialCommodities> last_mc = null;
        bool editing = false;

        public delegate void ChangedCount(List<MaterialCommodities> ls);
        public event ChangedCount OnChangedCount;

        public delegate void RequestRefresh();
        public event RequestRefresh OnRequestRefresh;

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

            namecol = 0;
            abvcol = (materials) ? 1 : -1;
            catcol = (materials) ? 2 : -1;
            typecol = (materials) ? 3 : 1;
            numcol = (materials) ? 4 : 2;
            pricecol = (materials) ? -1 : 3;

            travelhistorycontrol.OnTravelSelectionChanged += Display;

            SetCheckBoxes();
        }

        void SetCheckBoxes()
        {
            checkBoxClear.Enabled = false;
            checkBoxClear.Checked = (materials) ? EDDiscoveryForm.EDDConfig.ClearMaterials : EDDiscoveryForm.EDDConfig.ClearCommodities;
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

            Dictionary<string, MaterialCommodities> mcchanges = new Dictionary<string, MaterialCommodities>();
            Dictionary<string, MaterialCommodities> mcorig = new Dictionary<string, MaterialCommodities>();

            if (editing)
            {
                foreach (MaterialCommodities m in GetMatCommodChanges())
                {
                    mcchanges[m.fdname] = m;
                }

                for (int i = 0; i < dataGridViewMC.Rows.Count; i++)
                {
                    if (dataGridViewMC.Rows[i].Tag != null && dataGridViewMC.Rows[i].Tag is MaterialCommodities)
                    {
                        MaterialCommodities rowmc = (MaterialCommodities)dataGridViewMC.Rows[i].Tag;
                        mcorig[rowmc.fdname] = rowmc;
                    }
                }
            }
            else
            {
                SetCheckBoxes();

                DisableEditing();
            }

            foreach (MaterialCommodities m in mc)
            {
                if (mcorig.ContainsKey(m.fdname) && mcchanges.ContainsKey(m.fdname))
                {
                    if (mcorig[m.fdname].count != m.count)
                    {
                        MaterialCommodities mcc = mcchanges[m.fdname];
                        mcc.count += m.count - mcorig[m.fdname].count;
                        mcchanges[m.fdname] = mcc;
                    }
                }

                mcorig[m.fdname] = m;
            }

            last_mc = mc;

            dataGridViewMC.Rows.Clear();

            if (mcorig.Count > 0)
            {
                labelNoItems.Visible = false;

                foreach (MaterialCommodities m in mcorig.Values)
                {
                    MaterialCommodities _m = m;

                    if (mcchanges.ContainsKey(m.fdname))
                    {
                        _m = mcchanges[m.fdname];
                    }

                    object[] rowobj;

                    if (materials)
                    {
                        rowobj = new[] { _m.name, _m.shortname, _m.category, _m.type, _m.count.ToString() };
                    }
                    else
                    {
                        rowobj = new[] { _m.name, _m.type, _m.count.ToString(), _m.price.ToString("0.#") };
                    }

                    int idx = dataGridViewMC.Rows.Add(rowobj);
                    dataGridViewMC.Rows[idx].Tag = m;
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

        bool ignoresi = false;
        private void comboBoxCustomAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoresi)
                return;

            int i = comboBoxCustomAdd.SelectedIndex;

            if (i < mclist.Count)
            {
                MaterialCommodities mc = mclist[i];

                int j = 0;

                for (; j < dataGridViewMC.Rows.Count; j++)
                {
                    if (((string)(dataGridViewMC.Rows[j].Cells[namecol].Value)).Equals(mc.name))
                        break;
                }

                if (j == dataGridViewMC.Rows.Count)
                {
                    object[] rowobj;

                    if (materials)
                    {
                        rowobj = new[] { mc.name, mc.shortname, mc.category, mc.type, "0" };
                    }
                    else
                    {
                        rowobj = new[] { mc.name, mc.type, "0", "0" };
                    }

                    int idx = dataGridViewMC.Rows.Add(rowobj);
                    dataGridViewMC.Rows[idx].Tag = mc;

                    labelNoItems.Visible = false;
                }
            }
            else
            {
                dataGridViewMC.Rows.Add();
                DataGridViewRow rw = dataGridViewMC.Rows[dataGridViewMC.Rows.Count - 1];
                rw.Cells[typecol].ReadOnly = true;
                if (materials)
                    rw.Cells[abvcol].ReadOnly = true;

                labelNoItems.Visible = false;
            }

            ignoresi = true;
            comboBoxCustomAdd.SelectedIndex = -1;
            comboBoxCustomAdd.Text = "Select another";
            ignoresi = false;
        }

        List<MaterialCommodities> mclist = new List<MaterialCommodities>();

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

        private void buttonExtModify_Click(object sender, EventArgs e)
        {
            if (buttonExtApply.Enabled)     // then its cancel
            {
                editing = false;
                DisableEditing();
                Display(last_mc);
            }
            else
            {
                dataGridViewMC.Columns[0].ReadOnly = dataGridViewMC.Columns[1].ReadOnly =
                    dataGridViewMC.Columns[3].ReadOnly = dataGridViewMC.Columns[2].ReadOnly = false;
                if (materials)
                    dataGridViewMC.Columns[4].ReadOnly = false;

                buttonExtApply.Enabled = true;
                comboBoxCustomAdd.Enabled = true;
                buttonExtModify.Text = "Cancel";

                ResetCombo();
                editing = true;
            }
        }

        private void ResetCombo()
        {
            List<MaterialCommodity> list = MaterialCommodity.GetMaterialsCommoditiesList;
            comboBoxCustomAdd.Items.Clear();
            mclist.Clear();
            foreach (MaterialCommodity mc in list)
            {
                if (!mc.category.Equals(MaterialCommodity.CommodityCategory) == materials)
                {
                    mclist.Add(new MaterialCommodities(mc));
                    comboBoxCustomAdd.Items.Add(mc.name);
                }
            }

            comboBoxCustomAdd.Items.Add("User defined");
        }

        private void DisableEditing()
        {
            if (buttonExtApply.Enabled)
            {
                foreach (DataGridViewColumn c in dataGridViewMC.Columns)
                    c.ReadOnly = true;

                comboBoxCustomAdd.Enabled = false;
                buttonExtApply.Enabled = false;
                buttonExtModify.Text = "Modify";
            }
        }

        private List<MaterialCommodities> GetMatCommodChanges(bool updatedb, out bool dbupdated)
        {
            List<MaterialCommodities> mcchange = new List<MaterialCommodities>();

            dbupdated = false;

            for (int i = 0; i < dataGridViewMC.Rows.Count; i++)
            {
                object tag = dataGridViewMC.Rows[i].Tag;
                string name = (string)dataGridViewMC.Rows[i].Cells[namecol].Value;
                string type = (string)dataGridViewMC.Rows[i].Cells[typecol].Value;
                string abv = (abvcol >= 0) ? (string)dataGridViewMC.Rows[i].Cells[abvcol].Value : "";
                string cat = (catcol >= 0) ? (string)dataGridViewMC.Rows[i].Cells[catcol].Value : (materials ? MaterialCommodities.MaterialRawCategory : MaterialCommodities.CommodityCategory);

                if (tag != null && tag is MaterialCommodities)
                {
                    MaterialCommodities mc = (MaterialCommodities)tag;

                    if (abvcol < 0)
                        abv = mc.shortname ?? "";
                    if (catcol < 0)
                        cat = mc.category ?? (materials ? MaterialCommodities.MaterialRawCategory : MaterialCommodities.CommodityCategory);

                    if (updatedb && (mc.name != name || mc.shortname != abv || mc.category != cat || mc.type != type))
                    {
                        //System.Diagnostics.Debug.WriteLine("Row " + i + " changed text");
                        MaterialCommodity.ChangeDbText(mc.fdname, name, abv, cat, type);
                        dbupdated = true;
                    }

                    int numvalue = 0;
                    int.TryParse((string)dataGridViewMC.Rows[i].Cells[numcol].Value, out numvalue);

                    double price = 0;
                    bool pricechange = false;

                    if (!materials)
                    {
                        double.TryParse((string)dataGridViewMC.Rows[i].Cells[pricecol].Value, out price);
                        pricechange = Math.Abs(mc.price - price) >= 0.01;
                    }

                    if (mc.count != numvalue || pricechange)
                    {
                        mcchange.Add(new MaterialCommodities(new MaterialCommodity(0, mc.category, mc.name, mc.fdname, mc.type, mc.shortname, Color.Red, 0), 0, numvalue, (pricechange) ? price : 0));
                        //System.Diagnostics.Debug.WriteLine("Row " + i + " changed number");
                    }
                }
                else
                {
                    string fdname = Tools.FDName(name);

                    int numvalue = 0;
                    bool numok = int.TryParse((string)dataGridViewMC.Rows[i].Cells[numcol].Value, out numvalue);

                    double price = 0;

                    if (!materials)
                        double.TryParse((string)dataGridViewMC.Rows[i].Cells[pricecol].Value, out price);

                    if (numok && cat.Length > 0 && name.Length > 0)
                    {
                        mcchange.Add(new MaterialCommodities(new MaterialCommodity(0, cat, name, fdname, type, abv, Color.Red, 0), 0, numvalue, price));
                    }
                }
            }

            return mcchange;
        }

        private List<MaterialCommodities> GetMatCommodChanges()
        {
            bool updateddb;
            return GetMatCommodChanges(false, out updateddb);
        }

        private void buttonExtApply_Click(object sender, EventArgs e)
        {
            bool updateddb = false;
            List<MaterialCommodities> mcchange = GetMatCommodChanges(true, out updateddb);

            DisableEditing();

            if (OnChangedCount != null)
                OnChangedCount(mcchange);
            else if ( updateddb && OnRequestRefresh != null)
                OnRequestRefresh();
        }

        private void dataGridViewMC_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("End edit");
            DataGridViewCell c = dataGridViewMC.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (e.ColumnIndex == numcol)
            {
                int originalvalue = (e.RowIndex < last_mc.Count) ? last_mc[e.RowIndex].count : 0;
                int newvalue = 0;
                if (!int.TryParse((string)c.Value, out newvalue) && newvalue >= 0)
                    c.Value = originalvalue.ToString();
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
