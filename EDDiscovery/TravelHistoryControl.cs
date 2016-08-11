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

namespace EDDiscovery
{
    public partial class TravelHistoryControl : UserControl
    {
        private const int MaximumJumpRange = 45; // max jump range is ~42Ly
        private bool ShutdownEDD = false;

        public class TravelHistoryColumns
        {
            public const int Time = 0;
            public const int SystemName = 1;
            public const int Distance = 2;
            public const int Note = 3;
            public const int Map = 4;
        }

        public class ClosestSystemsColumns
        {
            public const int SystemName = 0;
        }

        private const int DefaultTravelHistoryFilterIndex = 8;
        private const string SingleCoordinateFormat = "0.#####";

        private static EDDiscoveryForm _discoveryForm;
        public int defaultMapColour;
        public EDSMSync sync;

        internal List<VisitedSystemsClass> visitedSystems = new List<VisitedSystemsClass>();
        internal bool EDSMSyncTo = true;
        internal bool EDSMSyncFrom = true;

        public NetLogClass netlog;
        private VisitedSystemsClass currentSysPos = null;

        SummaryPopOut summaryPopOut = null;

        private int activecommander = 0;
        List<EDCommander> commanders = null;

        public event EventHandler HistoryRefreshed;

        public TravelHistoryControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            netlog = new NetLogClass(_discoveryForm);
            sync = new EDSMSync(_discoveryForm);
            defaultMapColour = EDDConfig.Instance.DefaultMapColour;
            EDSMSyncTo = SQLiteDBClass.GetSettingBool("EDSMSyncTo", true);
            EDSMSyncFrom = SQLiteDBClass.GetSettingBool("EDSMSyncFrom", true);
            checkBoxEDSMSyncTo.Checked = EDSMSyncTo;
            checkBoxEDSMSyncFrom.Checked = EDSMSyncFrom;
            comboBoxHistoryWindow.Enabled = false;
            comboBoxHistoryWindow.DataSource = new[]
            {
                TravelHistoryFilter.FromHours(6),
                TravelHistoryFilter.FromHours(12),
                TravelHistoryFilter.FromHours(24),
                TravelHistoryFilter.FromDays(3),
                TravelHistoryFilter.FromWeeks(1),
                TravelHistoryFilter.FromWeeks(2),
                TravelHistoryFilter.LastMonth(),
                TravelHistoryFilter.Last(20),
                TravelHistoryFilter.NoFilter,
            };

            richTextBoxNote.TextBoxChanged += richTextBoxNote_TextChanged;

            comboBoxHistoryWindow.DisplayMember = nameof(TravelHistoryFilter.Label);

            comboBoxHistoryWindow.SelectedIndex = SQLiteDBClass.GetSettingInt("EDUIHistory", DefaultTravelHistoryFilterIndex);
            comboBoxHistoryWindow.Enabled = true;

            LoadCommandersListBox();

            closestthread = new Thread(CalculateClosestSystems) { Name = "Closest Calc", IsBackground = true };
            closestthread.Start();
        }

        private void button_RefreshHistory_Click(object sender, EventArgs e)
        {
            visitedSystems.Clear();
            try
            {
                LogText("Refresh History." + Environment.NewLine);
                RefreshHistoryAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);

                LogTextHighlight("Exception : " + ex.Message);
                LogTextHighlight(ex.StackTrace);
            }
        }

        public void LogText(string text)
        {
            LogTextColor(text, _discoveryForm.theme.TextBlockColor );
        }

        public void LogTextHighlight(string text)
        {
            LogTextColor(text, _discoveryForm.theme.TextBlockHighlightColor);
        }

        public void LogTextSuccess(string text)
        {
            LogTextColor(text, _discoveryForm.theme.TextBlockSuccessColor);
        }

        public void LogTextColor( string text, Color color)
        {
            //Console.WriteLine("Text <" + text + "> from " + Environment.StackTrace);
            richTextBox_History.AppendText(text, color);
        }

        public void RefreshHistoryAsync()
        {
            if (activecommander >= 0)
            {
                if (!_refreshWorker.IsBusy)
                {
                    button_RefreshHistory.Enabled = false;
                    _refreshWorker.RunWorkerAsync();
                }
            }
            else
            {
                RefreshHistory(VisitedSystemsClass.GetAll(activecommander));
            }
        }

        public void CancelHistoryRefresh()
        {
            _refreshWorker.CancelAsync();
        }

        private void RefreshHistoryWorker(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

            string errmsg;
            netlog.StopMonitor();          // this is called by the foreground.  Ensure background is stopped.  Foreground must restart it.

            var vsclist = netlog.ParseFiles(out errmsg, defaultMapColour, () => worker.CancellationPending, (p,s) => worker.ReportProgress(p,s));   // Parse files stop monitor..

            if (worker.CancellationPending)
            {
                e.Cancel = true;
                e.Result = null;
                return;
            }

            if (errmsg != null)
            {
                throw new InvalidOperationException(errmsg);
            }

            e.Result = vsclist;
        }

        private void RefreshHistoryWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && !_discoveryForm.PendingClose)
            {
                if (e.Error != null)
                {
                    LogTextHighlight("History Refresh Error: " + e.Error.Message + Environment.NewLine);
                }
                else if (e.Result != null)
                {
                    RefreshHistory((List<VisitedSystemsClass>)e.Result);
                    _discoveryForm.ReportProgress(-1, "");
                    LogText("Refresh Complete." + Environment.NewLine);
                }
                button_RefreshHistory.Enabled = true;

                netlog.StartMonitor();

                if (HistoryRefreshed != null)
                    HistoryRefreshed(this, EventArgs.Empty);
            }
        }

        private void RefreshHistoryWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string name = (string)e.UserState;
            _discoveryForm.ReportProgress(e.ProgressPercentage, $"Processing log file {name}");
        }

        private void RefreshHistory(List<VisitedSystemsClass> vsc)
        {
            visitedSystems = vsc;

            if (visitedSystems == null)
                return;

            // Prevent the indexes from being removed from under our feet
            lock (_discoveryForm.SystemsUpdatingLock)
            {
                VisitedSystemsClass.UpdateSys(visitedSystems, EDDConfig.Instance.UseDistances, !_discoveryForm.SystemsUpdating);
            }

            var filter = (TravelHistoryFilter) comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;
            List<VisitedSystemsClass> result = filter.Filter(visitedSystems);

            dataGridViewTravel.Rows.Clear();

            for (int ii = 0; ii < result.Count; ii++) //foreach (var item in result)
            {
                AddNewHistoryRow(false, result[ii]);      // for every one in filter, add a row.
            }

            if (dataGridViewTravel.Rows.Count > 0)
            {
                ShowSystemInformation((VisitedSystemsClass)(dataGridViewTravel.Rows[0].Cells[TravelHistoryColumns.SystemName].Tag));
            }

            if (textBoxFilter.TextLength>0)
                FilterGridView();

            RedrawSummary();
            RefreshTargetInfo();
            UpdateDependentsWithSelection();
            _discoveryForm.Map.UpdateVisited(visitedSystems);           // update map
        }

        private void AddNewHistoryRow(bool insert, VisitedSystemsClass item)            // second part of add history row, adds item to view.
        {
            object[] rowobj = { item.Time, item.Name, item.strDistance, SystemNoteClass.GetSystemNoteOrEmpty(item.curSystem.name), "█" };
            int rownr;

            if (item.curSystem != null && item.curSystem.name.ToLower() != item.Name.ToLower())
            {
                rowobj[1] = $"{item.Name} ({item.curSystem.name})";
            }

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

            var cell = dataGridViewTravel.Rows[rownr].Cells[TravelHistoryColumns.SystemName];

            cell.Tag = item;

            bool hascoord; 

            if (item.HasTravelCoordinates)
                hascoord  = true;
            else 
                hascoord = item.curSystem.HasCoordinate;

            dataGridViewTravel.Rows[rownr].DefaultCellStyle.ForeColor = (hascoord) ? _discoveryForm.theme.VisitedSystemColor : _discoveryForm.theme.NonVisitedSystemColor;

            cell = dataGridViewTravel.Rows[rownr].Cells[TravelHistoryColumns.Map];
            cell.Style.ForeColor = Color.FromArgb(item.MapColour);
        }


        private void ShowSystemInformation(VisitedSystemsClass syspos)
        {
            if (syspos == null || syspos.Name==null)
                return;

            currentSysPos = syspos;
            textBoxSystem.Text = syspos.curSystem.name;
            
            if (syspos.curSystem.HasCoordinate)         // cursystem has them?
            {
                textBoxX.Text = syspos.curSystem.x.ToString(SingleCoordinateFormat);
                textBoxY.Text = syspos.curSystem.y.ToString(SingleCoordinateFormat);
                textBoxZ.Text = syspos.curSystem.z.ToString(SingleCoordinateFormat);

                textBoxSolDist.Text = Math.Sqrt(syspos.curSystem.x * syspos.curSystem.x + syspos.curSystem.y * syspos.curSystem.y + syspos.curSystem.z * syspos.curSystem.z).ToString("0.00");
            }
            else if ( !syspos.HasTravelCoordinates )    // vsc has them? No, ?
            {
                textBoxX.Text = "?";
                textBoxY.Text = "?";
                textBoxZ.Text = "?";
                textBoxSolDist.Text = "";
            }
            else
            {
                textBoxX.Text = syspos.X.ToString(SingleCoordinateFormat);
                textBoxY.Text = syspos.Y.ToString(SingleCoordinateFormat);
                textBoxZ.Text = syspos.Z.ToString(SingleCoordinateFormat);

                textBoxSolDist.Text = Math.Sqrt(syspos.X * syspos.X + syspos.Y * syspos.Y + syspos.Z * syspos.Z).ToString("0.00");
            }

            int count = GetVisitsCount(syspos.curSystem.name);
            textBoxVisits.Text = count.ToString();

            bool enableedddross = (currentSysPos.curSystem.id_eddb > 0);  // Only enable eddb/ross for system that it knows about

            buttonRoss.Enabled = buttonEDDB.Enabled = enableedddross;

            textBoxAllegiance.Text = EnumStringFormat(syspos.curSystem.allegiance.ToString());
            textBoxEconomy.Text = EnumStringFormat(syspos.curSystem.primary_economy.ToString());
            textBoxGovernment.Text = EnumStringFormat(syspos.curSystem.government.ToString());
            textBoxState.Text = EnumStringFormat(syspos.curSystem.state.ToString());
            richTextBoxNote.Text = EnumStringFormat(SystemNoteClass.GetSystemNoteOrEmpty(syspos.Name));

            closestsystem_queue.Add(syspos);
        }

        private string EnumStringFormat(string str)
        {
            if (str == null)
                return "";
            if (str.Equals("Unknown"))
                return "";

            return str.Replace("_", " ");
        }

        public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable      // special compare for sortedlist
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);
                return (result == 0) ? 1 : result;      // for this, equals just means greater than, to allow duplicate distance values to be added.
            }
        }

        Thread closestthread;
        BlockingCollection<VisitedSystemsClass> closestsystem_queue = new BlockingCollection<VisitedSystemsClass>();
        SortedList<double, ISystem> closestsystemlist = new SortedList<double, ISystem>(new DuplicateKeyComparer<double>()); //lovely list allowing duplicate keys - can only iterate in it.

        private void CalculateClosestSystems()
        {
            VisitedSystemsClass vsc;

            while ( true )
            {
                VisitedSystemsClass nextsys = null;
                VisitedSystemsClass cursys = null;
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

                    ISystem lastSystem = vsc.curSystem;

                    double x, y, z;

                    if (lastSystem == null)
                    {
                        if (!vsc.HasTravelCoordinates) // if not found, or no co-ord
                            break;

                        x = vsc.X;
                        y = vsc.Y;
                        z = vsc.Z;
                    }
                    else
                    {
                        x = lastSystem.x;
                        y = lastSystem.y;
                        z = lastSystem.z;
                    }

                    SystemClass.GetSystemSqDistancesFrom(closestsystemlist, x, y, z, 50, true, 1000);

                    Invoke((MethodInvoker)delegate      // being paranoid about threads..
                    {
                        VisitedSystemsClass.CalculateSqDistances(visitedSystems, closestsystemlist, x, y, z, 50, true);
                    });

                    cursys = vsc;
                } while (closestsystem_queue.TryTake(out vsc));     // if there is another one there, just re-run (slow down doggy!)

                Invoke((MethodInvoker)delegate
                {
                    labelclosests.Text = "";
                    dataGridViewNearest.Rows.Clear();

                    if (closestsystemlist.Count() > 0)
                    {
                        labelclosests.Text = "Closest systems from " + cursys.Name;
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
                closestsystem_queue.Add(currentSysPos);
                closestthread.Join();

            }
        }

        public VisitedSystemsClass CurrentSystemSelected
        {
            get
            {
                if (dataGridViewTravel == null || dataGridViewTravel.CurrentRow == null)
                    return null;
                return ((VisitedSystemsClass)dataGridViewTravel.CurrentRow.Cells[TravelHistoryColumns.SystemName].Tag);
            }
        }


        public ISystem GetLatestSystem
        {
            get
            {
                if (visitedSystems == null || visitedSystems.Count == 0)
                {
                    return null;
                }
                return (from systems in visitedSystems orderby systems.Time descending select systems.curSystem).First();
            }
        }


        private void TravelHistoryControl_Load(object sender, EventArgs e)
        {
            dataGridViewTravel.MakeDoubleBuffered();
        }

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            ignorewidthchange = true;
            if (SQLiteDBClass.keyExists("TravelControlDGVCol1"))        // if stored values, set back to what they were..
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
                Collapse(ref delta, 3);         // pick columns on preference list to shrink
                Collapse(ref delta, 2);
                Collapse(ref delta, 0);
                Collapse(ref delta, 1);
            }
            else
                dataGridViewTravel.Columns[3].Width += delta;   // note is used to fill out columns
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

        public void LoadCommandersListBox()
        {
            comboBoxCommander.Enabled = false;
            commanders = new List<EDCommander>();

            commanders.Add(new EDCommander(-1, "Hidden log", ""));
            commanders.AddRange(EDDiscoveryForm.EDDConfig.listCommanders);

            comboBoxCommander.DataSource = null;
            comboBoxCommander.DataSource = commanders;
            comboBoxCommander.ValueMember = "Nr";
            comboBoxCommander.DisplayMember = "Name";

            EDCommander currentcmdr = EDDiscoveryForm.EDDConfig.CurrentCommander;

            comboBoxCommander.SelectedIndex = commanders.IndexOf(currentcmdr);
            activecommander = currentcmdr.Nr;

            comboBoxCommander.Enabled = true;
        }
                
        private void comboBoxCommander_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCommander.SelectedIndex >= 0 && comboBoxCommander.Enabled )     // DONT trigger during LoadCommandersListBox
            {
                var itm = (EDCommander)comboBoxCommander.SelectedItem;
                activecommander = itm.Nr;
                if (itm.Nr >= 0)
                    EDDiscoveryForm.EDDConfig.CurrentCmdrID = itm.Nr;
                if (visitedSystems != null)
                    visitedSystems.Clear();
                RefreshHistoryAsync();
                if (_discoveryForm.Map != null)
                    _discoveryForm.Map.UpdateVisited(visitedSystems);
            }
        }


        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (visitedSystems != null)
                RefreshHistoryAsync();

            SQLiteDBClass.PutSettingInt("EDUIHistory", comboBoxHistoryWindow.SelectedIndex);
        }


        private void dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewSorter.DataGridSort(dataGridViewTravel, e.ColumnIndex);
            RedrawSummary();
            UpdateDependentsWithSelection();
        }

        public void buttonMap_Click(object sender, EventArgs e)
        {
            if (textBoxTarget.AutoCompleteCustomSource.Count == 0)         // wait till told system names is complete..
            {
                MessageBox.Show("Systems have not been loaded yet or none were available at program start, please wait or restart", "No Systems Available", MessageBoxButtons.OK);
                return;
            }

            var map = _discoveryForm.Map;
            var selectedLine = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                           .Select(cell => cell.OwningRow)
                                                           .OrderBy(row => row.Index)
                                                           .Select(r => (int?)r.Index)
                                                           .FirstOrDefault() ?? -1;
            VisitedSystemsClass selectedSys = null;
            if (selectedLine >= 0)
            {
                do
                {
                    selectedSys = (VisitedSystemsClass)dataGridViewTravel.Rows[selectedLine].Cells[TravelHistoryColumns.SystemName].Tag;
                    selectedLine += 1;
                } while (!selectedSys.curSystem.HasCoordinate && selectedLine < dataGridViewTravel.Rows.Count);
            }

            this.Cursor = Cursors.WaitCursor;

            string HomeSystem = _discoveryForm.settings.MapHomeSystem;

            map.Prepare(selectedSys, _discoveryForm.settings.MapHomeSystem,
                        _discoveryForm.settings.MapCentreOnSelection ? selectedSys?.curSystem : SystemClass.GetSystem(String.IsNullOrEmpty(HomeSystem) ? "Sol" : HomeSystem),
                        _discoveryForm.settings.MapZoom, _discoveryForm.SystemNames, visitedSystems);
            map.Show();
            this.Cursor = Cursors.Default;
        }

        private void dataGridViewTravel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                VisitedSystemsClass currentsys = (VisitedSystemsClass)(dataGridViewTravel.Rows[e.RowIndex].Cells[TravelHistoryColumns.SystemName].Tag);

                ShowSystemInformation(currentsys);
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
                    VisitedSystemsClass currentsys = (VisitedSystemsClass)(dataGridViewTravel.Rows[rowi].Cells[TravelHistoryColumns.SystemName].Tag);
                    _discoveryForm.Map.UpdateHistorySystem(currentsys.Name);
                    _discoveryForm.RouteControl.UpdateHistorySystem(currentsys.Name);
                }
            }
        }

        private void dataGridViewTravel_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                VisitedSystemsClass currentsys = (VisitedSystemsClass)(dataGridViewTravel.Rows[e.RowIndex].Cells[TravelHistoryColumns.SystemName].Tag);
                ShowSystemInformation(currentsys);
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
            if (currentSysPos == null || currentSysPos.curSystem == null)
                return;

            try
            {
                string txt = richTextBoxNote.Text.Trim();

                SystemNoteClass sn = SystemNoteClass.GetSystemNoteClass(currentSysPos.curSystem.name);

                if ( (sn == null && txt.Length>0) || (sn!=null && !sn.Note.Equals(txt))) // if no system note, and text,  or system not is not text
                {
                    if ( sn != null )           // already there, update
                    { 
                        sn.Note = txt;
                        sn.Time = DateTime.Now;
                        sn.Update();
                    }
                    else
                    {
                        sn = new SystemNoteClass();
                        sn.Name = currentSysPos.curSystem.name;
                        sn.Note = txt;
                        sn.Time = DateTime.Now;
                        sn.Add();
                    }

                    if (dataGridViewTravel.SelectedCells.Count > 0)          // if we have selected (we should!)
                        dataGridViewTravel.Rows[dataGridViewTravel.SelectedCells[0].OwningRow.Index].Cells[TravelHistoryColumns.Note].Value = txt;

                    EDSMClass edsm = new EDSMClass();

                    edsm.apiKey = EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey;
                    edsm.commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;

                    if (edsm.commanderName == null || edsm.apiKey == null)
                        return;

                    if (edsm.commanderName.Length>1 && edsm.apiKey.Length>1)
                        edsm.SetComment(sn);

                    _discoveryForm.Map.UpdateNote();
                    RefreshSummaryRow(dataGridViewTravel.Rows[dataGridViewTravel.SelectedCells[0].OwningRow.Index]);    // tell it this row was changed
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);

                LogTextHighlight("Exception : " + ex.Message);
                LogTextHighlight(ex.StackTrace);
            }
        }

        private void buttonSync_Click(object sender, EventArgs e)
        {
            if (EDDiscoveryForm.EDDConfig.CurrentCommander.Name.Equals(""))
            {
                MessageBox.Show("Please enter commander name before sending distances/ travel history to EDSM!");
                return;
            }

            var dists = DistanceClass.GetDistancesByStatus((int)DistancsEnum.EDDiscovery);

            EDSMClass edsm = new EDSMClass();

            foreach (var dist in dists)
            {
                string json;

                if (dist.Dist > 0)
                {
                    LogText("Add distance: " + dist.NameA + " => " + dist.NameB + " :" + dist.Dist.ToString("0.00") + "ly" + Environment.NewLine);
                    json = edsm.SubmitDistances(EDDiscoveryForm.EDDConfig.CurrentCommander.Name, dist.NameA, dist.NameB, dist.Dist);
                }
                else
                {
                    if (dist.Dist < 0)  // Can removedistance by adding negative value
                        dist.Delete();
                    else
                    {
                        dist.Status = DistancsEnum.EDDiscoverySubmitted;
                        dist.Update();
                    }
                    json = null;
                }
                if (json != null)
                {
                    string str="";
                    bool trilok;
                    if (edsm.ShowDistanceResponse(json, out str, out trilok))
                    {
                        LogText("EDSM Response " + str);
                        dist.Status = DistancsEnum.EDDiscoverySubmitted;
                        dist.Update();
                    }
                    else
                    {
                        LogText("EDSM Response " + str);
                    }
                }
            }

            if (EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey.Equals(""))
            {
                MessageBox.Show("Please enter EDSM api key (In settings) before sending travel history to EDSM!");
                return;

            }
            sync.StartSync(EDSMSyncTo, EDSMSyncFrom,defaultMapColour);

        }

        internal void RefreshEDSMEvent(object source)
        {
            Invoke((MethodInvoker)delegate
            {
                if( visitedSystems != null)
                {
                    visitedSystems.Clear();
                }
                RefreshHistoryAsync();
            });
        }

        public void NewPosition(VisitedSystemsClass item)         // in UI Thread..
        {
            Debug.Assert(Application.MessageLoop);              // ensure.. paranoia
            visitedSystems.Add(item);

            try
            {
                string name = item.Name;

                LogText("Arrived at system ");
                if (item.HasTravelCoordinates == false && (item.curSystem == null || item.curSystem.HasCoordinate == false))
                    LogTextHighlight(name);
                else
                    LogText(name);

                int count = GetVisitsCount(name);

                LogText(", Visit No. " + count.ToString() + Environment.NewLine);
                System.Diagnostics.Trace.WriteLine("Arrived at system: " + name + " " + count.ToString() + ":th visit.");

                if (checkBoxEDSMSyncTo.Checked == true)
                {
                    EDSMClass edsm = new EDSMClass();
                    edsm.apiKey = EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey;
                    edsm.commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;

                    Task taskEDSM = Task.Factory.StartNew(() => EDSMSync.SendTravelLog(edsm, item, null));
                }

                AddNewHistoryRow(true, item);

                StoreSystemNote();

                _discoveryForm.Map.UpdateVisited(visitedSystems);           // update map

                RefreshSummaryRow(dataGridViewTravel.Rows[0], true);         //Tell the summary new row has been added
                RefreshTargetInfo();                                        // tell the target system its changed the latest system

                // Move focus to new row
                if (EDDiscoveryForm.EDDConfig.FocusOnNewSystem)
                {
                    dataGridViewTravel.ClearSelection();
                    dataGridViewTravel.CurrentCell = dataGridViewTravel.Rows[0].Cells[1];       // its the current cell which needs to be set, moves the row marker as well
                    ShowSystemInformation(item);
                    UpdateDependentsWithSelection();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception NewPosition: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }

        private int GetVisitsCount(string name)
        {
            try
            {
                int count = (from row in visitedSystems
                             where row.Name == name
                             select row).Count();
                return count;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception GetVisitsCount: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                return 0;
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {           // autopaint the row number..
            var grid = sender as DataGridView;
            string rowIdx;

            if (_discoveryForm.settings.OrderRowsInverted )
                rowIdx = (dataGridViewTravel.Rows.Count - e.RowIndex).ToString();
            else
                rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);

            using ( Brush br = new SolidBrush(grid.RowHeadersDefaultCellStyle.ForeColor))
                e.Graphics.DrawString(rowIdx, grid.RowHeadersDefaultCellStyle.Font, br , headerBounds, centerFormat);
        }

        private void buttonEDDB_Click(object sender, EventArgs e)
        {
            if ( currentSysPos!= null && currentSysPos.curSystem.id_eddb>0)
                Process.Start("http://eddb.io/system/" + currentSysPos.curSystem.id_eddb.ToString());
        }

        private void buttonRoss_Click(object sender, EventArgs e)
        {
            if (currentSysPos!= null && currentSysPos.curSystem.id_eddb>0)
                Process.Start("http://ross.eddb.io/system/update/" + currentSysPos.curSystem.id_eddb.ToString());
        }

        private void buttonEDSM_Click(object sender, EventArgs e)
        {
            if (currentSysPos != null && currentSysPos.curSystem != null) // solve a possible exception
            {
                if (!String.IsNullOrEmpty(currentSysPos.curSystem.name))
                {
                    long? id_edsm = currentSysPos.curSystem.id_edsm;
                    if (id_edsm == 0)
                    {
                        id_edsm = null;
                    }

                    EDSMClass edsm = new EDSMClass();
                    string url = edsm.GetUrlToEDSMSystem(currentSysPos.curSystem.name, id_edsm);

                    if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                        Process.Start(url);
                    else
                        MessageBox.Show("System unknown to EDSM");
                }
            }
        }

        public string GetCommanderName()
        {
            var value = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;
            return !string.IsNullOrEmpty(value) ? value : null;
        }

        private void textBoxFilter_KeyUp(object sender, KeyEventArgs e)
        {
            FilterGridView();
            RedrawSummary();
            UpdateDependentsWithSelection();
        }

        private void FilterGridView()
        {
            string searchstr = textBoxFilter.Text.Trim();
            dataGridViewTravel.SuspendLayout();

            DataGridViewRow[] theRows = new DataGridViewRow[dataGridViewTravel.Rows.Count];
            dataGridViewTravel.Rows.CopyTo(theRows, 0);
            dataGridViewTravel.Rows.Clear();

            for (int loop = 0; loop < theRows.Length; loop++)
            {
                bool found = false;

                if (searchstr.Length < 1)
                    found = true;
                else
                {
                    foreach (DataGridViewCell cell in theRows[loop].Cells)
                    {
                        if (cell.Value != null)
                            if (cell.Value.ToString().IndexOf(searchstr, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                            {
                                found = true;
                                break;
                            }
                    }
                }
                theRows[loop].Visible = found;
            }
            dataGridViewTravel.Rows.AddRange(theRows);
            dataGridViewTravel.ResumeLayout();
        }


        private void textBoxPrevSystem_Enter(object sender, EventArgs e)
        {
            /* Automatically copy the contents to the clipboard whenever this control is activated */
            TextBox tb = sender as TextBox;
            if (tb != null && tb.Text != null)
            {
                Clipboard.SetText(tb.Text);
            }
        }

        private void checkBoxEDSMSyncTo_CheckedChanged(object sender, EventArgs e)
        {
            EDSMSyncTo = checkBoxEDSMSyncTo.Checked;
        }

        private void checkBoxEDSMSyncFrom_CheckedChanged(object sender, EventArgs e)
        {
            EDSMSyncFrom = checkBoxEDSMSyncFrom.Checked;
        }

        private void button2DMap_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FormSagCarinaMission frm = new FormSagCarinaMission(_discoveryForm);
            frm.Nowindowreposition = _discoveryForm.option_nowindowreposition;
            frm.Show();
            this.Cursor = Cursors.Default;
        }
        
        public bool SetTravelHistoryPosition(string sysname)
        {
            foreach (DataGridViewRow item in dataGridViewTravel.Rows)
            {
                string s = (string)item.Cells[TravelHistoryColumns.SystemName].Value;
                if (s.Equals(sysname) && item.Visible)
                {
                    dataGridViewTravel.ClearSelection();
                    item.Selected = true;           // select row
                    dataGridViewTravel.CurrentCell = item.Cells[TravelHistoryColumns.SystemName];       // and ensure visible.
                    return true;
                }
            }

            return false;
        }

        private void textBoxTarget_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string sn = textBoxTarget.Text;
                SystemClass sc = SystemClass.GetSystem(sn);
                VisitedSystemsClass vsc = visitedSystems.Find(x => x.Name.Equals(sn, StringComparison.InvariantCultureIgnoreCase));
                string msgboxtext = null;

                if ( (sc != null && sc.HasCoordinate) || ( vsc != null && vsc.HasTravelCoordinates))
                {
                    if (sc == null)
                        sc = new SystemClass(vsc.Name, vsc.X, vsc.Y, vsc.Z);            // make a double for the rest of the code..

                    SystemNoteClass nc = SystemNoteClass.GetSystemNoteClass(sn);        // has it got a note?

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
                    msgboxtext = "Unknown system or system without co-ordinates";

                RefreshTargetInfo();
                if (_discoveryForm.Map != null)
                    _discoveryForm.Map.UpdateBookmarks();

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

                SystemClass cs = VisitedSystemsClass.GetSystemClassFirstPosition(visitedSystems);
                if ( cs != null )
                    textBoxTargetDist.Text = SystemClass.Distance(cs, x, y, z).ToString("0.00");

                toolTipEddb.SetToolTip(textBoxTarget, "Position is " + x.ToString("0.00") + "," + y.ToString("0.00") + "," + z.ToString("0.00"));
            }
            else
            {
                textBoxTarget.Text = "Set target";
                textBoxTargetDist.Text = "";
                toolTipEddb.SetToolTip(textBoxTarget, "On 3D Map right click to make a bookmark, region mark or click on a notemark and then tick on Set Target, or type it here and hit enter");
            }

            if (summaryPopOut != null)
                summaryPopOut.RefreshTarget(dataGridViewTravel,visitedSystems);
        }

        #endregion 

        #region Summary Pop out

        public bool IsSummaryPopOutOn {  get { return summaryPopOut != null; } }
        public bool ToggleSummaryPopOut()
        {
            if (summaryPopOut == null )
            {
                summaryPopOut = new SummaryPopOut();
                summaryPopOut.RequiresRefresh += SummaryRefreshRequested;
                RedrawSummary();
                summaryPopOut.Show();
            }
            else
            { 
                summaryPopOut.Close();
                summaryPopOut = null;
            }

            return (summaryPopOut != null);     // on screen?
        }

        public void RedrawSummary()
        {
            if (summaryPopOut != null)
            {
                summaryPopOut.SetGripperColour(_discoveryForm.theme.LabelColor);
                summaryPopOut.ResetForm(dataGridViewTravel);
                summaryPopOut.RefreshTarget(dataGridViewTravel, visitedSystems);
            }
        }

        public void SummaryRefreshRequested(Object o, EventArgs e)
        {
            RedrawSummary();
        }

        public void RefreshSummaryRow(DataGridViewRow row , bool add = false )
        {
            if (summaryPopOut != null)
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
                LogTextHighlight("System could not be found - has not been synched or EDSM is unavailable" + Environment.NewLine);

            this.Cursor = Cursors.Default;
        }

        #endregion

        #region TravelHistoryRightClick

        VisitedSystemsClass rightclicksystem = null;
        int rightclickrow = -1;

        private void dataGridViewTravel_MouseDown(object sender, MouseEventArgs e)  // MAKES row selected when mouse down
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                rightclicksystem = null;
                rightclickrow = -1;

                if (dataGridViewTravel.SelectedCells.Count < 2 || dataGridViewTravel.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
                {
                    DataGridView.HitTestInfo hti = dataGridViewTravel.HitTest(e.X, e.Y);
                    if (hti.Type == DataGridViewHitTestType.Cell)
                    {
                        dataGridViewTravel.ClearSelection();                // select row under cursor.
                        dataGridViewTravel.Rows[hti.RowIndex].Selected = true;
                                                                            // Record who we clicked on.. only way to tell opening..
                        rightclickrow = hti.RowIndex;
                        rightclicksystem = (VisitedSystemsClass)dataGridViewTravel.Rows[hti.RowIndex].Cells[TravelHistoryColumns.SystemName].Tag;
                    }
                }
            }
        }

        private void historyContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewTravel.SelectedCells.Count == 0)      // need something selected  stops context menu opening on nothing..
                e.Cancel = true;

            enterDistanceToPreviousStarToolStripMenuItem.Enabled = (rightclicksystem != null && rightclicksystem.prevSystem != null && (!rightclicksystem.curSystem.HasCoordinate || !rightclicksystem.prevSystem.HasCoordinate));
            mapGotoStartoolStripMenuItem.Enabled = (rightclicksystem != null && rightclicksystem.curSystem.HasCoordinate);
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
                if (_discoveryForm.Map.MoveToSystem(rightclicksystem))
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
            mapColorDialog.Color = selectedRows.First().Cells[TravelHistoryColumns.Map].Style.ForeColor;

            if (mapColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                string sysName = "";
                foreach (DataGridViewRow r in selectedRows)
                {
                    r.Cells[TravelHistoryColumns.Map].Style.ForeColor = mapColorDialog.Color;
                    sysName = r.Cells[TravelHistoryColumns.SystemName].Value.ToString();

                    VisitedSystemsClass sp = null;
                    sp = (VisitedSystemsClass)r.Cells[TravelHistoryColumns.SystemName].Tag;
                    if (sp == null)
                        sp = visitedSystems.First(s => s.Name.ToUpperInvariant() == sysName.ToUpperInvariant());

                    {
                        sp.MapColour = mapColorDialog.Color.ToArgb();
                        sp.Update();
                    }
                }
                this.Cursor = Cursors.Default;
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
                VisitedSystemsClass sp = null;
                sp = (VisitedSystemsClass)r.Cells[TravelHistoryColumns.SystemName].Tag;

                if (sp != null)
                {
                    sp.Commander = -1;
                    sp.Update();
                }
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

            List<VisitedSystemsClass> listsyspos = new List<VisitedSystemsClass>();

            this.Cursor = Cursors.WaitCursor;
            foreach (DataGridViewRow r in selectedRows)
            {
                VisitedSystemsClass sp = null;

                sp = (VisitedSystemsClass)r.Cells[TravelHistoryColumns.SystemName].Tag;
                if (sp != null)
                {
                    listsyspos.Add(sp);
                }
            }

            MoveToCommander movefrm = new MoveToCommander();

            movefrm.Init(listsyspos.Count > 1);

            DialogResult red = movefrm.ShowDialog();
            if (red == DialogResult.OK)
            {
                foreach (VisitedSystemsClass sp in listsyspos)
                {
                    sp.Commander = movefrm.selectedCommander.Nr;
                    sp.Update();
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
            string sysName = "";
            foreach (DataGridViewRow r in selectedRows)
            {
                sysName = r.Cells[TravelHistoryColumns.SystemName].Value.ToString();
                tctrl.AddWantedSystem(sysName);
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
            string sysName = "";
            foreach (DataGridViewRow r in selectedRows)
            {
                sysName = r.Cells[TravelHistoryColumns.SystemName].Value.ToString();
                tctrl.AddSystemToDataGridViewDistances(sysName);
                tctrl.AddWantedSystem(sysName);
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
            string sysName = "";
            foreach (DataGridViewRow r in selectedRows)
            {
                sysName = r.Cells[TravelHistoryColumns.SystemName].Value.ToString();
                tctrl.AddSystemToDataGridViewDistances(sysName);
            }

            this.Cursor = Cursors.Default;
        }

        // enabled only if rightclick system is set.
        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EDSMClass edsm = new EDSMClass();
            long? id_edsm = rightclicksystem.curSystem?.id_edsm;

            if (id_edsm == 0)
            {
                id_edsm = null;
            }

            if (!edsm.ShowSystemInEDSM(rightclicksystem.Name, id_edsm))
                LogTextHighlight("System could not be found - has not been synched or EDSM is unavailable" + Environment.NewLine);

            this.Cursor = Cursors.Default;
        }

        // enabled only if rightclick system is set.
        private void enterDistanceToPreviousStarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DistanceForm frm = new DistanceForm();
            DialogResult res = frm.ShowDialog();

            if ( res == DialogResult.OK )
            {
                var dist = DistanceParser.ParseJumpDistance(frm.Value.Trim());

                if (!dist.HasValue)
                    MessageBox.Show("Distance in wrong format!");
                else
                {
                    DistanceClass distance = DistanceClass.GetDistanceClass(rightclicksystem.curSystem, rightclicksystem.prevSystem);
                    DistanceClass dstore = null;

                    if (distance == null)
                    {
                        dstore = new DistanceClass();
                        dstore.NameA = rightclicksystem.curSystem.name;
                        dstore.NameB = rightclicksystem.prevSystem.name;
                    }
                    else
                        dstore = distance;

                    dstore.Dist = dist.Value;
                    dstore.CreateTime = DateTime.UtcNow;
                    dstore.CommanderCreate = EDDiscoveryForm.EDDConfig.CurrentCommander.Name.Trim();
                    dstore.Status = DistancsEnum.EDDiscovery;

                    if (distance != null)
                        dstore.Update();
                    else
                        dstore.Store();

                    dataGridViewTravel.Rows[rightclickrow].Cells[TravelHistoryColumns.Distance].Value = frm.Value.Trim();
                }
            }

        }

        private void selectCorrectSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Forms.AssignTravelLogSystemForm form = new Forms.AssignTravelLogSystemForm(this, rightclicksystem))
            {
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    rightclicksystem.id_edsm_assigned = form.AssignedEdsmId;
                    rightclicksystem.curSystem = form.AssignedSystem;
                    rightclicksystem.Update();
                    RefreshHistoryAsync();
                }
            }
        }

        #endregion
    }
}
