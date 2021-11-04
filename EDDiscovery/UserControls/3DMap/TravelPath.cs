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

using OpenTK;
using GLOFC.GL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EliteDangerousCore;

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
        public DateTime TravelPathStartDate { get; set; } = new DateTime(2014, 12, 14);
        public DateTime TravelPathEndDate { get; set; } = DateTime.UtcNow.AddMonths(1);
        public bool TravelPathStartDateEnable { get; set; } = false;
        public bool TravelPathEndDateEnable { get; set; } = false;
        public Font Font { get; set; } = new Font("Arial", 8.5f);
        public Size BitMapSize { get; set; } = new Size(96, 30);
        public Color ForeText { get; set; } = Color.White;
        public Color BackText { get; set; } = Color.Transparent;
        public Vector3 LabelSize { get; set; } = new Vector3(5, 0, 5f/4f);
        public Vector3 LabelOffset { get; set; } = new Vector3(0, -1.2f, 0);

        public void Create(string name, int maxstars, float sunsize, float tapesize, GLStorageBlock bufferfindresults, bool depthtest, GLItemsList items, GLRenderProgramSortedList rObjects)
        {
            this.MaxStars = maxstars;
            this.tapesize = tapesize;
            this.sunsize = sunsize;

            // first the tape

            var tapetex = new GLTexture2D(BaseUtils.Icons.IconSet.GetBitmap("GalMap.chevron"), internalformat: OpenTK.Graphics.OpenGL4.SizedInternalFormat.Rgba8);        // tape image
            items.Add(tapetex);
            tapetex.SetSamplerMode(OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat, OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat);

            tapefrag = new GLPLFragmentShaderTextureTriStripColorReplace(1, Color.FromArgb(255, 206, 0, 0));
            var vert = new GLPLVertexShaderTextureWorldCoordWithTriangleStripCoordWRGB();
            tapeshader = new GLShaderPipeline(vert, tapefrag);
            items.Add(tapeshader);

            GLRenderState rts = GLRenderState.Tri(OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedByte, cullface: false);        // set up a Tri strip, Default primitive restart
            rts.DepthTest = depthtest;  // no depth test so always appears

            // now the renderer, set up with the render control, tape as the points, and bind a RenderDataTexture so the texture gets binded each time
            ritape = GLRenderableItem.CreateVector4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.TriangleStrip, rts, new Vector4[] { Vector4.Zero }, new GLRenderDataTexture(tapetex));
            tapepointbuf = items.LastBuffer();  // keep buffer for refill

            ritape.ElementBuffer = items.NewBuffer();       // empty buffer for element index for now
            ritape.Visible = false;     // until its filled, not visible (important, we don't want render to execute unless its been fully set up below)

            rObjects.Add(tapeshader, name + "-tape", ritape);   // add render to object list

            // now the stars

            starposbuf = items.NewBuffer();         // where we hold the vertexes for the suns, used by renderer and by finder

            // the colour index of the stars is selected by the w parameter of the world position vertexes. 
            // we autoscale to make them bigger at greater distances from eye
            sunvertex = new GLPLVertexShaderModelCoordWithWorldTranslationCommonModelTranslation(new Color[] { Color.Yellow, Color.FromArgb(255, 230, 230, 1) }, 
                                    autoscale:30, autoscalemin:1,autoscalemax:2, useeyedistance: false);
            sunshader = new GLShaderPipeline(sunvertex, new GLPLStarSurfaceFragmentShader());
            items.Add(sunshader);

            var shape = GLSphereObjectFactory.CreateSphereFromTriangles(2, sunsize);

            GLRenderState rt = GLRenderState.Tri();     // render is triangles, with no depth test so we always appear
            rt.DepthTest = depthtest;
            rt.DepthClamp = true;
            renderersun = GLRenderableItem.CreateVector4Vector4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, rt, shape, starposbuf, 0, null, 0, 1);
            renderersun.Visible = false;            // until its filled, not visible

            rObjects.Add(sunshader, name + "-suns", renderersun);

            // find compute

            var geofind = new GLPLGeoShaderFindTriangles(bufferfindresults, 16);
            findshader = items.NewShaderPipeline(null, sunvertex, null, null, geofind, null, null, null);
            rifind = GLRenderableItem.CreateVector4Vector4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, GLRenderState.Tri(), shape, starposbuf, ic: 0, seconddivisor: 1);

            // Sun names, handled by textrenderer
            textrenderer = new GLBitmaps( name + "-text", rObjects, BitMapSize, depthtest: depthtest, cullface: false);
            items.Add(textrenderer);
        }

        // tested to 50K+ stars
        public void CreatePath(HistoryList hl)
        {
            lasthl = hl;
            ISystem lastone = CurrentPos != -1 && CurrentPos < currentfilteredlistsys.Count ? currentfilteredlistsys[CurrentPos] : null;  // see if lastpos is there, and store it

            // create current filter list..
            currentfilteredlisthe = hl.FilterByTravelTime(TravelPathStartDateEnable ? TravelPathStartDate : default(DateTime?), TravelPathEndDateEnable ? TravelPathEndDate : default(DateTime?), true);

            if (currentfilteredlisthe.Count > MaxStars)
                currentfilteredlisthe = currentfilteredlisthe.Skip(currentfilteredlisthe.Count - MaxStars).ToList();

            currentfilteredlistsys = currentfilteredlisthe.Select(x => (ISystem)x.System).ToList();

            CurrentPos = lastone == null ? -1 : currentfilteredlistsys.IndexOf(lastone);        // may be -1, may have been removed

            CreatePathInt();
        }
        public void CreatePath(List<ISystem> syslist, Color tapecolour)
        {
            lasthl = null;
            ISystem lastone = CurrentPos != -1 && CurrentPos < currentfilteredlistsys.Count ? currentfilteredlistsys[CurrentPos] : null;  // see if lastpos is there, and store it

            // create current filter list..
            currentfilteredlisthe = null;
            currentfilteredlistsys = syslist;

            CurrentPos = lastone == null ? -1 : currentfilteredlistsys.IndexOf(lastone);        // may be -1, may have been removed

            CreatePathInt(tapecolour);
        }

        public void Refresh()           // must have drawn
        {
            if (lasthl != null)
                CreatePath(lasthl);
        }

        // currentfilteredlist set, go..
        private void CreatePathInt(Color? tapepathdefault = null)
        {
            // Note W here selects the colour index of the stars, 0 = first, 1 = second etc

            Vector4[] positionsv4 = currentfilteredlistsys.Select(x => new Vector4((float)x.X, (float)x.Y, (float)x.Z, 0)).ToArray();
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

            // a tape is a set of points (item1) and indexes to select them (item2), so we need an element index in the renderer to use.
            var tape = GLTapeObjectFactory.CreateTape(positionsv4, color, tapesize, seglen, 0F.Radians(), margin: sunsize * 1.2f);

            tapepointbuf.AllocateFill(tape.Item1.ToArray());        // replace the points with a new one
            ritape.RenderState.PrimitiveRestart = GL4Statics.DrawElementsRestartValue(tape.Item3);        // IMPORTANT missing bit Robert, must set the primitive restart value to the new tape size

            ritape.CreateElementIndex(ritape.ElementBuffer, tape.Item2.ToArray(), tape.Item3);       // update the element buffer
            ritape.Visible = tape.Item1.Count > 0;      // only visible if positions..

            starposbuf.AllocateFill(positionsv4);       // and update the star position buffers so find and sun renderer works
            renderersun.InstanceCount = positionsv4.Length; // update the number of suns to draw.
            renderersun.Visible = positionsv4.Length > 0;       // only visible if positions..

            rifind.InstanceCount = positionsv4.Length;  // update the find list

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
                                LabelSize, new Vector3(0, 0, 0), fmt: fmt, rotatetoviewer: true, rotateelevation: false, alphafadescalar: -200, alphafadepos: 300);
                    }
                }
            }
        }

        public void Update(ulong time, float eyedistance)
        {
            if ((currentfilteredlistsys?.Count ?? 0) > 0)
            {
                const int rotperiodms = 20000;
                time = time % rotperiodms;
                float fract = (float)time / rotperiodms;
                float angle = (float)(2 * Math.PI * fract);
                sunvertex.ModelTranslation = Matrix4.CreateRotationY(-angle);

                const int pathperiodms = 10000;

                float scale = Math.Max(1, Math.Min(4, eyedistance / 5000));
                //System.Diagnostics.Debug.WriteLine("Scale {0}", scale);
                sunvertex.ModelTranslation *= Matrix4.CreateScale(scale);           // scale them a little with distance to pick them out better
                tapefrag.TexOffset = new Vector2(-(float)(time % pathperiodms) / pathperiodms, 0);
            }
        }

        // returns HE, and z - if not found z = Max value, null
        public Object FindSystem(Point viewportloc, GLRenderState state, Size viewportsize, out float z)
        {
            z = float.MaxValue;

            if (EnableStars && (currentfilteredlistsys?.Count ?? 0) > 0)
            {
                var geo = findshader.GetShader<GLPLGeoShaderFindTriangles>(OpenTK.Graphics.OpenGL4.ShaderType.GeometryShader);
                geo.SetScreenCoords(viewportloc, viewportsize);

                rifind.Execute(findshader, state); // execute, discard

                var res = geo.GetResult();
                if (res != null)
                {
                    //for (int i = 0; i < res.Length; i++) System.Diagnostics.Debug.WriteLine(i + " = " + res[i]);
                    z = res[0].Z;
                    if (currentfilteredlisthe != null)
                        return currentfilteredlisthe[(int)res[0].Y];
                    else
                        return currentfilteredlistsys[(int)res[0].Y];
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
        private GLPLVertexShaderModelCoordWithWorldTranslationCommonModelTranslation sunvertex;
        private GLBuffer starposbuf;
        private GLRenderableItem renderersun;

        private GLBitmaps textrenderer;     // star names

        private GLShaderPipeline findshader;        // finder
        private GLRenderableItem rifind;

        private float tapesize;
        private float sunsize;

    }

}
