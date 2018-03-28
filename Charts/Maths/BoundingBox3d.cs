
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Plot3D.Primitives;

namespace nzy3D.Maths
{

	/// <summary>
	/// A BoundingBox3d stores a couple of maximal and minimal limit on
	///  each dimension (x, y) in cartesian coordinates. It provides functions for enlarging
	///  the box by adding cartesian coordinates or an other
	///  BoundingBox3d (that is equivalent to computing the union of the
	///  current BoundingBox and another one).
	/// </summary>
	public class BoundingBox3d
	{

		private double m_xmin;
		private double m_xmax;
		private double m_ymin;
		private double m_ymax;
		private double m_zmin;

		private double m_zmax;
		/// <summary>
		/// Initialize a BoundingBox by calling its reset method.
		/// </summary>
		public BoundingBox3d()
		{
			reset();
		}

		/// <summary>
		/// Initialize a BoundingBox by calling its reset method and then adding a set of coordinates
		/// </summary>
		public BoundingBox3d(List<Coord3d> lst)
		{
			reset();
			foreach (Coord3d c in lst) {
				@add(c);
			}
		}

		/// <summary>
		/// Initialize a BoundingBox by calling its reset method and then adding a set of coordinates from a polygon
		/// </summary>
		public BoundingBox3d(Polygon pol)
		{
			reset();
			foreach (Point p in pol.GetPoints) {
				@add(p);
			}
		}

		/// <summary>
		/// Initialize a BoundingBox with given centre and edgeLength (equals in all directions)
		/// </summary>
		public BoundingBox3d(Coord3d center, double edgeLength) : this(center.x - edgeLength / 2, center.x + edgeLength / 2, center.y - edgeLength / 2, center.y + edgeLength / 2, center.z - edgeLength / 2, center.z + edgeLength / 2)
		{
		}

		/// <summary>
		/// Initialize a BoundingBox with another bounding box (i.e. performs a copy)
		/// </summary>
		public BoundingBox3d(BoundingBox3d anotherBox) : this(anotherBox.m_xmin, anotherBox.m_xmax, anotherBox.m_ymin, anotherBox.m_ymax, anotherBox.m_zmin, anotherBox.m_zmax)
		{
		}

		/// <summary>
		/// Initialize a BoundingBox with raw values.
		/// </summary>
		public BoundingBox3d(double xmin, double xmax, double ymin, double ymax, double zmin, double zmax)
		{
			m_xmin = xmin;
			m_xmax = xmax;
			m_ymin = ymin;
			m_ymax = ymax;
			m_zmin = zmin;
			m_zmax = zmax;
		}

		/// <summary>
		///  Initialize the bounding box with Double.MAX_VALUE as minimum
		/// value, and Double.MIN_VALUE as maximum value for each dimension.
		/// </summary>
		public void reset()
		{
			m_xmin = double.MaxValue;
			m_xmax = double.MinValue;
			m_ymin = double.MaxValue;
			m_ymax = double.MinValue;
			m_zmin = double.MaxValue;
			m_zmax = double.MinValue;
		}

		/// <summary>
		/// Check if bounding box is valid (i.e. limits are consistents).
		/// </summary>
		public bool valid()
		{
			return (m_xmin <= m_xmax & m_ymin <= m_ymax & m_zmin <= m_zmax);
		}

		/// <summary>
		/// Adds an x,y,z point to the bounding box, and enlarge the bounding
		/// box if this points lies outside of it.
		/// </summary>
		public void @add(double x, double y, double z)
		{
			if (x > m_xmax)
				m_xmax = x;
			if (x < m_xmin)
				m_xmin = x;
			if (y > m_ymax)
				m_ymax = y;
			if (y < m_ymin)
				m_ymin = y;
			if (z > m_zmax)
				m_zmax = z;
			if (z < m_zmin)
				m_zmin = z;
		}

		/// <summary>
		/// Adds a <see cref="Coord3d"/> point to the bounding box, and enlarge the bounding
		/// box if this points lies outside of it.
		/// </summary>
		public void @add(Coord3d p)
		{
			this.@add(p.x, p.y, p.z);
		}

		/// <summary>
		/// Adds a set of coordinates from a polygon to the bounding box
		/// </summary>
        public void @add(Polygon pol)
		{
			foreach (Point p in pol.GetPoints) {
				@add(p);
			}
		}

		/// <summary>
		/// Adds a point to the bounding box
		/// </summary>
        public void @add(Point p)
		{
			@add(p.xyz.x, p.xyz.y, p.xyz.z);
		}

		/// <summary>
		/// Adds another <see cref="BoundingBox3d"/> to the bounding box, and enlarge the bounding
		/// box if its points lies outside of it (i.e. merge other bounding box inside current one)
		/// </summary>
		public void Add(BoundingBox3d b)
		{
			this.@add(b.m_xmin, b.m_ymin, b.m_zmin);
			this.@add(b.m_xmax, b.m_ymax, b.m_zmax);
		}

		/// <summary>
		/// Compute and return the center point of the BoundingBox3d
		/// </summary>
		public Coord3d getCenter()
		{
			return new Coord3d((m_xmax + m_xmin) / 2, (m_ymax + m_ymin) / 2, (m_zmax + m_zmin) / 2);
		}

		/// <summary>
		/// Return the radius of the Sphere containing the Bounding Box,
		/// i.e., the distance between the center and the point (xmin, ymin, zmin).
		/// </summary>
		public double getRadius()
		{
			return getCenter().distance(new Coord3d(m_xmin, m_ymin, m_zmin));
		}

		/// <summary>
		/// Return a copy of the current bounding box after scaling all limits relative to 0,0,0
		/// Scaling does not modify the current bounding box.
		/// </summary>
		/// <remarks>Current object is not modified, a new one is created.</remarks>
		public BoundingBox3d scale(Coord3d factors)
		{
			BoundingBox3d b = new BoundingBox3d();
			b.m_xmax = m_xmax * factors.x;
			b.m_xmin = m_xmin * factors.x;
			b.m_ymax = m_ymax * factors.y;
			b.m_ymin = m_ymin * factors.y;
			b.m_zmax = m_zmax * factors.z;
			b.m_zmin = m_zmin * factors.z;
			return b;
		}

		/// <summary>
		/// Return a copy of the current bounding box after shitfing all limits
		/// Shifting does not modify the current bounding box.
		/// </summary>
		/// <remarks>Current object is not modified, a new one is created.</remarks>
		public BoundingBox3d shift(Coord3d offset)
		{
			BoundingBox3d b = new BoundingBox3d();
			b.m_xmax = m_xmax + offset.x;
			b.m_xmin = m_xmin + offset.x;
			b.m_ymax = m_ymax + offset.y;
			b.m_ymin = m_ymin + offset.y;
			b.m_zmax = m_zmax + offset.z;
			b.m_zmin = m_zmin + offset.z;
			return b;
		}

		/// <summary>
		/// Return a copy of the current bounding box after adding a margin to all limits (positiv to max limits, negativ to min limits)
		/// </summary>
		/// <remarks>Current object is not modified, a new one is created.</remarks>
		public BoundingBox3d margin(double marg)
		{
			BoundingBox3d b = new BoundingBox3d();
			b.m_xmax = m_xmax + marg;
			b.m_xmin = m_xmin - marg;
			b.m_ymax = m_ymax + marg;
			b.m_ymin = m_ymin - marg;
			b.m_zmax = m_zmax + marg;
			b.m_zmin = m_zmin - marg;
			return b;
		}

		/// <summary>
		/// Return a copy of the current bounding box after adding a margin to all limits (positiv to max limits, negativ to min limits)
		/// </summary>
		/// <remarks>Modify current object.</remarks>
		public void selfMargin(double marg)
		{
			BoundingBox3d b = new BoundingBox3d();
			m_xmax += marg;
			m_xmin -= marg;
			m_ymax += marg;
			m_ymin -= marg;
			m_zmax += marg;
			m_zmin -= marg;
		}


		/// <summary>
		/// Return true if <paramref name="anotherBox"/> is contained in this box.
		/// </summary>
		/// <remarks>if b1.contains(b2), then b1.intersect(b2) as well.</remarks>
		public bool contains(BoundingBox3d anotherBox)
		{
			return m_xmin <= anotherBox.m_xmin & anotherBox.m_xmax <= m_xmax & m_ymin <= anotherBox.m_ymin & anotherBox.m_ymax <= m_ymax & m_zmin <= anotherBox.m_zmin & anotherBox.m_zmax <= m_zmax;
		}

		/// <summary>
		/// Return true if <paramref name="aPoint"/> is contained in this box.
		/// </summary>
		public bool contains(Coord3d aPoint)
		{
			return m_xmin <= aPoint.x & aPoint.x <= m_xmax & m_ymin <= aPoint.y & aPoint.y <= m_ymax & m_zmin <= aPoint.z & aPoint.z <= m_zmax;
		}

		/// <summary>
		/// Return true if <paramref name="anotherBox"/> intersects with this box.
		/// </summary>
		public bool intersect(BoundingBox3d anotherBox)
		{
			return (m_xmin <= anotherBox.m_xmin & anotherBox.m_xmin <= m_xmax) | (m_xmin <= anotherBox.m_xmax & anotherBox.m_xmax <= m_xmax) & (m_ymin <= anotherBox.m_ymin & anotherBox.m_ymin <= m_ymax) | (m_ymin <= anotherBox.m_ymax & anotherBox.m_ymax <= m_ymax) & (m_zmin <= anotherBox.m_zmin & anotherBox.m_zmin <= m_zmax) | (m_zmin <= anotherBox.m_zmax & anotherBox.m_zmax <= m_zmax);
		}

		/// <summary>
		/// Bounding box min x value
		/// </summary>
		public double xmin {
			get { return m_xmin; }
			set { m_xmin = value; }
		}

		/// <summary>
		/// Bounding box max x value
		/// </summary>
		public double xmax {
			get { return m_xmax; }
			set { m_xmax = value; }
		}

		/// <summary>
		/// Bounding box min y value
		/// </summary>
		public double ymin {
			get { return m_ymin; }
			set { m_ymin = value; }
		}

		/// <summary>
		/// Bounding box max y value
		/// </summary>
		public double ymax {
			get { return m_ymax; }
			set { m_ymax = value; }
		}

		/// <summary>
		/// Bounding box min z value
		/// </summary>
		public double zmin {
			get { return m_zmin; }
			set { m_zmin = value; }
		}

		/// <summary>
		/// Bounding box max z value
		/// </summary>
		public double zmax {
			get { return m_zmax; }
			set { m_zmax = value; }
		}

		public List<Coord3d> Vertices {
			get {
				List<Coord3d> edges = new List<Coord3d>();
				edges.Add(new Coord3d(xmin, ymin, zmin));
				edges.Add(new Coord3d(xmin, ymax, zmin));
				edges.Add(new Coord3d(xmax, ymax, zmin));
				edges.Add(new Coord3d(xmax, ymin, zmin));
				edges.Add(new Coord3d(xmin, ymin, zmax));
				edges.Add(new Coord3d(xmin, ymax, zmax));
				edges.Add(new Coord3d(xmax, ymax, zmax));
				edges.Add(new Coord3d(xmax, ymin, zmax));
				return edges;
			}
		}

		public static BoundingBox3d newBoundsAtOrign()
		{
			return new BoundingBox3d(Coord3d.ORIGIN, 0);
		}

		public override string ToString()
		{
			return "(BoundingBox3d)" + xmin + "<=x<=" + xmax + " | " + ymin + "<=y<=" + ymax + " | " + zmin + "<=y<=" + zmax;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
