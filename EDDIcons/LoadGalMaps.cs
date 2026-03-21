/*
 * Copyright © 2020 EDDiscovery development team
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

using BaseUtils;
using System.Collections.Generic;

namespace EDDiscovery.Icons
{
    public static class IconMaps
    {
        public static List<Map2d> StandardMaps()
        {
            List<Map2d> maps = new List<Map2d>();

            string json = "{\"x1\" : -45000,\"x2\" : 45000,\"y1\" : 70000,\"y2\" : -20000,\"px1\" : 0,\"px2\" : 4499,\"py1\" : 0,\"py2\" : 4499 }";

            // do not own bitmap, so do not dispose.
            maps.Add(new Map2d("Galaxy L", json, BaseUtils.Icons.IconSet.GetImage("GalMap.Galaxy_L"))); 
            maps.Add(new Map2d("Galaxy L Grid", json, BaseUtils.Icons.IconSet.GetImage("GalMap.Galaxy_L_Grid")));

            return maps;
        }
    }
}
