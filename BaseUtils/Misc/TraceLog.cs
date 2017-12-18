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
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Net;

namespace BaseUtils
{
    public class TraceLog
    {
        public static event Action<Exception> LogFileWriterException;
        public static long MaxLogDirSizeMB { get; set; } = 1024;
        public static string LogFileName { get; private set; }
        public static string LogFileBaseName { get; private set; }
        private static Thread LogFileWriterThread;
        private static BlockingCollection<string> LogLineQueue = new BlockingCollection<string>();
        private static AutoResetEvent LogLineQueueEvent = new AutoResetEvent(false);

        private class TraceLogWriter : TextWriter
        {
            private ThreadLocal<StringBuilder> logline = new ThreadLocal<StringBuilder>(() => new StringBuilder());

            public override Encoding Encoding { get { return Encoding.UTF8; } }
            public override IFormatProvider FormatProvider { get { return CultureInfo.InvariantCulture; } }

            public override void Write(string value)
            {
                if (value != null)
                {
                    logline.Value.Append(value);
                    string logval = logline.ToString();
                    while (logval.Contains("\n"))
                    {
                        string[] lines = logval.Split(new[] { '\n' }, 2);
                        TraceLog.WriteLine(lines[0]);
                        logline.Value.Clear();
                        logline.Value.Append(lines.Length == 2 ? lines[1] : "");
                        logval = logline.ToString();
                    }
                }
            }

            public override void Write(char value) { Write(new string(new[] { value })); }
            public override void WriteLine(string value) { Write((value ?? "") + "\n"); }
            public override void WriteLine() { Write("\n"); }
        }

        static public string logroot = "c:\\";
        static public string urlfeedback = "Unknown";

        public static void Init()
        {
            string logname = Path.Combine(logroot, "Log", $"Trace_{DateTime.Now.ToString("yyyyMMddHHmmss")}");
            LogFileBaseName = logname;
            LogFileWriterThread = new Thread(LogWriterThreadProc);
            LogFileWriterThread.IsBackground = true;
            LogFileWriterThread.Name = "Log Writer";
            LogFileWriterThread.Start();
            System.Diagnostics.Trace.AutoFlush = true;
            // Log trace events to the above file
            System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(new TraceLogWriter()));
            // Log unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            // Log unhandled UI exceptions
            Application.ThreadException += Application_ThreadException;
            // Redirect console to trace
            Console.SetOut(new TraceLogWriter());
        }

        public static void WriteLine(string msg)
        {
            LogLineQueue.Add(msg);
        }

        private static void LogWriterThreadProc()
        {
            int partnum = 0;
            Dictionary<string, int> msgrepeats = new Dictionary<string, int>();
            while (true)
            {
                try
                {
                    DeleteOldLogFiles();
                    LogFileName = $"{LogFileBaseName}.{partnum}.log";
                    using (TextWriter writer = new StreamWriter(LogFileName))
                    {
                        int linenum = 0;
                        while (true)
                        {
                            string msg = null;
                            if (msgrepeats.Count < 100 && !msgrepeats.Any(m => m.Value >= 10000) && LogLineQueue.TryTake(out msg, msgrepeats.Count > 1 ? 1000 : Timeout.Infinite))
                            {
                                if (msg == null)
                                {
                                    LogLineQueueEvent.Set();
                                    return;
                                }
                                else if (msgrepeats.ContainsKey(msg))
                                {
                                    msgrepeats[msg]++;
                                }
                                else
                                {
                                    writer.WriteLine($"[{DateTime.UtcNow.ToString("u")}] {msg}");
                                    writer.Flush();
                                    msgrepeats[msg] = 0;
                                    linenum++;
                                    if (linenum >= 100000)
                                    {
                                        partnum++;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                foreach (KeyValuePair<string, int> rptkvp in msgrepeats)
                                {
                                    if (rptkvp.Value >= 1)
                                    {
                                        writer.WriteLine($"[{DateTime.UtcNow.ToString("u")}] {rptkvp.Key}");
                                        if (rptkvp.Value > 1)
                                        {
                                            writer.WriteLine($"[{DateTime.UtcNow.ToString("u")}] Last message repeated {(rptkvp.Value)} times");
                                        }
                                    }
                                }

                                msgrepeats = new Dictionary<string, int>();
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
            try
            {
                long totsize = 0;
                // Create a reference to the Log directory.
                DirectoryInfo dir = new DirectoryInfo(Path.Combine(logroot, "Log"));

                Trace.WriteLine("Running logfile age check");
                // Create an array representing the files in the current directory.
                FileInfo[] files = dir.GetFiles("*.log").OrderByDescending(f => f.LastWriteTimeUtc).ToArray();

                foreach (FileInfo fi in files)
                {
                    DateTime time = fi.CreationTime;

                    TimeSpan maxage = new TimeSpan(30, 0, 0, 0);
                    TimeSpan fileage = DateTime.Now - time;
                    totsize += fi.Length;

                    if (fileage > maxage)
                    {
                        WriteLine(String.Format("File {0} is older then maximum age. Removing file from Logs.", fi.Name));
                        fi.Delete();
                    }
                    else if (totsize >= MaxLogDirSizeMB * 1048576)
                    {
                        WriteLine($"File {fi.Name} pushes total log directory size over limit of {MaxLogDirSizeMB}MB");
                        fi.Delete();
                    }
                }
            }
            catch
            {
            }
        }

        // We can't prevent an unhandled exception from killing the application.
        // See https://blog.codinghorror.com/improved-unhandled-exception-behavior-in-net-20/
        // Log the exception info if we can, and ask the user to report it.
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        [System.Security.SecurityCritical]
        [System.Runtime.ConstrainedExecution.ReliabilityContract(
            System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState,
            System.Runtime.ConstrainedExecution.Cer.Success)]
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                WriteLine($"\n==== UNHANDLED EXCEPTION ====\n{e.ExceptionObject.ToString()}\n==== cut ====");
                WriteLine(null);
                LogLineQueueEvent.WaitOne(100);
                ExceptionForm.ShowException(e.ExceptionObject as Exception, "An unhandled fatal exception has occurred.", urlfeedback, isFatal: true);
            }
            catch
            {
            }

            Environment.Exit(1);
        }

        // Handling a ThreadException leaves the application in an undefined state.
        // See https://msdn.microsoft.com/en-us/library/system.windows.forms.application.threadexception(v=vs.100).aspx
        // Log the exception, ask the user to report it, and exit.
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                WriteLine($"\n==== UNHANDLED UI EXCEPTION ====\n{e.Exception.ToString()}\n==== cut ====");
                ExceptionForm.ShowException(e.Exception, "There was an unhandled UI exception.", urlfeedback);
            }
            catch
            {
            }
        }

        // Mono does not implement AppDomain.CurrentDomain.FirstChanceException
        public static void RegisterFirstChanceExceptionHandler()
        {
            try
            {
                Type adtype = AppDomain.CurrentDomain.GetType();
                EventInfo fcexevent = adtype.GetEvent("FirstChanceException");
                if (fcexevent != null)
                {
                    fcexevent.AddEventHandler(AppDomain.CurrentDomain, new EventHandler<System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs>(CurrentDomain_FirstChanceException));
                }
            }
            catch
            {
            }
        }

        // Log exceptions were they occur so we can try to diagnose some
        // hard to debug issues.
        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            // Ignore HTTP NotModified exceptions
            if (e.Exception is System.Net.WebException)
            {
                var webex = (WebException)e.Exception;
                if (webex.Response != null && webex.Response is HttpWebResponse)
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
            else if (Thread.CurrentThread == LogFileWriterThread)
            {
                return;
            }

            var trace = new StackTrace(1, true);

            // Ignore first-chance exceptions in threads outside our code
            bool ourcode = false;
            foreach (var frame in trace.GetFrames())
            {
                var a = frame.GetMethod().DeclaringType.Assembly;
                if (a == Assembly.GetEntryAssembly() || a == Assembly.GetExecutingAssembly())
                {
                    ourcode = true;
                    break;
                }
            }

            if (ourcode)
                WriteLine($"First chance exception: {e.Exception.Message}\n{trace.ToString()}");
        }
    }
}
