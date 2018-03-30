
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Plot3D.Primitives;
using nzy3D.Plot3D.Rendering.View;

namespace nzy3D.Plot3D.Rendering.Ordering
{

	/// <summary>
	/// The <see cref="DefaultOrderingStrategy"/> let drawables be displayed in their original order
	/// @author Martin Pernollet
	/// </summary>
	public class DefaultOrderingStrategy : AbstractOrderingStrategy
	{

		public override int Compare(Primitives.AbstractDrawable d1, Primitives.AbstractDrawable d2)
		{
			return 0;
		}

		//
		// Operation must be:
		// symetric: compare(a,b)=-compare(b,a)
		// transitive: ((compare(x, y)>0) && (compare(y, z)>0)) implies compare(x, z)>0    true if all Drawables and the Camera don't change position!
		// consistency?: compare(x, y)==0  implies that sgn(compare(x, z))==sgn(compare(y, z))
		//

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
