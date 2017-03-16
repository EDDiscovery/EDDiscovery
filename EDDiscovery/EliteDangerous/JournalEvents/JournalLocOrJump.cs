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
using EDDiscovery.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    public abstract class JournalLocOrJump : JournalEntry
    {
        public string StarSystem { get; set; }
        public Vector3 StarPos { get; set; }
        public bool StarPosFromEDSM { get; set; }
        public bool EDSMFirstDiscover { get; set; }

        public bool HasCoordinate { get { return !float.IsNaN(StarPos.X); } }

        protected JournalLocOrJump(JObject jo, JournalTypeEnum jtype ) : base(jo, jtype)
        {
            StarSystem = jo["StarSystem"].Str();
            StarPosFromEDSM = jo["StarPosFromEDSM"].Bool(false);
            EDSMFirstDiscover = jo["EDD_EDSMFirstDiscover"].Bool(false);

            Vector3 pos = new Vector3();

            if (!jo["StarPos"].Empty())            // if its an old VS entry, may not have co-ords
            {
                JArray coords = jo["StarPos"] as JArray;
                pos.X = coords[0].Value<float>();
                pos.Y = coords[1].Value<float>();
                pos.Z = coords[2].Value<float>();
            }
            else
            {
                pos.X = pos.Y = pos.Z = float.NaN;
            }

            StarPos = pos;
        }

        public void UpdateEDSMFirstDiscover(bool firstdiscover)
        {
            jEventData["EDD_EDSMFirstDiscover"] = firstdiscover;
            Update();
            EDSMFirstDiscover = firstdiscover;
        }
    }
}
