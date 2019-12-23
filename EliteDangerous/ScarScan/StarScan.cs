/*
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

using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EliteDangerousCore
{
    public partial class StarScan
    {
        private Dictionary<Tuple<string, long>, SystemNode> scandata = new Dictionary<Tuple<string, long>, SystemNode>();
        private Dictionary<string, List<SystemNode>> scandataByName = new Dictionary<string, List<SystemNode>>();

        public class SystemNode
        {
            public ISystem system;
            public SortedList<string, ScanNode> starnodes;
            public bool EDSMCacheCheck = false;
            public bool EDSMWebChecked = false;
            public SortedList<int, ScanNode> NodesByID = new SortedList<int, ScanNode>();
            public int MaxTopLevelBodyID = 0;
            public int MinPlanetBodyID = 512;
            public int? FSSTotalBodies;

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

            public string StarTypesFound(bool bracketit = true) // first is primary star
            {
                var sortedset = (from x in Bodies where x.ScanData != null && x.type == ScanNodeType.star orderby x.ScanData.DistanceFromArrivalLS select x.ScanData.StarTypeID.ToString()).ToList();
                string s = string.Join("; ", sortedset);
                if (bracketit && s.HasChars())
                    s = "(" + s + ")";
                return s;
            }

            public int StarPlanetsScanned()      // not include anything but these.  This corresponds to FSSDiscoveryScan
            {
                return Bodies.Where(b => ( b.type == ScanNodeType.star || b.type == ScanNodeType.body) && b.ScanData != null).Count();
            }
            public int StarsScanned()      // only stars
            {
                return Bodies.Where(b => b.type == ScanNodeType.star && b.ScanData != null).Count();
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

            private JournalScan scandata;            // can be null if no scan, its a place holder, else its a journal scan
            private JournalScan.StarPlanetRing beltdata;    // can be null if not belt. if its a type==belt, it is populated with belt data
            private IBodyNameAndID bodyloc;         // can be null if not allocated via BodyID system, else its the journal entry (of any type) with body and id data
            private List<JournalSAASignalsFound.SAASignal> signals; // can be null if no signals for this node, else its a list of signals.

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

            public List<JournalSAASignalsFound.SAASignal> Signals
            {
                get
                {
                    return signals;
                }
                set
                {
                    if (value == null)
                        return;

                    signals = value;
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
            System.Diagnostics.Debug.Assert(sys != null);

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
    }
}
