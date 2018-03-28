
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Plot3D.Builder.Delaunay.Jdt;

namespace nzy3D.Plot3D.Builder.Delaunay
{

	public interface ITriangulation
	{
		void insertPoint(Point_dt p);
		IEnumerator<Triangle_dt> trianglesIterator();
		IEnumerator<Point_dt> verticesIterator();
		int trianglesSize();
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
