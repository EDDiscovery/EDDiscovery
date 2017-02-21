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

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: Player has escaped interdiction
    //Parameters: 
    //•	Interdictor: interdicting pilot name
    //•	IsPlayer: whether player or npc
    [JournalEntryType(JournalTypeEnum.EscapeInterdiction)]
    public class JournalEscapeInterdiction : JournalEntry
    {
        public JournalEscapeInterdiction(JObject evt ) : base(evt, JournalTypeEnum.EscapeInterdiction)
        {
            Interdictor = JSONHelper.GetStringDef(evt["Interdictor"]);
            IsPlayer = JSONHelper.GetBool(evt["IsPlayer"]);
        }
        public string Interdictor { get; set; }
        public bool IsPlayer { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.escapeinterdiction; } }
    }
}
