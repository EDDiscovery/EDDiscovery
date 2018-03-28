
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using nzy3D.Maths;
using OpenTK.Graphics.OpenGL;

namespace nzy3D.Plot3D.Transform
{

	public class Transform
	{


		private List<ITransformer> _sequence;
		public Transform()
		{
			_sequence = new List<ITransformer>();
		}

		public Transform(ITransformer transformer)
		{
			_sequence = new List<ITransformer>();
			_sequence.Add(transformer);
		}

		public Transform(Transform transform)
		{
			_sequence = new List<ITransformer>();
			foreach (ITransformer nextT in transform.Sequence) {
				_sequence.Add(nextT);
			}
		}

        public IEnumerable<ITransformer> Sequence
        {
			get { return _sequence; }
		}

		public void Add(ITransformer nextT)
		{
			_sequence.Add(nextT);
		}

		public void Add(Transform transform)
		{
			foreach (ITransformer nextT in transform.Sequence) {
				_sequence.Add(nextT);
			}
		}


		public void Execute()
		{
		}

		public void Execute(bool loadIdentity)
		{
			if (loadIdentity) {
				GL.LoadIdentity();
			}
			foreach (ITransformer nextT in _sequence) {
				nextT.Execute();
			}
		}

		public Coord3d Compute(Coord3d input)
		{
			Coord3d output = (Coord3d)input.Clone();
			foreach (ITransformer nextT in _sequence) {
				output = nextT.Compute(output);
			}
			return output;
		}

		/// <summary>
		/// Returns the string representation
		/// </summary>
		public override string ToString()
		{
			string txt = "";
			foreach (ITransformer nextT in _sequence) {
				txt += " * " + nextT.ToString();
			}
			return txt;
		}

	}

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
