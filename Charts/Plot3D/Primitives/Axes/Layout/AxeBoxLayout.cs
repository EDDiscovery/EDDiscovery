
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Colors;
using nzy3D.Plot3D.Primitives.Axes.Layout.Providers;
using nzy3D.Plot3D.Primitives.Axes.Layout.Renderers;

namespace nzy3D.Plot3D.Primitives.Axes.Layout
{

	public class AxeBoxLayout : IAxeLayout
	{

		internal string _xAxeLabel;
		internal string _yAxeLabel;
		internal string _zAxeLabel;
		internal bool _xAxeLabelDisplayed;
		internal bool _yAxeLabelDisplayed;
        internal bool _zAxeLabelDisplayed;
        internal bool _tickLineDisplayed;
		internal float[] _xTicks;
		internal float[] _yTicks;
		internal float[] _zTicks;
		internal ITickProvider _xTickProvider;
		internal ITickProvider _yTickProvider;
		internal ITickProvider _zTickProvider;
		internal ITickRenderer _xTickRenderer;
		internal ITickRenderer _yTickRenderer;
		internal ITickRenderer _zTickRenderer;
		internal Color _xTickColor;
		internal Color _yTickColor;
		internal Color _zTickColor;
		internal bool _xTickLabelDisplayed;
		internal bool _yTickLabelDisplayed;
		internal bool _zTickLabelDisplayed;
		internal bool _faceDisplayed;
		internal Color _quadColor;
		internal Color _gridColor;
		internal float _lastXmin = float.NaN;
		internal float _lastXmax = float.NaN;
		internal float _lastYmin = float.NaN;
		internal float _lastYmax = float.NaN;
		internal float _lastZmin = float.NaN;
		internal float _lastZmax = float.NaN;

		internal Color _mainColor;
		public AxeBoxLayout()
		{
			XAxeLabel = "X";
			YAxeLabel = "Y";
			ZAxeLabel = "Z";
			XAxeLabelDisplayed = true;
			YAxeLabelDisplayed = true;
			ZAxeLabelDisplayed = true;
			XTickProvider = new SmartTickProvider(5);
			YTickProvider = new SmartTickProvider(5);
			ZTickProvider = new SmartTickProvider(6);
			XTickRenderer = new DefaultDecimalTickRenderer(4);
			YTickRenderer = new DefaultDecimalTickRenderer(4);
			ZTickRenderer = new DefaultDecimalTickRenderer(6);
			FaceDisplayed = false;
			XTickLabelDisplayed = true;
            YTickLabelDisplayed = true;
            ZTickLabelDisplayed = true;
            TickLineDisplayed = true;
			MainColor = Color.BLACK;
		}

		/// <summary>
		/// Main axe box color.
		/// </summary>
		/// <value>Color to use</value>
		/// <returns>Main axe box color</returns>
		/// <remarks>When modified, grid and x/y/z ticks colors are also set to same color and quad color is set to negative of color</remarks>
		public Colors.Color MainColor {
			get { return _mainColor; }
			set {
				_mainColor = value;
				XTickColor = value;
				YTickColor = value;
				ZTickColor = value;
				GridColor = value;
				QuadColor = value.negativeColor();
			}
		}

		public float[] XTicks(float min, float max)
		{
			_lastXmin = min;
			_lastXmax = max;
			_xTicks = _xTickProvider.generateTicks(min, max);
			return _xTicks;
		}

		public float[] YTicks(float min, float max)
		{
			_lastYmin = min;
			_lastYmax = max;
			_yTicks = _yTickProvider.generateTicks(min, max);
			return _yTicks;
		}

		public float[] ZTicks(float min, float max)
		{
			_lastZmin = min;
			_lastZmax = max;
			_zTicks = _zTickProvider.generateTicks(min, max);
			return _zTicks;
		}

		public string XAxeLabel {
			get { return _xAxeLabel; }
			set { _xAxeLabel = value; }
		}

		public string YAxeLabel {
			get { return _yAxeLabel; }
			set { _yAxeLabel = value; }
		}

		public string ZAxeLabel {
			get { return _zAxeLabel; }
			set { _zAxeLabel = value; }
		}

		public float[] XTicks()
		{
			return _xTicks;
		}

		public float[] YTicks()
		{
			return _yTicks;
		}

		public float[] ZTicks()
		{
			return _zTicks;
		}

		public Providers.ITickProvider XTickProvider {
			get { return _xTickProvider; }
			set {
				_xTickProvider = value;
				// Update ticks if we can
				if (!float.IsNaN(_lastXmin)) {
					XTicks(_lastXmin, _lastXmax);
				}
			}
		}

		public Providers.ITickProvider YTickProvider {
			get { return _yTickProvider; }
			set {
				_yTickProvider = value;
				// Update ticks if we can
				if (!float.IsNaN(_lastYmin)) {
					YTicks(_lastYmin, _lastYmax);
				}
			}
		}

		public Providers.ITickProvider ZTickProvider {
			get { return _zTickProvider; }
			set {
				_zTickProvider = value;
				// Update ticks if we can
				if (!float.IsNaN(_lastZmin)) {
					ZTicks(_lastZmin, _lastZmax);
				}
			}
		}

		public Renderers.ITickRenderer XTickRenderer {
			get { return _xTickRenderer; }
			set { _xTickRenderer = value; }
		}

		public Renderers.ITickRenderer YTickRenderer {
			get { return _yTickRenderer; }
			set { _yTickRenderer = value; }
		}

		public Renderers.ITickRenderer ZTickRenderer {
			get { return _zTickRenderer; }
			set { _zTickRenderer = value; }
		}

		public Colors.Color XTickColor {
			get { return _xTickColor; }
			set { _xTickColor = value; }
		}

		public Colors.Color YTickColor {
			get { return _yTickColor; }
			set { _yTickColor = value; }
		}

		public Colors.Color ZTickColor {
			get { return _zTickColor; }
			set { _zTickColor = value; }
		}

		public bool FaceDisplayed {
			get { return _faceDisplayed; }
			set { _faceDisplayed = value; }
		}

		public Colors.Color QuadColor {
			get { return _quadColor; }
			set { _quadColor = value; }
		}

		public Colors.Color GridColor {
			get { return _gridColor; }
			set { _gridColor = value; }
		}

		public bool XAxeLabelDisplayed {
			get { return _xAxeLabelDisplayed; }
			set { _xAxeLabelDisplayed = value; }
		}

		public bool YAxeLabelDisplayed {
			get { return _yAxeLabelDisplayed; }
			set { _yAxeLabelDisplayed = value; }
		}

		public bool ZAxeLabelDisplayed {
			get { return _zAxeLabelDisplayed; }
			set { _zAxeLabelDisplayed = value; }
		}

		public bool XTickLabelDisplayed {
			get { return _xTickLabelDisplayed; }
			set { _xTickLabelDisplayed = value; }
		}

		public bool YTickLabelDisplayed {
			get { return _yTickLabelDisplayed; }
			set { _yTickLabelDisplayed = value; }
		}

		public bool ZTickLabelDisplayed {
			get { return _zTickLabelDisplayed; }
			set { _zTickLabelDisplayed = value; }
		}

        public bool TickLineDisplayed
        {
            get { return _tickLineDisplayed; }
            set { _tickLineDisplayed = value; }
        }

    }

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
