using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when a crime is recorded against the player
    //Parameters:
    //•	CrimeType
    //•	Faction
    //Optional parameters (depending on crime)
    //•	Victim
    //•	Fine
    //•	Bounty
    public class JournalCommitCrime : JournalEntry
    {
        public JournalCommitCrime(JObject evt ) : base(evt, JournalTypeEnum.CommitCrime)
        {
            CrimeType = JSONHelper.GetStringDef(evt["CrimeType"]);
            Faction = JSONHelper.GetStringDef(evt["Faction"]);
            Victim = JSONHelper.GetStringDef(evt["Victim"]);
            VictimLocalised = JSONHelper.GetStringDef(evt["Victim_Localised"]);
            Fine = JSONHelper.GetLongNull(evt["ddevent"]);
            Bounty = JSONHelper.GetLongNull(evt["Bounty"]);
        }
        public string CrimeType { get; set; }
        public string Faction { get; set; }
        public string Victim { get; set; }
        public string VictimLocalised { get; set; }
        public long? Fine { get; set; }
        public long? Bounty { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.commitcrime; } }

        public void LedgerNC(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            string v = (VictimLocalised.Length > 0) ? VictimLocalised : Victim;

            if (v.Length == 0)
                v = Faction;

            if (Fine.HasValue)
                v += " Fine " + Fine.Value.ToString("N0");

            if (Bounty.HasValue)
                v += " Bounty " + Bounty.Value.ToString("N0");

            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, CrimeType + " on " + v);
        }

    }
}
