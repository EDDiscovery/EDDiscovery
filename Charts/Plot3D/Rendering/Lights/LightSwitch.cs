
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace nzy3D.Plot3D.Rendering.Lights
{

	public class LightSwitch
	{
		public static void Enable(int lightId)
		{
			switch (lightId) {
				case 0:
					GL.Enable(EnableCap.Light0);
					break;
				case 1:
					GL.Enable(EnableCap.Light1);
					break;
				case 2:
					GL.Enable(EnableCap.Light2);
					break;
				case 3:
					GL.Enable(EnableCap.Light3);
					break;
				case 4:
					GL.Enable(EnableCap.Light4);
					break;
				case 5:
					GL.Enable(EnableCap.Light5);
					break;
				case 6:
					GL.Enable(EnableCap.Light6);
					break;
				case 7:
					GL.Enable(EnableCap.Light7);
					break;
				default:
					throw new ArgumentException("Light id must belong to [0;7]", "lightId");
			}
		}
		public static void Disable(int lightId)
		{
			switch (lightId) {
				case 0:
					GL.Disable(EnableCap.Light0);
					break;
				case 1:
					GL.Disable(EnableCap.Light1);
					break;
				case 2:
					GL.Disable(EnableCap.Light2);
					break;
				case 3:
					GL.Disable(EnableCap.Light3);
					break;
				case 4:
					GL.Disable(EnableCap.Light4);
					break;
				case 5:
					GL.Disable(EnableCap.Light5);
					break;
				case 6:
					GL.Disable(EnableCap.Light6);
					break;
				case 7:
					GL.Disable(EnableCap.Light7);
					break;
				default:
					throw new ArgumentException("Light id must belong to [0;7]", "lightId");
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
