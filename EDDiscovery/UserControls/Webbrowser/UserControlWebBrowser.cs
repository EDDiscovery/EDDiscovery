/*
 * Copyright © 2016 - 2022 EDDiscovery development team
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

using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.JournalEvents;
using System;
using System.Drawing;
using System.Windows.Forms;
using EDDiscovery.UserControls.Webbrowser;

namespace EDDiscovery.UserControls
{
    public partial class UserControlWebBrowser : UserControlCommonBase
    {
        private string source;
        private ISystem last_sys_tracked = null;        // this tracks the travel grid selection always
        private SystemClass override_system = null;     // if set, override to this system.. 

        private BrowserBase wbb;

        private Timer historychangedtimer = new System.Windows.Forms.Timer();
        private HistoryEntry historyhe;
        
        private string urlallowed;


        #region Init
        public UserControlWebBrowser()
        {
            InitializeComponent();
            toolTip.ShowAlways = true;

            BaseUtils.BrowserInfo.FixIECompatibility(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe");

        }

        public void Init(string source, string urlallowed)
        {
            this.source = source;
            this.urlallowed = urlallowed;

            DBBaseName = source + "AutoView";

            rollUpPanelTop.PinState = GetSetting("PinState", true);

            var enumlist = new Enum[] { EDTx.UserControlWebBrowser_extButtonIE11Warning };
            var enumlisttt = new Enum[] { EDTx.UserControlWebBrowser_extCheckBoxBack_ToolTip, EDTx.UserControlWebBrowser_extCheckBoxStar_ToolTip, EDTx.UserControlWebBrowser_checkBoxAutoTrack_ToolTip, EDTx.UserControlWebBrowser_extCheckBoxAllowedList_ToolTip };

            string thisname = typeof(UserControlWebBrowser).Name;
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, null, new string[] { thisname });          // lookup using the base name, not the derived name, so we don't have repeats
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this, new string[] { thisname });

            rollUpPanelTop.SetToolTip(toolTip);

            checkBoxAutoTrack.Checked = GetSetting("AutoTrack", true);
            this.checkBoxAutoTrack.CheckedChanged += new System.EventHandler(this.checkBoxAutoTrack_CheckedChanged);

            extCheckBoxStar.Visible = source != "Spansh";

            extButtonIE11Warning.Visible = false;

            historychangedtimer = new Timer();
            historychangedtimer.Interval = 100;
            historychangedtimer.Tick += HistoryChangedTimer_Tick;
        }

        public override void InitialDisplay()
        {
            System.Diagnostics.Trace.WriteLine($"Web browser initial display");
            var wv2 = new BrowserWebView2();
            wv2.LoadResult += (res) =>          // asynchronous to start up, UCTG, etc, winforms needs to run
            {
                System.Diagnostics.Trace.WriteLine($"Web browser view returned {res}");
                if (res == true)
                {
                    wbb = wv2;
                }
                else
                {
                    extButtonIE11Warning.Visible = true;
                    wbb = new BrowserIE11();
                }

                wbb.urlallowed = urlallowed;
                wbb.userurllist = GetSetting("Allowed", "");
                wbb.webbrowser.Dock = DockStyle.Fill;
                Controls.Add(wbb.webbrowser);
                Controls.SetChildIndex(wbb.webbrowser, 0);

                last_sys_tracked = null;             

                RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
            };
            wv2.Start();
        }

        bool isClosing = false;     // in case we are in a middle of a lookup

        public override void Closing()
        {
            historychangedtimer.Stop();
            isClosing = true;
            PutSetting("PinState", rollUpPanelTop.PinState);
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            System.Diagnostics.Debug.WriteLine($"Web browser HE change request {he.System.Name}");
            historyhe = he;
            historychangedtimer.Start();
        }

        private void HistoryChangedTimer_Tick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Web browser timer on {historyhe.System.Name} {wbb != null}");

            if (wbb != null)        // wait till we have a webbrowser
            {
                historychangedtimer.Stop();
                bool nosys = last_sys_tracked == null;

                if (nosys || last_sys_tracked.Name != historyhe.System.Name)
                {
                    last_sys_tracked = historyhe.System;       // we want to track system always

                    if (override_system == null && (checkBoxAutoTrack.Checked || nosys))        // if no overridden, and tracking (or no sys), present
                        PresentSystem(last_sys_tracked);
                }
                else if (historyhe.EntryType == JournalTypeEnum.StartJump)  // start jump prepresent system..
                {
                    if (override_system == null && checkBoxAutoTrack.Checked)       // if not overriding, and tracking, present
                    {
                        JournalStartJump jsj = historyhe.journalEntry as JournalStartJump;
                        last_sys_tracked = new SystemClass(jsj.StarSystem, jsj.SystemAddress);
                        PresentSystem(last_sys_tracked);
                    }
                }
            }
        }


        #endregion

        #region Display
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
                defaulturl = EDSMClass.RootURL;
            }
            else if (source == "Spansh")
            {
                if (sys.SystemAddress.HasValue)
                    url = EliteDangerousCore.Spansh.SpanshClass.URLForSystem(sys.SystemAddress.Value);
                else
                    url = EliteDangerousCore.Spansh.SpanshClass.RootURL;

                //defaulturl = urlallowed;
            }
            else if (source == "Inara")
            {
                //https://inara.cz/elite/starsystem/?search=Scirth
                var ir = new EliteDangerousCore.Inara.InaraClass();
                url = ir.URLForSystem(sys.Name);
            }

            System.Diagnostics.Debug.WriteLine("Url is " + last_sys_tracked.Name + "=" + url);

            this.BeginInvoke((MethodInvoker)delegate
            {
                if (!isClosing)
                {
                    if (url.HasChars())
                    {
                        SetControlText("Data on " + sys.Name);
                        wbb.Navigate(url);
                    }
                    else
                    {
                        SetControlText("No Data on " + sys.Name);
                        wbb.Navigate(defaulturl);
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
                f.Add(new ExtendedControls.ConfigurableEntryList.Entry("L", typeof(Label), "System:".T(EDTx.UserControlWebBrowser_System), new Point(10, 40), new Size(110, 24), null));
                f.Add(new ExtendedControls.ConfigurableEntryList.Entry("Sys", typeof(ExtendedControls.ExtTextBoxAutoComplete), "", new Point(120, 40), new Size(width - 120 - 20, 24), null));

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


        private void extCheckBoxClickBack(object sender, EventArgs e)
        {
            wbb.GoBack();
        }

        private void extCheckBoxAllowedList_Click(object sender, EventArgs e)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            int width = 430;

            string deflist = GetSetting("Allowed", "");

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("Text", typeof(ExtendedControls.ExtTextBox), deflist, new Point(10, 40), new Size(width - 10 - 20, 110), "URLs") { TextBoxMultiline = true });

            f.AddOK(new Point(width - 100, 180));
            f.AddCancel(new Point(width - 200, 180));

            f.Trigger += (dialogname, controlname, xtag) =>
            {
                if (controlname == "OK")
                    f.ReturnResult(DialogResult.OK);
                else if (controlname == "Close" || controlname == "Escape" || controlname == "Cancel")
                    f.ReturnResult(DialogResult.Cancel);
            };

            DialogResult res = f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "URLs", closeicon: true);
            if (res == DialogResult.OK)
            {
                string userurllist = f.Get("Text");
                wbb.userurllist = userurllist;
                PutSetting("Allowed", userurllist);
                PresentSystem(last_sys_tracked);
            }
        }

        private void extButtonIE11Warning_Click(object sender, EventArgs e)
        {
            BaseUtils.BrowserInfo.LaunchBrowser(Properties.Resources.URLWebView2);

        }
    }

}

