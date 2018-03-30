
using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace nzy3D.Maths.Graphs
{

	public class DefaultGraph<V, E> : IGraph<V, E> where E : class
	{

		internal List<V> vertices = new List<V>();
		internal List<E> edges = new List<E>();
        internal List<Tuple<E, V>> edgeStart = new List<Tuple<E, V>>();
        internal List<Tuple<E, V>> edgeStop = new List<Tuple<E, V>>();

		internal Random r = new Random();
		public void addEdge(E edge, V v1, V v2)
		{
			edges.Add(edge);
			edgeStart.Add(new Tuple<E, V>(edge, v1));
			edgeStop.Add(new Tuple<E, V>(edge, v2));
		}

		public void addVertex(V vertex)
		{
			vertices.Add(vertex);
		}

		public System.Collections.Generic.List<E> getEdges()
		{
			return edges;
		}

		public V getEdgeStartVertex(E e)
		{
			return edgeStart.Where(p => p.Item1 == e).Single().Item2;
		}

		public V getEdgeStopVertex(E e)
		{
            return edgeStop.Where(p => p.Item1 == e).Single().Item2;
		}

		public V getRandomVertex()
		{
			return getVertex(r.Next(0, vertices.Count - 1));
		}

		public V getVertex(int i)
		{
			return vertices[i];
		}

		public System.Collections.Generic.List<V> getVertices()
		{
			return vertices;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
