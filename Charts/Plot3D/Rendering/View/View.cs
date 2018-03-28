
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using nzy3D.Chart;
using nzy3D.Colors;
using nzy3D.Events;
using nzy3D.Factories;
using nzy3D.Maths;
using nzy3D.Plot3D.Primitives;
using nzy3D.Plot3D.Primitives.Axes;
using nzy3D.Plot3D.Rendering;
using nzy3D.Plot3D.Rendering.Canvas;
using nzy3D.Plot3D.Rendering.View.Modes;
using nzy3D.Plot3D.Transform;

namespace nzy3D.Plot3D.Rendering.View
{

	public class View
	{

		public static float STRETCH_RATIO = 0.25f;
			// force to have all object maintained in screen, meaning axebox won't always keep the same size.
		internal bool MAINTAIN_ALL_OBJECTS_IN_VIEW = false;
			// display a magenta parallelepiped (debug)
		internal bool DISPLAY_AXE_WHOLE_BOUNDS = false;
		internal bool _axeBoxDisplayed = true;
		internal bool _squared = true;
		internal Camera _cam;
		internal IAxe _axe;
		internal Quality _quality;
		// TODO : Implement overlay
		// Friend _overlay As Overlay 
		internal Scene _scene;
		internal ICanvas _canvas;
		internal Coord3d _viewpoint;
		internal Coord3d _center;
		internal Coord3d _scaling;
		internal BoundingBox3d _viewbounds;
		internal CameraMode _cameraMode;
		internal ViewPositionMode _viewmode;
		internal ViewBoundMode _boundmode;
		internal ImageViewport _bgViewport;
		internal System.Drawing.Bitmap _bgImg = null;
		internal BoundingBox3d _targetBox;
		internal Color _bgColor = Color.BLACK;
		internal Color _bgOverlay = new Color(0, 0, 0, 0);
		// TODO : Implement overlay
		//Friend _tooltips As List(Of ITooltipRenderer) 
		internal List<IRenderer2D> _renderers;
		internal List<IViewPointChangedListener> _viewPointChangedListeners;
		internal List<IViewIsVerticalEventListener> _viewOnTopListeners;
		internal bool _wasOnTopAtLastRendering;
		static internal float PI_div2 = Convert.ToSingle(System.Math.PI / 2);
		public static Coord3d DEFAULT_VIEW = new Coord3d(System.Math.PI / 3, System.Math.PI / 3, 2000);
		internal bool _dimensionDirty = false;
		internal bool _viewDirty = false;

		static internal View Current;
		public View(Scene scene, ICanvas canvas, Quality quality)
		{
			BoundingBox3d sceneBounds = scene.Graph.Bounds;
            _viewpoint = (Coord3d)DEFAULT_VIEW.Clone();
            _center = (Coord3d)sceneBounds.getCenter();
            _scaling = (Coord3d)Coord3d.IDENTITY.Clone();
			_viewmode = ViewPositionMode.FREE;
			_boundmode = ViewBoundMode.AUTO_FIT;
			_cameraMode = CameraMode.ORTHOGONAL;
			_axe = (IAxe)AxeFactory.getInstance(sceneBounds, this);
			_cam = CameraFactory.getInstance(_center);
			_scene = scene;
			_canvas = canvas;
			_quality = quality;
			_renderers = new List<IRenderer2D>();
			//_tooltips = New List(Of ITooltipRenderer)
			_bgViewport = new ImageViewport();
			_viewOnTopListeners = new List<IViewIsVerticalEventListener>();
			_viewPointChangedListeners = new List<IViewPointChangedListener>();
			_wasOnTopAtLastRendering = false;
			//_overlay = New Overlay
			View.Current = this;
		}

		public void Dispose()
		{
			_axe.Dispose();
			_cam = null;
			_renderers.Clear();
			_viewOnTopListeners.Clear();
			_scene = null;
			_canvas = null;
			_quality = null;
		}

		public void Shoot()
		{
			_canvas.ForceRepaint();
		}

		public void Project()
		{
			_scene.Graph.Project(_cam);
		}

		public Coord3d ProjectMouse(int x, int y)
		{
			return _cam.ScreenToModel(new Coord3d(x, y, 0));
		}

		#region "GENERAL DISPLAY CONTROLS"

		public void Rotate(Coord2d move)
		{
			Rotate(move, true);
		}

		public void Rotate(Coord2d move, bool updateView)
		{
			Coord3d eye = this.ViewPoint;
			eye.x -= move.x;
			eye.y += move.y;
			setViewPoint(eye, updateView);
			//fireControllerEvent(ControllerType.ROTATE, eye);
		}

		public void Shift(float factor)
		{
			Shift(factor, true);
		}

		public void Shift(float factor, bool updateView)
		{
			nzy3D.Maths.Scale current = this.Scale;
			nzy3D.Maths.Scale newScale = current.@add(factor * current.Range);
			setScale(newScale, updateView);
			//fireControllerEvent(ControllerType.SHIFT, newScale);
		}

		public void Zoom(float factor)
		{
			Zoom(factor, true);
		}

		public void Zoom(float factor, bool updateView)
		{
			nzy3D.Maths.Scale current = this.Scale;
			double range = current.Max - current.Min;
			if (range <= 0) {
				return;
			}
			double center = (current.Max + current.Min) / 2;
			double zmin = center + (current.Min - center) * factor;
			double zmax = center + (current.Max - center) * factor;
			// set min/max according to bounds
			nzy3D.Maths.Scale scale = null;
			if ((zmin < zmax)) {
				scale = new nzy3D.Maths.Scale(zmin, zmax);
			} else {
				// forbid to have zmin = zmax if we zoom in
				if ((factor < 1)) {
					scale = new nzy3D.Maths.Scale(center, center);
				}
			}
			if ((scale != null)) {
				setScale(scale, updateView);
				// fireControllerEvent(ControllerType.ZOOM, scale);
			}
		}

		public void ZoomX(float factor)
		{
			ZoomX(factor, true);
		}

		public void ZoomX(float factor, bool updateView)
		{
			double range = this.Bounds.xmax - this.Bounds.xmin;
			if (range <= 0) {
				return;
			}
			double center = (this.Bounds.xmax + this.Bounds.xmin) / 2;
			double min = center + (this.Bounds.xmin - center) * factor;
			double max = center + (this.Bounds.xmax - center) * factor;
			// set min/max according to bounds
			nzy3D.Maths.Scale scale = null;
			if ((min < max)) {
				scale = new nzy3D.Maths.Scale(min, max);
			} else {
				// forbid to have min = max if we zoom in
				if ((factor < 1)) {
					scale = new nzy3D.Maths.Scale(center, center);
				}
			}
			if ((scale != null)) {
				BoundingBox3d bounds = this.Bounds;
				bounds.xmin = scale.Min;
				bounds.xmax = scale.Max;
				this.BoundManual = bounds;
				if (updateView) {
					Shoot();
				}
				// fireControllerEvent(ControllerType.ZOOM, scale);
			}
		}

		public void ZoomY(float factor)
		{
			ZoomY(factor, true);
		}

		public void ZoomY(float factor, bool updateView)
		{
			double range = this.Bounds.ymax - this.Bounds.ymin;
			if (range <= 0) {
				return;
			}
			double center = (this.Bounds.ymax + this.Bounds.ymin) / 2;
			double min = center + (this.Bounds.ymin - center) * factor;
			double max = center + (this.Bounds.ymax - center) * factor;
			// set min/max according to bounds
			nzy3D.Maths.Scale scale = null;
			if ((min < max)) {
				scale = new nzy3D.Maths.Scale(min, max);
			} else {
				// forbid to have min = max if we zoom in
				if ((factor < 1)) {
					scale = new nzy3D.Maths.Scale(center, center);
				}
			}
			if ((scale != null)) {
				BoundingBox3d bounds = this.Bounds;
				bounds.ymin = scale.Min;
				bounds.ymax = scale.Max;
				this.BoundManual = bounds;
				if (updateView) {
					Shoot();
				}
				// fireControllerEvent(ControllerType.ZOOM, scale);
			}
		}

		public void ZoomZ(float factor)
		{
			ZoomZ(factor, true);
		}

		public void ZoomZ(float factor, bool updateView)
		{
			double range = this.Bounds.zmax - this.Bounds.zmin;
			if (range <= 0) {
				return;
			}
			double center = (this.Bounds.zmax + this.Bounds.zmin) / 2;
			double min = center + (this.Bounds.zmin - center) * factor;
			double max = center + (this.Bounds.zmax - center) * factor;
			// set min/max according to bounds
			nzy3D.Maths.Scale scale = null;
			if ((min < max)) {
				scale = new nzy3D.Maths.Scale(min, max);
			} else {
				// forbid to have min = max if we zoom in
				if ((factor < 1)) {
					scale = new nzy3D.Maths.Scale(center, center);
				}
			}
			if ((scale != null)) {
				BoundingBox3d bounds = this.Bounds;
				bounds.zmin = scale.Min;
				bounds.zmax = scale.Max;
				this.BoundManual = bounds;
				if (updateView) {
					Shoot();
				}
				// fireControllerEvent(ControllerType.ZOOM, scale);
			}
		}

		public bool DimensionDirty {
			get { return _dimensionDirty; }
			set { _dimensionDirty = value; }
		}

		public nzy3D.Maths.Scale Scale {
			get { return new nzy3D.Maths.Scale(this.Bounds.zmin, this.Bounds.zmax); }
			set { setScale(value, true); }
		}

		public void setScale(nzy3D.Maths.Scale scale, bool notify)
		{
			BoundingBox3d bounds = this.Bounds;
			bounds.zmin = scale.Min;
			bounds.zmax = scale.Max;
			this.BoundManual = bounds;
			if (notify) {
				Shoot();
			}
		}

		/// <summary>
		/// Set the surrounding AxeBox dimensions and the Camera target, and the
		/// colorbar range.
		/// </summary>
		public void lookToBox(BoundingBox3d box)
		{
            _center = box.getCenter();
			_axe.setAxe(box);
			_targetBox = box;
		}

		/// <summary>
		/// Get the <see cref="AxeBox"/>'s bounds
		/// </summary>
		public BoundingBox3d Bounds {
			get { return _axe.getBoxBounds(); }
		}

		public ViewBoundMode BoundsMode {
			get { return _boundmode; }
		}

		/// <summary>
		/// Set the ViewPositionMode applied to this view.
		/// </summary>
		public ViewPositionMode ViewMode {
			get { return _viewmode; }
			set { _viewmode = value; }
		}

		public Coord3d ViewPoint {
			get { return _viewpoint; }
			set { setViewPoint(value, true); }
		}

		public void setViewPoint(Coord3d polar, bool updateView)
		{
			_viewpoint = polar;
			_viewpoint.y = (_viewpoint.y < -PI_div2 ? -PI_div2 : _viewpoint.y);
			_viewpoint.y = (_viewpoint.y > PI_div2 ? PI_div2 : _viewpoint.y);
			if (updateView) {
				Shoot();
			}
			fireViewPointChangedEvent(new ViewPointChangedEventArgs(this, polar));
		}

		public Coord3d getLastViewScaling()
		{
			return _scaling;
		}

		public IAxe Axe {
			get { return _axe; }
			set {
				_axe = value;
				updateBounds();
			}
		}

		public bool Squared {
			get { return _squared; }
			set { _squared = value; }
		}

		public bool AxeBoxDisplayed {
			get { return _axeBoxDisplayed; }
			set { _axeBoxDisplayed = value; }
		}

		public Color BackgroundColor {
			get { return _bgColor; }
			set { _bgColor = value; }
		}

		public System.Drawing.Bitmap BackgroundImage {
			get { return _bgImg; }
			set {
				_bgImg = value;
				_bgViewport.Image = _bgImg;
			}
		}

		public Camera Camera {
			get { return _cam; }
		}

		/// <summary>
		/// Get the projection of this view, either CameraMode.ORTHOGONAL or CameraMode.PERSPECTIVE.
		/// </summary>
		public CameraMode CameraMode {
			get { return _cameraMode; }
			set { _cameraMode = value; }
		}

		public bool Maximized {
			get { return _cam.StretchToFill; }
			set { _cam.StretchToFill = value; }
		}

		public Scene Scene {
			get { return _scene; }
		}

		public System.Drawing.Rectangle SceneViewportRectangle {
			get { return _cam.Rectange; }
		}

		public ICanvas Canvas {
			get { return _canvas; }
		}

		public void addRenderer2d(IRenderer2D renderer)
		{
			_renderers.Add(renderer);
		}

		public void removeRenderer2d(IRenderer2D renderer)
		{
			_renderers.Remove(renderer);
		}

		public void addViewOnTopEventListener(IViewIsVerticalEventListener listener)
		{
			_viewOnTopListeners.Add(listener);
		}

		public void removeViewOnTopEventListener(IViewIsVerticalEventListener listener)
		{
			_viewOnTopListeners.Remove(listener);
		}

		internal void fireViewOnTopEvent(bool isOnTop)
		{
			ViewIsVerticalEventArgs e = new ViewIsVerticalEventArgs(this);
			if (isOnTop) {
				foreach (IViewIsVerticalEventListener listener in _viewOnTopListeners) {
					listener.ViewVerticalReached(e);
				}
			} else {
				foreach (IViewIsVerticalEventListener listener in _viewOnTopListeners) {
					listener.ViewVerticalLeft(e);
				}
			}
		}

		public void addViewPointChangedListener(IViewPointChangedListener listener)
		{
			_viewPointChangedListeners.Add(listener);
		}

        public void removeViewPointChangedListener(IViewPointChangedListener listener)
		{
			_viewPointChangedListeners.Remove(listener);
		}

		internal void fireViewPointChangedEvent(ViewPointChangedEventArgs e)
		{
			foreach (IViewPointChangedListener vp in _viewPointChangedListeners) {
				vp.ViewPointChanged(e);
			}
		}

		/// <summary>
		/// Select between an automatic bounding (that allows fitting the entire scene graph), or a custom bounding.
		/// </summary>
		public ViewBoundMode BoundMode {
			set {
				_boundmode = value;
				updateBounds();
			}
		}

		/// <summary>
		/// Set the bounds of the view according to the current BoundMode, and orders a Camera.shoot().
		/// </summary>
		public void updateBounds()
		{
			switch (_boundmode) {
				case ViewBoundMode.AUTO_FIT:
					lookToBox(Scene.Graph.Bounds);
					// set axe and camera
					break;
				case ViewBoundMode.MANUAL:
					lookToBox(_viewbounds);
					// set axe and camera
					break;
				default:
					throw new Exception("Unsupported bound mode : " + _boundmode);
			}
			Shoot();
		}

		/// <summary>
		/// Update the bounds according to the scene graph whatever is the current
		/// BoundMode, and orders a shoot() if refresh is True
		/// </summary>
		/// <param name="refresh">Wether to order a shoot() or not.</param>
		/// <remarks></remarks>
		public void updateBoundsForceUpdate(bool refresh)
		{
			lookToBox(Scene.Graph.Bounds);
			if (refresh) {
				Shoot();
			}
		}

		/// <summary>
		/// Set a manual bounding box and switch the bounding mode to
		/// ViewBoundMode.MANUAL, meaning that any call to updateBounds()
		/// will update view bounds to the current bounds.
		/// </summary>
		/// <value></value>
		/// <remarks>The camero.shoot is not called in this case</remarks>
		public BoundingBox3d BoundManual {
			set {
				_viewbounds = value;
				_boundmode = ViewBoundMode.MANUAL;
				lookToBox(_viewbounds);
			}
		}

		/// <summary>
		/// Return a 3d scaling factor that allows scaling the scene into a square
		/// box, according to the current ViewBoundMode.
		/// <p/>
		/// If the scene bounds are Infinite, NaN or zero, for a given dimension, the
		/// scaler will be set to 1 on the given dimension.
		///
		/// @return a scaling factor for each dimension.
		/// </summary>
		internal Coord3d Squarify()
		{
			// Get the view bounds
			BoundingBox3d bounds = default(BoundingBox3d);
			switch (_boundmode) {
				case ViewBoundMode.AUTO_FIT:
					bounds = Scene.Graph.Bounds;
					break;
				case ViewBoundMode.MANUAL:
					bounds = _viewbounds;
					break;
				default:
					throw new Exception("Unsupported bound mode : " + _boundmode);
			}
			// Compute factors
			float xLen = (float)(bounds.xmax - bounds.xmin);
			float yLen = (float)(bounds.ymax - bounds.ymin);
			float zLen = (float)(bounds.zmax - bounds.zmin);
			float lmax = Math.Max(Math.Max(xLen, yLen), zLen);
			if (float.IsInfinity(xLen) | float.IsNaN(xLen) | xLen == 0) {
				xLen = 1;
				// throw new ArithmeticException("x scale is infinite, nan or 0");
			}
			if (float.IsInfinity(yLen) | float.IsNaN(yLen) | yLen == 0) {
				yLen = 1;
				// throw new ArithmeticException("y scale is infinite, nan or 0");
			}
			if (float.IsInfinity(zLen) | float.IsNaN(zLen) | zLen == 0) {
				zLen = 1;
				// throw new ArithmeticException("z scale is infinite, nan or 0");
			}
			if (float.IsInfinity(lmax) | float.IsNaN(lmax) | lmax == 0) {
				lmax = 1;
				// throw new ArithmeticException("lmax is infinite, nan or 0");
			}
			return new Coord3d(lmax / xLen, lmax / yLen, lmax / zLen);
		}


		#endregion

		#region "GL2"

		/// <summary>
		/// The init function specifies general GL settings that impact the rendering
		/// quality and performance (computation speed).
		/// <p/>
		/// The rendering settings are set by the Quality instance given in
		/// the constructor parameters.
		/// </summary>
		/// <remarks></remarks>
		public void Init()
		{
			InitQuality();
			InitLights();
		}

		public void InitQuality()
		{
			// Activate Depth buffer
			if (_quality.DepthActivated) {
				GL.Enable(EnableCap.DepthTest);
				GL.DepthFunc(DepthFunction.Lequal);
			} else {
				GL.Disable(EnableCap.DepthTest);
			}
			// Blending
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			// on/off is handled by each viewport (camera or image)
			// Activate transparency
			if (_quality.AlphaActivated) {
				GL.Enable(EnableCap.AlphaTest);
				if (_quality.DisableDepthBufferWhenAlpha) {
					GL.Disable(EnableCap.DepthTest);
				}
			} else {
				GL.Disable(EnableCap.AlphaTest);
			}
			// Make smooth colors for polygons (interpolate color between points)
			if (_quality.SmoothColor) {
				GL.ShadeModel(ShadingModel.Smooth);
			} else {
				GL.ShadeModel(ShadingModel.Flat);
			}
			// Make smoothing setting
			if (_quality.SmoothLine) {
				GL.Enable(EnableCap.LineSmooth);
				GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
			} else {
				GL.Disable(EnableCap.LineSmooth);
			}
			if (_quality.SmoothPoint) {
				GL.Enable(EnableCap.PointSmooth);
				GL.Hint(HintTarget.PointSmoothHint, HintMode.Fastest);
			} else {
				GL.Disable(EnableCap.PointSmooth);
			}
		}

		public void InitLights()
		{
			// Init light
			Scene.LightSet.Init();
			Scene.LightSet.Enable();
		}

		// Clear color and depth buffer (same as ClearColorAndDepth)
		public void Clear()
		{
			ClearColorAndDepth();
		}

		// Clear color and depth buffer (same as Clear)
		public void ClearColorAndDepth()
		{
            GL.ClearColor((float)_bgColor.r, (float)_bgColor.g, (float)_bgColor.b, (float)_bgColor.a);
			// clear with background
			GL.ClearDepth(1);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}

		public virtual void Render()
		{
			RenderBackground(0, 1);
			RenderScene();
			RenderOverlay();
			if (_dimensionDirty) {
				_dimensionDirty = false;
			}
		}

		public void RenderBackground(float left, float right)
		{
			if ((_bgImg != null)) {
				_bgViewport.SetViewPort(_canvas.RendererWidth, _canvas.RendererHeight, left, right);
				_bgViewport.Render();
			}
		}

		public void RenderBackground(ViewPort viewport)
		{
			if ((_bgImg != null)) {
				_bgViewport.SetViewPort(viewport);
				_bgViewport.Render();
			}
		}

		public void RenderScene()
		{
			RenderScene(new ViewPort(_canvas.RendererWidth, _canvas.RendererHeight));
		}

		public void RenderScene(float left, float right)
		{
            RenderScene(new ViewPort(_canvas.RendererWidth, _canvas.RendererHeight, (int)left, (int)right));
		}

		public void RenderScene(ViewPort viewport)
		{
			UpdateQuality();
			UpdateCamera(viewport, computeScaling());
			RenderAxeBox();
			RenderSceneGraph();
		}

		public void UpdateQuality()
		{
			if (_quality.AlphaActivated) {
				GL.Enable(EnableCap.Blend);
			} else {
				GL.Disable(EnableCap.Blend);
			}
		}

		public BoundingBox3d computeScaling()
		{
			//-- Scale the scene's view -------------------
			if (Squared) {
				_scaling = Squarify();
			} else {
				_scaling = (Coord3d)Coord3d.IDENTITY.Clone();
			}
			// Compute the bounds for computing cam distance, clipping planes, etc ...
			if ((_targetBox == null)) {
				_targetBox = new BoundingBox3d(0, 1, 0, 1, 0, 1);
			}
			BoundingBox3d boundsScaled = new BoundingBox3d();
			boundsScaled.Add(_targetBox.scale(_scaling));
			if (MAINTAIN_ALL_OBJECTS_IN_VIEW) {
				boundsScaled.Add(Scene.Graph.Bounds.scale(_scaling));
			}
			return boundsScaled;
		}

		public void UpdateCamera(ViewPort viewport, BoundingBox3d boundsScaled)
		{
			UpdateCamera(viewport, boundsScaled, (float)boundsScaled.getRadius());
		}

		public void UpdateCamera(ViewPort viewport, BoundingBox3d boundsScaled, float sceneRadiusScaled)
		{
			Coord3d target = _center.multiply(_scaling);
			Coord3d eye = default(Coord3d);
			_viewpoint.z = sceneRadiusScaled * 2;
			// maintain a reasonnable distance to the scene for viewing it
			switch (_viewmode) {
				case Modes.ViewPositionMode.FREE:
					eye = _viewpoint.cartesian().@add(target);
					break;
				case Modes.ViewPositionMode.TOP:
					eye = _viewpoint;
					eye.x = -PI_div2;
					// on x
					eye.y = PI_div2;
					// on top
                    eye = eye.cartesian().@add(target);
					break;
				case Modes.ViewPositionMode.PROFILE:
					eye = _viewpoint;
					eye.y = 0;
                    eye = eye.cartesian().@add(target);
					break;
				default:
					throw new Exception("Unsupported viewmode : " + _viewmode);
			}
			Coord3d up = default(Coord3d);
			if (Math.Abs(_viewpoint.y) == PI_div2) {
				// handle up vector
                Coord2d direction = new Coord2d(_viewpoint.x, _viewpoint.y).cartesian();
				if (_viewpoint.y > 0) {
					// on top
					up = new Coord3d(-direction.x, -direction.y, 0);
				} else {
					up = new Coord3d(direction.x, direction.y, 0);
				}
				// handle "on-top" events
				if (!_wasOnTopAtLastRendering) {
					_wasOnTopAtLastRendering = true;
					fireViewOnTopEvent(true);
				}
			} else {
				// handle up vector
				up = new Coord3d(0, 0, 1);
				// handle "on-top" events
				if (_wasOnTopAtLastRendering) {
					_wasOnTopAtLastRendering = false;
					fireViewOnTopEvent(false);
				}
			}
			// Apply camera settings
			_cam.Target = target;
			_cam.Up = up;
			_cam.Eye = eye;
			// Set rendering volume
			if (_viewmode == Modes.ViewPositionMode.TOP) {
				_cam.RenderingSphereRadius = (float)(Math.Max(boundsScaled.xmax - boundsScaled.xmin, boundsScaled.ymax - boundsScaled.ymin) / 2);
				// correctCameraPositionForIncludingTextLabels(viewport) ' quite experimental !
			} else {
				_cam.RenderingSphereRadius = sceneRadiusScaled;
			}
			// Setup camera (i.e. projection matrix)
			//cam.setViewPort(canvas.getRendererWidth(),
			// canvas.getRendererHeight(), left, right);
			_cam.SetViewPort(viewport);
			_cam.shoot(_cameraMode);
		}

		public void RenderAxeBox()
		{
			if (_axeBoxDisplayed) {
				GL.MatrixMode(MatrixMode.Modelview);
				_scene.LightSet.Disable();
				_axe.setScale(_scaling);
				_axe.Draw(_cam);
				// for debug
				if (DISPLAY_AXE_WHOLE_BOUNDS) {
					AxeBox abox = (AxeBox)_axe;
					BoundingBox3d box = abox.WholeBounds;
					Parallelepiped p = new Parallelepiped(box);
					p.FaceDisplayed = false;
					p.WireframeColor = Color.MAGENTA;
					p.WireframeDisplayed = true;
					p.Draw(_cam);
				}
				_scene.LightSet.Enable();
			}
		}

		public void RenderSceneGraph()
		{
			RenderSceneGraph(true);
		}

		public void RenderSceneGraph(bool light)
		{
			if (light) {
				Scene.LightSet.apply(_scaling);
				// gl.glEnable(GL2.GL_LIGHTING);
				// gl.glEnable(GL2.GL_LIGHT0);
				// gl.glDisable(GL2.GL_LIGHTING);
			}
			Transform.Transform transform = new Transform.Transform(new nzy3D.Plot3D.Transform.Scale(_scaling));
			Scene.Graph.Transform = transform;
			Scene.Graph.Draw(_cam);
		}

		public void RenderOverlay()
		{
			RenderOverlay(new ViewPort(0, 0, _canvas.RendererWidth, _canvas.RendererHeight));
		}

		/// <summary>
		/// Renders all provided Tooltips and Renderer2ds on top of
		/// the scene.
		///
		/// Due to the behaviour of the Overlay implementation, Java2d
		/// geometries must be drawn relative to the Chart's
		/// IScreenCanvas, BUT will then be stretched to fit in the
		/// Camera's viewport. This bug is very important to consider, since
		/// the Camera's viewport may not occupy the full IScreenCanvas.
		/// Indeed, when View is not maximized (like the default behaviour), the
		/// viewport remains square and centered in the canvas, meaning the Overlay
		/// won't cover the full canvas area.
		///
		/// In other words, the following piece of code draws a border around the
		/// View, and not around the complete chart canvas, although queried
		/// to occupy chart canvas dimensions:
		///
		/// g2d.drawRect(1, 1, chart.getCanvas().getRendererWidth()-2,
		/// chart.getCanvas().getRendererHeight()-2);
		///
		/// renderOverlay() must be called while the OpenGL2 context for the
		/// drawable is current, and after the OpenGL2 scene has been rendered.
		/// </summary>
		/// <param name="viewport"></param>
		/// <remarks></remarks>
		public void RenderOverlay(ViewPort viewport)
		{
			// NOT Implemented so far
		}

		internal void correctCameraPositionForIncludingTextLabels(ViewPort viewport)
		{
			_cam.SetViewPort(viewport);
			_cam.shoot(_cameraMode);
			_axe.Draw(_cam);
			Clear();
			AxeBox abox = (AxeBox)_axe;
			BoundingBox3d newBounds = abox.WholeBounds.scale(_scaling);
			if (_viewmode == Modes.ViewPositionMode.TOP) {
				float radius = (float)Math.Max(newBounds.xmax - newBounds.xmin, newBounds.ymax - newBounds.ymin);
				radius += radius * STRETCH_RATIO;
				_cam.RenderingSphereRadius = radius;
			} else {
				_cam.RenderingSphereRadius = (float)newBounds.getRadius();
				Coord3d target = newBounds.getCenter();
				Coord3d eye = _viewpoint.cartesian().@add(target);
				_cam.Target = target;
				_cam.Eye = eye;
			}
		}

		#endregion

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
