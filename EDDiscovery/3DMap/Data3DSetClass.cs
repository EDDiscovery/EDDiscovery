using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Collections;
using EDDiscovery;
using System.Diagnostics;

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

        public PointData(double x, double y, double z, float sz)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.Size = sz;
        }

        public PointData(double x, double y, double z, float sz, Color c)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.Size = sz;
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
                    VtxVboID = 0;
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
                    VtxVboID = 0;
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
            else if (NumVertices+2 > Vertices.Length)
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

    public class TexturedQuadData : IDrawingPrimative
    {
        public Vector3d[] Vertices;                 // as passed
        public Vector4d[] Texcoords;
        public Bitmap Texture;
        public Color Color { get; set; }
        public float Size { get; set; }     // not used
        public PrimitiveType Type { get { return PrimitiveType.Quads; } }
        public Object Tag { get; set; }     // optional tag 
        public Object Tag2 { get; set; }     // optional tag

        private int _texid;                         // buffer for texture
        private Bitmap _texture;                    // and texture

        protected int NumVertices;                  // vertices
        protected int VtxVboID;
        protected int TexVboID;
        protected Vector3d[] _vboVertices;
        protected Vector4d[] _vboTexcoords;

        private GLControl _control;

        private List<TexturedQuadData> _childTextures = new List<TexturedQuadData>();
        private TexturedQuadData _parentTexture;

        public TexturedQuadData Parent
        {
            get
            {
                return _parentTexture;
            }
        }

        public IEnumerable<TexturedQuadData> Children
        {
            get
            {
                return _childTextures.AsEnumerable();
            }
        }

        public TexturedQuadData(Vector3d[] vertices, Vector4d[] texcoords, Bitmap texture)
        {
            this.Vertices = vertices;
            this.Texcoords = texcoords;
            this.Texture = texture;
            this.Color = Color.White;
        }

        public TexturedQuadData(Vector3d[] vertices, Vector4d[] texcoords, TexturedQuadData texture)
        {
            this.Vertices = vertices;
            this.Texcoords = texcoords;

            if (texture._parentTexture != null)
            {
                texture = texture._parentTexture;
            }

            texture._childTextures.Add(this);
            this._parentTexture = texture;
            this.Color = Color.White;
        }

        public void Dispose()
        {
            FreeTexture(true);
            FreeVertices(false);
        }

        public void UpdateVertices(Vector3d[] vertices)
        {
            this.Vertices = vertices;
            FreeVertices(false);
        }

        protected void CreateTexture()
        {
            if (_texid == 0)                                        // no texture, bind
            {
                Bitmap bmp = Texture;
                GL.GenTextures(1, out _texid);
                GL.BindTexture(TextureTarget.Texture2D, _texid);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                System.Drawing.Imaging.BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpdata.Width, bmpdata.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, bmpdata.Scan0);
                bmp.UnlockBits(bmpdata);
                _texture = Texture;
            }
        }

        public void FreeTexture(bool remove)
        {
            if (_control != null && _texid != 0)
            {
                if (_control.InvokeRequired)
                {
                    _control.Invoke(new Action<bool>(this.FreeTexture), remove);
                }
                else
                {
                    if (_texid != 0)
                    {
                        GL.DeleteTexture(_texid);
                        _texid = 0;
                    }
                }

                if (remove)
                {
                    if (_parentTexture != null)
                    {
                        _parentTexture._childTextures.Remove(this);

                        _parentTexture = null;
                    }
                    else if (_childTextures.Count != 0)
                    {
                        _childTextures[0]._parentTexture = null;

                        for (int i = 1; i < _childTextures.Count; i++)
                        {
                            _childTextures[i] = _childTextures[0];
                            _childTextures[0]._childTextures.Add(_childTextures[i]);
                        }

                        _childTextures.Clear();
                    }
                }
            }
        }

        protected void CreateVertices()
        {
            if (VtxVboID == 0 || TexVboID == 0)
            {
                NumVertices = (Vertices == null ? 0 : 6) + _childTextures.Count * 6;

                if (_vboVertices == null)
                {
                    _vboVertices = new Vector3d[NumVertices * 2];
                }
                else if (NumVertices > _vboVertices.Length)
                {
                    Array.Resize(ref _vboVertices, NumVertices * 2);
                }

                if (_vboTexcoords == null)
                {
                    _vboTexcoords = new Vector4d[NumVertices * 2];
                }
                else if (NumVertices > _vboTexcoords.Length)
                {
                    Array.Resize(ref _vboTexcoords, NumVertices * 2);
                }

                foreach (var tex in (Vertices == null ? new TexturedQuadData[0] : new[] { this }).Concat(_childTextures).Select((t, i) => new { index = i, tex = t }))
                {
                    int i = tex.index;
                    var t = tex.tex;
                    _vboVertices[i * 6 + 0] = t.Vertices[0]; _vboTexcoords[i * 6 + 0] = t.Texcoords[0];
                    _vboVertices[i * 6 + 1] = t.Vertices[1]; _vboTexcoords[i * 6 + 1] = t.Texcoords[1];
                    _vboVertices[i * 6 + 2] = t.Vertices[2]; _vboTexcoords[i * 6 + 2] = t.Texcoords[2];
                    _vboVertices[i * 6 + 3] = t.Vertices[0]; _vboTexcoords[i * 6 + 3] = t.Texcoords[0];
                    _vboVertices[i * 6 + 4] = t.Vertices[2]; _vboTexcoords[i * 6 + 4] = t.Texcoords[2];
                    _vboVertices[i * 6 + 5] = t.Vertices[3]; _vboTexcoords[i * 6 + 5] = t.Texcoords[3];

                }

                if (VtxVboID == 0)
                {
                    GL.GenBuffers(1, out VtxVboID);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(NumVertices * BlittableValueType.StrideOf(_vboVertices)), _vboVertices, BufferUsageHint.StaticDraw);
                }

                if (TexVboID == 0)
                {
                    GL.GenBuffers(1, out TexVboID);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, TexVboID);
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(NumVertices * BlittableValueType.StrideOf(_vboTexcoords)), _vboTexcoords, BufferUsageHint.StaticDraw);
                }
            }
        }

        public void FreeVertices(bool unused)
        {
            if (_control != null && (VtxVboID != 0 || TexVboID != 0))
            {
                if (_control.InvokeRequired)
                {
                    _control.Invoke(new Action<bool>(this.FreeVertices), false);
                }
                else
                {
                    if (VtxVboID != 0)
                    {
                        GL.DeleteBuffer(VtxVboID);
                        VtxVboID = 0;
                    }

                    if (TexVboID != 0)
                    {
                        GL.DeleteBuffer(TexVboID);
                        TexVboID = 0;
                    }
                }
            }
        }


        public void Draw(GLControl control)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<GLControl>(this.Draw), control);
            }
            else
            {
                _control = control;

                CreateTexture();
                CreateVertices();

                int vbosize = NumVertices;
                GL.Enable(EnableCap.Texture2D);
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.BindTexture(TextureTarget.Texture2D, _texid);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                GL.VertexPointer(3, VertexPointerType.Double, 0, 0);
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, TexVboID);
                GL.TexCoordPointer(4, TexCoordPointerType.Double, 0, 0);
                GL.Color4(Color);
                GL.DrawArrays(PrimitiveType.Triangles, 0, vbosize);
                GL.DisableClientState(ArrayCap.TextureCoordArray);
                GL.DisableClientState(ArrayCap.VertexArray);
                GL.Disable(EnableCap.Texture2D);
            }
        }

        // rotation (0,0,0) selects a bitmap lying flat on the y axis, looking up
        // rotation (90,0,0) selected a vertical bitmap, upside down 
        // rotation (-90,0,0) selected a vertical bitmap, upright
        // rotation (0,90,0) flat spins the bitmap facing right
        // rotation (0,0,90) spins the bitmap along its y axis, facing left.

        static public Vector3 NoRotation { get { return new Vector3(0, 0, 0); } }

        // we rotate around the centre point. hoff/voff allows you to place the bitmap to the side
        public static Vector3d[] GetVertices(Vector3d centre, Vector3 rotationdeg, float width, float height, float hoffset = 0, float voffset = 0)
        {
            Matrix4d rm = Matrix4d.Identity;
            if (rotationdeg.X != 0)
                rm *= Matrix4d.CreateRotationX((float)(rotationdeg.X * Math.PI / 180.0f));
            if (rotationdeg.Y != 0)
                rm *= Matrix4d.CreateRotationY((float)(rotationdeg.Y * Math.PI / 180.0f));
            if (rotationdeg.Z != 0)
                rm *= Matrix4d.CreateRotationZ((float)(rotationdeg.Z * Math.PI / 180.0f));

            width /= 2;
            height /= 2;

            Vector3d[] points = new Vector3d[]      // bitmap is placed on map, centred if hoff,voff=0. 
            {
                Vector3d.Transform(new Vector3d(-width + hoffset, 0, height + voffset), rm) + centre,           // top left, rotate and transform
                Vector3d.Transform(new Vector3d(width + hoffset, 0, height + voffset), rm) + centre,            // top right
                Vector3d.Transform(new Vector3d(width + hoffset, 0, -height + voffset), rm) + centre,           // bot right
                Vector3d.Transform(new Vector3d(-width + hoffset, 0, -height + voffset), rm) + centre,          // bot left
            };

            return points;
        }

        // Full version, left/bottom/right/top are pixel co-ords, width/height is size of bitmap. Allows a bit of the bitmap to be picked
        protected static Vector4d[] GetTexCoords(float left, float bottom, float right, float top, int width, int height)
        {
            Vector4d[] texcoords = new Vector4d[]
            {
                new Vector4d(left * 1.0 / width, bottom * 1.0 / height, 0, 1),      // order botleft,botright,topright,topleft 
                new Vector4d(right * 1.0 / width, bottom * 1.0 / height, 0, 1),     // paints bitmap correctly
                new Vector4d(right * 1.0 / width, top * 1.0 / height, 0, 1),
                new Vector4d(left * 1.0 / width, top * 1.0 / height, 0, 1)
            };

            Vector2d a = new Vector2d(right - left, top - bottom);
            Vector2d b = new Vector2d(left - right, top - bottom);
            double cross = a.X * b.Y - b.X * a.Y;

            if (cross != 0)
            {
                Vector2d c = new Vector2d(left - right, 0);
                double qa = (a.X * c.Y - c.X * a.Y) / cross;
                double qb = (b.X * c.Y - c.X * b.Y) / cross;
                if (qa > 0 && qa < 1 && qb > 0 && qb < 1)
                {
                    texcoords = new Vector4d[]
                    {
                        texcoords[0] * qb,
                        texcoords[1] * qa,
                        texcoords[2] * (1 - qb),
                        texcoords[3] * (1 - qa)
                    };
                }
            }

            return texcoords;
        }

        // Quicker version, whole of bitmap displayed
        protected static Vector4d[] GetTexCoords(int width, int height)     // simpler version for a bitmap
        {
            double h = (height * 1.0 - 1) / height;
            double w = (width * 1.0 - 1) / width;

            Vector4d[] texcoords = new Vector4d[]
            {
                new Vector4d(0, 0, 0, 1),       // this order paints the bitmaps generated by drawstring correctly on our upside down world!
                new Vector4d(w, 0, 0, 1),
                new Vector4d(w, h, 0, 1),
                new Vector4d(0, h, 0, 1)
            };

            Vector2d a = new Vector2d(width-1, height-1);
            Vector2d b = new Vector2d(-(width-1), height-1);
            double cross = a.X * b.Y - b.X * a.Y;

            if (cross != 0)
            {
                Vector2d c = new Vector2d(-(width-1), 0);
                double qa = (a.X * c.Y - c.X * a.Y) / cross;
                double qb = (b.X * c.Y - c.X * b.Y) / cross;
                if (qa > 0 && qa < 1 && qb > 0 && qb < 1)
                {
                    texcoords = new Vector4d[]
                    {
                        texcoords[0] * qb,
                        texcoords[1] * qa,
                        texcoords[2] * (1 - qb),
                        texcoords[3] * (1 - qa)
                    };
                }
            }

            return texcoords;
        }

        // whole versions
        public static TexturedQuadData FromBitmap(Bitmap bmp, PointData centre, Vector3 rotationdeg, float width, float height, float hoffset = 0, float voffset = 0)
        {
            return FromBitmap(bmp, new Vector3d(centre.x, centre.y , centre.z), rotationdeg, width, height, hoffset, voffset);
        }

        public static TexturedQuadData FromBitmap(Bitmap bmp, Vector3d centre, Vector3 rotationdeg, float width, float height, float hoffset = 0, float voffset = 0)
        {
            Vector4d[] texcoords = GetTexCoords(bmp.Width, bmp.Height);
            Vector3d[] vertices = GetVertices(centre, rotationdeg, width, height, hoffset, voffset);
            return new TexturedQuadData(vertices, texcoords, bmp);
        }

        public void UpdateVertices(PointData centre, Vector3 rotationdeg, float width, float height, float hoffset = 0, float voffset = 0)
        {
            Vector3d[] vertices = GetVertices(new Vector3d(centre.x, centre.y, centre.z), rotationdeg, width, height, hoffset, voffset);
            UpdateVertices(vertices);
        }

        static public Vector3d[] CalcVertices(PointData centre, Vector3 rotationdeg, float width, float height, float hoffset = 0, float voffset = 0)
        {
            return GetVertices(new Vector3d(centre.x, centre.y, centre.z), rotationdeg, width, height, hoffset, voffset);
        }

        public void UpdateVertices(Vector3d centre, Vector3 rotationdeg, float width, float height, float hoffset = 0, float voffset = 0)
        {
            Vector3d[] vertices = GetVertices(centre, rotationdeg, width, height, hoffset, voffset);
            UpdateVertices(vertices);
        }

        // lye flat at Y, pick a part of the bitmap to show.  Set texture yourself BEFORE calling this.
        public TexturedQuadData Horz( float left,float bottom, float right, float top,
                                      float pxleft, float pxbottom, float pxright, float pxtop,
                                      float y = 0)
        {
            Vector3d[] vertices = new Vector3d[]
            {
                  new Vector3d(left, y, top),           // top left/topright/rightbottom/left/bottom as per other transform
                  new Vector3d(right, y, top),
                  new Vector3d(right, y, bottom),
                  new Vector3d(left, y, bottom)
            };

            Vector4d[] texcoords = GetTexCoords(pxleft,pxbottom,pxright,pxtop, Texture.Width, Texture.Height);
            return new TexturedQuadData(vertices, texcoords, this);
        }

    }

    public class TexturedQuadDataCollection : Data3DSetClass<TexturedQuadData>, IDisposable
    {
        public HashSet<TexturedQuadData> BaseTextures = new HashSet<TexturedQuadData>();

        public TexturedQuadDataCollection(string name, Color color, float pointsize)
            : base(name, color, pointsize)
        {
        }

        public override void Add(TexturedQuadData primative)
        {
            if (primative.Parent == null && !BaseTextures.Contains(primative))
            {
                BaseTextures.Add(primative);
            }
            else if (!BaseTextures.Contains(primative.Parent))
            {
                primative.Parent.Color = this.color;
                BaseTextures.Add(primative.Parent);
            }

            base.Add(primative);
        }

        public override void DrawAll(GLControl control)
        {
            if (!Visible) return;

            if (control.InvokeRequired)
            {
                control.Invoke(new Action<GLControl>(this.DrawAll), control);
            }
            else
            {
                foreach (var primative in BaseTextures)
                {
                    primative.Draw(control);
                }
            }
        }

        public void Dispose()
        {
            foreach (var primitive in BaseTextures)
            {
                primitive.FreeTexture(false);
            }
        }
    }


    public class Polygon : IDrawingPrimative
    {
        public List<PointData> vertices;

        public Polygon(List<PointData> points, float size, Color c)
        {
            vertices = points;
            Size = size;
            Color = c;
        }

        public Polygon(List<Vector2> points, float size, Color c)
        {
            vertices = new List<PointData>();
            foreach (Vector2 f in points)
                vertices.Add(new PointData(f.X, 0, f.Y));

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
                foreach (PointData v in vertices)
                    GL.Vertex3(v.x, v.y, v.z);
                GL.End();
            }
        }
    }

    public class PolygonCollection : Data3DSetClass<Polygon>,  IDisposable      // can be either Polygons, LineLoop, Triangles
    {
        protected Vector3d[] Vertices;
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
                Vertices = new Vector3d[1024];
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

            foreach (PointData p in primative.vertices)
            {
                Colourarray[NumVertices] = cx;     // need colour per vertex.. if not, it does not work, or you can mix colours
                Vertices[NumVertices++] = new Vector3d(p.x, p.y , p.z);
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
                    Tools.LogToFile("Polygon throw");
                }

                GL.EnableClientState(ArrayCap.VertexArray);                     // MEASUREMENTS are showing this can for some reason take 40ms to draw.. at random times
                GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);              // Maybe we should tesselate the polygons to triangles 
                GL.VertexPointer(3, VertexPointerType.Double, 0, 0);
                GL.PointSize(pointSize);

                GL.EnableClientState(ArrayCap.ColorArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VtxColourVboId);
                GL.ColorPointer(4, ColorPointerType.UnsignedByte, 0, 0);

                GL.MultiDrawArrays(Primative, Polystart.ToArray(), Polycount.ToArray(), Polystart.Count);

                GL.DisableClientState(ArrayCap.ColorArray);
                GL.DisableClientState(ArrayCap.VertexArray);
            }
        }

    }


    public interface IData3DSet
    {
        string Name { get; set; }
        void DrawAll(GLControl control);
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
            else if (typeof(T) == typeof(TexturedQuadData))
            {
                return new TexturedQuadDataCollection(name, color, pointsize) as Data3DSetClass<T>;
            }
            else
            {
                return new Data3DSetClass<T>(name, color, pointsize);
            }
        }
    }


}
