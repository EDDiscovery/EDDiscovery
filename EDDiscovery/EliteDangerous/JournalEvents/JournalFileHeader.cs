using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    public class JournalFileHeader : JournalEntry
    {
        public JournalFileHeader(JObject evt ) : base(evt, JournalTypeEnum.FileHeader)
        {

            GameVersion = Tools.GetStringDef(evt["gameversion"]);
            Build = Tools.GetStringDef(evt["build"]);
            Language = Tools.GetStringDef(evt["language"]);
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

                if (GameVersion.Equals("2.2") && Build.Contains("r121645/r0"))
                    return true;

                return false;
            }
        }
    }
}
