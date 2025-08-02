/*
 * Copyright 2016-2024 EDDiscovery development team
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
using EliteDangerousCore.JournalEvents;
using QuickJSON;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    // Search UCs use the UCCB template BUT are not directly inserted into the normal panels.. they are inserted into the Search UCCB
    // Make sure DB saving has unique names.. they all share the same displayno.

    public partial class SearchScans : UserControlCommonBase
    {
        private string dbQuerySave = "Query";
        private string dbQuerySortCondition = "QuerySortCondition";
        private string dbQuerySortAscending = "QuerySortAscending";
        private string dbSplitterSave = "ScanSplitter";
        private string dbWordWrap = "ScanWordWrap";

        private string lastresultlog = null;

        #region Init

        public SearchScans()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip, this);
        }

        protected override void Init()
        {
            DBBaseName = "UCSearchScans";

            dataGridView.WebLookup = EliteDangerousCore.WebExternalDataLookup.None; // for this, only our data is shown
            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.Height = Font.ScalePixels(26);
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, true);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            List<BaseUtils.TypeHelpers.PropertyNameInfo> classnames = HistoryListQueries.PropertyList();

            conditionFilterUC.AutoCompleteOnMatch = true;
            conditionFilterUC.VariableNames = classnames;

            // output for wiki
            // string t = "";  foreach (var pni in classnames) t += $"{pni.Name} | {pni.Help.Replace(Environment.NewLine, ", ")} | {pni.Comment} {Environment.NewLine}"; System.IO.File.WriteAllText(@"c:\code\props.txt", t);

            splitContainer.SplitterDistance(GetSetting(dbSplitterSave, 0.2));

            string query = GetSetting(dbQuerySave, "");
            conditionFilterUC.InitConditionList(new BaseUtils.ConditionLists(query));   // will ignore if query is bad and return empty query
            scanSortControl.Set(GetSetting(dbQuerySortCondition, ""), GetSetting(dbQuerySortAscending, false));
            scanSortControl.AutoCompletes = new List<string>() 
            {
                "Compare(left.nMassKG,right.nMassKG)",
                "Compare(left.nEccentricity,right.nEccentricity)",
                "Compare(left.nOrbitalPeriod,right.nOrbitalPeriod)",
                "Compare(left.DistanceFromArrivalm,right.DistanceFromArrivalm)",
                "Compare(left.nRadius,right.nRadius)" ,
                "Compare(left.nRotationPeriod,right.nRotationPeriod)",
                "Compare(left.nSemiMajorAxis,right.nSemiMajorAxis)",
                "Compare(left.nAxialTilt,right.nAxialTilt)",

                "Compare(left.nSurfaceGravityG,right.nSurfaceGravityG)", 
                "Compare(left.nSurfaceTemperature,right.nSurfaceTemperature)",
                "Compare(left.nSurfacePressure,right.nSurfacePressure)",

                "Compare(left.nStellarMass,right.nStellarMass)",
                "Compare(left.nAbsoluteMagnitude,right.nAbsoluteMagnitude)",
                "Compare(left.nAge,right.nAge)",
            };

            scanSortControl.SortDirectionClicked += (chk) => { if (scanSortControl.Condition.HasChars()) SortGridBySortCriteria(); };

            dataGridView.Init(DiscoveryForm);
            dataGridView.Columns[4].Tag = "TooltipPopOut;TextPopOut";
            dataGridView.Columns[5].Tag = "TextPopOut";  // these two double click are text popouts

            dataGridView.UserChangedColumnVisibility += ChangeColumnVisibility;

            UpdateComboBoxSearches();
            comboBoxSearches.Text = "Select".Tx();
            comboBoxSearches.SelectedIndexChanged += ComboBoxSearches_SelectedIndexChanged;

            labelCount.Visible = false;

            dataGridView.GotoEntryClicked += (he) =>
            {
                if (ParentUCCB.RequestPanelOperation(this, new UserControlCommonBase.RequestTravelToJID() { JID = he.Journalid, MakeVisible = true }) == PanelActionState.Failed)
                    ExtendedControls.MessageBoxTheme.Show(DiscoveryForm, "Entry filtered out of grid".Tx(), "Warning".Tx());
            };

        }
        protected override void LoadLayout()
        {
            bool loaded = DGVLoadColumnLayout(dataGridView);
            
            if (!loaded)        // in this panel, we hide some when we have no stored setting to simplify the default view
            {
                ColumnParentParent.Visible = false;
                ColumnStar.Visible = false;
                ColumnStarStar.Visible = false;
            }

        }

        protected override void Closing()
        {
            DGVSaveColumnLayout(dataGridView);
            conditionFilterUC.Check();      // checks, ignore string return errors, fills in Result
            PutSetting(dbQuerySave, conditionFilterUC.Result.ToString());
            PutSetting(dbQuerySortCondition, scanSortControl.Condition);
            PutSetting(dbQuerySortAscending, scanSortControl.Ascending);
            PutSetting(dbSplitterSave, splitContainer.GetSplitterDistance());
        }

        #endregion

        #region UI

        private void ComboBoxSearches_SelectedIndexChanged(object sender, EventArgs e)
        {
            conditionFilterUC.Clear();
            var entry = HistoryListQueries.Instance.Searches[comboBoxSearches.SelectedIndex];
            conditionFilterUC.LoadConditions(new BaseUtils.ConditionLists(entry.Condition));
            scanSortControl.Set(entry.SortCondition, entry.SortAscending);
        }

        private void UpdateComboBoxSearches()
        {
            comboBoxSearches.Items.Clear();
            comboBoxSearches.Items.AddRange(HistoryListQueries.Instance.Searches.Select(x => x.Name));
        }

        private void extButtonNew_Click(object sender, EventArgs e)
        {
            UpdateComboBoxSearches();
            conditionFilterUC.Clear();
            comboBoxSearches.SelectedIndexChanged -= ComboBoxSearches_SelectedIndexChanged;
            comboBoxSearches.SelectedIndex = -1;
            comboBoxSearches.SelectedIndexChanged += ComboBoxSearches_SelectedIndexChanged;
        }

        private void extButtonSave_Click(object sender, EventArgs e)
        {
            BaseUtils.ConditionLists cond = Valid();
            if (cond != null)
            {
                string name = ExtendedControls.PromptSingleLine.ShowDialog(this.FindForm(), "Name".Tx()+": ", "", "Enter Search Name".Tx()+": ", this.FindForm().Icon, requireinput:true);
                if (name != null)
                {
                    HistoryListQueries.Instance.Set(name, cond.ToString(), HistoryListQueries.QueryType.User);
                    HistoryListQueries.Instance.SaveUserQueries();
                    UpdateComboBoxSearches();
                    comboBoxSearches.SelectedIndexChanged -= ComboBoxSearches_SelectedIndexChanged;
                    comboBoxSearches.SelectedItem = name;
                    comboBoxSearches.SelectedIndexChanged += ComboBoxSearches_SelectedIndexChanged;
                }
            }
        }
        private BaseUtils.ConditionLists Valid()
        {
            string errs = conditionFilterUC.Check();
            if (errs.HasChars())
            {
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Condition is not valid".Tx(), "Condition".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            else
                return conditionFilterUC.Result;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string name = comboBoxSearches.Text;
            if (comboBoxSearches.SelectedIndex>= 0 && HistoryListQueries.Instance.Searches[comboBoxSearches.SelectedIndex].User && name.HasChars())
            {
                if (ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Confirm deletion of".Tx()+ " " + name, "Delete".Tx(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    HistoryListQueries.Instance.Delete(name);
                    HistoryListQueries.Instance.SaveUserQueries();
                    UpdateComboBoxSearches();
                    comboBoxSearches.SelectedIndexChanged -= ComboBoxSearches_SelectedIndexChanged;
                    comboBoxSearches.SelectedIndex = -1;
                    comboBoxSearches.SelectedIndexChanged += ComboBoxSearches_SelectedIndexChanged;
                }
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Cannot delete this entry".Tx(), "Delete".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private async void buttonFind_Click(object sender, EventArgs e)
        {
            BaseUtils.ConditionLists cond = Valid();
            if (cond != null)
            {
                this.Cursor = Cursors.WaitCursor;
                dataGridView.Rows.Clear();
                labelCount.Text = "...";

                DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
                SortOrder sortorder = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Descending;

                DiscoveryForm.History.FillInScanNode();     // ensure all journal scan entries point to a scan node (expensive, done only when reqired in this panel)

                QueryFunctionHandler func = new QueryFunctionHandler();     // use same func handler as query
                BaseUtils.Eval evl = new BaseUtils.Eval(func);

                BaseUtils.Condition.InUse(cond.List, evl, out HashSet<string> allvars, out HashSet<string> allfuncs);

                // see if we need any default vars, at the moment, they all start with one
                var defaultvars = new BaseUtils.Variables();

                // we want to keep the doubleness of values as this means when divided by the eval engine we get a float/float divide
                if (allvars.StartsWithInList("one") >= 0)
                    defaultvars.AddPropertiesFieldsOfClass(new BodyPhysicalConstants(), "", null, 10,ensuredoublerep:true);
                else
                    defaultvars = null;

                //System.Diagnostics.Debug.WriteLine(defaultvars.ToString(separ:Environment.NewLine));

                var results = new Dictionary<string, List<HistoryListQueries.ResultEntry>>();

                var computedsearch = HistoryListQueries.NeededSearchableTypes(allvars,allfuncs);
                var helist = HistoryList.FilterByEventEntryOrder(DiscoveryForm.History.EntryOrder(), computedsearch);
                System.Diagnostics.Debug.WriteLine($"Helist is {helist.Count} entryorder {DiscoveryForm.History.EntryOrder().Count}");

                var sw = new System.Diagnostics.Stopwatch(); sw.Start();

                lastresultlog = await HistoryListQueries.Find(helist, results, "", cond, defaultvars, DiscoveryForm.History.StarScan, extCheckBoxDebug.Checked);

                if (IsClosed)       // may be closing during async process
                    return;

                System.Diagnostics.Debug.WriteLine($"Find complete {sw.ElapsedMilliseconds} on {helist.Count} results {results.Count}");

                ISystem cursystem = DiscoveryForm.History.CurrentSystem();        // could be null

                if (scanSortControl.Condition.HasChars())       // before we present, and we have a sort condition, update the sort vars
                {
                }

                int max = 10000;

                foreach ( var kvp in results.Take(max).EmptyIfNull())
                {
                    string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";

                    HistoryListQueries.GenerateReportFields(kvp.Key, kvp.Value, out string name, out string info, out string infotooltip, 
                                                            ColumnParent.Visible, out string pinfo,
                                                            ColumnParentParent.Visible, out string ppinfo, 
                                                            ColumnStar.Visible, out string sinfo, 
                                                            ColumnStarStar.Visible, out string ssinfo);

                    HistoryEntry he = kvp.Value.Last().HistoryEntry;
                    ISystem sys = he.System;

                    object[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC).ToString(),
                                            he.System.Name,
                                            name,
                                            he.System.X.ToString("0.##") + sep + sys.Y.ToString("0.##") + sep + sys.Z.ToString("0.##"),
                                            (cursystem != null ? cursystem.Distance(sys).ToString("0.#") : ""),
                                            info,  
                                            pinfo,
                                            ppinfo,
                                            sinfo,
                                            ssinfo,
                                            };

                    int row = dataGridView.Rows.Add(rowobj);
                    dataGridView.Rows[row].Tag = he;
                    dataGridView.Rows[row].Cells[0].Tag = kvp.Value;
                    dataGridView.Rows[row].Cells[4].ToolTipText = infotooltip;
                }

                if ( results.Count > max )
                {
                    object[] rowobj = { "","", "","",string.Format("Too many results to display, truncating to the first {0}".Tx(), max) };
                    dataGridView.Rows.Add(rowobj);
                }

                System.Diagnostics.Debug.WriteLine($"Search took {sw.ElapsedMilliseconds} Returned {results.Count}");

                if (scanSortControl.Condition.HasChars())       // custom sort, apply
                {
                    SortGridBySortCriteria();
                }
                else
                {
                    dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                    dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
                }

                this.Cursor = Cursors.Default;

                labelCount.Text = "Total".Tx()+ " " + results.Count + " / " + helist.Count.ToString();
                labelCount.Visible = true;
            }

        }

        private void extCheckBoxDebug_CheckedChanged(object sender, EventArgs e)
        {
            if (extCheckBoxDebug.Enabled == true)
            {
                if (extCheckBoxDebug.Checked == false)  // if turning it off, we don't allow that..
                {
                    extCheckBoxDebug.Enabled = false;
                    extCheckBoxDebug.Checked = true;        // turn it back on, causes recursion, so use enable to say don't
                    extCheckBoxDebug.Enabled = true;

                    if (lastresultlog.HasChars())
                    {
                        string fname = System.IO.Path.GetTempFileName() + ".log";
                        if ( BaseUtils.FileHelpers.TryWriteToFile(fname,lastresultlog))
                        {
                            System.Diagnostics.Process.Start(fname);
                        }
                    }
                }
            }
        }

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }

        private void UpdateWordWrap()
        {
            dataGridView.SetWordWrap(extCheckBoxWordWrap.Checked);
            dataViewScrollerPanel.UpdateScroll();
        }

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            dataGridView.Excel(dataGridView.ColumnCount);
        }

        private void extButtonExport_Click(object sender, EventArgs e)
        {
            var ja = HistoryListQueries.Instance.QueriesInJSON(HistoryListQueries.QueryType.User);
            if (ja != null && ja.Count > 0)
            {
                JObject hdr = new JObject() { ["Searches-Version"] = "1.0" , ["Searches"] = ja};        // note the file format..

                SaveFileDialog dlg = new SaveFileDialog();

                dlg.Filter = "Query| *.edduserq";
                dlg.Title = "Export user searches".Tx();

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    string path = dlg.FileName;
                    if (!BaseUtils.FileHelpers.TryWriteToFile(path, hdr.ToString(true)))
                    {
                        CSVHelpers.WriteFailed(this.FindForm(), path);
                    }
                }
            }
            else
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No searches to export", "Searches", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void extButtonImport_Click(object sender, EventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Query| *.edduserq";
            dlg.Title = "Import user searches".Tx();

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                string path = dlg.FileName;
                string text = BaseUtils.FileHelpers.TryReadAllTextFromFile(path);

                if ( text != null)
                {
                    JObject jo = JObject.Parse(text, JToken.ParseOptions.CheckEOL);
                    if ( jo != null && jo.Contains("Searches-Version") && jo.Contains("Searches"))
                    {
                        JArray ja = jo["Searches"].Array();

                        if (ja != null && HistoryListQueries.Instance.ReadJSONQueries(ja, HistoryListQueries.QueryType.User))
                        {
                            UpdateComboBoxSearches();
                            HistoryListQueries.Instance.SaveUserQueries();
                            return;     // all okay
                        }
                            
                    }
                }

                CSVHelpers.FailedToOpen(this.FindForm(), path); // both fail to this
            }
        }


        #endregion

        #region Data Grid View/Sort

        public class RowComparer : System.Collections.IComparer
        {
            public string InError { get; private set; }

            private bool ascending;
            private string condition;
            private BaseUtils.Eval evl = new BaseUtils.Eval(new QueryFunctionHandler());
            private HashSet<string>[] sorteval = new HashSet<string>[2];

            public RowComparer(string c, bool ad)
            {
                condition = c;
                ascending = ad;
                InError = null;

                evl.SymbolsFuncsInExpression(condition, out HashSet<string> vars, out HashSet<string> _);
                sorteval[0] = vars.Where(x => x.StartsWith("left.")).Select(x => x.Substring(5)).ToHashSet();
                sorteval[1] = vars.Where(x => x.StartsWith("right.")).Select(x => x.Substring(6)).ToHashSet();
            }

            public int Compare(object lo, object ro)
            {
                DataGridViewRow leftrow = (DataGridViewRow)lo;
                DataGridViewRow rightrow = (DataGridViewRow)ro;

                List<HistoryListQueries.ResultEntry> left = leftrow.Cells[0].Tag as List<HistoryListQueries.ResultEntry>;
                List<HistoryListQueries.ResultEntry> right = rightrow.Cells[0].Tag as List<HistoryListQueries.ResultEntry>;

                HistoryEntry lefthe = left?.Where(x => x.HistoryEntry.EntryType == JournalTypeEnum.Scan).Select(y=>y.HistoryEntry).FirstOrDefault();
                HistoryEntry righthe = right?.Where(x => x.HistoryEntry.EntryType == JournalTypeEnum.Scan).Select(y=>y.HistoryEntry).FirstOrDefault();

                // if we have left, and right, we can compare
                if (lefthe != null)
                {
                    if (righthe != null)
                    {
                        JournalScan leftscan = lefthe.journalEntry as JournalScan;
                        JournalScan rightscan = righthe.journalEntry as JournalScan;
                        Type[] ignoretypes = new Type[] { typeof(System.Drawing.Icon), typeof(System.Drawing.Image), typeof(System.Drawing.Bitmap), typeof(QuickJSON.JObject) };

                        // sorteval already has varsinuse computed, extract variables from scans

                        BaseUtils.Variables values = new BaseUtils.Variables();

                        values.AddPropertiesFieldsOfClass(leftscan, "left.", ignoretypes, 5, sorteval[0], ensuredoublerep: true, classsepar: ".");
                        values.AddPropertiesFieldsOfClass(rightscan, "right.", ignoretypes, 5, sorteval[1], ensuredoublerep: true, classsepar: ".");
                        values["left.Child.Count"] = ((lefthe?.ScanNode?.Children?.Count ?? 0)).ToStringInvariant();      // count of children
                        values["right.Child.Count"] = ((righthe?.ScanNode?.Children?.Count ?? 0)).ToStringInvariant();      // count of children

                        evl.ReturnSymbolValue = values;      // point evaluator at this set of values
                        object res = evl.Evaluate(condition);  // eval

                        if (res is long)       // long, we have a result, convert and store
                        {
                            int ires = (int)(long)res;
                            return ascending ? ires : -ires;
                        }
                        else
                        {
                            var err = res as BaseUtils.StringParser.ConvertError;       // if we have an error, record it
                            if (err != null)
                                InError = err.ErrorValue;
                        }
                    }
                    else
                    {
                        return ascending ? -1 : 1;
                    }
                }
                else if (righthe != null) // right has scan, so its greater
                {
                    return ascending ? 1 : -1;
                }

                return 0;
            }
        }

        private void SortGridBySortCriteria()
        {
            var rc = new RowComparer(scanSortControl.Condition, scanSortControl.Ascending);
            dataGridView.Sort(rc);
            if ( rc.InError != null )
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(),rc.InError, "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ChangeColumnVisibility(int c)
        {
            if (c >= ColumnParent.Index)    // parent onwards is optional
            {
                DataGridViewColumn col = dataGridView.Columns[c];
                if (col.Visible == true)    // if gone visible, then we need to clear the grid and make the user refind
                {
                    labelCount.Visible = false;
                    dataGridView.Rows.Clear();
                }
            }
        }

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == ColumnDate)
                e.SortDataGridViewColumnDate();
            else if (e.Column == ColumnBody || e.Column == ColumnSystem)
                e.SortDataGridViewColumnAlphaInt();
            else if (e.Column == ColumnCurrentDistance)
                e.SortDataGridViewColumnNumeric();
        }

        #endregion

    }
}
