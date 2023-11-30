﻿/*
 * Copyright © 2015 - 2021 EDDiscovery development team
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
 * 
 */

using QuickJSON;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class EDDiscoveryForm
    {
        #region Updators

        public void NewTargetSet(Object sender)
        {
            OnNewTarget?.Invoke(sender);
        }

        public void NoteChanged(Object sender, HistoryEntry snc)
        {
            OnNoteChanged?.Invoke(sender, snc);
        }

        #endregion

        #region webserver

        public bool WebServerControl(bool start, int portno)        // false if an error occurs
        {
            if (WebServer.Running)
                WebServer.Stop();

            if (start)
            {
                WebServer.Port = portno;

                string servefrom;
                bool valid;

                if (EDDOptions.Instance.WebServerFolder != null)
                {
                    servefrom = EDDOptions.Instance.WebServerFolder;
                    valid = Directory.Exists(servefrom);
                }
                else
                {
                    servefrom = Path.Combine(EDDOptions.ExeDirectory(), "eddwebsite.zip");
                    valid = File.Exists(servefrom);
                }

                if (valid)
                {
                    System.Diagnostics.Debug.WriteLine("Serve from " + servefrom + " on port " + EDDConfig.Instance.WebServerPort);

                    if (WebServer.Start(servefrom))   // may fail due to some security reasons
                        Controller.LogLine("Web server enabled".T(EDTx.EDDiscoveryForm_WSE));
                    else
                    {
                        Controller.LogLineHighlight("Web server failed to start".T(EDTx.EDDiscoveryForm_WSF));
                        return false;
                    }
                }
                else
                    Controller.LogLineHighlight("Web server disabled due to incorrect folder or missing zip file".T(EDTx.EDDiscoveryForm_WSERR));
            }
            else
            {
                Controller.LogLine("*** Web server is disabled ***".T(EDTx.EDDiscoveryForm_WSD));
            }

            return true;
        }

        #endregion

        #region EDSM

        public void EDSMSend()
        {
            var helist = EDSMJournalSync.GetListToSend(History.EntryOrder());               // find out what to send..

            if (helist.Count >= 500)
            {
                ExtendedControls.ConfigurableForm cf = new ExtendedControls.ConfigurableForm();

                int width = 400;

                DateTime lasthe = helist.Last().EventTimeUTC;

                cf.Add(new ExtendedControls.ConfigurableForm.Entry("UC", typeof(Label),
                            string.Format("There are {0} EDSM reports to send, this will take time and bandwidth, choose from the following what to do. Entries before this will be marked as sent.".T(EDTx.EDDiscoveryForm_SendEDSMCaption), helist.Count),
                             new Point(5, 30), new Size(width - 5 - 20, 100), null)
                { TextBoxMultiline = true });

                cf.Add(new ExtendedControls.ConfigurableForm.Entry("All", typeof(ExtendedControls.ExtButton),
                            "Send All to EDSM".T(EDTx.EDDiscoveryForm_SendEDSMAll),
                             new Point(5, 130), new Size(width - 5 - 20, 24), null));

                cf.Add(new ExtendedControls.ConfigurableForm.Entry("Today", typeof(ExtendedControls.ExtButton),
                            "Send Last 24 Hours of entries to EDSM".T(EDTx.EDDiscoveryForm_SendEDSM24),
                             new Point(5, 180), new Size(width - 5 - 20, 24), null));

                cf.Add(new ExtendedControls.ConfigurableForm.Entry("Custom", typeof(ExtendedControls.ExtButton),
                            "Send From".T(EDTx.EDDiscoveryForm_SendEDSMFrom),
                             new Point(5, 230), new Size(80, 24), null));

                cf.Add(new ExtendedControls.ConfigurableForm.Entry("Date", typeof(ExtendedControls.ExtDateTimePicker),
                                            lasthe.AddDays(-28).ToStringZulu(),
                                             new Point(100, 230), new Size(width - 100 - 20, 24), null));

                cf.Add(new ExtendedControls.ConfigurableForm.Entry("None", typeof(ExtendedControls.ExtButton),
                            "EDSM is up to date - send Nothing more".T(EDTx.EDDiscoveryForm_SendEDSMNone),
                             new Point(5, 280), new Size(width - 5 - 20, 24), null));

                cf.Add(new ExtendedControls.ConfigurableForm.Entry("Cancel", typeof(ExtendedControls.ExtButton),
                            "I'll decide later, do nothing".T(EDTx.EDDiscoveryForm_SendEDSMCancel),
                             new Point(5, 330), new Size(width - 5 - 20, 24), null));

                DateTime dateutc = DateTime.UtcNow;

                cf.Trigger += (dialogname, controlname, tag) =>
                {
                    if (controlname.Contains("All"))
                    {
                        dateutc = EDDConfig.GameLaunchTimeUTC();
                        cf.ReturnResult(DialogResult.OK);
                    }
                    else if (controlname.Contains("Today"))
                    {
                        dateutc = lasthe.AddDays(-1);
                        cf.ReturnResult(DialogResult.OK);
                    }
                    else if (controlname.Contains("Custom"))
                    {
                        dateutc = cf.GetDateTime("Date").Value.ToUniversalTime();
                        cf.ReturnResult(DialogResult.OK);
                    }
                    else if (controlname.Contains("None"))
                    {
                        cf.ReturnResult(DialogResult.OK);
                    }
                    else if (controlname.Contains("Cancel"))
                    {
                        cf.ReturnResult(DialogResult.Cancel);
                    }
                };

                if (cf.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "Sending a large number of EDSM Entries".T(EDTx.EDDiscoveryForm_SendEDSMTitle)) == DialogResult.Cancel)
                    return;

                var jes = helist.Where(x => x.EventTimeUTC < dateutc).Select(x => x.journalEntry).ToList();
                JournalEntry.SetEdsmSyncList(jes);

                helist = EDSMJournalSync.GetListToSend(History.EntryOrder());               // find out what to send..
            }

            // we send the list using the last gameversion/build as we may have early entries without these..
            if (helist.Count > 0)
            {
                var gb = History.GetLastGameversionBuild();

                EDSMJournalSync.SendEDSMEvents(l => LogLine(l), helist, gb.Item1, gb.Item2);

            }
        }


        #endregion

        #region 3dmap
        public UserControls.UserControl3DMap Open3DMap()         // Current map - open at last position or configured position
        {
            UserControls.UserControlCommonBase uccb = null;

            var tabfind3dmap = tabControlMain.Find(PanelInformation.PanelIDs.Map3D);

            if ( tabfind3dmap != null )
            {
                tabControlMain.SelectTab(tabfind3dmap.Item1);       // goto tab
                uccb = tabfind3dmap.Item2;
            }
            else
            { 
                var findpopoutform = PopOuts.Find(PanelInformation.PanelIDs.Map3D);
                if (findpopoutform != null)
                    uccb = findpopoutform.UserControl;
            }

            if (uccb == null )      // none found, make one, on major tab
            {
                var tp = tabControlMain.EnsureMajorTabIsPresent(PanelInformation.PanelIDs.Map3D, true);
                uccb = (UserControls.UserControlCommonBase)tp.Controls[0];
            }

            return (UserControls.UserControl3DMap)uccb;
        }

        public void Open3DMap(ISystem system, List<ISystem> route = null)     // current-map open/goto this system. system may be null. Optionally set a route
        {
            var map3d = Open3DMap();
            if ( route != null )
                map3d.SetRoute(route);
            map3d.GotoSystem(system);
        }

        #endregion

        #region Add Ons

        public bool AddNewMenuItemToAddOns(string menu, string menutext, string icon, string menuname, string packname)
        {
            ToolStripMenuItem parent;

            menu = menu.ToLowerInvariant();
            if (menu.Equals("add-ons"))
                parent = addOnsToolStripMenuItem;
            else if (menu.Equals("help"))
                parent = helpToolStripMenuItem;
            else if (menu.Equals("tools"))
                parent = toolsToolStripMenuItem;
            else if (menu.Equals("admin"))
                parent = adminToolStripMenuItem;
            else
                return false;

            Image img = BaseUtils.Icons.IconSet.GetIcon(icon);

            var x = (from ToolStripItem p in parent.DropDownItems where p.Text.Equals(menutext) && p.Tag != null && p.Name.Equals(menuname) select p);

            if (x.Count() == 0)           // double entries screened out of same menu text from same pack
            {
                ToolStripMenuItem it = new ToolStripMenuItem();
                it.Text = menutext;
                it.Name = menuname;
                it.Tag = packname;
                if (img != null)
                    it.Image = img;
                it.Size = new Size(313, 22);
                it.Click += installedAddOnMenuItem_Click;
                parent.DropDownItems.Add(it);
            }

            return true;
        }

        public bool IsMenuItemInstalled(string menuname)
        {
            foreach (ToolStripMenuItem tsi in mainMenu.Items)
            {
                List<ToolStripItem> presentlist = (from ToolStripItem s in tsi.DropDownItems where s.Name.Equals(menuname) select s).ToList();
                if (presentlist.Count() > 0)
                    return true;
            }

            return false;
        }

        public void RemoveMenuItemsFromAddOns(ToolStripMenuItem menu, string packname)
        {
            List<ToolStripItem> removelist = (from ToolStripItem s in menu.DropDownItems where s.Tag != null && ((string)s.Tag).Equals(packname) select s).ToList();
            foreach (ToolStripItem it in removelist)
            {
                menu.DropDownItems.Remove(it);
                it.Dispose();
            }
        }

        public void RemoveMenuItemsFromAddOns(string packname)
        {
            RemoveMenuItemsFromAddOns(addOnsToolStripMenuItem, packname);
            RemoveMenuItemsFromAddOns(helpToolStripMenuItem, packname);
            RemoveMenuItemsFromAddOns(toolsToolStripMenuItem, packname);
            RemoveMenuItemsFromAddOns(adminToolStripMenuItem, packname);
        }

        private void installedAddOnMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem it = sender as ToolStripMenuItem;
            BaseUtils.Variables vars = new BaseUtils.Variables(new string[]
            {   "MenuName", it.Name,
                "MenuText", it.Text,
                "TopLevelMenuName" , it.OwnerItem.Name,
            });

            actioncontroller.ActionRun(Actions.ActionEventEDList.onMenuItem, null,vars);
        }


        #endregion

        #region Actions

        public int ActionRunOnEntry(HistoryEntry he, ActionLanguage.ActionEvent av)
        { return actioncontroller.ActionRunOnEntry(he, av); }

        public int ActionRun(ActionLanguage.ActionEvent ev, BaseUtils.Variables additionalvars = null, string flagstart = null, bool now = false)
        { return actioncontroller.ActionRun(ev, null, additionalvars, flagstart, now); }

        #endregion

        #region Notifications

        void ShowNotification(List<BaseUtils.Notifications.Notification> popupnotificationlist)        // orgnanise pop ups one at a time..
        {
            if (popupnotificationlist.Count > 0)
            {
                BaseUtils.Notifications.NotificationParas p = popupnotificationlist[0].Select(EDDConfig.Instance.Language);

                Action<object> act = new Action<object>((o) =>      // on ack, update list of ack entries
                {
                    DateTime ackdate = (DateTime)o;
                    System.Diagnostics.Debug.WriteLine("Ack to " + ackdate.ToStringZulu());
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("NotificationLastAckTime", EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("NotificationLastAckTime", "") + ackdate.ToStringZulu());
                });

                ExtendedControls.InfoForm infoform = new ExtendedControls.InfoForm();
                infoform.Info(p.Caption, this.Icon, p.Text, pointsize: popupnotificationlist[0].PointSize,
                        acknowledgeaction: act,
                        acknowledgedata: popupnotificationlist[0].StartUTC,enableurls:true);
                infoform.LinkClicked += (e) => { BaseUtils.BrowserInfo.LaunchBrowser(e.LinkText); };
                infoform.FormClosed += (s, e1) => { ShowNotification(popupnotificationlist); };     // chain to next, one at a time..
                infoform.StartPosition = FormStartPosition.CenterParent;

                popupnotificationlist.RemoveAt(0);      // remove this one so it does not appear.
                infoform.Show(this);
            }
        }

        #endregion

        #region Themeing

        public void ApplyTheme(bool panelrefreshaswell = false)     // set true if your changing the theme
        {
            panel_close.Visible = !ExtendedControls.Theme.Current.WindowsFrame;
            panel_minimize.Visible = !ExtendedControls.Theme.Current.WindowsFrame;
            label_version.Visible = !ExtendedControls.Theme.Current.WindowsFrame && !EDDOptions.Instance.DisableVersionDisplay;

            // note in no border mode, this is not visible on the title bar but it is in the taskbar..
            this.Text = "EDDiscovery" + (EDDOptions.Instance.DisableVersionDisplay ? "" : " " + label_version.Text);

            OnThemeChanging?.Invoke();

            ExtendedControls.Theme.Current.ApplyStd(this);

            statusStripEDD.Font = contextMenuStripTabs.Font = this.Font;

            OnThemeChanged?.Invoke();

            if (panelrefreshaswell)
                Controller.RefreshDisplays(); // needed to cause them to cope with theme change

            this.Refresh();                                             // force thru refresh to make sure its repainted
        }

        #endregion

        #region DB Star Sync 

        public void ForceSystemDBFullRefresh()
        {
            SystemsDatabase.Instance.ForceFullUpdate();
            Controller.AsyncPerformSync(true);
        }

        #endregion

        #region Controller event handlers 

        string syncprogressstring = "", refreshprogressstring = "";

        private void ReportSyncProgress(int percentComplete, string message)
        {
            if (!Controller.PendingClose)
            {
                if (percentComplete >= 0)
                {
                    toolStripProgressBarEDD.Visible = true;
                    toolStripProgressBarEDD.Value = percentComplete;
                }
                else
                {
                    toolStripProgressBarEDD.Visible = false;
                }

                syncprogressstring = message;
                toolStripStatusLabelEDD.Text = ObjectExtensionsStrings.AppendPrePad(syncprogressstring, refreshprogressstring, " | ");
            }
        }

        private void ReportRefreshProgress(int percentComplete, string message)      // percent not implemented for this
        {
            if (!Controller.PendingClose)
            {
                refreshprogressstring = message;
                toolStripStatusLabelEDD.Text = ObjectExtensionsStrings.AppendPrePad(syncprogressstring, refreshprogressstring, " | ");
                Update();       // nasty but it works - needed since we are doing UI work here and the UI thread will be blocked
            }
        }

        #endregion

        #region Theme

        private ExtendedControls.Theme GetThemeFromDB()
        {
            if (EliteDangerousCore.DB.UserDatabase.Instance.KeyExists("ThemeNameOf"))           // 
            {
                // we convert into JSON and then let the JSON reader do the job, of course, if we were doing this again, the JSON would just be in the DB

                JObject jo = new JObject();
                string name = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("ThemeNameOf", "Custom");

                jo["windowsframe"] = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("ThemeWindowsFrame", true);
                jo["formopacity"] = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble("ThemeFormOpacity", 100);
                jo["fontname"] = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("ThemeFont", ExtendedControls.Theme.DefaultFont);
                jo["fontsize"] = (float)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble("ThemeFontSize", ExtendedControls.Theme.DefaultFontSize);
                jo["buttonstyle"] = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("ButtonStyle", ExtendedControls.Theme.ButtonstyleSystem);
                jo["textboxborderstyle"] = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("TextBoxBorderStyle", ExtendedControls.Theme.TextboxborderstyleFixed3D);

                // pick a default, based on the name. This is useful when new names are introduced, as if we are using a default theme, we will pick the new colour from the theme
                var defaulttheme = ThemeList.FindTheme(name) ?? ThemeList.FindTheme("Windows Default");

                foreach (ExtendedControls.Theme.CI ci in Enum.GetValues(typeof(ExtendedControls.Theme.CI)))
                {
                    var cname = "ThemeColor" + ci.ToString();
                    int d = defaulttheme.GetColor(ci).ToArgb();
                    int dbv = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(cname, d);
                    jo[ci.ToString()] = dbv;
                }

                ExtendedControls.Theme theme = new ExtendedControls.Theme();
                return theme.FromJSON(jo, name, defaulttheme) ? theme : null;
            }
            else
                return null;
        }

        private void SaveThemeToDB(ExtendedControls.Theme theme)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("ThemeNameOf", theme.Name);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("ThemeWindowsFrame", theme.WindowsFrame);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble("ThemeFormOpacity", theme.Opacity);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("ThemeFont", theme.FontName);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble("ThemeFontSize", theme.FontSize);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("ButtonStyle", theme.ButtonStyle);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("TextBoxBorderStyle", theme.TextBoxBorderStyle);

            foreach (ExtendedControls.Theme.CI ci in Enum.GetValues(typeof(ExtendedControls.Theme.CI)))
            {
                var cname = "ThemeColor" + ci.ToString();
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(cname, theme.GetColor(ci).ToArgb());
            }
        }

        public static bool DownloadGECFile(string file)
        {
            try
            {
                string url = @"https://edastro.com/gec/json/all";
                return BaseUtils.DownloadFile.HTTPDownloadFile(url, file, false, out bool newfile);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("DownloadFromGEC exception:" + ex.Message);
            }

            return false;
        }

        #endregion

        #region Debug
        public static void PostInitDebug()
        {
         //   var comitems = MaterialCommodityMicroResourceType.GetCommodities(MaterialCommodityMicroResourceType.SortMethod.AlphabeticalRaresLast);

            System.Diagnostics.Debug.WriteLine($"Post Init debug");
            var sp = new EliteDangerousCore.Spansh.SpanshClass();


            //JToken tk = JToken.Parse(BaseUtils.FileHelpers.TryReadAllTextFromFile(@"c:\code\spanshmodtypes"));
            //JArray mt = tk["values"].Array();
            //foreach (var m in mt)
            //{
            //    string name = m.Str().Replace(" ", "").Replace("-", "_");
            //    System.Diagnostics.Debug.Write($"{name},");
            //}





            //   var list = sp.GetServices("Scirth", new string[] { "Apex Interstellar", "Black Market" }, 12);
            // var list = sp.GetServices("Scirth", new string[] { "Interstellar Factors Contact" }, 12);

            // var sys = SystemCache.FindSystem("Lembava", WebExternalDataLookup.Spansh);

            //  var isy = sp.GetSystem("sol");
            //var queryid = sp.RequestRoadToRiches("Sol", "Col 359 Sector BF-Z d136", 30, 25, 100, false, true, true, 1000000, 100000);

            // while (true)
            // {
            //     System.Threading.Thread.Sleep(2000);
            //     var resp = sp.TryGetRoadToRiches(queryid);
            // }Aaw

            // var ret = sp.GetStations("Sol", 4);

            SystemClass sol = new SystemClass("Sol", 10477373803);
            //sp.GetBodies(sol);
            // sp.GetStationsByDump(sol,10000000,false);

            //EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();
            //sp.GetSystem("Sol");

            //EDSMClass edsm = new EDSMClass();
            // edsm.GetSystem("Sol");

            //var permitlist = SystemsDB.GetListPermitSystems();
            //foreach (var x in permitlist)
            //    System.Diagnostics.Debug.WriteLine($"{x.Name} {x.SystemAddress} {x.X} {x.Y} {x.Z}");
        }


        #endregion

    }
}
