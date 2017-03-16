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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//The "Passengers" event contains:

//"Manifest": array of passenger records, each containing:
//o MissionID (int)
//o Type (string)
//o VIP (bool)
//o Wanted (bool)
//o Count (int)
//from hchalkley
    [JournalEntryType(JournalTypeEnum.Passengers)]
    public class JournalPassengers : JournalEntry, IPassengersJournalEntry
    {
        public class Passengers
        {
            public int MissionID { get; set; }
            public string Type { get; set; }
            public bool VIP { get; set; }
            public bool Wanted { get; set; }
            public int Count { get; set; }

            public Passengers()
            { }
            public Passengers(int i, string t, bool v, bool w, int c)
            {
                MissionID = i; Type = t; VIP = v; Wanted = w; Count = c;
            }
            public Passengers Clone()
            {
                return new Passengers(MissionID, Type, VIP, Wanted, Count);
            }
        }

        public JournalPassengers(JObject evt) : base(evt, JournalTypeEnum.Passengers)
        {
            Manifest = evt["Manifest"]?.ToObject<Passengers[]>();

            if (Manifest != null )
            {
                foreach (Passengers p in Manifest)
                    p.Type = p.Type.SplitCapsWordFull();
            }
        }

        public Passengers[] Manifest { get; set; }

        public void UpdatePassengers(PassengersList shp, DB.SQLiteConnectionUser conn)
        {
            shp.SetPassengers(Manifest);
        }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.passengers; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //U
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "No Passengers";

            if (Manifest != null && Manifest.Length > 0)
            {
                info = "";
                foreach (Passengers p in Manifest)
                {
                    if (info.Length > 0)
                        info += ", ";
                    info += Tools.FieldBuilder("", p.Type , "> ", p.Count , "; (VIP)" , p.VIP , "; (Wanted)" , p.Wanted);
                }
            }

            detailed = "";
        }
    }
}
