
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Plot3D.Primitives;
using nzy3D.Events;
using System.Drawing;
using nzy3D.Plot3D.Rendering.View;
using nzy3D.Chart;

namespace nzy3D.Plot3D.Rendering.Legends
{

	/// <summary>
	/// A <see cref="Legend"/> represent information concerning a <see cref="AbstractDrawable"/> that may be
	/// displayed as a metadata in the <see cref="ChartView"/>.
	///
	/// The constructor of a <see cref="Legend"/> registers itself as listener of its
	/// parent <see cref="AbstractDrawable"/>, and unregister itself when it is disposed.
	///
	/// When defining a concrete <see cref="Legend"/>, one should:
	/// <ul>
	/// <li>override the {@link toImage(int width, int height)} method, that defines the picture representation.</li>
	/// <li>override the {@link drawableChanged(DrawableChangedEvent e)} method, that must select events that actually triggers an image update.</li>
	/// </ul>
	///
	/// Last, a <see cref="Legend"/> optimizes rendering by :
	/// <ul>
	/// <li>storing current image dimension,</li>
	/// <li>computing a new image only if the required <see cref="Legend"/> dimensions changed.</li>
	/// </ul>
	///
	/// @author Martin Pernollet
	/// </summary>
	/// <remarks></remarks>
	public abstract class Legend : ImageViewport, IDrawableListener
	{


		internal AbstractDrawable _parent;
		public Legend(AbstractDrawable parent)
		{
			_parent = parent;
			if (((_parent != null))) {
				_parent.addDrawableListener(this);
			}
		}

		public void Dispose()
		{
			if (((_parent != null))) {
				_parent.removeDrawableListener(this);
			}
		}

		public abstract Bitmap toImage(int width, int height);
		public abstract void DrawableChanged(DrawableChangedEventArgs e);

		public override void SetViewPort(int width, int height, float left, float right)
		{
			base.SetViewPort(width, height, left, right);
			int imgWidth = (int)(width * (right - left));
			if (_imageWidth != imgWidth | _imageHeight != height) {
				this.Image = toImage(imgWidth, height);
			}
		}

		/// <summary>
		/// Recompute the picture, using last used dimensions.
		/// </summary>
		public void UpdateImage()
		{
			this.Image = toImage(_imageWidth, _imageHeight);
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
