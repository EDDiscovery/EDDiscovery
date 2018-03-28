
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;

namespace nzy3D.Plot3D.Builder.Concrete
{
	public class OrthonormalGrid : Grid
	{

		public OrthonormalGrid(Range xrange, int xsteps, Range yrange, int ysteps) : base(xrange, xsteps, yrange, ysteps)
		{
		}

		public OrthonormalGrid(Range xyrange, int xysteps) : base(xyrange, xysteps)
		{
		}

		public override System.Collections.Generic.List<Maths.Coord3d> Apply(Mapper mapper)
		{
			double xstep = xrange.Range / (xsteps - 1);
			double ystep = yrange.Range / (ysteps - 1);
			List<Coord3d> output = new List<Coord3d>();
			for (int xi = 0; xi <= xsteps - 1; xi++) {
				for (int yi = 0; yi <= ysteps - 1; yi++) {
					double x = 0;
					double y = 0;
					x = xrange.Min + xi * xstep;
					y = yrange.Min + yi * ystep;
					output.Add(new Coord3d(x, y, mapper.f(x, y)));
				}
			}
			return output;
		}

	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
