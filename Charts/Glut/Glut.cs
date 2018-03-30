
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace nzy3D.Glut
{

	public class Glut
	{

		public const int STROKE_ROMAN = 0;
        public const int STROKE_MONO_ROMAN = 1;
        public const int BITMAP_9_BY_15 = 2;
        public const int BITMAP_8_BY_13 = 3;
        public const int BITMAP_TIMES_ROMAN_10 = 4;
        public const int BITMAP_TIMES_ROMAN_24 = 5;
        public const int BITMAP_HELVETICA_10 = 6;
        public const int BITMAP_HELVETICA_12 = 7;

        public const int BITMAP_HELVETICA_18 = 8;

		private static float[,] boxVertices;
		private static float[,] boxNormals = {
			{
				-1.0F,
				0.0F,
				0.0F
			},
			{
				0.0F,
				1.0F,
				0.0F
			},
			{
				1.0F,
				0.0F,
				0.0F
			},
			{
				0.0F,
				-1.0F,
				0.0F
			},
			{
				0.0F,
				0.0F,
				1.0F
			},
			{
				0.0F,
				0.0F,
				-1.0F
			}

		};
		private static int[,] boxFaces = {
			{
				0,
				1,
				2,
				3
			},
			{
				3,
				2,
				6,
				7
			},
			{
				7,
				6,
				5,
				4
			},
			{
				4,
				5,
				1,
				0
			},
			{
				5,
				6,
				2,
				1
			},
			{
				7,
				4,
				0,
				3
			}

		};
		private static void drawBox(float size, BeginMode type)
		{
			if (boxVertices == null) {
				float[,] v = new float[8, 3];
				v[0, 0] = -0.5F;
				v[1, 0] = -0.5F;
				v[2, 0] = -0.5F;
				v[3, 0] = -0.5F;
				v[4, 0] = 0.5F;
				v[5, 0] = 0.5F;
				v[6, 0] = 0.5F;
				v[7, 0] = 0.5F;
				v[0, 1] = -0.5F;
				v[1, 1] = -0.5F;
				v[4, 1] = -0.5F;
				v[5, 1] = -0.5F;
				v[2, 1] = 0.5F;
				v[3, 1] = 0.5F;
				v[6, 1] = 0.5F;
				v[7, 1] = 0.5F;
				v[0, 2] = -0.5F;
				v[3, 2] = -0.5F;
				v[4, 2] = -0.5F;
				v[7, 2] = -0.5F;
				v[1, 2] = 0.5F;
				v[2, 2] = 0.5F;
				v[5, 2] = 0.5F;
				v[6, 2] = 0.5F;
				boxVertices = v;
			}
			for (int i = 5; i >= 0; i += -1) {
				GL.Begin(type);
				GL.Normal3(boxVertices[i, 0], boxVertices[i, 1], boxVertices[i, 2]);
				int faceN = 0;
				faceN = boxFaces[i, 0];
				GL.Vertex3(boxVertices[faceN, 0] * size, boxVertices[faceN, 1] * size, boxVertices[faceN, 2] * size);
				faceN = boxFaces[i, 1];
				GL.Vertex3(boxVertices[faceN, 0] * size, boxVertices[faceN, 1] * size, boxVertices[faceN, 2] * size);
				faceN = boxFaces[i, 2];
				GL.Vertex3(boxVertices[faceN, 0] * size, boxVertices[faceN, 1] * size, boxVertices[faceN, 2] * size);
				faceN = boxFaces[i, 3];
				GL.Vertex3(boxVertices[faceN, 0] * size, boxVertices[faceN, 1] * size, boxVertices[faceN, 2] * size);
				GL.End();
			}
		}

		public static void SolidCube(float size)
		{
			drawBox(size, BeginMode.Quads);
		}

		public static void WireCube(float size)
		{
			drawBox(size, BeginMode.LineLoop);
		}

		public static bool UnProject(Vector4d winPos, Matrix4d modelMatrix, Matrix4d projMatrix, double[] viewport, ref Vector4d objPos)
		{

			Matrix4d p = Matrix4d.Mult(modelMatrix, projMatrix);
			Matrix4d finalMatrix = Matrix4d.Invert(p);
			Vector4d @in = winPos;

			// Map x and y from window coordinates 
			@in.X = (@in.X - viewport[0]) / viewport[2];
			@in.Y = (@in.Y - viewport[1]) / viewport[3];
			// Map to range -1 to 1 
			@in.X = @in.X * 2.0 - 1.0;
			@in.Y = @in.Y * 2.0 - 1.0;
			@in.Z = @in.Z * 2.0 - 1.0;
			@in.W = 1.0;
			Vector4d @out = Vector4d.Transform(@in, finalMatrix);

			if (@out.W == 0.0) {
				return false;
			}

			@out.X /= @out.W;
			@out.Y /= @out.W;
			@out.Z /= @out.W;
			objPos = @out;

			return true;

		}

		public static bool Project(Vector4d objPos, Matrix4d modelMatrix, Matrix4d projMatrix, double[] viewport, ref Vector4d winPos)
		{


			objPos.W = 1;

			Vector4d @out = default(Vector4d);

			@out = Vector4d.Transform(objPos, modelMatrix);
			@out = Vector4d.Transform(@out, projMatrix);

			if (@out.W == 0) {
				return false;
			}

			@out.W = (1 / @out.W) * 0.5;

			// Map X/Y/Z to range 0-1
			@out.X = @out.X * @out.W + 0.5;
			@out.Y = @out.Y * @out.W + 0.5;
			@out.Z = @out.Z * @out.W + 0.5;

			// Map x, y to viewport
			winPos = new Vector4d();
			winPos.X = @out.X * viewport[2] + viewport[0];
			winPos.Y = @out.Y * viewport[3] + viewport[1];
			winPos.Z = @out.Z;

			return true;

		}

		public static void Perspective(double fovy, double aspect, double zNear, double zFar)
		{
			double sine = 0;
			double cotangent = 0;
			double deltaZ = 0;
			double radians = fovy / 2 * System.Math.PI / 180;

			deltaZ = zFar - zNear;
			sine = System.Math.Sin(radians);

			if (((deltaZ == 0) | (sine == 0) | (aspect == 0))) {
				return;
			}

			cotangent = System.Math.Cos(radians) / sine;

			Matrix4d matrix = MakeIdentityD();
			matrix.M11 = cotangent / aspect;
			matrix.M22 = cotangent;
			matrix.M33 = -(zFar + zNear) / deltaZ;
			matrix.M34 = -1;
			matrix.M43 = -2 * zNear * zFar / deltaZ;
			matrix.M44 = 0;

			GL.MultMatrix(ref matrix);

		}

		public static Matrix4d MakeIdentityD()
		{
			return new Matrix4d(1, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			1, 0, 0, 0, 0, 1);
		}

		public static Matrix4d IDENTITY = new Matrix4d(1, 0, 0, 0, 0, 1, 0, 0, 0, 0,

		1, 0, 0, 0, 0, 1);

		public static void LookAt(double eyeX, double eyeY, double eyeZ, double centerX, double centerY, double centerZ, double upX, double upY, double upZ)
		{
			Vector3d forward = new Vector3d(centerX - eyeX, centerY - eyeY, centerZ - eyeZ);

			Vector3d up = new Vector3d(upX, upY, upZ);

			forward.Normalize();

			//Side = forward x up
			Vector3d side = Vector3d.Cross(forward, up);
			side.Normalize();

			//Recompute up as: up = side x forward 
			up = Vector3d.Cross(side, forward);

			Matrix4d matrix = MakeIdentityD();

			matrix.M11 = side.X;
			matrix.M21 = side.Y;
			matrix.M31 = side.Z;

			matrix.M12 = up.X;
			matrix.M22 = up.Y;
			matrix.M32 = up.Z;

			matrix.M13 = -forward.X;
			matrix.M23 = -forward.Y;
			matrix.M33 = -forward.Z;

			GL.MultMatrix(ref matrix);
			GL.Translate(-eyeX, -eyeY, -eyeZ);

		}

		public static void BitmapString(int font, string text)
		{
			int swapbytes = 0;
			int lsbfirst = 0;
			int rowlength = 0;
			int skiprows = 0;
			int skippixels = 0;
			int alignment = 0;
			beginBitmap(ref swapbytes, ref lsbfirst, ref rowlength, ref skiprows, ref skippixels, ref alignment);
			int len = text.Length;
			for (int i = 0; i <= len - 1; i++) {
				bitmapCharacterImpl(font, text[i]);
			}
			endBitmap(swapbytes, lsbfirst, rowlength, skiprows, skippixels, alignment);
		}

		private static void beginBitmap(ref int swapbytes, ref int lsbfirst, ref int rowlength, ref int skiprows, ref int skippixels, ref int alignment)
		{
			GL.GetInteger(GetPName.UnpackSwapBytes, out swapbytes);
            GL.GetInteger(GetPName.UnpackLsbFirst, out lsbfirst);
            GL.GetInteger(GetPName.UnpackRowLength, out rowlength);
            GL.GetInteger(GetPName.UnpackSkipRows, out skiprows);
            GL.GetInteger(GetPName.UnpackSkipPixels, out skippixels);
            GL.GetInteger(GetPName.UnpackAlignment, out alignment);
			//Little endian machines (DEC Alpha for example) could
			// benefit from setting GL_UNPACK_LSB_FIRST to GL_TRUE
			// instead of GL_FALSE, but this would require changing the
			// generated bitmaps too.
			GL.PixelStore(PixelStoreParameter.UnpackSwapBytes, 0);
			// GL_FALSE = 0 ?
			GL.PixelStore(PixelStoreParameter.UnpackLsbFirst, 0);
			// GL_FALSE = 0 ?
			GL.PixelStore(PixelStoreParameter.UnpackRowLength, 0);
			GL.PixelStore(PixelStoreParameter.UnpackSkipRows, 0);
			GL.PixelStore(PixelStoreParameter.UnpackSkipPixels, 0);
			GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
		}

		private static void endBitmap(int swapbytes, int lsbfirst, int rowlength, int skiprows, int skippixels, int alignment)
		{
			GL.PixelStore(PixelStoreParameter.UnpackSwapBytes, swapbytes);
			GL.PixelStore(PixelStoreParameter.UnpackLsbFirst, lsbfirst);
			GL.PixelStore(PixelStoreParameter.UnpackRowLength, rowlength);
			GL.PixelStore(PixelStoreParameter.UnpackSkipRows, skiprows);
			GL.PixelStore(PixelStoreParameter.UnpackSkipPixels, skippixels);
			GL.PixelStore(PixelStoreParameter.UnpackAlignment, alignment);
		}

		private static void bitmapCharacterImpl(int font, char cin)
		{
			BitmapFontRec fontinfo = getBitmapFont(font);
			int c =  (int)cin & 0xffff;
			if (c < fontinfo.first | c >= fontinfo.first + fontinfo.num_chars) {
				return;
			}
			BitmapCharRec ch = fontinfo.ch[c - fontinfo.first];
			if ((ch != null)) {
				GL.Bitmap(ch.width, ch.height, ch.xorig, ch.yorig, ch.advance, 0, ch.bitmap);
			}

		}

		private static BitmapFontRec[] bitmapFonts = new BitmapFontRec[9];

		private static StrokeFontRec[] strokeFonts = new StrokeFontRec[9];
		private static BitmapFontRec getBitmapFont(int font)
		{
			BitmapFontRec rec = bitmapFonts[font];
			if ((rec == null)) {
				switch (font) {
					case BITMAP_9_BY_15:
						rec = GLUTBitmap9x15.glutBitmap9By15;
						break;
					case BITMAP_8_BY_13:
						rec = GLUTBitmap8x13.glutBitmap8By13;
						break;
					case BITMAP_TIMES_ROMAN_10:
						rec = GLUTBitmapTimesRoman10.glutBitmapTimesRoman10;
						break;
					case BITMAP_TIMES_ROMAN_24:
						rec = GLUTBitmapTimesRoman24.glutBitmapTimesRoman24;
						break;
					case BITMAP_HELVETICA_10:
						rec = GLUTBitmapHelvetica10.glutBitmapHelvetica10;
						break;
					case BITMAP_HELVETICA_12:
						rec = GLUTBitmapHelvetica12.glutBitmapHelvetica12;
						break;
					case BITMAP_HELVETICA_18:
						rec = GLUTBitmapHelvetica18.glutBitmapHelvetica18;
						break;
					default:
						throw new Exception("Unknown bitmap font number :" + font);
				}
				bitmapFonts[font] = rec;
			}
			return rec;
		}

		public static int BitmapLength(int font, string s)
		{
			BitmapFontRec fontinfo = getBitmapFont(font);
			float length = 0;
			int len = s.Length;
			for (int pos = 0; pos <= len - 1; pos++) {
				int c = (int)s.ToCharArray(pos,1)[0] & 0xffff;
				if ((c >= fontinfo.first & c < fontinfo.first + fontinfo.num_chars)) {
					BitmapCharRec ch = fontinfo.ch[c - fontinfo.first];
					if ((ch != null)) {
						length += ch.advance;
					}
				}
			}
			return (int)length;
		}

	}


}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
