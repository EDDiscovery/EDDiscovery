
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
	/// A Parallelepiped is a parallelepiped rectangle that is Drawable
	/// and Wireframeable.
	/// A future version of Rectangle3d should consider it as a Composite3d.
	///
	/// This class has been implemented for debugging purpose and inconsistency
	/// of its input w.r.t other primitives should not be considered
	/// (no setData function).
	///
	/// @author Martin Pernollet
	/// </summary>
	public class Parallelepiped : AbstractWireframeable, ISingleColorable, IMultiColorable
	{

		private ColorMapper _mapper;
		private List<Polygon> _quads;

		private Color _color;
		public Parallelepiped() : base()
		{
			_bbox = new BoundingBox3d();
			_quads = new List<Polygon>(6);
		}

		public Parallelepiped(BoundingBox3d b) : base()
		{
			_bbox = new BoundingBox3d();
			_quads = new List<Polygon>();
			setData(b);
		}

		public override void Draw(Rendering.View.Camera cam)
		{
			foreach (Polygon quad in _quads) {
				quad.Draw(cam);
			}
		}

		/// <summary>
		/// Return the transform that was affected to this composite.
		/// </summary>
		public override Transform.Transform Transform {
			get { return _transform; }
			set {
				_transform = value;
				lock (_quads) {
					foreach (Polygon s in _quads) {
						if ((s != null)) {
							s.Transform = value;
						}
					}
				}
			}
		}

		public override Color WireframeColor {
			get { return base.WireframeColor; }
			set {
				base.WireframeColor = value;
				lock (_quads) {
					foreach (Polygon s in _quads) {
						if ((s != null)) {
							s.WireframeColor = value;
						}
					}
				}
			}
		}

		public override bool WireframeDisplayed {
			get { return base.WireframeDisplayed; }
			set {
				base.WireframeDisplayed = value;
				lock (_quads) {
					foreach (Polygon s in _quads) {
						if ((s != null)) {
							s.WireframeDisplayed = value;
						}
					}
				}
			}
		}

		public override float WireframeWidth {
			get { return base.WireframeWidth; }
			set {
				base.WireframeWidth = value;
				lock (_quads) {
					foreach (Polygon s in _quads) {
						if ((s != null)) {
							s.WireframeWidth = value;
						}
					}
				}
			}
		}

		public override bool FaceDisplayed {
			get { return base.FaceDisplayed; }
			set {
				base.FaceDisplayed = value;
				lock (_quads) {
					foreach (Polygon s in _quads) {
						if ((s != null)) {
							s.FaceDisplayed = value;
						}
					}
				}
			}
		}

		public void setData(BoundingBox3d box)
		{
			_bbox.reset();
			_bbox.Add(box);
			_quads = new List<Polygon>(6);
			// Add 6 polygons to list
			for (int i = 0; i <= 5; i++) {
				_quads.Add(new Polygon());
			}
			_quads[0].Add(new Point(new Coord3d(_bbox.xmax, _bbox.ymin, _bbox.zmax)));
			_quads[0].Add(new Point(new Coord3d(_bbox.xmax, _bbox.ymin, _bbox.zmin)));
			_quads[0].Add(new Point(new Coord3d(_bbox.xmax, _bbox.ymax, _bbox.zmin)));
			_quads[0].Add(new Point(new Coord3d(_bbox.xmax, _bbox.ymax, _bbox.zmax)));
			_quads[1].Add(new Point(new Coord3d(_bbox.xmin, _bbox.ymax, _bbox.zmax)));
			_quads[1].Add(new Point(new Coord3d(_bbox.xmin, _bbox.ymax, _bbox.zmin)));
			_quads[1].Add(new Point(new Coord3d(_bbox.xmin, _bbox.ymin, _bbox.zmin)));
			_quads[1].Add(new Point(new Coord3d(_bbox.xmin, _bbox.ymin, _bbox.zmax)));
			_quads[2].Add(new Point(new Coord3d(_bbox.xmax, _bbox.ymax, _bbox.zmax)));
			_quads[2].Add(new Point(new Coord3d(_bbox.xmax, _bbox.ymax, _bbox.zmin)));
			_quads[2].Add(new Point(new Coord3d(_bbox.xmin, _bbox.ymax, _bbox.zmin)));
			_quads[2].Add(new Point(new Coord3d(_bbox.xmin, _bbox.ymax, _bbox.zmax)));
			_quads[3].Add(new Point(new Coord3d(_bbox.xmin, _bbox.ymin, _bbox.zmax)));
			_quads[3].Add(new Point(new Coord3d(_bbox.xmin, _bbox.ymin, _bbox.zmin)));
			_quads[3].Add(new Point(new Coord3d(_bbox.xmax, _bbox.ymin, _bbox.zmin)));
			_quads[3].Add(new Point(new Coord3d(_bbox.xmax, _bbox.ymin, _bbox.zmax)));
			_quads[4].Add(new Point(new Coord3d(_bbox.xmin, _bbox.ymin, _bbox.zmax)));
			_quads[4].Add(new Point(new Coord3d(_bbox.xmax, _bbox.ymin, _bbox.zmax)));
			_quads[4].Add(new Point(new Coord3d(_bbox.xmax, _bbox.ymax, _bbox.zmax)));
			_quads[4].Add(new Point(new Coord3d(_bbox.xmin, _bbox.ymax, _bbox.zmax)));
			_quads[5].Add(new Point(new Coord3d(_bbox.xmax, _bbox.ymin, _bbox.zmin)));
			_quads[5].Add(new Point(new Coord3d(_bbox.xmin, _bbox.ymin, _bbox.zmin)));
			_quads[5].Add(new Point(new Coord3d(_bbox.xmin, _bbox.ymax, _bbox.zmin)));
			_quads[5].Add(new Point(new Coord3d(_bbox.xmax, _bbox.ymax, _bbox.zmin)));
		}

		public Colors.ColorMapper ColorMapper {
			get { return _mapper; }
			set {
				_mapper = value;
				lock (_quads) {
					foreach (Polygon s in _quads) {
						if ((s != null)) {
							s.ColorMapper = value;
						}
					}
				}
			}
		}

		public Colors.Color Color {
			get { return _color; }
			set {
				_color = value;
				lock (_quads) {
					foreach (Polygon s in _quads) {
						if ((s != null)) {
							s.Color = value;
						}
					}
				}
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
