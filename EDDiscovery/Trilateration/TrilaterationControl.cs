﻿/*
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using EDDiscovery.DB;
using EliteDangerousCore.EDSM;
using ExtendedControls;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery
{
    public partial class TrilaterationControl : UserControl
    {
        private EDDiscoveryForm _discoveryForm;
        private ISystem TargetSystem;
        private Thread EDSMSubmissionThread;
        private EDSMClass edsm;
        private List<WantedSystemClass> wanted;
        private List<string> pushed;

        /** This global should be set if the next CurrentCellChanged() event should skip to the next editable cell.
         * This should be the case whenver a keyboard event causes cells to change, but not on mouse-initiated events */
        private bool skipReadOnlyCells = false;

        public TrilaterationControl()
        {
            InitializeComponent();
            ColumnSystem.AutoCompleteGenerator = SystemClassDB.ReturnOnlySystemsListForAutoComplete;
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            FreezeTrilaterationUI();
            edsm = new EDSMClass();
            edsm.apiKey = EDCommander.Current.APIKey;
            edsm.commanderName = EDCommander.Current.EdsmName;
            SetTriStatus("Press Start New");
        }
        
        public void Set(ISystem system)
        {
            if (TargetSystem == null || system == null || !TargetSystem.Equals(system))
            {
                TargetSystem = system;
                ClearDataGridViewDistancesRows();
            }

            SetTargetSystemUI();

            UnfreezeTrilaterationUI();
            dataGridViewDistances.Focus();

            dataGridViewClosestSystems.Rows.Clear();
            PopulateLocalWantedSystems();
            Thread ViewPushedSystemsThread = new Thread(ViewPushedSystems) { Name = "EDSM get pushed systems" };
            ViewPushedSystemsThread.Start();

        }

        private void SetTargetSystemUI()
        {
            if (TargetSystem == null)
                return;

            textBoxSystemName.Text = TargetSystem.name;

            if (TargetSystem.HasCoordinate)
            {
                textBoxCoordinateX.Text = TargetSystem.x.ToString();
                textBoxCoordinateY.Text = TargetSystem.y.ToString();
                textBoxCoordinateZ.Text = TargetSystem.z.ToString();

                SetTriStatusSuccess("Has Coordinates!");
            }
            else
            {
                SetTriStatusError("Enter Distances");
            }
        }

        private List<SystemClass> GetEnteredSystems()
        {
            var systems = new List<SystemClass>();
            for (int i = 0, size = dataGridViewDistances.Rows.Count - 1; i < size; i++)
            {
                var cell = dataGridViewDistances[0, i];
                if (cell.Value != null && cell.Tag != null)
                {
                    systems.Add((SystemClass) cell.Tag);
                }
            }
            return systems;
        }

        private void dataGridViewDistances_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
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

                    var system = SystemClassDB.GetSystem(value);
                    var enteredSystems = GetEnteredSystems();
                    if (cell.Value != null)
                    {
                        enteredSystems.RemoveAll(s => s.name == cell.Value.ToString());
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

                    var parsed = DistanceParser.ParseInterstellarDistance(value);
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

        /* Tries to load the system data for the given name. If no system data is available, but the system is known,
         * it creates a new System entity, otherwise logs it and returns null. */
        private SystemClassDB getSystemForTrilateration(string systemName)
        {
            var system = SystemClassDB.GetSystem(systemName);

            if (system == null)
            {
                if (!edsm.IsKnownSystem(systemName))
                {
                    LogTextHighlight("Only systems with coordinates or already known to EDSM can be added" + Environment.NewLine);
                }
                else
                {
                    system = new SystemClassDB(systemName);
                }
            }
            return system;
        }

        /* Callback for when a new system has been added to the grid.
         * Performs some additional setup such as clearing data and setting the status. */
        private void newSystemAdded(DataGridViewCell cell, SystemClassDB system)
        {
            if (!cell.Value.Equals(system.name))            // if cell value is not the same as system name
            {
                cell.Value = system.name;
            }
            cell.Tag = system;
            // reset any calculated distances
            dataGridViewDistances[2, cell.RowIndex].Value = null;
            // (re)set status
            if (system.HasCoordinate)
            {
                dataGridViewDistances[3, cell.RowIndex].Value = "Pos: " + system.x.ToString() + ";" + "Pos: " + system.y.ToString() + ";" + "Pos: " + system.z.ToString();
            }
            else
            {
                dataGridViewDistances[3, cell.RowIndex].Value = "Position unknown";
                dataGridViewDistances[3, cell.RowIndex].Style.ForeColor = _discoveryForm.theme.NonVisitedSystemColor;
            }
        }

        private void dataGridViewDistances_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
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
                    if (value.Equals(enteredSystems[e.RowIndex].name)) // If we change a row to same value as before dont do anything from doubleclick or pastinf same new for example
                    {
                        return;
                    }
                }
                if (enteredSystems.Where(es => es.name == value).Count() > 0)
                {
                    LogTextHighlight("Duplicate system entry is not allowed" + Environment.NewLine);
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        if (!dataGridViewDistances.Rows[e.RowIndex].IsNewRow)
                            dataGridViewDistances.Rows.Remove(dataGridViewDistances.Rows[e.RowIndex]);
                    }));
                    return;
                }

                var system = getSystemForTrilateration(value);
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
                if (dataGridViewDistances[1, e.RowIndex].Value != null && !string.IsNullOrEmpty(dataGridViewDistances[1, e.RowIndex].Value.ToString().Trim()))
                {
                    var value = dataGridViewDistances[1, e.RowIndex].Value.ToString().Trim();
                    var parsedDistance = DistanceParser.ParseInterstellarDistance(value);
                    if (parsedDistance.HasValue)
                    {
                        dataGridViewDistances[1, e.RowIndex].Value = parsedDistance.Value.ToString("F2");
                    }
                }
            }
            /* skip to the next editable cell */
            skipReadOnlyCells = true;
        }


        public IEnumerable<SystemClassDB> CurrentReferenceSystems
        {
            get
            {
                return (dataGridViewDistances.Rows.OfType<DataGridViewRow>().Select(row => row.Cells[0].Tag)).OfType<SystemClassDB>();
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

                    var system = (SystemClassDB)systemCell.Tag;

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

        private void ClearCalculatedDataGridViewDistancesRows()
        {
            // keep systems and distances, clear calculated distances and statuses
            for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
            {
                var systemCell = dataGridViewDistances[0, i];
                var calculatedDistanceCell = dataGridViewDistances[2, i];
                var statusCell = dataGridViewDistances[3, i];

                var system = (SystemClassDB)systemCell.Tag;

                calculatedDistanceCell.Value = null;
                if (system != null && system.HasCoordinate) statusCell.Value = null;
            }
        }
        
        private void PopulateLocalWantedSystems()
        {
            wanted = WantedSystemClass.GetAllWantedSystems();

            if (wanted != null && wanted.Any())
            {
                foreach (WantedSystemClass sys in wanted)
                {
                    SystemClass star = SystemClassDB.GetSystem(sys.system);
                    if (star == null)
                        star = new SystemClassDB(sys.system);

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

        // Runs as a thread.
        private void ViewPushedSystems()
        {
            try
            {
                pushed = edsm.GetPushedSystems();

                foreach (String system in pushed)
                {
                    SystemClassDB star = SystemClassDB.GetSystem(system);
                    if (star == null)
                        star = new SystemClassDB(system);

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

        private void dataGridViewClosestSystems_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var system = (SystemClassDB)dataGridViewClosestSystems[1, e.RowIndex].Tag;
                AddSystemToDataGridViewDistances(system);
            }
        }

        /* Adds a system to the grid if it's not already in there */
        public void AddSystemToDataGridViewDistances(SystemClassDB system)
        {
            for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
            {
                var cell = dataGridViewDistances[0, i];
                SystemClassDB s2 = cell.Tag as SystemClassDB;
                if (s2 != null && s2.name.Equals(system.name, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
                /* Should we fall-back on comparing cell.Value if call.Tag is null? */
            }

            var index = dataGridViewDistances.Rows.Add(system.name);
            newSystemAdded(dataGridViewDistances[0, index], system);
        }

        public void AddSystemToDataGridViewDistances(string systemName)
        {
            var system = getSystemForTrilateration(systemName);
            if (system != null)
            {
                AddSystemToDataGridViewDistances(system);
            }
        }

        private void toolStripButtonSubmitDistances_Click(object sender, EventArgs e)
        {
            try
            {
                HistoryEntry he = _discoveryForm.history.GetLastFSD;
                if (he != null && !he.System.name.Equals(TargetSystem.name, StringComparison.OrdinalIgnoreCase))
                {
                    SysWarning warn = new SysWarning();
                    _discoveryForm.theme.ApplyToForm(warn);
                    warn.SetLabel(TargetSystem.name, he.System.name);
                    warn.ShowDialog();
                    switch (warn.Result)
                    {
                        case SysWarningResult.eCancel:
                            return;
                        case SysWarningResult.eUpdateAndSubmit:
                            TargetSystem = he.System;
                            SetTargetSystemUI();
                            break;
                    }
                }
                LogText("Submitting system to EDSM, please wait..." + Environment.NewLine);
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

        private void SubmitToEDSM()
        {
            try
            {
                edsm.apiKey = EDCommander.Current.APIKey;
                edsm.commanderName = EDCommander.Current.EdsmName;
                
                var travelHistoryControl = _discoveryForm.TravelControl;
                if (string.IsNullOrEmpty(edsm.commanderName))
                {
                    string commanderName = EDCommander.Current.EdsmName;

                    if (string.IsNullOrEmpty(commanderName))
                    {
                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            ExtendedControls.MessageBoxTheme.Show("Please enter commander name before submitting the system!");
                            UnfreezeTrilaterationUI();
                        }));
                        return;
                    }
                    edsm.commanderName = commanderName;
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
                        var parsedDistance = DistanceParser.ParseInterstellarDistance(value);

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

                    var responseM = edsm.SubmitDistances(edsm.commanderName, TargetSystem.name, dists);

                    Console.WriteLine(responseM);

                    string infoM;
                    bool trilaterationOkM;
                    var responseOkM = edsm.ShowDistanceResponse(responseM, out infoM, out trilaterationOkM);

                    if (trilaterationOkM)
                        trilatOk = true;

                    if (!responseOkM)
                        respOk = false;

                    Console.WriteLine(infoM);
                }

                BeginInvoke((MethodInvoker)delegate
               {
                   UnfreezeTrilaterationUI();

                   if (respOk && trilatOk)
                   {
                       LogTextSuccess("EDSM submission succeeded, trilateration successful." + Environment.NewLine);
                       _discoveryForm.RefreshHistoryAsync();
                       checkForUnknownSystemsNowKnown();
                   }
                   else if (respOk)
                   {
                       LogTextHighlight("EDSM submission succeeded, but trilateration failed. Try adding more distances." + Environment.NewLine);
                   }
                   else
                   {
                       LogTextHighlight("EDSM submission failed." + Environment.NewLine);
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

        private void FreezeTrilaterationUI()
        {
            
            dataGridViewDistances.Enabled = false;
            dataGridViewClosestSystems.Enabled = false;
        }

        private void UnfreezeTrilaterationUI()
        {
            dataGridViewDistances.Enabled = true;
            dataGridViewClosestSystems.Enabled = true;
            Dock = DockStyle.Fill;
        }
        
        private void SetTriStatus(string st)
        {
            textBox_status.ForeColor = _discoveryForm.theme.TextBlockColor;
            textBox_status.BackColor = _discoveryForm.theme.TextBackColor;
            textBox_status.Text = st;
        }

        private void SetTriStatusSuccess(string st)
        {
            textBox_status.ForeColor = _discoveryForm.theme.TextBackColor;
            textBox_status.BackColor = _discoveryForm.theme.TextBlockSuccessColor;
            textBox_status.Text = st;
        }

        private void SetTriStatusError(string st)
        {
            textBox_status.ForeColor = _discoveryForm.theme.TextBackColor;
            textBox_status.BackColor = _discoveryForm.theme.TextBlockHighlightColor;
            textBox_status.Text = st;
        }

        public void LogText(string text)
        {
            LogTextColor(text, _discoveryForm.theme.TextBlockColor);
        }
        public void LogTextHighlight(string text)
        {
            LogTextColor(text, _discoveryForm.theme.TextBlockHighlightColor);
        }
        public void LogTextSuccess(string text)
        {
            LogTextColor(text, _discoveryForm.theme.TextBlockSuccessColor);
        }

        private void LogTextColor(string text, Color color)
        {
            richTextBox_History.AppendText(text,color);
        }
        
        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            HistoryEntry he = _discoveryForm.history.GetLastFSD;
            if ( he != null )
                Set(he.System);
        }

        private void toolStripButtonMap_Click(object sender, EventArgs e)
        {
            var centerSystem = TargetSystem;
            if (centerSystem == null || !centerSystem.HasCoordinate)
                centerSystem = _discoveryForm.history.GetLastWithPosition.System;

            var map = _discoveryForm.Map;

            map.Prepare(centerSystem, _discoveryForm.GetHomeSystem(), centerSystem,
                        _discoveryForm.settings.MapZoom, _discoveryForm.history.FilterByTravel);

            map.Show();
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

                    if (text != null)
                    {
                        try
                        {
                            System.Windows.Forms.Clipboard.SetText(text);
                        }
                        catch
                        {
                            _discoveryForm.LogLineHighlight("Copying text to clipboard failed");
                        }
                    }
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
                    Object ob= dataGridViewDistances[e.ColumnIndex, e.RowIndex].Value;
                    if (ob != null)
                        text = ob.ToString();

                    System.Diagnostics.Trace.WriteLine("Click:" + e.RowIndex.ToString() + ":" + e.ColumnIndex.ToString());

                    if (text != null)
                    {
                        try
                        {
                            System.Windows.Forms.Clipboard.SetText(text);
                        }
                        catch
                        {
                            _discoveryForm.LogLineHighlight("Copying text to clipboard failed");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception dataGridViewDistances_CellLeave: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
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

        private void checkForUnknownSystemsNowKnown()
        {
            for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
            {
                var systemCell = dataGridViewDistances[0, i];
                var oldSystem = (SystemClassDB)systemCell.Tag;
                if (!oldSystem.HasCoordinate)
                {
                    var value = systemCell.Value as string;
                    var newSystem = SystemClassDB.GetSystem(value);
                    if (newSystem != null && newSystem.HasCoordinate)
                    {
                        systemCell.Tag = newSystem;
                        dataGridViewDistances[3, i].Style.ForeColor = _discoveryForm.theme.VisitedSystemColor;
                        dataGridViewDistances[3, i].Value = "Position found";
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
            if (text != null)
                try
                {
                    System.Windows.Forms.Clipboard.SetText(text);
                }
                catch
                {
                    _discoveryForm.LogLineHighlight("Copying text to clipboard failed");
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
            try {
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

        public void AddWantedSystem(string sysName)
        {
            if (wanted == null)
                PopulateLocalWantedSystems();
            
            WantedSystemClass entry = wanted.Where(x => x.system == sysName).FirstOrDefault();  //duplicate?

            if (entry == null)
            {
                WantedSystemClass toAdd = new WantedSystemClass(sysName);       // make one..
                toAdd.Add();                                                    // add to db.

                wanted.Add(toAdd);

                SystemClassDB star = SystemClassDB.GetSystem(sysName);
                if (star == null)
                    star = new SystemClassDB(sysName);

                var index = dataGridViewClosestSystems.Rows.Add("Local");
                dataGridViewClosestSystems[1, index].Value = sysName;
                dataGridViewClosestSystems[1, index].Tag = star;
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
                    LogText(String.Format("{0} is pushed from EDSM and cannot be removed", sysName) + Environment.NewLine);
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
                if (!edsm.ShowSystemInEDSM(sysName)) LogTextHighlight("System could not be found - has not been synched or EDSM is unavailable" + Environment.NewLine);
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
            if (!edsm.ShowSystemInEDSM(sysName)) LogTextHighlight("System could not be found - has not been synched or EDSM is unavailable" + Environment.NewLine);

            this.Cursor = Cursors.Default;
        }

        private void deleteAllWithKnownPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sysName = "";
            for (int i = dataGridViewClosestSystems.RowCount - 1; i>=0; i--)
            {
                DataGridViewRow r = dataGridViewClosestSystems.Rows[i];
                sysName = r.Cells[1].Value.ToString();
                if (r.Cells[0].Value.ToString() == "Local")
                {
                    SystemClassDB sys = getSystemForTrilateration(sysName);
                    if (sys.HasCoordinate)
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
        }

        private void addAllLocalSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wanted != null && wanted.Any())
            {
                foreach (WantedSystemClass sys in wanted)
                {
                    AddSystemToDataGridViewDistances(sys.system);
                }
            }
        }

        private void addAllEDSMSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pushed != null && pushed.Any())
            {
                foreach (string sys in pushed)
                {
                    AddSystemToDataGridViewDistances(sys);
                }
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

        private void dataGridViewDistances_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 1)
            {
                double? v1 = DistanceParser.ParseInterstellarDistance(e.CellValue1?.ToString());
                double? v2 = DistanceParser.ParseInterstellarDistance(e.CellValue2?.ToString());

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
    }
}
