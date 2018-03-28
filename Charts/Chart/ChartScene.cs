
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Plot3D.Rendering.Scene;
using nzy3D.Plot3D.Rendering.View;
using nzy3D.Maths;
using nzy3D.Plot3D.Rendering.Canvas;

namespace nzy3D.Chart
{

	public class ChartScene : Scene
	{

		internal int _nview;

		internal View _view;
		public ChartScene(bool graphsort) : base(graphsort)
		{
			_nview = 0;
		}

		public void Clear()
		{
			_view.BoundManual = new BoundingBox3d(0, 0, 0, 0, 0, 0);
		}

		public override View newView(ICanvas canvas, Quality quality)
		{
			if (_nview > 0) {
				throw new Exception("A view has already been defined for this scene. Can not use several views.");
			}
			_nview += 1;
			_view = base.newView(canvas, quality);
			return _view;
		}

		public override void clearView(View view)
		{
			base.clearView(view);
			_nview = 0;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
