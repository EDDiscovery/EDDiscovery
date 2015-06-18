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

namespace EDDiscovery
{
    public partial class TrilaterationControl : UserControl
    {
        public SystemClass TargetSystem;

        public TrilaterationControl()
        {
            InitializeComponent();
        }
        
        private List<string> GetEnteredSystems()
        {
            var systems = new List<string>();
            for (int i = 0, size = dataGridViewDistances.Rows.Count - 1; i < size; i++)
            {
                var cell = dataGridViewDistances[0, i];
                var value = cell.Value;
                if (value != null)
                {
                    var stringValue = value.ToString();
                    if (stringValue != "")
                    {
                        systems.Add(value.ToString());
                    }
                }
            }
            return systems;
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridViewDistances.CurrentCell.ColumnIndex != 0)
            {
                return;
            }

            var textbox = (TextBox)e.Control;
            textbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textbox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            var items = new AutoCompleteStringCollection();

            var enteredSystems = GetEnteredSystems().Select((s2) => s2.ToLower());
            items.AddRange((
                from s
                in SystemData.SystemList
                where s.HasCoordinate && (s.name == textbox.Text || !enteredSystems.Contains(s.name.ToLower()))
                orderby s.name ascending
                select s.name
            ).ToArray());
            
            textbox.AutoCompleteCustomSource = items;
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                var value = e.FormattedValue.ToString();

                if (value == "")
                {
                    return;
                }

                var cell = dataGridViewDistances[e.ColumnIndex, e.RowIndex];
                var system = SystemData.GetSystem(value);
                var enteredSystems = GetEnteredSystems();
                if (cell.Value != null)
                {
                    enteredSystems.RemoveAll((string s) => s == cell.Value.ToString());
                }

                if (system == null || (enteredSystems.Contains(system.name)))
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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
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


                // trigger trilateration calculation
            }
        }

        private void TrilaterationControl_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                textBoxSystemName.Text = TargetSystem.name;


                var trilat = new Trilateration();
                trilat.Logger = (s) => TravelHistoryControl.LogText(s + Environment.NewLine);

                // WIP: just for testing now...
                trilat.addDistance(28.3125, -75.71875, 42.28125, 8.23);
                trilat.addDistance(30.4375, -79.1875, 26.34375, 13.26);
                trilat.addDistance(44.8125, -82.53125, 24.09375, 17.93);
                trilat.addDistance(-29.21875, -341.3125, -17.34375, 276.79);

                try {
                    var result = trilat.run();
                } catch (Trilateration.MoreDistancesNeededException ex)
                {
                    TravelHistoryControl.LogText("Need more distances.");
                }
            }

            if (Visible == false)
            {
                textBoxSystemName.Text = null;

                // keep systems, clear distances
                for (int i = 0, count = dataGridViewDistances.Rows.Count - 1; i < count; i++)
                {
                    var cell = dataGridViewDistances[1, i];
                    cell.Value = null;
                }
            }
        }
    }
}
