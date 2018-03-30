
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{

	/// <summary>
	/// Storage for a 3 dimensional vector defined by two points.
	/// Provide the vector function that returns the vector
	/// as a Coord3d, as well as dot product and norm.
	/// </summary>
	public class Vector3d
	{

		private double m_x1;
		private double m_x2;
		private double m_y1;
		private double m_y2;
		private double m_z1;

		private double m_z2;
		public Vector3d(double x1, double x2, double y1, double y2, double z1, double z2)
		{
			m_x1 = x1;
			m_x2 = x2;
			m_y1 = y1;
			m_y2 = y2;
			m_z1 = z1;
			m_z2 = z2;
		}

		public Vector3d(Coord3d p1, Coord3d p2) : this(p1.x, p2.x, p1.y, p2.y, p1.z, p2.z)
		{
		}

		/// <summary>
		/// Return the vector induced by this set of coordinates
		/// </summary>
		public Coord3d vector {
			get { return new Coord3d(m_x2 - m_x1, m_y2 - m_y1, m_z2 - m_z1); }
		}

		/// <summary>
		/// Compute the dot product between and current and given vector.
		/// </summary>
		/// <remarks>Remind that the dot product is 0 if vectors are perpendicular</remarks>
		public double dot(Vector3d v)
		{
			return this.vector.dot(v.vector);
		}

		/// <summary>
		/// Computes the vectorial product of the current and the given vector.
		/// The result is a vector defined as a Coord3d, that is perpendicular to
		/// the plan induced by current vector and vector V.
		/// </summary>
		public Coord3d cross(Vector3d v)
		{
			Coord3d v1 = this.vector;
			Coord3d v2 = v.vector;
			Coord3d v3 = new Coord3d();
			v3.x = v1.y * v2.z - v1.z * v2.y;
			v3.y = v1.z * v2.x - v1.x * v2.z;
			v3.z = v1.x * v2.y - v1.y * v2.x;
			return v3;
		}

		/// <summary>
		/// Compute the norm of this vector.
		/// </summary>
		public double norm()
		{
			return Math.Sqrt(this.vector.magSquared());
		}

		/// <summary>
		/// Compute the distance between two coordinates.
		/// </summary>
		public double distance(Coord3d c)
		{
			return this.Center.distance(c);
		}

		public Coord3d Center {
			get { return new Coord3d((m_x1 + m_x2) / 2, (m_y1 + m_y2) / 2, (m_z1 + m_z2) / 2); }
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
