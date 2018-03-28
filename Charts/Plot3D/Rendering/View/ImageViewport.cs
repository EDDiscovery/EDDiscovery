
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace nzy3D.Plot3D.Rendering.View
{

	public class ImageViewport : AbstractViewport
	{

		internal int _imageHeight;
		internal int _imageWidth;
		internal System.Drawing.Bitmap _imageObj;

		internal IntPtr _imageData;
		public ImageViewport()
		{
			StretchToFill = false;
		}

		/// <summary>
		/// Renders the picture into the window, according to the viewport settings.
		/// If the picture is bigger than the viewport, it is simply centered in it,
		/// otherwise, it is scaled in order to fit into the viewport.
		/// </summary>
		/// <remarks></remarks>
		public virtual void Render()
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.PushMatrix();
			GL.LoadIdentity();
			ApplyViewPort();
			GL.Ortho(0, _screenWidth, 0, _screenHeight, -1, 1);
			// Zoom and layout
			GL.MatrixMode(MatrixMode.Modelview);
			GL.PushMatrix();
			GL.LoadIdentity();
			ImageRenderer.RenderImage(_imageData, _imageWidth, _imageHeight, _screenWidth, _screenHeight);
			// Restore matrices state
			GL.PopMatrix();
			GL.MatrixMode(MatrixMode.Projection);
			GL.PopMatrix();
		}

		public System.Drawing.Bitmap Image {
			get { return _imageObj; }
			set {
				_imageObj = value;
				_imageHeight = value.Height;
				_imageWidth = value.Width;
				_imageData = value.GetHbitmap();
			}
		}

		/// <summary>
		/// Return the minimum size for this graphic.
		/// </summary>
		public virtual System.Drawing.Size MinimumSize {
			get { return new System.Drawing.Size(0, 0); }
		}

		/// <summary>
		/// Return the prefered size for this graphic.
		/// </summary>
		public System.Drawing.Size PreferedSize {
			get { return new System.Drawing.Size(1, 1); }
		}


	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
