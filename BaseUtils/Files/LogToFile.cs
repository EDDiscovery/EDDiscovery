/*
 * Copyright © 2017 EDDiscovery development team
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public class LogToFile
    {
        public LogToFile()
        {
            rootpath = "c:\\";
        }

        public LogToFile(string s)
        {
            rootpath = s;
        }

        public void Dispose()
        {
            SetFile(rootpath);
        }

        string rootpath;
        StreamWriter debugout = null;
        Stopwatch debugtimer = null;

        public void SetFile(string p)
        {
            if (debugout != null)
            {
                debugout.Close();
                debugout.Dispose();
                debugtimer = null;
                debugout = null;
            }

            rootpath = p;
        }

        public void Write(string s)
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
