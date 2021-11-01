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

using EliteDangerousCore;
using EliteDangerousCore.DB;
using GLOFC;
using GLOFC.GL4;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Threading;

namespace EDDiscovery.UserControls.Map3D
{
    class GalaxyStars
    {
        public Vector3 CurrentPos { get; set; } = new Vector3(-1000000, -1000000, -1000000);
        public Font Font { get; set; } = new Font("Arial", 8.5f);
        public Size BitMapSize { get; set; } = new Size(96, 30);        // 30 is enough for two lines at 8.5f
        public Color ForeText { get; set; } = Color.FromArgb(255,220,220,220);
        public Color BackText { get; set; } = Color.Transparent;
        public Vector3 LabelSize { get; set; } = new Vector3(5, 0, 5f/4f);
        public Vector3 LabelOffset { get; set; } = new Vector3(0, -1.2f, 0);
        // 0 = off, bit 0= stars, bit1 = labels
        public int EnableMode { get { return enablemode; } set { enablemode = value; sunshader.Enable = (enablemode & 1) != 0; textshader.Enable = enablemode==3; } }
        public int MaxObjectsAllowed { get; set; } = 100000;
        public bool UseObjectLimit { get; set; } = true;
        public bool DBActive { get { return subthreadsrunning > 0; ; } }
        public bool ShowDistance { get; set; } = false;     // at the moment, can't use it, due to clashing with travel path stars

        public void Create(GLItemsList items, GLRenderProgramSortedList rObjects, float sunsize, GLStorageBlock findbufferresults)
        {
            sunvertex = new GLPLVertexShaderModelCoordWithWorldTranslationCommonModelTranslation(new Color[] { Color.FromArgb(255, 220, 220, 10), Color.FromArgb(255, 0,0,0) },
                 autoscale: 30, autoscalemin: 1, autoscalemax: 2, useeyedistance:false);
            sunshader = items.NewShaderPipeline(null, sunvertex, new GLPLStarSurfaceFragmentShader());

            shapebuf = items.NewBuffer(false);
            var shape = GLSphereObjectFactory.CreateSphereFromTriangles(2, sunsize);
            shapebuf.AllocateFill(shape);

            GLRenderState starrc = GLRenderState.Tri();     // render is triangles, with no depth test so we always appear
            starrc.DepthTest = true;
            starrc.DepthClamp = true;

            var textrc = GLRenderState.Quads();
            textrc.DepthTest = true;
            textrc.ClipDistanceEnable = 1;  // we are going to cull primitives which are deleted

            int texunitspergroup = 16;
            textshader = items.NewShaderPipeline(null, new GLPLVertexShaderQuadTextureWithMatrixTranslation(), new GLPLFragmentShaderTexture2DIndexedMulti(0, 0, true, texunitspergroup));

            slset = new GLSetOfObjectsWithLabels("SLSet", rObjects, texunitspergroup, 100, 10,
                                                            sunshader, shapebuf, shape.Length, starrc, OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles,
                                                            textshader, BitMapSize, textrc, SizedInternalFormat.Rgba8);

            items.Add(slset);

            var geofind = new GLPLGeoShaderFindTriangles(findbufferresults, 16);
            findshader = items.NewShaderPipeline(null, sunvertex, null, null, geofind, null, null, null);
        }

        public void Start()
        {
            requestorthread = new Thread(Requestor);
            requestorthread.Start();
        }

        public void Stop()
        {
            //System.Diagnostics.Debug.WriteLine("Request stop on gal stars");
            stop.Cancel();
            requestorthread.Join();
            while(subthreadsrunning > 0)
            {
                System.Diagnostics.Debug.WriteLine("Sub thread running");
                Thread.Sleep(100);
            }
            System.Diagnostics.Debug.WriteLine("Stopped on gal stars");

            while (cleanbitmaps.TryDequeue(out Sector sectoclean))
            {
                System.Diagnostics.Debug.WriteLine($"Final Clean bitmap for {sectoclean.pos}");
                BitMapHelpers.Dispose(sectoclean.bitmaps);
                sectoclean.bitmaps = null;
            }
        }

        public void Request9x3BoxConditional(Vector3 newpos)
        {
            // if out of pos, not too many requestes, and rebuild is not running
            if ((CurrentPos - newpos).Length >= SectorSize && requestedsectors.Count < MaxRequests )
            {
                Request9x3Box(newpos);
            }
        }

        public void Request9x3Box(Vector3 pos)
        {
            CurrentPos = pos;
            //System.Diagnostics.Debug.WriteLine($"Request 9 box ${pos}");

            for (int i = 0; i <= 2; i++)
            {
                int y = i == 0 ? 0 : i == 1 ? SectorSize : -SectorSize;
                RequestBox(new Vector3(pos.X , pos.Y + y, pos.Z));
                RequestBox(new Vector3(pos.X + SectorSize, pos.Y + y, pos.Z));
                RequestBox(new Vector3(pos.X - SectorSize, pos.Y + y, pos.Z));
                RequestBox(new Vector3(pos.X, pos.Y+y, pos.Z + SectorSize));
                RequestBox(new Vector3(pos.X, pos.Y + y, pos.Z - SectorSize));
                RequestBox(new Vector3(pos.X + SectorSize, pos.Y + y, pos.Z + SectorSize));
                RequestBox(new Vector3(pos.X + SectorSize, pos.Y + y, pos.Z - SectorSize));
                RequestBox(new Vector3(pos.X - SectorSize, pos.Y + y, pos.Z + SectorSize));
                RequestBox(new Vector3(pos.X - SectorSize, pos.Y + y, pos.Z - SectorSize));
            }
            //System.Diagnostics.Debug.WriteLine($"End 9 box");
        }

        // request a box around pos (nominalised to sectorsize) and if not already requested, send the request to the requestor using a blocking queue
        public void RequestBox(Vector3 pos)
        {
            int mm = 100000 + SectorSize / 2;
            pos.X = (int)(pos.X + mm) / SectorSize * SectorSize - mm;
            pos.Y = (int)(pos.Y + mm) / SectorSize * SectorSize - mm;
            pos.Z = (int)(pos.Z + mm) / SectorSize * SectorSize - mm;

            if (!slset.TagsToBlocks.ContainsKey(pos))
            {
                slset.ReserveTag(pos);      // important, stops repeated adds in the situation where it takes a while to add to set

                //System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 100000} {pos} request");
                requestedsectors.Add(new Sector(pos, SectorSize));
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 100000} {pos} request rejected");
            }
        }
        public void RequestBox(Vector3 pos, int size)   // pos is the centre
        {
            slset.ReserveTag(pos);      // unconditional reserve of this tag
            CurrentPos = pos;
            //System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 100000} {pos} request");
            requestedsectors.Add(new Sector(new Vector3(pos.X-size/2,pos.Y-size/2,pos.Z-size/2), size));
        }

        // clear all objects
        public void Clear()
        {
            slset.RemoveUntil(0);
            CurrentPos = new Vector3(-1000000, -1000000, -1000000);     //reset pos
        }

        // do this in a thread, as adding threads is computationally expensive so we don't want to do it in the foreground
        private void Requestor()
        {
            while (true)
            {
                try
                {
                    //  System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 100000} Requestor take");
                    var sector = requestedsectors.Take(stop.Token);       // blocks until take or told to stop

                    do
                    {
                        // reduce memory use first by bitmap cleaning 

                        lock (cleanbitmaps)     // foreground also does this if required
                        {
                            while (cleanbitmaps.TryDequeue(out Sector sectoclean))
                            {
                                //System.Diagnostics.Debug.WriteLine($"Clean bitmap for {sectoclean.pos}");
                                BitMapHelpers.Dispose(sectoclean.bitmaps);
                                sectoclean.bitmaps = null;
                            }
                        }

                        //System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 100000} {sector.pos} requestor accepts");

                        Interlocked.Add(ref subthreadsrunning, 1);      // committed to run, and count subthreads

                        Thread p = new Thread(FillSectorThread);
                        p.Start(sector);

                        while (subthreadsrunning >= MaxSubthreads)      // pause the take if we have too much work going on
                            Thread.Sleep(100);

                    } while (requestedsectors.TryTake(out sector));     // until empty..

                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
            //System.Diagnostics.Debug.WriteLine("Exit requestor");
        }

        // in a thread, look up the sector 
        private void FillSectorThread(Object seco)
        {
            Sector d = (Sector)seco;

            //System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 100000} {d.pos} {tno} start");

            // note d.text/d.positions may be much longer than d.systems

            if (ShowDistance)
            {
                Vector4 pos = new Vector4(d.pos.X+d.searchsize/2, d.pos.Y+d.searchsize/2, d.pos.Z+d.searchsize/2, 0);       // from centre of box

                d.systems = SystemsDB.GetSystemList(d.pos.X, d.pos.Y, d.pos.Z, d.searchsize, ref d.text, ref d.positions,
                    (x, y, z) => { return new Vector4((float)x / SystemClass.XYZScalar, (float)y / SystemClass.XYZScalar, (float)z / SystemClass.XYZScalar, 0); },
                    (v, s) => { var dist = (pos - v).Length; return s + $" @ {dist:0.#}ly"; });
            }
            else
            {
                d.systems = SystemsDB.GetSystemList(d.pos.X, d.pos.Y, d.pos.Z, d.searchsize, ref d.text, ref d.positions,
                                        (x, y, z) => { return new Vector4((float)x / SystemClass.XYZScalar, (float)y / SystemClass.XYZScalar, (float)z / SystemClass.XYZScalar, 0); },
                                        null);
            }

            if (d.systems > 0)      // may get nothing, so don't do this if so
            {
                // note only draw d.systems
                using (StringFormat fmt = new StringFormat())
                {
                    fmt.Alignment = StringAlignment.Center;

                    d.bitmaps = BitMapHelpers.DrawTextIntoFixedSizeBitmaps(slset.LabelSize, d.text, Font, System.Drawing.Text.TextRenderingHint.ClearTypeGridFit,
                                            ForeText, BackText, 0.5f, frmt: fmt, length: d.systems);
                }

                d.textpos = GLPLVertexShaderQuadTextureWithMatrixTranslation.CreateMatrices(d.positions, LabelOffset,  //offset
                                                                            LabelSize, //size
                                                                            new Vector3(0, 0, 0), // rot (unused due to below)
                                                                            true, false, // rotate, no elevation
                                                                            length: d.systems    // limit length
                                                                            );
            }

            generatedsectors.Enqueue(d);       // d has been filled
            //System.Diagnostics.Debug.WriteLine($"{Environment.TickCount % 100000} {d.pos} {tno} {d.systems} end");

            Interlocked.Add(ref subthreadsrunning, -1);
        }

        ulong timelastchecked = 0;
        ulong timelastbitmapcheck = 0;

        // foreground, called on each frame, allows update of shader and queuing of new objects
        public void Update(ulong time, float eyedistance)
        {
            if (time - timelastchecked > 50)
            {
                if (generatedsectors.Count > 0)
                {
                    int max = 2;
                    while (max-- > 0 && generatedsectors.TryDequeue(out Sector d) )      // limit fill rate.. (max first)
                    {
                        //System.Diagnostics.Debug.WriteLine($"Add {d.pos} number {d.systems} total {slset.Objects}");
                        if (d.systems > 0)      // may return zero
                        { 
                            slset.Add(d.pos, d.text, d.positions, d.textpos, d.bitmaps, 0, d.systems);
                            cleanbitmaps.Enqueue(d);            // ask for cleaning of these bitmaps
                        }

                        d.positions = null;     // don't need these
                        d.textpos = null;           // and these are not needed

                        //System.Diagnostics.Debug.WriteLine($"..add complete {d.pos} {slset.Objects}" );
                    }
                }

                if ( UseObjectLimit && slset.Objects > MaxObjectsAllowed )       // if set, and active
                {
                    slset.RemoveUntil(MaxObjectsAllowed-MaxObjectsMargin);
                }
                timelastchecked = time;
            }

            if ( time - timelastbitmapcheck > 60000)    // only do this infrequently
            {
                lock (cleanbitmaps)     // foreground also does this if required
                {
                    while (cleanbitmaps.TryDequeue(out Sector sectoclean))
                    {
                        //System.Diagnostics.Debug.WriteLine($"Foreground Clean bitmap for {sectoclean.pos}");
                        BitMapHelpers.Dispose(sectoclean.bitmaps);
                        sectoclean.bitmaps = null;
                    }
                }

                timelastbitmapcheck = time;
            }

            const int rotperiodms = 10000;
            time = time % rotperiodms;
            float fract = (float)time / rotperiodms;
            float angle = (float)(2 * Math.PI * fract);
            sunvertex.ModelTranslation = Matrix4.CreateRotationY(-angle);
            float scale = Math.Max(1, Math.Min(4, eyedistance / 5000));
            //     System.Diagnostics.Debug.WriteLine("Scale {0}", scale);
            sunvertex.ModelTranslation *= Matrix4.CreateScale(scale);           // scale them a little with distance to pick them out better
        }

        // returns name only, and z - if not found z = Max value, null
        public SystemClass Find(Point loc, GLRenderState rs, Size viewportsize, out float z)
        {
            z = float.MaxValue;

            if (sunshader.Enable)
            {
                var find = slset.FindBlock(findshader, rs, loc, viewportsize);      // return block tag, index, z
                if (find != null)
                {
                    z = find.Item5;
                    var userdata = slset.UserData[find.Item1[0].tag] as string[];
                    System.Diagnostics.Debug.WriteLine($"SLSet {find.Item2} {find.Item3} {find.Item4} {find.Item5} {userdata[find.Item2]}");

                    string name = userdata[find.Item4];
                    int atsign = name.IndexOf(" @");        // remove any information denoted by space @
                    if (atsign>=0)
                        name = name.Substring(0, atsign);
                    return new SystemClass() { Name = name };       // without position note
                }
            }

            return null;
        }



        private GLSetOfObjectsWithLabels slset; // main class holding drawing

        private GLShaderPipeline sunshader;     // sun drawer
        private GLShaderPipeline textshader;     // text shader
        private GLPLVertexShaderModelCoordWithWorldTranslationCommonModelTranslation sunvertex;
        private GLBuffer shapebuf;

        private GLShaderPipeline findshader;    // find shader for lookups

        private class Sector
        {
            public Vector3 pos;             // position
            public int searchsize;          // and size.. nominally SectorSize, but for local map its the search size
            public Sector(Vector3 pos, int size) { this.pos = pos; searchsize = size; }

            // generated by thread, passed to update, bitmaps pushed to cleanbitmaps and deleted by requestor
            public int systems;
            public Vector4[] positions;
            public string[] text;
            public Matrix4[] textpos;
            public Bitmap[] bitmaps;
        }

        // requested sectors from foreground to requestor
        private BlockingCollection<Sector> requestedsectors = new BlockingCollection<Sector>();

        // added to by subthread when sector is ready, picked up by foreground update. ones ready for final foreground processing
        private ConcurrentQueue<Sector> generatedsectors = new ConcurrentQueue<Sector>();

        // added to by update when cleaned up bitmaps, requestor will clear these for it
        private ConcurrentQueue<Sector> cleanbitmaps = new ConcurrentQueue<Sector>();

        private Thread requestorthread;
        private CancellationTokenSource stop =  new CancellationTokenSource();
        private int subthreadsrunning = 0;

        private int enablemode = 3;
        private const int MaxObjectsMargin = 1000;
        private const int SectorSize = 100;
        private const int MaxRequests = 27 * 2;
        private const int MaxSubthreads = 16;
    }

}
//}
