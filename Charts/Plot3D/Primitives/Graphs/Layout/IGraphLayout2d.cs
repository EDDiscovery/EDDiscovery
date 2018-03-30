
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;

namespace nzy3D.Plot3D.Primitives.Graphs.Layout
{

	public interface IGraphLayout2d<V>
	{

		Coord2d VertexPosition { get; set; }
		Coord2d getV(V v);
		List<Coord2d> values();

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
