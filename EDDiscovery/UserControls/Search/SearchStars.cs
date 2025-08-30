/*
 * Copyright © 2016 - 2023 EDDiscovery development team
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
 */

using EliteDangerousCore;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace EDDiscovery.UserControls
{
    // Search UCs use the UCCB template BUT are not directly inserted into the normal panels.. they are inserted into the Search UCCB
    // Make sure DB saving has unique names.. they all share the same displayno.

    public partial class SearchStars : UserControlCommonBase
    {
        private List<ISystem> routeSystems;

        #region Init

        public SearchStars()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);

            DBBaseName = "UCSearchStars";
        }

        protected override void Init()
        {
            dataGridView.WebLookup = EliteDangerousCore.WebExternalDataLookup.All;
            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.Height = Font.ScalePixels(26);
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            UserDatabaseSettingsSaver db = new UserDatabaseSettingsSaver(this, "SearchFindSys");

            findSystemsUserControl.Init(db, true, DiscoveryForm);
            findSystemsUserControl.Excel += () => { dataGridView.Excel(dataGridView.ColumnCount); };
            findSystemsUserControl.Map3D += () => { if (routeSystems?.Count > 0) RouteHelpers.Open3DMap(routeSystems, DiscoveryForm); };
            findSystemsUserControl.SaveExpedition += () => 
            {
                if (routeSystems?.Count > 0)
                {
                    string name = routeSystems[0].Name + " - " + routeSystems.Last().Name;
                    RouteHelpers.ExpeditionSave(this.FindForm(), name, routeSystems);
                }
            };
            findSystemsUserControl.SendToExpedition += () =>
            {
                if (routeSystems?.Count > 0)
                {
                    string name = routeSystems[0].Name + " - " + routeSystems.Last().Name;
                    RouteHelpers.ExpeditionPush(name, routeSystems, ParentUCCB, DiscoveryForm);       // we need to give it the search uc as the thing to push the request thru
                }
            };

            findSystemsUserControl.ReturnSystems += StarsFound;

            dataGridView.Init(DiscoveryForm);
        }

        protected override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView);
        }

        protected override void Closing()
        {
            DGVSaveColumnLayout(dataGridView);
            findSystemsUserControl.Save();
        }

        #endregion

        private void StarsFound(List<Tuple<ISystem, double>> systems)       // systems may be null
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);

            dataGridView.Rows.Clear();

            if ((systems?.Count??0) > 0)
            {
                if ((dataGridView.SortedColumn==null || dataGridView.SortedColumn == ColumnCentreDistance) && systems[0].Item2 == -1 )       // if sorting on centre distance, but no data due to search
                    dataGridView.Sort(ColumnStar, ListSortDirection.Ascending);

                DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
                SortOrder sortorder = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Ascending;

                ISystem cursystem = DiscoveryForm.History.CurrentSystem();        // could be null
                bool centresort = false;

                routeSystems = new List<ISystem>();
                int index = 1;
                foreach (Tuple<ISystem, double> ret in systems)
                {
                    ISystem sys = ret.Item1;
                    string sep = " / ";
                    object[] rowobj = {     index.ToString(),
                                            sys.Name,
                                            Stars.StarName(sys.MainStarType),
                                            (ret.Item2>=0 ? ret.Item2.ToString("0.#") : ""),
                                            (cursystem != null ? cursystem.Distance(sys).ToString("0.#") : ""),
                                            sys.X.ToString("0.#") + sep + sys.Y.ToString("0.#") + sep + sys.Z.ToString("0.#")
                                           };

                    var rowindex = dataGridView.Rows.Add(rowobj);
                    dataGridView.Rows[rowindex].Tag = sys;
                    centresort |= ret.Item2 >= 0;

                    routeSystems.Add(sys);
                    index++;
                }

                dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
            }

        }

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == ColumnIndex.Index || e.Column.Index == ColumnCentreDistance.Index || e.Column.Index == ColumnCurrentDistance.Index )
                e.SortDataGridViewColumnNumeric();
            else if ( e.Column.Index == ColumnStar.Index)
                e.SortDataGridViewColumnAlphaInt();
        }
        
    }
}
