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

        public static long TickCount
        {
            get
            {
                if (stopwatch == null)
                {
                    stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();
                }

                prevtick = stopwatch.ElapsedMilliseconds;
                return prevtick;
            }
        }

        public static string TickCount100
        {
            get
            {
                long lasttick = prevtick;
                long tc = TickCount;
                //System.Diagnostics.Debug.WriteLine(lasttick + " " + tc);
                return string.Format("{0} +{1}", tc, tc - lasttick);
            }
        }
    }
}
