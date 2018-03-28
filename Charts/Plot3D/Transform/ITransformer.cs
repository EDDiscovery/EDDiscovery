
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;

namespace nzy3D.Plot3D.Transform
{

	public interface ITransformer
	{

			// Execute the effective GL transformation held by this class.
		void Execute();
		Coord3d Compute(Coord3d input);
		// Apply the transformations to the input coordinates. (Warning: this method is a utility that may not be implemented.)

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
