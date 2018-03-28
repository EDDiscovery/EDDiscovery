
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using nzy3D.Plot3D.Rendering.Canvas;
using nzy3D.Plot3D.Rendering.Legends;
using nzy3D.Plot3D.Rendering.Scene;
using nzy3D.Plot3D.Rendering.View;
using nzy3D.Plot3D.Primitives;

namespace nzy3D.Chart
{

	/// <summary>
	/// A <see cref="ChartView"/> allows displaying a 3d scene on the left,
	/// and a set of <see cref="AbstractDrawable"/>'s <see cref="Legend"/> on the right.
	/// @author Martin Pernollet
	/// </summary>
	/// <remarks></remarks>
	public class ChartView : View
	{

		internal System.Drawing.Rectangle _zone1;

		internal System.Drawing.Rectangle _zone2;
		public ChartView(Scene scene, ICanvas canvas, Quality quality) : base(scene, canvas, quality)
		{
			// display zones
			_zone1 = new System.Drawing.Rectangle(0, 0, 0, 0);
			_zone2 = new System.Drawing.Rectangle(0, 0, 0, 0);
		}

		/// <summary>
		/// Set the camera held by this view, and draw the scene graph.
		/// Performs all transformations of eye, target coordinates to adapt the camera settings
		/// to the scaled scene.
		/// </summary>
		/// <remarks></remarks>
		public override void Render()
		{
			List<Legend> list = _scene.Graph.Legends;
			bool hasMeta = (list.Count > 0);
			// Compute an optimal layout so that we use the minimal area for metadata
			float screenSeparator = 1.0f;
			if ((hasMeta)) {
				int minwidth = 0;
				foreach (Legend data in list) {
					minwidth += data.MinimumSize.Width;
				}
				screenSeparator = (_canvas.RendererWidth - minwidth) / _canvas.RendererWidth;
				///0.7f
			}
			ViewPort sceneViewPort = ViewPort.Slice(_canvas.RendererWidth, _canvas.RendererHeight, 0, screenSeparator);
			ViewPort backgroundViewPort = new ViewPort(_canvas.RendererWidth, _canvas.RendererHeight);
			RenderBackground(backgroundViewPort);
			RenderScene(sceneViewPort);
			if ((hasMeta)) {
				renderFaces(screenSeparator, 1);
			}
			// fix overlay on top of chart
			//System.out.println(scenePort);
			RenderOverlay(_cam.LastViewPort);
			//renderOverlay(gl);
			if ((_dimensionDirty)) {
				_dimensionDirty = false;
			}
		}

		internal void renderFaces(float left, float right)
		{
			List<Legend> data = _scene.Graph.Legends;
			float slice = (right - left) / data.Count;
			int k = 0;
			foreach (Legend layer in data) {
				layer.StretchToFill = true;
				layer.SetViewPort(_canvas.RendererWidth, _canvas.RendererHeight, left + slice * k, left + slice * (k + 1));
				// correction par rapport à l'incrémentation des indices
				k += 1;
				layer.Render();
			}
		}


	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
