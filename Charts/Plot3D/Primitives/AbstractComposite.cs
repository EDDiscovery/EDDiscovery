
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
using nzy3D.Plot3D.Rendering.View;
using nzy3D.Plot3D.Transform;

namespace nzy3D.Plot3D.Primitives
{

	/// <summary>
	/// A Composite gathers several Drawable and provides default methods
	/// for rendering them all in one call.
	///
	/// @author Martin Pernollet
	/// </summary>
	/// <remarks></remarks>
	public class AbstractComposite : AbstractWireframeable, ISingleColorable, IMultiColorable
	{

		internal List<AbstractDrawable> _components = new List<AbstractDrawable>();
		internal ColorMapper _mapper;
		internal Color _color;

		internal bool _detailedToString = false;
		public AbstractComposite() : base()
		{
		}

		/// <summary>
		/// Remove all drawables stored by this composite.
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
			}
		}

		/// <summary>
		/// Clear the list of drawables stored by this composite.
		/// </summary>
		public void Clear()
		{
			lock (_components) {
				_components.Clear();
			}
		}

		/// <summary>
		/// Add a Drawable stored by this composite.
		/// </summary>
		public void Add(AbstractDrawable drawable)
		{
			lock (_components) {
				_components.Add(drawable);
			}
		}

		/// <summary>
		/// Remove a Drawable stored by this composite.
		/// </summary>
		public void Remove(AbstractDrawable drawable)
		{
			lock (_components) {
				_components.Remove(drawable);
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

		/// <summary>
		/// Delegate rendering iteratively to all Drawable of this composite.
		/// </summary>
		public override void Draw(Rendering.View.Camera cam)
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
		/// Return the transform that was affected to this composite.
		/// </summary>
		public override Transform.Transform Transform {
			get { return _transform; }
			set {
				_transform = value;
				lock (_components) {
					foreach (AbstractDrawable s in _components) {
						if ((s != null)) {
							s.Transform = value;
						}
					}
				}
			}
		}

		/// <summary>
		/// Creates and return a BoundingBox3d that embed all available Drawable bounds
		/// </summary>
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

		public override Color WireframeColor {
			get { return base.WireframeColor; }
			set {
				base.WireframeColor = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
                        AbstractWireframeable cWF = c as AbstractWireframeable;
                        if (cWF != null)
                        {
                            cWF.WireframeColor = value;
						}
					}
				}
			}
		}

		public override bool WireframeDisplayed {
			get { return base.WireframeDisplayed; }
			set {
				base.WireframeDisplayed = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
                        AbstractWireframeable cWF = c as AbstractWireframeable;
                        if (cWF != null)
                        {
                            cWF.WireframeDisplayed = value;
						}
					}
				}
			}
		}

		public override float WireframeWidth {
			get { return base.WireframeWidth; }
			set {
				base.WireframeWidth = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
                        AbstractWireframeable cWF = c as AbstractWireframeable;
                        if (cWF != null)
                        {
                            cWF.WireframeWidth = value;
						}
					}
				}
			}
		}

		public override bool FaceDisplayed {
			get { return base.FaceDisplayed; }
			set {
				base.FaceDisplayed = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
                        AbstractWireframeable cWF = c as AbstractWireframeable;
                        if (cWF != null)
                        {
                            cWF.FaceDisplayed = value;
						}
					}
				}
			}
		}

		public override bool Displayed {
			get { return base.Displayed; }
			set {
				base.Displayed = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
                        AbstractWireframeable cWF = c as AbstractWireframeable;
						if (cWF != null) {
                            cWF.Displayed = value;
						}
					}
				}
			}
		}

		public Colors.ColorMapper ColorMapper {
			get { return _mapper; }
			set {
				_mapper = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
                        IMultiColorable cIM = c as IMultiColorable;
                        ISingleColorable cIC = c as ISingleColorable;
                        if (cIM != null) {
								cIM.ColorMapper = value;
						} else if (cIC != null) {
							cIC.Color = value.Color(c.Barycentre);
						}
					}
				}
				fireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
			}
		}

		public Colors.Color Color {
			get { return _color; }
			set {
				_color = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
                        ISingleColorable cIC = c as ISingleColorable;
                        if (cIC !=  null)
                        {
							cIC.Color = value;
						}
					}
				}
				fireDrawableChanged(new DrawableChangedEventArgs(this, DrawableChangedEventArgs.FieldChanged.Color));
			}
		}

		/// <summary>
		/// Returns the string representation of this composite
		/// </summary>
		public override string ToString()
		{
			return toString(0);
		}

		public override string toString(int depth)
		{
			string output = Utils.blanks(depth) + "(Composite3d) #elements:" + _components.Count + " | isDisplayed=" + this.Displayed;
			if (_detailedToString) {
				int k = 0;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
                        AbstractComposite cAC = c as AbstractComposite;
                        if (cAC != null) {
							output += "\r\n" + ((AbstractComposite)c).toString(depth + 1);
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
