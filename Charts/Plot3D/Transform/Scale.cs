
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using nzy3D.Maths;

namespace nzy3D.Plot3D.Transform
{

	public class Scale : ITransformer
	{


		private Coord3d _scale;
		public Scale(Coord3d scale)
		{
			_scale = scale;
		}

		public Maths.Coord3d Compute(Maths.Coord3d input)
		{
			return input.multiply(_scale);
		}

		public void Execute()
		{
			GL.Scale(_scale.x, _scale.y, _scale.z);
		}

		public override string ToString()
		{
			return "(Scale)" + _scale.ToString();
		}
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
