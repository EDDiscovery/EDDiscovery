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
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    /*
     When written: when the player's ship has been repaired by a repair drone
    Parameters:
     HullRepaired
     CockpitRepaired
     CorrosionRepaired
    Each of these is a number indicating the amount of damage that has been repaired 
     */
    [JournalEntryType(JournalTypeEnum.RepairDrone)]
    public class JournalRepairDrone : JournalEntry
    {
        public JournalRepairDrone(JObject evt ) : base(evt, JournalTypeEnum.RepairDrone)
        {
            HullRepaired = evt["HullRepaired"].Str();
            CockpitRepaired = evt["CockpitRepaired"].Str();
            CorrosionRepaired = evt["CorrosionRepaired"].Str();

        }

        public string HullRepaired { get; set; }
        public string CockpitRepaired { get; set; }
        public string CorrosionRepaired { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EliteDangerous.Properties.Resources.repair; } }  // TODO?



        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Hull:", HullRepaired, "Cockpit:", CockpitRepaired, "Corrosion:", CorrosionRepaired);
            detailed = "";
        }
    }
}
