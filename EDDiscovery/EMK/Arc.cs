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
using EMK.LightGeometry;


namespace EMK.Cartography
{
	/// <summary>
	/// An arc is defined with its two extremity nodes StartNode and EndNode therefore it is oriented.
	/// It is also characterized by a crossing factor named 'Weight'.
	/// This value represents the difficulty to reach the ending node from the starting one.
	/// </summary>
	[Serializable]
	public class Arc
	{
		Node _StartNode, _EndNode;
		double _Weight;
		bool _Passable;
		double _Length;
		bool _LengthUpdated;

		/// <summary>
		/// Arc constructor.
		/// </summary>
		/// <exception cref="ArgumentNullException">Extremity nodes cannot be null.</exception>
		/// <exception cref="ArgumentException">StartNode and EndNode must be different.</exception>
		/// <param name="Start">The node from which the arc starts.</param>
		/// <param name="End">The node to which the arc ends.</param>
		public Arc(Node Start, Node End)
		{
			StartNode = Start;
			EndNode = End;
			Weight = 1;
			LengthUpdated = false;
			Passable = true;
		}

		/// <summary>
		/// Gets/Sets the node from which the arc starts.
		/// </summary>
		/// <exception cref="ArgumentNullException">StartNode cannot be set to null.</exception>
		/// <exception cref="ArgumentException">StartNode cannot be set to EndNode.</exception>
		public Node StartNode
		{
			set
			{
				if ( value==null ) throw new ArgumentNullException("StartNode");
				if ( EndNode!=null && value.Equals(EndNode) ) throw new ArgumentException("StartNode and EndNode must be different");
				if ( _StartNode!=null ) _StartNode.OutgoingArcs.Remove(this);
				_StartNode = value;
				_StartNode.OutgoingArcs.Add(this);
			}
			get { return _StartNode; }
		}

		/// <summary>
		/// Gets/Sets the node to which the arc ends.
		/// </summary>
		/// <exception cref="ArgumentNullException">EndNode cannot be set to null.</exception>
		/// <exception cref="ArgumentException">EndNode cannot be set to StartNode.</exception>
		public Node EndNode
		{
			set
			{
				if ( value==null ) throw new ArgumentNullException("EndNode");
				if ( StartNode!=null && value.Equals(StartNode) ) throw new ArgumentException("StartNode and EndNode must be different");
				if ( _EndNode!=null ) _EndNode.IncomingArcs.Remove(this);
				_EndNode = value;
				_EndNode.IncomingArcs.Add(this);
			}
			get { return _EndNode; }
		}

		/// <summary>
		/// Sets/Gets the weight of the arc.
		/// This value is used to determine the cost of moving through the arc.
		/// </summary>
		public double Weight
		{
			set { _Weight = value; }
			get { return _Weight; }
		}

		/// <summary>
		/// Gets/Sets the functional state of the arc.
		/// 'true' means that the arc is in its normal state.
		/// 'false' means that the arc will not be taken into account (as if it did not exist or if its cost were infinite).
		/// </summary>
		public bool Passable
		{
			set { _Passable = value; }
			get { return _Passable; }		
		}

		internal bool LengthUpdated
		{
			set { _LengthUpdated = value; }
			get { return _LengthUpdated; }
		}

		/// <summary>
		/// Gets arc's length.
		/// </summary>
		public double Length
		{
			get
			{
				if ( LengthUpdated==false )
				{
					_Length = CalculateLength();
					LengthUpdated = true;
				}
				return _Length;
			}
		}

		/// <summary>
		/// Performs the calculous that returns the arc's length
		/// Can be overriden for derived types of arcs that are not linear.
		/// </summary>
		/// <returns></returns>
		virtual protected double CalculateLength()
		{
			return Point3D.DistanceBetween(_StartNode.Position, _EndNode.Position);
		}

		/// <summary>
		/// Gets the cost of moving through the arc.
		/// Can be overriden when not simply equals to Weight*Length.
		/// </summary>
		virtual public double Cost
		{
			get { return Weight*Length; }
		}

		/// <summary>
		/// Returns the textual description of the arc.
		/// object.ToString() override.
		/// </summary>
		/// <returns>String describing this arc.</returns>
		public override string ToString()
		{
			return _StartNode.ToString()+"-->"+_EndNode.ToString();
		}

		/// <summary>
		/// Object.Equals override.
		/// Tells if two arcs are equal by comparing StartNode and EndNode.
		/// </summary>
		/// <exception cref="ArgumentException">Cannot compare an arc with another type.</exception>
		/// <param name="O">The arc to compare with.</param>
		/// <returns>'true' if both arcs are equal.</returns>
		public override bool Equals(object O)
		{
			Arc A = (Arc) O;
			if ( A==null ) throw new ArgumentException("Cannot compare type "+GetType()+" with type "+O.GetType()+" !");
			return _StartNode.Equals(A._StartNode) && _EndNode.Equals(A._EndNode);
		}

		/// <summary>
		/// Object.GetHashCode override.
		/// </summary>
		/// <returns>HashCode value.</returns>
		public override int GetHashCode() { return (int)Length; }
	}
}

