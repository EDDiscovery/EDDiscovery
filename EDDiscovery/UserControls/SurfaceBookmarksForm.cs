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
    public partial class SurfaceBookmarksForm : UserControl
    {
        public bool Edited = false;

        public PlanetMarks PlanetMarks
        {
            get { return internalPlanetMarks; }
            private set { internalPlanetMarks = value; }
        }

        PlanetMarks internalPlanetMarks;
        BookmarkClass thisBookmark;

        public SurfaceBookmarksForm()
        {
            InitializeComponent();
        }

        #region Bookmark Form

        public void NewForSystem(string systemName)     // from bookmark form
        {
            Edited = false;
            PlanetMarks = new PlanetMarks();
            dataGridViewMarks.Rows.Clear();
            SetSystem(systemName);
            buttonSave.Hide();
            sendToCompassToolStripMenuItem.Enabled = false;
        }

        public void AddSurfaceLocation(string planet, double latitude, double longitude)
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

        #endregion

        void SetSystem(string systemName)
        {
            ISystem thisSystem = EDDApplicationContext.EDDMainForm.history.FindSystem(systemName);
            ((DataGridViewComboBoxColumn)dataGridViewMarks.Columns["BodyName"]).Items.Clear();
            ((DataGridViewComboBoxColumn)dataGridViewMarks.Columns["BodyName"]).Items.Add("");
            if (thisSystem != null)
            {
                var landables = EDDApplicationContext.EDDMainForm.history.starscan.FindSystem(thisSystem, true)?.Bodies?.Where(b => b.ScanData != null && b.ScanData.IsLandable)?.Select(b => b.fullname);
                if (landables != null)
                {
                    foreach (string s in landables)
                    {
                        ((DataGridViewComboBoxColumn)dataGridViewMarks.Columns["BodyName"]).Items.Add(s);
                    }
                }
            }
        }

        #region Display Interface to Form/Bookmarks

        public void DisplayPlanetMarks(BookmarkClass bk)
        {
            Edited = false;
            dataGridViewMarks.Rows.Clear();

            if (bk == null)
            {
                dataGridViewMarks.AllowUserToAddRows = false;
                buttonSave.Hide();
                return;
            }

            if(bk.isRegion)
            {
                dataGridViewMarks.AllowUserToAddRows = false;
                buttonSave.Hide();
                return;
            }

            dataGridViewMarks.SuspendLayout();

            dataGridViewMarks.AllowUserToAddRows = true;
            SetSystem(bk.StarName);
            thisBookmark = bk;
            PlanetMarks = bk.PlanetaryMarks == null ? new PlanetMarks() : bk.PlanetaryMarks;

            if (PlanetMarks.Planets != null)
            {
                foreach (Planet pl in PlanetMarks.Planets)
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

        #endregion

        #region UI

        private void dataGridViewMarks_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if(e.ColumnIndex == 3 || e.ColumnIndex == 4)
            {
                //latitude or longitude
                if (Double.TryParse(e.FormattedValue.ToString(), out double parsed))
                {
                    if (parsed < -180 || parsed > 180)
                    {
                        e.Cancel = true;
                        dataGridViewMarks.Rows[e.RowIndex].ErrorText = "Invalid coordinate";
                    }
                    else
                        dataGridViewMarks.Rows[e.RowIndex].ErrorText = "";
                }
                else
                {
                    if (dataGridViewMarks.Rows[e.RowIndex].Cells[0].Value != null)
                    {
                        e.Cancel = true;
                        dataGridViewMarks.Rows[e.RowIndex].ErrorText = "Invalid number";
                    }
                }
            }
        }

        private void dataGridViewMarks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 3 || e.ColumnIndex == 4)
            {
                Double parsed = Double.Parse(dataGridViewMarks[e.ColumnIndex, e.RowIndex].Value.ToString());
                dataGridViewMarks[e.ColumnIndex, e.RowIndex].Value = parsed.ToString("F4");
            }
            DataGridViewRow dr = dataGridViewMarks.Rows[e.RowIndex];
            if (ValidRow(dr))
            {
                SaveLocation(dr);
            }
            else
                ((DataGridViewCheckBoxCell)dr.Cells[5]).Value = false;
        }

        private void SaveLocation(DataGridViewRow dr)
        {
            Location newLoc = dr.Tag != null ? (Location)dr.Tag : new Location();
            newLoc.Name = dr.Cells[1].Value.ToString();
            newLoc.Comment = dr.Cells[2].Value?.ToString();
            newLoc.Latitude = Double.Parse(dr.Cells[3].Value.ToString());
            newLoc.Longitude = Double.Parse(dr.Cells[4].Value.ToString());
            internalPlanetMarks.AddOrUpdateLocation(dr.Cells[0].Value.ToString(), newLoc);
            dr.Tag = newLoc;
            dr.Cells[0].ReadOnly = true;    // can't change the planet or location name once it's in the collection or we'll not be able to look it up again
            dr.Cells[1].ReadOnly = true;
            ((DataGridViewCheckBoxCell)dr.Cells[5]).Value = true;
            Edited = true;
        }
        
        private bool ValidRow(DataGridViewRow dr)
        {
            if (dr.Cells[3].Value == null || dr.Cells[4].Value == null) return false;
            if (!Double.TryParse(dr.Cells[3].Value.ToString(), out double lat)) return false;
            if (!Double.TryParse(dr.Cells[4].Value.ToString(), out double lon)) return false;

            return dr.Cells[0].Value?.ToString() != "" && dr.Cells[1].Value?.ToString() != "" && lat >= -180 && lat <= 180 && lon >= -180 && lon <= 180;
        }

        private void dataGridViewMarks_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if(e.Row.Tag != null)
            {
                internalPlanetMarks.DeleteLocation(e.Row.Cells[0].Value.ToString(), ((Location)e.Row.Tag).Name);
                Edited = true;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if(Edited)
            {
                GlobalBookMarkList.Instance.AddOrUpdateBookmark(thisBookmark, true, thisBookmark.StarName, thisBookmark.x, thisBookmark.y, thisBookmark.z, thisBookmark.Time, thisBookmark.Note, internalPlanetMarks);
                Edited = false;
            }
        }

        private void sendToCompassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dataGridViewMarks.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .FirstOrDefault();
            if (dr != null)
            {
                if (dr.Tag == null && ValidRow(dr)) SaveLocation(dr);

                if (thisBookmark != null && dr.Tag != null)
                {
                    if (Edited)
                    {
                        GlobalBookMarkList.Instance.AddOrUpdateBookmark(thisBookmark, true, thisBookmark.StarName, thisBookmark.x, thisBookmark.y, thisBookmark.z, thisBookmark.Time, thisBookmark.Note, internalPlanetMarks);
                        Edited = false;
                    }

                    UserControlCompass comp = (UserControlCompass)EDDApplicationContext.EDDMainForm.PopOuts.PopOut(PanelInformation.PanelIDs.Compass);
                    comp.SetSurfaceBookmark(thisBookmark, dr.Cells[0].Value.ToString(), dr.Cells[1].Value.ToString());
                }
            }
        }

        #endregion
    }
}
