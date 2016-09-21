using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery2;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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



    public abstract class JournalEntry
    {
        public int Id;
        public int JournalId;
        protected string eventTypeStr;
        private JournalTypeEnum eventType;
        private DateTime eventTimeUTC;
        private JObject jEventData;
        public int Synced;


        public DateTime EventTimeUTC
        {
            get
            {
                return eventTimeUTC;
            }

        }
        public DateTime EventTimeLocal
        {
            get
            {
                return eventTimeUTC.ToLocalTime();
            }

        }

        public string EventTypeStr
        {
            get
            {
                return eventTypeStr ?? eventType.ToString();
            }
        }

        public JournalTypeEnum EventType
        {
            get
            {
                return eventType;
            }
        }

        public string EventDataString
        {
            get
            {
                return jEventData.ToString();
            }
        }

        public virtual void FillInformation(out string summary, out string info, out string detailed)
        {
            summary = Tools.SplitCapsWord(eventType.ToString());
            info = "Event"; // TO do.. pick first three?
            detailed = Tools.SplitCapsWord(ToShortString().Replace("\"", ""));  // something like this..
        }

        public JournalEntry(JObject jo, JournalTypeEnum jtype, EDJournalReader reader)
        {
            jEventData = jo;
            eventType = jtype;

            eventTimeUTC = DateTime.Parse(jo.Value<string>("timestamp"), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            JournalId = reader.JournalId;
        }

        public string ToShortString()
        {
            JObject jo = JObject.Parse(EventDataString);  // Create a clone

            jo.Property("timestamp").Remove();
            jo.Property("event").Remove();
            return jo.ToString().Replace("{", "").Replace("}", "").Replace("\"", "");
        }

        static public JournalEntry CreateJournalEntry(string text, EDJournalReader reader)
        {
            JournalEntry je=null;

            JObject jo = (JObject)JObject.Parse(text);

            string Eventstr = Tools.GetString(jo["event"]);

            if (Eventstr == null)  // Should normaly not happend unless corrupt string.
                return null;

            switch (Eventstr)
            {
                case "BuyAmmo":
                    je = new JournalBuyAmmo(jo, reader);
                    break;
                case "BuyDrones":
                    je = new JournalBuyDrones(jo, reader);
                    break;
                case "BuyExplorationData":
                    je = new JournalBuyExplorationData(jo, reader);
                    break;
                case "BuyTradeData":
                    je = new JournalBuyTradeData(jo, reader);
                    break;


                case "CockpitBreached":
                    je = new JournalCockpitBreached(jo, reader);
                    break;
                case "CollectCargo":
                    je = new JournalCollectCargo(jo, reader);
                    break;
                case "CommitCrime":
                    je = new JournalCommitCrime(jo, reader);
                    break;
                case "CommunityGoalJoin":
                    je = new JournalCommunityGoalJoin(jo, reader);
                    break;
                case "CommunityGoalReward":
                    je = new JournalCommunityGoalReward(jo, reader);
                    break;

                case "Docked":
                    je = new JournalDocked(jo, reader);
                    break;

                case "Died":
                    je = new JournalDied(jo, reader);
                    break;

                case "fileheader":
                case "Fileheader":
                    je = new JournalFileHeader(jo, reader);
                    break;

                case "FSDJump":
                    je = new JournalFSDJump(jo, reader);
                    break;

                case "Location":
                    je = new JournalLocation(jo, reader);
                    break;

                case "LoadGame":
                    je = new JournalLoadGame(jo, reader);
                    break;

                case "Scan":
                    je = new JournalScan(jo, reader);
                    break;


                case "SellExplorationData":
                    je = new JournalSellExplorationData(jo, reader);
                    break;

                case "Undocked":
                    je = new JournalUndocked(jo, reader);
                    break;

                case "DockingCancelled":
                    je = new JournalDockingCancelled(jo, reader);
                    break;

                case "DockingDenied":
                    je = new JournalDockingDenied(jo, reader);
                    break;
                case "DockingGranted":
                    je = new JournalDockingGranted(jo, reader);
                    break;

                case "DockingRequested":
                    je = new JournalDockingRequested(jo, reader);
                    break;

                case "DockingTimeout":
                    je = new JournalDockingTimeout(jo, reader);
                    break;


                case "EngineerApply":
                    je = new JournalEngineerApply(jo, reader);
                    break;
                case "EngineerCraft":
                    je = new JournalEngineerCraft(jo, reader);
                    break;
                case "EngineerProgress":
                    je = new JournalEngineerProgress(jo, reader);
                    break;

                case "EscapeInterdiction":
                    je = new JournalEscapeInterdiction(jo, reader);
                    break;
                case "FactionKillBond":
                    je = new JournalFactionKillBond(jo, reader);
                    break;
                case "HeatDamage":
                    je = new JournalHeatDamage(jo, reader);
                    break;
                case "HeatWarning":
                    je = new JournalHeatWarning(jo, reader);
                    break;
                case "HullDamage":
                    je = new JournalHullDamage(jo, reader);
                    break;
                case "Interdicted":
                    je = new JournalInterdicted(jo, reader);
                    break;
                case "Interdiction":
                    je = new JournalInterdiction(jo, reader);
                    break;



                case "FuelScoop":
                    je = new JournalFuelScoop(jo, reader);
                    break;

                case "JetConeBoost":
                    je = new JournalJetConeBoost(jo, reader);
                    break;
                case "JetConeDamage":
                    je = new JournalJetConeDamage(jo, reader);
                    break;


                case "LaunchSRV":
                    je = new JournalLaunchSRV(jo, reader);
                    break;
                case "Liftoff":
                    je = new JournalLiftoff(jo, reader);
                    break;
                case "MarketBuy":
                    je = new JournalMarketBuy(jo, reader);
                    break;
                case "MarketSell":
                    je = new JournalMarketSell(jo, reader);
                    break;


                case "Continued":
                    je = new JournalContinued(jo, reader);
                    break;

                case "Rank":
                    je = new JournalRank(jo, reader);
                    break;
                    
                case "Progress":
                    je = new JournalRank(jo, reader);
                    break;

                case "SupercruiseEntry":
                    je = new JournalSupercruiseEntry(jo, reader);
                    break;
                case "SupercruiseExit":
                    je = new JournalSupercruiseExit(jo, reader);
                    break;

                case "ModuleBuy":
                    je = new JournalModuleBuy(jo, reader);
                    break;
                case "ModuleSell":
                    je = new JournalModuleSell(jo, reader);
                    break;
                case "ModuleSwap":
                    je = new JournalModuleSwap(jo, reader);
                    break;

                case "RefuelAll":
                    je = new JournalRefuelAll(jo, reader);
                    break;
                case "RefuelPartial":
                    je = new JournalRefuelPartial(jo, reader);
                    break;
                case "Repair":
                    je = new JournalRepair(jo, reader);
                    break;
                case "RestockVehicle":
                    je = new JournalRestockVehicle(jo, reader);
                    break;

                case "Resurrect":
                    je = new JournalResurrect(jo, reader);
                    break;

                case "Screenshot":
                    je = new JournalScreenshot(jo, reader);
                    break;
                case "SelfDestruct":
                    je = new JournalShieldState(jo, reader);
                    break;
                case "ShieldState":
                    je = new JournalSelfDestruct(jo, reader);
                    break;
                case "ShipyardBuy":
                    je = new JournalShipyardBuy(jo, reader);
                    break;
                case "ShipyardNew":
                    je = new JournalShipyardNew(jo, reader);
                    break;
                case "ShipyardSell":
                    je = new JournalShipyardSell(jo, reader);
                    break;
                case "ShipyardSwap":
                    je = new JournalShipyardSwap(jo, reader);
                    break;
                case "ShipyardTransfer":
                    je = new JournalShipyardTransfer(jo, reader);
                    break;

                case "Synthesis":
                    je = new JournalSynthesis(jo, reader);
                    break;
                case "Touchdown":
                    je = new JournalTouchdown(jo, reader);
                    break;
                case "USSDrop":
                    je = new JournalUSSDrop(jo, reader);
                    break;
                case "VehicleSwitch":
                    je = new JournalVehicleSwitch(jo, reader);
                    break;
                case "WingAdd":
                    je = new JournalWingAdd(jo, reader);
                    break;
                case "WingJoin":
                    je = new JournalWingJoin(jo, reader);
                    break;
                case "WingLeave":
                    je = new JournalWingLeave(jo, reader);
                    break;



                case "Bounty":
              
                case "CapShipBond":
                case "ClearSavedGame":
              
                case "DatalinkScan":
                case "DockFighter":
                case "DockSRV":
                case "EjectCargo":
                case "LaunchFighter":
 
                case "MaterialCollected":
                case "MaterialDiscarded":
                case "MaterialDiscovered":
                case "MiningRefined":
                case "MissionAbandoned":
                case "MissionAccepted":
                case "MissionCompleted":
                case "MissionFailed":
                case "NewCommander":
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

          
           
                    je = new JournalUnhandled(jo, Eventstr, reader);
                    break;

                default:
                    je = new JournalUnknown(jo, Eventstr, reader);
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

    }

}