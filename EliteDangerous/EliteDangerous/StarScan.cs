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

    public class StarScan
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
            public bool IsMapped;
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
                        scandata = value;
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
                        Process(js, sys, true);
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

        // used by historylist during full history processing in background
        public bool AddScanToBestSystem(JournalScan je, int startindex, List<HistoryEntry> hl)
        {
            HistoryEntry he;
            JournalLocOrJump jl;
            return AddScanToBestSystem(je, startindex, hl, out he, out jl);
        }

        // used by historylist directly for a single update during play, in foreground..  Also used by above.. so can be either in fore/back
        public bool AddScanToBestSystem(JournalScan je, int startindex, List<HistoryEntry> hl, out HistoryEntry he, out JournalLocOrJump jl)
        {
            for (int j = startindex; j >= 0; j--)
            {
                he = hl[j];

                if (he.IsLocOrJump)
                {
                    jl = (JournalLocOrJump)he.journalEntry;
                    string designation = GetBodyDesignation(je, he.System.Name);

                    if (je.IsStarNameRelated(he.System.Name, designation))       // if its part of the name, use it
                    {
                        je.BodyDesignation = designation;
                        return Process(je, he.System, true);
                    }
                    else if (jl != null && je.IsStarNameRelated(jl.StarSystem, designation))
                    {
                        // Ignore scans where the system name has changed
                        return false;
                    }
                }
            }

            jl = null;
            he = null;

            je.BodyDesignation = GetBodyDesignation(je, hl[startindex].System.Name);
            return Process(je, hl[startindex].System, true);         // no relationship, add..
        }

        // used by historylist during full history processing in background
        public bool AddBodyToBestSystem(IBodyNameAndID je, int startindex, List<HistoryEntry> hl)
        {
            HistoryEntry he;
            JournalLocOrJump jl;
            return AddBodyToBestSystem(je, startindex, hl, out he, out jl);
        }

        // used by historylist directly for a single update during play, in foreground..  Also used by above.. so can be either in fore/back
        public bool AddBodyToBestSystem(IBodyNameAndID je, int startindex, List<HistoryEntry> hl, out HistoryEntry he, out JournalLocOrJump jl)
        {
            if (je.Body == null || je.BodyType == "Station" || je.BodyType == "StellarRing" || je.BodyType == "PlanetaryRing" || je.BodyType == "SmallBody")
            {
                he = null;
                jl = null;
                return false;
            }

            for (int j = startindex; j >= 0; j--)
            {
                he = hl[j];

                if (he.IsLocOrJump && he.System.Name == je.StarSystem && (he.System.SystemAddress == null || je.SystemAddress == null || he.System.SystemAddress == je.SystemAddress))
                {
                    jl = (JournalLocOrJump)he.journalEntry;
                    string designation = GetBodyDesignation(je);

                    if (IsStarNameRelated(he.System.Name, je.Body, designation))       // if its part of the name, use it
                    {
                        je.BodyDesignation = designation;

                        return Process(je, he.System, true);
                    }
                    else if (jl != null && IsStarNameRelated(jl.StarSystem, je.Body, designation))
                    {
                        // Ignore scans where the system name has changed
                        return false;
                    }
                }
            }

            jl = null;
            he = null;

            je.BodyDesignation = GetBodyDesignation(je);
            return Process(je, hl[startindex].System, true);         // no relationship, add..
        }

        // used by historylist during full history processing in background
        public bool AddScanToBestSystem(JournalSAAScanComplete je, int startindex, List<HistoryEntry> hl)
        {
            HistoryEntry he;
            JournalLocOrJump jl;
            return AddScanToBestSystem(je, startindex, hl, out he, out jl);
        }

        // used by historylist directly for a single update during play, in foreground..  Also used by above.. so can be either in fore/back
        public bool AddScanToBestSystem(JournalSAAScanComplete je, int startindex, List<HistoryEntry> hl, out HistoryEntry he, out JournalLocOrJump jl)
        {
            for (int j = startindex; j >= 0; j--)
            {
                he = hl[j];

                if (he.IsLocOrJump)
                {
                    jl = (JournalLocOrJump)he.journalEntry;
                    string designation = GetBodyDesignation(je, he.System.Name);

                    if (IsStarNameRelated(he.System.Name, designation))       // if its part of the name, use it
                    {
                        je.BodyDesignation = designation;
                        return Process(je, he.System, true);
                    }
                    else if (jl != null && IsStarNameRelated(jl.StarSystem, designation))
                    {
                        // Ignore scans where the system name has changed
                        return false;
                    }
                }
            }

            jl = null;
            he = null;

            je.BodyDesignation = GetBodyDesignation(je, hl[startindex].System.Name);
            return Process(je, hl[startindex].System, true);         // no relationship, add..
        }

        public void SetFSSDiscoveryScan(JournalFSSDiscoveryScan je, ISystem sys)
        {
            SystemNode sn = GetOrCreateSystemNode(sys);
            sn.TotalBodies = je.BodyCount;
        }

        private bool Process(JournalScan sc, ISystem sys, bool reprocessPrimary = false)  // background or foreground.. FALSE if you can't process it
        {
            SystemNode sn = GetOrCreateSystemNode(sys);

            // handle Earth, starname = Sol
            // handle Eol Prou LW-L c8-306 A 4 a and Eol Prou LW-L c8-306
            // handle Colonia 4 , starname = Colonia, planet 4
            // handle Aurioum B A BELT
            // Kyloasly OY-Q d5-906 13 1

            ScanNodeType starscannodetype = ScanNodeType.star;          // presuming.. 
            bool isbeltcluster = false;
            bool isring = false;

            // Extract elements from name
            List<string> elements = ExtractElements(sc, sys, out isbeltcluster, out starscannodetype, out isring);

            // Bail out if no elements extracted
            if (elements.Count == 0)
            {
                System.Diagnostics.Trace.WriteLine($"Failed to add body {sc.BodyName} to system {sys.Name} - not enough elements");
                return false;
            }
            // Bail out if more than 5 elements extracted
            else if (elements.Count > 5)
            {
                System.Diagnostics.Trace.WriteLine($"Failed to add body {sc.BodyName} to system {sys.Name} - too deep");
                return false;
            }

            // Get custom name if different to designation
            string customname = GetCustomName(sc, sys);

            // Process elements
            ScanNode node = ProcessElements(sc, sys, sn, customname, elements, starscannodetype, isbeltcluster, isring);

            if (node.BodyID != null)
            {
                sn.NodesByID[(int)node.BodyID] = node;
            }

            // Process top-level star
            if (elements.Count == 1)
            {
                // Process any belts if present
                ProcessBelts(sc, node);

                // Process primary star in multi-star system
                if (elements[0].Equals("A", StringComparison.InvariantCultureIgnoreCase))
                {
                    CachePrimaryStar(sc, sys);

                    // Reprocess if we've encountered the primary (A) star and we already have a "Main Star"
                    if (reprocessPrimary && sn.starnodes.Any(n => n.Key.Length > 1 && n.Value.type == ScanNodeType.star))
                    {
                        ReProcess(sn);
                    }
                }
            }

            return true;
        }

        private bool Process(IBodyNameAndID sc, ISystem sys, bool reprocessPrimary = false)  // background or foreground.. FALSE if you can't process it
        {
            SystemNode sn = GetOrCreateSystemNode(sys);
            ScanNode relatedScan = null;

            if ((sc.BodyDesignation == null || sc.BodyDesignation == sc.Body) && (sc.Body != sc.StarSystem || sc.BodyType != "Star"))
            {
                foreach (var body in sn.Bodies)
                {
                    if ((body.fullname == sc.Body || body.customname == sc.Body) &&
                        (body.fullname != sc.StarSystem || (sc.BodyType == "Star" && body.level == 0) || (sc.BodyType != "Star" && body.level != 0)))
                    {
                        relatedScan = body;
                        sc.BodyDesignation = body.fullname;
                        break;
                    }
                }
            }

            if (relatedScan == null)
            {
                foreach (var body in sn.Bodies)
                {
                    if ((body.fullname == sc.Body || body.customname == sc.Body) &&
                        (body.fullname != sc.StarSystem || (sc.BodyType == "Star" && body.level == 0) || (sc.BodyType != "Star" && body.level != 0)))
                    {
                        relatedScan = body;
                        break;
                    }
                }
            }

            if (relatedScan != null && relatedScan.ScanData == null)
            {
                relatedScan.BodyLoc = sc;
                return true; // We already have the scan
            }

            // handle Earth, starname = Sol
            // handle Eol Prou LW-L c8-306 A 4 a and Eol Prou LW-L c8-306
            // handle Colonia 4 , starname = Colonia, planet 4
            // handle Aurioum B A BELT
            // Kyloasly OY-Q d5-906 13 1

            ScanNodeType starscannodetype = ScanNodeType.star;          // presuming.. 
            bool isbeltcluster = false;

            // Extract elements from name
            List<string> elements = ExtractElements(sc, sys, out isbeltcluster, out starscannodetype);

            // Bail out if no elements extracted
            if (elements.Count == 0)
            {
                System.Diagnostics.Trace.WriteLine($"Failed to add body {sc.Body} to system {sys.Name} - not enough elements");
                return false;
            }
            // Bail out if more than 5 elements extracted
            else if (elements.Count > 5)
            {
                System.Diagnostics.Trace.WriteLine($"Failed to add body {sc.Body} to system {sys.Name} - too deep");
                return false;
            }

            // Get custom name if different to designation
            string customname = GetCustomName(sc, sys);

            // Process elements
            ScanNode node = ProcessElements(sc, sys, sn, customname, elements, starscannodetype, isbeltcluster);

            if (node.BodyID != null)
            {
                sn.NodesByID[(int)node.BodyID] = node;
            }

            return true;
        }

        private bool Process(JournalSAAScanComplete sc, ISystem sys, bool reprocessPrimary = false)  // background or foreground.. FALSE if you can't process it
        {
            SystemNode sn = GetOrCreateSystemNode(sys);
            ScanNode relatedScan = null;

            if (sn.NodesByID.ContainsKey((int)sc.BodyID))
            {
                relatedScan = sn.NodesByID[(int)sc.BodyID];
                if (relatedScan.ScanData != null && relatedScan.ScanData.BodyDesignation != null)
                {
                    sc.BodyDesignation = relatedScan.ScanData.BodyDesignation;
                }
            }
            else if (sc.BodyDesignation != null && sc.BodyDesignation != sc.BodyName)
            {
                foreach (var body in sn.Bodies)
                {
                    if (body.fullname == sc.BodyDesignation)
                    {
                        relatedScan = body;
                        break;
                    }
                }
            }

            if (relatedScan == null)
            {
                foreach (var body in sn.Bodies)
                {
                    if ((body.fullname == sc.BodyName || body.customname == sc.BodyName) &&
                        (body.fullname != sys.Name || body.level != 0))
                    {
                        relatedScan = body;
                        break;
                    }
                }
            }

            if (relatedScan != null)
            {
                relatedScan.IsMapped = true;
                relatedScan.WasMappedEfficiently = sc.ProbesUsed <= sc.EfficiencyTarget;
                return true; // We already have the scan
            }

            return false;
        }

        private void ReProcess(SystemNode sysnode)
        {
            List<JournalScan> bodies = sysnode.Bodies.Where(b => b.ScanData != null).Select(b => b.ScanData).ToList();
            sysnode.starnodes = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>());
            sysnode.NodesByID = new SortedList<int, ScanNode>();

            foreach (JournalScan sn in bodies)
            {
                sn.BodyDesignation = GetBodyDesignation(sn, sysnode.system.Name);
                Process(sn, sysnode.system);
            }
        }

        #region Scan Processing

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

        private List<string> ExtractElements(JournalScan sc, ISystem sys, out bool isbeltcluster, out ScanNodeType starscannodetype, out bool isring)
        {
            starscannodetype = ScanNodeType.star;
            isbeltcluster = false;
            isring = false;
            List<string> elements;
            string rest = sc.IsStarNameRelatedReturnRest(sys.Name);

            if (rest != null)                                   // if we have a relationship..
            {
                if (sc.IsStar && !sc.IsEDSMBody && sc.DistanceFromArrivalLS == 0 && rest.Length >= 2)
                {
                    elements = new List<string> { rest };
                    starscannodetype = ScanNodeType.star;
                }
                else if (rest.Length > 0)
                {
                    elements = rest.Split(' ').ToList();

                    if (elements.Count == 4 && elements[0].Length == 1 && char.IsLetter(elements[0][0]) &&
                            elements[1].Equals("belt", StringComparison.InvariantCultureIgnoreCase) &&
                            elements[2].Equals("cluster", StringComparison.InvariantCultureIgnoreCase))
                    {
                        elements = new List<string> { "Main Star", elements[0] + " " + elements[1], elements[2] + " " + elements[3] };
                        isbeltcluster = true;
                    }
                    else if (elements.Count == 5 && elements[0].Length >= 1 &&
                            elements[1].Length == 1 && char.IsLetter(elements[1][0]) &&
                            elements[2].Equals("belt", StringComparison.InvariantCultureIgnoreCase) &&
                            elements[3].Equals("cluster", StringComparison.InvariantCultureIgnoreCase))
                    {
                        elements = new List<string> { elements[0], elements[1] + " " + elements[2], elements[3] + " " + elements[4] };
                        isbeltcluster = true;
                    }
                    else if (elements.Count >= 3 &&
                             elements[elements.Count - 1].Equals("ring", StringComparison.InvariantCultureIgnoreCase) &&
                             elements[elements.Count - 2].Length == 1 &&
                             char.IsLetter(elements[elements.Count - 2][0]))
                    {
                        elements = elements.Take(elements.Count - 2).Concat(new string[] { elements[elements.Count - 2] + " " + elements[elements.Count - 1] }).ToList();
                        isring = true;
                    }

                    if (char.IsDigit(elements[0][0]))       // if digits, planet number, no star designator
                        elements.Insert(0, "Main Star");         // no star designator, main star, add MAIN
                    else if (elements[0].Length > 1)        // designator, is it multiple chars.. 
                        starscannodetype = ScanNodeType.barycentre;
                }
                else
                {
                    elements = new List<string>();          // only 1 item, the star, which is the same as the system name..
                    elements.Add("Main Star");              // Sol / SN:Sol should come thru here
                }
            }
            else if (sc.IsStar && !sc.IsEDSMBody && sc.DistanceFromArrivalLS == 0)
            {
                elements = new List<string> { sc.BodyName };
                starscannodetype = ScanNodeType.star;
            }
            else
            {                                               // so not part of starname        
                elements = sc.BodyName.Split(' ').ToList();     // not related in any way (earth) so assume all bodyparts, and 
                elements.Insert(0, "Main Star");                     // insert the MAIN designator as the star designator
            }

            return elements;
        }

        private List<string> ExtractElements(IBodyNameAndID sc, ISystem sys, out bool isbeltcluster, out ScanNodeType starscannodetype)
        {
            starscannodetype = ScanNodeType.star;
            isbeltcluster = false;
            List<string> elements;
            string rest = IsStarNameRelatedReturnRest(sys.Name, sc.Body, sc.BodyDesignation);

            if (rest != null)                                   // if we have a relationship..
            {
                if (rest.Length > 0)
                {
                    elements = rest.Split(' ').ToList();

                    if (elements.Count == 4 && elements[0].Length == 1 && char.IsLetter(elements[0][0]) &&
                            elements[1].Equals("belt", StringComparison.InvariantCultureIgnoreCase) &&
                            elements[2].Equals("cluster", StringComparison.InvariantCultureIgnoreCase))
                    {
                        elements = new List<string> { "Main Star", elements[0] + " " + elements[1], elements[2] + " " + elements[3] };
                        isbeltcluster = true;
                    }
                    else if (elements.Count == 5 && elements[0].Length >= 1 &&
                            elements[1].Length == 1 && char.IsLetter(elements[1][0]) &&
                            elements[2].Equals("belt", StringComparison.InvariantCultureIgnoreCase) &&
                            elements[3].Equals("cluster", StringComparison.InvariantCultureIgnoreCase))
                    {
                        elements = new List<string> { elements[0], elements[1] + " " + elements[2], elements[3] + " " + elements[4] };
                        isbeltcluster = true;
                    }

                    if (char.IsDigit(elements[0][0]))       // if digits, planet number, no star designator
                        elements.Insert(0, "Main Star");         // no star designator, main star, add MAIN
                    else if (elements[0].Length > 1)        // designator, is it multiple chars.. 
                        starscannodetype = ScanNodeType.barycentre;
                }
                else
                {
                    elements = new List<string>();          // only 1 item, the star, which is the same as the system name..
                    elements.Add("Main Star");              // Sol / SN:Sol should come thru here
                }
            }
            else
            {                                               // so not part of starname        
                elements = sc.Body.Split(' ').ToList();     // not related in any way (earth) so assume all bodyparts, and 
                elements.Insert(0, "Main Star");                     // insert the MAIN designator as the star designator
            }

            return elements;
        }

        private string GetCustomName(JournalScan sc, ISystem sys)
        {
            string rest = sc.IsStarNameRelatedReturnRest(sys.Name);
            string customname = null;

            if (sc.BodyName.StartsWith(sys.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                customname = sc.BodyName.Substring(sys.Name.Length).TrimStart(' ', '-');

                if (customname == "" && !sc.IsStar)
                {
                    customname = sc.BodyName;
                }
                else if (customname == "" || customname == rest)
                {
                    customname = null;
                }
            }
            else if (rest == null || !sc.BodyName.EndsWith(rest))
            {
                customname = sc.BodyName;
            }

            return customname;
        }

        private string GetCustomName(IBodyNameAndID sc, ISystem sys)
        {
            string rest = IsStarNameRelatedReturnRest(sys.Name, sc.Body, sc.BodyDesignation);
            string customname = null;

            if (sc.Body.StartsWith(sys.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                customname = sc.Body.Substring(sys.Name.Length).TrimStart(' ', '-');

                if (customname == "" && sc.BodyType != "Star")
                {
                    customname = sc.Body;
                }
                else if (customname == "" || customname == rest)
                {
                    customname = null;
                }
            }
            else if (rest == null || !sc.Body.EndsWith(rest))
            {
                customname = sc.Body;
            }

            return customname;
        }

        private ScanNode ProcessElements(JournalScan sc, ISystem sys, SystemNode sn, string customname, List<string> elements, ScanNodeType starscannodetype, bool isbeltcluster, bool isring = false)
        {
            SortedList<string, ScanNode> cnodes = sn.starnodes;
            ScanNode node = null;
            List<JournalScan.BodyParent> ancestors = sc.Parents?.AsEnumerable()?.ToList();
            List<JournalScan.BodyParent> ancestorbodies = ancestors?.Where(a => a.Type == "Star" || a.Type == "Planet" || a.Type == "Belt")?.Reverse()?.ToList();
            ScanNode toplevelnode = null;

            if ((ancestorbodies != null) && (starscannodetype != ScanNodeType.star))
            {
                ancestorbodies.Insert(0, null);
            }

            for (int lvl = 0; lvl < elements.Count; lvl++)
            {
                ScanNode sublv;
                ScanNodeType sublvtype;
                string ownname = elements[lvl];

                if (lvl == 0)
                    sublvtype = starscannodetype;
                else if (isbeltcluster)
                    sublvtype = lvl == 1 ? ScanNodeType.belt : ScanNodeType.beltcluster;
                else if (isring && lvl == elements.Count - 1)
                    sublvtype = ScanNodeType.ring;
                else
                    sublvtype = ScanNodeType.body;

                if (cnodes == null || !cnodes.TryGetValue(elements[lvl], out sublv))
                {
                    if (node != null && node.children == null)
                    {
                        node.children = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>());
                        cnodes = node.children;
                    }

                    sublv = new ScanNode
                    {
                        ownname = ownname,
                        fullname = lvl == 0 ? (sys.Name + (ownname.Contains("Main") ? "" : (" " + ownname))) : node.fullname + " " + ownname,
                        ScanData = null,
                        children = null,
                        type = sublvtype,
                        level = lvl,
                        IsTopLevelNode = lvl == 0
                    };

                    cnodes.Add(ownname, sublv);
                }

                if (ancestorbodies != null && lvl < ancestorbodies.Count && ancestorbodies[lvl] != null)
                {
                    if (sublv.BodyID == null)
                    {
                        sublv.BodyID = ancestorbodies[lvl].BodyID;
                        sn.NodesByID[(int)sublv.BodyID] = sublv;
                    }
                }

                node = sublv;
                cnodes = node.children;

                if (lvl == elements.Count - 1)
                {
                    node.ScanData = sc;
                    node.customname = customname;

                    if (sc.BodyID != null)
                    {
                        node.BodyID = sc.BodyID;
                    }
                }

                if (lvl == 0)
                {
                    toplevelnode = node;
                }

                if (node.BodyID != null)
                {
                    if (lvl == 0 && node.BodyID > sn.MaxTopLevelBodyID)
                    {
                        sn.MaxTopLevelBodyID = (int)node.BodyID;
                    }
                    else if (lvl > 0 && node.BodyID < sn.MinPlanetBodyID)
                    {
                        sn.MinPlanetBodyID = (int)node.BodyID;
                    }
                }
            }

            if (ancestors != null && ancestorbodies != null && ancestorbodies.Count > 0 && ancestorbodies[0] == null && toplevelnode.BodyID == null)
            {
                for (int lvl = 1; lvl < ancestors.Count; lvl++)
                {
                    if (ancestors[lvl - 1].BodyID >= sn.MinPlanetBodyID && ancestors[lvl].BodyID <= sn.MaxTopLevelBodyID)
                    {
                        toplevelnode.BodyID = ancestors[lvl].BodyID;
                        sn.NodesByID[(int)toplevelnode.BodyID] = toplevelnode;
                    }
                }
            }

            return node;
        }

        private ScanNode ProcessElements(IBodyNameAndID sc, ISystem sys, SystemNode sn, string customname, List<string> elements, ScanNodeType starscannodetype, bool isbeltcluster)
        {
            SortedList<string, ScanNode> cnodes = sn.starnodes;
            ScanNode node = null;

            for (int lvl = 0; lvl < elements.Count; lvl++)
            {
                ScanNode sublv;
                ScanNodeType sublvtype;
                string ownname = elements[lvl];

                if (lvl == 0)
                    sublvtype = starscannodetype;
                else if (isbeltcluster)
                    sublvtype = lvl == 1 ? ScanNodeType.belt : ScanNodeType.beltcluster;
                else
                    sublvtype = ScanNodeType.body;

                if (cnodes == null || !cnodes.TryGetValue(elements[lvl], out sublv))
                {
                    if (node != null && node.children == null)
                    {
                        node.children = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>());
                        cnodes = node.children;
                    }

                    sublv = new ScanNode
                    {
                        ownname = ownname,
                        fullname = lvl == 0 ? (sys.Name + (ownname.Contains("Main") ? "" : (" " + ownname))) : node.fullname + " " + ownname,
                        ScanData = null,
                        children = null,
                        type = sublvtype,
                        level = lvl,
                        IsTopLevelNode = lvl == 0
                    };

                    cnodes.Add(ownname, sublv);
                }

                node = sublv;
                cnodes = node.children;

                if (lvl == elements.Count - 1)
                {
                    node.BodyLoc = sc;
                    node.customname = customname;

                    if (sc.BodyID != null)
                    {
                        node.BodyID = sc.BodyID;
                    }

                    if (sc.BodyType == "" || sc.BodyType == "Null")
                        node.type = ScanNodeType.barycentre;
                    else if (sc.BodyType == "Belt")
                        node.type = ScanNodeType.belt;
                }
            }

            return node;
        }

        private void ProcessBelts(JournalScan sc, ScanNode node)
        {
            if (sc.HasRings)
            {
                foreach (JournalScan.StarPlanetRing ring in sc.Rings)
                {
                    ScanNode belt;
                    string beltname = ring.Name;
                    string stardesig = sc.BodyDesignation ?? sc.BodyName;

                    if (beltname.StartsWith(stardesig, StringComparison.InvariantCultureIgnoreCase))
                    {
                        beltname = beltname.Substring(stardesig.Length).Trim();
                    }
                    else if (stardesig.ToLowerInvariant() == "lave" && beltname.ToLowerInvariant() == "castellan belt")
                    {
                        beltname = "A Belt";
                    }

                    if (node.children == null || !node.children.TryGetValue(beltname, out belt))
                    {
                        if (node.children == null)
                            node.children = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>());

                        belt = new ScanNode
                        {
                            ownname = beltname,
                            fullname = node.fullname + " " + beltname,
                            customname = ring.Name,
                            ScanData = null,
                            BeltData = ring,
                            children = null,
                            type = ScanNodeType.belt,
                            level = 1
                        };

                        node.children.Add(beltname, belt);
                    }

                    belt.BeltData = ring;
                }
            }
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

        #endregion

        #region Helpers

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

            if (je.IsStar && je.BodyName == system && je.nOrbitalPeriod != null)
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

        private string GetBodyDesignation(IBodyNameAndID je)
        {
            if (je.Body == null || je.BodyType == null || je.StarSystem == null)
                return null;

            string system = je.StarSystem;
            string bodyname = je.Body;
            bool isstar = je.BodyType == "Star";
            int bodyid = je.BodyID ?? -1;

            if (je.BodyID != null && bodyIdDesignationMap.ContainsKey(system) && bodyIdDesignationMap[system].ContainsKey(bodyid) && bodyIdDesignationMap[system][bodyid].NameEquals(bodyname))
            {
                return bodyIdDesignationMap[system][bodyid].Designation;
            }

            Dictionary<string, Dictionary<string, string>> desigmap = isstar ? starDesignationMap : planetDesignationMap;

            // Special case for m Centauri
            if (isstar && system.ToLowerInvariant() == "m centauri")
            {
                if (bodyname == "m Centauri")
                {
                    return "m Centauri A";
                }
                else if (bodyname == "M Centauri")
                {
                    return "m Centauri B";
                }
            }
            // Special case for Castellan Belt
            else if (system.ToLowerInvariant() == "lave" && bodyname.StartsWith("Castellan Belt ", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Lave A Belt " + bodyname.Substring("Castellan Belt ".Length);
            }

            if (desigmap.ContainsKey(system) && desigmap[system].ContainsKey(bodyname))
            {
                return desigmap[system][bodyname];
            }

            if (bodyname.Equals(system, StringComparison.InvariantCultureIgnoreCase) || bodyname.StartsWith(system + " ", StringComparison.InvariantCultureIgnoreCase))
            {
                return bodyname;
            }

            return bodyname;
        }

        private string GetBodyDesignation(JournalSAAScanComplete je, string system)
        {
            if (je.BodyName == null || system == null)
                return null;

            string bodyname = je.BodyName;
            int bodyid = (int)je.BodyID;

            if (bodyIdDesignationMap.ContainsKey(system) && bodyIdDesignationMap[system].ContainsKey(bodyid) && bodyIdDesignationMap[system][bodyid].NameEquals(bodyname))
            {
                return bodyIdDesignationMap[system][bodyid].Designation;
            }

            if (planetDesignationMap.ContainsKey(system) && planetDesignationMap[system].ContainsKey(bodyname))
            {
                return planetDesignationMap[system][bodyname];
            }

            if (bodyname.Equals(system, StringComparison.InvariantCultureIgnoreCase) || bodyname.StartsWith(system + " ", StringComparison.InvariantCultureIgnoreCase))
            {
                return bodyname;
            }

            return bodyname;
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
