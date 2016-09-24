﻿using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery2;
using EDDiscovery2.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous
{
    public enum JournalTypeEnum
    {
        Unknown = 0,

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
        ModuleSell = 520,
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
        Rank = 680,
        RebootRepair = 690,
        ReceiveText = 700,
        RedeemVoucher = 710,
        RefuelAll = 720,
        RefuelPartial = 730,
        Repair = 740,
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


    enum SyncFlags
    {
        EDSM = 0x01,
        EDDN = 0x02,
        Future = 0x04,
    };

    public abstract class JournalEntry
    {
        public int Id;                          // this is the entry ID
        public int JournalId;                   // this ID of the journal tlu (aka TravelLogId)
        public int CommanderId;                 // commander Id of entry

        protected string eventTypeStr;          // these two duplicate each other, string if for debuggin in the db view of a browser
        private JournalTypeEnum eventTypeID;

        private DateTime eventTimeUTC;
        protected JObject jEventData;           // event string from the log

        public int EdsmID;                      // 0 = unassigned, >0 = assigned
        private int Synced;                     // sync flags

        public DateTime EventTimeUTC { get { return eventTimeUTC; } }
        public DateTime EventTimeLocal { get { return eventTimeUTC.ToLocalTime(); } }
        public string EventTypeStr { get { return eventTypeStr ?? eventTypeID.ToString(); } }
        public JournalTypeEnum EventType { get { return eventTypeID; } }
        public string EventDataString { get { return jEventData.ToString(); } }     // Get only, functions will modify them to add additional data on

        public bool SyncedEDSM
        {
            get
            {
                return (Synced & (int)SyncFlags.EDSM) == (int)SyncFlags.EDSM;
            }

            set
            {
                if (value == true)
                    Synced |= (int)SyncFlags.EDSM;
                else
                    Synced &= (int)(~SyncFlags.EDSM);
            }
        }

        public bool SyncedEDDN
        {
            get
            {
                return (Synced & (int)SyncFlags.EDDN) == (int)SyncFlags.EDDN;
            }

            set
            {
                if (value == true)
                    Synced |= (int)SyncFlags.EDDN;
                else
                    Synced &= (int)(~SyncFlags.EDDN);
            }
        }

        public virtual void FillInformation(out string summary, out string info, out string detailed)
        {
            summary = Tools.SplitCapsWord(EventType.ToString());
            info = "Event";
            detailed = Tools.SplitCapsWord(ToShortString().Replace("\"", ""));  // something like this..
        }

        public string ToShortString()
        {
            JObject jo = JObject.Parse(EventDataString);  // Create a clone
            jo.Property("timestamp").Remove();
            jo.Property("event").Remove();
            jo.Property("EDDMapColor").Remove();
            return jo.ToString().Replace("{", "").Replace("}", "").Replace("\"", "");
        }


        public JournalEntry(JObject jo, JournalTypeEnum jtype)
        {
            jEventData = jo;
            eventTypeID = jtype;
            eventTypeStr = jtype.ToString();
            eventTimeUTC = DateTime.Parse(jo.Value<string>("timestamp"), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            JournalId = 0;
        }

        static public JournalEntry CreateJournalEntry(DataRow dr)
        {
            string EDataString = (string)dr["EventData"];

            JournalEntry jr = JournalEntry.CreateJournalEntry(EDataString);     // this sets EventTypeId, EventTypeStr and UTC via constructor above.. 

            jr.Id = (int)(long)dr["Id"];
            jr.JournalId = (int)(long)dr["TravelLogId"];
            jr.CommanderId = (int)(long)dr["CommanderId"];
            jr.eventTimeUTC = (DateTime)dr["EventTime"];
            jr.eventTypeID = (JournalTypeEnum)(long)dr["eventTypeID"];
            jr.EdsmID = (int)(long)dr["EdsmID"];
            jr.Synced = (int)(long)dr["Synced"];
            return jr;
        }

        static public JournalEntry CreateJournalEntry(DbDataReader dr)
        {
            string EDataString = (string)dr["EventData"];

            JournalEntry jr = JournalEntry.CreateJournalEntry(EDataString);

            jr.Id = (int)(long)dr["Id"];
            jr.JournalId = (int)(long)dr["TravelLogId"];
            jr.CommanderId = (int)(long)dr["CommanderId"];
            jr.eventTimeUTC = (DateTime)dr["EventTime"];
            jr.eventTypeID = (JournalTypeEnum)(long)dr["eventTypeID"];
            jr.EdsmID = (int)(long)dr["EdsmID"];
            jr.Synced = (int)(long)dr["Synced"];
            return jr;
        }

        public bool Add()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                bool ret = Add(cn);
                return ret;
            }
        }

        public bool Add(SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into JournalEntries (EventTime, TravelLogID, CommanderId, EventTypeId , EventType, EventData, EdsmId, Synced) values (@EventTime, @TravelLogID, @CommanderID, @EventTypeId , @EventType, @EventData, @EdsmId, @Synced)", tn))
            {
                cmd.AddParameterWithValue("@EventTime", eventTimeUTC);
                cmd.AddParameterWithValue("@TravelLogID", JournalId);
                cmd.AddParameterWithValue("@CommanderID", CommanderId);
                cmd.AddParameterWithValue("@EventTypeId", eventTypeID);
                cmd.AddParameterWithValue("@EventType", EventType);
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
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Update JournalEntries set EventTime=@EventTime, TravelLogID=@TravelLogID, CommanderID=@CommanderID, EventTypeId=@EventTypeId, EventType=@EventType, EventData=@EventData, EdsmId=@EdsmId, Synced=@Synced where ID=@id"))
            {
                cmd.AddParameterWithValue("@ID", Id);
                cmd.AddParameterWithValue("@EventTime", eventTimeUTC);
                cmd.AddParameterWithValue("@TravelLogID", JournalId);
                cmd.AddParameterWithValue("@CommanderID", CommanderId);
                cmd.AddParameterWithValue("@EventTypeId", eventTypeID);
                cmd.AddParameterWithValue("@EventType", EventType);
                cmd.AddParameterWithValue("@EventData", EventDataString);
                cmd.AddParameterWithValue("@EdsmId", EdsmID);
                cmd.AddParameterWithValue("@Synced", Synced);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                return true;
            }
        }

        static public List<JournalEntry> GetAll(int commander = -999)
        {
            List<JournalEntry> list = new List<JournalEntry>();

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = cn.CreateCommand("select * from JournalEntries where CommanderID=@commander Order by EventTime ASC"))
                {
                    if (commander == -999)
                        cmd.CommandText = "select * from JournalEntries Order by Time ";

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

        public static List<JournalEntry> GetAllByTLU(long tluid )
        {
            List<JournalEntry> vsc = new List<JournalEntry>();

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT * FROM JournalEntries WHERE TravelLogId = @source ORDER BY Time ASC"))
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
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT * FROM JournalEntries WHERE CommanderId = @cmdrid AND Time < @time ORDER BY Time DESC"))
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
                    je = new JournalRank(jo);
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
                case "ModuleSell":
                    je = new JournalModuleSell(jo);
                    break;
                case "ModuleSwap":
                    je = new JournalModuleSwap(jo);
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
                    je = new JournalShieldState(jo);
                    break;
                case "ShieldState":
                    je = new JournalSelfDestruct(jo);
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



                
              
                case "CapShipBond":
                case "ClearSavedGame":
              
              
                case "EjectCargo":
                case "LaunchFighter":
 

                case "PayFines":
                case "PayLegacyFines":
                case "PowerplayCollect":
                case "PowerplayDefect":
                case "PowerplayDeliver":
                case "PowerplayFastTrack":
                case "PowerplayJoin":
                case "PowerplayLeave":
                case "PowerplaySalary":
                case "PowerplayVote":
                case "PowerplayVoucher":

                case "Promotion":
                case "RebootRepair":
                case "ReceiveText":
                case "RedeemVoucher":

                case "SellDrones":
                case "SendText":
                    je = new JournalUnhandled(jo, Eventstr);
                    System.Diagnostics.Trace.WriteLine("Unhandled event: " + Eventstr);
                    break;

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
            // TODO..
            return true;
        }
    }

}