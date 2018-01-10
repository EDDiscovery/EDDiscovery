using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Scatter
{
    public partial class ScatterPlot : UserControl
    {
        List<List<double[]>> Points = new List<List<double[]>>();
        List<PointF[]> ProjPoints = new List<PointF[]>();
        private double f = 1000;
        private double d = 5;
        private double[] d_w = new double[3];
        private double last_azimuth, azimuth = 0, last_elevation, elevation = 0;
        private bool leftMousePressed = false;
        private PointF ptMouseClick;

        public double Distance
        {
            get { return d; }
            set { d = (value >= 0.1) ? d = value : d; UpdateProjection(); }
        }

        public double F
        {
            get { return f; }
            set { f = value; UpdateProjection(); }
        }

        public double[] CameraPos
        {
            get { return d_w;}
            set { d_w = value; UpdateProjection(); }
        }

        public double Azimuth
        {
            get { return azimuth; }
            set { azimuth = value; UpdateProjection(); }
        }
        
        public double Elevation
        {
            get { return elevation; }
            set { elevation = value; UpdateProjection(); }
        }

        public ScatterPlot()
        {
            InitializeComponent();
            MouseWheelHandler.Add(this, MyOnMouseWheel);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        Color[] colorIdx = new Color[] { Color.Blue, Color.Red, Color.Green, Color.Orange, Color.Fuchsia, Color.Black };

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = this.CreateGraphics();
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, this.Width, this.Height));
            if (ProjPoints != null)
            {
                for (int i = 0; i < ProjPoints.Count; i++)
                {
                    foreach (PointF p in ProjPoints[i])
                    {
                        g.FillEllipse(new SolidBrush(colorIdx[i % colorIdx.Length]), new RectangleF(p.X, p.Y, 4, 4));
                    }
                }
            }
        }

        public void AddPoint(double x, double y, double z, int series)
        {
            if (Points.Count - 1 < series)
            {
                Points.Add(new List<double[]>());
            }

            Points[series].Add(new double[] { x, y, z });

            foreach (List<double[]> ser in Points)
            {
                if (ProjPoints.Count - 1 < series)
                    ProjPoints.Add(Projection.ProjectVector(ser, this.Width, this.Height, f, d_w, azimuth, elevation));
                else
                    ProjPoints[series] = Projection.ProjectVector(ser, this.Width, this.Height, f, d_w, azimuth, elevation);
            }
            this.Invalidate();
        }

        public void AddPoints(List<double[]> points)
        {
            List<double[]> _tmp = new List<double[]>(points);
            Points.Add(_tmp);
            ProjPoints.Add(Projection.ProjectVector(Points[Points.Count-1], this.Width, this.Height, f, d_w, azimuth, elevation));
            UpdateProjection();
        }

        public void Clear()
        {
            ProjPoints.Clear();
            Points.Clear();
            Azimuth = 0;
            Elevation = 0;
        }

        private void ScatterPlot_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftMousePressed)
            {
                azimuth = last_azimuth - (ptMouseClick.X - e.X) / 100;
                elevation = last_elevation + (ptMouseClick.Y - e.Y) / 100;
                UpdateProjection();
            }
        }

        private void ScatterPlot_SizeChanged(object sender, EventArgs e)
        {
            if (ProjPoints != null)
                UpdateProjection();
        }

        private void ScatterPlot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                leftMousePressed = true;
                ptMouseClick = new PointF(e.X, e.Y);
                last_azimuth = azimuth;
                last_elevation = elevation;
            }
        }

        private void ScatterPlot_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                leftMousePressed = false;
        }

        private void MyOnMouseWheel(MouseEventArgs e)
        {
            Distance += -e.Delta / 500D;
        }

        private void UpdateProjection()
        {
            if (ProjPoints == null)
                return;
            double x = d * Math.Cos(elevation) * Math.Cos(azimuth);
            double y = d * Math.Cos(elevation) * Math.Sin(azimuth);
            double z = d * Math.Sin(elevation);
            d_w = new double[3] { -y, z, -x };
            for (int i = 0; i < ProjPoints.Count; i++) 
                ProjPoints[i] = Projection.ProjectVector(Points[i], this.Width, this.Height, f, d_w, azimuth, elevation);
            this.Invalidate();
        }

    }


}
