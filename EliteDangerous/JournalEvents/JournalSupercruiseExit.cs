﻿/*
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
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    //When written: leaving supercruise for normal space
    //Parameters:
    //•	Starsystem
    //•	Body
    [JournalEntryType(JournalTypeEnum.SupercruiseExit)]
    public class JournalSupercruiseExit : JournalEntry
    {
        public JournalSupercruiseExit(JObject evt ) : base(evt, JournalTypeEnum.SupercruiseExit)
        {
            StarSystem = evt["StarSystem"].Str();
            Body = evt["Body"].Str();
            BodyType = evt["BodyType"].Str();
            if (BodyType.Equals("Null", System.StringComparison.InvariantCultureIgnoreCase)) // obv a frontier bug
                BodyType = "";
        }
        public string StarSystem { get; set; }
        public string Body { get; set; }
        public string BodyType { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EliteDangerous.Properties.Resources.supercruiseexit; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("At ", Body , "< in ", StarSystem, "Type:" , BodyType);
            detailed = "";
        }
    }
}
