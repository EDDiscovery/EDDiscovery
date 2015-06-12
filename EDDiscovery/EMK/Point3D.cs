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


namespace EMK.LightGeometry
{
	/// <summary>
	/// Basic geometry class : easy to replace
	/// Written so as to be generalized
	/// </summary>
	[Serializable]
	public class Point3D
	{
		double[] _Coordinates = new double[3];

		/// <summary>
		/// Point3D constructor.
		/// </summary>
		/// <exception cref="ArgumentNullException">Argument array must not be null.</exception>
		/// <exception cref="ArgumentException">The Coordinates' array must contain exactly 3 elements.</exception>
		/// <param name="Coordinates">An array containing the three coordinates' values.</param>
		public Point3D(double[] Coordinates)
		{
			if ( Coordinates == null ) throw new ArgumentNullException();
			if ( Coordinates.Length!=3 ) throw new ArgumentException("The Coordinates' array must contain exactly 3 elements.");
			X = Coordinates[0]; Y = Coordinates[1]; Z = Coordinates[2];
		}

		/// <summary>
		/// Point3D constructor.
		/// </summary>
		/// <param name="CoordinateX">X coordinate.</param>
		/// <param name="CoordinateY">Y coordinate.</param>
		/// <param name="CoordinateZ">Z coordinate.</param>
		public Point3D(double CoordinateX, double CoordinateY, double CoordinateZ)
		{
			X = CoordinateX; Y = CoordinateY; Z = CoordinateZ;
		}

		/// <summary>
		/// Accede to coordinates by indexes.
		/// </summary>
		/// <exception cref="IndexOutOfRangeException">Index must belong to [0;2].</exception>
		public double this[int CoordinateIndex]
		{
			get { return _Coordinates[CoordinateIndex]; }
			set	{ _Coordinates[CoordinateIndex] = value; }
		}

		/// <summary>
		/// Gets/Set X coordinate.
		/// </summary>
		public double X { set { _Coordinates[0] = value; } get { return _Coordinates[0]; } }

		/// <summary>
		/// Gets/Set Y coordinate.
		/// </summary>
		public double Y { set { _Coordinates[1] = value; } get { return _Coordinates[1]; } }

		/// <summary>
		/// Gets/Set Z coordinate.
		/// </summary>
		public double Z { set { _Coordinates[2] = value; } get { return _Coordinates[2]; } }

		/// <summary>
		/// Returns the distance between two points.
		/// </summary>
		/// <param name="P1">First point.</param>
		/// <param name="P2">Second point.</param>
		/// <returns>Distance value.</returns>
		public static double DistanceBetween(Point3D P1, Point3D P2)
		{
            return Math.Sqrt((P1.X - P2.X) * (P1.X - P2.X) + (P1.Y - P2.Y) * (P1.Y - P2.Y) + (P1.Y - P2.Z) * (P1.Y - P2.Z));
		}

		/// <summary>
		/// Returns the projection of a point on the line defined with two other points.
		/// When the projection is out of the segment, then the closest extremity is returned.
		/// </summary>
		/// <exception cref="ArgumentNullException">None of the arguments can be null.</exception>
		/// <exception cref="ArgumentException">P1 and P2 must be different.</exception>
		/// <param name="Pt">Point to project.</param>
		/// <param name="P1">First point of the line.</param>
		/// <param name="P2">Second point of the line.</param>
		/// <returns>The projected point if it is on the segment / The closest extremity otherwise.</returns>
		public static Point3D ProjectOnLine(Point3D Pt, Point3D P1, Point3D P2)
		{
			if ( Pt==null || P1==null || P2==null ) throw new ArgumentNullException("None of the arguments can be null.");
			if ( P1.Equals(P2) ) throw new ArgumentException("P1 and P2 must be different.");
			Vector3D VLine = new Vector3D(P1, P2);
			Vector3D V1Pt = new Vector3D(P1, Pt);
			Vector3D Translation = VLine*(VLine|V1Pt)/VLine.SquareNorm;
			Point3D Projection = P1+Translation;

			Vector3D V1Pjt = new Vector3D(P1, Projection);
			double D1 = V1Pjt|VLine;
			if ( D1<0 ) return P1;

			Vector3D V2Pjt = new Vector3D(P2, Projection);
			double D2 = V2Pjt|VLine;
			if ( D2>0 ) return P2;

			return Projection;
		}

		/// <summary>
		/// Object.Equals override.
		/// Tells if two points are equal by comparing coordinates.
		/// </summary>
		/// <exception cref="ArgumentException">Cannot compare Point3D with another type.</exception>
		/// <param name="Point">The other 3DPoint to compare with.</param>
		/// <returns>'true' if points are equal.</returns>
		public override bool Equals(object Point)
		{
			Point3D P = (Point3D)Point;
			if ( P==null ) throw new ArgumentException("Object must be of type "+GetType());
			bool Resultat = true;
			for (int i=0; i<3; i++) Resultat &= P[i].Equals(this[i]);
			return Resultat;
		}

		/// <summary>
		/// Object.GetHashCode override.
		/// </summary>
		/// <returns>HashCode value.</returns>
		public override int GetHashCode()
		{
			double HashCode = 0;
			for (int i=0; i<3; i++) HashCode += this[i];
			return (int)HashCode;
		}

		/// <summary>
		/// Object.GetHashCode override.
		/// Returns a textual description of the point.
		/// </summary>
		/// <returns>String describing this point.</returns>
		public override string ToString()
		{
			string Deb = "{";
			string Sep = ";";
			string Fin = "}";
			string Resultat = Deb;
			int Dimension = 3;
			for (int i=0; i<Dimension; i++)
				Resultat += _Coordinates[i].ToString() + (i!=Dimension-1 ? Sep : Fin);
			return Resultat;
		}
	}
}
