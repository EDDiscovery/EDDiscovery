
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Chart;
using nzy3D.Events;

namespace nzy3D.Chart.Controllers
{

	public class AbstractController
	{

		protected List<Chart> _targets = new List<Chart>();

		protected List<IControllerEventListener> _controllerListeners = new List<IControllerEventListener>();
		public AbstractController()
		{
		}

		public AbstractController(Chart chart)
		{
			Register(chart);
		}

		public virtual void Register(Chart chart)
		{
			_targets.Add(chart);
		}

		public void Unregister(Chart chart)
		{
			_targets.Remove(chart);
		}

		protected Chart Chart {
			get { return _targets[0]; }
		}

		public virtual void Dispose()
		{
			_targets.Clear();
			_controllerListeners.Clear();
		}

		public void addControllerEventListener(IControllerEventListener listener)
		{
			_controllerListeners.Add(listener);
		}

		public void removeControllerEventListener(IControllerEventListener listener)
		{
			_controllerListeners.Remove(listener);
		}

		protected void fireControllerEvent(ControllerType type, object value)
		{
			ControllerEventArgs e = new ControllerEventArgs(this, type, value);
			foreach (IControllerEventListener aListener in _controllerListeners) {
				aListener.ControllerEventFired(e);
			}
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
