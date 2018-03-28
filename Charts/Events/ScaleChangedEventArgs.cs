
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;

namespace nzy3D.Events
{

	public class ScaleChangedEventArgs : ObjectEventArgs
	{

		private Scale _scaling;

		private int _scaleID;
		public ScaleChangedEventArgs(object objectChanged, Scale scaling, int scaleID) : base(objectChanged)
		{
			_scaling = scaling;
			_scaleID = scaleID;
		}

		public ScaleChangedEventArgs(object objectChanged, Scale scaling) : this(objectChanged, scaling, -1)
		{
		}

		public Scale Scaling {
			get { return _scaling; }
		}

		public int ScaleId {
			get { return _scaleID; }
		}

		public override string ToString()
		{
			return "ScaleChangeEventArgs:id" + ScaleId + ", scale=" + Scaling.ToString();
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
