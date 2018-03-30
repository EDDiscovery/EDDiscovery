
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Plot3D.Builder.Delaunay.Jdt
{

	/// <summary>
	/// This class represents a 3D triangle in a Triangulation
	/// </summary>
	public class Triangle_dt
	{
		private Point_dt _a;
		private Point_dt _b;
		private Point_dt _c;
		private Triangle_dt _abnext;
		private Triangle_dt _bcnext;
		private Triangle_dt _canext;
		private Circle_dt _circum;
			//modcounter for triangulation fast update
		private int _mc = 0;
			//true if it is an infinite face
		private bool _halfplane = false;
			//tag - for bfs algorithms
		private bool _mark = false;
		private int _counter = 0;

		private int _c2 = 0;
		/// <summary>
		/// Constructs a triangle form 3 point - store it in counterclockwised order.
		/// A should be before B and B before C in counterclockwise order.
		/// </summary>
		/// <param name="A"></param>
		/// <param name="B"></param>
		/// <param name="C"></param>
		/// <remarks></remarks>
		public Triangle_dt(Point_dt A, Point_dt B, Point_dt C)
		{
			_a = A;
			int res = C.pointLineTest(A, B);
			if ((res <= Point_dt.LEFT | (res == Point_dt.INFRONTOFA | res == Point_dt.BEHINDB))) {
				_b = B;
				_c = C;
			//RIGHT
			} else {
				Console.WriteLine("Warning, Triangle_dt(A,B,C) expects points in counterclockwise order.");
                Console.WriteLine(A.ToString() + B.ToString() + C.ToString());
				_b = C;
				_c = B;
			}
			circumcircle();
		}

		/// <summary>
		/// Creates a half plane using the segment (A,B).
		/// </summary>
		public Triangle_dt(Point_dt A, Point_dt B)
		{
			_a = A;
			_b = B;
			_halfplane = true;
		}

		/// <summary>
		/// Returns true if this triangle is actually a half plane
		/// </summary>
		public bool isHalfplane {
			get { return _halfplane; }
			set { _halfplane = value; }
		}

		/// <summary>
		/// tag - for bfs algorithms
		/// </summary>
		public bool mark {
			get { return _mark; }
			set { _mark = value; }
		}

		/// <summary>
		/// Returns the first vertex of this triangle.
		/// </summary>
		public Point_dt p1 {
			get { return _a; }
		}

		/// <summary>
		/// Returns the second vertex of this triangle.
		/// </summary>
		public Point_dt p2 {
			get { return _b; }
		}

		/// <summary>
		/// Returns the third vertex of this triangle.
		/// </summary>
		public Point_dt p3 {
			get { return _c; }
		}

		/// <summary>
		/// Returns the consecutive triangle which shares this triangle p1,p2 edge.
		/// </summary>
		public Triangle_dt next_12 {
			get { return _abnext; }
		}

		/// <summary>
		/// Returns the consecutive triangle which shares this triangle p2,p3 edge.
		/// </summary>
		public Triangle_dt next_23 {
			get { return _bcnext; }
		}

		/// <summary>
		/// Returns the consecutive triangle which shares this triangle p3,p1 edge.
		/// </summary>
		public Triangle_dt next_31 {
			get { return _canext; }
		}

		/// <summary>
		/// The bounding rectange between the minimum and maximum coordinates of the triangle.
		/// </summary>
		public BoundingBox BoundingBox {
			get {
				dynamic lowerLeft = default(Point_dt);
				Point_dt upperRight = new Point_dt();
				lowerLeft = new Point_dt(Math.Min(_a.x, Math.Min(_b.x, _c.x)), Math.Min(_a.y, Math.Min(_b.y, _c.y)));
				upperRight = new Point_dt(Math.Max(_a.x, Math.Max(_b.x, _c.x)), Math.Max(_a.y, Math.Max(_b.y, _c.y)));
				return new BoundingBox(lowerLeft, upperRight);
			}
		}

		public void switchneighbors(Triangle_dt old_t, Triangle_dt new_t)
		{
			if (_abnext.Equals(old_t)) {
				_abnext = new_t;
			} else if (_bcnext.Equals(old_t)) {
				_bcnext = new_t;
			} else if (_canext.Equals(old_t)) {
				_canext = new_t;
			} else {
				Console.WriteLine("Error, switchneighbors can't find Old.");
			}
		}

		public Triangle_dt neighbor(Point_dt p)
		{
			if (_a.Equals(p)) {
				return _canext;
			} else if (_b.Equals(p)) {
				return _abnext;
			} else if (_c.Equals(p)) {
				return _bcnext;
			} else {
				Console.WriteLine("Error, neighbors can't find p.");
				return null;
			}
		}

		/// <summary>
		/// Returns the neighbors that shares the given corner and is not the previous triangle.
		/// </summary>
		/// <param name="p">The given corner.</param>
		/// <param name="prevTriangle">The previous triangle.</param>
		/// <returns>The neighbors that shares the given corner and is not the previous triangle.</returns>
		public object nextNeighbor(Point_dt p, Triangle_dt prevTriangle)
		{
			Triangle_dt neighbor = null;
			if (_a.Equals(p)) {
				neighbor = _canext;
			} else if (_b.Equals(p)) {
				neighbor = _abnext;
			} else if (_c.Equals(p)) {
				neighbor = _bcnext;
			}
			if (neighbor.Equals(prevTriangle) | neighbor.isHalfplane) {
				if (_a.Equals(p)) {
					neighbor = _abnext;
				} else if (_b.Equals(p)) {
					neighbor = _bcnext;
				} else if (_c.Equals(p)) {
					neighbor = _canext;
				}
			}
			return neighbor;
		}

		public Circle_dt circumcircle()
		{
			double u = ((_a.x - _b.x) * (_a.x + _b.x) + (_a.y - _b.y) * (_a.y + _b.y)) / 2.0;
			double v = ((_b.x - _c.x) * (_b.x + _c.x) + (_b.y - _c.y) * (_b.y + _c.y)) / 2.0;
			double den = (_a.x - _b.x) * (_b.y - _c.y) - (_b.x - _c.x) * (_a.y - _b.y);
			// oops, degenerate case
			if ((den == 0)) {
				_circum = new Circle_dt(_a, double.PositiveInfinity);
			} else {
				Point_dt cen = new Point_dt((u * (_b.y - _c.y) - v * (_a.y - _b.y)) / den, (v * (_a.x - _b.x) - u * (_b.x - _c.x)) / den);
				_circum = new Circle_dt(cen, cen.distance2(_a));
			}
			return _circum;
		}

		public bool circumcircle_contains(Point_dt p)
		{
			return _circum.Radius > _circum.Center.distance2(p);
		}

		public override string ToString()
		{
			string res = "";
			res += "A: " + _a.ToString() + " B: " + _b.ToString();
			if ((!_halfplane)) {
				res += " C: " + _c.ToString();
			}
			return res;
		}

		/// <summary>
		/// Determines if this triangle contains the point p.
		/// </summary>
		/// <param name="p">The query point</param>
		/// <returns>True if p is not null and is inside this triangle</returns>
		/// <remarks>Note: on boundary is considered inside</remarks>
		public bool contains(Point_dt p)
		{
			if (_halfplane | p == null) {
				return false;
			}
			if (isCorner(p)) {
				return true;
			}
			int a12 = p.pointLineTest(_a, _b);
			int a23 = p.pointLineTest(_b, _c);
			int a31 = p.pointLineTest(_c, _a);
            if ((a12 == Point_dt.LEFT && a23 == Point_dt.LEFT && a31 == Point_dt.LEFT)
                || (a12 == Point_dt.RIGHT && a23 == Point_dt.RIGHT && a31 == Point_dt.RIGHT)
                || (a12 == Point_dt.ONSEGMENT || a23 == Point_dt.ONSEGMENT || a31 == Point_dt.ONSEGMENT))
                return true;
            else
                return false;
		}

		/// <summary>
		/// Determines if this triangle contains the point p.
		/// </summary>
		/// <param name="p">The query point</param>
		/// <returns>True if p is not null and is inside this triangle</returns>
		/// <remarks>Note: on boundary is considered outside</remarks>
		public bool contains_BoundaryIsOutside(Point_dt p)
        {
            if (_halfplane | p == null)
            {
                return false;
            }
            if (isCorner(p))
            {
                return false;
            }
            int a12 = p.pointLineTest(_a, _b);
            int a23 = p.pointLineTest(_b, _c);
            int a31 = p.pointLineTest(_c, _a);
            if ((a12 == Point_dt.LEFT && a23 == Point_dt.LEFT && a31 == Point_dt.LEFT)
                || (a12 == Point_dt.RIGHT && a23 == Point_dt.RIGHT && a31 == Point_dt.RIGHT))
                return true;
            else
                return false;
        }

		/// <summary>
		/// Checks if the given point is a corner of this triangle.
		/// </summary>
		/// <param name="p">The given point.</param>
		/// <returns>True if the given point is a corner of this triangle.</returns>
		public bool isCorner(Point_dt p)
		{
			return (p.x == _a.x & p.y == _a.y) | (p.x == _b.x & p.y == _b.y) | (p.x == _c.x & p.y == _c.y);
		}

		/// <summary>
		/// compute the Z value for the X, Y values of q.
		/// Assume current triangle represent a plane --> q does NOT need to be contained in this triangle.
		/// </summary>
		/// <param name="q">A x/y point.</param>
		/// <returns></returns>
		/// <remarks>Current triangle must not be a halfplane.</remarks>
		public double z_value(Point_dt q)
		{
			if (q == null) {
				throw new ArgumentException("Input point cannot be Nothing", "q");
			}
			if (_halfplane) {
				throw new Exception("Cannot approximate the z value from a halfplane triangle");
			}
			if (q.x == _a.x & q.y == _a.y)
				return _a.z;
			if (q.x == _b.x & q.y == _b.y)
				return _b.z;
			if (q.x == _c.x & q.y == _c.y)
				return _c.z;
			double X = 0;
			double x0 = q.x;
			double x1 = _a.x;
			double x2 = _b.x;
			double x3 = _c.x;
			double Y = 0;
			double y0 = q.y;
			double y1 = _a.y;
			double y2 = _b.y;
			double y3 = _c.y;
			double Z = 0;
			double m01 = 0;
			double k01 = 0;
			double m23 = 0;
			double k23 = 0;
			// 0 - regular, 1-horizontal , 2-vertical
			int flag01 = 0;
			if (x0 != x1) {
				m01 = (y0 - y1) / (x0 - x1);
				k01 = y0 - m01 * x0;
				if (m01 == 0)
					flag01 = 1;
			//2-vertical
			} else {
				flag01 = 2;
				//x01 = x0
			}
			int flag23 = 0;
			if (x2 != x3) {
				m23 = (y2 - y3) / (x2 - x3);
				k23 = y2 - m23 * x2;
				if (m23 == 0)
					flag23 = 1;
			//2-vertical
			} else {
				flag23 = 2;
				//x01 = x0
			}
			if (flag01 == 2) {
				X = x0;
				Y = m23 * X + k23;
			} else if (flag23 == 2) {
				X = x2;
				Y = m01 * X + k01;
			} else {
				X = (k23 - k01) / (m01 - m23);
				Y = m01 * X + k01;
			}
			double r = 0;
			if (flag23 == 2) {
				r = (y2 - Y) / (y2 - y3);
			} else {
				r = (x2 - X) / (x2 - x3);
			}
			Z = _b.z + (_c.z - _b.z) * r;
			if (flag01 == 2) {
				r = (y1 - y0) / (y1 - Y);
			} else {
				r = (x1 - x0) / (x1 - X);
			}
			double qZ = _a.z + (Z - _a.z) * r;
			return qZ;
		}

		/// <summary>
		/// Compute the Z value for the X, Y values 
		/// Assume current triangle represent a plane --> q does NOT need to be contained in this triangle.
		/// </summary>
		/// <returns></returns>
		/// <remarks>Current triangle must not be a halfplane.</remarks>
		public double z(double x, double y)
		{
			return z_value(new Point_dt(x, y));
		}

		/// <summary>
		/// compute the Z value for the X, Y values of q.
		/// Assume current triangle represent a plane --> q does NOT need to be contained in this triangle.
		/// </summary>
		/// <param name="q">A x/y point.</param>
		/// <returns>A new <see cref="Point_dt"/> with same x/y than <paramref name="q"/> and computed z value</returns>
		/// <remarks>Current triangle must not be a halfplane.</remarks>
		public Point_dt z(Point_dt q)
		{
			double newz = z_value(q);
			return new Point_dt(q.x, q.y, newz);
		}

		public Point_dt a {
			get { return _a; }
			set { _a = value; }
		}

		public Point_dt b {
			get { return _b; }
			set { _b = value; }
		}

		public Point_dt c {
			get { return _c; }
			set { _c = value; }
		}

		public Triangle_dt abnext {
			get { return _abnext; }
			set { _abnext = value; }
		}

		public Triangle_dt bcnext {
			get { return _bcnext; }
			set { _bcnext = value; }
		}

		public Triangle_dt canext {
			get { return _canext; }
			set { _canext = value; }
		}

		public int mc {
			get { return _mc; }
			set { _mc = value; }
		}

		public Circle_dt circum {
			get { return _circum; }
		}

	}


}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
