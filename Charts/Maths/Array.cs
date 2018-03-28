
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{

	public class Array
	{

		public static int[] Clone(int[] input)
		{
			return Array.Clone(input, input.Length);
		}

		public static float[] Clone(float[] input)
		{
			return Array.Clone(input, input.Length);
		}

		public static double[] Clone(double[] input)
		{
			return Array.Clone(input, input.Length);
		}

		public static int[] Clone(int[] input, int length)
		{
			int lim = Math.Min(input.Length, length);
			int[] output = new int[lim];
			System.Array.Copy(input, output, length);
			return output;
		}

		public static float[] Clone(float[] input, int length)
		{
			int lim = Math.Min(input.Length, length);
			float[] output = new float[lim];
			System.Array.Copy(input, output, length);
			return output;
		}

		public static double[] Clone(double[] input, int length)
		{
			int lim = Math.Min(input.Length, length);
			double[] output = new double[lim];
			System.Array.Copy(input, output, length);
			return output;
		}

		public static int[] Append(int[] input, int value)
		{
			int[] output = new int[input.Length + 1];
			System.Array.Copy(input, output, input.Length);
			output[output.Length - 1] = value;
			return output;
		}

		/// <summary>
		/// output(i, 0) = input(i)
		/// </summary>
		public static double[,] toColumnMatrix(double[] input)
		{
			double[,] output = new double[input.Length, 1];
			for (int i = 0; i < input.Length; i++) {
				output[i, 0] = input[i];
			}
			return output;
		}

		/// <summary>
		/// output(i, 0) = input(i)
		/// </summary>
		public static float[,] toColumnMatrix(float[] input)
		{
			float[,] output = new float[input.Length, 1];
			for (int i = 0; i < input.Length; i++) {
				output[i, 0] = input[i];
			}
			return output;
		}

		/// <summary>
		/// output(i, 0) = input(i)
		/// </summary>
		public static double[,] toColumnMatrixAsDouble(float[] input)
		{
			double[,] output = new double[input.Length, 1];
			for (int i = 0; i <= input.Length - 1; i++) {
				output[i, 0] = input[i];
			}
			return output;
		}

		/// <summary>
		/// output(i, 0) = input(i)
		/// </summary>
		public static bool Find(double[] values, double value)
		{
			return (System.Array.Find(values, x => (x == value)) >= 0);
		}

		/// <summary>
		/// output(i, 0) = input(i)
		/// </summary>
		public static bool Find(int[] values, double value)
		{
			return (System.Array.Find(values, x => (x == value)) >= 0);
		}

		public static double[] Merge(double[] array1, double[] array2)
		{
			double[] merged = new double[array1.Length + array2.Length];
			System.Array.ConstrainedCopy(array1, 0, merged, 0, array1.Length);
			System.Array.ConstrainedCopy(array2, 0, merged, array1.Length, array2.Length);
			return merged;
		}

		public static double[] Flatten(double[,] matrix)
		{
			return Array.Flatten(matrix, false);
		}

		public static double[] Flatten(double[,] matrix, bool ignoreNaN)
		{
			if (matrix.Length == 0) {
				return new double[0];
			}
			double[] vector = new double[matrix.Length];
			int k = 0;
			for (int i = 0; i <= matrix.GetLength(0) - 1; i++) {
				for (int j = 0; j <= matrix.GetLength(1) - 1; j++) {
					if (!(ignoreNaN && double.IsNaN(matrix[i, j]))) {
						vector[k] = matrix[i, j];
						k += 1;
					}
				}
			}
			// reduce length of output to forbid elements not initialized due to NaN
			return Array.Clone(vector, k);
		}

		public static float[] Flatten(float[,] matrix)
		{
			return Array.Flatten(matrix, false);
		}

		public static float[] Flatten(float[,] matrix, bool ignoreNaN)
		{
			if (matrix.Length == 0) {
				return new float[0];
			}
			float[] vector = new float[matrix.Length];
			int k = 0;
			for (int i = 0; i <= matrix.GetLength(0) - 1; i++) {
				for (int j = 0; j <= matrix.GetLength(1) - 1; j++) {
					if (!(ignoreNaN && double.IsNaN(matrix[i, j]))) {
						vector[k] = matrix[i, j];
						k += 1;
					}
				}
			}
			// reduce length of output to forbid elements not initialized due to NaN
			return Array.Clone(vector, k);
		}


        public static double[] FilterNaNs(double[] input)
		{
			if (input.Length == 0) {
				return new double[0];
			}
			double[] vector = new double[input.Length];
			int k = 0;
			for (int i = 0; i <= input.GetLength(0) - 1; i++) {
				if (!(double.IsNaN(input[i]))) {
					vector[k] = input[i];
					k += 1;
				}
			}
			// reduce length of output to forbid elements not initialized due to NaN
			return Array.Clone(vector, k);
		}

		public static int CountNaNs(double[] input)
		{
			int k = 0;
			for (int i = 0; i <= input.GetLength(0) - 1; i++) {
				if (double.IsNaN(input[i])) {
					k += 1;
				}
			}
			return k;
		}

		public static bool atLeastOneNonNan(double[] input)
		{
			for (int i = 0; i <= input.GetLength(0) - 1; i++) {
				if (!double.IsNaN(input[i])) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Sort input array, and return the final order of initial values.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		/// <remarks>Input array is modified and sorted after call to this method.</remarks>
		public static int[] sortAscending(int[] input)
		{
			int[] ordered = new int[input.Length];
			for (int i = 0; i <= ordered.Length - 1; i++) {
				ordered[i] = i;
			}
			System.Array.Sort(input, ordered);
			return ordered;
		}

		/// <summary>
		/// Sort input array, and return the final order of initial values.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		/// <remarks>Input array is modified and sorted after call to this method.</remarks>
		public static int[] sortAscending(float[] input)
		{
			int[] ordered = new int[input.Length];
			for (int i = 0; i <= ordered.Length - 1; i++) {
                ordered[i] = i;
			}
			System.Array.Sort(input, ordered);
			return ordered;
		}

		/// <summary>
		/// Sort input array, and return the final order of initial values.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		/// <remarks>Input array is modified and sorted after call to this method.</remarks>
		public static int[] sortAscending(double[] input)
		{
			int[] ordered = new int[input.Length];
			for (int i = 0; i <= ordered.Length - 1; i++) {
                ordered[i] = i;
			}
			System.Array.Sort(input, ordered);
			return ordered;
		}

		/// <summary>
		/// Sort input array, and return the final order of initial values.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		/// <remarks>Input array is modified and sorted after call to this method.</remarks>
		public static int[] sortAscending(System.DateTime[] input)
		{
			int[] ordered = new int[input.Length];
			for (int i = 0; i <= ordered.Length - 1; i++) {
                ordered[i] = i;
			}
			System.Array.Sort(input, ordered);
			return ordered;
		}

		/// <summary>
		/// Sort input array in descending order, and return the final order of initial values.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		/// <remarks>Input array is modified and sorted after call to this method.</remarks>
		public static int[] sortDescending(int[] input)
		{
			int[] ordered = sortAscending(input);
			System.Array.Reverse(ordered);
			System.Array.Reverse(input);
			return ordered;
		}

		/// <summary>
		/// Sort input array in descending order, and return the final order of initial values.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		/// <remarks>Input array is modified and sorted after call to this method.</remarks>
		public static int[] sortDescending(float[] input)
		{
			int[] ordered = sortAscending(input);
			System.Array.Reverse(ordered);
			System.Array.Reverse(input);
			return ordered;
		}

		/// <summary>
		/// Sort input array in descending order, and return the final order of initial values.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		/// <remarks>Input array is modified and sorted after call to this method.</remarks>
		public static int[] sortDescending(double[] input)
		{
			int[] ordered = sortAscending(input);
			System.Array.Reverse(ordered);
			System.Array.Reverse(input);
			return ordered;
		}

		/// <summary>
		/// Sort input array in descending order, and return the final order of initial values.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		/// <remarks>Input array is modified and sorted after call to this method.</remarks>
		public static int[] sortDescending(System.DateTime[] input)
		{
			int[] ordered = sortAscending(input);
			System.Array.Reverse(ordered);
			System.Array.Reverse(input);
			return ordered;
		}

		public static void WriteConsole(Coord3d[] input)
		{
			for (int i = 0; i <= input.Length - 1; i++) {
                Console.WriteLine(input[i]);
			}
			Console.WriteLine("");
		}

		public static void WriteConsole(double[] input)
		{
			for (int i = 0; i <= input.Length - 1; i++) {
                Console.WriteLine(input[i]);
			}
			Console.WriteLine("");
		}

		public static void WriteConsole(float[] input)
		{
			for (int i = 0; i <= input.Length - 1; i++) {
                Console.WriteLine(input[i]);
			}
			Console.WriteLine("");
		}

		public static void WriteConsole(int[] input)
		{
			for (int i = 0; i <= input.Length - 1; i++) {
				Console.WriteLine(input[i]);
			}
			Console.WriteLine("");
		}

		public static void WriteConsole(char[] input)
		{
			for (int i = 0; i <= input.Length - 1; i++) {
				Console.WriteLine(input[i]);
			}
			Console.WriteLine("");
		}

		public static void WriteConsole(double[,] input)
		{
			for (int i = 0; i <= input.GetLength(0) - 1; i++) {
				for (int j = 0; j <= input.GetLength(1) - 1; j++) {
					Console.Write(input[i, j] + "\t");
				}
				Console.WriteLine("");
			}
			Console.WriteLine("");
		}

		public static void WriteConsole(float[,] input)
		{
			for (int i = 0; i <= input.GetLength(0) - 1; i++) {
				for (int j = 0; j <= input.GetLength(1) - 1; j++) {
                    Console.Write(input[i, j] + "\t");
				}
				Console.WriteLine("");
			}
			Console.WriteLine("");
		}

		public static void WriteConsole(int[,] input)
		{
			for (int i = 0; i <= input.GetLength(0) - 1; i++) {
				for (int j = 0; j <= input.GetLength(1) - 1; j++) {
					Console.Write(input[i, j] + "\t");
				}
				Console.WriteLine("");
			}
			Console.WriteLine("");
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
