
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths.Algorithms
{

	public class ScaleFinder
	{

		/// <summary>
		/// Apply an outlier remover on input data (<see cref="OutlierRemover.getInlierValues" />)
		/// and retrieve the min and max values of the non-rejected values.
		/// </summary>
		public static Scale getFilteredScale(double[] values, int nvariance)
		{
			return getMinMaxScale(OutlierRemover.getInlierValues(values, nvariance));
		}

		/// <summary>
		/// Simply returns the min and max values of the input array into
		/// a Scale object.
		/// </summary>
		public static Scale getMinMaxScale(double[] values)
		{
			if (values.Length == 0) {
				return new Scale(double.NaN, double.NaN);
			}
			return new Scale(Statistics.Min(values), Statistics.Max(values));
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
