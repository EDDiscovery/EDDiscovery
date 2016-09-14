﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    public class JournalFileHeader : JournalEntry
    {
        public JournalFileHeader(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.FileHeader, reader)
        {

            GameVersion = evt.Value<string>("gameversion");
            Build = evt.Value<string>("build");
            Language = evt.Value<string>("language");
        }

        public string GameVersion { get; set; }
        public string Build { get; set; }
        public string Language { get; set; }
    }
}
