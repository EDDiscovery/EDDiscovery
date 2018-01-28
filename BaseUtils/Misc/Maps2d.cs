/*
 * Copyright © 2015 - 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace BaseUtils
{
    public class Map2d
    {
        public string FilePath;
        
        public Point TopLeft, TopRight, BottomLeft, BottomRight; // galaxy loc
        public double Area;                                         

        public Point pxTopLeft, pxTopRight, pxBottomLeft, pxBottomRight;        // bitmap

        public List<Point> Yaxispoints;
        private List<Double> polynoms; 

        public Map2d(string filename)
        {
            FilePath = filename;
            Yaxispoints = new List<Point>();
        }

        public Point LYPos(Point p)     // p is pixel.. (0,0) = top of map
        {
            return new Point(p.X * LYWidth / PixelWidth + TopLeft.X, TopRight.Y - p.Y * LYHeight / PixelHeight);
        }

        public int PixelWidth { get { return pxTopRight.X - pxTopLeft.X; } }
        public int PixelHeight { get { return pxBottomRight.Y - pxTopRight.Y; } }

        public int LYWidth { get { return BottomRight.X - BottomLeft.X; } }
        public int LYHeight { get { return (TopLeft.Y - BottomLeft.Y); } }

        public string FileName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(FilePath);
            }
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

        public static List<Map2d> LoadImages(string datapath)
        {
            List<Map2d> maps = new List<Map2d>();

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(datapath);
                FileInfo[] allFiles = dirInfo.GetFiles("*.json");

                if (allFiles != null)
                {
                    foreach (FileInfo fi in allFiles)
                    {
                        Map2d map = LoadImage(fi.FullName);
                        if (map != null)
                            maps.Add(map);
                    }

                    maps.Sort(delegate (Map2d p1, Map2d p2)      // biggest first.. name if same.. 
                    {
                        if (p1.Area == p2.Area)
                            return p1.FileName.CompareTo(p2.FileName);
                        else if (p1.Area < p2.Area)
                            return 1;
                        else
                            return -1;
                    }
                    );
                }
            }
            catch
            {
            }

            return maps;
        }

        static public Map2d LoadImage(string filename)          // give the JSON file name
        {
            JObject pfile = null;
            string json = BaseUtils.FileHelpers.TryReadAllTextFromFile(filename);

            if (json != null)
            {
                try
                {
                    pfile = (JObject)JObject.Parse(json);

                    Map2d map;

                    if (File.Exists(filename.Replace(".json", ".png")))
                        map = new Map2d(filename.Replace(".json", ".png"));
                    else
                        map = new Map2d(filename.Replace(".json", ".jpg"));

                    map.TopLeft = new Point(pfile["x1"].Int(), pfile["y1"].Int());
                    map.pxTopLeft = new Point(pfile["px1"].Int(), pfile["py1"].Int());

                    map.TopRight = new Point(pfile["x2"].Int(), pfile["y1"].Int());
                    map.pxTopRight = new Point(pfile["px2"].Int(), pfile["py1"].Int());

                    map.BottomLeft = new Point(pfile["x1"].Int(), pfile["y2"].Int());
                    map.pxBottomLeft = new Point(pfile["px1"].Int(), pfile["py2"].Int());

                    map.BottomRight = new Point(pfile["x2"].Int(), pfile["y2"].Int());
                    map.pxBottomRight = new Point(pfile["px2"].Int(), pfile["py2"].Int());

                    map.Area = (double)(map.TopRight.X - map.TopLeft.X) * (double)(map.TopLeft.Y - map.BottomRight.Y);

                    return map;
                }
                catch
                {
                }
            }

            return null;
        }
    }
}
