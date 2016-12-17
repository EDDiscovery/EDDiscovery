using EDDiscovery.DB;
using EDDiscovery.EDSM;
using EDDiscovery.EliteDangerous;
using EDDiscovery.HTTP;
using EDDiscovery2;
using EDDiscovery2.DB;
using EDDiscovery2.EDSM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery
{
    public class EDDiscoveryFormBase : Form
    {
        protected class RefreshWorkerArgs
        {
            public string NetLogPath;
            public bool ForceNetLogReload;
            public bool ForceJournalReload;
            public bool CheckEdsm;
            public int CurrentCommander;
        }

        protected class RefreshWorkerResults
        {
            public List<HistoryEntry> rethistory;
            public MaterialCommoditiesLedger retledger;
            public StarScan retstarscan;
        }

        protected class TraceLogWriter : TextWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
            public override IFormatProvider FormatProvider { get { return CultureInfo.InvariantCulture; } }
            public override void Write(string value) { Trace.Write(value); }
            public override void WriteLine(string value) { Trace.WriteLine(value); }
            public override void WriteLine() { Trace.WriteLine(""); }
        }

        #region Properties, Fields and Events

        #region Public Properties
        public int DisplayedCommander { get; set; } = 0;
        public HistoryList history { get; protected set; } = new HistoryList();
        public bool option_nowindowreposition { get; protected set; } = false;                             // Cmd line options
        public bool option_debugoptions { get; protected set; } = false;
        public EDSMSync EdsmSync { get; protected set; }
        public string LogText { get { return logtext; } }
        public bool PendingClose { get { return safeClose != null; } }           // we want to close boys!

        public static EDDConfig EDDConfig { get; protected set; }
        public static GalacticMapping galacticMapping { get; protected set; }
        #endregion

        #region Events
        public event Action HistoryRefreshed; // this is an internal hook
        public event Action<HistoryList> OnHistoryChange;
        public event Action<HistoryEntry, HistoryList> OnNewEntry;
        public event Action<string, Color> OnNewLogEntry;
        #endregion

        #region Protected Properties or Fields
        protected ManualResetEvent _syncWorkerCompletedEvent = new ManualResetEvent(false);
        protected ManualResetEvent _checkSystemsWorkerCompletedEvent = new ManualResetEvent(false);
        protected Task<bool> downloadMapsTask = null;
        protected Task checkInstallerTask = null;
        protected string logname = "";
        protected BackgroundWorker dbinitworker = null;
        protected EDJournalClass journalmonitor;
        protected GitHubRelease newRelease;
        protected bool performedsmsync = false;
        protected bool performeddbsync = false;
        protected string logtext = "";     // to keep in case of no logs..
        protected bool performhistoryrefresh = false;
        protected bool syncwasfirstrun = false;
        protected bool syncwaseddboredsm = false;
        protected Thread safeClose;
        protected System.Windows.Forms.Timer closeTimer;
        protected Thread backgroundWorker;
        protected AutoResetEvent readyForInitialLoad;
        protected AutoResetEvent refreshRequested;
        protected RefreshWorkerArgs refreshWorkerArgs;
        protected AutoResetEvent resyncRequested;

        protected bool CanSkipSlowUpdates
        {
            get
            {
#if DEBUG
                return EDDConfig.CanSkipSlowUpdates;
#else
                return false;
#endif
            }
        }
        #endregion

        #endregion

        #region Event Callers
        protected void InvokeOnNewLogEntry(string text, Color color)
        {
            OnNewLogEntry?.Invoke(text, color);
        }

        protected void InvokeOnNewEntry(HistoryEntry l, HistoryList hl)
        {
            OnNewEntry?.Invoke(l, hl);
        }

        protected void InvokeOnHistoryChange(HistoryList hl)
        {
            OnHistoryChange?.Invoke(hl);
        }

        protected void InvokeHistoryRefreshed()
        {
            HistoryRefreshed?.Invoke();
        }
        #endregion

        #region Initialization
        public EDDiscoveryFormBase()
        {

        }

        protected void Init()
        {
            SQLiteConnectionUser.EarlyReadRegister();
            EDDConfig.Instance.Update(write: false);

            backgroundWorker = new Thread(BackgroundWorkerThread);
            backgroundWorker.IsBackground = true;
            backgroundWorker.Name = "Background Worker Thread";
            backgroundWorker.Start();

            EDDConfig = EDDConfig.Instance;
            galacticMapping = new GalacticMapping();
            EdsmSync = new EDSMSync((EDDiscoveryForm)this);
            journalmonitor = new EDJournalClass();
            DisplayedCommander = EDDiscoveryForm.EDDConfig.CurrentCommander.Nr;
        }
        #endregion

        #region Background Worker Thread
        private void BackgroundWorkerThread()
        {
            InitializeDatabases();
            readyForInitialLoad.WaitOne();
        }

        protected void InvokeAsyncOnUIThread(Action a)
        {
            BeginInvoke(a);
        }

        protected void InvokeSyncOnUIThread(Action a)
        {
            Invoke(a);
        }

        private void InitializeDatabases()
        {
            Trace.WriteLine("Initializing database");
            SQLiteConnectionOld.Initialize();
            SQLiteConnectionUser.Initialize();
            SQLiteConnectionSystem.Initialize();
            Trace.WriteLine("Database initialization complete");
            InvokeAsyncOnUIThread(() => DatabaseInitializationComplete());
        }

        protected virtual void DatabaseInitializationComplete()
        {

        }
        #endregion
    }
}
