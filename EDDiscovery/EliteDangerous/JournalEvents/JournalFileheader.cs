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
