
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using nzy3D.Maths;
using nzy3D.Plot3D.Primitives;
using nzy3D.Plot3D.Rendering.Legends;
using nzy3D.Plot3D.Rendering.Ordering;
using nzy3D.Plot3D.Rendering.View;
using nzy3D.Plot3D.Transform;

namespace nzy3D.Plot3D.Rendering.Scene
{

	/// <summary>
	/// The scene's <see cref="Graph"/> basically stores the scene content and facilitate objects control
	///
	/// The graph may decompose all <see cref="AbstractComposite"/> into a list of their <see cref="AbstractDrawable"/>s primitives
	/// if constructor is called with parameters enabling sorting.
	///
	/// The list of primitives is ordered using either the provided <see cref="DefaultOrderingStrategy"/>
	/// or an other specified <see cref="AbstractOrderingStrategy"/>. Sorting is usefull for handling transparency
	/// properly.
	///
	/// The <see cref="Graph"/> maintains a reference to its mother <see cref="Scene"/> in order to
	/// inform the <see cref="View"/>s when its content has change and that repainting is required.
	///
	/// The add() method allows adding a <see cref="AbstractDrawable"/> to the scene Graph and updates
	/// all views' viewpoint in order to target the center of the scene.
	///
	/// @author Martin Pernollet
	/// </summary>
	/// <remarks></remarks>
	public class Graph
	{

		internal List<AbstractDrawable> _components;
		internal View.Scene _scene;
		internal Transform.Transform _transform;
		internal AbstractOrderingStrategy _strategy;

		internal bool _sort = true;
		public Graph(View.Scene scene) : this(scene, new DefaultOrderingStrategy(), true)
		{
		}

		public Graph(View.Scene scene, bool sort) : this(scene, new DefaultOrderingStrategy(), sort)
		{
		}

		public Graph(View.Scene scene, AbstractOrderingStrategy strategy) : this(scene, strategy, true)
		{
		}

		public Graph(View.Scene scene, AbstractOrderingStrategy strategy, bool sort)
		{
			_scene = scene;
			_strategy = strategy;
			_sort = sort;
			_components = new List<AbstractDrawable>();
		}

		public void Dispose()
		{
			lock (_components) {
				foreach (AbstractDrawable c in _components) {
					if ((c != null)) {
						c.Dispose();
					}
				}
				_components.Clear();
			}
			_scene = null;
		}

		public void Add(AbstractDrawable drawable, bool updateViews)
		{
			lock (_components) {
				_components.Add(drawable);
			}
			if (updateViews) {
				foreach (View.View view in _scene.Views) {
					view.updateBounds();
				}
			}
		}

		public void Add(AbstractDrawable drawable)
		{
			Add(drawable, true);
		}

		public void Add(IEnumerable<AbstractDrawable> drawables, bool updateViews)
		{
			foreach (AbstractDrawable d in drawables) {
				Add(d, false);
			}
			if (updateViews) {
				foreach (View.View view in _scene.Views) {
					view.updateBounds();
				}
			}
		}

		public void Add(IEnumerable<AbstractDrawable> drawables)
		{
			Add(drawables, true);
		}

		public void Remove(AbstractDrawable drawable, bool updateViews)
		{
			bool output = false;
			lock (_components) {
				output = _components.Remove(drawable);
			}
			BoundingBox3d bbox = this.Bounds;
			foreach (View.View view in _scene.Views) {
				view.lookToBox(bbox);
				if (updateViews) {
					view.Shoot();
				}
			}
		}

		public void Remove(AbstractDrawable drawable)
		{
			Remove(drawable, true);
		}

		public IEnumerable<AbstractDrawable> All {
			get { return _components; }
		}

		public IEnumerable<IGLBindedResource> AllGLBindedResources {
			get {
				List<IGLBindedResource> @out = new List<IGLBindedResource>();
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
                        IGLBindedResource cIGL = c as IGLBindedResource;
						if (cIGL != null) {
                            @out.Add(cIGL);
						}
					}
				}
				return @out;
			}
		}

		public void MountAllGLBindedResources()
		{
			foreach (IGLBindedResource r in this.AllGLBindedResources) {
				if (!r.hasMountedOnce()) {
					r.Mount();
				}
			}
		}

		public void Draw(Camera camera)
		{
			Draw(camera, _components, _sort);
		}

		public void Draw(Camera camera, List<AbstractDrawable> components, bool sort)
		{
			GL.MatrixMode(MatrixMode.Modelview);
			lock (components) {
				if (!sort) {
					// render all items of the graph
					foreach (AbstractDrawable d in components) {
						if (d.Displayed) {
							d.Draw(camera);
						}
					}
				} else {
					// expand all composites into a list of monotypes
					List<AbstractDrawable> monotypes = Decomposition.GetDecomposition(components);
					//Compute order of monotypes for rendering
					_strategy.Sort(monotypes, camera);
					//Render sorted monotypes
					foreach (AbstractDrawable d in monotypes) {
						if (d.Displayed) {
							d.Draw(camera);
						}
					}
				}
			}
		}

		/// <summary>
		/// Update all interactive <see cref="AbstractDrawable"/> projections
		/// </summary>
		public void Project(Camera camera)
		{
			lock (_components) {
				foreach (AbstractDrawable d in _components) {
                    ISelectable dS = d as ISelectable;
					if (dS != null) {
                        dS.Project(camera);
					}
				}
			}
		}

		public AbstractOrderingStrategy Strategy {
			get { return _strategy; }
			set { _strategy = value; }
		}

		/// <summary>
		/// Get/Set the transformation of this Graph
		/// When set, transforming is delegated iteratively to all Drawable of this graph.*/
		/// </summary>
		public Transform.Transform Transform {
			get { return _transform; }
			set {
				_transform = value;
				lock (_components) {
					foreach (AbstractDrawable c in _components) {
						if ((c != null)) {
							c.Transform = value;
						}
					}
				}
			}
		}

		public BoundingBox3d Bounds {
			get {
				if (_components.Count == 0) {
					return new BoundingBox3d(0, 0, 0, 0, 0, 0);
				} else {
					BoundingBox3d box = new BoundingBox3d();
					lock (_components) {
						foreach (AbstractDrawable a in _components) {
							if (((a != null)) && ((a.Bounds != null))) {
								box.Add(a.Bounds);
							}
						}
					}
					return box;
				}
			}
		}

		/// <summary>
		/// Return the list of available <see cref="AbstractDrawable"/>'s  displayed <see cref="Legend"/>
		/// </summary>
		public List<Legend> Legends {
			get {
				List<Legend> list = new List<Legend>();
				lock (_components) {
					foreach (AbstractDrawable a in _components) {
						if (((a != null)) && (a.HasLegend & a.LegendDisplayed)) {
							list.Add(a.Legend);
						}
					}
				}
				return list;
			}
		}

		/// <summary>
		/// Returns the number of components with displayed legend
		/// </summary>
		public int hasLengends()
		{
			int k = 0;
			lock (_components) {
				foreach (AbstractDrawable a in _components) {
					if (((a != null)) && (a.HasLegend & a.LegendDisplayed)) {
						k += 1;
					}
				}
			}
			return k;
		}

		/// <summary>
		/// Print out information concerning all Drawable of this composite
		/// </summary>
		public override string ToString()
		{
			string output = "(Graph) #elements:" + _components.Count + ":\r\n";
			int k = 0;
			lock (_components) {
				foreach (AbstractDrawable a in _components) {
					if (((a != null))) {
                        output += " Graph element [" + k + "]:" + a.toString(1) + "\r\n";
					} else {
                        output += " Graph element [" + k + "]:(null)" + "\r\n";
					}
					k += 1;
				}
			}
			return output;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
