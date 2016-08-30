using EMK.LightGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.DB
{
    class TargetClass
    {
        public enum TargetType { Bookmark, Notemark, GMO, None };

        public static void SetTargetBookmark(string name, long id, double x, double y, double z)                                                 // set bookmark as ID
        {
            SQLiteDBClass.PutSettingString("TargetPositionName", name);
            SQLiteDBClass.PutSettingInt("TargetPositionType", (int)TargetType.Bookmark);
            SQLiteDBClass.PutSettingInt("TargetPositionID", (int)id);
            SQLiteDBClass.PutSettingDouble("TargetPositionX", x);
            SQLiteDBClass.PutSettingDouble("TargetPositionY", y);
            SQLiteDBClass.PutSettingDouble("TargetPositionZ", z);
        }

        public static void SetTargetNotedSystem(string name, long id, double x, double y, double z)                                                 // set bookmark as ID
        {
            SQLiteDBClass.PutSettingString("TargetPositionName", name);
            SQLiteDBClass.PutSettingInt("TargetPositionType", (int)TargetType.Notemark);
            SQLiteDBClass.PutSettingInt("TargetPositionID", (int)id);
            SQLiteDBClass.PutSettingDouble("TargetPositionX", x);
            SQLiteDBClass.PutSettingDouble("TargetPositionY", y);
            SQLiteDBClass.PutSettingDouble("TargetPositionZ", z);
        }

        public static void SetTargetGMO(string name, long id, double x, double y, double z)                                                 // set bookmark as ID
        {
            SQLiteDBClass.PutSettingString("TargetPositionName", name);
            SQLiteDBClass.PutSettingInt("TargetPositionType", (int)TargetType.GMO);
            SQLiteDBClass.PutSettingInt("TargetPositionID", (int)id);
            SQLiteDBClass.PutSettingDouble("TargetPositionX", x);
            SQLiteDBClass.PutSettingDouble("TargetPositionY", y);
            SQLiteDBClass.PutSettingDouble("TargetPositionZ", z);
        }

        public static void ClearTarget()
        {
            SQLiteDBClass.PutSettingInt("TargetPositionType", (int)TargetType.None);
        }

        public static long GetTargetBookmark()      // 0 if not a bookmark or not set.
        {
            TargetType tt = (TargetType)SQLiteDBClass.GetSettingInt("TargetPositionType", (int)TargetType.None);
            return (tt == TargetType.Bookmark) ? SQLiteDBClass.GetSettingInt("TargetPositionID", 0) : 0;
        }

        public static long GetTargetNotedSystem()      // 0 if not a noted system target or not set.
        {
            TargetType tt = (TargetType)SQLiteDBClass.GetSettingInt("TargetPositionType", (int)TargetType.None);
            return (tt == TargetType.Notemark) ? SQLiteDBClass.GetSettingInt("TargetPositionID", 0) : 0;
        }

        public static long GetTargetGMO()               // 0 if not a GMO or not set.
        {
            TargetType tt = (TargetType)SQLiteDBClass.GetSettingInt("TargetPositionType", (int)TargetType.None);
            return (tt == TargetType.GMO) ? SQLiteDBClass.GetSettingInt("TargetPositionID", 0) : 0;
        }

        // true if target set with its name, x/y/z
        public static bool GetTargetPosition(out string name, out double x, out double y, out double z)
        {
            TargetType tt = (TargetType)SQLiteDBClass.GetSettingInt("TargetPositionType", (int)TargetType.None);
            name = SQLiteDBClass.GetSettingString("TargetPositionName", "");
            x = SQLiteDBClass.GetSettingDouble("TargetPositionX", double.NaN);
            y = SQLiteDBClass.GetSettingDouble("TargetPositionY", double.NaN);
            z = SQLiteDBClass.GetSettingDouble("TargetPositionZ", double.NaN);
            return tt != TargetType.None;
        }

        public static bool GetTargetPosition(out string name, out Point3D t)
        {
            double x, y, z;
            bool ret = GetTargetPosition(out name, out x, out y, out z);
            t = new Point3D(x, y, z);
            return ret;
        }

        public static string GetNameWithoutPrefix(string name)
        {
            int indexof = name.IndexOf(':');

            if (indexof == -1)
                return name;
            else if (name.Length > indexof + 1)
                return name.Substring(indexof + 1, name.Length - indexof - 1);
            else
                return "";
        }
    }
}
