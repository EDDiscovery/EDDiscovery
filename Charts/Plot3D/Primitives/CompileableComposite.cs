
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
using nzy3D.Plot3D.Rendering.Canvas;
using nzy3D.Plot3D.Rendering.View;

namespace nzy3D.Plot3D.Primitives
{

	/// <summary>
	/// A <see cref="CompileableComposite"/> allows storage and subsequent faster execution of individual
	/// contained instances drawing routines in an OpenGL display list.
	///
	/// Compiling the object take the time needed to render it as a standard <see cref="AbstractComposite"/>,
	/// and rendering it once it is compiled seems to take roughly half the time up to now.
	/// Since compilation occurs during a <see cref="CompileableComposite.Draw" />, the first call to <see cref="CompileableComposite.Draw" /> is supposed
	/// to be 1.5x longer than a standard <see cref="AbstractComposite"/>, while all next cycles would be 0.5x
	/// longer.
	///
	/// Compilation occurs when the content or the display attributes of this Composite changes
	/// (then all add(), remove(), setColor(), setWireFrameDisplayed(), etc). One can also force
	/// rebuilding the object by calling recompile();
	///
	/// IMPORTANT: for the moment, <see cref="CompileableComposite"/> should not be use in a charts using a
	/// <see cref="Quality"/> superior to Intermediate, in other word, you should not desire to have alpha
	/// enabled in your scene. Indeed, alpha requires ordering of polygons each time the viewpoint changes,
	/// which would require to recompile the object.
	///
	/// @author Nils Hoffmann
	/// </summary>
	/// <remarks></remarks>
	public class CompileableComposite : AbstractWireframeable, ISingleColorable, IMultiColorable
	{

		private int _dlID = -1;
		private bool _resetDL = false;
		internal ColorMapper _mapper;
		internal Color _color;
		internal bool _detailedToString = false;

		internal List<AbstractDrawable> _components;
		public CompileableComposite() : base()
		{
			_components = new List<AbstractDrawable>();
		}

		/// <summary>
		/// Force the object to be rebuilt and stored as a display list at the next call to draw(). 
		/// </summary>
		/// <remarks>This operation does not rebuilt the object, but only marks it as "to be rebuilt" for new call to draw().</remarks>
		public void Recompile()
		{
			_resetDL = true;
		}

		/// <summary>
		/// Reset the object if required, compile the object if it is not compiled,
		/// and execute actual rendering. 
		/// </summary>
		/// <param name="cam">Camera to draw for.</param>
		public override void Draw(Rendering.View.Camera cam)
		{
			if (_resetDL) {
				this.Reset();
			}
			if (_dlID == -1) {
				this.Compile(cam);
			}
			this.Execute(cam);
		}

		/// <summary>
		/// If you call compile, the display list will be regenerated. 
		/// </summary>
		internal void Compile(Rendering.View.Camera cam)
		{
			this.Reset();
			// clear old list
			this.NullifyChildrenTransforms();
			_dlID = GL.GenLists(1);
			GL.NewList(_dlID, ListMode.Compile);
			this.DrawComponents(cam);
			GL.EndList();
		}

		internal void Execute(Rendering.View.Camera cam)
		{
			if ((_transform != null)) {
				_transform.Execute();
			}
			GL.CallList(_dlID);
		}

		internal void Reset()
		{
			if (_dlID != -1) {
				if (GL.IsList(_dlID)) {
					GL.DeleteLists(_dlID, 1);
				}
				_dlID = -1;
			}
			_resetDL = false;
		}

		/// <summary>
		/// When a drawable has a null transform, no transform is applied at draw(...). 
		/// </summary>
		internal void NullifyChildrenTransforms()
		{

			lock (_components) {
			}
			foreach (AbstractDrawable c in _components) {
				if ((c != null)) {
					c.Transform = null;
				}
			}
		}

		internal void DrawComponents(Camera cam)
		{
			lock (_components) {
				foreach (AbstractDrawable s in _components) {
					if ((s != null)) {
						s.Draw(cam);
					}
				}
			}
		}

		/// <summary>
		/// Add all drawables stored by this composite.
		/// </summary>
		public void Add(List<AbstractDrawable> drawables)
		{
            this.Add(drawables);
		}

		/// <summary>
		/// Remove all drawables stored by this composite.
		/// </summary>
		public void Add(IEnumerable<AbstractDrawable> drawables)
		{
			lock (_components) {
				_components.AddRange(drawables);
				Recompile();
			}
		}

		/// <summary>
		/// Clear the list of drawables stored by this composite.
		/// </summary>
		public void Clear()
		{
			lock (_components) {
				_components.Clear();
				Recompile();
			}
		}

		/// <summary>
		/// Add a Drawable stored by this composite.
		/// </summary>
		public void Add(AbstractDrawable drawable)
		{
			lock (_components) {
				_components.Add(drawable);
				Recompile();
			}
		}

		/// <summary>
		/// Remove a Drawable stored by this composite.
		/// </summary>
		public void Remove(AbstractDrawable drawable)
		{
			lock (_components) {
				_components.Remove(drawable);
				Recompile();
			}
		}

		/// <summary>
		/// Get a Drawable stored by this composite.
		/// </summary>
		public AbstractDrawable GetDrawable(int p) {
			return _components[p]; 
		}

		/// <summary>
		/// Get an enumerator through the list of drawabless stored by this composite.
		/// </summary>
		public IEnumerable<AbstractDrawable> GetDrawables {
			get { return _components; }
		}

		/// <summary>
		/// Return the number of Drawable stored by this composite.
		/// </summary>
		public int Size {
			get { return _components.Count; }
		}

		public override BoundingBox3d Bounds {
			get {
				BoundingBox3d box = new BoundingBox3d();
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
						if ((c != null) && (c.Bounds != null)) {
							box.Add(c.Bounds);
						}
					}
				}
				return box;
			}
		}

        public override Color WireframeColor
        {
			get { return base.WireframeColor; }
			set {
				base.WireframeColor = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
                        AbstractWireframeable cWf = c as AbstractWireframeable;
                        if (cWf != null) {
							cWf.WireframeColor = Color;
						}
					}
				}
				Recompile();
			}
		}

        public override bool WireframeDisplayed
        {
			get { return base.WireframeDisplayed; }
			set {
				base.WireframeDisplayed = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
						AbstractWireframeable cWf = c as AbstractWireframeable;
                        if (cWf != null) {
							cWf.WireframeDisplayed = value;
						}
					}
				}
				Recompile();
			}
		}

        public override float WireframeWidth
        {
			get { return base.WireframeWidth; }
			set {
				base.WireframeWidth = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
						AbstractWireframeable cWf = c as AbstractWireframeable;
                        if (cWf != null) {
							cWf.WireframeWidth = value;
						}
					}
				}
				Recompile();
			}
		}

        public override bool FaceDisplayed
        {
			get { return base.FaceDisplayed; }
			set {
				base.FaceDisplayed = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
                        AbstractWireframeable cWf = c as AbstractWireframeable;
                        if (cWf != null)
                        {
                            cWf.FaceDisplayed = value;
						}
					}
				}
				Recompile();
			}
		}

		public Colors.ColorMapper ColorMapper {
			get { return _mapper; }
			set {
				_mapper = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
						IMultiColorable cMC = c as IMultiColorable;
                        ISingleColorable cSC = c as ISingleColorable;
                        if (cMC != null)
                        {
                            cMC.ColorMapper = value;
						} else if (cSC != null) {
							cSC.Color = value.Color(c.Barycentre);
						}
					}
				}
				fireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
				Recompile();
			}
		}

		public Colors.Color Color {
			get { return _color; }
			set {
				_color = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
                        ISingleColorable cSC = c as ISingleColorable;
                        if (cSC != null) {
							cSC.Color = value;
						}
					}
				}
				fireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
				Recompile();
			}
		}

		/// <summary>
		/// Returns the string representation of this composite
		/// </summary>
		public override string ToString()
		{
			return toString(0);
		}

		public string ToString(int depth)
		{
			string output = Utils.blanks(depth) + "(Composite3d) #elements:" + _components.Count + " | isDisplayed=" + this.Displayed;
			if (_detailedToString) {
				int k = 0;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
                        AbstractComposite cAc = c as AbstractComposite;
                        if (cAc != null) {
							output += "\r\n" + cAc.toString(depth + 1);
						} else if (c != null) {
                            output += "\r\n" + Utils.blanks(depth + 1) + "Composite element[" + k + "]:" + c.ToString();
						} else {
                            output += "\r\n" + Utils.blanks(depth + 1) + "(null)";
						}
						k += 1;
					}
				}
			}
			return output;
		}

		/// <summary>
		/// Get / Set the property.
		/// When to true, the <see cref="CompileableComposite.toString"/> method will give the detail of each element
		/// of this composite object in a tree like layout.
		/// </summary>
		public bool DetailedToString {
			get { return _detailedToString; }
			set { _detailedToString = value; }
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
