
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{

	/// <summary>
	/// A simple utility class for storing a list of x, y, and z coordinates as
	/// arrays of float values.
	/// </summary>
	/// <remarks></remarks>
	public class Coordinates
	{

		private float[] m_x;
		private float[] m_y;

		private float[] m_z;
		public Coordinates(float[] xi, float[] yi, float[] zi)
		{
			this.m_x = xi;
			this.m_y = yi;
			this.m_z = zi;
		}

		public Coordinates(Coord3d[] coords)
		{
			int nbCoords = coords.Length;
			m_x = new float[nbCoords];
			m_y = new float[nbCoords];
			m_z = new float[nbCoords];
			for (int iCoord = 0; iCoord <= nbCoords - 1; iCoord++) {
				m_x[iCoord] = (float)coords[iCoord].x;
                m_y[iCoord] = (float)coords[iCoord].y;
                m_z[iCoord] = (float)coords[iCoord].z;
			}
		}

		public Coordinates(List<Coord3d> coords)
		{
			int nbCoords = coords.Count;
			m_x = new float[nbCoords];
			m_y = new float[nbCoords];
			m_z = new float[nbCoords];
			for (int iCoord = 0; iCoord <= nbCoords - 1; iCoord++) {
                m_x[iCoord] = (float)coords[iCoord].x;
                m_y[iCoord] = (float)coords[iCoord].y;
                m_z[iCoord] = (float)coords[iCoord].z;
			}
		}

		public float[] x {
			get { return this.m_x; }
		}

		public float[] y {
			get { return this.m_y; }
		}

		public float[] z {
			get { return this.m_z; }
		}

		public Coord3d[] toArray()
		{
			Coord3d[] array = new Coord3d[m_x.Length];
			for (int iCoord = 0; iCoord <= m_x.Length - 1; iCoord++) {
				array[iCoord] = new Coord3d(m_x[iCoord], m_y[iCoord], m_z[iCoord]);
			}
			return array;
		}

		public override string ToString()
		{
			string txt = "";
            for (int iCoord = 0; iCoord <= m_x.Length - 1; iCoord++)
            {
				if (iCoord > 0) {
					txt += "\r\n";
				}
				txt += "[" + iCoord + "]" + m_x[iCoord] + "|" + m_y[iCoord] + "|" + m_z[iCoord];
			}
			return txt;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
