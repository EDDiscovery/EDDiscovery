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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    public class JournalFileheader : JournalEntry
    {
        public JournalFileheader(JObject evt ) : base(evt, JournalTypeEnum.Fileheader)
        {

            GameVersion = JSONHelper.GetStringDef(evt["gameversion"]);
            Build = JSONHelper.GetStringDef(evt["build"]);
            Language = JSONHelper.GetStringDef(evt["language"]);
        }

        public string GameVersion { get; set; }
        public string Build { get; set; }
        public string Language { get; set; }

        public bool Beta
        {
            get
            {
                if (GameVersion.Contains("Beta"))
                    return true;

                if (GameVersion.Equals("2.2") && (Build.Contains("r121645/r0") || Build.Contains("r129516/r0")))
                    return true;

                return false;
            }
        }
    }
}
