
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace nzy3D.Glut
{

	public class BitmapCharRec
	{
		public int width;
		public int height;
		public float xorig;
		public float yorig;
		public float advance;

		public byte[] bitmap;
		public BitmapCharRec(int width, int height, float xorig, float yorig, float advance, byte[] bitmap)
		{
			this.width = width;
			this.height = height;
			this.xorig = xorig;
			this.yorig = yorig;
			this.advance = advance;
			this.bitmap = bitmap;
		}


	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
