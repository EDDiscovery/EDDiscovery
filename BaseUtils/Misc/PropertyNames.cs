/*
 * Copyright © 2017 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
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
