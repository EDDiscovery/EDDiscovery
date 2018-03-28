
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Events.Mouse
{

	public interface IMouseListener
	{

		/// <summary>
		/// Invoked when the mouse button has been clicked (pressed and released) on a component.
		/// </summary>

		void MouseClicked(object sender, System.Windows.Forms.MouseEventArgs e);
		/// <summary>
		/// Invoked when a mouse button has been pressed on a component. 
		/// </summary>

		void MousePressed(object sender, System.Windows.Forms.MouseEventArgs e);
		/// <summary>
		/// Invoked when a mouse button has been released on a component. 
		/// </summary>

		void MouseReleased(object sender, System.Windows.Forms.MouseEventArgs e);
		/// <summary>
		/// Invoked when a mouse button has been double clicked. 
		/// </summary>

		void MouseDoubleClicked(object sender, System.Windows.Forms.MouseEventArgs e);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
