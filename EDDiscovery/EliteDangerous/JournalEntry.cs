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

        public JournalEntry(JObject jo, JournalTypeEnum jtype, EDJournalReader reader)
        {
            jEventData = jo;
            eventType = jtype;

            eventTimeUTC = DateTime.Parse(jo.Value<string>("timestamp"), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            JournalId = reader.JournalId;
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
                case "Docked":
                    je = new JournalDocked(jo, reader);
                    break;

                case "Died":
                    je = new JournalDied(jo, reader);
                    break;

                case "fileheader":
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

                case "Continued":
                    je = new JournalContinued(jo, reader);
                    break;


                case "Bounty":
                case "BuyAmmo":
                case "BuyDrones":
                case "BuyExplorationData":
                case "BuyTradeData":
                case "CapShipBond":
                case "ClearSavedGame":
                case "CockpitBreached":
                case "CollectCargo":
                case "CommitCrime":
                case "CommunityGoalJoin":
                case "CommunityGoalReward":
                case "DatalinkScan":
                case "DockFighter":
                case "DockSRV":
                case "EjectCargo":
                case "EngineerApply":
                case "EngineerCraft":
                case "EngineerProgress":
                case "EscapeInterdiction":
                case "FactionKillBond":
                case "FuelScoop":
                case "HeatDamage":
                case "HeatWarning":
                case "HullDamage":
                case "Interdicted":
                case "Interdiction":
                case "LaunchFighter":
                case "LaunchSRV":
                case "Liftoff":
                case "MarketBuy":
                case "MarketSell":
                case "MaterialCollected":
                case "MaterialDiscarded":
                case "MaterialDiscovered":
                case "MiningRefined":
                case "MissionAbandoned":
                case "MissionAccepted":
                case "MissionCompleted":
                case "MissionFailed":
                case "ModuleBuy":
                case "ModuleSell":
                case "ModuleSwap":
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
                case "Progress":
                case "Promotion":
                case "Rank":
                case "RebootRepair":
                case "ReceiveText":
                case "RedeemVoucher":
                case "RefuelAll":
                case "RefuelPartial":
                case "Repair":
                case "RestockVehicle":
                case "Resurrect":
                case "Screenshot":
                case "SelfDestruct":
                case "SellDrones":
                case "SendText":
                case "ShieldState":
                case "ShipyardBuy":
                case "ShipyardNew":
                case "ShipyardSell":
                case "ShipyardSwap":
                case "ShipyardTransfer":
                case "SupercruiseEntry":
                case "SupercruiseExit":
                case "Synthesis":
                case "Touchdown":
                case "USSDrop":
                case "VehicleSwitch":
                case "WingAdd":
                case "WingJoin":
                case "WingLeave":
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