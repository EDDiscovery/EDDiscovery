
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths.Algorithms
{

	public class OutlierRemover
	{

		public static int[] getOutlierIndices(double[] values, int nVariance)
		{
			throw new NotImplementedException();
		}

		public static int[] getInlierIndices(double[] values, int nVariance)
		{
			throw new NotImplementedException();
		}

		public static double[] getOutlierValues(double[] values, int nVariance)
		{
			Scale bounds = getInlierBounds(values, nVariance);
			return System.Array.FindAll<double>(values, x => !bounds.Contains(x));
		}

		public static double[] getInlierValues(double[] values, int nVariance)
		{
			Scale bounds = getInlierBounds(values, nVariance);
			return System.Array.FindAll<double>(values, x => bounds.Contains(x));
		}

		public static Scale getInlierBounds(double[] values, int nVariance)
		{
			if (values.Length == 0) {
				return new Scale(double.NaN, double.NaN);
			}
			double[] dists = new double[values.Length];
			double med = Statistics.Median(values, true);
			double mad = 0;
			for (int i = 0; i <= values.Length - 1; i++) {
                dists[i] = Math.Abs(values[i] - med);
			}
			mad = Statistics.Median(dists, true);
			double upperBound = med + mad * nVariance;
			double lowerBound = med - mad * nVariance;
			return new Scale(lowerBound, upperBound);
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
