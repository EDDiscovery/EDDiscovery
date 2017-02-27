using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
{
    public struct ShipModule
    {
        public string slot { get; set; }
        public string item { get; set; }
        public bool enabled { get; set; }
        public int priority { get; set; }
        public int ammoclip { get; set; }               // or 0
        public int ammohopper { get; set; }             // or 0
        public string blueprint { get; set; }           // or null
        public int blueprintlevel { get; set; }         // or 0

        public string slotfriendlyname { get { return slot.SplitCapsWordUnderscoreTitleCase(); } }
        public string itemfriendlyname { get { return item.SplitCapsWordUnderscoreTitleCase(); } }
    }

    public class ShipModules
    {
        public int shipid { get; set; }
        public string shiptype { get; set; }
        public string shipusername { get; set; }
        public List<ShipModule> items { get; private set; }

        public ShipModules(int id )
        {
            items = new List<ShipModule>();
            shipid = id;
        }
    }

    public class ShipListModules
    {
        private Dictionary<int, ShipModules> ships;         // by shipid
        private ShipModules currentship;

        public ShipListModules()
        {
            ships = new Dictionary<int, ShipModules>();
            currentship = null;
        }

        public void SetCurrentShip( string type, int id)
        {
            ShipModules sm = EnsureShip(id);
            sm.shiptype = type;
            currentship = sm;
        }

        private ShipModules EnsureShip( int id )
        {
            if (ships.ContainsKey(id))
                return ships[id];
            else
            {
                ShipModules sm = new ShipModules(id);
                ships[id] = sm;
                return sm;
            }
        }
    }

}
