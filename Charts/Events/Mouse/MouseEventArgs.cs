
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Events.Mouse
{

	public class MouseEventArgs
	{

		private double _x;
		private double _y;

		private MouseButton _button;
		public MouseEventArgs(double x, double y, MouseButton button)
		{
			_x = x;
			_y = y;
			_button = button;
		}

		public double X {
			get { return _x; }
		}

		public double Y {
			get { return _y; }
		}

		public MouseButton Button {
			get { return _button; }
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
