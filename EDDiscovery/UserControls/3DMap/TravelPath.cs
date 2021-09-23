using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
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

using GLOFC;
using GLOFC.GL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EliteDangerousCore;

namespace EDDiscovery.UserControls.Map3D
{
    class TravelPath
    {
        public List<HistoryEntry> CurrentList { get { return currentfilteredlist; } }       // whats being displayed
        public bool Enable { get { return tapeshader.Enable; } set { tapeshader.Enable = textrenderer.Enable = value; } }
        public int MaxStars { get; }
        public DateTime TravelPathStartDate { get; set; } = new DateTime(2014, 12, 14);
        public DateTime TravelPathEndDate { get; set; } = DateTime.UtcNow.AddMonths(1);
        public bool TravelPathStartDateEnable { get; set; } = false;
        public bool TravelPathEndDateEnable { get; set; } = false;

        private List<HistoryEntry> currentfilteredlist;
        //private List<HistoryEntry> unfilteredlist;

        public TravelPath(int maxstars, float sunsize, float tapesize, int bufferfindbinding, bool depthtest)
        {
            this.MaxStars = maxstars;
            this.sunsize = sunsize;
            this.tapesize = tapesize;
            this.depthtest = depthtest;
            this.bufferfindbinding = bufferfindbinding;
        }

        // tested to 50K+ stars
        public void CreatePath(GLItemsList items, GLRenderProgramSortedList rObjects, HistoryList hl)
        {
            HistoryEntry lastone = lastpos != -1 && lastpos < currentfilteredlist.Count ? currentfilteredlist[lastpos] : null;  // see if lastpos is there, and store it

            // create current filter list..
            currentfilteredlist = hl.FilterByTravelTime(TravelPathStartDateEnable ? TravelPathStartDate : default(DateTime?), TravelPathEndDateEnable ? TravelPathEndDate : default(DateTime?));

            if (currentfilteredlist.Count > MaxStars)
                currentfilteredlist = currentfilteredlist.Skip(currentfilteredlist.Count - MaxStars).ToList();

            lastpos = lastone == null ? -1 : currentfilteredlist.IndexOf(lastone);        // may be -1, may have been removed

            CreatePathInt(items, rObjects);
        }

        // currentfilteredlist set, go..
        private void CreatePathInt(GLItemsList items, GLRenderProgramSortedList rObjects)
        { 
            var positionsv4 = currentfilteredlist.Select(x => new Vector4((float)x.System.X, (float)x.System.Y, (float)x.System.Z, 0)).ToArray();
            var color = new Color[positionsv4.Length];

            for (int i = 0; i < positionsv4.Length; i++)        // if we are on a jump colour entry, then pick up its colour, otherwise use the last, unless its at the beginning
                color[i] = (currentfilteredlist[i] is IJournalJumpColor) ? (currentfilteredlist[i] as IJournalJumpColor).MapColorARGB : (i > 0 ? color[i - 1] : Color.Green);
            float seglen = tapesize * 10;

            // a tape is a set of points (item1) and indexes to select them (item2), so we need an element index in the renderer to use.
            var tape = GLTapeObjectFactory.CreateTape(positionsv4, color, tapesize, seglen, 0F.Radians(), margin: sunsize * 1.2f);

            if (ritape == null) // first time..
            {
                // first the tape

                var tapetex = new GLTexture2D(BaseUtils.Icons.IconSet.Instance.Get("GalMap.chevron") as Bitmap, internalformat:OpenTK.Graphics.OpenGL4.SizedInternalFormat.Rgba8);        // tape image
                items.Add(tapetex);
                tapetex.SetSamplerMode(OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat, OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat);

                tapefrag = new GLPLFragmentShaderTextureTriStripColorReplace(1, Color.FromArgb(255, 206, 0, 0));
                var vert = new GLPLVertexShaderTextureWorldCoordWithTriangleStripCoordWRGB();
                tapeshader = new GLShaderPipeline(vert, tapefrag);
                items.Add(tapeshader);

                GLRenderState rts = GLRenderState.Tri(tape.Item3, cullface: false);        // set up a Tri strip, primitive restart value set from tape, no culling
                rts.DepthTest = depthtest;  // no depth test so always appears

                // now the renderer, set up with the render control, tape as the points, and bind a RenderDataTexture so the texture gets binded each time
                ritape = GLRenderableItem.CreateVector4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.TriangleStrip, rts, tape.Item1.ToArray(), new GLRenderDataTexture(tapetex));
                tapepointbuf = items.LastBuffer();  // keep buffer for refill
                ritape.Visible = tape.Item1.Count > 0;      // no items, set not visible, so it won't except over the BIND with nothing in the element buffer

                ritape.CreateElementIndex(items.NewBuffer(), tape.Item2.ToArray(), tape.Item3); // finally, we are using index to select vertexes, so create an index

                rObjects.Add(tapeshader, "travelpath-tape", ritape);   // add render to object list

                // now the stars

                starposbuf = items.NewBuffer();         // where we hold the vertexes for the suns, used by renderer and by finder

                starposbuf.AllocateFill(positionsv4);
                //Vector4[] vectors = starposbuf.ReadVector4s(0, starposbuf.Length / 16);

                sunvertex = new GLPLVertexShaderModelCoordWithWorldTranslationCommonModelTranslation(new Color[] { Color.Yellow, Color.FromArgb(255, 230, 230, 1) });
                items.Add(sunvertex);
                sunshader = new GLShaderPipeline(sunvertex, new GLPLStarSurfaceFragmentShader());
                items.Add(sunshader);

                var shape = GLSphereObjectFactory.CreateSphereFromTriangles(2, sunsize);

                GLRenderState rt = GLRenderState.Tri();     // render is triangles, with no depth test so we always appear
                rt.DepthTest = depthtest;
                rt.DepthClamp = true;
                renderersun = GLRenderableItem.CreateVector4Vector4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, rt, shape, starposbuf, 0, null, currentfilteredlist.Count, 1);
                rObjects.Add(sunshader,"travelpath-suns", renderersun);

                // find compute

                findshader = items.NewShaderPipeline(null, sunvertex, null, null, new GLPLGeoShaderFindTriangles(bufferfindbinding, 16), null, null, null);
                items.Add(findshader);
                rifind = GLRenderableItem.CreateVector4Vector4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, GLRenderState.Tri(), shape, starposbuf, ic: currentfilteredlist.Count, seconddivisor: 1);

                // Sun names, handled by textrenderer
                textrenderer = new GLBitmaps("bm-travelmap",rObjects, new Size(128, 40), depthtest: depthtest, cullface: false);
                items.Add(textrenderer);

            }
            else
            {
                tapepointbuf.AllocateFill(tape.Item1.ToArray());        // replace the points with a new one
                ritape.RenderState.PrimitiveRestart = GL4Statics.DrawElementsRestartValue(tape.Item3);        // IMPORTANT missing bit Robert, must set the primitive restart value to the new tape size
                ritape.CreateElementIndex(ritape.ElementBuffer, tape.Item2.ToArray(), tape.Item3);       // update the element buffer
                ritape.Visible = tape.Item1.Count > 0;

                starposbuf.AllocateFill(positionsv4);       // and update the star position buffers so find and sun renderer works
                renderersun.InstanceCount = positionsv4.Length; // update the number of suns to draw.

                rifind.InstanceCount = positionsv4.Length;  // update the find list
            }

            // name bitmaps

            HashSet<object> hashset = new HashSet<object>(currentfilteredlist);            // so it can find it quickly
            textrenderer.CurrentGeneration++;                                   // setup for next generation
            textrenderer.RemoveGeneration(textrenderer.CurrentGeneration - 1, hashset); // and remove all of the previous one which are not in hashset.

            Font fnt = new Font("Arial", 8.5F);
            using (StringFormat fmt = new StringFormat())
            {
                fmt.Alignment = StringAlignment.Center;
                foreach (var isys in currentfilteredlist)
                {
                    if (textrenderer.Exist(isys) == false)                   // if does not exist already, need a new label
                    {
                        textrenderer.Add(isys, isys.System.Name, fnt, Color.White, Color.Transparent, new Vector3((float)isys.System.X, (float)isys.System.Y - 5, (float)isys.System.Z),
                                new Vector3(20, 0, 0), new Vector3(0, 0, 0), fmt: fmt, rotatetoviewer: true, rotateelevation: false, alphafadescalar: -200, alphafadepos: 300);
                    }
                }
            }

            fnt.Dispose();
        }

        public void Update(ulong time, float eyedistance)
        {
            const int rotperiodms = 20000;
            time = time % rotperiodms;
            float fract = (float)time / rotperiodms;
            float angle = (float)(2 * Math.PI * fract);
            sunvertex.ModelTranslation = Matrix4.CreateRotationY(-angle);
            float scale = Math.Max(1, Math.Min(4, eyedistance / 5000));
            //System.Diagnostics.Debug.WriteLine("Scale {0}", scale);
            sunvertex.ModelTranslation *= Matrix4.CreateScale(scale);           // scale them a little with distance to pick them out better
            tapefrag.TexOffset = new Vector2(-(float)(time % 2000) / 2000, 0);
        }

        public HistoryEntry FindSystem(Point viewportloc, GLRenderState state, Size viewportsize)
        {
            var geo = findshader.GetShader<GLPLGeoShaderFindTriangles>(OpenTK.Graphics.OpenGL4.ShaderType.GeometryShader);
            geo.SetScreenCoords(viewportloc, viewportsize);

            rifind.Execute(findshader, state, discard:true); // execute, discard

            var res = geo.GetResult();
            if (res != null)
            {
                //for (int i = 0; i < res.Length; i++) System.Diagnostics.Debug.WriteLine(i + " = " + res[i]);
                return currentfilteredlist[(int)res[0].Y];
            }

            return null;
        }

        public HistoryEntry CurrentSystem { get { return currentfilteredlist!=null && lastpos != -1 ? currentfilteredlist[lastpos] : null; } }

        public bool SetSystem(HistoryEntry s)
        {
            if (currentfilteredlist != null)
            {
                lastpos = currentfilteredlist.IndexOf(s); // -1 if not in list, hence no system
            }
            else
                lastpos = -1;
            return lastpos != -1;
        }

        public bool SetSystem(int i)
        {
            if (currentfilteredlist != null && i >= 0 && i < currentfilteredlist.Count)
            {
                lastpos = i;
                return true;
            }
            else
                return false;
        }

        public HistoryEntry NextSystem()
        {
            if (currentfilteredlist == null)
                return null;

            if (lastpos == -1)
                lastpos = 0;
            else if (lastpos < currentfilteredlist.Count - 1)
                lastpos++;

            return currentfilteredlist[lastpos];
        }

        public HistoryEntry PrevSystem()
        {
            if (currentfilteredlist == null)
                return null;

            if (lastpos == -1)
                lastpos = currentfilteredlist.Count - 1;
            else if (lastpos > 0)
                lastpos--;

            return currentfilteredlist[lastpos];
        }

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

        private int lastpos = -1;       // -1 no system, in currentfilteredlist

        private float sunsize;
        private float tapesize;
        private int bufferfindbinding;

        private bool depthtest;
    }

}
