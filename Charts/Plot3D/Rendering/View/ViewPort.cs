
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Plot3D.Rendering.View
{

	public class ViewPort
	{

		internal int _width;
		internal int _height;
		internal int _x;

		internal int _y;
		public ViewPort(int width, int height) : this(width, height, 0, 0)
		{
		}

		public ViewPort(int width, int height, int x, int y)
		{
			_width = width;
			_height = height;
			_x = x;
			_y = y;
		}

		public static ViewPort Slice(int width, int height, float left, float right)
		{
			int thiswidth = Convert.ToInt32((right - left) * width);
			int thisheight = height;
			int thisx = Convert.ToInt32(left * width);
			int thisy = thisx + thiswidth;
			return new ViewPort(thiswidth, thisheight, thisx, thisy);
		}

		public int Width {
			get { return _width; }
			set { _width = value; }
		}

		public int Height {
			get { return _height; }
			set { _height = value; }
		}

		public int X {
			get { return _x; }
			set { _x = value; }
		}

		public int Y {
			get { return _y; }
			set { _y = value; }
		}

		public override string ToString()
		{
			return "(ViewPort) width=" + Width + " height=" + Height + " x=" + X + " y=" + Y;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
