
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;
using nzy3D.Plot3D.Primitives;

namespace nzy3D.Plot3D.Builder
{

	public abstract class Tessellator
	{


		public Tessellator()
		{
		}

		public AbstractComposite build(List<Coord3d> coordinates)
		{
			Coordinates coords = new Coordinates(coordinates);
			return build(coords.x, coords.y, coords.z);
		}

		public abstract AbstractComposite build(float[] x, float[] y, float[] z);

		public AbstractComposite build(double[] x, double[] y, double[] z)
		{
			float[] xs = new float[x.Length];
			System.Array.Copy(x, xs, x.Length);
			float[] ys = new float[y.Length];
			System.Array.Copy(y, ys, y.Length);
			float[] zs = new float[z.Length];
			System.Array.Copy(z, zs, z.Length);
			return build(xs, ys, zs);
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
