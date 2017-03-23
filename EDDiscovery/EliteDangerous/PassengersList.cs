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
    public class PassengersList
    {
        List<JournalPassengers.Passengers> list;        // a list of passengers groups

        PassengersList()
        {
            list = new List<JournalPassengers.Passengers>();
        }

        PassengersList(List<JournalPassengers.Passengers> other)
        {
            list = new List<JournalPassengers.Passengers>(other);
        }

        public void SetPassengers(JournalPassengers.Passengers[] p)     // replace whole list, so no clone required.
        {
            list = p.ToList();
        }

        public void LoadPassengers(JournalPassengers.Passengers p)        // given p.MissionID, add a clo
        {
            // find it, add/remove from list, its already shallow copied so we can remove it from our list okay
        }

        public void UnloadPassengers(JournalPassengers.Passengers p)        // given p.MissionID, add a clo
        {
            // find it, add/remove from list, its already shallow copied
        }

        private PassengersList ShallowClone()                      // employ a shallow clone for this class.. keep pointing at same data.
        {
            PassengersList pl = new PassengersList(this.list);
            return pl;
        }

        static public PassengersList Process(JournalEntry je, PassengersList oldslm, DB.SQLiteConnectionUser conn)
        {
            PassengersList newslm = (oldslm == null) ? new PassengersList() : oldslm;

            if (je is IPassengersJournalEntry)
            {
                IPassengersJournalEntry e = je as IPassengersJournalEntry;
                newslm = newslm.ShallowClone();
                e.UpdatePassengers(newslm, conn);
            }

            return newslm;
        }


    }
}
