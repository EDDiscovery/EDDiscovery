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

            engine.Execute("var trilat = new Trilateration();").GetCompletionValue();

            foreach (var entry in entries)
            {
                engine.SetValue("distance", new {
                    x = entry.Coordinate.X,
                    y = entry.Coordinate.Y,
                    z = entry.Coordinate.Z,
                    distance = entry.Distance
                });
                engine.Execute("trilat.addDistance(distance);");
            }

            // engine.Execute("trilat.addDistance({ x: 0, y: 0, z: 0, distance: 0 });");

            var hasBestCandidate = engine.Execute("trilat.best && trilat.best.length === 1").GetCompletionValue().AsBoolean();

            // TODO result validation (https://github.com/SteveHodge/ed-systems/blob/master/entry.html#L703)

            if (hasBestCandidate)
            {
                engine.Execute("EDDlog('x = ' + trilat.best[0].x + ', y = ' + trilat.best[0].y  + ', z = ' + trilat.best[0].z);");

                var result = engine.Execute("trilat.best[0]").GetCompletionValue().AsObject();
                var coordinate = new Coordinate(result.Get("x").AsNumber(), result.Get("y").AsNumber(), result.Get("z").AsNumber());
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

                engine.Execute("EDDlog('trilat.bestCount: ' + trilat.bestCount);");
                engine.Execute("EDDlog('trilat.nextBest: ' + trilat.nextBest);");
                var isPreciseEnough = engine.Execute("(trilat.bestCount - trilat.nextBest) >= 2").GetCompletionValue().AsBoolean();
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

        private Jint.Engine PrepareEngine()
        {
            Jint.Engine engine = new Jint.Engine();

            engine.Execute(File.ReadAllText("fround.js"));
            engine.Execute(File.ReadAllText("trilateration.js"));
            engine.SetValue("EDDlog", new Action<object>(delegate (object o) { logger(o.ToString()); }));

            return engine;
        }
        
        public class CalculationErrorException : Exception { }
    }
}
