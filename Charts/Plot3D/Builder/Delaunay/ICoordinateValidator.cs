
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Plot3D.Builder.Delaunay
{

	public interface ICoordinateValidator
	{
		float[] getX();
		float[] getY();
		float[,] get_Z_as_fxy();
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
