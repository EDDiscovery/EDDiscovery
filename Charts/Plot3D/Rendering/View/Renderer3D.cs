
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Plot3D.Rendering.Canvas;
using nzy3D.Plot3D.Rendering.Scene;
using nzy3D.Events;

namespace nzy3D.Plot3D.Rendering.View
{

	/// <summary>
	/// 
	/// </summary>
	/// <remarks></remarks>
	public class Renderer3D : OpenTK.GLControl, ICanvas, IControllerEventListener
	{

		// TODO  : add trace add debug capabilities
		internal View _view;
		internal int _width = 0;
		internal int _height = 0;
		internal bool _doScreenshotAtNextDisplay = false;
		internal bool _traceGL;
		internal bool _debugGL;

		internal System.Drawing.Bitmap _image;
		//Public Sub New(view As View)
		//  Me.New(view, False, False)
		//End Sub

		//Public Sub New(view As View, traceGL As Boolean, debugGL As Boolean)
		//  _view = view
		//  _traceGL = traceGL
		//  _debugGL = debugGL
		//End Sub

		//Private Sub Renderer3D_Load(sender As Object, e As System.EventArgs) Handles Me.Load

		//End Sub

		private void Renderer3D_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if ((_view != null)) {
				_view.Clear();
				_view.Render();
				this.SwapBuffers();
				if (_doScreenshotAtNextDisplay) {
					GrabScreenshot2();
					_doScreenshotAtNextDisplay = false;
				}
			}
		}

		private void Renderer3D_Resize(object sender, System.EventArgs e)
		{
			_width = this.ClientSize.Width;
			_height = this.ClientSize.Height;
			if ((_view != null)) {
				_view.DimensionDirty = true;
			}
		}

		private void GrabScreenshot2()
		{
			if (_image == null || _image.Width != this.Width || _image.Height != this.Height) {
				_image = new System.Drawing.Bitmap(ClientSize.Width, ClientSize.Height);
			}
			System.Drawing.Imaging.BitmapData data = _image.LockBits(this.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			//OpenTK.Graphics.OpenGL.GL.ReadPixels(0, 0, ClientSize.Width, ClientSize.Height, OpenTK.Graphics.PixelFormat.Bgr, OpenTK.Graphics.PixelType.UnsignedByte, data.Scan0)
            OpenTK.Graphics.OpenGL.PixelFormat pxFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
            OpenTK.Graphics.OpenGL.PixelType pxType = OpenTK.Graphics.OpenGL.PixelType.UnsignedByte;

            OpenTK.Graphics.OpenGL.GL.ReadPixels(0, 0, ClientSize.Width, ClientSize.Height, pxFormat, pxType, data.Scan0);
			_image.UnlockBits(data);
			_image.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipY);
		}

		public void nextDisplayUpdateScreenshot()
		{
			_doScreenshotAtNextDisplay = true;
		}

		public System.Drawing.Bitmap LastScreenshot {
			get { return _image; }
		}

        public new int Width
        {
			get { return _width; }
		}

        public new int Height
        {
			get { return _height; }
		}

		public void addKeyListener(Events.Keyboard.IKeyListener listener)
		{
			KeyUp += listener.KeyReleased;
			KeyDown += listener.KeyPressed;
			// be cautious with cross-terminology (key_down / key_pressed / key_typed)
			KeyPress += listener.KeyTyped;
			// be cautious with cross-terminology (key_down / key_pressed / key_typed)
		}

		public void addMouseListener(Events.Mouse.IMouseListener listener)
		{
			MouseClick += listener.MouseClicked;
			MouseDown += listener.MousePressed;
			MouseUp += listener.MouseReleased;
			MouseDoubleClick += listener.MouseDoubleClicked;
		}

		public void addMouseMotionListener(Events.Mouse.IMouseMotionListener listener)
		{
			MouseMove += listener.MouseMoved;
			// NOT AVAILABLE IN WinForms : AddHandler ???, AddressOf listener.MouseDragged
		}

		public void addMouseWheelListener(Events.Mouse.IMouseWheelListener listener)
		{
			MouseWheel += listener.MouseWheelMoved;
		}


		public void Dispose1()
		{
		}
		void Canvas.ICanvas.Dispose()
		{
			Dispose1();
		}

		public void ForceRepaint()
		{
			this.Invalidate();
		}

		public void removeKeyListener(Events.Keyboard.IKeyListener listener)
		{
			KeyUp -= listener.KeyReleased;
			KeyDown -= listener.KeyPressed;
			// be cautious with cross-terminology (key_down / key_pressed / key_typed)
			KeyPress -= listener.KeyTyped;
			// be cautious with cross-terminology (key_down / key_pressed / key_typed)
		}

		public void removeMouseListener(Events.Mouse.IMouseListener listener)
		{
			MouseClick -= listener.MouseClicked;
			MouseDown -= listener.MousePressed;
			MouseUp -= listener.MouseReleased;
		}

		public void removeMouseMotionListener(Events.Mouse.IMouseMotionListener listener)
		{
			MouseMove -= listener.MouseMoved;
			// NOT AVAILABLE IN WinForms : RemoveHandler ???, AddressOf listener.MouseDragged
		}

		public void removeMouseWheelListener(Events.Mouse.IMouseWheelListener listener)
		{
			MouseWheel -= listener.MouseWheelMoved;
		}

		public int RendererHeight {
			get { return _height; }
		}

		public int RendererWidth {
			get { return _width; }
		}

		public System.Drawing.Bitmap Screenshot()
		{
			//Throw New NotImplementedException()
			this.GrabScreenshot2();
			return _image;

		}

		public View View {
				//Throw New NotImplementedException("Property View is not implemented in nzy3D renderer, should not be necessary")
			get { return _view; }
		}

		public void setView(View value)
		{
			_view = value;
			_view.Init();
			_view.Scene.Graph.MountAllGLBindedResources();
			_view.BoundManual = _view.Scene.Graph.Bounds;
		}

		public void ControllerEventFired(Events.ControllerEventArgs e)
		{
			this.ForceRepaint();
		}
		public Renderer3D()
		{
			Resize += Renderer3D_Resize;
			Paint += Renderer3D_Paint;
		}
	}

}


//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
