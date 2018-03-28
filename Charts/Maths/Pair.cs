
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace nzy3D.Maths
{

	public class Pair<X, Y>
	{
		//Implements ISerializable


		public X a;

		public Y b;

		public Pair(X aa, Y bb)
		{
			a = aa;
			b = bb;
		}

		//Public Function hashCode() As Integer
		//  Dim prime As Integer = 31
		//  Dim result As Integer = 1
		//  result = primer * result
		//End Function

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
