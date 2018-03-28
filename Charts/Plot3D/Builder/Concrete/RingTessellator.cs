
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Colors;
using nzy3D.Maths;
using nzy3D.Plot3D.Primitives;

namespace nzy3D.Plot3D.Builder.Concrete
{

	public class RingTessellator : OrthonormalTessellator
	{

		internal float _ringMin;
		internal float _ringMax;
		internal ColorMapper _cmap;

		internal Color _factor;
		public RingTessellator(float ringMin, float ringMax, ColorMapper cmap, Color factor)
		{
			_ringMin = ringMin;
			_ringMax = ringMax;
			_cmap = cmap;
			_factor = factor;
		}

		private RingTessellator()
		{
			throw new Exception("Forbidden constructor");
		}

		public override Primitives.AbstractComposite build(float[] x, float[] y, float[] z)
		{
			setData(x, y, z);
			Shape s = new Shape();
			s.Add(getInterpolatedRingPolygons());
			return s;
		}

		/// <summary>
		/// Load data standing on an orthonormal grid.
		/// <br/>
		/// Each input point (i.e. the association of x(i), y(j), z(i)(j)) will be
		/// represented by a polygon centered on this point. The default coordinates
		/// of this polygon will be:
		/// <ul>
		/// <li>x(i-1), y(j+1), z(i-1)(j+1)</li>
		/// <li>x(i-1), y(j-1), z(i-1)(j-1)</li>
		/// <li>x(i+1), y(j-1), z(i+1)(j-1)</li>
		/// <li>x(i+1), y(j+1), z(i+1)(j+1)</li>
		/// </ul>
		/// There are thus three types of polygons:
		/// <ul>
		/// <li>those that stand completely inside the ringMin and ringMax radius and
		/// that have the previous coordinates.</li>
		/// <li>those that stand completely outside the ringMin and ringMax radius and
		/// that won't be added to the list of polygons.</li>
		/// <li>those that have some points in and some points out of the ringMin and
		/// ringMax radius. These polygons are recomputed so that "out" points are replaced
		/// by two points that make the smooth contour. According to the number of "out"
		/// points, the modified polygon will gather 3, 4, or 5 points.</li>
		/// </ul>
		/// <br/>
		/// As a consequence, it is suggested to provide data ranging outside of ringMin
		/// and ringMax, in order to be sure to have a perfect round surface.
		/// </summary>
		public List<Polygon> getInterpolatedRingPolygons()
		{
			List<Polygon> polygons = new List<Polygon>();
			bool[] isIn = null;
			for (int xi = 0; xi <= x.Length - 2; xi++) {
				for (int yi = 0; yi <= y.Length - 2; yi++) {
					// Compute points surrounding current point
					Point[] p = getRealQuadStandingOnPoint(xi, yi);
					p[0].Color = _cmap.Color(p[0].xyz);
					p[1].Color = _cmap.Color(p[1].xyz);
					p[2].Color = _cmap.Color(p[2].xyz);
					p[3].Color = _cmap.Color(p[3].xyz);
					p[0].rgb.mul(_factor);
					p[1].rgb.mul(_factor);
					p[2].rgb.mul(_factor);
					p[3].rgb.mul(_factor);
					float[] radius = new float[p.Length];
					for (int i = 0; i <= p.Length - 1; i++) {
                        radius[i] = radius2d(p[i]);
					}
					// Compute status of each point according to there radius, or NaN status
					isIn = isInside(p, radius, _ringMin, _ringMax);
					// Ignore polygons that are out
					if (((!isIn[0]) & (!isIn[1]) & (!isIn[2]) & (!isIn[3]))) {
						continue;
					}
					if ((isIn[0] & isIn[1] & isIn[2] & isIn[3])) {
						// Directly store polygons that have non NaN values for all points
						Polygon quad = new Polygon();
						for (int pi = 0; pi <= p.Length - 1; pi++) {
							quad.Add(p[pi]);
						}
						polygons.Add(quad);
					} else {
						// Partly inside: generate points that intersect a radius
						Polygon polygon = new Polygon();
						Point intersection = default(Point);
						// generated point
						float ringRadius = 0;
						int[] seq = {
							0,
							1,
							2,
							3,
							0
						};
						bool[] done = new bool[4];
						for (int pi = 0; pi <= done.Length - 1; pi++) {
							done[pi] = false;
						}
						// Handle all square edges and shift "out" points
						for (int s = 0; s <= seq.Length - 2; s++) {
							// Case of point s "in" and point s+1 "in"
							if ((isIn[seq[s]] & isIn[seq[s + 1]])) {
								if ((!done[seq[s]])) {
									polygon.Add(p[seq[s]]);
									done[seq[s]] = true;
								}
								if ((!done[seq[s + 1]])) {
									polygon.Add(p[seq[s + 1]]);
									done[seq[s + 1]] = true;
								}
							} else if ((isIn[seq[s]] & (!isIn[seq[s + 1]]))) {
								// Case of point s "in" and point s+1 "out"
								if ((!done[seq[s]])) {
									polygon.Add(p[seq[s]]);
									done[seq[s]] = true;
								}
								// Select the radius on which the point is supposed to stand
								if ((Math.Abs(radius[seq[s + 1]] - _ringMin) < Math.Abs(radius[seq[s + 1]] - _ringMax))) {
									ringRadius = _ringMin;
								} else {
									ringRadius = _ringMax;
								}
								// Generate a point on the circle that replaces s+1
								intersection = findPoint(p[seq[s]], p[seq[s + 1]], ringRadius);
								intersection.Color = _cmap.Color(intersection.xyz);
								intersection.rgb.mul(_factor);
								polygon.Add(intersection);


							} else if (((!isIn[seq[s]]) & isIn[seq[s + 1]])) {
								//Case of point s "out" and point s+1 "in"
								// Select the radius on which the point is supposed to stand
								if ((Math.Abs(radius[seq[s + 1]] - _ringMin) < Math.Abs(radius[seq[s + 1]] - _ringMax))) {
									ringRadius = _ringMin;
								} else {
									ringRadius = _ringMax;
								}
								// Generate a point on the circle that replaces s
								intersection = findPoint(p[seq[s]], p[seq[s + 1]], ringRadius);
								intersection.Color = _cmap.Color(intersection.xyz);
								intersection.rgb.mul(_factor);
								polygon.Add(intersection);
								if ((!done[seq[s + 1]])) {
									polygon.Add(p[seq[s + 1]]);
									done[seq[s + 1]] = true;
								}
							}
							// end case 3
						}
						// end polygon construction loop
						polygons.Add(polygon);
					}
					// end switch quad/polygon
				}
				// end for y
			}
			// end for x	
			return polygons;
		}

		/// <summary>
		/// Indicates which point lies inside and outside the given min and max radius.
		/// </summary>
		internal bool[] isInside(Point[] p, float[] radius, float minRadius, float maxRadius)
		{
			bool[] isIn = new bool[4];
			isIn[0] = (!double.IsNaN(p[0].xyz.z)) & radius[0] < maxRadius & radius[0] >= minRadius;
            isIn[1] = (!double.IsNaN(p[1].xyz.z)) & radius[1] < maxRadius & radius[1] >= minRadius;
            isIn[2] = (!double.IsNaN(p[2].xyz.z)) & radius[2] < maxRadius & radius[2] >= minRadius;
            isIn[3] = (!double.IsNaN(p[3].xyz.z)) & radius[3] < maxRadius & radius[3] >= minRadius;
			return isIn;
		}

		internal float radius2d(Point p)
		{
			return (float)Math.Sqrt(p.xyz.x * p.xyz.x + p.xyz.y * p.xyz.y);
		}

		/// <summary>
		/// Return a point that is the intersection between a segment and a circle
		/// Throws ArithmeticException if points do not stand on an squared (orthonormal) grid.
		/// </summary>
		private Point findPoint(Point p1, Point p2, float ringRadius)
		{
			// We know that the seeked point is on a horizontal or vertial line
			double x3 = 0;
            double y3 = 0;
            double z3 = 0;
            double w1 = 0;
            double w2 = 0;
            double alpha = 0;
			//We know x3 and radius and seek y3, using intermediate alpha
			if ((p1.xyz.x == p2.xyz.x)) {
				x3 = p1.xyz.x;
				alpha = Math.Acos(x3 / ringRadius);
				if ((p1.xyz.y < 0 & p2.xyz.y < 0)) {
					y3 = -Math.Sin(alpha) * ringRadius;
				} else if ((p1.xyz.y > 0 & p2.xyz.y > 0)) {
					y3 = Math.Sin(alpha) * ringRadius;
				} else if ((p1.xyz.y == -p2.xyz.y)) {
					y3 = 0;
					// ne peut pas arriver
				} else {
					throw new ArithmeticException(("no alignement between p1(" + p1.xyz.x + "," + p1.xyz.y + "," + p1.xyz.z + ") and p2(" + p2.xyz.x + "," + p2.xyz.y + "," + p2.xyz.z + ")"));
				}
				// and now get z3
                if (((!double.IsNaN(p1.xyz.z)) & double.IsNaN(p2.xyz.z)))
                {
					z3 = p1.xyz.z;
                }
                else if ((double.IsNaN(p1.xyz.z) & (!double.IsNaN(p2.xyz.z))))
                {
					z3 = p2.xyz.z;
                }
                else if (((!double.IsNaN(p1.xyz.z)) & (!double.IsNaN(p2.xyz.z))))
                {
					w2 = (Math.Sqrt((x3 - p1.xyz.x) * (x3 - p1.xyz.x) + (y3 - p1.xyz.y) * (y3 - p1.xyz.y)) / Math.Sqrt((p2.xyz.x - p1.xyz.x) * (p2.xyz.x - p1.xyz.x) + (p2.xyz.y - p1.xyz.y) * (p2.xyz.y - p1.xyz.y)));
					w1 = 1 - w2;
					z3 = w1 * p1.xyz.z + w2 * p2.xyz.z;
				} else {
					throw new ArithmeticException(("can't compute z3 with p1(" + p1.xyz.x + "," + p1.xyz.y + ") and p2(" + p2.xyz.x + "," + p2.xyz.y + ")"));
				}
				// We know y3 and radius and seek x3, using intermediate alpha
			} else if ((p1.xyz.y == p2.xyz.y)) {
				y3 = p1.xyz.y;
				alpha = Math.Asin(y3 / ringRadius);
				if ((p1.xyz.x < 0 & p2.xyz.x < 0)) {
					x3 = -Math.Cos(alpha) * ringRadius;
				} else if ((p1.xyz.x > 0 & p2.xyz.x > 0)) {
					x3 = Math.Cos(alpha) * ringRadius;
				} else if ((p1.xyz.x == -p2.xyz.x)) {
					x3 = 0;
					// ne peut pas arriver
				} else {
					throw new ArithmeticException(("no alignement between p1(" + p1.xyz.x + "," + p1.xyz.y + "," + p1.xyz.z + ") and p2(" + p2.xyz.x + "," + p2.xyz.y + "," + p2.xyz.z + ")"));
				}
				// and now get z3
                if (((!double.IsNaN(p1.xyz.z)) & double.IsNaN(p2.xyz.z)))
                {
					z3 = p1.xyz.z;
                }
                else if ((double.IsNaN(p1.xyz.z) & (!double.IsNaN(p2.xyz.z))))
                {
					z3 = p2.xyz.z;
                }
                else if (((!double.IsNaN(p1.xyz.z)) & (!double.IsNaN(p2.xyz.z))))
                {
					w2 = (Math.Sqrt((x3 - p1.xyz.x) * (x3 - p1.xyz.x) + (y3 - p1.xyz.y) * (y3 - p1.xyz.y)) / Math.Sqrt((p2.xyz.x - p1.xyz.x) * (p2.xyz.x - p1.xyz.x) + (p2.xyz.y - p1.xyz.y) * (p2.xyz.y - p1.xyz.y)));
					w1 = 1 - w2;
					z3 = w1 * p1.xyz.z + w2 * p2.xyz.z;

				} else {
					throw new ArithmeticException(("can't compute z3 with p1(" + p1.xyz.x + "," + p1.xyz.y + ") and p2(" + p2.xyz.x + "," + p2.xyz.y + ")"));
				}
			} else {
				throw new ArithmeticException(("no alignement between p1(" + p1.xyz.x + "," + p1.xyz.y + ") and p2(" + p2.xyz.x + "," + p2.xyz.y + ")"));
			}
			return new Point(new Coord3d(x3, y3, z3));
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
