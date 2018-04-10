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
        public double Distance(Point3D P2)
        {
            return Math.Sqrt((this.X - P2.X) * (this.X - P2.X) + (this.Y - P2.Y) * (this.Y - P2.Y) + (this.Z - P2.Z) * (this.Z - P2.Z));
        }

        public static double DistanceBetween(Point3D P1, Point3D P2)
        {
            return Math.Sqrt((P1.X - P2.X) * (P1.X - P2.X) + (P1.Y - P2.Y) * (P1.Y - P2.Y) + (P1.Z - P2.Z) * (P1.Z - P2.Z));
        }

        public static double DistanceBetweenX2(Point3D P1, Point3D P2)
        {
            return (P1.X - P2.X) * (P1.X - P2.X) + (P1.Y - P2.Y) * (P1.Y - P2.Y) + (P1.Z - P2.Z) * (P1.Z - P2.Z);
        }

        public double DOTP( Point3D other )
        {
            return this.X * other.X + this.Y * other.Y + this.Z * other.Z;
        }
                                                                
        public double InterceptPercent( Point3D x2, Point3D x0) // % along the path THIS(x1)->X2 that a vector from X0 perpendicular meets it      
        {
            double dotp = (this.X - x0.X) * (x2.X - this.X) + (this.Y - x0.Y) * (x2.Y - this.Y) + (this.Z - x0.Z) * (x2.Z - this.Z);
            double mag2 = ((x2.X - this.X) * (x2.X - this.X) + (x2.Y - this.Y) * (x2.Y - this.Y) + (x2.Z - this.Z) * (x2.Z - this.Z));
            return -dotp / mag2;              // its -((x1-x0) dotp (x2-x1) / |x2-x1|^2)
        }

        public Point3D PointAlongPath(Point3D x1, double i) // i = 0 to 1.0, on the path. Negative before the path, >1 after the path
        {
            return new Point3D(this.X + (x1.X - this.X) * i, this.Y + (x1.Y - this.Y) * i, this.Z + (x1.Z - this.Z) * i);
        }

        public Point3D InterceptPoint(Point3D x2, Point3D x0)     // from this(x1) to X2, given a point x0, where do the perpendiclar intercept?
        {
            return PointAlongPath(x2, InterceptPercent(x2, x0));
        }

        public double InterceptPercentageDistance(Point3D x2, Point3D x0, out double dist )     // from this(x1) to X2, given a point x0, where do the perpendiclar intercept?
        {
            double ip = InterceptPercent(x2, x0);
            Point3D point = PointAlongPath(x2, ip);
            dist = DistanceBetween(x0,point);
            return ip;
        }

        public Point3D Subtract(Point3D other )
        {
            return new Point3D(this.X - other.X, this.Y - other.Y, this.Z - other.Z);
        }

        public static double InterceptPercent000(Point3D x2, Point3D x0) // % along the path 0,0,0->X2 that a vector from X0 perpendicular meets it      
        {
            double dotp = (-x0.X) * (x2.X) + (-x0.Y) * (x2.Y) + (-x0.Z) * (x2.Z);       // this is 0,0,0 in effect, so remove terms
            double mag2 = ((x2.X) * (x2.X) + (x2.Y) * (x2.Y) + (x2.Z) * (x2.Z));
            return -dotp / mag2;              // its -((x1-x0) dotp (x2-x1) / |x2-x1|^2)    where x0 =0,0,0
        }

        public static Point3D PointAlongVector(Point3D x1, double i) // i = 0 to 1.0, on the vector from 000
        {
            return new Point3D(x1.X * i, x1.Y * i, x1.Z * i);
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
