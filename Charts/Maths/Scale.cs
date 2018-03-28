
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{


	public class Scale
	{

		private double m_min;

		private double m_max;
		public Scale(double min, double max)
		{
			m_min = min;
			m_max = max;
		}

		public double Min {
			get { return m_min; }
			set { m_min = value; }
		}

		public double Max {
			get { return m_max; }
			set { m_max = value; }
		}

		public double Range {
			get { return Max - Min; }
		}

		/// <summary>
		/// Add a value to min and max values of the scale
		/// </summary>
		/// <param name="value">Value to add</param>
		/// <returns>New scale with added value to min &amp; max</returns>
		/// <remarks>Current object is not modified</remarks>
		public Scale @add(double value)
		{
			return new Scale(Min + value, Max + value);
		}


		/// <summary>
		/// Return True if value is inside [Min;Max]
		/// </summary>
		public bool Contains(double value)
		{
			return (Min <= value & value <= Max);
		}

		public bool isMaxNan()
		{
			return double.IsNaN(Max);
		}

		public bool isMinNan()
		{
			return double.IsNaN(Min);
		}

		/// <summary>
		/// Returns True if Min &lt;= Max
		/// </summary>
		public bool valid()
		{
			return Min <= Max;
		}

		public static Scale Widest(Scale scale1, Scale scale2)
		{
			return new Scale(Math.Min(scale1.Min, scale2.Min), Math.Max(scale1.Max, scale2.Max));
		}

		public static Scale Thinest(Scale scale1, Scale scale2)
		{
			return new Scale(Math.Max(scale1.Min, scale2.Min), Math.Min(scale1.Max, scale2.Max));
		}

		public static Scale Enlarge(Scale scale, double ratio)
		{
			double offset = (scale.Max - scale.Min) * ratio;
			if (offset == 0) {
				offset = 1;
			}
			return new Scale(scale.Min - offset, scale.Max + offset);
		}

		public override string ToString()
		{
			return ("min=" + Min + " max=" + Max);
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
