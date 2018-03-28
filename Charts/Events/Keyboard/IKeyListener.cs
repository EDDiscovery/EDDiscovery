
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Events.Keyboard
{

	public interface IKeyListener
	{

		/// <summary>
		/// Invoked when a key has been typed (key pressed in .net). 
		/// </summary>

		void KeyTyped(object sender, System.Windows.Forms.KeyPressEventArgs e);
		/// <summary>
		/// Invoked when a key has been pressed (key down in .net).
		/// </summary>

		void KeyPressed(object sender, System.Windows.Forms.KeyEventArgs e);
		/// <summary>
		/// Invoked when a key has been released (key up in .net).
		/// </summary>

		void KeyReleased(object sender, System.Windows.Forms.KeyEventArgs e);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
