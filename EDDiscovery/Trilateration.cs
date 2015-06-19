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
            private double x, y, z;

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
                var result = (Dictionary<string, object>) engine.GetParameter("bestResult");
                var coordinate = new Coordinate((double) result["x"], (double)result["y"], (double)result["z"]);
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
