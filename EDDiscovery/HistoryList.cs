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
using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery.EDSM;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EDDiscovery
{
    [DebuggerDisplay("Event {EntryType} {System.name} ({System.x,nq},{System.y,nq},{System.z,nq}) {EventTimeUTC} JID:{Journalid}")]
    public class HistoryEntry           // DONT store commander ID.. this history is externally filtered on it.
    {
        #region Variables

        public int Indexno;            // for display purposes.

        public EliteDangerous.JournalTypeEnum EntryType;
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
        public string EventSummary;
        public string EventDescription;
        public string EventDetailedInfo;

        public int MapColour;

        public bool IsStarPosFromEDSM;  // flag populated from journal entry when HE is made. Was the star position taken from EDSM?
        public bool IsEDSMFirstDiscover;// flag populated from journal entry when HE is made. Were we the first to report the system to EDSM?
        public bool EdsmSync;           // flag populated from journal entry when HE is made. Have we synced?
        public bool EDDNSync;           // flag populated from journal entry when HE is made. Have we synced?
        public bool StartMarker;        // flag populated from journal entry when HE is made. Is this a system distance measurement system
        public bool StopMarker;         // flag populated from journal entry when HE is made. Is this a system distance measurement stop point
        public bool IsFSDJump { get { return EntryType == EliteDangerous.JournalTypeEnum.FSDJump; } }
        public bool IsLocOrJump { get { return EntryType == JournalTypeEnum.FSDJump || EntryType == JournalTypeEnum.Location; } }
        public bool IsFuelScoop { get { return EntryType == JournalTypeEnum.FuelScoop; } }
        public bool ISEDDNMessage
        {
            get
            {
                DateTime ed22 = new DateTime(2016, 10, 25, 12, 0, 0);
                if ((EntryType == JournalTypeEnum.Scan || EntryType == JournalTypeEnum.Docked || EntryType == JournalTypeEnum.FSDJump) && EventTimeUTC>ed22 ) return true; else return false;
            }
        }

        public MaterialCommoditiesList MaterialCommodity { get { return materialscommodities; } }
        public ShipInformation ShipInformation { get { return shipmodules; } set { shipmodules = value; } }     // may be null if not set up yet
        public ModulesInStore StoredModules { get { return storedmodules; } set { storedmodules = value; } }
        public MissionList MissionList { get { return missionlist; } set { missionlist = value; } }
        
        // Calculated values, not from JE

        public SystemNoteClass snc;     // system note class found attached to this entry

        private double travelled_distance;  // start/stop distance and time computation
        public double TravelledDistance { get { return travelled_distance; } }

        private TimeSpan travelled_seconds;
        public TimeSpan TravelledSeconds { get { return travelled_seconds; } }

        bool travelling;
        public bool isTravelling { get { return travelling; } }

        int travelled_missingjump;
        public int TravelledMissingjump { get { return travelled_missingjump; } }
        public int Travelledjumps { get { return travelled_jumps; } }
        int travelled_jumps;

        MaterialCommoditiesList materialscommodities;
        ShipInformation shipmodules;
        ModulesInStore storedmodules;
        MissionList missionlist;                    // mission state at this point..

        private bool? docked;                       // are we docked.  Null if don't know, else true/false
        private bool? landed;                       // are we landed on the planet surface.  Null if don't know, else true/false
        private string whereami = "";               // where we think we are
        private int shipid = -1;                            // ship id, -1 unknown
        private string shiptype = "Unknown";        // and the ship

        public bool IsLanded { get { return landed.HasValue && landed.Value == true; } }
        public bool IsDocked { get { return docked.HasValue && docked.Value == true; } }
        public string WhereAmI { get { return whereami; } }
        public string ShipType { get { return shiptype; } }
        public int ShipId { get { return shipid; } }

        #endregion

        #region Constructors

        private HistoryEntry()
        {

        }

        public static HistoryEntry MakeVSEntry(ISystem sys, DateTime eventt, int m, string dist, string info, int journalid = 0, bool firstdiscover = false)
        {
            Debug.Assert(sys != null);
            return new HistoryEntry
            {
                EntryType = EliteDangerous.JournalTypeEnum.FSDJump,
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

        public static HistoryEntry FromJournalEntry(EliteDangerous.JournalEntry je, HistoryEntry prev, bool checkedsm, out bool journalupdate, SQLiteConnectionSystem conn = null, EDCommander cmdr = null)
        {
            ISystem isys = prev == null ? new SystemClass("Unknown") : prev.System;
            int indexno = prev == null ? 1 : prev.Indexno + 1;

            int mapcolour = 0;
            journalupdate = false;
            bool starposfromedsm = false;
            bool firstdiscover = false;


            if (je.EventTypeID == EliteDangerous.JournalTypeEnum.Location || je.EventTypeID == EliteDangerous.JournalTypeEnum.FSDJump)
            {
                EDDiscovery.EliteDangerous.JournalEvents.JournalLocOrJump jl = je as EDDiscovery.EliteDangerous.JournalEvents.JournalLocOrJump;
                EDDiscovery.EliteDangerous.JournalEvents.JournalFSDJump jfsd = je as EDDiscovery.EliteDangerous.JournalEvents.JournalFSDJump;

                ISystem newsys;

                if (jl.HasCoordinate)       // LAZY LOAD IF it has a co-ord.. the front end will when it needs it
                {
                    newsys = new SystemClass(jl.StarSystem, jl.StarPos.X, jl.StarPos.Y, jl.StarPos.Z);
                    newsys.id_edsm = jl.EdsmID < 0 ? 0 : jl.EdsmID;       // pass across the EDSMID for the lazy load process.

                    if (jfsd != null && jfsd.JumpDist <= 0 && isys.HasCoordinate)     // if we don't have a jump distance (pre 2.2) but the last sys does have pos, we can compute distance and update entry
                    {
                        jfsd.JumpDist = SystemClass.Distance(isys, newsys); // fill it out here
                        journalupdate = true;
                    }

                }
                else
                {                           // Default one
                    newsys = new SystemClass(jl.StarSystem);
                    newsys.id_edsm = jl.EdsmID;

                    if (checkedsm)          // see if we can find the right system
                    {
                        SystemClass s = SystemClass.FindEDSM(newsys, conn);      // has no co-ord, did we find it?

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
                                jfsd.JumpDist = SystemClass.Distance(isys, newsys); // fill it out here.  EDSM systems always have co-ords, but we should check anyway
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
                        jfsd.JumpDist = SystemClass.Distance(isys, newsys); // fill it out here
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
                StartMarker = je.StartMarker,
                StopMarker = je.StopMarker,
                EventSummary = summary,
                EventDescription = info,
                EventDetailedInfo = detailed,
                IsStarPosFromEDSM = starposfromedsm,
                IsEDSMFirstDiscover = firstdiscover,
                Commander = cmdr ?? EDCommander.GetCommander(je.CommanderId)
            };

            if (prev != null && prev.travelling)      // if we are travelling..
            {
                he.travelled_distance = prev.travelled_distance;
                he.travelled_missingjump = prev.travelled_missingjump;
                he.travelled_jumps = prev.travelled_jumps;

                if (he.IsFSDJump)
                {
                    double dist = ((EliteDangerous.JournalEvents.JournalFSDJump)je).JumpDist;
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

                if (he.EntryType != EliteDangerous.JournalTypeEnum.LoadGame && diff < new TimeSpan(2, 0, 0))   // time between last entry and load game is not real time
                {
                    he.travelled_seconds += diff;
                }

                if (he.StopMarker || he.StartMarker)
                {
                    he.travelling = false;
                    he.EventDetailedInfo += ((he.EventDetailedInfo.Length > 0) ? Environment.NewLine : "") + "Travelled " + he.travelled_distance.ToString("0.0") + " LY"
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
                he.travelling = true;

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
            }

            if (je.EventTypeID == JournalTypeEnum.Location)
            {
                EliteDangerous.JournalEvents.JournalLocation jl = je as EliteDangerous.JournalEvents.JournalLocation;
                he.docked = jl.Docked;
                he.whereami = jl.Docked ? jl.StationName : jl.Body;
            }
            else if (je.EventTypeID == JournalTypeEnum.Docked)
            {
                EliteDangerous.JournalEvents.JournalDocked jl = je as EliteDangerous.JournalEvents.JournalDocked;
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
                he.whereami = (je as EliteDangerous.JournalEvents.JournalSupercruiseEntry).StarSystem;
            else if (je.EventTypeID == JournalTypeEnum.SupercruiseExit)
                he.whereami = (je as EliteDangerous.JournalEvents.JournalSupercruiseExit).Body;
            else if (je.EventTypeID == JournalTypeEnum.FSDJump)
                he.whereami = (je as EliteDangerous.JournalEvents.JournalFSDJump).StarSystem;
            else if (je.EventTypeID == JournalTypeEnum.LoadGame)
            {
                EliteDangerous.JournalEvents.JournalLoadGame jl = je as EliteDangerous.JournalEvents.JournalLoadGame;

                he.landed = jl.StartLanded;

                if (jl.Ship.IndexOf("buggy", StringComparison.InvariantCultureIgnoreCase) == -1)        // load game with buggy, can't tell what ship we get back into, so ignore
                {
                    he.shiptype = (je as EliteDangerous.JournalEvents.JournalLoadGame).Ship;
                    he.shipid = (je as EliteDangerous.JournalEvents.JournalLoadGame).ShipId;
                }
            }
            else if (je.EventTypeID == JournalTypeEnum.ShipyardBuy)         // BUY does not have ship id, but the new entry will that is written later - journals 8.34
                he.shiptype = (je as EliteDangerous.JournalEvents.JournalShipyardBuy).ShipType;
            else if (je.EventTypeID == JournalTypeEnum.ShipyardNew)
            {
                he.shiptype = (je as EliteDangerous.JournalEvents.JournalShipyardNew).ShipType;
                he.shipid = (je as EliteDangerous.JournalEvents.JournalShipyardNew).ShipId;
            }
            else if (je.EventTypeID == JournalTypeEnum.ShipyardSwap)
            {
                he.shiptype = (je as EliteDangerous.JournalEvents.JournalShipyardSwap).ShipType;
                he.shipid = (je as EliteDangerous.JournalEvents.JournalShipyardSwap).ShipId;
            }

            return he;
        }

        public void ProcessWithUserDb(EliteDangerous.JournalEntry je, HistoryEntry prev, HistoryList hl , SQLiteConnectionUser conn )      // called after above with a USER connection
        {
            materialscommodities = MaterialCommoditiesList.Process(je, prev?.materialscommodities, conn, EDDiscoveryForm.EDDConfig.ClearMaterials, EDDiscoveryForm.EDDConfig.ClearCommodities);

            snc = SystemNoteClass.GetNoteOnJournalEntry(Journalid);

            if (snc == null && IsFSDJump)
            {
                snc = SystemNoteClass.GetNoteOnSystem(System.name, System.id_edsm);

                if ( snc != null )      // if found..
                {
                    if ( System.id_edsm > 0 && snc.EdsmId <= 0 )    // if we have a system id, but snc not set, update it for next time.
                    {
                        snc.EdsmId = System.id_edsm;
                        snc.Update(conn);
                    }
                }
            }
        }

        #endregion

        public System.Drawing.Bitmap GetIcon
        {
            get
            {
                if (journalEntry != null)
                    return journalEntry.Icon;
                else if (EntryType == JournalTypeEnum.FSDJump)
                    return EDDiscovery.Properties.Resources.hyperspace;
                else
                    return EDDiscovery.Properties.Resources.genericevent;
            }
        }


        public void UpdateMapColour(int v)
        {
            if (EntryType == EliteDangerous.JournalTypeEnum.FSDJump)
            {
                MapColour = v;
                if (Journalid != 0)
                    EliteDangerous.JournalEntry.UpdateMapColour(Journalid, v);
            }
        }

        public void UpdateCommanderID(int v)
        {
            if (Journalid != 0)
            {
                EliteDangerous.JournalEntry.UpdateCommanderID(Journalid, v);
            }
        }

        public void SetEdsmSync()
        {
            EdsmSync = true;
            if (Journalid != 0)
            {
                EliteDangerous.JournalEntry.UpdateSyncFlagBit(Journalid, EliteDangerous.SyncFlags.EDSM, true );
            }
        }
        public void SetEddnSync()
        {
            EDDNSync = true;
            if (Journalid != 0)
            {
                EliteDangerous.JournalEntry.UpdateSyncFlagBit(Journalid, EliteDangerous.SyncFlags.EDDN, true);
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

        public bool UpdateSystemNote(string txt, bool commit = true)
        {
            if ((snc == null && txt.Length > 0) || (snc != null && !snc.Note.Equals(txt))) // if no system note, and text,  or system not is not text
            {
                if (snc != null && (snc.Journalid == Journalid || snc.Journalid == 0 || (snc.EdsmId > 0 && snc.EdsmId == System.id_edsm) || (snc.EdsmId <= 0 && snc.Name.Equals(System.name, StringComparison.InvariantCultureIgnoreCase))))           // already there, update
                {
                    snc.Note = txt;
                    snc.Time = DateTime.Now;
                    snc.Name = (IsFSDJump) ? System.name : "";
                    snc.Journalid = Journalid;
                    snc.EdsmId = IsFSDJump ? System.id_edsm : 0;

                    if (commit)
                        snc.Update();
                }
                else
                {
                    snc = new SystemNoteClass();
                    snc.Note = txt;
                    snc.Time = DateTime.Now;
                    snc.Name = IsFSDJump ? System.name : "";
                    snc.Journalid = Journalid;
                    snc.EdsmId = IsFSDJump ? System.id_edsm : 0;
                    snc.Add();
                }

                Debug.WriteLine("Store note {0} {1}", snc.Name, snc.EdsmId);

                return true;
            }

            return false;
        }

        public void CommitSystemNote()
        {
            snc.Update();
        }
    }


    public class HistoryList : IEnumerable<HistoryEntry>
    {
        private List<HistoryEntry> historylist = new List<HistoryEntry>();  // oldest first here

        private JournalFuelScoop FuelScoopAccum;

        public Ledger materialcommodititiesledger = new Ledger();       // and the ledger..
        public ShipInformationList shipinformationlist = new ShipInformationList();     // ship info
        public MissionListAccumulator missionlistaccumulator = new MissionListAccumulator(); // and mission list..

        public EliteDangerous.StarScan starscan = new StarScan();                                           // and the results of scanning

        public int CommanderId;

        public static bool AccumulateFuelScoops { get; set; } = true;
        public static int FuelScoopAccumPeriod { get; set; } = 10;

        public HistoryList() { }

        public HistoryList(List<HistoryEntry> hl) { historylist = hl; }

        public void Clear()
        {
            historylist.Clear();
        }

        public void Add(HistoryEntry e)
        {
            historylist.Add(e);
        }

        public List<HistoryEntry> FilterByNumber(int max)
        {
            return historylist.OrderByDescending(s => s.EventTimeUTC).Take(max).ToList();
        }

        public List<HistoryEntry> FilterByDate(TimeSpan days)
        {
            var oldestData = DateTime.UtcNow.Subtract(days);
            return (from systems in historylist where systems.EventTimeUTC >= oldestData orderby systems.EventTimeUTC descending select systems).ToList();
        }

        public int Count { get { return historylist.Count;  } }

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


        public List<HistoryEntry> FilterByFSDAndPosition
        {
            get
            {
                return (from s in historylist where s.IsFSDJump && s.System.HasCoordinate select s).ToList();
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

        public void FillInPositionsFSDJumps()       // call if you want to ensure we have the best posibile position data on FSD Jumps.  Only occurs on pre 2.1 with lazy load of just name/edsmid
        {
            List<Tuple<HistoryEntry, SystemClass>> updatesystems = new List<Tuple<HistoryEntry, SystemClass>>();

            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
            {
                foreach (HistoryEntry he in historylist)
                {
                    if (he.IsFSDJump && !he.System.HasCoordinate)   // try and load ones without position.. if its got pos we are happy
                        updatesystems.Add(new Tuple<HistoryEntry, SystemClass>(he, FindEDSM(he)));
                }
            }

            foreach (Tuple<HistoryEntry, SystemClass> he in updatesystems)
            {
                FillEDSM(he.Item1, edsmsys: he.Item2);  // fill, we already have an EDSM system to use
            }
        }

        private SystemClass FindEDSM(HistoryEntry syspos, SQLiteConnectionSystem conn = null, bool reload = false)
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

                return SystemClass.FindEDSM(syspos.System, conn);
            }
            finally
            {
                if (ownconn && conn != null)
                {
                    conn.Dispose();
                }
            }
        }

        public void FillEDSM(HistoryEntry syspos, SystemClass edsmsys = null, bool reload = false, SQLiteConnectionUser uconn = null )       // call to fill in ESDM data for entry, and also fills in all others pointing to the system object
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
                    bool updatepos = (he.EntryType == EliteDangerous.JournalTypeEnum.FSDJump || he.EntryType == EliteDangerous.JournalTypeEnum.Location) && !syspos.System.HasCoordinate && edsmsys.HasCoordinate;

                    if (updatepos || updateedsmid)
                        EliteDangerous.JournalEntry.UpdateEDSMIDPosJump(he.Journalid, edsmsys, updatepos, -1 , uconn);  // update pos and edsmid, jdist not updated

                    he.System = edsmsys;
                }
            }
            else
            {
                foreach (HistoryEntry he in alsomatching)       // list of systems in historylist using the same system object
                    he.System.id_edsm = -1;                     // can't do it
            }
        }

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
            ISystem ds1 = SystemClass.GetSystem(name);

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
                        return new SystemClass(gmo.name, gmo.points[0].X, gmo.points[0].Y, gmo.points[0].Z);        // fudge it into a system
                    }
                }
            }

            return ds1;
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

        public void SetStartStop( HistoryEntry hs )
        {
            bool started = false;

            foreach ( HistoryEntry he in historylist )
            {
                if (hs == he)
                {
                    if (he.StartMarker)
                    {
                        EliteDangerous.JournalEntry.UpdateSyncFlagBit(hs.Journalid, EliteDangerous.SyncFlags.StartMarker, false);
                        he.StartMarker = false;
                    }
                    else if (he.StopMarker)
                    {
                        EliteDangerous.JournalEntry.UpdateSyncFlagBit(hs.Journalid, EliteDangerous.SyncFlags.StopMarker, false);
                        he.StopMarker = false;
                    }
                    else if (started == false)
                    {
                        EliteDangerous.JournalEntry.UpdateSyncFlagBit(hs.Journalid, EliteDangerous.SyncFlags.StartMarker, true);
                        he.StartMarker = true;
                    }
                    else
                    {
                        EliteDangerous.JournalEntry.UpdateSyncFlagBit(hs.Journalid, EliteDangerous.SyncFlags.StopMarker, true);
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

                    materialcommodititiesledger.Process(je, conn);            // update the ledger     

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
                    if (scoop.Scooped == 5.0)
                    {
                        if (FuelScoopAccum == null)
                        {
                            FuelScoopAccum = new JournalFuelScoop(je.GetJson());
                            yield break;
                        }
                        else if (scoop.EventTimeUTC.Subtract(FuelScoopAccum.EventTimeUTC).TotalSeconds < FuelScoopAccumPeriod)
                        {
                            FuelScoopAccum.EventTimeUTC = scoop.EventTimeUTC;
                            FuelScoopAccum.Scooped += scoop.Scooped;
                            FuelScoopAccum.Total = scoop.Total;
                            yield break;
                        }
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
                        EliteDangerous.JournalEvents.JournalFSDJump jfsd = je as EliteDangerous.JournalEvents.JournalFSDJump;

                        if (jfsd != null)
                        {
                            EliteDangerous.JournalEntry.UpdateEDSMIDPosJump(jfsd.Id, he.System, !jfsd.HasCoordinate && he.System.HasCoordinate, jfsd.JumpDist);
                        }
                    }

                    using (SQLiteConnectionUser conn = new SQLiteConnectionUser())
                    {
                        he.ProcessWithUserDb(je, last, this, conn);           // let some processes which need the user db to work

                        materialcommodititiesledger.Process(je, conn);

                        Tuple<ShipInformation, ModulesInStore> ret = shipinformationlist.Process(je, conn);
                        he.ShipInformation = ret.Item1;
                        he.StoredModules = ret.Item2;

                        he.MissionList = missionlistaccumulator.Process(je, he.System, he.WhereAmI, conn);
                    }


                    Add(he);

                    if (je.EventTypeID == JournalTypeEnum.Scan)
                    {
                        JournalScan js = je as JournalScan;
                        JournalLocOrJump jl;
                        HistoryEntry jlhe;
                        if (!starscan.AddScanToBestSystem(js, Count - 1, EntryOrder, out jlhe, out jl))
                        {
                            // Ignore scans where the system name has been changed
                            if (jl == null || jl.StarSystem.Equals(jlhe.System.name, StringComparison.InvariantCultureIgnoreCase))
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



        static private DateTime LastEDSMAPiCommanderTime = DateTime.Now;
        static private ShipInformation LastShipInfo = null;
        static private long LastEDSMCredtis = -1;
        static private long LastShipID = -1;

        private void ProcessEDSMApiCommander()
        {
            try
            {
                if (this.CommanderId < 0)  // Only sync for real commander.
                    return;

                var commander = EDCommander.GetCommander(CommanderId);

                string edsmname = commander.Name;
                if (!string.IsNullOrEmpty(commander.EdsmName))
                    edsmname = commander.EdsmName;

                // check if we shall sync commander info to EDSM
                if (!commander.SyncToEdsm || string.IsNullOrEmpty(commander.APIKey) || string.IsNullOrEmpty(edsmname))
                    return;

                JournalProgress progress = historylist.FindLast(x => x.EntryType == JournalTypeEnum.Progress).journalEntry as JournalProgress;
                JournalRank rank = historylist.FindLast(x => x.EntryType == JournalTypeEnum.Rank).journalEntry as JournalRank;
                //= evt.journalEntry as JournalProgress;

                if (progress == null || rank == null)
                    return;

                EDSMClass edsm = new EDSMClass();

                edsm.apiKey = commander.APIKey;
                edsm.commanderName = edsmname;


                if (progress.EventTimeUTC != LastEDSMAPiCommanderTime) // Different from last sync with EDSM
                {
                    edsm.SetRanks((int)rank.Combat, progress.Combat, (int)rank.Trade, progress.Trade, (int)rank.Explore, progress.Explore, (int)rank.CQC, progress.CQC, (int)rank.Federation, progress.Federation, (int)rank.Empire, progress.Empire);
                    LastEDSMAPiCommanderTime = progress.EventTimeUTC;
                }

                
                JournalLoadGame loadgame = historylist.FindLast(x => x.EntryType == JournalTypeEnum.LoadGame).journalEntry as JournalLoadGame;
                HistoryEntry shipinfohe = historylist.FindLast(he => he.ShipInformation != null && he.ShipInformation.SubVehicle == ShipInformation.SubVehicleType.None);

                if (shipinfohe != null && !shipinfohe.ShipInformation.Equals(LastShipInfo))
                {
                    ShipInformation shipinfo = shipinfohe.ShipInformation;
                    List<MaterialCommodities> commod = shipinfohe.MaterialCommodity.Sort(true);
                    int cargoqty = commod.Aggregate(0, (n, c) => n + c.count);

                    edsm.CommanderUpdateShip(shipinfo.ID, shipinfo.ShipType, shipinfo, cargoqty);

                    if (LastShipID != shipinfo.ID)
                    {
                        edsm.CommanderSetCurrentShip(loadgame.ShipId);
                    }

                    LastShipID = shipinfo.ID;
                    LastShipInfo = shipinfo;
                }

                if (loadgame != null)
                {
                    if (LastEDSMCredtis != loadgame.Credits)
                        edsm.SetCredits(loadgame.Credits, loadgame.Loan);

                    LastEDSMCredtis = loadgame.Credits;


                    if (shipinfohe == null && LastShipID != loadgame.ShipId)
                    {
                        edsm.CommanderUpdateShip(loadgame.ShipId, loadgame.Ship);
                        edsm.CommanderSetCurrentShip(loadgame.ShipId);
                        LastShipID = loadgame.ShipId;
                        LastShipInfo = null;
                    }
                }



            }
            catch
            {
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
                    NetLogClass.ParseFiles(NetLogPath, out errstr, EDDConfig.Instance.DefaultMapColour, () => cancelRequested(), (p, s) => reportProgress(p, s), ForceNetLogReload, currentcmdrid: CurrentCommander);
                }
            }

            reportProgress(-1, "Resolving systems");

            List<EliteDangerous.JournalEntry> jlist = EliteDangerous.JournalEntry.GetAll(CurrentCommander).OrderBy(x => x.EventTimeUTC).ThenBy(x => x.Id).ToList();
            List<Tuple<EliteDangerous.JournalEntry, HistoryEntry>> jlistUpdated = new List<Tuple<EliteDangerous.JournalEntry, HistoryEntry>>();

            using (SQLiteConnectionSystem conn = new SQLiteConnectionSystem())
            {
                HistoryEntry prev = null;
                HistoryList hlist = new HistoryList();
                foreach (EliteDangerous.JournalEntry inje in jlist)
                {
                    foreach (JournalEntry je in hlist.ProcessJournalEntry(inje))
                    {
                        bool journalupdate = false;
                        HistoryEntry he = HistoryEntry.FromJournalEntry(je, prev, CheckEdsm, out journalupdate, conn, cmdr);

                        prev = he;

                        hist.Add(he);                        // add to the history list here..

                        if (journalupdate)
                        {
                            jlistUpdated.Add(new Tuple<EliteDangerous.JournalEntry, HistoryEntry>(je, he));
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
                        foreach (Tuple<EliteDangerous.JournalEntry, HistoryEntry> jehe in jlistUpdated)
                        {
                            EliteDangerous.JournalEntry je = jehe.Item1;
                            HistoryEntry he = jehe.Item2;
                            EliteDangerous.JournalEvents.JournalFSDJump jfsd = je as EliteDangerous.JournalEvents.JournalFSDJump;
                            if (jfsd != null)
                            {
                                EliteDangerous.JournalEntry.UpdateEDSMIDPosJump(jfsd.Id, he.System, !jfsd.HasCoordinate && he.System.HasCoordinate, jfsd.JumpDist, conn, txn);
                            }
                        }

                        txn.Commit();
                    }
                }
            }

            // now database has been updated due to initial fill, now fill in stuff which needs the user database

            hist.CommanderId = CurrentCommander;

            hist.ProcessUserHistoryListEntries(h => h.ToList());      // here, we update the DBs in HistoryEntry and any global DBs in historylist

            hist.ProcessEDSMApiCommander();

            return hist;
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

        public static List<HistoryEntry> FilterByJournalEvent(List<HistoryEntry> he , string eventstring , out int count)
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

    }
}
