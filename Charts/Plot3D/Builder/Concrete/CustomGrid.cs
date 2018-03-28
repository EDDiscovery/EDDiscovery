
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;

namespace nzy3D.Plot3D.Builder.Concrete
{
	public class CustomGrid : Grid
	{


		internal double[,] coordinates;
		public CustomGrid(double[,] coordinates) : base(null, 0)
		{
			if (coordinates.GetLength(1) != 2) {
				throw new ArgumentException("Input coordinates array must be have a length of 2 in second dimension. Current array second dimension has a lenght of " + coordinates.GetLength(1), "coordinates");
			}
			this.coordinates = coordinates;
		}

		public override System.Collections.Generic.List<Maths.Coord3d> Apply(Mapper mapper)
		{
			List<Coord3d> output = new List<Coord3d>();
			for (int i = 0; i <= coordinates.Length - 1; i++) {
				output.Add(new Coord3d(coordinates[i, 0], coordinates[i, 1], mapper.f(coordinates[i, 0], coordinates[i, 1])));
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
