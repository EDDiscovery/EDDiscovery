using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    public class JournalLoadGame : JournalEntry
    {
        public JournalLoadGame(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.LoadGame, reader)
        {
            Commander = Tools.GetStringDef("Commander");
            Ship = Tools.GetStringDef("Ship");
            ShipId = Tools.GetInt("ShipID");
            StartLanded = Tools.GetBool("StartLanded");
            StartDead = Tools.GetBool("StartDead");
            GameMode = Tools.GetStringDef("GameMode");
            Group = Tools.GetStringDef("Group");
            Credits = Tools.GetInt("Credits");
            Loan = Tools.GetInt("Loan");

            var cmdr = reader.Commander;

            if (cmdr == null || cmdr.Name != Commander)
            {
                cmdr = EDDiscovery2.EDDConfig.Instance.listCommanders.FirstOrDefault(c => c.Name.Equals(Commander, StringComparison.InvariantCultureIgnoreCase));
                if (cmdr == null)
                {
                    cmdr = EDDiscovery2.EDDConfig.Instance.GetNewCommander(Commander);
                }
                reader.Commander = cmdr;
            }
        }

        public string Commander { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public bool StartLanded { get; set; }
        public bool StartDead { get; set; }
        public string GameMode { get; set; }
        public string Group { get; set; }
        public int Credits { get; set; }
        public int Loan { get; set; }
    }
}
