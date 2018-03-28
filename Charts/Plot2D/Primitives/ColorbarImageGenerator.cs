
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Colors;
using nzy3D.Plot3D.Primitives.Axes.Layout.Providers;
using nzy3D.Plot3D.Primitives.Axes.Layout.Renderers;
using nzy3D.Colors.ColorMaps;

namespace nzy3D.Plot2D.Primitive
{

	public class ColorbarImageGenerator
	{

		internal ColorMapper _mapper;
		internal ITickProvider _provider;
		internal ITickRenderer _renderer;
		internal float _min;
		internal float _max;
		internal bool _hasBackground = false;
		internal Color _backgroundColor;
		internal Color _foregroundColor = Color.BLACK;
		public static int MIN_BAR_WIDTH = 100;

		public static int MIN_BAR_HEIGHT = 100;

		public ColorbarImageGenerator(IColorMap map, float min, float max, ITickProvider provider, ITickRenderer renderer)
		{
			_mapper = new ColorMapper(map, min, max);
			_min = min;
			_max = max;
			_provider = provider;
			_renderer = renderer;
		}

        public ColorbarImageGenerator(ColorMapper mapper, ITickProvider provider, ITickRenderer renderer)
            : this(mapper.ColorMap, (float)mapper.ZMin, (float)mapper.ZMax, provider, renderer)
		{
		}

		public System.Drawing.Bitmap toImage(int width, int height)
		{
			return toImage(width, height, 20);
		}

		public System.Drawing.Bitmap toImage(int width, int height, int barWidth)
		{
			if ((barWidth > width)) {
				return null;
			}
			// Init image output
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(image);
			int txtSize = 12;
			// Draw background
			if (_hasBackground) {
                graphic.FillRectangle(new System.Drawing.SolidBrush(_backgroundColor.toColor()), 0, 0, width, height);
			}
			// Draw colorbar centering in half the Legend text height
			for (int h = txtSize / 2; h <= (height - txtSize / 2); h++) {
				// Compute value & color
				float v = _min + (_max - _min) * h / (height - txtSize);
				//			Color c = mapper.getColor(new Coord3d(0,0,v));
				Color c = _mapper.Color(v);
				//To allow the Color to be a variable independent of the coordinates
				// Draw line
                graphic.DrawLine(new System.Drawing.Pen(new System.Drawing.SolidBrush(c.toColor())), 0, height - h, barWidth, height - h);
			}
			// Contour of bar
            graphic.FillRectangle(new System.Drawing.SolidBrush(_foregroundColor.toColor()), 0, Convert.ToSingle(txtSize / 2), barWidth, height - txtSize);
			// Text annotation
			if (((_provider != null))) {
				float[] ticks = _provider.generateTicks(_min, _max);
				float ypos = 0;
				string txt = null;
				for (int t = 0; t <= ticks.Length - 1; t++) {
					//			ypos = (int)(height-height*((ticks[t]-min)/(max-min)));
					ypos = txtSize + (height - txtSize - (height - txtSize) * ((ticks[t] - _min) / (_max - _min)));
					//Making sure that the first and last tick appear in the colorbar
					txt = _renderer.Format(ticks[t]);
                    graphic.DrawString(txt, new System.Drawing.Font("Arial", txtSize, System.Drawing.GraphicsUnit.Pixel), new System.Drawing.SolidBrush(_foregroundColor.toColor()), barWidth + 1, ypos);
				}
			}
			return image;
		}

		public bool HasBackground {
			get { return _hasBackground; }
			set { _hasBackground = value; }
		}

		public Color BackgroundColor {
			get { return _backgroundColor; }
			set { _backgroundColor = value; }
		}

		public Color ForegroundColor {
			get { return _foregroundColor; }
			set { _foregroundColor = value; }
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
