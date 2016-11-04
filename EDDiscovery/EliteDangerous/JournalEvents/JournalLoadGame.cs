using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    public class JournalLoadGame : JournalEntry
    {
        public JournalLoadGame(JObject evt ) : base(evt, JournalTypeEnum.LoadGame)
        {
            LoadGameCommander = JSONHelper.GetStringDef(evt["Commander"]);
            Ship = JSONHelper.GetStringDef(evt["Ship"]);
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            StartLanded = JSONHelper.GetBool(evt["StartLanded"]);
            StartDead = JSONHelper.GetBool(evt["StartDead"]);
            GameMode = JSONHelper.GetStringDef(evt["GameMode"]);
            Group = JSONHelper.GetStringDef(evt["Group"]);
            Credits = JSONHelper.GetLong(evt["Credits"]);
            Loan = JSONHelper.GetLong(evt["Loan"]);
        }

        public string LoadGameCommander { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public bool StartLanded { get; set; }
        public bool StartDead { get; set; }
        public string GameMode { get; set; }
        public string Group { get; set; }
        public long Credits { get; set; }
        public long Loan { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.loadgame; } }


        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            if (mcl.CashTotal != Credits)
            {
                mcl.AddEvent(Id, EventTimeUTC, EventTypeID, "Cash total differs, adjustment", Credits - mcl.CashTotal);
            }
        }

    }
}
