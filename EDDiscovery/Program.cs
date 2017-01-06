using System;
using System.Collections.Generic;
using System.Diagnostics;               //Process
using System.Linq;
using System.Reflection;                //Assembly
using System.Runtime.InteropServices;   //GuidAttribute, Win32 DllImports: SetForegoundWindow, ShowWindowAsync, IsIconic
using System.Security.AccessControl;    //MutexAccessRule
using System.Security.Principal;        //SecurityIdentifier
using System.Threading;                 // Tasks and Mutex
using System.Windows.Forms;


namespace EDDiscovery
{
    static class Program
    {
        #region Win32 API needed to focus another application's window.
        private const int SW_RESTORE = 9;

        /// <summary>
        /// Brings the thread that created the specified window into the foreground and activates the window.
        /// </summary>
        /// Keyboard input is directed to the window, and various visual cues are changed for
        /// the user. The system assigns a slightly higher priority to the thread that created the
        /// foreground window than it does to other threads.
        /// <param name="hWnd">A handle to the window.</param>
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// WIN32: Sets the show state of a window without waiting for the operation to complete.
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="nCmdShow">Controls how the window is to be shown. See ShowWindow on MSDN for reference.</param>
        /// <returns>If the operation was successfully started, the return value is true.</returns>
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// WIN32: Determines whether the specified window is minimized (iconic).
        /// </summary>
        /// <param name="hWnd">A handle to the window to be tested.</param>
        /// <returns>true if the window is minimized, false otherwise</returns>
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                using (new SingleGlobalInstance(1000))
                {
                    Application.Run(new EDDiscoveryForm());
                }
            }
            catch (TimeoutException)
            {
                /* Could not lock the app-global mutex, which means another copy of the App is running.
                 * Let's try and show it. If we can't, then fallback to showing the classic MessageBox.
                 */
                if (!RaiseOtherProcess() && MessageBox.Show("EDDiscovery is already running. Launch anyway?", "EDDiscovery", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.Run(new EDDiscoveryForm());
                }
            }
        }

        /// <summary>
        /// Attempt to bring the main window of a different process to the foreground.
        /// This is really only useful if we are being launched twice because the user
        /// was not aware that the application was already running.
        /// </summary>
        /// <returns><c>true</c> if the other application windw was raised. <c>false</c> otherwise</returns>
        static bool RaiseOtherProcess()
        {
            bool retval = false;
            Process proc = Process.GetCurrentProcess();
            foreach (Process otherProc in Process.GetProcessesByName(proc.ProcessName))
            {
                if (proc.Id != otherProc.Id)
                {
                    IntPtr hWnd = otherProc.MainWindowHandle;
                    if (IsIconic(hWnd))
                    {
                        ShowWindowAsync(hWnd, SW_RESTORE);
                    }
                    SetForegroundWindow(hWnd);
                    retval = true;
                }
            }
            return retval;
        }
    }


    /** This is a helper class to wrap an app-unique global mutex. It can be used to ensure that
     * only a single instance of a piece of code runs on a machine.  If this is used to wrap main()
     * it ensures that only a single instance of the entire application can run.
     * Code copied from http://stackoverflow.com/questions/229565/what-is-a-good-pattern-for-using-a-global-mutex-in-c/229567
     */
    class SingleGlobalInstance : IDisposable
    {
        public bool hasHandle = false;
        Mutex mutex;

        private void InitMutex()
        {
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
            string mutexId = string.Format("Global\\{{{0}}}", appGuid);
            mutex = new Mutex(false, mutexId);

            try
            {
                var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
                var securitySettings = new MutexSecurity();
                securitySettings.AddAccessRule(allowEveryoneRule);
                mutex.SetAccessControl(securitySettings);
            }
            catch (PlatformNotSupportedException)
            {
                System.Diagnostics.Trace.WriteLine("Unable to set mutex security");
            }
        }

        public SingleGlobalInstance(int timeOut)
        {
            InitMutex();
            try
            {
                if (timeOut < 0)
                    hasHandle = mutex.WaitOne(Timeout.Infinite, false);
                else
                    hasHandle = mutex.WaitOne(timeOut, false);

                if (hasHandle == false)
                    throw new TimeoutException("Timeout waiting for exclusive access on SingleInstance");
            }
            catch (AbandonedMutexException)
            {
                hasHandle = true;
            }
        }


        public void Dispose()
        {
            if (mutex != null)
            {
                if (hasHandle)
                    mutex.ReleaseMutex();
                mutex.Dispose();
            }
        }
    }
}
