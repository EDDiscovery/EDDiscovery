
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;

namespace nzy3D.Plot3D.Builder.Delaunay
{

	public class DelaunayCoordinateValidator : ICoordinateValidator
	{

		private float[] _x;
		private float[] _y;

		private float[,] _z_as_fxy;
		public DelaunayCoordinateValidator(Coordinates coords)
		{
			if (coords == null) {
				throw new ArgumentException("Function call with illegal value 'Nothing' for parameter coords.", "coords");
			}
			if (coords.x == null) {
				throw new ArgumentException("Illegal result value 'Nothing' on x property of parameter coords.", "coords");
			}
			if (coords.y == null) {
				throw new ArgumentException("Illegal result value 'Nothing' on y property of parameter coords.", "coords");
			}
			if (coords.z == null) {
				throw new ArgumentException("Illegal result value 'Nothing' on z property of parameter coords.", "coords");
			}
			if (coords.x.Length != coords.y.Length) {
				throw new ArgumentException("Parameter coords has different x size (" + coords.x.Length + ") than y size (" + coords.y.Length + ")", "coords");
			}
			if (coords.x.Length != coords.z.Length) {
				throw new ArgumentException("Parameter coords has different x size (" + coords.x.Length + ") than z size (" + coords.z.Length + ")", "coords");
			}
			_x = coords.x;
			_y = coords.y;
			_z_as_fxy = setData(coords.z);
		}

		internal float[,] setData(float[] z)
		{
			int length = z.Length;
			float[,] z_as_fxy = new float[length, length];
			for (int p = 0; p <= length - 1; p++) {
				z_as_fxy[p, p] = z[p];
			}
			return z_as_fxy;
		}

		public float[,] get_Z_as_fxy()
		{
			return _z_as_fxy;
		}

		public float[] getX()
		{
			return _x;
		}

		public float[] getY()
		{
			return _y;
		}
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
