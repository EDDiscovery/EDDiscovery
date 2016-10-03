using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EDDiscovery.DB;
using System.Diagnostics;
using EDDiscovery2;
using EDDiscovery2.DB;
using EDDiscovery2.EDSM;
using System.Threading.Tasks;
using EDDiscovery.Controls;
using System.Threading;
using System.Collections.Concurrent;
using EDDiscovery.EDSM;
using EDDiscovery.EliteDangerous;

namespace EDDiscovery
{
    public partial class TravelHistoryControl : UserControl
    {
        public class TravelHistoryColumns
        {
            public const int Time = 0;
            public const int Icon = 1;
            public const int Description = 2;
            public const int Information = 3;
            public const int Note = 4;

            public const int HistoryTag = Description;      // where the tags are used
            public const int NoteTag = Note;
        }

        public class ClosestSystemsColumns
        {
            public const int SystemName = 0;
        }

        private const string SingleCoordinateFormat = "0.##";

        private static EDDiscoveryForm _discoveryForm;

        DataGridViewRow currentGridRow = null;
        SummaryPopOut summaryPopOut = null;
        List<EDCommander> commanders = null;

        EventFilterSelector cfs = new EventFilterSelector();

        private bool ShutdownEDD = false;
        
        public HistoryEntry CurrentSystemSelected
        {
            get
            {
                if (dataGridViewTravel == null || dataGridViewTravel.CurrentRow == null)
                    return null;
                return ((HistoryEntry)dataGridViewTravel.CurrentRow.Cells[TravelHistoryColumns.HistoryTag].Tag);
            }
        }

        public TravelHistoryControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;

            checkBoxEDSMSyncTo.Checked = SQLiteDBClass.GetSettingBool("EDSMSyncTo", true);
            checkBoxEDSMSyncFrom.Checked = SQLiteDBClass.GetSettingBool("EDSMSyncFrom", true);
            buttonSync.Enabled = checkBoxEDSMSyncTo.Checked | checkBoxEDSMSyncFrom.Checked;

            TravelHistoryFilter.InitaliseComboBox(comboBoxHistoryWindow, "EDUIHistory");
            richTextBoxNote.TextBoxChanged += richTextBoxNote_TextChanged;

            LoadCommandersListBox();

            closestthread = new Thread(CalculateClosestSystems) { Name = "Closest Calc", IsBackground = true };
            closestthread.Start();

            textBoxTarget.SetAutoCompletor(EDDiscovery.DB.SystemClass.ReturnSystemListForAutoComplete);

            cfs.Changed += EventFilterChanged;
        }

        private void TravelHistoryControl_Load(object sender, EventArgs e)
        {
            dataGridViewTravel.MakeDoubleBuffered();
        }

        public void Display()
        {
            int rowno = (dataGridViewTravel.CurrentCell != null) ? dataGridViewTravel.CurrentCell.RowIndex : 0;
            int cellno = (dataGridViewTravel.CurrentCell != null) ? dataGridViewTravel.CurrentCell.ColumnIndex : 0;

            var filter = (TravelHistoryFilter)comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;

            List<HistoryEntry> result = filter.Filter(_discoveryForm.history);

            result = HistoryList.FilterByJournalEvent(result, SQLiteDBClass.GetSettingString("TravelHistoryControlEventFilter", "All"));

            dataGridViewTravel.Rows.Clear();

            for (int ii = 0; ii < result.Count; ii++) //foreach (var item in result)
            {
                AddNewHistoryRow(false, result[ii]);      // for every one in filter, add a row.
            }

            StaticFilters.FilterGridView(dataGridViewTravel, textBoxFilter.Text);

            if (dataGridViewTravel.Rows.Count > 0)
            {
                rowno = Math.Min(rowno, dataGridViewTravel.Rows.Count - 1);
                dataGridViewTravel.CurrentCell = dataGridViewTravel.Rows[rowno].Cells[cellno];       // its the current cell which needs to be set, moves the row marker as well            currentGridRow = (rowno!=-1) ? 
                ShowSystemInformation(dataGridViewTravel.Rows[rowno]);
            }
            else
                currentGridRow = null;

            RedrawSummary();
            RefreshTargetInfo();
            UpdateDependentsWithSelection();
            _discoveryForm.Map.UpdateSystemList(_discoveryForm.history.FilterByFSDAndPosition);           // update map

            dataGridViewTravel.Columns[0].HeaderText = EDDiscoveryForm.EDDConfig.DisplayUTC ? "Game Time" : "Time";
        }

        public void AddNewHistoryRow(bool insert, HistoryEntry item)            // second part of add history row, adds item to view.
        {
            SystemNoteClass snc = SystemNoteClass.GetNoteOnJournalEntry(item.Journalid);
            if ( snc == null && item.IsFSDJump )
                snc = SystemNoteClass.GetNoteOnSystem(item.System.name);

            //string debugt = item.Journalid + "  " + item.System.id_edsm + " " + item.System.GetHashCode() + " "; // add on for debug purposes to a field below

            object[] rowobj = { EDDiscoveryForm.EDDConfig.DisplayUTC ? item.EventTimeUTC : item.EventTimeLocal, "", item.EventSummary, item.EventDescription , (snc != null) ? snc.Note : "" };

            int rownr;
            if (insert)
            {
                dataGridViewTravel.Rows.Insert(0, rowobj);
                rownr = 0;
            }
            else
            {
                dataGridViewTravel.Rows.Add(rowobj);
                rownr = dataGridViewTravel.Rows.Count - 1;
            }

            dataGridViewTravel.Rows[rownr].Cells[TravelHistoryColumns.HistoryTag].Tag = item;
            dataGridViewTravel.Rows[rownr].Cells[TravelHistoryColumns.NoteTag].Tag = snc;

            dataGridViewTravel.Rows[rownr].DefaultCellStyle.ForeColor = (item.System.HasCoordinate || item.EntryType != JournalTypeEnum.FSDJump) ? _discoveryForm.theme.VisitedSystemColor : _discoveryForm.theme.NonVisitedSystemColor;

            string tip = item.EventSummary + Environment.NewLine + item.EventDescription + Environment.NewLine + item.EventDetailedInfo;
            dataGridViewTravel.Rows[rownr].Cells[0].ToolTipText = tip;
            dataGridViewTravel.Rows[rownr].Cells[1].ToolTipText = tip;
            dataGridViewTravel.Rows[rownr].Cells[2].ToolTipText = tip;
            dataGridViewTravel.Rows[rownr].Cells[3].ToolTipText = tip;
            dataGridViewTravel.Rows[rownr].Cells[4].ToolTipText = tip;
        }

        public void AddNewEntry(HistoryEntry he)
        {
            try
            {
                StoreSystemNote();

                if (he.IsFSDJump)
                {
                    int count = _discoveryForm.history.GetVisitsCount(he.System.name);
                    LogLine(string.Format("Arrived at system {0} Visit No. {1}", he.System.name, count));

                    System.Diagnostics.Trace.WriteLine("Arrived at system: " + he.System.name + " " + count + ":th visit.");

                    if (checkBoxEDSMSyncTo.Checked == true)
                        EDSMSync.SendTravelLog(he);
                }

                AddNewHistoryRow(true, he);

                if (he.IsFSDJump)
                    _discoveryForm.Map.UpdateSystemList(_discoveryForm.history.FilterByFSDAndPosition);           // update map - only cares about FSD changes

                RefreshSummaryRow(dataGridViewTravel.Rows[0], true);         //Tell the summary new row has been added
                RefreshTargetInfo();                                        // tell the target system its changed the latest system

                // Move focus to new row
                if (EDDiscoveryForm.EDDConfig.FocusOnNewSystem)
                {
                    dataGridViewTravel.ClearSelection();
                    dataGridViewTravel.CurrentCell = dataGridViewTravel.Rows[0].Cells[1];       // its the current cell which needs to be set, moves the row marker as well
                    ShowSystemInformation(dataGridViewTravel.Rows[0]);
                    UpdateDependentsWithSelection();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception NewPosition: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }

        }

        public void ShowSystemInformation(DataGridViewRow rw)
        {
            currentGridRow = rw;

            _discoveryForm.history.FillEDSM(rw.Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry); // Fill in any EDSM info we have

            SystemNoteClass note = rw.Cells[TravelHistoryColumns.NoteTag].Tag as SystemNoteClass;
            HistoryEntry syspos = rw.Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry;     // reload, it may have changed
            Debug.Assert(syspos != null);

            textBoxSystem.Text = syspos.System.name;
            
            if (syspos.System.HasCoordinate)         // cursystem has them?
            {
                textBoxX.Text = syspos.System.x.ToString(SingleCoordinateFormat);
                textBoxY.Text = syspos.System.y.ToString(SingleCoordinateFormat);
                textBoxZ.Text = syspos.System.z.ToString(SingleCoordinateFormat);

                textBoxSolDist.Text = Math.Sqrt(syspos.System.x * syspos.System.x + syspos.System.y * syspos.System.y + syspos.System.z * syspos.System.z).ToString("0.00");
            }
            else 
            {
                textBoxX.Text = "?";
                textBoxY.Text = "?";
                textBoxZ.Text = "?";
                textBoxSolDist.Text = "";
            }

            int count = _discoveryForm.history.GetVisitsCount(syspos.System.name);
            textBoxVisits.Text = count.ToString();

            bool enableedddross = (syspos.System.id_eddb > 0);  // Only enable eddb/ross for system that it knows about

            buttonRoss.Enabled = buttonEDDB.Enabled = enableedddross;

            textBoxAllegiance.Text = EnumStringFormat(syspos.System.allegiance.ToString());
            textBoxEconomy.Text = EnumStringFormat(syspos.System.primary_economy.ToString());
            textBoxGovernment.Text = EnumStringFormat(syspos.System.government.ToString());
            textBoxState.Text = EnumStringFormat(syspos.System.state.ToString());
            richTextBoxNote.Text = EnumStringFormat(note != null ? note.Note : "");

            closestsystem_queue.Add(syspos.System);
        }

        private string EnumStringFormat(string str)
        {
            if (str == null)
                return "";
            if (str.Equals("Unknown"))
                return "";

            return str.Replace("_", " ");
        }

        #region Closest System

        public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable      // special compare for sortedlist
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);
                return (result == 0) ? 1 : result;      // for this, equals just means greater than, to allow duplicate distance values to be added.
            }
        }

        Thread closestthread;
        BlockingCollection<ISystem> closestsystem_queue = new BlockingCollection<ISystem>();
        SortedList<double, ISystem> closestsystemlist = new SortedList<double, ISystem>(new DuplicateKeyComparer<double>()); //lovely list allowing duplicate keys - can only iterate in it.

        private void CalculateClosestSystems()
        {
            ISystem vsc;

            while ( true )
            {
                ISystem nextsys = null;
                ISystem cursys = null;
                cursys = closestsystem_queue.Take();           // block until got one..

                closestsystemlist.Clear();

                do
                {
                    vsc = cursys;

                    if (ShutdownEDD)
                        return;

                    while (closestsystem_queue.TryTake(out nextsys))    // try and empty the queue in case multiple ones are there
                    {
                        //Console.WriteLine("Chuck " + closestname);
                        vsc = nextsys;

                        if (ShutdownEDD)
                            return;
                    }

                    SystemClass.GetSystemSqDistancesFrom(closestsystemlist, vsc.x, vsc.y, vsc.z, 50, true, 1000);

                    Invoke((MethodInvoker)delegate      // being paranoid about threads..
                    {
                        _discoveryForm.history.CalculateSqDistances(closestsystemlist, vsc.x, vsc.y, vsc.z, 50, true);
                    });

                    cursys = vsc;
                } while (closestsystem_queue.TryTake(out vsc));     // if there is another one there, just re-run (slow down doggy!)

                Invoke((MethodInvoker)delegate
                {
                    labelclosests.Text = "";
                    dataGridViewNearest.Rows.Clear();

                    if (closestsystemlist.Count() > 0)
                    {
                        labelclosests.Text = "Closest systems from " + cursys.name;
                        foreach (KeyValuePair<double, ISystem> tvp in closestsystemlist)
                        {
                            object[] rowobj = { tvp.Value.name, Math.Sqrt(tvp.Key).ToString("0.00") };       // distances are stored squared for speed, back to normal.
                            int rowindex = dataGridViewNearest.Rows.Add(rowobj);
                            dataGridViewNearest.Rows[rowindex].Tag = tvp.Value;
                        }
                    }
                });
            }
        }

        public void CloseClosestSystemThread()
        {
            if (closestthread != null && closestthread.IsAlive)
            {
                ShutdownEDD = true;
                closestsystem_queue.Add(null);
                closestthread.Join();

            }
        }

        #endregion

        #region Grid Layout

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            ignorewidthchange = true;
            if (SQLiteConnectionUser.keyExists("TravelControlDGVCol1"))        // if stored values, set back to what they were..
            {
                for (int i = 0; i < dataGridViewTravel.Columns.Count; i++)
                {
                    int w = SQLiteDBClass.GetSettingInt("TravelControlDGVCol" + ((i + 1).ToString()), -1);
                    if (w > 10)        // in case something is up (min 10 pixels)
                        dataGridViewTravel.Columns[i].Width = w;
                }
            }

            FillDGVOut();
            ignorewidthchange = false;
        }

        public void SaveSettings()     // called by form when closing
        {
            for (int i = 0; i < dataGridViewTravel.Columns.Count; i++)
                SQLiteDBClass.PutSettingInt("TravelControlDGVCol" + ((i + 1).ToString()), dataGridViewTravel.Columns[i].Width);
        }

        void FillDGVOut()
        {
            int twidth = dataGridViewTravel.RowHeadersWidth;        // get how many pixels we are using..
            for (int i = 0; i < dataGridViewTravel.Columns.Count; i++)
                twidth += dataGridViewTravel.Columns[i].Width;

            int delta = dataGridViewTravel.Width - twidth;

            if (delta < 0)        // not enough space
            {
                Collapse(ref delta, TravelHistoryColumns.Note);         // pick columns on preference list to shrink
                Collapse(ref delta, TravelHistoryColumns.Information);
                Collapse(ref delta, TravelHistoryColumns.Time);
                Collapse(ref delta, TravelHistoryColumns.Icon);
                Collapse(ref delta, TravelHistoryColumns.Description);
            }
            else
                dataGridViewTravel.Columns[TravelHistoryColumns.Note].Width += delta;   // note is used to fill out columns
        }

        void Collapse(ref int delta, int col)
        {
            if (delta < 0)
            {
                int colsaving = dataGridViewTravel.Columns[col].Width - dataGridViewTravel.Columns[col].MinimumWidth;

                if (-delta <= colsaving)       // if can save 30 from col3, and delta is -20, 20<=30, do it.
                {
                    dataGridViewTravel.Columns[col].Width += delta;
                    delta = 0;
                }
                else
                {
                    delta += colsaving;
                    dataGridViewTravel.Columns[col].Width = dataGridViewTravel.Columns[col].MinimumWidth;
                }
            }
        }

        private void TravelHistoryControl_Resize(object sender, EventArgs e)
        {
            ignorewidthchange = true;
            FillDGVOut();
            ignorewidthchange = false;
        }

        bool ignorewidthchange = false;
        private void dataGridViewTravel_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (!ignorewidthchange)
            {
                ignorewidthchange = true;
                FillDGVOut();       // scale out so its filled..
                ignorewidthchange = false;
            }
        }

        #endregion


        public void CheckCommandersListBox()
        {
            if (comboBoxCommander.Items.Count-1 != EDDConfig.Instance.ListOfCommanders.Count)       // account for hidden log
                LoadCommandersListBox();
        }

        public void LoadCommandersListBox()
        {
            comboBoxCommander.Enabled = false;
            commanders = new List<EDCommander>();

            commanders.Add(new EDCommander(-1, "Hidden log", ""));
            commanders.AddRange(EDDiscoveryForm.EDDConfig.ListOfCommanders);

            comboBoxCommander.DataSource = null;
            comboBoxCommander.DataSource = commanders;
            comboBoxCommander.ValueMember = "Nr";
            comboBoxCommander.DisplayMember = "Name";

            EDCommander currentcmdr = EDDiscoveryForm.EDDConfig.CurrentCommander;
            comboBoxCommander.SelectedIndex = commanders.IndexOf(currentcmdr);
            comboBoxCommander.Enabled = true;
        }
                
        private void comboBoxCommander_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCommander.SelectedIndex >= 0 && comboBoxCommander.Enabled )     // DONT trigger during LoadCommandersListBox
            {
                var itm = (EDCommander)comboBoxCommander.SelectedItem;
                _discoveryForm.DisplayedCommander = itm.Nr;
                if (itm.Nr >= 0)
                    EDDiscoveryForm.EDDConfig.CurrentCmdrID = itm.Nr;

                _discoveryForm.RefreshHistoryAsync();                                   // which will cause DIsplay to be called as some point
            }
        }

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            Display();
            SQLiteDBClass.PutSettingInt("EDUIHistory", comboBoxHistoryWindow.SelectedIndex);
        }


        private void dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != TravelHistoryColumns.Icon)
            {
                DataGridViewSorter.DataGridSort(dataGridViewTravel, e.ColumnIndex);
                RedrawSummary();
                UpdateDependentsWithSelection();
            }
        }

        public void buttonMap_Click(object sender, EventArgs e)
        {
            var map = _discoveryForm.Map;

            HistoryEntry he = CurrentSystemSelected;
            this.Cursor = Cursors.WaitCursor;

            string HomeSystem = _discoveryForm.settings.MapHomeSystem;

            _discoveryForm.history.FillInPositionsFSDJumps();

            map.Prepare(he?.System, _discoveryForm.settings.MapHomeSystem,
                        _discoveryForm.settings.MapCentreOnSelection ? he?.System : SystemClass.GetSystem(String.IsNullOrEmpty(HomeSystem) ? "Sol" : HomeSystem),
                        _discoveryForm.settings.MapZoom, _discoveryForm.history.FilterByFSDAndPosition);
            map.Show();
            this.Cursor = Cursors.Default;
        }

        private void dataGridViewTravel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ShowSystemInformation(dataGridViewTravel.Rows[e.RowIndex]);

                UpdateDependentsWithSelection();

                if (e.ColumnIndex == TravelHistoryColumns.Note)
                {
                    richTextBoxNote.TextBox.Select(richTextBoxNote.Text.Length, 0);     // move caret to end and focus.
                    richTextBoxNote.TextBox.ScrollToCaret();
                    richTextBoxNote.TextBox.Focus();
                }
            }
        }

        private void UpdateDependentsWithSelection()
        {
            if (dataGridViewTravel.CurrentCell != null)
            {
                int rowi = dataGridViewTravel.CurrentCell.RowIndex;
                if (rowi >= 0)
                {
                    HistoryEntry currentsys = dataGridViewTravel.Rows[rowi].Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry;
                    _discoveryForm.Map.UpdateHistorySystem(currentsys.System);
                    _discoveryForm.RouteControl.UpdateHistorySystem(currentsys.System.name);
                }
            }
        }

        private void dataGridViewTravel_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ShowSystemInformation(dataGridViewTravel.Rows[e.RowIndex]);
                UpdateDependentsWithSelection();
            }
        }

        private void richTextBoxNote_Leave(object sender, EventArgs e)
        {
            StoreSystemNote();
        }

        private void richTextBoxNote_TextChanged(object sender, EventArgs e)
        {
            if (dataGridViewTravel.SelectedCells.Count > 0)          // if we have selected (we should!)
                dataGridViewTravel.Rows[dataGridViewTravel.SelectedCells[0].OwningRow.Index].Cells[TravelHistoryColumns.Note].Value = richTextBoxNote.Text;     // keep the grid up to date to make it seem more interactive
        }

        private void StoreSystemNote()
        {
            if (currentGridRow == null )
                return;

            try
            {
                HistoryEntry sys = currentGridRow.Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry;
                SystemNoteClass sn = currentGridRow.Cells[TravelHistoryColumns.NoteTag].Tag as SystemNoteClass;

                string txt = richTextBoxNote.Text.Trim();

                if ( (sn == null && txt.Length>0) || (sn!=null && !sn.Note.Equals(txt))) // if no system note, and text,  or system not is not text
                {
                    if ( sn != null )           // already there, update
                    { 
                        sn.Note = txt;
                        sn.Time = DateTime.Now;
                        sn.Name = (sys.IsFSDJump) ? sys.System.name : "";
                        sn.Journalid = (sys.IsFSDJump) ? 0 : sys.Journalid;
                        sn.Update();
                    }
                    else
                    {
                        sn = new SystemNoteClass();
                        sn.Note = txt;
                        sn.Time = DateTime.Now;
                        sn.Name = (sys.IsFSDJump) ? sys.System.name : "";
                        sn.Journalid = (sys.IsFSDJump) ? 0 : sys.Journalid;
                        sn.Add();
                        currentGridRow.Cells[TravelHistoryColumns.NoteTag].Tag = sn;
                    }

                    currentGridRow.Cells[TravelHistoryColumns.Note].Value = txt;

                    if (checkBoxEDSMSyncTo.Checked && sys.IsFSDJump )       // only send on FSD jumps
                        EDSMSync.SendComments(sn.Name,sn.Note);

                    _discoveryForm.Map.UpdateNote();
                    RefreshSummaryRow(dataGridViewTravel.Rows[dataGridViewTravel.SelectedCells[0].OwningRow.Index]);    // tell it this row was changed
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);

                LogLineHighlight("Exception : " + ex.Message);
                LogLineHighlight(ex.StackTrace);
            }
        }

        private void buttonSync_Click(object sender, EventArgs e)
        {
            if (!EDDConfig.Instance.CheckCommanderEDSMAPI)
            {
                MessageBox.Show("Please ensure a commander is selected and it has a EDSM API key set");
                return;
            }

            try
            {
                _discoveryForm.EdsmSync.StartSync(checkBoxEDSMSyncTo.Checked, checkBoxEDSMSyncFrom.Checked, EDDConfig.Instance.DefaultMapColour);
            }
            catch (Exception ex)
            {
                _discoveryForm.LogLine($"EDSM Sync failed: {ex.Message}");
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            PaintEventColumn(grid, e ,
                            _discoveryForm.history.Count, (HistoryEntry)dataGridViewTravel.Rows[e.RowIndex].Cells[TravelHistoryColumns.HistoryTag].Tag ,
                            grid.RowHeadersWidth + grid.Columns[0].Width , grid.Columns[1].Width , true);
        }

        public static void PaintEventColumn( DataGridView grid, DataGridViewRowPostPaintEventArgs e, 
                                             int totalentries, HistoryEntry he , 
                                             int hpos, int colwidth , bool showfsdmapcolour )
        {
            string rowIdx; 

            if (_discoveryForm.settings.OrderRowsInverted)
                 rowIdx = he.Indexno.ToString();            // oldest has the highest index
            else
                rowIdx = (totalentries - he.Indexno + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);

            using ( Brush br = new SolidBrush(grid.RowHeadersDefaultCellStyle.ForeColor))
                e.Graphics.DrawString(rowIdx, grid.RowHeadersDefaultCellStyle.Font, br , headerBounds, centerFormat);

            int noicons = (he.IsFSDJump && showfsdmapcolour) ? 2 : 1;
            if (he.StartMarker || he.StopMarker)
                noicons++;

            int padding = 4;
            int size = 18;
            int hstart = (hpos + colwidth / 2) - size / 2 * noicons - padding / 2 * (noicons - 1);

            int top = (e.RowBounds.Top + e.RowBounds.Bottom) / 2 - size / 2;

            e.Graphics.DrawImage(he.GetIcon, new Rectangle(hstart, top, size, size));
            hstart += size + padding;

            if (he.IsFSDJump && showfsdmapcolour)
            {
                using (Brush b = new SolidBrush(Color.FromArgb(he.MapColour)))
                {
                    e.Graphics.FillEllipse(b, new Rectangle(hstart+2, top+2, size-4, size-4));
                }

                hstart += size + padding;
            }

            if (he.StartMarker)
                e.Graphics.DrawImage(EDDiscovery.Properties.Resources.startflag, new Rectangle(hstart, top, size, size));
            else if (he.StopMarker)
                e.Graphics.DrawImage(EDDiscovery.Properties.Resources.stopflag, new Rectangle(hstart, top, size, size));

        }

        private void buttonEDDB_Click(object sender, EventArgs e)
        {
            HistoryEntry sys = currentGridRow.Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry;

            if ( currentGridRow!= null && sys.System.id_eddb>0)
                Process.Start("http://eddb.io/system/" + sys.System.id_eddb.ToString());
        }

        private void buttonRoss_Click(object sender, EventArgs e)
        {
            HistoryEntry sys = currentGridRow.Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry;

            if (currentGridRow!= null && sys.System.id_eddb>0)
                Process.Start("http://ross.eddb.io/system/update/" + sys.System.id_eddb.ToString());
        }

        private void buttonEDSM_Click(object sender, EventArgs e)
        {
            HistoryEntry sys = currentGridRow.Cells[TravelHistoryColumns.HistoryTag].Tag as HistoryEntry;

            if (currentGridRow != null && sys.System != null) // solve a possible exception
            {
                if (!String.IsNullOrEmpty(sys.System.name))
                {
                    long? id_edsm = sys.System.id_edsm;
                    if (id_edsm == 0)
                    {
                        id_edsm = null;
                    }

                    EDSMClass edsm = new EDSMClass();
                    string url = edsm.GetUrlToEDSMSystem(sys.System.name, id_edsm);

                    if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                        Process.Start(url);
                    else
                        MessageBox.Show("System unknown to EDSM");
                }
            }
        }

        public void RefreshButton(bool state)
        {
            button_RefreshHistory.Enabled = state;
        }

        private void button_RefreshHistory_Click(object sender, EventArgs e)
        {
            LogLine("Refresh History.");
            _discoveryForm.RefreshHistoryAsync();
        }

        private void textBoxFilter_KeyUp(object sender, KeyEventArgs e)
        {
            StaticFilters.FilterGridView(dataGridViewTravel, textBoxFilter.Text);
            RedrawSummary();
            UpdateDependentsWithSelection();
        }


        private void checkBoxEDSMSyncTo_CheckedChanged(object sender, EventArgs e)
        {
            buttonSync.Enabled = checkBoxEDSMSyncTo.Checked | checkBoxEDSMSyncFrom.Checked;
        }

        private void checkBoxEDSMSyncFrom_CheckedChanged(object sender, EventArgs e)
        {
            buttonSync.Enabled = checkBoxEDSMSyncTo.Checked | checkBoxEDSMSyncFrom.Checked;
        }

        private void button2DMap_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FormSagCarinaMission frm = new FormSagCarinaMission(_discoveryForm.history.FilterByFSDAndPosition);
            frm.Nowindowreposition = _discoveryForm.option_nowindowreposition;
            frm.Show();
            this.Cursor = Cursors.Default;
        }
        
        private void textBoxTarget_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string sn = textBoxTarget.Text;
                ISystem sc = _discoveryForm.history.FindSystem(sn);
                string msgboxtext = null;

                if (sc != null && sc.HasCoordinate)
                {
                    SystemNoteClass nc = SystemNoteClass.GetNoteOnSystem(sn);        // has it got a note?

                    if (nc != null)
                    {
                        TargetClass.SetTargetNotedSystem(sc.name, nc.id, sc.x, sc.y, sc.z);
                        msgboxtext = "Target set on system with note " + sc.name;
                    }
                    else
                    {
                        BookmarkClass bk = BookmarkClass.FindBookmarkOnSystem(textBoxTarget.Text);    // has it been bookmarked?

                        if (bk != null)
                        {
                            TargetClass.SetTargetBookmark(sc.name, bk.id, bk.x, bk.y, bk.z);
                            msgboxtext = "Target set on booked marked system " + sc.name;
                        }
                        else
                        {
                            if (MessageBox.Show("Make a bookmark on " + sc.name + " and set as target?", "Make Bookmark", MessageBoxButtons.OKCancel) == DialogResult.OK)
                            {
                                BookmarkClass newbk = new BookmarkClass();
                                newbk.StarName = sn;
                                newbk.x = sc.x;
                                newbk.y = sc.y;
                                newbk.z = sc.z;
                                newbk.Time = DateTime.Now;
                                newbk.Note = "";
                                newbk.Add();
                                TargetClass.SetTargetBookmark(sc.name, newbk.id, newbk.x, newbk.y, newbk.z);
                            }
                        }
                    }

                }
                else
                {
                    if (sn.Length > 2 && sn.Substring(0, 2).Equals("G:"))
                        sn = sn.Substring(2, sn.Length - 2);

                    GalacticMapObject gmo = EDDiscoveryForm.galacticMapping.Find(sn, true, true);    // ignore if its off, find any part of string, find if disabled

                    if (gmo != null)
                    {
                        TargetClass.SetTargetGMO("G:" + gmo.name, gmo.id, gmo.points[0].X, gmo.points[0].Y, gmo.points[0].Z);
                        msgboxtext = "Target set on galaxy object " + gmo.name;
                    }
                    else
                    {
                        msgboxtext = "Unknown system, system is without co-ordinates or galaxy object not found";
                    }
                }

                RefreshTargetInfo();
                if (_discoveryForm.Map != null)
                    _discoveryForm.Map.UpdateBookmarksGMO(true);

                if ( msgboxtext != null)
                    MessageBox.Show(msgboxtext,"Create a target", MessageBoxButtons.OK);
            }
        }

#region Target System

        public void RefreshTargetInfo()
        {
            string name;
            double x, y, z;

            if (TargetClass.GetTargetPosition(out name, out x, out y, out z))
            {
                textBoxTarget.Text = name;
                textBoxTargetDist.Text = "No Pos";

                HistoryEntry cs = _discoveryForm.history.GetLastWithPosition;
                if ( cs != null )
                    textBoxTargetDist.Text = SystemClass.Distance(cs.System, x, y, z).ToString("0.00");

                toolTipEddb.SetToolTip(textBoxTarget, "Position is " + x.ToString("0.00") + "," + y.ToString("0.00") + "," + z.ToString("0.00"));
            }
            else
            {
                textBoxTarget.Text = "Set target";
                textBoxTargetDist.Text = "";
                toolTipEddb.SetToolTip(textBoxTarget, "On 3D Map right click to make a bookmark, region mark or click on a notemark and then tick on Set Target, or type it here and hit enter");
            }

            if (IsSummaryPopOutReady)
                summaryPopOut.RefreshTarget(dataGridViewTravel, _discoveryForm.history.GetLastWithPosition);
        }

#endregion

#region Summary Pop out
        
        public bool IsSummaryPopOutReady { get { return summaryPopOut != null && !summaryPopOut.IsFormClosed; } }

        public bool ToggleSummaryPopOut()
        {
            if (summaryPopOut == null || summaryPopOut.IsFormClosed)
            {
                SummaryPopOut p = new SummaryPopOut();
                p.RequiresRefresh += SummaryRefreshRequested;
                p.SetGripperColour(_discoveryForm.theme.LabelColor);
                p.ResetForm(dataGridViewTravel);
                p.RefreshTarget(dataGridViewTravel, _discoveryForm.history.GetLastWithPosition); 
                p.Show();
                summaryPopOut = p;          // do it like this in case of race conditions 
                return true;
            }
            else
            { 
                summaryPopOut.Close();      // there is no point null it, as if the user closes it, it never gets the chance to be nulled
                return false;
            }
        }

        public void RedrawSummary()
        {
            if (IsSummaryPopOutReady)
            {
                summaryPopOut.SetGripperColour(_discoveryForm.theme.LabelColor);
                summaryPopOut.ResetForm(dataGridViewTravel);
                summaryPopOut.RefreshTarget(dataGridViewTravel, _discoveryForm.history.GetLastWithPosition);
            }
        }

        public void SummaryRefreshRequested(Object o, EventArgs e)
        {
            RedrawSummary();
        }

        public void RefreshSummaryRow(DataGridViewRow row , bool add = false )
        {
            if (IsSummaryPopOutReady)
                summaryPopOut.RefreshRow(dataGridViewTravel, row, add);
        }

        private void buttonExtSummaryPopOut_Click(object sender, EventArgs e)
        {
            ToggleSummaryPopOut();
        }

#endregion

#region ClosestSystemRightClick

        private void addToTrilaterationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TrilaterationControl tctrl = _discoveryForm.trilaterationControl;

            IEnumerable<DataGridViewRow> selectedRows = dataGridViewNearest.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            this.Cursor = Cursors.WaitCursor;
            string sysName = "";
            foreach (DataGridViewRow r in selectedRows)
            {
                sysName = r.Cells[ClosestSystemsColumns.SystemName].Value.ToString();

                tctrl.AddSystemToDataGridViewDistances(sysName);
            }

            this.Cursor = Cursors.Default;
        }

        private void viewOnEDSMToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewNearest.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            this.Cursor = Cursors.WaitCursor;
            ISystem system = (ISystem)selectedRows.First<DataGridViewRow>().Tag;
            EDSMClass edsm = new EDSMClass();
            if (!edsm.ShowSystemInEDSM(system.name, system.id_edsm))
                LogLineHighlight("System could not be found - has not been synched or EDSM is unavailable");

            this.Cursor = Cursors.Default;
        }

        #endregion

        #region CLicks

        HistoryEntry rightclicksystem = null;
        int rightclickrow = -1;
        HistoryEntry leftclicksystem = null;
        int leftclickrow = -1;

        private void dataGridViewTravel_MouseDown(object sender, MouseEventArgs e)  // MAKES row selected when mouse down
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                rightclicksystem = null;
                rightclickrow = -1;
            }
            if (e.Button == MouseButtons.Left)         // right click on travel map, get in before the context menu
            {
                leftclicksystem = null;
                leftclickrow = -1;
            }

            if (dataGridViewTravel.SelectedCells.Count < 2 || dataGridViewTravel.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
            {
                DataGridView.HitTestInfo hti = dataGridViewTravel.HitTest(e.X, e.Y);
                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    dataGridViewTravel.ClearSelection();                // select row under cursor.
                    dataGridViewTravel.Rows[hti.RowIndex].Selected = true;

                    if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
                    {
                        rightclickrow = hti.RowIndex;
                        rightclicksystem = (HistoryEntry)dataGridViewTravel.Rows[hti.RowIndex].Cells[TravelHistoryColumns.HistoryTag].Tag;
                    }
                    if (e.Button == MouseButtons.Left)         // right click on travel map, get in before the context menu
                    {
                        leftclickrow = hti.RowIndex;
                        leftclicksystem = (HistoryEntry)dataGridViewTravel.Rows[hti.RowIndex].Cells[TravelHistoryColumns.HistoryTag].Tag;
                    }
                }
            }

        }

        private void dataGridViewTravel_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ( leftclickrow>=0)                                                   // Click expands it..
            {
                int ch = dataGridViewTravel.Rows[leftclickrow].Height;
                bool expanded = (ch < 25);

                string text = leftclicksystem.EventDescription + ((expanded) ? (Environment.NewLine + leftclicksystem.EventDetailedInfo) : "");
                int h = 22;

                if (expanded)
                {
                    SizeF sizef;
                    using (Graphics g = Parent.CreateGraphics())
                    {
                        sizef = g.MeasureString(text, dataGridViewTravel.Font, dataGridViewTravel.Columns[TravelHistoryColumns.Information].Width);
                    }

                    h = (int)(sizef.Height + 2);
                }

                dataGridViewTravel.Rows[leftclickrow].Height = h;
                dataGridViewTravel.Rows[leftclickrow].Cells[TravelHistoryColumns.Information].Value = text;

                DataGridViewTriState ti = (expanded) ? DataGridViewTriState.True : DataGridViewTriState.False;

                dataGridViewTravel.Rows[leftclickrow].Cells[TravelHistoryColumns.Information].Style.WrapMode = ti;
                dataGridViewTravel.Rows[leftclickrow].Cells[TravelHistoryColumns.Description].Style.WrapMode = ti;
                dataGridViewTravel.Rows[leftclickrow].Cells[TravelHistoryColumns.Note].Style.WrapMode = ti;
            }

        }
        
        #endregion

        #region TravelHistoryRightClick

        private void historyContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewTravel.SelectedCells.Count == 0)      // need something selected  stops context menu opening on nothing..
                e.Cancel = true;

            HistoryEntry prev = _discoveryForm.history.PreviousFrom(rightclicksystem, true);    // null can be passed in safely

            mapGotoStartoolStripMenuItem.Enabled = (rightclicksystem != null && rightclicksystem.System.HasCoordinate);
            viewOnEDSMToolStripMenuItem.Enabled = (rightclicksystem != null);
        }

        // enabled only if rightclick system is set.
        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (!_discoveryForm.Map.Is3DMapsRunning)            // if not running, click the 3dmap button
                buttonMap_Click(sender, e);
            this.Cursor = Cursors.Default;

            if (_discoveryForm.Map.Is3DMapsRunning)             // double check here! for paranoia.
            {
                if (_discoveryForm.Map.MoveToSystem(rightclicksystem.System))
                    _discoveryForm.Map.Show();
            }
        }

        private void starMapColourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                           .Select(cell => cell.OwningRow)
                                                           .Distinct();
            ColorDialog mapColorDialog = new ColorDialog();
            mapColorDialog.AllowFullOpen = true;
            mapColorDialog.FullOpen = true;
            HistoryEntry sp2 = (HistoryEntry)selectedRows.First().Cells[TravelHistoryColumns.HistoryTag].Tag;
            mapColorDialog.Color = Color.FromArgb(sp2.MapColour);

            if (mapColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (DataGridViewRow r in selectedRows)
                {
                    HistoryEntry sp = (HistoryEntry)r.Cells[TravelHistoryColumns.HistoryTag].Tag;
                    Debug.Assert(sp != null);
                    sp.UpdateMapColour(mapColorDialog.Color.ToArgb());
                }

                this.Cursor = Cursors.Default;
                Display();
            }
        }

        private void hideSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => cell.OwningRow)
                .Distinct();

            this.Cursor = Cursors.WaitCursor;

            foreach (DataGridViewRow r in selectedRows)
            {
                HistoryEntry sp = (HistoryEntry)r.Cells[TravelHistoryColumns.HistoryTag].Tag;
                Debug.Assert(sp != null);
                sp.UpdateCommanderID(-1);
            }

            // Remove rows
            if (selectedRows.Count<DataGridViewRow>() == dataGridViewTravel.Rows.Count)
            {
                dataGridViewTravel.Rows.Clear();
            }
            else
            {
                foreach (DataGridViewRow row in selectedRows.ToList<DataGridViewRow>())
                {
                    dataGridViewTravel.Rows.Remove(row);
                }
            }

            this.Cursor = Cursors.Default;
        }

        private void moveToAnotherCommanderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => cell.OwningRow)
                .Distinct();

            List<HistoryEntry> listsyspos = new List<HistoryEntry>();

            this.Cursor = Cursors.WaitCursor;
            foreach (DataGridViewRow r in selectedRows)
            {
                HistoryEntry sp = (HistoryEntry)r.Cells[TravelHistoryColumns.HistoryTag].Tag;
                Debug.Assert(sp != null);
                listsyspos.Add(sp);
            }

            MoveToCommander movefrm = new MoveToCommander();

            movefrm.Init(listsyspos.Count > 1);

            DialogResult red = movefrm.ShowDialog();
            if (red == DialogResult.OK)
            {
                foreach (HistoryEntry sp in listsyspos)
                {
                    sp.UpdateCommanderID(movefrm.selectedCommander.Nr);
                }

                foreach (DataGridViewRow row in selectedRows)
                {
                    dataGridViewTravel.Rows.Remove(row);
                }
            }

            this.Cursor = Cursors.Default;
        }

        private void wantedSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrilaterationControl tctrl = _discoveryForm.trilaterationControl;

            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            this.Cursor = Cursors.WaitCursor;
            foreach (DataGridViewRow r in selectedRows)
            {
                HistoryEntry sp = (HistoryEntry)r.Cells[TravelHistoryColumns.HistoryTag].Tag;
                tctrl.AddWantedSystem(sp.System.name);
            }

            this.Cursor = Cursors.Default;
        }

        private void bothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrilaterationControl tctrl = _discoveryForm.trilaterationControl;

            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            this.Cursor = Cursors.WaitCursor;
            foreach (DataGridViewRow r in selectedRows)
            {
                HistoryEntry sp = (HistoryEntry)r.Cells[TravelHistoryColumns.HistoryTag].Tag;
                tctrl.AddSystemToDataGridViewDistances(sp.System.name);
                tctrl.AddWantedSystem(sp.System.name);
            }

            this.Cursor = Cursors.Default;
        }

        private void trilaterationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TrilaterationControl tctrl = _discoveryForm.trilaterationControl;

            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            this.Cursor = Cursors.WaitCursor;
            foreach (DataGridViewRow r in selectedRows)
            {
                HistoryEntry sp = (HistoryEntry)r.Cells[TravelHistoryColumns.HistoryTag].Tag;
                tctrl.AddSystemToDataGridViewDistances(sp.System.name);
            }

            this.Cursor = Cursors.Default;
        }

        // enabled only if rightclick system is set.
        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EDSMClass edsm = new EDSMClass();
            long? id_edsm = rightclicksystem.System?.id_edsm;

            if (id_edsm == 0)
            {
                id_edsm = null;
            }

            if (!edsm.ShowSystemInEDSM(rightclicksystem.System.name, id_edsm))
                LogLineHighlight("System could not be found - has not been synched or EDSM is unavailable");

            this.Cursor = Cursors.Default;
        }
        
        private void selectCorrectSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<JournalEntry> jents = JournalEntry.GetAll(EDDConfig.Instance.CurrentCmdrID).OrderBy(j => j.EventTimeUTC).ThenBy(j => j.Id).ToList();
            int selindex = jents.FindIndex(j => j.Id == rightclicksystem.Journalid);
            int firstrow = selindex;
            int lastrow = selindex;

            if (selindex < 0)
            {
                // Selected entry is not in history for commander - abort.
                return;
            }

            EliteDangerous.JournalEvents.JournalLocOrJump journalent = null;

            if (jents[selindex].EventTypeID != JournalTypeEnum.FSDJump)
            {
                for (int i = selindex - 1; i >= 0; i--)
                {
                    var jent = jents[i];
                    if (jent.EdsmID != rightclicksystem.System.id_edsm || jent.EventTypeID == JournalTypeEnum.Died)
                        break;
                    firstrow = i;
                    if (jent.EventTypeID == JournalTypeEnum.FSDJump)
                        break;
                }
            }

            for (int i = rightclickrow + 1; i < dataGridViewTravel.RowCount; i++)
            {
                var jent = jents[i];
                if (jent.EdsmID != rightclicksystem.System.id_edsm || jent.EventTypeID == JournalTypeEnum.FSDJump)
                    break;
                lastrow = i;
                if (jent.EventTypeID == JournalTypeEnum.Died)
                    break;
            }

            var _jents = jents;
            jents = new List<JournalEntry>();

            for (int i = firstrow; i <= lastrow; i++)
            {
                jents.Add(_jents[i]);
            }

            journalent = jents.OfType<EliteDangerous.JournalEvents.JournalLocOrJump>().FirstOrDefault();

            if (journalent == null)
            {
                MessageBox.Show("Could not find Location or FSDJump entry associated with selected journal entry");
                return;
            }

            using (Forms.AssignTravelLogSystemForm form = new Forms.AssignTravelLogSystemForm(this, journalent))
            {
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    foreach (var jent in jents)
                    {
                        jent.EdsmID = (int)form.AssignedEdsmId;
                        jent.Update();
                    }

                    _discoveryForm.RefreshHistoryAsync();
                }
            }
        }

        private void routeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<HistoryEntry> toAdd = new List<HistoryEntry>();

            foreach (DataGridViewCell cell in dataGridViewTravel.SelectedCells)
            {
                HistoryEntry vsc = (HistoryEntry)cell.OwningRow.Cells[TravelHistoryColumns.HistoryTag].Tag;
                if (!toAdd.Any(v => !v.System.name.Equals(vsc.System.name)))
                {
                    toAdd.Add(vsc);
                }
            }

            _discoveryForm.savedRouteExpeditionControl1.AppendRows(toAdd.Select(v => v.System.name).ToArray());
        }

        private void toolStripMenuItemStartStop_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
            {
                _discoveryForm.history.SetStartStop(rightclicksystem);
                _discoveryForm.RefreshFrontEnd();                                   // which will cause DIsplay to be called as some point
            }
        }


        #endregion

        #region Event Filter

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            cfs.FilterButton("TravelHistoryControlEventFilter", b,
                             _discoveryForm.theme.TextBackColor, _discoveryForm.theme.TextBlockColor);
        }

        private void EventFilterChanged(object sender, EventArgs e)
        {
            Display();
        }

        #endregion

        #region LogOut

        public void LogLine(string text)
        {
            LogLineColor(text, _discoveryForm.theme.TextBlockColor);
        }

        public void LogLineHighlight(string text)
        {
            LogLineColor(text, _discoveryForm.theme.TextBlockHighlightColor);
        }

        public void LogLineSuccess(string text)
        {
            LogLineColor(text, _discoveryForm.theme.TextBlockSuccessColor);
        }

        public void LogLineColor(string text, Color color)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    richTextBox_History.AppendText(text + Environment.NewLine, color);
                });
            }
            catch { }
        }

        #endregion

    }
}
