﻿/*
 * Copyright © 2019 EDDiscovery development team
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using static EliteDangerousCore.DB.PlanetMarks;

namespace EDDiscovery.Forms
{
    // now a leaf class - does not know about callers data.. much easier to understand

    public partial class SurfaceBookmarkUserControl : UserControl
    {
        public bool Edited = false;         // set if edit taken
        public PlanetMarks PlanetMarks { get { return planetmarks; } }      // current set..

        public Action<PlanetMarks> Changed;     // if a change to data is made
        public Action<string, string> CompassSeleted; // when compass is clicked

        private PlanetMarks planetmarks;

        #region Initialisation

        public SurfaceBookmarkUserControl()
        {
            InitializeComponent();
            var enumlist = new Enum[] { EDTx.SurfaceBookmarkUserControl_BodyName, EDTx.SurfaceBookmarkUserControl_SurfaceName, EDTx.SurfaceBookmarkUserControl_SurfaceDesc, EDTx.SurfaceBookmarkUserControl_Latitude, EDTx.SurfaceBookmarkUserControl_Longitude, EDTx.SurfaceBookmarkUserControl_Valid, EDTx.SurfaceBookmarkUserControl_labelSurface };
            var enumlistcms = new Enum[] { EDTx.SurfaceBookmarkUserControl_sendToCompassToolStripMenuItem, EDTx.SurfaceBookmarkUserControl_deleteToolStripMenuItem, EDTx.SurfaceBookmarkUserControl_addPlanetManuallyToolStripMenuItem };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);
        }

        public void Init(string systemName, HistoryList helist, PlanetMarks pm = null)
        {
            Edited = false;

            planetmarks = pm != null ? pm : new PlanetMarks();

            dataGridViewMarks.CancelEdit();
            dataGridViewMarks.Rows.Clear();
            dataGridViewMarks.Enabled = true;
            sendToCompassToolStripMenuItem.Enabled = false;
            dataGridViewMarks.ColumnHeadersDefaultCellStyle.BackColor = dataGridViewMarks.RowHeadersDefaultCellStyle.BackColor = ExtendedControls.Theme.Current.GridBorderBack;

            LoadGrid(systemName, helist);
        }

        // we do this in an async task, as the FindSystemAsync will go away for a while
        // when it returns, we can then fill the grid if required with the right body name set.
        private async void LoadGrid(string systemName, HistoryList helist)
        {
            //System.Diagnostics.Debug.WriteLine($"Lookup for planets '{systemName}'");
            var lookup = await helist.StarScan.FindSystemAsync(new SystemClass(systemName), true);

            // lets present all, even if not landable, as you may want a whole planet bookmark
            var bodies = lookup?.Bodies?.Select(b => b.FullName.ReplaceIfStartsWith(systemName) + (b.CustomName != null ? " " + b.CustomName : ""));

            //System.Diagnostics.Debug.WriteLine($".. Finish lookup for planets '{systemName}' Got {bodies?.ToArray().Length}");

            if (bodies != null)
            {
                BodyName.Items.Clear();
                foreach (string s in bodies)
                {
                    //System.Diagnostics.Debug.WriteLine($"Add bookmark landable '{s}'");
                    BodyName.Items.Add(s);
                }

                if (PlanetMarks?.Planets != null)
                {
                    foreach (Planet pl in PlanetMarks.Planets)
                    {
                        foreach (Location loc in pl.Locations)
                        {
                            System.Diagnostics.Debug.WriteLine($"Add row {pl.Name} {loc.Name} {loc.Longitude} {loc.Latitude}");
                            using (DataGridViewRow dr = dataGridViewMarks.Rows[dataGridViewMarks.Rows.Add()])
                            {
                                if (!BodyName.Items.Contains(pl.Name))      // ensure planet in collection so we don't get errors..
                                    BodyName.Items.Add(pl.Name);

                                dr.Cells[0].Value = pl.Name;
                                dr.Cells[0].ReadOnly = true;
                                dr.Cells[1].Value = loc.Name;
                                dr.Cells[2].Value = loc.Comment;

                                if (loc.IsWholePlanetBookmark)              // whole planet gets empty lat/long
                                {
                                    dr.Cells[3].Value = dr.Cells[4].Value = "";
                                }
                                else
                                {
                                    dr.Cells[3].Value = loc.Latitude.ToString("F4");
                                    dr.Cells[4].Value = loc.Longitude.ToString("F4");
                                }

                                ((DataGridViewCheckBoxCell)dr.Cells[5]).Value = true;
                                dr.Tag = loc;
                            }
                        }
                    }
                }
            }
        }

        public void Disable()
        {
            Edited = false;
            planetmarks = null;
            dataGridViewMarks.CancelEdit();
            dataGridViewMarks.ColumnHeadersDefaultCellStyle.BackColor = dataGridViewMarks.RowHeadersDefaultCellStyle.BackColor = ExtendedControls.Theme.Current.GridBorderBack.Multiply(0.5F);
            dataGridViewMarks.Rows.Clear();
            dataGridViewMarks.Enabled = false;
        }

        public void AddSurfaceLocation(string planet, double latitude, double longitude)    // call After Reset to add a surface bookmark
        {
            using (DataGridViewRow dr = dataGridViewMarks.Rows[dataGridViewMarks.Rows.Add()])
            {
                if (!BodyName.Items.Contains(planet))
                    BodyName.Items.Add(planet);

                dr.Cells[0].Value = planet;
                dr.Cells[0].ReadOnly = true;
                dr.Cells[1].Value = "Enter a name".T(EDTx.SurfaceBookmarkUserControl_Enter);
                dr.Cells[2].Value = "";
                dr.Cells[3].Value = latitude.ToString("F4");
                dr.Cells[4].Value = longitude.ToString("F4");
                ((DataGridViewCheckBoxCell)dr.Cells[5]).Value = false;
                dr.Cells[1].Selected = true;
            }
        }

        #endregion

        #region UI

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewMarks.RightClickRow >= 0)
            {
                deleteToolStripMenuItem.Enabled = dataGridViewMarks.Rows[dataGridViewMarks.RightClickRow].IsNewRow == false;
                var check = (DataGridViewCheckBoxCell)dataGridViewMarks.Rows[dataGridViewMarks.RightClickRow].Cells[5];
                sendToCompassToolStripMenuItem.Enabled = check.Value != null && (bool)(check.Value) == true && !dataGridViewMarks.Rows[dataGridViewMarks.RightClickRow].Cells[1].IsNullOrEmpty();
            }
        }

        private void addPlanetManuallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            int width = 430;
            int ctrlleft = 150;

            f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "Planet:".T(EDTx.SurfaceBookmarkUserControl_PL), new Point(10, 40), new Size(140, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("Planet", typeof(ExtendedControls.ExtTextBox),
                "", new Point(ctrlleft, 40), new Size(width - ctrlleft - 20, 24), "Enter planet name".T(EDTx.SurfaceBookmarkUserControl_EPN)));

            f.AddOK(new Point(width - 100, 70), "Press to Accept".T(EDTx.SurfaceBookmarkUserControl_PresstoAccept));
            f.AddCancel(new Point(width - 200, 70), "Press to Cancel".T(EDTx.SurfaceBookmarkUserControl_PresstoCancel));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "OK")
                {
                    f.ReturnResult(DialogResult.OK);
                }
                else if (controlname == "Cancel" || controlname == "Close" )
                {
                    f.ReturnResult(DialogResult.Cancel);
                }
            };

            DialogResult res = f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "Manually Add Planet".T(EDTx.SurfaceBookmarkUserControl_MAP), closeicon:true);

            string pname = f.Get("Planet");
            if (res == DialogResult.OK && pname.HasChars())
            {
                if ( !BodyName.Items.Contains(pname))
                    BodyName.Items.Add(pname);
            }
        }


        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewMarks.RightClickRow >= 0)
            {
                DataGridViewRow rw = dataGridViewMarks.Rows[dataGridViewMarks.RightClickRow];

                if (rw.Tag != null)
                {
                    planetmarks.DeleteLocation(rw.Cells[0].Value.ToString(), ((Location)rw.Tag).Name);
                    dataGridViewMarks.Rows.RemoveAt(dataGridViewMarks.RightClickRow);
                    Edited = true;
                    Changed?.Invoke(planetmarks);
                }
                else if (!rw.IsNewRow)
                    dataGridViewMarks.Rows.RemoveAt(dataGridViewMarks.RightClickRow); // its not a saved one, just a spurious one..
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

        private void dataGridViewMarks_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Tag != null)
            {
                planetmarks.DeleteLocation(e.Row.Cells[0].Value.ToString(), ((Location)e.Row.Tag).Name);
                Edited = true;
                Changed?.Invoke(planetmarks);
            }
        }

        private void dataGridViewMarks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow rw = dataGridViewMarks.Rows[e.RowIndex];
            bool valid = ValidRow(rw);
            ((DataGridViewCheckBoxCell)rw.Cells[5]).Value = valid;

            if (valid)
            {
                Edited = true;
                System.Diagnostics.Debug.WriteLine("Commit " + rw.Cells[0].Value.ToString());

                Location newLoc = rw.Tag != null ? (Location)rw.Tag : new Location();

                newLoc.Name = rw.Cells[1].IsNullOrEmpty() ? "" : rw.Cells[1].Value.ToString();      // planet bookmarks not required to set name
                newLoc.Comment = rw.Cells[2].IsNullOrEmpty() ? "" : rw.Cells[2].Value.ToString(); // not required to set descr.
                if (rw.Cells[3].IsNullOrEmpty() || !double.TryParse(rw.Cells[3].Value.ToString(), out newLoc.Latitude))        // planet bookmarks not required to set pos
                    newLoc.Latitude = 0;
                if (rw.Cells[4].IsNullOrEmpty() || !double.TryParse(rw.Cells[4].Value.ToString(), out newLoc.Longitude))
                    newLoc.Longitude = 0;

                planetmarks.AddOrUpdateLocation(rw.Cells[0].Value.ToString(), newLoc);
                rw.Tag = newLoc;
                rw.Cells[0].ReadOnly = true;    // can't change the planet.  Can change the name as we are directly editing the loc in memory
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
                    if (s == "-")
                    {

                    }
                    else
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
        }

        private bool ValidRow(DataGridViewRow dr)
        {
            if (dr.Cells[0].IsNullOrEmpty() || dr.Cells[1].IsNullOrEmpty())      // must have a planet and a description
                return false;

            if (!dr.Cells[1].IsNullOrEmpty() && dr.Cells[3].IsNullOrEmpty() && dr.Cells[4].IsNullOrEmpty())     // if both empty, its a planet wide bookmark
                return true;

            if (dr.Cells[3].IsNullOrEmpty() || dr.Cells[4].IsNullOrEmpty())     // if either empty, invalid
                return false;

            if (!Double.TryParse(dr.Cells[3].Value.ToString(), out double lat)) // else both must decode
                return false;

            if (!Double.TryParse(dr.Cells[4].Value.ToString(), out double lon))
                return false;

            return lat >= -90 && lat <= 90 && lon >= -180 && lon <= 180;      // and be valid
        }

        // if the bookmark contains a planet no longer there, we get this, so ignore the error
        private void dataGridViewMarks_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;

        }

        #endregion

    }
}
