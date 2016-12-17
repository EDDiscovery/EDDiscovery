using EDDiscovery.DB;
using EDDiscovery2;
using EDDiscovery2.DB;
using EDDiscovery2.EDSM;
using EDDiscovery2.Forms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using EDDiscovery.EDSM;
using System.Threading.Tasks;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using EDDiscovery.HTTP;
using EDDiscovery.Forms;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery.EDDN;

namespace EDDiscovery
{

    public delegate void DistancesLoaded();

    public partial class EDDiscoveryForm : EDDiscoveryFormBase
    {
        #region Variables

        public const int WM_MOVE = 3;
        public const int WM_SIZE = 5;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCLBUTTONUP = 0xA2;
        public const int WM_NCMOUSEMOVE = 0xA0;
        public const int HT_CLIENT = 0x1;
        public const int HT_CAPTION = 0x2;
        public const int HT_LEFT = 0xA;
        public const int HT_RIGHT = 0xB;
        public const int HT_BOTTOM = 0xF;
        public const int HT_BOTTOMRIGHT = 0x11;
        public const int WM_NCL_RESIZE = 0x112;
        public const int HT_RESIZE = 61448;
        public const int WM_NCHITTEST = 0x84;

        // Mono compatibility
        private bool _window_dragging = false;
        private Point _window_dragMousePos = Point.Empty;
        private Point _window_dragWindowPos = Point.Empty;
        public EDDTheme theme;

        public TravelHistoryControl TravelControl { get { return travelHistoryControl1; } }
        public RouteControl RouteControl { get { return routeControl1; } }
        public ExportControl ExportControl { get { return exportControl1; } }
        public EDDiscovery2.ImageHandler.ImageHandler ImageHandler { get { return imageHandler1; } }

        public EDDiscovery2._3DMap.MapManager Map { get; private set; }

        private bool themeok = true;
        private Forms.SplashForm splashform = null;
        #endregion

        #region Initialisation

        public EDDiscoveryForm()
        {
            InitializeComponent();

            base.Init();
            label_version.Text = VersionDisplayString;

            theme = new EDDTheme();

            ToolStripManager.Renderer = theme.toolstripRenderer;
            theme.LoadThemes();                                         // default themes and ones on disk loaded
            themeok = theme.RestoreSettings();                                    // theme, remember your saved settings

            trilaterationControl.InitControl(this);
            travelHistoryControl1.InitControl(this);
            imageHandler1.InitControl(this);
            settings.InitControl(this);
            journalViewControl1.InitControl(this,0);
            routeControl1.InitControl(this);
            savedRouteExpeditionControl1.InitControl(this);
            exportControl1.InitControl(this);


            Map = new EDDiscovery2._3DMap.MapManager(option_nowindowreposition, travelHistoryControl1);

            this.TopMost = EDDConfig.KeepOnTop;

            ApplyTheme();
        }

        protected override void DatabaseInitializationComplete()
        {
            if (splashform != null)
            {
                splashform.Close();
            }
        }


        private void EDDiscoveryForm_Layout(object sender, LayoutEventArgs e)       // Manually position, could not get gripper under tab control with it sizing for the life of me
        {
        }

        private void EDDiscoveryForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsDatabaseInitialized)
                {
                    splashform = new SplashForm();
                    splashform.ShowDialog(this);
                }

                PostInit_Load();

                RepositionForm();
                InitFormControls();
                settings.InitSettingsTab();
                savedRouteExpeditionControl1.LoadControl();
                travelHistoryControl1.LoadControl();


                if (option_debugoptions)
                {
                    button_test.Visible = true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("EDDiscoveryForm_Load exception: " + ex.Message + "\n" + "Trace: " + ex.StackTrace);
            }
        }

        private void EDDiscoveryForm_Shown(object sender, EventArgs e)
        {
            PostInit_Shown();

            if (!themeok)
            {
                LogLineHighlight("The theme stored has missing colors or other missing information");
                LogLineHighlight("Correct the missing colors or other information manually using the Theme Editor in Settings");
            }
        }

        protected override void OnNewReleaseAvailable()
        {
            PanelInfoNewRelease();
        }

        private void PanelInfoNewRelease()
        {
            panelInfo.BackColor = Color.Green;
            labelPanelText.Text = "Download new release!";
            panelInfo.Visible = true;
        }


        private void InitFormControls()
        {
            labelPanelText.Text = "Loading. Please wait!";
            panelInfo.Visible = true;
            panelInfo.BackColor = Color.Gold;

            routeControl1.travelhistorycontrol1 = travelHistoryControl1;
        }

        private void RepositionForm()
        {
            var top = SQLiteDBClass.GetSettingInt("FormTop", -1);
            if (top >= 0 && option_nowindowreposition == false)
            {
                var left = SQLiteDBClass.GetSettingInt("FormLeft", 0);
                var height = SQLiteDBClass.GetSettingInt("FormHeight", 800);
                var width = SQLiteDBClass.GetSettingInt("FormWidth", 800);

                // Adjust so window fits on screen; just in case user unplugged a monitor or something

                var screen = SystemInformation.VirtualScreen;
                if (height > screen.Height) height = screen.Height;
                if (top + height > screen.Height + screen.Top) top = screen.Height + screen.Top - height;
                if (width > screen.Width) width = screen.Width;
                if (left + width > screen.Width + screen.Left) left = screen.Width + screen.Left - width;
                if (top < screen.Top) top = screen.Top;
                if (left < screen.Left) left = screen.Left;

                this.Top = top;
                this.Left = left;
                this.Height = height;
                this.Width = width;

                this.CreateParams.X = this.Left;
                this.CreateParams.Y = this.Top;
                this.StartPosition = FormStartPosition.Manual;

            }

            travelHistoryControl1.LoadLayoutSettings();
            journalViewControl1.LoadLayoutSettings();
        }

        private void EDDiscoveryForm_Activated(object sender, EventArgs e)
        {
        }

        public void ApplyTheme()
        {
            ToolStripManager.Renderer = theme.toolstripRenderer;
            panel_close.Visible = !theme.WindowsFrame;
            panel_minimize.Visible = !theme.WindowsFrame;
            label_version.Visible = !theme.WindowsFrame;

            this.Text = "EDDiscovery " + label_version.Text;            // note in no border mode, this is not visible on the title bar but it is in the taskbar..

            theme.ApplyToForm(this);

            InvokeOnHistoryChange(history);

            TravelControl.RedrawSummary();
        }

#endregion

#region Initial Check Systems


        protected override void OnCheckSystemsCompleted()
        {
            ReportProgress(-1, "");
            imageHandler1.StartWatcher();
            routeControl1.EnableRouteTab(); // now we have systems, we can update this..

            routeControl1.travelhistorycontrol1 = travelHistoryControl1;
            journalmonitor.OnNewJournalEntry += NewPosition;
            EdsmSync.OnDownloadedSystems += RefreshDueToEDSMDownloadedSystems;


            LogLine("Reading travel history");
            HistoryRefreshed += _travelHistoryControl1_InitialRefreshDone;

            RefreshHistoryAsync();

            panelInfo.Visible = false;

            if (EDDN.EDDNClass.CheckforEDMC()) // EDMC is running
            {
                if (EDDiscoveryForm.EDDConfig.CurrentCommander.SyncToEddn)  // Both EDD and EDMC should not sync to EDDN.
                {
                    LogLineHighlight("EDDiscovery and EDMarketConnector should not both sync to EDDN. Stop EDMC or uncheck 'send to EDDN' in settings tab!");
                }
            }
        }

        private void RefreshDueToEDSMDownloadedSystems()
        {
            Invoke((MethodInvoker)delegate
            {
                RefreshHistoryAsync();
            });
        }


        private void _travelHistoryControl1_InitialRefreshDone()
        {
            HistoryRefreshed -= _travelHistoryControl1_InitialRefreshDone;

            if (!PendingClose)
            {
                AsyncPerformSync();                              // perform any async synchronisations

                if (performeddbsync || performedsmsync)
                {
                    string databases = (performedsmsync && performeddbsync) ? "EDSM and EDDB" : ((performedsmsync) ? "EDSM" : "EDDB");

                    LogLine("ED Discovery will now synchronise to the " + databases + " databases to obtain star information." + Environment.NewLine +
                                    "This will take a while, up to 15 minutes, please be patient." + Environment.NewLine +
                                    "Please continue running ED Discovery until refresh is complete.");
                }
            }
        }


        private void _checkSystemsWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ReportProgress(e.ProgressPercentage, (string)e.UserState);
        }

        #endregion

        #region Async EDSM/EDDB Full Sync

        private void AsyncPerformSync()
        {
            if (!_syncWorker.IsBusy)
            {
                edsmRefreshTimer.Enabled = false;
                _syncWorker.RunWorkerAsync();
            }
        }

        private void _syncWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                var worker = (System.ComponentModel.BackgroundWorker)sender;

                PerformSync(() => worker.CancellationPending, (p, s) => worker.ReportProgress(p, s));
                if (worker.CancellationPending)
                    e.Cancel = true;
            }
            catch (Exception ex) { e.Result = ex; }       // ignore any excepctions
            finally
            {
                _syncWorkerCompletedEvent.Set();
            }
        }


        private void PerformSync(Func<bool> cancelRequested, Action<int, string> reportProgress)           // big check.. done in a thread.
        {
            reportProgress(-1, "");

            performhistoryrefresh = false;
            syncwasfirstrun = SystemClass.IsSystemsTableEmpty();                 // remember if DB is empty

            // Force a full sync if newest data is more than 14 days old

            bool outoforder = SQLiteConnectionSystem.GetSettingBool("EDSMSystemsOutOfOrder", true);
            DateTime lastmod = outoforder ? SystemClass.GetLastSystemModifiedTime() : SystemClass.GetLastSystemModifiedTimeFast();
            if (DateTime.UtcNow.Subtract(lastmod).TotalDays >= 14)
            {
                performedsmsync = true;
            }

            bool edsmoreddbsync = performedsmsync || performeddbsync;           // remember if we are syncing
            syncwaseddboredsm = edsmoreddbsync;

            if (performedsmsync || performeddbsync)
            {
                if (performedsmsync && !cancelRequested())
                {
                    // Download new systems
                    performhistoryrefresh |= PerformEDSMFullSync(this, cancelRequested, reportProgress);
                }

                if (!cancelRequested())
                {
                    LogLine("Indexing systems table");
                    SQLiteConnectionSystem.CreateSystemsTableIndexes();

                    PerformEDDBFullSync(cancelRequested, reportProgress);
                    performhistoryrefresh = true;
                }
            }

            if (!cancelRequested())
            {
                LogLine("Indexing systems table");
                SQLiteConnectionSystem.CreateSystemsTableIndexes();

                if (CanSkipSlowUpdates)
                {
                    LogLine("Skipping loading updates (DEBUG option). Need to turn this back on again? Look in the Settings tab.");
                }
                else
                {
                    lastmod = outoforder ? SystemClass.GetLastSystemModifiedTime() : SystemClass.GetLastSystemModifiedTimeFast();
                    if (DateTime.UtcNow.Subtract(lastmod).TotalHours >= 1)
                    {
                        LogLine("Checking for new EDSM systems (may take a few moments).");
                        EDSMClass edsm = new EDSMClass();
                        long updates = edsm.GetNewSystems(this, cancelRequested, reportProgress);
                        LogLine("EDSM updated " + updates + " systems.");
                        performhistoryrefresh |= (updates > 0);
                    }
                }
            }

            reportProgress(-1, "");
        }

        private void _syncWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Exception ex = e.Cancelled ? null : (e.Error ?? e.Result as Exception);
            ReportProgress(-1, "");

            if (!e.Cancelled && !PendingClose)
            {
                if (ex != null)
                {
                    LogLineHighlight("Check Systems exception: " + ex.Message + Environment.NewLine + "Trace: " + ex.StackTrace);
                }

                long totalsystems = SystemClass.GetTotalSystems();
                LogLineSuccess("Loading completed, total of " + totalsystems + " systems");

                if (performhistoryrefresh)
                {
                    LogLine("Refresh due to updating systems");
                    HistoryRefreshed += HistoryFinishedRefreshing;
                    RefreshHistoryAsync();
                }

                edsmRefreshTimer.Enabled = true;
            }
        }

        private void HistoryFinishedRefreshing()
        {
            HistoryRefreshed -= HistoryFinishedRefreshing;
            LogLine("Refreshing complete.");

            if (syncwasfirstrun)
            {
                LogLine("EDSM and EDDB update complete. Please restart ED Discovery to complete the synchronisation ");
            }
            else if (syncwaseddboredsm)
                LogLine("EDSM and/or EDDB update complete.");
        }

        private void _syncWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ReportProgress(e.ProgressPercentage, (string)e.UserState);
        }

#endregion

#region EDSM and EDDB syncs code

        private bool PerformEDSMFullSync(EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            string rwsystime = SQLiteConnectionSystem.GetSettingString("EDSMLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
            DateTime edsmdate;

            if (!DateTime.TryParse(rwsystime, CultureInfo.InvariantCulture, DateTimeStyles.None, out edsmdate))
            {
                edsmdate = new DateTime(2000, 1, 1);
            }

            long updates = 0;

            try
            {
                // Delete all old systems
                SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", "2010-01-01 00:00:00");
                SQLiteConnectionSystem.PutSettingString("EDDBSystemsTime", "0");

                EDSMClass edsm = new EDSMClass();

                LogLine("Get hidden systems from EDSM and remove from database");

                SystemClass.RemoveHiddenSystems();

                if (cancelRequested())
                    return false;

                LogLine("Download systems file from EDSM.");

                string edsmsystems = Path.Combine(Tools.GetAppDataDirectory(), "edsmsystems.json");

                LogLine("Resyncing all downloaded EDSM systems with local database." + Environment.NewLine + "This will take a while.");

                bool newfile;
                bool success = EDDiscovery2.HTTP.DownloadFileHandler.DownloadFile(EDSMClass.ServerAddress + "dump/systemsWithCoordinates.json", edsmsystems, out newfile, (n, s) =>
                {
                    SQLiteConnectionSystem.CreateTempSystemsTable();

                    string rwsysfiletime = "2014-01-01 00:00:00";
                    bool outoforder = false;
                    using (var reader = new StreamReader(s))
                        updates = SystemClass.ParseEDSMUpdateSystemsStream(reader, ref rwsysfiletime, ref outoforder, true, discoveryform, cancelRequested, reportProgress, useCache: false, useTempSystems: true);
                    if (!cancelRequested())       // abort, without saving time, to make it do it again
                    {
                        SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", rwsysfiletime);
                        LogLine("Replacing old systems table with new systems table and re-indexing - please wait");
                        reportProgress(-1, "Replacing old systems table with new systems table and re-indexing - please wait");
                        SQLiteConnectionSystem.ReplaceSystemsTable();
                        SQLiteConnectionSystem.PutSettingBool("EDSMSystemsOutOfOrder", outoforder);
                        reportProgress(-1, "");
                    }
                    else
                    {
                        throw new OperationCanceledException();
                    }
                });

                if (!success)
                {
                    LogLine("Failed to download EDSM system file from server, will check next time");
                    return false;
                }

                // Stop if requested
                if (cancelRequested())
                    return false;

                LogLine("Local database updated with EDSM data, " + updates + " systems updated.");

                performedsmsync = false;
                GC.Collect();
            }
            catch (Exception ex)
            {
                LogLineHighlight("GetAllEDSMSystems exception:" + ex.Message);
            }

            return (updates > 0);
        }

        private void edsmRefreshTimer_Tick(object sender, EventArgs e)
        {
            AsyncPerformSync();
        }

        private void PerformEDDBFullSync(Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            try
            {
                LogLine("Get systems from EDDB.");

                string eddbdir = Path.Combine(Tools.GetAppDataDirectory(), "eddb");
                if (!Directory.Exists(eddbdir))
                    Directory.CreateDirectory(eddbdir);

                string systemFileName = Path.Combine(eddbdir, "systems_populated.jsonl");

                bool success = EDDiscovery2.HTTP.DownloadFileHandler.DownloadFile("http://robert.astronet.se/Elite/eddb/v5/systems_populated.jsonl", systemFileName);

                if (success)
                {
                    if (cancelRequested())
                        return;

                    LogLine("Resyncing all downloaded EDDB data with local database." + Environment.NewLine + "This will take a while.");

                    long number = SystemClass.ParseEDDBUpdateSystems(systemFileName, LogLineHighlight);

                    LogLine("Local database updated with EDDB data, " + number + " systems updated");
                    SQLiteConnectionSystem.PutSettingString("EDDBSystemsTime", DateTime.UtcNow.Ticks.ToString());
                }
                else
                    LogLineHighlight("Failed to download EDDB Systems. Will try again next run.");

                GC.Collect();
                performeddbsync = false;
            }
            catch (Exception ex)
            {
                LogLineHighlight("GetEDDBUpdate exception: " + ex.Message);
            }
        }


        #endregion

        #region Logging

        protected override Color GetLogNormalColour() { return theme.TextBlockColor; }
        protected override Color GetLogHighlightColour() { return theme.TextBlockHighlightColor; }
        protected override Color GetLogSuccessColour() { return theme.TextBlockSuccessColor; }

        protected override void OnReportProgress(int percentComplete, string message)
        {
            if (!PendingClose)
            {
                if (percentComplete >= 0)
                {
                    this.toolStripProgressBar1.Visible = true;
                    this.toolStripProgressBar1.Value = percentComplete;
                }
                else
                {
                    this.toolStripProgressBar1.Visible = false;
                }

                this.toolStripStatusLabel1.Text = message;
            }
        }

#endregion

#region JSONandMisc
        static public string LoadJsonFile(string filename)
        {
            string json = null;
            try
            {
                if (!File.Exists(filename))
                    return null;

                StreamReader reader = new StreamReader(filename);
                json = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {
            }

            return json;
        }

        internal void ShowTrilaterationTab()
        {
            tabControl1.SelectedIndex = 1;
        }

#endregion

#region Closing

        private void SaveSettings()
        {
            settings.SaveSettings();

            SQLiteDBClass.PutSettingInt("FormWidth", this.Width);
            SQLiteDBClass.PutSettingInt("FormHeight", this.Height);
            SQLiteDBClass.PutSettingInt("FormTop", this.Top);
            SQLiteDBClass.PutSettingInt("FormLeft", this.Left);
            routeControl1.SaveSettings();
            theme.SaveSettings(null);
            travelHistoryControl1.SaveSettings();
            journalViewControl1.SaveSettings();

        }


        private void EDDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (safeClose == null)                  // so a close is a request now, and it launches a thread which cleans up the system..
            {
                e.Cancel = true;
                closeRequested.Set();
                edsmRefreshTimer.Enabled = false;
                EDDNSync.StopSync();
                _syncWorker.CancelAsync();
                labelPanelText.Text = "Closing, please wait!";
                panelInfo.Visible = true;
                LogLineHighlight("Closing down, please wait..");
                Console.WriteLine("Close.. safe close launched");
                safeClose = new Thread(SafeClose) { Name = "Close Down", IsBackground = true };
                safeClose.Start();
            }
            else if (safeClose.IsAlive)   // still working, cancel again..
            {
                e.Cancel = true;
            }
            else
            {
                Console.WriteLine("go for close");
            }
        }

        private void SafeClose()        // ASYNC thread..
        {
            Thread.Sleep(1000);

            Console.WriteLine("Waiting for full sync to close");
            if (_syncWorker.IsBusy)
                _syncWorkerCompletedEvent.WaitOne();

            Console.WriteLine("Stopping discrete threads");
            journalmonitor.StopMonitor();

            if (EdsmSync != null)
                EdsmSync.StopSync();

            travelHistoryControl1.CloseClosestSystemThread();

            Console.WriteLine("Go for close timer!");

            Invoke((MethodInvoker)delegate          // we need this thread to die so close will work, so kick off a timer
            {
                closeTimer = new System.Windows.Forms.Timer();
                closeTimer.Interval = 100;
                closeTimer.Tick += new EventHandler(CloseItFinally);
                closeTimer.Start();
            });
        }

        void CloseItFinally(Object sender, EventArgs e)
        {
            if (safeClose.IsAlive)      // still alive, try again
                closeTimer.Start();
            else
            {
                closeTimer.Stop();      // stop timer now. So it won't try to save it multiple times during close down if it takes a while - this caused a bug in saving some settings
                SaveSettings();         // do close now
                Close();
                Application.Exit();
            }
        }

#endregion

#region Buttons, Mouse, Menus

        private void button_test_Click(object sender, EventArgs e)
        {
        }

        private void addNewStarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://robert.astronet.se/Elite/ed-systems/entry.html");
        }

        private void frontierForumThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://forums.frontier.co.uk/showthread.php?t=138155&p=2113535#post2113535");
        }

        private void eDDiscoveryFGESupportThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://firstgreatexpedition.org/mybb/showthread.php?tid=1406");
        }

        private void eDDiscoveryHomepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/EDDiscovery/EDDiscovery/wiki");
        }

        private void openEliteDangerousDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (EliteDangerousClass.EDDirectory != null && !EliteDangerousClass.EDDirectory.Equals(""))
                    Process.Start(EliteDangerousClass.EDDirectory);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Open EliteDangerous directory exception: " + ex.Message);
            }

        }

        private void showLogfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                EDCommander cmdr = EDDConfig.Instance.ListOfCommanders.Find(x => x.Nr == EDDConfig.Instance.CurrentCmdrID);

                if (cmdr != null)
                {
                    string cmdrfolder = cmdr.JournalDir;
                    if (cmdrfolder.Length < 1)
                        cmdrfolder = EliteDangerous.EDJournalClass.GetDefaultJournalDir();
                    Process.Start(cmdrfolder);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Show log files exception: " + ex.Message);
            }
        }

        private void show2DMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSagCarinaMission frm = new FormSagCarinaMission(history.FilterByFSDAndPosition);
            frm.Nowindowreposition = option_nowindowreposition;
            frm.Show();
        }

        private void show3DMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TravelControl.buttonMap_Click(sender, e);
        }

        private void forceEDDBUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_syncWorker.IsBusy)      // we want it to have run, to completion, to allow another go..
            {
                performeddbsync = true;
                AsyncPerformSync();
            }
            else
                MessageBox.Show("Synchronisation to databases is in operation or pending, please wait");
        }

        private void syncEDSMSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_syncWorker.IsBusy)      // we want it to have run, to completion, to allow another go..
            {
                performedsmsync = true;
                AsyncPerformSync();
            }
            else
                MessageBox.Show("Synchronisation to databases is in operation or pending, please wait");
        }

        private void gitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/EDDiscovery/EDDiscovery");
        }

        private void reportIssueIdeasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/EDDiscovery/EDDiscovery/issues");
        }

        internal void keepOnTopChanged(bool keepOnTop)
        {
            this.TopMost = keepOnTop;
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void changeMapColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings.panel_defaultmapcolor_Click(sender, e);
        }

        private void editThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings.button_edittheme_Click(this, null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutBox()
        {
            AboutForm frm = new AboutForm();
            frm.labelVersion.Text = this.Text;
            frm.TopMost = EDDiscoveryForm.EDDConfig.KeepOnTop;
            frm.ShowDialog(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox();
        }

        private void eDDiscoveryChatDiscordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/0qIqfCQbziTWzsQu");
        }

        protected override void WndProc(ref Message m)
        {
            // Compatibility movement for Mono
            if (m.Msg == WM_LBUTTONDOWN && (int)m.WParam == 1 && !theme.WindowsFrame)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                _window_dragMousePos = new Point(x, y);
                _window_dragWindowPos = this.Location;
                _window_dragging = true;
                m.Result = IntPtr.Zero;
                this.Capture = true;
            }
            else if (m.Msg == WM_MOUSEMOVE && (int)m.WParam == 1 && _window_dragging)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                Point delta = new Point(x - _window_dragMousePos.X, y - _window_dragMousePos.Y);
                _window_dragWindowPos = new Point(_window_dragWindowPos.X + delta.X, _window_dragWindowPos.Y + delta.Y);
                this.Location = _window_dragWindowPos;
                this.Update();
                m.Result = IntPtr.Zero;
            }
            else if (m.Msg == WM_LBUTTONUP)
            {
                _window_dragging = false;
                _window_dragMousePos = Point.Empty;
                _window_dragWindowPos = Point.Empty;
                m.Result = IntPtr.Zero;
                this.Capture = false;
            }
            // Windows honours NCHITTEST; Mono does not
            else if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);
                //System.Diagnostics.Debug.WriteLine( Environment.TickCount + " Res " + ((int)m.Result));

                if ((int)m.Result == HT_CLIENT)
                {
                    int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                    int y = unchecked((short)((uint)m.LParam >> 16));
                    Point p = PointToClient(new Point(x, y));

                    if (p.X > this.ClientSize.Width - statusStrip1.Height && p.Y > this.ClientSize.Height - statusStrip1.Height)
                    {
                        m.Result = (IntPtr)HT_BOTTOMRIGHT;
                    }
                    else if ( p.Y > this.ClientSize.Height - statusStrip1.Height )
                    {
                        m.Result = (IntPtr)HT_BOTTOM;
                    }
                    else if (p.X > this.ClientSize.Width - 5)       // 5 is generous.. really only a few pixels gets thru before the subwindows grabs them
                    {
                        m.Result = (IntPtr)HT_RIGHT;
                    }
                    else if (p.X < 5)
                    {
                        m.Result = (IntPtr)HT_LEFT;
                    }
                    else if (!theme.WindowsFrame)
                    {
                        m.Result = (IntPtr)HT_CAPTION;
                    }
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }
        
        private void paneleddiscovery_Click(object sender, EventArgs e)
        {
            AboutBox();
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void read21AndFormerLogFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            adminToolStripMenuItem.DropDown.Close();
            if (DisplayedCommander >= 0)
            {
                EDCommander cmdr = EDDConfig.ListOfCommanders.Find(c => c.Nr == DisplayedCommander);
                if (cmdr != null)
                {
                    string netlogpath = cmdr.NetLogDir;
                    FolderBrowserDialog dirdlg = new FolderBrowserDialog();
                    if (netlogpath != null && Directory.Exists(netlogpath))
                    {
                        dirdlg.SelectedPath = netlogpath;
                    }

                    DialogResult dlgResult = dirdlg.ShowDialog();

                    if (dlgResult == DialogResult.OK)
                    {
                        string logpath = dirdlg.SelectedPath;

                        if (logpath != netlogpath)
                        {
                            cmdr.NetLogDir = logpath;
                            EDDConfig.UpdateCommanders(new List<EDCommander> { cmdr });
                        }

                        //string logpath = "c:\\games\\edlaunch\\products\\elite-dangerous-64\\logs";
                        RefreshHistoryAsync(netlogpath: logpath, forcenetlogreload: false, currentcmdr: cmdr.Nr);
                    }
                }
            }
        }

        private void read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DisplayedCommander >= 0)
            {
                EDCommander cmdr = EDDConfig.ListOfCommanders.Find(c => c.Nr == DisplayedCommander);
                if (cmdr != null)
                {
                    string netlogpath = cmdr.NetLogDir;
                    FolderBrowserDialog dirdlg = new FolderBrowserDialog();
                    if (netlogpath != null && Directory.Exists(netlogpath))
                    {
                        dirdlg.SelectedPath = netlogpath;
                    }

                    DialogResult dlgResult = dirdlg.ShowDialog();

                    if (dlgResult == DialogResult.OK)
                    {
                        string logpath = dirdlg.SelectedPath;

                        if (logpath != netlogpath)
                        {
                            cmdr.NetLogDir = logpath;
                            EDDConfig.UpdateCommanders(new List<EDCommander> { cmdr });
                        }

                        //string logpath = "c:\\games\\edlaunch\\products\\elite-dangerous-64\\logs";
                        RefreshHistoryAsync(netlogpath: logpath, forcenetlogreload: true, currentcmdr: cmdr.Nr);
                    }
                }
            }
        }

        private void dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm you wish to reset all history entries to the current commander", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                EliteDangerous.JournalEntry.ResetCommanderID(-1, EDDConfig.CurrentCommander.Nr);
                RefreshHistoryAsync();
            }
        }


        private void rescanAllJournalFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshHistoryAsync(forcejournalreload: true, checkedsm: true);
        }

        private void checkForNewReleaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckForNewinstaller())
            {
                if (newRelease != null)
                {
                    NewReleaseForm frm = new NewReleaseForm();
                    frm.release = newRelease;

                    frm.ShowDialog(this);
                }
            }
            else
            {
                MessageBox.Show("No new release found", "EDDiscovery", MessageBoxButtons.OK);
            }
        }

        private void deleteDuplicateFSDJumpEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm you remove any duplicate FSD entries from the current commander", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                int n = EliteDangerous.JournalEntry.RemoveDuplicateFSDEntries(EDDConfig.CurrentCommander.Nr);
                LogLine("Removed " + n + " FSD entries");
                RefreshHistoryAsync();
            }
        }

        private void panelInfo_Click(object sender, EventArgs e)
        {
            if (newRelease != null)
            {
                NewReleaseForm frm = new NewReleaseForm();
                frm.release = newRelease;

                frm.ShowDialog(this);
            }
        }

        private void labelPanelText_Click(object sender, EventArgs e)
        {
            if (newRelease != null)
            {
                NewReleaseForm frm = new NewReleaseForm();
                frm.release = newRelease;

                frm.ShowDialog(this);
            }
        }

        public void Open3DMap(HistoryEntry he)
        {
            this.Cursor = Cursors.WaitCursor;

            string HomeSystem = settings.MapHomeSystem;

            history.FillInPositionsFSDJumps();

            Map.Prepare(he?.System, HomeSystem,
                        settings.MapCentreOnSelection ? he?.System : SystemClass.GetSystem(String.IsNullOrEmpty(HomeSystem) ? "Sol" : HomeSystem),
                        settings.MapZoom, history.FilterByTravel);
            Map.Show();
            this.Cursor = Cursors.Default;
        }
        #endregion

        #region Update Data

        protected override void OnRefreshHistoryRequested()
        {
            travelHistoryControl1.RefreshButton(false);
            journalViewControl1.RefreshButton(false);

            journalmonitor.StopMonitor();          // this is called by the foreground.  Ensure background is stopped.  Foreground must restart it.
        }


        protected override void OnRefreshHistoryWorkerCompleted(RefreshWorkerResults res)
        {
            travelHistoryControl1.RefreshButton(true);
            journalViewControl1.RefreshButton(true);
        }

        protected override void OnNewBodyScan(JournalScan scan)
        {
            travelHistoryControl1.NewBodyScan(scan);
        }

        protected override void OnRefreshCommanders()
        {
            travelHistoryControl1.LoadCommandersListBox();  // because we may have new commanders
            settings.UpdateCommandersListBox();
        }

        #endregion



        private void sendUnsuncedEDDNEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<HistoryEntry> hlsyncunsyncedlist = history.FilterByScanNotEDDNSynced;        // first entry is oldest
            EDDNSync.SendEDDNEvents(this, hlsyncunsyncedlist);
        }

        private void materialSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindMaterialsForm frm = new FindMaterialsForm();

            frm.Show(this);


        }
    }
}

