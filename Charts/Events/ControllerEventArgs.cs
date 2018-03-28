
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Chart.Controllers;

namespace nzy3D.Events
{

	public class ControllerEventArgs : ObjectEventArgs
	{

		private ControllerType _type;

		private object _value;
		public enum FieldChanged : int
		{
			Data = 0,
			Transform = 1,
			Color = 2,
			Metadata = 3,
			Displayed = 4
		}

		public ControllerEventArgs(object objectChanged, ControllerType type, object value) : base(objectChanged)
		{
			_type = type;
			_value = value;
		}

		public ControllerType Type {
			get { return _type; }
		}

		public object Value {
			get { return _value; }
		}

		public override string ToString()
		{
			return ("ControllerEvent(type,value): " + Type + ", " + Value);
		}

	}

}


//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
