
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{

	/// <summary>
	/// An Angle3d stores three 3d points, considering the angle is on the second one.
	/// An instance may return angle(), cos() and sin()
	/// </summary>
	public class Angle3d
	{

		#region "Members"
		private double x1;
		private double x2;
		private double x3;
		private double y1;
		private double y2;
		private double y3;
		private double z1;
		private double z2;
			#endregion
		private double z3;

		#region "Constructors"

		public Angle3d(double x1, double x2, double x3, double y1, double y2, double y3, double z1, double z2, double z3)
		{
			this.x1 = x1;
			this.x2 = x2;
			this.x3 = x3;
			this.y1 = y1;
			this.y2 = y2;
			this.y3 = y3;
			this.z1 = z1;
			this.z2 = z2;
			this.z3 = z3;
		}

		/// <summary>
		/// Create an angle, described by three coordinates.
		/// The angle is supposed to be on p2
		/// </summary>
		public Angle3d(Coord3d p1, Coord3d p2, Coord3d p3) : this(p1.x, p2.x, p3.x, p1.y, p2.y, p3.y, p1.z, p2.z, p3.z)
		{
		}

		#endregion

		#region "Functions"

		/// <summary>
		/// Computes the sinus of the angle
		/// </summary>
		public double sin()
		{
			Coord3d c2 = new Coord3d(x2, y2, z2);
			Vector3d v1 = new Vector3d(x1, y1, z1, x2, y2, z2);
			Vector3d v3 = new Vector3d(x3, y3, z3, x2, y2, z2);
			Coord3d c4 = v1.cross(v3).@add(c2);
			Vector3d v4 = new Vector3d(c4, c2);
			return (c4.z >= 0 ? 1 : -1) * v4.norm() / (v1.norm() * v3.norm());
		}

		/// <summary>
		/// Computes the cosinus of the angle
		/// </summary>
		public double cos()
		{
			Vector3d v1 = new Vector3d(x1, y1, z1, x2, y2, z2);
			Vector3d v3 = new Vector3d(x3, y3, z3, x2, y2, z2);
            return v1.dot(v3) / (v1.norm() * v3.norm());
		}

		/// <summary>
		/// Computes an angle between 0 and 2*PI
		/// </summary>
		public double angle()
		{
			// between 0 and PI: Math.acos(cos());
			if ((sin() > 0)) {
				return Math.Acos(cos());
			} else {
				return Math.PI * 2 - Math.Acos(cos());
			}
		}

		#endregion
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
