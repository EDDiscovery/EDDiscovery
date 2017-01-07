using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    class StringExpander
    {
        static bool ExpandString( string line, Dictionary<string,string> vars)
        {
            string res = "";

            int pos = 0;
            while( pos < line.Length )
            {
                pos = line.IndexOf('$', pos);

                if (pos >= 0)
                {
                }
            }

            return true;
        }
    }
}
