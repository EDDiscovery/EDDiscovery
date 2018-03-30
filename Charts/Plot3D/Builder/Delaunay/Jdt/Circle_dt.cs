
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Plot3D.Builder.Delaunay.Jdt
{

	/// <summary>
	/// This class represents a 3D simple circle used by the Delaunay Triangulation class
	/// </summary>
	public class Circle_dt
	{
		private Point_dt m_c;

		private double m_r;
		/// <summary>
		/// Constructs a new Circle_dt.
		/// </summary>
		/// <param name="c">Center of the circle.</param>
		/// <param name="r">Radius of the circle.</param>
		/// <remarks></remarks>
		public Circle_dt(Point_dt c, double r)
		{
			m_c = c;
			m_r = r;
		}

		/// <summary>
		/// Copy Constructor. Creates a new Circle with same properties of <paramref name="circle"/>
		/// </summary>
		/// <param name="circle">Circle to clone.</param>
		/// <remarks></remarks>
		public Circle_dt(Circle_dt circle) : this(circle.Center, circle.Radius)
		{
		}

		/// <summary>
		/// Gets the center of the circle.
		/// </summary>
		/// <returns>The center of the circle.</returns>
		public Point_dt Center {
			get { return m_c; }
		}

		/// <summary>
		/// Gets the radius of the circle.
		/// </summary>
		/// <returns>The radius of the circle.</returns>
		public double Radius {
			get { return m_r; }
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
