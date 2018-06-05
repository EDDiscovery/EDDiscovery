/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public static class AppTicks
    {
        static System.Diagnostics.Stopwatch stopwatch;
        static long prevtick;

        public static long TickCount        // current tick, with stopwatch creation
        {
            get
            {
                if (stopwatch == null)
                {
                    stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();
                }

                return stopwatch.ElapsedMilliseconds;
            }
        }

        public static string TickCountContinuous     // delta without last point reset
        {
            get
            {
                long tc = TickCount;
                return string.Format("{0} +{1}", tc, tc - prevtick);
            }
        }

        public static string TickCount100       // strange name but keep for now.. reset last point
        {
            get
            {
                long tc = TickCount;
                string s = string.Format("{0} +{1}", tc, tc - prevtick);
                prevtick = tc;
                return s;
            }
        }

    }
}
