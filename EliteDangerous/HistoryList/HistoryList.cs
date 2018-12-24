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
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using EliteDangerousCore.EDSM;

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
            return historylist.Where(s=>s.EventTimeUTC >= startutc && s.EventTimeUTC<=endutc).OrderByDescending(s => s.EventTimeUTC).ToList();
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


        public List<HistoryEntry> FilterByCommodityPricesBackwards      // full commodity price information only
        {
            get
            {
                return (from s in historylist where (s.journalEntry is JournalCommodityPricesBase && (s.journalEntry as JournalCommodityPricesBase).Commodities.Count > 0 )
                        orderby s.EventTimeUTC descending select s).ToList();
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
                else if ((resurrect && he.EntryType == JournalTypeEnum.Location) || he.EntryType == JournalTypeEnum.FSDJump)
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

        public T GetLastJournalEntry<T>(Predicate<HistoryEntry> where) where T:class             // may be Null
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


        public int GetVisitsCount(string name, long edsmid = 0)
        {
            return (from he in historylist.AsParallel()
                    where (he.IsFSDJump && (edsmid <= 0 || he.System.EDSMID == edsmid) && he.System.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    select he).Count();
        }
        public List<JournalScan> GetScans(string name, long edsmid = 0)
        {
            return (from s in historylist.AsParallel()
                    where (s.journalEntry.EventTypeID == JournalTypeEnum.Scan && (edsmid <= 0 || s.System.EDSMID == edsmid) && s.System.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    select s.journalEntry as JournalScan).ToList<JournalScan>();
        }

        public int GetFSDJumps(TimeSpan t)
        {
            DateTime tme = DateTime.UtcNow.Subtract(t);
            return (from s in historylist where s.IsFSDJump && s.EventTimeUTC >= tme select s).Count();
        }

        public int GetFSDJumps(DateTime start, DateTime to)
        {
            return (from s in historylist where s.IsFSDJump && s.EventTimeLocal >= start && s.EventTimeLocal < to select s).Count();
        }

        public int GetFSDJumps(string forShipKey)
        {
            return (from s in historylist where s.IsFSDJump && $"{s.ShipTypeFD.ToLowerInvariant()}:{s.ShipId}" == forShipKey select s).Count();
        }

        public int GetNrScans(DateTime start, DateTime to)
        {
            return (from s in historylist where s.journalEntry.EventTypeID == JournalTypeEnum.Scan && s.EventTimeLocal >= start && s.EventTimeLocal < to select s).Count();
        }

        public long GetScanValue(DateTime start, DateTime to)
        {
            var list = (from s in historylist where s.EntryType == JournalTypeEnum.Scan && s.EventTimeLocal >= start && s.EventTimeLocal < to select s.journalEntry as JournalScan).ToList<JournalScan>();

            return (from t in list select (long)t.EstimatedValue).Sum();
        }

        public int GetDocked(DateTime start, DateTime to)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.Docked && s.EventTimeLocal >= start && s.EventTimeLocal < to select s).Count();
        }

        public int GetJetConeBoost(DateTime start, DateTime to)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.JetConeBoost && s.EventTimeLocal >= start && s.EventTimeLocal < to select s).Count();
        }

        public int GetFSDBoostUsed(DateTime start, DateTime to, int boostValue = -1)
        {
            if (boostValue >= 1 && boostValue <= 3)
            {
                return (from s in historylist
                        where (s.EntryType == JournalTypeEnum.FSDJump && s.EventTimeLocal >= start && s.EventTimeLocal < to && ((JournalFSDJump)s.journalEntry).BoostValue == boostValue)
                        select s).Count();
            }
            else
            { 
                return (from s in historylist
                        where (s.EntryType == JournalTypeEnum.FSDJump && s.EventTimeLocal >= start && s.EventTimeLocal < to && ((JournalFSDJump)s.journalEntry).BoostUsed == true)
                        select s).Count();
            }
        }
                
        public int GetPlayerControlledTouchDown(DateTime start, DateTime to)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.Touchdown && s.EventTimeLocal >= start && s.EventTimeLocal < to select s)
                .ToList().ConvertAll<JournalTouchdown>(e => e.journalEntry as JournalTouchdown).Where(j => j.PlayerControlled.HasValue && j.PlayerControlled.Value).Count();
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

        public double GetTraveledLy(string forShipKey)
        {
            var list = (from s in historylist where s.EntryType == JournalTypeEnum.FSDJump && $"{s.ShipTypeFD.ToLowerInvariant()}:{s.ShipId}" == forShipKey select s.journalEntry as JournalFSDJump).ToList<JournalFSDJump>();

            return (from s in list select s.JumpDist).Sum();
        }


        public List<JournalScan> GetScanList(DateTime start, DateTime to)
        {
            return (from s in historylist where s.EntryType == JournalTypeEnum.Scan && s.EventTimeLocal >= start && s.EventTimeLocal < to select s.journalEntry as JournalScan).ToList<JournalScan>();
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
            return (from s in historylist where s.EntryType == JournalTypeEnum.Scan && $"{s.ShipTypeFD.ToLowerInvariant()}:{s.ShipId}" == forShipKey select s).Count();
        }

        public int GetFSDJumpsBeforeUTC(DateTime utc)
        {
            return (from s in historylist where s.IsFSDJump && s.EventTimeLocal < utc select s).Count();
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

        public static IEnumerable<ISystem> FindSystemsWithinLy(List<HistoryEntry> he, ISystem centre, double minrad, double maxrad, bool spherical )
        {
            IEnumerable<ISystem> list;

            if ( spherical )
                list = (from x in he where x.System.HasCoordinate && x.System.Distance(centre, minrad, maxrad) select x.System );
            else
                list = (from x in he where x.System.HasCoordinate && x.System.Cuboid(centre, minrad, maxrad) select x.System);

            return list.GroupBy(x => x.Name).Select(group => group.First());
        }

        #endregion

        #region EDSM

        public void FillInPositionsFSDJumps()       // call if you want to ensure we have the best posibile position data on FSD Jumps.  Only occurs on pre 2.1 with lazy load of just name/edsmid
        {
            List<Tuple<HistoryEntry, ISystem>> updatesystems = new List<Tuple<HistoryEntry, ISystem>>();

            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
            {
                foreach (HistoryEntry he in historylist)
                {
                    if (he.IsFSDJump && !he.System.HasCoordinate)// try and load ones without position.. if its got pos we are happy
                    {           // done in two IFs for debugging, in case your wondering why!
                        if (he.System.status != SystemStatusEnum.EDSM && he.System.EDSMID == 0)   // and its not from EDSM and we have not already tried
                        {
                            ISystem found = SystemCache.FindSystem(he.System, cn);
                            if (found != null)
                                updatesystems.Add(new Tuple<HistoryEntry, ISystem>(he, found));
                        }
                    }
                }
            }

            if (updatesystems.Count > 0)
            {
                using (SQLiteConnectionUser uconn = new SQLiteConnectionUser(utc: true))
                {
                    using (DbTransaction txn = uconn.BeginTransaction())        // take a transaction over this
                    {
                        foreach (Tuple<HistoryEntry, ISystem> he in updatesystems)
                        {
                            FillInSystemFromDBInt(he.Item1, he.Item2, uconn, txn);  // fill, we already have an EDSM system to use
                        }

                        txn.Commit();
                    }
                }
            }
        }

        public void FillEDSM(HistoryEntry syspos)       // call to fill in ESDM data for entry, and also fills in all others pointing to the system object
        {
            if (syspos.System.status == SystemStatusEnum.EDSM || syspos.System.EDSMID == -1)  // if set already, or we tried and failed..
            {
                //System.Diagnostics.Debug.WriteLine("Checked System {0} already id {1} ", syspos.System.name , syspos.System.id_edsm);
                return;
            }

            ISystem edsmsys = SystemCache.FindSystem(syspos.System);        // see if we have it..

            if (edsmsys != null)                                            // if we found it externally, fill in info
            {
                using (SQLiteConnectionUser uconn = new SQLiteConnectionUser(utc: true))        // lets do this in a transaction for speed.
                {
                    using (DbTransaction txn = uconn.BeginTransaction())
                    {
                        FillInSystemFromDBInt(syspos, edsmsys, uconn, txn); // and fill in using this connection/tx
                        txn.Commit();
                    }
                }
            }
            else
                FillInSystemFromDBInt(syspos, null, null, null);        // else fill in using null system, which means just mark it checked
        }

        private void FillInSystemFromDBInt(HistoryEntry syspos, ISystem edsmsys, SQLiteConnectionUser uconn, DbTransaction utn )       // call to fill in ESDM data for entry, and also fills in all others pointing to the system object
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

                bool updateedsmid = oldsys.EDSMID <= 0 && edsmsys.EDSMID > 0;
                bool updatesyspos = !oldsys.HasCoordinate && edsmsys.HasCoordinate;
                bool updatename = oldsys.HasCoordinate && edsmsys.HasCoordinate &&
                                  oldsys.Distance(edsmsys) < 0.1 &&
                                  !String.Equals(edsmsys.Name, oldsys.Name, StringComparison.InvariantCultureIgnoreCase) &&
                                  edsmsys.UpdateDate > syspos.EventTimeUTC;

                ISystem newsys = new SystemClass
                {
                    Name = updatename ? edsmsys.Name : oldsys.Name,
                    X = updatesyspos ? edsmsys.X : oldsys.X,
                    Y = updatesyspos ? edsmsys.Y : oldsys.Y,
                    Z = updatesyspos ? edsmsys.Z : oldsys.Z,
                    EDSMID = updateedsmid ? edsmsys.EDSMID : oldsys.EDSMID,
                    SystemAddress = oldsys.SystemAddress ?? edsmsys.SystemAddress,
                    Allegiance = oldsys.Allegiance == EDAllegiance.Unknown ? edsmsys.Allegiance : oldsys.Allegiance,
                    Government = oldsys.Government == EDGovernment.Unknown ? edsmsys.Government : oldsys.Government,
                    Population = oldsys.Government == EDGovernment.Unknown ? edsmsys.Population : oldsys.Population,
                    PrimaryEconomy = oldsys.PrimaryEconomy == EDEconomy.Unknown ? edsmsys.PrimaryEconomy : oldsys.PrimaryEconomy,
                    Security = oldsys.Security == EDSecurity.Unknown ? edsmsys.Security : oldsys.Security,
                    State = oldsys.State == EDState.Unknown ? edsmsys.State : oldsys.State,
                    Faction = oldsys.Faction ?? edsmsys.Faction,
                    CommanderCreate = edsmsys.CommanderCreate,
                    CommanderUpdate = edsmsys.CommanderUpdate,
                    CreateDate = edsmsys.CreateDate,
                    EDDBID = edsmsys.EDDBID,
                    EDDBUpdatedAt = edsmsys.EDDBUpdatedAt,
                    GridID = edsmsys.GridID,
                    NeedsPermit = edsmsys.NeedsPermit,
                    RandomID = edsmsys.RandomID,
                    UpdateDate = edsmsys.UpdateDate,
                    SystemNote = edsmsys.SystemNote,
                    status = SystemStatusEnum.EDSM
                };

                foreach (HistoryEntry he in alsomatching)       // list of systems in historylist using the same system object
                {
                    bool updatepos = (he.EntryType == JournalTypeEnum.FSDJump || he.EntryType == JournalTypeEnum.Location) && updatesyspos;

                    if (updatepos || updateedsmid)
                        JournalEntry.UpdateEDSMIDPosJump(he.Journalid, edsmsys, updatepos, -1, uconn , utn);  // update pos and edsmid, jdist not updated

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

        public void CalculateSqDistances(BaseUtils.SortedListDoubleDuplicate<ISystem> distlist, double x, double y, double z, 
                        int maxitems, double mindistance, double maxdistance , bool spherical )
        {
            HashSet<long> listids = new HashSet<long>();
            HashSet<string> listnames = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (ISystem sys in distlist.Values.ToList())
            {
                listids.Add(sys.ID);
                listnames.Add(sys.Name);
            }

            mindistance *= mindistance;

            foreach (HistoryEntry pos in historylist)
            {
                if (pos.System.HasCoordinate && !listnames.Contains(pos.System.Name) && !listids.Contains(pos.System.ID) )
                {
                    double dx = (pos.System.X - x);
                    double dy = (pos.System.Y - y);
                    double dz = (pos.System.Z - z);
                    double distsq = dx * dx + dy * dy + dz * dz;

                    listnames.Add(pos.System.Name); //stops repeats..

                    if ( distsq >= mindistance && 
                            (( spherical && distsq <= maxdistance*maxdistance) || 
                            (!spherical && Math.Abs(dx) <= maxdistance && Math.Abs(dy) <= maxdistance && Math.Abs(dz) <= maxdistance)))
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
                    {
                        hs.journalEntry.UpdateSyncFlagBit(SyncFlags.StartMarker, false, SyncFlags.StopMarker, false);
                    }
                    else if (started == false)
                    {
                        hs.journalEntry.UpdateSyncFlagBit(SyncFlags.StartMarker, true, SyncFlags.StopMarker, false);
                    }
                    else
                    {
                        hs.journalEntry.UpdateSyncFlagBit(SyncFlags.StartMarker, false, SyncFlags.StopMarker, true);
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

        // Called on a New Entry, by EDDiscoveryController:NewEntry, to add an journal entry in

        public HistoryEntry AddJournalEntry(JournalEntry je, Action<string> logerror)   // always return he
        {
            HistoryEntry prev = GetLast;

            bool journalupdate = false;
            HistoryEntry he = HistoryEntry.FromJournalEntry(je, prev, out journalupdate);     // we may check edsm for this entry

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
                he.ProcessWithUserDb(je, prev, this, conn);           // let some processes which need the user db to work

                cashledger.Process(je, conn);
                he.Credits = cashledger.CashTotal;

                shipyards.Process(je, conn);
                outfitting.Process(je, conn);

                Tuple<ShipInformation, ModulesInStore> ret = shipinformationlist.Process(je, conn,he.WhereAmI,he.System);
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
                    if (jl == null || (jl.StarSystem.Equals(jlhe.System.Name, StringComparison.InvariantCultureIgnoreCase) && !js.BodyDesignation.ToLowerInvariant().Contains(" belt cluster ")))
                    {
                        logerror("Cannot add scan to system - alert the EDDiscovery developers using either discord or Github (see help)" + Environment.NewLine +
                                            "Scan object " + js.BodyName + " in " + he.System.Name);
                    }
                }
            }
            else if (je is JournalSAAScanComplete)
            {
                starscan.AddScanToBestSystem((JournalSAAScanComplete)je, Count - 1, EntryOrder);
            }
            else if (je is JournalFSSDiscoveryScan && he.System != null)
            {
                starscan.SetFSSDiscoveryScan((JournalFSSDiscoveryScan)je, he.System);
            }
            else if (je is IBodyNameAndID)
            {
                JournalLocOrJump jl;
                HistoryEntry jlhe;
                starscan.AddBodyToBestSystem((IBodyNameAndID)je, Count - 1, EntryOrder, out jlhe, out jl);
            }

            return he;
        }

        public static HistoryList LoadHistory(EDJournalClass journalmonitor, Func<bool> cancelRequested, Action<int, string> reportProgress,
                                    string NetLogPath = null,
                                    bool ForceNetLogReload = false,
                                    bool ForceJournalReload = false,
                                    int CurrentCommander = Int32.MinValue,
                                    bool Keepuievents = true,
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
                    NetLogClass.ParseFiles(NetLogPath, out errstr, EliteConfigInstance.InstanceConfig.DefaultMapColour, () => cancelRequested(), (p, s) => reportProgress(p, s), ForceNetLogReload, currentcmdrid: CurrentCommander);
                }
            }

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Files read " );

            reportProgress(-1, "Reading Database");

            List<JournalEntry> jlist;
            
            if ( fullhistoryloaddaylimit >0 )
            {
                var list = (essentialitems == nameof(JournalEntry.JumpScanEssentialEvents)) ? JournalEntry.JumpScanEssentialEvents :
                           (essentialitems == nameof(JournalEntry.JumpEssentialEvents)) ? JournalEntry.JumpEssentialEvents :
                           (essentialitems == nameof(JournalEntry.NoEssentialEvents)) ? JournalEntry.NoEssentialEvents :
                            JournalEntry.EssentialEvents;

                jlist = JournalEntry.GetAll(CurrentCommander, 
                    ids: list,
                    allidsafter: DateTime.UtcNow.Subtract(new TimeSpan(fullhistoryloaddaylimit, 0, 0, 0))
                    ).OrderBy(x => x.EventTimeUTC).ThenBy(x => x.Id).ToList();
            }
            else
                jlist = JournalEntry.GetAll(CurrentCommander).OrderBy(x => x.EventTimeUTC).ThenBy(x => x.Id).ToList();

            Trace.WriteLine(BaseUtils.AppTicks.TickCountLap() + " Database read " + jlist.Count);

            List<Tuple<JournalEntry, HistoryEntry>> jlistUpdated = new List<Tuple<JournalEntry, HistoryEntry>>();

            using (SQLiteConnectionSystem conn = new SQLiteConnectionSystem())
            {
                HistoryEntry prev = null;
                JournalEntry jprev = null;

                reportProgress(-1, "Creating History");

                foreach (JournalEntry je in jlist)
                {
                    if (MergeEntries(jprev, je))        // if we merge.. we may have updated info, so reprint.
                    {
                        //jprev.FillInformation(out prev.EventSummary, out prev.EventDescription, out prev.EventDetailedInfo);    // need to keep this up to date..
                        continue;
                    }

                    if (je.IsUIEvent && !Keepuievents)              // filter out any UI events
                    {
                        //System.Diagnostics.Debug.WriteLine("**** Filter out " + je.EventTypeStr + " on " + je.EventTimeLocal.ToString());
                        continue;
                    }

                    bool journalupdate = false;
                    HistoryEntry he = HistoryEntry.FromJournalEntry(je, prev, out journalupdate, conn);

                    prev = he;
                    jprev = je;

                    hist.historylist.Add(he);

                    if (journalupdate)
                    {
                        jlistUpdated.Add(new Tuple<JournalEntry, HistoryEntry>(je, he));
                        Debug.WriteLine("Queued update requested {0} {1}", he.System.EDSMID, he.System.Name);
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

                            double dist = (je is JournalFSDJump) ? (je as JournalFSDJump).JumpDist : 0;
                            bool updatecoord = (je is JournalLocOrJump) ? (!(je as JournalLocOrJump).HasCoordinate && he.System.HasCoordinate) : false;

                            Debug.WriteLine("Push update {0} {1} to JE {2} HE {3}", he.System.EDSMID, he.System.Name, je.Id, he.Indexno);
                            JournalEntry.UpdateEDSMIDPosJump(je.Id, he.System, updatecoord, dist, conn, txn);
                        }

                        txn.Commit();
                    }
                }
            }

            // now database has been updated due to initial fill, now fill in stuff which needs the user database

            hist.CommanderId = CurrentCommander;

            reportProgress(-1, "Updating user statistics");

            hist.ProcessUserHistoryListEntries(h => h.ToList());      // here, we update the DBs in HistoryEntry and any global DBs in historylist

            reportProgress(-1, "Done");

            return hist;
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

                    cashledger.Process(je, conn);            // update the ledger     
                    he.Credits = cashledger.CashTotal;

                    shipyards.Process(je, conn);
                    outfitting.Process(je, conn);

                    Tuple<ShipInformation, ModulesInStore> ret = shipinformationlist.Process(je, conn, he.WhereAmI, he.System);  // the ships
                    he.ShipInformation = ret.Item1;
                    he.StoredModules = ret.Item2;

                    he.MissionList = missionlistaccumulator.Process(je, he.System, he.WhereAmI, conn);                           // the missions

                    if (je.EventTypeID == JournalTypeEnum.Scan)
                    {
                        if (!this.starscan.AddScanToBestSystem(je as JournalScan, i, hl))
                        {
                            System.Diagnostics.Debug.WriteLine("******** Cannot add scan to system " + (je as JournalScan).BodyName + " in " + he.System.Name);
                        }
                    }
                    else if (je.EventTypeID == JournalTypeEnum.SAAScanComplete)
                    {
                        this.starscan.AddScanToBestSystem((JournalSAAScanComplete)je, i, hl);
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
        }

        public static int MergeTypeDelay(JournalEntry je)   //0 = none
        {
            if (je.EventTypeID == JournalTypeEnum.Friends)
                return 2000;
            else if (je.EventTypeID == JournalTypeEnum.FSSSignalDiscovered)
                return 2000;
            else if (je.EventTypeID == JournalTypeEnum.FuelScoop)
                return 10000;
            else
                return 0;
        }

        // true if merged back to previous..
        public static bool MergeEntries(JournalEntry prev, JournalEntry je)
        {
            if (prev != null)
            {
                if (je.EventTypeID == JournalTypeEnum.FuelScoop && prev.EventTypeID == JournalTypeEnum.FuelScoop)  // merge scoops
                {
                    EliteDangerousCore.JournalEvents.JournalFuelScoop jfs = je as EliteDangerousCore.JournalEvents.JournalFuelScoop;
                    EliteDangerousCore.JournalEvents.JournalFuelScoop jfsprev = prev as EliteDangerousCore.JournalEvents.JournalFuelScoop;
                    jfsprev.Scooped += jfs.Scooped;
                    jfsprev.Total = jfs.Total;
                    //System.Diagnostics.Debug.WriteLine("Merge FS " + jfsprev.EventTimeUTC);
                    return true;
                }
                else if (je.EventTypeID == JournalTypeEnum.Friends && prev.EventTypeID == JournalTypeEnum.Friends) // merge friends
                {
                    EliteDangerousCore.JournalEvents.JournalFriends jfprev = prev as EliteDangerousCore.JournalEvents.JournalFriends;
                    EliteDangerousCore.JournalEvents.JournalFriends jf = je as EliteDangerousCore.JournalEvents.JournalFriends;
                    jfprev.AddFriend(jf);
                    //System.Diagnostics.Debug.WriteLine("Merge Friends " + jfprev.EventTimeUTC + " " + jfprev.NameList.Count);
                    return true;
                }
                else if (je.EventTypeID == JournalTypeEnum.FSSSignalDiscovered && prev.EventTypeID == JournalTypeEnum.FSSSignalDiscovered) // merge friends
                {
                    var jdprev = prev as EliteDangerousCore.JournalEvents.JournalFSSSignalDiscovered;
                    var jd = je as EliteDangerousCore.JournalEvents.JournalFSSSignalDiscovered;
                    jdprev.Add(jd);
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Common info extractors

        #endregion

        public void ReturnSystemInfo(HistoryEntry he, out string allegiance, out string economy, out string gov, 
                                out string faction, out string factionstate , out string security)
        {
            EliteDangerousCore.JournalEvents.JournalFSDJump lastfsd =
                GetLastHistoryEntry(x => x.journalEntry is EliteDangerousCore.JournalEvents.JournalFSDJump, he)?.journalEntry as EliteDangerousCore.JournalEvents.JournalFSDJump;
            // same code in spanel.. not sure where to put it
            allegiance = lastfsd != null && lastfsd.Allegiance.Length > 0 ? lastfsd.Allegiance : he.System.Allegiance.ToNullUnknownString();
            economy = lastfsd != null && lastfsd.Economy_Localised.Length > 0 ? lastfsd.Economy_Localised : he.System.PrimaryEconomy.ToNullUnknownString();
            gov = lastfsd != null && lastfsd.Government_Localised.Length > 0 ? lastfsd.Government_Localised : he.System.Government.ToNullUnknownString();
            faction = lastfsd != null && lastfsd.FactionState.Length > 0 ? lastfsd.Faction : "Unknown Faction";
            factionstate = lastfsd != null && lastfsd.FactionState.Length > 0 ? lastfsd.FactionState : he.System.State.ToNullUnknownString();
            factionstate = factionstate.SplitCapsWord();
            security = lastfsd != null && lastfsd.Security_Localised.Length > 0 ? lastfsd.Security_Localised : "Unknown Security";
        }
    }
}
