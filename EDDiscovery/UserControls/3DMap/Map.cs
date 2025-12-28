/*
 * Copyright 2019-2023 Robbyxp1 @ github.com
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

using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.GMO;
using GLOFC;
using GLOFC.Controller;
using GLOFC.GL4;
using GLOFC.GL4.Controls;
using GLOFC.GL4.Operations;
using GLOFC.GL4.Shaders;
using GLOFC.GL4.Shaders.Compute;
using GLOFC.GL4.Shaders.Fragment;
using GLOFC.GL4.Shaders.Sprites;
using GLOFC.GL4.ShapeFactory;
using GLOFC.GL4.Textures;
using GLOFC.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static GLOFC.GL4.Controls.GLBaseControl;

namespace EDDiscovery.UserControls.Map3D
{
    public interface MapSaver           // saver interface
    {
        void PutSetting<T>(string id, T value);
        T GetSetting<T>(string id, T defaultvalue);
        void DeleteSetting(string id);
    }

    public partial class Map
    {
        public Controller3D gl3dcontroller;
        public GLControlDisplay displaycontrol;
        public enum Parts       // select whats displayed
        {
            None = 0,
            Galaxy = (1 << 0),
            GalObjects = (1 << 1),
            Regions = (1 << 2),
            Grid = (1 << 3),
            StarDots = (1 << 4),
            TravelPath = (1 << 5),
            GalaxyStars = (1 << 6),
            NavRoute = (1 << 7),
            Route = (1 << 8),
            Bookmarks = (1 << 9),
            ImageList = (1<<10),

            Menu = (1<<16),
            RightClick = (1 << 17),
            SearchBox = (1 << 18),
            GalaxyResetPos = (1 << 19),
            PerspectiveChange = (1 << 20),
            YHoldButton = (1 << 21),
            LimitSelector = (1<<22),

            AutoEDSMStarsUpdate = (1 << 28),
            PrepopulateGalaxyStarsLocalArea = (1 << 29),

            Map3D = 0x10ffffff,
            //Map3D = 0x010ff0001,
        }

        public Action<List<ISystem>,bool> AddSystemsToExpedition;

        private Parts parts;

        private GLMatrixCalc matrixcalc;
        private GLOFC.WinForm.GLWinFormControl glwfc;

        private GLRenderProgramSortedList rObjects = new GLRenderProgramSortedList();
        private GLItemsList items = new GLItemsList();

        private Vector4[] volumetricboundingbox;
        private GLVolumetricUniformBlock volumetricblock;
        private GLRenderableItem galaxyrenderable;
        private GalaxyShader galaxyshader;

        private ImageCache userimages;
        private GLOFC.GL4.Bitmaps.GLBindlessTextureBitmaps usertexturebitmaps;

        private GLShaderPipeline gridtextshader;
        private GLShaderPipeline gridshader;
        private GLRenderableItem gridrenderable;

        private TravelPath travelpath;
        private TravelPath routepath;
        private TravelPath navroute;

        public GalacticMapping galacticmapping;

        private GalMapObjects galmapobjects;
        private GalMapRegions edsmgalmapregions;
        private GalMapRegions elitemapregions;

        private GalaxyStarDots stardots;
        private GLPointSpriteShader starsprites;
        private GalaxyStars galaxystars;

        private Bookmarks bookmarks;

        private GLContextMenu rightclickmenu;

        private MapMenu galaxymenu;

        // global buffer blocks used
        private const int volumenticuniformblock = 2;
        private const int findblock = 3;
        private const int userbitmapsarbblock = 4;

        private System.Diagnostics.Stopwatch hptimer = new System.Diagnostics.Stopwatch();

        private int localareasize = 50;
        private UserControlCommonBase parent;

        private int autoscalegmo = 30;
        private int autoscalebookmarks = 30;
        private int autoscalegalstars = 30;

        private bool mapcreatedokay = false; 

        public Map()
        {
        }

        public void Dispose()
        {
            if (galaxystars != null)
                galaxystars.Stop();
            items.Dispose();
            
           // GLStatics.VerifyAllDeallocated(); // enable during debugging only - will except if multiple windows are open
        }

        public ulong ElapsedTimems { get { return glwfc.ElapsedTimems; } }

        #region Initialise
        public bool Start(GLOFC.WinForm.GLWinFormControl glwfc, GalacticMapping edsmmappingp, GalacticMapping eliteregions, UserControlCommonBase parent, Parts parts)
        {
            this.parent = parent;

            this.parts = parts;
            this.glwfc = glwfc;
            this.galacticmapping = edsmmappingp;

            hptimer.Start();

            GLShaderLog.Reset();
            GLShaderLog.AssertOnError = false;

            items.Add(new GLMatrixCalcUniformBlock(), "MCUB");     // create a matrix uniform block 

            int lyscale = 1;
            int front = -20000 / lyscale, back = front + 90000 / lyscale, left = -45000 / lyscale, right = left + 90000 / lyscale, vsize = 2000 / lyscale;

            // parts = parts - (1 << 9);

            System.Diagnostics.Debug.Assert(glwfc.IsContextCurrent());

            Bitmap galaxybitmap = BaseUtils.Icons.IconSet.GetBitmap("GalMap.Galaxy_L180");

            if ((parts & Parts.ImageList) != 0)
            {
                userimages = new ImageCache(items, rObjects);
                usertexturebitmaps = new GLOFC.GL4.Bitmaps.GLBindlessTextureBitmaps("UserBitmaps", rObjects, userbitmapsarbblock, false);
                items.Add(usertexturebitmaps);
            }

            if ((parts & Parts.Galaxy) != 0) // galaxy
            {
                volumetricboundingbox = new Vector4[]
                    {
                        new Vector4(left,-vsize,front,1),
                        new Vector4(left,vsize,front,1),
                        new Vector4(right,vsize,front,1),
                        new Vector4(right,-vsize,front,1),

                        new Vector4(left,-vsize,back,1),
                        new Vector4(left,vsize,back,1),
                        new Vector4(right,vsize,back,1),
                        new Vector4(right,-vsize,back,1),
                    };


                const int gnoisetexbinding = 3;     //tex bindings are attached per shaders so are not global
                const int gdisttexbinding = 4;
                const int galtexbinding = 1;

                volumetricblock = new GLVolumetricUniformBlock(volumenticuniformblock);
                items.Add(volumetricblock, "VB");

                int sc = 1;
                GLTexture3D noise3d = new GLTexture3D(1024 * sc, 64 * sc, 1024 * sc, OpenTK.Graphics.OpenGL4.SizedInternalFormat.R32f); // red channel only
                items.Add(noise3d, "Noise");
                ComputeShaderNoise3D csn = new ComputeShaderNoise3D(noise3d.Width, noise3d.Height, noise3d.Depth, 128 * sc, 16 * sc, 128 * sc, gnoisetexbinding);       // must be a multiple of localgroupsize in csn
                csn.StartAction += (A, m) => { noise3d.BindImage(gnoisetexbinding); };
                csn.Run();      // compute noise
                csn.Dispose();  // and finish with it

                GLTexture1D gaussiantex = new GLTexture1D(1024, OpenTK.Graphics.OpenGL4.SizedInternalFormat.R32f); // red channel only
                items.Add(gaussiantex, "Gaussian");

                // set centre=width, higher widths means more curve, higher std dev compensate.
                // fill the gaussiantex with data
                ComputeShaderGaussian gsn = new ComputeShaderGaussian(gaussiantex.Width, 2.0f, 2.0f, 1.4f, gdisttexbinding);
                gsn.StartAction += (A, m) => { gaussiantex.BindImage(gdisttexbinding); };
                gsn.Run();      // compute noise
                gsn.Dispose();

                GL.MemoryBarrier(MemoryBarrierFlags.AllBarrierBits);

                // load one upside down and horz flipped, because the volumetric co-ords are 0,0,0 bottom left, 1,1,1 top right
                GLTexture2D galtex = new GLTexture2D(galaxybitmap, SizedInternalFormat.Rgba8);
                items.Add(galtex, "galtex");
                galaxyshader = new GalaxyShader(volumenticuniformblock, galtexbinding, gnoisetexbinding, gdisttexbinding);
                items.Add(galaxyshader, "Galaxy-sh");
                // bind the galaxy texture, the 3dnoise, and the gaussian 1-d texture for the shader
                galaxyshader.StartAction += (a, m) => { galtex.Bind(galtexbinding); noise3d.Bind(gnoisetexbinding); gaussiantex.Bind(gdisttexbinding); };      // shader requires these, so bind using shader

                GLRenderState rt = GLRenderState.Tri();
                galaxyrenderable = GLRenderableItem.CreateNullVertex(OpenTK.Graphics.OpenGL4.PrimitiveType.Points, rt);   // no vertexes, all data from bound volumetric uniform, no instances as yet
                rObjects.Add(galaxyshader, "galshader", galaxyrenderable);
            }

            if ((parts & Parts.Regions) != 0)
            {
                var corr = new GalMapRegions.ManualCorrections[] {          // nerf the centeroid position slightly
                    new GalMapRegions.ManualCorrections("The Galactic Aphelion", y: -2000 ),
                    new GalMapRegions.ManualCorrections("The Abyss", y: +3000 ),
                    new GalMapRegions.ManualCorrections("Eurus", y: -3000 ),
                    new GalMapRegions.ManualCorrections("The Perseus Transit", x: -3000, y: -3000 ),
                    new GalMapRegions.ManualCorrections("Zephyrus", x: 0, y: 2000 ),
                };

                edsmgalmapregions = new GalMapRegions();
                edsmgalmapregions.CreateObjects("edsmregions", items, rObjects, galacticmapping, 8000, corr: corr);
            }

            if ((parts & Parts.Regions) != 0)
            {
                elitemapregions = new GalMapRegions();
                elitemapregions.CreateObjects("eliteregions", items, rObjects, eliteregions, 8000);
                EliteRegionsEnable = false;
            }

            if ((parts & Parts.StarDots) != 0)
            {
                int gran = 8;
                Bitmap heat = galaxybitmap.Function(galaxybitmap.Width / gran, galaxybitmap.Height / gran, mode: GLOFC.Utils.BitMapHelpers.BitmapFunction.HeatMap);
                //heat.Save(@"c:\code\heatmap.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                Random rnd = new Random(23);

                GLBuffer buf = items.NewBuffer(16 * 350000, false);     // since RND is fixed, should get the same number every time.
                buf.StartWrite(0); // get a ptr to the whole schebang

                int xcw = (right - left) / heat.Width;
                int zch = (back - front) / heat.Height;

                int points = 0;

                for (int x = 0; x < heat.Width; x++)
                {
                    for (int z = 0; z < heat.Height; z++)
                    {
                        int i = heat.GetPixel(x, z).R;
                        if (i > 32)
                        {
                            int gx = left + x * xcw;
                            int gz = front + z * zch;

                            float dx = (float)Math.Abs(gx) / 45000;
                            float dz = (float)Math.Abs(25889 - gz) / 45000;
                            double d = Math.Sqrt(dx * dx + dz * dz);     // 0 - 0.1412
                            d = 1 - d;  // 1 = centre, 0 = unit circle
                            d = d * 2 - 1;  // -1 to +1
                            double dist = ObjectExtensionsNumbersBool.GaussianDist(d, 1, 1.4);

                            int c = Math.Min(Math.Max(i * i * i / 120000, 1), 40);
                            //int c = Math.Min(Math.Max(i * i * i / 24000000, 1), 40);

                            dist *= 2000 / lyscale;
                            //System.Diagnostics.Debug.WriteLine("{0} {1} : dist {2} c {3}", x, z, dist, c);
                            //System.Diagnostics.Debug.Write(c);
                            GLPointsFactory.RandomStars4(buf, c, gx, gx + xcw, gz, gz + zch, (int)dist, (int)-dist, rnd, w: 0.8f);
                            points += c;
                            System.Diagnostics.Debug.Assert(points < buf.Length / 16);
                        }
                    }
                    //System.Diagnostics.Debug.WriteLine(".");
                }

                buf.StopReadWrite();

                stardots = new GalaxyStarDots();
                items.Add(stardots);
                GLRenderState rc = GLRenderState.Points(1);
                rc.DepthTest = false; // note, if this is true, there is a wierd different between left and right in view.. not sure why
                rObjects.Add(stardots, "stardots", GLRenderableItem.CreateVector4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.Points, rc, buf, points));
                System.Diagnostics.Debug.WriteLine("Stars " + points);
            }

            rObjects.Add(new GLOperationClearDepthBuffer()); // clear depth buffer and now use full depth testing on the rest

            if ((parts & Parts.StarDots) != 0)
            {
                Bitmap starflare = BaseUtils.Icons.IconSet.GetBitmap("GalMap.StarFlare2");
                items.Add(new GLTexture2D(starflare, SizedInternalFormat.Rgba8), "lensflare");
                starsprites = new GLPointSpriteShader(items.Tex("lensflare"), 64, 40);
                items.Add(starsprites, "PS");
                var p = GLPointsFactory.RandomStars4(1000, 0, 25899 / lyscale, 10000 / lyscale, 1000 / lyscale, -1000 / lyscale);
                GLRenderState rps = GLRenderState.PointsByProgram();
                rObjects.Add(starsprites, "starsprites", GLRenderableItem.CreateVector4Color4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.Points, rps, p, 
                                                new Color4[] { Color.White }));
            }

            if ((parts & Parts.Grid) != 0)
            {
                var gridvertshader = new DynamicGridVertexShader(Color.Cyan);
                var gridfragshader = new GLPLFragmentShaderVSColor();
                gridshader = new GLShaderPipeline(gridvertshader, gridfragshader);
                items.Add(gridshader, "DYNGRID");

                GLRenderState rl = GLRenderState.Lines();
                gridrenderable = GLRenderableItem.CreateNullVertex(OpenTK.Graphics.OpenGL4.PrimitiveType.Lines, rl, drawcount: 2);
                rObjects.Add(gridshader, "DYNGRIDRENDER", gridrenderable);
            }

            if ((parts & Parts.Grid) != 0)
            {
                var gridtextlabelsvertshader = new DynamicGridCoordVertexShader();
                var gridtextfragshader = new GLPLFragmentShaderTexture2DIndexed(0);
                gridtextshader = new GLShaderPipeline(gridtextlabelsvertshader, gridtextfragshader);
                items.Add(gridtextshader, "DYNGRIDBitmap");

                GLRenderState rl = GLRenderState.Tri(cullface: false);
                GLTexture2DArray gridtexcoords = new GLTexture2DArray();
                items.Add(gridtexcoords, "PLGridBitmapTextures");
                rObjects.Add(gridtextshader, "DYNGRIDBitmapRENDER", GLRenderableItem.CreateNullVertex(OpenTK.Graphics.OpenGL4.PrimitiveType.TriangleStrip, rl, drawcount: 4, instancecount: 9));
            }

            GLStorageBlock findresults = items.NewStorageBlock(findblock);

            var starimagearray = new GLTexture2DArray();
            Bitmap[] starcroppedbitmaps = BodyToImages.StarBitmaps(new RectangleF(16, 16, 68, 68));     // we own these
            if ( !EliteDangerousCore.DB.SystemsDatabase.Instance.HasStarType )
            {
                starcroppedbitmaps[(int)EDStar.Unknown] = starcroppedbitmaps[(int)EDStar.G];        // no star info, use G type.
            }
            // debug output of what is shown..
            //for (int b = 0; b < starcroppedbitmaps.Length; b++) starcroppedbitmaps[b].Save(@"c:\code\" + $"star{b}_{Enum.GetName(typeof(EDStar),b)}.bmp", System.Drawing.Imaging.ImageFormat.Png);

            // textures are all the same size, based on texture[0]. Make sure its the biggest
            starimagearray.CreateLoadBitmaps(starcroppedbitmaps, SizedInternalFormat.Rgba8, ownbmp: true, texturesize: starcroppedbitmaps[(int)EDStar.O].Size, alignment: ContentAlignment.MiddleCenter);
            items.Add(starimagearray);

            // control word for each star type. Note we have the ability to cull by star type by setting the control word to -1. bit 0-15 = image, bit 16 = no sunspots.
            // See LPLFragmentShaderTexture2DWSelectorSunspot (subspot and texture select) and GLPLVertexShaderModelWorldTextureAutoScale (culling)
            long[] starimagearraycontrolword = new long[starcroppedbitmaps.Length];
            for (uint i = 0; i < starcroppedbitmaps.Length; i++)
                starimagearraycontrolword[i] = i;
            starimagearraycontrolword[(int)EDStar.N] |= 65536;          // don't show sunspots for these
            starimagearraycontrolword[(int)EDStar.H] |= 65536;
            starimagearraycontrolword[(int)EDStar.X] |= 65536;
            starimagearraycontrolword[(int)EDStar.Nebula] |= 65536;
            starimagearraycontrolword[(int)EDStar.StellarRemnantNebula] |= 65536;
            starimagearraycontrolword[(int)EDStar.SuperMassiveBlackHole] |= 65536;

            if ((parts & Parts.GalObjects) != 0 && galacticmapping.VisibleMapObjects.Length > 0)            // add a check to stop loading it if we don't have any yet
            {
                galmapobjects = new GalMapObjects();
                galmapobjects.CreateObjects(items, rObjects, galacticmapping, findresults, true);
            }

            float galaxysunsize = 0.4f;
            float travelsunsize = 0.5f;        // slightly bigger hides the double painting
            float labelw = galaxysunsize * 10;
            float labelh = galaxysunsize * 10 / 6;
            float labelhoff = -(Math.Max(travelsunsize,galaxysunsize)/2.0f + labelh / 2.0f);
            Size labelbitmapsize = new Size(160, 16);
            Font labelfont = new Font("Arial", 8.25f);
            float tapesize = 0.25f;

            if ((parts & Parts.TravelPath) != 0)
            {
                travelpath = new TravelPath();
                travelpath.TextBitMapSize = labelbitmapsize;
                travelpath.LabelSize = new Vector3(labelw, 0, labelh);
                travelpath.LabelOffset = new Vector3(0, labelhoff, 0);
                travelpath.Font = labelfont;
                travelpath.Start("TP", 200000, travelsunsize, tapesize, findresults, new Tuple<GLTexture2DArray, long[]>(starimagearray, starimagearraycontrolword), true, items, rObjects);
                travelpath.CreatePath(parent.DiscoveryForm.History, galmapobjects?.PositionsWithEnable);
                travelpath.SetSystem(parent.DiscoveryForm.History.LastSystem);
            }

            if ((parts & Parts.NavRoute) != 0)
            {
                navroute = new TravelPath();
                navroute.TextBitMapSize = labelbitmapsize;
                navroute.LabelSize = new Vector3(labelw, 0, labelh);
                navroute.LabelOffset = new Vector3(0, labelhoff, 0);
                navroute.Font = labelfont;
                navroute.Start("NavRoute", 10000, travelsunsize, tapesize, findresults, new Tuple<GLTexture2DArray, long[]>(starimagearray, starimagearraycontrolword), true, items, rObjects);
                UpdateNavRoute();
            }

            if ((parts & Parts.Bookmarks) != 0)
            {
                bookmarks = new Bookmarks();
                bookmarks.Start(items, rObjects, 1.0f, findresults, true);
                UpdateBookmarks();
            }

            if ((parts & Parts.GalaxyStars) != 0)
            {
                galaxystars = new GalaxyStars();

                if ((parts & Parts.PrepopulateGalaxyStarsLocalArea) != 0)        
                {
                    galaxystars.SectorSize = 20;
                    //    galaxystars.ShowDistance = true;// decided show distance is a bad idea, but we keep the code in case i change my mind
                    //    galaxystars.BitMapSize = new Size(galaxystars.BitMapSize.Width, galaxystars.BitMapSize.Height * 2);     // more v height for ly text
                }

                galaxystars.TextBitMapSize = labelbitmapsize;
                galaxystars.LabelSize = new Vector3(labelw, 0, labelh );
                galaxystars.LabelOffset = new Vector3(0,labelhoff,0);
                galaxystars.Font = labelfont;

                galaxystars.Create(items, rObjects, new Tuple<GLTexture2DArray, long[]>(starimagearray, starimagearraycontrolword), galaxysunsize, findresults, galmapobjects);
            }

            if ((parts & Parts.Route) != 0)
            {
                routepath = new TravelPath();       // we have no data here, it needs a push to display this, so we just create
                routepath.TextBitMapSize = labelbitmapsize;
                routepath.LabelSize = new Vector3(labelw, 0, labelh);
                routepath.LabelOffset = new Vector3(0, labelhoff, 0);
                routepath.Font = labelfont;

                routepath.Start("Route", 10000, travelsunsize, tapesize, findresults, new Tuple<GLTexture2DArray, long[]>(starimagearray, starimagearraycontrolword), true, items, rObjects);
            }

            System.Diagnostics.Debug.Assert(glwfc.IsContextCurrent());

            // Matrix calc holding transform info

            matrixcalc = new GLMatrixCalc();
            matrixcalc.PerspectiveNearZDistance = 0.5f;
            matrixcalc.PerspectiveFarZDistance = 120000f / lyscale;
            matrixcalc.InPerspectiveMode = true;
            matrixcalc.ResizeViewPort(this, glwfc.Size);          // must establish size before starting

            // menu system

            displaycontrol = new GLControlDisplay(items, glwfc, matrixcalc, true, 0.00001f, 0.00001f);       // hook form to the window - its the master
            displaycontrol.Font = new Font("Arial", 10f);
            displaycontrol.Focusable = true;          // we want to be able to focus and receive key presses.
            displaycontrol.SetFocus();

            displaycontrol.Paint += (ts) => {
                System.Diagnostics.Debug.Assert(displaycontrol.IsContextCurrent());
                // MCUB set up by Controller3DDraw which did the work first
                galaxymenu?.UpdateCoords(gl3dcontroller);
                displaycontrol.Animate(glwfc.ElapsedTimems);

                GLStatics.ClearDepthBuffer();         // clear the depth buffer, so we are on top of all previous renders.
                displaycontrol.Render(glwfc.RenderState, ts);
            };


            GLBaseControl.Themer = MapThemer.Theme;

            // 3d controller

            gl3dcontroller = new Controller3D();
            gl3dcontroller.PosCamera.ZoomMax = 5000;   
            gl3dcontroller.ZoomDistance = 3000F / lyscale;
            gl3dcontroller.PosCamera.ZoomMin = 0.1f;
            gl3dcontroller.PosCamera.ZoomScaling = 1.1f;
            gl3dcontroller.YHoldMovement = true;
            gl3dcontroller.PaintObjects = Controller3DDraw;
            gl3dcontroller.KeyboardTravelSpeed = (ms, eyedist) =>
            {
                double eyedistr = Math.Pow(eyedist, 1.0);
                float v = (float)Math.Max(eyedistr / 2000, 0);
                // System.Diagnostics.Debug.WriteLine("Speed " + eyedistr + " "+ v);
                return (float)ms * v;
            };

            // start hooks the glwfc paint function up, first, so it gets to go first
            // No ui events from glwfc.
            gl3dcontroller.Start(matrixcalc, glwfc, new Vector3(0, 0, 0), new Vector3(140.75f, 0, 0), 0.5F, registermouseui: false, registerkeyui: false);

            if ((parts & Parts.Menu) != 0)
            {
                galaxymenu = new MapMenu(this, parts, userimages);
            }

            if ((parts & Parts.RightClick) != 0)
            {
                NLD rightclickmenunld = null;

                rightclickmenu = new GLContextMenu("RightClickMenu",true,
                    new GLMenuItemLabel("RCMTitle", "Context Menu"),
                    new GLMenuItemSeparator("RCMSepar", 5,5, ContentAlignment.MiddleCenter),
                    new GLMenuItem("RCMInfo", "Information")
                    {
                        MouseClick = (s, e) =>
                        {
                            if (e.Button == GLMouseEventArgs.MouseButtons.Left)
                            {
                                var bkm = rightclickmenu.Tag as EliteDangerousCore.DB.BookmarkClass;

                                GLFormConfigurable cfg = new GLFormConfigurable("Info");
                                cfg.Tag = "SolidBackground";
                                GLMultiLineTextBox tb = new GLMultiLineTextBox("MLT", new Rectangle(10, 10, 1000, 1000), rightclickmenunld.Description);
                                tb.Font = cfg.Font = displaycontrol.Font;                             // set the font up first, as its needed for config
                                var sizer = tb.CalculateTextArea(new Size(50, 24), new Size(displaycontrol.Width - 64, displaycontrol.Height - 64));
                                tb.Size = sizer.Item1;
                                tb.EnableVerticalScrollBar = true;
                                tb.EnableHorizontalScrollBar = sizer.Item2;
                                tb.CursorToEnd();
                                tb.BackColor = cfg.BackColor;
                                tb.ReadOnly = true;
                                tb.SetSelection(0, 0);
                                cfg.AddOK("OK");            // order important for tab control
                                cfg.AddButton("goto", "Goto", new Point(0, 0), anchor: AnchorType.AutoPlacement);
                                if (bkm != null)
                                    cfg.AddButton("edit", "Edit", new Point(0, 0), anchor: AnchorType.AutoPlacement);
                                cfg.Add("tb", tb);
                                cfg.Init(e.ViewportLocation, rightclickmenunld.DescriptiveName);
                                cfg.InstallStandardTriggers();
                                cfg.Trigger += (form, entry, name, tag) =>
                                {
                                    if (name == "goto")
                                    {
                                        gl3dcontroller.SlewToPositionZoom(rightclickmenunld.Location, 300, -1);
                                    }
                                    else if (name == "edit")
                                    {
                                        cfg.Close();
                                        EditBookmark(bkm);
                                    }
                                };
                                cfg.ResumeLayout();
                                displaycontrol.Add(cfg);
                                cfg.Moveable = true;
                            }
                        }
                    },
                    new GLMenuItem("RCMEditBookmark", "Edit Bookmark")
                    {
                        Click = (s1) =>
                        {
                            rightclickmenu.Visible = false;     // because its a winform dialog we are showing, the menu won't shut down during show
                                                                // so we set this to invisible (not close, won't work inside here)
                            var bkm = rightclickmenu.Tag as EliteDangerousCore.DB.BookmarkClass;

                            if (bkm == null)    // if find tag is not a bookmark
                            {
                                if (rightclickmenunld.SystemC != null )   // if it has an associated system (it must if its been disabled, but we will triage)
                                    bkm = EliteDangerousCore.DB.GlobalBookMarkList.Instance.FindBookmarkOnSystem(rightclickmenunld.SystemC.Name);   
                            }

                            var res = BookmarkHelpers.ShowBookmarkForm(parent.DiscoveryForm, parent.DiscoveryForm, null, bkm);
                            if (res)
                                UpdateBookmarks();

                        }
                    },
                    new GLMenuItem("RCMZoomIn", "Goto Zoom In")
                    {
                        Click = (s1) =>
                        {
                            gl3dcontroller.SlewToPositionZoom(rightclickmenunld.Location, 300, -1);
                        }
                    },
                    new GLMenuItem("RCMGoto", "Goto Position")
                    {
                        Click = (s1) =>
                        {
                            gl3dcontroller.SlewToPosition(rightclickmenunld.Location, -1);
                        }
                    },
                    new GLMenuItem("RCMLookAt", "Look At")
                    {
                        Click = (s1) =>
                        {
                            gl3dcontroller.PanTo(rightclickmenunld.Location, -1);
                        }
                    },
                    new GLMenuItem("RCMViewStarDisplay", "Display system")
                    {
                        Click = (s1) =>
                        {
                            ScanDisplayForm.ShowScanOrMarketForm(parent.FindForm(), rightclickmenunld.SystemC,  parent.DiscoveryForm.History, 0.8f, 
                                            System.Drawing.Color.Purple, WebExternalDataLookup.SpanshThenEDSM);
                        }
                    },
                    new GLMenuItem("RCMViewSpansh", "View on Spansh")
                    {
                        Click = (s1) =>
                        {
                            EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForSystem(rightclickmenunld.SystemC);
                        }
                    },
                    new GLMenuItem("RCMViewEDSM", "View on EDSM")
                    {
                        Click = (s1) =>
                        {
                            EDSMClass edsm = new EDSMClass();
                            if (!edsm.ShowSystemInEDSM(rightclickmenunld.SystemC.Name))
                                ExtendedControls.MessageBoxTheme.Show(parent.FindForm(), "System could not be found - has not been synched or EDSM is unavailable");
                        }
                    },
                    new GLMenuItem("RCMNewBookmark", "New Bookmark")
                    {
                        Click = (s1) =>
                        {
                            rightclickmenu.Visible = false;     // see above for this reason

                            var res = BookmarkHelpers.ShowBookmarkForm(parent.DiscoveryForm, parent.DiscoveryForm, rightclickmenunld.SystemC, null);

                            if ( res )      // if changed
                            {
                                UpdateBookmarks();
                            }
                        }
                    },
                    new GLMenuItem("RCMDeleteBookmark", "Delete Bookmark")
                    {
                        Click = (s1) =>
                        {
                            var bkm = rightclickmenu.Tag as EliteDangerousCore.DB.BookmarkClass;
                            if (bkm == null)
                            {
                                bkm = EliteDangerousCore.DB.GlobalBookMarkList.Instance.FindBookmarkOnSystem(rightclickmenunld.SystemC.Name);
                            }

                            if ( bkm != null )
                                DeleteBookmark(bkm);
                        }
                    },
                    new GLMenuItem("RCMAddExpedition", "Add to expedition")
                    {
                        Click = (s1) =>
                        {
                            AddSystemsToExpedition?.Invoke(new List<ISystem>() { rightclickmenunld.SystemC }, true);      // use call back to pass back up
                        }
                    },
                    new GLMenuItem("RCMAddExpeditionCont", "Add to expedition (don't swap to)")
                    {
                        Click = (s1) =>
                        {
                            AddSystemsToExpedition?.Invoke(new List<ISystem>() { rightclickmenunld.SystemC }, false);      // use call back to pass back up
                        }
                    },
                    new GLMenuItem("RCMCopyName", "Copy name to Clipboard")
                    {
                        Click = (s1) =>
                        {
                            parent.SetClipboardText(rightclickmenunld.SystemC.Name);
                        }
                    }
                );

                rightclickmenu.Opening += (ms,opentag) =>
                {
                    rightclickmenu.Size = new Size(10, 10);     // reset to small, and let the autosizer work again as we are going to change content

                    // Tag is finder tag, turn into information

                    rightclickmenunld = NameLocationDescription(rightclickmenu.Tag, parent.DiscoveryForm.History.GetLast);

                    ((GLMenuItemLabel)ms["RCMTitle"]).Text = rightclickmenunld.DescriptiveName + " " + rightclickmenunld.Location.ToString() + (rightclickmenunld.PermitRequired  ? " Permit Required":"");

                    ms["RCMAddExpeditionCont"].Visible = ms["RCMAddExpedition"].Visible = ms["RCMViewStarDisplay"].Visible = ms["RCMViewEDSM"].Visible = ms["RCMViewSpansh"].Visible =
                    ms["RCMNewBookmark"].Visible = ms["RCMCopyName"].Visible = rightclickmenunld.SystemC != null;

                    ms["RCMDeleteBookmark"].Visible = ms["RCMEditBookmark"].Visible = rightclickmenunld.SystemC != null ? (EliteDangerousCore.DB.GlobalBookMarkList.Instance.FindBookmarkOnSystem(rightclickmenunld.SystemC.Name) != null) : false;

                    System.Diagnostics.Debug.WriteLine($"Right click on {rightclickmenunld.DescriptiveName} at system {rightclickmenunld.SystemC?.Name}");
                };
            }

            // Autocomplete text box at top for searching

            GLTextBoxAutoComplete tbac = galaxymenu?.EntryTextBox;
            if (tbac != null)
            {
                tbac.PerformAutoCompleteInThread = (s, obj, set) =>       // in the autocomplete thread, so EDSM lookup
                {
                    System.Diagnostics.Debug.WriteLine($"Autocomplete look up EDSM systems on {s}");
                    EliteDangerousCore.SystemCache.ReturnSystemAutoCompleteList(s, obj, set);      // perform the system cache autocomplete

                    foreach( var gmo in galacticmapping.AllObjects)
                    {
                        foreach( var name in gmo.DescriptiveNames)
                        {
                            if (s.Length < 3 ? name.StartsWith(s, StringComparison.InvariantCultureIgnoreCase) : name.Contains(s, StringComparison.InvariantCultureIgnoreCase))
                                set.Add(name);
                        }
                    }
                };

                tbac.PerformAutoCompleteInUIThread = (s, obj, set) =>    // complete autocomplete in UI thread, for parts declared in UI, run second
                {
                    System.Diagnostics.Debug.Assert(Application.MessageLoop);
                    System.Diagnostics.Debug.WriteLine($"Autocomplete look up GMO/Travel systems on {s}");

                    List<string> list = new List<string>();
                    if (travelpath?.CurrentListSystem != null)
                        list.AddRange(travelpath.CurrentListSystem.Where(x => s.Length < 3 ? x.Name.StartsWith(s, StringComparison.InvariantCultureIgnoreCase) : x.Name.Contains(s, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Name));
                    if (routepath?.CurrentListSystem != null)
                        list.AddRange(routepath.CurrentListSystem.Where(x => s.Length < 3 ? x.Name.StartsWith(s, StringComparison.InvariantCultureIgnoreCase) : x.Name.Contains(s, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Name));
                    if (navroute?.CurrentListSystem != null)
                        list.AddRange(navroute.CurrentListSystem.Where(x => s.Length < 3 ? x.Name.StartsWith(s, StringComparison.InvariantCultureIgnoreCase) : x.Name.Contains(s, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Name));

                    foreach (var x in list)
                        set.Add(x);
                };

                tbac.SelectedEntry = (a) =>     // in UI thread
                {
                    System.Diagnostics.Debug.Assert(Application.MessageLoop);       // must be in UI thread
                    System.Diagnostics.Debug.WriteLine("Selected " + tbac.Text);
                    var gmo = galacticmapping?.FindDescriptiveNameOrSystem(tbac.Text,true);

                    Vector3? pos = null;
                    if (gmo != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Move to gmo " + gmo.Points[0]);
                        pos = new Vector3((float)gmo.Points[0].X, (float)gmo.Points[0].Y, (float)gmo.Points[0].Z);
                    }
                    else
                    {
                        var isys = travelpath?.CurrentListSystem?.Find(x => x.Name.Equals(tbac.Text, StringComparison.InvariantCultureIgnoreCase));
                        if (isys == null)
                            isys = routepath?.CurrentListSystem?.Find(x => x.Name.Equals(tbac.Text, StringComparison.InvariantCultureIgnoreCase));
                        if (isys == null)
                            isys = navroute?.CurrentListSystem?.Find(x => x.Name.Equals(tbac.Text, StringComparison.InvariantCultureIgnoreCase));
                        if (isys == null)
                            isys = EliteDangerousCore.SystemCache.FindSystem(tbac.Text);     // final chance, the system DB

                        if (isys != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Move to sys " + isys.Name);
                            pos = new Vector3((float)isys.X, (float)isys.Y, (float)isys.Z);
                        }
                        else
                            tbac.InErrorCondition = true;
                    }

                    if ( pos != null )
                    {
                        gl3dcontroller.SlewToPosition(pos.Value, -1);

                        if ((parts & Parts.PrepopulateGalaxyStarsLocalArea) != 0)
                        {
                            galaxystars.Request9x3Box(pos.Value);
                        }
                    }
                };
            }

            if (galaxystars != null)
            {
                galaxystars.Start();
            }

            System.Diagnostics.Debug.Assert(glwfc.IsContextCurrent());

            string shaderlog = GLShaderLog.ShaderLog;
            if (shaderlog.HasChars())
            {
                var inf = new ExtendedControls.InfoForm();
                string text = "";
                text += $"Version  {GLStatics.GetVersion()}"+ Environment.NewLine;
                text += $"Version  {GLStatics.GetVersionString()}"+ Environment.NewLine;
                text += $"Vendor  {GLStatics.GetVendor()}"+ Environment.NewLine;
                text += $"Shading lang {GLStatics.GetShaderLanguageVersion()}"+ Environment.NewLine;
                text += $"Shading lang {GLStatics.GetShadingLanguageVersionString()}"+ Environment.NewLine;
                text += $"UBS={GLOFC.GL4.GL4Statics.GetMaxUniformBlockSize()}" + Environment.NewLine;
                GLOFC.GL4.GL4Statics.GetMaxUniformBuffers(out int vertex, out int fragfment, out int geo, out int tesscontrol, out int tesseval);
                text += $"UB v{vertex} f{fragfment} g{geo} tc{tesscontrol} te{tesseval}"+ Environment.NewLine;
                text += $"tex layers {GLOFC.GL4.GL4Statics.GetMaxTextureDepth()} "+ Environment.NewLine;
                text += $"Vertex attribs {GLOFC.GL4.GL4Statics.GetMaxVertexAttribs()} "+ Environment.NewLine;
                text += $"Shader storage buffer bindings {GLOFC.GL4.GL4Statics.GetShaderStorageMaxBindingNumber()} "+ Environment.NewLine;

                text += shaderlog;

                inf.Info("Shader log - report to EDD team", Properties.Resources.edlogo_3mo_icon, text);
                inf.Show();
            }

            // change - we now wait until every is set up before we hook to get user events.  If a click comes in too soon, we may not be ready

            System.Diagnostics.Debug.WriteLine($"Map created, hook events");

            gl3dcontroller.Hook(displaycontrol, glwfc); // we get 3dcontroller events from displaycontrol, so it will get them when everything else is unselected
            displaycontrol.Hook();  // now we hook up display control to glwin, and paint
            displaycontrol.MouseClick += MouseClickOnMap;       // grab mouse UI

            System.Diagnostics.Debug.WriteLine($"Map create finished");
            // finished
            mapcreatedokay = GLShaderLog.Okay;      // record shader status

            displaycontrol.ClearLayoutFlags();
            displaycontrol.InvalidateLayout();
            return mapcreatedokay;
        }
        #endregion

        #region Display

        // Context is set.
        public void Systick()
        {
            System.Diagnostics.Debug.Assert(glwfc.IsContextCurrent());

            gl3dcontroller.HandleKeyboardSlews(true, OtherKeys);
            gl3dcontroller.RecalcMatrixIfMoved();
            glwfc.Invalidate();
        }

        int lastgridwidth = 100;

        // Context is set.
        private void Controller3DDraw(Controller3D c3d, ulong time)
        {
            if (!mapcreatedokay)
                return;

            //System.Diagnostics.Debug.WriteLine($"Controller 3d draw");
            System.Diagnostics.Debug.Assert(glwfc.IsContextCurrent());

            GLMatrixCalcUniformBlock mcb = ((GLMatrixCalcUniformBlock)items.UB("MCUB"));
            mcb.SetFull(gl3dcontroller.MatrixCalc);        // set the matrix unform block to the controller 3d matrix calc.

            // set up the grid shader size

            if (gridshader != null && gridshader.Enable)
            {
                // set the dynamic grid properties

                var vertshader = gridshader.GetShader<DynamicGridVertexShader>(ShaderType.VertexShader);

                gridrenderable.InstanceCount = vertshader.ComputeGridSize(gl3dcontroller.MatrixCalc.LookAt.Y, gl3dcontroller.MatrixCalc.EyeDistance, out lastgridwidth);

                vertshader.SetUniforms(gl3dcontroller.MatrixCalc.LookAt, lastgridwidth, gridrenderable.InstanceCount);

                // set the coords fader

                float coordfade = lastgridwidth == 10000 ? (0.7f - (c3d.MatrixCalc.EyeDistance / 20000).Clamp(0.0f, 0.7f)) : 0.7f;
                Color coordscol = Color.FromArgb(coordfade < 0.05 ? 0 : 150, Color.Cyan);
                var tvshader = gridtextshader.GetShader<DynamicGridCoordVertexShader>(ShaderType.VertexShader);
                tvshader.ComputeUniforms(lastgridwidth, gl3dcontroller.MatrixCalc, gl3dcontroller.PosCamera.CameraDirection, coordscol, Color.Transparent);
            }

            //// set the galaxy volumetric block

            if (galaxyrenderable != null && galaxyshader.Enable)
            {
                galaxyrenderable.InstanceCount = volumetricblock.Set(gl3dcontroller.MatrixCalc, volumetricboundingbox, gl3dcontroller.MatrixCalc.InPerspectiveMode ? 50.0f : 0);        // set up the volumentric uniform
                //System.Diagnostics.Debug.WriteLine("GI {0}", galaxyrendererable.InstanceCount);
                galaxyshader.SetFader(gl3dcontroller.MatrixCalc.EyePosition, gl3dcontroller.MatrixCalc.EyeDistance, gl3dcontroller.MatrixCalc.InPerspectiveMode);
            }

            if (travelpath != null)
                travelpath.Update(time, gl3dcontroller.MatrixCalc.EyeDistance);
            if (routepath != null)
                routepath.Update(time, gl3dcontroller.MatrixCalc.EyeDistance);
            if (navroute != null)
                navroute.Update(time, gl3dcontroller.MatrixCalc.EyeDistance);

            if (galmapobjects != null && galmapobjects.Enable)
                galmapobjects.Update(time, gl3dcontroller.MatrixCalc.EyeDistance);

            if (galaxystars != null)
            {
                if (galaxystars.EnableMode > 0)      // enable mode must be on to show something
                {
                    // if auto update, and eyedistance close, and db is okay, try it

                    if ((parts & Parts.AutoEDSMStarsUpdate) != 0 && gl3dcontroller.MatrixCalc.EyeDistance < 400 &&
                                    EliteDangerousCore.DB.SystemsDatabase.Instance.DBUpdating == false)
                    {
                        galaxystars.Request9x3BoxConditional(gl3dcontroller.PosCamera.LookAt);
                    }

                    galaxystars.Update(time, gl3dcontroller.MatrixCalc.EyeDistance);
                }

                if ( galaxymenu != null )
                    galaxymenu.DBStatus.Visible = galaxystars.DBActive;     // always indicate DB Active status

            }

            rObjects.Render(glwfc.RenderState, gl3dcontroller.MatrixCalc, verbose: false);

            GL.Flush(); // ensure everything is in the grapghics pipeline
        }

        #endregion

    }
}
