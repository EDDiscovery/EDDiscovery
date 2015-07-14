using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using EDDiscovery.DB;
using ThreadState = System.Threading.ThreadState;

namespace EDDiscovery
{
    public partial class TrilaterationControl : UserControl
    {
        public SystemClass TargetSystem;
        private Thread trilaterationThread;
        private Trilateration.Result lastTrilatelationResult;
        private Dictionary<SystemClass, Trilateration.Entry> lastTrilatelationEntries;
        private Thread EDSCSubmissionThread;

        public TrilaterationControl()
        {
            InitializeComponent();
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
            var textbox = (TextBox)e.Control;

            if (dataGridViewDistances.CurrentCell.ColumnIndex != 0)
            {
                textbox.AutoCompleteMode = AutoCompleteMode.None;
                return;
            }
            
            textbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textbox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            var items = new AutoCompleteStringCollection();

            var enteredSystems = GetEnteredSystems();
            items.AddRange((
                from s
                in SystemData.SystemList
                where s.HasCoordinate && (s.name == textbox.Text || !enteredSystems.Contains(s))
                orderby s.name ascending
                select s.name
            ).ToArray());
            
            textbox.AutoCompleteCustomSource = items;
        }

        private void dataGridViewDistances_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var value = e.FormattedValue.ToString();
                var cell = dataGridViewDistances[e.ColumnIndex, e.RowIndex];

                if (value == "" && (cell.Value == null || cell.Value.ToString() == ""))
                {
                    return;
                }

                var system = SystemData.GetSystem(value);
                var enteredSystems = GetEnteredSystems();
                if (cell.Value != null)
                {
                    enteredSystems.RemoveAll(s => s.name == cell.Value.ToString());
                }

                if (system == null || (enteredSystems.Contains(system)))
                {
                    //e.Cancel = true;
                    return;
                }
            }

            if (e.ColumnIndex == 1)
            {
                var value = e.FormattedValue.ToString();

                if (value == "")
                {
                    return;
                }

                var regex = new Regex(@"^\d{1,5}([,.]\d{1,2})?$");
                e.Cancel = !regex.Match(e.FormattedValue.ToString()).Success;
            }
        }

        private void dataGridViewDistances_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var cell = dataGridViewDistances[e.ColumnIndex, e.RowIndex];
                var value = cell.Value;
                cell.Style.BackColor = Color.White;
                if (value == null)
                {
                    return;
                }
                var stringValue = value.ToString();
                var system = SystemData.GetSystem(stringValue);

                if (system == null)
                {
                    cell.Value = null;
                    //cell.Style.BackColor = Color.Salmon;
                    return;
                }

                if (stringValue != system.name)
                {
                    cell.Value = system.name;
                }
                cell.Tag = system;
            }

            // reset any calculated distances
            dataGridViewDistances[2, e.RowIndex].Value = null;
            dataGridViewDistances[3, e.RowIndex].Value = null;

            // trigger trilateration calculation
            RunTrilateration();
        }

        private void TrilaterationControl_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true && TargetSystem != null)
            {
                textBoxSystemName.Text = TargetSystem.name;
                labelStatus.Text = "Enter Distances";
                labelStatus.BackColor = Color.LightBlue;

                UnfreezeTrilaterationUI();
                dataGridViewDistances.Focus();

                PopulateSuggestedSystems();
                PopulateClosestSystems();
            }

            if (Visible == false)
            {
                if (trilaterationThread != null)
                {
                    trilaterationThread.Abort();
                    trilaterationThread = null;
                }

                textBoxSystemName.Text = null;
                textBoxCoordinateX.Text = "?";
                textBoxCoordinateY.Text = "?";
                textBoxCoordinateZ.Text = "?";
                labelLastKnownSystem.Text = "Unknown";

                ClearDataGridViewDistancesRows();
                ClearDataGridViewClosestSystemsRows();
                ClearDataGridViewSuggestedSystemsRows();

                lastTrilatelationResult = null;
                lastTrilatelationEntries = null;
            }
        }

        private void RunTrilateration()
        {
            if (trilaterationThread != null)
            {
                if (trilaterationThread.ThreadState != ThreadState.Stopped)
                    TravelHistoryControl.LogText("Aborting previous trilateration attempt." + Environment.NewLine);
                trilaterationThread.Abort();
            }
            trilaterationThread = new Thread(RunTrilaterationWorker) { Name = "Trilateration" };
            trilaterationThread.Start();
        }

        private void RunTrilaterationWorker()
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
                var culture = new CultureInfo("en-US");
                var distance = double.Parse(distanceCell.Value.ToString().Replace(",", "."), culture);

                var entry = new Trilateration.Entry(system.x, system.y, system.z, distance);

                systemsEntries.Add(system, entry);
            }

            if (systemsEntries.Count < 3)
            {
                return;
            }

            Invoke((MethodInvoker) delegate
            {
                TravelHistoryControl.LogText("Starting trilateration..." + Environment.NewLine);
                labelStatus.Text = "Calculating…";
                labelStatus.BackColor = Color.Gold;
            });

            var trilateration = new Trilateration {Logger = Console.WriteLine};

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
                Invoke((MethodInvoker) delegate
                {
                    TravelHistoryControl.LogText("Trilateration successful (" + spentTimeString + "), exact coordinates found." + Environment.NewLine);
                    //TravelHistoryControl.LogText("x=" + trilaterationResult.Coordinate.X + ", y=" + trilaterationResult.Coordinate.Y + ", z=" + trilaterationResult.Coordinate.Z + Environment.NewLine);
                    labelStatus.Text = "Success, coordinates found!";
                    labelStatus.BackColor = Color.LawnGreen;
                });
            } else if (trilaterationResult.State == Trilateration.ResultState.NotExact || trilaterationResult.State == Trilateration.ResultState.MultipleSolutions)
            {
                Invoke((MethodInvoker) delegate
                {
                    TravelHistoryControl.LogText("Trilateration not successful (" + spentTimeString + "), only approximate coordinates found." + Environment.NewLine);
                    //TravelHistoryControl.LogText("x=" + trilaterationResult.Coordinate.X + ", y=" + trilaterationResult.Coordinate.Y + ", z=" + trilaterationResult.Coordinate.Z + Environment.NewLine);
                    TravelHistoryControl.LogText("Enter more distances." + Environment.NewLine);
                    labelStatus.Text = "Enter More Distances";
                    labelStatus.BackColor = Color.Orange;
                });
            } else if (trilaterationResult.State == Trilateration.ResultState.NeedMoreDistances)
            {
                Invoke((MethodInvoker) delegate
                {
                    TravelHistoryControl.LogText("Trilateration not successful (" + spentTimeString + "), coordinates not found." + Environment.NewLine);
                    TravelHistoryControl.LogText("Enter more distances." + Environment.NewLine);
                    labelStatus.Text = "Enter More Distances";
                    labelStatus.BackColor = Color.Red;
                    ClearCalculatedDataGridViewDistancesRows();
                });
            }
            
            // update trilaterated coordinates
            if (trilaterationResult.Coordinate != null)
            {
                Invoke((MethodInvoker) delegate
                {
                    textBoxCoordinateX.Text = trilaterationResult.Coordinate.X.ToString();
                    textBoxCoordinateY.Text = trilaterationResult.Coordinate.Y.ToString();
                    textBoxCoordinateZ.Text = trilaterationResult.Coordinate.Z.ToString();
                });
            }
            else
            {
                Invoke((MethodInvoker) delegate
                {
                    textBoxCoordinateX.Text = "?";
                    textBoxCoordinateY.Text = "?";
                    textBoxCoordinateZ.Text = "?";
                });
            }

            var hasInvalidDistances = false;

            // update dataGrid with calculated distances and status
            var entriesDistances = trilaterationResult.EntriesDistances;
                
            for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
            {
                var systemCell = dataGridViewDistances[0, i];
                var calculatedDistanceCell = dataGridViewDistances[2, i];
                var statusCell = dataGridViewDistances[3, i];

                calculatedDistanceCell.Value = null;
                statusCell.Value = null;

                if (entriesDistances == null || systemCell.Value == null || systemCell.Tag == null)
                {
                    continue;
                }

                var system = (SystemClass) systemCell.Tag;

                if (!systemsEntries.ContainsKey(system)) // calculated without this system, so skip the row
                {
                    continue;
                }

                var systemEntry = systemsEntries[system];
                var calculatedDistance = entriesDistances[systemEntry];

                calculatedDistanceCell.Value = calculatedDistance.ToString();

                if (systemEntry.Distance == calculatedDistance)
                {
                    calculatedDistanceCell.Style.ForeColor = Color.Green;
                    statusCell.Value = "OK";
                    statusCell.Style.ForeColor = Color.Green;
                } else
                {
                    hasInvalidDistances = true;
                    calculatedDistanceCell.Style.ForeColor = Color.Salmon;
                    statusCell.Value = "Wrong distance?";
                    statusCell.Style.ForeColor = Color.Salmon;
                }
            }

            Invoke((MethodInvoker) delegate
            {
                buttonSubmitToEDSC.Enabled = trilaterationResult.State == Trilateration.ResultState.Exact && !hasInvalidDistances;
            });
        }

        private void ClearDataGridViewDistancesRows()
        {
            // keep systems, clear distances
            for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
            {
                var distanceCell = dataGridViewDistances[1, i];
                var calculatedDistanceCell = dataGridViewDistances[2, i];
                var statusCell = dataGridViewDistances[3, i];

                distanceCell.Value = null;
                calculatedDistanceCell.Value = null;
                statusCell.Value = null;
            }
        }

        private void ClearCalculatedDataGridViewDistancesRows()
        {
            // keep systems and distances, clear calculated distances and statuses
            for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
            {
                var calculatedDistanceCell = dataGridViewDistances[2, i];
                var statusCell = dataGridViewDistances[3, i];

                calculatedDistanceCell.Value = null;
                statusCell.Value = null;
            }
        }

        private void ClearDataGridViewClosestSystemsRows()
        {
            dataGridViewClosestSystems.Rows.Clear();
        }

        private void ClearDataGridViewSuggestedSystemsRows()
        {
            dataGridViewSuggestedSystems.Rows.Clear();
        }

        private void PopulateSuggestedSystems()
        {
            // TODO for now, just add few systems at different locations
            // eventually we might implement something more clever here

            var suggestedSystems = new List<string>
            {
                "Sol", "Sadr", "Maia", "Polaris", "EZ Orionis",
                "Kappa-2 Coronae Austrinae", "Eta Carinae", "HR 969", "UX Sculptoris"
            };
            foreach (var system in SystemData.SystemList)
            {
                if (!suggestedSystems.Contains(system.name))
                {
                    continue;
                }

                var index = dataGridViewSuggestedSystems.Rows.Add(system.name);
                dataGridViewSuggestedSystems[0, index].Tag = system;
            }
        }

        private void PopulateClosestSystems()
        {
            // TODO: in future, we want this to be "predicted" by the direction and distances
            
            var lastKnown = (from systems
                             in ((TravelHistoryControl)Parent).visitedSystems
                             where systems.curSystem != null && systems.curSystem.HasCoordinate
                             orderby systems.time descending
                             select systems.curSystem).FirstOrDefault();

            if (lastKnown == null)
            {
                return;
            }

            labelLastKnownSystem.Text = lastKnown.name;

            var closest = (from systems
                           in SystemData.SystemList
                           where systems != lastKnown && systems.HasCoordinate
                           select new
                           {
                               System = systems,
                               Distance = Math.Sqrt(Math.Pow(lastKnown.x - systems.x, 2) + Math.Pow(lastKnown.y - systems.y, 2) + Math.Pow(lastKnown.z - systems.z, 2))
                           })
                          .OrderBy(c => c.Distance)
                          .Take(30);

            foreach (var item in closest)
            {
                var index = dataGridViewClosestSystems.Rows.Add(item.System.name, Math.Round(item.Distance, 2).ToString("0.00") + " Ly");
                dataGridViewClosestSystems[0, index].Tag = item.System;
            }
        }

        private void dataGridViewClosestSystems_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var system = (SystemClass) dataGridViewClosestSystems[0, e.RowIndex].Tag;
            AddSystemToDataGridViewDistances(system);
        }

        private void dataGridViewSuggestedSystems_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var system = (SystemClass)dataGridViewSuggestedSystems[0, e.RowIndex].Tag;
            AddSystemToDataGridViewDistances(system);
        }

        private void AddSystemToDataGridViewDistances(SystemClass system)
        {
            for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
            {
                var cell = dataGridViewDistances[0, i];
                if (cell.Tag != null && (SystemClass)cell.Tag == system)
                {
                    return;
                }
            }

            var index = dataGridViewDistances.Rows.Add(system.name);
            dataGridViewDistances[0, index].Tag = system;
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            TravelHistoryControl.LogText("Submitting system to EDSC, please wait..." + Environment.NewLine);
            FreezeTrilaterationUI();

            if (trilaterationThread != null)
            {
                trilaterationThread.Join();
                trilaterationThread = null;
            }

            // edge case - make sure distances were trilaterated
            if (lastTrilatelationResult == null)
            {
                TravelHistoryControl.LogText("EDSC submission aborted, local trilateration did not run properly." + Environment.NewLine, Color.Red);
                UnfreezeTrilaterationUI();
                return;
            }

            // edge case - should not happen, usually, but, just in case...
            if (lastTrilatelationResult.State != Trilateration.ResultState.Exact)
            {
                TravelHistoryControl.LogText("EDSC submission aborted, local trilateration failed." + Environment.NewLine, Color.Red);
                UnfreezeTrilaterationUI();
                return;
            }

            EDSCSubmissionThread = new Thread(SubmitToEDSC) {Name = "EDSC Submission"};
            EDSCSubmissionThread.Start();
        }

        private void SubmitToEDSC()
        {
            var travelHistoryControl = (TravelHistoryControl)Parent;
            string commanderName = travelHistoryControl.GetCommanderName();

            if (string.IsNullOrEmpty(commanderName))
            {
                MessageBox.Show("Please enter commander name before submitting the system!");
                UnfreezeTrilaterationUI();
                return;
            }

            var distances = new Dictionary<string, double>();
            foreach (var item in lastTrilatelationEntries)
            {
                var system = item.Key;
                var entry = item.Value;
                distances.Add(system.name, entry.Distance);
            }

            var edsc = new EDSCClass();
            
            //if (!EDSCClass.UseTest)
            //{
            //    Invoke((MethodInvoker) delegate
            //    {
            //        // TODO temporarily mess with EDSC in test mode only
            //        TravelHistoryControl.LogText("Forcibly switching to EDSC UseTest mode." + Environment.NewLine, Color.OrangeRed);
            //    });
            //    EDSCClass.UseTest = true;
            //}
     
            var response = edsc.SubmitDistances(commanderName, TargetSystem.name, distances);
            
            Console.WriteLine(response);

            string info;
            var responseOk = edsc.ShowDistanceResponse(response, out info);
            var trilaterationOk = info.IndexOf("Trilateration succesful") != -1; // FIXME this is ugly
            Console.WriteLine(info);

            Invoke((MethodInvoker) delegate
            {
                if (responseOk && trilaterationOk)
                {
                    TravelHistoryControl.LogText("EDSC submission succeeded, trilateration successful." + Environment.NewLine, Color.Green);
                }
                else if (responseOk)
                {
                    TravelHistoryControl.LogText("EDSC submission succeeded, but trilateration failed. Try adding more distances." + Environment.NewLine, Color.Orange);
                }
                else
                {
                    TravelHistoryControl.LogText("EDSC submission failed." + Environment.NewLine, Color.Red);
                }
            });

            if (responseOk && trilaterationOk)
            {
                Invoke((MethodInvoker) delegate
                {
                    Visible = false;
                    travelHistoryControl.TriggerEDSCRefresh(); // TODO we might eventually avoid this by further parsing EDSC response
                    travelHistoryControl.RefreshHistory();
                });
            }
            else
            {
                Invoke((MethodInvoker) UnfreezeTrilaterationUI);
                lastTrilatelationResult = null;
                lastTrilatelationEntries = null;
            }
        }

        private void FreezeTrilaterationUI()
        {
            buttonSubmitToEDSC.Enabled = false;
            dataGridViewDistances.Enabled = false;
            dataGridViewClosestSystems.Enabled = false;
            dataGridViewSuggestedSystems.Enabled = false;
        }

        private void UnfreezeTrilaterationUI()
        {
            buttonSubmitToEDSC.Enabled = lastTrilatelationResult != null && lastTrilatelationResult.State == Trilateration.ResultState.Exact;
            dataGridViewDistances.Enabled = true;
            dataGridViewClosestSystems.Enabled = true;
            dataGridViewSuggestedSystems.Enabled = true;
        }

        private void radioButtonAlgorithm_CheckedChanged(object sender, EventArgs e)
        {
            // when algorithm is changed, we want to re-run trilateration
            RunTrilateration();
        }

        private void buttonCloseTrilateration_Click(object sender, EventArgs e)
        {
            Visible = false;
        }
    }
}
