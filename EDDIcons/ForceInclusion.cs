using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Icons
{
    public static class ForceInclusion
    {
        public static void Include()         // if no direct references to the assembly is present, it gets optimised away.
        {
            System.Diagnostics.Debug.WriteLine("Included EDDicons");
        }
    }
}
