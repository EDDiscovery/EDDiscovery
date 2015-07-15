using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace EDDiscovery2._3DMap
{
    public interface IDrawingPrimative
    {
        PrimitiveType Type { get; }
        float Size { get; set; }
        Color Color { get; set; }
        void Draw();
    }

    public class PointData : IDrawingPrimative
    {
        public double x, y, z;

        public PointData(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public PrimitiveType Type { get { return PrimitiveType.Points; } }
        public Color Color { get; set; }
        public float Size { get; set; }

        public void Draw()
        {
            GL.PointSize(Size);

            GL.Begin(Type);
            GL.Color3(Color);
            GL.Vertex3(x, y, z);
            GL.End();
        }
    }

    public class LineData : IDrawingPrimative
    {
        public double x1, y1, z1;
        public double x2, y2, z2;

        public LineData(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.z1 = z1;
            this.x2 = x2;
            this.y2 = y2;
            this.z2 = z2;
        }

        public PrimitiveType Type { get { return PrimitiveType.Lines; } }
        public Color Color { get; set; }
        public float Size { get; set; }

        public void Draw()
        {
            GL.PointSize(Size);

            GL.Begin(Type);
            GL.Color3(Color);
            GL.Vertex3(x1, y1, z1);
            GL.Vertex3(x2, y2, z2);
            GL.End();
        }
    }
    public class Data3DSetClass<T> : IData3DSet where T : IDrawingPrimative
    {
        public string Name;
        private readonly Color color;
        private readonly float pointSize;
        private readonly List<T> primatives;

        public bool Visible;

        public Data3DSetClass(string name, Color color, float pointsize)
        {
            Name = name;
            this.color = color;
            pointSize = pointsize;
            primatives = new List<T>();
            Visible = true;
        }


        public void Add(T primative)
        {
            primative.Color = color;
            primative.Size = pointSize;
            primatives.Add(primative);
        }

        public void DrawAll()
        {
            if (!Visible) return;

            foreach (var primative in primatives)
            {
                primative.Draw();
            }
        }
    }

    public interface IData3DSet
    {
        void DrawAll();
    }
}
