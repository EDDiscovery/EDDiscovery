
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{

	/// <summary>
	/// A <see cref="maths.Coord2d"/> stores a 2 dimensional coordinate for cartesian (x,y) or
	/// polar (a,r) mode, and provide operators allowing to add, substract,
	/// multiply and divises coordinate values, as well as computing the distance between
	/// two points, and converting polar and cartesian coordinates.
	/// </summary>
	/// <remarks></remarks>
	public class Coord2d
	{

		#region "Members"
		public double x;
			#endregion
		public double y;

		#region "Constants"
		public static Coord2d ORIGIN = new Coord2d(0, 0);
		public static Coord2d INVALID = new Coord2d(double.NaN, double.NaN);
			#endregion
		public static Coord2d IDENTITY = new Coord2d(1, 1);

		#region "Constructors"
		public Coord2d() : this(0, 0)
		{
		}

		public Coord2d(double xi, double yi)
		{
			this.x = xi;
			this.y = yi;
		}
		#endregion

		#region "Constructors"

		/// <summary>
		/// Set all values of Coord2d
		/// </summary>
		/// <returns>Self</returns>
		/// <remarks></remarks>
		public Coord2d setvalues(double xx, double yy)
		{
			x = xx;
			y = yy;
			return this;
		}


		/// <summary>
		/// Add a value to all components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="value">Value to add to both coordinates (x and y)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d @add(double value)
		{
			return new Coord2d(this.x + value, this.y + value);
		}

		/// <summary>
		/// Add values to components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="xi">x value to add</param>
		/// <param name="yi">y value to add</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d @add(double xi, double yi)
		{
			return new Coord2d(this.x + xi, this.y + yi);
		}

		/// <summary>
		/// Add values of another <see cref="Coord2d"/> to all components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to add</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d @add(Coord2d coord)
		{
			return new Coord2d(this.x + coord.x, this.y + coord.y);
		}

		/// <summary>
		/// Add a value to all components of the current <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="value">Value to add to both coordinates (x and y)</param>
		public void addSelf(double value)
		{
			this.x += value;
			this.y += value;
		}

		/// <summary>
		/// Add values to components of the current <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="xi">x value to add</param>
		/// <param name="yi">y value to add</param>
		public void addSelf(double xi, double yi)
		{
			this.x += xi;
			this.y += yi;
		}

		/// <summary>
		/// Add values of another <see cref="Coord2d"/> to all components of the current <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to add</param>
		public void addSelf(Coord2d coord)
		{
			this.x += coord.x;
			this.y += coord.y;
		}

		/// <summary>
		/// Substract a value to all components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="value">Value to substract to both coordinates (x and y)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d substract(double value)
		{
			return new Coord2d(this.x - value, this.y - value);
		}

		/// <summary>
		/// Substract values to components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="xi">x value to substract</param>
		/// <param name="yi">y value to substract</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d substract(double xi, double yi)
		{
			return new Coord2d(this.x - xi, this.y - yi);
		}

		/// <summary>
		/// Substract values of another <see cref="Coord2d"/> to all components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to substract</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d substract(Coord2d coord)
		{
			return new Coord2d(this.x - coord.x, this.y - coord.y);
		}

		/// <summary>
		/// Substract a value to all components of the current <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="value">Value to substract to both coordinates (x and y)</param>
		public void substractSelf(double value)
		{
			this.x -= value;
			this.y -= value;
		}

		/// <summary>
		/// Substract values to components of the current <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="xi">x value to substract</param>
		/// <param name="yi">y value to substract</param>
		public void substractSelf(double xi, double yi)
		{
			this.x -= xi;
			this.y -= yi;
		}

		/// <summary>
		/// Substract values of another <see cref="Coord2d"/> to all components of the current <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to substract</param>
		public void substractSelf(Coord2d coord)
		{
			this.x -= coord.x;
			this.y -= coord.y;
		}

		/// <summary>
		/// Multiply all components of the current <see cref="Coord2d"/> by a given value and return the result
		/// in a new <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="value">Value to multiply both coordinates with (x and y)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d multiply(double value)
		{
			return new Coord2d(this.x * value, this.y * value);
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord2d"/> by given values and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="xi">x value to multiply with</param>
		/// <param name="yi">y value to multiply with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d multiply(double xi, double yi)
		{
			return new Coord2d(this.x * xi, this.y * yi);
		}

		/// <summary>
		/// Multiply components of another <see cref="Coord2d"/> with components of the current <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to multiply with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d multiply(Coord2d coord)
		{
			return new Coord2d(this.x * coord.x, this.y * coord.y);
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord2d"/> with a given value. 
		/// </summary>
		/// <param name="value">Value to multiply both coordinates with (x and y)</param>
		public void multiplySelf(double value)
		{
			this.x *= value;
			this.y *= value;
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord2d"/> with given values. 
		/// </summary>
		/// <param name="xi">x value to multiply with</param>
		/// <param name="yi">y value to multiply with</param>
		public void multiplySelf(double xi, double yi)
		{
			this.x *= xi;
			this.y *= yi;
		}

		/// <summary>
		/// Multiply components of the current <see cref="Coord2d"/> with values of another <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to multiply with</param>
		public void multiplySelf(Coord2d coord)
		{
			this.x *= coord.x;
			this.y *= coord.y;
		}

		/// <summary>
		/// Divide all components of the current <see cref="Coord2d"/> by a given value and return the result
		/// in a new <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="value">Value to multiply both coordinates with (x and y)</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d divide(double value)
		{
			return new Coord2d(this.x / value, this.y / value);
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord2d"/> by given values and return the result
		/// in a new <see cref="Coord2d"/>.
		/// </summary>
		/// <param name="xi">x value to divide with</param>
		/// <param name="yi">y value to divide with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d divide(double xi, double yi)
		{
			return new Coord2d(this.x / xi, this.y / yi);
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord2d"/> by components of another <see cref="Coord2d"/> and return the result
		/// in a new <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to divide with</param>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d divide(Coord2d coord)
		{
			return new Coord2d(this.x / coord.x, this.y / coord.y);
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord2d"/> by a given value. 
		/// </summary>
		/// <param name="value">Value to divide both coordinates by (x and y)</param>
		public void divideSelf(double value)
		{
			this.x /= value;
			this.y /= value;
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord2d"/> by given values. 
		/// </summary>
		/// <param name="xi">x value to divide by</param>
		/// <param name="yi">y value to divide by</param>
		public void divideSelf(double xi, double yi)
		{
			this.x /= xi;
			this.y /= yi;
		}

		/// <summary>
		/// Divide components of the current <see cref="Coord2d"/> by values of another <see cref="Coord2d"/>. 
		/// </summary>
		/// <param name="coord">Coordinate with values to divide by</param>
		public void divideSelf(Coord2d coord)
		{
			this.x /= coord.x;
			this.y /= coord.y;
		}

		/// <summary>
		/// Assuming current coordinate is in polar system, returns a new coordinate in cartesian system
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d cartesian()
		{
			return new Coord2d(Math.Cos(x) * y, Math.Sin(x) * y);
		}

		/// <summary>
		/// Assuming current coordinate is in cartesian system, returns a new coordinate in polar system
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d polar()
		{
			return new Coord2d(Math.Atan(y / x), Math.Sqrt(x * x + y * y));
		}

		/// <summary>
		/// Assuming current coordinate is in cartesian system, returns a new coordinate in polar system
		/// with real polar values, i.e. with an angle in the range [0, 2*PI]
		/// Source : http://fr.wikipedia.org/wiki/Coordonn%C3%A9es_polaires
		/// </summary>
		/// <remarks>Current object is not modified</remarks>
		public Coord2d fullPolar()
		{
			double radius = Math.Sqrt(x * x + y * y);
			if (x < 0) {
				return new Coord2d(Math.Atan(y / x), radius);
			} else if (x > 0) {
				if (y >= 0) {
					return new Coord2d(Math.Atan(y / x), radius);
				} else {
					return new Coord2d(Math.Atan(y / x) + 2 * Math.PI, radius);
				}
			//x=0
			} else {
				if (y > 0) {
					return new Coord2d(Math.PI / 2, radius);
				} else if (y > 0) {
					return new Coord2d(3 * Math.PI / 2, radius);
				//y=0
				} else {
					return new Coord2d(0, 0);
				}
			}
		}

		/// <summary>
		/// Compute the distance between two coordinates.
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		public double distance(Coord2d coord)
		{
			return Math.Sqrt(Math.Pow((this.x - coord.x), 2) + Math.Pow((this.y - coord.y), 2));
		}

		public override string ToString()
		{
			return ("x=" + x + " y=" + y);
		}

		public double[] toArray()
		{
			return new double[] {
				x,
				y
			};
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
