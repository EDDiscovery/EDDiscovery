/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EDDiscovery.DB;
using System.Diagnostics;
using EDDiscovery.EDSM;
using System.Threading.Tasks;
using EDDiscovery.Controls;
using System.Threading;
using System.Collections.Concurrent;
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

        List<EDCommander> commanders = null;

        string lastclosestname;
        SortedList<double, ISystem> lastclosestsystems;

        string[] spanelbuttonlist = new string[]            // MUST match PopOuts list order
        {
            "S-Panel", "Trip-Panel", "Note Panel", "Route Tracker", // not in tabs
            "Log", "Nearest Stars" , "Materials", "Commodities" , "Ledger" , "Journal", // matching PopOuts order
            "Travel Grid" , "Screen Shot", "Statistics" , "Scan" , "Loadout" , "Exploration", "Synthesis" , "Missions", "Engineering", "Market Data"
        };

        Bitmap[] tabbitmaps = new Bitmap[] { EDDiscovery.Properties.Resources.Log,      // Match pop out enum PopOuts, from start, list only ones which should be in tabs
                                        EDDiscovery.Properties.Resources.star,
                                        EDDiscovery.Properties.Resources.material ,
                                        EDDiscovery.Properties.Resources.commodities,
                                        EDDiscovery.Properties.Resources.ledger ,
                                        EDDiscovery.Properties.Resources.journal ,
                                        EDDiscovery.Properties.Resources.travelgrid ,
                                        EDDiscovery.Properties.Resources.screenshot,
                                        EDDiscovery.Properties.Resources.stats,
                                        EDDiscovery.Properties.Resources.scan,
                                        EDDiscovery.Properties.Resources.module,
                                        EDDiscovery.Properties.Resources.sellexplorationdata,
                                        EDDiscovery.Properties.Resources.synthesis,
                                        EDDiscovery.Properties.Resources.missionaccepted,
                                        EDDiscovery.Properties.Resources.engineercraft,
                                        EDDiscovery.Properties.Resources.marketdata,
                                        };

        string[] tabtooltips = new string[] { "Display the program log",     // MAtch Pop out enum
                                               "Display the nearest stars to the currently selected entry",
                                               "Display the material count at the currently selected entry",
                                               "Display the commodity count at the currently selected entry",
                                               "Display a ledger of cash related entries",
                                               "Display the journal grid view",
                                               "Display the history grid view",
                                               "Display the screen shot view",
                                               "Display statistics from the history",
                                               "Display scan data",
                                               "Display Loadout for current ships and also stored modules",
                                               "Display Exploration view",
                                               "Display Synthesis planner",
                                               "Display Missions",
                                               "Display Engineering planner",
                                               "Display Market Data (Requires login to Frontier using Commander Frontier log in details)",
                                            };

        HistoryEntry notedisplayedhe = null;            // remember the particulars of the note displayed, so we can save it later

        public HistoryEntry GetTravelHistoryCurrent {  get { return userControlTravelGrid.GetCurrentHistoryEntry; } }
        public TravelHistoryFilter GetPrimaryFilter { get { return userControlTravelGrid.GetHistoryFilter; } }  // some classes want to know out filter

        public ExtendedControls.TabStrip GetTabStrip( string name )
        {
            if (name.Equals(tabStripBottom.Name, StringComparison.InvariantCultureIgnoreCase))
                return tabStripBottom;
            if (name.Equals(tabStripBottomRight.Name, StringComparison.InvariantCultureIgnoreCase))
                return tabStripBottomRight;
            if (name.Equals(tabStripMiddleRight.Name, StringComparison.InvariantCultureIgnoreCase))
                return tabStripMiddleRight;
            return null;
        }

        // Subscribe to these to get various events - layout controls via their Init function do this.

        public delegate void TravelSelectionChanged(HistoryEntry he, HistoryList hl);       // called when current travel sel changed
        public event TravelSelectionChanged OnTravelSelectionChanged;

        public delegate void NearestStarList(string name, SortedList<double, ISystem> csl); // called when star computation has a new list
        public event NearestStarList OnNearestStarListChanged;


        #region Initialisation

        public TravelHistoryControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            _discoveryForm.OnNewTarget += RefreshTargetDisplay;

            richTextBoxNote.TextBoxChanged += richTextBoxNote_TextChanged;

            LoadCommandersListBox();

            comboBoxCustomPopOut.Enabled = false;

            comboBoxCustomPopOut.Items.AddRange(spanelbuttonlist);
            comboBoxCustomPopOut.SelectedIndex = 0;
            comboBoxCustomPopOut.Enabled = true;

            userControlTravelGrid.Init(_discoveryForm, 0);       // primary first instance - this registers with events in discoveryform to get info
                                                        // then this display, to update its own controls..
            userControlTravelGrid.OnRedisplay += UpdatedDisplay;        // after the TG has redisplayed..
            userControlTravelGrid.OnAddedNewEntry += UpdatedWithAddNewEntry;        // call back when you've added a new entry..
            userControlTravelGrid.OnChangedSelection += ChangedSelection;   // and if the user clicks on something
            userControlTravelGrid.OnResort += Resort;   // and if he or she resorts
            userControlTravelGrid.OnPopOut += TGPopOut;

            TabConfigure(tabStripBottom,"Bottom",1000);          // codes are used to save info, 0 = primary (journal/travelgrid), 1..N are popups, these are embedded UCs
            TabConfigure(tabStripBottomRight,"Bottom-Right",1001);
            TabConfigure(tabStripMiddleRight,"Middle-Right",1002);

            textBoxTarget.SetAutoCompletor(EDDiscovery.DB.SystemClass.ReturnSystemListForAutoComplete);

            buttonSync.Enabled = EDCommander.Current.SyncToEdsm | EDCommander.Current.SyncFromEdsm;
        }

        public void LoadControl()
        {
        }

        #endregion

        #region TAB control

        void TabConfigure(ExtendedControls.TabStrip t, string name, int displayno)
        {
            t.Images = tabbitmaps;
            t.ToolTips = tabtooltips;
            t.Tag = displayno;             // these are IDs for purposes of identifying different instances of a control.. 0 = main ones (main travel grid, main tab journal). 1..N are popups
            t.OnRemoving += TabRemoved;
            t.OnCreateTab += TabCreate;
            t.OnPostCreateTab += TabPostCreate;
            t.OnPopOut += TabPopOut;
            t.Name = name;
        }

        void TabRemoved(ExtendedControls.TabStrip t, Control c )     // called by tab strip when a control is removed
        {
            UserControlCommonBase uccb = c as UserControlCommonBase;
            uccb.Closing();
        }

        Control TabCreate(ExtendedControls.TabStrip t, int si)        // called by tab strip when selected index changes.. create a new one.. only create.
        {
            PopOutControl.PopOuts i = (PopOutControl.PopOuts)(si + PopOutControl.PopOuts.StartTabButtons);

            _discoveryForm.ActionRun("onPanelChange", "UserUIEvent", null, new ConditionVariables(new string[] { "PanelTabName", PopOutControl.popoutinfo[i].WindowRefName, "PanelTabTitle" , PopOutControl.popoutinfo[i].WindowTitlePrefix , "PanelName" , t.Name }));

            return PopOutControl.Create(i);
        }

        void TabPostCreate(ExtendedControls.TabStrip t, Control ctrl , int i)        // called by tab strip after control has been added..
        {                                                           // now we can do the configure of it, with the knowledge the tab has the right size
            int displaynumber = (int)t.Tag;                         // tab strip - use tag to remember display id which helps us save context.

            UserControlCommonBase uc = ctrl as UserControlCommonBase;

            if (uc != null)
            {
                UserControlPostCreate(displaynumber, uc);
                uc.Display(userControlTravelGrid.GetCurrentHistoryEntry, _discoveryForm.history);
            }

            //System.Diagnostics.Debug.WriteLine("And theme {0}", i);
            _discoveryForm.theme.ApplyToControls(t);
        }

        public void UserControlPostCreate(int displaynumber, UserControlCommonBase ctrl)
        {
            ctrl.Init(_discoveryForm, displaynumber);
            ctrl.LoadLayout();

            if (ctrl is UserControlLog)
            {
                UserControlLog sc = ctrl as UserControlLog;
                sc.AppendText(_discoveryForm.LogText, _discoveryForm.theme.TextBlockColor);
            }
            else if (ctrl is UserControlStarDistance)
            {
                UserControlStarDistance sc = ctrl as UserControlStarDistance;
                if (lastclosestsystems != null)           // if we have some, fill in this grid
                    sc.FillGrid(lastclosestname, lastclosestsystems);
            }
            else if (ctrl is UserControlMaterials)
            {
                UserControlMaterials ucm = ctrl as UserControlMaterials;
            }
            else if (ctrl is UserControlCommodities)
            {
                UserControlCommodities ucm = ctrl as UserControlCommodities;
            }
            else if (ctrl is UserControlLedger)
            {
                UserControlLedger ucm = ctrl as UserControlLedger;
                ucm.OnGotoJID += GotoJID;
            }
            else if (ctrl is UserControlJournalGrid)
            {
                UserControlJournalGrid ucm = ctrl as UserControlJournalGrid;
                ucm.NoHistoryIcon();
                ucm.NoPopOutIcon();
            }
            else if (ctrl is UserControlTravelGrid)
            {
                UserControlTravelGrid ucm = ctrl as UserControlTravelGrid;
                ucm.NoHistoryIcon();
                ucm.NoPopOutIcon();
            }
        }

        void TabPopOut(ExtendedControls.TabStrip t, int i)        // pop out clicked
        {
            _discoveryForm.PopOuts.PopOut((PopOutControl.PopOuts)(i+ PopOutControl.PopOuts.StartTabButtons));
        }

        #endregion

        #region Panel sizing

        private void panel_topright_Resize(object sender, EventArgs e)
        {
            // Move controls around on topright

            int width = panel_topright.Width;
            int xpos = buttonMap2D.Left;
            int ypos = buttonMap2D.Top;
            int butoffsetx = buttonMap.Left - buttonMap2D.Left;
            int butoffsety = buttonMap2D.Top - button_RefreshHistory.Top;

            // Refresh, Cmdr label, Cmdr Dropdown
            comboBoxCommander.Width = Math.Min(Math.Max(width - comboBoxCommander.Location.X - 4,64),192);

            // always 2dmap, 3dmap
            if (width >= xpos + butoffsetx * 3 + buttonSync.Width + 4)  // 2(r,cmd) + 4 (2dmap, 3dmap, popout, sync)
            {
                comboBoxCustomPopOut.Location = new Point(xpos + butoffsetx * 2, ypos);
                buttonSync.Location = new Point(xpos + butoffsetx * 3, ypos);
            }
            else if (width >= xpos + butoffsetx * 2 + comboBoxCustomPopOut.Width + 4)  // 2(r,cmd) + 2 (2d,3d) + 2 (popout, sync)
            {
                comboBoxCustomPopOut.Location = new Point(xpos , ypos + butoffsety);
                buttonSync.Location = new Point(xpos + butoffsetx * 1, ypos + butoffsety);
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

            if (width > 250)                            // can we fit onto one line?
            {
                labelTarget.Location = new Point(2, 2);
                textBoxTarget.Location = new Point(labelTarget.Right + 6, labelTarget.Location.Y);
                textBoxTarget.Width = width - textBoxTarget.Location.X - 20 - textBoxTargetDist.Width - buttonEDSMTarget.Width;
                textBoxTargetDist.Location = new Point(textBoxTarget.Right + 4, labelTarget.Location.Y);
                buttonEDSMTarget.Location = new Point(textBoxTargetDist.Right + 4, labelTarget.Location.Y-2);
            }
            else
            {
                labelNote.Location = new Point(2, 2);
                textBoxTarget.Location = new Point(2, labelNote.Location.Y + labelNote.Height + 8);
                textBoxTarget.Width = width - 4;
                textBoxTargetDist.Location = new Point(2, textBoxTarget.Bottom + 8);
                buttonEDSMTarget.Location = new Point(textBoxTargetDist.Right + 4, textBoxTargetDist.Top-2);
            }

            panelTarget.Height = buttonEDSMTarget.Bottom + 6;
        }

        #endregion

        void GotoJID(long v)
        {
            userControlTravelGrid.GotoPosByJID(v);
        }

        #region New Stars

        private void NewStarListComputed(string name, SortedList<double, ISystem> csl)      // thread..
        {
            BeginInvoke((MethodInvoker)delegate
            {
                lastclosestname = name;
                lastclosestsystems = csl;

                if (OnNearestStarListChanged != null)
                    OnNearestStarListChanged(name, csl);
            });
        }

        #endregion

        #region Display history

        public void UpdatedDisplay(HistoryList hl)                      // called from main travelgrid when refreshed display
        {
            ShowSystemInformation(userControlTravelGrid.GetCurrentRow);
            RefreshTargetDisplay();
            UpdateDependentsWithSelection();
        }

        public void UpdatedWithAddNewEntry(HistoryEntry he, HistoryList hl, bool accepted)     // main travel grid calls after getting a new entry
        {
            try
            {   // try is a bit old, probably do not need it.
                if (he.IsFSDJump)
                {
                    int count = _discoveryForm.history.GetVisitsCount(he.System.name);
                    _discoveryForm.LogLine(string.Format("Arrived at system {0} Visit No. {1}", he.System.name, count));

                    System.Diagnostics.Trace.WriteLine("Arrived at system: " + he.System.name + " " + count + ":th visit.");

                    if (EDCommander.Current.SyncToEdsm == true)
                        EDSMSync.SendTravelLog(he);
                }

                if (he.ISEDDNMessage && he.AgeOfEntry() < TimeSpan.FromDays(1.0))
                {
                    if (EDCommander.Current.SyncToEddn == true)
                    {
                        EDDNSync.SendEDDNEvents(_discoveryForm, he);
                    }
                }

                if ( accepted )                                                 // if accepted it on main grid..
                {
                    RefreshTargetDisplay();                                     // tell the target system its changed the latest system

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
            StoreSystemNote(true);      // save any previous note

            HistoryEntry he = null;

            if (rw == null)
            {
                textBoxSystem.Text = textBoxBody.Text = textBoxX.Text = textBoxY.Text = textBoxZ.Text =
                textBoxAllegiance.Text = textBoxEconomy.Text = textBoxGovernment.Text =
                textBoxVisits.Text = textBoxState.Text = textBoxHomeDist.Text = richTextBoxNote.Text = textBoxSolDist.Text = "";
                buttonRoss.Enabled = buttonEDDB.Enabled = false;
            }
            else
            {
                he = userControlTravelGrid.GetHistoryEntry(rw.Index);     // reload, it may have changed
                Debug.Assert(he != null);

                _discoveryForm.history.FillEDSM(he, reload: true); // Fill in any EDSM info we have, force it to try again.. in case system db updated

                notedisplayedhe = he;

                textBoxSystem.Text = he.System.name;
                textBoxBody.Text = he.WhereAmI;

                if (he.System.HasCoordinate)         // cursystem has them?
                {
                    textBoxX.Text = he.System.x.ToString(SingleCoordinateFormat);
                    textBoxY.Text = he.System.y.ToString(SingleCoordinateFormat);
                    textBoxZ.Text = he.System.z.ToString(SingleCoordinateFormat);

                    ISystem homesys = _discoveryForm.GetHomeSystem();

                    toolTipEddb.SetToolTip(textBoxHomeDist, $"Distance to home system ({homesys.name})");
                    textBoxHomeDist.Text = SystemClass.Distance(he.System, homesys).ToString(SingleCoordinateFormat);
                    textBoxSolDist.Text = SystemClass.Distance(he.System, 0, 0, 0).ToString(SingleCoordinateFormat);
                }
                else
                {
                    textBoxX.Text = "?";
                    textBoxY.Text = "?";
                    textBoxZ.Text = "?";
                    textBoxHomeDist.Text = "";
                    textBoxSolDist.Text = "";
                }

                int count = _discoveryForm.history.GetVisitsCount(he.System.name);
                textBoxVisits.Text = count.ToString();

                bool enableedddross = (he.System.id_eddb > 0);  // Only enable eddb/ross for system that it knows about

                buttonRoss.Enabled = buttonEDDB.Enabled = enableedddross;

                textBoxAllegiance.Text = he.System.allegiance.ToNullUnknownString();
                textBoxEconomy.Text = he.System.primary_economy.ToNullUnknownString();
                textBoxGovernment.Text = he.System.government.ToNullUnknownString();
                textBoxState.Text = he.System.state.ToNullUnknownString();
                richTextBoxNote.Text = he.snc != null ? he.snc.Note : "";

                _discoveryForm.CalculateClosestSystems(he.System, (s, d) => NewStarListComputed(s.name, d));
            }

            if (OnTravelSelectionChanged != null)
                OnTravelSelectionChanged(he, _discoveryForm.history);
        }

        public void UpdateNoteJID(long jid, string txt)
        {
            userControlTravelGrid.UpdateNoteJID(jid, txt);
            if (notedisplayedhe != null && notedisplayedhe.Journalid == jid)
            {
                string oldtext = richTextBoxNote.Text.Trim();

                if (oldtext != txt)
                {
                    richTextBoxNote.Text = txt;
                }
            }
        }

        #endregion


        #region Grid Layout

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            // ORDER IMPORTANT for right outer/inner splitter, otherwise windows fixes it 

            if (!EDDConfig.Options.NoWindowReposition)
            {
                try
                {
                    splitContainerLeftRight.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterLR", splitContainerLeftRight.SplitterDistance);
                    splitContainerLeft.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterL", splitContainerLeft.SplitterDistance);
                    splitContainerRightOuter.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterRO", splitContainerRightOuter.SplitterDistance);
                    splitContainerRightInner.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterR", splitContainerRightInner.SplitterDistance);
                }
                catch { };          // so splitter can except, if values are strange, but we don't really care, so lets throw away the exception
            }

            userControlTravelGrid.LoadLayout();

            // NO NEED to reload the three tabstrips - code below will cause a LoadLayout on the one selected.

            int max = (int)PopOutControl.PopOuts.MaxTabButtons;
            tabStripBottom.SelectedIndex = Math.Min( SQLiteDBClass.GetSettingInt("TravelControlBottomTab", (int)(PopOutControl.PopOuts.Scan - PopOutControl.PopOuts.StartTabButtons)), max);
            tabStripBottomRight.SelectedIndex = Math.Min(SQLiteDBClass.GetSettingInt("TravelControlBottomRightTab", (int)(PopOutControl.PopOuts.Log - PopOutControl.PopOuts.StartTabButtons) ), max );
            tabStripMiddleRight.SelectedIndex = Math.Min(SQLiteDBClass.GetSettingInt("TravelControlMiddleRightTab", (int)(PopOutControl.PopOuts.StarDistance - PopOutControl.PopOuts.StartTabButtons)), max);
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
            commanders.AddRange(EDCommander.GetList());

            comboBoxCommander.DataSource = null;
            comboBoxCommander.DataSource = commanders;
            comboBoxCommander.ValueMember = "Nr";
            comboBoxCommander.DisplayMember = "Name";

            if (_discoveryForm.history.CommanderId == -1)
                comboBoxCommander.SelectedIndex = 0;
            else
                comboBoxCommander.SelectedItem = EDCommander.Current;

            comboBoxCommander.Enabled = true;
        }

        private void comboBoxCommander_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCommander.SelectedIndex >= 0 && comboBoxCommander.Enabled)     // DONT trigger during LoadCommandersListBox
            {
                var itm = (EDCommander)comboBoxCommander.SelectedItem;
                if (itm.Nr >= 0)
                    EDCommander.CurrentCmdrID = itm.Nr;

                buttonSync.Enabled = EDCommander.Current.SyncToEdsm | EDCommander.Current.SyncFromEdsm;

                _discoveryForm.RefreshHistoryAsync(currentcmdr: itm.Nr);                                   // which will cause DIsplay to be called as some point
            }
        }

        public void buttonMap_Click(object sender, EventArgs e)
        {
            _discoveryForm.Open3DMap(userControlTravelGrid.GetCurrentHistoryEntry);
        }

        private void Resort()       // user travel grid to say it resorted
        {
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

                if (userControlTravelGrid.GetCurrentHistoryEntry!= null)        // paranoia
                    _discoveryForm.ActionRun("onHistorySelection", "UserUIEvent", userControlTravelGrid.GetCurrentHistoryEntry);

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
            StoreSystemNote(true);
        }

        private void richTextBoxNote_TextChanged(object sender, EventArgs e)
        {
            StoreSystemNote(false);
        }

        private void StoreSystemNote(bool send)
        {
            _discoveryForm.StoreSystemNote(notedisplayedhe, richTextBoxNote.Text.Trim(), send);
        }

        public void buttonSync_Click(object sender, EventArgs e)
        {
            EDSMClass edsm = new EDSMClass();

            if (!edsm.IsApiKeySet)
            {
                EDDiscovery.Forms.MessageBoxTheme.Show("Please ensure a commander is selected and it has a EDSM API key set");
                return;
            }

            try
            {
                _discoveryForm.EdsmSync.StartSync(edsm, EDCommander.Current.SyncToEdsm, EDCommander.Current.SyncFromEdsm, EDDConfig.Instance.DefaultMapColour);
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

        private void buttonEDSM_Click(object sender, EventArgs e)
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
                        EDDiscovery.Forms.MessageBoxTheme.Show("System unknown to EDSM");
                }
            }
        }

        public void RefreshButton(bool state)
        {
            button_RefreshHistory.Enabled = state;
            _discoveryForm.PopOuts.SetRefreshState(state);
        }

        private void button_RefreshHistory_Click(object sender, EventArgs e)
        {
            _discoveryForm.LogLine("Refresh History.");
            _discoveryForm.RefreshHistoryAsync(checkedsm: true);
        }

        private void button2DMap_Click(object sender, EventArgs e)
        {
            _discoveryForm.Open2DMap();
        }
        
        private void comboBoxCustomPopOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!comboBoxCustomPopOut.Enabled)
                return;

            _discoveryForm.PopOuts.PopOut((PopOutControl.PopOuts)(comboBoxCustomPopOut.SelectedIndex));

            comboBoxCustomPopOut.Enabled = false;
            comboBoxCustomPopOut.SelectedIndex = 0;
            comboBoxCustomPopOut.Enabled = true;
        }


        void TGPopOut()
        {
            _discoveryForm.PopOuts.PopOut(PopOutControl.PopOuts.TravelGrid);
        }

        #endregion

        #region Target System

        private void textBoxTarget_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RoutingUtils.setTargetSystem(_discoveryForm, textBoxTarget.Text);
            }
        }

        public void RefreshTargetDisplay()              // called when a target has been changed.. via EDDiscoveryform
        {
            string name;
            double x, y, z;

            System.Diagnostics.Debug.WriteLine("Refresh target display");

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
        }

        private void buttonEDSMTarget_Click(object sender, EventArgs e)
        {
            EDSMClass edsm = new EDSMClass();
            string url = edsm.GetUrlToEDSMSystem(textBoxTarget.Text, null);

            if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                Process.Start(url);
            else
                EDDiscovery.Forms.MessageBoxTheme.Show("System unknown to EDSM");


        }

        #endregion

    }
}
