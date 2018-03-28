
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;

namespace nzy3D.Plot3D.Builder.Delaunay
{

	public class OrthonormalCoordinateValidator : ICoordinateValidator
	{

		private float[] _x;
		private float[] _y;
		private float[] _z;
		private float[,] _z_as_fxy;
		private int _findxi;

		private int _findyj;
		public OrthonormalCoordinateValidator(Coordinates coords)
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
			setData(coords);
		}

		internal void setData(Coordinates coords)
		{
			_x = MakeCoordinatesUnique(coords.x);
			_y = MakeCoordinatesUnique(coords.y);
			_z_as_fxy = new float[_x.Length, _y.Length];
			for (int i = 0; i <= _x.Length - 1; i++) {
				for (int j = 0; j <= _y.Length - 1; j++) {
					_z_as_fxy[i, j] = float.NaN;
				}
			}
			bool found = false;
			for (int p = 0; p <= coords.z.Length - 1; p++) {
				found = Find(_x, _y, coords.x[p], coords.y[p]);
				if (!found) {
					throw new Exception("It seems (x[p],y[p]) has not been properly stored into (this.x,this.y)");
				}
				_z_as_fxy[_findxi, _findyj] = coords.z[p];
			}
		}

		internal bool Find(float[] x, float[] y, float vx, float vy)
		{
			for (int i = 0; i <= x.Length - 1; i++) {
				for (int j = 0; j <= y.Length - 1; j++) {
					if (x[i] == vx & y[j] == vy) {
						_findxi = i;
						_findyj = j;
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Compute a sorted array from input, with a unique occurrence of each
		/// value. Note: any NaN value will be ignored and won't appear in the output
		/// array.
		/// </summary>
		/// <param name="data">Input array</param>
		/// <returns>A sorted array containing only one occurrence of each input value, without NaN</returns>
		internal float[] MakeCoordinatesUnique(float[] data)
		{
			float[] copy = (float[])data.Clone() ;
			System.Array.Sort(copy);
			int nunique = 0;
			float last = 0;
			last = float.NaN;
			for (int i = 0; i <= copy.Length - 1; i++) {
				if (float.IsNaN(copy[i])) {
					// Ignore NaN values
				} else if (copy[i] != last) {
					nunique += 1;
					last = copy[i];
				}
			}
			float[] result = new float[nunique];
			int r = 0;
			last = float.NaN;
			for (int i = 0; i <= copy.Length - 1; i++) {
				if (double.IsNaN(copy[i])) {
					// Ignore NaN values
				} else if (copy[i] != last) {
					result[r] = copy[i];
					r += 1;
					last = copy[i];
				}
			}
			return result;
		}

		public float[,] get_Z_as_fxy()
		{
			return this._z_as_fxy;
		}

		public float[] getX()
		{
			return this._y;
		}

		public float[] getY()
		{
			return this._y;
		}

		public float[] getZ()
		{
			return this._z;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
