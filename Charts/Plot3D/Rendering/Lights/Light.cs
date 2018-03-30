
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Colors;
using nzy3D.Maths;
using OpenTK.Graphics.OpenGL;

namespace nzy3D.Plot3D.Rendering.Lights
{

	public class Light
	{
		internal int _lightId;
		internal bool _enabled;
		internal Color _ambiantColor;
		internal Color _diffuseColor;
		internal Color _specularColor;
		internal Coord3d _position;
		internal float[] _positionZero = {
			0.0f,
			0.0f,
			0.0f,
			1.0f
		};

		internal bool _representationDisplayed;
		/// <summary>
		/// Initialise a new light
		/// </summary>
		/// <param name="id">Light Id in rangle [0,7]</param>
		/// <remarks>Light will be enabled and represented on the scene by default</remarks>
		public Light(int id) : this(id, true)
		{
		}

		/// <summary>
		/// Initialise a new light
		/// </summary>
		/// <param name="id">Light Id in rangle [0,7]</param>
		/// <param name="representationDisplayed">If true, light is represented on the scene by a square</param>
		/// <remarks>Light will be enabled by default</remarks>
		public Light(int id, bool representationDisplayed) : this(id, true, representationDisplayed)
		{
		}

		/// <summary>
		/// Initialise a new light
		/// </summary>
		/// <param name="id">Light Id in rangle [0,7]</param>
		/// <param name="enabled">IF true, light is enabled ?</param>
		/// <param name="representationDisplayed">If true, light is represented on the scene by a square</param>
		public Light(int id, bool enabled, bool representationDisplayed)
		{
			this._lightId = id;
			this._enabled = enabled;
			this._representationDisplayed = representationDisplayed;
			this._ambiantColor = Color.WHITE;
			this._diffuseColor = Color.WHITE;
			this._specularColor = Color.WHITE;
		}

		public void Apply(Coord3d scale)
		{
			if (_enabled) {
				GL.MatrixMode(MatrixMode.Modelview);
				GL.LoadIdentity();
				GL.Translate(_position.x * scale.x, _position.y * scale.y, _position.z * scale.z);
				// Light position representation (cube)
				if ((_representationDisplayed)) {
					GL.Disable(EnableCap.Lighting);
					GL.Color3(0.0, 1.0, 1.0);
					GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
					Glut.Glut.SolidCube(10);
					GL.Enable(EnableCap.Lighting);
				}
				// Actual light source setting	TODO: check we really need to define @ each rendering	
				LightSwitch.Enable(_lightId);
				GL.Light(LightName.Light0, LightParameter.Position, _positionZero);
				GL.Light(LightName.Light0, LightParameter.Ambient, _ambiantColor.OpenTKColor4);
				GL.Light(LightName.Light0, LightParameter.Diffuse, _diffuseColor.OpenTKColor4);
				GL.Light(LightName.Light0, LightParameter.Specular, _specularColor.OpenTKColor4);
			} else {
				GL.Disable(EnableCap.Lighting);
			}
		}

		/// <summary>
		/// Indicates if a square is drawn to show the light position. 
		/// </summary>
		public bool RepresentationDisplayed {
			get { return _representationDisplayed; }
			set { _representationDisplayed = value; }
		}

		public int Id {
			get { return _lightId; }
		}

		public Coord3d Position {
			get { return _position; }
			set { _position = value; }
		}

		public bool Enabled {
			get { return _enabled; }
			set { _enabled = value; }
		}

		public Color AmbiantColor {
			get { return _ambiantColor; }
			set { _ambiantColor = value; }
		}

		public Color DiffuseColor {
			get { return _diffuseColor; }
			set { _diffuseColor = value; }
		}

		public Color SpecularColor {
			get { return _specularColor; }
			set { _specularColor = value; }
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
