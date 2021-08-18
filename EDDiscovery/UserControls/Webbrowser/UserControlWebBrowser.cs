/*
 * Copyright © 2016 - 2021 EDDiscovery development team
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


using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.JournalEvents;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlWebBrowser : UserControlCommonBase
    {
        private string source;
        private string urlallowed;
        private ISystem last_sys_tracked = null;        // this tracks the travel grid selection always
        private SystemClass override_system = null;     // if set, override to this system.. 

        #region Init
        public UserControlWebBrowser()
        {
            InitializeComponent();
            toolTip.ShowAlways = true;

            BaseUtils.BrowserInfo.FixIECompatibility(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe");
        }

        public void Init(string source,string urlallowed)
        {
            this.source = source;
            this.urlallowed = urlallowed;
            DBBaseName = source + "AutoView";

            rollUpPanelTop.PinState = GetSetting("PinState", true);
            rollUpPanelTop.SetToolTip(toolTip);

            string thisname = typeof(UserControlWebBrowser).Name; 
            BaseUtils.Translator.Instance.Translate(this, thisname, null);          // lookup using the base name, not the derived name, so we don't have repeats
            BaseUtils.Translator.Instance.Translate(this, toolTip, thisname);

            checkBoxAutoTrack.Checked = GetSetting("AutoTrack", true);
            this.checkBoxAutoTrack.CheckedChanged += new System.EventHandler(this.checkBoxAutoTrack_CheckedChanged);

            extCheckBoxStar.Visible = source != "Spansh";

            webBrowser.Visible = false; // hide ugly white until load
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void InitialDisplay()
        {
            last_sys_tracked = uctg.GetCurrentHistoryEntry?.System;
            PresentSystem(last_sys_tracked);    // may be null
        }

        bool isClosing = false;     // in case we are in a middle of a lookup

        public override void Closing()
        {
            isClosing = true;
            PutSetting("PinState", rollUpPanelTop.PinState);
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
        }

        #endregion

        #region Display


        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (he != null) // paranoia
            {
                // If not tracked last system, or name differs, its a new system..

                bool nosys = last_sys_tracked == null;

                if (nosys || last_sys_tracked.Name != he.System.Name) 
                {
                    last_sys_tracked = he.System;       // we want to track system always

                    if (override_system == null && (checkBoxAutoTrack.Checked||nosys))        // if no overridden, and tracking (or no sys), present
                        PresentSystem(last_sys_tracked);
                }
                else if (he.EntryType == JournalTypeEnum.StartJump)  // start jump prepresent system..
                {
                    if (override_system == null && checkBoxAutoTrack.Checked)       // if not overriding, and tracking, present
                    {
                        JournalStartJump jsj = he.journalEntry as JournalStartJump;
                        last_sys_tracked = new SystemClass(jsj.SystemAddress, jsj.StarSystem);
                        PresentSystem(last_sys_tracked);
                    }
                }
            }
        }

        private void PresentSystem(ISystem sys)
        {
            SetControlText("No Entry");
            if (sys != null)
            {
                new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LookUpThread)).Start(sys);
            }
        }

        private void LookUpThread(object s)
        {
            ISystem sys = s as ISystem;
            string url = "";
            string defaulturl = "";

            if (source == "EDSM")
            {
                EDSMClass edsm = new EDSMClass();
                url = edsm.GetUrlToSystem(sys.Name);
                defaulturl = EDSMClass.ServerAddress;
            }
            else if (source == "Spansh")
            {
                if (sys.SystemAddress.HasValue)
                    url = Properties.Resources.URLSpanshSystemSystemId + sys.SystemAddress.Value.ToStringInvariant();
                else
                    url = "https://spansh.co.uk";

                defaulturl = urlallowed;
            }
            else if (source == "EDDB")
            {
                url = Properties.Resources.URLEDDBSystemName + System.Web.HttpUtility.UrlEncode(sys.Name);
                defaulturl = urlallowed;
            }
            else if (source == "Inara")
            {
                url = Properties.Resources.URLInaraStarSystem + System.Web.HttpUtility.UrlEncode(sys.Name);
                defaulturl = urlallowed;
            }

            System.Diagnostics.Debug.WriteLine("Url is " + last_sys_tracked.Name + "=" + url);

            this.BeginInvoke((MethodInvoker)delegate
            {
               if (!isClosing)
               {
                   if (url.HasChars())
                   {
                       SetControlText("Data on " + sys.Name);
                       webBrowser.Navigate(url);
                   }
                   else
                   {
                       SetControlText("No Data on " + sys.Name);
                       webBrowser.Navigate(defaulturl);
                   }
               }
           });
        }

        #endregion

        #region User interaction

        private void checkBoxAutoTrack_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting("AutoTrack", checkBoxAutoTrack.Checked);
            if (checkBoxAutoTrack.Checked && override_system == null)       // if tracking now, and not overridden, present last track
                PresentSystem(last_sys_tracked);
        }

        private void extCheckBoxStar_Click(object sender, EventArgs e)
        {
            if (extCheckBoxStar.Checked == true)
            {
                ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
                int width = 500;
                f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "System:".T(EDTx.UserControlWebBrowser_System), new Point(10, 40), new Size(110, 24), null));
                f.Add(new ExtendedControls.ConfigurableForm.Entry("Sys", typeof(ExtendedControls.ExtTextBoxAutoComplete), "", new Point(120, 40), new Size(width - 120 - 20, 24), null));

                f.AddOK(new Point(width - 20 - 80, 80));
                f.AddCancel(new Point(width - 200, 80));

                f.Trigger += (dialogname, controlname, tag) =>
                {
                    if (controlname == "OK" || controlname == "Cancel" || controlname == "Close")
                    {
                        f.ReturnResult(controlname == "OK" ? DialogResult.OK : DialogResult.Cancel);
                    }
                    else if (controlname == "Sys:Return")
                    {
                        if (f.Get("Sys").HasChars())
                        {
                            f.ReturnResult(DialogResult.OK);
                        }

                        f.SwallowReturn = true;
                    }
                };

                f.InitCentred(this.FindForm(), this.FindForm().Icon, "Show System".T(EDTx.UserControlWebBrowser_EnterSys), null, null, closeicon: true);
                f.GetControl<ExtendedControls.ExtTextBoxAutoComplete>("Sys").SetAutoCompletor(SystemCache.ReturnSystemAutoCompleteList, true);
                DialogResult res = f.ShowDialog(this.FindForm());

                if (res == DialogResult.OK)
                {
                    string sname = f.Get("Sys");
                    if (sname.HasChars())
                    {
                        override_system = new EliteDangerousCore.SystemClass(sname);
                        PresentSystem(override_system);
                        extCheckBoxStar.Checked = true;
                    }
                    else
                        extCheckBoxStar.Checked = false;
                }
                else
                    extCheckBoxStar.Checked = false;
            }
            else
            {
                override_system = null;
                PresentSystem(last_sys_tracked);
                extCheckBoxStar.Checked = false;
            }
        }


        #endregion

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string[] allowed = new string[] {               
                "about:",
                "https://www.google.com/recaptcha",     // these three is enough to allow Spansh to work
                "https://consentcdn.cookiebot.com",
                "https://auth.frontierstore.net"
            };

            // if not starting with the current url for the site, or not in whitelist..

            if (!e.Url.Host.StartsWith(urlallowed, StringComparison.InvariantCultureIgnoreCase) && allowed.StartsWith(e.Url.AbsoluteUri, StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                System.Diagnostics.Debug.WriteLine("Webbrowser Disallowed " + e.Url.Host + " : " + e.Url.AbsoluteUri);
                e.Cancel = true;
            }
            else
            {
               System.Diagnostics.Debug.WriteLine("Webbrowser Allowed " + e.Url.Host + " : " + e.Url.AbsoluteUri);
            }
        }

        private void webBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void extCheckBoxClickBack(object sender, EventArgs e)
        {
            webBrowser.GoBack();
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser.Visible = true;
        }
    }


    public partial class UserControlEDSM : UserControlWebBrowser
    {
        public override void Init()
        {
            Init("EDSM", "www.edsm.net");
        }
    }

    public partial class UserControlSpansh : UserControlWebBrowser
    {
        public override void Init()
        {
            Init("Spansh", "spansh.co.uk");
        }
    }

    public partial class UserControlEDDB : UserControlWebBrowser
    {
        public override void Init()
        {
            Init("EDDB", "eddb.io");
        }
    }

    public partial class UserControlInara : UserControlWebBrowser
    {
        public override void Init()
        {
            Init("Inara", "inara.cz");
        }
    }

}

