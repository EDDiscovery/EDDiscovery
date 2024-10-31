/*
 * Copyright © 2019-2024 EDDiscovery development team
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
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.UserControls
{
    public partial class SurfaceBookmarkUserControl : UserControl
    {
        public bool Edited = false;         // set if edit taken
        public PlanetMarks PlanetMarks { get { return planetmarks; } }      // current set..

        public string TagFilter {get;set;}

        public Action<PlanetMarks> Changed;     // if a change to data is made
        public Action<string, string, double, double> CompassSelected; // when compass is clicked

        private PlanetMarks planetmarks;
        private Timer searchtimer;
        const int TagSize = 24;

        #region Initialisation

        public SurfaceBookmarkUserControl()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.None;  // don't double scale

            var enumlist = new Enum[] { EDTx.SurfaceBookmarkUserControl_BodyName, EDTx.SurfaceBookmarkUserControl_SurfaceName, 
                                    EDTx.SurfaceBookmarkUserControl_SurfaceDesc, EDTx.SurfaceBookmarkUserControl_Latitude, 
                                    EDTx.SurfaceBookmarkUserControl_Longitude, EDTx.SurfaceBookmarkUserControl_Valid, 
                                    EDTx.SurfaceBookmarkUserControl_labelSurface, EDTx.SurfaceBookmarkUserControl_ColTags, EDTx.SurfaceBookmarkUserControl_labelSearch };
            var enumlistcms = new Enum[] { EDTx.SurfaceBookmarkUserControl_sendToCompassToolStripMenuItem, 
                                EDTx.SurfaceBookmarkUserControl_deleteToolStripMenuItem, 
                                EDTx.SurfaceBookmarkUserControl_addPlanetManuallyToolStripMenuItem };
            var enumlisttt = new Enum[] { EDTx.SurfaceBookmarkUserControl_textBoxFilter_ToolTip, EDTx.SurfaceBookmarkUserControl_buttonFilter_ToolTip }; ;

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;
        }

        // we do this in an async task, as the FindSystemAsync will go away for a while
        // when it returns, we can then fill the grid if required with the right body name set.

        public async void Display(string systemName, HistoryList helist, Func<bool> IsClosed, PlanetMarks pm = null)
        {
            Edited = false;

            planetmarks = pm != null ? pm : new PlanetMarks();

            dataGridView.CancelEdit();
            dataGridView.Rows.Clear();
            dataGridView.Enabled = true;
            sendToCompassToolStripMenuItem.Enabled = false;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = 
                dataGridView.RowHeadersDefaultCellStyle.BackColor = ExtendedControls.Theme.Current.GridBorderBack;

            //System.Diagnostics.Debug.WriteLine($"Surface UC Lookup for planets '{systemName}'");
            var lookup = await helist.StarScan.FindSystemAsync(new SystemClass(systemName), EliteDangerousCore.WebExternalDataLookup.SpanshThenEDSM);

            if (IsClosed())
                return;

            BodyName.Items.Clear();

            // lets present all, even if not landable, as you may want a whole planet bookmark
            var bodies = lookup?.Bodies()?.Where(c=>c.NodeType==StarScan.ScanNodeType.body).Select(b => b.FullName.ReplaceIfStartsWith(systemName) + (b.CustomName != null ? " " + b.CustomName : ""));

            foreach (string s in bodies.EmptyIfNull())
            {
                if (s.HasChars())
                {
                    //System.Diagnostics.Debug.WriteLine($"Surface UC Add bookmark landable '{s}'");
                    BodyName.Items.Add(s);
                }
            }

            if (PlanetMarks?.Planets != null)
            {
                foreach (PlanetMarks.Planet pl in PlanetMarks.Planets)
                {
                    foreach (PlanetMarks.Location loc in pl.Locations)
                    {
                        //System.Diagnostics.Debug.WriteLine($"Surface UC Add row {pl.Name} {loc.Name} {loc.Longitude} {loc.Latitude} {loc.Tags}");

                        using (DataGridViewRow dr = dataGridView.Rows[dataGridView.Rows.Add()])
                        {
                            if (!BodyName.Items.Contains(pl.Name))      // ensure planet in collection so we don't get errors..
                                BodyName.Items.Add(pl.Name);

                            dr.Cells[BodyName.Index].Value = pl.Name;
                            dr.Cells[BodyName.Index].ReadOnly = true;
                            dr.Cells[SurfaceName.Index].Value = loc.Name;
                            dr.Cells[SurfaceDesc.Index].Value = loc.Comment;

                            if (loc.IsWholePlanetBookmark)              // whole planet gets empty lat/long
                            {
                                dr.Cells[Latitude.Index].Value = dr.Cells[Longitude.Index].Value = "";
                            }
                            else
                            {
                                dr.Cells[Latitude.Index].Value = loc.Latitude.ToString("F4");
                                dr.Cells[Longitude.Index].Value = loc.Longitude.ToString("F4");
                            }

                            dr.Cells[ColTags.Index].Value = "";
                            dr.Cells[ColTags.Index].Tag = loc.Tags;
                            var taglist = EDDConfig.BookmarkTagArray(loc.Tags);
                            dr.Cells[ColTags.Index].ToolTipText = string.Join(Environment.NewLine, taglist);
                            TagsForm.SetMinHeight(taglist.Length, dr, ColTags.Width, TagSize);

                            ((DataGridViewCheckBoxCell)dr.Cells[Valid.Index]).Value = true;
                                
                            dr.Tag = loc;

                            dr.Visible = textBoxFilter.Text.Length > 0 ? dr.IsTextInRow(textBoxFilter.Text) : true;
                        }
                    }
                }

                if (textBoxFilter.Text.HasChars() || TagFilter != ExtendedControls.CheckedIconUserControl.All)
                    FilterView();
            }
        }

        private void dataGridViewMarks_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewRow rw = dataGridView.Rows[e.RowIndex];
            var tags = rw.Cells[ColTags.Index].Tag as string;        // last editing row does not have tagslist
            if (tags != null)
            {
                var taglist = EDDConfig.BookmarkTagArray(tags);
                TagsForm.PaintTags(taglist, EDDConfig.Instance.BookmarkTagDictionary,
                        dataGridView.GetCellDisplayRectangle(ColTags.Index, rw.Index, false), e.Graphics, TagSize);
            }
        }   

        public void Disable()
        {
            Edited = false;
            planetmarks = null;
            dataGridView.CancelEdit();
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = dataGridView.RowHeadersDefaultCellStyle.BackColor = ExtendedControls.Theme.Current.GridBorderBack.Multiply(0.5F);
            dataGridView.Rows.Clear();
            dataGridView.Enabled = false;
        }

        public void AddSurfaceLocation(string planet, double latitude, double longitude)    // call After Reset to add a surface bookmark
        {
            using (DataGridViewRow dr = dataGridView.Rows[dataGridView.Rows.Add()])
            {
                if (!BodyName.Items.Contains(planet))
                    BodyName.Items.Add(planet);

                dr.Cells[BodyName.Index].Value = planet;
                dr.Cells[BodyName.Index].ReadOnly = true;
                dr.Cells[SurfaceName.Index].Value = "Enter a name".T(EDTx.SurfaceBookmarkUserControl_Enter);
                dr.Cells[SurfaceDesc.Index].Value = "";
                dr.Cells[Latitude.Index].Value = latitude.ToString("F4");
                dr.Cells[Longitude.Index].Value = longitude.ToString("F4");
                dr.Cells[ColTags.Index].Value = "";     // no tags
                dr.Cells[ColTags.Index].Tag = "";
                ((DataGridViewCheckBoxCell)dr.Cells[Valid.Index]).Value = false;
                dr.Cells[SurfaceName.Index].Selected = true;
            }
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            searchtimer.Stop();
            searchtimer.Start();
        }

        private void Searchtimer_Tick(object sender, EventArgs e)
        {
            searchtimer.Stop();
            dataGridView.CancelEdit();
            FilterView();
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            string curtags = TagsForm.UniqueTags(GlobalBookMarkList.Instance.GetAllTags(true), EDDConfig.TagSplitStringBK);
            TagsForm.EditTags(this.FindForm(),
                                        EDDConfig.Instance.BookmarkTagDictionary, curtags, TagFilter,
                                        buttonFilter.PointToScreen(new System.Drawing.Point(0, buttonFilter.Height)),
                                        TagFilterChanged, null, EDDConfig.TagSplitStringBK,
                                        true,// we want ALL back to include everything in case we don't know the tag (due to it being removed)
                                        true);          // and we want Or/empty
        }

        private void TagFilterChanged(string newtags, Object tag)
        {
            TagFilter = newtags;
            FilterView();
        }

        private void FilterView()
        {
            dataGridView.FilterGridView((row) => row.IsTextInRow(textBoxFilter.Text) && TagsForm.AreTagsInFilter(row.Cells[ColTags.Index].Tag, TagFilter, EDDConfig.TagSplitStringBK));
        }


        #endregion

        #region Grid UI

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)    // row -1 is the header..
            {
                DataGridViewRow rw = dataGridView.Rows[e.RowIndex];
                if (e.ColumnIndex == ColTags.Index)
                {
                    string taglist = (rw.Cells[ColTags.Index].Tag as string) ?? ""; // may be new column, in which case it will be null

                    TagsForm.EditTags(this.FindForm(),
                                                EDDConfig.Instance.BookmarkTagDictionary, taglist , taglist,
                                                dataGridView.PointToScreen(dataGridView.GetCellDisplayRectangle(ColTags.Index, rw.Index, false).Location),
                                                TagsChanged, rw, EDDConfig.TagSplitStringBK);

                }

            }
        }

        private void TagsChanged(string newtags, Object tag)
        {
            DataGridViewRow rw = tag as DataGridViewRow;
            rw.Cells[ColTags.Index].Tag = newtags;
            var taglist = EDDConfig.BookmarkTagArray(newtags);
            rw.Cells[ColTags.Index].ToolTipText = string.Join(Environment.NewLine, taglist);
            TagsForm.SetMinHeight(taglist.Length, rw, ColTags.Width, TagSize);
            dataGridView.InvalidateRow(rw.Index);
            Store(rw);
        }

        private void dataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column == ColTags)
            {
                foreach (DataGridViewRow rw in dataGridView.Rows)
                {
                    string tags = rw.Cells[ColTags.Index].Tag as string;
                    if ( tags!= null)       // last row does not have tags
                    {
                        var taglist = EDDConfig.BookmarkTagArray(tags);
                        TagsForm.SetMinHeight(taglist.Length, rw, ColTags.Width, TagSize);
                    }
                }
            }
        }

        private void dataGridViewMarks_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Tag != null)
            {
                planetmarks.DeleteLocation(e.Row.Cells[BodyName.Index].Value.ToString(), ((PlanetMarks.Location)e.Row.Tag).Name);
                Edited = true;
                Changed?.Invoke(planetmarks);
            }
        }

        private void dataGridViewMarks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow rw = dataGridView.Rows[e.RowIndex];
            Store(rw);
        }

        private void dataGridViewMarks_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == Latitude.Index || e.ColumnIndex == Longitude.Index)       
            {
                string s = e.FormattedValue.ToString();

                dataGridView.Rows[e.RowIndex].ErrorText = "";

                if (s.Length > 0)
                {
                    if (s == "-")
                    {
                    }
                    else
                    {
                        double? v = s.ParseDoubleNull();
                        double lm = e.ColumnIndex == Latitude.Index ? 90 : 180;

                        if (v == null || v.Value < -lm || v.Value > lm)
                        {
                            e.Cancel = true;
                            dataGridView.Rows[e.RowIndex].ErrorText = "Invalid coordinate";
                        }
                    }
                }
            }
        }

        // try and store rw as long as its valid
        private void Store(DataGridViewRow rw)
        {
            var valid = ValidRow(rw, out double lat, out double lon);

            ((DataGridViewCheckBoxCell)rw.Cells[Valid.Index]).Value = valid != RowValidity.Invalid;

            if (valid != RowValidity.Invalid)
            {
                System.Diagnostics.Debug.WriteLine("Surface UC Commit " + rw.Cells[BodyName.Index].Value.ToString());
                Edited = true;
                var newLoc = rw.Tag != null ? (PlanetMarks.Location)rw.Tag : new PlanetMarks.Location();
                newLoc.Name = rw.Cells[SurfaceName.Index].IsNullOrEmpty() ? "" : rw.Cells[SurfaceName.Index].Value.ToString();      // planet bookmarks not required to set name
                newLoc.Comment = rw.Cells[SurfaceDesc.Index].IsNullOrEmpty() ? "" : rw.Cells[SurfaceDesc.Index].Value.ToString(); // not required to set descr.
                newLoc.Latitude = lat;
                newLoc.Longitude = lon;
                newLoc.Tags = (rw.Cells[ColTags.Index].Tag as string) ?? "";
                planetmarks.AddOrUpdateLocation(rw.Cells[BodyName.Index].Value.ToString(), newLoc);
                rw.Tag = newLoc;
                rw.Cells[BodyName.Index].ReadOnly = true;    // can't change the planet.  Can change the name as we are directly editing the loc in memory
                Changed?.Invoke(planetmarks);
            }
        }

        // check validity, and return lat lon
        enum RowValidity { Invalid, ValidNoLatLong, ValidWithLatLong }
        private RowValidity ValidRow(DataGridViewRow dr, out double lat, out double lon)
        {
            lat = lon = 0;

            if (dr.Cells[BodyName.Index].IsNullOrEmpty() || dr.Cells[SurfaceName.Index].IsNullOrEmpty())      // must have a planet and a description
                return RowValidity.Invalid;

            if (!dr.Cells[SurfaceName.Index].IsNullOrEmpty() && dr.Cells[Latitude.Index].IsNullOrEmpty() && dr.Cells[Longitude.Index].IsNullOrEmpty())     // if both empty, its a planet wide bookmark
                return RowValidity.ValidNoLatLong;

            if (dr.Cells[Latitude.Index].IsNullOrEmpty() || dr.Cells[Longitude.Index].IsNullOrEmpty())     // if either empty, invalid
                return RowValidity.Invalid;

            if (!Double.TryParse(dr.Cells[Latitude.Index].Value.ToString(), out lat)) // else both must decode in current culture
                return RowValidity.Invalid;

            if (!Double.TryParse(dr.Cells[Longitude.Index].Value.ToString(), out lon))
                return RowValidity.Invalid;

            return lat >= -90 && lat <= 90 && lon >= -180 && lon <= 180 ? RowValidity.ValidWithLatLong : RowValidity.Invalid;
        }

        // if the bookmark contains a planet no longer there, we get this, so ignore the error
        private void dataGridViewMarks_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }


        #endregion

        #region Right click UI

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridView.RightClickRow >= 0)
            {
                var rw = dataGridView.Rows[dataGridView.RightClickRow];
                deleteToolStripMenuItem.Enabled = rw.IsNewRow == false;
                sendToCompassToolStripMenuItem.Enabled = CompassSelected != null && ValidRow(rw, out double _1, out double _2) == RowValidity.ValidWithLatLong;
            }
            else
                sendToCompassToolStripMenuItem.Enabled = deleteToolStripMenuItem.Enabled = false;

        }

        private void addPlanetManuallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            int width = 430;
            int ctrlleft = 150;

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("L", typeof(Label), "Planet:".T(EDTx.SurfaceBookmarkUserControl_PL), new Point(10, 40), new Size(140, 24), ""));
            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("Planet", typeof(ExtendedControls.ExtTextBox),
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
            if (dataGridView.RightClickRow >= 0)
            {
                DataGridViewRow rw = dataGridView.Rows[dataGridView.RightClickRow];

                if (rw.Tag != null)
                {
                    planetmarks.DeleteLocation(rw.Cells[BodyName.Index].Value.ToString(), ((PlanetMarks.Location)rw.Tag).Name);
                    dataGridView.Rows.RemoveAt(dataGridView.RightClickRow);
                    Edited = true;
                    Changed?.Invoke(planetmarks);
                }
                else if (!rw.IsNewRow)
                {
                    dataGridView.Rows.RemoveAt(dataGridView.RightClickRow); // its not a saved one, just a spurious one..
                }
            }
        }

        private void sendToCompassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.RightClickRow >= 0)
            {
                DataGridViewRow rw = dataGridView.Rows[dataGridView.RightClickRow];
                ValidRow(rw, out double lat, out double lon);
                System.Diagnostics.Debug.WriteLine($"Send to compass {rw.Cells[BodyName.Index].Value} {rw.Cells[SurfaceName.Index].Value}: {lat} {lon}");
                CompassSelected(rw.Cells[BodyName.Index].Value as string, rw.Cells[SurfaceName.Index].Value as string, lat, lon);
            }
        }




        #endregion

    }
}
