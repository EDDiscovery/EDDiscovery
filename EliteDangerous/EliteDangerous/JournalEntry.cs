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
using EDDiscovery.Icons;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EliteDangerousCore
{
    public enum JournalTypeEnum
    {
        Unknown = 0,

        AfmuRepairs = 3,
        ApproachBody = 4,
        ApproachSettlement = 5,
        AppliedToSquadron = 6,
        AsteroidCracked = 7,
        Bounty = 10,
        BuyAmmo = 20,
        BuyDrones = 30,
        BuyExplorationData = 40,
        BuyTradeData = 50,
        CapShipBond = 60,
        Cargo = 63,
        CargoDepot = 64,
        ChangeCrewRole = 65,
        ClearSavedGame = 70,
        CockpitBreached = 80,
        CodexEntry = 85,
        CollectCargo = 90,
        Commander = 95,
        CommitCrime = 100,
        CommunityGoal = 109,
        CommunityGoalJoin = 110,
        CommunityGoalReward = 120,
        CommunityGoalDiscard = 1040,
        Continued = 125,
        CrewAssign = 126,
        CrewFire = 127,
        CrewHire = 128,
        CrewLaunchFighter = 1268,
        CrewMemberJoins = 1270,
        CrewMemberQuits = 1280,
        CrewMemberRoleChange = 1285,
        CrimeVictim = 129,
        DataScanned = 1030,
        DatalinkScan = 130,
        DatalinkVoucher = 1020,
        Died = 140,
        DiscoveryScan = 141,
        DisbandedSquadron = 142,
        Docked = 145,
        DockFighter = 150,
        DockingCancelled = 160,
        DockingDenied = 170,
        DockingGranted = 180,
        DockingRequested = 190,
        DockingTimeout = 200,
        DockSRV = 210,
        EjectCargo = 220,
        EndCrewSession = 225,
        EngineerApply = 230,
        EngineerContribution = 235,
        EngineerCraft = 240,
        EngineerLegacyConvert = 241,
        EngineerProgress = 250,
        EscapeInterdiction = 260,
        FactionKillBond = 270,
        FetchRemoteModule = 1000,
        FSDJump = 280,
        FSDTarget = 281,
        FSSAllBodiesFound = 285,
        FuelScoop = 290,
        Fileheader = 300,
        FighterDestroyed = 303,
        FighterRebuilt = 304,
        Friends = 305,
        FSSDiscoveryScan = 306,
        FSSSignalDiscovered = 307,
        HeatDamage = 310,
        HeatWarning = 320,
        HullDamage = 330,
        Interdicted = 340,
        Interdiction = 350,
        InvitedToSquadron = 351,
        JoinedSquadron = 353,
        JetConeBoost = 354,
        JetConeDamage = 355,
        JoinACrew = 356,
        KickCrewMember = 357,
        KickedFromSquadron = 358,
        LaunchDrone = 359,
        LaunchFighter = 360,
        LaunchSRV = 370,
        LeftSquadron = 371,
        LeaveBody = 375,
        Liftoff = 380,
        LoadGame = 390,
        Loadout = 395,
        Location = 400,
        MassModuleStore = 1010,
        Market = 405,
        MarketBuy = 410,
        MarketSell = 420,
        MaterialCollected = 430,
        MaterialDiscarded = 440,
        MaterialDiscovered = 450,
        MaterialTrade = 451,
        Materials = 455,
        MiningRefined = 460,
        Missions = 465,
        MissionAbandoned = 470,
        MissionAccepted = 480,
        MissionCompleted = 490,
        MissionFailed = 500,
        MissionRedirected = 505,
        ModuleInfo = 508,
        ModuleBuy = 510,
        ModuleRetrieve = 515,
        ModuleSell = 520,
        ModuleSellRemote = 990,
        ModuleStore = 525,
        ModuleSwap = 530,
        MultiSellExplorationData = 533,
        Music = 535,
        NavBeaconScan = 538,
        NewCommander = 540,
        NpcCrewPaidWage = 541,
        NpcCrewRank = 542,
        Outfitting = 543,
        Passengers = 545,
        PayFines = 550,
        PayBounties = 551,
        PayLegacyFines = 560,
        Powerplay = 565,
        PowerplayCollect = 570,
        PowerplayDefect = 580,
        PowerplayDeliver = 590,
        PowerplayFastTrack = 600,
        PowerplayJoin = 610,
        PowerplayLeave = 620,
        PowerplaySalary = 630,
        PowerplayVote = 640,
        PowerplayVoucher = 650,
        Progress = 660,
        Promotion = 670,
        ProspectedAsteroid = 673,
        PVPKill = 675,
        QuitACrew = 677,
        Rank = 680,
        RebootRepair = 690,
        ReceiveText = 700,
        RedeemVoucher = 710,
        RefuelAll = 720,
        RefuelPartial = 730,
        Repair = 740,
        RepairAll = 745,
        RepairDrone = 747,
        Reputation = 748,
        RestockVehicle = 750,
        Resurrect = 760,
        ReservoirReplenished = 763,
        SAAScanComplete = 765,
        Scan = 770,
        Scanned = 772,
        ScientificResearch = 775,
        Screenshot = 780,
        SearchAndRescue = 785,
        SelfDestruct = 790,
        SellDrones = 800,
        SellExplorationData = 810,
        SellShipOnRebuy = 815,
        SendText = 820,
        SetUserShipName = 825,
        SharedBookmarkToSquadron = 826,
        ShieldState = 830,
        Shipyard = 837,
        ShipyardBuy = 840,
        ShipyardNew = 850,
        ShipyardSell = 860,
        ShipyardSwap = 870,
        ShipyardTransfer = 880,
        ShipTargeted = 881,
        Shutdown = 882,
        SRVDestroyed = 883,
        StartJump = 885,
        Statistics = 888,
        StoredModules = 886,
        StoredShips = 887,
        SupercruiseEntry = 890,
        SquadronCreated = 891,
        SquadronDemotion = 892,
        SquadronPromotion = 893,
        SquadronStartup = 894,
        SupercruiseExit = 900,
        Synthesis = 910,
        SystemsShutdown = 915,
        TechnologyBroker = 918,
        Touchdown = 920,
        UnderAttack = 925,
        Undocked = 930,
        USSDrop = 940,
        VehicleSwitch = 950,
        WingAdd = 960,
        WingInvite = 965,
        WingJoin = 970,
        WingLeave = 980,
        WonATrophyForSquadron = 985,

        // EDD Entries

        EDDItemSet = 2000,
        EDDCommodityPrices = 2010,

        // Specials Event IDs for ICON selection - alternate Icons for these events
        RestockVehicle_SRV = 10750,
        RestockVehicle_Fighter = 10751,
        ShieldState_ShieldsUp = 10830,
        ShieldState_ShieldsDown = 10831,
        VehicleSwitch_Mothership = 10950,
        VehicleSwitch_Fighter = 10951,
    }

    public enum SyncFlags
    {
        NoBit = 0,                      // for sync change func only
        EDSM = 0x01,
        EDDN = 0x02,
        EGO = 0x04,
        StartMarker = 0x0100,           // measure distance start pos marker
        StopMarker = 0x0200,            // measure distance stop pos marker
    }

    [DebuggerDisplay("Event {EventTypeStr} {EventTimeUTC} EdsmID {EdsmID} JID {Id} C {CommanderId}")]
    public abstract class JournalEntry
    {
        #region Instance properties and fields
        public long Id { get; private set; }                    // this is the entry ID
        public long TLUId { get; private set; }                 // this ID of the journal tlu (aka TravelLogId)
        public int CommanderId { get; private set; }            // commander Id of entry

        public JournalTypeEnum EventTypeID { get; private set; }
        public string EventTypeStr { get { return EventTypeID.ToString(); } }             // name of event. these two duplicate each other, string if for debuggin in the db view of a browser

        public System.Drawing.Image Icon { get { return JournalTypeIcons.ContainsKey(this.IconEventType) ? JournalTypeIcons[this.IconEventType] : JournalTypeIcons[JournalTypeEnum.Unknown]; } }   // Icon to paint for this

        public DateTime EventTimeUTC { get;  set; }

        public long EdsmID { get; protected set; }                      // 0 = unassigned, >0 = assigned

        private int Synced { get; set; }                     // sync flags

        public DateTime EventTimeLocal { get { return EventTimeUTC.ToLocalTime(); } }

        public bool SyncedEDSM { get { return (Synced & (int)SyncFlags.EDSM) != 0; } }
        public bool SyncedEDDN { get { return (Synced & (int)SyncFlags.EDDN) != 0; } }
        public bool SyncedEGO { get { return (Synced & (int)SyncFlags.EGO) != 0; } }
        public bool StartMarker { get { return (Synced & (int)SyncFlags.StartMarker) != 0; } }
        public bool StopMarker { get { return (Synced & (int)SyncFlags.StopMarker) != 0; } }
        private bool? beta;                        // True if journal entry is from beta
        public virtual bool Beta
        {
            get
            {
                if (beta == null)
                {
                    TravelLogUnit tlu = TravelLogUnit.Get(TLUId);
                    beta = tlu?.Beta ?? false;
                }

                return beta ?? false;
            }
        }

        public bool IsUIEvent { get { return this is IUIEvent; } }

        public void SetTLUCommander(long t, int cmdr)         // used during log reading..
        {
            TLUId = t;
            CommanderId = cmdr;
        }
        public void SetCommander(int cmdr)         // used during log reading..
        {
            CommanderId = cmdr;
        }
        public void SetEDSMId(long edsmid)          // used if edsm id is changed
        {
            EdsmID = edsmid;
        }

        #endregion

        #region Static properties and fields

        static public JournalTypeEnum[] EssentialEvents = new JournalTypeEnum[]     // 
            {
                // due to materials/commodities
                JournalTypeEnum.Cargo, JournalTypeEnum.CargoDepot,JournalTypeEnum.CollectCargo,
                JournalTypeEnum.EjectCargo,
                JournalTypeEnum.EngineerContribution,
                JournalTypeEnum.EngineerCraft, JournalTypeEnum.MarketBuy, JournalTypeEnum.MarketSell,
                JournalTypeEnum.MaterialCollected, JournalTypeEnum.MaterialDiscarded, JournalTypeEnum.Materials, JournalTypeEnum.MaterialTrade,
                JournalTypeEnum.Synthesis, JournalTypeEnum.TechnologyBroker,

                // Missions
                JournalTypeEnum.MissionAccepted, JournalTypeEnum.MissionCompleted, JournalTypeEnum.MissionAbandoned, JournalTypeEnum.MissionFailed, JournalTypeEnum.MissionRedirected,

                // Combat
                JournalTypeEnum.Bounty, JournalTypeEnum.CommitCrime, JournalTypeEnum.FactionKillBond,  JournalTypeEnum.PVPKill,
                JournalTypeEnum.Died, JournalTypeEnum.Resurrect, JournalTypeEnum.SelfDestruct, 

                // Journey
                JournalTypeEnum.FSDJump, JournalTypeEnum.Location, JournalTypeEnum.Docked,

                // Ship state
                JournalTypeEnum.Loadout, JournalTypeEnum.MassModuleStore, JournalTypeEnum.ModuleBuy, JournalTypeEnum.ModuleSell,
                JournalTypeEnum.ModuleRetrieve,
                JournalTypeEnum.ModuleSellRemote, JournalTypeEnum.ModuleStore, JournalTypeEnum.ModuleSwap, JournalTypeEnum.SellShipOnRebuy,
                JournalTypeEnum.SetUserShipName, JournalTypeEnum.ShipyardBuy, JournalTypeEnum.ShipyardNew, JournalTypeEnum.ShipyardSell,
                JournalTypeEnum.ShipyardSwap , JournalTypeEnum.ShipyardTransfer, JournalTypeEnum.StoredModules, JournalTypeEnum.StoredShips,

                // scan
                JournalTypeEnum.Scan, JournalTypeEnum.SellExplorationData, 

                // misc
                JournalTypeEnum.ClearSavedGame,
            };

        static public JournalTypeEnum[] FullStatsEssentialEvents
        {
            get
            {
                var statsAdditional = new JournalTypeEnum[]
                {
                    // Travel
                    JournalTypeEnum.JetConeBoost, JournalTypeEnum.Touchdown, JournalTypeEnum.HeatWarning, JournalTypeEnum.HeatDamage,
                    JournalTypeEnum.FuelScoop, JournalTypeEnum.SAAScanComplete
                };
                return EssentialEvents.Concat(statsAdditional).ToArray();
            }
        }

        static public JournalTypeEnum[] JumpScanEssentialEvents = new JournalTypeEnum[]     // 
            {
                JournalTypeEnum.FSDJump,
                JournalTypeEnum.Scan,
            };
        static public JournalTypeEnum[] JumpEssentialEvents = new JournalTypeEnum[]     // 
            {
                JournalTypeEnum.FSDJump,
            };

        static public JournalTypeEnum[] NoEssentialEvents = new JournalTypeEnum[]     // 
            {
            };

        private static Dictionary<JournalTypeEnum, Type> JournalEntryTypes = GetJournalEntryTypes();        // enum -> type

        /// <summary>
        /// Gets the mapping of journal type value to JournalEntry type
        /// </summary>
        /// <returns>Map of type values to types</returns>
        private static Dictionary<JournalTypeEnum, Type> GetJournalEntryTypes()
        {
            Dictionary<JournalTypeEnum, Type> typedict = new Dictionary<JournalTypeEnum, Type>();
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var types = asm.GetTypes().Where(t => typeof(JournalEntry).IsAssignableFrom(t) && !t.IsAbstract).ToList();

            foreach (Type type in types)
            {
                JournalEntryTypeAttribute typeattrib = type.GetCustomAttributes(false).OfType<JournalEntryTypeAttribute>().FirstOrDefault();
                if (typeattrib != null)
                {
                    typedict[typeattrib.EntryType] = type;
                }
            }

            return typedict;
        }

        // enum -> icons 

        public static IReadOnlyDictionary<JournalTypeEnum, Image> JournalTypeIcons { get; } = new IconGroup<JournalTypeEnum>("Journal");

        // enum -> Summary name

        private static Dictionary<JournalTypeEnum, string> SummaryNames = GetJournalSummaryNames();     // precompute the names due to the expense of splitcapsword

        private static Dictionary<JournalTypeEnum, string> GetJournalSummaryNames()
        {
            var v = Enum.GetValues(typeof(JournalTypeEnum)).OfType<JournalTypeEnum>();
            return v.ToDictionary(e => e, e => e.ToString().SplitCapsWord().Tx(typeof(JournalTypeEnum),e.ToString()));
        }

        #endregion

        #region Formatting control and Icons

        protected virtual JournalTypeEnum IconEventType     // entry may be overridden to dynamically change icon event for an event
        {
            get
            {
                return EventTypeID;
            }
        }

        public abstract void FillInformation(out string info, out string detailed);     // all entries must implement

        public virtual string SummaryName(ISystem sys) { return SummaryNames.ContainsKey(EventTypeID) ? SummaryNames[EventTypeID] : EventTypeID.ToString(); }  // entry may be overridden for specialist output

        #endregion

        #region Creation

        public JournalEntry(DateTime utc, int synced , JournalTypeEnum jtype)       // manual creation via NEW
        {
            EventTypeID = jtype;
            //EventSummaryName = FillSummary;     // after creation, so journal fields are populated.
            EventTimeUTC = utc;
            Synced = synced;
            TLUId = 0;
        }

        public JournalEntry(JObject jo, JournalTypeEnum jtype)              // called by journal entries to create themselves
        {
            EventTypeID = jtype;
            if (DateTime.TryParse(jo["timestamp"].Str(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out DateTime etime))
                EventTimeUTC = etime;
            else
                EventTimeUTC = DateTime.MinValue;
            TLUId = 0;
        }

        static public JournalEntry CreateJournalEntry(DataRow dr)
        {
            string EDataString = (string)dr["EventData"];

            JournalEntry jr = JournalEntry.CreateJournalEntry(EDataString);     // this sets EventTypeId, EventTypeStr and UTC via constructor above.. 

            jr.Id = (int)(long)dr["Id"];
            jr.TLUId = (int)(long)dr["TravelLogId"];
            jr.CommanderId = (int)(long)dr["CommanderId"];
            if (jr.EventTimeUTC == default(DateTime))
                jr.EventTimeUTC = (DateTime)dr["EventTime"];
            if (jr.EventTypeID == JournalTypeEnum.Unknown)
                jr.EventTypeID = (JournalTypeEnum)(long)dr["eventTypeID"];
            jr.EdsmID = (long)dr["EdsmID"];
            jr.Synced = (int)(long)dr["Synced"];
            return jr;
        }

        static public JournalEntry CreateJournalEntry(DbDataReader dr)
        {
            string EDataString = (string)dr["EventData"];

            JournalEntry jr = JournalEntry.CreateJournalEntry(EDataString);

            jr.Id = (int)(long)dr["Id"];
            jr.TLUId = (int)(long)dr["TravelLogId"];
            jr.CommanderId = (int)(long)dr["CommanderId"];
            if (jr.EventTimeUTC == default(DateTime))
                jr.EventTimeUTC = ((DateTime)dr["EventTime"]).ToUniversalTime();
            if (jr.EventTypeID == JournalTypeEnum.Unknown)
                jr.EventTypeID = (JournalTypeEnum)(long)dr["eventTypeID"];
            jr.EdsmID = (long)dr["EdsmID"];
            jr.Synced = (int)(long)dr["Synced"];
            return jr;
        }

        #endregion

        #region DB

        public bool Add(JObject jo)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                bool ret = Add(jo, cn);
                return ret;
            }
        }

        public bool Add(JObject jo, SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into JournalEntries (EventTime, TravelLogID, CommanderId, EventTypeId , EventType, EventData, EdsmId, Synced) values (@EventTime, @TravelLogID, @CommanderID, @EventTypeId , @EventStrName, @EventData, @EdsmId, @Synced)", tn))
            {
                cmd.AddParameterWithValue("@EventTime", EventTimeUTC);           // MUST use UTC connection
                cmd.AddParameterWithValue("@TravelLogID", TLUId);
                cmd.AddParameterWithValue("@CommanderID", CommanderId);
                cmd.AddParameterWithValue("@EventTypeId", EventTypeID);
                cmd.AddParameterWithValue("@EventStrName", EventTypeStr);
                cmd.AddParameterWithValue("@EventData", jo.ToString());
                cmd.AddParameterWithValue("@EdsmId", EdsmID);
                cmd.AddParameterWithValue("@Synced", Synced);

                cn.SQLNonQueryText( cmd);

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from JournalEntries"))
                {
                    Id = (int)(long)cn.SQLScalar( cmd2);
                }
                return true;
            }
        }

        public bool Update()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("Update JournalEntries set EventTime=@EventTime, TravelLogID=@TravelLogID, CommanderID=@CommanderID, EventTypeId=@EventTypeId, EventType=@EventStrName, EdsmId=@EdsmId, Synced=@Synced where ID=@id", tn))
            {
                cmd.AddParameterWithValue("@ID", Id);
                cmd.AddParameterWithValue("@EventTime", EventTimeUTC);  // MUST use UTC connection
                cmd.AddParameterWithValue("@TravelLogID", TLUId);
                cmd.AddParameterWithValue("@CommanderID", CommanderId);
                cmd.AddParameterWithValue("@EventTypeId", EventTypeID);
                cmd.AddParameterWithValue("@EventStrName", EventTypeStr);
                cmd.AddParameterWithValue("@EdsmId", EdsmID);
                cmd.AddParameterWithValue("@Synced", Synced);
                cn.SQLNonQueryText( cmd);

                return true;
            }
        }

        public void UpdateJsonEntry(JObject jo, SQLiteConnectionUser cn = null, DbTransaction tn = null)
        {
            bool ownconn = false;

            try
            {
                if (cn == null)
                {
                    ownconn = true;
                    cn = new SQLiteConnectionUser(utc: true);
                }

                using (DbCommand cmd = cn.CreateCommand("Update JournalEntries set EventData=@EventData where ID=@id", tn))
                {
                    cmd.AddParameterWithValue("@ID", Id);
                    cmd.AddParameterWithValue("@EventData", jo.ToString());
                    cn.SQLNonQueryText( cmd);
                }
            }
            finally
            {
                if (ownconn)
                {
                    cn.Dispose();
                }
            }
        }

        static public void Delete(long idvalue)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                Delete(idvalue, cn);
            }
        }

        static public void Delete(long idvalue, SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("DELETE FROM JournalEntries WHERE id = @id"))
            {
                cmd.AddParameterWithValue("@id", idvalue);
                cn.SQLNonQueryText( cmd);
            }
        }

        //dist >0 to update
        public static void UpdateEDSMIDPosJump(long journalid, ISystem system, bool jsonpos, double dist, SQLiteConnectionUser cn = null, DbTransaction tn = null)
        {
            bool ownconn = false;

            try
            {
                if (cn == null)
                {
                    ownconn = true;
                    cn = new SQLiteConnectionUser(utc: true);
                }

                bool updatejson = jsonpos || dist > 0;

                JObject jo = updatejson ? GetJson(journalid, cn, tn) : null;       // if JSON pos update, get it, else null
                                                                                    // no need to JSON read if just doing an EDSM update
                if (jo != null || !updatejson )        // if got it, or no pos
                {
                    if (jsonpos)
                    {
                        jo["StarPos"] = new JArray() { system.X, system.Y, system.Z };
                        jo["StarPosFromEDSM"] = true;
                    }

                    if (dist > 0)
                        jo["JumpDist"] = dist;

                    using (DbCommand cmd2 = cn.CreateCommand("Update JournalEntries set EdsmId = @EdsmId where ID = @ID", tn))
                    {
                        if ( updatejson )
                        {
                            cmd2.CommandText = "Update JournalEntries set EventData = @EventData, EdsmId = @EdsmId where ID = @ID";
                            cmd2.AddParameterWithValue("@EventData", jo.ToString());
                            System.Diagnostics.Trace.WriteLine(string.Format("Update journal ID {0} with pos/edsmid {1} dist {2}", journalid, system.EDSMID, dist));
                        }
                        else
                        {
                            System.Diagnostics.Trace.WriteLine(string.Format("Update journal ID {0} with edsmid {1}", journalid, system.EDSMID));
                        }

                        cmd2.AddParameterWithValue("@ID", journalid);
                        cmd2.AddParameterWithValue("@EdsmId", system.EDSMID);

                        cmd2.ExecuteNonQuery();
                    }
                }
            }
            finally
            {
                if (ownconn)
                {
                    cn.Dispose();
                }
            }
        }

        public void UpdateSyncFlagBit(SyncFlags bit1, bool value1, SyncFlags bit2, bool value2 , SQLiteConnectionUser cn = null, DbTransaction txn = null)
        {
            bool closeConn = false;

            try
            {
                if (cn == null)
                {
                    closeConn = true;
                    cn = new SQLiteConnectionUser(utc: true);
                }

                if (value1)
                    Synced |= (int)bit1;
                else
                    Synced &= ~(int)bit1;

                if (value2)
                    Synced |= (int)bit2;
                else
                    Synced &= ~(int)bit2;

                using (DbCommand cmd = cn.CreateCommand("Update JournalEntries set Synced = @sync where ID=@journalid", txn))
                {
                    cmd.AddParameterWithValue("@journalid", Id);
                    cmd.AddParameterWithValue("@sync", Synced);
                    System.Diagnostics.Trace.WriteLine(string.Format("Update sync flag ID {0} with {1}", Id , Synced));
                    cn.SQLNonQueryText( cmd);
                }
            }
            finally
            {
                if (closeConn && cn != null)
                {
                    cn.Dispose();
                }
            }
        }

        public void UpdateCommanderID(int cmdrid)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                using (DbCommand cmd = cn.CreateCommand("Update JournalEntries set CommanderID = @cmdrid where ID=@journalid"))
                {
                    cmd.AddParameterWithValue("@journalid", Id);
                    cmd.AddParameterWithValue("@cmdrid", cmdrid);
                    System.Diagnostics.Trace.WriteLine(string.Format("Update cmdr id ID {0} with map colour", Id));
                    cn.SQLNonQueryText( cmd);
                    CommanderId = cmdrid;
                }
            }
        }

        static public bool ResetCommanderID(int from, int to)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                using (DbCommand cmd = cn.CreateCommand("Update JournalEntries set CommanderID = @cmdridto where CommanderID=@cmdridfrom"))
                {
                    if (from == -1)
                        cmd.CommandText = "Update JournalEntries set CommanderID = @cmdridto";

                    cmd.AddParameterWithValue("@cmdridto", to);
                    cmd.AddParameterWithValue("@cmdridfrom", from);
                    System.Diagnostics.Trace.WriteLine(string.Format("Update cmdr id ID {0} with {1}", from, to));
                    cn.SQLNonQueryText( cmd);
                }
            }
            return true;
        }

        public static JournalEDDCommodityPrices AddEDDCommodityPrices(int cmdrid, DateTime dt, string station, string faction, JArray jcommodities)     // add item, return journal ID
        {
            JObject jo;
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                jo = new JObject();
                jo["timestamp"] = dt;
                jo["event"] = JournalTypeEnum.EDDCommodityPrices.ToString();
                jo["station"] = station;
                jo["faction"] = faction;
                jo["commodities"] = jcommodities;
                JournalEDDCommodityPrices jis = new JournalEDDCommodityPrices(jo);
                jis.CommanderId = cmdrid;
                jis.Add(jo, cn);

                if (jis.Commodities.Count == 0)
                    return null;
                else
                    return jis;
            }
        }

        public JObject GetJson()
        {
            return GetJson(Id);
        }

        static public JObject GetJson(long journalid)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                return GetJson(journalid, cn);
            }
        }

        static public JObject GetJson(long journalid, SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("select EventData from JournalEntries where ID=@journalid", tn))
            {
                cmd.AddParameterWithValue("@journalid", journalid);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string EDataString = (string)reader["EventData"];

                        try
                        {
                            return JObject.Parse(EDataString);
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine($"Error parsing journal entry\n{EDataString}\n{ex.ToString()}");
                            return null;
                        }
                    }
                }
            }

            return null;
        }

        static public JournalEntry Get(long journalid)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                return Get(journalid, cn);
            }
        }

        static public JournalEntry Get(long journalid, SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("select * from JournalEntries where ID=@journalid", tn))
            {
                cmd.AddParameterWithValue("@journalid", journalid);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return CreateJournalEntry(reader);
                    }
                }
            }

            return null;
        }

        static public List<JournalEntry> Get(string eventtype)            // any commander, find me an event of this type..
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                return Get(eventtype, cn);
            }
        }

        static public List<JournalEntry> Get(string eventtype, SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            Dictionary<long, TravelLogUnit> tlus = TravelLogUnit.GetAll().ToDictionary(t => t.id);

            using (DbCommand cmd = cn.CreateCommand("select * from JournalEntries where EventType=@ev", tn))
            {
                cmd.AddParameterWithValue("@ev", eventtype);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    List<JournalEntry> entries = new List<JournalEntry>();

                    while (reader.Read())
                    {
                        JournalEntry je = CreateJournalEntry(reader);
                        je.beta = tlus.ContainsKey(je.TLUId) ? tlus[je.TLUId].Beta : false;
                        entries.Add(je);
                    }

                    return entries;
                }
            }
        }

        static public List<JournalEntry> GetAll(int commander = -999, DateTime? after = null, DateTime? before = null , 
                            JournalTypeEnum[] ids = null , DateTime? allidsafter = null )
        {
            Dictionary<long, TravelLogUnit> tlus = TravelLogUnit.GetAll().ToDictionary(t => t.id);

            List<JournalEntry> list = new List<JournalEntry>();

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                using (DbCommand cmd = cn.CreateCommand("select * from JournalEntries"))
                {
                    string cnd = "";
                    if (commander != -999)
                    {
                        cnd = cnd.AppendPrePad("CommanderID = @commander", " and ");
                        cmd.AddParameterWithValue("@commander", commander);
                    }
                    if (after != null)
                    {
                        cnd = cnd.AppendPrePad("EventTime >= @after", " and ");
                        cmd.AddParameterWithValue("@after", after.Value);
                    }
                    if (before != null)
                    {
                        cnd = cnd.AppendPrePad("EventTime <= @before", " and ");
                        cmd.AddParameterWithValue("@before", before.Value);
                    }
                    if ( ids != null )
                    {
                        int[] array = Array.ConvertAll(ids, x => (int)x);
                        if (allidsafter != null)
                        {
                            cmd.AddParameterWithValue("@idafter", allidsafter.Value);
                            cnd = cnd.AppendPrePad("(EventTypeId in (" + string.Join(",", array) + ") Or EventTime>=@idafter)", " and ");
                        }
                        else
                        {
                            cnd = cnd.AppendPrePad("EventTypeId in (" + string.Join(",", array) + ")", " and ");
                        }
                    }

                    if (cnd.HasChars())
                        cmd.CommandText += " where " + cnd;

                    cmd.CommandText += " Order By EventTime ASC";

                    DataSet ds = cn.SQLQueryText( cmd);

                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return list;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        JournalEntry sys = JournalEntry.CreateJournalEntry(dr);
                        sys.beta = tlus.ContainsKey(sys.TLUId) ? tlus[sys.TLUId].Beta : false;
                        list.Add(sys);
                    }

                    return list;
                }
            }
        }


        public static List<JournalEntry> GetByEventType(JournalTypeEnum eventtype, int commanderid, DateTime start, DateTime stop)
        {
            Dictionary<long, TravelLogUnit> tlus = TravelLogUnit.GetAll().ToDictionary(t => t.id);

            List<JournalEntry> vsc = new List<JournalEntry>();

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT * FROM JournalEntries WHERE EventTypeID = @eventtype and  CommanderID=@commander and  EventTime >=@start and EventTime<=@Stop ORDER BY EventTime ASC"))
                {
                    cmd.AddParameterWithValue("@eventtype", (int)eventtype);
                    cmd.AddParameterWithValue("@commander", (int)commanderid);
                    cmd.AddParameterWithValue("@start", start);
                    cmd.AddParameterWithValue("@stop", stop);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JournalEntry je = CreateJournalEntry(reader);
                            je.beta = tlus.ContainsKey(je.TLUId) ? tlus[je.TLUId].Beta : false;
                            vsc.Add(je);
                        }
                    }
                }
            }
            return vsc;
        }



        public static List<JournalEntry> GetAllByTLU(long tluid)
        {
            TravelLogUnit tlu = TravelLogUnit.Get(tluid);
            List<JournalEntry> vsc = new List<JournalEntry>();

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT * FROM JournalEntries WHERE TravelLogId = @source ORDER BY EventTime ASC"))
                {
                    cmd.AddParameterWithValue("@source", tluid);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JournalEntry je = CreateJournalEntry(reader);
                            je.beta = tlu?.Beta ?? false;
                            vsc.Add(je);
                        }
                    }
                }
            }
            return vsc;
        }

        public static JournalEntry GetLast(int cmdrid, DateTime before, Func<JournalEntry, bool> filter)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT * FROM JournalEntries WHERE CommanderId = @cmdrid AND EventTime < @time ORDER BY EventTime DESC"))
                {
                    cmd.AddParameterWithValue("@cmdrid", cmdrid);
                    cmd.AddParameterWithValue("@time", before);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JournalEntry ent = CreateJournalEntry(reader);
                            if (filter(ent))
                            {
                                return ent;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public static JournalEntry GetLast(DateTime before, Func<JournalEntry, bool> filter)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT * FROM JournalEntries WHERE EventTime < @time ORDER BY EventTime DESC"))
                {
                    cmd.AddParameterWithValue("@time", before);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JournalEntry ent = CreateJournalEntry(reader);
                            if (filter(ent))
                            {
                                return ent;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public static T GetLast<T>(int cmdrid, DateTime before, Func<T, bool> filter = null)
            where T : JournalEntry
        {
            return (T)GetLast(cmdrid, before, e => e is T && (filter == null || filter((T)e)));
        }

        public static T GetLast<T>(DateTime before, Func<T, bool> filter = null)
            where T : JournalEntry
        {
            return (T)GetLast(before, e => e is T && (filter == null || filter((T)e)));
        }

        public static JObject RemoveEDDGeneratedKeys(JObject obj)      // obj not changed
        {
            JObject jcopy = null;

            foreach (JProperty prop in obj.Properties().ToList())
            {
                if (prop.Name.StartsWith("EDD") || prop.Name.Equals("StarPosFromEDSM"))//|| (removeLocalised && prop.Name.EndsWith("_Localised")))
                {
                    if (jcopy == null)      // only pay the expense if it has one of the entries in it
                        jcopy = (JObject)obj.DeepClone();

                    jcopy.Remove(prop.Name);
                }
            }

            return jcopy != null ? jcopy : obj;
        }

        // optionally pass in json for speed reasons.  Guaranteed that ent1jo and 2 are not altered by the compare!
        public static bool AreSameEntry(JournalEntry ent1, JournalEntry ent2, JObject ent1jo = null, JObject ent2jo = null)
        {
            if (ent1jo == null && ent1 != null)
            {
                ent1jo = GetJson(ent1.Id);      // read from db the json since we don't have it
            }

            if (ent2jo == null && ent2 != null)
            {
                ent2jo = GetJson(ent2.Id);      // read from db the json since we don't have it
            }

            if (ent1jo == null || ent2jo == null)
            {
                return false;       
            }

            //System.Diagnostics.Debug.WriteLine("Compare " + ent1jo.ToString() + " with " + ent2jo.ToString());

            // Fixed problem #1518, Prev. the remove was only done on GetJson's above.  
            // during a scan though, ent1jo is filled in, so the remove was not being performed on ent1jo.
            // So if your current map colour was different in FSD entries then
            // the newly created entry would differ from the db version by map colour - causing #1518
            // secondly, this function should not alter ent1jo/ent2jo as its a compare function.  it was.  Change RemoveEDDGenKeys to copy if it alters it.

            JObject ent1jorm = RemoveEDDGeneratedKeys(ent1jo);     // remove keys, but don't alter originals as they can be used later 
            JObject ent2jorm = RemoveEDDGeneratedKeys(ent2jo);

            return JToken.DeepEquals(ent1jorm, ent2jorm);
        }

        public static List<JournalEntry> FindEntry(JournalEntry ent, JObject entjo = null)      // entjo is not changed.
        {
            List<JournalEntry> entries = new List<JournalEntry>();
            if (entjo == null)
            {
                entjo = GetJson(ent.Id);
            }

            entjo = RemoveEDDGeneratedKeys(entjo);

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT * FROM JournalEntries WHERE CommanderId = @cmdrid AND EventTime = @time AND TravelLogId = @tluid AND EventTypeId = @evttype ORDER BY Id ASC"))
                {
                    cmd.AddParameterWithValue("@cmdrid", ent.CommanderId);
                    cmd.AddParameterWithValue("@time", ent.EventTimeUTC);
                    cmd.AddParameterWithValue("@tluid", ent.TLUId);
                    cmd.AddParameterWithValue("@evttype", ent.EventTypeID);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JournalEntry jent = CreateJournalEntry(reader);
                            if (AreSameEntry(ent, jent, entjo))
                            {
                                entries.Add(jent);
                            }
                        }
                    }
                }
            }

            return entries;
        }

        public static int RemoveDuplicateFSDEntries(int currentcmdrid)
        {
            // list of systems in journal, sorted by time
            List<JournalLocOrJump> vsSystemsEnts = JournalEntry.GetAll(currentcmdrid).OfType<JournalLocOrJump>().OrderBy(j => j.EventTimeUTC).ToList();

            int count = 0;
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                for (int ji = 1; ji < vsSystemsEnts.Count; ji++)
                {
                    JournalEvents.JournalFSDJump prev = vsSystemsEnts[ji - 1] as JournalEvents.JournalFSDJump;
                    JournalEvents.JournalFSDJump current = vsSystemsEnts[ji] as JournalEvents.JournalFSDJump;

                    if (prev != null && current != null)
                    {
                        bool previssame = (prev.StarSystem.Equals(current.StarSystem, StringComparison.CurrentCultureIgnoreCase) && (!prev.HasCoordinate || !current.HasCoordinate || (prev.StarPos - current.StarPos).LengthSquared < 0.01));

                        if (previssame)
                        {
                            Delete(prev.Id, cn);
                            count++;
                            System.Diagnostics.Debug.WriteLine("Dup {0} {1} {2} {3}", prev.Id, current.Id, prev.StarSystem, current.StarSystem);
                        }
                    }
                }
            }

            return count;
        }

        public static void ClearEDSMID(int currentcmdrid = -2)      // -2 is all
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                using (DbCommand cmd = cn.CreateCommand("UPDATE JournalEntries SET EdsmId=0"))
                {
                    if (currentcmdrid != -2)
                    {
                        cmd.CommandText = "UPDATE JournalEntries SET EdsmId=0 WHERE CommanderId==@cmd";
                        cmd.AddParameterWithValue("@cmd", currentcmdrid);
                    }

                    cn.SQLNonQueryText( cmd);
                }
            }
        }

        #endregion

        #region Factory creation

        static public Type TypeOfJournalEntry(JournalTypeEnum type)
        {
            if (JournalEntryTypes.ContainsKey(type))
            {
                return JournalEntryTypes[type];
            }
            else
            {
                return TypeOfJournalEntry(type.ToString());
            }
        }

        static string JournalRootClassname = typeof(JournalEvents.JournalTouchdown).Namespace;        // pick one at random to find out root classname

        static public Type TypeOfJournalEntry(string text)
        {
            Type t = Type.GetType(JournalRootClassname + ".Journal" + text, false, true); // no exception, ignore case here
            return t;
        }

        static public JournalEntry CreateJournalEntry(string text)
        {
            JObject jo;

            try
            {
                jo = (JObject)JObject.Parse(text);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error parsing journal entry\n{text}\n{ex.ToString()}");
                return new JournalUnknown(new JObject());
            }

            return CreateJournalEntry(jo);
        }

        static public JournalEntry CreateJournalEntry(JObject jo)
        {
            string Eventstr = jo["event"].StrNull();

            JournalEntry ret = null;

            if (Eventstr == null)  // Should normaly not happend unless corrupt string.
                ret = new JournalUnknown(jo);      // MUST return something
            else
            {
                JournalTypeEnum jte = JournalTypeEnum.Unknown;
                Type jtype = Enum.TryParse(Eventstr, out jte) ? TypeOfJournalEntry(jte) : TypeOfJournalEntry(Eventstr);

                if (jtype == null)
                    ret = new JournalUnknown(jo);
                else
                    ret = (JournalEntry)Activator.CreateInstance(jtype, jo);
            }

            return ret;
        }

        #endregion

        #region Misc

        static public List<string> GetListOfEventsWithOptMethod(bool towords, string method = null, string method2 = null)
        {
            List<string> ret = new List<string>();

            foreach (JournalTypeEnum jte in Enum.GetValues(typeof(JournalTypeEnum)))
            {
                string n = jte.ToString();

                if (method == null)
                {
                    ret.Add((towords) ? n.SplitCapsWord() : n);
                }
                else
                {
                    Type jtype = TypeOfJournalEntry(jte);

                    if (jtype != null)      // may be null, Unknown for instance
                    {
                        System.Reflection.MethodInfo m = jtype.GetMethod(method);

                        if (m == null && method2 != null)
                            m = jtype.GetMethod(method2);

                        if (m != null)
                            ret.Add((towords) ? n.SplitCapsWord() : n);
                    }
                }
            }

            return ret;
        }

        protected JObject ReadAdditionalFile( string extrafile, bool waitforfile, bool checktimestamptype )       // read file, return new JSON
        {
            for (int retries = 0; retries < 25 ; retries++)
            {
                try
                {
                    string json = System.IO.File.ReadAllText(extrafile);        // try the current file

                    if (json != null)
                    {
                        JObject joaf = JObject.Parse(json);       // this has the full version of the event, including data, at the same timestamp

                        string newtype = joaf["event"].Str();
                        DateTime newUTC = DateTime.Parse(joaf.Value<string>("timestamp"), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal);

                        if (checktimestamptype == false || (newUTC != null && newUTC == EventTimeUTC && newtype == EventTypeStr))
                        {
                            return joaf;                        // good current file..
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine($"Unable to read extra info from {extrafile}: {ex.Message}");
                }

                if (!waitforfile)               // if don't wait, continue with no return
                    return null;

                System.Diagnostics.Debug.WriteLine("Current file is not the right one, waiting for it to appear.." + retries);
                System.Threading.Thread.Sleep(100);
            }

            return null;
        }

        #endregion

    }
}

