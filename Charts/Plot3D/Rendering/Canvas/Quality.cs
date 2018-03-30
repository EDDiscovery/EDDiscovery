
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Plot3D.Rendering.Canvas
{

	/// <summary>
	/// Provides a structure for setting the rendering quality, i.e., the tradeoff
	/// between computation speed, and graphic quality. Following mode have an impact
	/// on the way the {@link View} makes its GL2 initialization.
	/// The {@link Quality} may also activate an {@link AbstractOrderingStrategy} algorithm
	/// that enables clean alpha results.
	///
	/// Fastest:
	/// - No transparency, no color shading, just handle depth buffer.
	///
	/// Intermediate:
	/// - include Fastest mode abilities
	/// - Color shading, mainly usefull to have interpolated colors on polygons.
	///
	/// Advanced:
	/// - include Intermediate mode abilities
	/// - Transparency (GL2 alpha blending + polygon ordering in scene graph)
	///
	/// Nicest:
	/// - include Advanced mode abilities
	/// - Anti aliasing on wires
	///
	///
	/// Toggling rendering model: one may either choose to have a repaint-on-demand
	/// or repaint-continuously model. Setting isAnimated(false) will desactivate a
	/// the {@link Animator} updating the choosen {@link ICanvas} implementation.
	///
	/// setAutoSwapBuffer(false) will equaly configure the {@link ICanvas}.
	///
	/// @author Martin Pernollet
	/// </summary>
	/// <remarks></remarks>
	public class Quality
	{

		private bool _depthActivated;
		private bool _alphaActivated;
		private bool _smoothColor;
		private bool _smoothPoint;
		private bool _smoothLine;
		private bool _smoothPolygon;
		internal bool _disableDepthBufferWhenAlpha;
		internal bool _isAnimated = true;

		internal bool _isAutoSwapBuffer = true;
		public Quality(bool depthActivated, bool alphaActivated, bool smoothColor, bool smoothPoint, bool smoothLine, bool smoothPolygon, bool disableDepth)
		{
			_depthActivated = depthActivated;
			_alphaActivated = alphaActivated;
			_smoothColor = smoothColor;
			_smoothPoint = smoothPoint;
			_smoothLine = smoothLine;
			_smoothPolygon = smoothPolygon;
			_disableDepthBufferWhenAlpha = disableDepth;
		}

		public bool DepthActivated {
			get { return _depthActivated; }
			set { _depthActivated = value; }
		}

		public bool AlphaActivated {
			get { return _alphaActivated; }
			set { _alphaActivated = value; }
		}

		public bool SmoothColor {
			get { return _smoothColor; }
			set { _smoothColor = value; }
		}

		public bool SmoothLine {
			get { return _smoothLine; }
			set { _smoothLine = value; }
		}

		public bool SmoothEdge {
			get { return _smoothLine; }
			set { _smoothLine = value; }
		}

		public bool SmoothPoint {
			get { return _smoothPoint; }
			set { _smoothPoint = value; }
		}

		public bool SmoothPolygon {
			get { return _smoothPolygon; }
			set { _smoothPolygon = value; }
		}

		public bool DisableDepthBufferWhenAlpha {
			get { return _disableDepthBufferWhenAlpha; }
			set { _disableDepthBufferWhenAlpha = value; }
		}

		public bool IsAnimated {
			get { return _isAnimated; }
			set { _isAnimated = value; }
		}

		public bool IsAutoSwapBuffer {
			get { return _isAutoSwapBuffer; }
			set { _isAutoSwapBuffer = value; }
		}

		/// <summary>
		/// Enables alpha, color interpolation and antialiasing on lines, points, and polygons.
		/// </summary>

		public static Quality Nicest = new Quality(true, true, true, true, true, true, true);
		/// <summary>
		/// Enables alpha and color interpolation.
		/// </summary>

		public static Quality Advanced = new Quality(true, true, true, false, false, false, true);
		/// <summary>
		/// Enables color interpolation.
		/// </summary>

		public static Quality Intermediate = new Quality(true, false, true, false, false, false, true);
		/// <summary>
		/// Minimal quality to allow fastest rendering (no alpha, interpolation or antialiasing).
		/// </summary>

		public static Quality Fastest = new Quality(true, false, false, false, false, false, true);

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
