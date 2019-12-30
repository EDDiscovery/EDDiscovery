/*`
 * Copyright © 2015 - 2019 EDDiscovery development team
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
using System.IO;
using System.Linq;

namespace EliteDangerousCore
{

    public partial class StarScan
    {

        // fudge tables
        private static Dictionary<string, Dictionary<string, string>> planetDesignationMap = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
        private static Dictionary<string, Dictionary<string, string>> starDesignationMap = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
        private static Dictionary<string, Dictionary<int, BodyDesignations.DesigMap>> bodyIdDesignationMap = new Dictionary<string, Dictionary<int, BodyDesignations.DesigMap>>(StringComparer.InvariantCultureIgnoreCase);

        public static void LoadBodyDesignationMap()
        {
            string desigmappath = Path.Combine(EliteConfigInstance.InstanceOptions.AppDataDirectory, "bodydesignations.csv");

            if (!File.Exists(desigmappath))
            {
                desigmappath = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "bodydesignations.csv");
            }

            if (File.Exists(desigmappath))
            {
                foreach (string line in File.ReadLines(desigmappath))
                {
                    string[] fields = line.Split(',').Select(s => s.Trim('"')).ToArray();
                    if (fields.Length == 3)
                    {
                        string sysname = fields[0];
                        string bodyname = fields[1];
                        string desig = fields[2];
                        Dictionary<string, Dictionary<string, string>> desigmap = planetDesignationMap;

                        if (desig == sysname || (desig.Length == sysname.Length + 2 && desig[sysname.Length + 1] >= 'A' && desig[sysname.Length + 1] <= 'F'))
                        {
                            desigmap = starDesignationMap;
                        }

                        if (!desigmap.ContainsKey(sysname))
                        {
                            desigmap[sysname] = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                        }

                        desigmap[sysname][bodyname] = desig;
                    }
                }
            }
            else
            {
                foreach (var skvp in BodyDesignations.Stars)
                {
                    if (!starDesignationMap.ContainsKey(skvp.Key))
                    {
                        starDesignationMap[skvp.Key] = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                    }

                    foreach (var bkvp in skvp.Value)
                    {
                        starDesignationMap[skvp.Key][bkvp.Key] = bkvp.Value;
                    }
                }

                foreach (var skvp in BodyDesignations.Planets)
                {
                    if (!planetDesignationMap.ContainsKey(skvp.Key))
                    {
                        planetDesignationMap[skvp.Key] = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                    }

                    foreach (var bkvp in skvp.Value)
                    {
                        planetDesignationMap[skvp.Key][bkvp.Key] = bkvp.Value;
                    }
                }

                foreach (var skvp in BodyDesignations.ByBodyId)
                {
                    if (!bodyIdDesignationMap.ContainsKey(skvp.Key))
                    {
                        bodyIdDesignationMap[skvp.Key] = new Dictionary<int, BodyDesignations.DesigMap>();
                    }

                    foreach (var bkvp in skvp.Value)
                    {
                        bodyIdDesignationMap[skvp.Key][bkvp.Key] = bkvp.Value;
                    }
                }
            }
        }
        
    }
}
