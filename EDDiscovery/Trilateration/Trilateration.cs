using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EDDiscovery
{
    /// <summary>
    /// Distance calculator using trilateration method.
    /// Wrapper around trilateration.js.
    /// </summary>
    public class Trilateration
    {
        public enum Algorithm
        {
            RedWizzard_Emulated,
            RedWizzard_Native
        }

        public class Coordinate
        {
            public double X { get { return x; } }
            public double Y { get { return y; } }
            public double Z { get { return z; } }
            protected double x, y, z;

            public Coordinate()
            {
            }

            public Coordinate(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public static double operator %(Coordinate a, Coordinate b)
            {
                return Math.Round(Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2) + Math.Pow(a.z - b.z, 2)), 2);
            }
        }

        public class Candidate : Coordinate
        {
            public double totalSqErr;
            public int i1, i2, i3;

            public Candidate(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public Candidate(Coordinate co)
            {
                this.x = co.X;
                this.y = co.Y;
                this.z = co.Z;
            }

        }

        public class Entry
        {
            public Coordinate Coordinate { get; private set; }

            public double Distance { get; private set; }

            public Entry(Coordinate coordinate, double distance)
            {
                Coordinate = coordinate;
                Distance = distance;
            }

            public Entry(double x, double y, double z, double distance)
                : this(new Coordinate(x, y, z), distance)
            {
            }
        }

        public class DistanceTest
        {
            public double distance;
            public double error;
            public int dp;
        }

        public enum ResultState
        {
            Exact,
            NotExact,
            NeedMoreDistances,
            MultipleSolutions,
        }

        public class Result
        {
            public ResultState State { get; private set; }
            public Coordinate Coordinate { get; private set; }
            public Dictionary<Entry, double> EntriesDistances { get; private set; }

            public Result(ResultState state)
            {
                State = state;
            }

            public Result(ResultState state, Coordinate coordinate)
                : this(state)
            {
                Coordinate = coordinate;
            }

            public Result(ResultState state, Coordinate coordinate, Dictionary<Entry, double> entriesDistances)
                : this(state, coordinate)
            {
                EntriesDistances = entriesDistances;
            }
        }

        public class Region
        {
            public int regionSize = 1;	// 1/32 Ly grid locations to search around candidate coordinate
            public double minx, maxx;
            public double miny, maxy;
            public double minz, maxz;

            public List<Coordinate> origins;

            public Region(Coordinate center)
            {
                origins = new List<Coordinate>();

                origins.Add(center);
                this.minx = Math.Floor(center.X * 32 - regionSize) / 32;
                this.maxx = Math.Ceiling(center.X * 32 + regionSize) / 32;
                this.miny = Math.Floor(center.Y * 32 - regionSize) / 32;
                this.maxy = Math.Ceiling(center.Y * 32 + regionSize) / 32;
                this.minz = Math.Floor(center.Z * 32 - regionSize) / 32;
                this.maxz = Math.Ceiling(center.Z * 32 + regionSize) / 32;

            }

            // number of grid coordinates in the region
            // for a region that has not been merged this is typically (2*regionSize)^3 (though if the center
            // was grid aligned it will be (2*regionsize-1)^3)
            public double Volume()
            {
                return (32768 * (this.maxx - this.minx + 1 / 32) * (this.maxy - this.miny + 1 / 32) * (this.maxz - this.minz + 1 / 32));
            }

            // p has properties x, y, z. returns true if p is in this region, false otherwise
            public bool contains(Coordinate p)
            {
                return (p.X >= this.minx && p.X <= this.maxx
                        && p.Y >= this.miny && p.Y <= this.maxy
                        && p.Z >= this.minz && p.Z <= this.maxz);
            }


            public Region()
            {
            }

            // returns a new region that represents the union of this and r
            public Region union(Region r)
            {
                //    if (!(r instanceof Region)) return null;

                Region u = new Region();

                u.origins = new List<Coordinate>();

                u.origins.AddRange(origins);
                u.origins.AddRange(r.origins);

                u.minx = Math.Min(this.minx, r.minx);
                u.miny = Math.Min(this.miny, r.miny);
                u.minz = Math.Min(this.minz, r.minz);
                u.maxx = Math.Max(this.maxx, r.maxx);
                u.maxy = Math.Max(this.maxy, r.maxy);
                u.maxz = Math.Max(this.maxz, r.maxz);

                return u;
            }

            // returns the highest coordinate of the vector from p to the closest origin point. this is the
            // minimum value of region size (in Ly) that would include the specified point
            public double centrality(Coordinate p)
            {
                double d, best;
                best = 0;
                for (int i = 0; i < this.origins.Count; i++)
                {
                    d = Math.Max(
                        Math.Abs(this.origins[i].X - p.X),
                        Math.Max(Math.Abs(this.origins[i].Y - p.Y),
                        Math.Abs(this.origins[i].Z - p.Z))
                    );
                    if (d < best || best == 0)
                        best = d;
                }
                return best;
            }

            /*
        this.toString = function() {
            return "Region ["+this.minx+", "+this.miny+", "+this.minz
                +"] - ["+this.maxx+", "+this.maxy+", "+this.maxz+"] ("+this.volume()+" points)";
        };*/
        };




        public delegate void Log(string message);
        public Log Logger
        {
            get { return logger; }
            set { logger = value; }
        }
        private Log logger = delegate { };

        private List<Entry> entries = new List<Entry>();

        public Trilateration()
        {
        }





        //private Regions regions;


        private List<Region> getRegions()
        {
            List<Region> regions = new List<Region>();

            // find candidate locations by trilateration on all combinations of the input distances
            // expand candidate locations to regions, combining as necessary
            for (int i1 = 0; i1 < this.entries.Count; i1++)
            {
                for (int i2 = i1 + 1; i2 < this.entries.Count; i2++)
                {
                    for (int i3 = i2 + 1; i3 < this.entries.Count; i3++)
                    {
                        Candidate[] candidates = getCandidates(this.entries.ToArray(), i1, i2, i3);

                        //candidates.forEach(function(candidate) 
                        foreach (Candidate candidate in candidates)
                        {
                            var r = new Region(candidate);
                            // see if there is existing region we can merge this new one into
                            for (var j = 0; j < regions.Count; j++)
                            {
                                Region u = r.union(regions[j]);
                                if (u.Volume() < r.Volume() + regions[j].Volume())
                                {
                                    // volume of union is less than volume of individual regions so merge them
                                    regions[j] = u;
                                    // TODO should really rescan regions to see if there are other existing regions that can be merged into this
                                    r = null;	// clear r so we know not to add it
                                    break;
                                }
                            }
                            if (r != null)
                            {
                                regions.Add(r);
                            }
                        }
                    }
                }
            }

            //		console.log("Candidate regions:");
            //		$.each(regions, function() {
            //			console.log(this.toString());
            //		});
            return regions;
        }

        private int checkDistances(Coordinate p)
        {
            var count = 0;

            foreach (Entry entry in entries)

            //self.distances.forEach(function(dist) 
            {
                var dp = 2;
                /*
                            if (typeof dist.distance === 'string') {
                                // if dist is a string then check if it has exactly 3 decimal places:
                                if (dist.distance.indexOf('.') === dist.distance.length-4) dp = 3;
                            } else if (dist.distance.toFixed(3) === dist.distance.toString()) {
                                // assume it's 3 dp if its 3 dp rounded string matches the string version
                                dp = 3;
                            }
                            */


                if (entry.Distance == eddist(p, entry.Coordinate, dp))
                    count++;

            }

            return count;
        }


        private List<Region> regions;
        private int bestCount;
        private int nextBest;
        private List<Coordinate> best;
        private List<Coordinate> next;


        private Result RunCSharp()
        {

            this.regions = getRegions();
            // check the number of matching distances for each grid location in each region
            // track the best number of matches (and the corresponding locations) and the next
            // best number
            this.bestCount = 0;
            this.best = new List<Coordinate>();
            this.nextBest = 0;
            this.next = new List<Coordinate>();

            var sortedregions = from a in regions where a.regionSize > 0 orderby a.origins.Count descending select a;


            foreach (Region region in sortedregions)
            {
                //this.regions.forEach(function(region) {
                //bestCount = 0;
                //best = new List<Coordinate>();
                //nextBest = 0;
                //next = new List<Coordinate>();

                for (var x = region.minx; x <= region.maxx; x += 1 / 32.0)
                {
                    for (var y = region.miny; y <= region.maxy; y += 1 / 32.0)
                    {
                        for (var z = region.minz; z <= region.maxz; z += 1 / 32.0)
                        {
                            Coordinate p = new Coordinate(x, y, z);
                            var matches = checkDistances(p);
                            if (matches > this.bestCount)
                            {
                                this.nextBest = this.bestCount;
                                this.next = this.best;
                                this.bestCount = matches;
                                this.best = new List<Coordinate>();
                                this.best.Add(p);

                            }
                            else if (matches == this.bestCount)
                            {
                                this.best.Add(p);
                            }
                            else if (matches > this.nextBest)
                            {
                                this.nextBest = matches;
                                this.next = new List<Coordinate>();
                                this.next.Add(p);

                            }
                            else if (matches == this.nextBest)
                            {
                                this.next.Add(p);
                            }
                            if (matches > this.bestCount)
                            {
                                this.nextBest = this.bestCount;
                                this.next = this.best;
                                this.bestCount = matches;
                                this.best = new List<Coordinate>();
                                this.best.Add(p);
                            }
                            else if (matches == this.bestCount)
                            {
                                var found = false;
                                foreach (var e in this.best)
                                {
                                    //this.best.forEach(function(e) {
                                    if (e.X == p.X && e.Y == p.Y && e.Z == p.Z)
                                    {
                                        found = true;
                                        //return false;
                                        break;
                                    }
                                }
                                if (!found) this.best.Add(p);
                            }
                            else if (matches > this.nextBest)
                            {
                                this.nextBest = matches;
                                this.next = new List<Coordinate>();
                                this.next.Add(p);
                            }
                            else if (matches == this.nextBest)
                            {
                                var found = false;
                                foreach (var e in this.best)
                                {
                                    if (e.X == p.X && e.Y == p.Y && e.Z == p.Z)
                                    {
                                        found = true;
                                        //return false;
                                        break;
                                    }
                                }
                                if (!found) this.next.Add(p);
                            }
                        }
                    }
                }


                //var correctEntriesCount = 0;
                //var correctedEntries = new Dictionary<Entry, double>();

                //foreach (var entry in entries)
                //{
                //    var correctedDistance = entry.Coordinate % coordinate;
                //    if (correctedDistance == entry.Distance)
                //    {
                //        correctEntriesCount++;
                //    }
                //    correctedEntries.Add(entry, correctedDistance);
                //}

                if (bestCount >= 5 && (bestCount - nextBest) >= 2)
                {
                    Result res = new Result(ResultState.Exact, best[0], CalculateCSharpResultEntries(entries, best[0]));
                    return res;
                }
                else if ((bestCount - nextBest) >= 1)
                {
                    return new Result(ResultState.NotExact, best[0], CalculateCSharpResultEntries(entries, best[0]));
                }
                else if (best != null && best.Count > 1)
                    return new Result(ResultState.MultipleSolutions, best[0], CalculateCSharpResultEntries(entries, best[0])); // Trilatation.best.Count  shows how many solutions
                else
                    return new Result(ResultState.NeedMoreDistances);



            }
            return new Result(ResultState.NeedMoreDistances);
        }

        private Dictionary<Entry, double> CalculateCSharpResultEntries(List<Entry> entries, Coordinate coordinate)
        {
            var correctedEntries = new Dictionary<Entry, double>();

            foreach (var entry in entries)
            {
                correctedEntries.Add(entry, entry.Coordinate % coordinate);
            }

            return correctedEntries;
        }


        //  / p1 and p2 are objects that have x, y, and z properties
        // returns the difference p1 - p2 as a vector object (with x, y, z properties), calculated as single precision (as ED does)
        private Coordinate diff(Coordinate p1, Coordinate p2)
        {
            return new Coordinate(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }


        // p1 and p2 are objects that have x, y, and z properties
        // returns the sum p1 + p2 as a vector object (with x, y, z properties)
        private Coordinate sum(Coordinate p1, Coordinate p2)
        {
            return new Coordinate(
                p1.X + p2.X,
                p1.Y + p2.Y,
                p1.Z + p2.Z);

        }


        // p1 and p2 are objects that have x, y, and z properties
        // returns the distance between p1 and p2
        private double dist(Coordinate p1, Coordinate p2)
        {
            return length(diff(p2, p1));
        }

        // v is a vector obejct with x, y, and z properties
        // returns the length of v
        private double length(Coordinate v)
        {
            return Math.Sqrt(dotProd(v, v));
        }


        // p1 and p2 are objects that have x, y, and z properties
        // dp is optional number of decimal places to round to (defaults to 2)
        // returns the distance between p1 and p2, calculated as single precision (as ED does),
        // rounded to the specified number of decimal places
        private double eddist(Coordinate p1, Coordinate p2, int dp)
        {
            //  dp = (typeof dp === 'undefined') ? 2 : dp;
            var v = diff(p2, p1);
            var d = fround(Math.Sqrt(fround(fround(fround(v.X * v.X) + fround(v.Y * v.Y)) + fround(v.Z * v.Z))));
            return round(d, dp);
        }


        private float fround(double d) { return (float)d; }

        // round to the specified number of decimal places
        private double round(double v, int dp)
        {
            return Math.Round(v * Math.Pow(10, dp)) / Math.Pow(10, dp);
        }


        // p1 and p2 are objects that have x, y, and z properties
        // returns the scalar (dot) product p1 . p2
        private double dotProd(Coordinate p1, Coordinate p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
        }

        // p1 and p2 are objects that have x, y, and z properties
        // returns the vector (cross) product p1 x p2
        private Coordinate crossProd(Coordinate p1, Coordinate p2)
        {
            return new Coordinate(
                p1.Y * p2.Z - p1.Z * p2.Y,
                p1.Z * p2.X - p1.X * p2.Z,
                p1.X * p2.Y - p1.Y * p2.X);

        }

        // v is a vector obejct with x, y, and z properties
        // s is a scalar value
        // returns a new vector object containing the scalar product of s and v
        private Coordinate scalarProd(double s, Coordinate v)
        {
            return new Coordinate(
                s * v.X,
                s * v.Y,
                s * v.Z);

        }


        // returns a vector with the components of v rounded to 1/32
        private Coordinate gridLocation(Coordinate v)
        {
            return new Coordinate(
                (Math.Round(v.X * 32) / 32),
                (Math.Round(v.Y * 32) / 32),
                (Math.Round(v.Z * 32) / 32));
        }


        // dists is an array of reference objects (properties x, y, z, distance)
        // returns an object containing the best candidate found (properties x, y, z, totalSqErr, i1, i2, i3)
        // i1, i2, i3 are the indexes into dists[] that were reference points for the candidate
        // totalSqErr is the total of the squares of the difference between the supplied distance and the calculated distance to each system in dists[]
        private Candidate getBestCandidate(Entry[] dists)
        {
            int i1 = 0, i2 = 1, i3 = 2, i4;
            Candidate bestCandidate = null;

            // run the trilateration for each combination of 3 reference systems in the set of systems we have distance data for
            // we look for the best candidate over all trilaterations based on the lowest total (squared) error in the calculated
            // distances to all the reference systems
            for (i1 = 0; i1 < dists.Length; i1++)
            {
                for (i2 = i1 + 1; i2 < dists.Length; i2++)
                {
                    for (i3 = i2 + 1; i3 < dists.Length; i3++)
                    {
                        var candidates = getCandidates(dists, i1, i2, i3);
                        if (candidates.Length == 2)
                        {
                            candidates[0].totalSqErr = 0;
                            candidates[1].totalSqErr = 0;

                            for (i4 = 0; i4 < dists.Length; i4++)
                            {
                                DistanceTest err = checkDist((Coordinate)candidates[0], dists[i4].Coordinate, dists[i4].Distance);
                                candidates[0].totalSqErr += err.error * err.error;
                                err = checkDist((Coordinate)candidates[1], dists[i4].Coordinate, dists[i4].Distance);
                                candidates[1].totalSqErr += err.error * err.error;
                            }
                            if (bestCandidate == null || bestCandidate.totalSqErr > candidates[0].totalSqErr)
                            {
                                bestCandidate = candidates[0];
                                bestCandidate.i1 = i1;
                                bestCandidate.i2 = i2;
                                bestCandidate.i3 = i3;
                                //console.log("best candidate so far: (1st) "+JSON.stringify(bestCandidate,2));
                            }
                            if (bestCandidate.totalSqErr > candidates[1].totalSqErr)
                            {
                                bestCandidate = candidates[1];
                                bestCandidate.i1 = i1;
                                bestCandidate.i2 = i2;
                                bestCandidate.i3 = i3;
                                //console.log("best candidate so far: (2nd) "+JSON.stringify(bestCandidate,2));
                            }
                        }
                    }
                }
            }
            return bestCandidate;
        }


        // dists is an array of reference objects (properties x, y, z, distance)
        // i1, i2, i3 indexes of the references to use to calculate the candidates
        // returns an array of two points (properties x, y, z). if the supplied reference points are disjoint then an empty array is returned
        public Candidate[] getCandidates(Entry[] dists, int i1, int i2, int i3)
        {
            Coordinate p1 = dists[i1].Coordinate;
            Coordinate p2 = dists[i2].Coordinate;
            Coordinate p3 = dists[i3].Coordinate;
            double distance1 = dists[i1].Distance;
            double distance2 = dists[i2].Distance;
            double distance3 = dists[i3].Distance;

            Coordinate p1p2 = diff(p2, p1);
            var d = length(p1p2);
            var ex = scalarProd(1 / d, p1p2);
            var p1p3 = diff(p3, p1);
            var i = dotProd(ex, p1p3);
            var ey = diff(p1p3, scalarProd(i, ex));
            ey = scalarProd(1 / length(ey), ey);
            var j = dotProd(ey, diff(p3, p1));

            var x = (distance1 * distance1 - distance2 * distance2 + d * d) / (2 * d);
            var y = ((distance1 * distance1 - distance3 * distance3 + i * i + j * j) / (2 * j)) - (i * x / j);
            var zsq = distance1 * distance1 - x * x - y * y;
            if (zsq < 0)
            {
                //console.log("inconsistent distances (z^2 = "+zsq+")");
                return new Candidate[] { };
            }
            else
            {
                var z = Math.Sqrt(zsq);
                var ez = crossProd(ex, ey);
                Coordinate coord1 = sum(sum(p1, scalarProd(x, ex)), scalarProd(y, ey));
                Coordinate coord2 = diff(coord1, scalarProd(z, ez));
                coord1 = sum(coord1, scalarProd(z, ez));

                return new Candidate[] { new Candidate(coord1), new Candidate(coord2) };
            }
        }

        // calculates the distance between p1 and p2 and then calculates the error between the calculated distance and the supplied distance.
        // if dist has 3dp of precision then the calculated distance is also calculated with 3dp, otherwise 2dp are assumed
        // returns and object with properties (distance, error, dp)
        private DistanceTest checkDist(Coordinate p1, Coordinate p2, double dist)
        {
            DistanceTest ret = new DistanceTest();

            ret.dp = 2;

            /*
            if (dist.toFixed(3) === dist.toString()) {
                // assume it's 3 dp if its 3 dp rounded string matches the string version
                ret.dp = 3;
            }*/

            ret.distance = eddist(p1, p2, ret.dp);
            ret.error = Math.Abs(ret.distance - dist);
            return ret;
        }


        public void AddEntry(Entry distance)
        {
            entries.Add(distance);
        }

        public void AddEntry(double x, double y, double z, double distance)
        {
            AddEntry(new Entry(new Coordinate(x, y, z), distance));
        }

        /// <summary>
        /// Executes trilateration based on given distances.
        /// </summary>
        /// <returns>Result information, including coordinate and corrected Entry distances if found.</returns>
        public Result Run(Algorithm algorithm)
        {
                    return RunCSharp();
        }



        public class CalculationErrorException : Exception { }



    }
}
