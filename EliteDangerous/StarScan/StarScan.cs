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
using System.Diagnostics;
using System.Linq;

namespace EliteDangerousCore
{
    public partial class StarScan
    {
        private Dictionary<Tuple<string, long>, SystemNode> scandata = new Dictionary<Tuple<string, long>, SystemNode>();
        private Dictionary<string, List<SystemNode>> scandataByName = new Dictionary<string, List<SystemNode>>();
        private const string MainStar = "Main Star";

        [DebuggerDisplay("SN {system.Name}")]
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

        [DebuggerDisplay("SN {fullname} {type} {level}")]
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


            // given a list of scannodes, construst a tree of barynodes with their scans underneath.
            // reconstructs the node tree and inserts barynodes into it from the parent info

            static public ScanNode PopulateBarycentres(List<ScanNode> nodes)      
            {
                ScanNode top = new ScanNode();
                top.children = new SortedList<string, ScanNode>();

                foreach (var sn in nodes)
                {
                    if (sn.ScanData?.Parents != null)
                    {
                        //System.Diagnostics.Debug.WriteLine("Scan " + sn.ScanData.BodyName + ":" + sn.ScanData.BodyID + " " + sn.ScanData.ParentList());

                        for (int i = 0; i < sn.ScanData.Parents.Count; i++) // go thru all parents of the body
                        {
                            var sd = sn.ScanData;
                            var sp = sd.Parents[i];

                            if (sp.Type == "Null")      // any barycenters, process
                            {
                                ScanNode bodynode = null;

                                if (i > 0)              // so its not the last barycentre (remembering its in reverse order- last entry is the deepest (say Star) node
                                {
                                    int bodyid = sd.Parents[i - 1].BodyID;                  // pick up the body ID of the previous entry
                                    bodynode = nodes.Find((x) => x.BodyID == bodyid);       // see if its in the scan database

                                    if (bodynode == null && sd.Parents[i - 1].Type == "Null")   // if can't find, and its another barycentre, add a new dummy barycentre node
                                    {
                                        bodynode = new ScanNode() { BodyID = bodyid, type = ScanNodeType.barycentre, fullname = "Created Barynode " + bodyid, ownname = bodyid.ToString("00000") };
                                    }
                                }
                                else
                                {
                                    bodynode = sn;      // its directly under the body, so the node is the scan node (Node-barycentre)
                                }

                                if (bodynode != null)  
                                {
                                    string barykey = sp.BodyID.ToStringInvariant("00000"); // sp is a barycentre, so get its body id
                                    ScanNode cur = null;

                                    if (top.children.ContainsKey(barykey))      // if top has this barycentre..
                                        cur = top.children[barykey];
                                    else
                                    { 
                                        cur = new ScanNode() { BodyID = sp.BodyID, type = ScanNodeType.barycentre, fullname = "Created Barynode " + sp.BodyID };    // make it
                                        cur.children = new SortedList<string, ScanNode>();
                                        top.children[barykey] = cur;
                                    }

                                    //System.Diagnostics.Debug.WriteLine("Scan add " + entry + " to " + barykey);

                                    if (!cur.children.ContainsKey(bodynode.ownname))
                                    {
                                        cur.children[bodynode.ownname] = bodynode;
                                    }
                                }
                            }
                        }
                    }
                }

                List<string> keystodelete = new List<string>();

                foreach (var n in top.children)
                {
                    //System.Diagnostics.Debug.WriteLine("Top Node  " + n.Value.BodyID);
                    keystodelete.AddRange(ExpandRecurivelyBarynodeTree(top, n.Value));      // move bary-node on the top to their positions in the tree
                }

                foreach (var k in keystodelete)     // remove any moved keys
                    top.children.Remove(k);

                return top;
            }

            static private List<string> ExpandRecurivelyBarynodeTree( ScanNode top , ScanNode pos )     // go down tree, moving nodes from the top to their positions
            {
                List<string> keystodelete = new List<string>();

                foreach (var k in pos.children)     // all children of top
                {
                    string keyid = k.Value.BodyID.ToStringInvariant("00000");   // key from bodyid

                    if (k.Value.type == ScanNodeType.barycentre && top.children.ContainsKey(keyid)) // its a barycentre, and top has that barycentre, move it to here
                    {
                        //System.Diagnostics.Debug.WriteLine(".. barycenter  " + keyid);
                        ScanNode tocopy = top.children[keyid];

                        if (k.Value.children == null)
                            k.Value.children = new SortedList<string, ScanNode>();

                        foreach (var cc in tocopy.children)
                        {
                            string cckey = cc.Key;
                            if (!k.Value.children.ContainsKey(cckey))                               // may have been moved already, because we don't remove top keys until finished
                            {
                               // System.Diagnostics.Debug.WriteLine(".. " + cckey + " " + cc.Value.fullname + " onto " + keyid);
                                k.Value.children.Add(cckey, cc.Value);
                                ExpandRecurivelyBarynodeTree(top, k.Value);
                            }
                            else
                            {
                               // System.Diagnostics.Debug.WriteLine(".. Dup move " + cckey + " " + cc.Value.fullname + " onto " + keyid);
                            }
                        }

                        keystodelete.Add(keyid);
                    }
                }

                return keystodelete;
            }

            public static void DumpTree(ScanNode top, string key , int level)        // debug dump out
            {
                System.Diagnostics.Debug.WriteLine("                                                        ".Substring(0,level*3) + key + ":" + top.BodyID + " "+ top.fullname +  " " + top.type);
                if ( top.children != null )
                {
                    foreach (var c in top.children)
                        DumpTree(c.Value, c.Key, level + 1);
                }
           }

        };

        public bool HasWebLookupOccurred(ISystem sys)       // have we had a web checkup on this system?  false if sys does not exist
        {
            SystemNode sn = FindSystemNode(sys);
            return (sn != null && sn.EDSMWebChecked);
        }

        // ONLY use this if you must because the async await won't work in the call stack.  edsmweblookup here with true is strongly discouraged

        public SystemNode FindSystemSynchronous(ISystem sys, bool edsmweblookup)    // Find the system. Optionally do a EDSM web lookup
        {
            System.Diagnostics.Debug.Assert(sys != null);

            SystemNode sn = FindSystemNode(sys);

            // System.Diagnostics.Debug.WriteLine("Scan Lookup " + sys.Name + " found " + (sn != null) + " web? " + edsmweblookup + " edsm lookup " + (sn?.EDSMAdded ?? false));

            if ((sys.EDSMID > 0 || (sys.SystemAddress != null && sys.SystemAddress > 0) || (sys.Name.HasChars())) && (sn == null || sn.EDSMCacheCheck == false || (edsmweblookup && !sn.EDSMWebChecked)))
            {
                var jl = EliteDangerousCore.EDSM.EDSMClass.GetBodiesList(sys, edsmweblookup); // lookup, with optional web

                //if (edsmweblookup) System.Diagnostics.Debug.WriteLine("EDSM WEB Lookup bodies " + sys.Name + " " + sys.EDSMID + " result " + (jl?.Count ?? -1));

                if (jl != null && jl.Item2 == false) // found some bodies, not from the cache
                {
                    foreach (JournalScan js in jl.Item1)
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

        // you must be returning void to use this..

        public async System.Threading.Tasks.Task<SystemNode> FindSystemAsync(ISystem sys, bool edsmweblookup )    // Find the system. Optionally do a EDSM web lookup
        {
            System.Diagnostics.Debug.Assert(sys != null);

            SystemNode sn = FindSystemNode(sys);

            string trace = Environment.StackTrace.StackTrace("FindSystemAsync", 4);

            //System.Diagnostics.Debug.WriteLine("Scan Lookup " + trace + " " + sys.Name + " found " + (sn != null) + " web? " + edsmweblookup + " edsm lookup " + (sn?.EDSMWebChecked ?? false));

            if ((sys.EDSMID > 0 || (sys.SystemAddress != null && sys.SystemAddress > 0) || (sys.Name.HasChars())) && (sn == null || sn.EDSMCacheCheck == false || (edsmweblookup && !sn.EDSMWebChecked)))
            {
                System.Diagnostics.Debug.WriteLine("..Asking for EDSM data");

                var jl = await EliteDangerousCore.EDSM.EDSMClass.GetBodiesListAsync(sys, edsmweblookup); // lookup, with optional web

                // return bodies and a flag indicating if from cache.
                // Scenario: Three panels are asking for data, one at a time, since its the foreground thread
                // each one awaits, sets and runs a task, blocks until tasks completes, foreground continues to next panel where it does the same
                // we have three tasks, any which could run in any order. 
                // The tasks all go thru GetBodiesListAsync, which locks.  Only 1 task gets to do the lookup, the one which got there first, because it did not see
                // a cached version
                // once that task completes the lookups, and it unlocks, the other tasks can run, and they will see the cache setup.  They won't do an EDSM web access
                // since the body is in the cache.  
                // for now, i can't guarantee that the task which gives back the bodies first runs on the foreground task.  It may be task2 gets the bodies.
                // so we will just add them in again

                if (jl != null && jl.Item1 != null)
                {
                    // removed - can't guarantee if (jl.Item2 == false)      // only want them if not previously cached
                    {
                        System.Diagnostics.Debug.WriteLine("Process bodies from EDSM " + trace + " " + sys.Name + " " + sys.EDSMID + " result " + (jl.Item1?.Count ?? -1));
                        foreach (JournalScan js in jl.Item1)
                        {
                            js.BodyDesignation = GetBodyDesignation(js, sys.Name);
                            ProcessJournalScan(js, sys, true);
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine("Lookup System node again");
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
