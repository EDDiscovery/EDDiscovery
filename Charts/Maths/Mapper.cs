
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{

	/// <summary>
	/// A Mapper provides an abstract definition of a function that allows
	/// getting a Z value according to a pair of (X,Y) coordinates.
	/// It moreover provide the service of gathering input and computed output
	/// into a  <see cref="Coordinates"/> object, that provides arrays of
	/// X,Y, and Z coordinates as float values.
	/// <see cref="Mapper"/> is deprecated. One should use <see cref="plot3D.builder.Mapper"/> instead
	/// </summary>
	/// <remarks></remarks>
	public abstract class Mapper
	{

		/// <summary>
		/// Return an array of Z values according to the implemented function
		/// that provides an output according to an array of (X,Y) coordinates
		/// </summary>
		/// <param name="xy">Input array of (X,Y) coordinates. First dimension can be any length (equal to the number of coordinates). Second dimension must be of length 2.</param>
		/// <returns>Array of Z values for each (X,Y) coordinate</returns>
		public abstract double[] getZ(double[,] xy);

		/// <summary>
		/// Return a Z value according to the implemented function
		/// that provides an output according to an (X,Y) coordinate
		/// </summary>
		/// <param name="xy">One (X,Y) coordinate</param>
		/// <returns>Z value for (X,Y) coordinate</returns>
		public abstract double getZ(double[] xy);

		/// <summary>
		/// Return a structure containing X, Y, and Z coordinates as arrays of double.
		/// </summary>
		/// <param name="xy">Input array of (X,Y) coordinates as array of double. First dimension can be any length (equal to the number of coordinates). Second dimension must be of length 2.</param>
		public Coordinates getCoordinates(double[,] xy)
		{
			if (xy.GetLength(1) != 2) {
				throw new ArgumentException("Input array must have a length of 2 in second dimension", "xy");
			}
			double[] zd = getZ(xy);
			int nbCoordinates = xy.GetLength(0);
			float[] x = new float[nbCoordinates];
			float[] y = new float[nbCoordinates];
			float[] z = new float[nbCoordinates];
			for (int m = 0; m <= nbCoordinates - 1; m++) {
				x[m] = (float)xy[m, 0];
                y[m] = (float)xy[m, 1];
                z[m] = (float)zd[m];
			}
			return new Coordinates(x, y, z);
		}

		/// <summary>
		/// Return a structure containing X, Y, and Z coordinates as arrays of double.
		/// </summary>
		/// <param name="xy">Input array of (X,Y) coordinates as array of single. First dimension can be any length (equal to the number of coordinates). Second dimension must be of length 2.</param>
		public Coordinates getCoordinates(float[,] xy)
		{
			if (xy.GetLength(1) != 2) {
				throw new ArgumentException("Input array must have a length of 2 in second dimension", "xy");
			}
			double[,] xyDouble = new double[xy.GetLength(0), xy.GetLength(1)];
			for (int p = 0; p <= xy.GetLength(0) - 1; p++) {
                for (int d = 0; d <= xy.GetLength(1) - 1; d++)
                {
					xyDouble[p, d] = xy[p, d];
				}
			}
			return getCoordinates(xyDouble);
		}

		/// <summary>
		///  Return a structure containing X, Y, and Z coordinates as arrays of double.
		/// </summary>
		/// <param name="xy">A single coordinate point. Dimension must be equal to 2.</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public Coordinates getCoordinates(double[] xy)
		{
			if (xy.GetLength(0) != 2) {
				throw new ArgumentException("Input array must have a length of 2", "xy");
			}
			int nbCoordinates = xy.GetLength(0);
			float[] x = new float[1];
			float[] y = new float[1];
			float[] z = new float[1];
			x[0] = (float)xy[0];
            y[0] = (float)xy[1];
            z[0] = (float)getZ(xy);
			return new Coordinates(x, y, z);
		}

		/// <summary>
		///  Return a structure containing X, Y, and Z coordinates as arrays of double.
		/// </summary>
		/// <param name="xy">A single coordinate point. Dimension must be equal to 2.</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public Coordinates getCoordinates(float[] xy)
		{
			if (xy.GetLength(0) != 2) {
				throw new ArgumentException("Input array must have a length of 2", "xy");
			}
			double[] xyd = new double[2];
			xyd[0] = xy[0];
			xyd[1] = xy[1];
			return getCoordinates(xyd);
		}


	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
