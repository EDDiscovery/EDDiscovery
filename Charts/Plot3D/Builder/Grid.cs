
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;

namespace nzy3D.Plot3D.Builder
{

	public abstract class Grid
	{

		protected internal Range xrange;
		protected internal Range yrange;
		protected internal int xsteps;

		protected internal int ysteps;
		public Grid(Range xrange, int xsteps, Range yrange, int ysteps)
		{
			this.xrange = xrange;
			this.yrange = yrange;
			this.xsteps = xsteps;
			this.ysteps = ysteps;
		}

		public Grid(Range xyrange, int xysteps) : this(xyrange, xysteps, xyrange, xysteps)
		{
		}

		public abstract List<Coord3d> Apply(Mapper mapper);

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
