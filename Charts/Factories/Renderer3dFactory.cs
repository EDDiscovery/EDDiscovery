
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Plot3D.Rendering.View;

namespace nzy3D.Factories
{

	public class Renderer3dFactory
	{

		public static object getInstance(View view, bool traceGL, bool debugGL)
		{
			return new Renderer3d(view, traceGL, debugGL);
		}

	}

}


//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
