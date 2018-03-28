
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Plot3D.Rendering.View.Modes
{

	/// <summary>
	/// Allows to apply a restriction on the degree of freedom that is
	/// let on the View control.
	/// </summary>
	public enum ViewPositionMode
	{

		/// <summary>
		/// Enforce view point on top of the scene.
		/// </summary>
		TOP,

		/// <summary>
		/// Enforce view point on profile of the scene.
		/// </summary>
		PROFILE,

		/// <summary>
		/// No enforcement of view point: let the user freely turn around the scene.
		/// </summary>
		FREE

	}

}


//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
