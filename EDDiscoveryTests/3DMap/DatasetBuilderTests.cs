using EDDiscovery;
using EDDiscovery2.DB;
using EDDiscovery.DB;
using EDDiscovery2._3DMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;

namespace EDDiscovery2._3DMap.Tests
{
    [TestClass()]
    public class DatasetBuilderTests
    {
        private DatasetBuilder _subject;
        private List<ISystem> _starList;
        private List<ISystem> SpawnStars()
        {
            _starList = new List<ISystem>()
            {
                new DB.InMemory.SystemClass() { id=1000, name="Sol", x=0.0, y=0.0, z=0.0, population=16999999880 },
                new DB.InMemory.SystemClass() { id=1100, name="Sagittarius A*", x=25.21875, y=-20.90625, z=25899.96875 },
                new DB.InMemory.SystemClass() { id=1200, name="Beagle Point", x=-1111.562, y=-134.21875, z=65269.75 },
                new DB.InMemory.SystemClass() { id=1300, name="Eonorth PA-C d14-0", x=26134.6875, y=-236.34375, z=5287.78125 },
                new DB.InMemory.SystemClass() { id=1400, name="Void's Brink", x=39307.25, y=-92.4375, z=19338.375 },
                new DB.InMemory.SystemClass() { id=1500, name="HIP 72043", x=-22.0, y=118.5, z=58.78125, population=2166322767 },
                new DB.InMemory.SystemClass() { id=1600, name="Achenar", x=67.5, y=-119.46875, z=24.84375, population=12999999523 },
                new DB.InMemory.SystemClass() { id=1600, name="Alioth", x=-33.65625, y=72.46875, z=-20.65625, population=10000000000 },
                new DB.InMemory.SystemClass() { id=1700, name="Legoworld", x=100.0, y=50.0, z=200.0, population=1000 },
                new DB.InMemory.SystemClass() { id=1800, name="Reverse Legoworld", x=-100.0, y=-50.0, z=-200.0, population=1001 },
            };

            return _starList;
        }

        private List<SystemPosition> SpawnVisitedSystems()
        {
            var blueVS = new DB.InMemory.VisitedSystemsClass() { MapColour = Color.Blue.ToArgb() };
            var greenVS = new DB.InMemory.VisitedSystemsClass() { MapColour = Color.Green.ToArgb() };

            return new List<SystemPosition>()
            {
                new SystemPosition() {
                    Name = "Sol",
                    curSystem = _starList.Find( s => s.name == "Sol" ),
                    vs = blueVS
                },
                new SystemPosition() {
                    Name = "HIP 723403",
                    curSystem = _starList.Find( s => s.name == "HIP 723403"),
                    lastKnownSystem =  _starList.Find( s => s.name == "Sol" ),
                    vs = greenVS
                },
                new SystemPosition() {
                    Name = "Sagittarius A*",
                    curSystem = _starList.Find( s => s.name == "Sagittarius A*"),
                    lastKnownSystem =  _starList.Find( s => s.name == "HIP 723403" ),
                    vs = greenVS
                }
            };
        }
        private DB.InMemory.SystemClass SpawnOrigin()
        {
            return new DB.InMemory.SystemClass()
            {
                x = 1000.0,
                y = 1000.0,
                z = 1000.0,
                name = "centerWorld"
            };
        }

        [TestInitialize]
        public void Initialize()
        {
            _subject = new DatasetBuilder
            {
                CenterSystem = new DB.InMemory.SystemClass(),
                StarList = SpawnStars()
            };
        }

        [TestMethod()]
        public void When_building_a_grid()
        {
            _subject.GridLines = true;
            _subject.MinGridPos = new Vector2(2000.0f, 12000.0f);
            _subject.MaxGridPos = new Vector2(4000.0f, 14000.0f);
            var datasets = _subject.BuildGridLines(1.0);

            var dataset = (Data3DSetClass<LineData>)datasets[0];
            Assert.AreEqual(2000.0, dataset.Primatives[0].x1);
            Assert.AreEqual(12000.0, dataset.Primatives[0].z1);
        }

        [TestMethod()]
        public void When_building_all_systems()
        {
            _subject.AllSystems = true;
            var datasets = _subject.BuildStars();

            var dataset = (Data3DSetClass<PointData>)datasets[0];

            Assert.AreEqual("stars", dataset.Name);

            var sagA = dataset.Primatives[1];

            Assert.AreEqual(25.21875, sagA.x);
            Assert.AreEqual(-20.90625, sagA.y);
            Assert.AreEqual(25899.96875, sagA.z);
            Assert.IsTrue(dataset.Primatives.Count >= 10);
        }

        [TestMethod()]
        public void When_building_stations()
        {
            _subject.Stations = true;
            var datasets = _subject.BuildStars();

            var dataset = (Data3DSetClass<PointData>)datasets[0];

            Assert.AreEqual("stations", dataset.Name);

            var iger = dataset.Primatives[1];

            Assert.AreEqual(-22.0, iger.x);
            Assert.AreEqual(118.5, iger.y);
            Assert.AreEqual(58.78125, iger.z);
            Assert.AreEqual(6, dataset.Primatives.Count);
        }


        // This one's a pain. Will manually test for now.

        //[TestMethod()]
        //public void When_building_visited_systems_as_points()
        //{
        //    _subject.VisitedSystems = SpawnVisitedSystems();
        //    _subject.DrawLines = false;
        //    var datasets = _subject.Build();
            
        //    Assert.AreEqual("visitedstars-16776961", datasets[0].Name);
        //    Assert.AreEqual("visitedstars-16744448", datasets[1].Name);

        //    var dataset = (Data3DSetClass<PointData>) datasets[0];
        //    var iger = dataset.Primatives[1];

        //    Assert.AreEqual(-1022.0, iger.x);   // -1000
        //    Assert.AreEqual(-881.5, iger.y);    // -1000
        //    Assert.AreEqual(941.21875, iger.z); // inverted then -1000
        //    Assert.AreEqual(2, dataset.Primatives.Count);
        //}


        [TestMethod()]
        public void When_building_with_no_stars()
        {
            _subject.StarList = null;
            _subject.AllSystems = true;
            var datasets = _subject.BuildStars();

            Assert.AreEqual("Center", datasets[0].Name);
            Assert.AreEqual("Interest", datasets[1].Name);
        }
    }
}