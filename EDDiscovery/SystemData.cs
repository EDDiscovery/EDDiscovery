using EDDiscovery.DB;
using System;
using System.Collections.Generic;

namespace EDDiscovery
{
    public enum SystemInfoSource
    {
        RW = 1,
        EDSC = 2,
        EDDB = 4,
        EDSM = 5

    }


    public class SystemData
    {

        static public List<SystemClass> SystemList
        {
            get
            {
                return SQLiteDBClass.globalSystems;
            }
        }

        static public SystemClass GetSystem(string name)
        {
            if (name==null)
                return null;

            string lname = name.Trim().ToLower();


            if (SQLiteDBClass.dictSystems.ContainsKey(lname))
                return SQLiteDBClass.dictSystems[lname];
            else 
                return null;
        }

        public static double Distance(SystemClass s1, SystemClass s2)
        {
            if (s1 == null || s2== null)
                return -1;

            return Math.Sqrt((s1.x - s2.x) * (s1.x - s2.x) + (s1.y - s2.y) * (s1.y - s2.y) + (s1.z - s2.z) * (s1.z - s2.z));
        }
    }
}
