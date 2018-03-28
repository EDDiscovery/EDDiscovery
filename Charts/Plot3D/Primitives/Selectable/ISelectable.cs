
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
	public interface ISelectable
	{

		void Project(Camera cam);

		List<Coord3d> LastProjection { get; }
	}
}


//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
