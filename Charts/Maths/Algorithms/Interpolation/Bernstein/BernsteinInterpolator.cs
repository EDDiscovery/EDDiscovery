
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths.Algorithms.Interpolation.Bernstein
{

	public class BernsteinInterpolator : IInterpolator
	{

		public System.Collections.Generic.List<Coord3d> Interpolate(System.Collections.Generic.List<Coord3d> controlpoints, int resolution)
		{
			Spline3D spline = new Spline3D(controlpoints);
			return spline.ComputeVertices(resolution);
		}
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
