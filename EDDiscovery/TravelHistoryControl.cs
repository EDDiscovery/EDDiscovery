﻿using System;
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

        private const int DefaultTravelHistoryFilterIndex = 4;
        private const string SingleCoordinateFormat = "0.#####";

        private static EDDiscoveryForm _discoveryForm;
        public int defaultMapColour;
        public EDSMSync sync;

        internal List<VisitedSystemsClass> visitedSystems;
        internal bool EDSMSyncTo = true;
        internal bool EDSMSyncFrom = true;

        public NetLogClass netlog = new NetLogClass();
        List<SystemDist> sysDist = null;
        private VisitedSystemsClass currentSysPos = null;


        private static ExtendedControls.RichTextBoxScroll static_richTextBox;
        private int activecommander = 0;
        List<EDCommander> commanders = null;
        
        public TravelHistoryControl()
        {
            InitializeComponent();
            static_richTextBox = richTextBox_History;
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            sync = new EDSMSync(_discoveryForm);
            var db = new SQLiteDBClass();
            defaultMapColour = EDDConfig.Instance.DefaultMapColour;
            EDSMSyncTo = db.GetSettingBool("EDSMSyncTo", true);
            EDSMSyncFrom = db.GetSettingBool("EDSMSyncFrom", true);
            checkBoxEDSMSyncTo.Checked = EDSMSyncTo;
            checkBoxEDSMSyncFrom.Checked = EDSMSyncFrom;
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

            comboBoxHistoryWindow.DisplayMember = nameof(TravelHistoryFilter.Label);

            comboBoxHistoryWindow.SelectedIndex = db.GetSettingInt("EDUIHistory", DefaultTravelHistoryFilterIndex);
            LoadCommandersListBox();
        }


        private void button_RefreshHistory_Click(object sender, EventArgs e)
        {
            visitedSystems = null;

            TriggerEDSMRefresh();
            RefreshHistory();


            EliteDangerous.CheckED();
        }

        public void TriggerEDSMRefresh()
        {
            SQLiteDBClass db = new SQLiteDBClass();
            EDSMClass edsm = new EDSMClass();
            edsm.GetNewSystems(db);
            db.GetAllSystems();
        }


        static public void LogText(string text)
        {
            LogTextColor(text, _discoveryForm.theme.TextBlockColor );
        }

        static public void LogTextHighlight(string text)
        {
            LogTextColor(text, _discoveryForm.theme.TextBlockHighlightColor);
        }

        static public void LogTextSuccess(string text)
        {
            LogTextColor(text, _discoveryForm.theme.TextBlockSuccessColor);
        }

        static public void LogTextColor( string text, Color color)
        {
            static_richTextBox.AppendText(text, color);
        }

        public void RefreshHistory()
        {
            var sw1 = Stopwatch.StartNew();

            if (visitedSystems == null || visitedSystems.Count == 0)
                GetVisitedSystems();

            if (visitedSystems == null)
                return;

            for (int i = 0; i < visitedSystems.Count; i++)  // added, to make sure cursystem is filled in
            {
                UpdateVisitedSystemsEntries(visitedSystems[i], (i < visitedSystems.Count - 1) ? visitedSystems[i + 1] : null);
            }

            var filter = (TravelHistoryFilter) comboBoxHistoryWindow.SelectedItem ?? TravelHistoryFilter.NoFilter;
            List<VisitedSystemsClass> result = filter.Filter(visitedSystems);

            dataGridViewTravel.Rows.Clear();

            //            System.Diagnostics.Trace.WriteLine("SW1: " + (sw1.ElapsedMilliseconds / 1000.0).ToString("0.000"));

            for (int ii = 0; ii < result.Count; ii++) //foreach (var item in result)
            {
                VisitedSystemsClass item = result[ii];
                AddNewHistoryRow(false, item);      // for every one in filter, add a row.
            }

            //            System.Diagnostics.Trace.WriteLine("SW2: " + (sw1.ElapsedMilliseconds / 1000.0).ToString("0.000"));

            if (dataGridViewTravel.Rows.Count > 0)
            {
                ShowSystemInformation((VisitedSystemsClass)(dataGridViewTravel.Rows[0].Cells[TravelHistoryColumns.SystemName].Tag));
            }
//            System.Diagnostics.Trace.WriteLine("SW3: " + (sw1.ElapsedMilliseconds / 1000.0).ToString("0.000"));
            sw1.Stop();

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

        private void UpdateVisitedSystemsEntries(VisitedSystemsClass item, VisitedSystemsClass item2)           // this is a split in two version with the same code of AddHistoryRow..
        {
            SystemClass sys1 = null, sys2;                                                                      // fills in cursystem and prevsystem, and calcs distance
            double dist;

            sys1 = SystemData.GetSystem(item.Name);
            if (sys1 == null)
            {
                sys1 = new SystemClass(item.Name);
                if (SQLiteDBClass.globalSystemNotes.ContainsKey(sys1.SearchName))
                {
                    sys1.Note = SQLiteDBClass.globalSystemNotes[sys1.SearchName].Note;
                }
                if (item.HasTravelCoordinates)
                {
                    sys1.x = item.X;
                    sys1.y = item.Y;
                    sys1.z = item.Z;
                }
            }
            if (item2 != null)
            {
                sys2 = SystemData.GetSystem(item2.Name);
                if (sys2 == null)
                {
                    sys2 = new SystemClass(item2.Name);
                    if (item2.HasTravelCoordinates)
                    {
                        sys2.x = item2.X;
                        sys2.y = item2.Y;
                        sys2.z = item2.Z;
                    }
                }
            }
            else
                sys2 = null;

            item.curSystem = sys1;
            item.prevSystem = sys2;

            string diststr = "";
            dist = 0;
            if (sys2 != null)
            {
                if (sys1.HasCoordinate && sys2.HasCoordinate)
                    dist = SystemData.Distance(sys1, sys2);
                else
                {
                    dist = DistanceClass.Distance(sys1, sys2);
                }

                if (dist > 0)
                    diststr = dist.ToString("0.00");
            }

            item.strDistance = diststr;
        }

        private void AddNewHistoryRow(bool insert, VisitedSystemsClass item)            // second part of add history row, adds item to view.
        { 
            object[] rowobj = { item.Time, item.Name, item.strDistance, item.curSystem.Note, "█" };
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
            textBoxPrevSystem.Clear();
            textBoxDistance.Text = syspos.strDistance;


            if (syspos.curSystem.HasCoordinate)
            {
                textBoxX.Text = syspos.curSystem.x.ToString(SingleCoordinateFormat);
                textBoxY.Text = syspos.curSystem.y.ToString(SingleCoordinateFormat);
                textBoxZ.Text = syspos.curSystem.z.ToString(SingleCoordinateFormat);

                textBoxSolDist.Text = Math.Sqrt(syspos.curSystem.x * syspos.curSystem.x + syspos.curSystem.y * syspos.curSystem.y + syspos.curSystem.z * syspos.curSystem.z).ToString("0.00");
            }
            else if (syspos.X == 0.0 && syspos.Y == 0.0 && syspos.Z == 0.0)
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
            richTextBoxNote.Text = EnumStringFormat(syspos.curSystem.Note);

            bool distedit = false;

            if (syspos.prevSystem != null)
            {
                textBoxPrevSystem.Text = syspos.prevSystem.name;

                if (syspos.curSystem.status == SystemStatusEnum.Unknown || syspos.prevSystem.status == SystemStatusEnum.Unknown)
                    distedit = true;

            }

            textBoxDistance.Enabled = distedit;
            buttonUpdate.Enabled = distedit;
            buttonTrilaterate.Enabled = !syspos.curSystem.HasCoordinate && syspos.curSystem == GetCurrentSystem();
            //buttonTrilaterate.Enabled = true; // FIXME for debugging only


            ShowClosestSystems(syspos.Name);
        }

        private string EnumStringFormat(string str)
        {
            if (str == null)
                return "";
            if (str.Equals("Unknown"))
                return "";

            return str.Replace("_", " ");
        }

        private void ShowClosestSystems(string name)
        {
            Debug.Assert(name != null && name.Length != 0);

            labelclosests.Text = "";
            dataGridViewNearest.Rows.Clear();

            SystemClass lastSystem = SystemData.GetSystem(name);

            double x, y, z;

            if ( lastSystem == null )
            {
                VisitedSystemsClass vsc = visitedSystems.Find(q => q.Name.Equals(name) );

                if (vsc == null || !vsc.HasTravelCoordinates) // if not found, or no co-ord
                    return;

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

            labelclosests.Text = "Closest systems from " + name;

            sysDist = new List<SystemDist>();
            double dist;
            double dx, dy, dz;

            foreach (SystemClass pos in SystemData.SystemList)
            {
                dx = (pos.x - x);
                dy = (pos.y - y);
                dz = (pos.z - z);
                dist = dx * dx + dy * dy + dz * dz;

                if (dist > 0)
                {
                    SystemDist sdist = new SystemDist();
                    sdist.name = pos.name;
                    sdist.dist = Math.Sqrt(dist);
                    sysDist.Add(sdist);
                }
            }

            foreach (VisitedSystemsClass pos in visitedSystems)
            {
                if (pos.HasTravelCoordinates && SystemData.GetSystem(name) == null)
                {
                    dx = (pos.X - x);
                    dy = (pos.Y - y);
                    dz = (pos.Z - z);
                    dist = dx * dx + dy * dy + dz * dz;

                    if (dist > 0)
                    {
                        SystemDist sdist = new SystemDist();
                        sdist.name = pos.Name;
                        sdist.dist = Math.Sqrt(dist);
                        sysDist.Add(sdist);
                    }
                }
            }

            var list = (from t in sysDist orderby t.dist select t).Take(50);

            foreach (SystemDist sdist in list)
            {
                object[] rowobj = { sdist.name, sdist.dist.ToString("0.00") };
                dataGridViewNearest.Rows.Add(rowobj);
            }
        }


        public ISystem GetCurrentSystem()
        {
            if (visitedSystems == null || visitedSystems.Count == 0)
            {
                return null;
            }
            return (from systems in visitedSystems orderby systems.Time descending select systems.curSystem).First();
        }


        private void TravelHistoryControl_Load(object sender, EventArgs e)
        {
           dataGridViewTravel.MakeDoubleBuffered();
        }

        private void LoadCommandersListBox()
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
            if (comboBoxCommander.SelectedIndex >= 0 )
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

            var db = new SQLiteDBClass();
            db.PutSettingInt("EDUIHistory", comboBoxHistoryWindow.SelectedIndex);
        }


        private void dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewSorter.DataGridSort(dataGridViewTravel, e.ColumnIndex);
        }

        private void buttonMap_Click(object sender, EventArgs e)
        {
            if (_discoveryForm.SystemNames.Count == 0)
            {
                MessageBox.Show("Systems have not been loaded yet, please wait", "No Systems Available", MessageBoxButtons.OK);
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

            string selname = (selectedSys != null && selectedSys.curSystem.HasCoordinate) ? selectedSys.Name : textBoxSystem.Text.Trim();
            map.Prepare(selname, _discoveryForm.settings.MapHomeSystem,
                        _discoveryForm.settings.MapCentreOnSelection ? selname : _discoveryForm.settings.MapHomeSystem,
                        _discoveryForm.settings.MapZoom, _discoveryForm.SystemNames, visitedSystems);
            map.Show();
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
                else  if (e.ColumnIndex == TravelHistoryColumns.Distance && textBoxDistance.Enabled == true )       // distance column and on..
                {
                    textBoxDistance.Select(textBoxDistance.Text.Length, 0);     // move caret to end (in case something is there) and focus
                    textBoxDistance.Focus();
                }
            }

        }

        private void dataGridViewTravel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                if (dataGridViewTravel.SelectedCells.Count < 2 || dataGridViewTravel.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
                {
                    DataGridView.HitTestInfo hti = dataGridViewTravel.HitTest(e.X, e.Y);
                    if (hti.Type == DataGridViewHitTestType.Cell)
                    {
                        dataGridViewTravel.ClearSelection();                // select row under cursor.
                        dataGridViewTravel.Rows[hti.RowIndex].Selected = true;
                    }
                }
            }
        }

        private void historyContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewTravel.SelectedCells.Count == 0)      // need something selected  stops context menu opening on nothing..
                e.Cancel = true;
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

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            var dist = DistanceParser.ParseJumpDistance(textBoxDistance.Text.Trim());

            if (!dist.HasValue)
                MessageBox.Show("Distance in wrong format!");
            else
            {
                DistanceClass distance = new DistanceClass();

                distance.Dist = dist.Value;
                distance.CreateTime = DateTime.UtcNow;
                distance.CommanderCreate = EDDiscoveryForm.EDDConfig.CurrentCommander.Name.Trim();
                distance.NameA = textBoxSystem.Text;
                distance.NameB = textBoxPrevSystem.Text;
                distance.Status = DistancsEnum.EDDiscovery;

                distance.Store();
                SQLiteDBClass.AddDistanceToCache(distance);

                if (dataGridViewTravel.SelectedCells.Count > 0)          // if we have selected (we should!)
                    dataGridViewTravel.Rows[dataGridViewTravel.SelectedCells[0].OwningRow.Index].Cells[TravelHistoryColumns.Distance].Value = textBoxDistance.Text.Trim();

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
                SQLiteDBClass db = new SQLiteDBClass();


                edsm.apiKey = EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey;
                edsm.commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;


                if (currentSysPos == null || currentSysPos.curSystem == null)
                    return;

                txt = richTextBoxNote.Text;


                if (currentSysPos.curSystem.Note == null)
                    currentSysPos.curSystem.Note = "";

                if (currentSysPos != null && !txt.Equals(currentSysPos.curSystem.Note))
                {
                    SystemNoteClass sn;

                    if (SQLiteDBClass.globalSystemNotes.ContainsKey(currentSysPos.curSystem.SearchName))
                    {
                        sn = SQLiteDBClass.globalSystemNotes[currentSysPos.curSystem.SearchName];
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

                    currentSysPos.curSystem.Note = txt;

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
            var db = new SQLiteDBClass();

            var dists = db.GetDistancesByStatus((int)DistancsEnum.EDDiscovery);
            //var dists = from p in SQLiteDBClass.dictDistances where p.Value.Status == DistancsEnum.EDDiscovery  orderby p.Value.CreateTime  select p.Value;

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
                        LogText(str);
                        dist.Status = DistancsEnum.EDDiscoverySubmitted;
                        dist.Update();
                    }
                    else
                    {
                        LogText(str);
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

        private void UpdateNewPosition(string name)
        {
            LogText("Arrived at system: ");
            SystemClass sys1 = SystemData.GetSystem(name);
            if (sys1 == null || sys1.HasCoordinate == false)
                LogTextHighlight(name);
            else
                LogText(name);

            int count = GetVisitsCount(name);

            LogText(": Visit No. " + count.ToString() + Environment.NewLine);
            System.Diagnostics.Trace.WriteLine("Arrived at system: " + name + " " + count.ToString() + ":th visit.");

            var result = visitedSystems.OrderByDescending(a => a.Time).ToList<VisitedSystemsClass>();

            

            VisitedSystemsClass item = result[0];
            VisitedSystemsClass item2;

            if (result.Count > 1)
                item2 = result[1];
            else
                item2 = null;

            if (checkBoxEDSMSyncTo.Checked == true)
            {
                EDSMClass edsm = new EDSMClass();
                edsm.apiKey = EDDiscoveryForm.EDDConfig.CurrentCommander.APIKey;
                edsm.commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;

                Task taskEDSM = Task.Factory.StartNew(() => EDSMSync.SendTravelLog(edsm, item, null));
            }

            // grab distance to next (this) system
            textBoxDistanceToNextSystem.Enabled = false;
            if (textBoxDistanceToNextSystem.Text.Length > 0 && item2 != null)
            {
                SystemClass currentSystem = null, previousSystem = null;
                SystemData.SystemList.ForEach(s =>
                {
                    if (s.name == item.Name) currentSystem = s;
                    if (s.name == item2.Name) previousSystem = s;
                });

                if (currentSystem == null || previousSystem == null || !currentSystem.HasCoordinate || !previousSystem.HasCoordinate)
                {
                    var presetDistance = DistanceParser.ParseJumpDistance(textBoxDistanceToNextSystem.Text.Trim(), MaximumJumpRange);
                    if (presetDistance.HasValue)
                    {
                        var distance = new DistanceClass
                        {
                            Dist = presetDistance.Value,
                            CreateTime = DateTime.UtcNow,
                            CommanderCreate = EDDiscoveryForm.EDDConfig.CurrentCommander.Name,
                            NameA = item.Name,
                            NameB = item2.Name,
                            Status = DistancsEnum.EDDiscovery
                        };
                        Console.Write("Pre-set distance " + distance.NameA + " -> " + distance.NameB + " = " + distance.Dist);
                        distance.Store();
                        SQLiteDBClass.AddDistanceToCache(distance);
                    }
                }
            }

            textBoxDistanceToNextSystem.Clear();
            textBoxDistanceToNextSystem.Enabled = true;

            UpdateVisitedSystemsEntries(item, item2);
            AddNewHistoryRow(true, item);
            StoreSystemNote();

            Invoke((MethodInvoker)delegate
            {
                _discoveryForm.Map.UpdateVisited(visitedSystems);      // update in UI thread.
            });

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
            if (currentSysPos.curSystem.id_eddb>0)
                Process.Start("http://eddb.io/system/" + currentSysPos.curSystem.id_eddb.ToString());
        }

        private void buttonRoss_Click(object sender, EventArgs e)
        {
            if (currentSysPos.curSystem.id_eddb>0)
                Process.Start("http://ross.eddb.io/system/update/" + currentSysPos.curSystem.id_eddb.ToString());
        }

        private void buttonTrilaterate_Click(object sender, EventArgs e)
        {
            ISystem currSys = GetCurrentSystem();

            if (currSys != null)
            {
                TrilaterationControl tctrl = _discoveryForm.trilaterationControl;

                _discoveryForm.ShowTrilaterationTab();
                tctrl.Set(currSys);
            }
        }

        public ISystem CurrentSystem
        {
            get
            {
                if (dataGridViewTravel == null || dataGridViewTravel.CurrentRow == null) return null;
                return ((VisitedSystemsClass)dataGridViewTravel.CurrentRow.Cells[TravelHistoryColumns.SystemName].Tag).curSystem;
            }
        }

        public string GetCommanderName()
        {
            var value = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;
            return !string.IsNullOrEmpty(value) ? value : null;
        }

        private void textBoxDistanceToNextSystem_Validating(object sender, CancelEventArgs e)
        {
            var value = textBoxDistanceToNextSystem.Text.Trim();
            if (value.Length == 0)
            {
                return;
            }

            if (!DistanceParser.ParseJumpDistance(value, MaximumJumpRange).HasValue)
            {
                e.Cancel = true;
            }
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

        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_discoveryForm.Map.Is3DMapsRunning)            // if not running, click the 3dmap button
                buttonMap_Click(sender, e);

            if (_discoveryForm.Map.Is3DMapsRunning)             // double check here! for paranoia.
            {
                if (dataGridViewTravel.SelectedCells.Count > 0)          // if we have selected (we should!)
                {
                    string s = (string)dataGridViewTravel.Rows[dataGridViewTravel.SelectedCells[0].OwningRow.Index].Cells[TravelHistoryColumns.SystemName].Value;

                    if (_discoveryForm.Map.MoveToSystem(s))
                        _discoveryForm.Map.Show();
                    else
                        MessageBox.Show("System does not have co-ordinates");
                }
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
                foreach(DataGridViewRow r in selectedRows)
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
                sp = (VisitedSystemsClass) r.Cells[TravelHistoryColumns.SystemName].Tag;

                if (sp != null )
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

                sp = (VisitedSystemsClass) r.Cells[TravelHistoryColumns.SystemName].Tag;
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
            FormSagCarinaMission frm = new FormSagCarinaMission(_discoveryForm);
            frm.Nowindowreposition = _discoveryForm.option_nowindowreposition;
            frm.Show();
        }

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

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);
            this.Cursor = Cursors.WaitCursor;
            string sysName = selectedRows.First<DataGridViewRow>().Cells[TravelHistoryColumns.SystemName].Value.ToString();
            EDSMClass edsm = new EDSMClass();
            if (!edsm.ShowSystemInEDSM(sysName)) LogTextHighlight("System could not be found - has not been synched or EDSM is unavailable" + Environment.NewLine);

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
            if (!edsm.ShowSystemInEDSM(sysName)) LogTextHighlight("System could not be found - has not been synched or EDSM is unavailable" + Environment.NewLine);

            this.Cursor = Cursors.Default;
        }

        private void buttonEDSM_Click(object sender, EventArgs e)
        {
            if (currentSysPos != null && currentSysPos.curSystem != null) // solve a possible exception
            {
                if (!String.IsNullOrEmpty(currentSysPos.curSystem.name))
                {
                    EDSMClass edsm = new EDSMClass();
                    string url = edsm.GetUrlToEDSMSystem(currentSysPos.curSystem.name);
                    Process.Start(url);
                    //if (currentSysPos.curSystem.id_eddb > 0)
                    //Process.Start("http://ross.eddb.io/system/update/" + currentSysPos.curSystem.id_eddb.ToString());
                }
            }
        }

        public bool SetTravelHistoryPosition( string sysname )
        {
            foreach (DataGridViewRow item in dataGridViewTravel.Rows)
            {
                string s = (string)item.Cells[TravelHistoryColumns.SystemName].Value;
                if (s.Equals(sysname) && item.Visible )
                {
                    dataGridViewTravel.ClearSelection();
                    item.Selected = true;           // select row
                    dataGridViewTravel.CurrentCell = item.Cells[TravelHistoryColumns.SystemName];       // and ensure visible.
                    return true;
                }
            }

            return false;
        }
    }



    public class SystemDist
    {
        public string name;
        public double dist;
    }

}
