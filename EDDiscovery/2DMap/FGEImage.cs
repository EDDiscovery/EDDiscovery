using EDDiscovery2.EDDB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2
{
    public class FGEImage
    {
        public string FilePath;
        
        public Point TopLeft, TopRight, BottomLeft, BottomRight;
        public Point pxTopLeft, pxTopRight, pxBottomLeft, pxBottomRight;
        public List<Point> Yaxispoints;
        private List<Double> polynoms; 


        public FGEImage(string filename)
        {
            FilePath = filename;
            Yaxispoints = new List<Point>();
        }

        public string FileName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(FilePath);
            }
        }

        public string ToJson()
        {
            JObject jo = new JObject(
                new JProperty("Name", FilePath));


            jo.Add(AddPoint("TopLeft", TopLeft));
            jo.Add(AddPoint("TopRight", TopRight));
            jo.Add(AddPoint("BottomLeft", BottomLeft));
            jo.Add(AddPoint("BottomRight", BottomRight));

            jo.Add(AddPoint("pxTopLeft", pxTopLeft));
            jo.Add(AddPoint("pxTopRight", pxTopRight));
            jo.Add(AddPoint("pxBottomLeft", pxBottomLeft));
            jo.Add(AddPoint("pxBottomRight", pxBottomRight));

            jo.Add(new JProperty("YAxaispoints",
                new JArray(
                    from p in Yaxispoints
                    select new JObject(
                        new JProperty("x", p.X),
                        new JProperty("y", p.Y)
                    ))));


            return jo.ToString();
        }

        private JProperty AddPoint(string name, Point obj)
        {
            JObject jo = new JObject();
            jo.Add(new JProperty("x", obj.X));
            jo.Add(new JProperty("y", obj.Y));

            JProperty jp = new JProperty(name, jo);
            return jp;
        }

        public Point TransformCoordinate(Point coordinate)
        {
            int diffx1, diffx2, diffy1, diffy2;
            int diffpx1, diffpx2, diffpy1, diffpy2;

            // string json = ToJson();
            if (polynoms == null)
            {
                if (Yaxispoints!= null && Yaxispoints.Count>0)
                    polynoms = FindPolynomialLeastSquaresFit(Yaxispoints, 3);

            }
            //Transform trans;

            diffx1 = TopRight.X - TopLeft.X;
            diffx2 = BottomRight.X - BottomLeft.X;
            diffy1 = TopLeft.Y - BottomLeft.Y;
            diffy2 = TopRight.Y - BottomRight.Y;

            diffpx1 = pxTopRight.X - pxTopLeft.X;
            diffpx2 = pxBottomRight.X - pxBottomLeft.X;
            diffpy1 = pxTopLeft.Y - pxBottomLeft.Y;
            diffpy2 = pxTopRight.Y - pxBottomRight.Y;

            double dx1, dx2, dy1, dy2;

            dx1 = diffpx1 / (double)diffx1;
            dx2 = diffpx2 / (double)diffx2;
            dy1 = diffpy1 / (double)diffy1;
            dy2 = diffpy2 / (double)diffy2;

            Point newPoint = new Point(coordinate.X - BottomLeft.X, coordinate.Y - BottomLeft.Y);

            // Calculate dx and dy for point;
            double dx, dy;

            dx = dx2 + newPoint.Y / (double)diffy1 * (dx1 - dx2);
            dy = dy2 + newPoint.X / (double)diffx1 * (dy1- dy2);


            int x, y;

            x = (int)(newPoint.X * dx + pxBottomLeft.X + newPoint.Y / (double)diffy1 * (pxTopLeft.X - pxBottomLeft.X));
            if (polynoms != null)
                y = (int)(CalcYPixel(coordinate.Y) + newPoint.X / (double)diffx1 * (pxTopRight.Y - pxTopLeft.Y));
            else
                y = (int)(newPoint.Y * dy + pxBottomLeft.Y + newPoint.X / (double)diffx1 * (pxTopRight.Y - pxTopLeft.Y));


            return new Point(x, y);

        }


        private double CalcYPixel(double ypos)
        {
            double pos = 0;

            for (int ii=0; ii< polynoms.Count; ii++) 
            {
                pos +=  Math.Pow(ypos, ii) * polynoms[ii];
            }

            return pos;
        }

        // Find the least squares linear fit.
        public  List<double> FindPolynomialLeastSquaresFit(List<Point> points, int degree)
        {
            // Allocate space for (degree + 1) equations with 
            // (degree + 2) terms each (including the constant term).
            double[,] coeffs = new double[degree + 1, degree + 2];

            // Calculate the coefficients for the equations.
            for (int j = 0; j <= degree; j++)
            {
                // Calculate the coefficients for the jth equation.

                // Calculate the constant term for this equation.
                coeffs[j, degree + 1] = 0;
                foreach (Point pt in points)
                {
                    coeffs[j, degree + 1] -= Math.Pow(pt.X, j) * pt.Y;
                }

                // Calculate the other coefficients.
                for (int a_sub = 0; a_sub <= degree; a_sub++)
                {
                    // Calculate the dth coefficient.
                    coeffs[j, a_sub] = 0;
                    foreach (Point pt in points)
                    {
                        coeffs[j, a_sub] -= Math.Pow(pt.X, a_sub + j);
                    }
                }
            }

            // Solve the equations.
            double[] answer = GaussianElimination(coeffs);

            // Return the result converted into a List<double>.
            return answer.ToList<double>();
        }


        // Perform Gaussian elimination on these coefficients.
        // Return the array of values that gives the solution.
        private static double[] GaussianElimination(double[,] coeffs)
        {
            int max_equation = coeffs.GetUpperBound(0);
            int max_coeff = coeffs.GetUpperBound(1);
            for (int i = 0; i <= max_equation; i++)
            {
                // Use equation_coeffs[i, i] to eliminate the ith
                // coefficient in all of the other equations.

                // Find a row with non-zero ith coefficient.
                if (coeffs[i, i] == 0)
                {
                    for (int j = i + 1; j <= max_equation; j++)
                    {
                        // See if this one works.
                        if (coeffs[j, i] != 0)
                        {
                            // This one works. Swap equations i and j.
                            // This starts at k = i because all
                            // coefficients to the left are 0.
                            for (int k = i; k <= max_coeff; k++)
                            {
                                double temp = coeffs[i, k];
                                coeffs[i, k] = coeffs[j, k];
                                coeffs[j, k] = temp;
                            }
                            break;
                        }
                    }
                }

                // Make sure we found an equation with
                // a non-zero ith coefficient.
                double coeff_i_i = coeffs[i, i];
                if (coeff_i_i == 0)
                {
                    throw new ArithmeticException(String.Format(
                        "There is no unique solution for these points.",
                        coeffs.GetUpperBound(0) - 1));
                }

                // Normalize the ith equation.
                for (int j = i; j <= max_coeff; j++)
                {
                    coeffs[i, j] /= coeff_i_i;
                }

                // Use this equation value to zero out
                // the other equations' ith coefficients.
                for (int j = 0; j <= max_equation; j++)
                {
                    // Skip the ith equation.
                    if (j != i)
                    {
                        // Zero the jth equation's ith coefficient.
                        double coef_j_i = coeffs[j, i];
                        for (int d = 0; d <= max_coeff; d++)
                        {
                            coeffs[j, d] -= coeffs[i, d] * coef_j_i;
                        }
                    }
                }
            }

            // At this point, the ith equation contains
            // 2 non-zero entries:
            //      The ith entry which is 1
            //      The last entry coeffs[max_coeff]
            // This means Ai = equation_coef[max_coeff].
            double[] solution = new double[max_equation + 1];
            for (int i = 0; i <= max_equation; i++)
            {
                solution[i] = coeffs[i, max_coeff];
            }

            // Return the solution values.
            return solution;
        }

    }
}
