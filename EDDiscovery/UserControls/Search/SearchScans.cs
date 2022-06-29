/*
 * Copyright © 2016 - 2021 EDDiscovery development team
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
        private string dbSplitterSave = "Splitter";

        private string lastresultlog = null;
        private bool wantreport = false;

        #region Init

        public SearchScans()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "UCSearchScans";

            dataGridView.CheckEDSM = false; // for this, only our data is shown
            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.Height = Font.ScalePixels(26);
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            var enumlist = new Enum[] { EDTx.SearchScans_ColumnDate, EDTx.SearchScans_ColumnStar, EDTx.SearchScans_ColumnInformation, EDTx.SearchScans_ColumnCurrentDistance, 
                EDTx.SearchScans_ColumnPosition,  EDTx.SearchScans_ColumnParent };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);

            var enumlisttt = new Enum[] { EDTx.SearchScans_comboBoxSearches_ToolTip, EDTx.SearchScans_buttonFind_ToolTip, EDTx.SearchScans_buttonSave_ToolTip, EDTx.SearchScans_buttonDelete_ToolTip, 
                                EDTx.SearchScans_buttonExtExcel_ToolTip, EDTx.SearchScans_extButtonResultsLog_ToolTip ,EDTx.SearchScans_extButtonExport_ToolTip, EDTx.SearchScans_extButtonImport_ToolTip };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            List<BaseUtils.TypeHelpers.PropertyNameInfo> classnames = BaseUtils.TypeHelpers.GetPropertyFieldNames(typeof(JournalScan), bf: System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly, excludearrayslist:true);
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("EventTimeUTC", "Date Time in UTC", BaseUtils.ConditionEntry.MatchType.DateAfter));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("EventTimeLocal", "Date Time in Local time", BaseUtils.ConditionEntry.MatchType.DateAfter));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("SyncedEDSM", "Synced to EDSM, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..

            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("Level", "Level of body in system, 0 =star, 1 = Planet, 2 = moon, 3 = submoon", BaseUtils.ConditionEntry.MatchType.NumericEquals));     // add on ones we synthesise
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("Sibling.Count", "Number of siblings", BaseUtils.ConditionEntry.MatchType.NumericEquals));     // add on ones we synthesise
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("Child.Count", "Number of child moons", BaseUtils.ConditionEntry.MatchType.NumericEquals));     // add on ones we synthesise
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("JumponiumCount", "Number of jumponium materials available", BaseUtils.ConditionEntry.MatchType.NumericGreaterEqual));     // add on ones we synthesise

            // from FSSBodySignals or SAASignalsFound
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsGeoSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsBioSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsThargoidSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsGuardianSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsHumanSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsOtherSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsUncategorisedSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..

            var defaultvars = new BaseUtils.Variables();
            defaultvars.AddPropertiesFieldsOfClass(new BodyPhysicalConstants(), "", null, 10);
            foreach(var v in defaultvars.NameEnumuerable)
                classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo(v, "Constant", BaseUtils.ConditionEntry.MatchType.NumericEquals));     // add on a few from the base class..

            classnames.Sort(delegate (BaseUtils.TypeHelpers.PropertyNameInfo left, BaseUtils.TypeHelpers.PropertyNameInfo right) { return left.Name.CompareTo(right.Name); });

            conditionFilterUC.VariableNames = classnames;

           // foreach (var pni in classnames) System.Diagnostics.Debug.WriteLine($"{pni.Name} | {pni.Help.Replace(Environment.NewLine,", ")}"); // debug output for wiki

            string query = GetSetting(dbQuerySave, "");
            conditionFilterUC.InitConditionList(new BaseUtils.ConditionLists(query));   // will ignore if query is bad and return empty query

            dataGridView.Init(discoveryform);
            dataGridView.Columns[4].Tag = dataGridView.Columns[5].Tag = "TextPopOut";  // these two double click are text popouts

            UpdateComboBoxSearches();
            comboBoxSearches.Text = "Select".T(EDTx.SearchScans_Select);
            comboBoxSearches.SelectedIndexChanged += ComboBoxSearches_SelectedIndexChanged;

            labelCount.Visible = false;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg = thc;
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView);
            splitContainer.SplitterDistance(GetSetting(dbSplitterSave, 0.2));
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView);
            conditionFilterUC.Check();      // checks, ignore string return errors, fills in Result
            PutSetting(dbQuerySave, conditionFilterUC.Result.ToString());
            PutSetting(dbSplitterSave, splitContainer.GetSplitterDistance());
        }

        #endregion

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index == 3)
                e.SortDataGridViewColumnNumeric();
        }

        private void ComboBoxSearches_SelectedIndexChanged(object sender, EventArgs e)
        {
            conditionFilterUC.Clear();
            conditionFilterUC.LoadConditions(new BaseUtils.ConditionLists(HistoryListQueries.Instance.Searches[comboBoxSearches.SelectedIndex].Condition));
        }

        private void UpdateComboBoxSearches()
        {
            comboBoxSearches.Items.Clear();
            comboBoxSearches.Items.AddRange(HistoryListQueries.Instance.Searches.Select(x => x.Name));
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            BaseUtils.ConditionLists cond = Valid();
            if (cond != null)
            {
                string name = ExtendedControls.PromptSingleLine.ShowDialog(this.FindForm(), "Name:".T(EDTx.SearchScans_Name), "", "Enter Search Name:".T(EDTx.SearchScans_SN), this.FindForm().Icon);
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

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string name = comboBoxSearches.Text;
            if (comboBoxSearches.SelectedIndex>= 0 && HistoryListQueries.Instance.Searches[comboBoxSearches.SelectedIndex].User && name.HasChars())
            {
                if (ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Confirm deletion of".T(EDTx.SearchScans_DEL) + " " + name, "Delete".T(EDTx.Delete), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
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
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Cannot delete this entry".T(EDTx.SearchScans_DELNO), "Delete".T(EDTx.Delete), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        private async void buttonFind_Click(object sender, EventArgs e)
        {
            BaseUtils.ConditionLists cond = Valid();
            if (cond != null)
            {
                this.Cursor = Cursors.WaitCursor;
                dataGridView.Rows.Clear();

                DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
                SortOrder sortorder = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Descending;

                discoveryform.history.FillInScanNode();     // ensure all journal scan entries point to a scan node (expensive, done only when reqired in this panel)

                var helist = HistoryList.FilterByEventEntryOrder(discoveryform.history.EntryOrder(), HistoryListQueries.SearchableJournalTypes);

                // what variables are in use, so we don't enumerate the lot.
                var allvars = BaseUtils.Condition.EvalVariablesUsed(cond.List);

                // see if we need any default vars, at the moment, they all start with one
                var defaultvars = new BaseUtils.Variables();

                if (allvars.StartsWith("one") >= 0)
                    defaultvars.AddPropertiesFieldsOfClass(new BodyPhysicalConstants(), "", null, 10);
                else
                    defaultvars = null;
    
                //System.Diagnostics.Debug.WriteLine(defaultvars.ToString(separ:Environment.NewLine));

                Dictionary<string, HistoryListQueries.Results> results = new Dictionary<string, HistoryListQueries.Results>();

                var sw = new System.Diagnostics.Stopwatch(); sw.Start();

                lastresultlog = await HistoryListQueries.Find(helist, results, "", cond, defaultvars, wantreport);

                if (IsClosed)       // may be closing during async process
                    return;

                System.Diagnostics.Debug.WriteLine($"Find complete {sw.ElapsedMilliseconds} on {helist.Count}");

                ISystem cursystem = discoveryform.history.CurrentSystem();        // could be null

                foreach ( var kvp in results.EmptyIfNull())
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

                    object[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC).ToString(),
                                            name,
                                            sys.X.ToString("0.##") + sep + sys.Y.ToString("0.##") + sep + sys.Z.ToString("0.##"),
                                            (cursystem != null ? cursystem.Distance(sys).ToString("0.#") : ""),
                                            info,
                                            pinfo,
                                            };

                    dataGridView.Rows.Add(rowobj);
                    dataGridView.Rows[dataGridView.Rows.Count - 1].Tag = he.System;
                }

                System.Diagnostics.Debug.WriteLine($"Search took {sw.ElapsedMilliseconds} Returned {results.Count}");
                dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

                this.Cursor = Cursors.Default;

                labelCount.Text = "Total".TxID(EDTx.UserControlMaterialCommodities_Total) + " " + dataGridView.Rows.Count.ToString();
                labelCount.Visible = true;
            }

        }

        private void extButtonResultsLog_Click(object sender, EventArgs e)
        {
            wantreport = true;
            if (lastresultlog.HasChars())
            {
                ExtendedControls.InfoForm ifrm = new ExtendedControls.InfoForm();
                ifrm.Info("Log", discoveryform.Icon, lastresultlog);
                ifrm.Show(this);
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(),"OK".TxID(EDTx.OK),"");
            }

        }

        private BaseUtils.ConditionLists Valid()
        {
            string errs = conditionFilterUC.Check();
            if (errs.HasChars())
            {
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Condition is not valid".T(EDTx.SearchScans_CNV), "Condition".T(EDTx.SearchScans_CD), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            else
                return conditionFilterUC.Result;
        }

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            dataGridView.Excel(dataGridView.ColumnCount);
        }

        private void extButtonExport_Click(object sender, EventArgs e)
        {
            var ja = HistoryListQueries.Instance.QueriesInJSON(HistoryListQueries.QueryType.User);
            if (ja.Count > 0)
            {
                SaveFileDialog dlg = new SaveFileDialog();

                dlg.Filter = "Query| *.edduserq";
                dlg.Title = "Export user searches".T(EDTx.SearchScans_Export);

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    string path = dlg.FileName;
                    if (!BaseUtils.FileHelpers.TryWriteToFile(path, ja.ToString(true)))
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
            dlg.Title = "Import user searches".T(EDTx.SearchScans_Import);

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                string path = dlg.FileName;
                string text = BaseUtils.FileHelpers.TryReadAllTextFromFile(path);

                if ( text != null)
                {
                    JArray ja = JArray.Parse(text, JToken.ParseOptions.CheckEOL);
                    if ( ja != null )
                    {
                        if (HistoryListQueries.Instance.ReadJSONQueries(ja))
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
    }
}
