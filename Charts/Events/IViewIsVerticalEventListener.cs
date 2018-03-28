
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Events
{

	public interface IViewIsVerticalEventListener
	{
		void ViewVerticalReached(ViewIsVerticalEventArgs e);
		void ViewVerticalLeft(ViewIsVerticalEventArgs e);
	}

}


//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
