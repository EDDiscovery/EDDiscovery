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

using OpenTK;
using GLOFC.GL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EliteDangerousCore;
using GLOFC.GL4.Shaders.Fragment;
using GLOFC.GL4.Shaders;
using GLOFC.GL4.Shaders.Vertex;
using GLOFC.GL4.Shaders.Stars;
using GLOFC.GL4.Shaders.Geo;
using GLOFC.GL4.Bitmaps;
using GLOFC.GL4.ShapeFactory;
using GLOFC.GL4.Textures;

namespace EDDiscovery.UserControls.Map3D
{
    class TravelPath
    {
        public List<ISystem> CurrentListSystem { get { return currentfilteredlistsys; } }       // always
        public List<HistoryEntry> CurrentListHE { get { return currentfilteredlisthe; } }      // if created with HEs
        public HistoryEntry CurrentSystemHE { get { return currentfilteredlisthe != null && CurrentPos != -1 ? currentfilteredlisthe[CurrentPos] : null; } }
        public ISystem CurrentSystem { get { return currentfilteredlistsys != null && CurrentPos != -1 ? currentfilteredlistsys[CurrentPos] : null; } }
        public ISystem LastSystem {  get { return currentfilteredlistsys.LastOrDefault(); } }
        public int CurrentPos { get; private set; } = -1;                                         // -1 no system, else index

        public bool EnableTape { get { return tapeshader.Enable; } set { tapeshader.Enable = value; } }
        public bool EnableText { get { return textrenderer.Enable; } set { textrenderer.Enable = value; } }
        public bool EnableStars { get { return sunshader.Enable; } set { sunshader.Enable = value; } }
        public int MaxStars { get; private set; }
        public DateTime TravelPathStartDateUTC { get; set; } = EDDConfig.GameLaunchTimeUTC();
        public DateTime TravelPathEndDateUTC { get; set; } = DateTime.UtcNow.AddMonths(1);
        public bool TravelPathStartDateEnable { get; set; } = false;
        public bool TravelPathEndDateEnable { get; set; } = false;
        public Font Font { get; set; } = new Font("Arial", 8.5f);
        public Color ForeText { get; set; } = Color.White;
        public Color BackText { get; set; } = Color.Transparent;
        public Vector3 LabelSize { get; set; } = new Vector3(5, 0, 5f/4f);
        public Vector3 LabelOffset { get; set; } = new Vector3(0, -1.2f, 0);
        public Size TextBitMapSize { get; set; } = new Size(160, 16);

        public void Start(string name, int maxstars, float sunsize, float tapesize, GLStorageBlock bufferfindresults, Tuple<GLTexture2DArray, long[]> starimagearrayp, bool depthtest, GLItemsList items, GLRenderProgramSortedList rObjects)
        {
            this.MaxStars = maxstars;
            this.tapesize = tapesize;
            this.sunsize = sunsize;
            this.starimagearray = starimagearrayp;

            // first the tape
            {

                var tapetex = new GLTexture2D(BaseUtils.Icons.IconSet.GetBitmap("GalMap.chevron"), internalformat: OpenTK.Graphics.OpenGL4.SizedInternalFormat.Rgba8);        // tape image
                items.Add(tapetex);
                tapetex.SetSamplerMode(OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat, OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat);

                // configure the fragger, set the replacement color, and set the distance where the replacement color is used for all pixels
                tapefrag = new GLPLFragmentShaderTextureTriStripColorReplace(1, Color.FromArgb(255, 206, 0, 0), 1000);
                // create the vertex shader with the autoscale required
                var vert = new GLPLVertexShaderWorldTextureTriStripNorm(100, 1, 10000);
                vert.SetWidth(tapesize);        // set the nominal tape width

                tapeshader = new GLShaderPipeline(vert, tapefrag);
                items.Add(tapeshader);

                GLRenderState rts = GLRenderState.Tri(OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedByte, cullface: false);        // set up a Tri strip, Default primitive restart
                rts.DepthTest = depthtest;  // no depth test so always appears

                // now the renderer, set up with the render control, tape as the points, and bind a RenderDataTexture so the texture gets binded each time

                var zerotape = new Vector4[] { Vector4.Zero };      // just use an dummy array to get this going
                ritape = GLRenderableItem.CreateVector4Vector4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.TriangleStrip, rts, zerotape, zerotape, new GLRenderDataTexture(tapetex));

                tapepointbuf = items.LastBuffer();  // keep buffer for refill

                ritape.ElementBuffer = items.NewBuffer();       // empty buffer for element index for now
                ritape.Visible = false;     // until its filled, not visible (important, we don't want render to execute unless its been fully set up below)

                rObjects.Add(tapeshader, name + "-tape", ritape);   // add render to object list
            }

            // now the stars
            {
                starposbuf = items.NewBuffer();         // where we hold the vertexes for the suns, used by renderer and by finder

                // globe shape
                var shape = GLSphereObjectFactory.CreateTexturedSphereFromTriangles(2, sunsize);

                // globe vertex
                starshapebuf = new GLBuffer();
                items.Add(starshapebuf);
                starshapebuf.AllocateFill(shape.Item1);

                // globe tex coord
                startexcoordbuf = new GLBuffer();
                items.Add(startexcoordbuf);
                startexcoordbuf.AllocateFill(shape.Item2);

                // the sun shader
                sunvertexshader = new GLPLVertexShaderModelWorldTextureAutoScale(autoscale: 50, autoscalemin: 1f, autoscalemax: 50f, useeyedistance: false);
                var sunfragmenttexture = new GLPLFragmentShaderTexture2DWSelectorSunspot();
                sunshader = new GLShaderPipeline(sunvertexshader, sunfragmenttexture);
                items.Add(sunshader);

                GLRenderDataTexture rdt = new GLRenderDataTexture(starimagearray.Item1);  // RDI is used to attach the texture

                GLRenderState starrc = GLRenderState.Tri();     // render is triangles
                starrc.DepthTest = depthtest;
                starrc.DepthClamp = true;
                starrc.ClipDistanceEnable =1;       // and we enable clipping and culling

                renderersun = GLRenderableItem.CreateVector4Vector2Vector4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, starrc,
                                             starshapebuf, 0, shape.Item1.Length,
                                             startexcoordbuf, 0,
                                             starposbuf, 0,
                                             rdt, 
                                             0,         // don't know at this point the instance count
                                             1);        // divide starposbuf into instances

                renderersun.Visible = false;            // until its filled, not visible

                rObjects.Add(sunshader, name + "-suns", renderersun);
            }

            // find compute, find shader has obey culling enabled!
            {
                var geofind = new GLPLGeoShaderFindTriangles(bufferfindresults, 32768);
                findshader = items.NewShaderPipeline(null, sunvertexshader, null, null, geofind, null, null, null);
            }

            // Sun names, handled by textrenderer
            {
                textrenderer = new GLBitmaps(name + "-text", rObjects, TextBitMapSize, depthtest: depthtest, cullface: false);
                items.Add(textrenderer);
            }
        }

        // tested to 50K+ stars
        public void CreatePath(HistoryList hl, HashSet<GalMapObjects.ObjectPosXYZ> nosunlist)
        {
            lasthl = hl;
            ISystem lastone = CurrentPos != -1 && CurrentPos < currentfilteredlistsys.Count ? currentfilteredlistsys[CurrentPos] : null;  // see if lastpos is there, and store it

            // create current filter list..
            currentfilteredlisthe = hl.FilterByTravelTimeAndMulticrew(TravelPathStartDateEnable ? TravelPathStartDateUTC : default(DateTime?), TravelPathEndDateEnable ? TravelPathEndDateUTC : default(DateTime?), true);

            if (currentfilteredlisthe.Count > MaxStars)
                currentfilteredlisthe = currentfilteredlisthe.Skip(currentfilteredlisthe.Count - MaxStars).ToList();

            currentfilteredlistsys = currentfilteredlisthe.Select(x => (ISystem)x.System).ToList();

            CurrentPos = lastone == null ? -1 : currentfilteredlistsys.IndexOf(lastone);        // may be -1, may have been removed

            CreatePathInt(null,nosunlist);
        }

        public void CreatePath(List<ISystem> syslist, Color tapecolour, HashSet<GalMapObjects.ObjectPosXYZ> nosunlist)
        {
            lasthl = null;
            ISystem lastone = CurrentPos != -1 && CurrentPos < currentfilteredlistsys.Count ? currentfilteredlistsys[CurrentPos] : null;  // see if lastpos is there, and store it

            // create current filter list..
            currentfilteredlisthe = null;
            currentfilteredlistsys = syslist;

            CurrentPos = lastone == null ? -1 : currentfilteredlistsys.IndexOf(lastone);        // may be -1, may have been removed

            CreatePathInt(tapecolour,nosunlist);
        }

        // currentfilteredlist set, go.
        private void CreatePathInt(Color? tapepathdefault = null, HashSet<GalMapObjects.ObjectPosXYZ> nosunlist = null)
        {
            if (!tapeshader.Compiled || !sunshader.Compiled)
                return;

            // Note W here selects the colour index of the stars, 0 = first, 1 = second etc

            Vector4[] positionsv4 = currentfilteredlistsys.Select(sys => new Vector4((float)sys.X, (float)sys.Y, (float)sys.Z,
                            (nosunlist?.Contains(new GalMapObjects.ObjectPosXYZ(sys.X, sys.Y, sys.Z))??false) ? -1 : starimagearray.Item2[(int)sys.MainStarType])).ToArray();

            Color[] color = new Color[currentfilteredlistsys.Count];

            if (currentfilteredlisthe != null)
            {
                for (int i = 0; i < positionsv4.Length; i++)        // if we are on a jump colour entry, then pick up its colour, otherwise use the last, unless its at the beginning
                {
                    var je = currentfilteredlisthe[i].journalEntry as IJournalJumpColor;
                    if (je != null)
                        color[i] = je.MapColorARGB;
                    else
                        color[i] = i > 0 ? color[i - 1] : Color.Green;
                }
            }
            else
            {
                for (int i = 0; i < positionsv4.Length; i++)        // for an ISystem path, we use the default color
                    color[i] = tapepathdefault.Value;
            }

            float seglen = tapesize * 10;

            // set the tape up

            var tape = GLTapeNormalObjectFactory.CreateTape(positionsv4, color, seglen, 0F.Radians(), margin: sunsize * 1.2f); // create tape
            
            // we fill in the buffer again, and reset the binding position of entry two, update the element index, set visibility
            tapepointbuf.AllocateBytes((tape.Item1.Count + tape.Item2.Count) * GLBuffer.Vec4size);
            tapepointbuf.Fill(tape.Item1.ToArray());
            tapepointbuf.Fill(tape.Item2.ToArray());
            tapepointbuf.Bind(ritape.VertexArray, 1, tapepointbuf.Positions[1], 16);  // for the second one, need to update and rebind positions. First one always at zero

            ritape.RenderState.PrimitiveRestart = GL4Statics.DrawElementsRestartValue(tape.Item4);   // IMPORTANT missing bit Robert, must set the primitive restart value to the new tape size
            ritape.CreateElementIndex(ritape.ElementBuffer, tape.Item3.ToArray(), tape.Item4);       // update the element buffer, DrawCount, ElementIndexSize
            ritape.Visible = tape.Item1.Count > 0;      // only visible if positions..

            starposbuf.AllocateFill(positionsv4);       // and update the star position buffers so find and sun renderer works
            renderersun.InstanceCount = positionsv4.Length; // update the number of suns to draw.
            renderersun.Visible = positionsv4.Length > 0;       // only visible if positions..

            // name bitmaps

            HashSet<object> hashset = new HashSet<object>(currentfilteredlistsys);            // so it can find it quickly
            textrenderer.CurrentGeneration++;                                   // setup for next generation
            textrenderer.RemoveGeneration(textrenderer.CurrentGeneration - 1, hashset); // and remove all of the previous one which are not in hashset.

            using (StringFormat fmt = new StringFormat())
            {
                fmt.Alignment = StringAlignment.Center;
                foreach (var isys in currentfilteredlistsys)
                {
                    if (textrenderer.Exist(isys) == false)                   // if does not exist already, need a new label
                    {
                        textrenderer.Add(isys, isys.Name, Font, ForeText, BackText, new Vector3((float)isys.X+LabelOffset.X, (float)isys.Y + LabelOffset.Y, (float)isys.Z+LabelOffset.Z),
                                LabelSize, new Vector3(0, 0, 0), textformat: fmt, rotatetoviewer: true, rotateelevation: false, alphafadescalar: -200, alphafadepos: 300);
                    }
                }
            }
        }

        public void Update(ulong time, float eyedistance)
        {
            if ((currentfilteredlistsys?.Count ?? 0) > 0)
            {
                const int rotperiodms = 10000;
                time = time % rotperiodms;
                float fract = (float)time / rotperiodms;
                float angle = (float)(2 * Math.PI * fract);
                float scale = Math.Max(1, Math.Min(4, eyedistance / 5000));

                sunvertexshader.ModelTranslation = Matrix4.CreateRotationY(-angle);
                sunvertexshader.ModelTranslation *= Matrix4.CreateScale(scale);           // scale them a little with distance to pick them out better

                const int pathperiodms = 10000;
                tapefrag.TexOffset = new Vector2(-(float)(time % pathperiodms) / pathperiodms, 0);
            }
        }

        // returns HE, and z - if not found z = Max value, null
        public Object FindSystem(Point viewportloc, GLRenderState state, Size viewportsize, out float z)
        {
            z = float.MaxValue;

            if (EnableStars && findshader.Compiled && (currentfilteredlistsys?.Count ?? 0) > 0)
            {
                var geo = findshader.GetShader<GLPLGeoShaderFindTriangles>(OpenTK.Graphics.OpenGL4.ShaderType.GeometryShader);
                geo.SetScreenCoords(viewportloc, viewportsize);

                renderersun.Execute(findshader, state); // execute, discard

                var res = geo.GetResult();
                if (res != null)
                {
                    //for (int i = 0; i < res.Length; i++) System.Diagnostics.Debug.WriteLine($" TP Find {i} {res[i]}");
                    z = res[0].Z;
                    int index = (int)res[0].Y;

                    if (index < currentfilteredlistsys.Count)
                    {
                        if (currentfilteredlisthe != null && index < currentfilteredlisthe.Count)       // #3266 has an exception here, no idea why, since sys+he should be the same length
                            return currentfilteredlisthe[index];
                        else
                            return currentfilteredlistsys[index];
                    }
                }
            }

            return null;
        }

        public bool SetSystem(ISystem s)
        {
            if (currentfilteredlistsys != null)
            {
                CurrentPos = currentfilteredlistsys.IndexOf(s); // -1 if not in list, hence no system
            }
            else
                CurrentPos = -1;
            return CurrentPos != -1;
        }

        public bool SetSystem(string s)
        {
            if (currentfilteredlistsys != null && s.HasChars())
            {
                CurrentPos = currentfilteredlistsys.FindLastIndex(x=>x.Name.Equals(s,StringComparison.InvariantCultureIgnoreCase)); 
            }
            else
                CurrentPos = -1;
            return CurrentPos != -1;
        }

        public ISystem NextSystem()
        {
            if (currentfilteredlistsys == null || currentfilteredlistsys.Count == 0)
                return null;

            if (CurrentPos == -1)
                CurrentPos = 0;
            else if (CurrentPos < currentfilteredlistsys.Count - 1)
                CurrentPos++;

            return currentfilteredlistsys[CurrentPos];
        }

        public ISystem PrevSystem()
        {
            if (currentfilteredlistsys == null || currentfilteredlistsys.Count == 0)
                return null;

            if (CurrentPos == -1)
                CurrentPos = currentfilteredlistsys.Count - 1;
            else if (CurrentPos > 0)
                CurrentPos--;

            return currentfilteredlistsys[CurrentPos];
        }

        private List<ISystem> currentfilteredlistsys;
        private List<HistoryEntry> currentfilteredlisthe;
        private HistoryList lasthl;

        private GLShaderPipeline tapeshader;
        private GLPLFragmentShaderTextureTriStripColorReplace tapefrag;
        private GLBuffer tapepointbuf;
        private GLRenderableItem ritape;

        private GLShaderPipeline sunshader;
        private GLPLVertexShaderModelWorldTextureAutoScale sunvertexshader;
        private GLBuffer starshapebuf;
        private GLBuffer startexcoordbuf;
        private GLBuffer starposbuf;
        private Tuple<GLTexture2DArray, long[]> starimagearray;
        private GLRenderableItem renderersun;

        private GLBitmaps textrenderer;     // star names

        private GLShaderPipeline findshader;        // finder

        private float tapesize;
        private float sunsize;
    }

}
