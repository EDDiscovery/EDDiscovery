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
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
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
        public bool ISEDDNMessage
        {
            get
            {
                DateTime ed22 = new DateTime(2016, 10, 25, 12, 0, 0);
                if ((EntryType == JournalTypeEnum.Scan || EntryType == JournalTypeEnum.Docked || EntryType == JournalTypeEnum.FSDJump) && EventTimeUTC>ed22 ) return true; else return false;
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
        public string GameModeGroup { get { return gamemode + ((group!=null && group.Length>0) ? (":" + group) : ""); } }

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
            else if (je.EventTypeID == JournalTypeEnum.QuitACrew )
                he.onCrewWithCaptain = null;

            if (prev != null && prev.travelling)      // if we are travelling..
            {
                he.travelled_distance = prev.travelled_distance;
                he.travelled_missingjump = prev.travelled_missingjump;
                he.travelled_jumps = prev.travelled_jumps;

                if (he.IsFSDJump && !he.MultiPlayer )   // if jump, and not multiplayer..
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
                    Debug.WriteLine("Travelling stop at " + he.Indexno);
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
                Debug.WriteLine("Travelling start at " + he.Indexno);
                he.travelling = true;
            }

            return he;
        }

        public void ProcessWithUserDb(JournalEntry je, HistoryEntry prev, HistoryList hl, SQLiteConnectionUser conn)      // called after above with a USER connection
        {
            materialscommodities = MaterialCommoditiesList.Process(je, prev?.materialscommodities, conn, EliteConfigInstance.InstanceConfig.ClearMaterials, EliteConfigInstance.InstanceConfig.ClearCommodities);

            snc = SystemNoteClass.GetSystemNote(Journalid, IsFSDJump, System);       // may be null
        }

        public void SetJournalSystemNoteText(string text, bool commit , bool sendtoedsm)
        {
            if (snc == null || snc.Journalid == 0 )           // if no system note, or its one on a system, from now on we assign journal system notes only from this IF
                snc = SystemNoteClass.MakeSystemNote("",DateTime.Now,System.name,Journalid, System.id_edsm , IsFSDJump);

            snc = snc.UpdateNote(text,commit,DateTime.Now,snc.EdsmId , IsFSDJump);        // and update info, and update our ref in case it has changed or gone null
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
                JournalEntry.UpdateSyncFlagBit(Journalid, SyncFlags.EDSM, true );
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


    public class HistoryList : IEnumerable<HistoryEntry>
    {
        private List<HistoryEntry> historylist = new List<HistoryEntry>();  // oldest first here
        public Ledger cashledger { get; private set; } = new Ledger();       // and the ledger..
        public ShipInformationList shipinformationlist { get; private set; } = new ShipInformationList();     // ship info
        private MissionListAccumulator missionlistaccumulator = new MissionListAccumulator(); // and mission list..
        public StarScan starscan { get; private set; } = new StarScan();                                           // and the results of scanning
        public int CommanderId { get; private set; }

        private JournalFuelScoop FuelScoopAccum;        // no need to copy
        public static bool AccumulateFuelScoops { get; set; } = true;

        public HistoryList() { }

        public HistoryList(List<HistoryEntry> hl) { historylist = hl; }         // SPECIAL USE ONLY - DOES NOT COMPUTE ALL THE OTHER STUFF

        public void Copy( HistoryList other )       // Must copy all relevant items.. been caught out by this 23/6/2017
        {
            historylist.Clear();

            foreach (var ent in other.EntryOrder)
            {
                historylist.Add(ent);
                Debug.Assert(ent.MaterialCommodity != null);
            }

            cashledger = other.cashledger;
            starscan = other.starscan;
            shipinformationlist = other.shipinformationlist;
            CommanderId = other.CommanderId;
            missionlistaccumulator = other.missionlistaccumulator;
        }

        public int Count { get { return historylist.Count; } }

        #region Output filters and stats

        public List<HistoryEntry> FilterByNumber(int max)
        {
            return historylist.OrderByDescending(s => s.EventTimeUTC).Take(max).ToList();
        }

        public List<HistoryEntry> FilterToLastDock()
        {
            List<HistoryEntry> inorder = historylist.OrderByDescending(s => s.EventTimeUTC).ToList();
            int lastdockpos = inorder.FindIndex(x => !x.MultiPlayer && x.EntryType == JournalTypeEnum.Docked);
            if (lastdockpos >= 0)
                inorder = inorder.Take(lastdockpos + 1).ToList();
            return inorder;
        }

        public List<HistoryEntry> FilterByDate(TimeSpan days)
        {
            var oldestData = DateTime.UtcNow.Subtract(days);
            return (from systems in historylist where systems.EventTimeUTC >= oldestData orderby systems.EventTimeUTC descending select systems).ToList();
        }


        public List<HistoryEntry> EntryOrder
        {
            get
            {
                return historylist;
            }
        }

        public List<HistoryEntry> LastFirst
        {
            get
            {
                return historylist.OrderByDescending(s => s.Indexno).ToList();
            }
        }

        public List<HistoryEntry> OrderByDate
        {
            get
            {
                return historylist.OrderByDescending(s => s.EventTimeUTC).ToList();
            }
        }

        public List<HistoryEntry> FilterByNotEDSMSyncedAndFSD
        {
            get
            {
                return (from s in historylist where s.EdsmSync == false && s.IsFSDJump orderby s.EventTimeUTC ascending select s).ToList();
            }
        }

        public List<HistoryEntry> FilterByScanNotEDDNSynced
        {
            get
            {
                return (from s in historylist where s.EDDNSync == false && s.EntryType== JournalTypeEnum.Scan  orderby s.EventTimeUTC ascending select s).ToList();
            }
        }

        public List<HistoryEntry> FilterByScanNotEGOSynced
        {
            get
            {
                DateTime start2_3 = new DateTime(2017, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc);
                return (from s in historylist where s.EGOSync == false && s.EntryType == JournalTypeEnum.Scan && s.EventTimeUTC >= start2_3 orderby s.EventTimeUTC ascending select s).ToList();
            }
        }


        public List<HistoryEntry> FilterByFSDAndPosition
        {
            get
            {
                return (from s in historylist where s.IsFSDJump && s.System.HasCoordinate select s).ToList();
            }
        }


        public List<HistoryEntry> FilterByEDDCommodityPricesBackwards
        {
            get
            {
                return (from s in historylist where s.EntryType == JournalTypeEnum.EDDCommodityPrices orderby s.EventTimeUTC descending select s ).ToList();
            }
        }

        public List<HistoryEntry> FilterByTravel
        {
            get
            {
                List<HistoryEntry> ents = new List<HistoryEntry>();
                bool resurrect = true;
                foreach (HistoryEntry he in historylist)
                {
                    if (he.EntryType == JournalTypeEnum.Resurrect || he.EntryType == JournalTypeEnum.Died)
                    {
                        resurrect = true;
                        ents.Add(he);
                    }
                    else if ((resurrect && he.EntryType == JournalTypeEnum.Location) || he.EntryType == JournalTypeEnum.FSDJump)
                    {
                        resurrect = false;
                        ents.Add(he);
                    }
                }

                return ents;
            }
        }

        public List<HistoryEntry> FilterByFSD
        {
            get
            {
                return (from s in historylist where s.IsFSDJump select s).ToList();
            }
        }

        public HistoryEntry GetByJID(long jid)
        {
            return historylist.Find(x => x.Journalid == jid);
        }

        public int GetIndex(long jid)
        {
            return EntryOrder.FindIndex(x => x.Journalid == jid);
        }

        public HistoryEntry GetLast
        {
            get
            {
                if (historylist.Count > 0)
                    return historylist[historylist.Count - 1];
                else
                    return null;
            }
        }

        public HistoryEntry GetLastFSD
        {
            get
            {
                return historylist.FindLast(x => x.IsFSDJump);
            }
        }

        public HistoryEntry GetLastHistoryEntry(Predicate<HistoryEntry> where)
        {
            return historylist.FindLast(where);
        }

        public HistoryEntry GetLastHistoryEntry(Predicate<HistoryEntry> where, HistoryEntry frominclusive )
        {
            int hepos = historylist.FindIndex(x => x.Journalid == frominclusive.Journalid);
            if (hepos != -1)
                hepos = historylist.FindLastIndex(hepos, where);

            return (hepos != -1) ? historylist[hepos] : null;
        }

        public HistoryEntry GetLastWithPosition
        {
            get
            {
                return historylist.FindLast(x => x.System.HasCoordinate);
            }
        }

        public DateTime GetMaxDate
        {
            get
            {
                return historylist.Max(x => x.EventTimeUTC);
            }
        }
        public DateTime GetMinDate
        {
            get
            {
                return historylist.Min(x => x.EventTimeUTC);
            }
        }


        public int GetVisitsCount(string name, long edsmid = 0)
        {
            return (from he in historylist.AsParallel()
                   where (he.IsFSDJump && (edsmid <= 0 || he.System.id_edsm == edsmid) && he.System.name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                   select he).Count();
        }
        public List<JournalScan> GetScans(string name, long edsmid = 0)
        {
            return (from s in historylist.AsParallel()
                    where (s.journalEntry.EventTypeID == JournalTypeEnum.Scan && (edsmid <= 0 || s.System.id_edsm == edsmid) && s.System.name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    select s.journalEntry as JournalScan).ToList<JournalScan>();
        }

        public int GetFSDJumps( TimeSpan t )
        {
            DateTime tme = DateTime.UtcNow.Subtract(t);
            return (from s in historylist where s.IsFSDJump && s.EventTimeUTC>=tme select s).Count();
        }

        public int GetFSDJumps(DateTime start, DateTime to)
        {
            return (from s in historylist where s.IsFSDJump && s.EventTimeLocal >= start && s.EventTimeLocal<to  select s).Count();
        }

        public int GetNrScans(DateTime start, DateTime to)
        {
            return (from s in historylist where s.journalEntry.EventTypeID == JournalTypeEnum.Scan && s.EventTimeLocal >= start && s.EventTimeLocal < to select s).Count();
        }

        public int GetScanValue(DateTime start, DateTime to)
        {
            var list = (from s in historylist where s.EntryType == JournalTypeEnum.Scan && s.EventTimeLocal >= start && s.EventTimeLocal < to select s.journalEntry as JournalScan).ToList<JournalScan>();

            return (from t in list select t.EstimatedValue()).Sum();
        }

        public int GetDocked(DateTime start, DateTime to)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.Docked && s.EventTimeLocal >= start && s.EventTimeLocal < to select s).Count();
        }

        public int GetJetConeBoost(DateTime start, DateTime to)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.JetConeBoost && s.EventTimeLocal >= start && s.EventTimeLocal < to select s).Count();
        }

        public int GetFSDBoostUsed(DateTime start, DateTime to)
        {
            return (from s in historylist
                    where (s.EntryType == JournalTypeEnum.FSDJump && s.EventTimeLocal >= start && s.EventTimeLocal < to && ((JournalFSDJump)s.journalEntry).BoostUsed == true)
                    select s).Count();
        }



        public int GetTouchDown(DateTime start, DateTime to)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.Touchdown && s.EventTimeLocal >= start && s.EventTimeLocal < to select s).Count();
        }

        public int GetHeatWarning(DateTime start, DateTime to)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.HeatWarning && s.EventTimeLocal >= start && s.EventTimeLocal < to select s).Count();
        }


        public int GetHeatDamage(DateTime start, DateTime to)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.HeatDamage && s.EventTimeLocal >= start && s.EventTimeLocal < to select s).Count();
        }

        public int GetFuelScooped(DateTime start, DateTime to)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.FuelScoop && s.EventTimeLocal >= start && s.EventTimeLocal < to select s).Count();
        }

        public double GetFuelScoopedTons(DateTime start, DateTime to)
        {
             var list = (from s in historylist where s.EntryType == JournalTypeEnum.FuelScoop && s.EventTimeLocal >= start && s.EventTimeLocal < to select s.journalEntry as JournalFuelScoop).ToList<JournalFuelScoop>();

            return (from s in list select s.Scooped).Sum();
        }

        public double GetTraveledLy(DateTime start, DateTime to)
        {
            var list = (from s in historylist where s.EntryType == JournalTypeEnum.FSDJump && s.EventTimeLocal >= start && s.EventTimeLocal < to select s.journalEntry as JournalFSDJump).ToList<JournalFSDJump>();

            return (from s in list select s.JumpDist).Sum();
        }


        public List<JournalScan> GetScanList(DateTime start, DateTime to)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.Scan && s.EventTimeLocal >= start && s.EventTimeLocal < to select s.journalEntry as JournalScan).ToList<JournalScan>();
        }

        public int GetFSDJumpsBeforeUTC(DateTime utc)
        {
            return (from s in historylist where s.IsFSDJump && s.EventTimeLocal < utc select s).Count();
        }

        public delegate bool FurthestFund(HistoryEntry he, ref double lastv);
        public HistoryEntry GetConditionally( double lastv, FurthestFund f )              // give a comparision function, find entry
        {
            HistoryEntry best = null;
            foreach( HistoryEntry s in historylist )
            {
                if (f(s, ref lastv))
                    best = s;
            }

            return best;
        }

        public static HistoryEntry FindLastFSDKnownPosition(List<HistoryEntry> syslist)
        {
            return syslist.FindLast(x => x.System.HasCoordinate && x.IsLocOrJump);
        }

        public static HistoryEntry FindByPos(List<HistoryEntry> syslist, float x, float y, float z, double limit)     // go thru setting the lastknowsystem
        {
            return syslist.FindLast(s => s.System.HasCoordinate &&
                                            Math.Abs(s.System.x - x) < limit &&
                                            Math.Abs(s.System.y - y) < limit &&
                                            Math.Abs(s.System.z - z) < limit);
        }

        public static List<HistoryEntry> FilterByJournalEvent(List<HistoryEntry> he, string eventstring, out int count)
        {
            count = 0;
            if (eventstring.Equals("All"))
                return he;
            else
            {
                string[] events = eventstring.Split(';');
                List<HistoryEntry> ret = (from systems in he where systems.IsJournalEventInEventFilter(events) select systems).ToList();
                count = he.Count - ret.Count;
                return ret;
            }
        }


        #endregion

        #region EDSM

        public void FillInPositionsFSDJumps()       // call if you want to ensure we have the best posibile position data on FSD Jumps.  Only occurs on pre 2.1 with lazy load of just name/edsmid
        {
            List<Tuple<HistoryEntry, SystemClassDB>> updatesystems = new List<Tuple<HistoryEntry, SystemClassDB>>();

            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
            {
                foreach (HistoryEntry he in historylist)
                {
                    if (he.IsFSDJump && !he.System.HasCoordinate)   // try and load ones without position.. if its got pos we are happy
                        updatesystems.Add(new Tuple<HistoryEntry, SystemClassDB>(he, FindEDSM(he)));
                }
            }

            foreach (Tuple<HistoryEntry, SystemClassDB> he in updatesystems)
            {
                FillEDSM(he.Item1, edsmsys: he.Item2);  // fill, we already have an EDSM system to use
            }
        }

        private SystemClassDB FindEDSM(HistoryEntry syspos, SQLiteConnectionSystem conn = null, bool reload = false)
        {
            if (syspos.System.status == SystemStatusEnum.EDSC || (!reload && syspos.System.id_edsm == -1))  // if set already, or we tried and failed..
                return null;

            bool ownconn = false;

            try
            {
                if (conn == null)
                {
                    ownconn = true;
                    conn = new SQLiteConnectionSystem();
                }

                return SystemClassDB.FindEDSM(syspos.System, conn);
            }
            finally
            {
                if (ownconn && conn != null)
                {
                    conn.Dispose();
                }
            }
        }

        public void FillEDSM(HistoryEntry syspos, SystemClassDB edsmsys = null, bool reload = false, SQLiteConnectionUser uconn = null )       // call to fill in ESDM data for entry, and also fills in all others pointing to the system object
        {
            if (syspos.System.status == SystemStatusEnum.EDSC || (!reload && syspos.System.id_edsm == -1) )  // if set already, or we tried and failed..
                return;

            List<HistoryEntry> alsomatching = new List<HistoryEntry>();

            foreach (HistoryEntry he in historylist)       // list of systems in historylist using the same system object
            {
                if (Object.ReferenceEquals(he.System, syspos.System))
                    alsomatching.Add(he);
            }

            if (edsmsys==null)                              // if we found it externally, do not find again
                edsmsys = FindEDSM(syspos, reload: reload);

            if (edsmsys != null)
            {
                foreach (HistoryEntry he in alsomatching)       // list of systems in historylist using the same system object
                {
                    bool updateedsmid = he.System.id_edsm <= 0;
                    bool updatepos = (he.EntryType == JournalTypeEnum.FSDJump || he.EntryType == JournalTypeEnum.Location) && !syspos.System.HasCoordinate && edsmsys.HasCoordinate;

                    if (updatepos || updateedsmid)
                        JournalEntry.UpdateEDSMIDPosJump(he.Journalid, edsmsys, updatepos, -1 , uconn);  // update pos and edsmid, jdist not updated

                    he.System = edsmsys;
                }
            }
            else
            {
                foreach (HistoryEntry he in alsomatching)       // list of systems in historylist using the same system object
                    he.System.id_edsm = -1;                     // can't do it
            }
        }

        #endregion

        #region General Info

        public void CalculateSqDistances(SortedList<double, ISystem> distlist, double x, double y, double z, int maxitems, bool removezerodiststar)
        {
            double dist;
            double dx, dy, dz;
            var list = distlist.Values.ToList();

            foreach (HistoryEntry pos in historylist)
            {
                if (pos.System.HasCoordinate && !list.Any(qx => (qx.id == pos.System.id || qx.name.Equals(pos.System.name, StringComparison.InvariantCultureIgnoreCase))))
                {
                    dx = (pos.System.x - x);
                    dy = (pos.System.y - y);
                    dz = (pos.System.z - z);
                    dist = dx * dx + dy * dy + dz * dz;

                    list.Add(pos.System);

                    if (dist >= 0.1 || !removezerodiststar)
                    {
                        if (distlist.Count < maxitems)          // if less than max, add..
                            distlist.Add(dist, pos.System);
                        else if (dist < distlist.Last().Key)   // if last entry (which must be the biggest) is greater than dist..
                        {
                            distlist.Add(dist, pos.System);           // add in
                            distlist.RemoveAt(maxitems);        // remove last..
                        }
                    }
                }
            }
        }

        public HistoryEntry FindByName(string name, bool fsdjump = false)
        {
            if (fsdjump)
                return historylist.FindLast(x => x.System.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            else
                return historylist.FindLast(x => x.IsFSDJump && x.System.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public ISystem FindSystem(string name, EDSM.GalacticMapping glist = null)        // in system or name
        {
            ISystem ds1 = SystemClassDB.GetSystem(name);

            if (ds1 == null)
            {
                HistoryEntry vs = FindByName(name);

                if (vs != null)
                    ds1 = vs.System;
                else if (glist != null)
                {
                    EDSM.GalacticMapObject gmo = glist.Find(name, true, true);

                    if (gmo != null && gmo.points.Count > 0)
                    {
                        ds1 = SystemClassDB.GetSystem(gmo.galMapSearch);

                        if (ds1 != null)
                        {
                            return new EDSM.GalacticMapSystem(ds1, gmo);
                        }
                        else
                        {
                            return new EDSM.GalacticMapSystem(gmo);
                        }
                    }
                }
            }

            return ds1;
        }

        public static HistoryEntry FindNextSystem(List<HistoryEntry> syslist, string sysname, int dir)
        {
            int index = syslist.FindIndex(x => x.System.name.Equals(sysname));

            if (index != -1)
            {
                if (dir == -1)
                {
                    if (index < 1)                                  //0, we go to the end and work from back..
                        index = syslist.Count;

                    int indexn = syslist.FindLastIndex(index - 1, x => x.System.HasCoordinate);

                    if (indexn == -1)                             // from where we were, did not find one, try from back..
                        indexn = syslist.FindLastIndex(x => x.System.HasCoordinate);

                    return (indexn != -1) ? syslist[indexn] : null;
                }
                else
                {
                    index++;

                    if (index == syslist.Count)             // if at end, go to beginning
                        index = 0;

                    int indexn = syslist.FindIndex(index, x => x.System.HasCoordinate);

                    if (indexn == -1)                             // if not found, go to beginning
                        indexn = syslist.FindIndex(x => x.System.HasCoordinate);

                    return (indexn != -1) ? syslist[indexn] : null;
                }
            }
            else
            {
                index = syslist.FindLastIndex(x => x.System.HasCoordinate);
                return (index != -1) ? syslist[index] : null;
            }
        }


        public HistoryEntry PreviousFrom(HistoryEntry e, bool fsdjumps)
        {
            if (e != null)
            {
                int curindex = historylist.IndexOf(e);

                if (curindex > 0)       // no point with index=0, there is no last.
                {
                    int lastindex = historylist.FindLastIndex(curindex - 1, x => (fsdjumps) ? x.IsFSDJump : true);

                    if (lastindex >= 0)
                        return historylist[lastindex];
                }
            }

            return null;
        }

        #endregion
        
        #region Enumeration

        public IEnumerator<HistoryEntry> GetEnumerator()
        {
            foreach (var e in historylist)
            {
                yield return e;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Markers

        public void SetStartStop( HistoryEntry hs )
        {
            bool started = false;

            foreach ( HistoryEntry he in historylist )
            {
                if (hs == he)
                {
                    if (he.StartMarker)
                    {
                        JournalEntry.UpdateSyncFlagBit(hs.Journalid, SyncFlags.StartMarker, false);
                        he.StartMarker = false;
                    }
                    else if (he.StopMarker)
                    {
                        JournalEntry.UpdateSyncFlagBit(hs.Journalid, SyncFlags.StopMarker, false);
                        he.StopMarker = false;
                    }
                    else if (started == false)
                    {
                        JournalEntry.UpdateSyncFlagBit(hs.Journalid, SyncFlags.StartMarker, true);
                        he.StartMarker = true;
                    }
                    else
                    {
                        JournalEntry.UpdateSyncFlagBit(hs.Journalid, SyncFlags.StopMarker, true);
                        he.StopMarker = true;
                    }

                    break;
                }
                else if (he.StartMarker)
                    started = true;
                else if (he.StopMarker)
                    started = false;
            }
        }

        #endregion

        #region Entry processing

        // go through the history list and recalculate the materials ledger and the materials count, plus any other stuff..
        public void ProcessUserHistoryListEntries(Func<HistoryList, List<HistoryEntry>> hlfilter)
        {
            List<HistoryEntry> hl = hlfilter(this);

            using (SQLiteConnectionUser conn = new SQLiteConnectionUser())      // splitting the update into two, one using system, one using user helped
            {
                for (int i = 0; i < hl.Count; i++)
                {
                    HistoryEntry he = hl[i];
                    JournalEntry je = he.journalEntry;
                    he.ProcessWithUserDb(je, (i > 0) ? hl[i - 1] : null, this, conn);        // let the HE do what it wants to with the user db

                    Debug.Assert(he.MaterialCommodity != null);

                    // **** REMEMBER NEW Journal entry needs this too *****************

                    cashledger.Process(je, conn);            // update the ledger     

                    Tuple<ShipInformation,ModulesInStore> ret = shipinformationlist.Process(je, conn);  // the ships
                    he.ShipInformation = ret.Item1;
                    he.StoredModules = ret.Item2;

                    he.MissionList = missionlistaccumulator.Process(je, he.System, he.WhereAmI, conn);                           // the missions

                    if (je.EventTypeID == JournalTypeEnum.Scan)
                    {
                        if (!this.starscan.AddScanToBestSystem(je as JournalScan, i, hl))
                        {
                            System.Diagnostics.Debug.WriteLine("******** Cannot add scan to system " + (je as JournalScan).BodyName + " in " + he.System.name);
                        }
                    }
                }
            }
        }

        private IEnumerable<JournalEntry> ProcessJournalEntry(JournalEntry je)
        {
            if (je.EventTypeID == JournalTypeEnum.FuelScoop)
            {
                JournalFuelScoop scoop = je as JournalFuelScoop;
                if (scoop != null)
                {
                    if (scoop.Scooped >= 5.0)
                    {
                        if (FuelScoopAccum == null)
                        {
                            FuelScoopAccum = new JournalFuelScoop(je.GetJson());
                            yield break;
                        }
                        else
                        {
                            FuelScoopAccum.Id = scoop.Id;
                            FuelScoopAccum.TLUId = scoop.TLUId;
                            FuelScoopAccum.CommanderId = scoop.CommanderId;
                            FuelScoopAccum.EdsmID = scoop.EdsmID;
                            FuelScoopAccum.EventTimeUTC = scoop.EventTimeUTC;
                            FuelScoopAccum.Scooped += scoop.Scooped;
                            FuelScoopAccum.Total = scoop.Total;
                            yield break;
                        }
                    }
                    else if (FuelScoopAccum != null)
                    {
                        scoop.Scooped += FuelScoopAccum.Scooped;
                        FuelScoopAccum = null;
                    }
                }
            }

            if (FuelScoopAccum != null)
            {
                yield return FuelScoopAccum;
                FuelScoopAccum = null;
            }

            yield return je;
        }

        // Called on a New Entry, by EDDiscoveryController:NewEntry, to add an journal entry in

        public IEnumerable<HistoryEntry> AddJournalEntry(JournalEntry inje, Action<string> logerror)
        {
            if (inje.CommanderId == CommanderId)     // we are only interested at this point accepting ones for the display commander
            {
                HistoryEntry last = GetLast;

                foreach (JournalEntry je in ProcessJournalEntry(inje))
                {
                    bool journalupdate = false;
                    HistoryEntry he = HistoryEntry.FromJournalEntry(je, last, true, out journalupdate);

                    if (journalupdate)
                    {
                        JournalFSDJump jfsd = je as JournalFSDJump;

                        if (jfsd != null)
                        {
                            JournalEntry.UpdateEDSMIDPosJump(jfsd.Id, he.System, !jfsd.HasCoordinate && he.System.HasCoordinate, jfsd.JumpDist);
                        }
                    }

                    using (SQLiteConnectionUser conn = new SQLiteConnectionUser())
                    {
                        he.ProcessWithUserDb(je, last, this, conn);           // let some processes which need the user db to work

                        cashledger.Process(je, conn);

                        Tuple<ShipInformation, ModulesInStore> ret = shipinformationlist.Process(je, conn);
                        he.ShipInformation = ret.Item1;
                        he.StoredModules = ret.Item2;

                        he.MissionList = missionlistaccumulator.Process(je, he.System, he.WhereAmI, conn);
                    }

                    historylist.Add(he);

                    if (je.EventTypeID == JournalTypeEnum.Scan)
                    {
                        JournalScan js = je as JournalScan;
                        JournalLocOrJump jl;
                        HistoryEntry jlhe;
                        if (!starscan.AddScanToBestSystem(js, Count - 1, EntryOrder, out jlhe, out jl))
                        {
                            // Ignore scans where the system name has been changed
                            // Also ignore belt clusters
                            if (jl == null || (jl.StarSystem.Equals(jlhe.System.name, StringComparison.InvariantCultureIgnoreCase) && !js.BodyDesignation.ToLowerInvariant().Contains(" belt cluster ")))
                            {
                                logerror("Cannot add scan to system - alert the EDDiscovery developers using either discord or Github (see help)" + Environment.NewLine +
                                                 "Scan object " + js.BodyName + " in " + he.System.name);
                            }
                        }
                    }

                    yield return he;

                    last = he;
                }
            }
        }

        public void SendEDSMStatusInfo(HistoryEntry he, bool async)     // he points to ship info to send from..
        {
            if (CommanderId >= 0)
            {
                var commander = EDCommander.GetCommander(CommanderId);

                string edsmname = commander.Name;
                if (!string.IsNullOrEmpty(commander.EdsmName))
                    edsmname = commander.EdsmName;

                if (!commander.SyncToEdsm || string.IsNullOrEmpty(commander.APIKey) || string.IsNullOrEmpty(edsmname))
                    return;

                EDSMClass edsm = new EDSMClass { apiKey = commander.APIKey, commanderName = edsmname };

                // find last ship info currently
                HistoryEntry lastshipinfocurrenthe = GetLastHistoryEntry(x => x.ShipInformation != null && x.ShipInformation.SubVehicle == ShipInformation.SubVehicleType.None);

                // based on he position, find one before it with ship info that is a normal si not a srv
                HistoryEntry lastshipinfohe = GetLastHistoryEntry(x => x.ShipInformation != null && x.ShipInformation.SubVehicle == ShipInformation.SubVehicleType.None, he);

                long loan = 0;

                if ( lastshipinfohe != null)       // we have a ship info
                {
                    // and based on that position, find a last load game.  May be null
                    HistoryEntry lastloadgamehe = GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.LoadGame, lastshipinfohe);
                    loan = (lastloadgamehe != null) ? ((JournalLoadGame)lastloadgamehe.journalEntry).Loan : 0;
                }

                JournalProgress progress = historylist.FindLast(x => x.EntryType == JournalTypeEnum.Progress).journalEntry as JournalProgress;
                JournalRank rank = historylist.FindLast(x => x.EntryType == JournalTypeEnum.Rank).journalEntry as JournalRank;

                if (async)
                {
                    Task edsmtask = Task.Factory.StartNew(() =>
                    {
                        edsm.SendShipInfo(lastshipinfohe.ShipInformation, lastshipinfohe.MaterialCommodity.CargoCount, lastshipinfocurrenthe.ShipInformation, cashledger.CashTotal, loan, progress, rank);
                    });
                }
                else
                {
                    edsm.SendShipInfo(lastshipinfohe.ShipInformation, lastshipinfohe.MaterialCommodity.CargoCount, lastshipinfocurrenthe.ShipInformation, cashledger.CashTotal, loan, progress, rank);
                }
            }
        }

        public static HistoryList LoadHistory(EDJournalClass journalmonitor, Func<bool> cancelRequested, Action<int, string> reportProgress, string NetLogPath = null, bool ForceNetLogReload = false, bool ForceJournalReload = false, bool CheckEdsm = false, int CurrentCommander = Int32.MinValue)
        {
            HistoryList hist = new HistoryList();
            EDCommander cmdr = null;

            if (CurrentCommander >= 0)
            {
                cmdr = EDCommander.GetCommander(CurrentCommander);
                journalmonitor.ParseJournalFiles(() => cancelRequested(), (p, s) => reportProgress(p, s), forceReload: ForceJournalReload);   // Parse files stop monitor..

                if (NetLogPath != null)
                {
                    string errstr = null;
                    NetLogClass.ParseFiles(NetLogPath, out errstr, EliteConfigInstance.InstanceConfig.DefaultMapColour, () => cancelRequested(), (p, s) => reportProgress(p, s), ForceNetLogReload, currentcmdrid: CurrentCommander);
                }
            }

            reportProgress(-1, "Resolving systems");

            List<JournalEntry> jlist = JournalEntry.GetAll(CurrentCommander).OrderBy(x => x.EventTimeUTC).ThenBy(x => x.Id).ToList();
            List<Tuple<JournalEntry, HistoryEntry>> jlistUpdated = new List<Tuple<JournalEntry, HistoryEntry>>();

            using (SQLiteConnectionSystem conn = new SQLiteConnectionSystem())
            {
                HistoryEntry prev = null;
                foreach (JournalEntry inje in jlist)
                {
                    foreach (JournalEntry je in hist.ProcessJournalEntry(inje))
                    {
                        bool journalupdate = false;
                        HistoryEntry he = HistoryEntry.FromJournalEntry(je, prev, CheckEdsm, out journalupdate, conn, cmdr);

                        prev = he;

                        hist.historylist.Add(he);

                        if (journalupdate)
                        {
                            jlistUpdated.Add(new Tuple<JournalEntry, HistoryEntry>(je, he));
                        }
                    }
                }
            }

            if (jlistUpdated.Count > 0)
            {
                reportProgress(-1, "Updating journal entries");

                using (SQLiteConnectionUser conn = new SQLiteConnectionUser(utc: true))
                {
                    using (DbTransaction txn = conn.BeginTransaction())
                    {
                        foreach (Tuple<JournalEntry, HistoryEntry> jehe in jlistUpdated)
                        {
                            JournalEntry je = jehe.Item1;
                            HistoryEntry he = jehe.Item2;
                            JournalFSDJump jfsd = je as JournalFSDJump;
                            if (jfsd != null)
                            {
                                JournalEntry.UpdateEDSMIDPosJump(jfsd.Id, he.System, !jfsd.HasCoordinate && he.System.HasCoordinate, jfsd.JumpDist, conn, txn);
                            }
                        }

                        txn.Commit();
                    }
                }
            }

            // now database has been updated due to initial fill, now fill in stuff which needs the user database

            hist.CommanderId = CurrentCommander;

            hist.ProcessUserHistoryListEntries(h => h.ToList());      // here, we update the DBs in HistoryEntry and any global DBs in historylist

            hist.SendEDSMStatusInfo(hist.GetLast, true);

            return hist;
        }

        #endregion
    }
}
