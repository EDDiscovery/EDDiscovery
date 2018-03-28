
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{

	/// <summary>
	/// A BoundingBox2d stores a couple of maximal and minimal limit on
	///  each dimension (x, y) in cartesian coordinates. It provides functions for enlarging
	///  the box by adding cartesian coordinates or an other
	///  BoundingBox2d (that is equivalent to computing the union of the
	///  current BoundingBox and another one).
	/// </summary>
	public class BoundingBox2d
	{

		private double m_xmin;
		private double m_xmax;
		private double m_ymin;

		private double m_ymax;
		/// <summary>
		/// Initialize a BoundingBox by calling its reset method.
		/// </summary>
		public BoundingBox2d()
		{
			reset();
		}

		/// <summary>
		/// Initialize a BoundingBox by calling its reset method and then adding a set of coordinates
		/// </summary>
		public BoundingBox2d(List<Coord2d> lst)
		{
			reset();
			foreach (Coord2d c in lst) {
				@add(c);
			}
		}

		/// <summary>
		/// Initialize a BoundingBox with raw values.
		/// </summary>
		public BoundingBox2d(double xmin, double xmax, double ymin, double ymax)
		{
			m_xmin = xmin;
			m_xmax = xmax;
			m_ymin = ymin;
			m_ymax = ymax;
		}

		/// <summary>
		///  Initialize the bounding box with Float.MAX_VALUE as minimum
		/// value, and -Float.MAX_VALUE as maximum value for each dimension.
		/// </summary>
		public void reset()
		{
			m_xmin = double.MaxValue;
			m_xmax = double.MinValue;
			m_ymin = double.MaxValue;
			m_ymax = double.MinValue;
		}

		/// <summary>
		/// Adds an x,y point to the bounding box, and enlarge the bounding
		/// box if this points lies outside of it.
		/// </summary>
		public void @add(double x, double y)
		{
			if (x > m_xmax)
				m_xmax = x;
			if (x < m_xmin)
				m_xmin = x;
			if (y > m_xmax)
				m_ymax = y;
			if (y < m_xmin)
				m_ymin = y;
		}

		/// <summary>
		/// Adds a <see cref="Coord2d"/> point to the bounding box, and enlarge the bounding
		/// box if this points lies outside of it.
		/// </summary>
		public void @add(Coord2d p)
		{
			this.@add(p.x, p.y);
		}

		/// <summary>
		/// Adds another <see cref="BoundingBox2d"/> to the bounding box, and enlarge the bounding
		/// box if its points lies outside of it (i.e. merge other bounding box inside current one)
		/// </summary>
		public void @add(BoundingBox2d b)
		{
			this.@add(b.m_xmin, b.m_ymin);
			this.@add(b.m_xmax, b.m_ymax);
		}

		/// <summary>
		/// Compute and return the center point of the BoundingBox3d
		/// </summary>
		public Coord2d getCenter()
		{
			return new Coord2d((m_xmax + m_xmin) / 2, (m_ymax + m_ymin) / 2);
		}

		/// <summary>
		/// Return the radius of the Sphere containing the Bounding Box,
		/// i.e., the distance between the center and the point (xmin, ymin).
		/// </summary>
		public double getRadius()
		{
			return getCenter().distance(new Coord2d(m_xmin, m_ymin));
		}

		/// <summary>
		/// Return a copy of the current bounding box after scaling all limits relative to 0,0
		/// Scaling does not modify the current bounding box.
		/// </summary>
		/// <remarks>Current object is not modified, a new one is created.</remarks>
		public BoundingBox2d scale(Coord2d factors)
		{
			BoundingBox2d b = new BoundingBox2d();
			b.m_xmax = m_xmax * factors.x;
			b.m_xmin = m_xmin * factors.x;
			b.m_ymax = m_ymax * factors.y;
			b.m_ymin = m_ymin * factors.y;
			return b;
		}

		/// <summary>
		/// Return true if <paramref name="anotherBox"/> is contained in this box.
		/// </summary>
		/// <remarks>if b1.contains(b2), then b1.intersect(b2) as well.</remarks>
		public bool contains(BoundingBox2d anotherBox)
		{
			return m_xmin <= anotherBox.m_xmin & anotherBox.m_xmax <= m_xmax & m_ymin <= anotherBox.m_ymin & anotherBox.m_ymax <= m_ymax;
		}

		/// <summary>
		/// Return true if <paramref name="aPoint"/> is contained in this box.
		/// </summary>
		public bool contains(Coord2d aPoint)
		{
			return m_xmin <= aPoint.x & aPoint.x <= m_xmax & m_ymin <= aPoint.y & aPoint.y <= m_ymax;
		}

		/// <summary>
		/// Return true if <paramref name="anotherBox"/> intersects with this box.
		/// </summary>
		public bool intersect(BoundingBox2d anotherBox)
		{
			return (m_xmin <= anotherBox.m_xmin & anotherBox.m_xmin <= m_xmax) | (m_xmin <= anotherBox.m_xmax & anotherBox.m_xmax <= m_xmax) & (m_ymin <= anotherBox.m_ymin & anotherBox.m_ymin <= m_ymax) | (m_ymin <= anotherBox.m_ymax & anotherBox.m_ymax <= m_ymax);
		}

		/// <summary>
		/// Bounding box min x value
		/// </summary>
		public double xmin {
			get { return m_xmin; }
		}

		/// <summary>
		/// Bounding box max x value
		/// </summary>
		public double xmax {
			get { return m_xmax; }
		}

		/// <summary>
		/// Bounding box min y value
		/// </summary>
		public double ymin {
			get { return m_ymin; }
		}

		/// <summary>
		/// Bounding box max y value
		/// </summary>
		public double ymax {
			get { return m_ymax; }
		}

		public override string ToString()
		{
			return "(BoundingBox2d)" + xmin + "<=x<=" + xmax + " | " + ymin + "<=y<=" + ymax;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
