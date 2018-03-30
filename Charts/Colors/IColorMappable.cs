
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Colors
{
	public interface IColorMappable
	{

		/// <summary>
		/// Get/Set the lower value boundary for a <see cref="colors.ColorMaps.IColorMap"/>.
		/// </summary>
		/// <remarks></remarks>

		double ZMin { get; set; }
		/// <summary>
		/// Get/Set the upper value boundary for a <see cref="colors.ColorMaps.IColorMap"/>.
		/// </summary>
		/// <remarks></remarks>

		double ZMax { get; set; }
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
