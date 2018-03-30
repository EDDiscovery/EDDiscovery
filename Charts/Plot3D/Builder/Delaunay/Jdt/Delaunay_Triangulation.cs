
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Plot3D.Builder.Delaunay.Jdt
{

	/// <summary>
	/// This class represents a Delaunay Triangulation. The class was written for a
	/// large scale triangulation (1000 - 200,000 vertices). The application main use
	/// is 3D surface (terrain) presentation. 
	/// The class main properties are the following:
	/// - fast point location. (O(n^0.5)), practical runtime is often very fast. 
	/// - handles degenerate cases and none general position input (ignores duplicate
	/// points). 
	/// - save &amp; load from\to text file in TSIN format. 
	/// - 3D support: including z value approximation.
	/// - standard java (1.5 generic) iterators for the vertices and triangles. 
	/// - smart iterator to only the updated triangles - for terrain simplification 
	/// 
	///
	/// Testing (done in early 2005): Platform java 1.5.02 windows XP (SP2), AMD
	/// laptop 1.6G sempron CPU 512MB RAM. Constructing a triangulation of 100,000
	/// vertices takes ~ 10 seconds. point location of 100,000 points on a
	/// triangulation of 100,000 vertices takes ~ 5 seconds.
	///
	/// Note: constructing a triangulation with 200,000 vertices and more requires
	/// extending java heap size (otherwise an exception will be thrown).
	///
	/// Bugs: if U find a bug or U have an idea as for how to improve the code,
	/// please send me an email to: benmo@ariel.ac.il
	///
	/// @author Boaz Ben Moshe 5/11/05 
	///         The project uses some ideas presented in the VoroGuide project,
	///         written by Klasse f?r Kreise (1996-1997), For the original applet
	///         see: http://www.pi6.fernuni-hagen.de/GeomLab/VoroGlide/.
	/// </summary>
	/// <remarks></remarks>
	public class Delaunay_Triangulation : ITriangulation
	{

		// The first and last points (used only for first step construction)
		private Point_dt firstP;
		private Point_dt lastP;
		// for degenerate case!
		private bool allColinear;
		//the first and last triangles (used only for first step construction)
		private Triangle_dt firstT;
		private Triangle_dt lastT;
		private Triangle_dt currT;
		// the triangle the fond (search start from
		private Triangle_dt startTriangle;
		// the triangle the convex hull starts from
		public Triangle_dt startTriangleHull;
			// number of points
		private int nPoints = 0;
		// additional data 4/8/05 used by the iterators
		private List<Point_dt> _vertices;
		private List<Triangle_dt> _triangles;
		// The triangles that were deleted in the last deletePoint iteration.
		//Private deletedTriangles As List(Of Triangle_dt)
		// The triangles that were added in the last deletePoint iteration.
		//Private addedTriangles As List(Of Triangle_dt)
		private int _modCount = 0;
		private int _modCount2 = 0;
		// the Bounding Box, {{x0,y0,z0} , {x1,y1,z1}}
		private Point_dt _bb_min;
		private Point_dt _bb_max;
		// Index for faster point location searches

		private GridIndex gridIndex = null;
		public Delaunay_Triangulation() : this(new Point_dt[0])
		{
		}

		public Delaunay_Triangulation(Point_dt[] ps)
		{
			_modCount = 0;
			_modCount2 = 0;
			_bb_min = null;
			_bb_max = null;
			_vertices = new List<Point_dt>();
			_triangles = new List<Triangle_dt>();
			//deletedTriangles = Nothing
			//addedTriangles = New List(Of Triangle_dt)
			allColinear = true;
			for (int i = 0; i <= ps.Length - 1; i++) {
                insertPoint(ps[i]);
			}
		}

		/// <summary>
		/// Returns he number of vertices in this triangulation.
		/// </summary>
		public int Size()
		{
			if (_vertices == null)
				return 0;
			return _vertices.Count;
		}

		/// <summary>
		/// Returns the number of triangles in the triangulation, including infinite faces
		/// </summary>
		public int TrianglesSize()
		{
			initTriangles();
			return _triangles.Count;
		}
		int ITriangulation.trianglesSize()
		{
			return TrianglesSize();
		}

		/// <summary>
		/// Returns the changes counter for this triangulation
		/// </summary>
		public int ModeCounter {
			get { return _modCount; }
		}

		public void insertPoint(Point_dt p)
		{
			if (_vertices.Contains(p))
				return;
			_modCount += 1;
			updateBoundingBox(p);
			_vertices.Add(p);
			Triangle_dt t = insertPointSimple(p);
			if (t == null)
				return;
			Triangle_dt tt = t;
			currT = t;
			// recall the last point for fast (last) update iterator
			do {
				Flip(tt, _modCount);
				tt = tt.canext;
			} while (tt.Equals(t) & !tt.isHalfplane);
			if ((gridIndex != null)) {
				gridIndex.updateIndex(getLastUpdatedTriangles());
			}

		}

		public IEnumerator<Triangle_dt> getLastUpdatedTriangles()
		{
			List<Triangle_dt> tmp = new List<Triangle_dt>();
			if (this.TrianglesSize() > 1) {
				Triangle_dt t = currT;
				allTriangles(t, tmp, this._modCount);
			}
			return tmp.GetEnumerator();
		}

		private void allTriangles(Triangle_dt curr, List<Triangle_dt> front, int mc)
		{
			if (((curr != null)) && curr.mc == mc && (!(front.Contains(curr)))) {
				front.Add(curr);
				allTriangles(curr.abnext, front, mc);
				allTriangles(curr.bcnext, front, mc);
				allTriangles(curr.canext, front, mc);
			}
		}


		private Triangle_dt insertPointSimple(Point_dt p)
		{
			nPoints += 1;
			if ((!allColinear)) {
				Triangle_dt t = Find(startTriangle, p);
				if (t.isHalfplane) {
					startTriangle = extendOutside(t, p);
				} else {
					startTriangle = extendInside(t, p);
				}
				return startTriangle;
			}
			if (nPoints == 1) {
				firstP = p;
				return null;
			}
			if (nPoints == 2) {
				startTriangulation(firstP, p);
				return null;
			}
			switch (p.pointLineTest(firstP, lastP)) {
				case Point_dt.LEFT:
					startTriangle = extendOutside(firstT.abnext, p);
					allColinear = false;
					break; // TODO: might not be correct. Was : Exit Select

					break;
				case Point_dt.RIGHT:
					startTriangle = extendOutside(firstT, p);
					allColinear = false;
					break; // TODO: might not be correct. Was : Exit Select

					break;
				case Point_dt.ONSEGMENT:
					insertCollinear(p, Point_dt.ONSEGMENT);
					break; // TODO: might not be correct. Was : Exit Select

					break;
				case Point_dt.INFRONTOFA:
					insertCollinear(p, Point_dt.INFRONTOFA);
					break; // TODO: might not be correct. Was : Exit Select

					break;
				case Point_dt.BEHINDB:
					insertCollinear(p, Point_dt.BEHINDB);
					break; // TODO: might not be correct. Was : Exit Select

					break;
			}
			return null;
		}

		private void insertCollinear(Point_dt p, int res)
		{
			Triangle_dt t = default(Triangle_dt);
			Triangle_dt tp = default(Triangle_dt);
			Triangle_dt u = default(Triangle_dt);
			switch (res) {
				case Point_dt.INFRONTOFA:
					t = new Triangle_dt(firstP, p);
					tp = new Triangle_dt(p, firstP);
					t.abnext = tp;
					tp.abnext = t;
					t.bcnext = tp;
					tp.canext = t;
					t.canext = firstT;
					firstT.bcnext = t;
					tp.bcnext = firstT.abnext;
					firstT.abnext.canext = tp;
					firstT = t;
					firstP = p;
					break; // TODO: might not be correct. Was : Exit Select

					break;
				case Point_dt.BEHINDB:
					t = new Triangle_dt(p, lastP);
					tp = new Triangle_dt(lastP, p);
					t.abnext = tp;
					tp.abnext = t;
					t.bcnext = lastT;
					lastT.canext = t;
					t.canext = tp;
					tp.bcnext = t;
					tp.canext = lastT.abnext;
					lastT.abnext.bcnext = tp;
					lastT = t;
					lastP = p;
					break; // TODO: might not be correct. Was : Exit Select

					break;
				case Point_dt.ONSEGMENT:
					u = firstT;
					while (p.isGreater(u.a)) {
						u = u.canext;
					}
					t = new Triangle_dt(p, u.b);
					tp = new Triangle_dt(u.b, p);
					u.b = p;
					u.abnext.a = p;
					t.abnext = tp;
					tp.abnext = t;
					t.bcnext = u.bcnext;
					u.bcnext.canext = t;
					t.canext = u;
					u.bcnext = t;
					tp.canext = u.abnext.canext;
					u.abnext.canext.bcnext = tp;
					tp.bcnext = u.abnext;
					u.abnext.canext = tp;
					if ((firstT.Equals(u)))
						firstT = t;
					break; // TODO: might not be correct. Was : Exit Select

					break;
			}
		}

		private void startTriangulation(Point_dt p1, Point_dt p2)
		{
			Point_dt ps = default(Point_dt);
			Point_dt pb = default(Point_dt);
			if ((p1.isLess(p2))) {
				ps = p1;
				pb = p2;
			} else {
				ps = p2;
				pb = p1;
			}
			firstT = new Triangle_dt(pb, ps);
			lastT = firstT;
			Triangle_dt t = new Triangle_dt(ps, pb);
			firstT.abnext = t;
			t.abnext = firstT;
			firstT.bcnext = t;
			t.canext = firstT;
			firstT.canext = t;
			t.bcnext = firstT;
			firstP = firstT.b;
			lastP = lastT.a;
			startTriangleHull = firstT;
		}

		private Triangle_dt extendInside(Triangle_dt t, Point_dt p)
		{
			Triangle_dt h1 = default(Triangle_dt);
			Triangle_dt h2 = default(Triangle_dt);
			h1 = treatDegeneracyInside(t, p);
			if ((h1 != null))
				return h1;
			h1 = new Triangle_dt(t.c, t.a, p);
			h2 = new Triangle_dt(t.b, t.c, p);
			t.c = p;
			t.circumcircle();
			h1.abnext = t.canext;
			h1.bcnext = t;
			h1.canext = h2;
			h2.abnext = t.bcnext;
			h2.bcnext = h1;
			h2.canext = t;
			h1.abnext.switchneighbors(t, h1);
			h2.abnext.switchneighbors(t, h2);
			t.bcnext = h2;
			t.canext = h1;
			return t;
		}

		private Triangle_dt treatDegeneracyInside(Triangle_dt t, Point_dt p)
		{
			if ((t.abnext.isHalfplane & p.pointLineTest(t.b, t.a) == Point_dt.ONSEGMENT))
				return extendOutside(t.abnext, p);
			if ((t.bcnext.isHalfplane & p.pointLineTest(t.c, t.b) == Point_dt.ONSEGMENT))
				return extendOutside(t.bcnext, p);
			if ((t.canext.isHalfplane & p.pointLineTest(t.a, t.c) == Point_dt.ONSEGMENT))
				return extendOutside(t.canext, p);
			return null;
		}

		private Triangle_dt extendOutside(Triangle_dt t, Point_dt p)
		{
			if ((p.pointLineTest(t.a, t.b) == Point_dt.ONSEGMENT)) {
				Triangle_dt dg = new Triangle_dt(t.a, t.b, p);
				Triangle_dt hp = new Triangle_dt(p, t.b);
				t.b = p;
				dg.abnext = t.abnext;
				dg.abnext.switchneighbors(t, dg);
				dg.bcnext = hp;
				hp.abnext = dg;
				dg.canext = t;
				t.abnext = dg;
				hp.bcnext = t.bcnext;
				hp.bcnext.canext = hp;
				hp.canext = t;
				t.bcnext = hp;
				return dg;
			}
			Triangle_dt ccT = extendcounterclock(t, p);
			Triangle_dt cT = extendclock(t, p);
			ccT.bcnext = cT;
			cT.canext = ccT;
			startTriangleHull = cT;
			return cT.abnext;
		}

		private Triangle_dt extendcounterclock(Triangle_dt t, Point_dt p)
		{
			t.isHalfplane = false;
			t.c = p;
			t.circumcircle();
			Triangle_dt tca = t.canext;
			if ((p.pointLineTest(tca.a, tca.b) >= Point_dt.RIGHT)) {
				Triangle_dt nT = new Triangle_dt(t.a, p);
				nT.abnext = t;
				t.canext = nT;
				nT.canext = tca;
				tca.bcnext = nT;
				return nT;
			}
			return extendcounterclock(tca, p);
		}

		private Triangle_dt extendclock(Triangle_dt t, Point_dt p)
		{
			t.isHalfplane = false;
			t.c = p;
			t.circumcircle();
			Triangle_dt tbc = t.bcnext;
			if ((p.pointLineTest(tbc.a, tbc.b) >= Point_dt.RIGHT)) {
				Triangle_dt nT = new Triangle_dt(p, t.b);
				nT.abnext = t;
				t.bcnext = nT;
				nT.bcnext = tbc;
				tbc.canext = nT;
				return nT;
			}
			return extendclock(tbc, p);
		}

		private void Flip(Triangle_dt t, int mc)
		{
			Triangle_dt u = t.abnext;
			Triangle_dt v = default(Triangle_dt);
			t.mc = mc;
			if ((u.isHalfplane | (!(u.circumcircle_contains(t.c)))))
				return;
			if ((t.a.Equals(u.a))) {
				v = new Triangle_dt(u.b, t.b, t.c);
				v.abnext = u.bcnext;
				t.abnext = u.abnext;
			} else if ((t.a.Equals(u.b))) {
				v = new Triangle_dt(u.c, t.b, t.c);
				v.abnext = u.canext;
				t.abnext = u.bcnext;
			} else if ((t.a.Equals(u.c))) {
				v = new Triangle_dt(u.a, t.b, t.c);
				v.abnext = u.abnext;
				t.abnext = u.canext;
			} else {
				throw new Exception("Error in flip.");
			}
			v.mc = mc;
			v.bcnext = t.bcnext;
			v.abnext.switchneighbors(u, v);
			v.bcnext.switchneighbors(t, v);
			t.bcnext = v;
			v.canext = t;
			t.b = v.a;
			t.abnext.switchneighbors(u, t);
			t.circumcircle();
			currT = v;
			Flip(t, mc);
			Flip(v, mc);
		}

		/// <summary>
		/// finds the triangle the query point falls in, note if out-side of this
		/// triangulation a half plane triangle will be returned (see contains), the
		/// search has expected time of O(n^0.5), and it starts form a fixed triangle
		/// (me.startTriangle).
		/// </summary>
		/// <param name="p">Query point</param>
		/// <returns>The triangle that point <paramref name="p"/> is in.</returns>
		public Triangle_dt Find(Point_dt p)
		{
			// If triangulation has a spatial index try to use it as the starting
			//triangle
			Triangle_dt searchTriangle = startTriangle;
			if ((gridIndex != null)) {
				Triangle_dt indexTriangle = gridIndex.findCellTriangleOf(p);
				if ((indexTriangle != null)) {
					searchTriangle = indexTriangle;
				}
			}
			// Search for the point's triangle starting from searchTriangle
			return Find(searchTriangle, p);
		}

		/// <summary>
		/// finds the triangle the query point falls in, note if out-side of this
		/// triangulation a half plane triangle will be returned (see contains). the
		/// search starts from the the start triangle
		/// </summary>
		/// <param name="p">Query point</param>
		/// <param name="start">The triangle the search starts at.</param>
		/// <returns>The triangle that point <paramref name="p"/> is in.</returns>
		public Triangle_dt Find(Point_dt p, Triangle_dt start)
		{
			if (start == null)
				start = startTriangle;
			Triangle_dt T = Find(start, p);
			return T;
		}

		private static Triangle_dt Find(Triangle_dt curr, Point_dt p)
		{
			if (p == null)
				return null;
			Triangle_dt next_t = default(Triangle_dt);
			if ((curr.isHalfplane)) {
				next_t = findnext2(p, curr);
				if ((next_t == null | next_t.isHalfplane))
					return curr;
				curr = next_t;
			}
			while (true) {
				next_t = findnext1(p, curr);
				if ((next_t == null))
					return curr;
				if ((next_t.isHalfplane))
					return next_t;
				curr = next_t;
			}
			return null;
			// Never supposed to get here
		}

		/// <summary>
		/// assumes v is NOT an halfplane! returns the next triangle for find.
		/// </summary>
		private static Triangle_dt findnext1(Point_dt p, Triangle_dt v)
		{
			if ((p.pointLineTest(v.a, v.b) == Point_dt.RIGHT & (!(v.abnext.isHalfplane))))
				return v.abnext;
			if ((p.pointLineTest(v.b, v.c) == Point_dt.RIGHT & (!(v.bcnext.isHalfplane))))
				return v.bcnext;
			if ((p.pointLineTest(v.c, v.a) == Point_dt.RIGHT & (!(v.canext.isHalfplane))))
				return v.canext;
			if ((p.pointLineTest(v.a, v.b) == Point_dt.RIGHT))
				return v.abnext;
			if ((p.pointLineTest(v.b, v.c) == Point_dt.RIGHT))
				return v.bcnext;
			if ((p.pointLineTest(v.c, v.a) == Point_dt.RIGHT))
				return v.canext;
			return null;
		}

		/// <summary>
		/// assumes v is an halfplane! - returns another (none halfplane) triangle 
		/// </summary>
		private static Triangle_dt findnext2(Point_dt p, Triangle_dt v)
		{
			if ((((v.abnext != null)) & (!(v.abnext.isHalfplane))))
				return v.abnext;
			if ((((v.bcnext != null)) & (!(v.bcnext.isHalfplane))))
				return v.bcnext;
			if ((((v.canext != null)) & (!(v.canext.isHalfplane))))
				return v.canext;
			return null;
		}

		/// <summary>
		/// Search for p within current triangulation
		/// </summary>
		/// <param name="p">Query point</param>
		/// <returns>Return true if p is within current triangulation (in its 2D convex hull).</returns>
		public bool contains(Point_dt p)
		{
			Triangle_dt tt = Find(p);
			return !tt.isHalfplane;
		}

		/// <summary>
		/// Search for x/y point within current triangulation
		/// </summary>
		/// <param name="x">Query point x coordinate</param>
		/// <param name="y">Query point y coordinate</param>
		/// <returns>Return true if x/y is within current triangulation (in its 2D convex hull).</returns>
		public bool contains(double x, double y)
		{
			Triangle_dt tt = Find(new Point_dt(x, y));
			return !tt.isHalfplane;
		}

		/// <summary>
		/// Return point with x/y and updated Z value (z value is as given by the triangulation)
		/// </summary>
		/// <param name="p">Query point (x/y=</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public Point_dt z(Point_dt p)
		{
			Triangle_dt t = Find(p);
			return t.z(p);
		}

		/// <summary>
		/// Return point with x/y and updated Z value (z value is as given by the triangulation)
		/// </summary>
		/// <param name="x">Query point x coordinate</param>
		/// <param name="y">Query point y coordinate</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public Point_dt z(double x, double y)
		{
			return this.z(new Point_dt(x, y));
		}

		private void updateBoundingBox(Point_dt p)
		{
			double x = p.x;
			double y = p.y;
			double z = p.z;
			if (_bb_min == null) {
				_bb_min = new Point_dt(p);
				_bb_max = new Point_dt(p);
			} else {
				if ((x < _bb_min.x)) {
					_bb_min.x = x;
				} else if ((x > _bb_max.x)) {
					_bb_max.x = x;
				}
				if ((y < _bb_min.y)) {
					_bb_min.y = y;
				} else if ((y > _bb_max.y)) {
					_bb_max.y = y;
				}
				if ((z < _bb_min.z)) {
					_bb_min.z = z;
				} else if ((z > _bb_max.z)) {
					_bb_max.z = z;
				}
			}
		}

		/// <summary>
		/// Returns the bounding rectange between the minimum and maximum coordinates
		/// </summary>
		/// <returns>The bounding rectange between the minimum and maximum coordinates</returns>
		public BoundingBox BoundingBox {
			get { return new BoundingBox(_bb_min, _bb_max); }
		}

		/// <summary>
		/// Returns the min point of the bounding box of this triangulation
		/// </summary>
		/// <returns>The min point of the bounding box of this triangulation</returns>
		public Point_dt MinBoundingBox {
			get { return _bb_min; }
		}

		/// <summary>
		/// Returns the max point of the bounding box of this triangulation
		/// </summary>
		/// <returns>The max point of the bounding box of this triangulation</returns>
		public Point_dt MaxBoundingBox {
			get { return _bb_max; }
		}

		/// <summary>
		/// computes the current set of all triangles and return an iterator to them.
		/// </summary>
		/// <returns>An iterator to the current set of all triangles</returns>
		public System.Collections.Generic.IEnumerator<Triangle_dt> trianglesIterator()
		{
			if (this.Size() <= 2) {
				_triangles = new List<Triangle_dt>();
			}
			initTriangles();
			return _triangles.GetEnumerator();
		}

		/// <summary>
		/// Returns an iterator to the set of points composing this triangulation
		/// </summary>
		/// <returns>An iterator to the set of points composing this triangulation</returns>
		public System.Collections.Generic.IEnumerator<Point_dt> verticesIterator()
		{
            return _vertices.GetEnumerator();
		}

		private void initTriangles()
		{
			if (_modCount == _modCount2)
				return;
			if (this.Size() > 2) {
				_modCount2 = _modCount;
				List<Triangle_dt> front = new List<Triangle_dt>();
				_triangles = new List<Triangle_dt>();
				front.Add(this.startTriangle);
				while ((front.Count > 0)) {
					Triangle_dt t = front[0];
					front.RemoveAt(0);
					if ((t.mark == false)) {
						t.mark = true;
						_triangles.Add(t);
						if (((t.abnext != null)) && (!t.abnext.mark)) {
							front.Add(t.abnext);
						}
						if (((t.bcnext != null)) && (!t.bcnext.mark)) {
							front.Add(t.bcnext);
						}
						if (((t.canext != null)) && (!t.canext.mark)) {
							front.Add(t.bcnext);
						}
					}
				}
				foreach (Triangle_dt aTriangle in _triangles) {
					aTriangle.mark = false;
				}
			}
		}

		/// <summary>
		/// Index the triangulation using a grid index
		/// </summary>
		/// <param name="xCellCount">number of grid cells in a row</param>
		/// <param name="yCellCount">number of grid cells in a column</param>
		/// <remarks></remarks>
		public void IndexData(int xCellCount, int yCellCount)
		{
			gridIndex = new GridIndex(this, xCellCount, yCellCount);
		}

		/// <summary>
		/// Remove any existing spatial indexing
		/// </summary>
		public void RemoveIndex()
		{
			gridIndex = null;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
