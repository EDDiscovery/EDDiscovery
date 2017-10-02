/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseUtils
{
    /// <summary>
    /// A class to manage writing trace logs and exceptions to disk. Initiate logging via
    /// <see cref="Init(string, string, bool, IEnumerable{Assembly})"/>, and use the various
    /// <see cref="Trace.WriteLine(string)"/>, etc, method overrides for writing diagnostic data.
    /// </summary>
    public static class TraceLog
    {
        #region Public interfaces

        #region Properties and events

        /// <summary>
        /// Occurs when the background-thread file writer encounters an error. This generally means that this author
        /// made a mistake in the <see cref="TraceLog"/> code and should be bugged in order to fix it.
        /// </summary>
        public static event Action<Exception> LogFileWriterException;

        /// <summary>
        /// Whether or not the <see cref="TraceLog"/> is registerd to receive
        /// <see cref="AppDomain.FirstChanceException"/> events.
        /// </summary>
        public static bool IsFirstChanceExceptionLoggingEnabled { get; private set; } = false;

        /// <summary>
        /// Whether or not the <see cref="TraceLog"/> is registered to <see cref="Trace.Listeners"/>.
        /// </summary>
        public static bool IsTraceListenerLoggingEnabled { get; private set; } = false;

        /// <summary>
        /// The maximum allowed age for a log file. The default is 30 days. Logs older than this will be deleted during
        /// startup.
        /// </summary>
        public static TimeSpan MaxLogAge { get; set; } = new TimeSpan(30, 0, 0, 0);

        /// <summary>
        /// The disk quota available for the logging folder. Default is 100 MiB. This size may be exceeding during use,
        /// but the oldest logs will be deleted at startup to fit within this limit.
        /// </summary>
        public static long MaxLogDirSizeMiB { get; set; } = 100;

        /// <summary>
        /// The full path to the current log file, or an empty string if <see cref="Init"/> has not yet been invoked.
        /// </summary>
        public static string LogFileName { get; private set; } = string.Empty;

        #endregion

        /// <summary>
        /// Initialize the <see cref="TraceLog"/> class.
        /// </summary>
        /// <param name="logPath">The full path to write log files to. If <c>null</c> or <c>string.Empty</c>, a temp folder will be used.</param>
        /// <param name="urlFeedback">An URL for reporting feedback, such as an https://github.com/team/proj/issues link. Will be displayed alongside exceptions.</param>
        /// <param name="attachToTraceListener">Whether to subscribe to <see cref="Trace.Listeners"/>. Default is <c>true</c>, but set this <c>false</c> when running in a debugger.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="logPath"/> is not a valid directory path.</exception>
        /// <exception cref="InvalidOperationException">Thrown when called more than once during execution.</exception>
        public static void Init(string logPath = null, string urlFeedback = "(Unknown)", bool attachToTraceListener = true)
        {
            if (_HasBeenInitialized)
                throw new InvalidOperationException($"{nameof(TraceLog)} has previously been initialized. Please don't call {nameof(Init)} more than once!");
            else
                _HasBeenInitialized = true;

            if (!string.IsNullOrEmpty(logPath))
            {
                _LogFolderPath = logPath;
            }
            else
            {
                _LogFileRootName = "EDDiscovery_Trace_";
                _LogFolderPath = Path.GetTempPath();
            }

            if (string.IsNullOrEmpty(_LogFolderPath) || !Directory.Exists(_LogFolderPath))
                throw new ArgumentException("The provided argument is not a valid path.", nameof(logPath));

            _FeedbackURL = urlFeedback;
            _LogFolderPath = _LogFolderPath.TrimEnd(Path.DirectorySeparatorChar);
            _LogFileBasePath = Path.Combine(_LogFolderPath, $"{_LogFileRootName}{DateTime.Now.ToString("yyyyMMddHHmmss")}");

            _LogFileWriterThread = new Thread(LogWriterThreadProc)
            {
                IsBackground = true,
                Name = $"{nameof(TraceLog)}.{nameof(LogWriterThreadProc)}",
            };
            _LogFileWriterThread.Start();
            Application.ApplicationExit += (s, e) => EndLogWriterThread();

            HookTraceListener(attachToTraceListener);
        }

        /// <summary>
        /// Attach first chance exception logging to the provided assemblies, or disable by providing <c>null</c>. Note
        /// that first chance exceptions are not supported on Mac OS, and perhaps elsewhere.
        /// </summary>
        /// <param name="assemblies">If <c>null</c>, first chance exception logging will be disabled. Otherwise, the
        /// provided assemblies will be monitored for first chance exceptions.</param>
        public static void EnableFirstChanceExceptionLogging(params Assembly[] assemblies)
        {
            bool attach = assemblies != null;

            if (attach)
                _FirstChanceAssemblies = _FirstChanceAssemblies.Concat(assemblies).Distinct().ToList();
            else
                _FirstChanceAssemblies.Clear();

            if (IsFirstChanceExceptionLoggingEnabled == attach)
                return;

            // Mono does not implement AppDomain.CurrentDomain.FirstChanceException. Do this carefully.
            try
            {
                EventInfo fcexevent = AppDomain.CurrentDomain.GetType().GetEvent(nameof(AppDomain.FirstChanceException));
                if (fcexevent != null)
                {
                    if (attach)
                        fcexevent.AddEventHandler(AppDomain.CurrentDomain, _fcExDelegate);
                    else
                        fcexevent.RemoveEventHandler(AppDomain.CurrentDomain, _fcExDelegate);

                    IsFirstChanceExceptionLoggingEnabled = attach;
                }
            }
            catch { }
        }

        #endregion


        #region Implementation

        // misc
        private static string nl { get; } = Environment.NewLine;        // That's way too much text to type out every time it is needed.
        private static bool _HasBeenInitialized = false;                // Whether or not Init(...) has been called.

        // First chance exceptions handling
        private static List<Assembly> _FirstChanceAssemblies = new List<Assembly>();    // Assemblies that will be included for first chance exception handling.
        private static EventHandler<FirstChanceExceptionEventArgs> _fcExDelegate = CurrentDomain_FirstChanceException; // Safety net for Mono lacking first chance exceptions.

        // the following fields are all assigned during Init().
        private static string _FeedbackURL = null;                      // An URL for the user to report bugs, such as https://github.com/EDDiscovery/EDDiscovery/issues
        private static string _LogFolderPath = null;                    // The full path to the logging folder, such as C:\Users\Bob\AppData\Local\EDDiscovery\Log
        private static string _LogFileBasePath = null;                  // The full path to a base log file, such as C:\Users\Bob\AppData\Local\EDDiscovery\Log\Trace_20161225062542
        private static string _LogFileRootName = "Trace_";              // The root name of log files without path, date, nor file extension.
        private static TraceLogWriter _LogWriter = null;                // The TextWriter that transfers Trace/Console messages to _LogLineQueue. Owned by _traceListener.
        private static TextWriterTraceListener _traceListener = null;   // Ferry trace messages from Trace to _LogWriter.

        // Thread management and cross-thread communications.
        private static Thread _LogFileWriterThread = null;              // The thread that actually writes log files via LogWriterThreadProc.
        private static BlockingCollection<string> _LogLineQueue = new BlockingCollection<string>(); // The messages waiting to be written to disk.
        private static AutoResetEvent _LogWriterThreadExitEvent = new AutoResetEvent(false);        // Allows EndLogWriterThread() to wait for _LogFileWriterThread to gracefully close.

        // Perform cleanup.
        private static void EndLogWriterThread(int millisecondsTimeout = 1000)
        {
            EnableFirstChanceExceptionLogging(null);
            HookTraceListener(false);

            if (_LogFileWriterThread?.IsAlive == true)
            {
                _LogLineQueue.Add(null);
                _LogWriterThreadExitEvent.WaitOne(millisecondsTimeout);
            }
            _LogFileWriterThread = null;
        }

        // Attach or detach the Trace/Console listener log writer, as well as the unhandled exception handlers.
        private static void HookTraceListener(bool attach)
        {
            if (IsTraceListenerLoggingEnabled == attach)
                return;

            // Duplicate subscriptions are naughty.
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
            Application.ThreadException -= Application_ThreadException;

            if (attach)
            {
                _LogWriter = new TraceLogWriter();
                _traceListener = new TextWriterTraceListener(_LogWriter, nameof(_traceListener));

                Trace.Listeners.Add(_traceListener);
                Console.SetOut(_LogWriter);

                // Log unhandled exceptions
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                // Log unhandled UI exceptions
                Application.ThreadException += Application_ThreadException;

                Trace.AutoFlush = true;
            }
            else
            {
                Console.SetOut(TextWriter.Null);
                Trace.Listeners.Remove(_traceListener);

                _traceListener.Dispose();   // also disposes of _LogWriter.
                _traceListener = null;
                _LogWriter = null;
            }
            IsTraceListenerLoggingEnabled = attach;
        }

        #region Exception handlers

        // Handling a ThreadException leaves the application in an undefined state.
        // See https://msdn.microsoft.com/en-us/library/system.windows.forms.application.threadexception(v=vs.100).aspx
        // Log the exception, ask the user to report it, and exit.
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            DialogResult res = DialogResult.Abort;

            try
            {
                Trace.WriteLine($"{nl}==== UNHANDLED UI EXCEPTION ===={nl}{e.Exception.ToString()}{nl}==== cut ===={nl}");
                Trace.Flush();
                res = MessageBox.Show($"There was an unhandled UI exception.{nl}Please report this at {_FeedbackURL} and attach {LogFileName}{nl}Exception: {e.Exception.Message}{nl}{e.Exception.StackTrace}{nl}{nl}Do you wish to abort, or ignore the exception and try to continue?", "Unhandled Exception", MessageBoxButtons.AbortRetryIgnore);
            }
            catch { }

            if (res == DialogResult.Abort)
            {
                Environment.Exit(1);
            }
        }

        // Log exceptions where they occur so we can try to diagnose some hard-to-debug issues. Activated via "-logexceptions" command-line.
        private static void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            if (_FirstChanceAssemblies?.Count < 1)
                return;

            // Ignore HTTP NotModified exceptions
            if (e.Exception is WebException)
            {
                var webex = (WebException)e.Exception;
                if (webex?.Response is HttpWebResponse)
                {
                    var resp = (HttpWebResponse)webex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotModified)
                    {
                        return;
                    }
                }
            }
            // Ignore DLL Not Found exceptions from OpenTK
            else if (e.Exception is DllNotFoundException && e.Exception.Source == "OpenTK")
            {
                return;
            }
            else if (Thread.CurrentThread == _LogFileWriterThread)
            {
                return;
            }

            var trace = new StackTrace(e.Exception, 1, true);

            // Ignore any first-chance exceptions that don't contain a frame from our _firstChanceAssemblies list
            if (trace.GetFrames().Select(f => f.GetMethod().DeclaringType.Assembly).Distinct().Any(a => _FirstChanceAssemblies.Contains(a)))
            {
                string msg = $"{nl}==== FIRST CHANCE EXCEPTION ===={nl}{e.Exception}{nl}==== cut ====";

                if (IsTraceListenerLoggingEnabled || Debugger.IsAttached)
                    Trace.WriteLine(msg);
                else
                    _LogLineQueue?.Add(msg);
            }
        }

        // We can't prevent an unhandled exception from killing the application.
        // See https://blog.codinghorror.com/improved-unhandled-exception-behavior-in-net-20/
        // Log the exception info if we can, and ask the user to report it.
        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Trace.WriteLine($"{nl}==== UNHANDLED EXCEPTION ===={nl}{e.ExceptionObject.ToString()}{nl}==== cut ====");
                Trace.Flush();
                MessageBox.Show($"There was an unhandled exception.{nl}Please report this at {_FeedbackURL} and attach {LogFileName}{nl}Exception: {e.ExceptionObject.ToString()}{nl}{nl}This application must now close", "Unhandled Exception");
            }
            catch { }

            Environment.Exit(1);
        }

        #endregion

        #region Background log writer thread

        [STAThread]
        private static void LogWriterThreadProc()
        {
            const int maxlines = 100000;    // The maximum number of lines allowed in a log file.
            int partnum = 0;                // If the log file exceeds maxlines, this number will increment.
            int rptcount = 0;               // The number of times the last message has been repeated.
            string lastmsg = null;          // The last message that was written.

            while (true)
            {
                try
                {
                    DeleteOldLogFiles();
                    LogFileName = $"{_LogFileBasePath}.{partnum}.log";
                    using (var writer = new StreamWriter(LogFileName, true, Encoding.UTF8))
                    {
                        int linenum = 0;
                        while (true)
                        {
                            string msg = null;
                            if (_LogLineQueue.TryTake(out msg, Timeout.Infinite))
                            {
                                if (msg == null)
                                {
                                    if (!writer.AutoFlush)
                                        writer.Flush();
                                    writer.Close();
                                    _LogWriterThreadExitEvent.Set();
                                    return;
                                }
                                else if (msg.Equals(lastmsg))
                                {
                                    rptcount++;
                                }
                                else
                                {
                                    if (rptcount > 0)
                                    {
                                        writer.WriteLine($"[{DateTime.UtcNow.ToString("u")}] Last message repeated {rptcount:N0} {(rptcount == 1 ? "time" : "times")}.");
                                        linenum++;
                                        rptcount = 0;
                                    }
                                    writer.WriteLine($"[{DateTime.UtcNow.ToString("u")}] {msg}");
                                    lastmsg = msg;
                                    linenum++;

                                    if (!writer.AutoFlush)
                                        writer.Flush();

                                    if (linenum >= maxlines)
                                    {
                                        partnum++;
                                        writer.Close();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFileWriterException?.Invoke(ex);
                    Thread.Sleep(30000);
                }
            }
        }

        private static void DeleteOldLogFiles()
        {
            long totsize = 0;                               // Running total of the size of all log files.
            long maxsize = MaxLogDirSizeMiB * 1024 * 1024;  // Maximum size of log files in bytes.
            int keepCnt = 0;                                // The number of log files remaining after trimming.
            List<FileInfo> filesToDelete = new List<FileInfo>();

            Trace.WriteLine("Running logfile age and size checks...");

            try
            {
                // Create a reference to the Log directory.
                DirectoryInfo dir = new DirectoryInfo(_LogFolderPath);

                // Iterate the files in the Log directory, oldest first. May be %TEMP%, so be sure to filter using _LogFileRootName so we're not deleted _everything_.
                foreach (var fi in dir.GetFiles($"{_LogFileRootName}*.log").OrderByDescending(f => f.LastWriteTimeUtc))
                {
                    var fileage = DateTime.Now - fi.CreationTime;
                    var fsize = fi.Length;

                    if (fileage > MaxLogAge)
                    {
                        Trace.WriteLine($"File {fi.Name}, created at {fi.CreationTimeUtc.ToString("u")}, is older than maximum age. Marking file for deletion.");
                        filesToDelete.Add(fi);
                    }
                    else if (totsize >= maxsize - fsize)
                    {
                        Trace.WriteLine($"File {fi.Name} brings total log directory size over limit of {MaxLogDirSizeMiB:N0} MiB. Marking file for deletion.");
                        filesToDelete.Add(fi);
                    }
                    else
                    {
                        keepCnt++;
                        totsize += fsize;
                    }
                }
            }
            catch (Exception ex)
            {   // This should only be seen if the user lacks read permissions to the log folder.
                Trace.WriteLine($"{nameof(TraceLog)}.{nameof(DeleteOldLogFiles)}: Caught exception while iterating log files:{nl}{ex.ToString()}");
            }

            if (filesToDelete.Count > 0)
            {
                if (keepCnt < 1)
                    filesToDelete.RemoveAt(filesToDelete.Count - 1);    // always keep at least one, even if it is ancient or huge.

                try
                {
                    foreach (var fi in filesToDelete)
                        fi.Delete();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"{nameof(TraceLog)}.{nameof(DeleteOldLogFiles)}: Caught exception while deleting expired/oversized log file:{nl}{ex.ToString()}");
                }
            }

            Trace.WriteLine($"Total log file size after checking: {totsize / (float)(1024 * 1024):N} MiB.");
        }

        #endregion

        #region private class TraceLogWriter : TextWriter

        // Take messages provided to Trace/Console and pass them to _LogLineQueue for writing to disk.
        private class TraceLogWriter : TextWriter
        {
            private ThreadLocal<StringBuilder> logline = new ThreadLocal<StringBuilder>(() => new StringBuilder());

            public override Encoding Encoding { get { return Encoding.UTF8; } }
            public override IFormatProvider FormatProvider { get { return CultureInfo.InvariantCulture; } }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    logline?.Dispose();
                }
                logline = null;
                base.Dispose(disposing);
            }

            public override void Write(string value)
            {
                if (logline?.Value != null)
                {
                    if (!string.IsNullOrEmpty(value))
                        logline.Value.Append(value);

                    string logval = logline.Value.ToString();
                    while (logval.Contains(NewLine))
                    {
                        string[] lines = logval.Split(new[] { NewLine }, 2, StringSplitOptions.None);
                        _LogLineQueue.Add(lines[0]);
                        logline.Value.Clear();
                        logline.Value.Append(lines.Length >= 2 ? lines[1] : string.Empty);
                        logval = logline.Value.ToString();
                    }
                }
            }

            public override void Write(char value) { Write(new string(new[] { value })); }
            public override void WriteLine(string value) { Write((value ?? string.Empty) + NewLine); }
            public override void WriteLine() { Write(NewLine); }
        }

        #endregion

        #endregion
    }
}
