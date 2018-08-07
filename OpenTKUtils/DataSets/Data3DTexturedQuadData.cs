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
    public class TexturedQuadData : IDrawingPrimative
    {
        public Vector3[] Vertices;                 // as passed
        public Vector4[] Texcoords;
        public Bitmap Texture;
        public Color Color { get; set; }            // use Color to set Alpha only
        public float Size { get; set; }     // not used
        public PrimitiveType Type { get { return PrimitiveType.Quads; } }
        public Object Tag { get; set; }     // optional tag 
        public Object Tag2 { get; set; }     // optional tag

        private Bitmap _texture;                    // and texture

        protected int NumVertices;                  // vertices
        protected int VtxVboID;
        protected int TexVboID;
        protected int TextureVboID;                         // buffer for texture
        protected Vector3[] _vboVertices;
        protected Vector4[] _vboTexcoords;

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

        public TexturedQuadData(Vector3[] vertices, Vector4[] texcoords, Bitmap texture)
        {
            this.Vertices = vertices;
            this.Texcoords = texcoords;
            this.Texture = texture;
            this.Color = Color.White;           // ENSUREs alpha is 255.. use Color to set Alpha only
        }

        public TexturedQuadData(Vector3[] vertices, Vector4[] texcoords, TexturedQuadData texture)
        {
            this.Vertices = vertices;
            this.Texcoords = texcoords;

            if (texture._parentTexture != null)
            {
                texture = texture._parentTexture;
            }

            texture._childTextures.Add(this);
            this._parentTexture = texture;
            this.Color = Color.White;           // ENSUREs alpha is 255.. use Color to set Alpha only
        }

        public void Dispose()
        {
            FreeTexture(true);
            FreeVerticesTri();
            FreeVerticesTex();
        }

        public void UpdateVertices(Vector3[] vertices)
        {
            this.Vertices = vertices;
            FreeVerticesTri();
        }

        protected void CreateTexture()
        {
            if (TextureVboID == 0)                                        // no texture, bind
            {
                Bitmap bmp = Texture;
                GL.GenTextures(1, out TextureVboID);
                GL.BindTexture(TextureTarget.Texture2D, TextureVboID);
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
            if (_control != null && TextureVboID != 0)
            {
                if (_control.InvokeRequired)
                {
                    _control.Invoke(new Action<bool>(this.FreeTexture), remove);
                }
                else
                {
                    if (TextureVboID != 0)
                    {
                        GL.DeleteTexture(TextureVboID);
                        TextureVboID = 0;
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

        protected void CreateVerticesTri()
        {
            if (VtxVboID == 0)
            {
                NumVertices = (Vertices == null ? 0 : 6) + _childTextures.Count * 6;

                if (_vboVertices == null || _vboVertices.Length < NumVertices)
                    _vboVertices = new Vector3[NumVertices];

                foreach (var tex in (Vertices == null ? new TexturedQuadData[0] : new[] { this }).Concat(_childTextures).Select((t, i) => new { index = i, tex = t }))
                {
                    int i = tex.index;
                    var t = tex.tex;
                    _vboVertices[i * 6 + 0] = t.Vertices[0];
                    _vboVertices[i * 6 + 1] = t.Vertices[1];
                    _vboVertices[i * 6 + 2] = t.Vertices[2];
                    _vboVertices[i * 6 + 3] = t.Vertices[0];
                    _vboVertices[i * 6 + 4] = t.Vertices[2];
                    _vboVertices[i * 6 + 5] = t.Vertices[3];

                }

                GL.GenBuffers(1, out VtxVboID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(NumVertices * BlittableValueType.StrideOf(_vboVertices)), _vboVertices, BufferUsageHint.StaticDraw);
            }
        }

        public void FreeVerticesTri()
        {
            if (Parent != null)
            {
                Parent.FreeVerticesTri();
            }
            else
            {
                if (_control != null && !_control.IsDisposed && VtxVboID != 0)
                {
                    if (_control.InvokeRequired)
                    {
                        _control.Invoke(new Action(this.FreeVerticesTri));
                    }
                    else
                    {
                        GL.DeleteBuffer(VtxVboID);
                        VtxVboID = 0;
                    }
                }
            }
        }


        protected void CreateVerticesTex()
        {
            if (TexVboID == 0)
            {
                NumVertices = (Vertices == null ? 0 : 6) + _childTextures.Count * 6;

                if (_vboTexcoords == null || _vboTexcoords.Length < NumVertices)
                    _vboTexcoords = new Vector4[NumVertices];

                foreach (var tex in (Vertices == null ? new TexturedQuadData[0] : new[] { this }).Concat(_childTextures).Select((t, i) => new { index = i, tex = t }))
                {
                    int i = tex.index;
                    var t = tex.tex;
                    _vboTexcoords[i * 6 + 0] = t.Texcoords[0];
                    _vboTexcoords[i * 6 + 1] = t.Texcoords[1];
                    _vboTexcoords[i * 6 + 2] = t.Texcoords[2];
                    _vboTexcoords[i * 6 + 3] = t.Texcoords[0];
                    _vboTexcoords[i * 6 + 4] = t.Texcoords[2];
                    _vboTexcoords[i * 6 + 5] = t.Texcoords[3];
                }

                GL.GenBuffers(1, out TexVboID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, TexVboID);
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(NumVertices * BlittableValueType.StrideOf(_vboTexcoords)), _vboTexcoords, BufferUsageHint.StaticDraw);
            }
        }

        public void FreeVerticesTex()
        {
            if (Parent != null)
            {
                Parent.FreeVerticesTex();
            }
            else
            {
                if (_control != null && !_control.IsDisposed && TexVboID != 0)
                {
                    if (_control.InvokeRequired)
                    {
                        _control.Invoke(new Action(this.FreeVerticesTex));
                    }
                    else
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

                if (TextureVboID == 0)                                  
                    CreateTexture();
                if (VtxVboID == 0)
                    CreateVerticesTri();
                if (TexVboID == 0)
                    CreateVerticesTex();

                int vbosize = NumVertices;
                GL.Enable(EnableCap.Texture2D);
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.BindTexture(TextureTarget.Texture2D, TextureVboID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, TexVboID);
                GL.TexCoordPointer(4, TexCoordPointerType.Float, 0, 0);
                GL.Color4(Color);           // used to set the alpha
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
        public static Vector3[] GetVertices(Vector3 centre, Vector3 rotationdeg, float width, float height, float hoffset = 0, float voffset = 0)
        {
            Matrix3 rm = Matrix3.Identity;
            if (rotationdeg.X != 0)
                rm *= Matrix3.CreateRotationX((float)(rotationdeg.X * Math.PI / 180.0f));
            if (rotationdeg.Y != 0)
                rm *= Matrix3.CreateRotationY((float)(rotationdeg.Y * Math.PI / 180.0f));
            if (rotationdeg.Z != 0)
                rm *= Matrix3.CreateRotationZ((float)(rotationdeg.Z * Math.PI / 180.0f));

            width /= 2;
            height /= 2;

            Vector3[] points = new Vector3[]      // bitmap is placed on map, centred if hoff,voff=0. 
            {
                Vector3.Transform(new Vector3(-width + hoffset, 0, height + voffset), rm) + centre,           // top left, rotate and transform
                Vector3.Transform(new Vector3(width + hoffset, 0, height + voffset), rm) + centre,            // top right
                Vector3.Transform(new Vector3(width + hoffset, 0, -height + voffset), rm) + centre,           // bot right
                Vector3.Transform(new Vector3(-width + hoffset, 0, -height + voffset), rm) + centre,          // bot left
            };

            return points;
        }

        // Full version, left/bottom/right/top are pixel co-ords, width/height is size of bitmap. Allows a bit of the bitmap to be picked
        protected static Vector4[] GetTexCoords(float left, float bottom, float right, float top, int width, int height)
        {
            Vector4[] texcoords = new Vector4[]
            {
                new Vector4(left * 1.0F / width, bottom * 1.0F / height, 0, 1),      // order botleft,botright,topright,topleft 
                new Vector4(right * 1.0F / width, bottom * 1.0F / height, 0, 1),     // paints bitmap correctly
                new Vector4(right * 1.0F / width, top * 1.0F / height, 0, 1),
                new Vector4(left * 1.0F / width, top * 1.0F / height, 0, 1)
            };

            Vector2 a = new Vector2(right - left, top - bottom);
            Vector2 b = new Vector2(left - right, top - bottom);
            float cross = a.X * b.Y - b.X * a.Y;

            if (cross != 0)
            {
                Vector2 c = new Vector2(left - right, 0);
                float qa = (a.X * c.Y - c.X * a.Y) / cross;
                float qb = (b.X * c.Y - c.X * b.Y) / cross;
                if (qa > 0 && qa < 1 && qb > 0 && qb < 1)
                {
                    texcoords = new Vector4[]
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
        protected static Vector4[] GetTexCoords(int width, int height)     // simpler version for a bitmap
        {
            float h = (height * 1.0F - 1) / height;
            float w = (width * 1.0F - 1) / width;

            Vector4[] texcoords = new Vector4[]
            {
                new Vector4(0, 0, 0, 1),       // this order paints the bitmaps generated by drawstring correctly on our upside down world!
                new Vector4(w, 0, 0, 1),
                new Vector4(w, h, 0, 1),
                new Vector4(0, h, 0, 1)
            };

            Vector2 a = new Vector2(width-1, height-1);
            Vector2 b = new Vector2(-(width-1), height-1);
            float cross = a.X * b.Y - b.X * a.Y;

            if (cross != 0)
            {
                Vector2 c = new Vector2(-(width-1), 0);
                float qa = (a.X * c.Y - c.X * a.Y) / cross;
                float qb = (b.X * c.Y - c.X * b.Y) / cross;
                if (qa > 0 && qa < 1 && qb > 0 && qb < 1)
                {
                    texcoords = new Vector4[]
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
            return FromBitmap(bmp, centre.Pos, rotationdeg, width, height, hoffset, voffset);
        }

        public static TexturedQuadData FromBitmap(Bitmap bmp, Vector3 centre, Vector3 rotationdeg, float width, float height, float hoffset = 0, float voffset = 0)
        {
            Vector4[] texcoords = GetTexCoords(bmp.Width, bmp.Height);
            Vector3[] vertices = GetVertices(centre, rotationdeg, width, height, hoffset, voffset);
            return new TexturedQuadData(vertices, texcoords, bmp);
        }

        public static TexturedQuadData FromBaseTexture(TexturedQuadData basetex, PointData centre, Vector3 rotationdeg, float width, float height, float hoffset = 0, float voffset = 0)
        {
            return FromBaseTexture(basetex, centre.Pos, rotationdeg, width, height, hoffset, voffset);
        }

        public static TexturedQuadData FromBaseTexture(TexturedQuadData basetex, Vector3 centre, Vector3 rotationdeg, float width, float height, float hoffset = 0, float voffset = 0)
        {
            Vector4[] texcoords = GetTexCoords(basetex.Texture.Width, basetex.Texture.Height);
            Vector3[] vertices = GetVertices(centre, rotationdeg, width, height, hoffset, voffset);
            return new TexturedQuadData(vertices, texcoords, basetex);
        }

        public static TexturedQuadData FromBaseTexture(TexturedQuadData basetex, Vector3 centre, Vector3 rotationdeg, Rectangle bounds, float width, float height, float hoffset = 0, float voffset = 0)
        {
            Vector4[] texcoords = GetTexCoords(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom, basetex.Texture.Width, basetex.Texture.Height);
            Vector3[] vertices = GetVertices(centre, rotationdeg, width, height, hoffset, voffset);
            return new TexturedQuadData(vertices, texcoords, basetex);
        }

        public void UpdateVertices(PointData centre, Vector3 rotationdeg, float width, float height, float hoffset = 0, float voffset = 0)
        {
            Vector3[] vertices = GetVertices(centre.Pos, rotationdeg, width, height, hoffset, voffset);
            UpdateVertices(vertices);
        }

        static public Vector3[] CalcVertices(Vector3 centre, Vector3 rotationdeg, float width, float height, float hoffset = 0, float voffset = 0)
        {
            return GetVertices(centre, rotationdeg, width, height, hoffset, voffset);
        }

        public void UpdateVertices(Vector3 centre, Vector3 rotationdeg, float width, float height, float hoffset = 0, float voffset = 0)
        {
            Vector3[] vertices = GetVertices(centre, rotationdeg, width, height, hoffset, voffset);
            UpdateVertices(vertices);
        }

        // lye flat at Y, pick a part of the bitmap to show.  Set texture yourself BEFORE calling this.
        public TexturedQuadData Horz( float left,float bottom, float right, float top,
                                      float pxleft, float pxbottom, float pxright, float pxtop,
                                      float y = 0)
        {
            Vector3[] vertices = new Vector3[]
            {
                  new Vector3(left, y, top),           // top left/topright/rightbottom/left/bottom as per other transform
                  new Vector3(right, y, top),
                  new Vector3(right, y, bottom),
                  new Vector3(left, y, bottom)
            };

            Vector4[] texcoords = GetTexCoords(pxleft,pxbottom,pxright,pxtop, Texture.Width, Texture.Height);
            return new TexturedQuadData(vertices, texcoords, this);
        }

    }

    public class TexturedQuadDataCollection : Data3DCollection<TexturedQuadData>, IDisposable
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
                primative.Parent.Color = this.Color;
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

        public void SetColour(Color c)
        {
            foreach (var primitive in BaseTextures)
            {
                primitive.Color = c;
            }
        }
    }

}
