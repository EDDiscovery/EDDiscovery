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
            LoadGameCommander = Tools.GetStringDef(evt["Commander"]);
            Ship = Tools.GetStringDef(evt["Ship"]);
            ShipId = Tools.GetInt(evt["ShipID"]);
            StartLanded = Tools.GetBool(evt["StartLanded"]);
            StartDead = Tools.GetBool(evt["StartDead"]);
            GameMode = Tools.GetStringDef(evt["GameMode"]);
            Group = Tools.GetStringDef(evt["Group"]);
            Credits = Tools.GetInt(evt["Credits"]);
            Loan = Tools.GetInt(evt["Loan"]);
        }

        public string LoadGameCommander { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public bool StartLanded { get; set; }
        public bool StartDead { get; set; }
        public string GameMode { get; set; }
        public string Group { get; set; }
        public int Credits { get; set; }
        public int Loan { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.loadgame; } }

    }
}
