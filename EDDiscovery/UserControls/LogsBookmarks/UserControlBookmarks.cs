using EDDiscovery.Forms;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlBookmarks : UserControlCommonBase
    {
        DataGridViewRow currentedit = null;
        Timer searchtimer;

        #region init
        public UserControlBookmarks()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "UCBookmarks";

            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;
            GlobalBookMarkList.Instance.OnBookmarkChange += BookmarksChanged;

            var enumlist = new Enum[] { EDTx.UserControlBookmarks_ColType, EDTx.UserControlBookmarks_ColBookmarkName, EDTx.UserControlBookmarks_ColDescription, 
                                            EDTx.UserControlBookmarks_labelSearch };
            var enumlistcms = new Enum[] { EDTx.UserControlBookmarks_toolStripMenuItemGotoStar3dmap, EDTx.UserControlBookmarks_openInEDSMToolStripMenuItem };
            var enumlisttt = new Enum[] { EDTx.UserControlBookmarks_textBoxFilter_ToolTip, EDTx.UserControlBookmarks_buttonNew_ToolTip, 
                                            EDTx.UserControlBookmarks_buttonEdit_ToolTip, EDTx.UserControlBookmarks_extButtonEditSystem_ToolTip, 
                                            EDTx.UserControlBookmarks_buttonDelete_ToolTip, EDTx.UserControlBookmarks_buttonExtExcel_ToolTip, 
                                            EDTx.UserControlBookmarks_buttonExtImport_ToolTip,
                                            EDTx.UserControlBookmarks_extButtonNewRegion_ToolTip};
            
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, new Control[] { userControlSurfaceBookmarks });
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStripBookmarks, enumlistcms, this);
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewBookMarks);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewBookMarks);

            SaveBackAnyChanges();

            searchtimer.Dispose();

            GlobalBookMarkList.Instance.OnBookmarkChange -= BookmarksChanged;
        }
        #endregion

        #region display
        public override void InitialDisplay()
        {
            Display();
            userControlSurfaceBookmarks.Changed += ChangeLocations;
            userControlSurfaceBookmarks.CompassSeleted += CompassSelected;
        }

        
        private void Display()
        {
            this.dataGridViewBookMarks.SelectionChanged -= new System.EventHandler(this.dataGridViewBookMarks_SelectionChanged);

            int lastrow = dataGridViewBookMarks.CurrentCell != null ? dataGridViewBookMarks.CurrentCell.RowIndex : -1;

            DataGridViewColumn sortcol = dataGridViewBookMarks.SortedColumn != null ? dataGridViewBookMarks.SortedColumn : dataGridViewBookMarks.Columns[0];
            SortOrder sortorder = dataGridViewBookMarks.SortOrder;

            dataViewScrollerPanel.SuspendLayout();
            dataGridViewBookMarks.SuspendLayout();

            dataGridViewBookMarks.Rows.Clear();
            
            foreach (BookmarkClass bk in GlobalBookMarkList.Instance.Bookmarks)
            {
                //System.Diagnostics.Debug.WriteLine("Bookmark " + bk.Name  +":" + bk.Note);
                var rw = dataGridViewBookMarks.RowTemplate.Clone() as DataGridViewRow;
                rw.CreateCells( dataGridViewBookMarks , bk.isRegion ? "Region" : "System" ,
                    bk.isRegion ? bk.Heading : bk.StarName,
                    bk.Note,
                    bk.x.ToString("0.##"),
                    bk.y.ToString("0.##"),
                    bk.z.ToString("0.##") );
                rw.Tag = bk;

                dataGridViewBookMarks.Rows.Add(rw);
            }

            dataGridViewBookMarks.ResumeLayout();
            dataViewScrollerPanel.ResumeLayout();

            dataGridViewBookMarks.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
            dataGridViewBookMarks.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

            if (lastrow >= 0 && lastrow < dataGridViewBookMarks.Rows.Count)
                dataGridViewBookMarks.SetCurrentAndSelectAllCellsOnRow(Math.Min(lastrow, dataGridViewBookMarks.Rows.Count - 1));

            RefreshCurrentEdit();

            this.dataGridViewBookMarks.SelectionChanged += new System.EventHandler(this.dataGridViewBookMarks_SelectionChanged);
        }

        private void RefreshCurrentEdit()
        {
            if (dataGridViewBookMarks.CurrentCell != null)
            {
                currentedit = dataGridViewBookMarks.Rows[dataGridViewBookMarks.CurrentCell.RowIndex];
                BookmarkClass bk = (BookmarkClass)(currentedit.Tag);
                //System.Diagnostics.Debug.WriteLine("Move to row " + currentedit.Index + " Notes " + bk.Name);
                if (bk.isRegion)
                    userControlSurfaceBookmarks.Disable();
                else
                    userControlSurfaceBookmarks.Init(bk.StarName,discoveryform.history, bk.PlanetaryMarks);
            }
            else
            {
                currentedit = null;
                userControlSurfaceBookmarks.Disable();
            }
        }

        private void SaveBackAnyChanges()
        {
            if (currentedit != null)
            {
                BookmarkClass bk = (BookmarkClass)currentedit.Tag;
                string newNote = "";
                if (null != currentedit.Cells[2].Value)
                {
                    newNote = currentedit.Cells[2].Value.ToString();
                }
                //System.Diagnostics.Debug.WriteLine("Checking for save " + currentedit.Index);

                if (!newNote.Equals(bk.Note) || userControlSurfaceBookmarks.Edited)     // notes or planet marks changed
                {
                    updating = true;
                    //System.Diagnostics.Debug.WriteLine("Save back " + bk.Name + " " + newNote);
                    currentedit.Tag = GlobalBookMarkList.Instance.AddOrUpdateBookmark(bk, !bk.isRegion,
                                    bk.isRegion ? bk.Heading : bk.StarName,
                                    bk.x, bk.y, bk.z, bk.TimeUTC,
                                    newNote,
                                    userControlSurfaceBookmarks.PlanetMarks);
                    updating = false;
                    userControlSurfaceBookmarks.Edited = false;
                }
            }
        }

        private void dataGridViewBookMarks_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 3)
                e.SortDataGridViewColumnNumeric();
        }

        #endregion

        #region UI

        private void buttonNew_Click(object sender, EventArgs e)
        {
            updating = true;
            BookmarkHelpers.ShowBookmarkForm(this.FindForm(), discoveryform, null, null);
            updating = false;
            Display();
        }

        private void extButtonNewRegion_Click(object sender, EventArgs e)
        {
            updating = true;
            BookmarkHelpers.ShowBookmarkForm(this.FindForm(), discoveryform, null, null, true);
            updating = false;
            Display();
        }

        private void extButtonEditSystem_Click(object sender, EventArgs e)
        {
            updating = true;
            BookmarkHelpers.ShowBookmarkForm(this.FindForm(), discoveryform, discoveryform.history.GetLast?.System, null);
            updating = false;
            Display();
        }


        private void dataGridViewBookMarks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            buttonEdit_Click(sender, e);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (currentedit != null)      // if we have a current cell.. 
            {
                BookmarkClass bk = (BookmarkClass)currentedit.Tag;

                SaveBackAnyChanges();
                EliteDangerousCore.ISystem sys = bk.isStar ? SystemCache.FindSystem(bk.Name, discoveryform.galacticMapping, true) : null;

                updating = true;
                BookmarkHelpers.ShowBookmarkForm(this.FindForm(), discoveryform, sys, bk);
                updating = false;
                Display();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int[] rows = null;

            if (dataGridViewBookMarks.SelectedCells.Count > 0)      // being paranoid
            {
                rows = (from DataGridViewCell x in dataGridViewBookMarks.SelectedCells select x.RowIndex).Distinct().ToArray();
            }

            //System.Diagnostics.Debug.WriteLine("cells {0} rows {1} selrows {2}", dataGridViewBookMarks.SelectedCells.Count, dataGridViewBookMarks.SelectedRows.Count , rows.Length);

            if ( rows != null && rows.Length > 1 )
            {
                if (ExtendedControls.MessageBoxTheme.Show(FindForm(), string.Format(("Do you really want to delete {0} bookmarks?" + Environment.NewLine + "Confirm or Cancel").T(EDTx.UserControlBookmarks_CFN), rows.Length), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    updating = true;
                    foreach (int r in rows)
                    {
                        BookmarkClass bk = (BookmarkClass)dataGridViewBookMarks.Rows[r].Tag;
                        //System.Diagnostics.Debug.WriteLine("Delete " + bk.Name);
                        GlobalBookMarkList.Instance.Delete(bk);
                    }
                    updating = false;
                    Display();
                }

            }
            else if (currentedit != null)      // if we have a current cell.. 
            {
                BookmarkClass bk = (BookmarkClass)currentedit.Tag;

                if (ExtendedControls.MessageBoxTheme.Show(FindForm(), string.Format(("Do you really want to delete the bookmark for {0}" + Environment.NewLine + "Confirm or Cancel").T(EDTx.UserControlBookmarks_CF),  bk.Name ), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    updating = true;
                    GlobalBookMarkList.Instance.Delete(bk);
                    updating = false;
                    Display();
                }
            }
        }

        private void dataGridViewBookMarks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 2)
            {
                SaveBackAnyChanges();
            }
        }

        private void dataGridViewBookMarks_SelectionChanged(object sender, EventArgs e)
        {
            SaveBackAnyChanges();
            RefreshCurrentEdit();
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            searchtimer.Stop();
            searchtimer.Start();
        }

        private void Searchtimer_Tick(object sender, EventArgs e)
        {
            searchtimer.Stop();
            this.Cursor = Cursors.WaitCursor;

            SaveBackAnyChanges();

            dataGridViewBookMarks.FilterGridView(textBoxFilter.Text);

            RefreshCurrentEdit();

            this.Cursor = Cursors.Default;
        }


        private void ChangeLocations(PlanetMarks p)     // planetary bits edited.. call back from planetaryform
        {
            SaveBackAnyChanges();
        }

        private void CompassSelected(string planet, string locname)
        {
            if (currentedit != null)      // if we have a current cell.. 
            {
                BookmarkClass bk = (BookmarkClass)currentedit.Tag;

                UserControlCompass comp = (UserControlCompass)discoveryform.PopOuts.PopOut(PanelInformation.PanelIDs.Compass);
                comp.SetSurfaceBookmark(bk, planet, locname);
            }
        }

        #endregion

        #region Reaction to bookmarks doing stuff from outside sources

        bool updating;
        private void BookmarksChanged(BookmarkClass bk, bool deleted)
        {
            //System.Diagnostics.Debug.WriteLine("Changed called " + updating);
            if (updating)
                return;

            // removed this - this can overwrite commanded changes SaveBackAnyChanges();
            Display();
        }

        #endregion

        #region Right clicks

        BookmarkClass rightclickbookmark = null;

        private void dataGridViewBookMarks_MouseDown(object sender, MouseEventArgs e)
        {
            rightclickbookmark = (dataGridViewBookMarks.RightClickRowValid) ? (BookmarkClass)dataGridViewBookMarks.Rows[dataGridViewBookMarks.RightClickRow].Tag : null;
        }

        private void contextMenuStripBookmarks_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolStripMenuItemGotoStar3dmap.Enabled = rightclickbookmark != null;
            openInEDSMToolStripMenuItem.Enabled = rightclickbookmark != null && rightclickbookmark.isStar;
        }

        private void toolStripMenuItemGotoStar3dmap_Click(object sender, EventArgs e)
        {
            discoveryform.Open3DMap(new EliteDangerousCore.SystemClass("Unknown", rightclickbookmark.x, rightclickbookmark.y, rightclickbookmark.z));
        }

        private void openInEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EliteDangerousCore.EDSM.EDSMClass edsm = new EDSMClass();
            
            if (!edsm.ShowSystemInEDSM(rightclickbookmark.StarName))
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable".T(EDTx.UserControlBookmarks_SysU));

            this.Cursor = Cursors.Default;
        }

        #endregion

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            if (dataGridViewBookMarks.Rows.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No data to export", "Export EDSM", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(false, new string[] { "Export Current View" }, showflags: new Forms.ExportForm.ShowFlags[] { Forms.ExportForm.ShowFlags.DisableDateTime });

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (frm.SelectedIndex == 0)
                {
                    string path = frm.Path;               //string path = "C:\\code\\f.csv"; // debug

                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                    grd.SetCSVDelimiter(frm.Comma);

                    List<string> colh = new List<string>();
                    colh.AddRange(new string[] { "Type", "Time", "System/Region", "Note","X","Y", "Z", "Planet", "Name", "Comment", "Lat","Long"});

                    grd.GetHeader += delegate (int c)
                    {
                        return (c < colh.Count && frm.IncludeHeader) ? colh[c] : null;
                    };

                    int bkrowno = 0;
                    IEnumerator<Tuple<PlanetMarks.Planet, PlanetMarks.Location>> planetloc = null;

                    System.Diagnostics.Debug.WriteLine("Rows " + dataGridViewBookMarks.Rows.Count);

                    grd.GetLineStatus += delegate (int r)
                    {
                        return bkrowno < dataGridViewBookMarks.Rows.Count ? BaseUtils.CSVWriteGrid.LineStatus.OK : BaseUtils.CSVWriteGrid.LineStatus.EOF;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        DataGridViewRow rw = dataGridViewBookMarks.Rows[bkrowno];
                        BookmarkClass bk = rw.Tag as BookmarkClass;
                        bool firstplanetrow = false;

                        if (planetloc == null && bk.hasPlanetaryMarks)          // if not iterating planets, but it has one, iterate
                        {
                            planetloc = bk.PlanetaryMarks.GetEnumerator();
                            planetloc.MoveNext();       // move to first
                            firstplanetrow = true;
                        }

                        List<Object> retrow = new List<Object>
                        {
                            bk.isRegion ? "Region" : "System",
                            EDDConfig.Instance.ConvertTimeToSelectedFromUTC(bk.TimeUTC).ToStringYearFirst(),
                            bk.isRegion ? bk.Heading : bk.StarName,
                            bk.Note,
                            bk.x.ToString("0.####"),
                            bk.y.ToString("0.####"),
                            bk.z.ToString("0.####"),
                        };

                        System.Diagnostics.Debug.WriteLine("Export system " + bkrowno + " " + bk.StarName);

                        if (planetloc != null)
                        {
                            var plloc = planetloc.Current;
                            List<Object> planetrow = new List<Object>
                            {
                                plloc.Item1.Name,
                                plloc.Item2.Name,
                                plloc.Item2.Comment,
                                plloc.Item2.IsWholePlanetBookmark ? "" : plloc.Item2.Latitude.ToString("0.##"),
                                plloc.Item2.IsWholePlanetBookmark ? "" : plloc.Item2.Longitude.ToString("0.##"),
                            };

                            if (!firstplanetrow)
                            {
                                retrow = new List<object>() { "", "", "", "", "", "", "" };
                            }

                            retrow.AddRange(planetrow);
                        }

                        if (planetloc == null || planetloc.MoveNext() == false)
                        {
                            planetloc = null;
                            bkrowno++;
                        }

                        return retrow.ToArray();
                    };

                    grd.WriteGrid(path, frm.AutoOpen, FindForm());
                }
            }
        }

        private void buttonExtImport_Click(object sender, EventArgs e)
        {
            var frm = new Forms.ExportForm();

            frm.Init(true, new string[] { "CSV"},
                 new string[] { "CSV|*.csv" },
                 new Forms.ExportForm.ShowFlags[] { Forms.ExportForm.ShowFlags.DTOI });

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                string path = frm.Path;

                BaseUtils.CSVFile csv = new BaseUtils.CSVFile();

                if (csv.Read(path, System.IO.FileShare.ReadWrite, frm.Comma))
                {
                    List<BaseUtils.CSVFile.Row> rows = csv.RowsExcludingHeaderRow;

                    BookmarkClass currentbk = null;

                    var Regexyyyyddmm = new System.Text.RegularExpressions.Regex(@"\d\d\d\d-\d\d-\d\d", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.Singleline);

                    foreach (var r in rows)
                    {
                        string type = r[0];
                        string date = r[1];

                        if (type.HasChars() && date.HasChars())
                        {
                            bool region = type?.Equals("Region", StringComparison.InvariantCultureIgnoreCase) ?? false;

                            DateTime timeutc = DateTime.MinValue;

                            bool isyyyy = Regexyyyyddmm.IsMatch(date);      // excel, after getting our output in, converts the damn thing to local dates.. this distinguishes it.

                            bool success = isyyyy ? DateTime.TryParse(date, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out timeutc) :
                                                                                    DateTime.TryParse(date, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out timeutc);

                            if (success)
                            {
                                timeutc = EDDConfig.Instance.ConvertTimeToUTCFromSelected(timeutc);     // assume import is in selected time base, convert

                                string name = r[2];
                                string note = r[3];
                                double? x = r[4].InvariantParseDoubleNull();
                                double? y = r[5].InvariantParseDoubleNull();
                                double? z = r[6].InvariantParseDoubleNull();

                                if (x != null && y != null && z != null)
                                {
                                    System.Diagnostics.Debug.WriteLine("Bookmark {0} {1} {2} {3} ({4},{5},{6}", type, timeutc.ToStringZulu(), name, note, x, y, z);

                                    currentbk = GlobalBookMarkList.Instance.FindBookmark(name, region);

                                    if (currentbk != null)
                                    {
                                        GlobalBookMarkList.Instance.AddOrUpdateBookmark(currentbk, !region, name, x.Value, y.Value, z.Value, timeutc, note, currentbk.PlanetaryMarks);
                                    }
                                    else
                                        currentbk = GlobalBookMarkList.Instance.AddOrUpdateBookmark(null, !region, name, x.Value, y.Value, z.Value, timeutc, note, null);
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine("Not a system with valid coords {0} {1}", r[0], r[1]);
                                }
                            }
                            else
                                System.Diagnostics.Debug.WriteLine("Rejected due to date {0} {1}", r[0], r[1]);
                        }

                        string planet = r[7];

                        if (planet.HasChars() && currentbk != null)
                        {
                            string locname = r[8];
                            string comment = r[9];
                            double? latitude = r[10].InvariantParseDoubleNull();
                            double? longitude = r[11].InvariantParseDoubleNull();

                            if (!locname.HasChars() && latitude == null && longitude == null) // whole planet bookmark
                            {
                                currentbk.AddOrUpdatePlanetBookmark(planet, comment);
                            }
                            else if (locname.HasChars() && latitude.HasValue && longitude.HasValue)
                            {
                                currentbk.AddOrUpdateLocation(planet, locname, comment, latitude.Value, longitude.Value);
                            }
                        }
                    }

                    PutSetting("ImportExcelFolder", System.IO.Path.GetDirectoryName(path));
                }
                else
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to read " + path, "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

    }
}
