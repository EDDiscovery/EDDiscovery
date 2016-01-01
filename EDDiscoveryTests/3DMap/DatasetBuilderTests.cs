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
                new OfflineSystemClass() { id=1600, name="Alioth", x=-33.65625, y=72.46875, z=-20.65625, population=10000000000 }

            };
        }

        [TestInitialize]
        public void Initialize()
        {
            SpawnStars();
            _subject = new DatasetBuilder
            {
                StarList = _starList
            };
        }

        [TestMethod()]
        public void When_building_all_systems_specifically()
        {
            _subject.AllSystems = true;
            var datasets = _subject.Build();        
            Data3DSetClass<PointData> starData = (Data3DSetClass <PointData>) datasets[0];

            Assert.AreEqual("stars", starData.Name);
            Assert.AreEqual(8, starData.Primatives.Count);

            var sagA = starData.Primatives[1];

            Assert.AreEqual(25.21875, sagA.x);
            Assert.AreEqual(-20.90625, sagA.y);
            Assert.AreEqual(-25899.96875, sagA.z);
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