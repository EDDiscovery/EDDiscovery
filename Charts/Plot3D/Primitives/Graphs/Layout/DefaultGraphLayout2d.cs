
using Microsoft.VisualBasic;
using nzy3D.Maths;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
namespace nzy3D.Plot3D.Primitives.Graphs.Layout
{

	public class DefaultGraphLayout2d<V> : IGraphLayout2d<V>
	{

        private List<Tuple<V, Maths.Coord2d>> _values = new List<Tuple<V,Maths.Coord2d>>();
        /*
		public Maths.Coord2d getV(V v)
		{
            return new Maths.Coord2d();
		}

		public System.Collections.Generic.List<Maths.Coord2d> values()
		{
            return _values;
		}
        */



        public Coord2d VertexPosition
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Coord2d getV(V v)
        {
            throw new NotImplementedException();
        }

        public List<Coord2d> values()
        {
            throw new NotImplementedException();
        }
    }

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
