
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Events
{

	public class ObjectEventArgs : EventArgs
	{


		private object _objectChanged;
		public ObjectEventArgs(object objectChanged) : base()
		{
			_objectChanged = objectChanged;
		}

		public object ObjectChanged {
			get { return _objectChanged; }
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
