/*
 * Copyright © 2018-2022 EDDiscovery development team
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
        private string dbStartDate = "SD";
        private string dbStartDateOn = "SDOn";
        private string dbEndDate = "ED";
        private string dbEndDateOn = "EDOn";

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
            DBBaseName = "CaptainsLogPanel";

            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;
            GlobalCaptainsLogList.Instance.OnLogEntryChanged += LogChanged;

            var enumlist = new Enum[] { EDTx.CaptainsLogEntries_ColTime, EDTx.CaptainsLogEntries_ColSystem, EDTx.CaptainsLogEntries_ColBodyName, EDTx.CaptainsLogEntries_ColNote, EDTx.CaptainsLogEntries_ColTags, EDTx.CaptainsLogEntries_labelDateStart, EDTx.CaptainsLogEntries_labelEndDate, EDTx.CaptainsLogEntries_labelSearch };
            var enumlistcms = new Enum[] { EDTx.CaptainsLogEntries_toolStripMenuItemGotoStar3dmap, EDTx.CaptainsLogEntries_openInEDSMToolStripMenuItem, EDTx.CaptainsLogEntries_openAScanPanelViewToolStripMenuItem };
            var enumlisttt = new Enum[] { EDTx.CaptainsLogEntries_textBoxFilter_ToolTip, EDTx.CaptainsLogEntries_buttonNew_ToolTip, EDTx.CaptainsLogEntries_buttonDelete_ToolTip, EDTx.CaptainsLogEntries_buttonTags_ToolTip };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, new Control[] { });
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            ColNote.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            dateTimePickerStartDate.Value = GetSetting(dbStartDate, EDDConfig.GameLaunchTimeUTC()).StartOfDay();
            dateTimePickerStartDate.Checked = GetSetting(dbStartDateOn, false);
            dateTimePickerEndDate.Value = GetSetting(dbEndDate, DateTime.UtcNow).EndOfDay();
            dateTimePickerEndDate.Checked = GetSetting(dbEndDateOn, false);
            VerifyDates();
            dateTimePickerStartDate.ValueChanged += (s, e) => { if (!updateprogramatically) Display(); };
            dateTimePickerEndDate.ValueChanged += (s, e) => { if (!updateprogramatically) Display(); };

            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;

        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView);

            PutSetting(dbStartDate, dateTimePickerStartDate.Value);
            PutSetting(dbEndDate, dateTimePickerEndDate.Value);
            PutSetting(dbStartDateOn, dateTimePickerStartDate.Checked);
            PutSetting(dbEndDateOn, dateTimePickerEndDate.Checked);

            searchtimer.Dispose();
            GlobalCaptainsLogList.Instance.OnLogEntryChanged -= LogChanged;

            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }
        #endregion

        #region display

        public override void InitialDisplay()
        {
            Display();
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            VerifyDates();      // if date time mode changes, history change is fired by settings. check date validation
            Display();
        }


        private void Display()
        {
            int lastrow = dataGridView.CurrentCell != null ? dataGridView.CurrentCell.RowIndex : -1;

            DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
            SortOrder sortorder = dataGridView.SortOrder;

            dataViewScrollerPanel.SuspendLayout();
            dataGridView.SuspendLayout();

            dataGridView.Rows.Clear();

            // be paranoid about end/start of day
            DateTime startutc = dateTimePickerStartDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerStartDate.Value.StartOfDay()) : EDDConfig.GameLaunchTimeUTC();
            DateTime endutc = dateTimePickerEndDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerEndDate.Value.EndOfDay()) : EDDConfig.GameEndTimeUTC();
            System.Diagnostics.Debug.WriteLine($"Captains Log display filter {startutc} {endutc}");

            foreach (CaptainsLogClass entry in GlobalCaptainsLogList.Instance.LogEntries)
            {
                if (entry.Commander == EDCommander.CurrentCmdrID)
                {
                    if ( entry.TimeUTC >= startutc && entry.TimeUTC <= endutc)
                    {
                        var rw = dataGridView.RowTemplate.Clone() as DataGridViewRow;
                        rw.CreateCells(dataGridView,
                            EDDConfig.Instance.ConvertTimeToSelectedFromUTC(entry.TimeUTC),
                            entry.SystemName,
                            entry.BodyName,
                            entry.Note,
                            ""
                            );

                        rw.Tag = entry;
                        rw.Cells[0].Tag = entry.TimeUTC;      // column 0 gets time utc
                        //System.Diagnostics.Debug.WriteLine($"Captains Log {rw.Index} date time utc {rw.Cells[0].Tag}");

                        List<string> taglist = entry.Tags?.SplitNoEmptyStrings(';');        // may be null - we do not use all or none note
                        rw.Cells[4].Tag = taglist;
                        if (taglist != null)
                            rw.MinimumHeight = Math.Max(taglist.Count * TagSpacing, MinRowSize);

                        dataGridView.Rows.Add(rw);
                    }
                }
            }

            dataGridView.ResumeLayout();
            dataViewScrollerPanel.ResumeLayout();

            dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
            dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
                 
            if ( textBoxFilter.Text.HasChars() )
                dataGridView.FilterGridView(textBoxFilter.Text, checktags: true);

            if (lastrow >= 0 && lastrow < dataGridView.Rows.Count && dataGridView.Rows[lastrow].Visible)
                dataGridView.SetCurrentAndSelectAllCellsOnRow(Math.Min(lastrow, dataGridView.Rows.Count - 1));
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewRow rw = dataGridView.Rows[e.RowIndex];
            List<string> tagstring = rw.Cells[4].Tag as List<string>;

            if (tagstring != null)
            {
                //System.Diagnostics.Debug.WriteLine("Row " + e.RowIndex + " Tags '" + tagstring.Count + "'");

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
                e.SortDataGridViewColumnTagsAsStringsLists(4);
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

        // click on item
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
                // cell contains a datetime object (dec 22 bug) or maybe a string to be defensive

                string dt = rw.Cells[0].Value is DateTime ? ((DateTime)rw.Cells[0].Value).ToString() : rw.Cells[0].Value as string;

                System.Globalization.DateTimeStyles dts = System.Globalization.DateTimeStyles.AllowWhiteSpaces;     // convert straight no conversion

                if ( dt != null && DateTime.TryParse(dt, System.Globalization.CultureInfo.CurrentCulture, dts, out DateTime datetimeselected) && 
                            EDDConfig.Instance.DateTimeInRangeForGame(datetimeselected))
                {
                    var utc = EDDConfig.Instance.ConvertTimeToUTCFromPicker(datetimeselected);        // go to UTC like a picker
                    rw.Cells[0].Tag = utc;
                    System.Diagnostics.Debug.WriteLine($"Captains Log Edit row {rw.Index} datetime {datetimeselected} -> date time utc {rw.Cells[0].Tag}");
                    StoreRow(rw);
                }
                else
                {
                    ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Bad/Out of Range Date Time format".T(EDTx.CaptainsLogEntries_DTF), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DateTime prevutc = (DateTime)rw.Cells[0].Tag;
                    rw.Cells[0].Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(prevutc);       // note we go back to selected
                }
            }
            else 
            {
                StoreRow(rw);
            }
        }

        private void ClearDates()
        {
            updateprogramatically = true;
            dateTimePickerStartDate.Checked = dateTimePickerEndDate.Checked = false;
            Display();
            updateprogramatically = false;
        }

        private void MakeNew(DateTime selectedtime, string system, string body)
        {
            var rw = dataGridView.RowTemplate.Clone() as DataGridViewRow;

            rw.CreateCells(dataGridView,
                selectedtime,
                system,
                body,
                "",
                ""
             );

            rw.Tag = null;
            rw.Cells[0].Tag = EDDConfig.Instance.ConvertTimeToUTCFromSelected(selectedtime);

            dataGridView.Rows.Insert(0, rw);
            dataGridView.SetCurrentSelOnRow(0, 2);
            StoreRow(rw);
        }

        private void EditNote(DataGridViewRow rw)
        {
            string notes = rw.Cells[3].Value != null ? (string)rw.Cells[3].Value : "";

            string s = ExtendedControls.PromptSingleLine.ShowDialog(this.FindForm(), "Note:".T(EDTx.CaptainsLogEntries_Note), notes,
                            "Enter Note".T(EDTx.CaptainsLogEntries_EnterNote), this.FindForm().Icon, multiline: true, cursoratend: true, widthboxes:400, heightboxes:400);

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

            ExtendedControls.CheckedIconListBoxFormGroup cfs = new ExtendedControls.CheckedIconListBoxFormGroup();
            cfs.CloseBoundaryRegion = new Size(32, 32);
            cfs.AllOrNoneBack = false;      // we want the whole list, makes it easier.
            cfs.SaveSettings += TagsChanged;
            cfs.AddAllNone();
            cfs.AddStandardOption(options);
            cfs.ImageSize = new Size(24, 24);


            List<string> curtags = rw.Cells[4].Tag as List<string>;     // may be null
            string taglist = curtags != null ? string.Join(";", curtags) : "";
            System.Diagnostics.Debug.WriteLine("Cur keys" + curtags);

            Point loc = dataGridView.PointToScreen(dataGridView.GetCellDisplayRectangle(4, rw.Index, false).Location);

            cfs.Show(taglist, loc, this.FindForm(), tag:rw);
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

            if ( rw.Cells[1].IsNullOrEmpty())   // we must have system
                rw.Cells[1].Value = "?";

            if (rw.Cells[2].IsNullOrEmpty())    // and body. User can remove these during editing.
                rw.Cells[2].Value = "?";

            string notes = rw.Cells[3].IsNullOrEmpty() ? "" : (string)rw.Cells[3].Value;
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


        private void VerifyDates()
        {
            updateprogramatically = true;
            if (!EDDConfig.Instance.DateTimeInRangeForGame(dateTimePickerStartDate.Value) || !EDDConfig.Instance.DateTimeInRangeForGame(dateTimePickerEndDate.Value))
            {
                dateTimePickerStartDate.Checked = dateTimePickerEndDate.Checked = false;
                dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(EDDConfig.GameLaunchTimeUTC());
                dateTimePickerEndDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow.EndOfDay());
            }
            updateprogramatically = false;
        }

        #endregion

        #region Interactions with other tabs

        public void SelectDate(DateTime datestartutc, DateTime dateendutc, bool createnew)
        {
            System.Diagnostics.Debug.WriteLine($"Selected date range {datestartutc}-{dateendutc}");

            updateprogramatically = true;

            dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(datestartutc);      // will assert if not utc
            dateTimePickerEndDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(dateendutc);
            dateTimePickerEndDate.Checked = dateTimePickerStartDate.Checked = true;

            updateprogramatically = false;
            Display();

            if (createnew)
            {
                MakeNew(EDDConfig.Instance.ConvertTimeToSelectedFromUTC(datestartutc).StartOfDay(), "?", "?");
            }
        }

        #endregion

        #region UI

        private void buttonNew_Click(object nu1, EventArgs nu2)
        {
            ClearDates();
            HistoryEntry he = DiscoveryForm.history.GetLast;
            MakeNew(EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow), he?.System.Name ?? "?", he?.WhereAmI ?? "?");
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
                if (ExtendedControls.MessageBoxTheme.Show(FindForm(), string.Format(("Do you really want to delete {0} notes?" + Environment.NewLine + "Confirm or Cancel").T(EDTx.CaptainsLogEntries_CFN), rows.Length), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
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

                    if (ExtendedControls.MessageBoxTheme.Show(FindForm(), string.Format(("Do you really want to delete the note for {0}" + Environment.NewLine + "Confirm or Cancel").T(EDTx.CaptainsLogEntries_CF), entry.SystemName + ":" + entry.BodyName), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
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
            tg.Init("Set Tags".T(EDTx.CaptainsLogEntries_SetTags), this.FindForm().Icon, EDDConfig.Instance.CaptainsLogTagImage);

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
            rightclickentry = dataGridView.RightClickRowValid ? (CaptainsLogClass)dataGridView.Rows[dataGridView.RightClickRow].Tag : null;
        }

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolStripMenuItemGotoStar3dmap.Enabled = rightclickentry != null;
            openInEDSMToolStripMenuItem.Enabled = rightclickentry != null;
            openAScanPanelViewToolStripMenuItem.Enabled = rightclickentry != null;
        }

        private void toolStripMenuItemGotoStar3dmap_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.ISystem s = SystemCache.FindSystem(rightclickentry.SystemName, DiscoveryForm.galacticMapping, true);
            DiscoveryForm.Open3DMap(s);
        }

        private void openInEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EliteDangerousCore.EDSM.EDSMClass edsm = new EDSMClass();
            
            if (!edsm.ShowSystemInEDSM(rightclickentry.SystemName))
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable".T(EDTx.CaptainsLogEntries_SysU));

            this.Cursor = Cursors.Default;
        }

        private void openAScanPanelViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISystem sys = SystemCache.FindSystem(rightclickentry.SystemName, DiscoveryForm.galacticMapping, true);

            if ( sys != null )
                ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), sys, true, DiscoveryForm.history);
            else
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "No such system".T(EDTx.CaptainsLogEntries_NSS) + " " + rightclickentry.SystemName, "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void extButtonExcel_Click(object sender, EventArgs e)
        {
            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(false, new string[] { "Export Current View", "All" }, showflags: new Forms.ExportForm.ShowFlags[] { Forms.ExportForm.ShowFlags.DisableDateTime, Forms.ExportForm.ShowFlags.DisableDateTime });

            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                grd.SetCSVDelimiter(frm.Comma);

                grd.GetLineHeader += delegate (int c)
                {
                    if (c == 0)
                        return new string[] { "Time", "System","Body","Note","Tags" };
                    else
                        return null;
                };

                if (frm.SelectedIndex == 1)
                {
                    List<CaptainsLogClass> logs = GlobalCaptainsLogList.Instance.LogEntries;
                    int i = 0;

                    grd.GetLine += delegate (int r)
                    {
                        while (i < logs.Count)
                        {
                            CaptainsLogClass ent = logs[i++];
                            if (ent.Commander == EDCommander.CurrentCmdrID)
                            {
                                return new object[] { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ent.TimeUTC),
                                                      ent.SystemName , ent.BodyName, ent.Note, ent.Tags };
                            }
                        }

                        return null;
                    };
                }
                else
                {
                    grd.GetLine += delegate (int r)
                    {
                        if (r < dataGridView.RowCount)
                        {
                            DataGridViewRow rw = dataGridView.Rows[r];
                            CaptainsLogClass ent = rw.Tag as CaptainsLogClass;
                            return new Object[] { rw.Cells[0].Value, rw.Cells[1].Value, rw.Cells[2].Value, rw.Cells[3].Value, ent.Tags};
                        }

                        return null;
                    };

                }

                grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
            }

        }

        #endregion

    }
}
