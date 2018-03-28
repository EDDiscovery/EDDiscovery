
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Plot3D.Primitives.Axes.Layout.Providers
{

	public abstract class AbstractTickProvider : ITickProvider
	{

		public float[] generateTicks(float min, float max)
		{
			return generateTicks(min, max, DefaultSteps);
		}

		public abstract int DefaultSteps { get; }

		public abstract float[] generateTicks(float min, float max, int steps);

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
