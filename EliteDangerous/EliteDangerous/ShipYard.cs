using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
{
    public class ShipYard : IEquatable<ShipYard>
    {
        public class ShipyardItem : IEquatable<ShipyardItem>
        {
            public long id;
            public string FDShipType;
            public string ShipType;
            public string ShipType_Localised;
            public long ShipPrice;

            public void Normalise()
            {
                FDShipType = ShipType;
                ShipType = JournalFieldNaming.GetBetterShipName(ShipType);
                ShipType_Localised = ShipType_Localised.Alt(ShipType);
            }

            public bool Equals(ShipyardItem other)
            {
                return id == other.id && string.Compare(FDShipType, other.FDShipType) == 0 &&
                            string.Compare(ShipType_Localised, other.ShipType_Localised) == 0 && ShipPrice == other.ShipPrice;
            }
        }

        public ShipyardItem[] Ships { get; private set; }
        public string StationName { get; private set; }
        public string StarSystem { get; private set; }
        public DateTime Datetime { get; private set; }

        public ShipYard()
        {
        }

        public ShipYard(string st, string sy, DateTime dt , ShipyardItem[] it  )
        {
            StationName = st;
            StarSystem = sy;
            Datetime = dt;
            Ships = it;
            if ( Ships != null )
                foreach (ShipYard.ShipyardItem i in Ships)
                    i.Normalise();
        }

        public bool Equals(ShipYard other)
        {
            return string.Compare(StarSystem, other.StarSystem) == 0 && string.Compare(StationName, other.StationName) == 0 &&
                CollectionStaticHelpers.Equals(Ships,other.Ships);
        }

        public string Location { get { return StarSystem + ":" + StationName; } }

        public string Ident(bool utc) { return StarSystem + ":" + StationName + " on " +
                ((utc) ? Datetime.ToString() : Datetime.ToLocalTime().ToString());
        }

        public List<string> ShipList() { return (from x1 in Ships select x1.ShipType_Localised).ToList(); }

        public ShipyardItem Find(string shiplocname) { return Array.Find(Ships, x => x.ShipType_Localised.Equals(shiplocname)); }

    }

    public class ShipYardList
    {
        public List<ShipYard> ShipYards { get; private set; }

        public ShipYardList()
        {
            ShipYards = new List<ShipYard>();
        }

        public List<string> ShipList()
        {
            List<string> ships = new List<string>();
            foreach (ShipYard yard in ShipYards)
                ships.AddRange(yard.ShipList());
            return (from x in ships select x).Distinct().ToList();
        }

        public List<ShipYard> YardListWithoutRepeats()
        {
            ShipYard last = null;
            List<ShipYard> yards = new List<ShipYard>();

            foreach (var x1 in ShipYards)
            {
                if (last == null || !x1.Location.Equals(last.Location) || (x1.Datetime - last.Datetime).Seconds > 60 * 5)
                {
                    yards.Add(x1);
                    last = x1;
                }
            }

            return yards;
        }

        public List<Tuple<ShipYard, ShipYard.ShipyardItem>> GetShipLocationsFromYardsWithoutRepeat(string ship)       // without repeats note
        {
            List<Tuple<ShipYard, ShipYard.ShipyardItem>> list = new List<Tuple<ShipYard, ShipYard.ShipyardItem>>();
            
            foreach ( ShipYard yard in YardListWithoutRepeats())
            {
                ShipYard.ShipyardItem i = yard.Find(ship);
                if (i != null)
                    list.Add(new Tuple<ShipYard, ShipYard.ShipyardItem>(yard, i));
            }

            return list;
        }

        public void Process(JournalEntry je, SQLiteConnectionUser conn)
        {
            if ( je.EventTypeID == JournalTypeEnum.Shipyard)
            {
                JournalEvents.JournalShipyard js = je as JournalEvents.JournalShipyard;
                if (js.Yard.Ships != null)     // just in case we get a bad shipyard with no ship data
                {
                    //System.Diagnostics.Debug.WriteLine("Add yard data for " + js.Yard.StarSystem + ":" + js.Yard.StationName);
                    ShipYards.Add(js.Yard);
                }
            }
        }
    }
}
