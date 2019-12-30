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

using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EliteDangerousCore
{
    public partial class StarScan
    {
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
            if (je?.BodyName == null)
            {
                he = null;
                jl = null;
                return false;
            }

            for (int j = startindex; j >= 0; j--)
            {
                he = hl[j];

                if (he.IsLocOrJump)
                {
                    jl = (JournalLocOrJump)he.journalEntry;
                    string designation = GetBodyDesignation(je, he.System.Name);

                    if (je.IsStarNameRelated(he.System.Name, designation, he.System.SystemAddress))       // if its part of the name, use it
                    {
                        je.BodyDesignation = designation;
                        return ProcessJournalScan(je, he.System, true);
                    }
                    else if (jl != null && je.IsStarNameRelated(jl.StarSystem, designation, jl.SystemAddress))
                    {
                        // Ignore scans where the system name has changed
                        System.Diagnostics.Trace.WriteLine($"Rejecting body {designation} ({je.BodyName}) in system {he.System.Name} => {jl.StarSystem} due to system rename");
                        return false;
                    }
                }
            }

            jl = null;
            he = null;

            je.BodyDesignation = GetBodyDesignation(je, hl[startindex].System.Name);
            return ProcessJournalScan(je, hl[startindex].System, true);         // no relationship, add..
        }

        // take the journal scan and add it to the node tree

        private bool ProcessJournalScan(JournalScan sc, ISystem sys, bool reprocessPrimary = false)  // background or foreground.. FALSE if you can't process it
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
            List<string> elements = ExtractElementsJournalScan(sc, sys, out isbeltcluster, out starscannodetype, out isring);

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
            string customname = GetCustomNameJournalScan(sc, sys);

            // Process elements
            ScanNode node = ProcessElementsJournalScan(sc, sys, sn, customname, elements, starscannodetype, isbeltcluster, isring);

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

        private void ReProcess(SystemNode sysnode)
        {
            List<JournalScan> bodies = sysnode.Bodies.Where(b => b.ScanData != null).Select(b => b.ScanData).ToList();
            sysnode.starnodes = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>());
            sysnode.NodesByID = new SortedList<int, ScanNode>();

            foreach (JournalScan sn in bodies)
            {
                sn.BodyDesignation = GetBodyDesignation(sn, sysnode.system.Name);
                ProcessJournalScan(sn, sysnode.system);
            }
        }

        private List<string> ExtractElementsJournalScan(JournalScan sc, ISystem sys, out bool isbeltcluster, out ScanNodeType starscannodetype, out bool isring)
        {
            starscannodetype = ScanNodeType.star;
            isbeltcluster = false;
            isring = false;
            List<string> elements;
            string rest = sc.IsStarNameRelatedReturnRest(sys.Name, sys.SystemAddress);

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
                    else if (elements[0].Length > 1 && elements[0] != "Main Star")        // designator, is it multiple chars.. 
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

        private string GetCustomNameJournalScan(JournalScan sc, ISystem sys)
        {
            string rest = sc.IsStarNameRelatedReturnRest(sys.Name, sys.SystemAddress);
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

        private ScanNode ProcessElementsJournalScan(JournalScan sc, ISystem sys, SystemNode sn, string customname, List<string> elements, ScanNodeType starscannodetype, bool isbeltcluster, bool isring = false)
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
                {
                    if (lvl == 1)
                        sublvtype = ScanNodeType.belt;          // A Belt
                    else
                        sublvtype = ScanNodeType.beltcluster;   // A Belt Cluster 1
                }
                else if (isring && lvl == elements.Count - 1)       // detailed scans of rings.. placed under planets
                {
                    sublvtype = ScanNodeType.ring;
                }
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
                    node.ScanData = sc;     // only overwrites if scan is better
                    node.ScanData.SetMapped(node.IsMapped, node.WasMappedEfficiently);      // pass this data to node, as we may have previously had a SAA Scan
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

        // asteroid belts, not rings, are assigned to sub nodes of the star in the node heirarchy as type==belt.

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

    }
}
