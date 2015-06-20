using Noesis.Javascript;
using System;
using System.Collections.Generic;
using System.IO;

namespace EDDiscovery
{
    /// <summary>
    /// Distance calculator using trilateration method.
    /// Wrapper around trilateration.js.
    /// </summary>
    public class Trilateration
    {
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

            public static double operator%(Coordinate a, Coordinate b)
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
            public Coordinate Coordinate { get { return coordinate; } }
            private Coordinate coordinate;

            public double Distance { get { return distance; } }
            private double distance;

            public Entry(Coordinate coordinate, double distance)
            {
                this.coordinate = coordinate;
                this.distance = distance;
            }

            public Entry(double x, double y, double z, double distance) : this(new Coordinate(x, y, z), distance)
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
            NeedMoreDistances
        }

        public class Result
        {
            public ResultState State { get { return state; } }
            private ResultState state;

            public Coordinate Coordinate { get { return coordinate; } }
            private Coordinate coordinate;

            public Dictionary<Entry, double> EntriesDistances { get { return entriesDistances; } }
            private Dictionary<Entry, double> entriesDistances;

            public Result(ResultState state)
            {
                this.state = state;
            }

            public Result(ResultState state, Coordinate coordinate) : this(state)
            {
                this.coordinate = coordinate;
            }

            public Result(ResultState state, Coordinate coordinate, Dictionary<Entry, double> entriesDistances) : this(state, coordinate)
            {
                this.entriesDistances = entriesDistances;
            }
        }

        public delegate void Log(string message);
        public Log Logger {
            get { return logger; }
            set { logger = value; }
        }
        private Log logger = delegate { };

        private List<Entry> entries = new List<Entry>();

        public Trilateration()
        {
        }


//  / p1 and p2 are objects that have x, y, and z properties
// returns the difference p1 - p2 as a vector object (with x, y, z properties), calculated as single precision (as ED does)
    private Coordinate  diff(Coordinate p1, Coordinate p2) 
    {
	return  new Coordinate( p1.X - p2.X, p1.Y - p2.Y,  p1.Z - p2.Z);
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
    private double  dist(Coordinate p1, Coordinate p2) 
    {
	    return length(diff(p2,p1));
    }

    // v is a vector obejct with x, y, and z properties
    // returns the length of v
    private double length(Coordinate v) 
    {
	return Math.Sqrt(dotProd(v,v));
    }


    // p1 and p2 are objects that have x, y, and z properties
    // dp is optional number of decimal places to round to (defaults to 2)
    // returns the distance between p1 and p2, calculated as single precision (as ED does),
    // rounded to the specified number of decimal places
    private double eddist(Coordinate p1, Coordinate p2, int dp) 
    {
	  //  dp = (typeof dp === 'undefined') ? 2 : dp;
	    var v = diff(p2,p1);
	    var d = fround(Math.Sqrt(fround(fround(fround(v.X*v.X) + fround(v.Y*v.Y)) + fround(v.Z*v.Z))));
	    return round(d,dp);
    }


    private float fround(double d)    { return (float)d; }

    // round to the specified number of decimal places
    private double round(double v, int dp) 
    {
	    return Math.Round(v*Math.Pow(10,dp))/Math.Pow(10,dp);
    }


    // p1 and p2 are objects that have x, y, and z properties
    // returns the scalar (dot) product p1 . p2
    private double dotProd(Coordinate p1, Coordinate p2) 
    {
	    return p1.X*p2.X + p1.Y*p2.Y + p1.Z*p2.Z;
    }

    // p1 and p2 are objects that have x, y, and z properties
    // returns the vector (cross) product p1 x p2
    private Coordinate crossProd(Coordinate p1, Coordinate p2) 
    {
	    return new Coordinate(
		    p1.Y*p2.Z - p1.Z*p2.Y,
		    p1.Z*p2.X - p1.X*p2.Z,
		    p1.X*p2.Y - p1.Y*p2.X);
	    
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
		    (Math.Round(v.X*32)/32),
		    (Math.Round(v.Y*32)/32),
		    (Math.Round(v.Z*32)/32));
    }


    // dists is an array of reference objects (properties x, y, z, distance)
    // returns an object containing the best candidate found (properties x, y, z, totalSqErr, i1, i2, i3)
    // i1, i2, i3 are the indexes into dists[] that were reference points for the candidate
    // totalSqErr is the total of the squares of the difference between the supplied distance and the calculated distance to each system in dists[]
    private Candidate  getBestCandidate(Entry [] dists) 
    {
	    int i1 = 0, i2 = 1, i3 = 2, i4;
	    Candidate bestCandidate = null;

	    // run the trilateration for each combination of 3 reference systems in the set of systems we have distance data for
	    // we look for the best candidate over all trilaterations based on the lowest total (squared) error in the calculated
	    // distances to all the reference systems
	    for (i1 = 0; i1 < dists.Length; i1++) {
		    for (i2 = i1+1; i2 < dists.Length; i2++) {
			    for (i3 = i2+1; i3 < dists.Length; i3++) {
		 		    var candidates = getCandidates(dists, i1, i2, i3);
		 		    if (candidates.Length == 2) {
					    candidates[0].totalSqErr = 0;
					    candidates[1].totalSqErr = 0;
					
		 			    for (i4 = 0; i4 < dists.Length; i4++) {
						    DistanceTest err = checkDist((Coordinate)candidates[0], dists[i4].Coordinate, dists[i4].Distance);
		 				    candidates[0].totalSqErr += err.error*err.error;
		 				    err = checkDist((Coordinate)candidates[1], dists[i4].Coordinate, dists[i4].Distance);
		 				    candidates[1].totalSqErr += err.error*err.error;
		 			    }
					    if (bestCandidate == null || bestCandidate.totalSqErr > candidates[0].totalSqErr) {
						    bestCandidate = candidates[0];
						    bestCandidate.i1 = i1;
						    bestCandidate.i2 = i2;
						    bestCandidate.i3 = i3;
						    //console.log("best candidate so far: (1st) "+JSON.stringify(bestCandidate,2));
					    }
					    if (bestCandidate.totalSqErr > candidates[1].totalSqErr) {
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
	    var ex = scalarProd(1/d, p1p2);
	    var p1p3 = diff(p3, p1);
	    var i = dotProd(ex, p1p3);
	    var ey = diff(p1p3, scalarProd(i, ex));
	    ey = scalarProd( 1/length(ey), ey);
	    var j = dotProd(ey, diff(p3, p1));

	    var x = (distance1*distance1 -distance2*distance2 + d*d) / (2*d);
	    var y = ((distance1*distance1 - distance3*distance3 + i*i + j*j) / (2*j)) - (i*x/j);
	    var zsq = distance1*distance1 - x*x - y*y;
	    if (zsq < 0) {
		    //console.log("inconsistent distances (z^2 = "+zsq+")");
		    return new Candidate[] {};
	    } else 
        {
		    var z = Math.Sqrt(zsq);
		    var ez = crossProd(ex, ey);
		    Coordinate coord1 = sum(sum(p1,scalarProd(x,ex)),scalarProd(y,ey));
		    Coordinate coord2 = diff(coord1,scalarProd(z,ez));
		    coord1 = sum(coord1,scalarProd(z,ez));
            
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
        public Result Run()
        {
            var engine = PrepareEngine();

            engine.Run("var trilat = new Trilateration();");

            foreach (var entry in entries)
            {
                var coord = entry.Coordinate;
                var entryDict = new Dictionary<string, double>() {
                    {  "x",  coord.X },
                    {  "y",  coord.Y },
                    {  "z",  coord.Z },
                    {  "distance",  entry.Distance }
                };
                engine.SetParameter("distance", entryDict);
                engine.Run("trilat.addDistance(distance);");
            }

            engine.Run("trilat.addDistance({ x: 0, y: 0, z: 0, distance: 0 });");

            engine.Run("var hasBestCandidate = trilat.best && trilat.best.length === 1;");
            var hasBestCandidate = (bool) engine.GetParameter("hasBestCandidate");

            // TODO result validation (https://github.com/SteveHodge/ed-systems/blob/master/entry.html#L703)

            if (hasBestCandidate)
            {
                engine.Run("EDDlog('x = ' + trilat.best[0].x + ', y = ' + trilat.best[0].y  + ', z = ' + trilat.best[0].z);");

                engine.Run("var bestResult = trilat.best[0];");
                var result = (Dictionary<string, object>) engine.GetParameter("bestResult"); // contains either int or double
                var coordinate = new Coordinate(
                    result["x"] is int ? (int)result["x"] * 1.0 : (double)result["x"],
                    result["y"] is int ? (int)result["y"] * 1.0 : (double)result["y"],
                    result["z"] is int ? (int)result["z"] * 1.0 : (double)result["z"]
                );
                var correctEntriesCount = 0;
                var correctedEntries = new Dictionary<Entry, double>();

                foreach (var entry in entries)
                {
                    var correctedDistance = entry.Coordinate % coordinate;
                    if (correctedDistance == entry.Distance)
                    {
                        correctEntriesCount++;
                    }
                    correctedEntries.Add(entry, correctedDistance);
                }

                engine.Run("EDDlog('trilat.bestCount: ' + trilat.bestCount);");
                engine.Run("EDDlog('trilat.nextBest: ' + trilat.nextBest);");
                engine.Run("var isPreciseEnough = (trilat.bestCount - trilat.nextBest) >= 2;");
                var isPreciseEnough = (bool) engine.GetParameter("isPreciseEnough");
                if (isPreciseEnough && correctEntriesCount >= 5)
                {
                    return new Result(ResultState.Exact, coordinate, correctedEntries);
                } else
                {
                    return new Result(ResultState.NotExact, coordinate, correctedEntries);
                }
            } else
            {
                return new Result(ResultState.NeedMoreDistances);
            }
        }

        private JavascriptContext PrepareEngine()
        {
            JavascriptContext context = new JavascriptContext();

            context.Run(File.ReadAllText("fround.js"));
            context.Run(File.ReadAllText("trilateration.js"));
            context.SetParameter("EDDlog", new Action<object>(delegate (object o) { logger(o.ToString()); }));

            return context;
        }
        
        public class CalculationErrorException : Exception { }
    }
}
