/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using EMK.LightGeometry;

namespace EliteDangerousCore.DB
{
    public class TargetClass
    {
        public enum TargetType { Bookmark, Notemark, GMO, None };

        public static void SetTargetBookmark(string name, long id, double x, double y, double z)                                                 // set bookmark as ID
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("TargetPositionName", name);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("TargetPositionType", (int)TargetType.Bookmark);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("TargetPositionID", (int)id);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble("TargetPositionX", x);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble("TargetPositionY", y);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble("TargetPositionZ", z);
        }

        public static void SetTargetNotedSystem(string name, long id, double x, double y, double z)                                                 // set bookmark as ID
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("TargetPositionName", name);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("TargetPositionType", (int)TargetType.Notemark);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("TargetPositionID", (int)id);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble("TargetPositionX", x);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble("TargetPositionY", y);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble("TargetPositionZ", z);
        }

        public static void SetTargetGMO(string name, long id, double x, double y, double z)                                                 // set bookmark as ID
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("TargetPositionName", name);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("TargetPositionType", (int)TargetType.GMO);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("TargetPositionID", (int)id);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble("TargetPositionX", x);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble("TargetPositionY", y);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble("TargetPositionZ", z);
        }

        public static void ClearTarget()
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("TargetPositionType", (int)TargetType.None);
        }

        public static long GetTargetBookmark()      // 0 if not a bookmark or not set.
        {
            TargetType tt = (TargetType)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("TargetPositionType", (int)TargetType.None);
            return (tt == TargetType.Bookmark) ? EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("TargetPositionID", 0) : 0;
        }

        public static long GetTargetNotedSystem()      // 0 if not a noted system target or not set.
        {
            TargetType tt = (TargetType)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("TargetPositionType", (int)TargetType.None);
            return (tt == TargetType.Notemark) ? EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("TargetPositionID", 0) : 0;
        }

        public static long GetTargetGMO()               // 0 if not a GMO or not set.
        {
            TargetType tt = (TargetType)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("TargetPositionType", (int)TargetType.None);
            return (tt == TargetType.GMO) ? EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("TargetPositionID", 0) : 0;
        }

        // true if target set with its name, x/y/z
        public static bool GetTargetPosition(out string name, out double x, out double y, out double z)
        {
            TargetType tt = (TargetType)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("TargetPositionType", (int)TargetType.None);
            name = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("TargetPositionName", "");
            x = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble("TargetPositionX", double.NaN);
            y = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble("TargetPositionY", double.NaN);
            z = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble("TargetPositionZ", double.NaN);
            return tt != TargetType.None;
        }

        public static bool IsTargetSet()
        {
            TargetType tt = (TargetType)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("TargetPositionType", (int)TargetType.None);
            return tt != TargetType.None;
        }

        public static TargetType GetTargetType()
        {
            TargetType tt = (TargetType)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("TargetPositionType", (int)TargetType.None);
            return tt;
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
