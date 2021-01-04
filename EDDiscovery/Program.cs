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
using System.Globalization;
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
            using (OpenTK.Toolkit.Init(new OpenTK.ToolkitOptions { EnableHighResolution = false, Backend = OpenTK.PlatformBackend.PreferNative }))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                try
                {
                    using (new SingleUserInstance(1000))
                    {
                        Application.Run(new EDDApplicationContext());
                    }
                }
                catch (TimeoutException)
                {
                    BaseUtils.Translator tx = new BaseUtils.Translator();
                    tx.LoadTranslation("Auto", CultureInfo.CurrentUICulture, new string[] { System.IO.Path.GetDirectoryName(Application.ExecutablePath) }, 0, System.IO.Path.GetTempPath());

                    if (System.Windows.Forms.MessageBox.Show(tx.Translate("EDDiscovery is already running. Launch anyway?","StartUp.DUPLOAD"), "EDDiscovery", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Application.Run(new EDDApplicationContext());
                    }

                    /* Could not lock the app-global mutex, which means another copy of the App is running.
                     * TODO: show a dialog and/or bring the current instance's window to the foreground.
                     */
                }
                catch (ThreadAbortException)
                {
                    if (EDDApplicationContext.RestartInSafeMode)
                    {
                        System.Diagnostics.Process.Start(Application.ExecutablePath, "-safemode");
                    }
                }
                finally
                {
                    EliteDangerousCore.DB.UserDatabase.Instance.Stop();     // need everything closed before we can shut down the DBs threads
                    EliteDangerousCore.DB.SystemsDatabase.Instance.Stop();

                    if (EDDApplicationContext.RestartInSafeMode)
                    {
                        System.Diagnostics.Process.Start(Application.ExecutablePath, "-safemode");
                    }
                }
            }
        }
    }


    /** This is a helper class to wrap an app-unique per-user mutex. It can be used to ensure that
     * only a single instance of a piece of code runs in a user session.  If this is used to wrap main()
     * it ensures that only a single instance of the entire application can run per-user.
     * Code copied from http://stackoverflow.com/questions/229565/what-is-a-good-pattern-for-using-a-global-mutex-in-c/229567
     */
    class SingleUserInstance : IDisposable
    {
        public bool hasHandle = false;
        Mutex mutex;

        private void InitMutex()
        {
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
            string usernm = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(Environment.UserDomainName ?? "none" + "-" + Environment.UserName ?? "none"));
            string mutexId = $"Global\\{usernm}-{{{appGuid}}}";
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

        public SingleUserInstance(int timeOut)
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
