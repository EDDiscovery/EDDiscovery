using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Data.SQLite;
using EDDiscovery.DB;
using System.Diagnostics;
using EDDiscovery2;
using EDDiscovery2.DB;
using System.Globalization;
using System.Text.RegularExpressions;
using EDDiscovery2.Trilateration;
using EDDiscovery2.EDSM;
using EDDiscovery2.HTTP;

namespace EDDiscovery
{
    public partial class TravelHistoryControl : UserControl
    {
        private static EDDiscoveryForm _discoveryForm;
        public int defaultMapColour;
        public EDSMSync sync;
        string datapath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Frontier_Development_s\\Products"; // \\FORC-FDEV-D-1001\\Logs\\";

        internal List<SystemPosition> visitedSystems;
        internal bool EDSMSyncTo = true;
        internal bool EDSMSyncFrom = true;

        public NetLogClass netlog = new NetLogClass();
        List<SystemDist> sysDist = null;
        private SystemPosition currentSysPos = null;


        private static RichTextBox static_richTextBox;
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
            defaultMapColour = db.GetSettingInt("DefaultMap", Color.Red.ToArgb());
            EDSMSyncTo = db.GetSettingBool("EDSMSyncTo", true);
            EDSMSyncFrom = db.GetSettingBool("EDSMSyncFrom", true);
            checkBoxEDSMSyncTo.Checked = EDSMSyncTo;
            checkBoxEDSMSyncFrom.Checked = EDSMSyncFrom;
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
            try
            {
                
                static_richTextBox.SelectionStart = static_richTextBox.TextLength;
                static_richTextBox.SelectionLength = 0;

                static_richTextBox.SelectionColor = color;
                static_richTextBox.AppendText(text);
                static_richTextBox.SelectionColor = static_richTextBox.ForeColor;

                static_richTextBox.SelectionStart = static_richTextBox.Text.Length;
                static_richTextBox.SelectionLength = 0;
                static_richTextBox.ScrollToCaret();
                static_richTextBox.Refresh();


                HttpCom.WriteLog(text, "");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception SystemClass: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }


        private void setRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
        }

        public void RefreshHistory()
        {
            Stopwatch sw1 = new Stopwatch();
            //richTextBox_History.Clear();


            sw1.Start();


            TimeSpan maxDataAge = TimeSpan.Zero;
            int atMost = 0;

            switch (comboBoxHistoryWindow.SelectedIndex)
            {
                case 0:
                    maxDataAge = new TimeSpan(6, 0, 0); // 6 hours
                    break;
                case 1:
                    maxDataAge = new TimeSpan(12, 0, 0); // 12 hours
                    break;
                case 2:
                    maxDataAge = new TimeSpan(24, 0, 0); // 24 hours
                    break;
                case 3:
                    maxDataAge = new TimeSpan(3 * 24, 0, 0); // 3 days
                    break;
                case 4:
                    maxDataAge = new TimeSpan(7 * 24, 0, 0); // 1 week
                    break;
                case 5:
                    maxDataAge = new TimeSpan(14 * 24, 0, 0); // 2 weeks
                    break;
                case 6:
                    maxDataAge = new TimeSpan(30, 0, 0, 0); // 30 days (month)
                    break;
                case 7:
                    atMost = 20; // Last 20
                    break;
                case 8:
                    maxDataAge = new TimeSpan(100000, 24, 0, 0); // all
                    break;
                default:
                    maxDataAge = new TimeSpan(7 * 24, 0, 0); // 1 week (default)
                    break;
            }


            if (visitedSystems == null || visitedSystems.Count == 0)
                GetVisitedSystems(activecommander);

            if (visitedSystems == null)
                return;
            
            List<SystemPosition> result;
            if (atMost > 0)
            {
                result = visitedSystems.OrderByDescending(s => s.time).Take(atMost).ToList();
            }
            else
            {
                var oldestData = DateTime.Now.Subtract(maxDataAge);
                result = (from systems in visitedSystems where systems.time > oldestData orderby systems.time descending select systems).ToList();
            }

            dataGridViewTravel.Rows.Clear();

            System.Diagnostics.Trace.WriteLine("SW1: " + (sw1.ElapsedMilliseconds / 1000.0).ToString("0.000"));
            
            for (int ii = 0; ii < result.Count; ii++) //foreach (var item in result)
            {
      
                SystemPosition item = result[ii];
                SystemPosition item2;

                if (ii < result.Count - 1)
                    item2 = result[ii + 1];
                else
                    item2 = null;

                AddHistoryRow(false, item, item2);
            }


            if (result.Count != visitedSystems.Count)
            {
                // we didn't put all the systems in the history grid
                // make sure that the LastKnown system is properly loaded if it's not visible so trilateration can find it...
                var lastKnown = (from systems
                                 in visitedSystems
                                 where systems.curSystem != null && systems.curSystem.HasCoordinate
                                 orderby systems.time descending
                                 select systems.curSystem).FirstOrDefault();
                if (lastKnown == null)
                {
                    for (int ii = visitedSystems.Count - 1; ii > 0; ii--)
                    {
                        SystemClass sys = SystemData.GetSystem(visitedSystems[ii].Name);
                        if (visitedSystems[ii].curSystem == null && sys != null)
                        {
                            visitedSystems[ii].curSystem = sys;
                            if (sys.HasCoordinate) break;
                        }
                    }
                }

            }

            System.Diagnostics.Trace.WriteLine("SW2: " + (sw1.ElapsedMilliseconds / 1000.0).ToString("0.000"));
            
            if (dataGridViewTravel.Rows.Count > 0)
            {
                ShowSystemInformation((SystemPosition)(dataGridViewTravel.Rows[0].Cells[1].Tag));
            }
            System.Diagnostics.Trace.WriteLine("SW3: " + (sw1.ElapsedMilliseconds / 1000.0).ToString("0.000"));
            sw1.Stop();

            if (textBoxFilter.TextLength>0)
                FilterGridView();
        }

        private void GetVisitedSystems(int commander)
        {                                                       // for backwards compatibility, don't store RGB value.
            visitedSystems = netlog.ParseFiles(richTextBox_History, defaultMapColour, commander);
        }

        private void AddHistoryRow(bool insert, SystemPosition item, SystemPosition item2)
        {
            SystemClass sys1 = null, sys2;
            double dist;
            

            sys1 = SystemData.GetSystem(item.Name);
            if (sys1 == null)
            {
                sys1 = new SystemClass(item.Name);
                if (SQLiteDBClass.globalSystemNotes.ContainsKey(sys1.SearchName))
                {
                    sys1.Note = SQLiteDBClass.globalSystemNotes[sys1.SearchName].Note;
                }
            }
            if (item2 != null)
            {
                sys2 = SystemData.GetSystem(item2.Name);
                if (sys2 == null)
                    sys2 = new SystemClass(item2.Name);

            }
            else
                sys2 = null;

            item.curSystem = sys1;
            item.prevSystem = sys2;
            if (!insert)
            {
                if (item.vs == null)
                {
                    SystemPosition known = visitedSystems.First(x => x.Name == item.Name);
                    if (known != null) item.vs = known.vs;
                }
            }

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

            //richTextBox_History.AppendText(item.time + " " + item.Name + Environment.NewLine);

                object[] rowobj = { item.time, item.Name, diststr, item.curSystem.Note, "█" };
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

                var cell = dataGridViewTravel.Rows[rownr].Cells[1];

                cell.Tag = item;

                dataGridViewTravel.Rows[rownr].DefaultCellStyle.ForeColor = (sys1.HasCoordinate) ? _discoveryForm.theme.VisitedSystemColor : _discoveryForm.theme.NonVisitedSystemColor;

                cell = dataGridViewTravel.Rows[rownr].Cells[4];
                cell.Style.ForeColor = (item.vs == null) ? Color.FromArgb(defaultMapColour) : Color.FromArgb(item.vs.MapColour);
        }



        private void ShowSystemInformation(SystemPosition syspos)
        {
            if (syspos == null || syspos.Name==null)
                return;

            currentSysPos = syspos;
            textBoxSystem.Text = syspos.curSystem.name;
            textBoxPrevSystem.Clear();
            textBoxDistance.Text = syspos.strDistance;
          

            if (syspos.curSystem.HasCoordinate)
            {
                textBoxX.Text = syspos.curSystem.x.ToString("#.#####");
                textBoxY.Text = syspos.curSystem.y.ToString("#.#####");
                textBoxZ.Text = syspos.curSystem.z.ToString("#.#####");

                textBoxSolDist.Text = Math.Sqrt(syspos.curSystem.x * syspos.curSystem.x + syspos.curSystem.y * syspos.curSystem.y + syspos.curSystem.z * syspos.curSystem.z).ToString("0.00");

                //// For test only
                //Stopwatch sw = new Stopwatch();
                //sw.Start();
                //SuggestedReferences refereces = new SuggestedReferences(syspos.curSystem.x, syspos.curSystem.y, syspos.curSystem.z);

                //ReferenceSystem rsys;

                //for (int ii = 0; ii < 16; ii++)
                //{
                //    rsys = refereces.GetCandidate();
                //    refereces.AddReferenceStar(rsys.System);
                //    System.Diagnostics.Trace.WriteLine(rsys.System.name + " Dist: " + rsys.Distance.ToString("0.00") + " x:" + rsys.System.x.ToString() + " y:" + rsys.System.y.ToString() + " z:" + rsys.System.z.ToString() );
                //}
                //sw.Stop();
                //System.Diagnostics.Trace.WriteLine("Reference stars time " + sw.Elapsed.TotalSeconds.ToString("0.000s"));


            }
            else
            {
                textBoxX.Text = "?";
                textBoxY.Text = "?";
                textBoxZ.Text = "?";
                textBoxSolDist.Text = "";

            }

            int count = GetVisitsCount(syspos.curSystem.name);
            textBoxVisits.Text = count.ToString();

            if (currentSysPos.curSystem.id_eddb > 0)  // Only enable eddb/ross for system that it knows about
            {
                buttonEDDB.Visible = true;
                buttonRoss.Visible = true;
            }
            else
            {
                buttonEDDB.Visible = false;
                buttonRoss.Visible = false;
            }


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
            sysDist = new List<SystemDist>();
            SystemClass LastSystem = null;
            float dx, dy, dz;
            double dist;

            try
            {
                if (name == null)
                {

                    var result = visitedSystems.OrderByDescending(a => a.time).ToList<SystemPosition>();


                    for (int ii = 0; ii < result.Count; ii++) //foreach (var item in result)
                    {
                        SystemPosition item = result[ii];

                        LastSystem = SystemData.GetSystem(item.Name);
                        name = item.Name;
                        if (LastSystem != null)
                            break;
                    }

                }
                else
                {
                    LastSystem = SystemData.GetSystem(name);
                }

                if (name !=null)
                    label3.Text = "Closest systems from " + name.ToString();

                dataGridViewNearest.Rows.Clear(); 

                if (LastSystem == null)
                    return;

                foreach (SystemClass pos in SystemData.SystemList)
                {

                    dx = (float)(pos.x - LastSystem.x);
                    dy = (float)(pos.y - LastSystem.y);
                    dz = (float)(pos.z - LastSystem.z);
                    dist = dx * dx + dy * dy + dz * dz;

                    //distance = (float)((system.x - arcsystem.x) * (system.x - arcsystem.x) + (system.y - arcsystem.y) * (system.y - arcsystem.y) + (system.z - arcsystem.z) * (system.z - arcsystem.z));

                    if (dist > 0)
                    {
                        SystemDist sdist = new SystemDist();
                        sdist.name = pos.name;
                        sdist.dist = Math.Sqrt(dist);
                        sysDist.Add(sdist);
                    }
                }

                var list = (from t in sysDist orderby t.dist select t).Take(50);

                foreach (SystemDist sdist in list)
                {
                    object[] rowobj = { sdist.name, sdist.dist.ToString("0.00") };
                    dataGridViewNearest.Rows.Add(rowobj);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

        }


        public ISystem GetCurrentSystem()
        {
            if (visitedSystems.Count == 0)
            {
                return null;
            }
            return (from systems in visitedSystems orderby systems.time descending select systems.curSystem).First();
        }


        private void TravelHistoryControl_Load(object sender, EventArgs e)
        {
            //if (!this.DesignMode)
            //    RefreshHistory();


            if (!this.DesignMode)
            {
                var db = new SQLiteDBClass();
                comboBoxHistoryWindow.SelectedIndex = db.GetSettingInt("EDUIHistory", 4);
                LoadCommandersListBox();
            }
            // this improves dataGridView's scrolling performance
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null,
                dataGridViewTravel,
                new object[] { true }
            );
        }

        private bool cmdlistloaded;
        private void LoadCommandersListBox()
        {
            commanders = new List<EDCommander>();

            commanders.Add(new EDCommander(-1, "Hidden log", ""));
            commanders.AddRange(EDDiscoveryForm.EDDConfig.listCommanders);

            cmdlistloaded = false;
            comboBoxCommander.DataSource = null;
            comboBoxCommander.DataSource = commanders;
            comboBoxCommander.ValueMember = "Nr";
            comboBoxCommander.DisplayMember = "Name";
            cmdlistloaded = true;
            comboBoxCommander.SelectedIndex = 1;


        }

        private void comboBoxCommander_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBoxCommander.SelectedIndex >= 0 && cmdlistloaded)
            {
                var itm = (EDCommander)comboBoxCommander.SelectedItem;
                activecommander = itm.Nr;
                if (visitedSystems != null)
                    visitedSystems.Clear();
                RefreshHistory();
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

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ShowSystemInformation((SystemPosition)(dataGridViewTravel.Rows[e.RowIndex].Cells[1].Tag));
            }

        }

        private void buttonMap_Click(object sender, EventArgs e)
        {
            var map = _discoveryForm.Map;
            var selectedLine = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                           .Select(cell => cell.OwningRow)
                                                           .OrderBy(row => row.Index)
                                                           .First().Index;
            SystemPosition selectedSys;
            do
            {
                selectedSys = (SystemPosition)dataGridViewTravel.Rows[selectedLine].Cells[1].Tag;
                selectedLine += 1;
            } while (!selectedSys.curSystem.HasCoordinate && selectedLine <= dataGridViewTravel.Rows.Count);
            _discoveryForm.updateMapData();
            map.Instance.Reset();
                        
            map.Instance.HistorySelection = selectedSys.curSystem.HasCoordinate ? selectedSys.Name : textBoxSystem.Text.Trim();
            map.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ShowSystemInformation((SystemPosition)(dataGridViewTravel.Rows[e.RowIndex].Cells[1].Tag));

                if (e.ColumnIndex == 3)       // note column
                {
                    richTextBoxNote.Select(richTextBoxNote.Text.Length, 0);     // move caret to end and focus.
                    richTextBoxNote.ScrollToCaret();
                    richTextBoxNote.Focus();
                }
                else  if (e.ColumnIndex == 2 && textBoxDistance.Enabled == true )       // distance column and on..
                {
                    textBoxDistance.Select(textBoxDistance.Text.Length, 0);     // move caret to end (in case something is there) and focus
                    textBoxDistance.Focus();
                }
            }

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            var dist = DistanceAsDouble(textBoxDistance.Text.Trim());
            
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
                    dataGridViewTravel.Rows[dataGridViewTravel.SelectedCells[0].OwningRow.Index].Cells[2].Value = textBoxDistance.Text.Trim();
                              
            }
        }
  

        private void richTextBoxNote_Leave(object sender, EventArgs e)
        {
            StoreSystemNote();
        }

        private void richTextBoxNote_TextChanged(object sender, EventArgs e)
        {
            if (dataGridViewTravel.SelectedCells.Count > 0)          // if we have selected (we should!)
                dataGridViewTravel.Rows[dataGridViewTravel.SelectedCells[0].OwningRow.Index].Cells[3].Value = richTextBoxNote.Text;     // keep the grid up to date to make it seem more interactive
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
                    List<SystemClass> systems = new List<SystemClass>();

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
                        dataGridViewTravel.Rows[dataGridViewTravel.SelectedCells[0].OwningRow.Index].Cells[3].Value = txt;

                    if (edsm.commanderName == null || edsm.apiKey == null)
                        return;

                    if (edsm.commanderName.Length>1 && edsm.apiKey.Length>1)
                        edsm.SetComment(sn);
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


        internal void NewPosition(object source)
        {
            try
            {
                string name = netlog.visitedSystems.Last().Name;
                Invoke((MethodInvoker)delegate
                {
                    LogText("Arrived to system: ");
                    SystemClass sys1 = SystemData.GetSystem(name);
                    if (sys1 == null || sys1.HasCoordinate == false)
                        LogTextHighlight(name);
                    else
                        LogText(name);


                    int count = GetVisitsCount(name);

                    LogText("  : Vist nr " + count.ToString()  + Environment.NewLine);
                    System.Diagnostics.Trace.WriteLine("Arrived to system: " + name + " " + count.ToString() + ":th visit.");

                    var result = visitedSystems.OrderByDescending(a => a.time).ToList<SystemPosition>();

                    //if (TrilaterationControl.Visible)
                    //{
                    //    CloseTrilateration();
                    //    MessageBox.Show("You have arrived to another system while trilaterating."
                    //                    + " As a pre-caution to prevent any mistakes with submitting wrong systems or distances"
                    //                    + ", your trilateration was aborted.");
                    //}

                    
                    SystemPosition item = result[0];
                    SystemPosition item2;

                    if (result.Count > 1)
                        item2 = result[1];
                    else
                        item2 = null;

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
                            var presetDistance = DistanceAsDouble(textBoxDistanceToNextSystem.Text.Trim(), 45);
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

                    AddHistoryRow(true, item, item2);
                    StoreSystemNote();
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception NewPosition: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }

        private int GetVisitsCount(string name)
        {
            int count = (from row in visitedSystems
                         where row.Name == name
                         select row).Count();
            return count;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);

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
                return ((SystemPosition)dataGridViewTravel.CurrentRow.Cells[1].Tag).curSystem;
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
            
            if (!DistanceAsDouble(value, 45).HasValue) // max jump range is ~42Ly
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Parse a distance as a positive double, in the formats "xx", "xx.yy", "xx,yy" or "xxxx,yy".
        /// </summary>
        /// <param name="value">Decimal string to be parsed.</param>
        /// <param name="maximum">Upper limit or null if not required.</param>
        /// <returns>Parsed value or null on conversion failure.</returns>
        private static double? DistanceAsDouble(string value, double? maximum = null)
        {
            if (value.Length == 0)
            {
                return null;
            }

            if (!new Regex(@"^\d+([.,]\d{1,2})?$").IsMatch(value))
            {
                return null;
            }

            double valueDouble;

            // Allow regions with , as decimal separator to  also use . as decimal separator and vice versa
            var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (!double.TryParse(decimalSeparator == "," ? value.Replace(".", ",") : value.Replace(",", "."), out valueDouble))
            {
                return null;
            }

            if (maximum.HasValue && valueDouble > maximum)
            {
                return null;
            }

            return valueDouble;
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

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {

        }

        private void starMapColourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                           .Select(cell => cell.OwningRow)
                                                           .Distinct();
            Color colour = selectedRows.First().Cells[4].Style.ForeColor;
            mapColorDialog.Color = colour;
            if (mapColorDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                string sysName = "";
                foreach(DataGridViewRow r in selectedRows)
                {
                    r.Cells[4].Style.ForeColor = mapColorDialog.Color;
                    sysName = r.Cells[1].Value.ToString();

                    SystemPosition sp = null;
                    sp = (SystemPosition)r.Cells[1].Tag;
                    if (sp == null)
                        sp = visitedSystems.First(s => s.Name.ToUpperInvariant() == sysName.ToUpperInvariant());
                    if (sp.vs != null)
                    {
                        sp.vs.MapColour = mapColorDialog.Color.ToArgb();
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


          
            
            {
                this.Cursor = Cursors.WaitCursor;
                string sysName = "";
                foreach (DataGridViewRow r in selectedRows)
                {
                    sysName = r.Cells[1].Value.ToString();
                    SystemPosition sp=null;

                    sp = (SystemPosition)r.Cells[1].Tag;

               

                    if (sp!= null && sp.vs != null)
                    {
                        sp.vs.Commander = -1;
                        sp.Update();
                    }
                }
                // Remove rows
                if (selectedRows.Count<DataGridViewRow>() == dataGridViewTravel.Rows.Count)
                {
                    dataGridViewTravel.Rows.Clear();
                }

                foreach (DataGridViewRow row in selectedRows.ToList<DataGridViewRow>())
                {
                    dataGridViewTravel.Rows.Remove(row);
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void moveToAnotherCommanderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct();



            List<SystemPosition> listsyspos = new List<SystemPosition>();

            {
                this.Cursor = Cursors.WaitCursor;
                string sysName = "";
                foreach (DataGridViewRow r in selectedRows)
                {
                    sysName = r.Cells[1].Value.ToString();
                    SystemPosition sp = null;

                    sp = (SystemPosition)r.Cells[1].Tag;
                    if (sp != null && sp.vs != null)
                    {
                        listsyspos.Add(sp);

                    }
                }

                MoveToCommander movefrm = new MoveToCommander();

                movefrm.Init(listsyspos.Count>1);

                DialogResult red = movefrm.ShowDialog();
                if (red == DialogResult.OK)
                {
                    if (movefrm.checkBoxAllInNetlog.Checked == false)   // Movel all in list.
                    {
                        foreach (SystemPosition sp in listsyspos)
                        {
                            sp.vs.Commander = movefrm.selectedCommander.Nr;
                            sp.Update();
                        }
                        this.Cursor = Cursors.Default;
                    }
                    else   // Move all systems from the same session
                    {

                    }

                }




                this.Cursor = Cursors.Default;
            }
        }



        /* Add selected systems to trilateration grid */
        private void addToTrilaterationToolStripMenuItem_Click(object sender, EventArgs e)
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
                sysName = r.Cells[1].Value.ToString();

                tctrl.AddSystemToDataGridViewDistances(sysName);
            }

            this.Cursor = Cursors.Default;
        }

        private void textBoxPrevSystem_Enter(object sender, EventArgs e)
        {
            /* Automatically copy the contents to the clipboard whenever this control is activated */
            TextBox tb = sender as TextBox;
            if (tb != null && tb.Text != null)
            {
                System.Windows.Forms.Clipboard.SetText(tb.Text);
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

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }



    public class SystemDist
    {
        public string name;
        public double dist;
    }

}