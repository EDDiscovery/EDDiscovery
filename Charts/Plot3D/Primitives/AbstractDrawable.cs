
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
using nzy3D.Plot3D.Primitives.Axes;
using nzy3D.Plot3D.Rendering.Canvas;
using nzy3D.Plot3D.Rendering.Legends;
using nzy3D.Plot3D.Rendering.View;
using nzy3D.Plot3D.Transform;

namespace nzy3D.Plot3D.Primitives
{

	/// <summary>
	/// A <see cref="AbstractDrawable"/> defines objects that may be rendered into an OpenGL
	/// context provided by a <see cref="ICanvas"/>.
	/// <br/>
	/// A <see cref="AbstractDrawable"/> must basically provide a rendering function called draw()
	/// that receives a reference to a GL2 and a GLU context. It may also
	/// use a reference to a Camera in order to implement specific behaviors
	/// according to the Camera position.
	/// <br/>
	/// A <see cref="AbstractDrawable"/> provides services for setting the transformation factor
	/// that is used inside the draw function, as well as a getter of the
	/// object's BoundingBox3d. Note that the BoundingBox must be set by
	/// a concrete descendant of a <see cref="AbstractDrawable"/>.
	/// <br/>
	/// A good practice is to define a setData function for initializing a <see cref="AbstractDrawable"/>
	/// and building its polygons. Since each class may have its own inputs, setData
	/// is not part of the interface but should be used as a convention.
	/// When not defining a setData function, a <see cref="AbstractDrawable"/> may have its data loaded by
	/// an "add(Drawable)" function.
	/// <br/>
	/// Note: A <see cref="AbstractDrawable"/> may last provide the information whether it is displayed or not,
	/// according to a rendering into the FeedBack buffer. This is currently supported
	/// specifically for the <see cref="AxeBox"/> object but could be extended with some few more
	/// algorithm for referencing all GL2 polygons.
	///
	/// @author Martin Pernollet
	/// </summary>
	/// <remarks></remarks>
	public abstract class AbstractDrawable : IGLRenderer, ISortableDraw
	{

		internal Transform.Transform _transform;
		internal BoundingBox3d _bbox;
		internal Legend _legend = null;
		internal List<IDrawableListener> _listeners = new List<IDrawableListener>();
		internal bool _displayed = true;

		internal bool _legendDisplayed = false;
		public void Dispose()
		{
			if ((_listeners != null)) {
				_listeners.Clear();
			}
		}

		public abstract void Draw(Rendering.View.Camera cam);

		internal void CallC(Color c)
		{
			GL.Color4(c.r, c.g, c.b, c.a);
		}

		internal void CallC(Color c, float alpha)
		{
			GL.Color4(c.r, c.g, c.b, alpha);
		}

		internal void CallWithAlphaFactor(Color c, float alpha)
		{
			GL.Color4(c.r, c.g, c.b, c.a * alpha);
		}

		/// <summary>
		/// Get / Set object's transformation that is applied at the
		/// beginning of a call to "draw()"
		/// </summary>
		public virtual Transform.Transform Transform {
			get { return _transform; }
			set {
				_transform = value;
				fireDrawableChanged(DrawableChangedEventArgs.FieldChanged.Transform);
			}
		}

		/// <summary>
		/// Return the BoundingBox of this object
		/// </summary>
		public virtual BoundingBox3d Bounds {
			get { return _bbox; }
		}

		/// <summary>
		/// Return the barycentre of this object, which is
		/// computed as the center of its bounding box. If the bounding
		/// box is not available, the returned value is <see cref=" Coord3d.INVALID"/>
		/// </summary>
		public virtual Coord3d Barycentre {
			get {
				if ((_bbox != null)) {
					return _bbox.getCenter();
				} else {
					return Coord3d.INVALID;
				}
			}
		}

		/// <summary>
		/// Get / Set the display status of this object
		/// </summary>
		public virtual bool Displayed {
			get { return _displayed; }
			set {
				_displayed = value;
				fireDrawableChanged(DrawableChangedEventArgs.FieldChanged.Displayed);
			}
		}

		public virtual double getDistance(Rendering.View.Camera camera)
		{
			return Barycentre.distance(camera.Eye);
		}

		public virtual double getLongestDistance(Rendering.View.Camera camera)
		{
			return getDistance(camera);
		}

		public virtual double getShortestDistance(Rendering.View.Camera camera)
		{
			return getDistance(camera);
		}

		public Legend Legend {
			get { return _legend; }
			set {
				_legend = value;
				_legendDisplayed = true;
				fireDrawableChanged(DrawableChangedEventArgs.FieldChanged.Metadata);
			}
		}

		public bool HasLegend {
			get { return (_legend != null); }
		}

		public bool LegendDisplayed {
			get { return _legendDisplayed; }
			set { _legendDisplayed = value; }
		}

		public void addDrawableListener(IDrawableListener listener)
		{
			_listeners.Add(listener);
		}

		public void removeDrawableListener(IDrawableListener listener)
		{
			_listeners.Remove(listener);
		}

		internal void fireDrawableChanged(DrawableChangedEventArgs.FieldChanged eventType)
		{
			fireDrawableChanged(new DrawableChangedEventArgs(this, eventType));
		}

		internal void fireDrawableChanged(DrawableChangedEventArgs e)
		{
            foreach (IDrawableListener listener in _listeners)
            {
				listener.DrawableChanged(e);
			}
		}

		/// <summary>
		/// Returns the string representation of this object
		/// </summary>
		public override string ToString()
		{
			return toString(0);
		}

		public virtual string toString(int depth)
		{
			return Utils.blanks(depth) + "(" + this.GetType().Name + ")";
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
