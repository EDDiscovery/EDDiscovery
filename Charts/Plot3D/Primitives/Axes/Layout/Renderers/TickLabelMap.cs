
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Plot3D.Primitives.Axes.Layout.Renderers
{

	/// <summary>
	/// An <see cref="ITickRenderer"/> that can store a list of labels for given axis values.
	/// </summary>
	/// <author>Martin Pernollet</author>
	public class TickLabelMap : ITickRenderer
	{


		internal Dictionary<float, string> _tickvalues = new Dictionary<float, string>();
		public void Register(float value, string label)
		{
			_tickvalues.Add(value, label);
		}

		public bool Contains(float value)
		{
			return _tickvalues.ContainsKey(value);
		}

		public bool Contains(string label)
		{
			return _tickvalues.ContainsValue(label);
		}

		public string Format(float value)
		{
			if (Contains(value)) {
                return _tickvalues[value];
			} else {
				return "";
			}
		}

	}

}


//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
