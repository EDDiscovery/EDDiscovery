
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using nzy3D.Colors;
using nzy3D.Events;
using nzy3D.Maths;
using nzy3D.Plot3D.Rendering.Scene;
using nzy3D.Plot3D.Rendering.View;

namespace nzy3D.Plot3D.Primitives
{

	/// <summary>
	/// A Point3d is a storage for a Coord3d and a Color that represents
	/// a drawable 3d point.
	/// <br/>
	/// The Point3d is used for:
	/// <ul>
	/// <li>adding a Point3d to a <see cref="Graph"/>.</li>
	/// <li>providing to other primitives (e.g. <see cref="Polygon"/>)
	/// a way to associate a coordinate and a color.</li>
	/// </ul>
	/// <br/>
	/// A Point3d is defined by the following methods:
	/// <ul>
	/// <li>setData() defines the point's position</li>
	/// <li>setColor() defines the point's color</li>
	/// <li>setWidth() defines the point's width</li>
	/// </ul>
	/// @author Martin Pernollet
	/// </summary>
	/// <remarks></remarks>
	public class Point : AbstractDrawable, ISingleColorable, ISortableDraw
	{


		internal Coord3d _xyz;
		internal Color _rgb;

		internal float _width;
		/// <summary>
		/// Initialize a point at the origin, with a white color and a width of 1.
		/// </summary>
		public Point() : this(Coord3d.ORIGIN, Color.WHITE, 1)
		{
		}

		/// <summary>
		/// Initialize a point at <paramref name="xyz"/> coordinate, with a white color and a width of 1.
		/// </summary>
		/// <param name="xyz">Point coordinates</param>
		public Point(Coord3d xyz) : this(xyz, Color.WHITE, 1)
		{
		}

		/// <summary>
		/// Initialize a point at <paramref name="xyz"/> coordinate, with a <paramref name="rgb"/> color and a width of 1.
		/// </summary>
		/// <param name="xyz">Point coordinates</param>
		/// <param name="rgb">Point color</param>
		public Point(Coord3d xyz, Color rgb) : this(xyz, rgb, 1)
		{
		}

		/// <summary>
		/// Initialize a point at <paramref name="xyz"/> coordinate, with a <paramref name="rgb"/> color and a width of <paramref name="width"/>.
		/// </summary>
		/// <param name="xyz">Point coordinates</param>
		/// <param name="rgb">Point color</param>
		/// <param name="width">Point width</param>
		/// <remarks></remarks>
		public Point(Coord3d xyz, Color rgb, float width)
		{
			_bbox = new BoundingBox3d();
			_xyz = xyz;
			_width = width;
			_rgb = rgb;
			UpdateBounds();
		}

		public override void Draw(Rendering.View.Camera cam)
		{
			if ((_transform != null)) {
				_transform.Execute();
			}
			GL.PointSize(_width);
			GL.Begin(BeginMode.Points);
			GL.Color4(_rgb.r, _rgb.g, _rgb.b, _rgb.a);
			GL.Vertex3(_xyz.x, _xyz.y, _xyz.z);
			GL.End();
		}

		public Coord3d Data {
			get { return _xyz; }
			set {
				_xyz = value;
				UpdateBounds();
			}
		}

		private void UpdateBounds()
		{
			_bbox.reset();
			_bbox.@add(_xyz);
		}

		public Colors.Color Color {
			get { return _rgb; }
			set {
				_rgb = value;
				fireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
			}
		}

		public Color rgb {
			get { return _rgb; }
			set { _rgb = value; }
		}

		public float Width {
			get { return _width; }
			set { _width = value; }
		}

		public Coord3d xyz {
			get { return this.Data; }
			set { this.Data = value; }
		}

		public override double getDistance(Rendering.View.Camera camera)
		{
			return _xyz.distance(camera.Eye);
		}

		public override double getLongestDistance(Rendering.View.Camera camera)
		{
			return getDistance(camera);
		}

		public override double getShortestDistance(Rendering.View.Camera camera)
		{
			return getDistance(camera);
		}

		public string toString(int depth)
		{
            return Utils.blanks(depth) + "(Point) coord={" + _xyz.ToString() + "}, color={" + _rgb.ToString() + "}";
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
