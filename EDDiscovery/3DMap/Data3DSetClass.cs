using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace EDDiscovery2._3DMap
{
    public class Data3DSetClass
    {
        public class VertexData
        {
            public double x, y, z;

            public VertexData(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }

        public string name;
        public PrimitiveType primtype;
        public Color color;
        public float pointSize;
        public List<VertexData> points;

        public bool Visible;

        public Data3DSetClass(string Name, PrimitiveType type, Color color, float pointsize)
        {
            name = Name;
            primtype = type;
            this.color = color;
            pointSize = pointsize;
            points = new List<VertexData>();
            Visible = true;
        }


        public void AddPoint(double x, double y, double z)
        {
            VertexData vertex = new VertexData(x, y, z);
            points.Add(vertex);
        }

        public void DrawPoints()
        {
            if (Visible)
            {
                GL.PointSize(pointSize);

                GL.Begin(primtype);
                GL.Color3(color);

                foreach (VertexData vertex in points)
                {
                    GL.Vertex3(vertex.x, vertex.y, vertex.z);
                }
                GL.End();
            }
        }
    }
}
