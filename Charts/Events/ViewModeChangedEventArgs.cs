
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Plot3D.Rendering.View.Modes;

namespace nzy3D.Events
{

	public class ViewModeChangedEventArgs : ObjectEventArgs
	{


		private ViewPositionMode _mode;
		public ViewModeChangedEventArgs(object objectChanged, ViewPositionMode mode) : base(objectChanged)
		{
			_mode = mode;
		}

		public ViewPositionMode Mode {
			get { return _mode; }
		}
	}

}


//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
