using EDDiscovery.Icons;
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
{
    public class PlanetIcons : Icons<EDPlanet>
    {
        public PlanetIcons()
        {
            Init("Planets", Enum.GetValues(typeof(EDPlanet)).OfType<EDPlanet>());
        }
    }

    public class StarIcons : Icons<EDStar>
    {
        public StarIcons()
        {
            Init("Stars", Enum.GetValues(typeof(EDStar)).OfType<EDStar>());
        }
    }

    public class JournalIcons : Icons<JournalTypeEnum>
    {
        public JournalIcons()
        {
            Init("Journal", Enum.GetValues(typeof(JournalTypeEnum)).OfType<JournalTypeEnum>());
        }
    }

    public class GalMapIcons : Icons<GalMapTypeEnum>
    {
        public GalMapIcons()
        {
            Init("GalMap", Enum.GetValues(typeof(GalMapTypeEnum)).OfType<GalMapTypeEnum>());
        }
    }

    public class IconSet : EliteIconSet
    {
        public IReadOnlyDictionary<EDStar, Image> StarTypeIcons { get; } = new StarIcons();

        public IReadOnlyDictionary<EDPlanet, Image> PlanetTypeIcons { get; } = new PlanetIcons();

        public IReadOnlyDictionary<JournalTypeEnum, Image> JournalTypeIcons { get; } = new JournalIcons();

        public IReadOnlyDictionary<GalMapTypeEnum, Image> GalMapTypeIcons { get; } = new GalMapIcons();
    }
}
