/*
 * Copyright © 2016 - 2020 EDDiscovery development team
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
        private string DbSave;
        private string source;
        private string urlallowed;
        ISystem last_sys = null;

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
            DbSave = DBName(source + "AutoView");

            rollUpPanelTop.PinState = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "PinState", true);
            rollUpPanelTop.SetToolTip(toolTip);

            string thisname = typeof(UserControlWebBrowser).Name; 
            BaseUtils.Translator.Instance.Translate(this, thisname, null);          // lookup using the base name, not the derived name, so we don't have repeats
            BaseUtils.Translator.Instance.Translate(this, toolTip, thisname);

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
            last_sys = uctg.GetCurrentHistoryEntry?.System;
            PresentSystem(last_sys);    // may be null
        }

        bool isClosing = false;     // in case we are in a middle of a lookup

        public override void Closing()
        {
            isClosing = true;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "PinState", rollUpPanelTop.PinState);
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
        }

        #endregion

        #region Display

        SystemClass override_system = null;

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (he != null)
            {
                if (last_sys == null || last_sys.Name != he.System.Name) // if new entry is scan, may be new data.. or not presenting or diff sys
                {
                    last_sys = he.System;       // even if overridden, we want to track system

                    if (override_system == null)
                        PresentSystem(last_sys);
                }
                else if (override_system == null && he.EntryType == JournalTypeEnum.StartJump)  // we ignore start jump if overriden      
                {
                    JournalStartJump jsj = he.journalEntry as JournalStartJump;
                    last_sys = new SystemClass(jsj.StarSystem);
                    PresentSystem(last_sys);
                }
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
                url = edsm.GetUrlToEDSMSystem(sys.Name, sys.EDSMID);
                defaulturl = EDSMClass.ServerAddress;
            }
            else if (source == "Spansh")
            {
                if (sys.SystemAddress.HasValue)
                    url = Properties.Resources.URLSpanshSystemSystemId + sys.SystemAddress.Value.ToStringInvariant();
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

            System.Diagnostics.Debug.WriteLine("Url is " + last_sys.Name + "=" + url);

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

        void PresentSystem(ISystem sys)
        {
            SetControlText("No Entry");
            if (sys != null)
            {
                new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LookUpThread)).Start(sys);
            }
        }

        #endregion

        #region User interaction

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
                PresentSystem(last_sys);
                extCheckBoxStar.Checked = false;
            }
        }


        #endregion

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (!e.Url.AbsoluteUri.StartsWith("about:"))
            {
                if (!e.Url.Host.StartsWith(urlallowed))
                {
                //    System.Diagnostics.Debug.WriteLine("Webbrowser Disallowed " + e.Url.Host + " : " + e.Url.AbsoluteUri);
                    e.Cancel = true;
                }
            }
            else
            {
               // System.Diagnostics.Debug.WriteLine("Webbrowser Allowed " + e.Url.Host + " : " + e.Url.AbsoluteUri);
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

