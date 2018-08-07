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
    public class PointData : IDrawingPrimative
    {
        public Vector3 Pos;
        public float x { get { return Pos.X; } }
        public float y { get { return Pos.Y; } }
        public float z { get { return Pos.Z; } }

        public PointData(Vector3 p)
        {
            Pos = p;
        }

        public PointData(float x, float y, float z)
        {
            Pos.X = x;
            Pos.Y = y;
            Pos.Z = z;
        }

        public PointData(double x, double y, double z)      // BC
        {
            Pos.X = (float)x;
            Pos.Y = (float)y;
            Pos.Z = (float)z;
        }

        public PointData(float x, float y, float z, Color c)
        {
            Pos.X = x;
            Pos.Y = y;
            Pos.Z = z;
            this.Color = c;
        }

        public PointData(float x, float y, float z, float size, Color c)
        {
            Pos.X = x;
            Pos.Y = y;
            Pos.Z = z;
            this.Size = size;
            this.Color = c;
        }

        public PointData(double x, double y, double z, Color c)     
        {
            Pos.X = (float)x;
            Pos.Y = (float)y;
            Pos.Z = (float)z;
            this.Color = c;
        }

        public PrimitiveType Type { get { return PrimitiveType.Points; } }
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
                GL.PointSize(Size);

                GL.Begin(Type);
                GL.Color3(Color);
                GL.Vertex3(Pos);
                GL.End();
            }
        }
    }

    public class PointDataCollection : Data3DCollection<PointData>, IList<PointData>, IDisposable
    {
        protected Vector3[] Vertices;
        private uint[] carray;
        protected int NumVertices;
        protected int VtxVboID;
        protected int VtxColorVboId = 0;
        protected GLControl GLContext;

        private bool UsePointDataColour { get { return this.Color.IsFullyTransparent(); } }

        public PointDataCollection(string name, Color color, float pointsize)       // Color Transparent to use Point Data colours.
            : base(name, color, pointsize)
        {
            Primatives = this;
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
                    if (VtxColorVboId != 0)
                        GL.DeleteBuffer(VtxColorVboId);
                    GLContext = null;
                    VtxVboID = 0;
                    VtxColorVboId = 0;
                }
            }
        }

        public override void Add(PointData primative)
        {
            Dispose();

            if (Vertices == null)
            {
                Vertices = new Vector3[1024];
                if (UsePointDataColour)
                    carray = new uint[1024];
            }
            else if (NumVertices == Vertices.Length)
            {
                Array.Resize(ref Vertices, Vertices.Length * 2);
                if (UsePointDataColour)
                    Array.Resize(ref carray, Vertices.Length * 2);
            }

            if (UsePointDataColour)
                carray[NumVertices] = BitConverter.ToUInt32(new byte[] { primative.Color.R, primative.Color.G, primative.Color.B, primative.Color.A }, 0);

            Vertices[NumVertices++] = primative.Pos;
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
                if (VtxVboID == 0)
                {
                    GL.GenBuffers(1, out VtxVboID);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(NumVertices * BlittableValueType.StrideOf(Vertices)), Vertices, BufferUsageHint.StaticDraw);

                    if (UsePointDataColour)
                    {
                        GL.GenBuffers(1, out VtxColorVboId);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, VtxColorVboId);
                        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(NumVertices * BlittableValueType.StrideOf(carray)), carray, BufferUsageHint.StaticDraw);
                    }

                    GLContext = control;
                }

                GL.EnableClientState(ArrayCap.VertexArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
                GL.PointSize(this.Size);

                if (UsePointDataColour)
                {
                    GL.EnableClientState(ArrayCap.ColorArray);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, VtxColorVboId);
                    GL.ColorPointer(4, ColorPointerType.UnsignedByte, 0, 0);
                    GL.DrawArrays(PrimitiveType.Points, 0, NumVertices);
                    GL.DisableClientState(ArrayCap.ColorArray);
                }
                else
                {
                    GL.Color4(this.Color);
                    GL.DrawArrays(PrimitiveType.Points, 0, NumVertices);
                }

                GL.DisableClientState(ArrayCap.VertexArray);
            }
        }

        #region IList interface

        public int Count
        {
            get
            {
                return NumVertices;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public int IndexOf(PointData p)
        {
            for (int i = 0; i < NumVertices; i++)
            {
                var v = Vertices[i];
                if (v == p.Pos)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, PointData point)
        {
            if (index > NumVertices)
            {
                throw new IndexOutOfRangeException();
            }

            Dispose();

            this.Add(new PointData(0, 0, 0));
            for (int i = NumVertices - 2; i >= index; i--)
            {
                Vertices[i + 1] = Vertices[i];
            }
            Vertices[index] = point.Pos;
        }

        public void RemoveAt(int index)
        {
            if (index >= NumVertices)
            {
                throw new IndexOutOfRangeException();
            }

            Dispose();

            for (int i = index; i < NumVertices - 1; i++)
            {
                Vertices[i] = Vertices[i + 1];
            }
            NumVertices--;
        }

        public void Clear()
        {
            Dispose();
            NumVertices = 0;
        }

        public bool Contains(PointData item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(PointData[] array, int arrayIndex)
        {
            for (int i = 0; i < NumVertices; i++)
            {
                array[i + arrayIndex] = new PointData(Vertices[i].X, Vertices[i].Y, Vertices[i].Z);
            }
        }

        public bool Remove(PointData item)
        {
            int index = IndexOf(item);

            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerator<PointData> GetEnumerator()
        {
            for (int i = 0; i < NumVertices; i++)
            {
                var v = Vertices[i];
                yield return new PointData(v.X, v.Y, v.Z) { Color = this.Color, Size = this.Size };
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public PointData this[int index]
        {
            get
            {
                if (index >= NumVertices)
                {
                    throw new IndexOutOfRangeException();
                }

                var v = Vertices[index];
                return new PointData(v.X, v.Y, v.Z) { Color = this.Color, Size = this.Size };
            }
            set
            {
                if (index >= NumVertices)
                {
                    Insert(index, value);
                }
                else
                {
                    Dispose();
                    Vertices[index] = value.Pos;
                }
            }
        }

        #endregion
    }

}
