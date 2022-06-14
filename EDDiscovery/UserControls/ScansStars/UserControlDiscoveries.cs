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
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlDiscoveries : UserControlCommonBase
    {
        private string dbTimeWindow = "TimeWindow";
        private string dbSearches = "Searches";
        private JournalTypeEnum[] journaltypes = new JournalTypeEnum[] { JournalTypeEnum.Scan, JournalTypeEnum.FSSBodySignals, JournalTypeEnum.SAASignalsFound };
        private string searchterms = "system:body:station:stationfaction";
        private Timer searchtimer;

        #region Init
        public UserControlDiscoveries()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "Discoveries";

            dataGridView.CheckEDSM = false; // for this, only our data is shown
            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.Height = Font.ScalePixels(26);
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            var enumlist = new Enum[] { EDTx.SearchScans_ColumnDate, EDTx.SearchScans_ColumnStar, EDTx.SearchScans_ColumnInformation,
                                        EDTx.SearchScans_ColumnCurrentDistance,  EDTx.SearchScans_ColumnSearches,  EDTx.SearchScans_ColumnPosition,  EDTx.SearchScans_ColumnParent, 
                                        EDTx.SearchScans_labelTime , EDTx.SearchScans_labelSearch};
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, subname: "SearchScans");

            discoveryform.OnNewEntry += NewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;

            rollUpPanelTop.SetToolTip(toolTip);     // set after translator

            TravelHistoryFilter.InitaliseComboBox(comboBoxTime, GetSetting(dbTimeWindow, ""), incldockstartend: true, inclnumberlimit:false);

            PopulateCtrlList();

            dataGridView.Init(discoveryform);

            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;

        }


        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView);                
        }

        public override void InitialDisplay()
        {
            Draw();
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView);
            PutSetting("PinState", rollUpPanelTop.PinState);
            discoveryform.OnNewEntry -= NewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }

        #endregion

        #region Display

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made.. check to see if its a scan update
        {
            // Star scan type, or material entry type, or a bodyname/id entry, or not set, or not same system
            if (journaltypes.Contains(he.EntryType))
            {
                Draw();
            }
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            Draw();
        }
        private void Draw()
        {
            lock (journaltypes)     // Don't allow double drawing due to await in DrawAsync, and then another hisotyr/newentry occurs
                DrawAsync();
        }

        private async void DrawAsync()
        {
            this.Cursor = Cursors.WaitCursor;

            dataGridView.Rows.Clear();

            DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
            SortOrder sortorder = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Descending;

            var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;
            List<HistoryEntry> helist = filter.Filter(discoveryform.history.EntryOrder(), journaltypes, false); // in entry order

            labelCount.Text = "...";

            if (helist.Count > 0 && searchesactive.Length > 0)
            {
                discoveryform.history.FillInScanNode();     // ensure all journal scan entries point to a scan node (expensive, done only when reqired in this panel)

                var defaultvars = new BaseUtils.Variables();
                defaultvars.AddPropertiesFieldsOfClass(new BodyPhysicalConstants(), "", null, 10);

                Dictionary<string, HistoryListQueries.Results> searchresults = new Dictionary<string, HistoryListQueries.Results>();

                System.Diagnostics.Debug.WriteLine($"{Environment.TickCount} Discoveries runs {searchesactive.Length} searches");

                var sw = new System.Diagnostics.Stopwatch(); sw.Start();

                foreach (var searchname in searchesactive)
                {
                    await HistoryListQueries.Instance.Find(helist, searchresults, searchname, defaultvars, false); // execute the searches
                    if (IsClosed)       // may be closing during async process
                        return;
                }

                System.Diagnostics.Debug.WriteLine($"Discoveries Find complete {sw.ElapsedMilliseconds} on {helist.Count}");

                ISystem cursystem = discoveryform.history.CurrentSystem();        // could be null

                var search = new BaseUtils.StringSearchTerms(textBoxSearch.Text, searchterms);

                foreach (var kvp in searchresults.EmptyIfNull())
                {
                    HistoryEntry he = kvp.Value.HistoryEntry;
                    ISystem sys = he.System;
                    string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";

                    JournalScan js = he.journalEntry as JournalScan;
                    JournalFSSBodySignals jb = he.journalEntry as JournalFSSBodySignals;
                    JournalSAASignalsFound jbs = he.journalEntry as JournalSAASignalsFound;

                    string name, info, pinfo = "";
                    if (js != null)
                    {
                        name = js.BodyName;
                        info = js.DisplayString();
                        if (he.ScanNode?.Parent != null)
                        {
                            var parentjs = he.ScanNode?.Parent?.ScanData;               // parent journal entry, may be null
                            pinfo = parentjs != null ? parentjs.DisplayString() : he.ScanNode.Parent.CustomNameOrOwnname + " " + he.ScanNode.Parent.NodeType;
                        }
                    }
                    else if (jb != null)
                    {
                        name = jb.BodyName;
                        jb.FillInformation(he.System, "", out info, out string d);
                    }
                    else
                    {
                        name = jbs.BodyName;
                        jbs.FillInformation(he.System, "", out info, out string d);
                    }

                    string[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC).ToString(),
                                            name,
                                            sys.X.ToString("0.##") + sep + sys.Y.ToString("0.##") + sep + sys.Z.ToString("0.##"),
                                            (cursystem != null ? cursystem.Distance(sys).ToString("0.#") : ""),
                                            string.Join(Environment.NewLine, kvp.Value.FiltersPassed),
                                            info,
                                            pinfo,
                                            };

                    if ( search.Enabled )
                    {
                        bool matched = false;

                        if (search.Terms[0] != null)   // primary text
                        {
                            foreach (var col in rowobj)
                            {
                                if (col.IndexOf(search.Terms[0], StringComparison.InvariantCultureIgnoreCase) >= 0)
                                {
                                    matched = true;
                                    break;
                                }
                            }
                        }

                        if (!matched && search.Terms[1] != null)       // system
                            matched = he.System.Name.WildCardMatch(search.Terms[1], true);
                        if (!matched && search.Terms[2] != null)       // body
                            matched = he.Status.BodyName?.WildCardMatch(search.Terms[2], true) ?? false;
                        if (!matched && search.Terms[3] != null)       // station
                            matched = he.Status.StationName?.WildCardMatch(search.Terms[3], true) ?? false;
                        if (!matched && search.Terms[4] != null)       // stationfaction
                            matched = he.Status.StationFaction?.WildCardMatch(search.Terms[4], true) ?? false;

                        if (!matched)
                            continue;
                    }

                    dataGridView.Rows.Add(rowobj);
                    dataGridView.Rows[dataGridView.Rows.Count - 1].Tag = he.System;
                }

                System.Diagnostics.Debug.WriteLine($"Discoveries took {sw.ElapsedMilliseconds} Returned {searchresults.Count}");
            }

            if (dataGridView.Rows.Count > 0)
                labelCount.Text = "Total".TxID(EDTx.UserControlMaterialCommodities_Total) + " " + dataGridView.Rows.Count.ToString();
            else
                labelCount.Text = "";

            dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

            this.Cursor = Cursors.Default;
        }

        #endregion

        #region UI

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            PutSetting(dbTimeWindow, comboBoxTime.Text);
            Draw();
        }

        private void extButtonSearches_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();
            displayfilter.AddAllNone();
            displayfilter.SettingsSplittingChar = '\u2188';     // pick a crazy one soe

            var searches = HistoryListQueries.Instance.Searches.Where(x => x.Standard || x.User).ToList();
            foreach (var s in searches)
                displayfilter.AddStandardOption(s.Name, s.Name);

            CommonCtrl(displayfilter, extButtonSearches, dbSearches);
        }

        private string[] searchesactive;
        private void PopulateCtrlList()
        {
            var searches = HistoryListQueries.Instance.Searches.Where(x => x.Standard || x.User).Select(y => y.Name).ToList();

            var slist = string.Join('\u2188'.ToString(), searches);     // default is all
            string set = GetSetting(dbSearches, slist);
            PutSetting(dbSearches, set);    // make sure its back into memory

            searchesactive = set.SplitNoEmptyStartFinish('\u2188');
        }

        private void CommonCtrl(ExtendedControls.CheckedIconListBoxFormGroup displayfilter, Control under, string saveasstring)
        {
            displayfilter.AllOrNoneBack = false;
            displayfilter.ImageSize = new Size(24, 24);
            displayfilter.ScreenMargin = new Size(0, 0);

            displayfilter.SaveSettings = (s, o) =>
            {
                if (saveasstring == null)
                    PutBoolSettingsFromString(s, displayfilter.SettingsTagList());
                else
                    PutSetting(saveasstring, s);

                PopulateCtrlList();
                Draw();
            };


            displayfilter.Show(GetSetting(saveasstring, ""), under, this.FindForm());
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            searchtimer.Stop();
            searchtimer.Start();
            //System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + "Char");
        }

        private void Searchtimer_Tick(object sender, EventArgs e)
        {
            searchtimer.Stop();
            Draw();
        }

        #endregion

        #region Export

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            dataGridView.Excel(dataGridView.ColumnCount);
        }
        #endregion

    }
}