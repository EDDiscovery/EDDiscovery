using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;                //Assembly
using System.Runtime.InteropServices;   //GuidAttribute
using System.Security.AccessControl;    //MutexAccessRule
using System.Security.Principal;        //SecurityIdentifier
using System.Threading;                 // Tasks and Mutex
using System.Windows.Forms;


namespace EDDiscovery
{
    static class Program
    {
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
                if (MessageBox.Show("EDDiscovery is already running. Launch anyway?", "EDDiscovery", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.Run(new EDDiscoveryForm());
                }

                /* Could not lock the app-global mutex, which means another copy of the App is running.
                 * TODO: show a dialog and/or bring the current instance's window to the foreground.
                 */
            }
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

            var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            mutex.SetAccessControl(securitySettings);
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
