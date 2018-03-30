
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Colors.ColorMaps
{
	public class ColorMapRBG : IColorMap
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
		/// Return the influence of a color, by comparing the input value to the color spectrum.
		/// 
		///  The design of a colormap implies defining the influence of each base color
		///  (red, green, and blue) over the range of input data.
		///  For this reason, the value given to this function is a number between 0 and 1,
		///  indicating the position of the input value in the colormap.
		///  Any value standing outside of colormap boundaries should have the "maximal" or
		///  "minimal" color.
		/// 
		///  Exemple:
		///  A rainbow colormap is a progressive transition from blue, to green and red.
		///  The mix between these 3 colors, may be expressed by the definition of 3 functions:
		///  <code>
		///        blue     green     red
		///      /-------\/-------\/-------\
		///     /        /\       /\        \
		///    /        /  \     /  \        \
		///   /        /    \   /    \        \
		///  |----------------|----------------|
		///  0               0.5               1
		///  </code>
		/// 
		///  In order to get the color of an input value standing between 0 and 1, the user
		///  should call:
		/// d
		/// <code>
		///  float blue  = (float) creneau_relatif( rel_value, 0.25, 0.25, 0.75 );
		///  float green = (float) creneau_relatif( rel_value, 0.50, 0.25, 0.75 );
		///  float red   = (float) creneau_relatif( rel_value, 0.75, 0.25, 0.75 );
		///  return new Color4f( red, green, blue, 1.0f );
		///  </code>
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
			double b = colorComponentRelative(rel_value, 0.25, 0.25, 0.75);
			double g = colorComponentRelative(rel_value, 0.5, 0.25, 0.75);
			double r = colorComponentRelative(rel_value, 0.75, 0.25, 0.75);
			return new Color(r, g, b);
		}

		private double colorComponentRelative(double value, double center, double topWidth, double bottomWidth)
		{
			return colorComponentAbsolute(value, center - (bottomWidth / 2), center + (bottomWidth / 2), center - (topWidth / 2), center + (topWidth / 2));
		}

        private double colorComponentAbsolute(double value, double bLeft, double bRight, double tLeft, double tRight)
		{
			if (value < bLeft | value >= bRight) {
				// a gauche ou a droite du creneau
				return 0;
			} else if (value >= tLeft & value < tRight) {
				// sur le plateau haut
				return 1;
			} else if (value >= bLeft & value < tLeft) {
				// sur la pente gauche du creneau
				return (value - bLeft) / (tLeft - bLeft);
			} else if (value >= tRight & value < bRight) {
				// sur la pente droite du creneau
				return (value - bRight) / (tRight - bRight);
			} else {
				//Throw New Exception("ColorMap did not achieve to compute current color.")
				return 0;

			}
		}

		/// <summary>
		/// Returns the string representation of this colormap
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		public override string ToString()
		{
			return "ColorMapRGB";
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
