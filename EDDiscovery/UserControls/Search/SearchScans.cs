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
        private string dbWordWrap = "WordWrap";

        private string lastresultlog = null;

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

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, true);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            var enumlist = new Enum[] { EDTx.SearchScans_ColumnDate, EDTx.SearchScans_ColumnBody, EDTx.SearchScans_ColumnInformation, EDTx.SearchScans_ColumnCurrentDistance, 
                EDTx.SearchScans_ColumnPosition,  EDTx.SearchScans_ColumnParent };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);

            var enumlisttt = new Enum[] { EDTx.SearchScans_comboBoxSearches_ToolTip, EDTx.SearchScans_buttonFind_ToolTip, EDTx.SearchScans_buttonSave_ToolTip, EDTx.SearchScans_buttonDelete_ToolTip, 
                                EDTx.SearchScans_extButtonExport_ToolTip, EDTx.SearchScans_extButtonImport_ToolTip, EDTx.SearchScans_extCheckBoxWordWrap_ToolTip, 
                                EDTx.SearchScans_buttonExtExcel_ToolTip, EDTx.SearchScans_extCheckBoxDebug_ToolTip};
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            List<BaseUtils.TypeHelpers.PropertyNameInfo> classnames = HistoryListQueries.PropertyList();

            conditionFilterUC.AutoCompleteOnMatch = true;
            conditionFilterUC.VariableNames = classnames;

            foreach (var pni in classnames) System.Diagnostics.Debug.WriteLine($"{pni.Name} | {pni.Help.Replace(Environment.NewLine,", ")} | {pni.Comment}"); // debug output for wiki

            string query = GetSetting(dbQuerySave, "");
            conditionFilterUC.InitConditionList(new BaseUtils.ConditionLists(query));   // will ignore if query is bad and return empty query

            dataGridView.Init(discoveryform);
            dataGridView.Columns[4].Tag = "TooltipPopOut;TextPopOut";
            dataGridView.Columns[5].Tag = "TextPopOut";  // these two double click are text popouts

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
                labelCount.Text = "...";

                DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
                SortOrder sortorder = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Descending;

                discoveryform.history.FillInScanNode();     // ensure all journal scan entries point to a scan node (expensive, done only when reqired in this panel)

                // what variables are in use, so we don't enumerate the lot.
                var allvars = BaseUtils.Condition.EvalVariablesUsed(cond.List);

                // see if we need any default vars, at the moment, they all start with one
                var defaultvars = new BaseUtils.Variables();

                // we want to keep the doubleness of values as this means when divided by the eval engine we get a float/float divide
                if (allvars.StartsWithInList("one") >= 0)
                    defaultvars.AddPropertiesFieldsOfClass(new BodyPhysicalConstants(), "", null, 10,ensuredoublerep:true);
                else
                    defaultvars = null;
    
                //System.Diagnostics.Debug.WriteLine(defaultvars.ToString(separ:Environment.NewLine));

                Dictionary<string, HistoryListQueries.Results> results = new Dictionary<string, HistoryListQueries.Results>();

                var computedsearch = HistoryListQueries.NeededSearchableTypes(allvars);
                var helist = HistoryList.FilterByEventEntryOrder(discoveryform.history.EntryOrder(), computedsearch);
                System.Diagnostics.Debug.WriteLine($"Helist is {helist.Count} entryorder {discoveryform.history.EntryOrder().Count}");

                var sw = new System.Diagnostics.Stopwatch(); sw.Start();

                lastresultlog = await HistoryListQueries.Find(helist, results, "", cond, defaultvars, discoveryform.history.StarScan, extCheckBoxDebug.Checked);

                if (IsClosed)       // may be closing during async process
                    return;

                System.Diagnostics.Debug.WriteLine($"Find complete {sw.ElapsedMilliseconds} on {helist.Count} results {results.Count}");

                ISystem cursystem = discoveryform.history.CurrentSystem();        // could be null

                int max = 100000;

                foreach ( var kvp in results.Take(max).EmptyIfNull())
                {
                    string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";

                    HistoryListQueries.GenerateReportFields(kvp.Key, kvp.Value.EntryList, out string name, out string info, out string infotooltip, out string pinfo);

                    HistoryEntry he = kvp.Value.EntryList.Last();
                    ISystem sys = he.System;

                    object[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC).ToString(),
                                            name,
                                            he.System.X.ToString("0.##") + sep + sys.Y.ToString("0.##") + sep + sys.Z.ToString("0.##"), //2
                                            (cursystem != null ? cursystem.Distance(sys).ToString("0.#") : ""),
                                            info,   //4
                                            pinfo,
                                            };

                    int row = dataGridView.Rows.Add(rowobj);
                    dataGridView.Rows[row].Tag = he.System;
                    dataGridView.Rows[row].Cells[4].ToolTipText = infotooltip;
                }

                if ( results.Count > max )
                {
                    object[] rowobj = { "Too many" };
                    dataGridView.Rows.Add(rowobj);
                }

                System.Diagnostics.Debug.WriteLine($"Search took {sw.ElapsedMilliseconds} Returned {results.Count}");
                dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

                this.Cursor = Cursors.Default;

                labelCount.Text = "Total".TxID(EDTx.UserControlMaterialCommodities_Total) + " " + results.Count + " / " + helist.Count.ToString();
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
                        if (lastresultlog.Length < 2000000)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            ExtendedControls.InfoForm ifrm = new ExtendedControls.InfoForm();
                            ifrm.Info("Log", discoveryform.Icon, lastresultlog);
                            ifrm.Show(this);
                            this.Cursor = Cursors.Default;
                        }
                        else
                        {
                            SaveFileDialog dlg = new SaveFileDialog();

                            dlg.Filter = "Log| *.log";
                            dlg.Title = "Export";

                            if (dlg.ShowDialog(this) == DialogResult.OK)
                            {
                                System.IO.File.WriteAllText(dlg.FileName, lastresultlog);
                            }
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
                JObject hdr = new JObject() { ["Searches-Version"] = "1.0" , ["Searches"] = ja};        // note the file format..

                SaveFileDialog dlg = new SaveFileDialog();

                dlg.Filter = "Query| *.edduserq";
                dlg.Title = "Export user searches".T(EDTx.SearchScans_Export);

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
            dlg.Title = "Import user searches".T(EDTx.SearchScans_Import);

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

                        if (ja != null && HistoryListQueries.Instance.ReadJSONQueries(ja))
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
