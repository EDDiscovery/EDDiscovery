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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using EDDiscovery.EliteDangerous.JournalEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
{

    public class StarScan
    {
        Dictionary<Tuple<string, long>, SystemNode> scandata = new Dictionary<Tuple<string, long>, SystemNode>();
        Dictionary<string, List<SystemNode>> scandataByName = new Dictionary<string, List<SystemNode>>();

        public class SystemNode
        {
            public EDDiscovery2.DB.ISystem system;
            public SortedList<string, ScanNode> starnodes;
            public bool EDSMAdded = false;
        };

        public enum ScanNodeType { star, barycentre, planet, moon, submoon, starbelt, rings };

        public class ScanNode
        {
            public ScanNodeType type;
            public string fullname;                 // full name
            public string ownname;                  // own name              
            public SortedList<string, ScanNode> children;         // kids

            private JournalScan scandata;            // can be null if no scan, its a place holder.

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
                    else if (!value.IsEDSMBody) // Always overwrtite if its a journalscan.
                        scandata = value;
                }
            }
        };

        public SystemNode FindSystem(EDDiscovery2.DB.ISystem sys)
        {
            Tuple<string, long> withedsm = new Tuple<string, long>(sys.name, sys.id_edsm);

            if (scandata.ContainsKey(withedsm))         // if with edsm (if id_edsm=0, then thats okay)
                return scandata[withedsm];

            if (scandataByName.ContainsKey(sys.name))
            {
                foreach (SystemNode sn in scandataByName[sys.name])
                {
                    if (sn.system.Equals(sys))
                    {
                        return sn;
                    }
                }
            }

            return null;
        }

        public bool AddScanToBestSystem(JournalScan je, int startindex, List<HistoryEntry> hl)
        {
            for (int j = startindex; j >= 0; j--)
            {
                if (je.IsStarNameRelated(hl[j].System.name))       // if its part of the name, use it
                {
                    return Process(je, hl[j].System);
                }
            }

            return Process(je, hl[startindex].System);         // no relationship, add..
        }

        public bool Process(JournalScan sc, EDDiscovery2.DB.ISystem sys)           // FALSE if you can't process it
        {
            Tuple<string, long> withedsm = new Tuple<string, long>(sys.name, sys.id_edsm);

            SystemNode sn = null;
            if (scandata.ContainsKey(withedsm))         // if with edsm (if id_edsm=0, then thats okay)
                sn = scandata[withedsm];
            else if (scandataByName.ContainsKey(sys.name))  // if we now have an edsm id, see if we have one without it 
            {
                foreach (SystemNode _sn in scandataByName[sys.name])
                {
                    if (_sn.system.Equals(sys))
                    {
                        if (sys.id_edsm != 0)             // yep, replace
                        {
                            scandata.Add(new Tuple<string, long>(sys.name, sys.id_edsm), _sn);
                            sn = _sn;
                        }
                        break;
                    }
                }
            }

            if (sn == null)
            {
                sn = new SystemNode() { system = sys, starnodes = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>()) };

                if (sys.id_edsm == 0)
                {
                    if (!scandataByName.ContainsKey(sys.name))
                    {
                        scandataByName[sys.name] = new List<SystemNode>();
                    }

                    scandataByName[sys.name].Add(sn);
                }
                else
                {
                    scandata.Add(new Tuple<string, long>(sys.name, sys.id_edsm), sn);
                }
            }

            // handle Earth, starname = Sol
            // handle Eol Prou LW-L c8-306 A 4 a and Eol Prou LW-L c8-306
            // handle Colonia 4 , starname = Colonia, planet 4
            // handle Aurioum B A BELT
            // Kyloasly OY-Q d5-906 13 1

            List<string> elements;

            ScanNodeType starscannodetype = ScanNodeType.star;          // presuming.. 

            string rest = sc.IsStarNameRelatedReturnRest(sys.name);
            if (rest != null)                                   // if we have a relationship..
            {
                if (rest.Length > 0)
                {
                    elements = rest.Split(' ').ToList();

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
                elements = sc.BodyName.Split(' ').ToList();     // not related in any way (earth) so assume all bodyparts, and 
                elements.Insert(0, "Main Star");                     // insert the MAIN designator as the star designator
            }

            if (elements.Count >= 1)                          // okay, we have an element.. first is the star..
            {
                ScanNode sublv0;

                if (!sn.starnodes.TryGetValue(elements[0], out sublv0))     // not found this node, add..
                {
                    sublv0 = new ScanNode()
                    {
                        ownname = elements[0],
                        fullname = sys.name + (elements[0].Contains("Main") ? "" : (" " + elements[0])),
                        ScanData = null,
                        children = null,
                        type = starscannodetype
                    };

                    sn.starnodes.Add(elements[0], sublv0);
                    //System.Diagnostics.Debug.WriteLine("Added star " + star.fullname + " '" + star.ownname + "'" + " under '" + designator + "' type " + ty);
                }

                if (elements.Count >= 2)                        // we have a sub designator..
                {
                    ScanNode sublv1;

                    if (sublv0.children == null || !sublv0.children.TryGetValue(elements[1], out sublv1))
                    {
                        if (sublv0.children == null)
                            sublv0.children = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>());

                        sublv1 = new ScanNode() { ownname = elements[1], fullname = sublv0.fullname + " " + elements[1], ScanData = null, children = null, type = ScanNodeType.planet };
                        sublv0.children.Add(elements[1], sublv1);
                    }

                    if (elements.Count >= 3)
                    {
                        ScanNode sublv2;

                        if (sublv1.children == null || !sublv1.children.TryGetValue(elements[2], out sublv2))
                        {
                            if (sublv1.children == null)
                                sublv1.children = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>());

                            sublv2 = new ScanNode() { ownname = elements[2], fullname = sublv0.fullname + " " + elements[1] + " " + elements[2], ScanData = null, children = null, type = ScanNodeType.moon };
                            sublv1.children.Add(elements[2], sublv2);
                        }

                        if (elements.Count >= 4)
                        {
                            ScanNode sublv3;

                            if (sublv2.children == null || !sublv2.children.TryGetValue(elements[3], out sublv3))
                            {
                                if (sublv2.children == null)
                                    sublv2.children = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>());

                                sublv3 = new ScanNode() { ownname = elements[3], fullname = sublv0.fullname + " " + elements[1] + " " + elements[2] + " " + elements[3], ScanData = null, children = null, type = ScanNodeType.submoon };
                                sublv2.children.Add(elements[3], sublv3);
                            }

                            if (elements.Count == 4)            // okay, need only 4 elements now.. if not, we have not coped..
                                sublv3.ScanData = sc;
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Failed to add system " + sc.BodyName + " too long");
                                return false;
                            }
                        }
                        else
                        {
                            sublv2.ScanData = sc;
                        }
                    }
                    else
                    {
                        sublv1.ScanData = sc;
                    }
                }
                else
                {
                    sublv0.ScanData = sc;
                }

                return true;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed to add system " + sc.BodyName + " not enough elements");
                return false;
            }
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

                return x.CompareTo(y);
            }
        }

    }
}
