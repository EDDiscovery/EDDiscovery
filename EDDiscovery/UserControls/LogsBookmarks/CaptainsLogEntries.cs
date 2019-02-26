/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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

using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    // CL UCs use the UCCB template BUT are not directly inserted into the normal panels.. they are inserted into the CL UCCB
    // Make sure DB saving has unique names.. they all share the same displayno.

    public partial class CaptainsLogEntries : UserControlCommonBase
    {
        private string DbColumnSave { get { return DBName("CaptainsLogPanel", "DGVCol"); } }
        private string DbStartDate { get { return DBName("CaptainsLogPanel", "SD"); } }
        private string DbStartDateOn { get { return DBName("CaptainsLogPanel", "SDOn"); } }
        private string DbEndDate { get { return DBName("CaptainsLogPanel", "ED"); } }
        private string DbEndDateOn { get { return DBName("CaptainsLogPanel", "EDOn"); } }

        const int TagHeight = 24;
        const int TagSpacing = 26;
        const int MinRowSize = 24;

        Timer searchtimer;
        bool updateprogramatically;

        #region init
        public CaptainsLogEntries()
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

            ColNote.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            dateTimePickerStartDate.Value = SQLiteConnectionUser.GetSettingDate(DbStartDate, new DateTime(2014, 12, 14));
            dateTimePickerStartDate.Checked = SQLiteConnectionUser.GetSettingBool(DbStartDateOn, false);
            dateTimePickerEndDate.Value = SQLiteConnectionUser.GetSettingDate(DbEndDate, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
            dateTimePickerEndDate.Checked = SQLiteConnectionUser.GetSettingBool(DbEndDateOn, false);
            dateTimePickerStartDate.ValueChanged += (s, e) => { if (!updateprogramatically) Display(); };
            dateTimePickerEndDate.ValueChanged += (s, e) => { if (!updateprogramatically) Display(); };

            discoveryform.OnRefreshCommanders += Discoveryform_OnRefreshCommanders;

        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView, DbColumnSave);

            SQLiteConnectionUser.PutSettingDate(DbStartDate, dateTimePickerStartDate.Value);
            SQLiteConnectionUser.PutSettingDate(DbEndDate, dateTimePickerEndDate.Value);
            SQLiteConnectionUser.PutSettingBool(DbStartDateOn, dateTimePickerStartDate.Checked);
            SQLiteConnectionUser.PutSettingBool(DbEndDateOn, dateTimePickerEndDate.Checked);

            searchtimer.Dispose();
            GlobalCaptainsLogList.Instance.OnLogEntryChanged -= LogChanged;

            discoveryform.OnRefreshCommanders -= Discoveryform_OnRefreshCommanders;
        }
        #endregion

        #region display

        public override void InitialDisplay()
        {
            Display();
        }

        private void Discoveryform_OnRefreshCommanders()
        {
            Display();
        }

        private void Display()
        {
            int lastrow = dataGridView.CurrentCell != null ? dataGridView.CurrentCell.RowIndex : -1;

            DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
            SortOrder sortorder = dataGridView.SortOrder;

            dataGridView.SuspendLayout();

            System.Diagnostics.Debug.WriteLine("Redraw");
            dataGridView.Rows.Clear();

            bool pickstart = dateTimePickerStartDate.Checked;       // Picker is UTC or local dependent on UTC Config selection.. just changes compare against entry
            bool pickend = dateTimePickerEndDate.Checked;
            DateTime pickenddate = dateTimePickerEndDate.Value.EndOfDay();

            bool utc = EDDConfig.Instance.DisplayUTC;

            foreach (CaptainsLogClass entry in GlobalCaptainsLogList.Instance.LogEntries)
            {
                if (entry.Commander == EDCommander.CurrentCmdrID)
                {
                    if ((pickstart == false || entry.Time(utc) >= dateTimePickerStartDate.Value) &&     // >= <= does not care about kind.
                        (pickend == false || entry.Time(utc) <= pickenddate))
                    {
                        //System.Diagnostics.Debug.WriteLine("Bookmark " + bk.Name  +":" + bk.Note);
                        var rw = dataGridView.RowTemplate.Clone() as DataGridViewRow;
                        rw.CreateCells(dataGridView,
                            entry.Time(utc),
                            entry.SystemName,
                            entry.BodyName,
                            entry.Note,
                            ""
                            );

                        rw.Tag = entry;
                        rw.Cells[0].Tag = entry.TimeUTC;      // column 0 gets time utc

                        List<string> taglist = entry.Tags?.SplitNoEmptyStrings(';');        // may be null - we do not use all or none note
                        rw.Cells[4].Tag = taglist;
                        if (taglist != null)
                            rw.MinimumHeight = Math.Max(taglist.Count * TagSpacing, MinRowSize);

                        dataGridView.Rows.Add(rw);
                    }
                }
            }

            dataGridView.ResumeLayout();

            dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
            dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

            if ( textBoxFilter.Text.HasChars() )
                dataGridView.FilterGridView(textBoxFilter.Text, checktags: true);

            if (lastrow >= 0 && lastrow < dataGridView.Rows.Count && dataGridView.Rows[lastrow].Visible)
                dataGridView.CurrentCell = dataGridView.Rows[Math.Min(lastrow, dataGridView.Rows.Count - 1)].Cells[3];
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewRow rw = dataGridView.Rows[e.RowIndex];
            List<string> tagstring = rw.Cells[4].Tag as List<string>;

            if (tagstring != null)
            {
                System.Diagnostics.Debug.WriteLine("Row " + e.RowIndex + " Tags '" + tagstring.Count + "'");

                Rectangle area = dataGridView.GetCellDisplayRectangle(4, rw.Index, false);
                area.Width = 24;
                area.Height = 24;

                int tagscount = 0;
                for (int i = 0; i < tagstring.Count; i++)
                {
                    if (EDDConfig.Instance.CaptainsLogTagImage.ContainsKey(tagstring[i]))
                    {
                        e.Graphics.DrawImage(EDDConfig.Instance.CaptainsLogTagImage[tagstring[i]], area);
                        area.Y += 26;
                        tagscount++;
                    }
                }
            }
        }

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index == 4)
                e.SortDataGridViewColumnTagsAsStringsLists(dataGridView);
        }

        #endregion
        #region Editing

        // keydown on form, see if to edit
        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Cell key down" + e.KeyCode);

            if (e.KeyCode == Keys.Enter && dataGridView.CurrentCell != null )
            {
                DataGridViewRow rw = dataGridView.CurrentCell.OwningRow;

                if (dataGridView.CurrentCell.ColumnIndex == 3)
                {
                    EditNote(rw);
                    e.Handled = true;
                }
                else if (dataGridView.CurrentCell.ColumnIndex == 4)
                {
                    EditTags(rw);
                    e.Handled = true;
                }
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)    // row -1 is the header..
            {
                DataGridViewRow rw = dataGridView.Rows[e.RowIndex];

                if (e.ColumnIndex == 3)
                    EditNote(rw);
                else if (e.ColumnIndex == 4)
                    EditTags(rw);
            }
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow rw = dataGridView.Rows[e.RowIndex];

            if ( e.ColumnIndex == 0 )
            {
                string v = rw.Cells[0].Value as string;

                System.Globalization.DateTimeStyles dts = EDDConfig.Instance.DisplayUTC ?
                    System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal :
                    System.Globalization.DateTimeStyles.AssumeLocal | System.Globalization.DateTimeStyles.AdjustToUniversal;

                if (v!= null && DateTime.TryParse(v, System.Globalization.CultureInfo.CurrentCulture, dts, out DateTime res))
                {
                    rw.Cells[0].Tag = res;
                    StoreRow(rw);
                }
                else
                {
                    ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Bad Date Time format".Tx(this, "DTF"), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DateTime prev = (DateTime)rw.Cells[0].Tag;
                    rw.Cells[0].Value = EDDConfig.Instance.DisplayUTC ? prev : prev.ToLocalTime();
                }
            }
            else if (e.ColumnIndex <= 2 )
            {
                if (rw.Cells[3].Value != null && ((string)rw.Cells[3].Value).Length > 0)
                    StoreRow(rw);
            }
        }

        private void EditNote(DataGridViewRow rw)
        {
            string notes = rw.Cells[3].Value != null ? (string)rw.Cells[3].Value : "";

            string s = ExtendedControls.PromptSingleLine.ShowDialog(this.FindForm(), "Note:".Tx(this), notes,
                            "Enter Note".Tx(this), this.FindForm().Icon, multiline: true, width: 800, vspacing: 400, cursoratend: true);

            if (s != null)
            {
                rw.Cells[3].Value = s;
                StoreRow(rw);
            }
        }



        private void EditTags(DataGridViewRow rw)
        {
            List<string> Dickeys = new List<string>(EDDConfig.Instance.CaptainsLogTagImage.Keys);
            Dickeys.Sort();
            List<Tuple<string,string,Image>> options = (from x in Dickeys select new Tuple<string,string,Image>(x.ToString(),x.ToString(),EDDConfig.Instance.CaptainsLogTagImage[x])).ToList();

            ExtendedControls.CheckedIconListBoxSelectionForm cfs = new ExtendedControls.CheckedIconListBoxSelectionForm();
            cfs.AllOrNoneBack = false;      // we want the whole list, makes it easier.
            cfs.Closing += TagsChanged;
            cfs.AddAllNone();
            cfs.AddStandardOption(options);

            List<string> curtags = rw.Cells[4].Tag as List<string>;     // may be null
            string taglist = curtags != null ? string.Join(";", curtags) : "";
            System.Diagnostics.Debug.WriteLine("Cur keys" + curtags);

            Point loc = dataGridView.PointToScreen(dataGridView.GetCellDisplayRectangle(4, rw.Index, false).Location);

            cfs.Show(taglist, loc, new Size(250, 600), this.FindForm(), tag:rw);
        }

        private void TagsChanged(string newtags, Object tag)
        {
            var slist = newtags.Split(';').ToList();            // ; at end due to definition..
            DataGridViewRow rwtagedited = tag as DataGridViewRow;
            rwtagedited.Cells[4].Tag = slist;
            rwtagedited.MinimumHeight = Math.Max((slist.Count-1) * TagSpacing, MinRowSize);
            StoreRow(rwtagedited);
            dataGridView.InvalidateRow(rwtagedited.Index);
        }

        private void StoreRow( DataGridViewRow rw)
        {
            inupdate = true;
            CaptainsLogClass entry = rw.Tag as CaptainsLogClass;        // may be null

            string notes = rw.Cells[3].Value != null ? (string)rw.Cells[3].Value : "";
            string tags = rw.Cells[4].Tag != null ? string.Join(";", rw.Cells[4].Tag as List<string>) : null;

            CaptainsLogClass cls = GlobalCaptainsLogList.Instance.AddOrUpdate(entry, EDCommander.CurrentCmdrID,
                           rw.Cells[1].Value as string,
                           rw.Cells[2].Value as string,
                           (DateTime)rw.Cells[0].Tag,       // tag is UTC
                           notes,
                           tags);

            rw.Tag = cls;

            inupdate = false;
        }

        #endregion

        #region Interactions with other tabs

        public void SelectDate(DateTime date, bool createnew)       // date is local or utc, dependent on config
        {
            updateprogramatically = true;
            dateTimePickerEndDate.Value = dateTimePickerStartDate.Value = date;
            dateTimePickerEndDate.Checked = dateTimePickerStartDate.Checked = true;

            updateprogramatically = false;
            Display();

            if (createnew)
                buttonNew_Click(null, null);
        }

        #endregion

        #region UI

        private void buttonNew_Click(object nu1, EventArgs nu2)
        {
            HistoryEntry he = discoveryform.history.GetLast;

            DateTime entrytimeutc = DateTime.UtcNow;
            string system = he?.System.Name ?? "?";
            string body = he?.WhereAmI ?? "?";

            if (dateTimePickerEndDate.Checked)      // we are not at the current time..
            {
                entrytimeutc = EDDConfig.Instance.DisplayUTC ? dateTimePickerEndDate.Value : dateTimePickerEndDate.Value.ToUniversalTime();
                system = "?";
                body = "?";
            }

            var rw = dataGridView.RowTemplate.Clone() as DataGridViewRow;

            rw.CreateCells(dataGridView,
                EDDConfig.Instance.DisplayUTC ? entrytimeutc : entrytimeutc.ToLocalTime(),
                system,
                body,
                "",
                ""
             );

            rw.Tag = null;
            rw.Cells[0].Tag = entrytimeutc;      // new ones store the date in here as UTC see StoreNote

            dataGridView.Rows.Insert(0,rw);
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
                    foreach (int r in rows)
                    {
                        CaptainsLogClass entry = (CaptainsLogClass)dataGridView.Rows[r].Tag;

                        if (entry != null)
                            GlobalCaptainsLogList.Instance.Delete(entry);
                    }
                    Display();
                }

            }
            else if ( dataGridView.CurrentCell != null)      // if we have a current cell.. 
            {
                DataGridViewRow rw = dataGridView.CurrentCell.OwningRow;

                if (rw.Tag != null)
                {
                    CaptainsLogClass entry = (CaptainsLogClass)rw.Tag;

                    if (ExtendedControls.MessageBoxTheme.Show(FindForm(), string.Format(("Do you really want to delete the note for {0}" + Environment.NewLine + "Confirm or Cancel").Tx(this, "CF"), entry.SystemName + ":" + entry.BodyName), "Warning".Tx(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        GlobalCaptainsLogList.Instance.Delete(entry);
                        Display();
                    }
                }
                else
                    Display();
            }
        }

        private void buttonTags_Click(object sender, EventArgs e)
        {
            TagsForm tg = new TagsForm();
            tg.Init("Set Tags".Tx(this), this.FindForm().Icon, EDDConfig.Instance.CaptainsLogTagImage);

            if (tg.ShowDialog() == DialogResult.OK)
            {
                EDDConfig.Instance.CaptainsLogTagImage = tg.Result;
            }
        }

        #endregion

        #region Filter

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            searchtimer.Stop();
            searchtimer.Start();
        }

        private void Searchtimer_Tick(object sender, EventArgs e)
        {
            searchtimer.Stop();
            this.Cursor = Cursors.WaitCursor;

            dataGridView.FilterGridView(textBoxFilter.Text, checktags: true);

            this.Cursor = Cursors.Default;
        }

        #endregion

        #region Reaction to bookmarks doing stuff from outside sources

        bool inupdate = false;
        private void LogChanged(CaptainsLogClass bk, bool deleted)
        {
            if (!inupdate)
            {
                if (dataGridView.IsCurrentCellInEditMode)
                    dataGridView.EndEdit();
                Display();
            }
        }

        #endregion

        #region Right clicks

        CaptainsLogClass rightclickentry = null;

        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                rightclickentry = null;
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
                        rightclickentry = (CaptainsLogClass)dataGridView.Rows[hti.RowIndex].Tag;
                    }
                }
            }
        }

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolStripMenuItemGotoStar3dmap.Enabled = rightclickentry != null;
            openInEDSMToolStripMenuItem.Enabled = rightclickentry != null;
            openAScanPanelViewToolStripMenuItem.Enabled = rightclickentry != null;
        }

        private void toolStripMenuItemGotoStar3dmap_Click(object sender, EventArgs e)
        {
            if (!discoveryform.Map.Is3DMapsRunning)            // if not running, click the 3dmap button
                discoveryform.Open3DMap(null);

            if (discoveryform.Map.Is3DMapsRunning)             // double check here! for paranoia.
            {
                EliteDangerousCore.ISystem s = EliteDangerousCore.SystemCache.FindSystem(rightclickentry.SystemName);

                if ( s != null && discoveryform.Map.MoveTo((float)s.X, (float)s.Y, (float)s.Z))
                    discoveryform.Map.Show();
            }
        }

        private void openInEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EliteDangerousCore.EDSM.EDSMClass edsm = new EDSMClass();
            
            if (!edsm.ShowSystemInEDSM(rightclickentry.SystemName, null))
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable".Tx(this,"SysU"));

            this.Cursor = Cursors.Default;
        }

        private void openAScanPanelViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISystem sys = SystemCache.FindSystem(rightclickentry.SystemName);

            if ( sys != null )
                ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), sys, true, discoveryform.history);
            else
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "No such system".Tx(this, "NSS") + " " + rightclickentry.SystemName, "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);

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
