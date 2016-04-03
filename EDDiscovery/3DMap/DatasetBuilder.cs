using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using EDDiscovery2.Trilateration;
using OpenTK;

namespace EDDiscovery2._3DMap
{
    public class DatasetBuilder
    {
        private List<IData3DSet> _datasets;

        public ISystem CenterSystem { get; set; } = new SystemClass();
        public ISystem SelectedSystem { get; set; } = new SystemClass();
        public List<ISystem> StarList { get; set; } = new List<ISystem>();
        public List<ISystem> ReferenceSystems { get; set; } = new List<ISystem>();
        public List<SystemPosition> VisitedSystems { get; set; }
        public List<ISystem> PlannedRoute { get; set; } = new List<ISystem>();

        public bool GridLines { get; set; } = false;
        public bool DrawLines { get; set; } = false;
        public bool AllSystems { get; set; } = false;
        public bool Stations { get; set; } = false;

        public Vector2 MinGridPos { get; set; } = new Vector2(-50000.0f, -20000.0f);
        public Vector2 MaxGridPos { get; set; } = new Vector2(50000.0f, 80000.0f);

        public DatasetBuilder()
        {            
        }

        public List<IData3DSet> Build()
        {
            _datasets = new List<IData3DSet>();
            AddGridLines();
            AddStandardSystems();
            AddStations();
            AddVisitedSystemsInformation();
            AddCenterPointToDataset();
            AddSelectedSystemToDataset();
            AddPOIsToDataset();
            AddTrilaterationInfoToDataset();
            AddRoutePlannerInfoToDataset();

            return _datasets;
        }

        public void AddGridLines()
        {
            int unitSize = 1000;
            if (GridLines)
            {
                bool addstations = !Stations;
                var datasetGrid = new Data3DSetClass<LineData>("grid", (Color)System.Drawing.ColorTranslator.FromHtml("#296A6C"), 0.6f);

                for (float x = MinGridPos.X; x <= MaxGridPos.X; x += unitSize)
                {
                    datasetGrid.Add(new LineData(x, 0, MinGridPos.Y, x,0,MaxGridPos.Y));
                }
                for (float z = MinGridPos.Y; z <= MaxGridPos.Y; z += unitSize)
                {
                    datasetGrid.Add(new LineData(MinGridPos.X, 0, z, MaxGridPos.X, 0, z));
                }

                _datasets.Add(datasetGrid);
            }
        }

        public void AddStandardSystems()
        {
            if (AllSystems && StarList != null)
            {
                bool addstations = !Stations;
                var datasetS = new Data3DSetClass<PointData>("stars", Color.White, 1.0f);

                foreach (ISystem si in StarList)
                {
                    if (addstations || si.population == 0)
                        AddSystem(si, datasetS);
                }
                _datasets.Add(datasetS);
            }
        }

        public void AddStations()
        {
            if (Stations)
            {
                var datasetS = new Data3DSetClass<PointData>("stations", Color.RoyalBlue, 1.0f);

                foreach (ISystem si in StarList)
                {
                    if (si.population > 0)
                        AddSystem(si, datasetS);
                }
                _datasets.Add(datasetS);
            }
        }

        public void AddVisitedSystemsInformation()
        {
            if (VisitedSystems != null && VisitedSystems.Any())
            {
                ISystem lastknownps = LastKnownSystemPosition();

                // For some reason I am unable to fathom this errors during the session after DBUpgrade8
                // colours just resolves to an object reference not set error, but after a restart it works fine
                // Not going to waste any more time, a one time restart is hardly the worst workaround in the world...
                IEnumerable<IGrouping<int, SystemPosition>> colours =
                    from SystemPosition sysPos in VisitedSystems where sysPos.vs!=null
                    group sysPos by sysPos.vs.MapColour;

                if (colours!=null)
                {
                    foreach (IGrouping<int, SystemPosition> colour in colours)
                    {
                        if (DrawLines)
                        {
                            var datasetl = new Data3DSetClass<LineData>("visitedstars" + colour.Key.ToString(), Color.FromArgb(colour.Key), 2.0f);
                            foreach (SystemPosition sp in colour)
                            {
                                if (sp.curSystem != null && sp.curSystem.HasCoordinate && sp.lastKnownSystem != null && sp.lastKnownSystem.HasCoordinate)
                                {
                                    datasetl.Add(new LineData(sp.curSystem.x, sp.curSystem.y, sp.curSystem.z,
                                        sp.lastKnownSystem.x, sp.lastKnownSystem.y, sp.lastKnownSystem.z));

                                }
                            }
                            _datasets.Add(datasetl);
                        }
                        else
                        {
                            var datasetvs = new Data3DSetClass<PointData>("visitedstars" + colour.Key.ToString(), Color.FromArgb(colour.Key), 2.0f);
                            foreach (SystemPosition sp in colour)
                            {
                                ISystem star = SystemData.GetSystem(sp.Name);
                                if (star != null && star.HasCoordinate)
                                {

                                    AddSystem(star, datasetvs);
                                }
                            }
                            _datasets.Add(datasetvs);
                        }

                    }
                }
            }
        }

        // Planned change: Centered system will be marked but won't be "center" of the galaxy
        // dataset anymore. The origin will stay at Sol.
        public void AddCenterPointToDataset()
        {
            var dataset = new Data3DSetClass<PointData>("Center", Color.Yellow, 5.0f);

            //GL.Enable(EnableCap.ProgramPointSize);
            dataset.Add(new PointData(CenterSystem.x, CenterSystem.y, CenterSystem.z));
            _datasets.Add(dataset);
        }

        public void AddSelectedSystemToDataset()
        {
            if (SelectedSystem != null)
            {
                var dataset = new Data3DSetClass<PointData>("Selected", Color.Orange, 5.0f);

                //GL.Enable(EnableCap.ProgramPointSize);
                dataset.Add(new PointData(SelectedSystem.x, SelectedSystem.y, SelectedSystem.z));
                _datasets.Add(dataset);
            }
        }

        public void AddPOIsToDataset()
        {
            var dataset = new Data3DSetClass<PointData>("Interest", Color.Purple, 10.0f);
            AddSystem("sol", dataset);
            AddSystem("sagittarius a*", dataset);
            //AddSystem("polaris", dataset);
            _datasets.Add(dataset);
        }

        public void AddTrilaterationInfoToDataset()
        {
            if (ReferenceSystems != null && ReferenceSystems.Any())
            {
                var referenceLines = new Data3DSetClass<LineData>("CurrentReference", Color.Green, 5.0f);
                foreach (var refSystem in ReferenceSystems)
                {
                    referenceLines.Add(new LineData(CenterSystem.x, CenterSystem.y, CenterSystem.z, refSystem.x, refSystem.y, refSystem.z));
                }

                _datasets.Add(referenceLines);

                var lineSet = new Data3DSetClass<LineData>("SuggestedReference", Color.DarkOrange, 5.0f);


                Stopwatch sw = new Stopwatch();
                sw.Start();
                SuggestedReferences references = new SuggestedReferences(CenterSystem.x, CenterSystem.y, CenterSystem.z);

                for (int ii = 0; ii < 16; ii++)
                {
                    var rsys = references.GetCandidate();
                    if (rsys == null) break;
                    var system = rsys.System;
                    references.AddReferenceStar(system);
                    if (ReferenceSystems != null && ReferenceSystems.Any(s => s.name == system.name)) continue;
                    System.Diagnostics.Trace.WriteLine(string.Format("{0} Dist: {1} x:{2} y:{3} z:{4}", system.name, rsys.Distance.ToString("0.00"), system.x, system.y, system.z));
                    lineSet.Add(new LineData(CenterSystem.x, CenterSystem.y, CenterSystem.z, system.x, system.y, system.z));
                }
                sw.Stop();
                System.Diagnostics.Trace.WriteLine("Reference stars time " + sw.Elapsed.TotalSeconds.ToString("0.000s"));
                _datasets.Add(lineSet);
            }
        }

        public void AddRoutePlannerInfoToDataset()
        {
            if (PlannedRoute != null && PlannedRoute.Any())
            {
                var routeLines = new Data3DSetClass<LineData>("PlannedRoute", Color.DarkOrange, 25.0f);
                ISystem prevSystem = PlannedRoute.First();
                foreach (ISystem point in PlannedRoute.Skip(1))
                {
                    routeLines.Add(new LineData(prevSystem.x, prevSystem.y, prevSystem.z, point.x, point.y, point.z));
                    prevSystem = point;
                }
                _datasets.Add(routeLines);
            }
        }

        private void AddSystem(string systemName, Data3DSetClass<PointData> dataset)
        {
            AddSystem(SystemData.GetSystem(systemName), dataset);
        }

        private void AddSystem(ISystem system, Data3DSetClass<PointData> dataset)
        {
            if (system != null && system.HasCoordinate)
            {
                dataset.Add(new PointData(system.x, system.y, system.z));
            }
        }

        private ISystem LastKnownSystemPosition()
        {
            ISystem lastknownps = null;
            foreach (SystemPosition ps in VisitedSystems)
            {
                if (ps.curSystem == null)
                {
                    ps.curSystem = SystemData.GetSystem(ps.Name);
                }

                if (ps.curSystem != null && ps.curSystem.HasCoordinate)
                {
                    ps.lastKnownSystem = lastknownps;
                    lastknownps = ps.curSystem;
                }
            }
            return lastknownps;
        }

    }
}
