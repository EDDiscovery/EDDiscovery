
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Events.Mouse;
using nzy3D.Events.Keyboard;

namespace nzy3D.Plot3D.Rendering.Canvas
{

	public interface ICanvas
	{

		/// <summary>
		/// Returns a reference to the held view.
		/// </summary>

		View.View View { get; }
		/// <summary>
		/// Returns the renderer's width, i.e. the display width.
		/// </summary>

		int RendererWidth { get; }
		/// <summary>
		/// Returns the renderer's height, i.e. the display height.
		/// </summary>

		int RendererHeight { get; }
		/// <summary>
		/// Invoked when a user requires the Canvas to be repainted (e.g. a non 3d layer has changed).
		/// </summary>

		void ForceRepaint();
		/// <summary>
		/// Returns an image with the current renderer's size.
		/// </summary>
		System.Drawing.Bitmap Screenshot();

		/// <summary>
		/// Performs all required cleanup when destroying a Canvas.
		/// </summary>

		void Dispose();
		void addMouseListener(IMouseListener listener);
		void removeMouseListener(IMouseListener listener);
		void addMouseWheelListener(IMouseWheelListener listener);
		void removeMouseWheelListener(IMouseWheelListener listener);
		void addMouseMotionListener(IMouseMotionListener listener);
		void removeMouseMotionListener(IMouseMotionListener listener);
		void addKeyListener(IKeyListener listener);

		void removeKeyListener(IKeyListener listener);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
