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

    public class TexturedQuadData : IDrawingPrimative
    {
        public Vector3d[] Vertices;
        public Vector4d[] Texcoords;
        public Bitmap Texture;

        public Color Color { get; set; }
        public float Size { get; set; }
        public PrimitiveType Type { get { return PrimitiveType.Quads; } }

        private int _texid;
        private GLControl _control;
        private Bitmap _texture;
        private List<TexturedQuadData> _childTextures = new List<TexturedQuadData>();
        private TexturedQuadData _parentTexture;
        protected Vector3d[] _vboVertices;
        protected Vector4d[] _vboTexcoords;
        protected int NumVertices;
        protected int VtxVboID;
        protected int TexVboID;

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
                    if (_texid != 0 || VtxVboID != 0 || TexVboID != 0)
                    {
                        if (_texid != 0)
                        {
                            GL.DeleteTexture(_texid);
                            _texid = 0;
                        }

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
        }

        protected void CreateTexture()
        {
            if (_texture != Texture ||
                NumVertices != (Vertices == null ? 0 : 6) + _childTextures.Count * 6)
            {
                FreeTexture(false);
            }

            if (_texid == 0)
            {
                Bitmap bmp = Texture;
                /*
                int newwidth = 1 << (int)(Math.Log(bmp.Width - 1, 2) + 1);
                int newheight = 1 << (int)(Math.Log(bmp.Height - 1, 2) + 1);
                if (bmp.Width != newwidth || bmp.Height != newheight)
                {
                    bmp = new Bitmap(Texture, new Size(newwidth, newheight));
                }
                    */
                GL.GenTextures(1, out _texid);
                GL.BindTexture(TextureTarget.Texture2D, _texid);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                System.Drawing.Imaging.BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpdata.Width, bmpdata.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, bmpdata.Scan0);
                bmp.UnlockBits(bmpdata);
                _texture = Texture;
            }

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

            foreach (var tex in (Vertices == null ? new TexturedQuadData[0] : new[] { this }).Concat(_childTextures).Select((t,i) => new { index = i, tex = t }))
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

        protected static Vector3d[] GetVertices(FGEImage img,float y = 0)
        {
            return GetVertices(img.TopLeft, img.TopRight, img.BottomLeft, img.BottomRight, y);
        }

        protected static Vector3d[] GetVertices(Point topleft, Point topright, Point bottomleft, Point bottomright, float y = 0)
        {
            return new Vector3d[]
            {
                new Vector3d(topleft.X, y, topleft.Y),
                new Vector3d(topright.X, y, topright.Y),
                new Vector3d(bottomright.X, y, bottomright.Y),
                new Vector3d(bottomleft.X, y, bottomleft.Y)
            };
        }

        protected static Vector4d[] GetTexCoords(FGEImage img, int width, int height)
        {
            return GetTexCoords(img.pxTopLeft, img.pxTopRight, img.pxBottomLeft, img.pxBottomRight, width, height);
        }

        protected static Vector4d[] GetTexCoords(Point pxTopLeft, Point pxTopRight, Point pxBottomLeft, Point pxBottomRight, int width, int height)
        {
            Vector4d[] texcoords = new Vector4d[]
            {
                new Vector4d(pxTopLeft.X * 1.0 / width, pxTopLeft.Y * 1.0 / height, 0, 1),
                new Vector4d(pxTopRight.X * 1.0 / width, pxTopRight.Y * 1.0 / height, 0, 1),
                new Vector4d(pxBottomRight.X * 1.0 / width, pxBottomRight.Y * 1.0 / height, 0, 1),
                new Vector4d(pxBottomLeft.X * 1.0 / width, pxBottomRight.Y * 1.0 / height, 0, 1)
            };

            Vector2d a = new Vector2d(pxTopRight.X - pxBottomLeft.X, pxTopRight.Y - pxBottomLeft.Y);
            Vector2d b = new Vector2d(pxTopLeft.X - pxBottomRight.X, pxTopLeft.Y - pxBottomRight.Y);
            double cross = a.X * b.Y - b.X * a.Y;

            if (cross != 0)
            {
                Vector2d c = new Vector2d(pxBottomLeft.X - pxBottomRight.X, pxBottomLeft.Y - pxBottomRight.Y);
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

        public static TexturedQuadData FromFGEImage(FGEImage img , float y = 0)
        {
            Bitmap bmp = (Bitmap)Bitmap.FromFile(img.FilePath);
            Vector3d[] vertices = GetVertices(img,y);
            Vector4d[] texcoords = GetTexCoords(img, bmp.Width, bmp.Height);
            return new TexturedQuadData(vertices, texcoords, bmp);
        }

        public static TexturedQuadData FromBitmap(Bitmap bmp, Point topleft, Point topright, Point bottomleft, Point bottomright, float y = 0)
        {
            Point pxTopLeft = new Point(0, bmp.Height - 1);
            Point pxTopRight = new Point(bmp.Width - 1, bmp.Height - 1);
            Point pxBottomLeft = new Point(0, 0);
            Point pxBottomRight = new Point(bmp.Width - 1, 0);
            Vector3d[] vertices = GetVertices(topleft, topright, bottomleft, bottomright, y);
            Vector4d[] texcoords = GetTexCoords(pxTopLeft, pxTopRight, pxBottomLeft, pxBottomRight, bmp.Width, bmp.Height);
            return new TexturedQuadData(vertices, texcoords, bmp);
        }

        public TexturedQuadData CreateSubTexture(Point topleft, Point topright, Point bottomleft, Point bottomright, Point pxTopLeft, Point pxTopRight, Point pxBottomLeft, Point pxBottomRight, float y = 0)
        {
            FGEImage fge = new FGEImage("null");
            fge.TopLeft = topleft;
            fge.TopRight = topright;
            fge.BottomLeft = bottomleft;
            fge.BottomRight = bottomright;
            fge.pxTopLeft = pxTopLeft;
            fge.pxTopRight = pxTopRight;
            fge.pxBottomLeft = pxBottomLeft;
            fge.pxBottomRight = pxBottomRight;
            Vector3d[] vertices = GetVertices(fge, y);
            Vector4d[] texcoords = GetTexCoords(fge, Texture.Width, Texture.Height);
            return new TexturedQuadData(vertices, texcoords, this);
        }
    }

    public class TexturedQuadDataCollection : Data3DSetClass<TexturedQuadData>, IDisposable
    {
        protected HashSet<TexturedQuadData> BaseTextures = new HashSet<TexturedQuadData>();

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

    public interface IData3DSet
    {
        string Name { get; set; }
        void DrawAll(GLControl control);
    }
}
