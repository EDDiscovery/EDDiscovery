
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{

	public class Range : Scale
	{

		public Range(double min, double max) : base(min, max)
		{
		}

		public new void Enlarge(double ratio)
		{
			double offset = (Max - Min) * ratio;
			if (offset == 0) {
				offset = 1;
			}
			Min -= offset;
			Max += offset;
		}

		public new Range CreateEnlarge(double ratio)
		{
			double offset = (Max - Min) * ratio;
			if (offset == 0) {
				offset = 1;
			}
			return new Range(Min - offset, Max + offset);
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
