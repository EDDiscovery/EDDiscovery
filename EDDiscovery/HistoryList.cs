using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery2;
using EDDiscovery2.DB;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
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

        private bool? docked;                       // are we docked.  Null if don't know, else true/false
        private bool? landed;                       // are we landed on the planet surface.  Null if don't know, else true/false
        private string wheredocked = "";            // empty if in space, else where docked

        public bool IsLanded { get { return landed.HasValue && landed.Value == true; } }
        public bool IsDocked { get { return docked.HasValue && docked.Value == true; } }
        public string WhereAmI { get { return wheredocked; } }

        #endregion

        #region Constructors

        public void MakeVSEntry(ISystem sys, DateTime eventt, int m, string dist, string info, int journalid = 0)
        {
            Debug.Assert(sys != null);
            EntryType = EliteDangerous.JournalTypeEnum.FSDJump;
            System = sys;
            EventTimeUTC = eventt;
            EventSummary = "Jump to " + System.name;
            EventDescription = dist;
            EventDetailedInfo = info;
            MapColour = m;
            Journalid = journalid;
            EdsmSync = true; 
        }

        public static HistoryEntry FromJournalEntry(EliteDangerous.JournalEntry je, HistoryEntry prev, bool checkedsm, out bool journalupdate, SQLiteConnectionSystem conn = null, EDCommander cmdr = null)
        {
            ISystem isys = prev == null ? new SystemClass("Unknown") : prev.System;
            int indexno = prev == null ? 1 : prev.Indexno + 1;

            int mapcolour = 0;
            journalupdate = false;
            bool starposfromedsm = false;



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
                Commander = cmdr ?? EDDConfig.Instance.Commander(je.CommanderId)
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
                    he.EventDetailedInfo += ((he.EventDetailedInfo.Length > 0) ? Environment.NewLine : "") + "Travelling";

                    if (he.IsFSDJump)
                    {
                        he.EventDetailedInfo += " distance " + he.travelled_distance.ToString("0.0") + " LY"
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

                he.wheredocked = prev.wheredocked;
            }

            if (je.EventTypeID == JournalTypeEnum.Location)
            {
                EliteDangerous.JournalEvents.JournalLocation jl = je as EliteDangerous.JournalEvents.JournalLocation;
                he.docked = jl.Docked;
                he.wheredocked = jl.Docked ? jl.StationName : "";
            }
            else if (je.EventTypeID == JournalTypeEnum.Docked)
            {
                EliteDangerous.JournalEvents.JournalDocked jl = je as EliteDangerous.JournalEvents.JournalDocked;
                he.docked = true;
                he.wheredocked = jl.StationName;
            }
            else if (je.EventTypeID == JournalTypeEnum.Undocked)
            {
                he.docked = false;
            }
            else if (je.EventTypeID == JournalTypeEnum.Touchdown)
            {
                he.landed = true;
            }   
            else if (je.EventTypeID == JournalTypeEnum.Liftoff)
                he.landed = false;
            else if (je.EventTypeID == JournalTypeEnum.LoadGame)
                he.landed = (je as EliteDangerous.JournalEvents.JournalLoadGame).StartLanded;

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
                return EliteDangerous.JournalEntry.GetIcon(EntryType.ToString(),EventDescription);
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

        public bool IsJournalEventInEventFilter(string[] events)
        {
            return events.Contains(Tools.SplitCapsWord(EntryType.ToString()));
        }

        public bool IsJournalEventInEventFilter(string eventstr)
        {
            return eventstr == "All" || IsJournalEventInEventFilter(eventstr.Split(';'));
        }

        public bool UpdateSystemNote(string txt)
        {
            if ((snc == null && txt.Length > 0) || (snc != null && !snc.Note.Equals(txt))) // if no system note, and text,  or system not is not text
            {
                if (snc != null && (snc.Journalid == Journalid || snc.Journalid == 0 || (snc.Name.Equals(System.name, StringComparison.InvariantCultureIgnoreCase) && snc.EdsmId <= 0) || (snc.EdsmId > 0 && snc.EdsmId == System.id_edsm)))           // already there, update
                {
                    snc.Note = txt;
                    snc.Time = DateTime.Now;
                    snc.Name = (IsFSDJump) ? System.name : "";
                    snc.Journalid = Journalid;
                    snc.EdsmId = IsFSDJump ? System.id_edsm : 0;
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
    }


    public class HistoryList : IEnumerable<HistoryEntry>
    {
        private List<HistoryEntry> historylist = new List<HistoryEntry>();  // oldest first here

        public MaterialCommoditiesLedger materialcommodititiesledger;       // and the ledger..

        public EliteDangerous.StarScan starscan;                                           // and the results of scanning

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

        public HistoryEntry GetLastFuelScoop
        {
            get
            {
                return historylist.FindLast(x => x.IsFuelScoop);
            }
        }


        public HistoryEntry GetLastRefuel
        {
            get
            {
                return historylist.FindLast(x => x.journalEntry.EventTypeID==JournalTypeEnum.RefuelAll
                    || x.journalEntry.EventTypeID == JournalTypeEnum.RefuelPartial);
            }
        }
        public HistoryEntry GetLastFSDOrFuelScoop
        {
            get
            {
                return historylist.FindLast(x => x.journalEntry.EventTypeID == JournalTypeEnum.FSDJump
                    || x.journalEntry.EventTypeID == JournalTypeEnum.FuelScoop);
            }
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
            return historylist.Where(he => he.IsFSDJump && he.System.name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && (edsmid <= 0 || he.System.id_edsm == edsmid)).Count();
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
            var list = (from s in historylist where s.EntryType == JournalTypeEnum.FSDJump && s.EventTimeLocal >= start && s.EventTimeLocal   < to select s.journalEntry as JournalFSDJump).ToList<JournalFSDJump>();
            return (from s in list where s.BoostUsed == true select s  ).Count();
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
            var list = (from s in historylist where s.EntryType == JournalTypeEnum.Scan && s.EventTimeLocal >= start && s.EventTimeLocal < to select s.journalEntry as JournalScan).ToList<JournalScan>();
            return list;
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

            foreach (HistoryEntry pos in historylist)
            {
                var list = distlist.Values.ToList();

                if (pos.System.HasCoordinate && list.FindIndex(qx => qx.name.Equals(pos.System.name, StringComparison.InvariantCultureIgnoreCase)) == -1)
                {
                    dx = (pos.System.x - x);
                    dy = (pos.System.y - y);
                    dz = (pos.System.z - z);
                    dist = dx * dx + dy * dy + dz * dz;

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
                return historylist.FindLast(x => x.System.name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && x.IsFSDJump);
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

        public static HistoryEntry FindByPos(List<HistoryEntry> syslist, Vector3 p, double limit)     // go thru setting the lastknowsystem
        {
            return syslist.FindLast(x => x.System.HasCoordinate &&
                                            Math.Abs(x.System.x - p.X) < limit &&
                                            Math.Abs(x.System.y - p.Y) < limit &&
                                            Math.Abs(x.System.z - p.Z) < limit);
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
