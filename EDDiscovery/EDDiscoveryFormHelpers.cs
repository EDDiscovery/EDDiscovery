/*
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
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

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
            if (OnNewTarget != null)
                OnNewTarget(sender);
        }

        public void NoteChanged(Object sender, HistoryEntry snc, bool committed)
        {
            if (OnNoteChanged != null)
                OnNoteChanged(sender, snc, committed);
        }

        public void NewCalculatedRoute(List<ISystem> list)
        {
            if (OnNewCalculatedRoute != null)
                OnNewCalculatedRoute(list);
        }

        #endregion

        #region webserver

        public bool WebServerControl(bool start)        // false if an error occurs
        {
            if (WebServer.Running)
                WebServer.Stop();

            if (start)
            {
                WebServer.Port = EDDConfig.Instance.WebServerPort;

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
            var helist = EDSMJournalSync.GetListToSend(history.EntryOrder());               // find out what to send..

            if (helist.Count >= 500)
            {
                ExtendedControls.ConfigurableForm cf = new ExtendedControls.ConfigurableForm();

                int width = 400;

                DateTime lasthe = helist.Last().EventTimeUTC;

                cf.Add(new ExtendedControls.ConfigurableForm.Entry("UC", typeof(Label),
                            string.Format("There are {0} EDSM reports to send, this will take time and bandwidth, choose from the following what to do. Entries before this will be marked as sent.".T(EDTx.EDDiscoveryForm_SendEDSMCaption), helist.Count),
                             new Point(5, 30), new Size(width - 5 - 20, 100), null)
                { textboxmultiline = true });

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

                DateTime date = DateTime.UtcNow;

                cf.Trigger += (dialogname, controlname, tag) =>
                {
                    if (controlname.Contains("All"))
                    {
                        date = new DateTime(1900, 1, 1);
                        cf.ReturnResult(DialogResult.OK);
                    }
                    else if (controlname.Contains("Today"))
                    {
                        date = lasthe.AddDays(-1);
                        cf.ReturnResult(DialogResult.OK);
                    }
                    else if (controlname.Contains("Custom"))
                    {
                        date = cf.GetDateTime("Date").Value.ToUniversalTime();
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

                var jes = helist.Where(x => x.EventTimeUTC < date).Select(x => x.journalEntry).ToList();
                JournalEntry.SetEdsmSyncList(jes);

                helist = EDSMJournalSync.GetListToSend(history.EntryOrder());               // find out what to send..
            }

            if (helist.Count > 0)
                EDSMJournalSync.SendEDSMEvents(l => LogLine(l), helist);

        }


        #endregion

        #region 3dmap
        public void OpenOld3DMap()     // Old map open
        {
            if (!old3DMap.Is3DMapsRunning)            // if not running, click the 3dmap button
            {
                this.Cursor = Cursors.WaitCursor;

                ISystem last = PrimaryCursor.GetCurrentHistoryEntry?.System;

                old3DMap.Prepare(last, EDCommander.Current.HomeSystemTextOrSol,
                            EDCommander.Current.MapCentreOnSelection ? last : EDCommander.Current.HomeSystemIOrSol,
                            EDCommander.Current.MapZoom, Controller.history.FilterByTravelTime(null, null));
                old3DMap.Show();
                this.Cursor = Cursors.Default;
            }
        }

        public UserControls.UserControl3DMap Open3DMap()         // Current map - open at last position or configured position
        {
            var t3dmap = typeof(UserControls.UserControl3DMap);

            UserControls.UserControlCommonBase uccb = null;

            var tabfind3dmap = tabControlMain.Find(t3dmap);

            if ( tabfind3dmap != null )
            {
                tabControlMain.SelectTab(tabfind3dmap.Item1);       // goto tab
                uccb = tabfind3dmap.Item2;
            }
            else
            { 
                var findpopoutform = PopOuts.FindUCCB(t3dmap);
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

        List<Notifications.Notification> popupnotificationlist = new List<Notifications.Notification>();
        void ShowNotification()        // orgnanise pop ups one at a time..
        {
            if (popupnotificationlist.Count > 0)
            {
                Notifications.NotificationParas p = popupnotificationlist[0].Select(EDDConfig.Instance.Language);

                Action<object> act = new Action<object>((o) =>      // on ack, update list of ack entries
                {
                    DateTime ackdate = (DateTime)o;
                    System.Diagnostics.Debug.WriteLine("Ack to " + ackdate.ToStringZulu());
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("NotificationLastAckTime", EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("NotificationLastAckTime", "") + ackdate.ToStringZulu());
                });

                ExtendedControls.InfoForm infoform = new ExtendedControls.InfoForm();
                infoform.Info(p.Caption, this.Icon, p.Text, pointsize: popupnotificationlist[0].PointSize,
                        acknowledgeaction: act,
                        acknowledgedata: popupnotificationlist[0].StartUTC);

                infoform.FormClosed += (s, e1) => { ShowNotification(); };     // chain to next, one at a time..

                popupnotificationlist.RemoveAt(0);
                infoform.Show();
            }
        }

        #endregion

        #region DLL

        public bool DLLRunAction(string eventname, string paras)
        {
            System.Diagnostics.Debug.WriteLine("Run " + eventname + "(" + paras + ")");
            actioncontroller.ActionRun(Actions.ActionEventEDList.DLLEvent(eventname), new BaseUtils.Variables(paras, BaseUtils.Variables.FromMode.MultiEntryComma));
            return true;
        }

        public bool DLLRequestHistory(long index, bool isjid, out EDDDLLInterfaces.EDDDLLIF.JournalEntry f)
        {
            HistoryEntry he = isjid ? history.GetByJID(index) : history.GetByEntryNo((int)index);
            f = EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(history, he);
            return he != null;
        }


        #endregion

        #region Themeing

        public void ApplyTheme(bool panelrefreshaswell = false)     // set true if your changing the theme
        {
            panel_close.Visible = !theme.WindowsFrame;
            panel_minimize.Visible = !theme.WindowsFrame;
            label_version.Visible = !theme.WindowsFrame && !EDDOptions.Instance.DisableVersionDisplay;

            // note in no border mode, this is not visible on the title bar but it is in the taskbar..
            this.Text = "EDDiscovery" + (EDDOptions.Instance.DisableVersionDisplay ? "" : " " + label_version.Text);

            theme.ApplyStd(this);

            statusStripEDD.Font = contextMenuStripTabs.Font = this.Font;

            this.Refresh();                                             // force thru refresh to make sure its repainted

            //System.Diagnostics.Debug.WriteLine("Label version " + label_version.Location + " " + label_version.Size + " " + mainMenu.Size);

            OnThemeChanged?.Invoke();

            if (panelrefreshaswell)
                Controller.RefreshDisplays(); // needed to cause them to cope with theme change
        }

        #endregion

        #region EDSM Star Sync 

        private void edsmRefreshTimer_Tick(object sender, EventArgs e)
        {
            Controller.AsyncPerformSync();
        }

        public void ForceEDSMFullRefresh()
        {
            SystemsDatabase.Instance.ForceEDSMFullUpdate();
            SystemsDatabase.Instance.ForceEDSMAliasFullUpdate();
            Controller.AsyncPerformSync(true, true);
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


    }
}
