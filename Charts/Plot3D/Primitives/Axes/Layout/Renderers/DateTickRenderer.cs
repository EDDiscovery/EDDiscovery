
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Plot3D.Primitives.Axes.Layout.Renderers
{

	/// <summary>
	/// Force number to be represented with a given number of decimals
	/// </summary>
	public class DateTickRenderer : ITickRenderer
	{


		internal string _format;
		public DateTickRenderer() : this("dd/MM/yyyy HH:mm:ss")
		{
		}

		public DateTickRenderer(string format)
		{
			_format = format;
		}

		public string Format(float value)
		{
			DateTime ldate = nzy3D.Maths.Utils.num2date(Convert.ToInt64(value));
			return nzy3D.Maths.Utils.dat2str(ldate, _format);
		}

	}

}



//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
