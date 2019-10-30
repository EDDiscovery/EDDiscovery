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
        public bool AddBodyToBestSystem(IBodyNameAndID je, int startindex, List<HistoryEntry> hl)
        {
            HistoryEntry he;
            JournalLocOrJump jl;

            if (je.Body == null || je.BodyType == "Station" || je.BodyType == "StellarRing" || je.BodyType == "PlanetaryRing" || je.BodyType == "SmallBody")
            {
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

                        return ProcessBodyAndID(je, he.System, true);
                    }
                    else if (jl != null && IsStarNameRelated(jl.StarSystem, je.Body, designation))
                    {
                        // Ignore scans where the system name has changed
                        return false;
                    }
                }
            }

            je.BodyDesignation = GetBodyDesignation(je);
            return ProcessBodyAndID(je, hl[startindex].System, true);         // no relationship, add..
        }

        private bool ProcessBodyAndID(IBodyNameAndID sc, ISystem sys, bool reprocessPrimary = false)  // background or foreground.. FALSE if you can't process it
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
            List<string> elements = ExtractElementsBodyAndID(sc, sys, out isbeltcluster, out starscannodetype);

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
            string customname = GetCustomNameBodyAndID(sc, sys);

            // Process elements
            ScanNode node = ProcessElementsBodyAndID(sc, sys, sn, customname, elements, starscannodetype, isbeltcluster);

            if (node.BodyID != null)
            {
                sn.NodesByID[(int)node.BodyID] = node;
            }

            return true;
        }

        private List<string> ExtractElementsBodyAndID(IBodyNameAndID sc, ISystem sys, out bool isbeltcluster, out ScanNodeType starscannodetype)
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

        private string GetCustomNameBodyAndID(IBodyNameAndID sc, ISystem sys)
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

        private ScanNode ProcessElementsBodyAndID(IBodyNameAndID sc, ISystem sys, SystemNode sn, string customname, List<string> elements, ScanNodeType starscannodetype, bool isbeltcluster)
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
    }
}
