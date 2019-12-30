/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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

using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
{

    public partial class StarScan
    {
        private static Dictionary<string, List<JournalScan>> primaryStarScans = new Dictionary<string, List<JournalScan>>(StringComparer.InvariantCultureIgnoreCase);

        // make or get a system node for a system  

        private SystemNode GetOrCreateSystemNode(ISystem sys)
        {
            Tuple<string, long> withedsm = new Tuple<string, long>(sys.Name, sys.EDSMID);

            SystemNode sn = null;
            if (scandata.ContainsKey(withedsm))         // if with edsm (if id_edsm=0, then thats okay)
                sn = scandata[withedsm];
            else if (scandataByName.ContainsKey(sys.Name))  // if we now have an edsm id, see if we have one without it 
            {
                foreach (SystemNode _sn in scandataByName[sys.Name])
                {
                    if (_sn.system.Equals(sys))
                    {
                        if (sys.EDSMID != 0)             // yep, replace
                        {
                            scandata.Add(new Tuple<string, long>(sys.Name, sys.EDSMID), _sn);
                        }
                        sn = _sn;
                        break;
                    }
                }
            }

            if (sn == null)
            {
                sn = new SystemNode() { system = sys, starnodes = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>()) };

                if (!scandataByName.ContainsKey(sys.Name))
                {
                    scandataByName[sys.Name] = new List<SystemNode>();
                }

                scandataByName[sys.Name].Add(sn);

                if (sys.EDSMID != 0)
                {
                    scandata.Add(new Tuple<string, long>(sys.Name, sys.EDSMID), sn);
                }
            }

            return sn;
        }

        private static bool CompareEpsilon(double? a, double? b, bool acceptNull = false, double epsilon = 0.001, Func<double?, double> fb = null)
        {
            if (a == null || b == null)
            {
                return !acceptNull;
            }

            double _a = (double)a;
            double _b = fb == null ? (double)b : fb(b);

            return _a == _b || (_a + _b != 0 && Math.Sign(_a + _b) == Math.Sign(_a) && Math.Abs((_a - _b) / (_a + _b)) < epsilon);
        }

        private void CachePrimaryStar(JournalScan je, ISystem sys)
        {
            string system = sys.Name;

            if (!primaryStarScans.ContainsKey(system))
            {
                primaryStarScans[system] = new List<JournalScan>();
            }

            if (!primaryStarScans[system].Any(s => CompareEpsilon(s.nAge, je.nAge) &&
                                                   CompareEpsilon(s.nEccentricity, je.nEccentricity) &&
                                                   CompareEpsilon(s.nOrbitalInclination, je.nOrbitalInclination) &&
                                                   CompareEpsilon(s.nOrbitalPeriod, je.nOrbitalPeriod) &&
                                                   CompareEpsilon(s.nPeriapsis, je.nPeriapsis) &&
                                                   CompareEpsilon(s.nRadius, je.nRadius) &&
                                                   CompareEpsilon(s.nRotationPeriod, je.nRotationPeriod) &&
                                                   CompareEpsilon(s.nSemiMajorAxis, je.nSemiMajorAxis) &&
                                                   CompareEpsilon(s.nStellarMass, je.nStellarMass)))
            {
                primaryStarScans[system].Add(je);
            }
        }

        private static bool IsStarNameRelated(string starname, string bodyname, string designation = null)
        {
            if (designation == null)
            {
                designation = bodyname;
            }

            if (designation.Length >= starname.Length)
            {
                string s = designation.Substring(0, starname.Length);
                return starname.Equals(s, StringComparison.InvariantCultureIgnoreCase);
            }
            else
                return false;
        }

        public static string IsStarNameRelatedReturnRest(string starname, string bodyname, string designation = null)          // null if not related, else rest of string
        {
            if (designation == null)
            {
                designation = bodyname;
            }

            if (designation.Length >= starname.Length)
            {
                string s = designation.Substring(0, starname.Length);
                if (starname.Equals(s, StringComparison.InvariantCultureIgnoreCase))
                    return designation.Substring(starname.Length).Trim();
            }

            return null;
        }

        private SystemNode FindSystemNode(ISystem sys)
        {
            Tuple<string, long> withedsm = new Tuple<string, long>(sys.Name, sys.EDSMID);

            if (scandata.ContainsKey(withedsm))         // if with edsm (if id_edsm=0, then thats okay)
                return scandata[withedsm];

            if (scandataByName.ContainsKey(sys.Name))
            {
                foreach (SystemNode sn in scandataByName[sys.Name])
                {
                    if (sn.system.Equals(sys))
                    {
                        return sn;
                    }
                }
            }

            return null;
        }

        // look to see if there is a better body designation for a scan.. using lookup tables (bodyDesignationMap) and direct code

        private string GetBodyDesignation(JournalScan je, string system)
        {
            Dictionary<string, Dictionary<string, string>> desigmap = je.IsStar ? starDesignationMap : planetDesignationMap;
            int bodyid = je.BodyID ?? -1;

            if (je.BodyID != null && bodyIdDesignationMap.ContainsKey(system) && bodyIdDesignationMap[system].ContainsKey(bodyid) && bodyIdDesignationMap[system][bodyid].NameEquals(je.BodyName))
            {
                return bodyIdDesignationMap[system][bodyid].Designation;
            }

            // Special case for m Centauri
            if (je.IsStar && system.ToLowerInvariant() == "m centauri")
            {
                if (je.BodyName == "m Centauri")
                {
                    return "m Centauri A";
                }
                else if (je.BodyName == "M Centauri")
                {
                    return "m Centauri B";
                }
            }

            // Special case for Castellan Belt
            if (system.ToLowerInvariant() == "lave" && je.BodyName.StartsWith("Castellan Belt ", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Lave A Belt " + je.BodyName.Substring("Castellan Belt ".Length);
            }

            // Special case for 9 Aurigae
            if (je.IsStar && system.ToLowerInvariant() == "9 Aurigae")
            {
                if (je.BodyName == "9 Aurigae C")
                {
                    if (je.nSemiMajorAxis > 1e13)
                    {
                        return "9 Aurigae D";
                    }
                    else
                    {
                        return "9 Aurigae C";
                    }
                }
            }

            if (desigmap.ContainsKey(system) && desigmap[system].ContainsKey(je.BodyName))
            {
                return desigmap[system][je.BodyName];
            }

            if (je.IsStar && je.BodyName.Equals(system, StringComparison.InvariantCultureIgnoreCase) && je.nOrbitalPeriod != null)
            {
                return system + " A";
            }

            if (je.BodyName.Equals(system, StringComparison.InvariantCultureIgnoreCase) || je.BodyName.StartsWith(system + " ", StringComparison.InvariantCultureIgnoreCase))
            {
                return je.BodyName;
            }

            if (je.IsStar && primaryStarScans.ContainsKey(system))
            {
                foreach (JournalScan primary in primaryStarScans[system])
                {
                    if (CompareEpsilon(je.nOrbitalPeriod, primary.nOrbitalPeriod) &&
                        CompareEpsilon(je.nPeriapsis, primary.nPeriapsis, acceptNull: true, fb: b => ((double)b + 180) % 360.0) &&
                        CompareEpsilon(je.nOrbitalInclination, primary.nOrbitalInclination) &&
                        CompareEpsilon(je.nEccentricity, primary.nEccentricity) &&
                        !je.BodyName.Equals(primary.BodyName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return system + " B";
                    }
                }
            }

            return je.BodyName;
        }

        private class DuplicateKeyComparer<TKey> : IComparer<string> where TKey : IComparable      // special compare for sortedlist
        {
            public int Compare(string x, string y)
            {
                if (x.Length > 0 && Char.IsDigit(x[0]))      // numbers..
                {
                    if (x.Length < y.Length)
                        return -1;
                    else if (x.Length > y.Length)
                        return 1;

                }

                return StringComparer.InvariantCultureIgnoreCase.Compare(x, y);
            }
        }

    }
}
