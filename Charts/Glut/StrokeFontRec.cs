
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Glut
{

	public class StrokeFontRec
	{
		public string name;
		public int num_chars;
		public StrokeCharRec[] ch;
		public float top;

		public float bottom;
		public StrokeFontRec(string name, int num_chars, StrokeCharRec[] ch, float top, float bottom)
		{
			this.name = name;
			this.num_chars = num_chars;
			this.ch = ch;
			this.top = top;
			this.bottom = bottom;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
