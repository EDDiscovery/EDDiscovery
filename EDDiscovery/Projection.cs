using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace Scatter
{
    static class Projection
    {
        static public PointF Project(double[] x, double s_x, double s_y, double f, double[] d_w, double azimuth, double elevation)
        {
            Algerbra.Matrix<double> Mext = GetMext(azimuth, elevation, d_w);
            Algerbra.Matrix<double> Mint = GetMint(s_x, s_y, f);
            Algerbra.Matrix<double> X_h = new Algerbra.Matrix<double>(4, 1);
            X_h.SetMatrix(new double[] { x[0], x[1], x[2], 1.0});
            //Debug.Print((Mint * Mext).ToString());
            Algerbra.Matrix<double> P = Mint * Mext * X_h;
            return new PointF((float)(P.GetValByIndex(0, 0) / P.GetValByIndex(2, 0)), (float)(P.GetValByIndex(1, 0) / P.GetValByIndex(2, 0)));
        }

        static public PointF[] ProjectVector(List<double[]> x, double s_x, double s_y, double f, double[] d_w, double azimuth, double elevation)
        {
            Algerbra.Matrix<double> Mext = GetMext(azimuth, elevation, d_w);
            Algerbra.Matrix<double> Mint = GetMint(s_x, s_y, f);
            Algerbra.Matrix<double> X_h = new Algerbra.Matrix<double>(4, 1);

            PointF[] Pvec = new PointF[x.Count];
            for (int i = 0; i < x.Count; i++)
            {
                X_h.SetMatrix(new double[] { x[i][0], x[i][1], x[i][2], 1.0 });
                Algerbra.Matrix<double> P = Mint * Mext * X_h;
                Pvec[i] = new PointF((float)(P.GetValByIndex(0, 0) / P.GetValByIndex(2, 0)), (float)(P.GetValByIndex(1, 0) / P.GetValByIndex(2, 0)));
            }
            return Pvec;
        }

        static Algerbra.Matrix<double> GetMint(double s_x, double s_y, double f)
        {
            Algerbra.Matrix<double> Mint = new Algerbra.Matrix<double>(3, 3);
            double o_x = s_x / 2;
            double o_y = s_y / 2;
            double a = 1;
            Mint.SetMatrix(new double[] { f, 0, o_x, 0, f * a, o_y, 0, 0, 1 });
            return Mint;
        }

        static Algerbra.Matrix<double> GetMext(double azimuth, double elevation, double[] d_w)
        {
            Algerbra.Matrix<double> R = RotationMatrix(azimuth, elevation);
            Algerbra.Matrix<double> dw = new Algerbra.Matrix<double>(3, 1);
            dw.SetMatrix(d_w);
            Algerbra.Matrix<double> Mext = R | (-R * dw);
            return Mext;
        }

        static Algerbra.Matrix<double> RotationMatrix(double azimuth, double elevation)
        {
            Algerbra.Matrix<double> R = new Algerbra.Matrix<double>(3, 3);
            R.SetMatrix(new double[] { Math.Cos(azimuth), 0, -Math.Sin(azimuth),
                                       Math.Sin(azimuth)*Math.Sin(elevation),  Math.Cos(elevation), Math.Cos(azimuth)*Math.Sin(elevation),
                                       Math.Cos(elevation)*Math.Sin(azimuth), -Math.Sin(elevation), Math.Cos(azimuth)*Math.Cos(elevation) });
            return R;
        }
    }
}
