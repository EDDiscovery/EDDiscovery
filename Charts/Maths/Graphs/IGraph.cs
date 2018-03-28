
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths.Graphs
{

	public interface IGraph<V, E>
	{
		void addVertex(V vertex);
		void addEdge(E edge, V v1, V v2);
		V getVertex(int i);
		V getRandomVertex();
		List<V> getVertices();
		List<E> getEdges();
		V getEdgeStartVertex(E e);
		V getEdgeStopVertex(E e);
	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
