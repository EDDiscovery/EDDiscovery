using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery2;
using EDDiscovery2.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous
{
    public enum JournalTypeEnum
    {
        Unknown = 0,

        ApproachSettlement = 5,
        Bounty = 10,
        BuyAmmo = 20,
        BuyDrones = 30,
        BuyExplorationData = 40,
        BuyTradeData = 50,
        CapShipBond = 60,
        ClearSavedGame = 70,
        CockpitBreached = 80,
        CollectCargo = 90,
        CommitCrime = 100,
        CommunityGoalJoin = 110,
        CommunityGoalReward = 120,
        Continued = 125,
        CrewAssign = 126,
        CrewFire = 127,
        CrewHire = 128,
        DatalinkScan = 130,
        Died = 140,
        Docked = 145,
        DockFighter = 150,
        DockingCancelled = 160,
        DockingDenied = 170,
        DockingGranted = 180,
        DockingRequested = 190,
        DockingTimeout = 200,
        DockSRV = 210,
        EjectCargo = 220,
        EngineerApply = 230,
        EngineerCraft = 240,
        EngineerProgress = 250,
        EscapeInterdiction = 260,
        FactionKillBond = 270,
        FSDJump = 280,
        FuelScoop = 290,
        FileHeader = 300,
        HeatDamage = 310,
        HeatWarning = 320,
        HullDamage = 330,
        Interdicted = 340,
        Interdiction = 350,
        JetConeBoost = 354,
        JetConeDamage = 355,
        LaunchFighter = 360,
        LaunchSRV = 370,
        Liftoff = 380,
        LoadGame = 390,
        Location = 400,
        MarketBuy = 410,
        MarketSell = 420,
        MaterialCollected = 430,
        MaterialDiscarded = 440,
        MaterialDiscovered = 450,
        MiningRefined = 460,
        MissionAbandoned = 470,
        MissionAccepted = 480,
        MissionCompleted = 490,
        MissionFailed = 500,
        ModuleBuy = 510,
        ModuleRetrieve = 515,
        ModuleSell = 520,
        ModuleStore = 525,
        ModuleSwap = 530,
        NewCommander = 540,
        PayFines = 550,
        PayLegacyFines = 560,
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
        PVPKill = 675,
        Rank = 680,
        RebootRepair = 690,
        ReceiveText = 700,
        RedeemVoucher = 710,
        RefuelAll = 720,
        RefuelPartial = 730,
        Repair = 740,
        RepairAll = 745,
        RestockVehicle = 750,
        Resurrect = 760,
        Scan = 770,
        Screenshot = 780,
        SelfDestruct = 790,
        SellDrones = 800,
        SellExplorationData = 810,
        SendText = 820,
        ShieldState = 830,
        ShipyardBuy = 840,
        ShipyardNew = 850,
        ShipyardSell = 860,
        ShipyardSwap = 870,
        ShipyardTransfer = 880,
        SupercruiseEntry = 890,
        SupercruiseExit = 900,
        Synthesis = 910,
        Touchdown = 920,
        Undocked = 930,
        USSDrop = 940,
        VehicleSwitch = 950,
        WingAdd = 960,
        WingJoin = 970,
        WingLeave = 980,

        EDDItemSet = 2000,
    }

    public enum CombatRank
    {
        Harmless = 0,
        MostlyHarmless,
        Novice,
        Competent,
        Expert,
        Master,
        Dangerous,
        Deadly,
        Elite
    }

    public enum TradeRank
    {
        Penniless = 0,
        MostlyPenniless,
        Peddler,
        Dealer,
        Merchant,
        Broker,
        Entrepreneur,
        Tycoon,
        Elite
    }

    public enum ExplorationRank
    {
        Aimless = 0,
        MostlyAimless,
        Scout,
        Surveyor,
        Explorer,
        Pathfinder,
        Ranger,
        Pioneer,
        Elite
    }

    public enum FederationRank
    {
        None = 0,
        Recruit,
        Cadet,
        Midshipman,
        PettyOfficer,
        ChiefPettyOfficer,
        WarrantOfficer,
        Ensign,
        Lieutenant,
        LtCommander,
        PostCommander,
        PostCaptain,
        RearAdmiral,
        ViceAdmiral,
        Admiral
    }

    public enum EmpireRank
    {
        None = 0,
        Outsider,
        Serf,
        Master,
        Squire,
        Knight,
        Lord,
        Baron,
        Viscount,
        Count,
        Earl,
        Marquis,
        Duke,
        Prince,
        King
    }

    public enum CQCRank
    {
        Helpless = 0,
        MostlyHelpless,
        Amateur,
        SemiProfessional,
        Professional,
        Champion,
        Hero,
        Legend,
        Elite
    }


    public enum SyncFlags
    {
        EDSM = 0x01,
        EDDN = 0x02,
        StartMarker = 0x0100,           // measure distance start pos marker
        StopMarker = 0x0200,            // measure distance stop pos marker
    };

    [DebuggerDisplay("Event {EventTypeStr} {EventTimeUTC} EdsmID {EdsmID} JID {Id} C {CommanderId}")]
    public abstract class JournalEntry
    {
        public long Id;                          // this is the entry ID
        public long TLUId;                       // this ID of the journal tlu (aka TravelLogId)
        public int CommanderId;                 // commander Id of entry

        public string EventTypeStr;          // these two duplicate each other, string if for debuggin in the db view of a browser
        public JournalTypeEnum EventTypeID;

        public DateTime EventTimeUTC;
        
        public int EdsmID;                      // 0 = unassigned, >0 = assigned

        protected JObject jEventData;           // event string from the log
        private int Synced;                     // sync flags

        public DateTime EventTimeLocal { get { return EventTimeUTC.ToLocalTime(); } }
        public string EventDataString { get { return jEventData.ToString(); } }     // Get only, functions will modify them to add additional data on

        public bool SyncedEDSM { get { return (Synced & (int)SyncFlags.EDSM) != 0; } }
        public bool SyncedEDDN { get { return (Synced & (int)SyncFlags.EDDN) != 0; } }
        public bool StartMarker { get { return (Synced & (int)SyncFlags.StartMarker) != 0; } }
        public bool StopMarker { get { return (Synced & (int)SyncFlags.StopMarker) != 0; } }


        public virtual void FillInformation(out string summary, out string info, out string detailed)
        {
            summary = Tools.SplitCapsWord(EventTypeStr);
            info = ToShortString();
            detailed = "";
        }

        public virtual string DefaultRemoveItems()
        {
            return "timestamp;event;EDDMapColor";
        }

        private static JSONConverters jsonconvcache;     //cache it

        public string ToShortString(string additionalremoves = null, JSONConverters jc = null)
        {
            if (jc == null)
            {
                if ( jsonconvcache == null )
                    jsonconvcache = StandardConverters();

                jc = jsonconvcache;
            }

            JSONPrettyPrint jpp = new JSONPrettyPrint(jc,DefaultRemoveItems() + ((additionalremoves!= null) ? (";" + additionalremoves) : ""),"_Localised",EventTypeStr);
            return jpp.PrettyPrint(EventDataString,80);
        }

        public JournalEntry(JObject jo, JournalTypeEnum jtype)
        {
            jEventData = jo;
            EventTypeID = jtype;
            EventTypeStr = jtype.ToString();
            EventTimeUTC = DateTime.Parse(jo.Value<string>("timestamp"), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            TLUId = 0;
        }

        static public JournalEntry CreateJournalEntry(DataRow dr)
        {
            string EDataString = (string)dr["EventData"];

            JournalEntry jr = JournalEntry.CreateJournalEntry(EDataString);     // this sets EventTypeId, EventTypeStr and UTC via constructor above.. 

            jr.Id = (int)(long)dr["Id"];
            jr.TLUId = (int)(long)dr["TravelLogId"];
            jr.CommanderId = (int)(long)dr["CommanderId"];
            jr.EventTimeUTC = (DateTime)dr["EventTime"];
            jr.EventTypeID = (JournalTypeEnum)(long)dr["eventTypeID"];
            jr.EdsmID = (int)(long)dr["EdsmID"];
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
            jr.EventTimeUTC = (DateTime)dr["EventTime"];
            jr.EventTypeID = (JournalTypeEnum)(long)dr["eventTypeID"];
            jr.EdsmID = (int)(long)dr["EdsmID"];
            jr.Synced = (int)(long)dr["Synced"];
            return jr;
        }

        public static JournalEntry CreateFSDJournalEntry(long tluid, int cmdrid, DateTime utc, string name, double x, double y, double z, int mc, int syncflag)
        {
            JObject jo = new JObject();
            jo["timestamp"] = utc.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
            jo["event"] = "FSDJump";
            jo["StarSystem"] = name;
            jo["StarPos"] = new JArray(x, y, z);
            jo["EDDMapColor"] = mc;

            JournalEntry je = CreateJournalEntry(jo.ToString());
            je.TLUId = tluid;
            je.CommanderId = cmdrid;
            je.Synced = syncflag;
            return je;
        }

        public bool Add()
        {
            using (SQLiteConnectionUserUTC cn = new SQLiteConnectionUserUTC())
            {
                bool ret = Add(cn);
                return ret;
            }
        }

        public bool Add(SQLiteConnectionUserUTC cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into JournalEntries (EventTime, TravelLogID, CommanderId, EventTypeId , EventType, EventData, EdsmId, Synced) values (@EventTime, @TravelLogID, @CommanderID, @EventTypeId , @EventStrName, @EventData, @EdsmId, @Synced)", tn))
            {
                cmd.AddParameterWithValue("@EventTime", EventTimeUTC);           // MUST use UTC connection
                cmd.AddParameterWithValue("@TravelLogID", TLUId);
                cmd.AddParameterWithValue("@CommanderID", CommanderId);
                cmd.AddParameterWithValue("@EventTypeId", EventTypeID);
                cmd.AddParameterWithValue("@EventStrName", EventTypeStr);
                cmd.AddParameterWithValue("@EventData", EventDataString);
                cmd.AddParameterWithValue("@EdsmId", EdsmID);
                cmd.AddParameterWithValue("@Synced", Synced);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from JournalEntries"))
                {
                    Id = (int)(long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }
                return true;
            }
        }

        public bool Update()
        {
            using (SQLiteConnectionUserUTC cn = new SQLiteConnectionUserUTC())
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnectionUserUTC cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("Update JournalEntries set EventTime=@EventTime, TravelLogID=@TravelLogID, CommanderID=@CommanderID, EventTypeId=@EventTypeId, EventType=@EventStrName, EventData=@EventData, EdsmId=@EdsmId, Synced=@Synced where ID=@id",tn))
            {
                cmd.AddParameterWithValue("@ID", Id);
                cmd.AddParameterWithValue("@EventTime", EventTimeUTC);  // MUST use UTC connection
                cmd.AddParameterWithValue("@TravelLogID", TLUId);
                cmd.AddParameterWithValue("@CommanderID", CommanderId);
                cmd.AddParameterWithValue("@EventTypeId", EventTypeID);
                cmd.AddParameterWithValue("@EventStrName", EventTypeStr);
                cmd.AddParameterWithValue("@EventData", EventDataString);
                cmd.AddParameterWithValue("@EdsmId", EdsmID);
                cmd.AddParameterWithValue("@Synced", Synced);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                return true;
            }
        }

        //dist >0 to update
        public static void UpdateEDSMIDPosJump(long journalid, ISystem system, bool jsonpos , double dist )
        {
            using (SQLiteConnectionUserUTC cn = new SQLiteConnectionUserUTC())
            {
                JournalEntry ent = Get(journalid, cn);

                if (ent != null)
                {
                    JObject jo = (JObject)JObject.Parse(ent.EventDataString);

                    if (jsonpos)
                    {
                        jo["StarPos"] = new JArray() { system.x, system.y, system.z };
                    }

                    if (dist > 0)
                        jo["JumpDist"] = dist;

                    using (DbCommand cmd2 = cn.CreateCommand("Update JournalEntries set EventData = @EventData, EdsmId = @EdsmId where ID = @ID"))
                    {
                        cmd2.AddParameterWithValue("@ID", journalid);
                        cmd2.AddParameterWithValue("@EventData", jo.ToString());
                        cmd2.AddParameterWithValue("@EdsmId", system.id_edsm);

                        System.Diagnostics.Trace.WriteLine(string.Format("Update journal ID {0} with pos {1}/edsmid {2} dist {3}", journalid, jsonpos, system.id_edsm, dist));
                        SQLiteDBClass.SQLNonQueryText(cn, cmd2);
                    }
                }
            }
        }

        public static void UpdateMapColour(long journalid, int mapcolour)
        {
            using (SQLiteConnectionUserUTC cn = new SQLiteConnectionUserUTC())
            {
                JournalEntry ent = Get(journalid, cn);

                if (ent != null)
                {
                    JObject jo = (JObject)JObject.Parse(ent.EventDataString);

                    jo["EDDMapColor"] = mapcolour;

                    using (DbCommand cmd2 = cn.CreateCommand("Update JournalEntries set EventData = @EventData where ID = @ID"))
                    {
                        cmd2.AddParameterWithValue("@ID", journalid);
                        cmd2.AddParameterWithValue("@EventData", jo.ToString());

                        System.Diagnostics.Trace.WriteLine(string.Format("Update journal ID {0} with map colour", journalid));
                        SQLiteDBClass.SQLNonQueryText(cn, cmd2);
                    }
                }
            }
        }

        public static void UpdateSyncFlagBit(long journalid, SyncFlags bit, bool value)
        {
            using (SQLiteConnectionUserUTC cn = new SQLiteConnectionUserUTC())
            {
                JournalEntry je = Get(journalid, cn);

                if (je != null)
                {
                    if (value)
                        je.Synced |= (int)bit;
                    else
                        je.Synced &= ~(int)bit;

                    using (DbCommand cmd = cn.CreateCommand("Update JournalEntries set Synced = @sync where ID=@journalid"))
                    {
                        cmd.AddParameterWithValue("@journalid", journalid);
                        cmd.AddParameterWithValue("@sync", je.Synced);
                        System.Diagnostics.Trace.WriteLine(string.Format("Update sync flag ID {0} with {1}", journalid , je.Synced));
                        SQLiteDBClass.SQLNonQueryText(cn, cmd);
                    }
                }
            }
        }

        public static void UpdateCommanderID(long journalid, int cmdrid)
        {
            using (SQLiteConnectionUserUTC cn = new SQLiteConnectionUserUTC())
            {
                using (DbCommand cmd = cn.CreateCommand("Update JournalEntries set CommanderID = @cmdrid where ID=@journalid"))
                {
                    cmd.AddParameterWithValue("@journalid", journalid);
                    cmd.AddParameterWithValue("@cmdrid", cmdrid);
                    System.Diagnostics.Trace.WriteLine(string.Format("Update cmdr id ID {0} with map colour", journalid));
                    SQLiteDBClass.SQLNonQueryText(cn, cmd);
                }
            }
        }

        static public JournalEntry Get(long journalid, SQLiteConnectionUserUTC cn)
        {
            using (DbCommand cmd = cn.CreateCommand("select * from JournalEntries where ID=@journalid"))
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

        static public JournalEntry Get(long journalid)
        {
            using (SQLiteConnectionUserUTC cn = new SQLiteConnectionUserUTC())
            {
                return Get(journalid, cn);
            }
        }




        static public List<JournalEntry> GetAll(int commander = -999)
        {
            List<JournalEntry> list = new List<JournalEntry>();

            using (SQLiteConnectionUserUTC cn = new SQLiteConnectionUserUTC())
            {
                using (DbCommand cmd = cn.CreateCommand("select * from JournalEntries where CommanderID=@commander Order by EventTime ASC"))
                {
                    if (commander == -999)
                        cmd.CommandText = "select * from JournalEntries Order by EventTime ";

                    cmd.AddParameterWithValue("@commander", commander);

                    DataSet ds = SQLiteDBClass.SQLQueryText(cn, cmd);

                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        return list;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        JournalEntry sys = JournalEntry.CreateJournalEntry(dr);
                        list.Add(sys);
                    }

                    return list;
                }
            }
        }


        public static List<JournalEntry> GetByEventType(JournalTypeEnum eventtype, int commanderid, DateTime start, DateTime stop)
        {
            List<JournalEntry> vsc = new List<JournalEntry>();

            using (SQLiteConnectionUserUTC cn = new SQLiteConnectionUserUTC())
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT * FROM JournalEntries WHERE EventTypeID = @eventtype and  CommanderID=@commander and  EventTime >@start and EventTime<@Stop ORDER BY EventTime ASC"))
                {
                    cmd.AddParameterWithValue("@eventtype", (int)eventtype);
                    cmd.AddParameterWithValue("@commander", (int)commanderid);
                    cmd.AddParameterWithValue("@start", start);
                    cmd.AddParameterWithValue("@stop", stop);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vsc.Add(JournalEntry.CreateJournalEntry(reader));
                        }
                    }
                }
            }
            return vsc;
        }



        public static List<JournalEntry> GetAllByTLU(long tluid )
        {
            List<JournalEntry> vsc = new List<JournalEntry>();

            using (SQLiteConnectionUserUTC cn = new SQLiteConnectionUserUTC())
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT * FROM JournalEntries WHERE TravelLogId = @source ORDER BY EventTime ASC"))
                {
                    cmd.AddParameterWithValue("@source", tluid);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vsc.Add(JournalEntry.CreateJournalEntry(reader));
                        }
                    }
                }
            }
            return vsc;
        }

        public static T GetLast<T>(int cmdrid, DateTime before)
            where T : JournalEntry
        {
            using (SQLiteConnectionUserUTC cn = new SQLiteConnectionUserUTC())
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
                            if (ent is T)
                            {
                                return (T)ent;
                            }
                        }
                    }
                }
            }

            return null;
        }


        static public JournalEntry CreateJournalEntry(string text)
        {
            JournalEntry je=null;

            JObject jo = (JObject)JObject.Parse(text);

            string Eventstr = Tools.GetStringNull(jo["event"]);

            if (Eventstr == null)  // Should normaly not happend unless corrupt string.
                return null;

            switch (Eventstr)
            {
                case "ApproachSettlement":
                    je = new JournalApproachSettlement(jo);
                    break;
                case "Bounty":
                    je = new JournalBounty(jo);
                    break;
                case "BuyAmmo":
                    je = new JournalBuyAmmo(jo);
                    break;
                case "BuyDrones":
                    je = new JournalBuyDrones(jo);
                    break;
                case "BuyExplorationData":
                    je = new JournalBuyExplorationData(jo);
                    break;
                case "BuyTradeData":
                    je = new JournalBuyTradeData(jo);
                    break;


                case "CockpitBreached":
                    je = new JournalCockpitBreached(jo);
                    break;
                case "CollectCargo":
                    je = new JournalCollectCargo(jo);
                    break;
                case "CommitCrime":
                    je = new JournalCommitCrime(jo);
                    break;
                case "CommunityGoalJoin":
                    je = new JournalCommunityGoalJoin(jo);
                    break;
                case "CommunityGoalReward":
                    je = new JournalCommunityGoalReward(jo);
                    break;

                case "CrewAssign":
                    je = new JournalCrewAssign(jo);
                    break;
                case "CrewFire":
                    je = new JournalCrewFire(jo);
                    break;
                case "CrewHire":
                    je = new JournalCrewHire(jo);
                    break;



                case "DatalinkScan":
                    je = new JournalDatalinkScan(jo);
                    break;
                case "DockFighter":
                    je = new JournalDockFighter(jo);
                    break;
                case "DockSRV":
                    je = new JournalDockSRV(jo);
                    break;

                case "Docked":
                    je = new JournalDocked(jo);
                    break;

                case "Died":
                    je = new JournalDied(jo);
                    break;

                case "fileheader":
                case "Fileheader":
                    je = new JournalFileHeader(jo);
                    break;

                case "FSDJump":
                    je = new JournalFSDJump(jo);
                    break;

                case "Location":
                    je = new JournalLocation(jo);
                    break;

                case "LoadGame":
                    je = new JournalLoadGame(jo);
                    break;

                case "Scan":
                    je = new JournalScan(jo);
                    break;


                case "SellExplorationData":
                    je = new JournalSellExplorationData(jo);
                    break;

                case "Undocked":
                    je = new JournalUndocked(jo);
                    break;

                case "DockingCancelled":
                    je = new JournalDockingCancelled(jo);
                    break;

                case "DockingDenied":
                    je = new JournalDockingDenied(jo);
                    break;
                case "DockingGranted":
                    je = new JournalDockingGranted(jo);
                    break;

                case "DockingRequested":
                    je = new JournalDockingRequested(jo);
                    break;

                case "DockingTimeout":
                    je = new JournalDockingTimeout(jo);
                    break;


                case "EngineerApply":
                    je = new JournalEngineerApply(jo);
                    break;
                case "EngineerCraft":
                    je = new JournalEngineerCraft(jo);
                    break;
                case "EngineerProgress":
                    je = new JournalEngineerProgress(jo);
                    break;

                case "EscapeInterdiction":
                    je = new JournalEscapeInterdiction(jo);
                    break;
                case "FactionKillBond":
                    je = new JournalFactionKillBond(jo);
                    break;
                case "HeatDamage":
                    je = new JournalHeatDamage(jo);
                    break;
                case "HeatWarning":
                    je = new JournalHeatWarning(jo);
                    break;
                case "HullDamage":
                    je = new JournalHullDamage(jo);
                    break;
                case "Interdicted":
                    je = new JournalInterdicted(jo);
                    break;
                case "Interdiction":
                    je = new JournalInterdiction(jo);
                    break;



                case "FuelScoop":
                    je = new JournalFuelScoop(jo);
                    break;

                case "JetConeBoost":
                    je = new JournalJetConeBoost(jo);
                    break;
                case "JetConeDamage":
                    je = new JournalJetConeDamage(jo);
                    break;


                case "LaunchSRV":
                    je = new JournalLaunchSRV(jo);
                    break;
                case "Liftoff":
                    je = new JournalLiftoff(jo);
                    break;
                case "MarketBuy":
                    je = new JournalMarketBuy(jo);
                    break;
                case "MarketSell":
                    je = new JournalMarketSell(jo);
                    break;


                case "MaterialCollected":
                    je = new JournalMaterialCollected(jo);
                    break;
                case "MaterialDiscarded":
                    je = new JournalMaterialDiscarded(jo);
                    break;
                case "MaterialDiscovered":
                    je = new JournalMaterialDiscovered(jo);
                    break;
                case "MiningRefined":
                    je = new JournalMiningRefined(jo);
                    break;

                case "MissionAbandoned":
                    je = new JournalMissionAbandoned(jo);
                    break;
                case "MissionAccepted":
                    je = new JournalMissionAccepted(jo);
                    break;
                case "MissionCompleted":
                    je = new JournalMissionCompleted(jo);
                    break;
                case "MissionFailed":
                    je = new JournalMissionFailed(jo);
                    break;
                case "NewCommander":
                    je = new JournalNewCommander(jo);
                    break;

                case "Continued":
                    je = new JournalContinued(jo);
                    break;

                case "Rank":
                    je = new JournalRank(jo);
                    break;
                    
                case "Progress":
                    je = new JournalProgress(jo);
                    break;

                case "SupercruiseEntry":
                    je = new JournalSupercruiseEntry(jo);
                    break;

                case "SupercruiseExit":
                    je = new JournalSupercruiseExit(jo);
                    break;

                case "ModuleBuy":
                    je = new JournalModuleBuy(jo);
                    break;

                case "ModuleRetrieve":
                    je = new JournalModuleRetrieve(jo);
                    break;

                case "ModuleSell":
                    je = new JournalModuleSell(jo);
                    break;

                case "ModuleStore":
                    je = new JournalModuleStore(jo);
                    break;

                case "ModuleSwap":
                    je = new JournalModuleSwap(jo);
                    break;

                case "PVPKill":
                    je = new JournalPVPKill(jo);
                    break;

                case "RefuelAll":
                    je = new JournalRefuelAll(jo);
                    break;
                case "RefuelPartial":
                    je = new JournalRefuelPartial(jo);
                    break;
                case "Repair":
                    je = new JournalRepair(jo);
                    break;
                case "RepairAll":
                    je = new JournalRepairAll(jo);
                    break;
                case "RestockVehicle":
                    je = new JournalRestockVehicle(jo);
                    break;

                case "Resurrect":
                    je = new JournalResurrect(jo);
                    break;

                case "Screenshot":
                    je = new JournalScreenshot(jo);
                    break;
                case "SelfDestruct":
                    je = new JournalSelfDestruct(jo);
                    break;
                case "ShieldState":
                    je = new JournalShieldState(jo);
                    break;
                case "ShipyardBuy":
                    je = new JournalShipyardBuy(jo);
                    break;
                case "ShipyardNew":
                    je = new JournalShipyardNew(jo);
                    break;
                case "ShipyardSell":
                    je = new JournalShipyardSell(jo);
                    break;
                case "ShipyardSwap":
                    je = new JournalShipyardSwap(jo);
                    break;
                case "ShipyardTransfer":
                    je = new JournalShipyardTransfer(jo);
                    break;

                case "Synthesis":
                    je = new JournalSynthesis(jo);
                    break;
                case "Touchdown":
                    je = new JournalTouchdown(jo);
                    break;
                case "USSDrop":
                    je = new JournalUSSDrop(jo);
                    break;
                case "VehicleSwitch":
                    je = new JournalVehicleSwitch(jo);
                    break;
                case "WingAdd":
                    je = new JournalWingAdd(jo);
                    break;
                case "WingJoin":
                    je = new JournalWingJoin(jo);
                    break;
                case "WingLeave":
                    je = new JournalWingLeave(jo);
                    break;

                case "ReceiveText":
                    je = new JournalReceiveText(jo);
                    break;
                case "SendText":
                    je = new JournalSendText(jo);
                    break;

                case "PayFines":
                    je = new JournalPayFines(jo);
                    break;
                case "PayLegacyFines":
                    je = new JournalPayLegacyFines(jo);
                    break;
                case "Promotion":
                    je = new JournalPromotion(jo);
                    break;
                case "RebootRepair":
                    je = new JournalRebootRepair(jo);
                    break;
                case "RedeemVoucher":
                    je = new JournalRedeemVoucher(jo);
                    break;


                case "CapShipBond":
                    je = new JournalCapShipBond(jo);
                    break;
                case "ClearSavedGame":
                    je = new JournalClearSavedGame(jo);
                    break;

                case "EjectCargo":
                    je = new JournalEjectCargo(jo);
                    break;
                case "LaunchFighter":
                    je = new JournalLaunchFighter(jo);
                    break;
                case "SellDrones":
                    je = new JournalSellDrones(jo);
                    break;


                case "PowerplayCollect":
                    je = new JournalPowerplayCollect(jo);
                    break;
                case "PowerplayDefect":
                    je = new JournalPowerplayDefect(jo);
                    break;
                case "PowerplayDeliver":
                    je = new JournalPowerplayDeliver(jo);
                    break;
                case "PowerplayFastTrack":
                    je = new JournalPowerplayFastTrack(jo);
                    break;
                case "PowerplayJoin":
                    je = new JournalPowerplayJoin(jo);
                    break;
                case "PowerplayLeave":
                    je = new JournalPowerplayLeave(jo);
                    break;
                case "PowerplaySalary":
                    je = new JournalPowerplaySalary(jo);
                    break;
                case "PowerplayVote":
                    je = new JournalPowerplayVote(jo);
                    break;
                case "PowerplayVoucher":
                    je = new JournalPowerplayVoucher(jo);
                    break;

                case "EDDItemSet":
                    je = new JournalEDDItemSet(jo);
                    break;

                    //je = new JournalUnhandled(jo, Eventstr);
                    //System.Diagnostics.Trace.WriteLine("Unhandled event: " + Eventstr);
                    //break;

                default:
                    je = new JournalUnknown(jo, Eventstr);
                    System.Diagnostics.Trace.WriteLine("Unknown event: " + Eventstr);

                    break;
            }

            return je;
        }


        static public JournalTypeEnum JournalString2Type(string str)
        {
            foreach (JournalTypeEnum mat in Enum.GetValues(typeof(JournalTypeEnum)))
            {
                if (str.ToLower().Equals(mat.ToString().ToLower()))
                    return mat;
            }

            return JournalTypeEnum.Unknown;
        }


        static public bool ResetCommanderID(int from , int to)
        {
            using (SQLiteConnectionUserUTC cn = new SQLiteConnectionUserUTC())
            {
                using (DbCommand cmd = cn.CreateCommand("Update JournalEntries set CommanderID = @cmdridto where CommanderID=@cmdridfrom"))
                {
                    if (from == -1)
                        cmd.CommandText = "Update JournalEntries set CommanderID = @cmdridto";

                    cmd.AddParameterWithValue("@cmdridto", to);
                    cmd.AddParameterWithValue("@cmdridfrom", from);
                    System.Diagnostics.Trace.WriteLine(string.Format("Update cmdr id ID {0} with {1}", from , to));
                    SQLiteDBClass.SQLNonQueryText(cn, cmd);
                }
            }
            return true;
        }

        public static JSONConverters StandardConverters()
        {
            JSONConverters jc = new JSONConverters();
            jc.AddScale("MassEM", 1.0, "0.0'em'", "Mass");
            jc.AddScale("MassMT", 1.0, "0.0'mt'", "Mass");
            jc.AddScale("SurfacePressure", 1.0, "0.0'p'");
            jc.AddScale("Radius", 1.0 / 1000, "0.0'km'");
            jc.AddScale("InnerRad", 1.0 / 1000, "0.0'km'", "Inner Radius");
            jc.AddScale("OuterRad", 1.0 / 1000, "0.0'km'", "Outer Radius");
            jc.AddScale("OrbitalPeriod;RotationPeriod", 1.0 / 86400, "0.0' days orbit'", "");
            jc.AddScale("SurfaceGravity", 1.0 / 9.8, "0.0'g'");
            jc.AddScale("SurfaceTemperature", 1.0, "0.0'K'");
            jc.AddScale("Scooped", 1.0, "'Scooped '0.0't'", "", "FuelScoop");
            jc.AddScale("Total", 1.0, "'Fuel Level '0.0't'", "", "FuelScoop");
            jc.AddScale("Fuel Level", 1.0, "Fuel Level Left '0.0't'", "");
            jc.AddScale("Amount", 1.0, "'Fuel Bought '0.0't'", "", "RefuelAll");
            jc.AddScale("BoostValue", 1.0, "0.0' boost'", "", "JetConeBoost");
            jc.AddScale("StarPos", 1.0, "0.0","");          // any entry StarPos loses it name (inside arrays). StarPos as an array name gets printed sep.

            jc.AddBool("TidalLock", "Not Tidally Locked", "Tidally Locked", ""); // remove name
            jc.AddBool("Landable", "Not Landable", "Landable", ""); // remove name
            jc.AddBool("ShieldsUp", "Shields Down", "Shields Up Captain", ""); // remove name
            jc.AddState("TerraformState", "Not Terrraformable", "");    // remove name
            jc.AddState("Atmosphere", "No Atmosphere", "");
            jc.AddState("Volcanism", "No Volcanism", "");
            jc.AddPrePostfix("StationType", "; Type", "");
            jc.AddPrePostfix("StationName", "; Station", "");
            jc.AddPrePostfix("DestinationSystem", "; Destination Star System", "");
            jc.AddPrePostfix("DestinationStation", "; Destination Station", "");
            jc.AddPrePostfix("StarSystem;System", "; Star System", "");
            jc.AddPrePostfix("Allegiance", "; Allegiance", "");
            jc.AddPrePostfix("Security", "; Security", "");
            jc.AddPrePostfix("Faction", "; Faction", "");
            jc.AddPrePostfix("Government", "Government Type ", "");
            jc.AddPrePostfix("Economy", "Economy Type ", "");
            jc.AddBool("Docked", "Not Docked", "Docked", "");   // remove name
            jc.AddBool("PlayerControlled", "NPC Controlled", "Player Controlled", ""); // remove name

            jc.AddPrePostfix("Body", "At ", "");

            jc.AddPrePostfix("Category", "; material", "", "MaterialCollected;MaterialDiscovered;MaterialDiscarded");
            jc.AddPrePostfix("Name", "", "", "MaterialCollected;MaterialDiscovered;MaterialDiscarded");
            jc.AddPrePostfix("Count", "; items", "", "MaterialDiscarded");

            jc.AddPrePostfix("To", "To ", "", "VehicleSwitch");
            jc.AddPrePostfix("Name", "", "", "CrewAssign");

            jc.AddPrePostfix("Role", "; role", "", "CrewAssign");
            jc.AddPrePostfix("Name", "", "", "MissionAccepted;MissionAbandoned;MissionCompleted;MissionFailed");
            jc.AddPrePostfix("Cost;ShipPrice;BaseValue", "; credits", "");
            jc.AddPrePostfix("Bonus", "; credits bonus", "");
            jc.AddPrePostfix("Amount", "; credits", "", "PayLegacyFines");
            jc.AddPrePostfix("BuyPrice", "Bought for ; credits", "");
            jc.AddPrePostfix("SellPrice", "Sold for ; credits", "");

            jc.AddPrePostfix("LandingPad", "On pad ", "");

            jc.AddPrePostfix("BuyItem", "; bought", "");
            jc.AddPrePostfix("SellItem", "; sold", "");

            jc.AddPrePostfix("Credits", "; credits", "", "LoadGame");
            jc.AddPrePostfix("Ship;ShipType", "Ship ;", "");
            jc.AddPrePostfix("StoreOldShip", "; stored", "");

            jc.AddScale("Health", 100.0, "'Health' 0.0'%'", "");

            jc.AddScale("Distance", 1.0 / 299792458.0 / 365 / 24 / 60 / 60, "'Distance' 0.0'ly'", "", "ShipyardTransfer");
            jc.AddPrePostfix("TransferPrice", "; credits", "", "ShipyardTransfer");

            jc.AddPrePostfix("Name", "; settlement", "", "ApproachSettlement");
            jc.AddPrePostfix("Item", ";", "", "Repair");

            jc.AddPrePostfix("BodyName", "Scan ", "", JL(new[] { JournalTypeEnum.Scan }));
            jc.AddScale("DistanceFromArrivalLS", 1.0, "0.0' ls from arrival point'", "", JL(new[] { JournalTypeEnum.Scan }));
            jc.AddPrePostfix("StarType", "; type star", "", JL(new[] { JournalTypeEnum.Scan }));
            jc.AddScale("StellarMass", 1.0, "0.0' stellar masses'", "", JL(new[] { JournalTypeEnum.Scan }));
            jc.AddScale("Radius", 1.0, "0.0' km radius'", "", JL(new[] { JournalTypeEnum.Scan }));
            jc.AddScale("AbsoluteMagnitude", 1.0, "0.0' absolute magnitude'", "", JL(new[] { JournalTypeEnum.Scan }));
            jc.AddScale("OrbitalPeriod", 1.0 / 86400, "0.0' days orbit'", "", JL(new[] { JournalTypeEnum.Scan }));
            jc.AddScale("RotationPeriod", 1.0 / 86400, "0.0' days rotation'", "", JL(new[] { JournalTypeEnum.Scan }));
            jc.AddPrePostfix("PlanetClass", "; planet class", "", JL(new[] { JournalTypeEnum.Scan })); 

            jc.AddPrePostfix("Engineer", "From ", "", JL(new[] { JournalTypeEnum.EngineerProgress, JournalTypeEnum.EngineerApply, JournalTypeEnum.EngineerCraft }));
            jc.AddPrePostfix("Progress", "", "", JL(new[] { JournalTypeEnum.EngineerProgress, JournalTypeEnum.EngineerApply, JournalTypeEnum.EngineerCraft }));

            jc.AddPrePostfix("Reward", "; credits", "", JL(new[] { JournalTypeEnum.Bounty }));

            jc.AddIndex("Combat", "; combat;0;Harmless;Mostly Harmless;Novice;Competent;Expert;Master;Dangerous;Deadly;Elite", "", JL(new[] { JournalTypeEnum.Rank }));
            jc.AddIndex("Trade", "; trader;0;Penniless;Mostly Penniless;Peddler;Dealer;Merchant;Broker;Entrepreneur;Tycoon;Elite", "", JL(new[] { JournalTypeEnum.Rank }));
            jc.AddIndex("Explore", "; explorer;0;Aimless;Mostly Aimless;Scout;Surveyor;Trailblazer;Pathfinder;Ranger;Pioneer;Elite", "", JL(new[] { JournalTypeEnum.Rank }));
            jc.AddIndex("Empire", "; Empire;0;None;Outsider;Serf;Master;Squire;Knight;Lord;Baron;Viscount;Count;Earl;Marquis;Duke;Prince;King", "", JL(new[] { JournalTypeEnum.Rank }));
            jc.AddIndex("Federation", "; Federation;0;None;Recruit;Cadet;Midshipman;Petty Officer;Chief Pretty Officer;Warren Officer;Ensign;Lieutenant;Lieutenant Commander;Post Commander;Post Captain;Rear Admiral;Vice Admiral;Admiral", "", JL(new[] { JournalTypeEnum.Rank }));
            return jc;
        }

        static string JL( JournalTypeEnum[] ar )
        {
            string s = "";
            foreach (JournalTypeEnum a in ar)
                s += ((s.Length > 0) ? ";" : "") + a.ToString();

            return s;
        }

    }

}