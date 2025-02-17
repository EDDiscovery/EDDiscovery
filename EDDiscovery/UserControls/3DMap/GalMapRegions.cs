/*
 * Copyright 2019-2021 Robbyxp1 @ github.com
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
 */

using EliteDangerousCore.GMO;
using OpenTK;
using GLOFC;
using GLOFC.GL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using GLOFC.GL4.Bitmaps;
using GLOFC.GL4.Shaders.Fragment;
using GLOFC.GL4.Shaders.Vertex;
using GLOFC.GL4.Shaders;

namespace EDDiscovery.UserControls.Map3D
{
    public class GalMapRegions
    {
        public GalMapRegions()
        {
        }

        public void Toggle() { renderstate = (renderstate + 1) % 8; UpdateEnables(); }
        public int SelectionMask { get { return renderstate; } set { renderstate = value; UpdateEnables(); } }
        public bool Enable { get { return enable; } set { enable = value;  if (value) UpdateEnables(); else regionshader.Enable = outlineshader.Enable = textrenderer.Enable = false; } }
        public bool Regions { get { return (renderstate & 1) != 0; } set { renderstate = (renderstate & 0x6) | (value ? 1 : 0); if ( enable) UpdateEnables(); } }
        public bool Outlines { get { return (renderstate & 2) != 0; } set { renderstate = (renderstate & 0x5) | (value ? 2 : 0); if ( enable) UpdateEnables(); } }
        public bool Text { get { return (renderstate & 4) != 0; } set { renderstate = (renderstate & 0x3) | (value ? 4 : 0); if (enable) UpdateEnables(); } }

        public class ManualCorrections
        {
            public ManualCorrections(string n, float x = 0, float y = 0) { name = n; this.x = x; this.y = y; }
            public string name;
            public float x, y;
        }

        public void CreateObjects(string name, GLItemsList items, GLRenderProgramSortedList rObjects, GalacticMapping galmap, float sizeofname = 5000, ManualCorrections[] corr = null)
        {
            List<Vector4> vertexcolourregions = new List<Vector4>();
            List<Vector4> vertexregionsoutlines = new List<Vector4>();
            List<ushort> vertexregionoutlineindex = new List<ushort>();

            Size bitmapsize = new Size(250, 22);
            textrenderer = new GLBitmaps(name + "-bitmaps", rObjects, bitmapsize,depthtest:false);
            items.Add(textrenderer);

            StringFormat fmt = new StringFormat(StringFormatFlags.NoWrap) { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            Font fnt = new Font("MS Sans Serif", 12F);

            int cindex = 0;

            foreach (GalacticMapObject gmo in galmap.AllObjects)
            {
                if (gmo.GalMapTypes[0].Group == GalMapType.GroupType.Regions)
                {
                    string gmoname = gmo.NameList;

                    List<Vector2> polygonxz = new List<Vector2>();                              // needs it in x/z and in vector2's
                    foreach (var pd in gmo.Points)
                    {
                        polygonxz.Add(new Vector2((float)pd.X, (float)pd.Z));                   // can be concave and wound the wrong way..
                        vertexregionoutlineindex.Add((ushort)(vertexregionsoutlines.Count));
                        vertexregionsoutlines.Add(new Vector4((float)pd.X, 0, (float)pd.Z, 1));
                    }

                    vertexregionoutlineindex.Add(0xffff);       // primitive restart to break polygon

                    List<List<Vector2>> polys = GLOFC.Utils.PolygonTriangulator.Triangulate(polygonxz, false);  // cut into convex polygons first - because we want the biggest possible area for naming purposes

                    Vector2 avgcentroid = new Vector2(0, 0);
                    int pointsaveraged = 0;

                    if (polys.Count > 0)                                                      // just in case..
                    {
                        foreach (List<Vector2> points in polys)                         // now for every poly
                        {
                            List<List<Vector2>> polytri;
                            if (points.Count == 3)                                    // already a triangle..
                                polytri = new List<List<Vector2>>() { new List<Vector2>() { points[0], points[1], points[2] } };
                            else
                                polytri = GLOFC.Utils.PolygonTriangulator.Triangulate(points, true);    // cut into triangles not polygons

                            foreach (List<Vector2> pt in polytri)
                            {
                                vertexcolourregions.Add(pt[0].ToVector4XZ(w: cindex));
                                vertexcolourregions.Add(pt[2].ToVector4XZ(w: cindex));
                                vertexcolourregions.Add(pt[1].ToVector4XZ(w: cindex));

                                var cx = (pt[0].X + pt[1].X + pt[2].X) / 3;
                                var cy = (pt[0].Y + pt[1].Y + pt[2].Y) / 3;
                                avgcentroid = new Vector2(avgcentroid.X + cx, avgcentroid.Y + cy);
                                pointsaveraged++;

                                //foreach (var pd in pt) // debug
                                //{
                                //    vertexregionoutlineindex.Add((ushort)(vertexregionsoutlines.Count));
                                //    vertexregionsoutlines.Add(new Vector4((float)pd.X, 0, (float)pd.Y, 1));
                                //}
                                //vertexregionoutlineindex.Add(0xffff);       // primitive restart to break polygon
                            }
                        }

                        cindex = (cindex+1) % array.Length;

                        Vector2 centeroid = GLOFC.Utils.PolygonTriangulator.WeightedCentroids(polys);

                        if (corr != null)   // allows the centeroid to be nerfed slightly
                        {
                            var entry = Array.Find(corr, x => gmo.NameList.Contains(x.name, StringComparison.InvariantCultureIgnoreCase));
                            if (entry != null)
                                centeroid = new Vector2(centeroid.X + entry.x, centeroid.Y + entry.y);
                        }

                        var final = GLOFC.Utils.PolygonTriangulator.FitInsideConvexPoly(polys, centeroid, new Vector2(sizeofname, sizeofname * (float)bitmapsize.Height / (float)bitmapsize.Width));

                        Vector3 bestpos = new Vector3(final.Item1.X, 0, final.Item1.Y);
                        Vector3 bestsize = new Vector3(final.Item2.X, 1, final.Item2.Y);
                        
                        textrenderer.Add(null, gmo.NameList, fnt, Color.White, Color.Transparent, bestpos, bestsize,new Vector3(0,0,0), fmt, alphafadescalar:5000, alphafadepos:500);
                    }
                }
            }

            fmt.Dispose();
            fnt.Dispose();

            // regions

            var vertregion = new GLPLVertexShaderWorldPalletColor(array.ToVector4(0.1f), true);
            var fragregion = new GLPLFragmentShaderVSColor();

            regionshader = new GLShaderPipeline(vertregion, fragregion, null, null);
            items.Add(regionshader);

            GLRenderState rt = GLRenderState.Tri();
            rt.DepthTest = false;
            var ridisplay = GLRenderableItem.CreateVector4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, rt, vertexcolourregions.ToArray());
            rObjects.Add(regionshader, name + "-regions", ridisplay);

            // outlines

            var vertoutline = new GLPLVertexShaderWorldCoord();
            var fragoutline = new GLPLFragmentShaderFixedColor(Color.Cyan);

            outlineshader = new GLShaderPipeline(vertoutline, fragoutline, null, null);
            items.Add(outlineshader);

            GLRenderState ro = GLRenderState.Lines();
            ro.DepthTest = false;
            ro.PrimitiveRestart = 0xffff;
            var rioutline = GLRenderableItem.CreateVector4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.LineStrip, ro, vertexregionsoutlines.ToArray());
            rioutline.CreateElementIndexUShort(items.NewBuffer(), vertexregionoutlineindex.ToArray());

            rObjects.Add(outlineshader, name + "-outlines", rioutline);

            renderstate = 7;
        }

        private GLShaderPipeline regionshader;
        private GLShaderPipeline outlineshader;
        private GLBitmaps textrenderer;
        private int renderstate = 0;
        private bool enable = true;

        private void UpdateEnables()
        {
            regionshader.Enable = Regions;
            outlineshader.Enable = Outlines;
            textrenderer.Enable = Text;
        }

        private static Color[] array = new Color[] { Color.Red, Color.Green, Color.Blue,
                                                    Color.Brown, Color.Crimson, Color.Coral,
                                                    Color.Aqua, Color.Yellow, Color.Violet,
                                                    Color.Sienna, Color.Silver, Color.Salmon,
                                                    Color.Pink , Color.AntiqueWhite , Color.Beige ,
                                                    Color.DarkCyan , Color.DarkGray , Color.ForestGreen , Color.LightSkyBlue ,
                                                    Color.Lime , Color.Maroon, Color.Olive, Color.SteelBlue};

    }

}
