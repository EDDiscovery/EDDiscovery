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

        public delegate void Log(string message);
        public Log Logger {
            get { return logger; }
            set { logger = value; }
        }
        private Log logger = delegate { };

        private List<Entry> distances = new List<Entry>();

        public Trilateration()
        {
        }

        public void addDistance(Entry distance)
        {
            distances.Add(distance);
        }

        public void addDistance(double x, double y, double z, double distance)
        {
            addDistance(new Entry(new Coordinate(x, y, z), distance));
        }

        /// <summary>
        /// Executes trilateration based on given distances.
        /// </summary>
        /// <returns>Exact coordinate, if found.</returns>
        public Coordinate run()
        {
            var engine = prepareEngine();

            var trilat = engine.Execute("var trilat = new Trilateration();").GetCompletionValue();

            foreach (var entry in distances)
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
            if (hasBestCandidate)
            {
                var result = engine.Execute("trilat.best[0]").GetCompletionValue().AsObject();
                engine.Execute("EDDlog('x = ' + trilat.best[0].x + ', y = ' + trilat.best[0].y  + ', z = ' + trilat.best[0].z);");

                // TODO result validation (https://github.com/SteveHodge/ed-systems/blob/master/entry.html#L703)

                return new Coordinate(result.Get("x").AsNumber(), result.Get("y").AsNumber(), result.Get("z").AsNumber());
            }
            else
            {
                throw new MoreDistancesNeededException();
            }
        }

        private Jint.Engine prepareEngine()
        {
            Jint.Engine engine = new Jint.Engine();

            engine.Execute(File.ReadAllText("fround.js"));
            engine.Execute(File.ReadAllText("trilateration.js"));
            engine.SetValue("EDDlog", new Action<object>(delegate (object o) { logger(o.ToString()); }));

            return engine;
        }

        public class MoreDistancesNeededException : Exception { }
        public class CalculationErrorException : Exception { }
    }
}
