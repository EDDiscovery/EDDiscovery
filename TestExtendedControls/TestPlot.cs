﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;


namespace DialogTest
{
    public partial class TestPlot : Form
    {
        private static int n = 100000;

        public TestPlot()
        {
            this.InitializeComponent();

            //
            var points = new List<ItemType3>(n);
            for (int i = 0; i < n; i++)
            {
                var x = (double)i / (n - 1);
                points.Add(new ItemType3(x, y(x)));
            }

            var model = new PlotModel { Title = "Using reflection with path (slow)" };
            model.Series.Add(new LineSeries { ItemsSource = points, DataFieldX = "Point.X", DataFieldY = "Point.Y" });
            
            //
            this.plotView1.Model = model;
        }

        public static PlotModel UsingIDataPoint()
        {
            var points = new List<DataPoint>(n);
            for (int i = 0; i < n; i++)
            {
                var x = (double)i / (n - 1);
                points.Add(new DataPoint(x, y(x)));
            }

            var model = new PlotModel { Title = "Using IDataPoint" };
            model.Series.Add(new LineSeries { ItemsSource = points });
            return model;
        }


        public static PlotModel UsingIDataPointProvider()
        {
            var points = new List<PointType1>(n);
            for (int i = 0; i < n; i++)
            {
                var x = (double)i / (n - 1);
                points.Add(new PointType1(x, y(x)));
            }

            var model = new PlotModel { Title = "Items implementing IDataPointProvider" };
            model.Series.Add(new LineSeries { ItemsSource = points });
            return model;
        }

        public static PlotModel UsingMappingProperty()
        {
            var points = new List<PointType2>(n);
            for (int i = 0; i < n; i++)
            {
                var x = (double)i / (n - 1);
                points.Add(new PointType2(x, y(x)));
            }

            var model = new PlotModel { Title = "Using Mapping property" };
            model.Series.Add(
                new LineSeries
                {
                    ItemsSource = points,
                    Mapping = item => new DataPoint(((PointType2)item).Abscissa, ((PointType2)item).Ordinate)
                });
            return model;
        }

        public static PlotModel UsingReflection()
        {
            var points = new List<PointType2>(n);
            for (int i = 0; i < n; i++)
            {
                var x = (double)i / (n - 1);
                points.Add(new PointType2(x, y(x)));
            }

            var model = new PlotModel { Title = "Using reflection (slow)" };
            model.Series.Add(new LineSeries { ItemsSource = points, DataFieldX = "Abscissa", DataFieldY = "Ordinate" });
            return model;
        }

        public static PlotModel UsingReflectionPath()
        {
            var points = new List<ItemType3>(n);
            for (int i = 0; i < n; i++)
            {
                var x = (double)i / (n - 1);
                points.Add(new ItemType3(x, y(x)));
            }

            var model = new PlotModel { Title = "Using reflection with path (slow)" };
            model.Series.Add(new LineSeries { ItemsSource = points, DataFieldX = "Point.X", DataFieldY = "Point.Y" });
            return model;
        }

        private class PointType1 : IDataPointProvider, ICodeGenerating
        {
            public PointType1(double abscissa, double ordinate)
            {
                this.Abscissa = abscissa;
                this.Ordinate = ordinate;
            }

            public double Abscissa { get; private set; }

            public double Ordinate { get; private set; }

            public DataPoint GetDataPoint()
            {
                return new DataPoint(Abscissa, Ordinate);
            }

            public string ToCode()
            {
                return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.Abscissa, this.Ordinate);
            }
        }

        private class PointType2
        {
            public PointType2(double abscissa, double ordinate)
            {
                this.Abscissa = abscissa;
                this.Ordinate = ordinate;
            }

            public double Abscissa { get; private set; }

            public double Ordinate { get; private set; }
        }

        private class ItemType3
        {
            public ItemType3(double x, double y)
            {
                this.Point = new ScreenPoint(x, y);
            }

            public ScreenPoint Point { get; private set; }
        }

        /// <summary>
        /// Evaluates a chaotic function.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <returns>A y value.</returns>
        private static double y(double x)
        {
            // http://computing.dcu.ie/~humphrys/Notes/Neural/chaos.html
            return Math.Sin(3 / x) * Math.Sin(5 / (1 - x));
        }
    }
}
