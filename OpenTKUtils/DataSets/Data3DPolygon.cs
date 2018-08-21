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
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace OpenTKUtils
{

    public class Polygon : IDrawingPrimative
    {
        public List<Vector3> vertices;

        public Polygon(List<Vector3> points, float size, Color c)
        {
            vertices = points;
            Size = size;
            Color = c;
        }

        public Polygon(List<Vector2> points, float size, Color c)
        {
            vertices = new List<Vector3>();
            foreach (Vector2 f in points)
                vertices.Add(new Vector3(f.X, 0, f.Y));

            Size = size;
            Color = c;
        }

        public PrimitiveType Type { get { return PrimitiveType.Polygon; } }
        public Color Color { get; set; }
        public float Size { get; set; }

        public void Draw(GLControl control)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<GLControl>(this.Draw), control);
            }
            else
            {
                GL.Begin(Type);
                GL.Color4(Color);
                foreach (Vector3 v in vertices)
                    GL.Vertex3(v);
                GL.End();
            }
        }
    }


    public class PolygonCollection : Data3DCollection<Polygon>,  IDisposable      // can be either Polygons, LineLoop, Triangles
    {
        protected Vector3[] Vertices;
        private uint[] Colourarray;
        protected int NumVertices;

        public OpenTK.Graphics.OpenGL.PrimitiveType Primative { get; set; }

        List<int> Polystart = new List<int>();
        List<int> Polycount = new List<int>();

        protected int VtxVboID;
        protected int VtxColourVboId;
        protected GLControl GLContext;

        public PolygonCollection(string name, Color color, float pointsize, OpenTK.Graphics.OpenGL.PrimitiveType ptype )
            : base(name, color, pointsize)
        {
            Primative = ptype;
        }

        public void Dispose()
        {
            if (GLContext != null)
            {
                if (GLContext.InvokeRequired)
                {
                    GLContext.Invoke(new Action(this.Dispose));
                }
                else
                {
                    GL.DeleteBuffer(VtxVboID);
                    GL.DeleteBuffer(VtxColourVboId);
                    VtxVboID = VtxColourVboId = 0;
                    GLContext = null;
                }
            }
        }

        public override void Add(Polygon primative)
        {
            Debug.Assert(Primative != PrimitiveType.Triangles || primative.vertices.Count == 3);

            Dispose();

            if (Vertices == null)
            {
                Vertices = new Vector3[1024];
                Colourarray = new uint[1024];
            }
            else if (NumVertices + primative.vertices.Count > Vertices.Length)
            {
                Array.Resize(ref Vertices, Vertices.Length * 2);
                Array.Resize(ref Colourarray, Vertices.Length * 2);
            }

            Polystart.Add(NumVertices);                                             // store poly details
            Polycount.Add(primative.vertices.Count);

            uint cx = BitConverter.ToUInt32(new byte[] { primative.Color.R, primative.Color.G, primative.Color.B, primative.Color.A }, 0);

            foreach (Vector3 p in primative.vertices)
            {
                Colourarray[NumVertices] = cx;     // need colour per vertex.. if not, it does not work, or you can mix colours
                Vertices[NumVertices++] = p;
            }
        }

        public override void DrawAll(GLControl control)
        {
            if (GLContext != null && GLContext != control)
            {
                Dispose();
            }

            if (control.InvokeRequired)
            {
                control.Invoke(new Action<GLControl>(this.DrawAll), control);
            }
            else
            {
                if (VtxVboID == 0)                                              // shove vertexes into a buffer
                {
                    GL.GenBuffers(1, out VtxVboID);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(NumVertices * BlittableValueType.StrideOf(Vertices)), Vertices, BufferUsageHint.StaticDraw);

                    GL.GenBuffers(1, out VtxColourVboId);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, VtxColourVboId);
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(NumVertices * BlittableValueType.StrideOf(Colourarray)), Colourarray, BufferUsageHint.StaticDraw);

                    GLContext = control;
                }

                GL.EnableClientState(ArrayCap.VertexArray);                     // MEASUREMENTS are showing this can for some reason take 40ms to draw.. at random times
                GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);              // Maybe we should tesselate the polygons to triangles 
                GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
                GL.PointSize(this.Size);

                GL.EnableClientState(ArrayCap.ColorArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VtxColourVboId);
                GL.ColorPointer(4, ColorPointerType.UnsignedByte, 0, 0);

                GL.MultiDrawArrays(Primative, Polystart.ToArray(), Polycount.ToArray(), Polystart.Count);

                GL.DisableClientState(ArrayCap.ColorArray);
                GL.DisableClientState(ArrayCap.VertexArray);
            }
        }

    }




}
