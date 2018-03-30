
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Colors;

namespace nzy3D.Plot3D.Primitives
{

	/// <summary>
	/// An <see cref="AbstractWireframeable"/> {@link AbstractWireframeable} is an <see cref=" AbstractDrawable"/>
	/// that has a wireframe mode for display.
	///
	/// Defining an object as Wireframeable means this object may have a wireframe
	/// mode status (on/off), a wireframe color, and a wireframe width.
	/// As a consequence of being wireframeable, a 3d object may have his faces
	/// displayed or not by setFaceDisplayed().
	/// </summary>
	public abstract class AbstractWireframeable : AbstractDrawable
	{

		internal Color _wfcolor;
		internal float _wfwidth;
		internal bool _wfstatus;

		internal bool _facestatus;

		public AbstractWireframeable() : base()
		{
			_wfcolor = Color.WHITE;
			_wfwidth = 1;
			_wfstatus = true;
			_facestatus = true;
		}

		/// <summary>
		/// Get/Set the wireframe color
		/// </summary>
		public virtual Color WireframeColor {
			get { return _wfcolor; }
			set { _wfcolor = value; }
		}

		/// <summary>
		/// Get/Set the wireframe width
		/// </summary>
		public virtual float WireframeWidth {
			get { return _wfwidth; }
			set { _wfwidth = value; }
		}

		/// <summary>
		/// Get/Set the wireframe display status
		/// </summary>
		/// <value>on (true) / off (false)</value>
		public virtual bool WireframeDisplayed {
			get { return _wfstatus; }
			set { _wfstatus = value; }
		}

		/// <summary>
		/// Get/Set the face display status
		/// </summary>
		/// <value>on (true) / off (false)</value>
		public virtual bool FaceDisplayed {
			get { return _facestatus; }
			set { _facestatus = value; }
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
