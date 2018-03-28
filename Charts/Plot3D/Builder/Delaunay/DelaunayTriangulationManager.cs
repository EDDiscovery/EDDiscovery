
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;
using nzy3D.Plot3D.Builder.Delaunay.Jdt;
using nzy3D.Plot3D.Primitives;

namespace nzy3D.Plot3D.Builder.Delaunay
{

	public class DelaunayTriangulationManager
	{

		protected float[] _x;
		protected float[] _y;
		protected float[,] _z_as_fxy;

		protected ITriangulation _triangulator;
		public DelaunayTriangulationManager(ICoordinateValidator cv, ITriangulation triangulator)
		{
			_triangulator = triangulator;
			this.x = cv.getX();
            this.y = cv.getY();
            this.z_as_fxy = cv.get_Z_as_fxy();
		}

		public AbstractDrawable buildDrawable()
		{
			Shape s = new Shape();
			s.Add(getFacets());
			return s;
		}

		// TODO: three different point classes coord3d, point_dt !!
		private List<Polygon> getFacets()
		{
			int xlen = _x.Length;
			for (int i = 0; i <= xlen - 1; i++) {
				Point_dt point_dt = new Point_dt(x[i], y[i], z_as_fxy[i, i]);
				_triangulator.insertPoint(point_dt);
			}
			List<Polygon> polygons = new List<Polygon>();
			IEnumerator<Triangle_dt> trianglesIter = _triangulator.trianglesIterator();
			while ((trianglesIter.MoveNext())) {
				Triangle_dt triangle = trianglesIter.Current;
				// isHalfplane means a degenerated triangle 
				if ((triangle.isHalfplane)) {
					continue;
				}
				Polygon newPolygon = buildPolygonFrom(triangle);
				polygons.Add(newPolygon);
			}
			return polygons;
		}

		private Polygon buildPolygonFrom(Triangle_dt triangle)
		{
			Coord3d c1 = triangle.p1.Coord3d;
			Coord3d c2 = triangle.p2.Coord3d;
			Coord3d c3 = triangle.p3.Coord3d;
			Polygon polygon = new Polygon();
			polygon.Add(new Point(c1));
			polygon.Add(new Point(c2));
			polygon.Add(new Point(c3));
			return polygon;
		}

		public float[] x {
			get { return _x; }
			set { _x = value; }
		}

		public float[] y {
			get { return _y; }
			set { _y = value; }
		}

		public float[,] z_as_fxy {
			get { return _z_as_fxy; }
			set { _z_as_fxy = value; }
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
