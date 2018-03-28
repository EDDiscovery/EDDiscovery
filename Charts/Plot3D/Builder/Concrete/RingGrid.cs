
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;

namespace nzy3D.Plot3D.Builder.Concrete
{

	public class RingGrid : Grid
	{


		internal double sqradius;
		public RingGrid(double radius, int xysteps, int enlargeSteps) : base(new Range(-radius - (enlargeSteps * radius / xysteps), radius + (enlargeSteps * radius / xysteps)), xysteps)
		{
			sqradius = (radius + (enlargeSteps * radius / xysteps)) * (radius + (enlargeSteps * radius / xysteps));
		}

		public RingGrid(double radius, int xysteps) : this(radius, xysteps, 0)
		{
		}

		public override System.Collections.Generic.List<Maths.Coord3d> Apply(Mapper mapper)
		{
			double xstep = xrange.Range / xsteps;
			double ystep = yrange.Range / ysteps;
			List<Coord3d> output = new List<Coord3d>();
			for (int xi = -(xsteps - 1) / 2; xi <= (xsteps - 1) / 2; xi++) {
				for (int yi = -(ysteps - 1) / 2; yi <= (ysteps - 1) / 2; yi++) {
					double x = 0;
					double y = 0;
					x = xi * xstep;
					y = yi * ystep;
					if (sqradius > x * x + y * y) {
						output.Add(new Coord3d(x, y, mapper.f(x, y)));
					}
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
