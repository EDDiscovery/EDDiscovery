using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System.Collections.Generic;
using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public static class UnitTestRoot
    {
        [BaseUtils.UnitTests.Test(99)]
        public static void Root()
        {

            CheckSection("FDName");

            {
                Dictionary<VehicleFDName, int> dict = new Dictionary<VehicleFDName, int>();

                var keyone = new VehicleFDName("ONE");
                var keytwo = new VehicleFDName("tWo");

                dict[keyone] = 1;
                dict[keytwo] = 1;

                CheckThat(dict.TryGetValue(new VehicleFDName("one"), out int value)).Is(true);
                CheckThat(dict.TryGetValue(new VehicleFDName("TWO"), out value)).Is(true);
                CheckThat(dict.TryGetValue(new VehicleFDName("theee"), out value)).Is(false);
            }
            {
                Dictionary<SystemAddress, int> dict = new Dictionary<SystemAddress, int>();     // testing dictionary comparisions

                var keyone = new SystemAddress(10);
                var keytwo = new SystemAddress(20);

                dict[keyone] = 1;
                dict[keytwo] = 1;

                CheckThat(dict.TryGetValue(new SystemAddress(10), out int value)).Is(true);
                CheckThat(dict.TryGetValue(new SystemAddress(20), out int value2)).Is(true);
                CheckThat(dict.TryGetValue(new SystemAddress(30), out int value3)).Is(false);
            }

            {
                var mat = MaterialCommodityMicroResourceType.GetByFDName(new MCFDName("goLD"));
                Check( mat != null);
                mat = MaterialCommodityMicroResourceType.GetByFDName(new MCFDName("Gold"));
                Check( mat != null);

                FDName a = new MCFDName("Gold");
                FDName b = null;
                FDName c = null;
                Check( a != b);       // this is not trivial, as you need to use left is null form etc not left != null
                Check( b != a);
                Check( b == c);
                Check( a.Equals("gold"));
                Check( a.Equals("Gold"));
                Check( !a.Equals("Goldx"));
            }
            {

                var a = new MCFDName("Gold");
                var b = new MCFDName("gold");
                var c = new MCFDName("gol");

                Check( a == b);
                Check( a != c);
            }
            {
                var bp = "Sensor_KillWarrantScanner_FastScan";
                var fd = EngineeringRecipeFDName.Normalise(bp, out string engname, null);
                CheckThat(fd).IsNotNull();
                CheckThat(fd).Is("Sensor_KillWarrantScanner_FastScan");
                CheckThat(engname).Is("Fast Scanner");
            }
            {
                var bp = "LifeSupport_LightWeight";
                var fd = EngineeringRecipeFDName.Normalise(bp, out string engname, null);
                CheckThat(fd).IsNotNull();
                CheckThat(fd).Is("LifeSupport_LightWeight");
                CheckThat(engname).Is("Lightweight");
            }

            {
                string cat = "$MICRORESOURCE_CATEGORY_Component;";
                CheckThat(MaterialCommodityMicroResourceType.ToCategory(cat)).Is(MaterialCommodityMicroResourceType.CatType.Component);
                cat = "$MICRORESOURCE_CATEGORY_Elements;";
                CheckThat(MaterialCommodityMicroResourceType.ToCategory(cat)).Is(MaterialCommodityMicroResourceType.CatType.Raw);
                cat = "$MICRORESOURCE_CATEGORY_Item;";
                CheckThat(MaterialCommodityMicroResourceType.ToCategory(cat)).Is(MaterialCommodityMicroResourceType.CatType.Item);
                cat = "$MICRORESOURCE_CATEGORY_Data;";
                CheckThat(MaterialCommodityMicroResourceType.ToCategory(cat)).Is(MaterialCommodityMicroResourceType.CatType.Data);
            }

        }
    }
}

