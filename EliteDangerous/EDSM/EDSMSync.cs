/*
 * Copyright © 2016 EDDiscovery development team
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
using EliteDangerousCore.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EliteDangerousCore.EDSM
{  
    public class EDSMSync
    {
        Thread ThreadEDSMSync;
        bool running = false;
        bool Exit = false;
        bool _syncTo = false;
        bool _syncFrom = false;
        int _defmapcolour = 0;
        private Action<string> logout;

        public EDSMSync(Action<string> logger)
        {
            logout = logger;
        }

        public bool StartSync(EDSMClass edsm, HistoryList hl, bool syncto, bool syncfrom, int defmapcolour)
        {
            if (running) // Only start once.
                return false;
            _syncTo = syncto;
            _syncFrom = syncfrom;
            _defmapcolour = defmapcolour;

            ThreadEDSMSync = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(SyncThread));
            ThreadEDSMSync.Name = "EDSM Sync";
            ThreadEDSMSync.IsBackground = true;
            ThreadEDSMSync.Start(new Tuple<EDSMClass,HistoryList>(edsm,hl));

            return true;
        }

        public void StopSync()
        {
            Exit = true;
        }

        private void SyncThread(object _edsm)
        {
            Tuple<EDSMClass, HistoryList> v = (Tuple<EDSMClass, HistoryList>)_edsm;
            EDSMClass edsm = v.Item1;
            HistoryList hl = v.Item2;
            running = true;
            Sync(edsm,hl);
            running = false;
        }

        private void Sync(EDSMClass edsm, HistoryList hl)
        {
            try
            {
                // Make sure the EDSM class has this history's commander set
                int cmdrid = hl.CommanderId;
                EDCommander cmdr = EDCommander.GetCommander(cmdrid);
                if (cmdr != null)
                {
                    edsm.commanderName = cmdr.EdsmName ?? cmdr.Name;
                    edsm.apiKey = cmdr.APIKey;
                }

                logout("EDSM sync begin");

                List<HistoryEntry> hlfsdunsyncedlist = hl.FilterByNotEDSMSyncedAndFSD;        // first entry is oldest

                if ( _syncTo && hlfsdunsyncedlist.Count > 0 )                   // send systems to edsm (verified with dates, 29/9/2016, utc throughout)
                {
                    DateTime logstarttime = DateTime.MinValue;
                    DateTime logendtime = DateTime.MinValue;

                    logout("EDSM: Sending " + hlfsdunsyncedlist.Count.ToString() + " flightlog entries");

                    List<HistoryEntry> edsmsystemlog = null;

                    int edsmsystemssent = 0;

                    foreach (var he in hlfsdunsyncedlist)
                    {
                        if (Exit)
                        {
                            running = false;
                            return;
                        }

                        if (edsmsystemlog == null || he.EventTimeUTC >= logendtime.AddDays(-1))
                        {
                            edsm.GetLogs(he.EventTimeUTC.AddDays(-1), null, out edsmsystemlog, out logstarttime, out logendtime);        // always returns a log, time is in UTC as per HistoryEntry and JournalEntry
                        }

                        if (logendtime < logstarttime)
                        {
                            running = false;
                            return;
                        }

                        if (edsmsystemlog == null)
                        {
                            running = false;
                            return;
                        }

                        HistoryEntry ps2 = (from c in edsmsystemlog where c.System.Name == he.System.Name && c.EventTimeUTC.Ticks == he.EventTimeUTC.Ticks select c).FirstOrDefault();

                        if (ps2 != null)                // it did, just make sure EDSM sync flag is set..
                        {
                            he.SetEdsmSync();
                        }
                        else
                        {
                            string errmsg;              // (verified with EDSM 29/9/2016)
                            int errno;
                            bool firstdiscover;
                            int edsmid;

                            if (edsm.SendTravelLog(he.System.Name, he.EventTimeUTC, he.System.HasCoordinate && !he.IsStarPosFromEDSM, he.System.X, he.System.Y, he.System.Z, out errmsg, out errno, out firstdiscover, out edsmid))
                            {
                                if (edsmid != 0 && he.System.EDSMID <= 0)
                                {
                                    he.System.EDSMID = edsmid;
                                    JournalEntry.UpdateEDSMIDPosJump(he.Journalid, he.System, false, -1);
                                }

                                if (firstdiscover)
                                {
                                    he.SetFirstDiscover();
                                }

                                he.SetEdsmSync();
                                edsmsystemssent++;
                            }

                            if (errmsg.Length > 0)
                            {
                                logout(errmsg);

                                if (errno != 303 && errno != 304) // Skip the system if EDSM rejects the system
                                {
                                    break;
                                }
                            }
                        }
                    }

                    logout(string.Format("EDSM Systems sent {0}", edsmsystemssent));
                }

                // TBD Comments to edsm?

                if ( _syncFrom )                                                            // Verified ok with time 29/9/2016
                {
                    var json = edsm.GetComments(new DateTime(2011, 1, 1));

                    if (json != null)
                    {
                        JObject msg = JObject.Parse(json);
                        int msgnr = msg["msgnum"].Value<int>();

                        JArray comments = (JArray)msg["comments"];
                        if (comments != null)
                        {
                            int commentsadded = 0;

                            foreach (JObject jo in comments)
                            {
                                string name = jo["system"].Value<string>();
                                string note = jo["comment"].Value<string>();
                                string utctime = jo["lastUpdate"].Value<string>();
                                int edsmid = 0;

                                if (!Int32.TryParse(jo["systemId"].Str("0"), out edsmid))
                                    edsmid = 0;

                                DateTime localtime = DateTime.ParseExact(utctime, "yyyy-MM-dd HH:mm:ss", 
                                            CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToLocalTime();

                                SystemNoteClass curnote = SystemNoteClass.GetNoteOnSystem(name, edsmid);

                                if (curnote != null)                // curnote uses local time to store
                                {
                                    if (localtime.Ticks > curnote.Time.Ticks)   // if newer, add on (verified with EDSM 29/9/2016)
                                    {
                                        curnote.UpdateNote(curnote.Note + ". EDSM: " + note, true, localtime, edsmid, true);
                                        commentsadded++;
                                    }
                                }
                                else
                                {
                                    SystemNoteClass.MakeSystemNote(note, localtime, name, 0, edsmid,true);   // new one!  its an FSD one as well
                                    commentsadded++;
                                }
                            }

                            logout(string.Format("EDSM Comments downloaded/updated {0}", commentsadded));
                        }
                    }
                }

                logout("EDSM sync Done");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception ex:" + ex.Message);
                logout("EDSM sync Exception " + ex.Message);
            }
        }

        public static void SendTravelLog(HistoryEntry he) // (verified with EDSM 29/9/2016, seen UTC time being sent, and same UTC time on ESDM).
        {
            EDSMClass edsm = new EDSMClass();
            var cmdr = he.Commander;
            edsm.commanderName = cmdr.EdsmName ?? cmdr.Name;
            edsm.apiKey = cmdr.APIKey;

            if (!edsm.IsApiKeySet)
                return;

            string errmsg;
            int errno;
            bool firstdiscover;
            int edsmid;
            Task taskEDSM = Task.Factory.StartNew(() =>
            {                                                   // LOCAL time, there is a UTC converter inside this call
                if (edsm.SendTravelLog(he.System.Name, he.EventTimeUTC, he.System.HasCoordinate, he.System.X, he.System.Y, he.System.Z, out errmsg, out errno, out firstdiscover, out edsmid))
                {
                    if (edsmid != 0 && he.System.EDSMID <= 0)
                    {
                        he.System.EDSMID = edsmid;
                        JournalEntry.UpdateEDSMIDPosJump(he.Journalid, he.System, false, -1);
                    }

                    if (firstdiscover)
                    {
                        he.SetFirstDiscover();
                    }

                    he.SetEdsmSync();
                }
            });
        }

    }
}
