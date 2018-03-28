
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Factories
{
	public class NzyFactories
	{
		public static OrderingStrategyFactory ordering = new OrderingStrategyFactory();
		public static AxeFactory axe = new AxeFactory();
		public static CameraFactory camera = new CameraFactory();
		public static ViewFactory view = new ViewFactory();
		public static SceneFactory scene = new SceneFactory();
		//Public Shared renderer3d As New Renderer3dFactory
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
