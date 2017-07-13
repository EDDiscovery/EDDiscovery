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
using EDDiscovery;
using EDDiscovery.DB;


namespace EMK.Cartography
{
	/// <summary>
	/// Basically a node is defined with a geographical position in space.
	/// It is also characterized with both collections of outgoing arcs and incoming arcs.
	/// </summary>
	[Serializable]
	public class Node
	{
		Point3D _Position;
		bool _Passable;
		ArrayList _IncomingArcs, _OutgoingArcs;
        SystemClassDB _si;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="PositionX">X coordinate.</param>
		/// <param name="PositionY">Y coordinate.</param>
		/// <param name="PositionZ">Z coordinate.</param>
		public Node(double PositionX, double PositionY, double PositionZ)
		{
			_Position = new Point3D(PositionX, PositionY, PositionZ);
			_Passable = true;
			_IncomingArcs = new ArrayList();
			_OutgoingArcs = new ArrayList();
		}


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="PositionX">X coordinate.</param>
        /// <param name="PositionY">Y coordinate.</param>
        /// <param name="PositionZ">Z coordinate.</param>
        /// <param name="si">Node name.</param>
        public Node(double PositionX, double PositionY, double PositionZ, SystemClassDB si)
        {
            _Position = new Point3D(PositionX, PositionY, PositionZ);
            _Passable = true;
            _IncomingArcs = new ArrayList();
            _OutgoingArcs = new ArrayList();
            _si = si;
        }


        public SystemClassDB System
        {
            get
            {
                return _si;
            }
        }

		/// <summary>
		/// Gets the list of the arcs that lead to this node.
		/// </summary>
		public IList IncomingArcs { get { return _IncomingArcs; } }

		/// <summary>
		/// Gets the list of the arcs that start from this node.
		/// </summary>
		public IList OutgoingArcs { get { return _OutgoingArcs; } }

		/// Gets/Sets the functional state of the node.
		/// 'true' means that the node is in its normal state.
		/// 'false' means that the node will not be taken into account (as if it did not exist).
		public bool Passable
		{
			set
			{
				foreach (Arc A in _IncomingArcs) A.Passable = value;
				foreach (Arc A in _OutgoingArcs) A.Passable = value;
				_Passable = value;
			}
			get { return _Passable; }		
		}

		/// <summary>
		/// Gets X coordinate.
		/// </summary>
		public double X { get { return Position.X; } }

		/// <summary>
		/// Gets Y coordinate.
		/// </summary>
		public double Y { get { return Position.Y; } }

		/// <summary>
		/// Gets Z coordinate.
		/// </summary>
		public double Z { get { return Position.Z; } }

		/// <summary>
		/// Modifies X, Y and Z coordinates
		/// </summary>
		/// <param name="PositionX">X coordinate.</param>
		/// <param name="PositionY">Y coordinate.</param>
		/// <param name="PositionZ">Z coordinate.</param>
		public void ChangeXYZ(double PositionX, double PositionY, double PositionZ)
		{
			Position = new Point3D(PositionX, PositionY, PositionZ);
		}

		/// <summary>
		/// Gets/Sets the geographical position of the node.
		/// </summary>
		/// <exception cref="ArgumentNullException">Cannot set the Position to null.</exception>
		public Point3D Position
		{
			set
			{
				if ( value==null ) throw new ArgumentNullException();
				foreach (Arc A in _IncomingArcs) A.LengthUpdated = false;
				foreach (Arc A in _OutgoingArcs) A.LengthUpdated = false;
				_Position = value;
			}
			get { return _Position; }
		}

		/// <summary>
		/// Gets the array of nodes that can be directly reached from this one.
		/// </summary>
		public Node[] AccessibleNodes
		{
			get
			{
				Node[] Tableau = new Node[_OutgoingArcs.Count];
				int i=0;
				foreach (Arc A in OutgoingArcs) Tableau[i++] = A.EndNode;
				return Tableau;
			}
		}

		/// <summary>
		/// Gets the array of nodes that can directly reach this one.
		/// </summary>
		public Node[] AccessingNodes
		{
			get
			{
				Node[] Tableau = new Node[_IncomingArcs.Count];
				int i=0;
				foreach (Arc A in IncomingArcs) Tableau[i++] = A.StartNode;
				return Tableau;
			}
		}
		
		/// <summary>
		/// Gets the array of nodes directly linked plus this one.
		/// </summary>
		public Node[] Molecule
		{
			get
			{
				int NbNodes = 1+_OutgoingArcs.Count+_IncomingArcs.Count;
				Node[] Tableau = new Node[NbNodes];
				Tableau[0] = this;
				int i=1;
				foreach (Arc A in OutgoingArcs) Tableau[i++] = A.EndNode;
				foreach (Arc A in IncomingArcs) Tableau[i++] = A.StartNode;
				return Tableau;
			}
		}
		
		/// <summary>
		/// Unlink this node from all current connected arcs.
		/// </summary>
		public void Isolate()
		{
			UntieIncomingArcs();
			UntieOutgoingArcs();
		}

		/// <summary>
		/// Unlink this node from all current incoming arcs.
		/// </summary>
		public void UntieIncomingArcs()
		{
			foreach (Arc A in _IncomingArcs)
				A.StartNode.OutgoingArcs.Remove(A);
			_IncomingArcs.Clear();
		}

		/// <summary>
		/// Unlink this node from all current outgoing arcs.
		/// </summary>
		public void UntieOutgoingArcs()
		{
			foreach (Arc A in _OutgoingArcs)
				A.EndNode.IncomingArcs.Remove(A);
			_OutgoingArcs.Clear();
		}

		/// <summary>
		/// Returns the arc that leads to the specified node if it exists.
		/// </summary>
		/// <exception cref="ArgumentNullException">Argument node must not be null.</exception>
		/// <param name="N">A node that could be reached from this one.</param>
		/// <returns>The arc leading to N from this / null if there is no solution.</returns>
		public Arc ArcGoingTo(Node N)
		{
			if ( N==null ) throw new ArgumentNullException();
			foreach (Arc A in _OutgoingArcs)
				if (A.EndNode == N) return A;
			return null;
		}

		/// <summary>
		/// Returns the arc that arc that comes to this from the specified node if it exists.
		/// </summary>
		/// <exception cref="ArgumentNullException">Argument node must not be null.</exception>
		/// <param name="N">A node that could reach this one.</param>
		/// <returns>The arc coming to this from N / null if there is no solution.</returns>
		public Arc ArcComingFrom(Node N)
		{
			if ( N==null ) throw new ArgumentNullException();
			foreach (Arc A in _IncomingArcs)
				if (A.StartNode == N) return A;
			return null;
		}
		
		void Invalidate()
		{
			foreach (Arc A in _IncomingArcs) A.LengthUpdated = false;
			foreach (Arc A in _OutgoingArcs) A.LengthUpdated = false;
		}

		/// <summary>
		/// object.ToString() override.
		/// Returns the textual description of the node.
		/// </summary>
		/// <returns>String describing this node.</returns>
		public override string ToString() { return Position.ToString(); }

		/// <summary>
		/// Object.Equals override.
		/// Tells if two nodes are equal by comparing positions.
		/// </summary>
		/// <exception cref="ArgumentException">A Node cannot be compared with another type.</exception>
		/// <param name="O">The node to compare with.</param>
		/// <returns>'true' if both nodes are equal.</returns>
		public override bool Equals(object O)
		{
			Node N = (Node)O;
			if ( N==null ) throw new ArgumentException("Type "+O.GetType()+" cannot be compared with type "+GetType()+" !");
			return Position.Equals(N.Position);
		}

		/// <summary>
		/// Returns a copy of this node.
		/// </summary>
		/// <returns>The reference of the new object.</returns>
		public object Clone()
		{
			Node N = new Node(X, Y, Z);
			N._Passable = _Passable;
			return N;
		}

		/// <summary>
		/// Object.GetHashCode override.
		/// </summary>
		/// <returns>HashCode value.</returns>
		public override int GetHashCode() { return Position.GetHashCode(); }

		/// <summary>
		/// Returns the euclidian distance between two nodes : Sqrt(Dx²+Dy²+Dz²)
		/// </summary>
		/// <param name="N1">First node.</param>
		/// <param name="N2">Second node.</param>
		/// <returns>Distance value.</returns>
		public static double EuclidianDistance(Node N1, Node N2)
		{
			return Math.Sqrt( SquareEuclidianDistance(N1, N2) );
		}

		/// <summary>
		/// Returns the square euclidian distance between two nodes : Dx²+Dy²+Dz²
		/// </summary>
		/// <exception cref="ArgumentNullException">Argument nodes must not be null.</exception>
		/// <param name="N1">First node.</param>
		/// <param name="N2">Second node.</param>
		/// <returns>Distance value.</returns>
		public static double SquareEuclidianDistance(Node N1, Node N2)
		{
			if ( N1==null || N2==null ) throw new ArgumentNullException();
			double DX = N1.Position.X - N2.Position.X;
			double DY = N1.Position.Y - N2.Position.Y;
			double DZ = N1.Position.Z - N2.Position.Z;
			return DX*DX+DY*DY+DZ*DZ;
		}

		/// <summary>
		/// Returns the manhattan distance between two nodes : |Dx|+|Dy|+|Dz|
		/// </summary>
		/// <exception cref="ArgumentNullException">Argument nodes must not be null.</exception>
		/// <param name="N1">First node.</param>
		/// <param name="N2">Second node.</param>
		/// <returns>Distance value.</returns>
		public static double ManhattanDistance(Node N1, Node N2)
		{
			if ( N1==null || N2==null ) throw new ArgumentNullException();
			double DX = N1.Position.X - N2.Position.X;
			double DY = N1.Position.Y - N2.Position.Y;
			double DZ = N1.Position.Z - N2.Position.Z;
			return Math.Abs(DX)+Math.Abs(DY)+Math.Abs(DZ);
		}

		/// <summary>
		/// Returns the maximum distance between two nodes : Max(|Dx|, |Dy|, |Dz|)
		/// </summary>
		/// <exception cref="ArgumentNullException">Argument nodes must not be null.</exception>
		/// <param name="N1">First node.</param>
		/// <param name="N2">Second node.</param>
		/// <returns>Distance value.</returns>
		public static double MaxDistanceAlongAxis(Node N1, Node N2)
		{
			if ( N1==null || N2==null ) throw new ArgumentNullException();
			double DX = Math.Abs(N1.Position.X - N2.Position.X);
			double DY = Math.Abs(N1.Position.Y - N2.Position.Y);
			double DZ = Math.Abs(N1.Position.Z - N2.Position.Z);
			return Math.Max(DX, Math.Max(DY, DZ));
		}
		
		/// <summary>
		/// Returns the bounding box that wraps the specified list of nodes.
		/// </summary>
		/// <exception cref="ArgumentException">The list must only contain elements of type Node.</exception>
		/// <exception cref="ArgumentException">The list of nodes is empty.</exception>
		/// <param name="NodesGroup">The list of nodes to wrap.</param>
		/// <param name="MinPoint">The point of minimal coordinates for the box.</param>
		/// <param name="MaxPoint">The point of maximal coordinates for the box.</param>
		static public void BoundingBox(IList NodesGroup, out double[] MinPoint, out double[] MaxPoint)
		{
			Node N1 = NodesGroup[0] as Node;
			if ( N1==null ) throw new ArgumentException("The list must only contain elements of type Node.");
			if ( NodesGroup.Count==0 ) throw new ArgumentException("The list of nodes is empty.");
			int Dim = 3;
			MinPoint = new double[Dim];
			MaxPoint = new double[Dim];
			for (int i=0; i<Dim; i++) MinPoint[i]=MaxPoint[i]=N1.Position[i];
			foreach ( Node N in NodesGroup )
			{
				for ( int i=0; i<Dim; i++ )
				{
					if ( MinPoint[i]>N.Position[i] ) MinPoint[i]=N.Position[i];
					if ( MaxPoint[i]<N.Position[i] ) MaxPoint[i]=N.Position[i];
				}
			}
		}
	}
}

