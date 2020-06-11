/*
 * Copyright © 2016 - 2020 EDDiscovery development team
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    // Search UCs use the UCCB template BUT are not directly inserted into the normal panels.. they are inserted into the Search UCCB
    // Make sure DB saving has unique names.. they all share the same displayno.

    public partial class SearchStars : UserControlCommonBase
    {
        private string DbColumnSave { get { return DBName("UCSearchStars", "DGVCol"); } }

        #region Init

        public SearchStars()
        {
            InitializeComponent();
            var corner = dataGridView.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridView.CheckEDSM = true;
            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.Height = Font.ScalePixels(26);
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            findSystemsUserControl.Init(displaynumber, "SearchFindSys", true, discoveryform);
            findSystemsUserControl.Excel += () => { dataGridView.Excel(3); };
            findSystemsUserControl.ReturnSystems += StarsFound;

            BaseUtils.Translator.Instance.Translate(this, new Control[] { findSystemsUserControl });
            BaseUtils.Translator.Instance.Translate(toolTip, this);
            //BaseUtils.Translator.Instance.Translate(dataGridViewEDSM.ContextMenu, this);

            dataGridView.Init(discoveryform);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg = thc;
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView, DbColumnSave);
            findSystemsUserControl.Closing();
        }

        #endregion

        private void StarsFound(List<Tuple<ISystem, double>> systems)       // systems may be null
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);

            dataGridView.Rows.Clear();

            if (systems != null)
            {
                DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[1];
                SortOrder sortorder = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Ascending;

                ISystem cursystem = discoveryform.history.CurrentSystem;        // could be null
                bool centresort = false;

                foreach (Tuple<ISystem, double> ret in systems)
                {
                    ISystem sys = ret.Item1;
                    string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";
                    object[] rowobj = {     sys.Name,
                                            (ret.Item2>=0 ? ret.Item2.ToString("0.#") : ""),
                                            (cursystem != null ? cursystem.Distance(sys).ToString("0.#") : ""),
                                            sys.X.ToString("0.#") + sep + sys.Y.ToString("0.#") + sep + sys.Z.ToString("0.#")
                                           };

                    dataGridView.Rows.Add(rowobj);
                    dataGridView.Rows[dataGridView.Rows.Count - 1].Tag = sys;
                    centresort |= ret.Item2 >= 0;
                }

                dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
            }

        }

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 1 || e.Column.Index == 2)
                e.SortDataGridViewColumnNumeric();
        }
        
    }
}
