/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlTrilateration : UserControlCommonBase
    {
        private ISystem targetsystem;
        private List<WantedSystemClass> wanted;
        private List<string> pushed;
        private List<string> sector;
        private string DbSectorSave { get { return "TrilaterationSectorSystems" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        private Thread EDSMSubmissionThread;
        private bool skipReadOnlyCells = false;

        #region Standard UC Interfaces

        public UserControlTrilateration()
        {
            InitializeComponent();
            var corner = dataGridViewDistances.TopLeftHeaderCell; // work around #1487
            var corner2 = dataGridViewClosestSystems.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            ColumnSystem.AutoCompleteGenerator = SystemCache.ReturnSystemAutoCompleteList;
            FreezeTrilaterationUI();
            toolStripButtonSector.Checked = UserDatabase.Instance.GetSettingBool(DbSectorSave, false);
            toolStripTextBoxSystem.Text = "Press Start New".T(EDTx.UserControlTrilateration_ToolStripText);

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(wantedContextMenu, this);
            BaseUtils.Translator.Instance.Translate(trilatContextMenu, this);
            BaseUtils.Translator.Instance.Translate(toolStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void LoadLayout()
        {
            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList += Discoveryform_OnNewStarsForTrilat;
        }

        public override void Closing()
        {
            UserDatabase.Instance.PutSettingBool(DbSectorSave, toolStripButtonSector.Checked);

            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList -= Discoveryform_OnNewStarsForTrilat;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList -= Discoveryform_OnNewStarsForTrilat;
            uctg = thc;
            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList += Discoveryform_OnNewStarsForTrilat;
        }


        #endregion

        #region Interaction with outside

        private void Discoveryform_OnNewStarsForTrilat(List<string> list, OnNewStarsPushType selection)      // when someone wants to send us sys, they do it via this IF
        {
            foreach (string s in list)
            {
                if (selection == OnNewStarsPushType.TriWanted)
                    AddWantedSystem(s);
                else if (selection == OnNewStarsPushType.TriSystems)
                    AddSystemToDataGridViewDistances(s, false);
            }
        }

        #endregion

        #region General UI

        private void FreezeTrilaterationUI()
        {
            dataGridViewDistances.Enabled = false;
            dataGridViewClosestSystems.Enabled = false;
        }

        private void UnfreezeTrilaterationUI()
        {
            dataGridViewDistances.Enabled = true;
            dataGridViewClosestSystems.Enabled = true;
        }

        private void buttonStartNew_Click(object sender, EventArgs e)
        {
            HistoryEntry he = discoveryform.history.GetLastFSDOnly;
            if (he != null)
                Set(he.System);
        }

        private void toolStripButtonSector_Click(object sender, EventArgs e)
        {
            HistoryEntry he = discoveryform.history.GetLastFSDOnly;
            if (he != null)
                Set(he.System);
        }

        public void LogText(string text)
        {
            LogTextColor(text, discoveryform.theme.TextBlockColor);
        }
        public void LogTextHighlight(string text)
        {
            LogTextColor(text, discoveryform.theme.TextBlockHighlightColor);
        }
        public void LogTextSuccess(string text)
        {
            LogTextColor(text, discoveryform.theme.TextBlockSuccessColor);
        }
        private void LogTextColor(string text, Color color)
        {
            richTextBox_History.AppendText(text, color);
        }

        #endregion

        #region UI grid

        private void dataGridViewClosestSystems_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewClosestSystems.Rows.Count)
            {
                var system = (ISystem)dataGridViewClosestSystems[1, e.RowIndex].Tag;
                AddSystemToDataGridViewDistances(system);
            }
        }

        private void dataGridViewDistances_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridViewDistances.Rows.Count)
                return;

            try
            {
                if (e.ColumnIndex == 0)
                {
                    var value = e.FormattedValue.ToString();
                    var cell = dataGridViewDistances[e.ColumnIndex, e.RowIndex];

                    if (value == "" && (cell.Value == null || cell.Value.ToString() == ""))
                    {
                        return;
                    }

                    var system = SystemCache.FindSystem(value);
                    var enteredSystems = GetEnteredSystems();
                    if (cell.Value != null)
                    {
                        enteredSystems.RemoveAll(s => s.Name == cell.Value.ToString());
                    }

                    if (system == null || (enteredSystems.Contains(system)))
                    {
                        return;
                    }
                }

                if (e.ColumnIndex == 1)
                {
                    var value = e.FormattedValue.ToString().Trim();
                    if (value == "")
                    {
                        dataGridViewDistances.Rows[e.RowIndex].ErrorText = null;
                        return;
                    }

                    var parsed = BaseUtils.DistanceParser.ParseInterstellarDistance(value);
                    if (parsed.HasValue)
                    {
                        dataGridViewDistances.Rows[e.RowIndex].ErrorText = null;
                    }
                    else
                    {
                        e.Cancel = true;
                        dataGridViewDistances.Rows[e.RowIndex].ErrorText = "Invalid number";
                    }
                }
            }
            catch (Exception ex)
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    LogTextHighlight("Exception:" + ex.Message);
                    LogText(ex.StackTrace);
                }));

            }
        }

        private void dataGridViewDistances_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridViewDistances.Rows.Count)
                return;

            if (e.ColumnIndex == 0)
            {
                var cell = dataGridViewDistances[e.ColumnIndex, e.RowIndex];
                var value = cell.Value as string;
                if (value == null)
                {
                    return;
                }

                var enteredSystems = GetEnteredSystems();

                if (enteredSystems.Count > e.RowIndex)  //don't do the below if we're entering something that's not in enteredSystems yet (we need to set cell.tag lower down the first time through here)
                {
                    if (value.Equals(enteredSystems[e.RowIndex].Name)) // If we change a row to same value as before dont do anything from doubleclick or pastinf same new for example
                    {
                        return;
                    }
                }
                if (enteredSystems.Where(es => es.Name == value).Count() > 0)
                {
                    LogTextHighlight("Duplicate system entry is not allowed".T(EDTx.UserControlTrilateration_DUP) + Environment.NewLine);
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        if (!dataGridViewDistances.Rows[e.RowIndex].IsNewRow)
                            dataGridViewDistances.Rows.Remove(dataGridViewDistances.Rows[e.RowIndex]);
                    }));
                    return;
                }

                var system = getSystemForTrilateration(value, false);
                if (system == null)
                {
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        if (!dataGridViewDistances.Rows[e.RowIndex].IsNewRow)
                            dataGridViewDistances.Rows.Remove(dataGridViewDistances.Rows[e.RowIndex]);
                    }));
                    return;
                }
                newSystemAdded(cell, system);
            }
            else if (e.ColumnIndex == 1)
            {
                var value = dataGridViewDistances[1, e.RowIndex].Value?.ToString().Trim();
                if (!string.IsNullOrEmpty(value))
                {
                    var parsedDistance = BaseUtils.DistanceParser.ParseInterstellarDistance(value);
                    if (parsedDistance.HasValue)
                    {
                        dataGridViewDistances[1, e.RowIndex].Value = parsedDistance.Value.ToString("F2");
                    }
                }
            }
            /* skip to the next editable cell */
            skipReadOnlyCells = true;
        }

        private void dataGridViewDistances_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string text = null;
            try
            {
                if (e.RowIndex == -1)
                    return;

                if (e.ColumnIndex == 0 && e.RowIndex < dataGridViewDistances.Rows.Count)
                {
                    Object ob = dataGridViewDistances[e.ColumnIndex, e.RowIndex].Value;
                    if (ob != null)
                        text = ob.ToString();

                    System.Diagnostics.Trace.WriteLine("Click:" + e.RowIndex.ToString() + ":" + e.ColumnIndex.ToString());

                    SetClipboardText(text);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception dataGridViewDistances_CellClick: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }

        }

        private void dataGridViewDistances_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            string text = null;
            try
            {
                if (e.ColumnIndex == 0 && e.RowIndex < dataGridViewDistances.Rows.Count)
                {
                    Object ob = dataGridViewDistances[e.ColumnIndex, e.RowIndex].Value;
                    if (ob != null)
                        text = ob.ToString();

                    System.Diagnostics.Trace.WriteLine("Click:" + e.RowIndex.ToString() + ":" + e.ColumnIndex.ToString());

                    SetClipboardText(text);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception dataGridViewDistances_CellLeave: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }


        private void dataGridViewDistances_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                DataGridView self = sender as DataGridView;
                /* On tab, skip over read-only cells */
                if (self.Focused && e.KeyCode == Keys.Tab)
                {
                    skipReadOnlyCells = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception dataGridViewDistances_KeyDown: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }

        /* This event is received when a new cell has been selected */
        private void dataGridViewDistances_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = sender as DataGridView;
                if (skipReadOnlyCells && dgv.CurrentCell.ReadOnly)
                {
                    /* Have to run this delayed as otherwise we enter an even loop when changing cells */
                    this.BeginInvoke(new MethodInvoker(() => {
                        SelectNextEditableCell(sender as DataGridView);
                    }));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception dataGridViewDistances_CurrentCellChanged: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }



        #endregion


        #region UI toolstrip at top

        private void toolStripButtonMap_Click(object sender, EventArgs e)
        {
            discoveryform.Open3DMapOnSystem(targetsystem);
        }

        private bool Query(string msg)
        {
            return MessageBoxTheme.Show(FindForm(), msg, "Trilateration Panel".T(EDTx.UserControlTrilateration_TP), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        private void toolStripButtonSubmitDistances_Click(object sender, EventArgs e)
        {
            try
            {
                HistoryEntry he = discoveryform.history.GetLastFSDOnly;

                if (he != null && !he.System.Name.Equals(targetsystem.Name, StringComparison.OrdinalIgnoreCase))
                {
                    string question1 = string.Format("You are about to submit distances from {0}.\r\nThe most recent known location in your history is {1}.\r\nSubmit distances without changing 'From' system?".T(EDTx.UserControlTrilateration_CHK), targetsystem.Name , he.System.Name);

                    if (!Query(question1))
                    {
                        string question2 = string.Format("Update 'From' system to current position ({0}) and submit entered distances?".T(EDTx.UserControlTrilateration_UPD), he.System.Name);
                        if (Query(question2))
                        {
                            targetsystem = he.System;
                            SetTargetSystemUI();
                        }
                        else
                            return;
                    }
                }
                LogText("Submitting system to EDSM, please wait...".T(EDTx.UserControlTrilateration_Sub) + Environment.NewLine);
                FreezeTrilaterationUI();

                EDSMSubmissionThread = new Thread(SubmitToEDSM) { Name = "EDSM Submission" };
                EDSMSubmissionThread.Start();
            }
            catch (Exception ex)
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    LogTextHighlight("SubmitDistances Exception:" + ex.Message);
                    LogText(ex.StackTrace);
                }));

            }
        }

        private void toolStripButtonRemoveAll_Click(object sender, EventArgs e)
        {
            dataGridViewDistances.Rows.Clear();
        }

        private void toolStripButtonRemoveUnused_Click(object sender, EventArgs e)
        {
            for (int i = dataGridViewDistances.Rows.Count - 1; i >= 0; i--)
            {
                if (!dataGridViewDistances.Rows[i].IsNewRow)
                {
                    var cell = dataGridViewDistances[1, i];
                    if (cell.Value == null)
                    {
                        dataGridViewDistances.Rows.RemoveAt(i);
                    }
                }
            }
        }

        private void toolStripAddFromHistory_Click(object sender, EventArgs e)
        {
            AddUnknownFromHistory(false);
        }

        private void toolStripAddRecentHistory_Click(object sender, EventArgs e)
        {
            AddUnknownFromHistory(true);
        }

        private void AddUnknownFromHistory(bool descending)
        {
            if (wanted == null) PopulateLocalWantedSystems();
            int i = 0;
            var unknown = discoveryform.history
                            .FilterByFSDOnly.ConvertAll<JournalFSDJump>(he => (he.journalEntry as JournalFSDJump))
                            .Where(fsd => !fsd.HasCoordinate);
            if(descending) unknown = unknown.OrderByDescending(fsd => fsd.EventTimeUTC);
            else unknown = unknown.OrderBy(fsd => fsd.EventTimeUTC);
            foreach (JournalFSDJump jmp in unknown)
            {
                if (wanted.Where(w => w.system == jmp.StarSystem).Count() == 0)
                {
                    AddWantedSystem(jmp.StarSystem);
                    i++;
                }
                if (i >= 20) break;
            }
        }

        #endregion

        #region Right click UI

        private void addToWantedSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewDistances.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);
            string sysName = "";
            foreach (DataGridViewRow r in selectedRows)
            {
                if (r.Cells[0].Value != null)
                {
                    sysName = r.Cells[0].Value.ToString();
                    AddWantedSystem(sysName);
                }
            }
        }

        private void removeFromWantedSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewClosestSystems.SelectedCells.Cast<DataGridViewCell>()
                                                                       .Select(cell => cell.OwningRow)
                                                                       .Distinct()
                                                                       .OrderBy(cell => cell.Index);
            string sysName = "";
            foreach (DataGridViewRow r in selectedRows)
            {
                sysName = r.Cells[1].Value.ToString();
                if (r.Cells[0].Value.ToString() == "Local")
                {
                    WantedSystemClass entry = wanted.Where(x => x.system == sysName).FirstOrDefault();
                    if (entry != null)
                    {
                        entry.Delete();
                        dataGridViewClosestSystems.Rows.Remove(r);
                        wanted.Remove(entry);
                    }
                }
                else
                {
                    LogText(String.Format("{0} is pushed from EDSM and cannot be removed".T(EDTx.UserControlTrilateration_NOTREM), sysName) + Environment.NewLine);
                }
            }
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewDistances.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            this.Cursor = Cursors.WaitCursor;
            var cellVal = selectedRows.First<DataGridViewRow>().Cells[0].Value;
            if (cellVal != null)
            {
                string sysName = cellVal.ToString();
                EDSMClass edsm = new EDSMClass();
                if (!edsm.ShowSystemInEDSM(sysName)) LogTextHighlight("System could not be found - has not been synched or EDSM is unavailable".T(EDTx.UserControlTrilateration_NoEDSM) + Environment.NewLine);
            }
            this.Cursor = Cursors.Default;
        }

        private void viewOnEDSMToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewClosestSystems.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            this.Cursor = Cursors.WaitCursor;
            string sysName = selectedRows.First<DataGridViewRow>().Cells[1].Value.ToString();
            EDSMClass edsm = new EDSMClass();
            if (!edsm.ShowSystemInEDSM(sysName)) LogTextHighlight("System could not be found - has not been synched or EDSM is unavailable".T(EDTx.UserControlTrilateration_NoEDSM) + Environment.NewLine);

            this.Cursor = Cursors.Default;
        }

        private void deleteAllWithKnownPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> edsmCheckNames = new List<string>();
            List<string> removeNames = new List<string>();
            string sysName = "";
            for (int i = dataGridViewClosestSystems.RowCount - 1; i >= 0; i--)
            {
                DataGridViewRow r = dataGridViewClosestSystems.Rows[i];
                sysName = r.Cells[1].Value.ToString();
                if (r.Cells[0].Value.ToString() == "Local")
                {
                    var sys = SystemCache.FindSystem(sysName);
                    if (sys == null)
                        edsmCheckNames.Add(sysName);
                    else
                        if (sys.HasCoordinate) removeNames.Add(sysName);
                }
            }
            if (edsmCheckNames.Count() > 0)
            {
                EDSMClass edsm = new EDSMClass();
                List<string> nowKnown = edsm.CheckForNewCoordinates(edsmCheckNames);
                foreach (string s in nowKnown)
                {
                    removeNames.Add(s);
                }
            }
            for (int i = dataGridViewClosestSystems.RowCount - 1; i >= 0; i--)
            {
                DataGridViewRow r = dataGridViewClosestSystems.Rows[i];
                sysName = r.Cells[1].Value.ToString();
                if (removeNames.Contains(sysName))
                {
                    WantedSystemClass entry = wanted.Where(x => x.system == sysName).FirstOrDefault();
                    if (entry != null)
                    {
                        entry.Delete();
                        dataGridViewClosestSystems.Rows.Remove(r);
                        wanted.Remove(entry);
                    }
                }
            }
        }

        private void addAllLocalSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wanted != null && wanted.Any())
            {
                foreach (WantedSystemClass sys in wanted)
                {
                    AddSystemToDataGridViewDistances(sys.system, false);
                }
            }
        }

        private void addAllEDSMSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pushed != null && pushed.Any())
            {
                foreach (string sys in pushed)
                {
                    AddSystemToDataGridViewDistances(sys, true);
                }
            }
        }

        private void addAllSectorSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sector != null && sector.Any())
            {
                sector.ForEach(s => AddSystemToDataGridViewDistances(s, true));
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string txt = Clipboard.GetText(TextDataFormat.UnicodeText);
            string[] lines = txt.Split('\n').Select(s => s.Trim('\r')).ToArray();
            foreach (string line in lines)
            {
                string[] fields = line.Split('\t');
                string sysname = fields[0].Trim();
                string dist = fields.Length >= 2 ? fields[1].Trim() : null;

                if (sysname != "")
                {
                    var row = dataGridViewDistances.Rows.Add(sysname, dist);
                    dataGridViewDistances_CellEndEdit(this, new DataGridViewCellEventArgs(0, row));
                    if (dist != "")
                    {
                        dataGridViewDistances_CellEndEdit(this, new DataGridViewCellEventArgs(1, row));
                    }
                }
            }
        }

        #endregion

        #region Send to EDSM

        private void SubmitToEDSM()
        {
            try
            {
                EDSMClass edsm = new EDSMClass();       // current commander credentials

                if (!edsm.ValidCredentials)
                {
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), "Please enter commander name before submitting the system!".T(EDTx.UserControlTrilateration_Cmdr));
                        UnfreezeTrilaterationUI();
                    }));
                    return;
                }

                var distances = new Dictionary<string, double>();
                for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
                {
                    var systemCell = dataGridViewDistances[0, i];
                    var distanceCell = dataGridViewDistances[1, i];
                    if (systemCell.Value != null && distanceCell.Value != null)
                    {
                        var system = systemCell.Value.ToString();

                        var value = distanceCell.Value.ToString().Trim();
                        var parsedDistance = BaseUtils.DistanceParser.ParseInterstellarDistance(value);

                        if (parsedDistance.HasValue)
                        {
                            // can over-ride drop down now if it's a real system so you could add duplicates if you wanted (even once I've figured out issue #81 which makes it easy if not likely...)
                            if (!distances.Keys.Contains(system))
                            {
                                distances.Add(system, parsedDistance.Value);
                            }
                        }
                    }

                }

                bool trilatOk = false;
                bool respOk = true;
                List<KeyValuePair<string, double>> distlist = distances.ToList();

                for (int i = 0; i < distances.Count; i += 20)
                {
                    var dists = new Dictionary<string, double>();
                    for (int j = i; j < i + 20 && j < distances.Count; j++)
                        dists[distlist[j].Key] = distlist[j].Value;

                    var responseM = edsm.SubmitDistances(targetsystem.Name, dists);

                    System.Diagnostics.Trace.WriteLine(responseM);

                    string infoM;
                    bool trilaterationOkM;
                    var responseOkM = edsm.ShowDistanceResponse(responseM, out infoM, out trilaterationOkM);

                    if (trilaterationOkM)
                        trilatOk = true;

                    if (!responseOkM)
                        respOk = false;

                    System.Diagnostics.Trace.WriteLine(infoM);
                }

                BeginInvoke((MethodInvoker)delegate
                {
                    UnfreezeTrilaterationUI();

                    if (respOk && trilatOk)
                    {
                        LogTextSuccess("EDSM submission succeeded, trilateration successful.".T(EDTx.UserControlTrilateration_Horray) + Environment.NewLine);
                        discoveryform.RefreshHistoryAsync();
                        checkForUnknownSystemsNowKnown();
                    }
                    else if (respOk)
                    {
                        LogTextHighlight("EDSM submission succeeded, but trilateration failed. Try adding more distances.".T(EDTx.UserControlTrilateration_Failed) + Environment.NewLine);
                    }
                    else
                    {
                        LogTextHighlight("EDSM submission failed.".T(EDTx.UserControlTrilateration_Dead) + Environment.NewLine);
                    }
                });
            }
            catch (Exception ex)
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    LogTextHighlight("SubmitToEDSM Exception:" + ex.Message);
                    LogText(ex.StackTrace);
                }));

            }
        }

        #endregion


        #region Helpers

        private void Set(ISystem system)        // called on New to set up trilat
        {
            if (targetsystem == null || system == null || !targetsystem.Equals(system))
            {
                targetsystem = system;
                ClearDataGridViewDistancesRows();
            }

            SetTargetSystemUI();
            toolStripLabelNoCoords.Text = discoveryform.history.FilterByFSDOnly.Where(j => !(j.journalEntry as JournalFSDJump).HasCoordinate).Count().ToString();

            UnfreezeTrilaterationUI();
            dataGridViewDistances.Focus();

            dataGridViewClosestSystems.Rows.Clear();
            PopulateLocalWantedSystems();
            Thread ViewPushedSystemsThread = new Thread(ViewPushedSystems) { Name = "EDSM get pushed systems" };
            ViewPushedSystemsThread.Start();

            sector = null;
            if (toolStripButtonSector.Checked)
            {
                Thread ViewSectorSystemsThread = new Thread(() => ViewSectorSystems(system)) { Name = "EDSM get sector systems" };
                ViewSectorSystemsThread.Start();
            }
        }

        public void ClearDataGridViewDistancesRows()
        {
            try
            {
                // keep systems, clear distances
                for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
                {
                    var systemCell = dataGridViewDistances[0, i];
                    var distanceCell = dataGridViewDistances[1, i];
                    var calculatedDistanceCell = dataGridViewDistances[2, i];
                    var statusCell = dataGridViewDistances[3, i];

                    var system = (ISystem)systemCell.Tag;

                    distanceCell.Value = null;
                    calculatedDistanceCell.Value = null;
                    if (system.HasCoordinate) statusCell.Value = null;
                }
            }
            catch (Exception ex)
            {
                LogTextHighlight("ClearDataGridViewDistancesRows Exception:" + ex.Message);
                LogText(ex.StackTrace);
            }
        }


        private void SetTargetSystemUI()
        {
            if (targetsystem == null)
                return;

            toolStripTextBoxSystem.Text = targetsystem.Name;
        }

        private List<ISystem> GetEnteredSystems()
        {
            var systems = new List<ISystem>();
            for (int i = 0, size = dataGridViewDistances.Rows.Count - 1; i < size; i++)
            {
                var cell = dataGridViewDistances[0, i];
                if (cell.Value != null && cell.Tag != null)
                {
                    systems.Add((ISystem) cell.Tag);
                }
            }
            return systems;
        }


        /* Tries to load the system data for the given name. If no system data is available, but the system is known,
         * it creates a new System entity, otherwise logs it and returns null. */
        private ISystem getSystemForTrilateration(string systemName, bool fromEDSM)
        {
            var system = SystemCache.FindSystem(systemName);

            if (system == null)
            {
                EDSMClass edsm = new EDSMClass();
                if (fromEDSM || edsm.IsKnownSystem(systemName))
                {
                    system = new SystemClass(systemName);
                }
                else
                {
                    LogTextHighlight("Only systems with coordinates or already known to EDSM can be added".T(EDTx.UserControlTrilateration_Co) + Environment.NewLine);
                }
            }
            return system;
        }

        /* Callback for when a new system has been added to the grid.
         * Performs some additional setup such as clearing data and setting the status. */
        private void newSystemAdded(DataGridViewCell cell, ISystem system)
        {
            if (!cell.Value.Equals(system.Name))            // if cell value is not the same as system name
            {
                cell.Value = system.Name;
            }
            cell.Tag = system;
            // reset any calculated distances
            dataGridViewDistances[2, cell.RowIndex].Value = null;
            // (re)set status
            if (system.HasCoordinate)
            {
                dataGridViewDistances[3, cell.RowIndex].Value = "Pos: " + system.X.ToString() + ";" + "Pos: " + system.Y.ToString() + ";" + "Pos: " + system.Z.ToString();
            }
            else
            {
                dataGridViewDistances[3, cell.RowIndex].Value = "Position unknown".T(EDTx.UserControlTrilateration_PU);
                dataGridViewDistances[3, cell.RowIndex].Style.ForeColor = discoveryform.theme.NonVisitedSystemColor;
            }
        }

        private void PopulateLocalWantedSystems()
        {
            wanted = WantedSystemClass.GetAllWantedSystems();

            if (wanted != null && wanted.Any())
            {
                foreach (WantedSystemClass sys in wanted)
                {
                    ISystem star = SystemCache.FindSystem(sys.system);
                    if (star == null)
                        star = new SystemClass(sys.system);

                    var index = dataGridViewClosestSystems.Rows.Add("Local");
                    dataGridViewClosestSystems[1, index].Value = sys.system;
                    dataGridViewClosestSystems[1, index].Tag = star;
                }
            }
            else
            {
                wanted = new List<WantedSystemClass>();
            }
        }

        // Runs as a thread from Set
        private void ViewPushedSystems()
        {
            try
            {
                EDSMClass edsm = new EDSMClass();
                pushed = edsm.GetPushedSystems();

                foreach (String system in pushed)
                {
                    ISystem star = SystemCache.FindSystem(system);
                    if (star == null)
                        star = new SystemClass(system);

                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        var index = dataGridViewClosestSystems.Rows.Add("EDSM");
                        dataGridViewClosestSystems[1, index].Value = system;
                        dataGridViewClosestSystems[1, index].Tag = star;
                    }));
                }
            }
            catch (Exception ex)
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    LogTextHighlight("ViewPushedSystems Exception:" + ex.Message);
                    LogText(ex.StackTrace);
                }));
            }
        }

        // Runs as a thread from Set
        private void ViewSectorSystems(ISystem system)
        {
            try
            {
                EDSMClass edsm = new EDSMClass();
                var splitName = system.Name.Split(' ');
                if (splitName.Length < 3)
                {
                    BeginInvoke(new MethodInvoker(() => LogTextHighlight("Sector name could not be derived from system name (it may not be procedurally generated).  Not getting sector systems.".T(EDTx.UserControlTrilateration_NotProcGen) + Environment.NewLine))); 
                    sector = new List<string>();
                    return;
                }

                var sectorName = string.Join(" ", splitName.Take(splitName.Length - 2));
                sector = edsm.GetUnknownSystemsForSector(sectorName);

                if (!sector.Any())
                {
                    BeginInvoke(new MethodInvoker(() => LogText("No systems with unknown coordinates were found for the current sector.".T(EDTx.UserControlTrilateration_NoSector) + Environment.NewLine)));
                    return;
                }

                BeginInvoke(new MethodInvoker(() => 
                    LogText(string.Format("{0} systems with unknown coordinates found in {1} sector.".T(EDTx.UserControlTrilateration_SectorCount) + Environment.NewLine, sector.Count(), sectorName))));

                foreach (string systemName in sector)
                {
                    ISystem star = SystemCache.FindSystem(systemName);
                    if (star == null)
                        star = new SystemClass(systemName);

                    BeginInvoke(new MethodInvoker(() =>
                    {
                        var index = dataGridViewClosestSystems.Rows.Add("Sector");
                        dataGridViewClosestSystems[1, index].Value = systemName;
                        dataGridViewClosestSystems[1, index].Tag = star;
                    }));
                }
            }
            catch (Exception ex)
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    LogTextHighlight("ViewSectorSystems Exception:" + ex.Message);
                    LogText(ex.StackTrace);
                }));
            }
        }

        /* Adds a system to the grid if it's not already in there */
        public void AddSystemToDataGridViewDistances(ISystem system)
        {
            for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
            {
                var cell = dataGridViewDistances[0, i];
                ISystem s2 = cell.Tag as ISystem;
                if (s2 != null && s2.Name.Equals(system.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
                /* Should we fall-back on comparing cell.Value if call.Tag is null? */
            }

            var index = dataGridViewDistances.Rows.Add(system.Name);
            newSystemAdded(dataGridViewDistances[0, index], system);
        }

        public void AddSystemToDataGridViewDistances(string systemName, bool fromEDSM)
        {
            var system = getSystemForTrilateration(systemName, fromEDSM);
            if (system != null)
            {
                AddSystemToDataGridViewDistances(system);
            }
        }

        private void checkForUnknownSystemsNowKnown()
        {
            for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
            {
                var systemCell = dataGridViewDistances[0, i];
                var oldSystem = (ISystem)systemCell.Tag;
                if (oldSystem != null && !oldSystem.HasCoordinate)
                {
                    var value = systemCell.Value as string;
                    var newSystem = SystemCache.FindSystem(value);
                    if (newSystem != null && newSystem.HasCoordinate)
                    {
                        systemCell.Tag = newSystem;
                        dataGridViewDistances[3, i].Style.ForeColor = discoveryform.theme.VisitedSystemColor;
                        dataGridViewDistances[3, i].Value = "Position found".T(EDTx.UserControlTrilateration_PF);
                    }
                }
            }
        }

        private void SelectNextEditableCell(DataGridView dataGridView)
        {
            DataGridViewCell cell = dataGridView.CurrentCell;
            if(cell == null) return;
            if (!skipReadOnlyCells) return;

            int row = cell.RowIndex;
            int col = cell.ColumnIndex;

            if(row == dataGridView.RowCount && col == dataGridView.RowCount) return;

            do {
                col++;
                if(col >= dataGridView.ColumnCount)
                {
                    col = 0;
                    row++;
                    if (row >= dataGridView.RowCount)
                    {
                        row = dataGridView.RowCount - 1;
                        dataGridView.CurrentCell = dataGridView.Rows[row].Cells[col];
                        return;
                    }
                }
                cell = dataGridView.Rows[row].Cells[col];
            } while( cell.ReadOnly );
            dataGridView.CurrentCell = cell;
            dataGridView.CurrentCell.Selected = true;
            skipReadOnlyCells = false;

            // Copy text to clipboard
            DataGridViewTextBoxCell ob = (DataGridViewTextBoxCell)cell;
            string text=null;
            if (ob != null)
                text = (string)ob.Value;
            SetClipboardText(text);
        }

        public void AddWantedSystem(string sysName)
        {
            if (wanted == null)
                PopulateLocalWantedSystems();
            else
            {
                // there can be multiple instances tied to the history form so this might already exist...
                List<WantedSystemClass> dbCheck = WantedSystemClass.GetAllWantedSystems();
                if (dbCheck != null && dbCheck.Where(s => s.system == sysName).Any()) // if we have wanted systems in the DB... and its there..
                    return;
            }

            WantedSystemClass entry = wanted.Where(x => x.system == sysName).FirstOrDefault();  //duplicate?

            if (entry == null)
            {
                WantedSystemClass toAdd = new WantedSystemClass(sysName);       // make one..
                toAdd.Add();                                                    // add to db.

                wanted.Add(toAdd);

                ISystem star = SystemCache.FindSystem(sysName);
                if (star == null)
                    star = new SystemClass(sysName);

                var index = dataGridViewClosestSystems.Rows.Add("Local");
                dataGridViewClosestSystems[1, index].Value = sysName;
                dataGridViewClosestSystems[1, index].Tag = star;
            }
        }

        #endregion

        #region Grid Sorter

        private void dataGridViewDistances_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 1)
            {
                double? v1 = BaseUtils.DistanceParser.ParseInterstellarDistance(e.CellValue1?.ToString());
                double? v2 = BaseUtils.DistanceParser.ParseInterstellarDistance(e.CellValue2?.ToString());

                if (v1 != null || v2 != null)
                {
                    if (v1 == null)
                    {
                        e.SortResult = 1;
                    }
                    else if (v2 == null)
                    {
                        e.SortResult = -1;
                    }
                    else
                    {
                        e.SortResult = v1.Value.CompareTo(v2.Value);
                    }

                    e.Handled = true;
                }
            }
        }

        #endregion
    }
}
