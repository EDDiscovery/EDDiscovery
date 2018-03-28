
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Chart;
using nzy3D.Chart.Controllers.Camera;
using nzy3D.Maths;

namespace nzy3D.Chart.Controllers.Thread.Camera
{

	public class CameraThreadController : AbstractCameraController
	{

		private Coord2d _move;
		private System.Threading.Thread _process;
			////1000/25; // nb milisecond wait between two frames
		private int _sleep = 1;

		private float _step = 0.0005f;
		public CameraThreadController()
		{
		}

		public CameraThreadController(Chart chart)
		{
			Register(chart);
		}

		public override void Dispose()
		{
			StopT();
			base.Dispose();
		}

		public void Start()
		{
			if ((_process == null)) {
				_process = new System.Threading.Thread(Run);
				_process.Name = "CameraThreadController, embedded by ChartThreadController";
				_process.Start();
			}
		}

		public void StopT()
		{
			if ((_process != null)) {
				_process.Interrupt();
				_process = null;
			}
		}

		public void Run()
		{
			_move = new Coord2d(_step, 0);
			while ((_process != null)) {
				try {
					Rotate(_move);
                    System.Threading.Thread.Sleep(_sleep);
				} catch (System.Threading.ThreadInterruptedException ex) {
					_process = null;
				}
			}
		}

		public float MoveStep {
			get { return _step; }
			set { _step = value; }
		}



	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
