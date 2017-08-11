﻿/*
 * Copyright © 2017 EDDiscovery development team
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
using EDDiscovery.Forms;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;                //Assembly
using System.Runtime.InteropServices;   //GuidAttribute
using System.Security.AccessControl;    //MutexAccessRule
using System.Security.Principal;        //SecurityIdentifier
using System.Threading;                 //Tasks and Mutex
using System.Threading.Tasks;
using System.Windows.Forms;

using Timer = System.Windows.Forms.Timer;

namespace EDDiscovery
{
    // Class for managing application initialization, and proud owner of SplashScreen and EDDiscoveryForm. Singleton.
    internal class EDDApplicationContext : ApplicationContext
    {
        public EDDApplicationContext() : base(new SplashForm())
        {
            if (Instance == null)
            {
                Instance = this;
                Timer timer = new Timer { Interval = 250, Enabled = true };
                timer.Tick += initTimer_Tick;
            }
        }

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


        /// <summary>
        /// The main <see cref="EDDiscoveryForm"/> of this application. If this is <c>null</c>, then you probably
        /// need to `<c>Application.Run(EDDApplicationContext.Instance);</c>` and check back later, because it is
        /// still being initialized and is not yet in a valid state.
        /// </summary>
        public static EDDiscoveryForm EDDMainForm { get; private set; } = null;

        /// <summary>
        /// The <see cref="EDDApplicationContext"/> of this application process.
        /// </summary>
        public static EDDApplicationContext Instance { get; private set; } = null;

        #endregion

        #region Implementation

        // methods

        // Display a loading message on the SplashForm, if it is visible.
        private void SetLoadingMsg(string msg)
        {
            if (MainForm != null && MainForm is SplashForm)
            {
                ((SplashForm)MainForm).SetLoadingText(msg ?? string.Empty);
            }
        }


        // event handlers

        // Initialize everything on the UI thread soon after the `Application` has been `.Run()`, and transfer context (MainForm) from the SplashForm to the EDDiscoveryForm.
        private void initTimer_Tick(object sender, EventArgs e)
        {
            ((Timer)sender)?.Stop();
            ((Timer)sender)?.Dispose();

            try
            {
                EDDMainForm = new EDDiscoveryForm();
                SetLoadingMsg("Checking Ship Systems");
                EDDiscoveryController.Initialize(Control.ModifierKeys.HasFlag(Keys.Shift), Control.ModifierKeys.HasFlag(Keys.Control), SetLoadingMsg);
                EDDMainForm.Init(SetLoadingMsg);     // call the init function, which will initialize the eddiscovery form

                SetLoadingMsg("Establishing Telepresence");
                EDDMainForm.Show();
            }
            catch (Exception ex)
            {   // There's so many ways that things could go wrong during init; let's fail for everything!
                EDDMainForm?.Dispose();
                string msg = ex.Message ?? "(Unknown exception)";
                string st = ex.StackTrace ?? "(Stack trace unavailable)";

                MessageBox.Show(MainForm, $"Error initializing EDDiscovery. Please report this at {Properties.Resources.URLProjectFeedback}:\n\n{msg}\n{st}", "Error Initializing EDDiscovery");
                ExitThread();
                return;
            }

            var splashForm = MainForm;
            MainForm = EDDMainForm; // Switch context
            splashForm.Close();     // and cleanup
        }

        #endregion // Implementation
    }
}
