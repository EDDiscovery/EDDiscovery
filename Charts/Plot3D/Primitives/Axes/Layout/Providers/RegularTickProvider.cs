
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Plot3D.Primitives.Axes.Layout.Providers
{

	public class RegularTickProvider : AbstractTickProvider, ITickProvider
	{


		internal int _steps;
		public RegularTickProvider() : this(3)
		{
		}

		public RegularTickProvider(int steps)
		{
			_steps = steps;
		}

		public override int DefaultSteps {
			get { return _steps; }
		}

		public override float[] generateTicks(float min, float max, int steps)
		{
			float[] ticks = new float[steps];
			float lstep = (max - min) / (steps - 1);
			ticks[0] = min;
			ticks[steps - 1] = max;
			for (int t = 1; t <= steps - 2; t++) {
				ticks[t] = min + t * lstep;
			}
			return ticks;
		}
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
