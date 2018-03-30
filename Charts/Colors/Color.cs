
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Colors
{
	public class Color
	{

		#region "Members"
		public double r;
		public double g;
		public double b;
		public double a;
        #endregion

        #region "Constants"
        public static Color BLACK = new Color(0.0, 0.0, 0.0);
		public static Color WHITE = new Color(1.0, 1.0, 1.0);
		public static Color GRAY = new Color(0.5, 0.5, 0.5);
		public static Color RED = new Color(1.0, 0.0, 0.0);
		public static Color GREEN = new Color(0.0, 1.0, 0.0);
		public static Color BLUE = new Color(0.0, 0.0, 1.0);
		public static Color YELLOW = new Color(1.0, 1.0, 0.0);
		public static Color MAGENTA = new Color(1.0, 0.0, 1.0);
		public static Color CYAN = new Color(0.0, 1.0, 1.0);
			#endregion
		static internal Random randObj = new Random();

		#region "Constructors"

		/// <summary>
		///  Initialize a color with values between 0 and 1 and an alpha channel set to maximum
		/// </summary>
		/// <param name="r">Red value (between 0 and 1)</param>
		/// <param name="g">Green value (between 0 and 1)</param>
		/// <param name="b">Blue value (between 0 and 1)</param>
		/// <remarks></remarks>
		public Color(double r, double g, double b) : this(r, g, b, 1)
		{
		}

		/// <summary>
		/// Initialize a color with values between 0 and 255 and an alpha channel set to maximum
		/// </summary>
		/// <param name="r">Red value (between 0 and 255)</param>
		/// <param name="g">Green value (between 0 and 255)</param>
		/// <param name="b">Blue value (between 0 and 255)</param>
		/// <remarks></remarks>
		public Color(int r, int g, int b) : this(Convert.ToDouble(r / 255), Convert.ToDouble(g / 255), Convert.ToDouble(b / 255), 1)
		{
		}

		/// <summary>
		/// Initialize a color with values between 0 and 1
		/// </summary>
		/// <param name="r">Red value (between 0 and 1)</param>
		/// <param name="g">Green value (between 0 and 1)</param>
		/// <param name="b">Blue value (between 0 and 1)</param>
		/// <param name="a">a value (between 0 and 1)</param>
		/// <remarks></remarks>
		public Color(double r, double g, double b, double a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		/// <summary>
		///  Initialize a color with values between 0 and 255
		/// </summary>
		/// <param name="r">Red value (between 0 and 255)</param>
		/// <param name="g">Green value (between 0 and 255)</param>
		/// <param name="b">Blue value (between 0 and 255)</param>
		/// <param name="a">a value (between 0 and 255)</param>
		/// <remarks></remarks>
		public Color(int r, int g, int b, int a) : this(Convert.ToDouble(r / 255), Convert.ToDouble(g / 255), Convert.ToDouble(b / 255), Convert.ToDouble(a / 255))
		{
		}

		#endregion

		#region "Methods"

		/// <summary>
		/// Multiply current color components (including alpha value) by <paramref name="factor"/> color components values and assign value to current color.
		/// </summary>
		/// <param name="factor">Multiply values.</param>
		/// <remarks></remarks>
		public void mul(Color factor)
		{
			this.r *= factor.r;
			this.g *= factor.g;
			this.b *= factor.b;
			this.a *= factor.a;
		}

		/// <summary>
		/// Returns the hexadecimal representation of this color, without alpha channel value
		/// </summary>
		public string toHexString {
			get {
				string hexa = "#";
				hexa += this.r.ToString("X2");
				hexa += this.g.ToString("X2");
				hexa += this.b.ToString("X2");
				return hexa;
			}
		}

		/// <summary>
		/// Returns the string representation of this color, including alpha channel value
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		public override string ToString()
		{
			return "(Color) r=" + r + " g=" + g + " b=" + b + " a=" + a;
		}

		public double[] toArray()
		{
			return new double[] {
				r,
				g,
				b,
				a
			};
		}

		public double[] negative()
		{
			return new double[] {
				1 - r,
				1 - g,
				1 - b,
				a
			};
		}

		public Color negativeColor()
		{
			return new Color(1 - r, 1 - g, 1 - b, a);
		}

		public static Color random()
		{
            return new Color(randObj.NextDouble(), randObj.NextDouble(), randObj.NextDouble());
		}

		public System.Drawing.Color toColor()
		{
            return System.Drawing.Color.FromArgb((int)a, (int)r, (int)g, (int)b);
		}
		#endregion

		public OpenTK.Graphics.Color4 OpenTKColor4 {
			get { return new OpenTK.Graphics.Color4(Convert.ToSingle(this.r), Convert.ToSingle(this.g), Convert.ToSingle(this.b), Convert.ToSingle(this.a)); }
		}

	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
