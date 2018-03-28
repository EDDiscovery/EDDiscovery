
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{

	/// <summary>
	/// Storage for a 2 dimensional vector defined by two points.
	/// Provide the vector function that returns the vector
	/// as a Coord3d, as well as dot product and norm.
	/// </summary>
	public class Vector2d
	{

		#region "Members"
		private double x1;
		private double x2;
		private double y1;
			#endregion
		private double y2;


		#region "Constructors"

		public Vector2d(double x1, double x2, double y1, double y2)
		{
			this.x1 = x1;
			this.x2 = x2;
			this.y1 = y1;
			this.y2 = y2;
		}

		public Vector2d(Coord2d p1, Coord2d p2) : this(p1.x, p2.x, p1.y, p2.y)
		{
		}

		#endregion

		#region "Functions"

		/// <summary>
		/// Return the vector (sizes) induced by this set of coordinates
		/// </summary>
		public Coord2d vector()
		{
			return new Coord2d(x2 - x1, y2 - y1);
		}

		/// <summary>
		/// Compute the dot product between the current and given vector
		/// </summary>
		public double dot(Vector2d v)
		{
			Coord2d v1 = this.vector();
			Coord2d v2 = v.vector();
			return v1.x * v2.x + v1.y * v2.y;
		}

		public double norm()
		{
			return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
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
