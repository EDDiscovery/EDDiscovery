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

        private static EDDiscoveryForm _discoveryForm;

        SummaryPopOut summaryPopOut = null;
        List<EDCommander> commanders = null;

        Forms.UserControlFormList usercontrolsforms;

        string lastclosestname;
        SortedList<double, ISystem> lastclosestsystems;

        public TravelHistoryFilter GetPrimaryFilter { get { return userControlTravelGrid.GetHistoryFilter; } }  // some classes want to know out filter

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
            comboBoxCustomPopOut.Items.AddRange(new string[] { "Pop Out", "Travel Grid", "Log", "Nearest Stars" });
            comboBoxCustomPopOut.SelectedIndex = 0;
            comboBoxCustomPopOut.Enabled = true;

            userControlTravelGrid.Init(_discoveryForm,0);       // primary first instance
            userControlTravelGrid.OnChangedSelection += ChangedSelection;
            userControlTravelGrid.OnResort += Resort;

            // This one is the master computer of star distances, the other windows just use its results..
            UserControls.UserControlStarDistance primaryucsd = (UserControls.UserControlStarDistance)tabControlBottomRight.TabPages["tabPageBottomRightStarList"].Controls[0];
            primaryucsd.Init(_discoveryForm);
            primaryucsd.OnNewStarList += NewStarListComputed;
            primaryucsd.StartComputeThread();

            // Secondary star list.. the compute engine is not used in this one
            UserControls.UserControlStarDistance secondaryuscd = (UserControls.UserControlStarDistance)tabControlBottom.TabPages["tabPageBottomStarList"].Controls[0];
            secondaryuscd.Init(_discoveryForm);

            textBoxTarget.SetAutoCompletor(EDDiscovery.DB.SystemClass.ReturnSystemListForAutoComplete);

            buttonSync.Enabled = EDDiscoveryForm.EDDConfig.CurrentCommander.SyncToEdsm | EDDiscoveryForm.EDDConfig.CurrentCommander.SyncFromEdsm;

            imageListIcons.Images.Add(EDDiscovery.Properties.Resources.star);           // DONT use the dialog method, add it programatically, since we can't point at resources using the dialog
            imageListIcons.Images.Add(EDDiscovery.Properties.Resources.Log);
        }

        private void panel_topright_Resize(object sender, EventArgs e)
        {
            // Move controls around on topright

            int width = panel_topright.Width;
            int butoffsetx = buttonMap.Location.X - buttonMap2D.Location.X;
            int butoffsety = buttonMap2D.Location.Y - button_RefreshHistory.Location.Y;

            comboBoxCommander.Width = Math.Max(width - comboBoxCommander.Location.X - 4,64);

            if ( width >= buttonMap2D.Location.X + butoffsetx * 4 + buttonSync.Width + 4)  // 2x5
            {
                comboBoxCustomPopOut.Location = new Point(buttonMap2D.Location.X + butoffsetx * 2, buttonMap2D.Location.Y);
                buttonExtSummaryPopOut.Location = new Point(buttonMap2D.Location.X + butoffsetx * 3, buttonMap2D.Location.Y);
                buttonSync.Location = new Point(buttonMap2D.Location.X + butoffsetx * 4, buttonMap2D.Location.Y);
            }
            else if (width >= buttonMap2D.Location.X + butoffsetx * 3 + buttonExtSummaryPopOut.Width + 4)   // 2x4x1
            {
                comboBoxCustomPopOut.Location = new Point(buttonMap2D.Location.X + butoffsetx * 2, buttonMap2D.Location.Y);
                buttonExtSummaryPopOut.Location = new Point(buttonMap2D.Location.X + butoffsetx * 3, buttonMap2D.Location.Y);
                buttonSync.Location = new Point(buttonMap2D.Location.X, buttonMap2D.Location.Y + butoffsety);
            }
            else  //2x2x2x1
            {
                comboBoxCustomPopOut.Location = new Point(buttonMap2D.Location.X, buttonMap2D.Location.Y + butoffsety);
                buttonExtSummaryPopOut.Location = new Point(buttonMap2D.Location.X + butoffsetx, comboBoxCustomPopOut.Location.Y);
                buttonSync.Location = new Point(buttonMap2D.Location.X, buttonExtSummaryPopOut.Location.Y + butoffsety);
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

        #region New Stars

        private void NewStarListComputed(string name, SortedList<double, ISystem> csl)
        {
            lastclosestname = name;
            lastclosestsystems = csl;
            UserControls.UserControlStarDistance primaryucsd = (UserControls.UserControlStarDistance)tabControlBottomRight.TabPages["tabPageBottomRightStarList"].Controls[0];
            primaryucsd.FillGrid(name, csl);

            UserControls.UserControlStarDistance secondaryuscd = (UserControls.UserControlStarDistance)tabControlBottom.TabPages["tabPageBottomStarList"].Controls[0];
            secondaryuscd.FillGrid(name, csl);

            foreach (UserControlCommonBase uc in usercontrolsforms.GetListOfControls(typeof(UserControls.UserControlStarDistance)))
                ((UserControls.UserControlStarDistance)uc).FillGrid(name, csl);
        }

        public void CloseClosestSystemThread()
        {
            ((UserControls.UserControlStarDistance)(tabControlBottomRight.TabPages[0].Controls[0])).StopComputeThread();
        }

        #endregion

        #region Display history

        public void Display()
        {
            userControlTravelGrid.Display(_discoveryForm.history);

            List<UserControlCommonBase> lc = usercontrolsforms.GetListOfControls(typeof(UserControls.UserControlTravelGrid));
            foreach (UserControlCommonBase uc in lc)
                ((UserControls.UserControlTravelGrid)uc).Display(_discoveryForm.history);

            ShowSystemInformation(userControlTravelGrid.GetCurrentRow);
            RedrawSummary();
            RefreshTargetInfo();
            UpdateDependentsWithSelection();
            _discoveryForm.Map.UpdateSystemList(_discoveryForm.history.FilterByFSDAndPosition);           // update map
        }


        public void AddNewEntry(HistoryEntry he)
        {
            try
            {
                StoreSystemNote();

                if (he.IsFSDJump)
                {
                    int count = _discoveryForm.history.GetVisitsCount(he.System.name, he.System.id_edsm);
                    LogLine(string.Format("Arrived at system {0} Visit No. {1}", he.System.name, count));

                    System.Diagnostics.Trace.WriteLine("Arrived at system: " + he.System.name + " " + count + ":th visit.");

                    if (EDDiscoveryForm.EDDConfig.CurrentCommander.SyncToEdsm == true)
                        EDSMSync.SendTravelLog(he);
                }

                if (he.ISEDDNMessage)
                {
                    if (EDDiscoveryForm.EDDConfig.CurrentCommander.SyncToEddn == true)
                        EDDNSync.SendEDDNEvent(he);
                }

                if (he.IsFSDJump)
                    _discoveryForm.Map.UpdateSystemList(_discoveryForm.history.FilterByFSDAndPosition);           // update map - only cares about FSD changes

                if ( userControlTravelGrid.AddNewEntry(he) )                    // ask for it to add it, and if it did..
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

                foreach (UserControlCommonBase uc in usercontrolsforms.GetListOfControls(typeof(UserControls.UserControlTravelGrid)))
                    ((UserControls.UserControlTravelGrid)uc).AddNewEntry(he);       // update these as well..
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception NewPosition: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }

        }

        public void ShowSystemInformation(DataGridViewRow rw)
        {
            if (rw == null)
            {
                textBoxSystem.Text = textBoxX.Text = textBoxY.Text = textBoxZ.Text =
                textBoxAllegiance.Text = textBoxEconomy.Text = textBoxGovernment.Text =
                textBoxVisits.Text = textBoxState.Text = textBoxSolDist.Text = richTextBoxNote.Text = "";
                buttonRoss.Enabled = buttonEDDB.Enabled = false;
            }
            else
            {
                HistoryEntry syspos = userControlTravelGrid.GetHistoryEntry(rw.Index);     // reload, it may have changed
                Debug.Assert(syspos != null);

                _discoveryForm.history.FillEDSM(syspos, reload: true); // Fill in any EDSM info we have

                SystemNoteClass note = userControlTravelGrid.GetSystemNoteClass(rw.Index);

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

                int count = _discoveryForm.history.GetVisitsCount(syspos.System.name, syspos.System.id_edsm);
                textBoxVisits.Text = count.ToString();

                bool enableedddross = (syspos.System.id_eddb > 0);  // Only enable eddb/ross for system that it knows about

                buttonRoss.Enabled = buttonEDDB.Enabled = enableedddross;

                textBoxAllegiance.Text = EnumStringFormat(syspos.System.allegiance.ToString());
                textBoxEconomy.Text = EnumStringFormat(syspos.System.primary_economy.ToString());
                textBoxGovernment.Text = EnumStringFormat(syspos.System.government.ToString());
                textBoxState.Text = EnumStringFormat(syspos.System.state.ToString());
                richTextBoxNote.Text = EnumStringFormat(note != null ? note.Note : "");

                UserControls.UserControlStarDistance primaryucsd = (UserControls.UserControlStarDistance)tabControlBottomRight.TabPages["tabPageBottomRightStarList"].Controls[0];
                primaryucsd.Add(syspos.System);     // ONLY use the primary to compute the new list, the call back will populate all of them NewStarListComputed

            }
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
            splitContainerLeftRight.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterLR", splitContainerLeftRight.SplitterDistance);
            splitContainerLeft.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterL", splitContainerLeft.SplitterDistance);
            splitContainerRight.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterR", splitContainerRight.SplitterDistance);
            userControlTravelGrid.LoadLayout();

            tabControlBottomRight.SelectedIndex = SQLiteDBClass.GetSettingInt("TravelControlBottomRightTab", 0);
            tabControlBottom.SelectedIndex = SQLiteDBClass.GetSettingInt("TravelControlBottomTab", 0);
        }

        public void SaveSettings()     // called by form when closing
        {
            userControlTravelGrid.SaveLayout();
            SQLiteDBClass.PutSettingInt("TravelControlSpliterLR", splitContainerLeftRight.SplitterDistance);
            SQLiteDBClass.PutSettingInt("TravelControlSpliterL", splitContainerLeft.SplitterDistance);
            SQLiteDBClass.PutSettingInt("TravelControlSpliterR", splitContainerRight.SplitterDistance);

            SQLiteDBClass.PutSettingInt("TravelControlBottomRightTab", tabControlBottomRight.SelectedIndex);
            SQLiteDBClass.PutSettingInt("TravelControlBottomTab", tabControlBottom.SelectedIndex);
        }

        #endregion

        #region Clicks

        public void CheckCommandersListBox()
        {
            if (comboBoxCommander.Items.Count - 1 != EDDConfig.Instance.ListOfCommanders.Count)       // account for hidden log
                LoadCommandersListBox();
        }

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

            EDCommander currentcmdr = EDDiscoveryForm.EDDConfig.CurrentCommander;
            comboBoxCommander.SelectedIndex = commanders.IndexOf(currentcmdr);
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
            }
        }

        private void richTextBoxNote_Leave(object sender, EventArgs e)
        {
            StoreSystemNote();
        }

        private void richTextBoxNote_TextChanged(object sender, EventArgs e)
        {
            userControlTravelGrid.UpdateCurrentNote(richTextBoxNote.Text);

            foreach (UserControlCommonBase uc in usercontrolsforms.GetListOfControls(typeof(UserControls.UserControlTravelGrid)))
                ((UserControls.UserControlTravelGrid)uc).UpdateNoteJID(userControlTravelGrid.GetCurrentHistoryEntry.Journalid, richTextBoxNote.Text);
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

                LogLineHighlight("Exception : " + ex.Message);
                LogLineHighlight(ex.StackTrace);
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

        private void buttonExtTabControls_Click(object sender, EventArgs e)
        {
            //TBD
        }

        #endregion

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

            if ( sys != null )
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

        public void RefreshButton(bool state)
        {
            button_RefreshHistory.Enabled = state;
        }

        private void button_RefreshHistory_Click(object sender, EventArgs e)
        {
            LogLine("Refresh History.");
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

            UserControlForm tcf = usercontrolsforms.NewForm(_discoveryForm.option_nowindowreposition);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EDDiscovery.EDDiscoveryForm));
            tcf.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            if (comboBoxCustomPopOut.SelectedIndex == 1)
            {
                UserControls.UserControlTravelGrid uctg = new UserControls.UserControlTravelGrid();
                tcf.AddUserControl(uctg);
                int numopened = usercontrolsforms.CountOf(typeof(UserControls.UserControlTravelGrid));  // used to determine name and also key for DB

                tcf.Init("Travel History " + ((numopened > 1) ? numopened.ToString() : ""), _discoveryForm.theme.WindowsFrame, _discoveryForm.TopMost, "TravelHistory" + numopened);
                uctg.Init(_discoveryForm, numopened);
                uctg.Display(_discoveryForm.history);
            }
            else if (comboBoxCustomPopOut.SelectedIndex == 2)
            {
                UserControls.UserControlLog uclog = new UserControls.UserControlLog(); // Add a log
                tcf.AddUserControl(uclog);
                int numopened = usercontrolsforms.CountOf(typeof(UserControls.UserControlLog));

                tcf.Init("Log " + ((numopened > 1) ? numopened.ToString() : ""), _discoveryForm.theme.WindowsFrame, _discoveryForm.TopMost, "Log" + numopened);
                UserControls.UserControlLog primarylog = (UserControls.UserControlLog)tabControlBottom.TabPages["tabPageBottomLog"].Controls[0];
                uclog.CopyTextFrom(primarylog);
            }
            else if (comboBoxCustomPopOut.SelectedIndex == 3)
            {
                UserControls.UserControlStarDistance ucsd = new UserControls.UserControlStarDistance(); // Add a closest distance tab
                tcf.AddUserControl(ucsd);
                int numopened = usercontrolsforms.CountOf(typeof(UserControls.UserControlStarDistance));

                tcf.Init("Nearest Stars " + ((numopened > 1) ? numopened.ToString() : ""), _discoveryForm.theme.WindowsFrame, _discoveryForm.TopMost, "StarDistance" + numopened);
                if (lastclosestsystems != null)           // if we have some, fill in this grid
                    ucsd.FillGrid(lastclosestname, lastclosestsystems);
            }

            comboBoxCustomPopOut.Enabled = false;
            comboBoxCustomPopOut.SelectedIndex = 0;
            comboBoxCustomPopOut.Enabled = true;

            _discoveryForm.theme.ApplyColors(tcf);
            tcf.Show();
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
                summaryPopOut.RefreshTarget(userControlTravelGrid.TravelGrid, _discoveryForm.history.GetLastWithPosition);
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
                summaryPopOut.SetGripperColour(_discoveryForm.theme.LabelColor);
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

        private void buttonExtSummaryPopOut_Click(object sender, EventArgs e)
        {
            ToggleSummaryPopOut();
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
                    UserControls.UserControlLog primarylog = (UserControls.UserControlLog)tabControlBottom.TabPages["tabPageBottomLog"].Controls[0];
                    primarylog.AppendText(text + Environment.NewLine, color);

                    UserControls.UserControlLog secondarylog = (UserControls.UserControlLog)tabControlBottomRight.TabPages["tabPageBottomRightLog"].Controls[0];
                    secondarylog.AppendText(text + Environment.NewLine, color);

                    foreach (UserControlCommonBase uc in usercontrolsforms.GetListOfControls(typeof(UserControls.UserControlLog)))
                        ((UserControls.UserControlLog)uc).AppendText(text + Environment.NewLine, color);
                });
            }
            catch { }
        }

        #endregion

    }
}
