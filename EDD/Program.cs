/*
 * Copyright © 2015 - 2022 EDDiscovery development team
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
 */

using System;
using System.Globalization;
using System.Threading;                 // Tasks and Mutex
using System.Windows.Forms;

namespace EDDiscovery
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            using (OpenTK.Toolkit.Init(new OpenTK.ToolkitOptions { EnableHighResolution = false, Backend = OpenTK.PlatformBackend.PreferNative }))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                try
                {
                    using (new BaseUtils.SingleUserInstance(1000))
                    {
                        Application.Run(new EDDApplicationContext());
                    }
                }
                catch (TimeoutException te)
                {
                    System.Diagnostics.Trace.WriteLine($"EDD Program Timeout exception {te} {Environment.StackTrace}");
                    BaseUtils.TranslatorMkII tx = new BaseUtils.TranslatorMkII();
                    tx.LoadTranslation("Auto", CultureInfo.CurrentUICulture, new string[] { System.IO.Path.GetDirectoryName(Application.ExecutablePath) }, 0, System.IO.Path.GetTempPath());

                    if (System.Windows.Forms.MessageBox.Show("EDDiscovery is already running. Launch anyway?".Tx(), "EDDiscovery", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Application.Run(new EDDApplicationContext());
                    }
                }
                catch (ThreadAbortException tae)
                {
                    System.Diagnostics.Trace.WriteLine($"EDD Program Thread Abort exception {tae} {Environment.StackTrace}");
                    if (EDDApplicationContext.RestartOptions != null)
                    {
                        System.Diagnostics.Process.Start(Application.ExecutablePath, EDDApplicationContext.RestartOptions);
                    }
                }
                finally 
                {
                    EliteDangerousCore.DB.UserDatabase.Instance.Stop();     // need everything closed before we can shut down the DBs threads
                    EliteDangerousCore.DB.SystemsDatabase.Instance.Stop();

                    if (EDDApplicationContext.RestartOptions != null)
                    {
                        System.Diagnostics.Process.Start(Application.ExecutablePath, EDDApplicationContext.RestartOptions);
                    }
                }
            }
        }
    }
}
