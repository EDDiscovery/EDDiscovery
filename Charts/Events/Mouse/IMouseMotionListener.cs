
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Events.Mouse
{

	public interface IMouseMotionListener
	{

		// ''' <summary>
		// ''' Invoked when a mouse button is pressed on a component and then dragged.
		// ''' </summary>
		//Sub MouseDragged(sender As Object, e As System.Windows.Forms.MouseEventArgs)
		//Never raised by winfoms

		/// <summary>
		/// Invoked when the mouse cursor has been moved onto a component but no buttons have been pushed.
		/// </summary>

		void MouseMoved(object sender, System.Windows.Forms.MouseEventArgs e);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
