﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using EDDiscovery.DB;
using EDDiscovery2;
using ThreadState = System.Threading.ThreadState;
using EDDiscovery2.Trilateration;
using EDDiscovery2.EDSM;
using EDDiscovery2.DB;

namespace EDDiscovery
{
    public partial class TrilaterationControl : UserControl
    {
        private EDDiscoveryForm _discoveryForm;
        private ISystem TargetSystem;
        private Thread trilaterationThread;
        private Trilateration.Result lastTrilatelationResult;
        private Dictionary<SystemClass, Trilateration.Entry> lastTrilatelationEntries;
        private Thread EDSMSubmissionThread;
        private EDSMClass edsm;
        private List<WantedSystemClass> wanted;

        /** This global should be set if the next CurrentCellChanged() event should skip to the next editable cell.
         * This should be the case whenver a keyboard event causes cells to change, but not on mouse-initiated events */
        private bool skipReadOnlyCells = false;

        public TrilaterationControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            FreezeTrilaterationUI();
            edsm = new EDSMClass();
            edsm.apiKey = EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey;
            edsm.commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;
            SetTriStatus("Press Start New");
        }
        
        public void Set(ISystem system)
        {
            if (TargetSystem == null || !TargetSystem.Equals(system))
            {
                TargetSystem = system;
                ClearDataGridViewDistancesRows();
            }

            if (TargetSystem == null) return;

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

            UnfreezeTrilaterationUI();
            dataGridViewDistances.Focus();

            PopulateSuggestedSystems();

            dataGridViewClosestSystems.Rows.Clear();
            PopulateLocalWantedSystems();
            Thread ViewPushedSystemsThread = new Thread(ViewPushedSystems) { Name = "EDSM get pushed systems" };
            ViewPushedSystemsThread.Start();

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

        private void dataGridViewDistances_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                var textbox = (TextBox)e.Control;

                if (dataGridViewDistances.CurrentCell.ColumnIndex != 0)
                {
                    textbox.AutoCompleteMode = AutoCompleteMode.None;
                    return;
                }

                textbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textbox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textbox.AutoCompleteCustomSource = _discoveryForm.SystemNames;  // had to simplify this..  previously had it making it up dynamically from global system data.
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

                    var system = SystemClass.GetSystem(value);
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
        private SystemClass getSystemForTrilateration(string systemName)
        {
            var system = SystemClass.GetSystem(systemName);

            if (system == null)
            {
                if (!edsm.IsKnownSystem(systemName))
                {
                    LogTextHighlight("Only systems with coordinates or already known to EDSM can be added" + Environment.NewLine);
                }
                else
                {
                    system = new SystemClass(systemName);
                }
            }
            return system;
        }

        /* Callback for when a new system has been added to the grid.
         * Performs some additional setup such as clearing data and setting the status. */
        private void newSystemAdded(DataGridViewCell cell, SystemClass system)
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
                dataGridViewDistances[3, cell.RowIndex].Value = null;
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
                        dataGridViewDistances.Rows.Remove(dataGridViewDistances.Rows[e.RowIndex]);
                    }));
                    return;
                }

                var system = getSystemForTrilateration(value);
                if (system == null)
                {
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
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
                        // trigger trilateration calculation
                        RunTrilateration();
                    }
                }
            }
            /* skip to the next editable cell */
            skipReadOnlyCells = true;
        }

        private void RunTrilateration()
        {
            if (trilaterationThread != null)
            {
                if (trilaterationThread.ThreadState != ThreadState.Stopped)
                    LogTextHighlight("Aborting previous trilateration attempt." + Environment.NewLine);
                trilaterationThread.Abort();
            }
            trilaterationThread = new Thread(RunTrilaterationWorker) { Name = "Trilateration" };
            trilaterationThread.Start();
        }


        public IEnumerable<SystemClass> CurrentReferenceSystems
        {
            get
            {
                return (dataGridViewDistances.Rows.OfType<DataGridViewRow>().Select(row => row.Cells[0].Tag)).OfType<SystemClass>();
            }
        }

        private void RunTrilaterationWorker()
        {
            try
            {
                var systemsEntries = new Dictionary<SystemClass, Trilateration.Entry>();

                for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
                {
                    var systemCell = dataGridViewDistances[0, i];
                    var distanceCell = dataGridViewDistances[1, i];

                    if (systemCell.Tag == null || distanceCell.Value == null)
                    {
                        continue;
                    }

                    var system = (SystemClass)systemCell.Tag;
                    if (system != null && system.HasCoordinate)
                    {
                        var value = distanceCell.Value.ToString().Trim();
                        var parsedDistance = DistanceParser.ParseInterstellarDistance(value);
                        if (parsedDistance.HasValue)
                        {
                            var entry = new Trilateration.Entry(system.x, system.y, system.z, parsedDistance.Value);

                            systemsEntries.Add(system, entry);
                        }
                    }
                }

                if (systemsEntries.Count < 3)
                {
                    return;
                }

                Invoke((MethodInvoker)delegate
               {
                   LogText("Starting trilateration..." + Environment.NewLine);
                   SetTriStatus("Calculating…");
               });

                var trilateration = new Trilateration { Logger = Console.WriteLine };

                foreach (var item in systemsEntries)
                {
                    trilateration.AddEntry(item.Value);
                }

                //var trilaterationResultCS = trilateration.RunCSharp();
                var trilaterationAlgorithm = radioButtonAlgorithmJs.Checked
                    ? Trilateration.Algorithm.RedWizzard_Emulated
                    : Trilateration.Algorithm.RedWizzard_Native;

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var trilaterationResult = trilateration.Run(trilaterationAlgorithm);

                stopwatch.Stop();
                var spentTimeString = (stopwatch.ElapsedMilliseconds / 1000.0).ToString("0.0000") + "ms";

                lastTrilatelationResult = trilaterationResult;
                lastTrilatelationEntries = systemsEntries;

                if (trilaterationResult.State == Trilateration.ResultState.Exact)
                {
                    Invoke((MethodInvoker)delegate
                   {
                       SystemClass s1, s2, s3;

                       s1 = SystemClass.GetSystem("Sol");
                       s2 = SystemClass.GetSystem("Sagittarius A*");
                       s3 = new SystemClass();

                       s3.x = trilaterationResult.Coordinate.X;
                       s3.y = trilaterationResult.Coordinate.Y;
                       s3.z = trilaterationResult.Coordinate.Z;

                       LogTextSuccess("Trilateration successful (" + spentTimeString + "), exact coordinates found." + Environment.NewLine);
                       LogText("x=" + trilaterationResult.Coordinate.X + ", y=" + trilaterationResult.Coordinate.Y + ", z=" + trilaterationResult.Coordinate.Z + " Sol: " + SystemClass.Distance(s1, s3).ToString("0.0") + " Sag A* " + SystemClass.Distance(s2, s3).ToString("0.0") + Environment.NewLine);
                       SetTriStatusSuccess("Success, coordinates found!");
                   });
                }
                else if (trilaterationResult.State == Trilateration.ResultState.NotExact || trilaterationResult.State == Trilateration.ResultState.MultipleSolutions)
                {
                    Invoke((MethodInvoker)delegate
                   {
                       LogTextHighlight("Trilateration not successful (" + spentTimeString + "), only approximate coordinates found." + Environment.NewLine);
                       //LogText("x=" + trilaterationResult.Coordinate.X + ", y=" + trilaterationResult.Coordinate.Y + ", z=" + trilaterationResult.Coordinate.Z + Environment.NewLine);
                       LogTextHighlight("Enter more distances." + Environment.NewLine);
                       SetTriStatusError("Enter More Distances");
                   });
                }
                else if (trilaterationResult.State == Trilateration.ResultState.NeedMoreDistances)
                {
                    Invoke((MethodInvoker)delegate
                   {
                       LogTextHighlight("Trilateration not successful (" + spentTimeString + "), coordinates not found." + Environment.NewLine);
                       LogTextHighlight("Enter more distances." + Environment.NewLine);
                       SetTriStatusError("Enter More Distances");
                       ClearCalculatedDataGridViewDistancesRows();
                   });
                }


                // update trilaterated coordinates
                if (trilaterationResult.Coordinate != null)
                {
                    Invoke((MethodInvoker)delegate
                   {
                       textBoxCoordinateX.Text = trilaterationResult.Coordinate.X.ToString();
                       textBoxCoordinateY.Text = trilaterationResult.Coordinate.Y.ToString();
                       textBoxCoordinateZ.Text = trilaterationResult.Coordinate.Z.ToString();
                       if (TargetSystem != null)
                       {
                           TargetSystem.x = trilaterationResult.Coordinate.X;
                           TargetSystem.y = trilaterationResult.Coordinate.Y;
                           TargetSystem.z = trilaterationResult.Coordinate.Z;
                       }
                       toolStripButtonMap.Enabled = (TargetSystem != null);

                   });


                    var suggestedSystems = GetListOfSuggestedSystems(trilaterationResult.Coordinate.X,
                                                                     trilaterationResult.Coordinate.Y,
                                                                     trilaterationResult.Coordinate.Z, 16);

                    Invoke((MethodInvoker)(() => PopulateSuggestedSystems(suggestedSystems)));
                }
                else
                {
                    Invoke((MethodInvoker)delegate
                   {
                       textBoxCoordinateX.Text = "?";
                       textBoxCoordinateY.Text = "?";
                       textBoxCoordinateZ.Text = "?";
                   });
                }

                //var hasInvalidDistances = false;

                // update dataGrid with calculated distances and status
                var entriesDistances = trilaterationResult.EntriesDistances;

                for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
                {
                    var systemCell = dataGridViewDistances[0, i];
                    var calculatedDistanceCell = dataGridViewDistances[2, i];
                    var statusCell = dataGridViewDistances[3, i];

                    var system = (SystemClass)systemCell.Tag;

                    if (system == null)
                    {
                        continue;
                    }

                    if (system.HasCoordinate)
                    {
                        calculatedDistanceCell.Value = null;
                        statusCell.Value = null;
                    }
                    if (entriesDistances == null || systemCell.Value == null || systemCell.Tag == null)
                    {
                        continue;
                    }

                    if (!systemsEntries.ContainsKey(system)) // calculated without this system, so skip the row
                    {
                        continue;
                    }

                    var systemEntry = systemsEntries[system];
                    var calculatedDistance = entriesDistances[systemEntry];

                    calculatedDistanceCell.Value = calculatedDistance.ToString();

                    if (systemEntry.Distance == calculatedDistance)
                    {
                        statusCell.Value = "OK";
                        statusCell.Style.ForeColor = _discoveryForm.theme.VisitedSystemColor;
                    }
                    else
                    {
                        statusCell.Value = "Wrong distance?";
                        statusCell.Style.ForeColor = _discoveryForm.theme.NonVisitedSystemColor;
                    }
                }
            }
            catch (Exception ex)
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    LogTextHighlight("RunTrilateration Exception:" + ex.Message);
                    LogText(ex.StackTrace);
                }));

            }
        }

        private static IEnumerable<SystemClass> GetListOfSuggestedSystems(double x, double y, double z, int count)
        {
            var references = new SuggestedReferences(x, y, z);
            var suggestedSystems = new List<SystemClass>();
            for (int ii = 0; ii < count; ii++)
            {
                var rsys = references.GetCandidate();
                if (rsys == null) break;
                var system = rsys.System;
                references.AddReferenceStar(system);
                System.Diagnostics.Trace.WriteLine(string.Format("{0} Dist: {1} x:{2} y:{3} z:{4}", system.name,
                    rsys.Distance.ToString("0.00"), system.x, system.y, system.z));
                suggestedSystems.Add(system);
            }
            return suggestedSystems;
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

                    var system = (SystemClass)systemCell.Tag;

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

                var system = (SystemClass)systemCell.Tag;

                calculatedDistanceCell.Value = null;
                if (system != null && system.HasCoordinate) statusCell.Value = null;
            }
        }

        //private void ClearDataGridViewClosestSystemsRows()
        //{
        //    dataGridViewClosestSystems.Rows.Clear();
        //}

        private void ClearDataGridViewSuggestedSystemsRows()
        {
            dataGridViewSuggestedSystems.Rows.Clear();
        }
        
        private void PopulateSuggestedSystems()
        {
            var lastKnown = LastKnownSystem;
            if (lastKnown != null)
            {
                var suggestedSystems = GetListOfSuggestedSystems(lastKnown.x,
                                                                 lastKnown.y,
                                                                 lastKnown.z, 16);
                PopulateSuggestedSystems(suggestedSystems);
                return;
            }

            var suggestedSystemNames = new List<string>
            {
                "Sol", "Sadr", "Maia", "Polaris", "EZ Orionis",
                "Kappa-2 Coronae Austrinae", "Eta Carinae", "HR 969", "UX Sculptoris"
            };
            PopulateSuggestedSystems(suggestedSystemNames);
        }

        private void PopulateSuggestedSystems(ICollection<string> suggestedSystems)
        {
            dataGridViewSuggestedSystems.Rows.Clear();

            foreach (string name in _discoveryForm.SystemNames)
            {
                if (!suggestedSystems.Contains(name))
                {
                    SystemClass v = SystemClass.GetSystem(name);
                    if (v != null)
                        AddSuggestedSystem(v);
                }
            }
        }

        private void AddSuggestedSystem(SystemClass system)
        {

            for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
            {
                var systemCell = dataGridViewDistances[0, i];
                if (systemCell.Value!=null)
                    if (systemCell.Value.Equals(system.name))  // Dont add list thats already in distances. 
                        return; 
            }

            var index = dataGridViewSuggestedSystems.Rows.Add(system.name + " (" + system.x.ToString("0.00") + "," + system.y.ToString("0.00") + "," + system.z.ToString("0.00") +")");
            dataGridViewSuggestedSystems[0, index].Tag = system;
        }

        private void PopulateSuggestedSystems(IEnumerable<SystemClass> suggestedSystems)
        {
            dataGridViewSuggestedSystems.Rows.Clear();
            foreach (var system in suggestedSystems)
            {
                AddSuggestedSystem(system);
            }
        }

        private void PopulateLocalWantedSystems()
        {
            wanted = WantedSystemClass.GetAllWantedSystems();

            if (wanted != null && wanted.Any())
            {
                foreach (WantedSystemClass sys in wanted)
                {
                    SystemClass star = SystemClass.GetSystem(sys.system);
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

        // Runs as a thread.
        private void ViewPushedSystems()
        {
            try
            {
                List<String> systems = edsm.GetPushedSystems();

                foreach (String system in systems)
                {
                    SystemClass star = SystemClass.GetSystem(system);
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

        public ISystem LastKnownSystem
        {
            get
            {
                var lastKnown = (from systems
                    in _discoveryForm.VisitedSystems
                    where systems.curSystem != null && systems.curSystem.HasCoordinate
                    orderby systems.Time descending
                    select systems.curSystem).FirstOrDefault();
                return lastKnown;
            }
        }


        public ISystem CurrentSystem
        {
            get
            {
                var currentKnown = (from systems
                    in _discoveryForm.VisitedSystems
                                 orderby systems.Time descending
                                 select systems.curSystem).FirstOrDefault();
                return currentKnown;
            }
        }

        private void dataGridViewClosestSystems_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var system = (SystemClass)dataGridViewClosestSystems[1, e.RowIndex].Tag;
                AddSystemToDataGridViewDistances(system);
            }
        }

        /* Adds a system to the grid if it's not already in there */
        public void AddSystemToDataGridViewDistances(SystemClass system)
        {
            for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
            {
                var cell = dataGridViewDistances[0, i];
                SystemClass s2 = cell.Tag as SystemClass;
                if (s2 != null && s2.SearchName.Equals(system.SearchName))
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
                LogText("Submitting system to EDSM, please wait..." + Environment.NewLine);
                FreezeTrilaterationUI();

                if (trilaterationThread != null)
                {
                    trilaterationThread.Join();
                    trilaterationThread = null;
                }

                //// edge case - make sure distances were trilaterated OR the current system already has known coordinates
                //if (lastTrilatelationResult == null && !CurrentSystem.HasCoordinate)
                //{
                //    LogText("EDSM submission aborted, local trilateration did not run properly." + Environment.NewLine, Color.Red);
                //    UnfreezeTrilaterationUI();
                //    return;
                //}

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
                edsm.apiKey = EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey;
                edsm.commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;

                var travelHistoryControl = _discoveryForm.TravelControl;
                if (string.IsNullOrEmpty(edsm.commanderName))
                {
                    string commanderName = travelHistoryControl.GetCommanderName();

                    if (string.IsNullOrEmpty(commanderName))
                    {
                        MessageBox.Show("Please enter commander name before submitting the system!");
                        UnfreezeTrilaterationUI();
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

                var responseM = edsm.SubmitDistances(edsm.commanderName, TargetSystem.name, distances);

                Console.WriteLine(responseM);

                string infoM;
                bool trilaterationOkM;
                var responseOkM = edsm.ShowDistanceResponse(responseM, out infoM, out trilaterationOkM);

                Console.WriteLine(infoM);

                Invoke((MethodInvoker)delegate
               {
                   if (responseOkM && trilaterationOkM)
                   {
                       LogTextSuccess("EDSM submission succeeded, trilateration successful." + Environment.NewLine);
                   }
                   else if (responseOkM)
                   {
                       LogTextHighlight("EDSM submission succeeded, but trilateration failed. Try adding more distances." + Environment.NewLine);
                   }
                   else
                   {
                       LogTextHighlight("EDSM submission failed." + Environment.NewLine);
                   }

               });

                if (responseOkM && trilaterationOkM)
                {
                    Invoke((MethodInvoker)delegate
                   {
                    //Visible = false;
                    UnfreezeTrilaterationUI();
                       travelHistoryControl.TriggerEDSMRefresh(); // TODO we might eventually avoid this by further parsing EDSC response
                    travelHistoryControl.RefreshHistory();
                       checkForUnknownSystemsNowKnown();
                   });
                }
                else
                {
                    Invoke((MethodInvoker)UnfreezeTrilaterationUI);
                    lastTrilatelationResult = null;
                    lastTrilatelationEntries = null;
                }

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
            dataGridViewSuggestedSystems.Enabled = false;
        }

        private void UnfreezeTrilaterationUI()
        {
            dataGridViewDistances.Enabled = true;
            dataGridViewClosestSystems.Enabled = true;
            dataGridViewSuggestedSystems.Enabled = true;
            Dock = DockStyle.Fill;
        }

        private void radioButtonAlgorithm_CheckedChanged(object sender, EventArgs e)
        {
            // when algorithm is changed, we want to re-run trilateration
            RunTrilateration();
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

        private void dataGridViewSuggestedSystems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            var system = (SystemClass)dataGridViewSuggestedSystems[0, e.RowIndex].Tag;
            AddSystemToDataGridViewDistances(system);
            dataGridViewSuggestedSystems.Rows.RemoveAt(e.RowIndex);
            System.Windows.Forms.Clipboard.SetText(system.name); // copy to clipboard, speeds up search
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            if (_discoveryForm.SystemNames.Count == 0)
            {
                MessageBox.Show("Systems have not been loaded yet, please wait", "No Systems Available", MessageBoxButtons.OK);
                return;
            }

            Set(CurrentSystem);

            //for (int i = 0; i < 100; i++)     // use this to test the docking is right
              //  LogText("Hello " + i.ToString() + Environment.NewLine);

        }

        private void toolStripButtonMap_Click(object sender, EventArgs e)
        {
            var centerSystem = TargetSystem;
            if (centerSystem == null || !centerSystem.HasCoordinate) centerSystem = LastKnownSystem;
            var map = _discoveryForm.Map;

            map.Prepare(centerSystem.name, _discoveryForm.settings.MapHomeSystem, centerSystem.name,
                        _discoveryForm.settings.MapZoom, _discoveryForm.SystemNames,_discoveryForm.VisitedSystems);

            map.SetReferenceSystems(CurrentReferenceSystems.ToList());
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
                        System.Windows.Forms.Clipboard.SetText(text);
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
                        System.Windows.Forms.Clipboard.SetText(text);
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
            PopulateSuggestedSystems();
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
                var oldSystem = (SystemClass)systemCell.Tag;
                if (!oldSystem.HasCoordinate)
                {
                    var value = systemCell.Value as string;
                    var newSystem = SystemClass.GetSystem(value);
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
                System.Windows.Forms.Clipboard.SetText(text);

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
            if (wanted == null) wanted = new List<WantedSystemClass>();
            WantedSystemClass entry = wanted.Where(x => x.system == sysName).FirstOrDefault();
            if (entry == null)
            {
                WantedSystemClass toAdd = new WantedSystemClass(sysName);
                wanted.Add(toAdd);
                SystemClass star = SystemClass.GetSystem(sysName);
                if (star == null)
                    star = new SystemClass(sysName);

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

        private void splitContainerBottom_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!splitContainer1.LayoutChanging && splitContainerTop.SplitterDistance != splitContainerBottom.SplitterDistance)
                splitContainerTop.SplitterDistance = splitContainerBottom.SplitterDistance;
        }

        private void splitContainerTop_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!splitContainer1.LayoutChanging && splitContainerBottom.SplitterDistance != splitContainerTop.SplitterDistance)
                splitContainerBottom.SplitterDistance = splitContainerTop.SplitterDistance;
        }
    }
}
