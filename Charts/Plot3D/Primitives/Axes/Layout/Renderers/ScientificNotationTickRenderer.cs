
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Plot3D.Primitives.Axes.Layout.Renderers
{

	/// <summary>
	/// Formats 1000 to '1.0e3'
	/// </summary>
	public class ScientificNotationTickRenderer : ITickRenderer
	{


		internal int _precision;
		public ScientificNotationTickRenderer() : this(1)
		{
		}

		public ScientificNotationTickRenderer(int precision)
		{
			_precision = precision;
		}

		public string Format(float value)
		{
			return nzy3D.Maths.Utils.num2str('e', value, _precision);
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
