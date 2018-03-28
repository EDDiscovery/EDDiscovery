using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;
using nzy3D.Colors;
using nzy3D.Colors.ColorMaps;
using nzy3D.Plot3D.Primitives;

namespace nzy3D.Plot3D.Builder.Concrete
{

	/// <summary>
	/// The <see cref="OrthonormalTessellator"/> checks that coordinates are lying on an orthormal grid,
	/// and is able to provide a <see cref="AbstractComposite"/> made of <see cref="Polygon"/>s built according to this grid
	///
	/// On this model, one input coordinate is represented by one <see cref="Polygon"/>, for which each point is
	/// a mean point between two grid ticks:
	///
	///  ^                           ^
	///  |                           |
	///  -   +   +   +               -   +   +   +
	///  |                           |     *---*
	///  -   +   o   +        >>     -   + | o | +
	///  |                           |     *---*
	///  -   +   +   +               -   +   +   +
	///  |                           |
	///  |---|---|---|-->            |---|---|---|-->
	///
	///
	///  In this figure, the representation of a coordinate ("o" on the left) is a polygon
	///  made of mean points ("*" on the right) that require the existence of four surrounding
	///  points (the "o" and the three "+")
	///
	/// @author Martin Pernollet
	/// </summary>
	public class OrthonormalTessellator : Tessellator
	{

		protected internal float[] x;
		protected internal float[] y;
		protected internal float[,] z;
		protected internal int findxi;

		protected internal int findyj;
		protected internal void setData(float[] x, float[] y, float[] z)
		{
			if (x.Length != y.Length | x.Length != z.Length) {
				throw new Exception("x, y, and z arrays must agree in length.");
			}
			// Initialize loading
			this.x = unique(x);
			this.y = unique(y);
			this.z = new float[this.x.Length + 1, this.y.Length + 1];
			for (int i = 0; i <= this.x.Length - 1; i++) {
				for (int j = 0; j <= this.y.Length - 1; j++) {
					this.z[i, j] = float.NaN;
				}
			}
			// Fill Z matrix and set surface minimum and maximum
			bool found = false;
			for (int p = 0; p <= z.Length - 1; p++) {
				found = find(this.x, this.y, x[p], y[p]);
				if ((!found)) {
					throw new Exception("it seems (x[p],y[p]) has not been properly stored into (this.x,this.y)");
				}
				this.z[findxi, findyj] = z[p];
			}
		}

		internal float[] unique(float[] data)
		{
            float[] copy = (float[])data.Clone();
			System.Array.Sort(copy);
			// count unique values
			int nunique = 0;
			float last = float.NaN;
			for (int i = 0; i <= copy.Length - 1; i++) {
				if (float.IsNaN(copy[i])) {
					//         /System.out.println("Ignoring NaN value at " + i);
				} else if (copy[i] != last) {
					nunique += 1;
					last = copy[i];
				}
			}
			// Fill a sorted unique array
			float[] result = new float[nunique];
			last = float.NaN;
			int r = 0;
			for (int d = 0; d <= copy.Length - 1; d++) {
				if (float.IsNaN(copy[d])) {
					//         /System.out.println("Ignoring NaN value at " + d);
				} else if (copy[d] != last) {
					result[r] = copy[d];
					last = copy[d];
					r += 1;
				}
			}
			return result;
		}

		/// <summary>
		/// Search in a couple of array a combination of values vx and vy.
		/// Positions xi and yi are returned in findxi and findyj class variables
		/// Function returns true if the couple of data may be retrieved,
		/// false otherwise (in this case, findxi and findyj remain unchanged).
		/// </summary>
		internal bool find(float[] x, float[] y, float vx, float vy)
		{
			int xi = -1;
			int yj = -1;
			for (int i = 0; i <= x.Length - 1; i++) {
				if (x[i] == vx) {
					xi = i;
				}
			}
			if (xi == -1) {
				return false;
			}
			for (int j = 0; j <= y.Length - 1; j++) {
				if (y[j] == vy) {
					yj = j;
				}
			}
			if (yj == -1) {
				return false;
			}
			findxi = xi;
			findyj = yj;
			return true;
		}

		public List<Polygon> getSquarePolygonsOnCoordinates()
		{
			return getSquarePolygonsOnCoordinates(null, null);
		}

		public List<Polygon> getSquarePolygonsOnCoordinates(ColorMapper cmap, Color colorFactor)
		{
			List<Polygon> polygons = new List<Polygon>();
			for (int xi = 0; xi <= x.Length - 2; xi++) {
				for (int yi = 0; yi <= y.Length - 2; yi++) {
					// Compute quad making a polygon
					Point[] p = getRealQuadStandingOnPoint(xi, yi);
					if ((!validZ(p))) {
						continue;
						// ignore non valid set of points
					}
					if (((cmap != null))) {
						p[0].Color = cmap.Color(p[0].xyz);
						p[1].Color = cmap.Color(p[1].xyz);
						p[2].Color = cmap.Color(p[2].xyz);
						p[3].Color = cmap.Color(p[3].xyz);
					}
					if (((colorFactor != null))) {
						p[0].rgb.mul(colorFactor);
						p[1].rgb.mul(colorFactor);
						p[2].rgb.mul(colorFactor);
						p[3].rgb.mul(colorFactor);
					}
					// Store quad
					Polygon quad = new Polygon();
					for (int pi = 0; pi <= p.Length - 1; pi++) {
						quad.Add(p[pi]);
					}
					polygons.Add(quad);
				}
			}
			return polygons;
		}

		public object getSquarePolygonsAroundCoordinates()
		{
			return getSquarePolygonsAroundCoordinates(null, null);
		}

		public object getSquarePolygonsAroundCoordinates(ColorMapper cmap, Color colorFactor)
		{
			List<Polygon> polygons = new List<Polygon>();
			for (int xi = 0; xi <= x.Length - 2; xi++) {
				for (int yi = 0; yi <= y.Length - 2; yi++) {
					// Compute quad making a polygon
					Point[] p = getEstimatedQuadSurroundingPoint(xi, yi);
					if ((!validZ(p))) {
						continue;
						// ignore non valid set of points
					}
					if (((cmap != null))) {
						p[0].Color = cmap.Color(p[0].xyz);
						p[1].Color = cmap.Color(p[1].xyz);
						p[2].Color = cmap.Color(p[2].xyz);
						p[3].Color = cmap.Color(p[3].xyz);
					}
					if (((colorFactor != null))) {
						p[0].rgb.mul(colorFactor);
						p[1].rgb.mul(colorFactor);
						p[2].rgb.mul(colorFactor);
						p[3].rgb.mul(colorFactor);
					}
					// Store quad
					Polygon quad = new Polygon();
					for (int pi = 0; pi <= p.Length - 1; pi++) {
						quad.Add(p[pi]);
					}
					polygons.Add(quad);
				}
			}
			return polygons;
		}

		protected internal Point[] getRealQuadStandingOnPoint(int xi, int yi)
		{
			Point[] p = new Point[4];
			p[0] = new Point(new Coord3d(x[xi], y[yi], z[xi, yi]));
			p[1] = new Point(new Coord3d(x[xi + 1], y[yi], z[xi + 1, yi]));
			p[2] = new Point(new Coord3d(x[xi + 1], y[yi + 1], z[xi + 1, yi + 1]));
			p[3] = new Point(new Coord3d(x[xi], y[yi + 1], z[xi, yi + 1]));
			return p;
		}

		internal Point[] getEstimatedQuadSurroundingPoint(int xi, int yi)
		{
			Point[] p = new Point[4];
			p[0] = new Point(new Coord3d((x[xi - 1] + x[xi]) / 2, (y[yi + 1] + y[yi]) / 2, (z[xi - 1, yi + 1] + z[xi - 1, yi] + z[xi, yi] + z[xi, yi + 1]) / 4));
			p[1] = new Point(new Coord3d((x[xi - 1] + x[xi]) / 2, (y[yi - 1] + y[yi]) / 2, (z[xi - 1, yi] + z[xi - 1, yi - 1] + z[xi, yi - 1] + z[xi, yi]) / 4));
			p[2] = new Point(new Coord3d((x[xi + 1] + x[xi]) / 2, (y[yi - 1] + y[yi]) / 2, (z[xi, yi] + z[xi, yi - 1] + z[xi + 1, yi - 1] + z[xi + 1, yi]) / 4));
			p[3] = new Point(new Coord3d((x[xi + 1] + x[xi]) / 2, (y[yi + 1] + y[yi]) / 2, (z[xi, yi + 1] + z[xi, yi] + z[xi + 1, yi] + z[xi + 1, yi + 1]) / 4));
			return p;
		}

		internal bool validZ(Point[] points)
		{
			foreach (Point p in points) {
				if ((!validZ(p))) {
					return false;
				}
			}
			return true;
		}

		internal bool validZ(Point p)
		{
			return !(double.IsNaN(p.xyz.z));
		}

		public override Primitives.AbstractComposite build(float[] x, float[] y, float[] z)
		{
			setData(x, y, z);
			Shape s = new Shape();
			s.Add(getSquarePolygonsOnCoordinates());
			return s;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
