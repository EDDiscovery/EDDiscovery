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
        Fileheader = 300,
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
        ScientificResearch = 775,
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
        ModuleSellRemote = 990,
        FetchRemoteModule = 1000,
        MassModuleStore = 1010,
        DatalinkVoucher = 1020,
        DataScanned = 1030,
        CommunityGoalDiscard = 1040,

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

        public JournalEntry(JournalTypeEnum jtype)
        {
            jEventData = null;
            EventTypeID = jtype;
            EventTypeStr = jtype.ToString();
            EventTimeUTC = DateTime.UtcNow;
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
            if (jr.EventTimeUTC == default(DateTime))
                jr.EventTimeUTC = ((DateTime)dr["EventTime"]).ToUniversalTime();
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
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                bool ret = Add(cn);
                return ret;
            }
        }

        public bool Add(SQLiteConnectionUser cn, DbTransaction tn = null)
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
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnectionUser cn, DbTransaction tn = null)
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

        static public void Delete(long idvalue)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                Delete(idvalue,cn);
            }
        }

        static public void Delete(long idvalue, SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("DELETE FROM JournalEntries WHERE id = @id"))
            {
                cmd.AddParameterWithValue("@id", idvalue);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);
            }
        }

        //dist >0 to update
        public static void UpdateEDSMIDPosJump(long journalid, ISystem system, bool jsonpos , double dist, SQLiteConnectionUser cn = null, DbTransaction tn = null)
        {
            bool ownconn = false;

            try
            {
                if (cn == null)
                {
                    ownconn = true;
                    cn = new SQLiteConnectionUser(utc: true);
                }

                JournalEntry ent = Get(journalid, cn, tn);

                if (ent != null)
                {
                    JObject jo = (JObject)JObject.Parse(ent.EventDataString);

                    if (jsonpos)
                    {
                        jo["StarPos"] = new JArray() { system.x, system.y, system.z };
                        jo["StarPosFromEDSM"] = true;
                    }

                    if (dist > 0)
                        jo["JumpDist"] = dist;

                    using (DbCommand cmd2 = cn.CreateCommand("Update JournalEntries set EventData = @EventData, EdsmId = @EdsmId where ID = @ID", tn))
                    {
                        cmd2.AddParameterWithValue("@ID", journalid);
                        cmd2.AddParameterWithValue("@EventData", jo.ToString());
                        cmd2.AddParameterWithValue("@EdsmId", system.id_edsm);

                        //System.Diagnostics.Trace.WriteLine(string.Format("Update journal ID {0} with pos {1}/edsmid {2} dist {3}", journalid, jsonpos, system.id_edsm, dist));
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

        public static void UpdateMapColour(long journalid, int mapcolour)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
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
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
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
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
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

        public static long AddEDDItemSet(int cmdrid, DateTime dt, long jidofitemset, List<MaterialCommodities> changelist )     // add item, return journal ID
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
            {
                JournalEDDItemSet jis;

                if (jidofitemset > 0)                                       // 0 means currently not on an item..
                    jis = (JournalEDDItemSet)Get(jidofitemset, cn);
                else
                {
                    jis = new JournalEDDItemSet();
                    jis.EventTimeUTC = dt;
                    jis.CommanderId = cmdrid;
                }

                foreach (MaterialCommodities mc in changelist)              // reset the list to these.. or add on if there are more
                {
                    if (mc.category.Equals(MaterialCommodities.CommodityCategory))
                        jis.Commodities.Set(mc.fdname, mc.count, mc.price);
                    else
                        jis.Materials.Set(mc.category, mc.fdname, mc.count);
                }

                jis.UpdateState();

                if (jidofitemset > 0)
                    jis.Update(cn);
                else
                    jis.Add(cn);

                return jis.Id;
            }
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
            using (DbCommand cmd = cn.CreateCommand("select * from JournalEntries where EventType=@ev", tn))
            {
                cmd.AddParameterWithValue("@ev", eventtype);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    List<JournalEntry> entries = new List<JournalEntry>();

                    while (reader.Read())
                    {
                        entries.Add(CreateJournalEntry(reader));
                    }

                    return entries;
                }
            }
        }

        static public List<JournalEntry> GetAll(int commander = -999)
        {
            List<JournalEntry> list = new List<JournalEntry>();

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
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

            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
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

        public static void RemoveGeneratedKeys(JObject obj, bool removeLocalised)
        {
            foreach (JProperty prop in obj.Properties().ToList())
            {
                if (prop.Name.StartsWith("EDD") || (removeLocalised && prop.Name.EndsWith("_Localised")))
                {
                    obj.Remove(prop.Name);
                }
            }

            obj.Remove("StarPosFromEDSM");
        }

        public static bool AreSameEntry(JournalEntry ent1, JournalEntry ent2, JObject ent1jo = null, JObject ent2jo = null)
        {
            if (ent1.jEventData == null || ent2.jEventData == null)
                return false;

            if (ent1jo == null)
            {
                ent1jo = (JObject)ent1.jEventData.DeepClone();
                RemoveGeneratedKeys(ent1jo, false);
            }

            if (ent2jo == null)
            {
                ent2jo = (JObject)ent2.jEventData.DeepClone();
                RemoveGeneratedKeys(ent2jo, false);
            }

            return JToken.DeepEquals(ent1jo, ent2jo);
        }

        public static List<JournalEntry> FindEntry(JournalEntry ent)
        {
            List<JournalEntry> entries = new List<JournalEntry>();
            JObject entjo = (JObject)ent.jEventData.DeepClone();
            RemoveGeneratedKeys(entjo, false);

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
                            if (!AreSameEntry(ent, jent, entjo))
                            {
                                entries.Add(jent);
                            }
                        }
                    }
                }
            }

            return entries;
        }

        public static int RemoveDuplicateFSDEntries(int currentcmdrid )
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

                    if ( prev != null && current != null )
                    {
                        bool previssame = (prev.StarSystem.Equals(current.StarSystem, StringComparison.CurrentCultureIgnoreCase) && (!prev.HasCoordinate || !current.HasCoordinate || (prev.StarPos - current.StarPos).LengthSquared < 0.01));

                        if ( previssame )
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

        static public Type TypeOfJournalEntry(string text)
        {
            //foreach (JournalTypeEnum jte in Enum.GetValues(typeof(JournalTypeEnum))) // check code only to make sure names match
            //{
                //Type p = Type.GetType("EDDiscovery.EliteDangerous.JournalEvents.Journal" + jte.ToString());
                //Debug.Assert(p != null);
            //}

            Type t = Type.GetType("EDDiscovery.EliteDangerous.JournalEvents.Journal" + text,false,true); // no exception, ignore case here
            return t;
        }

        static public JournalEntry CreateJournalEntry(string text)
        {
            JObject jo = (JObject)JObject.Parse(text);

            string Eventstr = JSONHelper.GetStringNull(jo["event"]);

            if (Eventstr == null)  // Should normaly not happend unless corrupt string.
                return null;

            Type jtype = TypeOfJournalEntry(Eventstr);

            if ( jtype == null )
            {
                System.Diagnostics.Trace.WriteLine("Unknown event: " + Eventstr);
                return new JournalUnknown(jo, Eventstr);
            }
            else
                return (JournalEntry)Activator.CreateInstance(jtype, jo);
        }

        static public System.Drawing.Bitmap GetIcon(string eventtypestr, string seltext = null )    // get ICON associated with the event type.
        {
            Type jtype = TypeOfJournalEntry(eventtypestr);

            if (jtype == null)
            {
                return EDDiscovery.Properties.Resources.genericevent;
            }

            System.Reflection.MethodInfo m = jtype.GetMethod("IconSelect");                 // first we see if the class defines this function..

            if ( m != null )
            {
                return (System.Drawing.Bitmap)m.Invoke(null, new Object [] { seltext });    // if so, pass it the string and let it pick the icon
            }
            else
            {
                System.Reflection.PropertyInfo p = jtype.GetProperty("Icon");               // else use the Icon property, or if its not defined, its a generic event
                System.Reflection.MethodInfo getter = p?.GetGetMethod();
                return (getter != null) ? ((System.Drawing.Bitmap)getter.Invoke(null, null)) : EDDiscovery.Properties.Resources.genericevent;
            }
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
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser(utc: true))
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

            {           // unique field names across multiple entries.  First up so later ones can override if required

                jc.AddScale("MassEM", 1.0, "0.0000'em'", "Mass");
                jc.AddScale("MassMT", 1.0, "0.0'mt'", "Mass");
                jc.AddScale("SurfacePressure", 1.0, "0.0'p'");
                jc.AddScale("Radius", 1.0 / 1000, "0.0'km'");
                jc.AddScale("InnerRad", 1.0 / 1000, "0.0'km'", "Inner Radius");
                jc.AddScale("OuterRad", 1.0 / 1000, "0.0'km'", "Outer Radius");
                jc.AddScale("SemiMajorAxis", 1.0 / 1000, "0.0'km'", "Semi Major Axis");
                jc.AddScale("OrbitalPeriod;RotationPeriod", 1.0 / 86400, "0.0' days orbit'", "");
                jc.AddScale("SurfaceGravity", 1.0 / 9.8, "0.0'g'");
                jc.AddScale("SurfaceTemperature", 1.0, "0.0'K'");
                jc.AddScale("Scooped", 1.0, "'Scooped '0.0't'", "", "FuelScoop");
                jc.AddScale("Total", 1.0, "'Fuel Level '0.0't'", "", "FuelScoop");
                jc.AddScale("FuelUsed", 1.0, "'Fuel Used '0.0't'", "");
                jc.AddScale("FuelLevel", 1.0, "'Fuel Level Left '0.0't'", "");
                jc.AddScale("Amount", 1.0, "'Fuel Bought '0.0't'", "", "RefuelAll");
                jc.AddScale("BoostValue", 1.0, "0.0' boost'", "", "JetConeBoost");
                jc.AddScale("StarPos", 1.0, "0.0", "");          // any entry StarPos loses it name (inside arrays). StarPos as an array name gets printed sep.

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

                jc.AddPrePostfix("To", "To ", "", "VehicleSwitch");
                jc.AddPrePostfix("Name", "", "", "CrewAssign");

                jc.AddPrePostfix("Role", "; role", "", "CrewAssign");
                jc.AddPrePostfix("Cost;ShipPrice;BaseValue", "; credits", "");
                jc.AddPrePostfix("Bonus", "; credits bonus", "");
                jc.AddPrePostfix("Amount", "; credits", "", "PayLegacyFines");
                jc.AddPrePostfix("BuyPrice", "Bought for ; credits", "");
                jc.AddPrePostfix("SellPrice", "Sold for ; credits", "");
                jc.AddPrePostfix("TotalCost", "Total cost ; credits", "");

                jc.AddPrePostfix("LandingPad", "On pad ", "");

                jc.AddPrePostfix("BuyItem", "; bought", "");
                jc.AddPrePostfix("SellItem", "; sold", "");

                jc.AddPrePostfix("Credits", "; credits", "", "LoadGame");
                jc.AddPrePostfix("Ship;ShipType", "Ship ;", "");
                jc.AddPrePostfix("StoreOldShip", "; stored", "");

                jc.AddScale("Health", 100.0, "'Health' 0.0'%'", "");

                jc.AddSpecial("Latitude", JSONConverters.Types.TLat, "");
                jc.AddSpecial("Longitude", JSONConverters.Types.TLong, "");

                jc.AddPrePostfix("Reward", "; credits", "");
            }

            {           //missions
                jc.AddPrePostfix("Name", "", "", "MissionAccepted;MissionAbandoned;MissionCompleted;MissionFailed");
            }

            {           // transfers
                string transfer = JL(new[] { JournalTypeEnum.ShipyardTransfer });
                jc.AddScale("Distance", 1.0 / 299792458.0 / 365 / 24 / 60 / 60, "'Distance' 0.0'ly'", "", transfer);
                jc.AddPrePostfix("TransferPrice", "; credits", "", transfer);
            }

            {           // misc
                jc.AddPrePostfix("Name", "; settlement", "", "ApproachSettlement");
                jc.AddPrePostfix("Item", ";", "", "Repair");
            }

            {           // scans
                string scan = JL(new[] { JournalTypeEnum.Scan });
                jc.AddPrePostfix("BodyName", "Scan ", "", scan);
                jc.AddScale("DistanceFromArrivalLS", 1.0, "0.0' ls from arrival point'", "", scan );
                jc.AddPrePostfix("StarType", "; type star", "", scan);
                jc.AddScale("StellarMass", 1.0, "0.0' stellar masses'", "", scan);
                jc.AddScale("Radius", 1.0 / 1000.0, "0.0' km radius'", "", scan);
                jc.AddScale("AbsoluteMagnitude", 1.0, "0.0' absolute magnitude'", "", scan);
                jc.AddScale("OrbitalPeriod", 1.0 / 86400, "0.0' days orbit'", "", scan);
                jc.AddScale("RotationPeriod", 1.0 / 86400, "0.0' days rotation'", "", scan);
                jc.AddPrePostfix("PlanetClass", "; planet class", "", scan);

            }

            {           // engineering
                string engineer = JL(new[] { JournalTypeEnum.EngineerProgress, JournalTypeEnum.EngineerApply, JournalTypeEnum.EngineerCraft });
                jc.AddPrePostfix("Engineer", "From ", "", engineer);
                jc.AddPrePostfix("Progress", "", "", engineer);
            }

            {           // bounties
            }

            {
                string rank = JL(new[] { JournalTypeEnum.Rank });
                jc.AddIndex("Combat", "; combat;0;Harmless;Mostly Harmless;Novice;Competent;Expert;Master;Dangerous;Deadly;Elite", "", rank);
                jc.AddIndex("Trade", "; trader;0;Penniless;Mostly Penniless;Peddler;Dealer;Merchant;Broker;Entrepreneur;Tycoon;Elite", "", rank);
                jc.AddIndex("Explore", "; explorer;0;Aimless;Mostly Aimless;Scout;Surveyor;Trailblazer;Pathfinder;Ranger;Pioneer;Elite", "", rank);
                jc.AddIndex("Empire", "; Empire;0;None;Outsider;Serf;Master;Squire;Knight;Lord;Baron;Viscount;Count;Earl;Marquis;Duke;Prince;King", "", rank);
                jc.AddIndex("Federation", "; Federation;0;None;Recruit;Cadet;Midshipman;Petty Officer;Chief Pretty Officer;Warren Officer;Ensign;Lieutenant;Lieutenant Commander;Post Commander;Post Captain;Rear Admiral;Vice Admiral;Admiral", "", rank);
            }


            {       // places where commodities occur
                string commodities = JL(new[] { JournalTypeEnum.MarketBuy, JournalTypeEnum.MarketSell , JournalTypeEnum.MiningRefined });
                jc.AddSpecial("Type", JSONConverters.Types.TMaterialCommodity, ";", "", commodities);
                jc.AddPrePostfix("Count", ";", "", commodities);
            }

            {
                string materials = JL(new[] { JournalTypeEnum.MaterialCollected, JournalTypeEnum.MaterialDiscarded, JournalTypeEnum.MaterialDiscovered });
                jc.AddSpecial("Name", JSONConverters.Types.TMaterialCommodity, ";", "", materials);
                jc.AddPrePostfix("Category", ";", "", materials);
                jc.AddPrePostfix("Count", "; items", "", materials);
            }

            return jc;
        }

        static string JL( JournalTypeEnum[] ar )
        {
            string s = "";
            foreach (JournalTypeEnum a in ar)
                s += ((s.Length > 0) ? ";" : "") + a.ToString();

            return s;
        }

        static public List<string> GetListOfEventsWithOptMethod(bool towords, string method = null, string method2 = null)
        {
            List<string> ret = new List<string>();

            foreach (JournalTypeEnum jte in Enum.GetValues(typeof(JournalTypeEnum)))
            {
                string n = jte.ToString();

                if (method == null)
                {
                    ret.Add((towords) ? Tools.SplitCapsWord(n) : n);
                }
                else
                {
                    Type jtype = TypeOfJournalEntry(n);

                    if (jtype != null)      // may be null, Unknown for instance
                    {
                        System.Reflection.MethodInfo m = jtype.GetMethod(method);

                        if (m == null && method2 != null )
                            m = jtype.GetMethod(method2);

                        if ( m != null )
                            ret.Add( (towords) ? Tools.SplitCapsWord(n) : n );
                    }
                }
            }

            return ret;
        }

    }

}