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
using EDDiscovery.EDDN;
using EDDiscovery.EliteDangerous.JournalEvents;
using Newtonsoft.Json.Linq;
using EDDiscovery.Export;
using EDDiscovery.UserControls;
using EDDiscovery.Forms;

namespace EDDiscovery
{
    public partial class TravelHistoryControl : UserControl
    {
        private const string SingleCoordinateFormat = "0.##";

        public EDDiscoveryForm _discoveryForm;

        SummaryPopOut summaryPopOut = null;
        List<EDCommander> commanders = null;

        Forms.UserControlFormList usercontrolsforms;

        ComputeStarDistance csd = new ComputeStarDistance();
        string lastclosestname;
        SortedList<double, ISystem> lastclosestsystems;

        public TravelHistoryFilter GetPrimaryFilter { get { return userControlTravelGrid.GetHistoryFilter; } }  // some classes want to know out filter

        // Subscribe to these to get various events - layout controls via their Init function do this.

        public delegate void TravelSelectionChanged(HistoryEntry he, HistoryList hl);
        public event TravelSelectionChanged OnTravelSelectionChanged;

        string[] popoutlist = new string[] { "S-Panel", "Trip-Panel", "Log", "Nearest Stars" , "Materials",
                                            "Commodities" , "Ledger" , "Journal", "Travel Grid" , "Screen Shot", "Statistics" , "Scan" };

        Bitmap[] popoutbitmaps = new Bitmap[] { EDDiscovery.Properties.Resources.Log,      // Match pop out enum PopOuts
                                        EDDiscovery.Properties.Resources.star,      // 2
                                        EDDiscovery.Properties.Resources.material , // 3
                                        EDDiscovery.Properties.Resources.commodities, // 4
                                        EDDiscovery.Properties.Resources.ledger , //5 
                                        EDDiscovery.Properties.Resources.journal , //6
                                        EDDiscovery.Properties.Resources.travelgrid , //7 
                                        EDDiscovery.Properties.Resources.screenshot, //8
                                        EDDiscovery.Properties.Resources.stats, //9
                                        EDDiscovery.Properties.Resources.scan, // 10
                                        };

        string[] popouttooltips = new string[] { "Display the program log",
                                               "Display the nearest stars to the currently selected entry",
                                               "Display the material count at the currently selected entry",
                                               "Display the commodity count at the currently selected entry",
                                               "Display a ledger of cash related entries",
                                               "Display the journal grid view",
                                               "Display the history grid view",
                                               "Display the screen shot view",
                                               "Display statistics from the history",
                                               "Display scan data"
                                            };

        public enum PopOuts        // in order added to tabcontrol and in order added to combo box
        {
            Log = 1,
            NS = 2,
            Materials = 3,
            Commodities = 4,
            Ledger = 5,
            Journal = 6,
            TravelGrid = 7,
            ScreenShot = 8,
            Statistics = 9,
            Scan = 10
        };
        
        #region Initialisation

        public TravelHistoryControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;

            usercontrolsforms = new UserControlFormList();

            richTextBoxNote.TextBoxChanged += richTextBoxNote_TextChanged;

            LoadCommandersListBox();

            comboBoxCustomPopOut.Enabled = false;

            comboBoxCustomPopOut.Items.AddRange(popoutlist);
            comboBoxCustomPopOut.SelectedIndex = 0;
            comboBoxCustomPopOut.Enabled = true;

            userControlTravelGrid.Init(_discoveryForm, 0);       // primary first instance - this registers with events in discoveryform to get info
                                                        // then this display, to update its own controls..
            userControlTravelGrid.OnRedisplay += UpdatedDisplay;        // call back when you've added a new entry..
            userControlTravelGrid.OnAddedNewEntry += UpdatedWithAddNewEntry;        // call back when you've added a new entry..
            userControlTravelGrid.OnChangedSelection += ChangedSelection;   // and if the user clicks on something
            userControlTravelGrid.OnResort += Resort;   // and if he or she resorts
            userControlTravelGrid.OnPopOut += TGPopOut;

            TabConfigure(tabStripBottom,1000);          // codes are used to save info, 0 = primary (journal/travelgrid), 1..N are popups, these are embedded UCs
            TabConfigure(tabStripBottomRight,1001);
            TabConfigure(tabStripMiddleRight,1002);

            csd.Init(_discoveryForm);
            csd.OnOtherStarDistances += OtherStarDistances;
            csd.OnNewStarList += NewStarListComputed;

            textBoxTarget.SetAutoCompletor(EDDiscovery.DB.SystemClass.ReturnSystemListForAutoComplete);

            buttonSync.Enabled = EDDiscoveryForm.EDDConfig.CurrentCommander.SyncToEdsm | EDDiscoveryForm.EDDConfig.CurrentCommander.SyncFromEdsm;
        }

        public void LoadControl()
        {
            csd.StartComputeThread();
        }

        #endregion

        #region TAB control

        void TabConfigure(TabStrip t, int displayno)
        {
            t.Images = popoutbitmaps;
            t.ToolTips = popouttooltips;
            t.Tag = displayno;             // these are IDs for purposes of identifying different instances of a control.. 0 = main ones (main travel grid, main tab journal). 1..N are popups
            t.OnRemoving += TabRemoved;
            t.OnCreateTab += TabCreate;
            t.OnPostCreateTab += TabPostCreate;
            t.OnPopOut += TabPopOut;
        }

        void TabRemoved(TabStrip t, Control c )     // called by tab strip when a control is removed
        {
            UserControlCommonBase uccb = c as UserControlCommonBase;
            uccb.Closing();
        }

        Control TabCreate(TabStrip t, int si)        // called by tab strip when selected index changes.. create a new one.. only create.
        {   
            PopOuts i = (PopOuts)(si + 1);

            if (i == PopOuts.Log)
                return new UserControlLog();
            else if (i == PopOuts.NS)
                return new UserControlStarDistance();
            else if (i == PopOuts.Materials)
                return new UserControlMaterials();
            else if (i == PopOuts.Commodities)
                return new UserControlCommodities();
            else if (i == PopOuts.Ledger)
                return new UserControlLedger();
            else if (i == PopOuts.Journal)
                return new UserControlJournalGrid();
            else if (i == PopOuts.TravelGrid)
                return new UserControlTravelGrid();
            else if (i == PopOuts.ScreenShot)
                return new UserControlScreenshot();
            else if (i == PopOuts.Statistics)
                return new UserControlStats();
            else if (i == PopOuts.Scan)
                return new UserControlScan();
            else
                return null;
        }

        void TabPostCreate(TabStrip t, Control ctrl , int i)        // called by tab strip after control has been added..
        {                                                           // now we can do the configure of it, with the knowledge the tab has the right size
            int displaynumber = (int)t.Tag;                         // tab strip - use tag to remember display id which helps us save context.

            if (ctrl is UserControlLog)
            {
                UserControlLog sc = ctrl as UserControlLog;
                sc.Text = "Log";
                sc.Init(_discoveryForm, displaynumber);
                sc.AppendText(_discoveryForm.LogText, _discoveryForm.theme.TextBlockColor);
            }
            else if (ctrl is UserControlStarDistance)
            {
                UserControlStarDistance sc = ctrl as UserControlStarDistance;
                sc.Text = "Stars";
                sc.Init(_discoveryForm, displaynumber);
                if (lastclosestsystems != null)           // if we have some, fill in this grid
                    sc.FillGrid(lastclosestname, lastclosestsystems);
            }
            else if (ctrl is UserControlMaterials)
            {
                UserControlMaterials ucm = ctrl as UserControlMaterials;
                ucm.OnChangedCount += MaterialCommodityChangeCount;
                ucm.OnRequestRefresh += MaterialCommodityRequireRefresh;
                ucm.Init(_discoveryForm, displaynumber);
                ucm.LoadLayout();
                ucm.Text = "Materials";
                if (userControlTravelGrid.GetCurrentHistoryEntry != null)
                    ucm.Display(userControlTravelGrid.GetCurrentHistoryEntry.MaterialCommodity.Sort(false));
            }
            else if (ctrl is UserControlCommodities)
            {
                UserControlCommodities ucm = ctrl as UserControlCommodities;
                ucm.Init(_discoveryForm, displaynumber);
                ucm.OnChangedCount += MaterialCommodityChangeCount;
                ucm.OnRequestRefresh += MaterialCommodityRequireRefresh;
                ucm.LoadLayout();
                ucm.Text = "Commodities";
                if (userControlTravelGrid.GetCurrentHistoryEntry != null)
                    ucm.Display(userControlTravelGrid.GetCurrentHistoryEntry.MaterialCommodity.Sort(true));
            }
            else if (ctrl is UserControlLedger)
            {
                UserControlLedger ucm = ctrl as UserControlLedger;
                ucm.Init(_discoveryForm, displaynumber);
                ucm.LoadLayout();
                ucm.Text = "Ledger";
                ucm.OnGotoJID += GotoJID;
                ucm.Display(_discoveryForm.history.materialcommodititiesledger);
            }
            else if (ctrl is UserControlJournalGrid)
            {
                UserControlJournalGrid ucm = ctrl as UserControlJournalGrid;
                ucm.Init(_discoveryForm, displaynumber);
                ucm.LoadLayout();
                ucm.Text = "Journal";
                ucm.Display(_discoveryForm.history);
                ucm.NoHistoryIcon();
                ucm.NoPopOutIcon();
            }
            else if (ctrl is UserControlTravelGrid)
            {
                UserControlTravelGrid ucm = ctrl as UserControlTravelGrid;
                ucm.Init(_discoveryForm, displaynumber);
                ucm.NoHistoryIcon();
                ucm.NoPopOutIcon();
                ucm.LoadLayout();
                ucm.Text = "History";
                ucm.Display(_discoveryForm.history);
                ucm.NoHistoryIcon();
                ucm.NoPopOutIcon();
            }
            else if (ctrl is UserControlScreenshot)
            {
                UserControlScreenshot ucm = ctrl as UserControlScreenshot;
                ucm.Init(_discoveryForm, displaynumber);
                ucm.LoadLayout();
                ucm.Text = "Screen Shot";
            }
            else if (ctrl is UserControlStats)
            {
                UserControlStats ucm = ctrl as UserControlStats;
                ucm.Init(_discoveryForm, displaynumber);
                ucm.LoadLayout();
                ucm.Text = "Statistics";
                ucm.SelectionChanged(userControlTravelGrid.GetCurrentHistoryEntry, _discoveryForm.history);
            }
            else if (ctrl is UserControlScan)
            {
                UserControlScan ucm = ctrl as UserControlScan;
                ucm.Init(_discoveryForm, displaynumber);
                ucm.LoadLayout();
                ucm.Text = "Scan";
                ucm.Display(userControlTravelGrid.GetCurrentHistoryEntry, _discoveryForm.history);
            }

            System.Diagnostics.Debug.WriteLine("And theme {0}", i);
            _discoveryForm.theme.ApplyToControls(t);
        }

        void TabPopOut(TabStrip t, int i)        // pop out clicked
        {
            PopOut((PopOuts)(i+1));
        }

        #endregion

        #region Panel sizing

        private void panel_topright_Resize(object sender, EventArgs e)
        {
            // Move controls around on topright

            int width = panel_topright.Width;
            int butoffsetx = buttonMap.Location.X - buttonMap2D.Location.X;
            int butoffsety = buttonMap2D.Location.Y - button_RefreshHistory.Location.Y;

            // always 2dmap, 3dmap

            comboBoxCommander.Width = Math.Min(Math.Max(width - comboBoxCommander.Location.X - 4,64),192);

            if ( width >= buttonMap2D.Location.X + butoffsetx * 3 + buttonSync.Width + 4)  // other two buttons beside (2, 4)
            {
                comboBoxCustomPopOut.Location = new Point(buttonMap2D.Location.X + butoffsetx * 2, buttonMap2D.Location.Y);
                buttonSync.Location = new Point(buttonMap2D.Location.X + butoffsetx * 3, buttonMap2D.Location.Y);
            }
            else if (width >= buttonMap2D.Location.X + butoffsetx *2 + comboBoxCustomPopOut.Width + 4)   // one button beside, on below (2,3,1)
            {
                comboBoxCustomPopOut.Location = new Point(buttonMap2D.Location.X + butoffsetx * 2, buttonMap2D.Location.Y);
                buttonSync.Location = new Point(buttonMap2D.Location.X, buttonMap2D.Location.Y + butoffsety);
            }
            else  // 2,2,2
            {
                comboBoxCustomPopOut.Location = new Point(buttonMap2D.Location.X, buttonMap2D.Location.Y + butoffsety);
                buttonSync.Location = new Point(buttonMap2D.Location.X + butoffsetx , comboBoxCustomPopOut.Location.Y);
            }
            
            panel_topright.Size = new Size(panel_topright.Width, buttonSync.Location.Y + buttonSync.Height + 6);

            // now do this in topright, because its moving around the lower panes. Works in here because topright won't be resized.

            int rossright = buttonRoss.Location.X + buttonRoss.Width;       // from the system panel, far right part

            if (width > rossright + 100)                                    // enough space to more to the right of topright panel?
                panel_system.Dock = DockStyle.Left;
            else
                panel_system.Dock = DockStyle.Top;

            panel_system.Size = new Size(rossright + 4, textBoxGovernment.Location.Y + textBoxGovernment.Height + 6);

            panelTarget.Width = panelNoteArea.Width = width - panel_system.Width;   // and size the target and note panels to..
        }

        private void panelNoteArea_Resize(object sender, EventArgs e)
        {
            int width = panelNoteArea.Width;

            if (width > 300)                              // can we fit onto one line?
            {
                labelNote.Location = new Point(2, 2);
                richTextBoxNote.Location = new Point(labelNote.Location.X + labelNote.Width + 6, labelNote.Location.Y);
                richTextBoxNote.Width = width - richTextBoxNote.Location.X - 4;
            }
            else
            {
                labelNote.Location = new Point(2, 2);
                richTextBoxNote.Location = new Point(2, labelNote.Location.Y + labelNote.Height + 4);
                richTextBoxNote.Width = width - 4;
            }

            panelNoteArea.Height = richTextBoxNote.Location.Y + richTextBoxNote.Height + 6;
        }

        private void panelTarget_Resize(object sender, EventArgs e)
        {
            int width = panelTarget.Width;

            if (width > 200)                            // can we fit onto one line?
            {
                labelTarget.Location = new Point(2, 2);
                textBoxTarget.Location = new Point(labelTarget.Location.X + labelTarget.Width + 6, labelTarget.Location.Y);
                textBoxTarget.Width = width - textBoxTarget.Location.X - 16 - textBoxTargetDist.Width;
                textBoxTargetDist.Location = new Point(textBoxTarget.Location.X + textBoxTarget.Width + 8, labelTarget.Location.Y);
            }
            else
            {
                labelNote.Location = new Point(2, 2);
                textBoxTarget.Location = new Point(2, labelNote.Location.Y + labelNote.Height + 8);
                textBoxTarget.Width = width - 4;
                textBoxTargetDist.Location = new Point(2, textBoxTarget.Location.Y + textBoxTarget.Height + 8);
            }

            panelTarget.Height = textBoxTargetDist.Location.Y + textBoxTargetDist.Height + 6;
        }

        #endregion

        void GotoJID(long v)
        {
            userControlTravelGrid.GotoPosByJID(v);
        }

        #region New Stars

        private void OtherStarDistances(SortedList<double, ISystem> closestsystemlist, ISystem vsc )       // on thread..
        {
            Invoke((MethodInvoker)delegate      // being paranoid about threads..
            {
                _discoveryForm.history.CalculateSqDistances(closestsystemlist, vsc.x, vsc.y, vsc.z, 50, true);
            });
        }

        private void NewStarListComputed(string name, SortedList<double, ISystem> csl)      // thread..
        {
            Invoke((MethodInvoker)delegate
            {
                lastclosestname = name;
                lastclosestsystems = csl;
                if (tabStripBottom.CurrentControl is UserControlStarDistance)
                    ((UserControlStarDistance)tabStripBottom.CurrentControl).FillGrid(name, csl);
                if (tabStripBottomRight.CurrentControl is UserControlStarDistance)
                    ((UserControlStarDistance)tabStripBottomRight.CurrentControl).FillGrid(name, csl);
                if (tabStripMiddleRight.CurrentControl is UserControlStarDistance)
                    ((UserControlStarDistance)tabStripMiddleRight.CurrentControl).FillGrid(name, csl);
                foreach (UserControlCommonBase uc in usercontrolsforms.GetListOfControls(typeof(UserControlStarDistance)))
                    ((UserControlStarDistance)uc).FillGrid(name, csl);
            });
        }

        public void CloseClosestSystemThread()
        {
            csd.StopComputeThread();
        }

        #endregion

        #region Material Commodities changers

        void MaterialCommodityChangeCount(List<MaterialCommodities> changelist)
        {
            HistoryEntry he = userControlTravelGrid.GetCurrentHistoryEntry;
            long jid = JournalEntry.AddEDDItemSet(EDDiscoveryForm.EDDConfig.CurrentCommander.Nr, he.EventTimeUTC, (he.EntryType == JournalTypeEnum.EDDItemSet) ? he.Journalid : 0, changelist);
            userControlTravelGrid.SetPreferredJIDAfterRefresh(jid);         // tell the main grid, please find and move here
            MaterialCommodities.LoadCacheList();        // in case we did anything..
            _discoveryForm.RefreshHistoryAsync();
        }

        void MaterialCommodityRequireRefresh()
        {
            MaterialCommodities.LoadCacheList();        // in case we did anything..
            _discoveryForm.RefreshHistoryAsync();
        }

        #endregion

        #region Display history

        public void UpdatedDisplay(HistoryList hl)                      // called from main travelgrid when refreshed display
        {
            ShowSystemInformation(userControlTravelGrid.GetCurrentRow);
            RedrawSummary();
            RefreshTargetInfo();
            UpdateDependentsWithSelection();
            _discoveryForm.Map.UpdateSystemList(_discoveryForm.history.FilterByTravel);           // update map
            RedrawTripPanel(hl);
        }


        public void UpdatedWithAddNewEntry(HistoryEntry he, HistoryList hl, bool accepted)     // main travel grid calls after getting a new entry
        {
            try
            {   // try is a bit old, probably do not need it.
                if (he.IsFSDJump)
                {
                    int count = _discoveryForm.history.GetVisitsCount(he.System.name, he.System.id_edsm);
                    _discoveryForm.LogLine(string.Format("Arrived at system {0} Visit No. {1}", he.System.name, count));

                    System.Diagnostics.Trace.WriteLine("Arrived at system: " + he.System.name + " " + count + ":th visit.");

                    if (EDDiscoveryForm.EDDConfig.CurrentCommander.SyncToEdsm == true)
                        EDSMSync.SendTravelLog(he);
                }
                if (he.IsFuelScoop)
                {
                    HistoryEntry lastFSD = hl.GetLastFSD;
                    if(lastFSD!=null)
                        lastFSD.FuelLevel= he.FuelTotal;
                }

                if (he.ISEDDNMessage)
                {
                    if (EDDiscoveryForm.EDDConfig.CurrentCommander.SyncToEddn == true)
                    {
                        EDDNSync.SendEDDNEvents(_discoveryForm, he);
                    }
                }

                if (he.IsFSDJump || he.EntryType == JournalTypeEnum.Location)
                    _discoveryForm.Map.UpdateSystemList(_discoveryForm.history.FilterByTravel);           // update map - only cares about FSD changes

                if ( accepted )                                                 // if accepted it on main grid..
                {
                    RefreshSummaryRow(userControlTravelGrid.GetRow(0), true);   // Tell the summary new row has been added
                    RefreshTargetInfo();                                        // tell the target system its changed the latest system

                    if (EDDiscoveryForm.EDDConfig.FocusOnNewSystem)   // Move focus to new row
                    {
                        userControlTravelGrid.SelectTopRow();
                        ShowSystemInformation(userControlTravelGrid.GetCurrentRow);
                        UpdateDependentsWithSelection();
                    }
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
            HistoryEntry syspos = null;

            if (rw == null)
            {
                textBoxSystem.Text = textBoxX.Text = textBoxY.Text = textBoxZ.Text =
                textBoxAllegiance.Text = textBoxEconomy.Text = textBoxGovernment.Text =
                textBoxVisits.Text = textBoxState.Text = textBoxHomeDist.Text = richTextBoxNote.Text = "";
                buttonRoss.Enabled = buttonEDDB.Enabled = false;
            }
            else
            {
                syspos = userControlTravelGrid.GetHistoryEntry(rw.Index);     // reload, it may have changed
                Debug.Assert(syspos != null);

                _discoveryForm.history.FillEDSM(syspos, reload: true); // Fill in any EDSM info we have

                SystemNoteClass note = userControlTravelGrid.GetSystemNoteClass(rw.Index);

                textBoxSystem.Text = syspos.System.name;

                if (syspos.System.HasCoordinate)         // cursystem has them?
                {
                    textBoxX.Text = syspos.System.x.ToString(SingleCoordinateFormat);
                    textBoxY.Text = syspos.System.y.ToString(SingleCoordinateFormat);
                    textBoxZ.Text = syspos.System.z.ToString(SingleCoordinateFormat);

                    textBoxHomeDist.Text = Math.Sqrt(syspos.System.x * syspos.System.x + syspos.System.y * syspos.System.y + syspos.System.z * syspos.System.z).ToString("0.00");
                }
                else
                {
                    textBoxX.Text = "?";
                    textBoxY.Text = "?";
                    textBoxZ.Text = "?";
                    textBoxHomeDist.Text = "";
                }

                int count = _discoveryForm.history.GetVisitsCount(syspos.System.name, syspos.System.id_edsm);
                textBoxVisits.Text = count.ToString();

                bool enableedddross = (syspos.System.id_eddb > 0);  // Only enable eddb/ross for system that it knows about

                buttonRoss.Enabled = buttonEDDB.Enabled = enableedddross;

                textBoxAllegiance.Text = EnumStringFormat(syspos.System.allegiance.ToString());
                textBoxEconomy.Text = EnumStringFormat(syspos.System.primary_economy.ToString());
                textBoxGovernment.Text = EnumStringFormat(syspos.System.government.ToString());
                textBoxState.Text = EnumStringFormat(syspos.System.state.ToString());
                richTextBoxNote.Text = EnumStringFormat(note != null ? note.Note : "");

                csd.Add(syspos.System);     // ONLY use the primary to compute the new list, the call back will populate all of them NewStarListComputed
            }

            if (OnTravelSelectionChanged != null)
                OnTravelSelectionChanged(syspos, _discoveryForm.history);
        }

        private string EnumStringFormat(string str)
        {
            if (str == null)
                return "";
            if (str.Equals("Unknown"))
                return "";

            return str.Replace("_", " ");
        }

        #endregion


        #region Grid Layout

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            // ORDER IMPORTANT for right outer/inner splitter, otherwise windows fixes it 

            try
            {
                splitContainerLeftRight.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterLR", splitContainerLeftRight.SplitterDistance);
                splitContainerLeft.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterL", splitContainerLeft.SplitterDistance);
                splitContainerRightOuter.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterRO", splitContainerRightOuter.SplitterDistance);
                splitContainerRightInner.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterR", splitContainerRightInner.SplitterDistance);
            }
            catch { };          // so splitter can except, if values are strange, but we don't really care, so lets throw away the exception

            userControlTravelGrid.LoadLayout();

            // NO NEED to reload the three tabstrips - code below will cause a LoadLayout on the one selected.

            tabStripBottom.SelectedIndex = SQLiteDBClass.GetSettingInt("TravelControlBottomTab", (int)PopOuts.Scan - 1);
            tabStripBottomRight.SelectedIndex = SQLiteDBClass.GetSettingInt("TravelControlBottomRightTab", (int)PopOuts.Log - 1);
            tabStripMiddleRight.SelectedIndex = SQLiteDBClass.GetSettingInt("TravelControlMiddleRightTab", (int)PopOuts.NS - 1);
        }

        public void SaveSettings()     // called by form when closing
        {
            userControlTravelGrid.Closing();
            ((UserControlCommonBase)(tabStripBottom.CurrentControl)).Closing();
            ((UserControlCommonBase)(tabStripBottomRight.CurrentControl)).Closing();
            ((UserControlCommonBase)(tabStripMiddleRight.CurrentControl)).Closing();

            SQLiteDBClass.PutSettingInt("TravelControlSpliterLR", splitContainerLeftRight.SplitterDistance);
            SQLiteDBClass.PutSettingInt("TravelControlSpliterL", splitContainerLeft.SplitterDistance);
            SQLiteDBClass.PutSettingInt("TravelControlSpliterRO", splitContainerRightOuter.SplitterDistance);
            SQLiteDBClass.PutSettingInt("TravelControlSpliterR", splitContainerRightInner.SplitterDistance);

            SQLiteDBClass.PutSettingInt("TravelControlBottomRightTab", tabStripBottomRight.SelectedIndex);
            SQLiteDBClass.PutSettingInt("TravelControlBottomTab", tabStripBottom.SelectedIndex);
            SQLiteDBClass.PutSettingInt("TravelControlMiddleRightTab", tabStripMiddleRight.SelectedIndex);
        }

        #endregion

        #region Clicks

        public void LoadCommandersListBox()
        {
            comboBoxCommander.Enabled = false;
            commanders = new List<EDCommander>();

            commanders.Add(new EDCommander(-1, "Hidden log", "", false, false, false));
            commanders.AddRange(EDDiscoveryForm.EDDConfig.ListOfCommanders);

            comboBoxCommander.DataSource = null;
            comboBoxCommander.DataSource = commanders;
            comboBoxCommander.ValueMember = "Nr";
            comboBoxCommander.DisplayMember = "Name";

            if (_discoveryForm.DisplayedCommander == -1)
                comboBoxCommander.SelectedIndex = 0;
            else
                comboBoxCommander.SelectedItem = EDDiscoveryForm.EDDConfig.CurrentCommander;

            comboBoxCommander.Enabled = true;
        }

        private void comboBoxCommander_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCommander.SelectedIndex >= 0 && comboBoxCommander.Enabled)     // DONT trigger during LoadCommandersListBox
            {
                var itm = (EDCommander)comboBoxCommander.SelectedItem;
                _discoveryForm.DisplayedCommander = itm.Nr;
                if (itm.Nr >= 0)
                    EDDiscoveryForm.EDDConfig.CurrentCmdrID = itm.Nr;

                buttonSync.Enabled = EDDiscoveryForm.EDDConfig.CurrentCommander.SyncToEdsm | EDDiscoveryForm.EDDConfig.CurrentCommander.SyncFromEdsm;

                _discoveryForm.RefreshHistoryAsync();                                   // which will cause DIsplay to be called as some point
            }
        }

        public void buttonMap_Click(object sender, EventArgs e)
        {
            _discoveryForm.Open3DMap(userControlTravelGrid.GetCurrentHistoryEntry);
        }

        private void Resort()       // user travel grid to say it resorted
        {
            RedrawSummary();
            UpdateDependentsWithSelection();
        }

        private void ChangedSelection(int rowno, int colno , bool doubleclick , bool note)      // User travel grid call back to say someone clicked somewhere
        {
            if (rowno >= 0)
            {
                ShowSystemInformation(userControlTravelGrid.GetRow(rowno));
                UpdateDependentsWithSelection();

                if (doubleclick == false && note)
                {
                    richTextBoxNote.TextBox.Select(richTextBoxNote.Text.Length, 0);     // move caret to end and focus.
                    richTextBoxNote.TextBox.ScrollToCaret();
                    richTextBoxNote.TextBox.Focus();
                }
            }
        }

        private void UpdateDependentsWithSelection()
        {
            if (userControlTravelGrid.currentGridRow >= 0)
            {
                HistoryEntry currentsys = userControlTravelGrid.GetCurrentHistoryEntry;
                _discoveryForm.Map.UpdateHistorySystem(currentsys.System);
                _discoveryForm.RouteControl.UpdateHistorySystem(currentsys.System.name);
                _discoveryForm.ExportControl.UpdateHistorySystem(currentsys.System.name);
            }
        }

        private void richTextBoxNote_Leave(object sender, EventArgs e)
        {
            StoreSystemNote();
        }

        private void richTextBoxNote_TextChanged(object sender, EventArgs e)
        {
            userControlTravelGrid.UpdateCurrentNote(richTextBoxNote.Text);

            foreach (UserControlCommonBase uc in usercontrolsforms.GetListOfControls(typeof(UserControlTravelGrid)))
                ((UserControlTravelGrid)uc).UpdateNoteJID(userControlTravelGrid.GetCurrentHistoryEntry.Journalid, richTextBoxNote.Text);
        }

        private void StoreSystemNote()
        {
            if (userControlTravelGrid.currentGridRow < 0)
                return;

            try
            {
                HistoryEntry sys = userControlTravelGrid.GetCurrentHistoryEntry;
                SystemNoteClass sn = userControlTravelGrid.GetCurrentSystemNoteClass;

                string txt = richTextBoxNote.Text.Trim();

                if ( (sn == null && txt.Length>0) || (sn!=null && !sn.Note.Equals(txt))) // if no system note, and text,  or system not is not text
                {
                    if ( sn != null && (sn.Journalid == sys.Journalid || sn.Journalid == 0 || (sn.Name.Equals(sys.System.name, StringComparison.InvariantCultureIgnoreCase) && sn.EdsmId <= 0) || (sn.EdsmId > 0 && sn.EdsmId == sys.System.id_edsm)) )           // already there, update
                    { 
                        sn.Note = txt;
                        sn.Time = DateTime.Now;
                        sn.Name = (sys.IsFSDJump) ? sys.System.name : "";
                        sn.Journalid = sys.Journalid;
                        sn.EdsmId = sys.IsFSDJump ? sys.System.id_edsm : 0;
                        sn.Update();
                    }
                    else
                    {
                        sn = new SystemNoteClass();
                        sn.Note = txt;
                        sn.Time = DateTime.Now;
                        sn.Name = (sys.IsFSDJump) ? sys.System.name : "";
                        sn.Journalid = sys.Journalid;
                        sn.EdsmId = sys.IsFSDJump ? sys.System.id_edsm : 0;
                        sn.Add();

                        userControlTravelGrid.UpdateCurrentNoteTag(sn);
                    }

                    userControlTravelGrid.UpdateCurrentNote(txt);

                    if (EDDiscoveryForm.EDDConfig.CurrentCommander.SyncToEdsm && sys.IsFSDJump )       // only send on FSD jumps
                        EDSMSync.SendComments(sn.Name,sn.Note,sn.EdsmId);

                    _discoveryForm.Map.UpdateNote();
                    RefreshSummaryRow(userControlTravelGrid.GetCurrentRow);    // tell it this row was changed
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);

                _discoveryForm.LogLineHighlight("Exception : " + ex.Message);
                _discoveryForm.LogLineHighlight(ex.StackTrace);
            }
        }

        private void buttonSync_Click(object sender, EventArgs e)
        {
            EDSMClass edsm = new EDSMClass();

            if (!edsm.IsApiKeySet)
            {
                MessageBox.Show("Please ensure a commander is selected and it has a EDSM API key set");
                return;
            }

            try
            {
                _discoveryForm.EdsmSync.StartSync(edsm, EDDiscoveryForm.EDDConfig.CurrentCommander.SyncToEdsm, EDDiscoveryForm.EDDConfig.CurrentCommander.SyncFromEdsm, EDDConfig.Instance.DefaultMapColour);
            }
            catch (Exception ex)
            {
                _discoveryForm.LogLine($"EDSM Sync failed: {ex.Message}");
            }
        }

        private void buttonEDDB_Click(object sender, EventArgs e)
        {
            HistoryEntry sys = userControlTravelGrid.GetCurrentHistoryEntry;

            if (sys != null && sys.System.id_eddb > 0)
                Process.Start("http://eddb.io/system/" + sys.System.id_eddb.ToString());
        }

        private void buttonRoss_Click(object sender, EventArgs e)
        {
            HistoryEntry sys = userControlTravelGrid.GetCurrentHistoryEntry;
            if (sys != null)
            {
                _discoveryForm.history.FillEDSM(sys, reload: true);

                if (sys != null && sys.System.id_eddb > 0)
                    Process.Start("http://ross.eddb.io/system/update/" + sys.System.id_eddb.ToString());
            }
        }

        void gotoEDSM()
        {
            HistoryEntry sys = userControlTravelGrid.GetCurrentHistoryEntry;

            if (sys != null)
                _discoveryForm.history.FillEDSM(sys, reload: true);

            if (sys != null && sys.System != null) // solve a possible exception
            {
                if (!String.IsNullOrEmpty(sys.System.name))
                {
                    long? id_edsm = sys.System.id_edsm;
                    if (id_edsm <= 0)
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

        private void buttonEDSM_Click(object sender, EventArgs e)
        {
            gotoEDSM();
        }

        public void RefreshButton(bool state)
        {
            button_RefreshHistory.Enabled = state;
            foreach (UserControlCommonBase uc in usercontrolsforms.GetListOfControls(typeof(UserControlJournalGrid)))
                ((UserControlJournalGrid)uc).RefreshButton(state);      // and the journal views need it
        }

        private void button_RefreshHistory_Click(object sender, EventArgs e)
        {
            _discoveryForm.LogLine("Refresh History.");
            _discoveryForm.RefreshHistoryAsync(checkedsm: true);
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
                    SystemNoteClass nc = SystemNoteClass.GetNoteOnSystem(sc.name, sc.id_edsm);        // has it got a note?

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


        private void comboBoxCustomPopOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!comboBoxCustomPopOut.Enabled)
                return;

            if (comboBoxCustomPopOut.SelectedIndex == 0)
                ToggleSummaryPopOut();
            else if (comboBoxCustomPopOut.SelectedIndex == 1)
                ToggleTripPanelPopOut();
            else
                PopOut((PopOuts)(comboBoxCustomPopOut.SelectedIndex-1));

            comboBoxCustomPopOut.Enabled = false;
            comboBoxCustomPopOut.SelectedIndex = 0;
            comboBoxCustomPopOut.Enabled = true;
        }

        public void PopOut(PopOuts selected)
        { 
            UserControlForm tcf = usercontrolsforms.NewForm(_discoveryForm.option_nowindowreposition);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EDDiscovery.EDDiscoveryForm));
            tcf.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            if (selected == PopOuts.Log)
            {
                int numopened = usercontrolsforms.CountOf(typeof(UserControlLog)) + 1;

                UserControlLog uclog = new UserControlLog(); // Add a log
                tcf.Init(uclog, "Log " + ((numopened > 1) ? numopened.ToString() : ""), _discoveryForm.theme.WindowsFrame,  "Log" + numopened, _discoveryForm.TopMost, false);
                uclog.Init(_discoveryForm, numopened);
                uclog.AppendText(_discoveryForm.LogText, _discoveryForm.theme.TextBackColor);
            }
            else if (selected == PopOuts.NS)
            {
                int numopened = usercontrolsforms.CountOf(typeof(UserControlStarDistance)) + 1;

                UserControlStarDistance ucsd = new UserControlStarDistance(); // Add a closest distance tab
                tcf.Init(ucsd, "Nearest Stars " + ((numopened > 1) ? numopened.ToString() : ""), _discoveryForm.theme.WindowsFrame,  "StarDistance" + numopened, _discoveryForm.TopMost, false);

                ucsd.Init(_discoveryForm, numopened);
                if (lastclosestsystems != null)           // if we have some, fill in this grid
                    ucsd.FillGrid(lastclosestname, lastclosestsystems);
            }
            else if (selected == PopOuts.Materials)
            {
                int numopened = usercontrolsforms.CountOf(typeof(UserControlMaterials)) + 1;

                UserControlMaterials ucmc = new UserControlMaterials(); // Add a closest distance tab
                tcf.Init(ucmc, "Materials " + ((numopened > 1) ? numopened.ToString() : ""), _discoveryForm.theme.WindowsFrame, "Materials" + numopened, _discoveryForm.TopMost, false);

                ucmc.Init(_discoveryForm, numopened);
                HistoryEntry curpos = userControlTravelGrid.GetCurrentHistoryEntry;
                if (curpos != null)
                    ucmc.Display(curpos.MaterialCommodity.Sort(false));
            }
            else if (selected == PopOuts.Commodities)
            {
                int numopened = usercontrolsforms.CountOf(typeof(UserControlCommodities)) + 1;

                UserControlCommodities ucmc = new UserControlCommodities(); // Add a closest distance tab
                tcf.Init(ucmc, "Commodities " + ((numopened > 1) ? numopened.ToString() : ""), _discoveryForm.theme.WindowsFrame, "Commodities" + numopened, _discoveryForm.TopMost, false);

                ucmc.Init(_discoveryForm, numopened);
                HistoryEntry curpos = userControlTravelGrid.GetCurrentHistoryEntry;
                if (curpos != null)
                    ucmc.Display(curpos.MaterialCommodity.Sort(true));
            }
            else if (selected == PopOuts.Ledger)
            {
                int numopened = usercontrolsforms.CountOf(typeof(UserControlLedger)) + 1;

                UserControlLedger ucmc = new UserControlLedger(); // Add a closest distance tab
                tcf.Init(ucmc, "Ledger " + ((numopened > 1) ? numopened.ToString() : ""), _discoveryForm.theme.WindowsFrame, "Ledger" + numopened, _discoveryForm.TopMost, false);

                ucmc.Init(_discoveryForm, numopened);
                ucmc.Display(_discoveryForm.history.materialcommodititiesledger);
                ucmc.OnGotoJID += GotoJID;
            }
            else if (selected == PopOuts.Journal)
            {
                int numopened = usercontrolsforms.CountOf(typeof(UserControlJournalGrid)) + 1;  // used to determine name and also key for DB
                UserControlJournalGrid uctg = new UserControlJournalGrid();
                tcf.Init(uctg, "Journal History " + ((numopened > 1) ? numopened.ToString() : ""), _discoveryForm.theme.WindowsFrame, "JournalHistory" + numopened, _discoveryForm.TopMost, false);
                uctg.Init(_discoveryForm, numopened);
                uctg.Display(_discoveryForm.history);
                uctg.NoPopOutIcon();
                uctg.NoHistoryIcon();
            }
            else if (selected == PopOuts.TravelGrid)    // match order in bitmap mp and comboBoxCustomPopOut
            {
                int numopened = usercontrolsforms.CountOf(typeof(UserControlTravelGrid)) + 1;  // used to determine name and also key for DB
                UserControlTravelGrid uctg = new UserControlTravelGrid();
                tcf.Init(uctg,"Travel History " + ((numopened > 1) ? numopened.ToString() : ""), _discoveryForm.theme.WindowsFrame, "TravelHistory" + numopened, _discoveryForm.TopMost, false);
                uctg.Init(_discoveryForm, numopened);
                uctg.Display(_discoveryForm.history);
                uctg.NoPopOutIcon();
                uctg.NoHistoryIcon();
            }
            else if (selected == PopOuts.ScreenShot)    // match order in bitmap mp and comboBoxCustomPopOut
            {
                int numopened = usercontrolsforms.CountOf(typeof(UserControlScreenshot)) + 1;  // used to determine name and also key for DB
                UserControlScreenshot ucm = new UserControlScreenshot();
                tcf.Init(ucm, "ScreenShot " + ((numopened > 1) ? numopened.ToString() : ""), _discoveryForm.theme.WindowsFrame, "ScreenShot" + numopened, _discoveryForm.TopMost, false);
                ucm.Init(_discoveryForm, numopened);
            }
            else if (selected == PopOuts.Statistics)    // match order in bitmap mp and comboBoxCustomPopOut
            {
                int numopened = usercontrolsforms.CountOf(typeof(UserControlStats)) + 1;  // used to determine name and also key for DB
                UserControlStats ucm = new UserControlStats();
                tcf.Init(ucm,"Statistics " + ((numopened > 1) ? numopened.ToString() : ""), _discoveryForm.theme.WindowsFrame, "Stats" + numopened, _discoveryForm.TopMost, false);
                ucm.Init(_discoveryForm, numopened);
                ucm.SelectionChanged(userControlTravelGrid.GetCurrentHistoryEntry, _discoveryForm.history);
            }
            else if (selected == PopOuts.Scan)
            {
                int numopened = usercontrolsforms.CountOf(typeof(UserControlScan)) + 1;  // used to determine name and also key for DB
                UserControlScan ucm = new UserControlScan();
                tcf.Init(ucm,"Scan " + ((numopened > 1) ? numopened.ToString() : ""), _discoveryForm.theme.WindowsFrame, "Scan" + numopened, _discoveryForm.TopMost, false);
                ucm.Init(_discoveryForm, numopened);
                ucm.Display(userControlTravelGrid.GetCurrentHistoryEntry, _discoveryForm.history);
            }

            tcf.Show();

            if ( tcf.UserControl != null )
                tcf.UserControl.Font = _discoveryForm.theme.GetFont;        // Important. Apply font autoscaling to the user control
                                                                        // ApplyToForm does not apply the font to the actual UC, only
                                                                        // specific children controls.  The TabControl in the discoveryform ends up autoscaling most stuff
                                                                        // the children directly attached to the discoveryform are not autoscaled

            _discoveryForm.theme.ApplyToForm(tcf);
        }

        #endregion

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
                summaryPopOut.RefreshTarget(userControlTravelGrid.TravelGrid, _discoveryForm.history.GetLastWithPosition);

            if (IsTripPanelPopOutReady)
                tripPanelPopOut.displayLastFSDOrFuel();
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
                p.SetTextColour(_discoveryForm.theme.SPanelColor);
                p.ResetForm(userControlTravelGrid.TravelGrid);
                p.RefreshTarget(userControlTravelGrid.TravelGrid, _discoveryForm.history.GetLastWithPosition); 
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
                summaryPopOut.ResetForm(userControlTravelGrid.TravelGrid);
                summaryPopOut.RefreshTarget(userControlTravelGrid.TravelGrid, _discoveryForm.history.GetLastWithPosition);
            }
        }

        public void SummaryRefreshRequested(Object o, EventArgs e)
        {
            RedrawSummary();
        }

        public void RefreshSummaryRow(DataGridViewRow row , bool add = false )
        {
            if (IsSummaryPopOutReady)
                summaryPopOut.RefreshRow(userControlTravelGrid.TravelGrid, row, add);
        }

        public void NewBodyScan(JournalScan js)
        {
            if (IsSummaryPopOutReady)
                summaryPopOut.ShowScanData(js);
        }

        void TGPopOut()
        {
            PopOut(PopOuts.TravelGrid);
        }

        #endregion

        #region LogOut


        #endregion


        #region Trip computer Pop out

        TripPanelPopOut tripPanelPopOut = null;

        public bool IsTripPanelPopOutReady { get { return tripPanelPopOut != null && !tripPanelPopOut.IsFormClosed; } }

        public bool ToggleTripPanelPopOut()
        {
            if (tripPanelPopOut == null || tripPanelPopOut.IsFormClosed)
            {
                TripPanelPopOut p = new TripPanelPopOut(_discoveryForm);
                p.SetGripperColour(_discoveryForm.theme.LabelColor);
                p.SetTextColour(_discoveryForm.theme.SPanelColor);
                p.displayLastFSDOrFuel();
                p.Show();
                tripPanelPopOut = p;          // do it like this in case of race conditions 
                return true;
            }
            else
            {
                tripPanelPopOut.Close();      // there is no point null it, as if the user closes it, it never gets the chance to be nulled
                return false;
            }
        }

        void RedrawTripPanel(HistoryList hl)
        {
            if (IsTripPanelPopOutReady)
            {
                tripPanelPopOut.displayLastFSDOrFuel();
            }
        }
    
        #endregion
    }
}
