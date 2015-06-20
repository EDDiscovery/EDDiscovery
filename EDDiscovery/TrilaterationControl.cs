using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery.DB;
using System.Text.RegularExpressions;
using System.Threading;

namespace EDDiscovery
{
    public partial class TrilaterationControl : UserControl
    {
        public SystemClass TargetSystem;
        private Thread trilaterationThread;

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
                    enteredSystems.RemoveAll((SystemClass s) => s.name == cell.Value.ToString());
                }

                if (system == null || (enteredSystems.Contains(system)))
                {
                    e.Cancel = true;
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
                if (value == null)
                {
                    return;
                }
                var stringValue = value.ToString();
                var system = SystemData.GetSystem(stringValue);

                if (system == null)
                {
                    cell.Value = null;
                    return;
                }

                if (stringValue != system.name)
                {
                    cell.Value = system.name;
                }
                cell.Tag = system;
            }

            // trigger trilateration calculation
            if (trilaterationThread != null)
            {
                TravelHistoryControl.LogText("Aborting previous trilateration attempt." + Environment.NewLine);
                trilaterationThread.Abort();
            }
            trilaterationThread = new Thread(new ThreadStart(RunTrilateration));
            trilaterationThread.Name = "Trilateration";
            
            trilaterationThread.Start();
        }

        private void TrilaterationControl_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true && TargetSystem != null)
            {
                textBoxSystemName.Text = TargetSystem.name;
                labelStatus.Text = "Enter Distances";
                labelStatus.BackColor = Color.LightBlue;

                //var trilat = new Trilateration();
                //trilat.Logger = (s) => TravelHistoryControl.LogText(s + Environment.NewLine);

                //// WIP: just for testing now...
                //trilat.addDistance(28.3125, -75.71875, 42.28125, 8.23);
                //trilat.addDistance(30.4375, -79.1875, 26.34375, 13.26);
                //trilat.addDistance(44.8125, -82.53125, 24.09375, 17.93);
                //trilat.addDistance(-29.21875, -341.3125, -17.34375, 276.79);

                //try
                //{
                //    var result = trilat.run();
                //}
                //catch (Trilateration.MoreDistancesNeededException ex)
                //{
                //    TravelHistoryControl.LogText("Need more distances.");
                //}
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

                ClearDataGridRows();
            }
        }

        private void RunTrilateration()
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
                var distance = Double.Parse(distanceCell.Value.ToString().Replace(".", ",")); // TODO i18n for delimiter

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
            });

            var trilateration = new Trilateration();
            trilateration.Logger = (s) => System.Console.WriteLine(s);

            foreach (var item in systemsEntries)
            {
                trilateration.AddEntry(item.Value);
            }

            trilateration.runTril();

            var trilaterationResult = trilateration.Run();

            if (trilaterationResult.State == Trilateration.ResultState.Exact)
            {
                Invoke((MethodInvoker) delegate
                {
                    TravelHistoryControl.LogText("Trilateration successful, exact coordinates found." + Environment.NewLine);
                    TravelHistoryControl.LogText("x=" + trilaterationResult.Coordinate.X + ", y=" + trilaterationResult.Coordinate.Y + ", z=" + trilaterationResult.Coordinate.Z + Environment.NewLine);
                    labelStatus.Text = "Success, coordinates found!";
                    labelStatus.BackColor = Color.LawnGreen;
                });
            } else if (trilaterationResult.State == Trilateration.ResultState.NotExact)
            {
                Invoke((MethodInvoker) delegate
                {
                    TravelHistoryControl.LogText("Trilateration not successful, only approximate coordinates found." + Environment.NewLine);
                    TravelHistoryControl.LogText("x=" + trilaterationResult.Coordinate.X + ", y=" + trilaterationResult.Coordinate.Y + ", z=" + trilaterationResult.Coordinate.Z + Environment.NewLine);
                    TravelHistoryControl.LogText("Enter more distances." + Environment.NewLine);
                    labelStatus.Text = "Enter More Distances";
                    labelStatus.BackColor = Color.Orange;
                });
            } else if (trilaterationResult.State == Trilateration.ResultState.NeedMoreDistances)
            {
                Invoke((MethodInvoker) delegate
                {
                    TravelHistoryControl.LogText("Trilateration not successful, coordinates not found." + Environment.NewLine);
                    TravelHistoryControl.LogText("Enter more distances." + Environment.NewLine);
                    labelStatus.Text = "Enter More Distances";
                    labelStatus.BackColor = Color.Red;
                    ClearCalculatedDataGridRows();
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

            // update dataGrid with calculated distances and status
            if (trilaterationResult.EntriesDistances != null)
            {
                var entriesDistances = trilaterationResult.EntriesDistances;
                var inversedSystemEntriesDict = systemsEntries.ToDictionary(x => x.Value, x => x.Key);
                
                for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
                {
                    var systemCell = dataGridViewDistances[0, i];
                    var calculatedDistanceCell = dataGridViewDistances[2, i];
                    var statusCell = dataGridViewDistances[3, i];

                    calculatedDistanceCell.Value = null;
                    statusCell.Value = null;

                    if (systemCell.Value == null || systemCell.Tag == null)
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
                        calculatedDistanceCell.Style.ForeColor = Color.Red;
                        statusCell.Value = "Wrong distance";
                        statusCell.Style.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void ClearDataGridRows()
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

        private void ClearCalculatedDataGridRows()
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
    }
}
