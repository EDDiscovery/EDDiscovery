using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public class EDDIconSet : EliteDangerousCore.EliteIconSet
    {
        private EDDIconSet() { }

        static EDDIconSet()
        {
            Instance = new EDDIconSet();
            EliteDangerousCore.EliteConfigInstance.InstanceIconSet = Instance;
        }

        public static EDDIconSet Instance { get; private set; }

        public Dictionary<EliteDangerousCore.EDStar, System.Drawing.Image> StarTypeIcons { get; set; }

        public Dictionary<EliteDangerousCore.EDPlanet, System.Drawing.Image> PlanetTypeIcons { get; set; }

        public Dictionary<EliteDangerousCore.JournalTypeEnum, System.Drawing.Image> JournalTypeIcons { get; set; }
    }
}
