using EDDiscovery.Icons;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDDiscovery.Forms;

namespace EDDiscovery
{
    public class EDDIconSet : EliteDangerousCore.IconSet
    {
        private static EDDIconSet _instance;

        private EDDIconSet() { }

        public static EDDIconSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EDDIconSet();
                    EliteDangerousCore.EliteConfigInstance.InstanceIconSet = _instance;
                }

                return _instance;
            }
            set
            {
                _instance = value;
                EliteDangerousCore.EliteConfigInstance.InstanceIconSet = value;
            }
        }

        public static void Init()
        {
            EliteDangerousCore.EliteConfigInstance.InstanceIconSet = Instance;
        }

        public IReadOnlyDictionary<PanelInformation.PanelIDs, Image> PanelTypeIcons { get; } = new PanelIcons();
    }

    public class PanelIcons : IconSet<PanelInformation.PanelIDs>
    {
        public PanelIcons()
        {
            Init("Panels", Enum.GetValues(typeof(PanelInformation.PanelIDs)).OfType<PanelInformation.PanelIDs>());
        }
    }
}
