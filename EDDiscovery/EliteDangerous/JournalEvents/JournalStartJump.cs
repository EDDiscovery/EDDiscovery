﻿/*
 * Copyright © 2017 EDDiscovery development team
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

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//    When written: at the start of a Hyperspace or Supercruise jump(start of countdown)
//Parameters:
//•	JumpType: "Hyperspace" or "Supercruise"
//•	StarClass: star type(only for a hyperspace jump)

    [JournalEntryType(JournalTypeEnum.StartJump)]
    public class JournalStartJump : JournalEntry
    {
        public JournalStartJump(JObject evt) : base(evt, JournalTypeEnum.StartJump)
        {
            JumpType = JSONHelper.GetStringDef(evt["JumpType"]);
            StarSystem = JSONHelper.GetStringDef(evt["StarSystem"]);
            StarClass = JSONHelper.GetStringDef(evt["StarClass"]);
        }

        public string JumpType { get; set; }            // Hyperspace, Supercruise
        public string StarSystem { get; set; }
        public string StarClass { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.genericevent; } }

    }
}
