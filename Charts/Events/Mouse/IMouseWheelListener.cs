
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Events.Mouse
{

	public interface IMouseWheelListener
	{

		/// <summary>
		/// Invoked when a mouse button is pressed on a component and then dragged.
		/// </summary>

		void MouseWheelMoved(object sender, System.Windows.Forms.MouseEventArgs e);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
