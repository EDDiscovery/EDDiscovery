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
using EliteDangerousCore.EDDN;
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace EDDiscovery
{
    public partial class EDDiscoveryForm
    {
        private void Controller_RefreshCommanders()
        {
            LoadCommandersListBox();             // in case a new commander has been detected
        }

        private void Controller_RefreshStarting()
        {
            RefreshButton(false);
            actioncontroller.ActionRun(Actions.ActionEventEDList.onRefreshStart);
        }

        private void Controller_RefreshComplete()
        {
            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Refresh complete");

            RefreshButton(true);
            actioncontroller.ActionRunOnRefresh();

            if (EDCommander.Current.SyncToInara && history.GetLast != null)
            {
                EliteDangerousCore.Inara.InaraSync.Refresh(LogLine, history.GetLast, EDCommander.Current);
            }

            if (DLLManager.Count > 0)
            {
                HistoryEntry lastfileh = history.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.Fileheader);

                if (lastfileh != null)
                {
                    for (int i = lastfileh.EntryNumber - 1; i < history.Count; i++)      // play thru last history entries up to last file position for the DLLs, indicating stored
                    {
                        //System.Diagnostics.Debug.WriteLine("{0} : {1} {2}", i, history.EntryOrder[i].EventTimeUTC, history.EntryOrder[i].EventSummary);
                        DLLManager.NewJournalEntry(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(history, history[i]), true);
                    }
                }

                DLLManager.Refresh(EDCommander.Current.Name, EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(history, history.GetLast));
            }

            FrontierCAPI.Disconnect();         // Disconnect capi from current user, but don't clear their credential file

            // available, and not hidden commander, and we have logged in previously
            if ( FrontierCAPI.ClientIDAvailable && EDCommander.Current.Id >= 0 && FrontierCAPI.HasUserBeenLoggedIn(EDCommander.Current.Name)) 
            {
                System.Threading.Tasks.Task.Run(() =>           // don't hold up the main thread, do it in a task, as its a HTTP operation
                {
                    if (FrontierCAPI.LogIn(EDCommander.Current.Name))      // try and get to Active.  May cause a new frontier login
                    {
                       LogLine(FrontierCAPI.Active ? "CAPI User Logged in" : "CAPI User requires new log in");
                    }
                });
            }

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Refresh complete finished");

            if (EDCommander.Current.SyncToEdsm)        // no sync, no credentials, no action
            {
                EDSMClass edsm = new EDSMClass();
                if (edsm.ValidCredentials)
                    EDSMSend();
            }
        }

        public void NewEntry(JournalEntry e)       // programatically do a new entry
        {
            Controller.NewEntry(e);                 // push it thru as if the monitor watcher saw it
        }

        private void Controller_NewEntrySecond(HistoryEntry he, HistoryList hl)         // called after all UI's have had their chance
        {
            BaseUtils.AppTicks.TickCountLapDelta("DFS", true);

            actioncontroller.ActionRunOnEntry(he, Actions.ActionEventEDList.NewEntry(he));

            var t1 = BaseUtils.AppTicks.TickCountLapDelta("DFS");
            if (t1.Item2 >= 80)
                System.Diagnostics.Trace.WriteLine("NE Second Actions slow " + t1.Item1);

            // all notes committed
            SystemNoteClass.CommitDirtyNotes((snc) => { if (EDCommander.Current.SyncToEdsm && snc.FSDEntry) EDSMClass.SendComments(snc.SystemName, snc.Note, 0, he.Commander); });

            var lastent = history.GetLast;
            if (!object.ReferenceEquals(he, lastent))
            {
                LogLineHighlight(string.Format("Current history entry is not last in history - possible re-entrancy.\nAlert the EDDiscovery developers using either discord or Github (see help) and attach log file {0}", BaseUtils.TraceLog.LogFileName));
                Trace.WriteLine($"Current history entry is not last in history");
                Trace.WriteLine($"Current entry: {he.journalEntry?.GetJsonString()}");
                Trace.WriteLine($"Last entry: {lastent.journalEntry?.GetJsonString()}");
                Trace.WriteLine($"Stack Trace:");
                Trace.WriteLine(new StackTrace(true).ToString());
            }

            if (he.IsFSDCarrierJump)
            {
                int count = history.GetVisitsCount(he.System.Name);
                LogLine(string.Format("Arrived at system {0} Visit No. {1}".T(EDTx.EDDiscoveryForm_Arrived), he.System.Name, count));
                System.Diagnostics.Trace.WriteLine("Arrived at system: " + he.System.Name + " " + count + ":th visit.");
            }

            if (EDCommander.Current.SyncToEdsm && EDSMJournalSync.OkayToSend(he))           // send this one, if allowed.
            {
                EDSMJournalSync.SendEDSMEvents(LogLine, new List<HistoryEntry>() { he });       // send, if bad credentials, EDSM will moan alerting the user
            }

            if (EDCommander.Current.SyncToInara)
            {
                EliteDangerousCore.Inara.InaraSync.NewEvent(LogLine, he);
            }

            if (EDCommander.Current.SyncToIGAU)
            {
                EliteDangerousCore.IGAU.IGAUSync.NewEvent(LogLine, he);
            }

            if (EDDNClass.IsEDDNMessage(he.EntryType) && he.AgeOfEntry() < TimeSpan.FromDays(1.0) && EDCommander.Current.SyncToEddn == true)
            {
                EDDNSync.SendEDDNEvents(LogLine, he);
            }

            DLLManager.NewJournalEntry(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(history, he), false);

            screenshotconverter.NewJournalEntry(he.journalEntry);       // tell the screenshotter.

            CheckActionProfile(he);

            // finally, CAPI, if docked, try and get commodity data, and if so, create a new EDD record
            // placed here because it causes a new set of newentries to be called

            if (he.EntryType == JournalTypeEnum.Docked && FrontierCAPI.Active)      
            {
                System.Threading.Tasks.Task.Run(() =>           // don't hold up the main thread, do it in a task, as its a HTTP operation
                {
                    var dockevt = he.journalEntry as EliteDangerousCore.JournalEvents.JournalDocked;

                    string marketjson = FrontierCAPI.Market();

                    CAPI.Market mk = new CAPI.Market(marketjson);
                    if (mk.IsValid)
                    {
                        System.IO.File.WriteAllText(@"c:\code\market.json", marketjson);

                        if (dockevt.StationName.Equals(mk.Name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            System.Diagnostics.Debug.WriteLine($"CAPI got market {mk.Name}");

                            var entry = new EliteDangerousCore.JournalEvents.JournalEDDCommodityPrices(he.EventTimeUTC.AddSeconds(1),
                                            mk.ID, mk.Name, he.System.Name, EDCommander.CurrentCmdrID, mk.Commodities);

                            var jo = entry.ToJSON();        // get json of it, and add it to the db
                            entry.Add(jo);

                            BeginInvoke((System.Windows.Forms.MethodInvoker)delegate
                            {
                                Debug.Assert(System.Windows.Forms.Application.MessageLoop);
                                System.Diagnostics.Debug.WriteLine("CAPI fire new entry");
                                NewEntry(entry);                // then push it thru. this will cause another set of calls to NewEntry First/Second
                                                                // EDDN handler will pick up EDDCommodityPrices and send it.
                            });
                        }
                        else
                            System.Diagnostics.Trace.WriteLine($"CAPI disagree on market {dockevt.StationName} vs {mk.Name}");
                    }
                    else
                        System.Diagnostics.Trace.WriteLine($"CAPI market invalid {marketjson}");
                });
            }
        }

        private void Controller_NewUIEvent(UIEvent uievent)
        {
            BaseUtils.Variables cv = new BaseUtils.Variables();

            string prefix = "EventClass_";
            cv.AddPropertiesFieldsOfClass(uievent, prefix, new Type[] { typeof(System.Drawing.Icon), typeof(System.Drawing.Image), typeof(System.Drawing.Bitmap), typeof(BaseUtils.JSON.JObject) }, 5);
            cv[prefix + "UIDisplayed"] = "0";
            actioncontroller.ActionRun(Actions.ActionEventEDList.onUIEvent, cv);
            actioncontroller.ActionRun(Actions.ActionEventEDList.EliteUIEvent(uievent), cv);

            if (!uievent.EventRefresh)      // don't send the refresh events thru the system..  see if profiles need changing
            {
                Actions.ActionVars.TriggerVars(cv, "UI" + uievent.EventTypeStr, "UIEvent");

                int i = EDDProfiles.Instance.ActionOn(cv, out string errlist);
                if (i >= 0)
                    ChangeToProfileId(i, true);

                if (errlist.HasChars())
                    LogLine("Profile reports errors in triggers:".T(EDTx.EDDiscoveryForm_PE1) + errlist);

                try
                {
                    if (DLLManager.Count > 0)       // if worth calling..
                    {
                        BaseUtils.JSON.JToken t = BaseUtils.JSON.JToken.FromObject(uievent, ignoreunserialisable: true,
                                                                                    ignored: new Type[] { typeof(Bitmap), typeof(Image) },
                                                                                    maxrecursiondepth: 3);
                        string output = t?.ToString();
                        if (output != null)
                        {
                            //  System.Diagnostics.Debug.WriteLine("DLL JSON UI String " + output);
                            DLLManager.NewUIEvent(output);
                        }
                        else
                            System.Diagnostics.Debug.WriteLine("JSON convert error from object in DLL");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Could not serialise " + uievent.EventTypeStr + " " + ex);
                }

            }
        }


    }
}
