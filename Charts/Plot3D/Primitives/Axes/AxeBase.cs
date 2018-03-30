
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using nzy3D.Maths;
using nzy3D.Plot3D.Primitives.Axes.Layout;
using nzy3D.Plot3D.Rendering.View;

namespace nzy3D.Plot3D.Primitives.Axes
{

	/// <summary>
	/// An AxeBase provide a simple 3-segment object which is configured by
	/// a BoundingBox.
	/// @author Martin Pernollet
	/// </summary>
	public class AxeBase : IAxe
	{

		internal Coord3d _scale;
		internal BoundingBox3d _bbox;

		internal IAxeLayout _layout;
		/// <summary>
		/// Create a simple axe centered on (0,0,0), with a dimension of 1.
		/// </summary>
		public AxeBase() : this(new BoundingBox3d(0, 1, 0, 1, 0, 1))
		{
		}

		/// <summary>
		/// Create a simple axe centered on (box.xmin, box.ymin, box.zmin)
		/// </summary>
		public AxeBase(BoundingBox3d box)
		{
			setAxe(box);
			setScale(new Coord3d(1, 1, 1));
		}

		public void Dispose()
		{
		}

		public void Draw(Rendering.View.Camera camera)
		{
			GL.LoadIdentity();
			GL.Scale(_scale.x, _scale.y, _scale.y);
			GL.LineWidth(2);
			GL.Begin(BeginMode.Lines);
			GL.Color3(1, 0, 0);
			// R
			GL.Vertex3(_bbox.xmin, _bbox.ymin, _bbox.zmin);
			GL.Vertex3(_bbox.xmax, 0, 0);
			GL.Color3(0, 1, 0);
			// G
			GL.Vertex3(_bbox.xmin, _bbox.ymin, _bbox.zmin);
			GL.Vertex3(0, _bbox.ymax, 0);
			GL.Color3(0, 0, 1);
			// B
			GL.Vertex3(_bbox.xmin, _bbox.ymin, _bbox.zmin);
			GL.Vertex3(0, 0, _bbox.zmax);
			GL.End();
		}

		public Maths.BoundingBox3d getBoxBounds()
		{
			return _bbox;
		}

		public Maths.Coord3d getCenter()
		{
			return new Coord3d(_bbox.xmin, _bbox.ymin, _bbox.zmin);
		}

		public Layout.IAxeLayout getLayout()
		{
			return _layout;
		}

		public void setAxe(Maths.BoundingBox3d box)
		{
			_bbox = box;
		}

		public void setScale(Maths.Coord3d scale)
		{
			_scale = scale;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
