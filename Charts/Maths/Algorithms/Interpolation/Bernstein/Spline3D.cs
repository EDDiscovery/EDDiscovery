
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths.Algorithms.Interpolation.Bernstein
{

	/// <summary>
	/// <para> This is a generic 3D B-Spline class for curves of arbitrary length, control
	///  handles and patches are created and joined automatically as described here:
	/// http://www.ibiblio.org/e-notes/Splines/Bint.htm">ibiblio.org/e-notes/Splines/Bint.htm
	/// </para>
	/// <para>
	/// Thanks to a bug report by Aaron Meyers (http://universaloscillation.com) the
	/// {@linkplain #computeVertices(int)} method has a slightly changed behaviour
	/// from version 0014 onwards. In earlier versions erroneous duplicate points
	/// would be added near each given control point, which lead to various weird
	/// results.
	/// </para>
	/// <para>
	/// The new behaviour of the curve interpolation/computation is described in the
	/// docs for the {@linkplain #computeVertices(int)} method below.
	/// </para>
	/// </summary>
	/// <remarks>
	/// Version 0014 Added user adjustable curve tightness control
	/// Version 0015 Added JAXB annotations and List support for dynamic building of spline
	/// </remarks>
	public class Spline3D
	{

		public static double DEFAULT_TIGHTNESS = 0.25;
		internal Coord3d[] points;
		public List<Coord3d> mpointList;
		public List<Coord3d> vertices;
		public BernsteinPolynomial bernstein;
		public Coord3d[] delta;
		public Coord3d[] coeffA;
		public double[] bi;
		internal double mtightness;
		internal double invTightness;

		internal int numP;
		/// <summary>
		/// Constructor (default tightness, no control points and no predefined bernstein polynomial)
		/// </summary>
		public Spline3D() : this(new List<Coord3d>(), null, DEFAULT_TIGHTNESS)
		{
		}

		/// <summary>
		///  Constructor (default tightness and no predefined bernstein polynomial)
		/// </summary>
		/// <param name="rawPoints">List of control point vectors</param>
		public Spline3D(List<Coord3d> rawPoints) : this(rawPoints, null, DEFAULT_TIGHTNESS)
		{
		}

		/// <summary>
		///  Constructor (default tightness and no predefined bernstein polynomial)
		/// </summary>
		/// <param name="rawPoints">List of control point vectors</param>
		/// <param name="b">Predefined Bernstein polynomial (good for reusing)</param>
		/// <param name="tightness">Default curve tightness used for the interpolated vertices</param>
		public Spline3D(List<Coord3d> rawPoints, BernsteinPolynomial b, double tightness)
		{
			this.Tightness = tightness;
			mpointList = new List<Coord3d>();
			mpointList.AddRange(rawPoints);
			bernstein = b;
		}

		/// <summary>
		///  Constructor (default tightness and no predefined bernstein polynomial)
		/// </summary>
		/// <param name="rawPoints">Array of control point vectors</param>
		public Spline3D(Coord3d[] rawPoints) : this(rawPoints, null, DEFAULT_TIGHTNESS)
		{
		}

		/// <summary>
		///  Constructor (default tightness and no predefined bernstein polynomial)
		/// </summary>
		/// <param name="rawPoints">Array of control point vectors</param>
		/// <param name="b">Predefined Bernstein polynomial (good for reusing)</param>
		/// <param name="tightness">Default curve tightness used for the interpolated vertices</param>
		public Spline3D(Coord3d[] rawPoints, BernsteinPolynomial b, double tightness) : this(new List<Coord3d>(rawPoints), b, DEFAULT_TIGHTNESS)
		{
		}

		/// <summary>
		/// Property to access (read/write) curve tightness used for the interpolated vertices
		/// of future curve interpolation calls. Default value is
		/// 0.25. A value of 0.0 equals linear interpolation between the given curve
		/// input points. Curve behaviour for values outside the 0.0 .. 0.5 interval
		/// is unspecified and becomes increasingly less intuitive. Negative values
		/// are possible too and create interesting results (in some cases).
		/// </summary>
		public double Tightness {
			get { return mtightness; }
			set {
				mtightness = value;
				invTightness = 1 / mtightness;
			}
		}

		public List<Coord3d> pointList {
			get { return mpointList; }
			set {
				mpointList.Clear();
				mpointList.AddRange(value);
			}
		}

		/// <summary>
		/// Returns the number of key points.
		/// </summary>
		public int NumPoints {
			get { return numP; }
		}

		/// <summary>
		/// Adds the given point to the list of control points.
		/// </summary>
		/// <returns>Itself</returns>
		public Spline3D Add(Coord3d p)
		{
			mpointList.Add(p);
			return this;
		}

		/// <summary>
		/// Adds the given point to the list of control points.
		/// </summary>
		/// <returns>Itself</returns>
		public Spline3D Add(double x, double y, double z)
		{
			return Add(new Coord3d(x, y, z));
		}

		public void UpdateCoefficients()
		{
			numP = pointList.Count;
			if (points == null || points.Length - 1 != numP) {
				coeffA = new Coord3d[numP];
				delta = new Coord3d[numP];
				bi = new double[numP];
				for (int i = 0; i <= numP - 1; i++) {
					coeffA[i] = new Coord3d();
                    delta[i] = new Coord3d();
				}
			}
			points = pointList.ToArray();
		}

		public List<Coord3d> ComputeVertices(int resolution)
		{
			UpdateCoefficients();
			resolution += 1;
			if (bernstein == null || bernstein.resolution != resolution) {
				bernstein = new BernsteinPolynomial(resolution);
			}
			if (vertices == null) {
				vertices = new List<Coord3d>();
			} else {
				vertices.Clear();
			}
			findCPoints();
			Coord3d deltaP = default(Coord3d);
			Coord3d deltaQ = default(Coord3d);
			resolution -= 1;
			for (int i = 0; i <= numP - 2; i++) {
				Coord3d p = points[i];
				Coord3d q = points[i + 1];
				deltaP = delta[i].@add(p);
				deltaQ = q.substract(delta[i + 1]);
				for (int k = 0; k <= resolution - 1; k++) {
					double x = p.x * bernstein.b0[k] + deltaP.x * bernstein.b1[k] + deltaQ.x * bernstein.b2[k] + q.x * bernstein.b3[k];
					double y = p.y * bernstein.b0[k] + deltaP.y * bernstein.b1[k] + deltaQ.y * bernstein.b2[k] + q.y * bernstein.b3[k];
					double z = p.z * bernstein.b0[k] + deltaP.z * bernstein.b1[k] + deltaQ.z * bernstein.b2[k] + q.z * bernstein.b3[k];
					vertices.Add(new Coord3d(x, y, z));
				}
			}
			vertices.Add(points[points.Length - 1]);
			return vertices;
		}

		internal void findCPoints()
		{
			bi[1] = -Tightness;
			coeffA[1].setvalues((points[2].x - points[0].x - delta[0].x) * Tightness, (points[2].y - points[0].y - delta[0].y) * Tightness, (points[2].z - points[0].z - delta[0].z) * Tightness);
			// correction: original java code : for (int i = 2; i < numP - 1; i++) 
			for (int i = 2; i <= numP - 2; i++) {
				bi[i] = -1 / (invTightness + bi[i - 1]);
				coeffA[i].setvalues(-(points[i + 1].x - points[i - 1].x - coeffA[i - 1].x) * bi[i], -(points[i + 1].y - points[i - 1].y - coeffA[i - 1].y) * bi[i], -(points[i + 1].z - points[i - 1].z - coeffA[i - 1].z) * bi[i]);
			}
			for (int i = numP - 2; i >= 1; i += -1) {
				delta[i].setvalues(coeffA[i].x + delta[i + 1].x * bi[i], coeffA[i].y + delta[i + 1].y * bi[i], coeffA[i].z + delta[i + 1].z * bi[i]);
			}
		}

		public List<Coord3d> getDecimatedVertices(double dstep)
		{
			List<Coord3d> steps = new List<Coord3d>();
			int num = vertices.Count;
			int i = 0;
			double segLen = 0;
			Coord3d a = default(Coord3d);
			Coord3d dir = default(Coord3d);
			Coord3d stepDir = default(Coord3d);
			Coord3d b = vertices[0];
			Coord3d curr = b.Clone();
			while (i < num - 1) {
				a = b;
				b = vertices[i + 1];
				dir = b.substract(a);
                segLen = 1 / dir.magSquared();
				stepDir = dir.normalizeTo(dstep);
				curr = a.interpolateTo(b, curr.substract(a).dot(dir) * segLen);
				while (curr.substract(a).dot(dir) / segLen <= 1) {
					steps.Add(curr.Clone());
					curr.addSelf(stepDir);
				}
				i += 1;
			}
			return steps;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
