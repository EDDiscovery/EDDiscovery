
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Colors.ColorMaps
{
	/// <summary>
	/// This interface defines the set of methods that any concrete colormap
	///  should define in order to be used by an object implementing the
	///  ColorMappable interface.
	/// 
	/// The ColorMappable interface impose to an object to provide a Z-scaling,
	///  that is, a minimum and maximum value on the Z axis.
	///  These values are used by concrete colormaps in order to set an interval
	///  for the possible colors.
	/// </summary>
	/// <remarks></remarks>
	public interface IColorMap
	{


		/// <summary>
		/// Returns color of a <paramref name="colorable"/> object at given point
		/// </summary>
		/// <param name="colorable">A <see cref="IColorMappable"/> object.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns>Color for the given point</returns>
		Color GetColor(IColorMappable colorable, double x, double y, double z);

		/// <summary>
		/// Returns color of a <paramref name="colorable"/> object at given point
		/// </summary>
		/// <param name="colorable">A <see cref="IColorMappable"/> object.</param>
		/// <param name="v">The variable that is Color-dependent, and can be independent of the coordinates</param>
		/// <returns></returns>
		/// <remarks></remarks>
		Color GetColor(IColorMappable colorable, double v);

		/// <summary>
		/// Indicates if the colormap use the standard (True) or reverted (False) color direction
		/// </summary>
		/// <remarks></remarks>

		bool Direction { get; set; }
		/// <summary>
		/// Returns the string representation of this color, including alpha channel value
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		string ToString();

	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
