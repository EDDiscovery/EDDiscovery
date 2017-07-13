using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public static class FieldNames
    {
        static public List<string> GetPropertyFieldNames(Type jtype, string prefix = "")       // give a list of properties for a given name
        {
            if (jtype != null)
            {
                List<string> ret = new List<string>();

                foreach (System.Reflection.PropertyInfo pi in jtype.GetProperties())
                {
                    if (pi.GetIndexParameters().GetLength(0) == 0)      // only properties with zero parameters are called
                        ret.Add(prefix + pi.Name);
                }

                foreach (System.Reflection.FieldInfo fi in jtype.GetFields())
                {
                    string name = prefix + fi.Name;
                }
                return ret;
            }
            else
                return null;
        }

    }
}
