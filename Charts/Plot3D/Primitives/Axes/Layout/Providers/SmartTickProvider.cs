
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Plot3D.Primitives.Axes.Layout.Providers
{

	public class SmartTickProvider : AbstractTickProvider, ITickProvider
	{


		internal int _steps;
		public SmartTickProvider() : this(5)
		{
		}

		public SmartTickProvider(int steps)
		{
			_steps = steps;
		}

		public override int DefaultSteps {
			get { return _steps; }
		}

		/// <summary>
		/// Compute the ticks placements automatically between values min and max.
		/// </summary>
		public override float[] generateTicks(float min, float max, int steps)
		{
			if ((min == max)) {
				float[] ticks = new float[1];
				ticks[0] = min;
				return ticks;
			} else if ((min > max)) {
				return null;
			} else {
				double absscale = Math.Floor(Math.Log10(max - min));
				double relscale = Math.Log10(max - min) - absscale;
				float ticksize = 0;
				if ((relscale < Math.Log10(0.2 * steps))) {
					ticksize = Convert.ToSingle(Math.Pow(10, absscale) * 0.2);
				} else if ((relscale < Math.Log10(0.5 * steps))) {
					ticksize = Convert.ToSingle(Math.Pow(10, absscale) * 0.5);
				} else if ((relscale < Math.Log10(1 * steps))) {
					ticksize = Convert.ToSingle(Math.Pow(10, absscale) * 1);
				} else {
					ticksize = Convert.ToSingle(Math.Pow(10, absscale) * 2);
				}
				int starti = Convert.ToInt32(Math.Ceiling(min / ticksize));
				int stopi = Convert.ToInt32(Math.Floor(max / ticksize));
				float[] ticks = new float[stopi - starti + 1];
				for (int t = starti; t <= stopi; t++) {
					ticks[t - starti] = (t * ticksize);
				}
				return ticks;
			}
		}
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
