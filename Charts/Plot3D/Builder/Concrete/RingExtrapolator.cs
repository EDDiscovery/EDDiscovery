
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Colors;
using nzy3D.Plot3D.Primitives;

namespace nzy3D.Plot3D.Builder.Concrete
{

	public class RingExtrapolator : OrthonormalTessellator
	{

		internal float _ringMax;
		internal ColorMapper _cmap;
		internal Color _factor;

		internal RingTessellator _interpolator;
		public RingExtrapolator(float ringMax, ColorMapper cmap, Color factor)
		{
			_ringMax = ringMax;
			_cmap = cmap;
			_factor = factor;
			_interpolator = new RingTessellator(0, _ringMax, _cmap, _factor);
		}

		private RingExtrapolator()
		{
			throw new Exception("Forbidden constructor");
		}

		public override Primitives.AbstractComposite build(float[] x, float[] y, float[] z)
		{
			setData(x, y, z);
			Shape s = new Shape();
			s.Add(getExtrapolatedRingPolygons());
			return s;
		}

		public List<Polygon> getExtrapolatedRingPolygons()
		{
			//backup current coords and extrapolate
			float[] xbackup = (float[])x.Clone();
            float[] ybackup = (float[])y.Clone();
            float[,] zbackup = (float[,])z.Clone();
			// compute required extrapolation
			float sstep = x[1] - x[0];
			int nstep = x.Length;
			int ENLARGE = 2;
			int required = (int)(Math.Ceiling((_ringMax * 2 - sstep * nstep) / sstep));
			required = (required < 0 ? ENLARGE : required + ENLARGE);
			if ((required > 0)) {
				extrapolate(required);
			}
			_interpolator.x = x;
			_interpolator.y = y;
			_interpolator.z = z;
			List<Polygon> polygons = _interpolator.getInterpolatedRingPolygons();
			// get back to previous grid
			x = xbackup;
			y = ybackup;
			z = zbackup;
			return polygons;
		}

		/// <summary>
		/// Add extrapolated points on the grid. If the grid is too small for extrapolation, the arrays
		/// are maximized
		/// </summary>
		/// <param name="n"></param>
		/// <remarks></remarks>
		public void extrapolate(int n)
		{
			float[] xnew = new float[x.Length + n * 2];
			float[] ynew = new float[y.Length + n * 2];
			float[,] znew = new float[x.Length + n * 2, y.Length + n * 2];
			// assume x and y grid are allready sorted and create new grids
			float xmin = x[0];
			float xmax = x[x.Length - 1];
			float xgap = x[1] - x[0];
			float ymin = y[0];
			float ymax = y[y.Length - 1];
			float ygap = y[1] - y[0];
			for (int i = 0; i <= xnew.Length - 1; i++) {
				// --- x grid ---
				// fill before
				if ((i < n)) {
					xnew[i] = xmin - (n - i) * xgap;
				// copy content
				} else if ((i >= n & i < x.Length + n)) {
					xnew[i] = x[i - n];
				// fill after
				} else if ((i >= x.Length + n)) {
					xnew[i] = xmax + (i - (x.Length + n) + 1) * xgap;
				}
				// --- y grid ---
				for (int j = 0; j <= ynew.Length - 1; j++) {
					// fill before
					if ((j < n)) {
						ynew[j] = ymin - (n - j) * ygap;
						znew[i, j] = float.NaN;
					// copy content
					} else if ((j >= n & j < (y.Length + n))) {
						ynew[j] = y[j - n];
						// copy z grid
						if ((i >= n & i < x.Length + n)) {
							znew[i, j] = z[i - n, j - n];
						} else {
							znew[i, j] = float.NaN;
						}
					// fill after
					} else if ((j >= (y.Length + n))) {
						ynew[j] = ymax + (j - (y.Length + n) + 1) * ygap;
						znew[i, j] = float.NaN;
					}
				}
			}
			// extrapolation
			float olddiameter = xgap * (x.Length) / 2;
			float newdiameter = xgap * (x.Length - 1 + n * 2) / 2;
			olddiameter *= olddiameter;
			newdiameter *= newdiameter;
			int xmiddle = (xnew.Length - 1) / 2;
			// assume it is an uneven grid
			int ymiddle = (ynew.Length - 1) / 2;
			// assume it is an uneven grid		
			// start from center, and add extrapolated values iteratively on each quadrant
			for (int i = xmiddle; i <= xnew.Length - 1; i++) {
				for (int j = ymiddle; j <= ynew.Length - 1; j++) {
					float sqrad = xnew[i] * xnew[i] + ynew[j] * ynew[j];
					// distance to center
					if ((sqrad < olddiameter)) {
						// ignore existing values
						continue;
					} else if ((sqrad < newdiameter & sqrad >= olddiameter)) {
						// ignore existing values
						int xopp = i - 2 * (i - xmiddle);
						int yopp = j - 2 * (j - ymiddle);
						znew[i, j] = getExtrapolatedZ(znew, i, j);
						// right up quadrant
						znew[xopp, j] = getExtrapolatedZ(znew, xopp, j);
						// left  up
						znew[i, yopp] = getExtrapolatedZ(znew, i, yopp);
						// right down
						znew[xopp, yopp] = getExtrapolatedZ(znew, xopp, yopp);
						// left  down
					//if(sqrad > newdiameter)
					} else {
						// ignore values standing outside desired diameter
						znew[i, j] = float.NaN;
					}
				}
			}
			// store result
			x = xnew;
			y = ynew;
			z = znew;
		}

		private float getExtrapolatedZ(float[,] grid, int currentXi, int currentYi)
		{
			dynamic left = (currentXi - 1 > 0 ? currentXi - 1 : currentXi);
			dynamic right = (currentXi + 1 < grid.Length ? currentXi + 1 : currentXi);
			dynamic bottom = (currentYi - 1 > 0 ? currentYi - 1 : currentYi);
			dynamic up = (currentYi + 1 < grid.GetLength(1) ? currentYi + 1 : currentYi);
			float cumval = 0;
			int nval = 0;
			for (int u = left; u <= right; u++) {
				for (int v = bottom; v <= up; v++) {
					if ((!float.IsNaN(grid[u, v]))) {
						cumval += grid[u, v];
						nval += 1;
					}
				}
			}
			if ((nval > 0)) {
				return cumval / nval;
			} else {
				return float.NaN;
			}
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
