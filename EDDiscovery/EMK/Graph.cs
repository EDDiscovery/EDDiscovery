// Copyright 2003 Eric Marchesin - <eric.marchesin@laposte.net>
//
// This source file(s) may be redistributed by any means PROVIDING they
// are not sold for profit without the authors expressed written consent,
// and providing that this notice and the authors name and all copyright
// notices remain intact.
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUR OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//-----------------------------------------------------------------------
using System;
using System.Collections;
using EMK.LightGeometry;
using System.Collections.Generic;


namespace EMK.Cartography
{
    /// <summary>
    /// Graph structure. It is defined with :
    /// It is defined with both a list of nodes and a list of arcs.
    /// </summary>
    [Serializable]
    public class Graph
    {
        List<Node> LN;
        List<Arc> LA;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Graph()
        {
            LN = new List<Node>();
            LA = new List<Arc>();
        }

        public int Count { get { return LN.Count; } }
        public Node GetN(int i) { return LN[i]; }
        public List<Node> GetNodes { get { return LN; } }

        /// <summary>
        /// Gets the List interface of the nodes in the graph.
        /// </summary>
        public List<Node> Nodes { get { return LN; } }

		/// <summary>
		/// Gets the List interface of the arcs in the graph.
		/// </summary>
		public List<Arc> Arcs { get { return LA; } }

		/// <summary>
		/// Empties the graph.
		/// </summary>
		public void Clear()
		{
			LN.Clear();
			LA.Clear();
		}

		/// <summary>
		/// Directly Adds a node to the graph.
		/// </summary>
		/// <param name="NewNode">The node to add.</param>
		/// <returns>'true' if it has actually been added / 'false' if the node is null or if it is already in the graph.</returns>
		public bool AddNode(Node NewNode)
		{
			if ( NewNode==null || LN.Contains(NewNode) ) return false;
			LN.Add(NewNode);
			return true;
		}

		/// <summary>
		/// Creates a node, adds to the graph and returns its reference.
		/// </summary>
		/// <param name="x">X coordinate.</param>
		/// <param name="y">Y coordinate.</param>
		/// <param name="z">Z coordinate.</param>
		/// <returns>The reference of the new node / null if the node is already in the graph.</returns>
		public Node AddNode(float x, float y, float z)
		{
			Node NewNode = new Node(x, y, z);
			return AddNode(NewNode) ? NewNode : null;
		}

		/// <summary>
		/// Directly Adds an arc to the graph.
		/// </summary>
		/// <exception cref="ArgumentException">Cannot add an arc if one of its extremity nodes does not belong to the graph.</exception>
		/// <param name="NewArc">The arc to add.</param>
		/// <returns>'true' if it has actually been added / 'false' if the arc is null or if it is already in the graph.</returns>
		public bool AddArc(Arc NewArc)
		{
			if ( NewArc==null || LA.Contains(NewArc) ) return false;
			if ( !LN.Contains(NewArc.StartNode) || !LN.Contains(NewArc.EndNode) )
				throw new ArgumentException("Cannot add an arc if one of its extremity nodes does not belong to the graph.");
			LA.Add(NewArc);
			return true;
		}

		/// <summary>
		/// Creates an arc between two nodes that are already registered in the graph, adds it to the graph and returns its reference.
		/// </summary>
		/// <exception cref="ArgumentException">Cannot add an arc if one of its extremity nodes does not belong to the graph.</exception>
		/// <param name="StartNode">Start node for the arc.</param>
		/// <param name="EndNode">End node for the arc.</param>
		/// <param name="Weight">Weight for the arc.</param>
		/// <returns>The reference of the new arc / null if the arc is already in the graph.</returns>
		public Arc AddArc(Node StartNode, Node EndNode, float Weight)
		{
			Arc NewArc = new Arc(StartNode, EndNode);
			NewArc.Weight = Weight;
			return AddArc(NewArc) ? NewArc : null;
		}

		/// <summary>
		/// Adds the two opposite arcs between both specified nodes to the graph.
		/// </summary>
		/// <exception cref="ArgumentException">Cannot add an arc if one of its extremity nodes does not belong to the graph.</exception>
		/// <param name="Node1"></param>
		/// <param name="Node2"></param>
		/// <param name="Weight"></param>
		public void Add2Arcs(Node Node1, Node Node2, float Weight)
		{
			AddArc(Node1, Node2, Weight);
			AddArc(Node2, Node1, Weight);
		}


        public Node AddNodeWithNoChk(Node NewNode)
        {
            LN.Add(NewNode);
            return NewNode;
        }
        public bool AddArcWithNoChk(Node StartNode, Node EndNode, float Weight)
        {
            Arc NewArc = new Arc(StartNode, EndNode);
            NewArc.Weight = Weight;
            LA.Add(NewArc);
            return true;
        }



		/// <summary>
		/// Removes a node from the graph as well as the linked arcs.
		/// </summary>
		/// <param name="NodeToRemove">The node to remove.</param>
		/// <returns>'true' if succeeded / 'false' otherwise.</returns>
		public bool RemoveNode(Node NodeToRemove)
		{
			if ( NodeToRemove==null ) return false;
			try
			{
				foreach ( Arc A in NodeToRemove.IncomingArcs )
				{
					A.StartNode.OutgoingArcs.Remove(A);
					LA.Remove(A);
				}
				foreach ( Arc A in NodeToRemove.OutgoingArcs )
				{
					A.EndNode.IncomingArcs.Remove(A);
					LA.Remove(A);
				}
				LN.Remove(NodeToRemove);
			}
			catch { return false; }
			return true;
		}

		/// <summary>
		/// Removes a node from the graph as well as the linked arcs.
		/// </summary>
		/// <param name="ArcToRemove">The arc to remove.</param>
		/// <returns>'true' if succeeded / 'false' otherwise.</returns>
		public bool RemoveArc(Arc ArcToRemove)
		{
			if ( ArcToRemove==null ) return false;
			try
			{
				LA.Remove(ArcToRemove);
				ArcToRemove.StartNode.OutgoingArcs.Remove(ArcToRemove);
				ArcToRemove.EndNode.IncomingArcs.Remove(ArcToRemove);
			}
			catch { return false; }
			return true;
		}

		/// <summary>
		/// Determines the bounding box of the entire graph.
		/// </summary>
		/// <exception cref="InvalidOperationException">Impossible to determine the bounding box for this graph.</exception>
		/// <param name="MinPoint">The point of minimal coordinates for the box.</param>
		/// <param name="MaxPoint">The point of maximal coordinates for the box.</param>
		public void BoundingBox(out double[] MinPoint, out double[] MaxPoint)
		{
			try
			{
				Node.BoundingBox(Nodes, out MinPoint, out MaxPoint);
			}
			catch(ArgumentException e)
			{ throw new InvalidOperationException("Impossible to determine the bounding box for this graph.\n", e); }
		}

		/// <summary>
		/// This function will find the closest node from a geographical position in space.
		/// </summary>
		/// <param name="PtX">X coordinate of the point from which you want the closest node.</param>
		/// <param name="PtY">Y coordinate of the point from which you want the closest node.</param>
		/// <param name="PtZ">Z coordinate of the point from which you want the closest node.</param>
		/// <param name="Distance">The distance to the closest node.</param>
		/// <param name="IgnorePassableProperty">if 'false', then nodes whose property Passable is set to false will not be taken into account.</param>
		/// <returns>The closest node that has been found.</returns>
		public Node ClosestNode(double PtX, double PtY, double PtZ, out double Distance, bool IgnorePassableProperty)
		{
			Node NodeMin = null;
			double DistanceMin = -1;
			Point3D P = new Point3D(PtX, PtY, PtZ);
			foreach ( Node N in LN )
			{
				if ( IgnorePassableProperty && N.Passable==false ) continue;
				double DistanceTemp = Point3D.DistanceBetween(N.Position, P);
				if ( DistanceMin==-1 || DistanceMin>DistanceTemp )
				{
					DistanceMin = DistanceTemp;
					NodeMin = N;
				}
			}
			Distance = DistanceMin;
			return NodeMin;
		}

		/// <summary>
		/// This function will find the closest arc from a geographical position in space using projection.
		/// </summary>
		/// <param name="PtX">X coordinate of the point from which you want the closest arc.</param>
		/// <param name="PtY">Y coordinate of the point from which you want the closest arc.</param>
		/// <param name="PtZ">Z coordinate of the point from which you want the closest arc.</param>
		/// <param name="Distance">The distance to the closest arc.</param>
		/// <param name="IgnorePassableProperty">if 'false', then arcs whose property Passable is set to false will not be taken into account.</param>
		/// <returns>The closest arc that has been found.</returns>
		public Arc ClosestArc(double PtX, double PtY, double PtZ, out double Distance, bool IgnorePassableProperty)
		{
			Arc ArcMin = null;
			double DistanceMin = -1;
			Point3D P = new Point3D(PtX, PtY, PtZ);
			foreach ( Arc A in LA )
			{
				if ( IgnorePassableProperty && A.Passable==false ) continue;
				Point3D Projection = Point3D.ProjectOnLine(P, A.StartNode.Position, A.EndNode.Position);
				double DistanceTemp = Point3D.DistanceBetween(P, Projection);
				if ( DistanceMin==-1 || DistanceMin>DistanceTemp )
				{
					DistanceMin = DistanceTemp;
					ArcMin = A;
				}
			}
			Distance = DistanceMin;
			return ArcMin;
		}
	}
}
