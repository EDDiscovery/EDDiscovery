using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Collections;

namespace EDDiscovery2._3DMap
{
    public interface IDrawingPrimative
    {
        PrimitiveType Type { get; }
        float Size { get; set; }
        Color Color { get; set; }
        void Draw(GLControl control);
    }

    public class PointData : IDrawingPrimative
    {
        public double x, y, z;

        public PointData(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
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
                GL.Vertex3(x, y, z);
                GL.End();
            }
        }
    }

    public class PointDataCollection : Data3DSetClass<PointData>, IList<PointData>, IDisposable
    {
        protected Vector3d[] Vertices;
        protected int NumVertices;
        protected int VtxVboID;
        protected GLControl GLContext;

        public PointDataCollection(string name, Color color, float pointsize)
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
                    GLContext = null;
                }
            }
        }

        public override void Add(PointData primative)
        {
            Dispose();

            if (Vertices == null)
            {
                Vertices = new Vector3d[1024];
            }
            else if (NumVertices == Vertices.Length)
            {
                Array.Resize(ref Vertices, Vertices.Length * 2);
            }

            Vertices[NumVertices++] = new Vector3d(primative.x, primative.y, primative.z);
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
                    GLContext = control;
                }

                int vbosize = NumVertices;
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                GL.VertexPointer(3, VertexPointerType.Double, 0, 0);
                GL.PointSize(pointSize);
                GL.Color3(color);
                GL.DrawArrays(PrimitiveType.Points, 0, vbosize);
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
                if (v.X == p.x && v.Y == p.y && v.Z == p.z)
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
            Vertices[index] = new Vector3d(point.x, point.y, point.z);
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
                yield return new PointData(v.X, v.Y, v.Z) { Color = color, Size = pointSize };
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
                return new PointData(v.X, v.Y, v.Z) { Color = color, Size = pointSize };
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
                    Vertices[index] = new Vector3d(value.x, value.y, value.z);
                }
            }
        }

        #endregion
    }

    public class LineData : IDrawingPrimative
    {
        public double x1, y1, z1;
        public double x2, y2, z2;

        public LineData(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.z1 = z1;
            this.x2 = x2;
            this.y2 = y2;
            this.z2 = z2;
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
                GL.Vertex3(x1, y1, z1);
                GL.Vertex3(x2, y2, z2);
                GL.End();
            }
        }
    }

    public class LineDataCollection : Data3DSetClass<LineData>, IList<LineData>, IDisposable
    {
        protected Vector3d[] Vertices;
        protected int NumVertices;
        protected int VtxVboID;
        protected GLControl GLContext;

        public LineDataCollection(string name, Color color, float pointsize)
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
                    GLContext = null;
                }
            }
        }

        public override void Add(LineData primative)
        {
            Dispose();

            if (Vertices == null)
            {
                Vertices = new Vector3d[1024];
            }
            else if (NumVertices == Vertices.Length)
            {
                Array.Resize(ref Vertices, Vertices.Length * 2);
            }

            Vertices[NumVertices++] = new Vector3d(primative.x1, primative.y1, primative.z1);
            Vertices[NumVertices++] = new Vector3d(primative.x2, primative.y2, primative.z2);
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
                }

                int vbosize = NumVertices;
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                GL.VertexPointer(3, VertexPointerType.Double, 0, 0);
                GL.PointSize(pointSize);
                GL.Color3(color);
                GL.DrawArrays(PrimitiveType.Lines, 0, vbosize);
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

                if (v1.X == l.x1 && v1.Y == l.y1 && v1.Z == l.z1 && v2.X == l.x2 && v2.Y == l.y2 && v2.Z == l.z2)
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

            this.Add(new LineData(0, 0, 0, 0, 0, 0));
            for (int i = NumVertices - 3; i >= index * 2; i--)
            {
                Vertices[i + 2] = Vertices[i];
            }
            Vertices[index * 2] = new Vector3d(line.x1, line.y1, line.z1);
            Vertices[index * 2 + 1] = new Vector3d(line.x2, line.y2, line.z2);
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

                return new LineData(v1.X, v1.Y, v1.Z, v2.X, v2.Y, v2.Z) { Color = color, Size = pointSize };
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
                    Vertices[index * 2] = new Vector3d(value.x1, value.y1, value.z1);
                    Vertices[index * 2 + 1] = new Vector3d(value.x2, value.y2, value.z2);
                }
            }
        }

        #endregion
    }

    public class Data3DSetClass<T> : IData3DSet where T : IDrawingPrimative
    {
        public string Name { get; set; }
        public IList<T> Primatives { get; protected set; }
        protected readonly Color color;
        protected readonly float pointSize;

        public bool Visible;

        protected Data3DSetClass(string name, Color color, float pointsize)
        {
            Name = name;
            this.color = color;
            pointSize = pointsize;
            Primatives = new List<T>();
            Visible = true;
        }

        public virtual void Add(T primative)
        {
            primative.Color = color;
            primative.Size = pointSize;
            Primatives.Add(primative);
        }

        public virtual void DrawAll(GLControl control)
        {
            if (!Visible) return;

            if (control.InvokeRequired)
            {
                control.Invoke(new Action<GLControl>(this.DrawAll), control);
            }
            else
            {
                foreach (var primative in Primatives)
                {
                    primative.Draw(control);
                }
            }
        }

        public static Data3DSetClass<T> Create(string name, Color color, float pointsize)
        {
            if (typeof(T) == typeof(PointData))
            {
                return new PointDataCollection(name, color, pointsize) as Data3DSetClass<T>;
            }
            else if (typeof(T) == typeof(LineData))
            {
                return new LineDataCollection(name, color, pointsize) as Data3DSetClass<T>;
            }
            else
            {
                return new Data3DSetClass<T>(name, color, pointsize);
            }
        }
    }

    public interface IData3DSet
    {
        string Name { get; set; }
        void DrawAll(GLControl control);
    }
}
