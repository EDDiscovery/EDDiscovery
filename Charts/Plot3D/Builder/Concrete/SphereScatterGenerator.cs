
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;

namespace nzy3D.Plot3D.Builder.Concrete
{

	public class SphereScatterGenerator
	{

		public static object Generate(Coord3d center, double radius, int steps, bool half)
		{
			List<Coord3d> coords = new List<Coord3d>();
			double inc = Math.PI / steps;
			double i = 0;
			int jrat = (half ? 1 : 2);
			while (i < (2 * Math.PI)) {
				double j = 0;
				while (j < (jrat * Math.PI)) {
					Coord3d c = (new Coord3d(i, j, radius)).cartesian();
					if ((center != null)) {
						c.x += center.x;
						c.y += center.y;
						c.z += center.z;
					}
					coords.Add(c);
					j += inc;
				}
				i += inc;
			}
			return coords;
		}

		public static object Generate(Coord3d center, double radius, int steps)
		{
			return Generate(center, radius, steps, false);
		}

		public static object Generate(double radius, int steps)
		{
			return Generate(null, radius, steps, false);
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
