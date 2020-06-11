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
        class BaryPointInfo
        {
            public Point point;
            public Point toppos;
            public int orderpos;
            public int est;
        }

        // sn is the scan nodes with barycentres structure, in correct heirachy.
        // nodecentres/nodes are paired with the drawn positions and node info

        const int baryspacing = 10;
        const int baryspacingmargin = baryspacing * 4;      // up to 4 bary lines..

        Tuple<Point, int, int> DisplayBarynode(StarScan.ScanNode sn, int level, Dictionary<StarScan.ScanNode, Point> nodecentres, List<StarScan.ScanNode> nodes, List<ExtPictureBox.ImageElement> pc, int imagesize, bool horz = false)
        {
            if (sn.children == null)
                return new Tuple<Point, int, int>(Point.Empty, level, 0);

            var tojoin = new List<BaryPointInfo>();

            int orderpos = 0;   // this records the last planet order pos for use to pass back up the tree - helps in ordering

            foreach (var c in sn.children)
            {
                if (c.Value.type == StarScan.ScanNodeType.barycentre)       // if a barycentre, draw it
                {
                    var x = DisplayBarynode(c.Value, level + 1, nodecentres, nodes, pc, imagesize, horz);
                    System.Diagnostics.Debug.WriteLine("                        ".Substring(0, level * 3) + level + " Draw bary " + c.Value.fullname + " " + x.Item1 + " " + level);
                    if (horz)
                        tojoin.Add(new BaryPointInfo() { point = x.Item1, toppos = x.Item1, orderpos = x.Item3 }); // make a join point for a barynode
                    else
                        tojoin.Add(new BaryPointInfo() { point = x.Item1, toppos = x.Item1, orderpos = x.Item3 }); // make a join point for a barynode. Leave the toppos for later
                }
                else
                {
                    // Note if EDSM draw is turned off, but it was in the data from another source, we would not have drawn it so we need to check that by
                    // seeing if the node has a nodecentre. No nodecentre, did not draw

                    if (nodecentres.ContainsKey(c.Value))
                    {
                        orderpos = nodes.IndexOf(c.Value);
                        System.Diagnostics.Debug.WriteLine("                        ".Substring(0, level * 3) + level + " Draw Body " + c.Value.fullname + " " + orderpos + " " + nodecentres[c.Value]);
                        if (horz)
                            tojoin.Add(new BaryPointInfo() { point = nodecentres[c.Value], toppos = new Point( nodecentres[c.Value].X - imagesize/2, nodecentres[c.Value].Y), orderpos = orderpos });
                        else
                            tojoin.Add(new BaryPointInfo() { point = nodecentres[c.Value], toppos = new Point(nodecentres[c.Value].X, nodecentres[c.Value].Y - 10*imagesize/8), orderpos = orderpos });
                    }
                }
            }

            if (tojoin.Count > 1)       // we need two or more to make a barynode line
            {
                tojoin.Sort(delegate (BaryPointInfo l, BaryPointInfo r) { return l.orderpos.CompareTo(r.orderpos); });      // make sure in order 

                if (horz)
                {
                    foreach (var c in tojoin)       // find min y of cohort (same line) and set estimate pos to it
                    {
                        int minx = tojoin.Where(x => Math.Abs(x.toppos.X - c.toppos.X) < baryspacingmargin).Select(x => x.toppos.X).Min();      // find minx in cohort
                        c.est = minx - baryspacing;     // don't change toppos, we need it for other estimations. hold in another var
                    }

                    foreach (var c in tojoin)
                        c.toppos = new Point(c.est, c.toppos.Y);    // apply calculated pos to toppos to align all barycentres to it
                }
                else
                {
                    foreach (var c in tojoin)       // find min y of cohort (same line) and set estimate pos to it
                    {
                        int miny = tojoin.Where(y => Math.Abs(y.toppos.Y - c.toppos.Y) < baryspacingmargin).Select(x => x.toppos.Y).Min();      // find miny in cohort
                        c.est = miny - baryspacing;     // don't change toppos, we need it for other estimations. hold in another var
                    }

                    foreach (var c in tojoin)
                        c.toppos = new Point(c.toppos.X, c.est);    // apply calculated pos to toppos to align all barycentres to it
                }

                ExtPictureBox.ImageElement ie = new ExtPictureBox.ImageElement();
                ie.OwnerDraw(DrawBaryTree, new Rectangle(0, 0, horz ? 1 : 0, 0), tojoin);         // use Width, which does not get affected by repositiontree, to record if horz
                pc.Insert(0, ie); // insert first so drawn under

                System.Diagnostics.Debug.Write("                        ".Substring(0, level * 3) + level + " Join co-ords");
                for (int i = 0; i < tojoin.Count; i++)
                    System.Diagnostics.Debug.Write(" " + tojoin[i].point + ":" + tojoin[i].toppos);

                Point pi;       // calculate and return the barycentre position 
                if (horz)
                {
                    if (tojoin.Average(x => x.toppos.X) == tojoin[0].toppos.X)      // if all on same line
                    {
                        pi = new Point(tojoin[0].toppos.X, (int)tojoin.Select(y => y.point.Y).Average());
                        System.Diagnostics.Debug.Write(" same xline");
                    }
                    else
                    {
                        pi = new Point(tojoin[0].toppos.X, tojoin[0].toppos.Y + imagesize); 
                        System.Diagnostics.Debug.Write(" not same xline");
                    }
                }
                else
                {
                    if (tojoin.Average(x => x.toppos.Y) == tojoin[0].toppos.Y)      // if all on same line
                    {
                        pi = new Point((int)tojoin.Select(x => x.point.X).Average(), tojoin[0].toppos.Y);
                        System.Diagnostics.Debug.Write(" same yline");
                    }
                    else
                    {
                        pi = new Point(tojoin[0].point.X + imagesize, tojoin[0].toppos.Y); // objects are not on same line, so we set the barypoint off to side a bit of the first
                        System.Diagnostics.Debug.Write(" not same yline");
                    }
                }

                System.Diagnostics.Debug.WriteLine(" Pass back " + pi);
                System.Diagnostics.Debug.WriteLine("");
                return new Tuple<Point, int, int>(pi, level, orderpos);
            }
            else
                return new Tuple<Point, int, int>(Point.Empty, level, 0);
        }


        void DrawBaryTree(Graphics g, ExtPictureBox.ImageElement e)
        {
            using (Pen p = new Pen(Color.FromArgb(255, 170, 170, 170), 2)) // gray
            {
                var tojoin = e.Tag as List<BaryPointInfo>;

                for (int i = 0; i < tojoin.Count; i++)
                {
                    var cur = tojoin[i];
                    g.DrawLine(p, cur.point, cur.toppos);           // always draw the sub from the centre to the top pos

                    if (i > 0)                                      // if we have a previous
                    {
                        var prev = tojoin[i - 1];

                        if (cur.toppos.Y == prev.toppos.Y || e.Location.Width == 1)  //.Width records if horizontal line, ie.moons.  If on same level, or horz
                        {
                            g.DrawLine(p, prev.toppos, cur.toppos); // draw from prev top to our top
                        }
                        else
                        {                                   // here, for vert (planets) the entries are not side by side, so we draw nice lines out
                            g.DrawLine(p, prev.toppos, new Point(prev.toppos.X + planetsize.Width * 2, prev.toppos.Y));
                            g.DrawLine(p, cur.toppos, new Point(cur.toppos.X - planetsize.Width, cur.toppos.Y));
                        }
                    }

                }

            }
        }
    }
}

