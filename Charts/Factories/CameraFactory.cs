
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;
using nzy3D.Plot3D.Rendering.View;

namespace nzy3D.Factories
{

	public class CameraFactory
	{

		public static Camera getInstance(Coord3d center)
		{
			return new Camera(center);
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
