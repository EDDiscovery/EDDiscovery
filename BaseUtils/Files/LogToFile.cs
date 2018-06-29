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

        public LogToFile(string s, string f = null, bool t = true)
        {
            rootpath = s;
            filename = f;
            time = t;
        }

        public void Dispose()
        {
            SetFile(rootpath);
        }

        string rootpath;
        string filename = null;
        bool time = true;

        StreamWriter debugout = null;
        Stopwatch debugtimer = null;
        Object lockit = new object();

        public void SetFile(string p, string f = null, bool t = true)      // f = null pick timedate one
        {
            lock (lockit)
            {
                if (debugout != null)
                {
                    debugout.Close();
                    debugout.Dispose();
                    debugtimer = null;
                    debugout = null;
                }

                rootpath = p;
                filename = f;
                time = t;
            }
        }

        public void WriteLine(string s)
        {
            lock (lockit)
            {
                if (debugout == null)
                    CreateFile();
                if ( time )
                    debugout.WriteLine((debugtimer.ElapsedMilliseconds % 100000) + ":" + s);
                else
                    debugout.WriteLine(s);
                debugout.Flush();
            }
        }

        bool atstart = true;

        public void Write(string s,bool lf=false)
        {
            lock (lockit)
            {
                if (debugout == null)
                    CreateFile();

                if (atstart)
                {
                    if ( time )
                        debugout.Write((debugtimer.ElapsedMilliseconds % 100000) + ":");

                    atstart = false;
                }

                if (lf)
                {
                    debugout.WriteLine(s);
                    atstart = false;
                    debugout.Flush();
                }
                else
                    debugout.Write(s);
            }
        }

        private void CreateFile()
        {
            debugout = new StreamWriter(Path.Combine(rootpath, filename.HasChars() ? filename : ("debuglog-" + DateTime.Now.ToString("yyyy-dd-MM-HH-mm-ss") + ".log")));
            debugtimer = new Stopwatch();
            debugtimer.Start();
        }
    }
}
