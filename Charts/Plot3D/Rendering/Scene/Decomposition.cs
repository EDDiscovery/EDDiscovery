
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Plot3D.Primitives;

namespace nzy3D.Plot3D.Rendering.Scene
{

	public class Decomposition
	{

		public static List<AbstractDrawable> GetDecomposition(List<AbstractDrawable> drawables)
		{
			List<AbstractDrawable> monotypes = new List<AbstractDrawable>();
			foreach (AbstractDrawable c in drawables) {
				if ((c != null) && c.Displayed) {
                    AbstractComposite cAC = c as AbstractComposite;
                    AbstractDrawable cAD = c as AbstractDrawable;
                    if (cAC != null)
                    {
                        monotypes.AddRange(GetDecomposition(cAC));
                    }
                    else if (cAD != null)
                    {
                        monotypes.Add(cAD);
					}
				}
			}
			return monotypes;
		}

		/// <summary>
		/// Recursively expand all monotype Drawables from the given Composite
		/// </summary>
		public static List<AbstractDrawable> GetDecomposition(AbstractComposite input)
		{
			List<AbstractDrawable> selection = new List<AbstractDrawable>();
			foreach (AbstractDrawable c in input.GetDrawables) {
				if ((c != null) && c.Displayed) {
                    AbstractComposite cAC = c as AbstractComposite;
                    AbstractDrawable cAD = c as AbstractDrawable;
                    if (cAC != null)
                    {
                        selection.AddRange(GetDecomposition(cAC));
                    }
                    else if (cAD != null)
                    {
                        selection.Add(cAD);
					}
				}
			}
			return selection;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
