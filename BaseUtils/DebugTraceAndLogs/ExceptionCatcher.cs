using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseUtils
{
    public static class ExceptionCatcher
    {
        static private string urlfeedback = "Unknown";

        public static void RedirectExceptions(string url)
        {
            urlfeedback = url;

            // Log unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            // Log unhandled UI exceptions
            Application.ThreadException += Application_ThreadException;
            // Redirect console to trace
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
                TraceLog.WriteLine($"\n==== UNHANDLED EXCEPTION ====\n{e.ExceptionObject.ToString()}\n==== cut ====");
                TraceLog.WriteLine(null);
                TraceLog.WaitForOutput();
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
                TraceLog.WriteLine($"\n==== UNHANDLED UI EXCEPTION ====\n{e.Exception.ToString()}\n==== cut ====");
                ExceptionForm.ShowException(e.Exception, "There was an unhandled UI exception.", urlfeedback);
            }
            catch
            {
            }
        }
    }


    public static class FirstChanceExceptionCatcher
    {
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
            else if (Thread.CurrentThread == TraceLog.LogFileWriterThread)     // prevents circular exceptions since we use tracelog
            {
                return;
            }

            var trace = new StackTrace(1, true);

            // Ignore first-chance exceptions in threads outside our code
            bool ourcode = false;
            foreach (var frame in trace.GetFrames())
            {
                var a = frame.GetMethod().DeclaringType.Assembly;
                if (a == Assembly.GetEntryAssembly() || a == Assembly.GetExecutingAssembly())      // look down the stack and note if its one of ours
                {
                    ourcode = true;
                    break;
                }
            }

            if (ourcode)
                TraceLog.WriteLine($"First chance exception: {e.Exception.Message}\n{trace.ToString()}");
        }

    }
}
