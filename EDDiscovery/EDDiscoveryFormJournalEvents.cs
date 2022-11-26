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
                        //System.Diagnostics.Debug.WriteLine($"DLL-> {history[i].EventTimeUTC} {history[i].EventSummary}");
                        DLLManager.NewJournalEntry(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(history, history[i]), true);
                    }
                }

                DLLManager.Refresh(EDCommander.Current.Name, EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(history, history.GetLast));
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
            Controller.NewJournalEntryFromScanner(e,null);                 // push it thru as if the monitor watcher saw it
        }

        // Called by controller before any HL removal reorder. The raw HE stream.  The MCMR etc databases have been updated
        public void Controller_NewHistoryEntryUnfiltered(HistoryEntry he)
        {
            //he.FillInformation(out string ed, out string edi);System.Diagnostics.Debug.WriteLine($"HE Unfiltered {he.EntryType} {he.EventSummary} {ed}");
            //System.Diagnostics.Debug.WriteLine($"{Environment.TickCount} EDF Unfiltered {he.EntryType}");

            // EDSM needs the raw stream, so send it here..

            if (EDCommander.Current.SyncToEdsm && EDSMJournalSync.OkayToSend(he))          
            {
                EDSMJournalSync.SendEDSMEvents(LogLine, new List<HistoryEntry>() { he });       // send, if bad credentials, EDSM will moan alerting the user
            }

            // as does Inara. Note the MCMR has been updated.  Needed here due to using materials/cargo
            if (EDCommander.Current.SyncToInara)
            {
                var mcmr = history.MaterialCommoditiesMicroResources.GetDict(he.MaterialCommodity);
                EliteDangerousCore.Inara.InaraSync.NewEvent(LogLine, he, mcmr);
            }

            if (DLLManager.Count > 0)
                DLLManager.NewUnfilteredJournalEntry(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(history, he));       // give DLL the unfiltered stream

        }

        // Called after HE removal/reorder, and after the UI's has had a chance to operate
        private void Controller_NewEntrySecond(HistoryEntry he, HistoryList hl)         
        {
            BaseUtils.AppTicks.TickCountLapDelta("DFS", true);

            actioncontroller.ActionRunOnEntry(he, Actions.ActionEventEDList.NewEntry(he));

            var t1 = BaseUtils.AppTicks.TickCountLapDelta("DFS");
            if (t1.Item2 >= 80)
                System.Diagnostics.Trace.WriteLine("NE Second Actions slow " + t1.Item1);

            if (he.IsFSDCarrierJump)
            {
                int count = history.GetVisitsCount(he.System.Name);
                LogLine(string.Format("Arrived at system {0} Visit No. {1}".T(EDTx.EDDiscoveryForm_Arrived), he.System.Name, count));
                System.Diagnostics.Trace.WriteLine("Arrived at system: " + he.System.Name + " " + count + ":th visit.");
            }

            if (EDCommander.Current.SyncToIGAU)
            {
                EliteDangerousCore.IGAU.IGAUSync.NewEvent(LogLine, he);
            }

            if (EDCommander.Current.SyncToEDAstro)
            {
                EliteDangerousCore.EDAstro.EDAstroSync.SendEDAstroEvents(new List<HistoryEntry>() { he });
            }

            if (EDCommander.Current.SyncToEddn == true)
            {
                if (queuedfsssd != null && ((EliteDangerousCore.JournalEvents.JournalFSSSignalDiscovered)queuedfsssd.journalEntry).Signals[0].SystemAddress == he.System.SystemAddress)     // if queued, and we are now in its system
                {
                    System.Diagnostics.Debug.WriteLine($"EDDN send of FSSSignalDiscovered is sent - now in system");
                    ((EliteDangerousCore.JournalEvents.JournalFSSSignalDiscovered)queuedfsssd.journalEntry).EDDNSystem = he.System; // override for EDDN purposes
                    EDDNSync.SendEDDNEvents(LogLine, new List<HistoryEntry> {queuedfsssd });
                    queuedfsssd = null;
                }

                if (EDDNClass.IsEDDNMessage(he.EntryType) && he.AgeOfEntry() < TimeSpan.FromDays(1.0))
                {
                    // if FSS Signal discovered, but the system address is not of the current system we think we are in, then queue it until location/jump comes about
                    if (he.EntryType == JournalTypeEnum.FSSSignalDiscovered && ((EliteDangerousCore.JournalEvents.JournalFSSSignalDiscovered)he.journalEntry).Signals[0].SystemAddress != he.System.SystemAddress)
                    {
                        queuedfsssd = he;
                        System.Diagnostics.Debug.WriteLine($"EDDN send of FSSSignalDiscovered is queued due to SystemAddress not being Isystem address");
                    }
                    else
                        EDDNSync.SendEDDNEvents(LogLine, new List<HistoryEntry> { he });
                }
            }

            if (DLLManager.Count > 0)
                DLLManager.NewJournalEntry(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(history, he), false);

            ScreenshotConverter.NewJournalEntry(he.journalEntry);       // tell the screenshotter.

            CheckActionProfile(he);

        }

        private HistoryEntry queuedfsssd = null;

        private void Controller_NewUIEvent(UIEvent uievent)
        {
            BaseUtils.Variables cv = new BaseUtils.Variables();

            string prefix = "EventClass_";
            cv.AddPropertiesFieldsOfClass(uievent, prefix, new Type[] { typeof(System.Drawing.Icon), typeof(System.Drawing.Image), typeof(System.Drawing.Bitmap), typeof(QuickJSON.JObject) }, 5);
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
                        QuickJSON.JToken t = QuickJSON.JToken.FromObject(uievent, ignoreunserialisable: true,
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
