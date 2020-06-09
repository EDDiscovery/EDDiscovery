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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using OpenTK;
using System.Resources;
using EDDiscovery.Properties;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.DB;
using EliteDangerousCore;
using System.IO;
using OpenTKUtils.GL1;
using OpenTKUtils;

namespace EDDiscovery._3DMap
{
    public class DatasetBuilder
    {
        private List<IData3DCollection> _datasets;
        private static Dictionary<string, TexturedQuadData> _cachedTextures = new Dictionary<string, TexturedQuadData>();
        private static Dictionary<string, Bitmap> _cachedBitmaps = new Dictionary<string, Bitmap>();

        Color CoarseGridLines = System.Drawing.ColorTranslator.FromHtml("#296A6C");
        Color FineGridLines = System.Drawing.ColorTranslator.FromHtml("#202020");
        Color CentredSystem = System.Drawing.Color.Yellow;
        Color PlannedRouteColor = System.Drawing.Color.Green;

        Bitmap SelectedMarker = (Bitmap)Icons.Controls.Map3D_Markers_Selected;

        public Vector2 MinGridPos { get; set; } = new Vector2(-50000.0f, -20000.0f);
        public Vector2 MaxGridPos { get; set; } = new Vector2(50000.0f, 80000.0f);

        int gridunitSize = 1000;

        public DatasetBuilder()
        {
            _datasets = new List<IData3DCollection>();
        }


        #region MAPS

       public List<IData3DCollection> AddMapImages(BaseUtils.Map2d [] list)
        {
            if (list.Length > 0 )
            {
                var datasetMapImg = Data3DCollection<TexturedQuadData>.Create("mapimage", Color.White, 1.0f);

                for( int i = 0; i < list.Length; i++)
                {
                    BaseUtils.Map2d img = list[i];

                    if (_cachedTextures.ContainsKey(img.FileName))
                    {
                        datasetMapImg.Add(_cachedTextures[img.FileName]);
                    }
                    else
                    {
                        Bitmap bmp = (Bitmap)Bitmap.FromFile(img.FilePath);
                                                                                    
                        Vector3 centre = new Vector3((img.TopLeft.X + img.BottomRight.X) / 2, 0, (img.TopRight.Y + img.BottomLeft.Y) / 2);
                        float width = img.TopRight.X - img.BottomLeft.X;
                        float height = img.TopLeft.Y - img.BottomRight.Y;           // its rectangular.. so does not really matter which left/right/top/bot you use

                        var texture = TexturedQuadData.FromBitmap(bmp, centre, TexturedQuadData.NoRotation, width, height );

                        _cachedTextures[img.FileName] = texture;
                        datasetMapImg.Add(texture);
                    }
                }
                _datasets.Add(datasetMapImg);
            }

            return _datasets;
        }

        #endregion

        #region Bookmarks

        public List<IData3DCollection> AddStarBookmarks(Bitmap mapstar, Bitmap mapregion, Bitmap maptarget, Bitmap mapsurface, float widthly, float heightly, Vector3 rotation )
        {
            var datasetbks = Data3DCollection<TexturedQuadData>.Create("bkmrs", Color.White, 1f);

            long bookmarktarget = TargetClass.GetTargetBookmark();

            foreach (BookmarkClass bc in GlobalBookMarkList.Instance.Bookmarks)
            {
                Bitmap touse = (bc.id == bookmarktarget) ? maptarget : (bc.isRegion ? mapregion : (bc.hasPlanetaryMarks ? mapsurface : mapstar));
                TexturedQuadData newtexture = TexturedQuadData.FromBitmap(touse, new PointData(bc.x, bc.y, bc.z), rotation, widthly, heightly,  0, heightly / 2);
                newtexture.Tag = bc;
                newtexture.Tag2 = 0;        // bookmark
                datasetbks.Add(newtexture);
            }

            _datasets.Add(datasetbks);

            return _datasets;
        }

        public List<IData3DCollection> AddNotedBookmarks(Bitmap map, Bitmap maptarget, float widthly, float heightly, Vector3 rotation, List<HistoryEntry> syslists)
        {
            var datasetbks = Data3DCollection<TexturedQuadData>.Create("bkmrs", Color.White, 1f);

            long bookmarknoted = TargetClass.GetTargetNotedSystem();

            if (syslists != null)
            {
                foreach (HistoryEntry vs in syslists.Where(s => s.IsLocOrJump))
                {
                    SystemNoteClass notecs = SystemNoteClass.GetNoteOnSystem(vs.System.Name, vs.System.EDSMID);

                    if (notecs != null)         // if we have a note..
                    {
                        string note = notecs.Note.Trim();

                        if (note.Length > 0)
                        {
                            PointData pd = new PointData(vs.System.X, vs.System.Y, vs.System.Z);

                            Bitmap touse = (notecs.id == bookmarknoted) ? maptarget : map;
                            TexturedQuadData newtexture = TexturedQuadData.FromBitmap(touse, pd, rotation, widthly, heightly, 0, heightly / 2);
                            newtexture.Tag = vs;
                            newtexture.Tag2 = 1;        // note mark
                            datasetbks.Add(newtexture);
                        }
                    }
                }
            }

            _datasets.Add(datasetbks);

            return _datasets;
        }

        public void UpdateBookmarks(ref List<IData3DCollection> _datasets, float widthly, float heightly, Vector3 rotation)
        {
            if (_datasets == null)
                return;

            foreach (IData3DCollection dataset in _datasets)
            {
                TexturedQuadDataCollection tqdc = dataset as TexturedQuadDataCollection;

                foreach (TexturedQuadData tqd in tqdc.Primatives)
                {
                    PointData pd;
                    if ( (int)tqd.Tag2 == 1)
                    {
                        HistoryEntry vs = (HistoryEntry)tqd.Tag;
                        pd = new PointData(vs.System.X, vs.System.Y, vs.System.Z);
                    }
                    else
                    {
                        BookmarkClass bc = (BookmarkClass)tqd.Tag;
                        pd = new PointData(bc.x, bc.y, bc.z);
                    }

                    tqd.UpdateVertices(pd, rotation, widthly, heightly, 0, heightly / 2);
                }
            }
        }

        #endregion

        #region GMOs

        static Font gmostarfont = BaseUtils.FontLoader.GetFont("MS Sans Serif", 20F);       // font size really determines the nicenest of the image, not its size on screen..
        float gmonameoff = -0.75F;      // bitmap is from +0.5 to -0.5.  
        float gmotargetoff = 1F;
        float gmoselonly = 0.75F;
        float gmoseltarget = 1.75F;

        public List<IData3DCollection> AddGalMapRegionsToDataset(GalacticMapping galmap, bool colourregions)
        {
            var polydataset = new PolygonCollection("regpolys", Color.White, 1f, OpenTK.Graphics.OpenGL.PrimitiveType.Triangles);      // ORDER AND NUMBER v.Important
            var outlinedataset = new PolygonCollection("reglines", Color.White, 1f , OpenTK.Graphics.OpenGL.PrimitiveType.LineLoop);   // DrawStars picks them out in a particular order
            var datasetbks = Data3DCollection<TexturedQuadData>.Create("regtext", Color.White, 1f);

            if (galmap != null && galmap.Loaded)
            {
                long gmotarget = TargetClass.GetTargetGMO();

                int cindex = 0;
                foreach (GalacticMapObject gmo in galmap.galacticMapObjects)
                {
                    if (gmo.galMapType.Enabled && gmo.galMapType.Group == GalMapType.GalMapGroup.Regions )
                    {
                        string name = gmo.name;

                        Color[] array = new Color[] { Color.Red, Color.Green, Color.Blue,
                                                    Color.Brown, Color.Crimson, Color.Coral,
                                                    Color.Aqua, Color.Yellow, Color.Violet,
                                                    Color.Sienna, Color.Silver, Color.Salmon,
                                                    Color.Pink , Color.AntiqueWhite , Color.Beige ,
                                                    Color.DarkCyan , Color.DarkGray , Color.ForestGreen , Color.LightSkyBlue ,
                                                    Color.Lime , Color.Maroon, Color.Olive, Color.SteelBlue};
                        Color c = array[cindex++ % array.Length];

                        List<Vector2> polygonxz = new List<Vector2>();                              // needs it in x/z and in vector2's
                        foreach( var pd in gmo.points )
                            polygonxz.Add(new Vector2((float)pd.X, (float)pd.Z));                   // can be concave and wound the wrong way..

                        Vector2 size, avg;
                        Vector2 centre = PolygonTriangulator.Centre(polygonxz, out size, out avg);  // default geographic centre (min x/z + max x/z/2) used in case poly triangulate fails (unlikely)
 
                        List<List<Vector2>> polys = PolygonTriangulator.Triangulate(polygonxz, false);  // cut into convex polygons first - because we want the biggest possible area for naming purposes
                        //Console.WriteLine("Region {0} decomposed to {1} ", name, polys.Count);

                        Vector2 bestpos = centre;
                        Vector2 bestsize = new Vector2(250, 250 / 5);

                        if (polys.Count > 0)                                                      // just in case..
                        {
                            centre = PolygonTriangulator.Centroids(polys);                       // weighted mean of the centroids
                            //Bitmap map3 = DrawString(String.Format("O{0}", cindex - 1), Color.White, gmostarfont); TexturedQuadData ntext3 = TexturedQuadData.FromBitmap(map3, new PointData(centre.X, 0, centre.Y), TexturedQuadData.NoRotation, 2000, 500); datasetbks.Add(ntext3);
                           
                            float mindist = float.MaxValue;

                            foreach (List<Vector2> points in polys)                         // now for every poly
                            {
                                if (colourregions)
                                {
                                    Color regcol = Color.FromArgb(64, c.R, c.G, c.B);

                                    if (points.Count == 3)                                    // already a triangle..
                                    {
                                        polydataset.Add(new Polygon(points, 1, regcol));
                                        //outlinedataset.Add(new Polygon(points, 1, Color.FromArgb(255, 255, 255, 0))); //DEBUG
                                    }
                                    else
                                    {
                                        List<List<Vector2>> polytri = PolygonTriangulator.Triangulate(points, true);    // cut into triangles not polygons

                                        foreach (List<Vector2> pt in polytri)
                                        {
                                            polydataset.Add(new Polygon(pt, 1, regcol));
                                            // outlinedataset.Add(new Polygon(pt, 1, Color.FromArgb(255, 255, 255, 0))); // DEBUG
                                        }
                                    }
                                }

                                //float area; Vector2 polycentrepos = PolygonTriangulator.Centroid(points,out area); Bitmap map2 = DrawString(String.Format("X") , Color.White, gmostarfont);  TexturedQuadData ntext2 = TexturedQuadData.FromBitmap(map2, new PointData(polycentrepos.X, 0, polycentrepos.Y), TexturedQuadData.NoRotation, 1000, 200); datasetbks.Add(ntext2);

                                PolygonTriangulator.FitInsideConvexPoly(points, centre, new Vector2(3000, 3000 / 5), new Vector2(200, 200),
                                                                        ref mindist, ref bestpos, ref bestsize, bestsize.X / 2);
                            }
                        }

                        Bitmap map = DrawString(gmo.name, Color.White, gmostarfont);
                        PointData bitmappos = new PointData(bestpos.X, 0, bestpos.Y);
                        TexturedQuadData ntext = TexturedQuadData.FromBitmap(map, bitmappos, TexturedQuadData.NoRotation,
                                                bestsize.X, bestsize.Y);

                        datasetbks.Add(ntext);

                        outlinedataset.Add(new Polygon(polygonxz, 1, Color.FromArgb(255, 128, 128, 128)));
                    }
                }
            }

            _datasets.Add(polydataset);
            _datasets.Add(outlinedataset);
            _datasets.Add(datasetbks);
            return _datasets;
        }

        public List<IData3DCollection> AddGalMapObjectsToDataset(GalacticMapping galmap, Bitmap target, float widthly, float heightly, Vector3 rotation, bool namethem , Color textc)
        {
            var datasetbks = Data3DCollection<TexturedQuadData>.Create("galobj", Color.White, 1f);

            if (galmap != null && galmap.Loaded)
            {
                long gmotarget = TargetClass.GetTargetGMO();

                foreach (GalacticMapObject gmo in galmap.galacticMapObjects)
                {
                    if (gmo.galMapType.Enabled)
                    {
                        Bitmap touse = gmo.galMapType.Image as Bitmap;                        // under our control, so must have it

                        if (touse != null && gmo.points.Count > 0)             // if it has an image its a point object , and has co-ord
                        {
                            Vector3 pd = gmo.points[0].Convert();
                            string tucachename = "GalMapType:" + gmo.galMapType.Typeid;
                            TexturedQuadData tubasetex = null;

                            if (_cachedTextures.ContainsKey(tucachename))
                            {
                                tubasetex = _cachedTextures[tucachename];
                            }
                            else
                            {
                                tubasetex = TexturedQuadData.FromBitmap(touse, pd, rotation, widthly, heightly);
                                _cachedTextures[tucachename] = tubasetex;
                            }


                            TexturedQuadData newtexture = TexturedQuadData.FromBaseTexture(tubasetex, pd, rotation, widthly, heightly);
                            newtexture.Tag = gmo;
                            newtexture.Tag2 = 0;
                            datasetbks.Add(newtexture);

                            if (gmo.id == gmotarget)
                            {
                                TexturedQuadData ntag = TexturedQuadData.FromBitmap(target, pd, rotation, widthly, heightly, 0, heightly * gmotargetoff);
                                ntag.Tag = gmo;
                                ntag.Tag2 = 2;
                                datasetbks.Add(ntag);
                            }
                        }
                    }
                }

                if (namethem)
                {
                    bool useaggregate = true;

                    if (useaggregate)
                    {
                        foreach (GalMapType t in galmap.galacticMapTypes)
                        {
                            if (t.Enabled)
                            {
                                Bitmap bmp = null;
                                TexturedQuadData nbasetex = null;
                                List<TexturedQuadData> ntex = new List<TexturedQuadData>();

                                string ncachename = "GalMapNames:" + t.Typeid + textc.ToString();
                                if (_cachedBitmaps.ContainsKey(ncachename) && _cachedTextures.ContainsKey(ncachename))
                                {
                                    bmp = _cachedBitmaps[ncachename];
                                    nbasetex = _cachedTextures[ncachename];
                                    ntex = nbasetex.Children.ToList();
                                }
                                else
                                {
                                    List<GalacticMapObject> tgmos = galmap.galacticMapObjects.Where(o => o.galMapType.Typeid == t.Typeid && o.points.Count > 0).ToList();

                                    float maxheight = 32;
                                    List<Rectangle> bounds = new List<Rectangle>();
                                    List<float> widths = new List<float>();

                                    Bitmap stringstarmeasurebitmap = new Bitmap(1, 1);
                                    using (Graphics g = Graphics.FromImage(stringstarmeasurebitmap))
                                    {
                                        foreach (GalacticMapObject gmo in tgmos)
                                        {
                                            SizeF sz = g.MeasureString(gmo.name, gmostarfont);
                                            if (sz.Height > maxheight)
                                            {
                                                maxheight = sz.Height;
                                            }
                                            widths.Add(sz.Width);
                                        }
                                    }

                                    int textheight = (int)(maxheight + 4);

                                    int x = 0;
                                    int y = 0;

                                    foreach (float twidth in widths)
                                    {
                                        int w = (int)(twidth + 4);

                                        if ((w + x) > 1024)
                                        {
                                            x = 0;
                                            y = y + textheight;

                                            if (((y + textheight) % 1024) < (y % 1024))
                                            {
                                                y = y + ((1024 - y) % 1024);
                                            }
                                        }

                                        bounds.Add(new Rectangle(x, y, w, textheight));
                                        x = x + w;
                                    }

                                    y = y + textheight;

                                    bmp = new Bitmap(1024, y);
                                    nbasetex = new TexturedQuadData(null, null, bmp);

                                    using (Graphics g = Graphics.FromImage(bmp))
                                    {
                                        for (int i = 0; i < tgmos.Count; i++)
                                        {
                                            GalacticMapObject gmo = tgmos[i];
                                            string cachename = gmo.name + textc.ToString();
                                            Vector3 pd = gmo.points[0].Convert();
                                            Rectangle clip = bounds[i];
                                            Point pos = clip.Location;
                                            g.SetClip(clip);
                                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                                            using (Brush br = new SolidBrush(textc))
                                                g.DrawString(gmo.name, gmostarfont, br, pos);
                                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                                            TexturedQuadData tex = TexturedQuadData.FromBaseTexture(nbasetex, pd, rotation, clip,
                                                            (widthly / 10 * gmo.name.Length),
                                                            (heightly / 3),
                                                            0, heightly * gmonameoff);
                                            tex.Tag = gmo;
                                            tex.Tag2 = 1;
                                            _cachedTextures[cachename] = tex;
                                            ntex.Add(tex);
                                        }
                                    }

                                    _cachedBitmaps[ncachename] = bmp;
                                    _cachedTextures[ncachename] = nbasetex;
                                }

                                foreach (TexturedQuadData tex in ntex)
                                {
                                    datasetbks.Add(tex);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (GalacticMapObject gmo in galmap.galacticMapObjects)
                        {
                            if (gmo.galMapType.Enabled && gmo.points.Count > 0)
                            {
                                Vector3 pd = gmo.points[0].Convert();
                                Bitmap map = null;
                                string cachename = gmo.name + textc.ToString();

                                if (_cachedBitmaps.ContainsKey(cachename))      // cache them, they take a long time to compute..
                                {
                                    map = _cachedBitmaps[cachename];
                                }
                                else
                                {
                                    map = DrawString(gmo.name, textc, gmostarfont);
                                    _cachedBitmaps.Add(cachename, map);
                                }

                                TexturedQuadData ntext = TexturedQuadData.FromBitmap(map, pd, rotation,
                                                        (widthly / 10 * gmo.name.Length),
                                                        (heightly / 3),
                                                        0, heightly * gmonameoff);
                                ntext.Tag = gmo;
                                ntext.Tag2 = 1;
                                datasetbks.Add(ntext);
                            }
                        }
                    }
                }
            }

            _datasets.Add(datasetbks);

            return _datasets;
        }

        public void UpdateGalObjects(ref List<IData3DCollection> _datasets, float widthly, float heightly , Vector3 rotation )
        {
            if (_datasets == null)
                return;

            foreach ( IData3DCollection dataset in _datasets )
            {
                if (dataset is TexturedQuadDataCollection)
                {
                    TexturedQuadDataCollection tqdc = dataset as TexturedQuadDataCollection;

                    foreach (TexturedQuadData tqd in tqdc.Primatives)
                    {
                        GalacticMapObject gmo = tqd.Tag as GalacticMapObject;
                        Debug.Assert(gmo != null);

                        if (gmo.points.Count > 0)       // paranoia since its an external data source
                        {
                            int id = (int)tqd.Tag2;
                            if (id == 0)
                                tqd.UpdateVertices(gmo.points[0].Convert(), rotation, widthly, heightly);
                            else if (id == 1)
                                tqd.UpdateVertices(gmo.points[0].Convert(), rotation, (widthly / 10 * gmo.name.Length), (heightly / 3), 0, heightly * gmonameoff);
                            else
                                tqd.UpdateVertices(gmo.points[0].Convert(), rotation, widthly, heightly, 0, heightly * gmotargetoff);
                        }
                    }
                }
            }
        }

        #endregion

        #region Grids

        public List<IData3DCollection> AddGridCoords()
        {
            string fontname = "MS Sans Serif";
            {
                Font fnt = BaseUtils.FontLoader.GetFont(fontname, 20F);

                int bitmapwidth, bitmapheight;
                Bitmap text_bmp = new Bitmap(300, 30);
                using (Graphics g = Graphics.FromImage(text_bmp))
                {
                    SizeF sz = g.MeasureString("-99999,-99999", fnt);
                    bitmapwidth = (int)sz.Width + 4;
                    bitmapheight = (int)sz.Height + 4;
                }
                var datasetMapImgLOD1 = Data3DCollection<TexturedQuadData>.Create("text bitmap LOD1", Color.White, 1.0f);
                var datasetMapImgLOD2 = Data3DCollection<TexturedQuadData>.Create("text bitmap LOD2", Color.FromArgb(128, Color.White), 1.0f);

                int textheightly = 50;
                int textwidthly = textheightly * bitmapwidth / bitmapheight;

                int gridwideLOD1 = (int)Math.Floor((MaxGridPos.X - MinGridPos.X) / gridunitSize + 1);
                int gridhighLOD1 = (int)Math.Floor((MaxGridPos.Y - MinGridPos.Y) / gridunitSize + 1);
                int gridwideLOD2 = (int)Math.Floor((MaxGridPos.X - MinGridPos.X) / (gridunitSize * 10) + 1);
                int gridhighLOD2 = (int)Math.Floor((MaxGridPos.Y - MinGridPos.Y) / (gridunitSize * 10) + 1);
                int texwide = 1024 / bitmapwidth;
                int texhigh = 1024 / bitmapheight;
                int numtexLOD1 = (int)Math.Ceiling((gridwideLOD1 * gridhighLOD1) * 1.0 / (texwide * texhigh));
                int numtexLOD2 = (int)Math.Ceiling((gridwideLOD2 * gridhighLOD2) * 1.0 / (texwide * texhigh));

                List<TexturedQuadData> basetexturesLOD1 = Enumerable.Range(0, numtexLOD1).Select(i => new TexturedQuadData(null, null, new Bitmap(1024, 1024))).ToList();
                List<TexturedQuadData> basetexturesLOD2 = Enumerable.Range(0, numtexLOD2).Select(i => new TexturedQuadData(null, null, new Bitmap(1024, 1024))).ToList();

                for (float x = MinGridPos.X; x < MaxGridPos.X; x += gridunitSize)
                {
                    for (float z = MinGridPos.Y; z <= MaxGridPos.Y; z += gridunitSize)
                    {
                        int num = (int)(Math.Floor((x - MinGridPos.X) / gridunitSize) * gridwideLOD1 + Math.Floor((z - MinGridPos.Y) / gridunitSize));
                        int tex_x = (num % texwide) * bitmapwidth;
                        int tex_y = ((num / texwide) % texhigh) * bitmapheight;
                        int tex_n = num / (texwide * texhigh);

                        //Console.WriteLine("num {0} tex_x {1} tex_y {2} txt_n {3}", num, tex_x, tex_y, tex_n);

                        DrawGridBitmap(basetexturesLOD1[tex_n].Texture, x, z, fnt, tex_x, tex_y);
                        datasetMapImgLOD1.Add(basetexturesLOD1[tex_n].Horz(
                            x, z,
                            x + textwidthly, z + textheightly,
                            tex_x, tex_y, tex_x + bitmapwidth, tex_y + bitmapheight
                          ));
                    }
                }

                for (float x = MinGridPos.X; x < MaxGridPos.X; x += gridunitSize * 10)
                {
                    for (float z = MinGridPos.Y; z <= MaxGridPos.Y; z += gridunitSize * 10)
                    {
                        int num = (int)(Math.Floor((x - MinGridPos.X) / (gridunitSize * 10)) * gridwideLOD2 + Math.Floor((z - MinGridPos.Y) / (gridunitSize * 10)));
                        int tex_x = (num % texwide) * bitmapwidth;
                        int tex_y = ((num / texwide) % texhigh) * bitmapheight;
                        int tex_n = num / (texwide * texhigh);

                        DrawGridBitmap(basetexturesLOD2[tex_n].Texture, x, z, fnt, tex_x, tex_y);
                        datasetMapImgLOD2.Add(basetexturesLOD2[tex_n].Horz(
                            x, z,
                            x + textwidthly*10, z + textheightly*10,
                            tex_x, tex_y, tex_x + bitmapwidth, tex_y + bitmapheight
                        ));
                    }
                }

                _datasets.Add(datasetMapImgLOD1);
                _datasets.Add(datasetMapImgLOD2);
            }

            return _datasets;
        }


        private Bitmap DrawGridBitmap(Bitmap text_bmp, float x, float z, Font fnt, int px, int py)
        {
            using (Graphics g = Graphics.FromImage(text_bmp))
            {
                using (Brush br = new SolidBrush(CoarseGridLines))
                    g.DrawString(x.ToStringInvariant("0") + "," + z.ToStringInvariant("0"), fnt, br, new Point(px, py));
            }

            return text_bmp;
        }

        public List<IData3DCollection> AddFineGridLines()
        {
            int smallUnitSize = gridunitSize / 10;
            var smalldatasetGrid = Data3DCollection<LineData>.Create("gridFine", FineGridLines, 0.6f);

            for (float x = MinGridPos.X; x <= MaxGridPos.X; x += smallUnitSize)
            {
                smalldatasetGrid.Add(new LineData(x, 0, MinGridPos.Y, x, 0, MaxGridPos.Y ));
            }

            for (float z = MinGridPos.Y; z <= MaxGridPos.Y; z += smallUnitSize)
            {
                smalldatasetGrid.Add(new LineData(MinGridPos.X, 0, z, MaxGridPos.X, 0, z));
            }

            _datasets.Add(smalldatasetGrid);
            return _datasets;
        }

        public List<IData3DCollection> AddCoarseGridLines()
        {
            var datasetGridLOD1 = Data3DCollection<LineData>.Create("gridNormal", CoarseGridLines, 0.6f);

            for (float x = MinGridPos.X; x <= MaxGridPos.X; x += gridunitSize)
            {
                datasetGridLOD1.Add(new LineData(x, 0, MinGridPos.Y, x, 0, MaxGridPos.Y));
            }

            for (float z = MinGridPos.Y; z <= MaxGridPos.Y; z += gridunitSize)
            {
                datasetGridLOD1.Add(new LineData(MinGridPos.X, 0, z, MaxGridPos.X, 0, z));
            }

            _datasets.Add(datasetGridLOD1);

            var datasetGridLOD2 = Data3DCollection<LineData>.Create("gridCoarse", CoarseGridLines, 0.6f);

            for (float x = MinGridPos.X; x <= MaxGridPos.X; x += gridunitSize * 10)
            {
                datasetGridLOD2.Add(new LineData(x, 0, MinGridPos.Y, x, 0, MaxGridPos.Y));
            }

            for (float z = MinGridPos.Y; z <= MaxGridPos.Y; z += gridunitSize * 10)
            {
                datasetGridLOD2.Add(new LineData(MinGridPos.X, 0, z, MaxGridPos.X, 0, z));
            }

            _datasets.Add(datasetGridLOD2);
            return _datasets;
        }

        public void UpdateGridZoom(ref List<IData3DCollection> _datasets, float zoom)
        {
            IData3DCollection grid = _datasets.SingleOrDefault(s => s.Name == "gridNormal");

            if ( grid != null )
            {
                LineDataCollection ldc = grid as LineDataCollection;
                var colour = CoarseGridLines;
                float LODfade = (float)Math.Max(Math.Min((zoom / 0.1 - 1.0) / 5.0, 1.0), 0.5);
                ldc.Color = Color.FromArgb((int)(colour.R * LODfade), (int)(colour.G * LODfade), (int)(colour.B * LODfade));
                //Console.WriteLine("LOD {0} fade {1} Color {2}", ldc.Name, LODfade, ldc.Color);
            }
        }

        public void UpdateGridCoordZoom(ref List<IData3DCollection> _datasets, float zoom)
        {
            IData3DCollection gridLOD2 = _datasets.SingleOrDefault(s => s.Name == "text bitmap LOD2");

            if (gridLOD2 != null)
            {
                TexturedQuadDataCollection tqdc = gridLOD2 as TexturedQuadDataCollection;

                float LODfade = (float)Math.Max(Math.Min((0.2 / zoom - 1.0) / 2.0, 1.0), 0.0);
                Color calpha = Color.FromArgb((int)(255 * LODfade), Color.White);

                tqdc.SetColour(calpha);
                //Console.WriteLine("LOD {0} fade {1} Color {2}", tqdc.Name, LODfade, calpha);
            }
        }

        #endregion

        #region Routes

        // DotColour = transparent, use the map colour associated with each entry.  Else use this colour for all

        public List<IData3DCollection> BuildSystems(bool DrawLines, bool DrawDots, Color DotColour, List<HistoryEntry> syslists)
        {
            // we use the expanded capability of Line and Point to holds colours for each element instead of the previous sorting system
            // This means less submissions to GL.

            if (syslists.Any() )
            {
                if (DrawLines)
                {
                    var datasetl = Data3DCollection<LineData>.Create("visitedstarslines", Color.Transparent, 2.0f);

                    Vector3d? lastpos = null;

                    Color drawcolour = Color.Green;

                    foreach (HistoryEntry sp in syslists)
                    {
                        if (sp.EntryType == JournalTypeEnum.Resurrect || sp.EntryType == JournalTypeEnum.Died)
                        {
                            lastpos = null;
                        }
                        else if (sp.IsLocOrJump && sp.System.HasCoordinate)
                        {
                            if (sp.journalEntry is IJournalJumpColor)
                            {
                                drawcolour = Color.FromArgb(((IJournalJumpColor)sp.journalEntry).MapColor);
                                if (drawcolour.GetBrightness() < 0.05)
                                    drawcolour = Color.Red;
                            }

                            if (lastpos.HasValue)
                            {
                                datasetl.Add(new LineData(sp.System.X, sp.System.Y, sp.System.Z,
                                                    lastpos.Value.X, lastpos.Value.Y , lastpos.Value.Z, drawcolour));
                            }

                            lastpos = new Vector3d(sp.System.X, sp.System.Y, sp.System.Z);
                        }
                    }

                    _datasets.Add(datasetl);
                }

                if ( DrawDots )
                {
                    var datasetp = Data3DCollection<PointData>.Create("visitedstarsdots", DotColour, 2.0f);

                    Color drawcolour = Color.Green;

                    foreach (HistoryEntry vs in syslists)
                    {
                        if (vs.System.HasCoordinate)
                        {
                            if (vs.journalEntry is IJournalJumpColor)
                            {
                                drawcolour = Color.FromArgb(((IJournalJumpColor)vs.journalEntry).MapColor);
                                if (drawcolour.GetBrightness() < 0.05)
                                    drawcolour = Color.Red;
                            }

                            datasetp.Add(new PointData(vs.System.X, vs.System.Y, vs.System.Z, drawcolour));
                        }
                    }

                    _datasets.Add(datasetp);
                }
            }

            return _datasets;
        }

        public List<IData3DCollection> BuildRouteTri(List<ISystem> PlannedRoute)
        {
            if (PlannedRoute != null && PlannedRoute.Any())
            {
                var routeLines = Data3DCollection<LineData>.Create("PlannedRoute", PlannedRouteColor, 25.0f);
                ISystem prevSystem = PlannedRoute.First();
                foreach (ISystem point in PlannedRoute.Skip(1))
                {
                    routeLines.Add(new LineData(prevSystem.X, prevSystem.Y, prevSystem.Z, point.X, point.Y, point.Z));
                    prevSystem = point;
                }
                _datasets.Add(routeLines);
            }
            return _datasets;
        }

        #endregion

        #region Systems

        public List<IData3DCollection> BuildSelected(ISystem centersystem, ISystem selectedsystem, GalacticMapObject selectedgmo, float widthly, float heightly, Vector3 rotation )
        {
            Bitmap selmark  = SelectedMarker;

            if (centersystem != null)
            {
                var dataset = Data3DCollection<PointData>.Create("Center", CentredSystem, 5.0f);
                dataset.Add(new PointData(centersystem.X, centersystem.Y, centersystem.Z));
                _datasets.Add(dataset);
            }

            if (selectedsystem != null)
            {
                var datasetbks = Data3DCollection<TexturedQuadData>.Create("selstar", Color.White, 1f);

                TexturedQuadData newtexture = TexturedQuadData.FromBitmap(selmark, new PointData(selectedsystem.X,selectedsystem.Y,selectedsystem.Z), rotation, widthly, heightly/2, 0, heightly/4+heightly/16);
                newtexture.Tag = 0;
                datasetbks.Add(newtexture);
                _datasets.Add(datasetbks);
            }

            if ( selectedgmo != null )
            {
                if (selectedgmo.points.Count > 0)               // paranoia
                {
                    var datasetbks = Data3DCollection<TexturedQuadData>.Create("selgmo", Color.White, 1f);
                    long gmotarget = TargetClass.GetTargetGMO();
                    float hoff = (gmotarget == selectedgmo.id) ? (heightly * gmoseltarget) : (heightly * gmoselonly);
                    TexturedQuadData newtexture = TexturedQuadData.FromBitmap(selmark, selectedgmo.points[0].Convert(), rotation, widthly, heightly / 2, 0, hoff);
                    newtexture.Tag = 1;
                    datasetbks.Add(newtexture);
                    _datasets.Add(datasetbks);
                }
            }

            return _datasets;
        }

        public void UpdateSelected(ref List<IData3DCollection> _datasets, ISystem selectedsystem, GalacticMapObject selectedgmo, float widthly, float heightly, Vector3 rotation)
        {
            if (_datasets == null)
                return;

            foreach (IData3DCollection dataset in _datasets)
            {
                if (dataset is TexturedQuadDataCollection)
                {
                    TexturedQuadDataCollection tqdc = dataset as TexturedQuadDataCollection;

                    foreach (TexturedQuadData tqd in tqdc.Primatives)
                    {
                        int id = (int)tqd.Tag;

                        if (id == 0)
                            tqd.UpdateVertices(new PointData(selectedsystem.X, selectedsystem.Y, selectedsystem.Z), rotation, widthly, heightly / 2, 0, heightly / 4 + heightly / 16);
                        else if (selectedgmo.points.Count > 0)
                        {
                            long gmotarget = TargetClass.GetTargetGMO();
                            float hoff = (gmotarget == selectedgmo.id) ? (heightly * gmoseltarget) : (heightly * gmoselonly);
                            tqd.UpdateVertices(selectedgmo.points[0].Convert(), rotation, widthly, heightly / 2, 0, hoff);
                        }
                    }
                }
            }
        }

#endregion

#region Helpers

        static public Bitmap DrawString(string str, Color textcolour, Font fnt)
        {
            Bitmap stringstarmeasurebitmap = new Bitmap(1, 1);
            using (Graphics g = Graphics.FromImage(stringstarmeasurebitmap))
            {
                SizeF sz = g.MeasureString(str, fnt);

                Bitmap text_bmp = new Bitmap((int)sz.Width + 4, (int)sz.Height + 4);
                using (Graphics h = Graphics.FromImage(text_bmp))
                {
                    h.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    using (Brush br = new SolidBrush(textcolour))
                        h.DrawString(str, fnt, br, new Point(0, 0));
                    h.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                }

                return text_bmp;
            }
        }

#endregion
    }

    static class StaticHelpers3dmap
    {
        public static OpenTK.Vector3 Convert(this EMK.LightGeometry.Vector3 v)
        {
            return new OpenTK.Vector3(v.X, v.Y, v.Z);
        }
    }
}
