using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EDDiscovery2._3DMap
{
    
    public class PolygonTriangulator
    {
        // some of it is from https://gist.github.com/KvanTTT/3855122 but a lot of it has been added/changed

        static bool Intersect(List<Vector2> polygon, int vertex1Ind, int vertex2Ind, int vertex3Ind)        // is any points within the triangle v1-v2-v3
        {
            float s1, s2, s3;
            for (int i = 0; i < polygon.Count; i++)
            {
                if ((i == vertex1Ind) || (i == vertex2Ind) || (i == vertex3Ind))
                    continue;

                s1 = PMSquare(polygon[vertex1Ind], polygon[vertex2Ind], polygon[i]);        // i against vector vertex1ind->vertex2Ind
                s2 = PMSquare(polygon[vertex2Ind], polygon[vertex3Ind], polygon[i]);        // i against vector vertex2ind->vertex3Ind
                                                                                            // in the direction A->B, PMSQUARE is positive if it to the right of the vector
                if (((s1 < 0) && (s2 > 0)) || ((s1 > 0) && (s2 < 0)))                       // if i is on different side to v1/v2 (signs differ)
                    continue;                                                               // if not, s1 and s2 are same sign (same side) continue

                s3 = PMSquare(polygon[vertex3Ind], polygon[vertex1Ind], polygon[i]);        // i against vector vertex3ind->vertex1ind

                if (((s3 >= 0) && (s2 >= 0)) || ((s3 <= 0) && (s2 <= 0)))                   // if i is on same side as s1 and s2.. its in the middle of the triangle.
                    return true;
            }

            return false;
        }

        public static bool InsidePolygon(List<Vector2> polygon, Vector2 point)             // Polygon is wound clockwise and must be convex
        {
            for (int i = 0; i < polygon.Count; i++)
            {
                int vertex2 = (i + 1) % polygon.Count;

                float s1 = PMSquare(polygon[i], polygon[vertex2], point);

                if (s1 < 0)
                    return false;
            }

            return true;
        }
                                                                                            // Polygon is wound clockwise and convex
        public static bool InsidePolygon(List<Vector2> polygon, Vector2 centre, float width, float height)                     
        {
            width /= 2;
            height /= 2;
            return InsidePolygon(polygon, new Vector2(centre.X - width, centre.Y - height)) &&
                    InsidePolygon(polygon, new Vector2(centre.X - width, centre.Y + height)) &&
                    InsidePolygon(polygon, new Vector2(centre.X + width, centre.Y + height)) &&
                    InsidePolygon(polygon, new Vector2(centre.X + width, centre.Y - height));
        }

        public static Vector2 Centroid(List<Vector2> polygon , out float area )              // Polygon is convex
        {
            float x = 0, y = 0;
            area = 0;
            for( int i = 0; i < polygon.Count; i++ )
            {
                int np = (i + 1) % polygon.Count;

                float second_factor = PMSquare(polygon[i], polygon[np]);

                x += (polygon[i].X + polygon[np].X) * second_factor;
                y += (polygon[i].Y + polygon[np].Y) * second_factor;
                area += second_factor;
            }

            area /= 2;
            x = x / 6 / area;
            y = y / 6 / area;
            return new Vector2(x, y);
        }

        static public Vector2 Centroids(List<List<Vector2>> polys)                          // Weighted average of convex polygons
        {                                                                                   // finds mean centre.
            Vector2 mean = new Vector2(0, 0);
            float totalweight = 0;
            foreach (List<Vector2> poly in polys)
            {
                float area;
                Vector2 pos = Centroid(poly, out area);
                pos *= area;
                mean += pos;
                totalweight += area;
            }

            if (totalweight > 0)
                mean /= totalweight;

            return mean;
        }

        public static float PolygonArea(List<Vector2> polygon)                               // Polygon area, and sign indicats winding. + means clockwise
        {
            float S = 0;
            if (polygon.Count >= 3)
            {
                for (int i = 0; i < polygon.Count - 1; i++)
                    S += PMSquare((Vector2)polygon[i], (Vector2)polygon[i + 1]);

                S += PMSquare((Vector2)polygon[polygon.Count - 1], (Vector2)polygon[0]);
            }

            return S/2;
        }

        static float PMSquare(Vector2 p1, Vector2 p2)
        {
            return (p2.X * p1.Y - p1.X * p2.Y);
        }

        static float PMSquare(Vector2 p1, Vector2 p2, Vector2 p3)       // p1,p2 is the line, p3 is the test point. which side of the line is it on?
        {
            return (p3.X - p1.X) * (p2.Y - p1.Y) - (p2.X - p1.X) * (p3.Y - p1.Y);
        }

        static List<List<Vector2>> SplitSelfJoiningPolygons(List<Vector2> Polygon)
        {
            int N = Polygon.Count;

            if (N >= 6)
            {
                for (int i = 0; i < N - 1; i++)
                {
                    for (int j = i + 1; j < N; j++)
                    {
                        if (Polygon[i].Equals(Polygon[j]))
                        {
                            // Split the polygon on the midpoint between the self-connection points
                            int start = i;
                            int end = j;

                            if (j - i >= N / 2)
                            {
                                start = j;
                                end = i + N;
                            }

                            if (end - start >= 4)
                            {
                                int mid = (start + end) / 2;
                                int outmid = (start + end + N) / 2;
                                int innerlen = (end - start);
                                int outerlen = N - innerlen;
                                int mp1 = start + 1;
                                int mp2 = end + 1;
                                float mindist = float.MaxValue;
                                float maxdist = 0;

                                // Find the nearest seam opposite this seam
                                for (int k = 1; k <= (mid - start); k++)
                                {
                                    for (int l = 1; l <= (outmid - end); l++)
                                    {
                                        float dist1 = (Polygon[(start + k) % N] - Polygon[(end + l) % N]).Length;
                                        float dist2 = (Polygon[(end - k) % N] - Polygon[(start + N - l) % N]).Length;

                                        if (dist1 > maxdist)
                                        {
                                            maxdist = dist1;
                                        }

                                        if (dist2 > maxdist)
                                        {
                                            maxdist = dist2;
                                        }

                                        // Avoid picking up points near the first seam
                                        if (dist1 < maxdist / 2 && dist1 < mindist)
                                        {
                                            mindist = dist1;
                                            mp1 = (start + k) % N;
                                            mp2 = (end + l) % N;
                                        }

                                        if (dist2 < maxdist / 2 && dist2 < mindist)
                                        {
                                            mindist = dist2;
                                            mp1 = (end - k) % N;
                                            mp2 = ((start + N) - l) % N;
                                        }
                                    }
                                }

                                int mp2l = (mp2 + N - mp1) % N;

                                var poly1 = Enumerable.Range(0, mp2l + 1).Select(n => Polygon[(n + mp1) % N]).ToList();
                                var poly2 = Enumerable.Range(mp2l, N - mp2l + 1).Select(n => Polygon[(n + mp1) % N]).ToList();

                                return SplitSelfJoiningPolygons(poly1).Concat(SplitSelfJoiningPolygons(poly2)).ToList();
                            }
                        }
                    }
                }
            }

            return new List<List<Vector2>> { Polygon };
        }

        public static List<List<Vector2>> Triangulate(List<Vector2> Polygon, bool triangulate = false)
        {
            var splitpolys = SplitSelfJoiningPolygons(Polygon);
            return splitpolys.SelectMany(p => _Triangulate(p, triangulate)).ToList();
        }

        static List<List<Vector2>> _Triangulate(List<Vector2> Polygon, bool triangulate = false)
        {
            var result = new List<List<Vector2>>();
            var tempPolygon = new List<Vector2>(Polygon);       // copy since we need to modify

            if (PolygonArea(tempPolygon) < 0)                  // make sure we wind in the same direction positive (clockwise)
                tempPolygon.Reverse();

            int begin_ind = 0;                                  // from point 0
            int N = Polygon.Count;

            while (N >= 3)
            {
                var convPolygon = new List<Vector2>();          // BUG in original code.. need a fresh one every time.
                int loop_ind = begin_ind;

                while (PMSquare(    tempPolygon[begin_ind],     // FIND next ear to remove, point +2 needs to be on the right, and not inside the triangle                   
                                    tempPolygon[(begin_ind + 1) % N],
                                    tempPolygon[(begin_ind + 2) % N]) < 0 ||
                                    Intersect(tempPolygon, begin_ind, (begin_ind + 1) % N, (begin_ind + 2) % N) == true
                      )
                {
                    begin_ind++;
                    begin_ind %= N;

                    if (begin_ind == loop_ind)
                    {
                        break;  // Break out if no ears can be found
                    }
                }

                int cur_ind = (begin_ind + 1) % N;
                convPolygon.Add(tempPolygon[begin_ind]);
                convPolygon.Add(tempPolygon[cur_ind]);
                convPolygon.Add(tempPolygon[(begin_ind + 2) % N]);

                if (triangulate == false)           // this goes thru and sees if we can find another part to add to the polygon
                {
                    int begin_ind1 = cur_ind;
                    while ( PMSquare(tempPolygon[cur_ind], tempPolygon[(cur_ind + 1) % N],tempPolygon[(cur_ind + 2) % N]) > 0 && 
                            (cur_ind + 2) % N != begin_ind )
                    {
                        if (Intersect(tempPolygon, begin_ind, (cur_ind + 1) % N, (cur_ind + 2) % N) == true ||
                                PMSquare(tempPolygon[begin_ind], tempPolygon[(begin_ind + 1) % N], tempPolygon[(cur_ind + 2) % N]) < 0
                           )
                        {
                            break;
                        }

                        convPolygon.Add(tempPolygon[(cur_ind + 2) % N]);
                        cur_ind++;
                        cur_ind %= N;
                    }
                }

                int Range = cur_ind - begin_ind;
                if (Range > 0)
                {
                    tempPolygon.RemoveRange(begin_ind + 1, Range);
                }
                else
                {
                    tempPolygon.RemoveRange(begin_ind + 1, N - begin_ind - 1);
                    tempPolygon.RemoveRange(0, cur_ind + 1);
                }

                N = tempPolygon.Count;
                begin_ind++;
                begin_ind %= N;

                if ( PolygonArea(convPolygon) != 0 )                 // algorithm can produce polygons in a straight line, reject them if they have no size (pos or neg)
                    result.Add(convPolygon);
            }

            return result;
        }

        static private int FlipOffset(int i) { return ((i & 1) == 0) ? ((i + 1) / 2) : (-(i + 1) / 2); }    // used to search

        // Polygon must be convex and clockwise, brute force but it works.
        static public void FitInsideConvexPoly(List<Vector2> points, Vector2 comparepoint, Vector2 startsize, Vector2 offsets,
                                                    ref float mindist, ref Vector2 bestpos , ref Vector2 bestsize, float minwidthallowed = 1 , float scaling = 0.9F)
        {
            Vector2 startpoint = comparepoint;

            if (!InsidePolygon(points, startpoint))                   // if given a point outside the polygon, no point starting there, pick the centroid
            {
                float area;
                startpoint = PolygonTriangulator.Centroid(points, out area);
            }

            while ( startsize.X > minwidthallowed )
            {
                Vector2 trysize = startsize;
                startsize *= scaling;

                bool foundoneatthissize = false;

                for (int i = 0; i < 1000; i++)
                {
                    Vector2 trypos = new Vector2(startpoint.X + FlipOffset(i / 50) * offsets.X, startpoint.Y + FlipOffset(i % 50) * offsets.Y);
                    bool inside = PolygonTriangulator.InsidePolygon(points, trypos, trysize.X , trysize.Y);

                    float fromcompare = (trypos - comparepoint).Length;

                    //string debugline = String.Format("  Poly {0} At {1} {2} try {3} dist {4} inside {5} step {6}", polynum, trypos.X, trypos.Y, trysize, textfromgeocentre, inside, i);
                    //writer.WriteLine(debugline);

                    if (inside)
                    {
                        if (fromcompare < mindist)
                        {
                            mindist = fromcompare;
                            bestpos = trypos;
                            bestsize = trysize;
                            //writer.WriteLine(String.Format("  {0} Best pos at {1} size {2} dist {3}", polynum, i, trysize, mindist));
                            foundoneatthissize = true;
                        }
                    }
                }

                if (foundoneatthissize)
                    break;
            }
        }

        // Polygon may be concave and wound either way
        static public Vector2 Centre(List<Vector2> Polygon, out Vector2 size, out Vector2 avg)     // work out some stats.
        {
            float minx = float.MaxValue, maxx = float.MinValue;
            float miny = float.MaxValue, maxy = float.MinValue;

            avg = new Vector2(0, 0);
            foreach (Vector2 v in Polygon)
            {
                if (v.X < minx)
                    minx = v.X;
                if (v.X > maxx)
                    maxx = v.X;
                if (v.Y < miny)
                    miny = v.Y;
                if (v.Y > maxy)
                    maxy = v.Y;

                avg.X += v.X;
                avg.Y += v.Y;
            }

            avg.X /= Polygon.Count;
            avg.Y /= Polygon.Count;

            size = new Vector2(maxx - minx, maxy - miny);
            return new Vector2((maxx + minx) / 2, (maxy + miny) / 2);
        }

        // KEEP for debug for now..

        #if false

        static public void Test()       
        {
            ///EDDiscovery2._3DMap.PolygonTriangulator.Test();
            if (true)
            {
                List<Vector2> points = new List<Vector2>();
                points.Add(new Vector2(100, 100));
                points.Add(new Vector2(100, 200));
                points.Add(new Vector2(200, 200));
                points.Add(new Vector2(200, 100));

                Vector2 centroid = PolygonTriangulator.Centroid(points);
                Console.WriteLine("{0} ", centroid);
            }


            if ( false )
            {
                List<Vector2> points = new List<Vector2>();

                points.Add(new Vector2(0, 100));
                points.Add(new Vector2(100, 200));
                points.Add(new Vector2(100, 100));
                points.Add(new Vector2(200, 200));
                points.Add(new Vector2(300, 100));
                points.Add(new Vector2(150, 0));
                points.Add(new Vector2(0, 50));

                List<List<Vector2>> res = PolygonTriangulator.Triangulate(points, false);

                foreach (List<Vector2> poly in res)
                {
                    Console.WriteLine("Poly:");
                    foreach (Vector2 p in poly)
                    {
                        Console.WriteLine("  {0},{1}", p.X, p.Y);
                    }

                }
            }

            Console.WriteLine("END");
        }

        #endif
    }
}
