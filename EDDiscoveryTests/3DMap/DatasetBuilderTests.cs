using Microsoft.VisualStudio.TestTools.UnitTesting;
using EDDiscovery2._3DMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDDiscovery.DB;
using EDDiscovery2.DB.Offline;

namespace EDDiscovery2._3DMap.Tests
{
    [TestClass()]
    public class DatasetBuilderTests
    {
        private DatasetBuilder _subject;
        private List<ISystemClass> _starList;

        private void SpawnStars()
        {
            _starList = new List<ISystemClass>()
            {
                new OfflineSystemClass() { id=1000, name="Sol", x=0.0, y=0.0, z=0.0, population=16999999880 },
                new OfflineSystemClass() { id=1100, name="Sagittarius A*", x=25.21875, y=-20.90625, z=25899.96875 },
                new OfflineSystemClass() { id=1200, name="Beagle Point", x=-1111.562, y=-134.21875, z=65269.75 },
                new OfflineSystemClass() { id=1300, name="Eonorth PA-C d14-0", x=26134.6875, y=-236.34375, z=5287.78125 },
                new OfflineSystemClass() { id=1400, name="Void's Brink", x=39307.25, y=-92.4375, z=19338.375 },
                new OfflineSystemClass() { id=1500, name="HIP 72043", x=-22.0, y=118.5, z=58.78125, population=2166322767 },
                new OfflineSystemClass() { id=1600, name="Achenar", x=67.5, y=-119.46875, z=24.84375, population=12999999523 },
                new OfflineSystemClass() { id=1600, name="Alioth", x=-33.65625, y=72.46875, z=-20.65625, population=10000000000 },
                new OfflineSystemClass() { id=1700, name="Legoworld", x=100.0, y=50.0, z=200.0, population=1000 },
            };
        }

        [TestInitialize]
        public void Initialize()
        {
            SpawnStars();
            _subject = new DatasetBuilder
            {
                Origin = new OfflineSystemClass(),
                StarList = _starList
            };
        }

        [TestMethod()]
        public void When_building_all_systems_specifically()
        {
            _subject.AllSystems = true;
            var datasets = _subject.Build();
                    
            Data3DSetClass<PointData> dataset = (Data3DSetClass <PointData>) datasets[0];

            Assert.AreEqual("stars", dataset.Name);

            var sagA = dataset.Primatives[1];

            Assert.AreEqual(25.21875, sagA.x);
            Assert.AreEqual(-20.90625, sagA.y);
            Assert.AreEqual(-25899.96875, sagA.z);
            Assert.IsTrue(dataset.Primatives.Count >= 9);
        }

        [TestMethod()]
        public void When_building_all_systems_with_legoworld_as_the_origin()
        {
            // Probably won't need this test for long. 
            // Just want to be sure of how the offsets are configured for a "centered" system
            _subject.AllSystems = true;
            _subject.Origin = new OfflineSystemClass()
            {
                x = 1000, y = 1000, z = 1000, name = "centerWorld"
            };
            var datasets = _subject.Build();

            Data3DSetClass<PointData> dataset = (Data3DSetClass<PointData>)datasets[0];
            const int LegoWorldIndex = 8;
            var legoWorld = dataset.Primatives[LegoWorldIndex];

            // Confirm we pulled the correct fixture
            Assert.AreEqual("Legoworld", _starList[LegoWorldIndex].name);

            // And the real tests...
            Assert.AreEqual(-900, legoWorld.x); // -1000
            Assert.AreEqual(-950, legoWorld.y); // -1000
            Assert.AreEqual(800, legoWorld.z);  // -1000 then inverted 
        }

        [TestMethod()]
        public void When_building_with_no_stars()
        {
            _subject.StarList = null;
            _subject.AllSystems = true;
            var datasets = _subject.Build();

            Assert.AreEqual("Center", datasets[0].Name);
            Assert.AreEqual("Interest", datasets[1].Name);
        }
    }
}