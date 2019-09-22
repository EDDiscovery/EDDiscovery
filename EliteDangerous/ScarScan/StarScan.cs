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
        Dictionary<Tuple<string, long>, SystemNode> scandata = new Dictionary<Tuple<string, long>, SystemNode>();
        Dictionary<string, List<SystemNode>> scandataByName = new Dictionary<string, List<SystemNode>>();
        private static Dictionary<string, Dictionary<string, string>> planetDesignationMap = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
        private static Dictionary<string, Dictionary<string, string>> starDesignationMap = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
        private static Dictionary<string, Dictionary<int, BodyDesignations.DesigMap>> bodyIdDesignationMap = new Dictionary<string, Dictionary<int, BodyDesignations.DesigMap>>(StringComparer.InvariantCultureIgnoreCase);
        private static Dictionary<string, List<JournalScan>> primaryStarScans = new Dictionary<string, List<JournalScan>>(StringComparer.InvariantCultureIgnoreCase);

        public class SystemNode
        {
            public ISystem system;
            public SortedList<string, ScanNode> starnodes;
            public bool EDSMCacheCheck = false;
            public bool EDSMWebChecked = false;
            public SortedList<int, ScanNode> NodesByID = new SortedList<int, ScanNode>();
            public int MaxTopLevelBodyID = 0;
            public int MinPlanetBodyID = 512;
            public int? TotalBodies;

            public IEnumerable<ScanNode> Bodies
            {
                get
                {
                    if (starnodes != null)
                    {
                        foreach (ScanNode sn in starnodes.Values)
                        {
                            yield return sn;

                            foreach (ScanNode c in sn.Descendants)
                            {
                                yield return c;
                            }
                        }
                    }
                }
            }

            public long ScanValue( bool includeedsmvalue )
            {
                long value = 0;

                foreach (var body in Bodies)
                {
                    if (body?.ScanData != null)
                    {
                        if (includeedsmvalue || !body.ScanData.IsEDSMBody)
                        {
                            value += body.ScanData.EstimatedValue;
                        }
                    }
                }

                return value;
            }
        };

        public enum ScanNodeType { star, barycentre, body, belt, beltcluster, ring };

        public class ScanNode
        {
            public ScanNodeType type;
            public string fullname;                 // full name
            public string ownname;                  // own name              
            public string customname;               // e.g. Earth
            public SortedList<string, ScanNode> children;         // kids
            public int level;                       // level within SystemNode
            public int? BodyID;
            public bool IsMapped;                   // recorded here since the scan data can be replaced by a better version later.
            public bool WasMappedEfficiently;

            public bool IsTopLevelNode;

            private JournalScan scandata;            // can be null if no scan, its a place holder.
            private JournalScan.StarPlanetRing beltdata;
            private IBodyNameAndID bodyloc;

            public JournalScan ScanData
            {
                get
                {
                    return scandata;
                }

                set
                {
                    if (value == null)
                        return;

                    if (scandata == null)
                        scandata = value;
                    else if ((!value.IsEDSMBody && value.ScanType != "Basic") || scandata.ScanType == "Basic") // Always overwrtite if its a journalscan (except basic scans)
                    {
                        //System.Diagnostics.Debug.WriteLine(".. overwrite " + scandata.ScanType + " with " + value.ScanType + " for " + scandata.BodyName);
                        scandata = value;
                    }
                }
            }

            public JournalScan.StarPlanetRing BeltData
            {
                get
                {
                    return beltdata;
                }

                set
                {
                    if (value == null)
                        return;

                    beltdata = value;
                }
            }

            public IBodyNameAndID BodyLoc
            {
                get
                {
                    return bodyloc;
                }
                set
                {
                    if (value == null)
                        return;

                    bodyloc = value;
                }
            }

            public bool DoesNodeHaveNonEDSMScansBelow()
            {
                if (ScanData != null && ScanData.IsEDSMBody == false)
                    return true;

                if (children != null)
                {
                    foreach (KeyValuePair<string, ScanNode> csn in children)
                    {
                        if (csn.Value.DoesNodeHaveNonEDSMScansBelow())
                            return true;
                    }
                }

                return false;
            }

            public bool IsBodyInFilter(string[] filternames, bool checkchildren)
            {
                if (IsBodyInFilter(filternames))
                    return true;

                if (checkchildren)
                {
                    foreach (var body in Descendants)
                    {
                        if (body.IsBodyInFilter(filternames))
                            return true;
                    }
                }
                return false;
            }

            public bool IsBodyInFilter(string[] filternames)    // stars/bodies use the xID type, others use the type
            {
                if (filternames.Contains("All"))
                    return true;
                string name = type.ToString();      // star etc..
                if (scandata != null)
                {
                    if (type == ScanNodeType.star)
                        name = scandata.StarTypeID.ToString();
                    else if (type == ScanNodeType.body)
                        name = scandata.PlanetTypeID.ToString();
                }

                return filternames.Contains(name, StringComparer.InvariantCultureIgnoreCase);
            }

            public IEnumerable<ScanNode> Descendants
            {
                get
                {
                    if (children != null)
                    {
                        foreach (ScanNode sn in children.Values)
                        {
                            yield return sn;

                            foreach (ScanNode c in sn.Descendants)
                            {
                                yield return c;
                            }
                        }
                    }
                }
            }
        };

        public bool HasWebLookupOccurred(ISystem sys)       // have we had a web checkup on this system?  false if sys does not exist
        {
            SystemNode sn = FindSystemNode(sys);
            return (sn != null && sn.EDSMWebChecked);
        }

        public SystemNode FindSystem(ISystem sys, bool edsmweblookup, bool byname = false)    // Find the system. Optionally do a EDSM web lookup
        {
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);  // foreground only

            SystemNode sn = FindSystemNode(sys);

            // System.Diagnostics.Debug.WriteLine("Scan Lookup " + sys.Name + " found " + (sn != null) + " web? " + edsmweblookup + " edsm lookup " + (sn?.EDSMAdded ?? false));

            if ((sys.EDSMID > 0 || (sys.SystemAddress != null && sys.SystemAddress > 0) || (byname && sys.Name.HasChars())) && (sn == null || sn.EDSMCacheCheck == false || (edsmweblookup && !sn.EDSMWebChecked)))
            {
                List<JournalScan> jl = EliteDangerousCore.EDSM.EDSMClass.GetBodiesList(sys.EDSMID, edsmweblookup: edsmweblookup, id64: sys.SystemAddress, sysname:sys.Name); // lookup, with optional web

                //if ( edsmweblookup) System.Diagnostics.Debug.WriteLine("Lookup bodies " + sys.Name + " " + sys.EDSMID + " result " + (jl?.Count ?? -1));

                if (jl != null) // found some bodies.. either from cache or from EDSM..
                {
                    foreach (JournalScan js in jl)
                    {
                        js.BodyDesignation = GetBodyDesignation(js, sys.Name);
                        ProcessJournalScan(js, sys, true);
                    }
                }

                if (sn == null) // refind to make sure SN is set
                    sn = FindSystemNode(sys);

                if (sn != null) // if we found it, set to indicate we did a cache check
                {
                    sn.EDSMCacheCheck = true;

                    if (edsmweblookup)      // and if we did a web check, set it too..
                        sn.EDSMWebChecked = true;
                }
            }

            return sn;
        }

        #region FSS DISCOVERY *************************************************************

        public void SetFSSDiscoveryScan(JournalFSSDiscoveryScan je, ISystem sys)
        {
            SystemNode sn = GetOrCreateSystemNode(sys);
            sn.TotalBodies = je.BodyCount;
        }

        #endregion
        
        #region Helpers

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

        #endregion

    }
}
