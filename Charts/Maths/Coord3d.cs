
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{

	/// <summary>
	/// A <see cref="maths.Coord3d"/> stores a 3 dimensional coordinate for cartesian (x, y, y) or
	/// polar (phi, theta, r) (azimuth, elevation/inclination, radius) mode, and provide operators allowing to add, substract,
	/// multiply and divises coordinate values, as well as computing the distance between
	/// two points, and converting polar and cartesian coordinates.
	/// </summary>
	/// <remarks></remarks>
	public class Coord3d
	{

		#region "Members"
		public double x;
		public double y;
			#endregion
		public double z;

		#region "Constants"
		public static Coord3d ORIGIN = new Coord3d(0, 0, 0);
		public static Coord3d INVALID = new Coord3d(double.NaN, double.NaN, double.NaN);
			#endregion
		public static Coord3d IDENTITY = new Coord3d(1, 1, 1);

		#region "Constructors"
		public Coord3d() : this(0, 0, 0)
		{
		}

		public Coord3d(double xi, double yi, double zi)
		{
			this.x = xi;
			this.y = yi;
			this.z = zi;
		}

		public Coord3d(Coord3d c, double zi) : this(c.x, c.y, zi)
		{
		}

		public Coord3d(double[] values) : this(values[0], values[1], values[2])
		{
			if (values.Length != 3) {
				throw new Exception("When creating a Coord3d from an array of double, the array must contain 3 elements (" + values.Length + " elements found here)");
			}
		}

		#endregion

		#region "Functions"

		/// <summary>
		/// Returns a memberwise clone of current object.
		/// </summary>
		public Coord3d Clone()
		{
			return (Coord3d)this.MemberwiseClone();
		}

		/// <summary>
		/// Set all values of Coord3d
		/// </summary>
		/// <returns>Self</returns>
		/// <remarks></remarks>
		public Coord3d setvalues(double xx, double yy, double zz)
		{
			x = xx;
			y = yy;
			z = zz;
			return this;
		}

		/// <summary>
		/// Set all values of Coord3d
		/// </summary>
		/// <returns>Self</returns>
		/// <remarks></remarks>
		public Coord3d set(Coord3d another)
		{
			return setvalues(another.x, another.y, another.z);
		}

		/// <summary>
		/// Returns the x and y components as 2d coordinate
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		public Coord2d getXY()
		{
			return new Coord2d(this.x, this.y);
		}

		/// <summary>
		/// Add a value to all components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="value">Value to add to all coordinates (x, y, z)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d @add(double value)
		{
			return new Coord3d(this.x + value, this.y + value, this.z + value);
		}

		/// <summary>
		/// Add values to components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="xi">x value to add</param>
		/// <param name="yi">y value to add</param>
		/// <param name="zi">z value to add</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d @add(double xi, double yi, double zi)
		{
			return new Coord3d(this.x + xi, this.y + yi, this.z + zi);
		}

		/// <summary>
		/// Add values of another <see cref="Coord3d"/> to all components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to add</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d @add(Coord3d coord)
		{
			return new Coord3d(this.x + coord.x, this.y + coord.y, this.z + coord.z);
		}

		/// <summary>
		/// Add a value to all components of the current <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="value">Value to add to all coordinates (x, y and z)</param>
		public void addSelf(double value)
		{
			this.x += value;
			this.y += value;
			this.z += value;
		}

		/// <summary>
		/// Add values to components of the current <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="xi">x value to add</param>
		/// <param name="yi">y value to add</param>
		/// <param name="zi">z value to add</param>
		public void addSelf(double xi, double yi, double zi)
		{
			this.x += xi;
			this.y += yi;
			this.z += zi;
		}

		/// <summary>
		/// Add values of another <see cref="Coord3d"/> to all components of the current <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to add</param>
		public void addSelf(Coord3d coord)
		{
			this.x += coord.x;
			this.y += coord.y;
			this.z += coord.z;
		}

		/// <summary>
		/// Substract a value to all components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="value">Value to substract to both coordinates (x, y and z)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d substract(double value)
		{
			return new Coord3d(this.x - value, this.y - value, this.z - value);
		}

		/// <summary>
		/// Substract values to components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="xi">x value to substract</param>
		/// <param name="yi">y value to substract</param>
		/// <param name="zi">z value to substract</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d substract(double xi, double yi, double zi)
		{
			return new Coord3d(this.x - xi, this.y - yi, this.z - zi);
		}

		/// <summary>
		/// Substract values of another <see cref="Coord3d"/> to all components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to substract</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d substract(Coord3d coord)
		{
			return new Coord3d(this.x - coord.x, this.y - coord.y, this.z - coord.z);
		}

		/// <summary>
		/// Substract a value to all components of the current <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="value">Value to substract to both coordinates (x, y and z)</param>
		public void substractSelf(double value)
		{
			this.x -= value;
			this.y -= value;
			this.z -= value;
		}

		/// <summary>
		/// Substract values to components of the current <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="xi">x value to substract</param>
		/// <param name="yi">y value to substract</param>
		/// <param name="zi">z value to substract</param>
		public void substractSelf(double xi, double yi, double zi)
		{
			this.x -= xi;
			this.y -= yi;
			this.z -= zi;
		}

		/// <summary>
		/// Substract values of another <see cref="Coord3d"/> to all components of the current <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to substract</param>
		public void substractSelf(Coord3d coord)
		{
			this.x -= coord.x;
			this.y -= coord.y;
			this.z -= coord.z;
		}

		/// <summary>
		/// Multiply all components of the current <see cref="Coord3d"/> by a given value and return the result
		/// in a new <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="value">Value to multiply both coordinates with (x, y and z)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d multiply(double value)
		{
			return new Coord3d(this.x * value, this.y * value, this.z * value);
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord3d"/> by given values and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="xi">x value to multiply with</param>
		/// <param name="yi">y value to multiply with</param>
		/// <param name="zi">z value to multiply with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d multiply(double xi, double yi, double zi)
		{
			return new Coord3d(this.x * xi, this.y * yi, this.z * zi);
		}

		/// <summary>
		/// Multiply components of another <see cref="Coord3d"/> with components of the current <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to multiply with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d multiply(Coord3d coord)
		{
			return new Coord3d(this.x * coord.x, this.y * coord.y, this.z * coord.z);
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord3d"/> with a given value. 
		/// </summary>
		/// <param name="value">Value to multiply both coordinates with (x, y and z)</param>
		public void multiplySelf(double value)
		{
			this.x *= value;
			this.y *= value;
			this.z *= value;
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord3d"/> with given values. 
		/// </summary>
		/// <param name="xi">x value to multiply with</param>
		/// <param name="yi">y value to multiply with</param>
		/// <param name="zi">z value to multiply with</param>
		public void multiplySelf(double xi, double yi, double zi)
		{
			this.x *= xi;
			this.y *= yi;
			this.z *= zi;
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord3d"/> with values of another <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to multiply with</param>
		public void multiplySelf(Coord3d coord)
		{
			this.x *= coord.x;
			this.y *= coord.y;
			this.z *= coord.z;
		}

		/// <summary>
		/// Divide all components of the current <see cref="Coord3d"/> by a given value and return the result
		/// in a new <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="value">Value to multiply both coordinates with (x, y and z)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d divide(double value)
		{
			return new Coord3d(this.x / value, this.y / value, this.z / value);
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord3d"/> by given values and return the result
		/// in a new <see cref="Coord3d"/>.
		/// </summary>
		/// <param name="xi">x value to divide with</param>
		/// <param name="yi">y value to divide with</param>
		/// <param name="zi">z value to divide with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d divide(double xi, double yi, double zi)
		{
			return new Coord3d(this.x / xi, this.y / yi, this.z / zi);
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord3d"/> by components of another <see cref="Coord3d"/> and return the result
		/// in a new <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to divide with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d divide(Coord3d coord)
		{
			return new Coord3d(this.x / coord.x, this.y / coord.y, this.z / coord.z);
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord3d"/> by a given value. 
		/// </summary>
		/// <param name="value">Value to divide both coordinates by (x, y and z)</param>
		public void divideSelf(double value)
		{
			this.x /= value;
			this.y /= value;
			this.z /= value;
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord3d"/> by given values. 
		/// </summary>
		/// <param name="xi">x value to divide by</param>
		/// <param name="yi">y value to divide by</param>
		/// <param name="zi">z value to divide by</param>
		public void divideSelf(double xi, double yi, double zi)
		{
			this.x /= xi;
			this.y /= yi;
			this.z /= zi;
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord3d"/> by values of another <see cref="Coord3d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to divide by</param>
		public void divideSelf(Coord3d coord)
		{
			this.x /= coord.x;
			this.y /= coord.y;
			this.z /= coord.z;
		}

		/// <summary>
		/// Returns a new coordinate equal to the negation of current one
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d negative()
		{
			return new Coord3d(-x, -y, -z);
		}

		/// <summary>
		/// Assuming current coordinate is in polar system, returns a new coordinate in cartesian system
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d cartesian()
		{
			return new Coord3d(Math.Cos(x) * Math.Cos(y) * z, Math.Sin(x) * Math.Cos(y) * z, Math.Sin(y) * z);
		}

		/// <summary>
		/// Assuming current coordinate is in cartesian system, returns a new coordinate in polar system
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d polar()
		{
			double a = 0;
			double e = 0;
			double r = 0;
			double d = 0;
			r = Math.Sqrt(x * x + y * y + z * z);
			d = Math.Sqrt(x * x + y * y);
			if (d == 0 & z > 0) {
				return new Coord3d(0, Math.PI / 2, r);
			} else if (d == 0 & z == 0) {
				return new Coord3d(0, 0, 0);
			} else if (d == 0 & z < 0) {
				return new Coord3d(0, -Math.PI / 2, r);
			} else {
				if (Math.Abs(x / d) < 1) {
					// Classical case for azimuth
					a = Math.Acos(x / d) * (y > 0 ? 1 : -1);
				} else if (y == 0 & x > 0) {
					a = 0;
				} else if (y == 0 & x < 0) {
					a = Math.PI;
				} else {
					a = 0;
				}
				e = Math.Atan(z / d);
			}
			return new Coord3d(a, e, r);
		}

		/// <summary>
		/// Compute the distance between two coordinates.
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		public double distance(Coord3d coord)
		{
			return Math.Sqrt(Math.Pow((this.x - coord.x), 2) + Math.Pow((this.y - coord.y), 2) + Math.Pow((this.z - coord.z), 2));
		}

		/// <summary>
		/// Returns the squared distance of coordinates ( x * x + y * y + z * z )
		/// </summary>
		public double magSquared()
		{
			return x * x + y * y + z * z;
		}

		/// <summary>
		/// Returns the dot product of current coordinate with another coordinate
		/// </summary>
		public double dot(Coord3d coord)
		{
			return x * coord.x + y * coord.y + z * coord.z;
		}

		/// <summary>
		/// Assuming current coordinate is in cartesian system, returns a new coordinate in polar system
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d normalizeTo(double len)
		{
			double mag = Math.Sqrt(magSquared());
			if (mag > 0) {
				mag = len / mag;
				return new Coord3d(x * mag, y * mag, z * mag);
			} else {
				return new Coord3d(0, 0, 0);
			}
		}

		/// <summary>
		/// Assuming current coordinate is in cartesian system, returns a new coordinate in polar system
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord3d interpolateTo(Coord3d coord, double f)
		{
			return new Coord3d(x + (coord.x - x) * f, y + (coord.y - y) * f, z + (coord.z - z) * f);
		}

		public override string ToString()
		{
			return ("x=" + x + " y=" + y + " z=" + z);
		}

		public double[] toArray()
		{
			return new double[] {
				x,
				y,
				z
			};
		}

		public static bool operator ==(Coord3d coord1, Coord3d coord2)
		{
			if (coord1 == null & coord2 == null) {
				return true;
			}

			if (coord1 == null | coord2 == null) {
				return false;
			} else {
				if (coord1.Equals(coord2)) {
					return true;
				}

				return (coord1.x == coord2.x) & (coord1.y == coord2.y) & (coord1.z == coord2.z);
			}
		}

		public static bool operator !=(Coord3d coord1, Coord3d coord2)
		{
			if (coord1 == null & coord2 == null) {
				return false;

			}
			if (coord1 == null | coord2 == null) {
				return true;
			}
			return !(coord1 == coord2);
		}

		#endregion

	}

}


//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
