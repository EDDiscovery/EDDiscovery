
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{

	public class Statistics
	{

		public static double Sum(double[] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			double vsum = 0;
			for (int i = 0; i <= values.Length - 1; i++) {
				if (!double.IsNaN(values[i])) {
					vsum += values[i];
				}
			}
			return vsum;
		}

		public static float Sum(float[] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			float vsum = 0;
            for (int i = 0; i <= values.Length - 1; i++)
            {
				if (!float.IsNaN(values[i])) {
					vsum += values[i];
				}
			}
			return vsum;
		}

		public static double Mean(double[] values)
		{
			return Sum(values) / values.Length;
		}

		public static float Mean(float[] values)
		{
			return Sum(values) / values.Length;
		}

		public static double Min(double[] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			double vmin = double.PositiveInfinity;
            for (int i = 0; i <= values.Length - 1; i++)
            {
				if (!double.IsNaN(values[i])) {
					if (values[i] < vmin) {
						vmin = values[i];
					}
				}
			}
			return vmin;
		}

		public static double Min(double[,] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			double vmin = double.PositiveInfinity;
            for (int i = 0; i <= values.GetLength(0) - 1; i++)
            {
                for (int j = 0; j <= values.GetLength(1) - 1; j++)
                {
					if (!double.IsNaN(values[i, j])) {
						if (values[i, j] < vmin) {
							vmin = values[i, j];
						}
					}
				}
			}
			return vmin;
		}

		public static float Min(float[] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			float vmin = float.PositiveInfinity;
            for (int i = 0; i <= values.Length - 1; i++)
            {
				if (!float.IsNaN(values[i])) {
					if (values[i] < vmin) {
						vmin = values[i];
					}
				}
			}
			return vmin;
		}

		public static float Min(float[,] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			float vmin = float.PositiveInfinity;
            for (int i = 0; i <= values.GetLength(0) - 1; i++)
            {
                for (int j = 0; j <= values.GetLength(1) - 1; j++)
                {
					if (!float.IsNaN(values[i, j])) {
						if (values[i, j] < vmin) {
							vmin = values[i, j];
						}
					}
				}
			}
			return vmin;
		}

		public static int Min(int[,] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			int vmin = int.MaxValue;
            for (int i = 0; i <= values.GetLength(0) - 1; i++)
            {
                for (int j = 0; j <= values.GetLength(1) - 1; j++)
                {
					if (values[i, j] < vmin) {
						vmin = values[i, j];
					}
				}
			}
			return vmin;
		}

		/// <summary>
		/// Returns the index (zero-based) where the minimal value stands
		/// </summary>
		public static int MinId(double[] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			double vmin = double.PositiveInfinity;
			int index = -1;
            for (int i = 0; i <= values.Length - 1; i++)
            {
				if (!double.IsNaN(values[i])) {
					if (values[i] < vmin) {
						vmin = values[i];
						index = i;
					}
				}
			}
			return index;
		}

		/// <summary>
		/// Returns the index (zero-based) where the minimal value stands
		/// </summary>
		public static int MinId(float[] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			float vmin = float.PositiveInfinity;
			int index = -1;
            for (int i = 0; i <= values.Length - 1; i++)
            {
				if (!float.IsNaN(values[i])) {
					if (values[i] < vmin) {
						vmin = values[i];
						index = i;
					}
				}
			}
			return index;
		}

		/// <summary>
		/// Returns the index (zero-based) where the minimal value stands
		/// </summary>
		public static int MinId(int[] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			int vmin = int.MaxValue;
			int index = -1;
            for (int i = 0; i <= values.Length - 1; i++)
            {
				if (values[i] < vmin) {
					vmin = values[i];
					index = i;
				}
			}
			return index;
		}

		public static double Max(double[] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			double vmin = double.NegativeInfinity;
            for (int i = 0; i <= values.Length - 1; i++)
            {
				if (!double.IsNaN(values[i])) {
					if (values[i] > vmin) {
						vmin = values[i];
					}
				}
			}
			return vmin;
		}

		public static double Max(double[,] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			double vmax = double.PositiveInfinity;
            for (int i = 0; i <= values.GetLength(0) - 1; i++)
            {
                for (int j = 0; j <= values.GetLength(1) - 1; j++)
                {
					if (!double.IsNaN(values[i, j])) {
						if (values[i, j] > vmax) {
							vmax = values[i, j];
						}
					}
				}
			}
			return vmax;
		}

		public static float Max(float[] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			float vmax = float.NegativeInfinity;
            for (int i = 0; i <= values.Length - 1; i++)
            {
				if (!float.IsNaN(values[i])) {
					if (values[i] > vmax) {
						vmax = values[i];
					}
				}
			}
			return vmax;
		}

		public static float Max(float[,] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			float vmax = float.PositiveInfinity;
            for (int i = 0; i <= values.GetLength(0) - 1; i++)
            {
                for (int j = 0; j <= values.GetLength(1) - 1; j++)
                {
					if (!float.IsNaN(values[i, j])) {
						if (values[i, j] > vmax) {
							vmax = values[i, j];
						}
					}
				}
			}
			return vmax;
		}

		public static int Max(int[,] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			int vmax = int.MaxValue;
            for (int i = 0; i <= values.GetLength(0) - 1; i++)
            {
                for (int j = 0; j <= values.GetLength(1) - 1; j++)
                {
					if (values[i, j] > vmax) {
						vmax = values[i, j];
					}
				}
			}
			return vmax;
		}

		/// <summary>
		/// Returns the index (zero-based) where the maximal value stands
		/// </summary>
		public static int MaxId(int[] values)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			int vmax = int.MinValue;
			int index = -1;
            for (int i = 0; i <= values.Length - 1; i++)
            {
				if (values[i] > vmax) {
					vmax = values[i];
					index = i;
				}
			}
			return index;
		}


		/// <summary>
		/// Computes the mad statistic, that is the median of all distances to the median of input
		/// values.
		/// </summary>
		/// <returns>Mad statistics</returns>
		/// <remarks>If the input array is empty, the output value is Double.NaN</remarks>
		public static double Mad(double[] values)
		{
			if (values.Length == 0) {
				return double.NaN;
			}
			double[] dists = new double[values.Length];
			double median = Statistics.Median(values, true);
            for (int i = 0; i <= values.Length - 1; i++)
            {
				dists[i] = Math.Abs(values[i] - median);
			}
			return Statistics.Median(dists, true);
		}


		/// <summary>
		/// Computes the standard deviation of an array of doubles.
		/// </summary>
		/// <returns>Standard deviation</returns>
		/// <remarks>If the input array is empty, the output value is Double.NaN</remarks>
		public static double Std(double[] values)
		{
			if (values.Length == 0) {
				return double.NaN;
			}
			return Math.Sqrt(Statistics.Variance(values));
		}

		/// <summary>
		/// Compute the variance of an array of doubles. This function normalizes the
		/// output by N-1 if N > 1, where N is the sample size.
		/// This is an unbiased estimator of the variance of the population
		/// For N=1, the output is 0.
		/// </summary>
		/// <remarks>If the input array is empty, the output value is Double.NaN</remarks>
		public static double Variance(double[] values)
		{
			if (values.Length == 0) {
				return double.NaN;
			}
			double mean = Statistics.Mean(values);
			double sum = 0;
			int count = 0;
			for (int i = 0; i <= values.Length - 1; i++) {
				if (!double.IsNaN(values[i])) {
					sum += Math.Pow(values[i] - mean, 2);
					count += 1;
				}
			}
			if (count == 0) {
				return double.NaN;
			} else if (count == 1) {
				return 0;
			} else {
				return sum / (count - 1);
			}
		}

		/// <summary>
		/// Computes the quantiles of an array of doubles. This method assumes the array
		/// has at least one element.
		/// </summary>
		/// <param name="values">Input array</param>
		/// <param name="levels">A list of levels that must belong to [0;100]</param>
		/// <param name="interpolated">If True, computes an interpolation of quantile when required quantile is not an exact vector id.
		/// If False, the quantile is not interpolated but nearest value is returned</param>
		/// <returns>The quantiles</returns>
		/// <remarks>Throws an IllegalArgumentException if a level is out of the [0;100] bounds. Return 0 if input array is empty.</remarks>
		public static double[] Quantile(double[] values, double[] levels, bool interpolated)
		{
			if (values.Length == 0) {
				return (double[])values.Clone();
			}
			double[] quantiles = new double[levels.Length];
			double[] sorted = new double[values.Length];
			System.Array.Copy(values, sorted, values.Length);
			System.Array.Sort(sorted);
			double quantileIdx = 0;
			double quantileIdxCeil = 0;
			double quantileIdxFloor = 0;
            for (int i = 0; i <= levels.Length - 1; i++)
            {
				if (levels[i] > 100 | levels[i] < 0) {
					throw new ArgumentException("Input level [" + i + "]=" + levels[i] + "is out of bounds [0;100]", "levels");
				}
				quantileIdx = (sorted.Length - 1) * levels[i] / 100;
				if (quantileIdx == Convert.ToInt32(quantileIdx)) {
					// quantile exactly fond
					quantiles[i] = sorted[Convert.ToInt32(quantileIdx)];
				} else {
					quantileIdxCeil = Math.Ceiling(quantileIdx);
					quantileIdxFloor = Math.Floor(quantileIdx);
					if (interpolated) {
						quantiles[i] = sorted[Convert.ToInt32(quantileIdxFloor)] * (quantileIdxCeil - quantileIdx) + sorted[Convert.ToInt32(quantileIdxCeil)] * (quantileIdx - quantileIdxFloor);
					} else {
						if ((quantileIdx - quantileIdxFloor < quantileIdxCeil - quantileIdx)) {
							quantiles[i] = sorted[Convert.ToInt32(quantileIdxFloor)];
						} else {
							quantiles[i] = sorted[Convert.ToInt32(quantileIdxCeil)];
						}
					}
				}
			}
			return quantiles;
		}

		/// <summary>
		/// Computes the quantiles of an array of doubles. This method assumes the array
		/// has at least one element. Interpolation of quantile is performed when required quantile is not an exact vector id.
		/// </summary>
		/// <param name="values">Input array</param>
		/// <param name="levels">A list of levels that must belong to [0;100]</param>
		/// <returns>The quantiles</returns>
		/// <remarks>This is only a helper function for Statistics.Quantile(values, levels, True). Throws an IllegalArgumentException if a level is out of the [0;100] bounds. Return 0 if input array is empty.</remarks>
		public static double[] Quantile(double[] values, double[] levels)
		{
			return Quantile(values, levels, true);
		}

		/// <summary>
		/// Computes the median value of an array of doubles.
		/// </summary>
		/// <param name="values">Input array</param>
		/// <param name="interpolated">If True, computes an interpolation of median when required, i.e. if median is not an exact vector id.
		/// If False, the median is not interpolated but nearest value is returned (either higher or lower value)</param>
		public static double Median(double[] values, bool interpolated)
		{
			if (values.Length == 0) {
				throw new ArgumentException("Input array must have a length greater than 0", "values");
			}
			double[] med = { 50 };
			double[] @out = Quantile(values, med, interpolated);
			return @out[0];
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
