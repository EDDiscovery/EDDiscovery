using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB.Offline;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using EDDiscovery2.Trilateration;

namespace EDDiscovery2._3DMap
{
    public class DatasetBuilder
    {
        private List<IData3DSet> _datasets;

        public SystemClass Origin { get; set; } = new SystemClass();
        public List<ISystemClass> StarList { get; set; } = new List<ISystemClass>();
        public List<SystemClass> ReferenceSystems { get; set; } = new List<SystemClass>();
        public List<SystemPosition> VisitedSystems { get; set; }

        public bool GridLines { get; set; } = false;
        public bool DrawLines { get; set; } = false;
        public bool AllSystems { get; set; } = false;
        public bool Stations { get; set; } = false;
 
        public DatasetBuilder()
        {            
        }

        public List<IData3DSet> Build()
        {
            _datasets = new List<IData3DSet>();
            AddGridLines();
            AddStandardSystems();
            AddStations();
            AddVisitedSystems();
            AddCenterPointToDataset();
            AddPOIsToDataset();
            AddTrilaterationInfoToDataset();

            return _datasets;
        }

        public void AddGridLines()
        {
            Point minPos = new Point(-50000, -20000);
            Point maxPos = new Point(50000, 70000);
            int unitSize = 1000;
            if (GridLines)
            {
                bool addstations = !Stations;
                var datasetGrid = new Data3DSetClass<LineData>("grid", (Color)System.Drawing.ColorTranslator.FromHtml("#296A6C"), 0.6f);

                for (int x = minPos.X; x <= maxPos.X; x += unitSize)
                {
                    datasetGrid.Add(new LineData(x - Origin.x,
                        0 - Origin.y,
                        Origin.x - minPos.Y,
                        x - Origin.x,
                        0 - Origin.y,
                        Origin.z - maxPos.Y));
                }
                for (int z = minPos.Y; z <= maxPos.Y; z += unitSize)
                {
                    datasetGrid.Add(new LineData(minPos.X - Origin.x,
                        0 - Origin.y,
                        Origin.z - z,
                        maxPos.Y - Origin.x,
                        0 - Origin.y,
                        Origin.z - z));
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

                foreach (ISystemClass si in StarList)
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

                foreach (SystemClass si in StarList)
                {
                    if (si.population > 0)
                        AddSystem(si, datasetS);
                }
                _datasets.Add(datasetS);
            }
        }

        public void AddVisitedSystems()
        {
            if (VisitedSystems != null && VisitedSystems.Any())
            {
                SystemClass lastknownps = null;
                foreach (SystemPosition ps in VisitedSystems)
                {
                    if (ps.curSystem != null && ps.curSystem.HasCoordinate)
                    {
                        ps.lastKnownSystem = lastknownps;
                        lastknownps = ps.curSystem;
                    }
                }

                // For some reason I am unable to fathom this errors during the session after DBUpgrade8
                // colours just resolves to an object reference not set error, but after a restart it works fine
                // Not going to waste any more time, a one time restart is hardly the worst workaround in the world...
                IEnumerable<IGrouping<int, SystemPosition>> colours =
                    from SystemPosition sysPos in VisitedSystems
                    group sysPos by sysPos.vs.MapColour;

                foreach (IGrouping<int, SystemPosition> colour in colours)
                {
                    if (DrawLines)
                    {
                        Data3DSetClass<LineData> datasetl = new Data3DSetClass<LineData>("visitedstars" + colour.Key.ToString(), Color.FromArgb(colour.Key), 2.0f);
                        foreach (SystemPosition sp in colour)
                        {
                            if (sp.curSystem != null && sp.curSystem.HasCoordinate && sp.lastKnownSystem != null && sp.lastKnownSystem.HasCoordinate)
                            {
                                datasetl.Add(new LineData(sp.curSystem.x - Origin.x, sp.curSystem.y - Origin.y, Origin.z - sp.curSystem.z,
                                    sp.lastKnownSystem.x - Origin.x, sp.lastKnownSystem.y - Origin.y, Origin.z - sp.lastKnownSystem.z));

                            }
                        }
                        _datasets.Add(datasetl);
                    }
                    else
                    {
                        var datasetvs = new Data3DSetClass<PointData>("visitedstars" + colour.Key.ToString(), Color.FromArgb(colour.Key), 2.0f);
                        foreach (SystemPosition sp in colour)
                        {
                            SystemClass star = SystemData.GetSystem(sp.Name);
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

        private void AddCenterPointToDataset()
        {
            var dataset = new Data3DSetClass<PointData>("Center", Color.Yellow, 5.0f);

            //GL.Enable(EnableCap.ProgramPointSize);
            dataset.Add(new PointData(0, 0, 0));
            _datasets.Add(dataset);
        }

        private void AddPOIsToDataset()
        {
            var dataset = new Data3DSetClass<PointData>("Interest", Color.Purple, 10.0f);
            AddSystem("sol", dataset);
            AddSystem("sagittarius a*", dataset);
            //AddSystem("polaris", dataset);
            _datasets.Add(dataset);
        }

        private void AddTrilaterationInfoToDataset()
        {
            if (ReferenceSystems != null && ReferenceSystems.Any())
            {
                var referenceLines = new Data3DSetClass<LineData>("CurrentReference", Color.Green, 5.0f);
                foreach (var refSystem in ReferenceSystems)
                {
                    referenceLines.Add(new LineData(0, 0, 0, refSystem.x - Origin.x, refSystem.y - Origin.y, Origin.z - refSystem.z));
                }

                _datasets.Add(referenceLines);

                var lineSet = new Data3DSetClass<LineData>("SuggestedReference", Color.DarkOrange, 5.0f);


                Stopwatch sw = new Stopwatch();
                sw.Start();
                SuggestedReferences references = new SuggestedReferences(Origin.x, Origin.y, Origin.z);

                for (int ii = 0; ii < 16; ii++)
                {
                    var rsys = references.GetCandidate();
                    if (rsys == null) break;
                    var system = rsys.System;
                    references.AddReferenceStar(system);
                    if (ReferenceSystems != null && ReferenceSystems.Any(s => s.name == system.name)) continue;
                    System.Diagnostics.Trace.WriteLine(string.Format("{0} Dist: {1} x:{2} y:{3} z:{4}", system.name, rsys.Distance.ToString("0.00"), system.x, system.y, system.z));
                    lineSet.Add(new LineData(0, 0, 0, system.x - Origin.x, system.y - Origin.y, Origin.z - system.z));
                }
                sw.Stop();
                System.Diagnostics.Trace.WriteLine("Reference stars time " + sw.Elapsed.TotalSeconds.ToString("0.000s"));
                _datasets.Add(lineSet);
            }
        }

        private void AddSystem(string systemName, Data3DSetClass<PointData> dataset)
        {
            AddSystem(SystemData.GetSystem(systemName), dataset);
        }

        private void AddSystem(ISystemClass system, Data3DSetClass<PointData> dataset)
        {
            if (system != null && system.HasCoordinate)
            {
                dataset.Add(new PointData(system.x - Origin.x, system.y - Origin.y, Origin.z - system.z));
            }
        }

    }
}
