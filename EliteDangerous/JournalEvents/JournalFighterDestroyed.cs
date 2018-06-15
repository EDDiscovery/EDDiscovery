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
using EliteDangerousCore.DB;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{

    [JournalEntryType(JournalTypeEnum.FighterDestroyed)]
    public class JournalFighterDestroyed : JournalEntry, IShipInformation
    {
        public JournalFighterDestroyed(JObject evt ) : base(evt, JournalTypeEnum.FighterDestroyed)
        {
        }

        public override void FillInformation(out string info, out string detailed) //V
        {
            
            info = "";
            detailed = "";
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, SQLiteConnectionUser conn)
        {
            shp.DockFighter();
        }
    }
}
