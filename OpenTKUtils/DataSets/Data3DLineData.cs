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
    public class LineData : IDrawingPrimative
    {
        public Vector3 p1;
        public Vector3 p2;

        public LineData(Vector3 a, Vector3 b)
        {
            p1 = a;
            p2 = b;
        }

        public LineData(float p1x, float p1y, float p1z, float p2x, float p2y, float p2z)
        {
            p1 = new Vector3(p1x, p1y, p1z);
            p2 = new Vector3(p2x, p2y, p2z);
        }

        public LineData(float p1x, float p1y, float p1z, float p2x, float p2y, float p2z, Color c)
        {
            p1 = new Vector3(p1x, p1y, p1z);
            p2 = new Vector3(p2x, p2y, p2z);
            this.Color = c;
        }

        public LineData(double p1x, double p1y, double p1z, double p2x, double p2y, double p2z)         // BC
        {
            p1 = new Vector3((float)p1x, (float)p1y, (float)p1z);
            p2 = new Vector3((float)p2x, (float)p2y, (float)p2z);
        }

        public LineData(double p1x, double p1y, double p1z, double p2x, double p2y, double p2z, Color c)         // BC
        {
            p1 = new Vector3((float)p1x, (float)p1y, (float)p1z);
            p2 = new Vector3((float)p2x, (float)p2y, (float)p2z);
            this.Color = c;
        }

        public PrimitiveType Type { get { return PrimitiveType.Lines; } }
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
                GL.Vertex3(p1);
                GL.Vertex3(p2);
                GL.End();
            }
        }
    }

    public class LineDataCollection : Data3DCollection<LineData>, IList<LineData>, IDisposable
    {
        protected Vector3[] Vertices;
        private uint[] carray;
        protected int NumVertices;
        protected int VtxVboID;
        protected int VtxColorVboId = 0;
        protected GLControl GLContext;

        private bool UseLineDataColour { get { return this.Color.IsFullyTransparent(); } }

        public LineDataCollection(string name, Color color, float pointsize) // Color Transparent to use Line Data colours
            : base(name, color, pointsize)
        {
            this.Primatives = this;
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

        public override void Add(LineData primative)
        {
            Dispose();

            if (Vertices == null)
            {
                Vertices = new Vector3[1024];
                if (UseLineDataColour)
                    carray = new uint[1024];
            }
            else if (NumVertices + 2 > Vertices.Length)
            {
                Array.Resize(ref Vertices, Vertices.Length * 2);
                if (UseLineDataColour)
                    Array.Resize(ref carray, Vertices.Length * 2);
            }

            if (UseLineDataColour)
                carray[NumVertices] = carray[NumVertices + 1] = BitConverter.ToUInt32(new byte[] { primative.Color.R, primative.Color.G, primative.Color.B, primative.Color.A }, 0);

            Vertices[NumVertices++] = primative.p1;
            Vertices[NumVertices++] = primative.p2;
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

                    if (UseLineDataColour)
                    {
                        GL.GenBuffers(1, out VtxColorVboId);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, VtxColorVboId);
                        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(NumVertices * BlittableValueType.StrideOf(carray)), carray, BufferUsageHint.StaticDraw);
                    }
                }

                GL.EnableClientState(ArrayCap.VertexArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
                GL.PointSize(this.Size);

                if (UseLineDataColour)
                {
                    GL.EnableClientState(ArrayCap.ColorArray);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, VtxColorVboId);
                    GL.ColorPointer(4, ColorPointerType.UnsignedByte, 0, 0);
                    GL.DrawArrays(PrimitiveType.Lines, 0, NumVertices);
                    GL.DisableClientState(ArrayCap.ColorArray);
                }
                else
                {
                    //Console.WriteLine("Draw " + this.Name + " at " + this.Color);
                    GL.Color4(this.Color);
                    GL.DrawArrays(PrimitiveType.Lines, 0, NumVertices);
                }

                GL.DisableClientState(ArrayCap.VertexArray);
            }
        }


        #region IList interface

        public int Count
        {
            get
            {
                return NumVertices / 2;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public int IndexOf(LineData l)
        {
            for (int i = 0; i < NumVertices / 2; i++)
            {
                var v1 = Vertices[i * 2];
                var v2 = Vertices[i * 2 + 1];

                if (v1 == l.p1 && v2 == l.p2)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, LineData line)
        {
            if (index > NumVertices / 2)
            {
                throw new IndexOutOfRangeException();
            }

            Dispose();

            this.Add(new LineData(new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
            for (int i = NumVertices - 3; i >= index * 2; i--)
            {
                Vertices[i + 2] = Vertices[i];
            }
            Vertices[index * 2] = line.p1;
            Vertices[index * 2 + 1] = line.p2;
        }

        public void RemoveAt(int index)
        {
            if (index >= NumVertices / 2)
            {
                throw new IndexOutOfRangeException();
            }

            Dispose();

            for (int i = index * 2; i < NumVertices - 2; i++)
            {
                Vertices[i] = Vertices[i + 2];
            }
            NumVertices -= 2;
        }

        public void Clear()
        {
            Dispose();
            NumVertices = 0;
        }

        public bool Contains(LineData item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(LineData[] array, int arrayIndex)
        {
            for (int i = 0; i < NumVertices / 2; i++)
            {
                array[i + arrayIndex] = new LineData(Vertices[i * 2].X, Vertices[i * 2].Y, Vertices[i * 2].Z, Vertices[i * 2 + 1].X, Vertices[i * 2 + 1].Y, Vertices[i * 2 + 1].Z);
            }
        }

        public bool Remove(LineData item)
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

        public IEnumerator<LineData> GetEnumerator()
        {
            for (int i = 0; i < NumVertices; i += 2)
            {
                yield return new LineData(Vertices[i].X, Vertices[i].Y, Vertices[i].Z, Vertices[i + 1].X, Vertices[i + 1].Y, Vertices[i + 1].Z);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public LineData this[int index]
        {
            get
            {
                if (index >= NumVertices / 2)
                {
                    throw new IndexOutOfRangeException();
                }

                var v1 = Vertices[index * 2];
                var v2 = Vertices[index * 2 + 1];

                return new LineData(v1.X, v1.Y, v1.Z, v2.X, v2.Y, v2.Z) { Color = this.Color, Size = this.Size };
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
                    Vertices[index * 2] = value.p1;
                    Vertices[index * 2 + 1] = value.p2;
                }
            }
        }

        #endregion
    }

}
