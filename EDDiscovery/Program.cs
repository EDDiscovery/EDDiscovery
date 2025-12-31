/*
 * Copyright © 2015 - 2026 EDDiscovery development team
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
using System.Threading; // Tasks and Mutex
using System.Windows.Forms;

namespace EDDiscovery
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            bool restartRequested = false;

            using (OpenTK.Toolkit.Init(new OpenTK.ToolkitOptions
            {
                EnableHighResolution = false,
                Backend = OpenTK.PlatformBackend.PreferNative
            }))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                try
                {
                    using (new BaseUtils.SingleUserInstance(1000))
                    {
                        using (var context = new EDDApplicationContext())
                        {
                            Application.Run(context);
                            restartRequested = context.RestartRequested;
                        }
                    }
                }
                catch (TimeoutException)
                {
                    LoadTranslator();

                    var result = MessageBox.Show(
                        "Another instance of EDDiscovery is already running.\n\nDo you want to start a second instance?".Tx(),
                        "EDDiscovery",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        using (var context = new EDDApplicationContext())
                        {
                            Application.Run(context);
                            restartRequested = context.RestartRequested;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine($"EDD fatal exception {ex}");
                    MessageBox.Show(
                        "A fatal error occurred and EDDiscovery needs to close.\n\nCheck logs for details.",
                        "EDDiscovery",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                finally
                {
                    ShutdownDatabases();

                    if (restartRequested)
                        Restart();
                }
            }
        }

        private static void LoadTranslator()
        {
            var tx = new BaseUtils.TranslatorMkII();
            tx.LoadTranslation(
                "Auto",
                CultureInfo.CurrentUICulture,
                new[] { AppContext.BaseDirectory },
                0,
                System.IO.Path.GetTempPath()
            );
        }

        private static void ShutdownDatabases()
        {
            try
            {
                EliteDangerousCore.DB.UserDatabase.Instance.Stop();
                EliteDangerousCore.DB.SystemsDatabase.Instance.Stop();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"DB shutdown error {ex}");
            }
        }

        private static void Restart()
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    Arguments = EDDApplicationContext.RestartOptions ?? "",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Restart failed {ex}");
            }
        }
    }
}
