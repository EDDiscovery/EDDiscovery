
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Colors
{
	/// <summary>
	/// <see cref=" ISingleColorable"/> objects have a single plain color and a must define a setter for it
	/// </summary>
	/// <remarks></remarks>
	public interface ISingleColorable
	{

		/// <summary>
		/// Get/Set the color
		/// </summary>
		/// <remarks></remarks>

		Color Color { get; set; }
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
