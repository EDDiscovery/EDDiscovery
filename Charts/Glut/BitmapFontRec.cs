
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace nzy3D.Glut
{

	public class BitmapFontRec
	{
		public string name;
		public int num_chars;
		public int first;

		public BitmapCharRec[] ch;
		public BitmapFontRec(string name, int num_chars, int first, BitmapCharRec[] ch)
		{
			this.name = name;
			this.num_chars = num_chars;
			this.first = first;
			this.ch = ch;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
