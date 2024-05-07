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
using GLOFC.GL4;
using GLOFC.GL4.Bitmaps;
using GLOFC.GL4.Shaders;
using GLOFC.GL4.Shaders.Fragment;
using GLOFC.GL4.Shaders.Geo;
using GLOFC.GL4.Shaders.Tesselation;
using GLOFC.GL4.Shaders.Vertex;
using GLOFC.GL4.ShapeFactory;
using GLOFC.GL4.Textures;
using OpenTK;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EDDiscovery.UserControls.Map3D
{
    public class GalMapObjects
    {
        public GalMapObjects()
        {
        }

        public Font Font { get; set; } = new Font("Arial", 8.5f);
        public bool GetGalObjectTypeEnableState(string id) { return !State.ContainsKey(id) || State[id] == true; }

        public bool Enable { get { return objectshader.Enable; } }

        public HashSet<ObjectPosXYZ> Positions { get; private set; } = new HashSet<ObjectPosXYZ>();     // positions of gal map objects enabled - even if main shader is off
        public HashSet<ObjectPosXYZ> PositionsWithEnable { get { return Enable ? Positions : new HashSet<ObjectPosXYZ>(); } }     // positions of gal map objects 

        // all enable set functions return a list of current gal objects positions
        public void SetShaderEnable(bool enable)
        {
            textrenderer.Enable = objectshader.Enable = enable;
            if (enable)
                UpdateEnables();
        }
        public void SetGalObjectTypeEnable(string id, bool state) { State[id] = state; UpdateEnables(); }
        public void SetAllEnables(string settings)
        {
            string[] ss = settings.Split(',');
            int i = 0;
            foreach (var o in GalMapType.VisibleTypes)
            {
                State[o.TypeName] = i >= ss.Length || !ss[i].Equals("-");              // on if we don't have enough, or on if its not -
                i++;
            }
            UpdateEnables();
        }
        public string GetAllEnables()
        {
            string s = "";
            foreach (var o in GalMapType.VisibleTypes)
            {
                s += GetGalObjectTypeEnableState(o.TypeName) ? "+," : "-,";
            }
            return s;
        }

        public void SetAutoScale(int max)
        {
            objectshader.GetShader<GLPLVertexScaleLookatConfigurable>().SetScalars(30, 1, max);
        }

        // used to pass back object positions used
        public struct ObjectPosXYZ
        {
            public int x;       // these are in SystemClass.XYZScalar
            public int y;
            public int z;

            public ObjectPosXYZ(float fx, float fy, float fz)
            {
                x = (int)(fx * EliteDangerousCore.SystemClass.XYZScalar);
                y = (int)(fy * EliteDangerousCore.SystemClass.XYZScalar);
                z = (int)(fz * EliteDangerousCore.SystemClass.XYZScalar);
            }
            public ObjectPosXYZ(double fx, double fy, double fz)
            {
                x = (int)(fx * EliteDangerousCore.SystemClass.XYZScalar);
                y = (int)(fy * EliteDangerousCore.SystemClass.XYZScalar);
                z = (int)(fz * EliteDangerousCore.SystemClass.XYZScalar);
            }
            public ObjectPosXYZ(int ix, int iy, int iz)
            {
                x = ix;
                y = iy;
                z = iz;
            }
        }

        public static IReadOnlyDictionary<GalMapType.VisibleObjectsType, Image> GalMapTypeIcons { get; } = new BaseUtils.Icons.IconGroup<GalMapType.VisibleObjectsType>("GalMap");

        public void CreateObjects(GLItemsList items, GLRenderProgramSortedList rObjects, GalacticMapping galmap, GLStorageBlock findbufferresults, bool depthtest)
        {
            this.galmap = galmap;

            // first gets the images and make a 2d array texture for them

            Bitmap[] images = GalMapType.VisibleTypes.Select(x => GalMapTypeIcons[x.VisibleType.Value] as Bitmap).ToArray();
            // 256 is defined normal size
            var objtex = new GLTexture2DArray(images, bmpmipmaplevels: 1, wantedmipmaplevels: 3, texturesize: new Size(256, 256), internalformat: OpenTK.Graphics.OpenGL4.SizedInternalFormat.Rgba8, alignment: ContentAlignment.BottomCenter);
            IGLTexture texarray = items.Add(objtex, "GalObjTex");

            const float objsize = 1.5f;        // size of object on screen
            const float wavesize = 0.1f;
            Size textbitmapsize = new Size(128, 40);
            Vector3 labelsize = new Vector3(4, 0, 4.0f * textbitmapsize.Height / textbitmapsize.Width);   // size of text

            // now build the shaders

            const int texbindingpoint = 1;
            var vert = new GLPLVertexScaleLookatConfigurable(rotatetoviewer: dorotate, rotateelevation: doelevation,       // a look at vertex shader
                                                             useeyedistance:true); 
            var tcs = new GLPLTesselationControl(10f);  // number of intermediate points
            tes = new GLPLTesselationEvaluateSinewave(wavesize, 1f);         // 0.2f in size, 1 wave across the object
            var frag = new GLPLFragmentShaderTexture2DDiscard(texbindingpoint);       // binding - takes image pos from tes. imagepos < 0 means discard
            objectshader = new GLShaderPipeline(vert, tcs, tes, null, frag);
            items.Add(objectshader);

            SetAutoScale(300);

            objectshader.StartAction += (s, m) =>
            {
                texarray.Bind(texbindingpoint);   // bind tex array to, matching above
            };

            // now the RenderControl for the objects

            GLRenderState rt = GLRenderState.Patches(4);
            rt.DepthTest = depthtest;

            // create a quad and all entries of the renderable map objects, zero at this point, with a zero instance count. UpdateEnables will fill it in later
            // but we need to give it the maximum buffer length at this point


            ridisplay = GLRenderableItem.CreateVector4Vector4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.Patches, rt,
                                GLShapeObjectFactory.CreateQuadTriStrip(objsize, objsize),         // quad2 4 vertexts
                                new Vector4[galmap.VisibleMapObjects.Length],        // world positions
                                ic: 0, seconddivisor: 1);

            modelworldbuffer = items.LastBuffer();
            int modelpos = modelworldbuffer.Positions[0];
            worldpos = modelworldbuffer.Positions[1];

            rObjects.Add(objectshader, "galmapobj", ridisplay);

            // add a find shader to look them up

            var geofind = new GLPLGeoShaderFindTriangles(findbufferresults, 32768);        // pass thru normal vert/tcs/tes then to geoshader for results
            findshader = items.NewShaderPipeline(null, vert, tcs, tes, geofind, null, null, null);

            // hook to modelworldbuffer, at modelpos and worldpos.  UpdateEnables will fill in instance count
            rifind = GLRenderableItem.CreateVector4Vector4(items, OpenTK.Graphics.OpenGL4.PrimitiveType.Patches, GLRenderState.Patches(4), modelworldbuffer, modelpos, ridisplay.DrawCount,
                                                                            modelworldbuffer, worldpos, null, ic: 0, seconddivisor: 1);

            // Text renderer for the labels

            textrenderer = new GLBitmaps("bm-galmapobjects", rObjects, textbitmapsize, depthtest: depthtest, cullface: false, textureformat: OpenTK.Graphics.OpenGL4.SizedInternalFormat.Rgba8);
            items.Add(textrenderer);

            // now make the text up for all the objects above

            using (StringFormat fmt = new StringFormat())
            {
                fmt.Alignment = StringAlignment.Center;

                var renderablegalmapobjects = galmap.VisibleMapObjects; // list of enabled entries

                List<Vector3> posset = new List<Vector3>();

                float offscale = objsize/2 + labelsize.Z / 2 + wavesize;       // this is the nominal centre of the text bitmap, offset in Y to the object

                for (int i = 0; i < renderablegalmapobjects.Length; i++)
                {
                    var o = renderablegalmapobjects[i];

                    float offset = -offscale;

                    for (int j = 0; j < i; j++)     // look up previous ones and see if we labeled it before
                    {
                        var d1 = new Vector3(o.Points[0].X, o.Points[0].Y + offset, o.Points[0].Z);
                        var d2 = posset[j];     // where it was placed.
                        var diff = d1 - d2;

                        if (diff.Length < offscale)        // close
                        {
                            if (offset > 0)         // if offset is positive, flip below and increase again
                                offset = -offset - offscale;
                            else
                                offset *= -1;       // flip over top
                                                    // System.Diagnostics.Debug.WriteLine($"close {renderablegalmapobjects[i].name} {d1} to {renderablegalmapobjects[j].name} {d2} {diff} select {offset}");
                        }
                    }

                    Vector3 pos = new Vector3(o.Points[0].X, o.Points[0].Y + offset, o.Points[0].Z);
                    posset.Add(pos);
                    //System.Diagnostics.Debug.WriteLine($"{renderablegalmapobjects[i].name} at {pos} {offset}");

                    textrenderer.Add(o.ID, o.NameList, Font,
                        Color.White, Color.FromArgb(0, 255, 0, 255),
                        pos,
                        labelsize, new Vector3(0, 0, 0), textformat: fmt, rotatetoviewer: dorotate, rotateelevation: doelevation,
                        alphafadescalar: -100, alphafadepos: 500); // fade in, alpha = 0 at >500, 1 at 400

                }
            }

            UpdateEnables();      // fill in worldpos's and update instance count, taking into 
        }

        private void UpdateEnables()           // rewrite the modelworld buffer with the ones actually enabled
        {
            Positions = new HashSet<ObjectPosXYZ>();

            modelworldbuffer.StartWrite(worldpos);                  // overwrite world positions

            var renderablegalmapobjects = galmap.VisibleMapObjects; // list of displayable entries
            indextoentry = new int[renderablegalmapobjects.Length];
            int mwpos = 0,entry=0;

            foreach (var o in renderablegalmapobjects)
            {
                // for all types in this object, are we on?. If any are on, we are on

                bool en = o.GalMapTypes.Where(x => GetGalObjectTypeEnableState(x.TypeName)).Select(x => x).Count() > 0;
                //System.Diagnostics.Debug.WriteLine($"3dmap galmapobject {o.GalMapType.TypeName} : {o.GalMapType.Description} : `{o.NameList}` Enabled : {en}");

                if (en)
                {
                    modelworldbuffer.Write(new Vector4(o.Points[0].X, o.Points[0].Y, o.Points[0].Z, o.GalMapTypes[0].Index + (!Animate(o.GalMapTypes[0].VisibleType.Value) ? 65536 : 0)));
                    indextoentry[mwpos++] = entry;
                    Positions.Add(new ObjectPosXYZ(o.Points[0].X, o.Points[0].Y, o.Points[0].Z));
                }

                textrenderer.SetVisiblityRotation(o.ID, en, dorotate, doelevation);
                entry++;
            }

            modelworldbuffer.StopReadWrite();
            //var f = modelworldbuffer.ReadVector4(worldpos, renderablegalmapobjects.Count());  foreach (var v in f) System.Diagnostics.Debug.WriteLine("Vector " + v);
            ridisplay.InstanceCount = rifind.InstanceCount = mwpos;
        }

        public GalacticMapObject FindPOI(Point viewportloc, GLRenderState state, Size viewportsize, out float z)
        {
            z = float.MaxValue;

            if (Enable)
            {
                var geo = findshader.GetShader<GLPLGeoShaderFindTriangles>(OpenTK.Graphics.OpenGL4.ShaderType.GeometryShader);
                geo.SetScreenCoords(viewportloc, viewportsize);

                System.Diagnostics.Debug.WriteLine($"Execute gal obj find");
                rifind.Execute(findshader, state); // execute, discard

                var res = geo.GetResult();
                if (res != null)
                {
                    System.Diagnostics.Debug.WriteLine($"gal obj find {res.Length}");
                    var renderablegalmapobjects = galmap.VisibleMapObjects; // list of displayable entries
                    int index = 0;

                    foreach (var o in renderablegalmapobjects)
                    {
                        bool en = o.GalMapTypes.Where(x => GetGalObjectTypeEnableState(x.TypeName)).Select(x => x).Count() > 0;
                        if (en)
                        {
                            if (index == (int)res[0].Y)
                            {
                                z = res[0].Z;
                                return o;
                            }
                            index++;
                        }
                    }
                }
            }

            return null;
        }

        public void Update(ulong time, float eyedistance)
        {
            const int rotperiodms = 5000;
            time = time % rotperiodms;
            float fract = (float)time / rotperiodms;
            //System.Diagnostics.Debug.WriteLine("Time " + time + "Phase " + fract);
            tes.Phase = fract;
        }

        private bool Animate(GalMapType.VisibleObjectsType e)
        {
            GalMapType.VisibleObjectsType[] list = { GalMapType.VisibleObjectsType.nebula, GalMapType.VisibleObjectsType.planetaryNebula,
                                                GalMapType.VisibleObjectsType.stellarRemnant, GalMapType.VisibleObjectsType.blackHole,
                                                GalMapType.VisibleObjectsType.pulsar, GalMapType.VisibleObjectsType.starCluster,
                                                GalMapType.VisibleObjectsType.cometaryBody,GalMapType.VisibleObjectsType.MarxNebula,
            };
            return list.Contains(e);
        }

        private GLPLTesselationEvaluateSinewave tes;
        private GLShaderPipeline objectshader;
        private GLBuffer modelworldbuffer;
        private int worldpos;
        private GLRenderableItem ridisplay;

        private GLBitmaps textrenderer;     // gmo names

        private GLShaderPipeline findshader;
        private GLRenderableItem rifind;
        private int[] indextoentry;
        private Dictionary<string, bool> State { get; set; } = new Dictionary<string, bool>();       // if not present, its on, else state 
        private GalacticMapping galmap;

        private const bool dorotate = true;
        private const bool doelevation = false;
    }

}
