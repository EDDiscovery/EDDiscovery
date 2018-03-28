
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{

	/// <summary>
	/// An Angle2d stores three 2d points, considering the angle is on the second one.
	/// An instance may return angle(), cos() and sin()
	/// </summary>
	public class Angle2d
	{

		#region "Members"
		private double x1;
		private double x2;
		private double x3;
		private double y1;
		private double y2;
			#endregion
		private double y3;

		#region "Constructors"

		public Angle2d(double x1, double x2, double x3, double y1, double y2, double y3)
		{
			this.x1 = x1;
			this.x2 = x2;
			this.x3 = x3;
			this.y1 = y1;
			this.y2 = y2;
			this.y3 = y3;
		}

		public Angle2d(Coord2d p1, Coord2d p2, Coord2d p3) : this(p1.x, p2.x, p3.x, p1.y, p2.y, p3.y)
		{
		}

		#endregion

		#region "Functions"

		/// <summary>
		/// Computes the sinus of the angle
		/// </summary>
		public double sin()
		{
			double x4 = 0;
			//(y1-y2)*(z3-z2) - (z1-z2)*(y3-y2);
			double y4 = 0;
			//(z1-z2)*(x3-x2) - (x1-x2)*(z3-z2);
			double z4 = (x1 - x2) * (y3 - y2) - (y1 - y2) * (x3 - x2);
			Vector3d v1 = new Vector3d(x1, y1, 0, x2, y2, 0);
			Vector3d v3 = new Vector3d(x3, y3, 0, x2, y2, 0);
			Vector3d v4 = new Vector3d(x4, y4, z4, x2, y2, 0);
            return (z4 >= 0 ? 1 : -1) * v4.norm() / (v1.norm() * v3.norm());
		}

		/// <summary>
		/// Computes the cosinus of the angle
		/// </summary>
		public double cos()
		{
			Vector2d v1 = new Vector2d(x1, y1, x2, y2);
			Vector2d v3 = new Vector2d(x3, y3, x2, y2);
            return v1.dot(v3) / (v1.norm() * v3.norm());
		}

		/// <summary>
		/// Computes the angle
		/// </summary>
		public double angle()
		{
			Vector2d v1 = new Vector2d(x1, y1, x2, y2);
			Vector2d v3 = new Vector2d(x3, y3, x2, y2);
			return Math.Acos(v1.dot(v3));
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
