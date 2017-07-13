using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public static class LogToFile
    {
        public static string rootpath = "c:\\";

        static StreamWriter debugout = null;
        static Stopwatch debugtimer = null;

        static public void Write(string s)
        {
            if (debugout == null)
            {
                debugout = new StreamWriter(Path.Combine(rootpath, "debuglog-" + DateTime.Now.ToString("yyyy-dd-MM-HH-mm-ss") + ".log"));
                debugtimer = new Stopwatch();
                debugtimer.Start();
            }

            debugout.WriteLine((debugtimer.ElapsedMilliseconds % 100000) + ":" + s);
            debugout.Flush();
        }
    }
}
