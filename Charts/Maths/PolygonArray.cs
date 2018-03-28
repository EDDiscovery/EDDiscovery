
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{

	public class PolygonArray
	{
		internal double[] _x;
		internal double[] _y;

		internal double[] _z;
		public PolygonArray(double[] x, double[] y, double[] z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public int Length {
			get { return _x.Length; }
		}

		public Coord3d Barycentre {
			get { return new Coord3d(Statistics.Mean(_x), Statistics.Mean(_y), Statistics.Mean(_z)); }
		}

		public double[] X {
			get { return _x; }
		}

		public double[] Y {
			get { return _y; }
		}

		public double[] Z {
			get { return _z; }
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
