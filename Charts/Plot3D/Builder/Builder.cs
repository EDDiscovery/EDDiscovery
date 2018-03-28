
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Colors;
using nzy3D.Colors.ColorMaps;
using nzy3D.Maths;
using nzy3D.Plot3D.Builder.Concrete;
using nzy3D.Plot3D.Builder.Delaunay;
using nzy3D.Plot3D.Primitives;

namespace nzy3D.Plot3D.Builder
{

	public class Builder
	{

		static internal IColorMap _colorMap = new ColorMapRainbow();
		static internal Color _colorFactor = new Color(1, 1, 1, 1);
		static internal bool _faceDisplayed = true;
		static internal bool _wireframeDisplayed = false;

		static internal Color _wireframeColor = Color.BLACK;
		public static Shape buildOrthonomal(OrthonormalGrid grid, Mapper mapper)
		{
			OrthonormalTessellator tesselator = new OrthonormalTessellator();
			return (Shape)tesselator.build(grid.Apply(mapper));
		}

		public static Shape buildRing(OrthonormalGrid grid, Mapper mapper, float ringMin, float ringMax)
		{
			RingTessellator tesselator = new RingTessellator(ringMin, ringMax, new ColorMapper(new ColorMapRainbow(), 0, 1), Color.BLACK);
			return (Shape)tesselator.build(grid.Apply(mapper));
		}

		public static Shape buildRing(OrthonormalGrid grid, Mapper mapper, float ringMin, float ringMax, ColorMapper cmap, Color factor)
		{
			RingTessellator tesselator = new RingTessellator(ringMin, ringMax, cmap, factor);
			return (Shape)tesselator.build(grid.Apply(mapper));
		}

		public static Shape buildDelaunay(List<Coord3d> coordinates)
		{
			DelaunayTessellator tesselator = new DelaunayTessellator();
			return (Shape)tesselator.build(coordinates);
		}

		// BIG SURFACE
		public static CompileableComposite buildOrthonormalBig(OrthonormalGrid grid, Mapper mapper)
		{
			OrthonormalTessellator tesselator = new OrthonormalTessellator();
			Shape s1 = (Shape)tesselator.build(grid.Apply(mapper));
			return buildComposite(applyStyling(s1));
		}

		public static Shape applyStyling(Shape s)
		{
			s.ColorMapper = new ColorMapper(_colorMap, s.Bounds.zmin, s.Bounds.zmax);
			s.FaceDisplayed = _faceDisplayed;
			s.WireframeDisplayed = _wireframeDisplayed;
			s.WireframeColor = _wireframeColor;
			return s;
		}

		public static CompileableComposite buildComposite(Shape s)
		{
			CompileableComposite sls = new CompileableComposite();
			sls.Add(s.GetDrawables);
			sls.ColorMapper = new ColorMapper(_colorMap, sls.Bounds.zmin, sls.Bounds.zmax, _colorFactor);
			sls.FaceDisplayed = s.FaceDisplayed;
			sls.WireframeDisplayed = s.WireframeDisplayed;
			sls.WireframeColor = s.WireframeColor;
			return sls;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
