/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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

using System;
using System.Diagnostics;
using System.Linq;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using EliteDangerousCore.EDSM;

namespace EliteDangerousCore
{
    [DebuggerDisplay("Event {EntryType} {System.name} ({System.x,nq},{System.y,nq},{System.z,nq}) {EventTimeUTC} JID:{Journalid}")]
    public class HistoryEntry           // DONT store commander ID.. this history is externally filtered on it.
    {
        #region Variables

        public int Indexno;            // for display purposes.

        public JournalTypeEnum EntryType;
        public long Journalid;
        public JournalEntry journalEntry;
        public EDCommander Commander;

        public ISystem System;         // Must be set! All entries, even if they are not FSD entries.
                                       // The Minimum is name and edsm_id 
                                       // x/y/z can be NANs or position. 
                                       // if edsm_id = 0, no edsm match was found.
                                       // when the front end needs to use it, it will call EnsureSystemEDSM/FillInPositions to see if it can be filled up
                                       // if System.status != SystemStatusEnum.EDSC then its presumed its an inmemory load and the system table can be checked
                                       //       and if edsm_id>0 a load from SystemTable will occur with the edsm_id used
                                       //       if edsm_id=-1 a load from SystemTable will occur with the name used
                                       // SO the journal reader can just read data in that table only, does not need to do a system match

        public DateTime EventTimeLocal { get { return EventTimeUTC.ToLocalTime(); } }
        public DateTime EventTimeUTC;
        public TimeSpan AgeOfEntry() { return DateTime.Now - EventTimeUTC; }
        public string EventSummary;
        public string EventDescription;
        public string EventDetailedInfo;

        public int MapColour;

        public bool IsStarPosFromEDSM;  // flag populated from journal entry when HE is made. Was the star position taken from EDSM?
        public bool IsEDSMFirstDiscover;// flag populated from journal entry when HE is made. Were we the first to report the system to EDSM?
        public bool EdsmSync;           // flag populated from journal entry when HE is made. Have we synced?
        public bool EDDNSync;           // flag populated from journal entry when HE is made. Have we synced?
        public bool EGOSync;            // flag populated from journal entry when HE is made. Have we synced?
        public bool StartMarker;        // flag populated from journal entry when HE is made. Is this a system distance measurement system
        public bool StopMarker;         // flag populated from journal entry when HE is made. Is this a system distance measurement stop point
        public bool IsFSDJump { get { return EntryType == JournalTypeEnum.FSDJump; } }
        public bool IsLocOrJump { get { return EntryType == JournalTypeEnum.FSDJump || EntryType == JournalTypeEnum.Location; } }
        public bool IsFuelScoop { get { return EntryType == JournalTypeEnum.FuelScoop; } }
        public bool IsShipChange { get { return (EntryType == JournalTypeEnum.LoadGame || EntryType == JournalTypeEnum.Docked) && ShipInformation != null; } }
        public bool IsBetaMessage { get { return journalEntry?.Beta ?? false; } }
        public bool ISEDDNMessage
        {
            get
            {
                DateTime ed22 = new DateTime(2016, 10, 25, 12, 0, 0);
                if ((EntryType == JournalTypeEnum.Scan || EntryType == JournalTypeEnum.Docked || EntryType == JournalTypeEnum.FSDJump) && EventTimeUTC > ed22) return true; else return false;
            }
        }

        public double TravelledDistance { get { return travelled_distance; } }
        public TimeSpan TravelledSeconds { get { return travelled_seconds; } }
        public bool isTravelling { get { return travelling; } }
        public int TravelledMissingjump { get { return travelled_missingjump; } }
        public int Travelledjumps { get { return travelled_jumps; } }
        public string TravelledJumpsAndMisses { get { return travelled_jumps.ToStringInvariant() + ((travelled_missingjump > 0) ? (" (" + travelled_missingjump.ToStringInvariant() + ")") : ""); } }

        public bool IsLanded { get { return landed.HasValue && landed.Value == true; } }
        public bool IsDocked { get { return docked.HasValue && docked.Value == true; } }
        public string WhereAmI { get { return whereami; } }
        public string ShipType { get { return shiptype; } }
        public int ShipId { get { return shipid; } }
        public bool MultiPlayer { get { return onCrewWithCaptain != null; } }
        public string GameMode { get { return gamemode; } }
        public string Group { get { return group; } }
        public string GameModeGroup { get { return gamemode + ((group != null && group.Length > 0) ? (":" + group) : ""); } }

        public long Credits { get; set; }       // set up by Historylist during ledger accumulation

        public bool ContainsRares() // function due to debugger and cost of working out
        {
            return materialscommodities != null && materialscommodities.ContainsRares();
        }

        // Calculated values, not from JE

        public MaterialCommoditiesList MaterialCommodity { get { return materialscommodities; } }
        public ShipInformation ShipInformation { get { return shipmodules; } set { shipmodules = value; } }     // may be null if not set up yet
        public ModulesInStore StoredModules { get { return storedmodules; } set { storedmodules = value; } }
        public MissionList MissionList { get { return missionlist; } set { missionlist = value; } }

        public SystemNoteClass snc;     // system note class found attached to this entry. May be null

        private double travelled_distance;  // start/stop distance and time computation
        private TimeSpan travelled_seconds;
        bool travelling;

        int travelled_missingjump;
        int travelled_jumps;

        MaterialCommoditiesList materialscommodities;
        ShipInformation shipmodules;
        ModulesInStore storedmodules;
        MissionList missionlist;                    // mission state at this point..

        private bool? docked;                       // are we docked.  Null if don't know, else true/false
        private bool? landed;                       // are we landed on the planet surface.  Null if don't know, else true/false
        private string whereami = "";               // where we think we are
        private int shipid = -1;                    // ship id, -1 unknown
        private string shiptype = "Unknown";        // and the ship
        private string onCrewWithCaptain = null;    // if not null, your in another multiplayer ship      
        private string gamemode = "Unknown";        // game mode, from LoadGame event
        private string group = "";                  // group..

        #endregion

        #region Constructors

        private HistoryEntry()
        {

        }
        // for importing old events in from 2.1 - logs
        public static HistoryEntry MakeVSEntry(ISystem sys, DateTime eventt, int m, string dist, string info, int journalid = 0, bool firstdiscover = false)
        {
            Debug.Assert(sys != null);
            return new HistoryEntry
            {
                EntryType = JournalTypeEnum.FSDJump,
                System = sys,
                EventTimeUTC = eventt,
                EventSummary = "Jump to " + sys.name,
                EventDescription = dist,
                EventDetailedInfo = info,
                MapColour = m,
                Journalid = journalid,
                IsEDSMFirstDiscover = firstdiscover,
                EdsmSync = true
            };
        }

        public static HistoryEntry FromJournalEntry(JournalEntry je, HistoryEntry prev, bool checkedsm, out bool journalupdate, SQLiteConnectionSystem conn = null, EDCommander cmdr = null)
        {
            ISystem isys = prev == null ? new SystemClassDB("Unknown") : prev.System;
            int indexno = prev == null ? 1 : prev.Indexno + 1;

            int mapcolour = 0;
            journalupdate = false;
            bool starposfromedsm = false;
            bool firstdiscover = false;


            if (je.EventTypeID == JournalTypeEnum.Location || je.EventTypeID == JournalTypeEnum.FSDJump)
            {
                JournalLocOrJump jl = je as JournalLocOrJump;
                JournalFSDJump jfsd = je as JournalFSDJump;

                ISystem newsys;

                if (jl.HasCoordinate)       // LAZY LOAD IF it has a co-ord.. the front end will when it needs it
                {
                    newsys = new SystemClassDB(jl.StarSystem, jl.StarPos.X, jl.StarPos.Y, jl.StarPos.Z);
                    newsys.id_edsm = jl.EdsmID < 0 ? 0 : jl.EdsmID;       // pass across the EDSMID for the lazy load process.

                    if (jfsd != null && jfsd.JumpDist <= 0 && isys.HasCoordinate)     // if we don't have a jump distance (pre 2.2) but the last sys does have pos, we can compute distance and update entry
                    {
                        jfsd.JumpDist = SystemClassDB.Distance(isys, newsys); // fill it out here
                        journalupdate = true;
                    }

                }
                else
                {                           // Default one
                    newsys = new SystemClassDB(jl.StarSystem);
                    newsys.id_edsm = jl.EdsmID;

                    if (checkedsm)          // see if we can find the right system
                    {
                        SystemClassDB s = SystemClassDB.FindEDSM(newsys, conn);      // has no co-ord, did we find it?

                        if (s != null)                                          // yes, use, and update the journal with the esdmid, and also the position if we have a co-ord
                        {                                                       // so next time we don't have to do this again..
                            if (jl.HasCoordinate)
                            {
                                s.x = Math.Round(jl.StarPos.X * 32.0) / 32.0;
                                s.y = Math.Round(jl.StarPos.Y * 32.0) / 32.0;
                                s.z = Math.Round(jl.StarPos.Z * 32.0) / 32.0;
                            }

                            newsys = s;

                            if (jfsd != null && jfsd.JumpDist <= 0 && newsys.HasCoordinate && isys.HasCoordinate)     // if we don't have a jump distance (pre 2.2) but the last sys does, we can compute
                            {
                                jfsd.JumpDist = SystemClassDB.Distance(isys, newsys); // fill it out here.  EDSM systems always have co-ords, but we should check anyway
                                journalupdate = true;
                            }

                            if (jl.EdsmID <= 0 && newsys.id_edsm > 0)
                            {
                                journalupdate = true;
                            }
                        }
                    }
                }

                if (jfsd != null)
                {
                    if (jfsd.JumpDist <= 0 && isys.HasCoordinate && newsys.HasCoordinate) // if no JDist, its a really old entry, and if previous has a co-ord
                    {
                        jfsd.JumpDist = SystemClassDB.Distance(isys, newsys); // fill it out here
                        journalupdate = true;
                    }

                    mapcolour = jfsd.MapColor;
                }

                isys = newsys;
                starposfromedsm = jl.HasCoordinate ? jl.StarPosFromEDSM : newsys.HasCoordinate;
                firstdiscover = jl.EDSMFirstDiscover;
            }

            string summary, info, detailed;
            je.FillInformation(out summary, out info, out detailed);

            HistoryEntry he = new HistoryEntry
            {
                Indexno = indexno,
                EntryType = je.EventTypeID,
                Journalid = je.Id,
                journalEntry = je,
                System = isys,
                EventTimeUTC = je.EventTimeUTC,
                MapColour = mapcolour,
                EdsmSync = je.SyncedEDSM,
                EDDNSync = je.SyncedEDDN,
                EGOSync = je.SyncedEGO,
                StartMarker = je.StartMarker,
                StopMarker = je.StopMarker,
                EventSummary = summary,
                EventDescription = info,
                EventDetailedInfo = detailed,
                IsStarPosFromEDSM = starposfromedsm,
                IsEDSMFirstDiscover = firstdiscover,
                Commander = cmdr ?? EDCommander.GetCommander(je.CommanderId)
            };


            // WORK out docked/landed state

            if (prev != null)
            {
                if (prev.docked.HasValue)                   // copy docked..
                    he.docked = prev.docked;
                if (prev.landed.HasValue)
                    he.landed = prev.landed;

                he.shiptype = prev.shiptype;
                he.shipid = prev.shipid;
                he.whereami = prev.whereami;
                he.onCrewWithCaptain = prev.onCrewWithCaptain;
                he.gamemode = prev.gamemode;
                he.group = prev.group;
            }

            if (je.EventTypeID == JournalTypeEnum.Location)
            {
                JournalLocation jl = je as JournalLocation;
                he.docked = jl.Docked;
                he.whereami = jl.Docked ? jl.StationName : jl.Body;
            }
            else if (je.EventTypeID == JournalTypeEnum.Docked)
            {
                JournalDocked jl = je as JournalDocked;
                he.docked = true;
                he.whereami = jl.StationName;
            }
            else if (je.EventTypeID == JournalTypeEnum.Undocked)
                he.docked = false;
            else if (je.EventTypeID == JournalTypeEnum.Touchdown)
                he.landed = true;
            else if (je.EventTypeID == JournalTypeEnum.Liftoff)
                he.landed = false;
            else if (je.EventTypeID == JournalTypeEnum.SupercruiseEntry)
                he.whereami = (je as JournalSupercruiseEntry).StarSystem;
            else if (je.EventTypeID == JournalTypeEnum.SupercruiseExit)
                he.whereami = (je as JournalSupercruiseExit).Body;
            else if (je.EventTypeID == JournalTypeEnum.FSDJump)
                he.whereami = (je as JournalFSDJump).StarSystem;
            else if (je.EventTypeID == JournalTypeEnum.LoadGame)
            {
                JournalLoadGame jl = je as JournalLoadGame;

                he.onCrewWithCaptain = null;    // can't be in a crew at this point
                he.gamemode = jl.GameMode;      // set game mode
                he.group = jl.Group;            // and group, may be empty
                he.landed = jl.StartLanded;

                if (jl.Ship.IndexOf("buggy", StringComparison.InvariantCultureIgnoreCase) == -1)        // load game with buggy, can't tell what ship we get back into, so ignore
                {
                    he.shiptype = (je as JournalLoadGame).Ship;
                    he.shipid = (je as JournalLoadGame).ShipId;
                }
            }
            else if (je.EventTypeID == JournalTypeEnum.ShipyardBuy)         // BUY does not have ship id, but the new entry will that is written later - journals 8.34
                he.shiptype = (je as JournalShipyardBuy).ShipType;
            else if (je.EventTypeID == JournalTypeEnum.ShipyardNew)
            {
                he.shiptype = (je as JournalShipyardNew).ShipType;
                he.shipid = (je as JournalShipyardNew).ShipId;
            }
            else if (je.EventTypeID == JournalTypeEnum.ShipyardSwap)
            {
                he.shiptype = (je as JournalShipyardSwap).ShipType;
                he.shipid = (je as JournalShipyardSwap).ShipId;
            }
            else if (je.EventTypeID == JournalTypeEnum.JoinACrew)
                he.onCrewWithCaptain = (je as JournalJoinACrew).Captain;
            else if (je.EventTypeID == JournalTypeEnum.QuitACrew)
                he.onCrewWithCaptain = null;

            if (prev != null && prev.travelling)      // if we are travelling..
            {
                he.travelled_distance = prev.travelled_distance;
                he.travelled_missingjump = prev.travelled_missingjump;
                he.travelled_jumps = prev.travelled_jumps;

                if (he.IsFSDJump && !he.MultiPlayer)   // if jump, and not multiplayer..
                {
                    double dist = ((JournalFSDJump)je).JumpDist;
                    if (dist <= 0)
                        he.travelled_missingjump++;
                    else
                    {
                        he.travelled_distance += dist;
                        he.travelled_jumps++;
                    }
                }

                he.travelled_seconds = prev.travelled_seconds;
                TimeSpan diff = he.EventTimeUTC.Subtract(prev.EventTimeUTC);

                if (he.EntryType != JournalTypeEnum.LoadGame && diff < new TimeSpan(2, 0, 0))   // time between last entry and load game is not real time
                {
                    he.travelled_seconds += diff;
                }

                if (he.StopMarker || he.StartMarker)
                {
                    //Debug.WriteLine("Travelling stop at " + he.Indexno);
                    he.travelling = false;
                    he.EventDetailedInfo += ((he.EventDetailedInfo.Length > 0) ? Environment.NewLine : "") + "Travelled " + he.travelled_distance.ToStringInvariant("0.0") + " LY"
                                        + ", " + he.travelled_jumps + " jumps"
                                        + ((he.travelled_missingjump > 0) ? ", " + he.travelled_missingjump + " unknown distance jumps" : "") +
                                        ", time " + he.travelled_seconds;

                    he.travelled_distance = 0;
                    he.travelled_seconds = new TimeSpan(0);
                }
                else
                {
                    he.travelling = true;

                    if (he.IsFSDJump)
                    {
                        he.EventDetailedInfo += ((he.EventDetailedInfo.Length > 0) ? Environment.NewLine : "") + "Travelling" +
                                        " distance " + he.travelled_distance.ToString("0.0") + " LY"
                                        + ", " + he.travelled_jumps + " jumps"
                                        + ((he.travelled_missingjump > 0) ? ", " + he.travelled_missingjump + " unknown distance jumps" : "") +
                                        ", time " + he.travelled_seconds;
                    }
                }
            }

            if (he.StartMarker)
            {
                //Debug.WriteLine("Travelling start at " + he.Indexno);
                he.travelling = true;
            }

            return he;
        }

        public void ProcessWithUserDb(JournalEntry je, HistoryEntry prev, HistoryList hl, SQLiteConnectionUser conn)      // called after above with a USER connection
        {
            materialscommodities = MaterialCommoditiesList.Process(je, prev?.materialscommodities, conn, EliteConfigInstance.InstanceConfig.ClearMaterials, EliteConfigInstance.InstanceConfig.ClearCommodities);

            snc = SystemNoteClass.GetSystemNote(Journalid, IsFSDJump, System);       // may be null
        }

        public void SetJournalSystemNoteText(string text, bool commit, bool sendtoedsm)
        {
            if (snc == null || snc.Journalid == 0)           // if no system note, or its one on a system, from now on we assign journal system notes only from this IF
                snc = SystemNoteClass.MakeSystemNote("", DateTime.Now, System.name, Journalid, System.id_edsm, IsFSDJump);

            snc = snc.UpdateNote(text, commit, DateTime.Now, snc.EdsmId, IsFSDJump);        // and update info, and update our ref in case it has changed or gone null
                                                                                            // remember for EDSM send purposes if its an FSD entry

            if (snc != null && commit && sendtoedsm && snc.FSDEntry)                    // if still have a note, and commiting, and send to esdm, and FSD jump
                EDSMSync.SendComments(snc.SystemName, snc.Note, snc.EdsmId);
        }

        #endregion

        public System.Drawing.Bitmap GetIcon
        {
            get
            {
                if (journalEntry != null)
                    return journalEntry.Icon;
                else if (EntryType == JournalTypeEnum.FSDJump)
                    return EliteDangerous.Properties.Resources.hyperspace;
                else
                    return EliteDangerous.Properties.Resources.genericevent;
            }
        }


        public void UpdateMapColour(int v)
        {
            if (EntryType == JournalTypeEnum.FSDJump)
            {
                MapColour = v;
                if (Journalid != 0)
                    JournalEntry.UpdateMapColour(Journalid, v);
            }
        }

        public void UpdateCommanderID(int v)
        {
            if (Journalid != 0)
            {
                JournalEntry.UpdateCommanderID(Journalid, v);
            }
        }

        public void SetEdsmSync()
        {
            EdsmSync = true;
            if (Journalid != 0)
            {
                JournalEntry.UpdateSyncFlagBit(Journalid, SyncFlags.EDSM, true);
            }
        }
        public void SetEddnSync()
        {
            EDDNSync = true;
            if (Journalid != 0)
            {
                JournalEntry.UpdateSyncFlagBit(Journalid, SyncFlags.EDDN, true);
            }
        }

        public void SetEGOSync()
        {
            EGOSync = true;
            if (Journalid != 0)
            {
                JournalEntry.UpdateSyncFlagBit(Journalid, SyncFlags.EGO, true);
            }
        }

        public void SetFirstDiscover(bool firstdiscover = true)
        {
            IsEDSMFirstDiscover = firstdiscover;
            if (journalEntry != null)
            {
                JournalLocOrJump jl = journalEntry as JournalLocOrJump;
                if (jl != null)
                {
                    jl.UpdateEDSMFirstDiscover(firstdiscover);
                }
            }
        }

        public bool IsJournalEventInEventFilter(string[] events)
        {
            return events.Contains(EntryType.ToString().SplitCapsWord());
        }

        public bool IsJournalEventInEventFilter(string eventstr)
        {
            return eventstr == "All" || IsJournalEventInEventFilter(eventstr.Split(';'));
        }

    }


}
