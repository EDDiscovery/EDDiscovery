
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;

namespace nzy3D.Events
{

	public class ViewPointChangedEventArgs : ObjectEventArgs
	{


		private Coord3d _viewPoint;
		public ViewPointChangedEventArgs(object objectChanged, Coord3d viewPoint) : base(objectChanged)
		{
			_viewPoint = viewPoint;
		}

		public Coord3d ViewPoint {
			get { return _viewPoint; }
		}
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
