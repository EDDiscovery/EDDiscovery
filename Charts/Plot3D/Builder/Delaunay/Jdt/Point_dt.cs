
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;

namespace nzy3D.Plot3D.Builder.Delaunay.Jdt
{

	/// <summary>
	/// This class represents a 3D point, with some simple geometric methods (pointLineTest).
	/// </summary>
	public class Point_dt
	{

		private double m_x;
		private double m_y;
		private double m_z;
		public const int ONSEGMENT = 0;
        public const int LEFT = 1;
        public const int RIGHT = 2;
        public const int INFRONTOFA = 3;
        public const int BEHINDB = 4;

        public const int ISERROR = 5;
		/// <summary>
		/// Default Constructor. Constructs a 3D point at (0,0,0).
		/// </summary>
		public Point_dt() : this(0, 0)
		{
		}

		/// <summary>
		/// Constructs a 3D point at (x,y,z)
		/// </summary>
		/// <param name="x">x coordinates</param>
		/// <param name="y">y coordinates</param>
		/// <param name="z">z coordinates</param>
		public Point_dt(double x, double y, double z)
		{
			this.m_x = x;
			this.m_y = y;
			this.m_z = z;
		}

		/// <summary>
		/// Constructs a 3D point at (x,y,0)
		/// </summary>
		/// <param name="x">x coordinates</param>
		/// <param name="y">y coordinates</param>
		public Point_dt(double x, double y) : this(x, y, 0)
		{
		}

		/// <summary>
		/// Simple copy constructor
		/// </summary>
		/// <param name="p">Another point</param>
		public Point_dt(Point_dt p) : this(p.x, p.y, p.z)
		{
		}

		public double x {
			get { return m_x; }
			set { m_x = value; }
		}

		public double y {
			get { return m_y; }
			set { m_y = value; }
		}

		public double z {
			get { return m_z; }
			set { m_z = value; }
		}

		public Coord3d Coord3d {
			get { return new Coord3d(x, y, z); }
		}

		public double distance2(Point_dt p) {
			return (p.x - x) * (p.x - x) + (p.y - y) * (p.y - y); 
		}

		public double distance2(double px, double py) {
			return (px - x) * (px - x) + (py - y) * (py - y); 
		}

        public bool isLess(Point_dt p)
        {
			return x < p.x | (x == p.x & y < p.y); 
		}

        public bool isGreater(Point_dt p)
        {
			return x > p.x | (x == p.x & y > p.y); 
		}

		public override string ToString()
		{
			return "(Point_dt) [" + x + "," + y + "," + z + "]";
		}

        public double distance(Point_dt p)
        {
			return Math.Sqrt((p.x - x) * (p.x - x) + (p.y - y) * (p.y - y)); 
		}

		public double distance3D(Point_dt p) {
			return Math.Sqrt((p.x - x) * (p.x - x) + (p.y - y) * (p.y - y) + (p.z - z) * (p.z - z));
		}

		/// <summary>
		///  tests the relation between current point (as a 2D [x,y] point) and a 2D
		/// segment a,b (the Z values are ignored), returns one of the following:
		/// LEFT, RIGHT, INFRONTOFA, BEHINDB, ONSEGMENT, ISERROR
		/// </summary>
		/// <param name="a">The first point of the segment</param>
		/// <param name="b">The second point of the segment</param>
		/// <returns>The value (flag) of the relation between this point and the a,b line-segment.</returns>
		/// <remarks></remarks>
		public int pointLineTest(Point_dt a, Point_dt b)
		{
			double dx = b.x - a.x;
			double dy = b.y - a.y;
			double res = dy * (x - a.x) - dx * (y - a.y);
			if (res < 0) {
				return LEFT;
			}
			if (res > 0) {
				return RIGHT;
			}
			if (dx > 0) {
				if (x < a.x) {
					return INFRONTOFA;
				}
				if (b.x < x) {
					return BEHINDB;
				}
				return ONSEGMENT;
			}
			if (dx < 0) {
				if (x > a.x) {
					return INFRONTOFA;
				}
				if (b.x > x) {
					return BEHINDB;
				}
				return ONSEGMENT;
			}
			if (dy > 0) {
				if (y < a.y) {
					return INFRONTOFA;
				}
				if (b.y < y) {
					return BEHINDB;
				}
				return ONSEGMENT;
			}
			if (dy < 0) {
				if (y > a.y) {
					return INFRONTOFA;
				}
				if (b.y > y) {
					return BEHINDB;
				}
				return ONSEGMENT;
			}
			return ISERROR;
		}

		public Point_dt circumcenter(Point_dt a, Point_dt b)
		{
			double u = ((a.x - b.x) * (a.x + b.x) + (a.y - b.y) * (a.y + b.y)) / 2.0;
			double v = ((b.x - x) * (b.x + x) + (b.y - y) * (b.y + y)) / 2.0;
			double den = (a.x - b.x) * (b.y - y) - (b.x - x) * (a.y - b.y);
			if ((den == 0)) {
				// oops
				Console.WriteLine("circumcenter, degenerate case");
			}
			return new Point_dt((u * (b.y - y) - v * (a.y - b.y)) / den, (v * (a.x - b.x) - u * (b.x - x)) / den);
		}

		public object Comparator(int flag) {
			return new Point_dt_Compare(flag);
		}

		public object Comparator() {
			return new Point_dt_Compare(0);
		}

	}

	public class Point_dt_Compare
	{


		private int m_flag;
		public Point_dt_Compare(int flag)
		{
			m_flag = flag;
		}

		public object Compare(Point_dt o1, Point_dt o2)
		{
			if (!(o1 == null | o2 == null)) {
				if ((m_flag == 0)) {
					if ((o1.x > o2.x))
						return 1;
					if ((o1.x < o2.x))
						return -1;
					// x1 == x2
					if ((o1.y > o2.y))
						return 1;
					if ((o1.y < o2.y))
						return -1;
				} else if ((m_flag == 1)) {
					if ((o1.x > o2.x))
						return -1;
					if ((o1.x < o2.x))
						return 1;
					// x1 == x2
					if ((o1.y > o2.y))
						return -1;
					if ((o1.y < o2.y))
						return 1;
				} else if ((m_flag == 2)) {
					if ((o1.y > o2.y))
						return 1;
					if ((o1.y < o2.y))
						return -1;
					// y1 == y2
					if ((o1.x > o2.x))
						return 1;
					if ((o1.x < o2.x))
						return -1;
				} else if ((m_flag == 3)) {
					if ((o1.y > o2.y))
						return -1;
					if ((o1.y < o2.y))
						return 1;
					// y1 == y2
					if ((o1.x > o2.x))
						return -1;
					if ((o1.x < o2.x))
						return 1;
				}
			} else {
				if (o1 == null & o2 == null) {
					return 0;
				}
				if (o1 == null & ((o2 != null))) {
					return 1;
				}
				if (((o1 != null)) & o2 == null) {
					return -1;
				}
			}
			throw new Exception("Unexpected behavior, comparer should never have reached this point");
		}
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
