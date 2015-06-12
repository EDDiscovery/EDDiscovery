// Copyright 2003 Eric Marchesin - <eric.marchesin@laposte.net>
//
// This source file(s) may be redistributed by any means PROVIDING they
// are not sold for profit without the authors expressed written consent,
// and providing that this notice and the authors name and all copyright
// notices remain intact.
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUR OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//-----------------------------------------------------------------------
using System;
using System.Collections;


namespace EMK.LightGeometry
{
	/// <summary>
	/// Basic geometry class : easy to replace
	/// Written so as to be generalized
	/// </summary>
	public class Vector3D
	{
		double[] _Coordinates = new double[3];

		/// <summary>
		/// Vector3D constructor.
		/// </summary>
		/// <exception cref="ArgumentNullException">Argument array must not be null.</exception>
		/// <exception cref="ArgumentException">The Coordinates' array must contain exactly 3 elements.</exception>
		/// <param name="Coordinates">An array containing the three coordinates' values.</param>
		public Vector3D(double[] Coordinates)
		{
			if ( Coordinates == null ) throw new ArgumentNullException();
			if ( Coordinates.Length!=3 ) throw new ArgumentException("The Coordinates' array must contain exactly 3 elements.");
			DX = Coordinates[0]; DY = Coordinates[1]; DZ = Coordinates[2];
		}

		/// <summary>
		/// Vector3D constructor.
		/// </summary>
		/// <param name="DeltaX">DX coordinate.</param>
		/// <param name="DeltaY">DY coordinate.</param>
		/// <param name="DeltaZ">DZ coordinate.</param>
		public Vector3D(double DeltaX, double DeltaY, double DeltaZ)
		{
			DX = DeltaX; DY = DeltaY; DZ = DeltaZ;
		}

		/// <summary>
		/// Constructs a Vector3D with two points.
		/// </summary>
		/// <param name="P1">First point of the vector.</param>
		/// <param name="P2">Second point of the vector.</param>
		public Vector3D(Point3D P1, Point3D P2)
		{
			DX = P2.X-P1.X; DY = P2.Y-P1.Y; DZ = P2.Z-P1.Z;
		}

		/// <summary>
		/// Accede to coordinates by indexes.
		/// </summary>
		/// <exception cref="IndexOutOfRangeException">Illegal value for CoordinateIndex.</exception>
		public double this[int CoordinateIndex]
		{
			get { return _Coordinates[CoordinateIndex]; }
			set { _Coordinates[CoordinateIndex] = value; }
		}

		/// <summary>
		/// Gets/Sets delta X value.
		/// </summary>
		public double DX { set { _Coordinates[0] = value; } get { return _Coordinates[0]; } }

		/// <summary>
		/// Gets/Sets delta Y value.
		/// </summary>
		public double DY { set { _Coordinates[1] = value; } get { return _Coordinates[1]; } }

		/// <summary>
		/// Gets/Sets delta Z value.
		/// </summary>
		public double DZ { set { _Coordinates[2] = value; } get { return _Coordinates[2]; } }

		/// <summary>
		/// Multiplication of a vector by a scalar value.
		/// </summary>
		/// <param name="V">Vector to operate.</param>
		/// <param name="Factor">Factor value.</param>
		/// <returns>New vector resulting from the multiplication.</returns>
		public static Vector3D operator*(Vector3D V, double Factor)
		{
			double[] New = new double[3];
			for(int i=0; i<3; i++) New[i] = V[i]*Factor;
			return new Vector3D(New);
		}

		/// <summary>
		/// Division of a vector by a scalar value.
		/// </summary>
		/// <exception cref="ArgumentException">Divider cannot be 0.</exception>
		/// <param name="V">Vector to operate.</param>
		/// <param name="Divider">Divider value.</param>
		/// <returns>New vector resulting from the division.</returns>
		public static Vector3D operator/(Vector3D V, double Divider)
		{
			if ( Divider==0 ) throw new ArgumentException("Divider cannot be 0 !\n");
			double[] New = new double[3];
			for(int i=0; i<3; i++) New[i] = V[i]/Divider;
			return new Vector3D(New);
		}

		/// <summary>
		/// Gets the square norm of the vector.
		/// </summary>
		public double SquareNorm
		{
			get
			{
				double Sum = 0;
				for (int i=0; i<3; i++) Sum += _Coordinates[i]*_Coordinates[i];
				return Sum;
			}
		}

		/// <summary>
		/// Gets the norm of the vector.
		/// </summary>
		/// <exception cref="InvalidOperationException">Vector's norm cannot be changed if it is 0.</exception>
		public double Norm
		{
			get { return Math.Sqrt(SquareNorm); }
			set
			{
				double N = Norm;
				if ( N==0 ) throw new InvalidOperationException("Cannot set norm for a nul vector !");
				if ( N!=value )
				{
					double Facteur = value/N;
					for (int i=0; i<3; i++) this[i]*=Facteur;
				}
			}
		}

		/// <summary>
		/// Scalar product between two vectors.
		/// </summary>
		/// <param name="V1">First vector.</param>
		/// <param name="V2">Second vector.</param>
		/// <returns>Value resulting from the scalar product.</returns>
		public static double operator|(Vector3D V1, Vector3D V2)
		{
			double ScalarProduct = 0;
			for(int i=0; i<3; i++) ScalarProduct += V1[i]*V2[i];
			return ScalarProduct;
		}

		/// <summary>
		/// Returns a point resulting from the translation of a specified point.
		/// </summary>
		/// <param name="P">Point to translate.</param>
		/// <param name="V">Vector to apply for the translation.</param>
		/// <returns>Point resulting from the translation.</returns>
		public static Point3D operator+(Point3D P, Vector3D V)
		{
			double[] New = new double[3];
			for(int i=0; i<3; i++) New[i] = P[i]+V[i];
			return new Point3D(New);
		}
	}
}
