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
            Commander = evt.Value<string>("Commander");
            Ship = evt.Value<string>("Ship");
            ShipId = evt.Value<int>("ShipID");
            StartLanded = evt.Value<bool>("StartLanded");
            StartDead = evt.Value<bool>("StartDead");
            GameMode = evt.Value<string>("GameMode");
            Group = evt.Value<string>("Group");
            Credits = evt.Value<int>("Credits");
            Loan = evt.Value<int>("Loan");

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
