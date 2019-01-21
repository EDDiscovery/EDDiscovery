using EDDiscovery.Forms;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlCaptainsLog : UserControlCommonBase
    {
        DataGridViewRow currentedit = null;

        Timer searchtimer;

        #region init
        public UserControlCaptainsLog()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;
            GlobalCaptainsLogList.Instance.OnLogEntryChanged += LogChanged;

            BaseUtils.Translator.Instance.Translate(this, new Control[] { });
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void Closing()
        {
            SaveBackAnyChanges();

            searchtimer.Dispose();

            GlobalCaptainsLogList.Instance.OnLogEntryChanged -= LogChanged;
        }
        #endregion

        #region display

        public override void InitialDisplay()
        {
            Display();
        }

       
        private void Display()
        {
            this.dataGridView.SelectionChanged -= new System.EventHandler(this.dataGridView_SelectionChanged);

            int lastrow = dataGridView.CurrentCell != null ? dataGridView.CurrentCell.RowIndex : -1;

            DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
            SortOrder sortorder = dataGridView.SortOrder;

            dataGridView.SuspendLayout();

            dataGridView.Rows.Clear();
            
            foreach (CaptainsLogClass entry in GlobalCaptainsLogList.Instance.LogEntries)
            {
                //System.Diagnostics.Debug.WriteLine("Bookmark " + bk.Name  +":" + bk.Note);
                var rw = dataGridView.RowTemplate.Clone() as DataGridViewRow;
                rw.CreateCells( dataGridView , entry.Tags ??"",
                    EDDConfig.Instance.DisplayUTC ? entry.TimeUTC : entry.TimeLocal,
                    entry.SystemName,
                    entry.BodyName,
                    entry.Note
                 );
                rw.Tag = entry;

                dataGridView.Rows.Add(rw);
            }

            dataGridView.ResumeLayout();

            dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
            dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

            if (lastrow >= 0 && lastrow < dataGridView.Rows.Count)
                dataGridView.CurrentCell = dataGridView.Rows[Math.Min(lastrow, dataGridView.Rows.Count - 1)].Cells[2];

            RefreshCurrentEdit();

            this.dataGridView.SelectionChanged += new System.EventHandler(this.dataGridView_SelectionChanged);
        }

        private void RefreshCurrentEdit()
        {
            if (dataGridView.CurrentCell != null)
            {
                currentedit = dataGridView.Rows[dataGridView.CurrentCell.RowIndex];
                System.Diagnostics.Debug.WriteLine("Move to row " + currentedit.Index );
            }
            else
            {
                currentedit = null;
            }
        }

        private void SaveBackAnyChanges()
        {
            if (currentedit != null)
            {
                //CaptainsLogClass bk = (CaptainsLogClass)currentedit.Tag;
                //string newNote = "";
                //if (null != currentedit.Cells[2].Value)
                //{
                //    newNote = currentedit.Cells[2].Value.ToString();
                //}
                ////System.Diagnostics.Debug.WriteLine("Checking for save " + currentedit.Index);

                //if (!newNote.Equals(bk.Note) )     // notes or planet marks changed
                //{
                //    updating = true;
                //    //System.Diagnostics.Debug.WriteLine("Save back " + bk.Name + " " + newNote);
                //    currentedit.Tag = GlobalBookMarkList.Instance.AddOrUpdateBookmark(bk, !bk.isRegion,
                //                    bk.isRegion ? bk.Heading : bk.StarName,
                //                    bk.x, bk.y, bk.z, bk.Time,
                //                    newNote
                //                    );
                //    updating = false;
               // }

                currentedit = null;
            }
        }

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //if (e.Column.Index >= 3)
                //e.SortDataGridViewColumnNumeric();
        }

        #endregion

        #region UI

        private void buttonNew_Click(object sender, EventArgs e)
        {
            updating = true;
            //UserControls.TargetHelpers.showBookmarkForm(this.FindForm(), discoveryform, null, null, false);
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
                //CaptainsLogClass bk = (CaptainsLogClass)currentedit.Tag;

                //SaveBackAnyChanges();
                //EliteDangerousCore.ISystem sys = bk.isStar ? EliteDangerousCore.SystemCache.FindSystem(bk.Name) : null;

                //updating = true;
                //UserControls.TargetHelpers.showBookmarkForm(this.FindForm(), discoveryform, sys, bk, false);
                //updating = false;
                //Display();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int[] rows = null;

            if (dataGridView.SelectedCells.Count > 0)      // being paranoid
            {
                rows = (from DataGridViewCell x in dataGridView.SelectedCells select x.RowIndex).Distinct().ToArray();
            }

            //System.Diagnostics.Debug.WriteLine("cells {0} rows {1} selrows {2}", dataGridViewBookMarks.SelectedCells.Count, dataGridViewBookMarks.SelectedRows.Count , rows.Length);

            if ( rows != null && rows.Length > 1 )
            {
                if (ExtendedControls.MessageBoxTheme.Show(FindForm(), string.Format(("Do you really want to delete {0} notes?" + Environment.NewLine + "Confirm or Cancel").Tx(this,"CFN"), rows.Length), "Warning".Tx(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    updating = true;
                    foreach (int r in rows)
                    {
                        CaptainsLogClass entry = (CaptainsLogClass)dataGridView.Rows[r].Tag;
                        //System.Diagnostics.Debug.WriteLine("Delete " + bk.Name);
                        GlobalCaptainsLogList.Instance.Delete(entry);
                    }
                    updating = false;
                    Display();
                }

            }
            else if (currentedit != null)      // if we have a current cell.. 
            {
                CaptainsLogClass entry = (CaptainsLogClass)currentedit.Tag;

                if (ExtendedControls.MessageBoxTheme.Show(FindForm(), string.Format(("Do you really want to delete the note for {0}" + Environment.NewLine + "Confirm or Cancel").Tx(this,"CF"),  entry.SystemName + ":" + entry.BodyName ), "Warning".Tx(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    updating = true;
                    GlobalCaptainsLogList.Instance.Delete(entry);
                    updating = false;
                    Display();
                }
            }
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 2)
            {
              //  SaveBackAnyChanges();
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
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

            dataGridView.FilterGridView(textBoxFilter.Text);

            RefreshCurrentEdit();

            this.Cursor = Cursors.Default;
        }

        #endregion

        #region Reaction to bookmarks doing stuff from outside sources

        bool updating;
        private void LogChanged(CaptainsLogClass bk, bool deleted)
        {
            //System.Diagnostics.Debug.WriteLine("Changed called " + updating);
            if (updating)
                return;

            // removed this - this can overwrite commanded changes SaveBackAnyChanges();
            Display();
        }

        #endregion

        #region Right clicks

        CaptainsLogClass rightclickbookmark = null;

        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                rightclickbookmark = null;
            }

            if (dataGridView.SelectedCells.Count < 2 || dataGridView.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
            {
                DataGridView.HitTestInfo hti = dataGridView.HitTest(e.X, e.Y);
                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    dataGridView.ClearSelection();                // select row under cursor.
                    dataGridView.Rows[hti.RowIndex].Selected = true;

                    if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
                    {
                        rightclickbookmark = (CaptainsLogClass)dataGridView.Rows[hti.RowIndex].Tag;
                    }
                }
            }
        }

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolStripMenuItemGotoStar3dmap.Enabled = rightclickbookmark != null;
            openInEDSMToolStripMenuItem.Enabled = rightclickbookmark != null;
        }

        private void toolStripMenuItemGotoStar3dmap_Click(object sender, EventArgs e)
        {
            if (!discoveryform.Map.Is3DMapsRunning)            // if not running, click the 3dmap button
                discoveryform.Open3DMap(null);

            if (discoveryform.Map.Is3DMapsRunning)             // double check here! for paranoia.
            {
                EliteDangerousCore.ISystem s = EliteDangerousCore.SystemCache.FindSystem(rightclickbookmark.SystemName);

                if ( s != null && discoveryform.Map.MoveTo((float)s.X, (float)s.Y, (float)s.Z))
                    discoveryform.Map.Show();
            }
        }

        private void openInEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EliteDangerousCore.EDSM.EDSMClass edsm = new EDSMClass();
            
            if (!edsm.ShowSystemInEDSM(rightclickbookmark.SystemName, null))
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable".Tx(this,"SysU"));

            this.Cursor = Cursors.Default;
        }

        #endregion

        //private void buttonExtExcel_Click(object sender, EventArgs e)
        //{
        //    if (dataGridViewBookMarks.Rows.Count == 0)
        //    {
        //        ExtendedControls.MessageBoxTheme.Show(FindForm(), "No data to export", "Export EDSM", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //    }

        //    Forms.ExportForm frm = new Forms.ExportForm();
        //    frm.Init(new string[] { "Export Current View" }, disablestartendtime: true);

        //    if (frm.ShowDialog(FindForm()) == DialogResult.OK)
        //    {
        //        if (frm.SelectedIndex == 0)
        //        {
        //            string path = frm.Path;               //string path = "C:\\code\\f.csv"; // debug

        //            BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
        //            grd.SetCSVDelimiter(frm.Comma);

        //            List<string> colh = new List<string>();
        //            colh.AddRange(new string[] { "Type", "Time", "System/Region", "Note","X","Y", "Z", "Planet", "Name", "Comment", "Lat","Long"});

        //            grd.GetHeader += delegate (int c)
        //            {
        //                return (c < colh.Count && frm.IncludeHeader) ? colh[c] : null;
        //            };

        //            int bkrowno = 0;
        //            IEnumerator<Tuple<PlanetMarks.Planet, PlanetMarks.Location>> planetloc = null;

        //            System.Diagnostics.Debug.WriteLine("Rows " + dataGridViewBookMarks.Rows.Count);

        //            grd.GetLineStatus += delegate (int r)
        //            {
        //                return bkrowno < dataGridViewBookMarks.Rows.Count ? BaseUtils.CSVWriteGrid.LineStatus.OK : BaseUtils.CSVWriteGrid.LineStatus.EOF;
        //            };

        //            grd.GetLine += delegate (int r)
        //            {
        //                DataGridViewRow rw = dataGridViewBookMarks.Rows[bkrowno];
        //                CaptainsLogClass bk = rw.Tag as CaptainsLogClass;
        //                bool firstplanetrow = false;

        //                if (planetloc == null && bk.hasPlanetaryMarks)          // if not iterating planets, but it has one, iterate
        //                {
        //                    planetloc = bk.PlanetaryMarks.GetEnumerator();
        //                    planetloc.MoveNext();       // move to first
        //                    firstplanetrow = true;
        //                }

        //                List<Object> retrow = new List<Object>
        //                {
        //                    bk.isRegion ? "Region" : "System",
        //                    bk.Time.ToStringYearFirst(),
        //                    bk.isRegion ? bk.Heading : bk.StarName,
        //                    bk.Note,
        //                    bk.x.ToString("0.##"),
        //                    bk.y.ToString("0.##"),
        //                    bk.z.ToString("0.##"),
        //                };

        //                System.Diagnostics.Debug.WriteLine("Export system " + bkrowno + " " + bk.StarName);

        //                if (planetloc != null)
        //                {
        //                    var plloc = planetloc.Current;
        //                    List<Object> planetrow = new List<Object>
        //                    {
        //                        plloc.Item1.Name,
        //                        plloc.Item2.Name,
        //                        plloc.Item2.Comment,
        //                        plloc.Item2.IsWholePlanetBookmark ? "" : plloc.Item2.Latitude.ToString("0.##"),
        //                        plloc.Item2.IsWholePlanetBookmark ? "" : plloc.Item2.Longitude.ToString("0.##"),
        //                    };

        //                    if (!firstplanetrow)
        //                    {
        //                        retrow = new List<object>() { "", "", "", "", "", "", "" };
        //                    }

        //                    retrow.AddRange(planetrow);
        //                }

        //                if (planetloc == null || planetloc.MoveNext() == false)
        //                {
        //                    planetloc = null;
        //                    bkrowno++;
        //                }

        //                return retrow.ToArray();
        //            };

        //            if (grd.WriteCSV(path))
        //            {
        //                if (frm.AutoOpen)
        //                    System.Diagnostics.Process.Start(path);
        //            }
        //            else
        //                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to write to " + path, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        }
        //    }
        //}

        //private void buttonExtImport_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog dlg = new OpenFileDialog();

        //    dlg.InitialDirectory = SQLiteConnectionUser.GetSettingString("BookmarkFormImportExcelFolder", "c:\\");

        //    if (!System.IO.Directory.Exists(dlg.InitialDirectory))
        //        System.IO.Directory.CreateDirectory(dlg.InitialDirectory);

        //    dlg.DefaultExt = "csv";
        //    dlg.AddExtension = true;
        //    dlg.Filter = "CVS Files (*.csv)|*.csv|All files (*.*)|*.*";

        //    if (dlg.ShowDialog(this) == DialogResult.OK)
        //    {
        //        string path = dlg.FileName;

        //        BaseUtils.CSVFile csv = new BaseUtils.CSVFile();

        //        if ( csv.Read(path, System.IO.FileShare.ReadWrite))
        //        {
        //            List<BaseUtils.CSVFile.Row> rows = csv.RowsExcludingHeaderRow;

        //            CaptainsLogClass currentbk = null;

        //            var Regexyyyyddmm = new System.Text.RegularExpressions.Regex(@"\d\d\d\d-\d\d-\d\d", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.Singleline);

        //            foreach ( var r in rows)
        //            {
        //                string type = r[0];
        //                string date = r[1];

        //                if (type.HasChars() && date.HasChars())
        //                {
        //                    bool region = type?.Equals("Region", StringComparison.InvariantCultureIgnoreCase) ?? false;

        //                    DateTime time = DateTime.MinValue;

        //                    bool isyyyy = Regexyyyyddmm.IsMatch(date);      // excel, after getting our output in, converts the damn thing to local dates.. this distinguishes it.

        //                    bool success = isyyyy ? DateTime.TryParse(date, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out time) :
        //                                                                            DateTime.TryParse(date, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out time);

        //                    if (success)
        //                    {
        //                        string name = r[2];
        //                        string note = r[3];
        //                        double? x = r[4].InvariantParseDoubleNull();
        //                        double? y = r[5].InvariantParseDoubleNull();
        //                        double? z = r[6].InvariantParseDoubleNull();

        //                        if (x != null && y != null && z != null)
        //                        {
        //                            System.Diagnostics.Debug.WriteLine("Bookmark {0} {1} {2} {3} ({4},{5},{6}", type, time.ToStringZulu(), name, note, x, y, z);

        //                            currentbk = GlobalBookMarkList.Instance.FindBookmark(name, region);

        //                            if (currentbk != null)
        //                            {
        //                                GlobalBookMarkList.Instance.AddOrUpdateBookmark(currentbk, !region, name, x.Value, y.Value, z.Value, time, note, currentbk.PlanetaryMarks);
        //                            }
        //                            else
        //                                currentbk = GlobalBookMarkList.Instance.AddOrUpdateBookmark(null, !region, name, x.Value, y.Value, z.Value, time, note, null);
        //                        }
        //                        else
        //                        {
        //                            System.Diagnostics.Debug.WriteLine("Not a system with valid coords {0} {1}", r[0], r[1]);
        //                        }
        //                    }
        //                    else
        //                        System.Diagnostics.Debug.WriteLine("Rejected due to date {0} {1}", r[0], r[1]);
        //                }

        //                string planet = r[7];

        //                if (planet.HasChars() && currentbk != null)
        //                {
        //                    string locname = r[8];
        //                    string comment = r[9];
        //                    double? latitude = r[10].InvariantParseDoubleNull();
        //                    double? longitude = r[11].InvariantParseDoubleNull();

        //                    if (!locname.HasChars() && latitude == null && longitude == null) // whole planet bookmark
        //                    {
        //                        currentbk.AddOrUpdatePlanetBookmark(planet, comment);
        //                    }
        //                    else if (locname.HasChars() && latitude.HasValue && longitude.HasValue)
        //                    {
        //                        currentbk.AddOrUpdateLocation(planet, locname, comment, latitude.Value, longitude.Value);
        //                    }
        //                }
        //            }

        //            SQLiteConnectionUser.PutSettingString("BookmarkFormImportExcelFolder", System.IO.Path.GetDirectoryName(path));
        //        }
        //        else
        //            ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to read " + path, "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    }
        //}
    }
}
