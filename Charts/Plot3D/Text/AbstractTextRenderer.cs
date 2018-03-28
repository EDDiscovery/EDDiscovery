
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;

namespace nzy3D.Plot3D.Text
{

	public abstract class AbstractTextRenderer : ITextRenderer
	{

		internal Coord2d defScreenOffset;

		internal Coord3d defSceneOffset;
		public AbstractTextRenderer()
		{
			defScreenOffset = new Coord2d();
			defSceneOffset = new Coord3d();
		}

        public abstract void drawSimpleText(Rendering.View.Camera cam, string s, Maths.Coord3d position, Colors.Color color);

		public Maths.BoundingBox3d drawText(Rendering.View.Camera cam, string s, Maths.Coord3d position, Align.Halign halign, Align.Valign valign, Colors.Color color)
		{
			return drawText(cam, s, position, halign, valign, color, defScreenOffset, defSceneOffset);
		}

		public Maths.BoundingBox3d drawText(Rendering.View.Camera cam, string s, Maths.Coord3d position, Align.Halign halign, Align.Valign valign, Colors.Color color, Maths.Coord2d screenOffset)
		{
			return drawText(cam, s, position, halign, valign, color, screenOffset, defSceneOffset);
		}

		public abstract Maths.BoundingBox3d drawText(Rendering.View.Camera cam, string s, Maths.Coord3d position, Align.Halign halign, Align.Valign valign, Colors.Color color, Maths.Coord2d screenOffset, Maths.Coord3d sceneOffset);

        public Maths.BoundingBox3d drawText(Rendering.View.Camera cam, string s, Maths.Coord3d position, Align.Halign halign, Align.Valign valign, Colors.Color color, Maths.Coord3d sceneOffset)
		{
			return drawText(cam, s, position, halign, valign, color, defScreenOffset, sceneOffset);
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
