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
using ExtendedControls;
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
        public EDDApplicationContext() : base(StartupForm)
        {
            if (typeof(SafeModeForm).IsAssignableFrom(MainForm?.GetType()))
            {
                ((SafeModeForm)MainForm).Run += ((p, theme, tabs, lang) => { GoForAutoSequenceStart(new EDDFormLaunchArgs(p, theme , tabs, lang)); });
            }
            else
            {
                GoForAutoSequenceStart();
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
        /// need to `<c>Application.Run(new EDDApplicationContext());</c>` and check back later, because it is
        /// still being initialized and is not yet in a valid state.
        /// </summary>
        public static EDDiscoveryForm EDDMainForm { get; private set; } = null;

        #endregion

        #region Implementation

        // Return whichever form should initially be displayed; normally SplashForm, but maybe SafeModeForm or even something else.
        private static Form StartupForm
        {   // Really just a workaround for the clumsy terniary operator if constructing new but different things.
            get
            {
                if (Control.ModifierKeys.HasFlag(Keys.Shift) || EDDOptions.Instance.SafeMode )
                    return new SafeModeForm();
                else
                    return new SplashForm();
            }
        }

        // Show SplashForm, if it's not already, then start a 250ms timer to ensure that we don't block the main loop during spool-up and ignition.
        private void GoForAutoSequenceStart(EDDFormLaunchArgs args = null)
        {
            if (MainForm == null || !(MainForm is SplashForm || MainForm.GetType().IsSubclassOf(typeof(SplashForm))))
            {
                SwitchContext(new SplashForm());
            }

            var tim = new Timer { Interval = 250 };
            tim.Tag = args;
            tim.Tick += MainEngineStart;
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
        private void SwitchContext(Form newContext)
        {
            Form f = MainForm;
            MainForm = newContext;
            f?.Close();
            MainForm?.Show();
        }


        // Initialize everything on the UI thread, and report+die for any problems or SwitchContext from SplashForm to EDDiscoveryForm.
        private void MainEngineStart(object sender, EventArgs e)
        {
            var tim = (Timer)sender;
            tim?.Stop();

            var launchArg = ((EDDFormLaunchArgs)tim?.Tag)?.Clone() ?? new EDDFormLaunchArgs();
            tim?.Dispose();

            try
            {
                EDDMainForm = new EDDiscoveryForm();
                SetLoadingMsg("Checking Ship Systems");

                EDDiscoveryController.Initialize(SetLoadingMsg);   // this loads up the options

                EDDOptions.Instance.NoWindowReposition |= launchArg.PositionReset;
                EDDOptions.Instance.NoTheme |= launchArg.ThemeReset;
                EDDOptions.Instance.TabsReset |= launchArg.TabsReset;
                EDDOptions.Instance.ResetLanguage |= launchArg.ResetLang;

                EDDMainForm.Init(SetLoadingMsg);    // call the init function, which will initialize the eddiscovery form

                SetLoadingMsg("Establishing Telepresence");
                SwitchContext(EDDMainForm);         // Ignition, and liftoff!
            }
            catch (Exception ex)
            {   // There's so many ways that things could go wrong during init; let's fail for everything!
                EDDMainForm?.Dispose();
                BaseUtils.ExceptionForm.ShowException(ex, "A fatal exception was encountered while initializing EDDiscovery.", Properties.Resources.URLProjectFeedback, isFatal: true, parent: MainForm);
            }
        }


        // Pass startup opts from GoForAutoSequenceStart to MainEngineStart without permanent heap impact.
        private class EDDFormLaunchArgs : EventArgs
        {   // Should probably be public and passed directly on to EDDiscoveryForm ctor() or Init() or something.

            public bool PositionReset { get; private set; }
            public bool ThemeReset { get; private set; }
            public bool TabsReset { get; private set; }
            public bool ResetLang { get; private set; }

            public EDDFormLaunchArgs() : this(false, false, false, false) { }

            public EDDFormLaunchArgs(bool positionReset, bool themeReset, bool tabsreset , bool resetlang)
            {
                PositionReset = positionReset;
                ThemeReset = themeReset;
                TabsReset = tabsreset;
                ResetLang = resetlang;
            }

            public EDDFormLaunchArgs Clone()
            {
                return new EDDFormLaunchArgs(PositionReset, ThemeReset, TabsReset, ResetLang);
            }
        }

        #endregion // Implementation
    }
}
