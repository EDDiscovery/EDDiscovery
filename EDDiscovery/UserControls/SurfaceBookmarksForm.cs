using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using static EliteDangerousCore.DB.PlanetMarks;
using EDDiscovery.Forms;

namespace EDDiscovery.UserControls
{
    // now a leaf class - does not know about callers data.. much easier to understand

    public partial class SurfaceBookmarksForm : UserControl
    {
        public bool Edited = false;         // set if edit taken
        public PlanetMarks PlanetMarks { get { return planetmarks; } }      // current set..

        public Action<PlanetMarks> Changed;     // if a change to data is made
        public Action<string, string> CompassSeleted; // when compass is clicked

        private PlanetMarks planetmarks;

        #region Initialisation

        public SurfaceBookmarksForm()
        {
            InitializeComponent();
        }

        public void Init(string systemName)     // from bookmark form.  new system or update with name..  name can be blank
        {
            planetmarks = new PlanetMarks();

            dataGridViewMarks.CancelEdit();
            dataGridViewMarks.Rows.Clear();
            dataGridViewMarks.Enabled = true;
            sendToCompassToolStripMenuItem.Enabled = false;
            dataGridViewMarks.ColumnHeadersDefaultCellStyle.BackColor = dataGridViewMarks.RowHeadersDefaultCellStyle.BackColor = EDDTheme.Instance.GridBorderBack;

            if (!string.IsNullOrEmpty(systemName))
                UpdateComboBox(systemName);

            Edited = false;
        }

        public void Disable()
        {
            Edited = false;
            planetmarks = null;
            dataGridViewMarks.CancelEdit();
            dataGridViewMarks.ColumnHeadersDefaultCellStyle.BackColor = dataGridViewMarks.RowHeadersDefaultCellStyle.BackColor = EDDTheme.Instance.GridBorderBack.Multiply(0.5F);
            dataGridViewMarks.Rows.Clear();
            dataGridViewMarks.Enabled = false;
        }

        public void Init(string systemName, PlanetMarks pm)          // from UCB or Bookmark form, Init with a bookmark
        {
            Edited = false;
            planetmarks = pm!=null ? pm : new PlanetMarks();

            dataGridViewMarks.CancelEdit();
            dataGridViewMarks.ColumnHeadersDefaultCellStyle.BackColor = dataGridViewMarks.RowHeadersDefaultCellStyle.BackColor = EDDTheme.Instance.GridBorderBack;
            dataGridViewMarks.Rows.Clear();
            dataGridViewMarks.Enabled = true;

            dataGridViewMarks.SuspendLayout();

            if (!string.IsNullOrEmpty(systemName))
                UpdateComboBox(systemName);

            if (planetmarks.Planets != null)
            {
                foreach (Planet pl in planetmarks.Planets)
                {
                    foreach (Location loc in pl.Locations)
                    {
                        if (((DataGridViewComboBoxCell)dataGridViewMarks.Rows[0].Cells[0]).Items.Contains(pl.Name))
                        {
                            using (DataGridViewRow dr = dataGridViewMarks.Rows[dataGridViewMarks.Rows.Add()])
                            {
                                dr.Cells[0].Value = pl.Name;
                                dr.Cells[0].ReadOnly = true;
                                dr.Cells[1].Value = loc.Name;
                                dr.Cells[1].ReadOnly = true;
                                dr.Cells[2].Value = loc.Comment;
                                dr.Cells[3].Value = loc.Latitude.ToString("F4");
                                dr.Cells[4].Value = loc.Longitude.ToString("F4");
                                ((DataGridViewCheckBoxCell)dr.Cells[5]).Value = true;
                                dr.Tag = loc;
                            }
                        }
                    }
                }
            }

            sendToCompassToolStripMenuItem.Enabled = true;

            dataGridViewMarks.ResumeLayout();
        }

        public void AddSurfaceLocation(string planet, double latitude, double longitude)    // call After Reset to add a surface bookmark
        {
            using (DataGridViewRow dr = dataGridViewMarks.Rows[dataGridViewMarks.Rows.Add()])
            {
                dr.Cells[0].Value = planet;
                dr.Cells[0].ReadOnly = true;
                dr.Cells[1].Value = "Enter a name";
                dr.Cells[2].Value = "";
                dr.Cells[3].Value = latitude.ToString("F4");
                dr.Cells[4].Value = longitude.ToString("F4");
                ((DataGridViewCheckBoxCell)dr.Cells[5]).Value = false;
                dr.Cells[1].Selected = true;
            }
        }

        private void UpdateComboBox(string systemName)
        {
            ISystem thisSystem = EDDApplicationContext.EDDMainForm.history.FindSystem(systemName);

            DataGridViewComboBoxColumn c = (DataGridViewComboBoxColumn)dataGridViewMarks.Columns["BodyName"];

            c.Items.Clear();
            if (thisSystem != null)
            {
                var landables = EDDApplicationContext.EDDMainForm.history.starscan.FindSystem(thisSystem, true)?.Bodies?.Where(b => b.ScanData != null && b.ScanData.IsLandable)?.Select(b => b.fullname);

                if (landables != null)
                {
                    foreach (string s in landables)
                    {
                        System.Diagnostics.Debug.WriteLine("Drop " + s);
                        c.Items.Add(s);
                    }
                }
            }
        }

        #endregion

        #region UI

        private void dataGridViewMarks_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if(e.Row.Tag != null)
            {
                planetmarks.DeleteLocation(e.Row.Cells[0].Value.ToString(), ((Location)e.Row.Tag).Name);
                Edited = true;
                Changed?.Invoke(planetmarks);
            }
        }

        private void sendToCompassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow c = dataGridViewMarks.CurrentRow;

            if (c != null && c.Tag != null)
            {
                CompassSeleted?.Invoke(c.Cells[0].Value.ToString(), ((Location)c.Tag).Name);
            }
        }

        #endregion


        private void dataGridViewMarks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow rw = dataGridViewMarks.Rows[e.RowIndex];
            bool valid = ValidRow(rw);
            ((DataGridViewCheckBoxCell)rw.Cells[5]).Value = valid;

            if (valid)
            {
                Edited = true;
                //System.Diagnostics.Debug.WriteLine("Commit " + rw.Cells[0].Value.ToString());

                Location newLoc = rw.Tag != null ? (Location)rw.Tag : new Location();
                newLoc.Name = rw.Cells[1].Value.ToString();
                newLoc.Comment = rw.Cells[2].Value?.ToString();
                newLoc.Latitude = Double.Parse(rw.Cells[3].Value.ToString());
                newLoc.Longitude = Double.Parse(rw.Cells[4].Value.ToString());
                planetmarks.AddOrUpdateLocation(rw.Cells[0].Value.ToString(), newLoc);
                rw.Tag = newLoc;
                rw.Cells[0].ReadOnly = true;    // can't change the planet or location name once it's in the collection or we'll not be able to look it up again
                rw.Cells[1].ReadOnly = true;
                Changed?.Invoke(planetmarks);
            }
        }

        private void dataGridViewMarks_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 3 || e.ColumnIndex == 4)
            {
                string s = e.FormattedValue.ToString();

                dataGridViewMarks.Rows[e.RowIndex].ErrorText = "";

                if (s.Length > 0)
                {
                    double? v = s.ParseDoubleNull();
                    double lm = e.ColumnIndex == 3 ? 90 : 180;

                    if (v == null || v.Value < -lm || v.Value > lm)
                    {
                        e.Cancel = true;
                        dataGridViewMarks.Rows[e.RowIndex].ErrorText = "Invalid coordinate";
                    }
                }
            }
        }

        private bool ValidRow(DataGridViewRow dr)
        {
            if (dr.Cells[3].Value == null || dr.Cells[4].Value == null) return false;
            if (!Double.TryParse(dr.Cells[3].Value.ToString(), out double lat)) return false;
            if (!Double.TryParse(dr.Cells[4].Value.ToString(), out double lon)) return false;

            return dr.Cells[0].Value?.ToString() != "" && dr.Cells[1].Value?.ToString() != "" && lat >= -180 && lat <= 180 && lon >= -180 && lon <= 180;
        }
    }
}
