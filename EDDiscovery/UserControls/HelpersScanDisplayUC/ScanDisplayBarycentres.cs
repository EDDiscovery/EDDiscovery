/*
 * Copyright © 2019 EDDiscovery development team
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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ExtendedControls;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

namespace EDDiscovery.UserControls
{
    public partial class ScanDisplayUserControl : UserControl
    {

int barycount;
        class BaryPointInfo
        {
            public Point point;
            public Point toppos;
            public int orderpos;
        }

        // sn is the scan nodes with barycentres structure, in correct heirachy.
        // nodecentres/nodes are paired with the drawn positions and node info

        Tuple<Point, int,int> DisplayBarynode(StarScan.ScanNode sn, int level, Dictionary<StarScan.ScanNode,Point> nodecentres, List<StarScan.ScanNode> nodes, List<ExtPictureBox.ImageElement> pc, int imagesize, bool horz = false)
        {
            if (sn.children == null)
                return new Tuple<Point, int,int>(Point.Empty, level,0);

            var tojoin = new List<BaryPointInfo>();
            int recursedepth = 0;

            // with the planet/moon nodes, we first find all the barycentres, and recurse into them, to calculate the recursion depth of the 
            // ones underneath - we need to to work out how high to place the lines

            foreach (var c in sn.children)
            {
                if (c.Value.type == StarScan.ScanNodeType.barycentre)       // deep dive down
                {
                    recursedepth = 1;
                    var x = DisplayBarynode(c.Value, level + 1, nodecentres, nodes, pc, imagesize, horz);
                    recursedepth = Math.Max(x.Item2, recursedepth);
                    System.Diagnostics.Debug.WriteLine("Draw bary " + x.Item1 + " " + level);
                    if ( horz )
                        tojoin.Add(new BaryPointInfo() { point = x.Item1, toppos = new Point(x.Item1.X-10, x.Item1.Y), orderpos = x.Item3 }); // make a join point for a barynode
                    else
                        tojoin.Add(new BaryPointInfo() { point = x.Item1, toppos = new Point(x.Item1.X, x.Item1.Y - 10), orderpos = x.Item3 }); // make a join point for a barynode
                }
            }

            int lineoff = imagesize + 8 + 10 * recursedepth;

            // now we go thru all the children which actually show, find them in the nodes list, add them to the join list

            int orderpos = 0;

            foreach (var c in sn.children)
            {
                if (c.Value.type != StarScan.ScanNodeType.barycentre)       // deep dive down
                {
                    int od = nodes.IndexOf(c.Value);
                    orderpos = od;
                    System.Diagnostics.Debug.WriteLine("Draw point " + nodecentres[c.Value] + " " + c.Value.fullname);
                    if ( horz )
                        tojoin.Add(new BaryPointInfo() { point = nodecentres[c.Value], toppos = new Point(nodecentres[c.Value].X - lineoff, nodecentres[c.Value].Y), orderpos = od });
                    else
                        tojoin.Add(new BaryPointInfo() { point = nodecentres[c.Value], toppos = new Point(nodecentres[c.Value].X, nodecentres[c.Value].Y - lineoff), orderpos = od });
                }
            }

            if (tojoin.Count > 1)       // we need two or more to make a job.
            {
                tojoin.Sort(delegate (BaryPointInfo l, BaryPointInfo r) { return l.orderpos.CompareTo(r.orderpos); });

                ExtPictureBox.ImageElement ie = new ExtPictureBox.ImageElement();
                ie.OwnerDraw(DrawBaryTree, new Rectangle(0,0,horz?1:0, 0), tojoin);         // use Width, which does not get affected by repositiontree, to record if horz
                pc.Insert(0, ie); // insert first so drawn under
barycount++;               
                System.Diagnostics.Debug.WriteLine("Lines to draw at " + level + " " + recursedepth + " linepos " + lineoff);
                for (int i = 0; i < tojoin.Count; i++)
                    System.Diagnostics.Debug.WriteLine(".. " + tojoin[i].point + " " + tojoin[i].toppos + " " + tojoin[i].orderpos);

                Point pi;
                if (horz)
                {
                    pi = new Point(tojoin[0].point.X - lineoff, (tojoin[0].point.Y + tojoin[1].point.Y) / 2);       // moons can't be split horzontally, don't need to worry like planets
                }
                else if (tojoin[0].point.Y == tojoin[1].point.Y)     // if first two are at the same level, use the mid point to star up a join point for the barycentre
                {
                    pi = new Point((tojoin[0].point.X + tojoin[1].point.X) / 2, tojoin[0].point.Y - lineoff);       // vert, on same line, so normal
                }
                else
                {
                    pi = new Point(tojoin[0].point.X + imagesize, tojoin[0].point.Y - lineoff);              // objects are not on same line, so we set the barypoint off to side a bit
                }

                return new Tuple<Point, int,int>(pi, level,orderpos);
            }
            else
                return new Tuple<Point, int, int>(Point.Empty, level,0);
        }


        void DrawBaryTree(Graphics g, ExtPictureBox.ImageElement e)
        {
            using (Pen p = new Pen(Color.FromArgb(255, 170, 170, 170), 2)) // gray
            {
                var tojoin = e.Tag as List<BaryPointInfo>;
                int linepos = e.Location.X;
                int sely = e.Location.Y;

                //int miny = tojoin.Select(x => x.Y).Min();

                var first = tojoin[0];
                for (int i = 0; i < tojoin.Count; i++)
                {
                    var n = tojoin[i];
                    g.DrawLine(p, n.point, n.toppos);

                    if (i > 0)
                    {
                        if (n.toppos.Y == first.toppos.Y || e.Location.Width == 1)      //.Width records if horizontal line, ie.moons.  If on same level, or horz
                        {
                            g.DrawLine(p, first.toppos, n.toppos);
                        }
                        else
                        {                                   // here, for vert (planets) the entries are not side by side, so we draw nice lines out
                            g.DrawLine(p, first.toppos, new Point(first.toppos.X + planetsize.Width * 2, first.toppos.Y));
                            g.DrawLine(p, n.toppos, new Point(n.toppos.X - planetsize.Width, n.toppos.Y));

                            first = tojoin[i];  // move the first point to the new line
                        }
                    }
                }

            }
        }
    }
}

