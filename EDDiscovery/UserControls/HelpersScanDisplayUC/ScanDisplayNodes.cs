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
using BaseUtils;

namespace EDDiscovery.UserControls
{
    public partial class ScanDisplayUserControl : UserControl
    {
        enum DrawLevel { TopLevelStar, PlanetLevel, MoonLevel };

        Dictionary<Bitmap, float> imageintensities = new Dictionary<Bitmap, float>();       // cached

        // return right bottom of area used from curpos

        Point DrawNode(List<ExtPictureBox.ImageElement> pc,
                            StarScan.ScanNode sn,
                            MaterialCommoditiesList curmats,    // curmats may be null
                            HistoryList hl,
                            Image notscanned,               // image if sn is not known
                            Point position,                 // position is normally left/middle, unless xiscentre is set.
                            bool xiscentre,
                            out int ximagecentre,           // centre in x of image
                            Size size,                      // nominal size
                            DrawLevel drawtype,          // drawing..
                            Color? backwash = null,         // optional back wash on image 
                            string appendlabeltext = ""     // any label text to append
                )
        {
            string tip;
            Point endpoint = position;
            ximagecentre = -1;

            JournalScan sc = sn.ScanData;

            if (sc != null && (!sc.IsEDSMBody || CheckEDSM))     // has a scan and its our scan, or we are showing EDSM
            {
                if (sn.type != StarScan.ScanNodeType.ring)       // not rings
                {
                    tip = sc.DisplayString(historicmatlist: curmats, currentmatlist: hl.GetLast?.MaterialCommodity);
                    if (sn.Signals != null)
                        tip += "\n" + "Signals".T(EDTx.ScanDisplayUserControl_Signals) + ":\n" + JournalSAASignalsFound.SignalList(sn.Signals, 4, "\n");

                    Bitmap nodeimage = (Bitmap)(sc.IsStar ? sc.GetStarTypeImage() : sc.GetPlanetClassImage());

                    string overlaytext = "";
                    var nodelabels = new string[2] { "", "" };

                    nodelabels[0] = sn.customname ?? sn.ownname;
                    if (sc.IsEDSMBody)
                        nodelabels[0] = "_" + nodelabels[0];

                    if (sc.IsStar)
                    {
                        if (ShowStarClasses)
                            overlaytext = sc.StarClassification;

                        if (sc.nStellarMass.HasValue)
                            nodelabels[1] = nodelabels[1].AppendPrePad($"{sc.nStellarMass.Value:N2} SM", Environment.NewLine);

                        if (drawtype == DrawLevel.TopLevelStar)
                        {
                            if (sc.nAge.HasValue)
                                nodelabels[1] = nodelabels[1].AppendPrePad($"{sc.nAge.Value:N0} MY", Environment.NewLine);

                            if (ShowHabZone)
                            {
                                var habZone = sc.GetHabZoneStringLs();
                                if (habZone.HasChars())
                                    nodelabels[1] = nodelabels[1].AppendPrePad($"{habZone}", Environment.NewLine);
                            }
                        }
                    }
                    else
                    {
                        if (ShowPlanetClasses)
                            overlaytext = Bodies.PlanetAbv(sc.PlanetTypeID);

                        if ((sn.ScanData.IsLandable || ShowAllG) && sn.ScanData.nSurfaceGravity != null)
                        {
                            nodelabels[1] = nodelabels[1].AppendPrePad($"{(sn.ScanData.nSurfaceGravity / JournalScan.oneGee_m_s2):N2}g", Environment.NewLine);
                        }
                    }

                    if (ShowDist)
                    {
                        if (drawtype != DrawLevel.MoonLevel)       // other than moons
                        {
                            if (sn.ScanData.IsOrbitingBaryCentre)          // if in orbit of barycentre
                            {
                                string s = $"{(sn.ScanData.DistanceFromArrivalLS):N1}ls";
                                if (sn.ScanData.nSemiMajorAxis.HasValue)
                                    s += "/" + sn.ScanData.SemiMajorAxisLSKM;
                                nodelabels[1] = nodelabels[1].AppendPrePad(s, Environment.NewLine);
                            }
                            else
                            {
                                nodelabels[1] = nodelabels[1].AppendPrePad($"{sn.ScanData.DistanceFromArrivalLS:N1}ls", Environment.NewLine);
                            }
                        }
                        else
                        {
                            if (!sn.ScanData.IsOrbitingBaryCentre && sn.ScanData.nSemiMajorAxis.HasValue)          // if not in orbit of barycentre
                            {
                                nodelabels[1] = nodelabels[1].AppendPrePad($"{(sn.ScanData.nSemiMajorAxis / JournalScan.oneLS_m):N1}ls", Environment.NewLine);
                            }
                        }
                    }

                    nodelabels[1] = nodelabels[1].AppendPrePad(appendlabeltext, Environment.NewLine);

   nodelabels[1] = nodelabels[1].AppendPrePad("" + sn.ScanData?.BodyID, Environment.NewLine);

                    bool valuable = sc.EstimatedValue >= ValueLimit;
                    int iconoverlays = ShowOverlays ? ((sc.Terraformable ? 1 : 0) + (sc.HasMeaningfulVolcanism ? 1 : 0) + (valuable ? 1 : 0) + (sc.Mapped ? 1 : 0) + (sn.Signals != null ? 1 : 0)) : 0;

                    //   if (sc.BodyName.Contains("4 b"))  iconoverlays = 0;

                    bool materialsicon = sc.HasMaterials && !ShowMaterials;
                    bool imageoverlays = sc.IsLandable || (sc.HasRings && drawtype != DrawLevel.TopLevelStar) || materialsicon;

                    int bitmapheight = size.Height * nodeheightratio / noderatiodivider;
                    int overlaywidth = bitmapheight / 6;
                    int imagewidtharea = (imageoverlays ? 2 : 1) * size.Width;            // area used by image+overlay if any
                    int iconwidtharea = (iconoverlays > 0 ? overlaywidth : 0);          // area used by icon width area on left

                    int bitmapwidth = iconwidtharea + imagewidtharea;                   // total width
                    int imageleft = iconwidtharea + imagewidtharea / 2 - size.Width / 2;  // calculate where the left of the image is 
                    int imagetop = bitmapheight / 2 - size.Height / 2;                  // and the top

                    Bitmap bmp = new Bitmap(bitmapwidth, bitmapheight);

                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        //backwash = Color.FromArgb(128, 40, 40, 40);
                        if (backwash.HasValue)
                        {
                            using (Brush b = new SolidBrush(backwash.Value))
                            {
                                g.FillRectangle(b, new Rectangle(iconwidtharea, 0, imagewidtharea, bitmapheight));
                            }
                        }

                        g.DrawImage(nodeimage, imageleft, imagetop, size.Width, size.Height);

                        if (sc.IsLandable)
                        {
                            int offset = size.Height * 4 / 16;
                            int scale = 5;
                            g.DrawImage(Icons.Controls.Scan_Bodies_Landable, new Rectangle(imageleft + size.Width / 2 - offset * scale / 2,
                                                                                           imagetop + size.Height / 2 - offset * scale / 2, offset * scale, offset * scale));
                        }

                        if (sc.HasRings && drawtype != DrawLevel.TopLevelStar)
                        {
                            g.DrawImage(sc.Rings.Count() > 1 ? Icons.Controls.Scan_Bodies_RingGap : Icons.Controls.Scan_Bodies_RingOnly,
                                            new Rectangle(imageleft - size.Width / 2, imagetop, size.Width * 2, size.Height));
                        }

                        if (iconoverlays > 0)
                        {
                            int ovsize = bmp.Height / 6;
                            int pos = 4;

                            if (sc.Terraformable)
                            {
                                g.DrawImage(Icons.Controls.Scan_Bodies_Terraformable, new Rectangle(0, pos, ovsize, ovsize));
                                pos += ovsize + 1;
                            }

                            if (sc.HasMeaningfulVolcanism) //this renders below the terraformable icon if present
                            {
                                g.DrawImage(Icons.Controls.Scan_Bodies_Volcanism, new Rectangle(0, pos, ovsize, ovsize));
                                pos += ovsize + 1;
                            }

                            if (valuable)
                            {
                                g.DrawImage(Icons.Controls.Scan_Bodies_HighValue, new Rectangle(0, pos, ovsize, ovsize));
                                pos += ovsize + 1;
                            }

                            if (sc.Mapped)
                            {
                                g.DrawImage(Icons.Controls.Scan_Bodies_Mapped, new Rectangle(0, pos, ovsize, ovsize));
                                pos += ovsize + 1;
                            }

                            if (sn.Signals != null)
                            {
                                g.DrawImage(Icons.Controls.Scan_Bodies_Signals, new Rectangle(0, pos, ovsize, ovsize));
                            }
                        }

                        if (materialsicon)
                        {
                            Image mm = Icons.Controls.Scan_Bodies_MaterialMore;
                            g.DrawImage(mm, new Rectangle(bmp.Width - mm.Width, bmp.Height - mm.Height, mm.Width, mm.Height));
                        }

                        if (overlaytext.HasChars())
                        {
                            float ii;
                            if (imageintensities.ContainsKey(nodeimage))        // find cache
                            {
                                ii = imageintensities[nodeimage];
                                //System.Diagnostics.Debug.WriteLine("Cached Image intensity of " + sn.fullname + " " + ii);
                            }
                            else
                            {
                                var imageintensity = nodeimage.Function(BitMapHelpers.BitmapFunction.Brightness, nodeimage.Width * 3 / 8, nodeimage.Height * 3 / 8, nodeimage.Width * 2 / 8, nodeimage.Height * 2 / 8);
                                ii = imageintensity.Item2;
                                imageintensities[nodeimage] = ii;
                                //System.Diagnostics.Debug.WriteLine("Calculated Image intensity of " + sn.fullname + " " + ii);
                            }

                            Color text = ii> 0.3f ? Color.Black : Color.FromArgb(255, 200, 200, 200);

                            using (Font f = new Font(EDDTheme.Instance.FontName, size.Width / 5.0f))
                            {
                                using (Brush b = new SolidBrush(text))
                                {
                                    g.DrawString(overlaytext, f, b, new Rectangle(iconwidtharea, 0, bitmapwidth - iconwidtharea, bitmapheight), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                                }
                            }
                        }
                    }

                    // for xcentre, center the image around position.X, not the bitmap, the icon width area is off to the left
                    Point postoplot = xiscentre ? new Point(position.X - imagewidtharea / 2 - iconwidtharea, position.Y) : position;

                    //System.Diagnostics.Debug.WriteLine("Body " + sc.BodyName + " plot at "  + postoplot + " " + bmp.Size + " " + (postoplot.X+imageleft) + "," + (postoplot.Y-bmp.Height/2+imagetop));


                    endpoint = CreateImageAndLabel(pc, bmp, postoplot, bmp.Size, out ximagecentre, nodelabels, tip);
                    ximagecentre += iconwidtharea / 2;       // adjust for left icon width, since we have iconwidth + image..

                    if (sc.HasMaterials && ShowMaterials)
                    {
                        Point matpos = new Point(endpoint.X + 4, position.Y);
                        Point endmat = CreateMaterialNodes(pc, sc, curmats, hl, matpos, materialsize);
                        endpoint = new Point(Math.Max(endpoint.X, endmat.X), Math.Max(endpoint.Y, endmat.Y)); // record new right point..
                    }
                }
            }
            else if (sn.type == StarScan.ScanNodeType.belt)
            {
                if (sn.BeltData != null)
                    tip = sn.BeltData.RingInformationMoons(true);
                else
                    tip = sn.ownname + Environment.NewLine + Environment.NewLine + "No scan data available".T(EDTx.ScanDisplayUserControl_NSD);

                if (sn.children != null && sn.children.Count != 0)
                {
                    foreach (StarScan.ScanNode snc in sn.children.Values)
                    {
                        if (snc.ScanData != null)
                        {
                            tip += "\n\n" + snc.ScanData.DisplayString();
                        }
                    }
                }

                Size bmpsize = new Size(size.Width, planetsize.Height * nodeheightratio / noderatiodivider);

                endpoint = CreateImageAndLabel(pc, Icons.Controls.Scan_Bodies_Belt, position, bmpsize, out ximagecentre, new string[] { sn.ownname + appendlabeltext }, tip, false);
            }
            else
            {
                if (sn.type == StarScan.ScanNodeType.barycentre)
                    tip = string.Format("Barycentre of {0}".T(EDTx.ScanDisplayUserControl_BC), sn.ownname);
                else
                    tip = sn.ownname + Environment.NewLine + Environment.NewLine + "No scan data available".T(EDTx.ScanDisplayUserControl_NSD);

                string nodelabel = sn.customname ?? sn.ownname;
                nodelabel += appendlabeltext;

                endpoint = CreateImageAndLabel(pc, notscanned, position, size, out ximagecentre, new string[] { nodelabel }, tip, false);
            }

            //    System.Diagnostics.Debug.WriteLine("Node " + sn.ownname + " " + position + " " + size + " -> "+ endpoint);
            return endpoint;
        }

        // curmats may be null
        Point CreateMaterialNodes(List<ExtPictureBox.ImageElement> pc, JournalScan sn, MaterialCommoditiesList curmats, HistoryList hl, Point matpos, Size matsize)
        {
            Point startpos = matpos;
            Point maximum = matpos;
            int noperline = 0;

            string matclicktext = sn.DisplayMaterials(2, curmats, hl.GetLast?.MaterialCommodity);

            foreach (KeyValuePair<string, double> sd in sn.Materials)
            {
                string tooltip = sn.DisplayMaterial(sd.Key, sd.Value, curmats, hl.GetLast?.MaterialCommodity);

                Color fillc = Color.Yellow;
                string abv = sd.Key.Substring(0, 1);

                MaterialCommodityData mc = MaterialCommodityData.GetByFDName(sd.Key);

                if (mc != null)
                {
                    abv = mc.Shortname;
                    fillc = mc.Colour;

                    if (HideFullMaterials)                 // check full
                    {
                        int? limit = mc.MaterialLimit();
                        MaterialCommodities matnow = curmats?.Find(mc);  // allow curmats = null

                        // debug if (matnow != null && mc.shortname == "Fe")  matnow.count = 10000;

                        if (matnow != null && limit != null && matnow.Count >= limit)        // and limit
                            continue;
                    }

                    if (ShowOnlyMaterialsRare && mc.IsCommonMaterial)
                        continue;
                }

                System.Drawing.Imaging.ColorMap colormap = new System.Drawing.Imaging.ColorMap();
                colormap.OldColor = Color.White;    // this is the marker colour to replace
                colormap.NewColor = fillc;

                Bitmap mat = BaseUtils.BitMapHelpers.ReplaceColourInBitmap((Bitmap)Icons.Controls.Scan_Bodies_Material, new System.Drawing.Imaging.ColorMap[] { colormap });

                BaseUtils.BitMapHelpers.DrawTextCentreIntoBitmap(ref mat, abv, stdfont, Color.Black);

                ExtPictureBox.ImageElement ie = new ExtPictureBox.ImageElement(
                                new Rectangle(matpos.X, matpos.Y, matsize.Width, matsize.Height), mat, tooltip + "\n\n" + "All " + matclicktext, tooltip);

                pc.Add(ie);

                maximum = new Point(Math.Max(maximum.X, matpos.X + matsize.Width), Math.Max(maximum.Y, matpos.Y + matsize.Height));

                if (++noperline == 4)
                {
                    matpos = new Point(startpos.X, matpos.Y + matsize.Height + materiallinespacerxy);
                    noperline = 0;
                }
                else
                    matpos.X += matsize.Width + materiallinespacerxy;
            }

            return maximum;
        }


        // plot at leftmiddle the image of size, return bot left accounting for label 
        // label can be null. returns ximagecentre of image

        Point CreateImageAndLabel(List<ExtPictureBox.ImageElement> c, Image i, Point leftmiddle, Size size, out int ximagecentre, string[] labels, string ttext, bool imgowned = true)
        {
            //System.Diagnostics.Debug.WriteLine("    " + label + " " + postopright + " size " + size + " hoff " + labelhoff + " laby " + (postopright.Y + size.Height + labelhoff));

            ExtPictureBox.ImageElement ie = new ExtPictureBox.ImageElement(new Rectangle(leftmiddle.X, leftmiddle.Y - size.Height / 2, size.Width, size.Height), i, ttext, ttext, imgowned);

            Point max = new Point(leftmiddle.X + size.Width, leftmiddle.Y + size.Height / 2);

            var labelie = new List<ExtPictureBox.ImageElement>();
            int laboff = 0;
            int vpos = leftmiddle.Y + size.Height / 2;

            foreach (string label in labels)
            {
                if (label.HasChars())
                {
                    Font f = stdfont;
                    int labcut = 0;
                    if (label[0] == '_')
                    {
                        f = stdfontUnderline;
                        labcut = 1;
                    }

                    Point labposcenthorz = new Point(leftmiddle.X + size.Width / 2, vpos);

                    ExtPictureBox.ImageElement labie = new ExtPictureBox.ImageElement();
                    Color backcolor = this.BackColor;       // override for debug

                    using (var frmt = new StringFormat() { Alignment = StringAlignment.Center })
                    {
                        labie.TextCentreAutosize(labposcenthorz, new Size(0, 1000), label.Substring(labcut), f, EDDTheme.Instance.LabelColor, backcolor, frmt: frmt);
                    }

                    labelie.Add(labie);

                    if (labie.Location.X < leftmiddle.X)
                        laboff = Math.Max(laboff, leftmiddle.X - labie.Location.X);
                    vpos += labie.Location.Height;
                }
            }


            foreach (var l in labelie)
            {
                l.Translate(laboff, 0);
                c.Add(l);
                max = new Point(Math.Max(max.X, l.Location.Right), Math.Max(max.Y, l.Location.Bottom));
                //System.Diagnostics.Debug.WriteLine("Label " + l.Location);
            }

            ie.Translate(laboff, 0);
            max = new Point(Math.Max(max.X, ie.Location.Right), Math.Max(max.Y, ie.Location.Bottom));
            c.Add(ie);

            //System.Diagnostics.Debug.WriteLine(".. Max " + max);

            ximagecentre = ie.Location.X + ie.Location.Width / 2;

            return max;
        }


    }
}

