using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    public class EDDIconSet : EliteDangerousCore.IconSet
    {
        private EDDIconSet() { }

        static EDDIconSet()
        {
            Instance = new EDDIconSet();
            EliteDangerousCore.EliteConfigInstance.InstanceIconSet = Instance;
        }

        public static EDDIconSet Instance { get; private set; }
    }
}
