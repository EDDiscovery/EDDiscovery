/*
* Copyright © 2017-2020 EDDiscovery development team
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

using BaseUtils.Win32;
using EDDiscovery.Forms;
using EliteDangerousCore.DB;
using System;
using System.IO;
using System.Reflection;                //Assembly
using System.Windows.Forms;

using Timer = System.Windows.Forms.Timer;

namespace EDDiscovery
{
    // Class for managing application initialization, and proud owner of SplashScreen and EDDiscoveryForm. Singleton.
    internal class EDDApplicationContext : ApplicationContext
    {
        #region Public static properties

        /// <summary>
        /// The concise version number of the EDDiscovery.exe assembly, in the form of "<c>3.14.15.926</c>".
        /// </summary>
        public static string AppVersion { get; } = Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];

        /// <summary>
        /// The friendly name of this assembly, i.e. "<c>EDDiscovery</c>".
        /// </summary>
        public static string FriendlyName { get; } = "EDDiscovery";

        /// <summary>
        /// The friendly name of this assembly combined with the version number, in the form of "<c>EDDiscovery v3.14.15.926</c>".
        /// </summary>
        public static string UserAgent { get; } = $"{FriendlyName} v{AppVersion}";

        public static string RestartOptions { get; set; } = null;

        #endregion

        #region Implementation

        // create application context with form chosen by StartupForm
        public EDDApplicationContext() : base(StartupForm())
        {
            if ( MainForm is SafeModeForm)
            {
                // give safemode the means to run EDD
                ((SafeModeForm)MainForm).Run += ((p, theme, tabs, lang) => { RunEDD(new LaunchArgs(p, theme , tabs, lang)); });
            }
            else
            {
                RunEDD();
            }
        }

        // Return whichever form should initially be displayed; normally SplashForm, but maybe SafeModeForm
        private static Form StartupForm()
        {   // Really just a workaround for the clumsy terniary operator if constructing new but different things.
            bool insafemode = EDDOptions.Instance.SafeMode;     // force reading of options, pick up safe mode option

            // check some basic things can be reached before we start

            string dberror = "Check status of the drive/share" + Environment.NewLine +
                            "Check options.txt and dboptions.txt for correctness in " + EDDOptions.Instance.AppDataDirectory + Environment.NewLine +
                            "Or use safemode reset DB to remove dboptions.txt " + Environment.NewLine +
                            "and go back to using the standard c: location";
            string apperror = "Check status of the drive/share" + Environment.NewLine +
                                "Also check options.txt is correct in your " + EDDOptions.ExeDirectory() + " folder";
            string sysdbdir = Path.GetDirectoryName(EDDOptions.Instance.SystemDatabasePath);
            string userdbdir = Path.GetDirectoryName(EDDOptions.Instance.UserDatabasePath);

            if (!Directory.Exists(EDDOptions.Instance.AppDataDirectory))
            {
                System.Windows.Forms.MessageBox.Show("Error: App Data Directory is inaccessible at " + EDDOptions.Instance.AppDataDirectory + Environment.NewLine + Environment.NewLine + apperror,
                                                        "Application Folder inaccessible", System.Windows.Forms.MessageBoxButtons.OK);
                Environment.Exit(1);
            }
            else if (!BaseUtils.FileHelpers.VerifyWriteToDirectory(EDDOptions.Instance.AppDataDirectory))
            {
                System.Windows.Forms.MessageBox.Show("Error: App Data Directory is not writable at " + EDDOptions.Instance.AppDataDirectory + Environment.NewLine + Environment.NewLine + apperror,
                                                        "Application Folder not writable", System.Windows.Forms.MessageBoxButtons.OK);
                Environment.Exit(1);
            }
            else if (!Directory.Exists(sysdbdir))
            {
                System.Windows.Forms.MessageBox.Show("Error: Systems database is inaccessible at " + EDDOptions.Instance.SystemDatabasePath + Environment.NewLine + Environment.NewLine + dberror,
                                                    "Systems DB inaccessible", System.Windows.Forms.MessageBoxButtons.OK);
                insafemode = true;
            }
            else if (!BaseUtils.FileHelpers.VerifyWriteToDirectory(sysdbdir))
            {
                System.Windows.Forms.MessageBox.Show("Error: Systems database folder is not writable at " + sysdbdir + Environment.NewLine + Environment.NewLine + dberror,
                                                    "Systems DB not writeable", System.Windows.Forms.MessageBoxButtons.OK);
                insafemode = true;
            }
            else if (!Directory.Exists(userdbdir))
            {
                System.Windows.Forms.MessageBox.Show("Error: User database is inaccessible at " + EDDOptions.Instance.UserDatabasePath + Environment.NewLine + Environment.NewLine + dberror,
                                                        "User DB inaccessible", System.Windows.Forms.MessageBoxButtons.OK);
                insafemode = true;
            }
            else if (!BaseUtils.FileHelpers.VerifyWriteToDirectory(userdbdir))
            {
                System.Windows.Forms.MessageBox.Show("Error: User database folder is not writable at " + sysdbdir + Environment.NewLine + Environment.NewLine + dberror,
                                                    "User DB not writeable", System.Windows.Forms.MessageBoxButtons.OK);
                insafemode = true;
            }

            if (Control.ModifierKeys.HasFlag(Keys.Shift) || insafemode )
                return new SafeModeForm();
            else
                return new SplashForm();
        }

        // Run EDD!
        // Show SplashForm, if it's not already, then start a 250ms timer to ensure that we don't block the main loop during spool-up and ignition.
        private void RunEDD(LaunchArgs args = null)
        {
            if (MainForm == null || !(MainForm is SplashForm || MainForm.GetType().IsSubclassOf(typeof(SplashForm))))
            {
                SwitchContext(new SplashForm());
            }

            var tim = new Timer { Interval = 250 };
            tim.Tag = args;
            tim.Tick += InitialiseEDD;
            tim.Start();
        }

        // Display a loading message on the SplashForm.
        private void SetLoadingMsg(string msg)
        {
            if (typeof(SplashForm).IsAssignableFrom(MainForm?.GetType()))
            {
                ((SplashForm)MainForm).SetLoadingText(msg ?? string.Empty);
            }
        }

        // Switch context from an existing Form to a different Form, Close() the previous Form, and Show() the new one.
        private void SwitchContext(Form newForm)
        {
            Form f = MainForm;
            MainForm = newForm;
            f?.Close();
            MainForm?.Show();
        }

        // Initialize everything on the UI thread, and report+die for any problems or SwitchContext from SplashForm to EDDiscoveryForm.
        private void InitialiseEDD(object sender, EventArgs e)
        {
            var tim = (Timer)sender;
            tim?.Stop();

            var launchArg = ((LaunchArgs)tim?.Tag)?.Clone() ?? new LaunchArgs();
            tim?.Dispose();

            EDDiscoveryForm EDDMainForm = null;

            try
            {
                System.Threading.Thread.CurrentThread.Name = "EDD Main Thread";

                EDDMainForm = new EDDiscoveryForm();

                SetLoadingMsg("Initialising Databases");

                string msg = UserDatabase.Instance.CheckConnection();
                if (msg.HasChars())
                {
                    System.Windows.Forms.MessageBox.Show("Error: User DB is corrupt at " + EliteDangerousCore.EliteConfigInstance.InstanceOptions.UserDatabasePath + Environment.NewLine + Environment.NewLine +
                                                         "Database is unusable. Use safe mode to remove it and start again. All user settings will be lost" + Environment.NewLine + Environment.NewLine +
                                                         msg,
                                                         "User DB corrupt", System.Windows.Forms.MessageBoxButtons.OK);
                    SwitchContext(new SafeModeForm(false));
                    return;
                }

                msg = SystemsDatabase.Instance.CheckConnection();
                if (msg.HasChars())
                {
                    System.Windows.Forms.MessageBox.Show("Error: System DB is corrupt at " + EliteDangerousCore.EliteConfigInstance.InstanceOptions.SystemDatabasePath + Environment.NewLine + Environment.NewLine +
                                                         "Database is unusable. Use safe mode to remove it and start again. User settings will be retained" + Environment.NewLine + Environment.NewLine +
                                                         msg,
                                                         "System DB corrupt", System.Windows.Forms.MessageBoxButtons.OK);
                    SwitchContext(new SafeModeForm(false));
                    return;
                }

                UserDatabase.Instance.Name = "UserDB";
                UserDatabase.Instance.MinThreads = 1;
                UserDatabase.Instance.MaxThreads = 2;
                UserDatabase.Instance.MultiThreaded = true;     // starts up the threads

                if (EDDOptions.Instance.DeleteSystemDB)
                    BaseUtils.FileHelpers.DeleteFileNoError(EliteDangerousCore.EliteConfigInstance.InstanceOptions.SystemDatabasePath);
                if (EDDOptions.Instance.DeleteUserDB)
                {
                    if (MessageBox.Show("Confirm deletion of user DB") == DialogResult.OK)
                    {
                        BaseUtils.FileHelpers.DeleteFileNoError(EliteDangerousCore.EliteConfigInstance.InstanceOptions.UserDatabasePath);
                    }
                }

                try
                {
                    UserDatabase.Instance.Initialize();
                }
                catch ( Exception ex)
                {
                    EliteDangerousCore.DB.UserDatabase.Instance.ClearDown();     // need everything closed down

                    System.Windows.Forms.MessageBox.Show("Error: User DB is corrupt at " + EliteDangerousCore.EliteConfigInstance.InstanceOptions.UserDatabasePath + Environment.NewLine + Environment.NewLine +
                                                         "Database is unusable. Use safe mode to remove it and start again. All user settings will be lost" + Environment.NewLine + Environment.NewLine +
                                                         ex.Message.ToString(),
                                                         "User DB corrupt", System.Windows.Forms.MessageBoxButtons.OK);
                    UserDatabase.Instance.Stop();
                    SwitchContext(new SafeModeForm(false));
                    return;
                }

                if (EDDOptions.Instance.DeleteUserJournals)
                {
                    if (MessageBox.Show($"Confirm deletion of user journals in {EliteDangerousCore.EliteConfigInstance.InstanceOptions.UserDatabasePath}", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        UserDatabase.Instance.ClearJournals();
                    }
                }

                SystemsDatabase.WALMode = true;
                SystemsDatabase.Instance.Name = "SystemDB";
                SystemsDatabase.Instance.MinThreads = 1;
                SystemsDatabase.Instance.MaxThreads = 8;
                SystemsDatabase.Instance.MultiThreaded = true;  // starts up the threads

                try
                {
                    SystemsDatabase.Instance.Initialize();
                }
                catch (Exception ex)
                {
                    EliteDangerousCore.DB.UserDatabase.Instance.ClearDown();     // need everything closed down
                    EliteDangerousCore.DB.SystemsDatabase.Instance.ClearDown();

                    System.Windows.Forms.MessageBox.Show("Error: System DB is corrupt at " + EliteDangerousCore.EliteConfigInstance.InstanceOptions.SystemDatabasePath + Environment.NewLine + Environment.NewLine +
                                                         "Database is unusable. Use safe mode to remove it and start again. User settings will be retained" + Environment.NewLine + Environment.NewLine +
                                                         ex.Message.ToString(),
                                                         "System DB corrupt", System.Windows.Forms.MessageBoxButtons.OK);
                    SystemsDatabase.Instance.Stop();
                    UserDatabase.Instance.Stop();
                    SwitchContext(new SafeModeForm(false));
                    return;
                }

                if ( !SystemsDatabase.Instance.VerifyTablesExist() )
                {
                    System.Windows.Forms.MessageBox.Show("Error: System DB is corruptted, or you are using an older version of the program with a DB " +
                                                         "produced by a newer version, at " + EliteDangerousCore.EliteConfigInstance.InstanceOptions.SystemDatabasePath + Environment.NewLine + Environment.NewLine +
                                                         "Database is unusable. Use safe mode to remove it and start again. User settings will be retained",
                                                         "System DB corrupt", System.Windows.Forms.MessageBoxButtons.OK);
                    UserDatabase.Instance.Stop();
                    SystemsDatabase.Instance.Stop();
                    SwitchContext(new SafeModeForm(false));
                    return;
                }

                EDDOptions.Instance.NoWindowReposition |= launchArg.PositionReset;
                EDDOptions.Instance.NoTheme |= launchArg.ThemeReset;
                EDDOptions.Instance.TabsReset |= launchArg.TabsReset;
                EDDOptions.Instance.ResetLanguage |= launchArg.ResetLang;

                SetLoadingMsg("Starting EDD");

                EDDMainForm.Init(SetLoadingMsg);    // call the init function, which will initialize the eddiscovery system

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    NativeMethods.STARTUPINFO_I si = new NativeMethods.STARTUPINFO_I();
                    UnsafeNativeMethods.GetStartupInfo(si);        // duplicate of form.cs WmCreate check of code.

                    if ((si.dwFlags & NativeMethods.STARTF_USESHOWWINDOW) != 0)
                    {
                        if (si.wShowWindow == NativeMethods.SW_MINIMIZE || si.wShowWindow == NativeMethods.SW_SHOWMINNOACTIVE)
                        {
                            EDDOptions.Instance.MinimiseOnOpen = true;
                        }
                        else if (si.wShowWindow == NativeMethods.SW_SHOWMAXIMIZED || si.wShowWindow == NativeMethods.SW_MAXIMIZE)
                        {
                            EDDOptions.Instance.MaximiseOnOpen = true;
                        }
                    }
                }


                SetLoadingMsg("Starting Program");
                SwitchContext(EDDMainForm);         // Ignition, and liftoff!
            }
            catch (Exception ex)
            {   // There's so many ways that things could go wrong during init; let's fail for everything!
                BaseUtils.ExceptionForm.ShowException(ex, "A fatal exception was encountered while initializing EDDiscovery.", Properties.Resources.URLProjectFeedback, isFatal: true, parent: MainForm);
                EDDMainForm?.Dispose();
            }
        }


        // Pass startup opts from GoForAutoSequenceStart to MainEngineStart without permanent heap impact.
        private class LaunchArgs : EventArgs
        {   

            public bool PositionReset { get; private set; }
            public bool ThemeReset { get; private set; }
            public bool TabsReset { get; private set; }
            public bool ResetLang { get; private set; }

            public LaunchArgs() : this(false, false, false, false) { }

            public LaunchArgs(bool positionReset, bool themeReset, bool tabsreset , bool resetlang)
            {
                PositionReset = positionReset;
                ThemeReset = themeReset;
                TabsReset = tabsreset;
                ResetLang = resetlang;
            }

            public LaunchArgs Clone()
            {
                return new LaunchArgs(PositionReset, ThemeReset, TabsReset, ResetLang);
            }
        }

        #endregion // Implementation
    }
}
