/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDDiscovery.EliteDangerous.JournalEvents;

namespace EDDiscovery.EliteDangerous
{
    public class ShipModules
    {
        public string shiptype { get; set; }
        public string shipusername { get; set; }
        public List<JournalLoadout.ShipModule> items { get; private set; }

        public ShipModules()
        {
            items = new List<JournalLoadout.ShipModule>();
        }

        public ShipModules Clone()          // call if changing the items/username
        {
            ShipModules sm = new ShipModules();
            sm.shiptype = this.shiptype;
            sm.shipusername = this.shipusername;
            sm.items = new List<JournalLoadout.ShipModule>();
            foreach (JournalLoadout.ShipModule si in this.items)
                sm.items.Add(si.Clone());

            return sm;
        }
    }

    public class ShipListModules
    {
        private Dictionary<int, ShipModules> ships;         // by shipid
        private int currentshipid;
        private bool CurrentShipIDValid { get { return currentshipid >= 0; } }

        public ShipListModules()
        {
            ships = new Dictionary<int, ShipModules>();
            currentshipid = -1;
        }

        public ShipListModules(Dictionary<int, ShipModules> other, int otherid )
        {
            ships = new Dictionary<int, ShipModules>(other);
            currentshipid = otherid;
        }

        public void SetCurrentShip(string type, int id)
        {
            ShipModules sm = EnsureShip(id,false);      // this either gets current ship or makes a new one.. don't need to clone
            sm.shiptype = type;                         // reset the type name, no need to clone, if its wrong it needs correcting
            currentshipid = id;
        }

        public void UpdateModules(List<JournalLoadout.ShipModule> lst)
        {
            if (CurrentShipIDValid )
            {
                ships[currentshipid] = ships[currentshipid].Clone();    // okay, going to change it, need to clone, but only the ship we change
// da da da 
            }
        }

        private ShipModules EnsureShip( int id , bool deepclone )
        {
            if (ships.ContainsKey(id))
            {
                if (deepclone)
                    ships[id] = ships[id].Clone();          // perform a deep clone

                return ships[id];
            }
            else
            {
                ShipModules sm = new ShipModules();
                ships[id] = sm;
                return sm;
            }
        }

        private ShipListModules ShallowClone()                      // employ a shallow clone for this class.. keep pointing at same data.
        {
            return new ShipListModules(this.ships, this.currentshipid);
        }

        static public ShipListModules Process(JournalEntry je, ShipListModules oldslm, DB.SQLiteConnectionUser conn)
        {
            ShipListModules newslm = (oldslm == null) ? new ShipListModules() : oldslm;

            if (je is IModuleJournalEntry)
            {
                IModuleJournalEntry e = je as IModuleJournalEntry;
                newslm = newslm.ShallowClone();                     // we get a new shallow copy..
                e.Module(newslm, conn);                             // not cloned.. up to callers to see if they need to
            }

            return newslm;
        }

    }

}
