
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using nzy3D.Colors;
using nzy3D.Plot2D.Primitive;
using nzy3D.Plot3D.Primitives;
using nzy3D.Plot3D.Primitives.Axes.Layout;
using nzy3D.Plot3D.Primitives.Axes.Layout.Providers;
using nzy3D.Plot3D.Primitives.Axes.Layout.Renderers;
using nzy3D.Plot3D.Rendering.Legends;

namespace nzy3D.Plot3D.Rendering.Legends.Colorbars
{

	public class ColorbarLegend : Legend
	{

		internal ITickProvider _provider;
		internal ITickRenderer _renderer;
		internal System.Drawing.Size _minimumDimension;
		internal Color _foreground;

		internal Color _background;
		public ColorbarLegend(AbstractDrawable parent, IAxeLayout layout) : this(parent, layout, layout.MainColor, null)
		{
		}

		public ColorbarLegend(AbstractDrawable parent, IAxeLayout layout, Color foreground) : this(parent, layout.ZTickProvider, layout.ZTickRenderer, foreground, null)
		{
		}

		public ColorbarLegend(AbstractDrawable parent, IAxeLayout layout, Color foreground, Color background) : this(parent, layout.ZTickProvider, layout.ZTickRenderer, foreground, background)
		{
		}

		public ColorbarLegend(AbstractDrawable parent, ITickProvider provider, ITickRenderer renderer) : this(parent, provider, renderer, Color.BLACK, Color.WHITE)
		{
		}

		public ColorbarLegend(AbstractDrawable parent, ITickProvider provider, ITickRenderer renderer, Color foreground, Color background) : base(parent)
		{
			_provider = provider;
			_renderer = renderer;
			_foreground = foreground;
			_background = background;
			_minimumDimension = new System.Drawing.Size(ColorbarImageGenerator.MIN_BAR_WIDTH, ColorbarImageGenerator.MIN_BAR_HEIGHT);
		}

		public override void Render()
		{
			// gl.glClear(GL2.GL_COLOR_BUFFER_BIT | GL2.GL_DEPTH_BUFFER_BIT);
			GL.Enable(EnableCap.Blend);
			base.Render();
		}

		public override void DrawableChanged(Events.DrawableChangedEventArgs e)
		{
			if (e.What == Events.DrawableChangedEventArgs.FieldChanged.Color) {
				UpdateImage();
			}
		}

		public override System.Drawing.Bitmap toImage(int width, int height)
		{
            IMultiColorable mc = _parent as IMultiColorable;
            if (mc != null)
            {
				if (((mc.ColorMapper != null))) {
					// setup generator
					ColorbarImageGenerator bar = new ColorbarImageGenerator(mc.ColorMapper, _provider, _renderer);
					if (((_foreground != null))) {
						bar.ForegroundColor = _foreground;
					} else {
						bar.ForegroundColor = Color.BLACK;
					}
					if (((_background != null))) {
						bar.BackgroundColor = _background;
						bar.HasBackground = true;
					} else {
						bar.HasBackground = false;
					}
					// render @ given dimensions
					return bar.toImage(Math.Max(width - 25, 1), Math.Max(height - 25, 1));
				}
			}
			return null;
		}

		public override System.Drawing.Size MinimumSize {
			get { return _minimumDimension; }
		}

		public void setMinimumSize(System.Drawing.Size value)
		{
			_minimumDimension = value;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
