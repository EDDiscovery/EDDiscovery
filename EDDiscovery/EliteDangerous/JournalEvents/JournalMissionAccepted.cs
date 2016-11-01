using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when starting a mission
    //Parameters:
    //•	Name: name of mission
    //•	Faction: faction offering mission
    //Optional Parameters (depending on mission type)
    //•	Commodity: commodity type
    //•	Count: number required / to deliver
    //•	Target: name of target
    //•	TargetType: type of target
    //•	TargetFaction: target’s faction
//•	Expiry: mission expiry time, in ISO 8601
//•	DestinationSystem
//•	DestinationStation
//•	PassengerCount
//•	PassengerVIPs: bool
//•	PassengerWanted: bool
//•	PassengerType: eg Tourist, Soldier, Explorer,...



    public class JournalMissionAccepted : JournalEntry
    {
        public JournalMissionAccepted(JObject evt ) : base(evt, JournalTypeEnum.MissionAccepted)
        {
            Name = JSONHelper.GetStringDef(evt["Name"]);
            Faction = JSONHelper.GetStringDef(evt["Faction"]);
            Commodity = JSONHelper.GetStringDef(evt["Commodity"]);
            Count = JSONHelper.GetIntNull(evt["Count"]);
            Target = JSONHelper.GetStringDef(evt["Target"]);
            TargetType = JSONHelper.GetStringDef(evt["TargetType"]);
            TargetFaction = JSONHelper.GetStringDef(evt["TargetFaction"]);
            MissionId = JSONHelper.GetInt(evt["MissionID"]);

            if (!JSONHelper.IsNullOrEmptyT(evt["Expiry"]))
                Expiry = DateTime.Parse(evt.Value<string>("Expiry"), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

            DestinationSystem = JSONHelper.GetStringDef(evt["DestinationSystem"]);
            DestinationStation = JSONHelper.GetStringDef(evt["DestinationStation"]);
            PassengerType = JSONHelper.GetStringDef(evt["PassengerType"]);

            PassengerCount = JSONHelper.GetInt(evt["PassengerCount"]);
            PassengerVIPs = JSONHelper.GetBool(evt["PassengerVIPs"]);
            PassengerWanted = JSONHelper.GetBool(evt["PassengerWanted"]);



        }
        public string Name { get; set; }
        public string Faction { get; set; }
        public string Commodity { get; set; }
        public int? Count { get; set; }
        public string Target { get; set; }
        public string TargetType { get; set; }
        public string TargetFaction { get; set; }
        public int MissionId { get; set; }

        public DateTime Expiry { get; set; }
        public string DestinationSystem { get; set; }
        public string DestinationStation { get; set; }
        public int PassengerCount { get; set; }
        public bool PassengerVIPs { get; set; }
        public bool PassengerWanted { get; set; }
        public string PassengerType { get; set; }



        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";MissionID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.missionaccepted; } }


    }

}

