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
        private string dbWordWrap = "WordWrap";

        private string searchterms = "system:body";
        private Timer searchtimer;
        private Timer updatetimer;


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

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, true);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            var enumlist = new Enum[] { EDTx.SearchScans_ColumnDate, EDTx.SearchScans_ColumnBody, EDTx.SearchScans_ColumnInformation,
                                       EDTx.SearchScans_ColumnSearches,  EDTx.SearchScans_ColumnPosition,  EDTx.SearchScans_ColumnParent, 
                                        EDTx.SearchScans_labelTime , EDTx.SearchScans_labelSearch};
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, subname: "SearchScans");

            var enumlisttt = new Enum[] { EDTx.UserControlDiscoveries_textBoxSearch_ToolTip, EDTx.UserControlDiscoveries_extButtonSearches_ToolTip,
                                EDTx.UserControlDiscoveries_extCheckBoxWordWrap_ToolTip, EDTx.UserControlDiscoveries_buttonExtExcel_ToolTip };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            discoveryform.OnNewEntry += NewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;

            rollUpPanelTop.SetToolTip(toolTip);     // set after translator

            TravelHistoryFilter.InitaliseComboBox(comboBoxTime, GetSetting(dbTimeWindow, ""), incldockstartend: true, inclnumberlimit:false);

            PopulateCtrlList();

            dataGridView.Init(discoveryform);
            dataGridView.Columns[4].Tag = dataGridView.Columns[5].Tag = "TextPopOut";       // these two double click are text popouts

            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;
            updatetimer = new Timer() { Interval = 1000 };
            updatetimer.Tick += Updatetimer_Tick;

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
            searchtimer.Stop();
            updatetimer.Stop();

            DGVSaveColumnLayout(dataGridView);
            PutSetting("PinState", rollUpPanelTop.PinState);
            discoveryform.OnNewEntry -= NewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            updatetimer.Stop();
            Draw();
        }

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made.. check to see if its a scan update
        {
            // Star scan type, or material entry type, or a bodyname/id entry, or not set, or not same system
            if (HistoryListQueries.AllSearchableJournalTypes.Contains(he.EntryType))
            {
                updatetimer.Stop();         // we kick the timer, to let multiple ones in, so we only search once when we get a glut of scans
                updatetimer.Start();
                DrawOnlySys = he.System;
                System.Diagnostics.Debug.WriteLine($"Discoveries {Environment.TickCount % 10000} timer started new entry on {DrawOnlySys.Name}");
            }
        }

        private void Updatetimer_Tick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Discoveries {Environment.TickCount % 10000} timer expired");
            updatetimer.Stop();
            Draw();
        }

        #endregion

        #region Display

        public int DrawCount = 0;       // incremented/decremeneted in UI thread to count number of draw requests
        public ISystem DrawOnlySys = null;  // if to limit to one system

        private async void Draw()       // we can, due to the await below, reenter this while the drawcount loop is executing, in the UI thread.
        {
            bool todraw = DrawCount++ == 0;

            if ( todraw )       // if DrawCount was zero, its the first draw, try it
            {
                this.Cursor = Cursors.WaitCursor;
                labelCount.Text = "...";

                while (DrawCount > 0)       // loop around while >0.   The execute search aboves if DrawCount>0 until we get to DrawCount=1 at which point we display result
                {
                    var entries = discoveryform.history.EntryOrder();
                    bool updateit = false;

                    if (DrawOnlySys != null)    // if filter by system
                    {
                        System.Diagnostics.Debug.WriteLine($"Discoveries {Environment.TickCount % 10000} Search Limit to {DrawOnlySys.Name}");
                        entries = entries.Where(x => x.System.Name == DrawOnlySys.Name).ToList();
                        updateit = true;
                        DrawOnlySys = null;     // cancel it. If another Draw is called, then this current one will abort and the next will be with the full list
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Discoveries {Environment.TickCount % 10000} Search All");
                    }

                    var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;
                    List<HistoryEntry> helist = filter.Filter(entries, HistoryListQueries.AllSearchableJournalTypes, false); // in entry order

                    Dictionary<string, HistoryListQueries.Results> searchresults;
                    if (helist.Count > 0 && searchesactive.Length > 0)      // if anything
                    {
                        searchresults = await ExecuteSearch(helist);
                    }
                    else
                    {
                        searchresults = null;
                    }

                    if (DrawCount == 1)     // if we have a single draw outstanding, then draw it, else don't bother, we are going to do another draw
                        DrawGrid(searchresults, updateit);

                    DrawCount--;
                }

                this.Cursor = Cursors.Default;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Discoveries {Environment.TickCount % 10000} We are drawing - ignoring");
            }
        }

        private async Task<Dictionary<string, HistoryListQueries.Results>> ExecuteSearch(List<HistoryEntry> helist)
        {
            discoveryform.history.FillInScanNode();     // ensure all journal scan entries point to a scan node (expensive, done only when reqired in this panel)

            var defaultvars = new BaseUtils.Variables();
            // we want to keep the doubleness of values as this means when divided by the eval engine we get a float/float divide
            defaultvars.AddPropertiesFieldsOfClass(new BodyPhysicalConstants(), "", null, 10, ensuredoublerep:true);    

            Dictionary<string, HistoryListQueries.Results> searchresults = new Dictionary<string, HistoryListQueries.Results>();

            System.Diagnostics.Debug.WriteLine($"Discoveries {Environment.TickCount % 10000} DC {DrawCount} runs {searchesactive.Length} searches on {helist.Count} entries");

            var sw = new System.Diagnostics.Stopwatch(); sw.Start();

            foreach (var searchname in searchesactive)
            {
                await HistoryListQueries.Instance.Find(helist, searchresults, searchname, defaultvars, false); // execute the searches
                //System.Threading.Thread.Sleep(1000);
                if (IsClosed)       // may be closing during async process
                    return null;

                if (DrawCount > 1)      // if we have another draw request, stop
                {
                    System.Diagnostics.Debug.WriteLine($"Discoveries {Environment.TickCount % 10000} abort search due to multiple draw counts");
                    return null;
                }
            }

            System.Diagnostics.Debug.WriteLine($"Discoveries {Environment.TickCount % 10000} find took {sw.ElapsedMilliseconds} Returned {searchresults.Count}");

            return searchresults;
        }

        private void DrawGrid(Dictionary<string, HistoryListQueries.Results> searchresults, bool updategrid = false)
        {
            if (!updategrid)
                dataGridView.Rows.Clear();

            DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
            SortOrder sortorder = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Descending;

            //ISystem cursystem = discoveryform.history.CurrentSystem();        // could be null

            var search = new BaseUtils.StringSearchTerms(textBoxSearch.Text, searchterms);

            foreach (var kvp in searchresults.EmptyIfNull())
            {
                HistoryEntry he = kvp.Value.HistoryEntry;
                ISystem sys = he.System;
                string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";

                SearchScans.GenerateReportFields(he, out string name, out string info, out string pinfo);

                string[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC).ToString(),            //0
                                        name,       //1
                                        sys.X.ToString("0.##") + sep + sys.Y.ToString("0.##") + sep + sys.Z.ToString("0.##"),   //2
                                        string.Join(", " + Environment.NewLine, kvp.Value.FiltersPassed),   //3
                                        info,   //4
                                        pinfo,  //5
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

                    if (!matched)
                        continue;
                }

                bool addto = true;

                if (updategrid)
                {
                    int existingrow = dataGridView.FindRowWithValue(1, name);       // is the result there with the name?
                    if ( existingrow>=0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Discoveries alter row {existingrow}");
                        addto = false;
                        dataGridView.Rows[existingrow].Cells[3].Value = rowobj[3];      // update what may have changed in 4,5,6. Rest will stay the same.
                        dataGridView.Rows[existingrow].Cells[4].Value = rowobj[4];
                        dataGridView.Rows[existingrow].Cells[5].Value = rowobj[5];
                    }
                }

                if ( addto )
                {
                    dataGridView.Rows.Add(rowobj);
                    dataGridView.Rows[dataGridView.Rows.Count - 1].Tag = he.System;
                }
            }

            System.Diagnostics.Debug.WriteLine($"Discoveries {Environment.TickCount % 10000} grid drawn");

            if (dataGridView.Rows.Count > 0)
                labelCount.Text = "Total".TxID(EDTx.UserControlMaterialCommodities_Total) + " " + dataGridView.Rows.Count.ToString();
            else
                labelCount.Text = "";

            dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

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
            displayfilter.AddGroupOption(HistoryListQueries.DefaultSearches, "Default".T(EDTx.ProfileEditor_Default));
            displayfilter.SettingsSplittingChar = '\u2188';     // pick a crazy one soe

            var searches = HistoryListQueries.Instance.Searches.Where(x => x.UserOrBuiltIn).ToList();
            foreach (var s in searches)
                displayfilter.AddStandardOption(s.Name, s.Name);

            CommonCtrl(displayfilter, extButtonSearches, dbSearches);
        }

        private string[] searchesactive;
        private void PopulateCtrlList()
        {
            var searches = HistoryListQueries.Instance.Searches.Where(x => x.UserOrBuiltIn).Select(y => y.Name).ToList();

            string set = GetSetting(dbSearches, HistoryListQueries.DefaultSearches);
            PutSetting(dbSearches, set);    // make sure its back into memory

            searchesactive = set.SplitNoEmptyStartFinish('\u2188');
        }

        private void CommonCtrl(ExtendedControls.CheckedIconListBoxFormGroup displayfilter, Control under, string saveasstring)
        {
            displayfilter.CloseBoundaryRegion = new Size(32, under.Height);
            displayfilter.AllOrNoneBack = false;
            displayfilter.ImageSize = new Size(24, 24);
            displayfilter.ScreenMargin = new Size(0, 0);

            displayfilter.SaveSettings = (s, o) =>
            {
                string curset = GetSetting(saveasstring, "xx");
                PutSetting(saveasstring, s);
                if (s != curset)
                {
                    System.Diagnostics.Debug.WriteLine($"Discoveries selected {s}");
                    PopulateCtrlList();
                    Draw();
                }
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

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }

        private void UpdateWordWrap()
        {
            dataGridView.DefaultCellStyle.WrapMode = extCheckBoxWordWrap.Checked ? DataGridViewTriState.True : DataGridViewTriState.False;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataViewScrollerPanel.UpdateScroll();
        }

        #endregion

        #region Export

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            dataGridView.Excel(dataGridView.ColumnCount);
        }
        #endregion

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index == 1)
                e.SortDataGridViewColumnNumeric();

        }
    }
}