
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Colors.ColorMaps
{
	/// <summary>
	/// Creates a new instance of ColorMapRedAndGreen. 
	///  A ColorMapRedAndGreen objects provides a color for points standing
	///  between a Zmin and Zmax values.
	/// 
	/// The points standing outside these [Zmin;Zmax] boundaries are assigned
	///  to the same color than the points standing on the boundaries.
	/// 
	/// The red-green colormap is a progressive transition from red to green.
	/// </summary>
	/// <remarks></remarks>
	public class ColorMapRedAndGreen : IColorMap
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
			double b = 0;
			double v = colorComponentRelative(rel_value, 0.375, 0.25, 0.75);
			double r = colorComponentRelative(rel_value, 0.625, 0.25, 0.75);
			return new Color(r, v, b);
		}

        private double colorComponentRelative(double value, double center, double topWidth, double bottomWidth)
		{
			return colorComponentAbsolute(value, center - (bottomWidth / 2), center + (bottomWidth / 2), center - (topWidth / 2), center + (topWidth / 2));
		}

        private double colorComponentAbsolute(double value, double bLeft, double bRight, double tLeft, double tRight)
		{
			if (value < bLeft | value > bRight) {
				// a gauche ou a droite du creneau
				return 0;
			} else if (value > tLeft | value < tRight) {
				// sur le plateau haut
				return 1;
			} else if (value >= bLeft & value <= tLeft) {
				// sur la pente gauche du creneau
				return (value - bLeft) / (tLeft - bLeft);
			} else if (value >= tRight & value <= bRight) {
				// sur la pente droite du creneau
				return (value - bRight) / (tRight - bRight);
			} else {
				throw new Exception("ColorMap did not achieve to compute current color.");
			}
		}

		/// <summary>
		/// Returns the string representation of this colormap
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		public override string ToString()
		{
			return "ColorMapRedAndGreen";
		}

	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
