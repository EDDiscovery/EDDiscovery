
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

using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace EliteDangerousCore
{
    public class HistoryList : IEnumerable<HistoryEntry>
    {
        private List<HistoryEntry> historylist = new List<HistoryEntry>();  // oldest first here
        public Ledger cashledger { get; private set; } = new Ledger();       // and the ledger..
        public ShipInformationList shipinformationlist { get; private set; } = new ShipInformationList();     // ship info
        private MissionListAccumulator missionlistaccumulator = new MissionListAccumulator(); // and mission list..
        public ShipYardList shipyards = new ShipYardList(); // yards in space (not meters)
        public OutfittingList outfitting = new OutfittingList();
        public StarScan starscan { get; private set; } = new StarScan();                                           // and the results of scanning
        public int CommanderId { get; private set; }

        public HistoryList() { }

        public HistoryList(List<HistoryEntry> hl) { historylist = hl; }         // SPECIAL USE ONLY - DOES NOT COMPUTE ALL THE OTHER STUFF

        public void Copy(HistoryList other)       // Must copy all relevant items.. been caught out by this 23/6/2017
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
            shipyards = other.shipyards;
            outfitting = other.outfitting;
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

        public List<HistoryEntry> FilterStartEnd()
        {
            List<HistoryEntry> entries = new List<HistoryEntry>();
            bool started = false;
            foreach (HistoryEntry he in historylist.OrderBy(s => s.EventTimeUTC).ToList())      // ascending order
            {
                if (he.StartMarker)
                {
                    started = true;
                    entries.Add(he);
                }
                else if (started)
                {
                    entries.Add(he);
                    if (he.StopMarker && !he.StartMarker)
                        started = false;
                }
            }
            return entries.OrderByDescending(s => s.EventTimeUTC).ToList();
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

        public List<HistoryEntry> FilterByDateRangeLatestFirst(DateTime startutc, DateTime endutc) // UTC! in time ascending order
        {
            return historylist.Where(s => s.EventTimeUTC >= startutc && s.EventTimeUTC <= endutc).OrderByDescending(s => s.EventTimeUTC).ToList();
        }

        public List<HistoryEntry> FilterByNotEDSMSyncedAndFSD
        {
            get
            {
                return (from s in historylist where s.EdsmSync == false && s.IsLocOrJump orderby s.EventTimeUTC ascending select s).ToList();
            }
        }

        public List<HistoryEntry> FilterByScanNotEDDNSynced
        {
            get
            {
                return (from s in historylist where s.EDDNSync == false && s.EntryType == JournalTypeEnum.Scan orderby s.EventTimeUTC ascending select s).ToList();
            }
        }

        public List<HistoryEntry> FilterByFSDAndPosition
        {
            get
            {
                return (from s in historylist where s.IsFSDJump && s.System.HasCoordinate select s).ToList();
            }
        }


        public List<HistoryEntry> FilterByCommodityPricesBackwards      // full commodity price information only
        {
            get
            {
                return (from s in historylist
                        where (s.journalEntry is JournalCommodityPricesBase && (s.journalEntry as JournalCommodityPricesBase).Commodities.Count > 0)
                        orderby s.EventTimeUTC descending
                        select s).ToList();
            }
        }

        public List<HistoryEntry> FilterByTravel { get { return FilterHLByTravel(historylist); } }

        static public List<HistoryEntry> FilterHLByTravel(List<HistoryEntry> hlist)        // filter, in its own order. return FSD and location events after death
        {
            List<HistoryEntry> ents = new List<HistoryEntry>();
            bool resurrect = true;
            foreach (HistoryEntry he in hlist)
            {
                if (he.EntryType == JournalTypeEnum.Resurrect || he.EntryType == JournalTypeEnum.Died)
                {
                    resurrect = true;
                    ents.Add(he);
                }
                else if ((resurrect && he.EntryType == JournalTypeEnum.Location) || he.EntryType == JournalTypeEnum.FSDJump || he.EntryType == JournalTypeEnum.CarrierJump)
                {
                    resurrect = false;
                    ents.Add(he);
                }
            }

            return ents;
        }

        public List<HistoryEntry> FilterByFSD
        {
            get
            {
                return (from s in historylist where s.IsFSDJump select s).ToList();
            }
        }

        public List<HistoryEntry> FilterByScan
        {
            get
            {
                return (from s in historylist where s.journalEntry.EventTypeID == JournalTypeEnum.Scan select s).ToList();
            }
        }

        public HistoryEntry GetByJID(long jid)
        {
            return historylist.Find(x => x.Journalid == jid);
        }

        public HistoryEntry GetByIndex(int index)
        {
            return (index >= 1 && index <= historylist.Count) ? historylist[index - 1] : null;
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

        static public List<List<HistoryEntry>> SystemAggregateList(List<HistoryEntry> result) // give a list of HEs, and return unique systems list of those HEs
        {
            Dictionary<string, List<HistoryEntry>> systemsentered = new Dictionary<string, List<HistoryEntry>>();

            foreach (HistoryEntry he in result)
            {
                if (!systemsentered.ContainsKey(he.System.Name))
                    systemsentered[he.System.Name] = new List<HistoryEntry>();

                systemsentered[he.System.Name].Add(he);
            }

            return systemsentered.Values.ToList();
        }

        #endregion

        #region Status

        public bool IsCurrentlyLanded { get { HistoryEntry he = GetLast; return (he != null) ? he.IsLanded : false; } }     //safe methods
        public bool IsCurrentlyDocked { get { HistoryEntry he = GetLast; return (he != null) ? he.IsDocked : false; } }
        public ISystem CurrentSystem { get { HistoryEntry he = GetLast; return (he != null) ? he.System : null; } }  // current system

        public double DistanceCurrentTo(string system)          // from current, if we have one, to system, if its found.
        {
            ISystem cursys = CurrentSystem;
            ISystem other = SystemCache.FindSystem(system);
            return cursys != null ? cursys.Distance(other) : -1;  // current can be null, shipsystem can be null, cursys can not have co-ord, -1 if failed.
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

        public T GetLastJournalEntry<T>(Predicate<HistoryEntry> where) where T : class             // may be Null
        {
            return historylist.FindLast(where)?.journalEntry as T;
        }

        public HistoryEntry GetLastHistoryEntry(Predicate<HistoryEntry> where, HistoryEntry frominclusive)
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


        public int GetVisitsCount(string name)
        {
            return (from he in historylist.AsParallel()
                    where (he.IsFSDJump && he.System.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    select he).Count();
        }
        public List<JournalScan> GetScans(string name)
        {
            return (from s in historylist.AsParallel()
                    where (s.journalEntry.EventTypeID == JournalTypeEnum.Scan && s.System.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    select s.journalEntry as JournalScan).ToList<JournalScan>();
        }

        public int GetFSDJumps(TimeSpan t)
        {
            DateTime tme = DateTime.UtcNow.Subtract(t);
            return (from s in historylist where s.IsFSDJump && s.EventTimeUTC >= tme select s).Count();
        }

        public int GetFSDJumpsUTC(DateTime startutc, DateTime toutc)
        {
            return (from s in historylist where s.IsFSDJump && s.EventTimeUTC >= startutc && s.EventTimeUTC < toutc select s).Count();
        }

        public int GetFSDJumps(string forShipKey)
        {
            return (from s in historylist where s.IsFSDJump && $"{s.ShipTypeFD.ToLowerInvariant()}:{s.ShipId}" == forShipKey select s).Count();
        }

        public int GetNrMappedUTC(DateTime startutc, DateTime toutc)
        {
            return (from s in historylist where s.journalEntry.EventTypeID == JournalTypeEnum.SAAScanComplete && s.EventTimeUTC >= startutc && s.EventTimeUTC < toutc select s).Count();
        }


        public Tuple<int, long> GetScanCountAndValueUTC(DateTime startutc, DateTime toutc)
        {
            var scans = historylist
                .Where(s => s.EntryType == JournalTypeEnum.Scan && s.EventTimeUTC >= startutc && s.EventTimeUTC < toutc)
                .Select(h => h.journalEntry as JournalScan)
                .Distinct(new ScansAreForSameBody()).ToArray();

            var total = scans.Sum(scan => (long)scan.EstimatedValue);

            return new Tuple<int, long>(scans.Length, total);
        }

        public int GetJetConeBoostUTC(DateTime startutc, DateTime toutc)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.JetConeBoost && s.EventTimeUTC >= startutc && s.EventTimeUTC < toutc select s).Count();
        }


        public HistoryFsdJumpStatistics GetFsdJumpStatistics(DateTime startUtc, DateTime toUtc)
        {
            var jumps = historylist
                .Where(s => s.EntryType == JournalTypeEnum.FSDJump && s.EventTimeUTC >= startUtc && s.EventTimeUTC < toUtc)
                .Select(h => h.journalEntry as JournalFSDJump)
                .ToArray();

            return new HistoryFsdJumpStatistics(
                jumps.Length,
                jumps.Sum(j => j.JumpDist),
                jumps.Where(j => j.BoostValue == 1).Count(),
                jumps.Where(j => j.BoostValue == 2).Count(),
                jumps.Where(j => j.BoostValue == 3).Count());
        }

        public bool IsTravellingUTC(out DateTime startTimeutc)
        {
            bool inTrip = false;
            startTimeutc = DateTime.UtcNow;
            HistoryEntry lastStartMark = GetLastHistoryEntry(x => x.StartMarker);
            if (lastStartMark != null)
            {
                HistoryEntry lastStopMark = GetLastHistoryEntry(x => x.StopMarker);
                inTrip = lastStopMark == null || lastStopMark.EventTimeUTC < lastStartMark.EventTimeUTC;
                if (inTrip)
                    startTimeutc = lastStartMark.EventTimeUTC;
            }
            return inTrip;
        }


        public double GetTraveledLy(string forShipKey)
        {
            var list = (from s in historylist where s.EntryType == JournalTypeEnum.FSDJump && $"{s.ShipTypeFD.ToLowerInvariant()}:{s.ShipId}" == forShipKey select s.journalEntry as JournalFSDJump).ToList<JournalFSDJump>();

            return (from s in list select s.JumpDist).Sum();
        }

        public List<JournalScan> GetScanListUTC(DateTime startutc, DateTime toutc)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.Scan && s.EventTimeUTC >= startutc && s.EventTimeUTC < toutc select s.journalEntry as JournalScan)
                .Distinct(new ScansAreForSameBody()).ToList();
        }

        public int GetTonnesBought(string forShipKey)
        {
            var list = (from s in historylist where s.EntryType == JournalTypeEnum.MarketBuy && $"{s.ShipTypeFD.ToLowerInvariant()}:{s.ShipId}" == forShipKey select s.journalEntry as JournalMarketBuy).ToList();

            return (from s in list select s.Count).Sum();
        }

        public int GetTonnesSold(string forShipKey)
        {
            var list = (from s in historylist where s.EntryType == JournalTypeEnum.MarketSell && $"{s.ShipTypeFD.ToLowerInvariant()}:{s.ShipId}" == forShipKey select s.journalEntry as JournalMarketSell).ToList();

            return (from s in list select s.Count).Sum();
        }

        public int GetDeathCount(string forShipKey)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.Died && $"{s.ShipTypeFD.ToLowerInvariant()}:{s.ShipId}" == forShipKey select s).Count();
        }

        public int GetBodiesScanned(string forShipKey)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.Scan && $"{s.ShipTypeFD.ToLowerInvariant()}:{s.ShipId}" == forShipKey select s.journalEntry as JournalScan)
                .Distinct(new ScansAreForSameBody()).Count();
        }

        public int GetFSDJumpsBeforeUTC(DateTime utc)
        {
            return (from s in historylist where s.IsFSDJump && s.EventTimeUTC < utc select s).Count();
        }

        public string GetCommanderFID()     // may be null
        {
            var cmdr = historylist.FindLast(x => x.EntryType == JournalTypeEnum.Commander);
            return (cmdr?.journalEntry as JournalCommander)?.FID;
        }

        public delegate bool FurthestFund(HistoryEntry he, ref double lastv);
        public HistoryEntry GetConditionally(double lastv, FurthestFund f)              // give a comparision function, find entry
        {
            HistoryEntry best = null;
            foreach (HistoryEntry s in historylist)
            {
                if (f(s, ref lastv))
                    best = s;
            }

            return best;
        }

        public bool IsBetween(HistoryEntry first, HistoryEntry last, Predicate<HistoryEntry> predicate)     // either direction
        {
            if (first.Indexno < last.Indexno)
                return historylist.Where(h => h.Indexno > first.Indexno && h.Indexno <= last.Indexno && predicate(h)).Any();
            else
                return historylist.Where(h => h.Indexno >= first.Indexno && h.Indexno < last.Indexno && predicate(h)).Any();
        }

        public bool AnyBetween(HistoryEntry first, HistoryEntry last, IEnumerable<JournalTypeEnum> journalTypes)
        {
            if (first.Indexno < last.Indexno)
                return historylist.Where(h => h.Indexno > first.Indexno && h.Indexno <= last.Indexno && journalTypes.Contains(h.EntryType)).Any();
            else
                return historylist.Where(h => h.Indexno >= first.Indexno && h.Indexno < last.Indexno && journalTypes.Contains(h.EntryType)).Any();
        }

        public static HistoryEntry FindLastFSDKnownPosition(List<HistoryEntry> syslist)
        {
            return syslist.FindLast(x => x.System.HasCoordinate && x.IsLocOrJump);
        }

        public static HistoryEntry FindByPos(List<HistoryEntry> syslist, float x, float y, float z, double limit)     // go thru setting the lastknowsystem
        {
            return syslist.FindLast(s => s.System.HasCoordinate &&
                                            Math.Abs(s.System.X - x) < limit &&
                                            Math.Abs(s.System.Y - y) < limit &&
                                            Math.Abs(s.System.Z - z) < limit);
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

        public static IEnumerable<ISystem> FindSystemsWithinLy(List<HistoryEntry> he, ISystem centre, double minrad, double maxrad, bool spherical)
        {
            IEnumerable<ISystem> list;

            if (spherical)
                list = (from x in he where x.System.HasCoordinate && x.System.Distance(centre, minrad, maxrad) select x.System);
            else
                list = (from x in he where x.System.HasCoordinate && x.System.Cuboid(centre, minrad, maxrad) select x.System);

            return list.GroupBy(x => x.Name).Select(group => group.First());
        }

        #endregion

        #region EDSM

        public void FillInPositionsFSDJumps()       // call if you want to ensure we have the best posibile position data on FSD Jumps.  Only occurs on pre 2.1 with lazy load of just name/edsmid
        {
            List<Tuple<HistoryEntry, ISystem>> updatesystems = new List<Tuple<HistoryEntry, ISystem>>();

            if (!SystemsDatabase.Instance.RebuildRunning)       // only run this when the system db is stable.. this prevents the UI from slowing to a standstill
            {
                SystemsDatabase.Instance.ExecuteWithDatabase(cn =>
                {
                    foreach (HistoryEntry he in historylist)
                    {
                        if (he.IsFSDJump && !he.System.HasCoordinate)// try and load ones without position.. if its got pos we are happy
                        {           // done in two IFs for debugging, in case your wondering why!
                            if (he.System.source != SystemSource.FromEDSM && he.System.EDSMID == 0)   // and its not from EDSM and we have not already tried
                            {
                                ISystem found = SystemCache.FindSystem(he.System, cn);
                                if (found != null)
                                    updatesystems.Add(new Tuple<HistoryEntry, ISystem>(he, found));
                            }
                        }
                    }
                });
            }

            if (updatesystems.Count > 0)
            {
                UserDatabase.Instance.ExecuteWithDatabase(cn =>
                {
                    using (DbTransaction txn = cn.Connection.BeginTransaction())        // take a transaction over this
                    {
                        foreach (Tuple<HistoryEntry, ISystem> he in updatesystems)
                        {
                            FillInSystemFromDBInt(he.Item1, he.Item2, cn.Connection, txn);  // fill, we already have an EDSM system to use
                        }

                        txn.Commit();
                    }
                });
            }
        }

        public void FillEDSM(HistoryEntry syspos)       // call to fill in ESDM data for entry, and also fills in all others pointing to the system object
        {
            if (syspos.System.source == SystemSource.FromEDSM || syspos.System.EDSMID == -1)  // if set already, or we tried and failed..
            {
                //System.Diagnostics.Debug.WriteLine("Checked System {0} already id {1} ", syspos.System.name , syspos.System.id_edsm);
                return;
            }

            ISystem edsmsys = SystemCache.FindSystem(syspos.System);        // see if we have it..

            if (edsmsys != null)                                            // if we found it externally, fill in info
            {
                UserDatabase.Instance.ExecuteWithDatabase(cn =>
                {
                    using (DbTransaction txn = cn.Connection.BeginTransaction())
                    {
                        FillInSystemFromDBInt(syspos, edsmsys, cn.Connection, txn); // and fill in using this connection/tx
                        txn.Commit();
                    }
                });
            }
            else
                FillInSystemFromDBInt(syspos, null, null, null);        // else fill in using null system, which means just mark it checked
        }

        private void FillInSystemFromDBInt(HistoryEntry syspos, ISystem edsmsys, SQLiteConnectionUser uconn, DbTransaction utn)       // call to fill in ESDM data for entry, and also fills in all others pointing to the system object
        {
            List<HistoryEntry> alsomatching = new List<HistoryEntry>();

            foreach (HistoryEntry he in historylist)       // list of systems in historylist using the same system object
            {
                if (Object.ReferenceEquals(he.System, syspos.System))
                    alsomatching.Add(he);
            }

            if (edsmsys != null)
            {
                ISystem oldsys = syspos.System;

                bool updateedsmid = oldsys.EDSMID != edsmsys.EDSMID;
                bool updatesyspos = edsmsys.HasCoordinate &&
                                    (edsmsys.Xi != oldsys.Xi || edsmsys.Yi != oldsys.Yi || edsmsys.Zi != oldsys.Zi) &&
                                    oldsys.source != SystemSource.FromJournal; // NEVER EVER EVER OVERRIDE JOURNAL COORDINATES
                bool updatename = oldsys.source != SystemSource.FromJournal ||
                                  !oldsys.Name.Equals(edsmsys.Name, StringComparison.InvariantCultureIgnoreCase);

                ISystem newsys = new SystemClass
                {
                    EDSMID = updateedsmid ? edsmsys.EDSMID : oldsys.EDSMID,
                    Name = updatename ? edsmsys.Name : oldsys.Name,
                    X = updatesyspos ? edsmsys.X : oldsys.X,
                    Y = updatesyspos ? edsmsys.Y : oldsys.Y,
                    Z = updatesyspos ? edsmsys.Z : oldsys.Z,
                    GridID = edsmsys.GridID,
                    SystemAddress = oldsys.SystemAddress ?? edsmsys.SystemAddress,

                    EDDBID = edsmsys.EDDBID,
                    Population = oldsys.Government == EDGovernment.Unknown ? edsmsys.Population : oldsys.Population,
                    Faction = oldsys.Faction ?? edsmsys.Faction,
                    Government = oldsys.Government == EDGovernment.Unknown ? edsmsys.Government : oldsys.Government,
                    Allegiance = oldsys.Allegiance == EDAllegiance.Unknown ? edsmsys.Allegiance : oldsys.Allegiance,
                    State = oldsys.State == EDState.Unknown ? edsmsys.State : oldsys.State,
                    Security = oldsys.Security == EDSecurity.Unknown ? edsmsys.Security : oldsys.Security,
                    PrimaryEconomy = oldsys.PrimaryEconomy == EDEconomy.Unknown ? edsmsys.PrimaryEconomy : oldsys.PrimaryEconomy,
                    Power = !oldsys.Power.HasChars() ? edsmsys.Power : oldsys.Power,
                    PowerState = !oldsys.PowerState.HasChars() ? edsmsys.PowerState : oldsys.PowerState,
                    NeedsPermit = edsmsys.NeedsPermit,
                    EDDBUpdatedAt = edsmsys.EDDBUpdatedAt,
                    source = SystemSource.FromEDSM
                };

                foreach (HistoryEntry he in alsomatching)       // list of systems in historylist using the same system object
                {
                    bool updatepos = he.IsLocOrJump && updatesyspos;

                    if (updatepos || updateedsmid)
                        JournalEntry.UpdateEDSMIDPosJump(he.Journalid, edsmsys, updatepos, -1, uconn, utn);  // update pos and edsmid, jdist not updated

                    he.System = newsys;
                }
            }
            else
            {
                foreach (HistoryEntry he in alsomatching)       // list of systems in historylist using the same system object
                    he.System.EDSMID = -1;                     // can't do it
            }
        }

        #endregion

        #region General Info

        // Add in any systems we have to the distlist 

        public void CalculateSqDistances(BaseUtils.SortedListDoubleDuplicate<ISystem> distlist, double x, double y, double z,
                                         int maxitems, double mindistance, double maxdistance, bool spherical)
        {
            HashSet<string> listnames = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (ISystem sys in distlist.Values.ToList())
                listnames.Add(sys.Name);

            mindistance *= mindistance;

            foreach (HistoryEntry pos in historylist)
            {
                if (pos.System.HasCoordinate && !listnames.Contains(pos.System.Name))
                {
                    double dx = (pos.System.X - x);
                    double dy = (pos.System.Y - y);
                    double dz = (pos.System.Z - z);
                    double distsq = dx * dx + dy * dy + dz * dz;

                    listnames.Add(pos.System.Name); //stops repeats..

                    if (distsq >= mindistance &&
                            (spherical && distsq <= maxdistance * maxdistance ||
                            !spherical && Math.Abs(dx) <= maxdistance && Math.Abs(dy) <= maxdistance && Math.Abs(dz) <= maxdistance))
                    {
                        if (distlist.Count < maxitems)          // if less than max, add..
                        {
                            distlist.Add(distsq, pos.System);
                        }
                        else if (distsq < distlist.Keys[distlist.Count - 1])   // if last entry (which must be the biggest) is greater than dist..
                        {
                            distlist.Add(distsq, pos.System);           // add in
                            distlist.RemoveAt(maxitems);        // remove last..
                        }
                    }
                }
            }
        }

        public HistoryEntry FindByName(string name, bool fsdjump = false)
        {
            if (fsdjump)
                return historylist.FindLast(x => x.System.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            else
                return historylist.FindLast(x => x.IsFSDJump && x.System.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public ISystem FindSystem(string name, EDSM.GalacticMapping glist = null)        // in system or name
        {
            ISystem ds1 = SystemCache.FindSystem(name);     // now go thru the cache..

            if (ds1 == null)
            {
                HistoryEntry vs = FindByName(name);         // else try and find in our list

                if (vs != null)
                    ds1 = vs.System;
                else if (glist != null)                     // if we have a galmap
                {
                    EDSM.GalacticMapObject gmo = glist.Find(name, true, true);

                    if (gmo != null && gmo.points.Count > 0)
                    {
                        ds1 = SystemCache.FindSystem(gmo.galMapSearch);

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
            int index = syslist.FindIndex(x => x.System.Name.Equals(sysname));

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

        public void SetStartStop(HistoryEntry hs)
        {
            bool started = false;

            foreach (HistoryEntry he in historylist)
            {
                if (hs == he)
                {
                    if (he.StartMarker || he.StopMarker)
                        hs.journalEntry.ClearStartEndFlag();
                    else if (started == false)
                        hs.journalEntry.SetStartFlag();
                    else
                        hs.journalEntry.SetEndFlag();

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

        // Called on a New Entry, by EDDiscoveryController:NewEntry, to add an journal entry in

        public HistoryEntry AddJournalEntry(JournalEntry je, Action<string> logerror)   // always return he
        {
            HistoryEntry prev = GetLast;

            bool journalupdate = false;
            HistoryEntry he = HistoryEntry.FromJournalEntry(je, prev, true, out journalupdate);     // we may check edsm for this entry

            if (journalupdate)
            {
                JournalFSDJump jfsd = je as JournalFSDJump;

                if (jfsd != null)
                {
                    UserDatabase.Instance.ExecuteWithDatabase(cn =>
                    {
                        JournalEntry.UpdateEDSMIDPosJump(jfsd.Id, he.System, !jfsd.HasCoordinate && he.System.HasCoordinate, jfsd.JumpDist, cn.Connection);
                    });
                }
            }

            he.ProcessWithUserDb(je, prev, this);           // let some processes which need the user db to work

            cashledger.Process(je);
            he.Credits = cashledger.CashTotal;

            shipyards.Process(je);
            outfitting.Process(je);

            Tuple<ShipInformation, ModulesInStore> ret = shipinformationlist.Process(je, he.WhereAmI, he.System);
            he.ShipInformation = ret.Item1;
            he.StoredModules = ret.Item2;

            he.MissionList = missionlistaccumulator.Process(je, he.System, he.WhereAmI);

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
                    var bodyname = js.BodyDesignation ?? js.BodyName;

                    if (bodyname == null)
                    {
                        logerror("Body name not set in scan entry");
                    }
                    else if (jl == null || (jl.StarSystem.Equals(jlhe.System.Name, StringComparison.InvariantCultureIgnoreCase) && !bodyname.ToLowerInvariant().Contains(" belt cluster ")))
                    {
                        logerror("Cannot add scan to system - alert the EDDiscovery developers using either discord or Github (see help)" + Environment.NewLine +
                                            "Scan object " + js.BodyName + " in " + he.System.Name);
                    }
                }
            }
            else if (je is JournalSAAScanComplete)
            {
                starscan.AddSAAScanToBestSystem((JournalSAAScanComplete)je, Count - 1, EntryOrder);
            }
            else if (je is JournalSAASignalsFound)
            {
                this.starscan.AddSAASignalsFoundToBestSystem((JournalSAASignalsFound)je, Count - 1, EntryOrder);
            }
            else if (je is JournalFSSDiscoveryScan && he.System != null)
            {
                starscan.SetFSSDiscoveryScan((JournalFSSDiscoveryScan)je, he.System);
            }
            else if (je is IBodyNameAndID)
            {
                starscan.AddBodyToBestSystem((IBodyNameAndID)je, Count - 1, EntryOrder);
            }

            return he;
        }

        public static HistoryList LoadHistory(EDJournalClass journalmonitor, Func<bool> cancelRequested, Action<int, string> reportProgress,
                                    string NetLogPath = null,
                                    bool ForceNetLogReload = false,
                                    bool ForceJournalReload = false,
                                    int CurrentCommander = Int32.MinValue,
                                    int fullhistoryloaddaylimit = 0,
                                    string essentialitems = ""
                                    )
        {
            HistoryList hist = new HistoryList();

            if (CurrentCommander >= 0)
            {
                journalmonitor.ParseJournalFiles(() => cancelRequested(), (p, s) => reportProgress(p, s), forceReload: ForceJournalReload);   // Parse files stop monitor..

                if (NetLogPath != null)
                {
                    string errstr = null;
                    NetLogClass.ParseFiles(NetLogPath, out errstr, EDCommander.Current.MapColour, () => cancelRequested(), (p, s) => reportProgress(p, s), ForceNetLogReload, currentcmdrid: CurrentCommander);
                }
            }

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Files read ");

            reportProgress(-1, "Reading Database");

            List<JournalEntry> jlist;

            if (fullhistoryloaddaylimit > 0)
            {
                var list = (essentialitems == nameof(JournalEssentialEvents.JumpScanEssentialEvents)) ? JournalEssentialEvents.JumpScanEssentialEvents :
                           (essentialitems == nameof(JournalEssentialEvents.JumpEssentialEvents)) ? JournalEssentialEvents.JumpEssentialEvents :
                           (essentialitems == nameof(JournalEssentialEvents.NoEssentialEvents)) ? JournalEssentialEvents.NoEssentialEvents :
                           (essentialitems == nameof(JournalEssentialEvents.FullStatsEssentialEvents)) ? JournalEssentialEvents.FullStatsEssentialEvents :
                            JournalEssentialEvents.EssentialEvents;

                jlist = JournalEntry.GetAll(CurrentCommander,
                    ids: list,
                    allidsafterutc: DateTime.UtcNow.Subtract(new TimeSpan(fullhistoryloaddaylimit, 0, 0, 0))
                    ).OrderBy(x => x.EventTimeUTC).ThenBy(x => x.Id).ToList();
            }
            else
                jlist = JournalEntry.GetAll(CurrentCommander).OrderBy(x => x.EventTimeUTC).ThenBy(x => x.Id).ToList();

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Database read " + jlist.Count);

            List<Tuple<JournalEntry, HistoryEntry>> jlistUpdated = new List<Tuple<JournalEntry, HistoryEntry>>();

            HistoryEntry prev = null;
            JournalEntry jprev = null;

            reportProgress(-1, "Creating History");

            bool checkforunknownsystemsindb = true;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            foreach (JournalEntry je in jlist)
            {
                if (MergeEntries(jprev, je))        // if we merge.. we may have updated info, so reprint.
                {
                    //jprev.FillInformation(out prev.EventSummary, out prev.EventDescription, out prev.EventDetailedInfo);    // need to keep this up to date..
                    continue;
                }

                // Clean up "UnKnown" systems from EDSM log
                if (je is JournalFSDJump && ((JournalFSDJump)je).StarSystem == "UnKnown")
                {
                    JournalEntry.Delete(je.Id);
                    continue;
                }

                if (je is EliteDangerousCore.JournalEvents.JournalMusic)      // remove music.. not shown.. now UI event. remove it for backwards compatibility
                {
                    //System.Diagnostics.Debug.WriteLine("**** Filter out " + je.EventTypeStr + " on " + je.EventTimeLocal.ToString());
                    continue;
                }

                long timetoload = sw.ElapsedMilliseconds;
                HistoryEntry he = HistoryEntry.FromJournalEntry(je, prev, checkforunknownsystemsindb, out bool journalupdate);

                if (sw.ElapsedMilliseconds - timetoload > 100)
                {
                    System.Diagnostics.Debug.WriteLine("DB is slow - probably 3dmap is being initialised, give up checking for old systems");
                    checkforunknownsystemsindb = false;
                }

                prev = he;
                jprev = je;

                hist.historylist.Add(he);

                if (journalupdate)
                {
                    jlistUpdated.Add(new Tuple<JournalEntry, HistoryEntry>(je, he));
                    Debug.WriteLine("Queued update requested {0} {1}", he.System.EDSMID, he.System.Name);
                }
            }

            if (jlistUpdated.Count > 0)
            {
                reportProgress(-1, "Updating journal entries");

                UserDatabase.Instance.ExecuteWithDatabase(cn =>
                {
                    using (DbTransaction txn = cn.Connection.BeginTransaction())
                    {
                        foreach (Tuple<JournalEntry, HistoryEntry> jehe in jlistUpdated)
                        {
                            JournalEntry je = jehe.Item1;
                            HistoryEntry he = jehe.Item2;

                            double dist = (je is JournalFSDJump) ? (je as JournalFSDJump).JumpDist : 0;
                            bool updatecoord = (je is JournalLocOrJump) ? (!(je as JournalLocOrJump).HasCoordinate && he.System.HasCoordinate) : false;

                            Debug.WriteLine("Push update {0} {1} to JE {2} HE {3}", he.System.EDSMID, he.System.Name, je.Id, he.Indexno);
                            JournalEntry.UpdateEDSMIDPosJump(je.Id, he.System, updatecoord, dist, cn.Connection, txn);
                        }

                        txn.Commit();
                    }
                });
            }

            // now database has been updated due to initial fill, now fill in stuff which needs the user database

            hist.CommanderId = CurrentCommander;

            reportProgress(-1, "Updating user statistics");

            hist.ProcessUserHistoryListEntries();      // here, we update the DBs in HistoryEntry and any global DBs in historylist

            reportProgress(-1, "Done");

            return hist;
        }


        // go through the history list and recalculate the materials ledger and the materials count, plus any other stuff..
        public void ProcessUserHistoryListEntries()
        {
            List<HistoryEntry> hl = historylist;

            for (int i = 0; i < hl.Count; i++)
            {
                HistoryEntry he = hl[i];
                JournalEntry je = he.journalEntry;

                he.ProcessWithUserDb(je, (i > 0) ? hl[i - 1] : null, this);        // let the HE do what it wants to with the user db

                Debug.Assert(he.MaterialCommodity != null);

                // **** REMEMBER NEW Journal entry needs this too *****************

                cashledger.Process(je);            // update the ledger     
                he.Credits = cashledger.CashTotal;

                shipyards.Process(je);
                outfitting.Process(je);

                Tuple<ShipInformation, ModulesInStore> ret = shipinformationlist.Process(je, he.WhereAmI, he.System);  // the ships
                he.ShipInformation = ret.Item1;
                he.StoredModules = ret.Item2;

                he.MissionList = missionlistaccumulator.Process(je, he.System, he.WhereAmI);                           // the missions

                if (je.EventTypeID == JournalTypeEnum.Scan && je is JournalScan)
                {
                    if (!this.starscan.AddScanToBestSystem(je as JournalScan, i, hl))
                    {
                        System.Diagnostics.Debug.WriteLine("******** Cannot add scan to system " + (je as JournalScan).BodyName + " in " + he.System.Name);
                    }
                }
                else if (je.EventTypeID == JournalTypeEnum.SAAScanComplete)
                {
                    this.starscan.AddSAAScanToBestSystem((JournalSAAScanComplete)je, i, hl);
                }
                else if (je.EventTypeID == JournalTypeEnum.SAASignalsFound)
                {
                    this.starscan.AddSAASignalsFoundToBestSystem((JournalSAASignalsFound)je, i, hl);
                }
                else if (je.EventTypeID == JournalTypeEnum.FSSDiscoveryScan && he.System != null)
                {
                    this.starscan.SetFSSDiscoveryScan((JournalFSSDiscoveryScan)je, he.System);
                }
                else if (je is IBodyNameAndID)
                {
                    this.starscan.AddBodyToBestSystem((IBodyNameAndID)je, i, hl);
                }
            }
        }

        public static int MergeTypeDelay(JournalEntry je)   // 0 = no delay, delay for attempts to merge items..
        {
            if (je.EventTypeID == JournalTypeEnum.Friends)
                return 2000;
            else if (je.EventTypeID == JournalTypeEnum.FSSSignalDiscovered)
                return 2000;
            else if (je.EventTypeID == JournalTypeEnum.FuelScoop)
                return 10000;
            else if (je.EventTypeID == JournalTypeEnum.ShipTargeted)
                return 250; // short, so normally does not merge unless your clicking around like mad
            else
                return 0;
        }

        // true if merged back to previous..
        public static bool MergeEntries(JournalEntry prev, JournalEntry je)
        {
            if (prev != null && !EliteConfigInstance.InstanceOptions.DisableMerge)
            {
                bool prevsame = je.EventTypeID == prev.EventTypeID;

                if (prevsame)
                {
                    if (je.EventTypeID == JournalTypeEnum.FuelScoop)  // merge scoops
                    {
                        EliteDangerousCore.JournalEvents.JournalFuelScoop jfs = je as EliteDangerousCore.JournalEvents.JournalFuelScoop;
                        EliteDangerousCore.JournalEvents.JournalFuelScoop jfsprev = prev as EliteDangerousCore.JournalEvents.JournalFuelScoop;
                        jfsprev.Scooped += jfs.Scooped;
                        jfsprev.Total = jfs.Total;
                        //System.Diagnostics.Debug.WriteLine("Merge FS " + jfsprev.EventTimeUTC);
                        return true;
                    }
                    else if (je.EventTypeID == JournalTypeEnum.Friends) // merge friends
                    {
                        EliteDangerousCore.JournalEvents.JournalFriends jfprev = prev as EliteDangerousCore.JournalEvents.JournalFriends;
                        EliteDangerousCore.JournalEvents.JournalFriends jf = je as EliteDangerousCore.JournalEvents.JournalFriends;
                        jfprev.AddFriend(jf);
                        //System.Diagnostics.Debug.WriteLine("Merge Friends " + jfprev.EventTimeUTC + " " + jfprev.NameList.Count);
                        return true;
                    }
                    else if (je.EventTypeID == JournalTypeEnum.FSSSignalDiscovered)
                    {
                        var jdprev = prev as EliteDangerousCore.JournalEvents.JournalFSSSignalDiscovered;
                        var jd = je as EliteDangerousCore.JournalEvents.JournalFSSSignalDiscovered;
                        jdprev.Add(jd);
                        return true;
                    }
                    else if (je.EventTypeID == JournalTypeEnum.ShipTargeted)
                    {
                        var jdprev = prev as EliteDangerousCore.JournalEvents.JournalShipTargeted;
                        var jd = je as EliteDangerousCore.JournalEvents.JournalShipTargeted;
                        jdprev.Add(jd);
                        return true;
                    }
                    else if (je.EventTypeID == JournalTypeEnum.UnderAttack)
                    {
                        var jdprev = prev as EliteDangerousCore.JournalEvents.JournalUnderAttack;
                        var jd = je as EliteDangerousCore.JournalEvents.JournalUnderAttack;
                        jdprev.Add(jd.Target);
                        return true;
                    }
                    else if (je.EventTypeID == JournalTypeEnum.ReceiveText)
                    {
                        var jdprev = prev as EliteDangerousCore.JournalEvents.JournalReceiveText;
                        var jd = je as EliteDangerousCore.JournalEvents.JournalReceiveText;

                        // merge if same channel 
                        if (jd.Channel == jdprev.Channel)
                        {
                            jdprev.Add(jd);
                            return true;
                        }
                    }
                    else if (je.EventTypeID == JournalTypeEnum.FSSAllBodiesFound)
                    {
                        var jdprev = prev as EliteDangerousCore.JournalEvents.JournalFSSAllBodiesFound;
                        var jd = je as EliteDangerousCore.JournalEvents.JournalFSSAllBodiesFound;

                        // throw away if same..
                        if (jdprev.SystemName == jd.SystemName && jdprev.Count == jd.Count) // if same, we just waste the repeater, ED sometimes spews out multiples
                        {
                            return true;
                        }
                    }

                }
            }

            return false;
        }

        #endregion

        #region Common info extractors

        public void ReturnSystemInfo(HistoryEntry he, out string allegiance, out string economy, out string gov,
                                out string faction, out string factionstate, out string security)
        {
            EliteDangerousCore.JournalEvents.JournalFSDJump lastfsd =
                GetLastHistoryEntry(x => x.journalEntry is EliteDangerousCore.JournalEvents.JournalFSDJump, he)?.journalEntry as EliteDangerousCore.JournalEvents.JournalFSDJump;
            // same code in spanel.. not sure where to put it
            allegiance = lastfsd != null && lastfsd.Allegiance.Length > 0 ? lastfsd.Allegiance : he.System.Allegiance.ToNullUnknownString();
            if (allegiance.IsEmpty())
                allegiance = "-";
            economy = lastfsd != null && lastfsd.Economy_Localised.Length > 0 ? lastfsd.Economy_Localised : he.System.PrimaryEconomy.ToNullUnknownString();
            if (economy.IsEmpty())
                economy = "-";
            gov = lastfsd != null && lastfsd.Government_Localised.Length > 0 ? lastfsd.Government_Localised : he.System.Government.ToNullUnknownString();
            faction = lastfsd != null && lastfsd.FactionState.Length > 0 ? lastfsd.Faction : "-";
            factionstate = lastfsd != null && lastfsd.FactionState.Length > 0 ? lastfsd.FactionState : he.System.State.ToNullUnknownString();
            factionstate = factionstate.SplitCapsWord();
            if (factionstate.IsEmpty())
                factionstate = "-";

            security = lastfsd != null && lastfsd.Security_Localised.Length > 0 ? lastfsd.Security_Localised : "-";
        }

        #endregion
    }
}
