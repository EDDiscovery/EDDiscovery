
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths
{
	public class TicToc
	{
		internal System.DateTime tstart;

		internal System.DateTime tstop;
		public void tic()
		{
			tstart = System.DateTime.Now;
		}

		public double toc()
		{
			tstop = System.DateTime.Now;
			return elapsedSecond;
		}

		public TimeSpan elapsedTimeSpan {
			get { return tstop - tstart; }
		}

		public double elapsedMillisecond {
			get { return this.elapsedTimeSpan.TotalMilliseconds; }
		}

		public double elapsedSecond {
			get { return this.elapsedTimeSpan.TotalSeconds; }
		}

	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
