using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Collections;
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

        internal List<VisitedSystemsClass> visitedSystems;
        internal bool EDSMSyncTo = true;
        internal bool EDSMSyncFrom = true;

        public NetLogClass netlog = new NetLogClass();
        private VisitedSystemsClass currentSysPos = null;


        private int activecommander = 0;
        List<EDCommander> commanders = null;
        
        public TravelHistoryControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
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
            visitedSystems = null;

            TriggerEDSMRefresh();
            LogText("Refresh History." + Environment.NewLine);
            RefreshHistory();
            LogText("Refresh Complete." + Environment.NewLine);

            EliteDangerous.CheckED();
        }

        public void TriggerEDSMRefresh()
        {
            LogText("Check for new EDSM systems." + Environment.NewLine);
            EDSMClass edsm = new EDSMClass();
            edsm.GetNewSystems();
            LogText("EDSM System check complete." + Environment.NewLine);
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

        public void RefreshHistory()
        {
            if (visitedSystems == null || visitedSystems.Count == 0)
                GetVisitedSystems();

            if (visitedSystems == null)
                return;

            VisitedSystemsClass.UpdateSys(visitedSystems, EDDiscoveryForm.EDDConfig.UseDistances);

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
        }

        private void GetVisitedSystems()
        {                                                       // for backwards compatibility, don't store RGB value.
            if (activecommander >= 0)
            {
                visitedSystems = netlog.ParseFiles(richTextBox_History, defaultMapColour);
            }
            else
            {
                visitedSystems = VisitedSystemsClass.GetAll(activecommander);
            }
        }


        private void AddNewHistoryRow(bool insert, VisitedSystemsClass item)            // second part of add history row, adds item to view.
        {
            object[] rowobj = { item.Time, item.Name, item.strDistance, SystemNoteClass.GetSystemNoteOrEmpty(item.curSystem.name), "█" };
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

            closestsystem_queue.Add(syspos.Name);
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
        BlockingCollection<string> closestsystem_queue = new BlockingCollection<string>();
        SortedList<double, string> closestsystemlist = new SortedList<double, string>(new DuplicateKeyComparer<double>()); //lovely list allowing duplicate keys - can only iterate in it.

        private void CalculateClosestSystems()
        {
            string closestname;

            while ( true )
            {
                closestname = closestsystem_queue.Take();           // block until got one..

                string namecalc="!";                    // need to keep the name as TryTake empties the string
                closestsystemlist.Clear();

                do
                {
                    string nextname;
                    while (closestsystem_queue.TryTake(out nextname))    // try and empty the queue in case multiple ones are there
                    {
                        //Console.WriteLine("Chuck " + closestname);
                        closestname = nextname;
                    }

                    SystemClass lastSystem = SystemClass.GetSystem(closestname);

                    double x, y, z;

                    if (lastSystem == null)
                    {
                        VisitedSystemsClass vsc = null;

                        Invoke((MethodInvoker)delegate      // being paranoid about threads..
                        {
                            vsc = visitedSystems.Find(q => q.Name.Equals(closestname));
                        });

                        if (vsc == null || !vsc.HasTravelCoordinates) // if not found, or no co-ord
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


                    SystemClass.GetSystemSqDistancesFrom(closestsystemlist, x, y, z, 50, true);

                    Invoke((MethodInvoker)delegate      // being paranoid about threads..
                    {
                        VisitedSystemsClass.CalculateSqDistances(visitedSystems, closestsystemlist, x, y, z, 50, true);
                    });

                    namecalc = closestname;

                } while (closestsystem_queue.TryTake(out closestname));     // if there is another one there, just re-run (slow down doggy!)

                Invoke((MethodInvoker)delegate
                {
                    labelclosests.Text = "";
                    dataGridViewNearest.Rows.Clear();

                    if (closestsystemlist.Count() > 0)
                    {
                        labelclosests.Text = "Closest systems from " + namecalc;
                        foreach (KeyValuePair<double, string> tvp in closestsystemlist)
                        {
                            object[] rowobj = { tvp.Value, Math.Sqrt(tvp.Key).ToString("0.00") };       // distances are stored squared for speed, back to normal.
                            dataGridViewNearest.Rows.Add(rowobj);
                        }
                    }
                });
            }
        }


        public VisitedSystemsClass CurrentSystemSelected
        {
            get
            {
                if (dataGridViewTravel == null || dataGridViewTravel.CurrentRow == null) return null;
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
                RefreshHistory();
                if (_discoveryForm.Map != null)
                    _discoveryForm.Map.UpdateVisited(visitedSystems);
            }
        }


        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (visitedSystems != null)
                RefreshHistory();

            SQLiteDBClass.PutSettingInt("EDUIHistory", comboBoxHistoryWindow.SelectedIndex);
        }


        private void dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewSorter.DataGridSort(dataGridViewTravel, e.ColumnIndex);
        }

        public void buttonMap_Click(object sender, EventArgs e)
        {
            if (_discoveryForm.SystemNames.Count == 0)
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

            string selname = (selectedSys != null && selectedSys.curSystem.HasCoordinate) ? selectedSys.Name : textBoxSystem.Text.Trim();
            map.Prepare(selname, _discoveryForm.settings.MapHomeSystem,
                        _discoveryForm.settings.MapCentreOnSelection ? selname : _discoveryForm.settings.MapHomeSystem,
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
                _discoveryForm.Map.UpdateHistorySystem(currentsys.Name);

                if (e.ColumnIndex == TravelHistoryColumns.Note)
                {
                    richTextBoxNote.TextBox.Select(richTextBoxNote.Text.Length, 0);     // move caret to end and focus.
                    richTextBoxNote.TextBox.ScrollToCaret();
                    richTextBoxNote.TextBox.Focus();
                }
            }
        }

        private void dataGridViewTravel_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                VisitedSystemsClass currentsys = (VisitedSystemsClass)(dataGridViewTravel.Rows[e.RowIndex].Cells[TravelHistoryColumns.SystemName].Tag);
                ShowSystemInformation(currentsys);
                _discoveryForm.Map.UpdateHistorySystem(currentsys.Name);
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
            string txt;

            try
            {
                EDSMClass edsm = new EDSMClass();
                
                edsm.apiKey = EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey;
                edsm.commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;
                
                if (currentSysPos == null || currentSysPos.curSystem == null)
                    return;

                txt = richTextBoxNote.Text;

                SystemNoteClass sn = SystemNoteClass.GetSystemNoteClass(currentSysPos.curSystem.name);

                if (currentSysPos != null && ( sn == null || !sn.Note.Equals(txt))) // if current system pos set, or no note or text change
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

                    if (edsm.commanderName == null || edsm.apiKey == null)
                        return;

                    if (edsm.commanderName.Length>1 && edsm.apiKey.Length>1)
                        edsm.SetComment(sn);

                    _discoveryForm.Map.UpdateNote();
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
                visitedSystems.Clear();
                RefreshHistory();
            });
        }


        internal void NewPosition(object source)            // Called from netlog thread beware.
        {
            try
            {
                string name = netlog.visitedSystems.Last().Name;
                Invoke((MethodInvoker)delegate
                {
                    UpdateNewPosition(name);

                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception NewPosition: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }

        private void UpdateNewPosition(string name)         // in UI Thread..
        {
            var result = visitedSystems.OrderByDescending(a => a.Time).ToList<VisitedSystemsClass>();
            
            VisitedSystemsClass item = result[0];
            VisitedSystemsClass item2;

            if (result.Count > 1)
                item2 = result[1];
            else
                item2 = null;

            VisitedSystemsClass.UpdateVisitedSystemsEntries(item, item2, EDDiscoveryForm.EDDConfig.UseDistances);       // ensure they have system classes behind them..

            LogText("Arrived at system ");
            if ( item.HasTravelCoordinates == false && ( item.curSystem == null || item.curSystem.HasCoordinate == false) )
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

            _discoveryForm.Map.UpdateVisited(visitedSystems);      // update map

            // Move focus to new row
            if (EDDiscoveryForm.EDDConfig.FocusOnNewSystem)
            {
                dataGridViewTravel.ClearSelection();
                dataGridViewTravel.Rows[0].Cells[0].Selected = true; // This won't raise the CellClick handler, which updates the rest of the form
                dataGridViewTravel_CellClick(dataGridViewTravel, new DataGridViewCellEventArgs(0, 0));
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
                    EDSMClass edsm = new EDSMClass();
                    string url = edsm.GetUrlToEDSMSystem(currentSysPos.curSystem.name);

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
            string sysName = selectedRows.First<DataGridViewRow>().Cells[ClosestSystemsColumns.SystemName].Value.ToString();
            EDSMClass edsm = new EDSMClass();
            if (!edsm.ShowSystemInEDSM(sysName))
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
                if (_discoveryForm.Map.MoveToSystem(rightclicksystem.Name))
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

            if (!edsm.ShowSystemInEDSM(rightclicksystem.Name))
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

        #endregion

    }

}
