
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Plot3D.Primitives;
using nzy3D.Plot3D.Primitives.Axes;
using nzy3D.Plot3D.Rendering.Canvas;
using nzy3D.Plot3D.Rendering.Scene;
using nzy3D.Plot3D.Rendering.View;
using nzy3D.Plot3D.Rendering.View.Modes;
using nzy3D.Maths;
using nzy3D.Chart.Controllers.Camera;
using nzy3D.Plot3D.Primitives.Axes.Layout;


namespace nzy3D.Chart
{

	public class Chart
	{

		protected ChartScene _scene;
		protected View _view;
		protected ICanvas _canvas;
		protected Coord3d _previousViewPointFree;
		protected Coord3d _previousViewPointTop;
		protected Coord3d _previousViewPointProfile;
		protected List<AbstractCameraController> _controllers;
		//protected  capabilities As GLCapabilities



		public static Quality DEFAULT_QUALITY = Quality.Intermediate;
		public Chart(ICanvas canvas) : this(canvas, DEFAULT_QUALITY)
		{
		}

		public Chart(ICanvas canvas, Quality quality)
		{
			// Store canvas
			this._canvas = canvas;
			// Set up controllers
			_controllers = new List<AbstractCameraController>();
			// Set up scene 
			_scene = initializeScene(quality.AlphaActivated);
			// Set up view
			_view = _scene.newView(canvas, quality);
			// create view with links in scene and canvas
			_view.BackgroundColor = Colors.Color.WHITE;
		}


		/// <summary>
		///     Provides a concrete scene. This method shoud be overriden to inject a custom scene,
		/// which may rely on several views, and could enhance manipulation of scene graph.
		/// </summary>
		protected virtual ChartScene initializeScene(bool graphsort)
		{
			return Factories.SceneFactory.getInstance(graphsort);
		}

		public void Clear()
		{
			_scene.Clear();
			_view.Shoot();
		}

		public void Dispose()
		{
			clearControllerList();
			_scene.Dispose();
			_canvas = null;
			_scene = null;
		}

		public void Render()
		{
			_view.Shoot();
		}

		public System.Drawing.Bitmap Screenshot()
		{
            return _canvas.Screenshot();
		}

		public void updateProjectionsAndRender()
		{
			_view.Shoot();
			_view.Project();
			Render();
		}

		/// <summary>
		/// Add a <see cref="AbstractCameraController"/> to this <see cref="Chart"/>.
		/// Warning: the <see cref="Chart"/> is not the owner of the controller. Disposing
		/// the chart thus just unregisters the controllers, but does not handle
		/// stopping and disposing controllers.
		/// </summary>
		public void addController(AbstractCameraController controller)
		{
			controller.Register(this);
			_controllers.Add(controller);
		}

		public void removeController(AbstractCameraController controller)
		{
			controller.Unregister(this);
			_controllers.Remove(controller);
		}

		public void clearControllerList()
		{
			foreach (AbstractCameraController controller in _controllers) {
				controller.Unregister(this);
			}
			_controllers.Clear();
		}

		public IEnumerable<AbstractCameraController> getControllers()
		{
            return _controllers;
		}

		public void addDrawable(AbstractDrawable drawable)
		{
			_scene.Graph.Add(drawable);
		}

		public void addDrawable(AbstractDrawable drawable, bool updateViews)
		{
			_scene.Graph.Add(drawable, updateViews);
		}

		public void addDrawable(IEnumerable<AbstractDrawable> drawables, bool updateViews)
		{
			_scene.Graph.Add(drawables, updateViews);
		}

		public void addDrawable(IEnumerable<AbstractDrawable> drawables)
		{
			_scene.Graph.Add(drawables);
		}

		public void removeDrawable(AbstractDrawable drawable)
		{
			_scene.Graph.Remove(drawable);
		}

		public void removeDrawable(AbstractDrawable drawable, bool updateViews)
		{
			_scene.Graph.Remove(drawable, updateViews);
		}

		public void addRenderer(IRenderer2D renderer2d)
		{
			_view.addRenderer2d(renderer2d);
		}

		public void removeRenderer(IRenderer2D renderer2d)
		{
			_view.removeRenderer2d(renderer2d);
		}

		public View View {
			get { return _view; }
		}

		public ChartScene Scene {
			get { return _scene; }
		}

		public ICanvas Canvas {
			get { return _canvas; }
		}

		public IAxeLayout AxeLayout {
			get { return _view.Axe.getLayout(); }
		}

		public bool AxeDisplayed {
			set {
				_view.AxeBoxDisplayed = value;
				_view.Shoot();
			}
		}

		public Coord3d Viewpoint {
			get { return _view.ViewPoint; }
			set {
				_view.ViewPoint = value;
				_view.Shoot();
			}
		}

		public ViewPositionMode ViewMode {
			get { return _view.ViewMode; }
			set {
				// Store current view mode and view point in memory
				ViewPositionMode previous = View.ViewMode;
				switch (previous) {
					case ViewPositionMode.FREE:
						_previousViewPointFree = View.ViewPoint;
						break;
					case ViewPositionMode.PROFILE:
						_previousViewPointTop = View.ViewPoint;
						break;
					case ViewPositionMode.TOP:
						_previousViewPointProfile = View.ViewPoint;
						break;
					default:
						throw new Exception("Unsupported ViewPositionMode :" + previous);
				}
				// Set new view mode and former view point
				_view.ViewMode = value;
				switch (previous) {
					case ViewPositionMode.FREE:
						_view.ViewPoint = ((_previousViewPointFree == null) ? View.DEFAULT_VIEW.Clone() : _previousViewPointFree);
						break;
					case ViewPositionMode.PROFILE:
                        _view.ViewPoint = ((_previousViewPointTop == null) ? View.DEFAULT_VIEW.Clone() : _previousViewPointTop);
						break;
					case ViewPositionMode.TOP:
                        _view.ViewPoint = ((_previousViewPointProfile == null) ? View.DEFAULT_VIEW.Clone() : _previousViewPointProfile);
						break;
					default:
						throw new Exception("Unsupported ViewPositionMode :" + previous);
				}
				_view.Shoot();
			}
		}

		public Scale Scale {
			get { return new Scale(_view.Bounds.zmin, _view.Bounds.zmax); }
			set { _view.setScale(value, true); }
		}

		public void setScale(Scale scale, bool notify)
		{
			_view.setScale(scale, notify);
		}

		public float Flip(float y)
		{
			return _canvas.RendererHeight - y;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
