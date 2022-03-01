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
using GLOFC.GL4;
using GLOFC.GL4.Shaders;
using GLOFC.GL4.Shaders.Fragment;
using GLOFC.GL4.Shaders.Geo;
using GLOFC.GL4.Shaders.Vertex;
using GLOFC.GL4.ShapeFactory;
using OpenTK;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EDDiscovery.UserControls.Map3D
{
    public class Bookmarks
    {
        public bool Enable { get { return objectshader.Enable; } set { objectshader.Enable = value; } }


        public void Start(GLItemsList items, GLRenderProgramSortedList rObjects, float bookmarksize, GLStorageBlock findbufferresults, bool depthtest)
        {
            var vert = new GLPLVertexScaleLookat(rotatetoviewer: dorotate, rotateelevation: doelevation, texcoords: true, generateworldpos: true,
                                                            autoscale: 30, autoscalemin: 1f, autoscalemax: 30f); // above autoscale, 1f

            GLOFC.GLStatics.Check();

            const int texbindingpoint = 1;
            var frag = new GLPLFragmentShaderTexture(texbindingpoint);       // binding - simple texturer based on vs model coords

            objectshader = new GLShaderPipeline(vert, null, null, null, frag);
            items.Add(objectshader);

            var objtex = items.NewTexture2D("Bookmarktex", BaseUtils.Icons.IconSet.GetBitmap("GalMap.Bookmark"), OpenTK.Graphics.OpenGL4.SizedInternalFormat.Rgba8);

            objectshader.StartAction += (s, m) =>
            {
                objtex.Bind(texbindingpoint);   // bind tex array to, matching above
            };

            bookmarkposbuf = items.NewBuffer();         // where we hold the vertexes for the suns, used by renderer and by finder

            GLRenderState rt = GLRenderState.Tri();
            rt.DepthTest = depthtest;

            // 0 is model pos, 1 is world pos by a buffer, 2 is tex co-ords
            ridisplay = GLRenderableItem.CreateVector4Vector4Vector2(items, OpenTK.Graphics.OpenGL4.PrimitiveType.TriangleStrip, rt,
                                GLShapeObjectFactory.CreateQuadTriStrip(bookmarksize, bookmarksize),         // quad2 4 vertexts as the model positions
                                bookmarkposbuf, 0,       // world positions come from here - not filled as yet
                                GLShapeObjectFactory.TexTriStripQuad,
                                ic: 0, seconddivisor: 1);

            rObjects.Add(objectshader, "bookmarks", ridisplay);

            var geofind = new GLPLGeoShaderFindTriangles(findbufferresults, 16);//, forwardfacing:false);
            findshader = items.NewShaderPipeline(null, vert, null, null, geofind, null, null, null);

            // hook to modelworldbuffer, at modelpos and worldpos.  UpdateEnables will fill in instance count
            rifind = GLRenderableItem.CreateVector4Vector4Vector2(items, OpenTK.Graphics.OpenGL4.PrimitiveType.TriangleStrip, rt,
                                                            GLShapeObjectFactory.CreateQuadTriStrip(bookmarksize, bookmarksize),         // quad2 4 vertexts as the model positions
                                                            bookmarkposbuf, 0,
                                                            GLShapeObjectFactory.TexTriStripQuad,
                                                            ic: 0, seconddivisor: 1);

        }

        public void Create(Vector4[] incomingsys)
        {
            bookmarkposbuf.AllocateFill(incomingsys);
            ridisplay.InstanceCount = rifind.InstanceCount = incomingsys.Length;
        }

        public int? Find(Point loc, GLRenderState state, Size viewportsize, out float z)
        {
            z = float.MaxValue;

            if (!objectshader.Enable)
                return null;

            var geo = findshader.GetShader<GLPLGeoShaderFindTriangles>(OpenTK.Graphics.OpenGL4.ShaderType.GeometryShader);
            geo.SetScreenCoords(loc, viewportsize);

            rifind.Execute(findshader, state); // execute. Geoshader discards geometry by not outputting anything

            var res = geo.GetResult();
            if (res != null)
            {
                for (int i = 0; i < res.Length; i++) System.Diagnostics.Debug.WriteLine($"bk {i} {res[i]}");
                z = res[0].Z;
                return (int)res[0].Y;
            }

            return null;
        }

        private GLBuffer bookmarkposbuf;
        private GLRenderableItem ridisplay;
        private GLShaderPipeline objectshader;
        private GLShaderPipeline findshader;
        private GLRenderableItem rifind;
        private const bool dorotate = true;
        private const bool doelevation = false;

    }
}
