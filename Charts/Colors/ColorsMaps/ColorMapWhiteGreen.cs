
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Colors.ColorMaps
{
	/// <summary>
	/// Creates a new instance of ColorMapWhiteGreen. 
	///  A ColorMapWhiteGreen objects provides a color for points standing
	///  between a Zmin and Zmax values.
	/// 
	/// The points standing outside these [Zmin;Zmax] boundaries are assigned
	///  to the same color than the points standing on the boundaries.
	/// 
	/// The white-red colormap is a progressive transition from white to green.
	/// </summary>
	public class ColorMapWhiteGreen : IColorMap
	{


		private bool m_direction;
		public bool Direction {
			get { return m_direction; }
			set { m_direction = value; }
		}

		public Color GetColor(IColorMappable colorable, double v)
		{
			return GetColor(0, 0, v, colorable.ZMin, colorable.ZMax);
		}

		public Color GetColor(IColorMappable colorable, double x, double y, double z)
		{
			return GetColor(x, y, z, colorable.ZMin, colorable.ZMax);
		}

		/// <summary>
		/// Helper function 
		/// </summary>
		private Color GetColor(double x, double y, double z, double zMin, double zMax)
		{
			double rel_value = 0;
			if (z < zMin) {
				rel_value = 0;
			} else if (z > zMax) {
				rel_value = 1;
			} else {
				if (m_direction) {
					rel_value = (z - zMin) / (zMax - zMin);
				} else {
					rel_value = (zMax - z) / (zMax - zMin);
				}
			}
			return new Color(rel_value, 1, rel_value);
		}

		/// <summary>
		/// Returns the string representation of this colormap
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		public override string ToString()
		{
			return "ColorMapWhiteGreen";
		}

	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
