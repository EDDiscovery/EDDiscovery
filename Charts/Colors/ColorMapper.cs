
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Colors.ColorMaps;
using nzy3D.Maths;

namespace nzy3D.Colors
{
	public class ColorMapper : IColorMappable
	{

		private double m_zmin;
		private double m_zmax;
		private ColorMaps.IColorMap m_colormap;

		private Color m_factor;
		public ColorMapper(IColorMap colormap, double zmin, double zmax, Color factor)
		{
			m_colormap = colormap;
			m_zmin = zmin;
			m_zmax = zmax;
			m_factor = factor;
		}

		public ColorMapper(IColorMap colormap, double zmin, double zmax) : this(colormap, zmin, zmax, null)
		{
		}

		public ColorMapper(ColorMapper colormapper, Color factor) : this(colormapper.ColorMap, colormapper.ZMin, colormapper.ZMax, factor)
		{
		}

		public IColorMap ColorMap {
			get { return m_colormap; }
		}

		public Color Color(Coord3d coord) {
			Color @out = m_colormap.GetColor(this, coord.x, coord.y, coord.z);
			if ((m_factor != null)) {
				@out.mul(m_factor);
			}
			return @out;
		}

		public Color Color(double v) {
			Color @out = m_colormap.GetColor(this, v);
			if ((m_factor != null)) {
				@out.mul(m_factor);
			}
			return @out;
		}

		public double ZMax {
			get { return m_zmax; }
			set { m_zmax = value; }
		}

		public double ZMin {
			get { return m_zmin; }
			set { m_zmin = value; }
		}

		/// <summary>
		/// Range representing zmin/zmax values (same as <see cref="Scale"/> with different object type)
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Range Range {
			get { return new Range(ZMin, ZMax); }
			set {
				ZMin = value.Min;
				ZMax = value.Max;
			}
		}

		/// <summary>
		/// Scale representing zmin/zmax values (same as <see cref="Range"/> with different object type)
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Scale Scale {
			get { return new Scale(ZMin, ZMax); }
			set {
				ZMin = value.Min;
				ZMax = value.Max;
			}
		}

		/// <summary>
		/// Returns the string representation of this colormapper
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		public override string ToString()
		{
            return "(ColorMapper) " + ColorMap.ToString() + " zmin=" + ZMin + " zmax=" + ZMax + " factor=" + m_factor.ToString();
		}


	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
