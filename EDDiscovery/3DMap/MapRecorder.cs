using EDDiscovery;
using Newtonsoft.Json.Linq;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace EDDiscovery2._3DMap
{
    public class MapRecorder
    {
        public bool InPlayBack { get { return playbackpos >= 0; } }
        public bool Recording { get { return record; } }
        public bool RecordingNormal { get { return record && !recordstep; } }
        public bool RecordingStep { get { return record && recordstep; } }
        public bool Paused { get { return pause; } }
        public bool Recorded { get { return record == false && entries != null; } }
        public bool Entries { get { return entries != null; } }

        class FlightEntry
        {
            public long offsettime; 
            public Vector3 pos;
            public long timetofly;        //0 = imm
            public Vector3 dir;
            public long timetopan;        //0 = imm
            public float zoom;
            public long timetozoom;        // NOT YET IMPLEMENTED BUT MAY NEED IT
            public string message;
        }

        private List<FlightEntry> entries = null;

        private bool record = false;
        private bool pause = false;
        private bool recordstep = false;
        private Vector3 lastpos, lastdir;
        private float lastzoom;
        private long lasttime;

        private Stopwatch timer;

        int playbackpos = -1;

        public void StartRecord( bool step )
        {
            StopPlayBack();

            entries = new List<FlightEntry>();

            lastpos = new Vector3(0, 0, float.MaxValue);
            lastdir = new Vector3(0, 0, float.MaxValue);
            lastzoom = float.MaxValue;
            lasttime = 0;

            record = true;
            pause = false;
            recordstep = step;
            timer = new Stopwatch();
            timer.Start();
        }

        public void StopRecord()
        {
            if (record)
            {
                record = false;
                pause = false;
                recordstep = false;
                timer.Stop();
            }
        }

        public void ToggleRecord( bool step )
        {
            if (record)
                StopRecord();
            else
                StartRecord(step);
        }

        public void TogglePause()
        {
            if (record || playbackpos>=0)
            {
                pause = !pause;

                if (!pause)     // if unpaused, reset the timer
                {
                    lasttime = timer.ElapsedMilliseconds;
                }
            }
        }

        public void Record(Vector3 pos, Vector3 d, float z)
        {
            if (record && !recordstep && !pause)
            {
                if (pos != lastpos || d != lastdir || z != lastzoom)
                {
                    FlightEntry fe = new FlightEntry();
                    long curtime = timer.ElapsedMilliseconds;
                    RecordEntry(curtime, pos, d, z, curtime - lasttime, 0, 0, 0, "");
                    Console.WriteLine("At {0} store {1} {2} {3}", entries[entries.Count - 1].offsettime, lastpos, lastdir, lastzoom);
                }
            }
        }

        public void RecordEntry(long curtime, Vector3 pos, Vector3 d, float z, long etime, long timetofly, long timetopan, long timetozoom, string msg)
        {
            FlightEntry fe = new FlightEntry();
            fe.pos = pos;
            fe.dir = d;
            fe.zoom = z;
            fe.offsettime = etime;                  // at this time , fly to pos turn to dir in time specified
            fe.timetofly = timetofly;
            fe.timetopan = timetopan;
            fe.timetozoom = timetozoom;
            fe.message = msg;
            entries.Add(fe);

            lasttime = curtime;                     // keep these up to date in case we are doing a normal record, or swap back to it
            lastdir = d;
            lastpos = pos;
            lastzoom = z;
        }

        // can do this when paused or in any record mode.
        public void RecordStep(Vector3 pos, Vector3 d, float z, long etime, long timetofly, long timetopan, long timetozoom, string msg, long holdhere)
        {
            if (record)
            {
                long curtime = timer.ElapsedMilliseconds;

                bool holding = (holdhere > 0);

                RecordEntry(curtime, pos, d, z, etime, timetofly, timetopan, timetozoom, (holding) ? "" : msg);
                Console.WriteLine("At {0} store {1} {2} {3}", entries[entries.Count - 1].offsettime, lastpos, lastdir, lastzoom);

                if ( holding )
                {
                    RecordEntry(curtime, pos, d, z, holdhere, 0,0,0, msg);
                    Console.WriteLine("At {0} store {1} {2} {3}", entries[entries.Count - 1].offsettime, lastpos, lastdir, lastzoom);
                }

            }
        }

        public void StartPlayBack()
        {
            StopRecord();

            if (entries != null)
            {
                playbackpos = 0;
                pause = false;
                timer = new Stopwatch();
                timer.Start();
                lasttime = 0;
            }
        }

        public void StopPlayBack()
        {
            StopRecord();

            if (playbackpos >= 0)
            {
                playbackpos = -1;
                timer.Stop();
            }
        }

        public void TogglePlayBack()
        {
            StopRecord();

            if (playbackpos < 0)
                StartPlayBack();
            else
                StopPlayBack();
        }

        public bool PlayBack(out Vector3 newpos, out long newpostime, 
                             out Vector3 newdir , out long newdirtime, 
                             out float newzoom , out long newzoomtime,
                             out string message )
        {
            newdir = newpos = new Vector3(0, 0, 0);
            newpostime = newdirtime = newzoomtime = 0;
            newzoom = 0;
            message = "";

            if (!pause && playbackpos >= 0)
            {
                if (playbackpos == entries.Count)
                    StopPlayBack();
                else
                {
                    long targettime = lasttime + entries[playbackpos].offsettime;
                    long ms = timer.ElapsedMilliseconds;

                    if (ms >= targettime )
                    {
                        lasttime = targettime;

                        newpos = entries[playbackpos].pos;
                        newpostime = entries[playbackpos].timetofly;
                        newdir = entries[playbackpos].dir;
                        newdirtime = entries[playbackpos].timetopan;
                        newzoom = entries[playbackpos].zoom;
                        newzoomtime = entries[playbackpos].timetozoom;
                        message = entries[playbackpos].message;

                        playbackpos++;
                        return true;
                    }
                }
            }

            return false;
        }

        public void RecordStepDialog(Vector3 pos, Vector3 dir, float zoom)
        {
            if (Recording)
            {
                RecordStep frm = new RecordStep();
                frm.Init(pos, dir, zoom);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RecordStep(pos, dir, zoom, frm.Elapsed, frm.Fly, frm.Pan, 0, frm.Msg, frm.HoldHere);
                }
            }
        }

        public void SaveDialog()
        {
            if ( Entries )      // note you can save partially thru a recording
            {
                SaveFileDialog dlg = new SaveFileDialog();

                dlg.InitialDirectory = Path.Combine(Tools.GetAppDataDirectory(), "Flights");

                if (!Directory.Exists(dlg.InitialDirectory))
                {
                    Directory.CreateDirectory(dlg.InitialDirectory);
                }

                dlg.DefaultExt = "flight";
                dlg.AddExtension = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if ( !SaveToFile(dlg.FileName) )
                        MessageBox.Show("Failed to save flight - check file path");
                }
            }
            else
                MessageBox.Show("No flight recorded");
        }

        public void LoadDialog()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = Path.Combine(Tools.GetAppDataDirectory(), "Flights");

            if (!Directory.Exists(dlg.InitialDirectory))
            {
                Directory.CreateDirectory(dlg.InitialDirectory);
            }

            dlg.DefaultExt = "flight";
            dlg.AddExtension = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (!ReadFromFile(dlg.FileName))
                    MessageBox.Show("Failed to load flight - check file path and file contents");
            }
        }

        public bool SaveToFile(string filename)
        {
#if false
            List<FlightEntry> entries = new List<FlightEntry>();

            for (int i = 0; i < 10; i++)
            {
                FlightEntry fe = new FlightEntry();
                fe.pos = new Vector3(i, i, i);
                fe.dir = new Vector3(i + 100, i + 100, i + 200);
                fe.zoom = i;
                fe.offsettime = i * 10;
                fe.timetofly = i;
                fe.timetopan = i * 1000;
                fe.message = "kwk" + i;
                entries.Add(fe);
            }
#endif

            try
            {
                using (StreamWriter sr = new StreamWriter(filename))
                {
                    using (JsonTextWriter jr = new JsonTextWriter(sr))
                    {
                        jr.WriteRaw("[");
                        jr.WriteStartObject();
                        jr.WritePropertyName("FlightType"); jr.WriteValue("V1");
                        jr.WritePropertyName("Date"); jr.WriteValue(DateTime.Now.ToString());
                        jr.WriteEndObject();
                        jr.WriteRaw(",");
                        jr.WriteWhitespace(Environment.NewLine);

                        for (int i = 0; i < entries.Count; i++)
                        {
                            jr.WriteStartObject();
                            jr.WritePropertyName("T"); jr.WriteValue(entries[i].offsettime);
                            jr.WritePropertyName("Z"); jr.WriteValue(entries[i].zoom);
                            jr.WritePropertyName("ZTime"); jr.WriteValue(entries[i].timetozoom);
                            jr.WritePropertyName("FlyTime"); jr.WriteValue(entries[i].timetofly);
                            jr.WritePropertyName("Pos"); jr.WriteStartArray(); jr.WriteValue(entries[i].pos.X);
                            jr.WriteValue(entries[i].pos.Y); jr.WriteValue(entries[i].pos.Z); jr.WriteEndArray();
                            jr.WritePropertyName("PanTime"); jr.WriteValue(entries[i].timetopan);
                            jr.WritePropertyName("Dir"); jr.WriteStartArray(); jr.WriteValue(entries[i].dir.X);
                            jr.WriteValue(entries[i].dir.Y); jr.WriteValue(entries[i].dir.Z); jr.WriteEndArray();
                            jr.WritePropertyName("Msg"); jr.WriteValue(entries[i].message);
                            jr.WriteEndObject();

                            if ( i < entries.Count-1 )
                                jr.WriteRaw(",");
                            jr.WriteWhitespace(Environment.NewLine);
                        }
                        jr.WriteRaw("]");
                    }
                }

                return true;
            }
            catch { }

            return false;
        }


        public bool ReadFromFile(string filename)
        {
            StopRecord();
            StopPlayBack();

            List<FlightEntry> newentries = new List<FlightEntry>();

            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    using (JsonTextReader jr = new JsonTextReader(sr))
                    {
                        bool first = true;

                        while (jr.Read())
                        {
                            if (jr.TokenType == JsonToken.StartObject)
                            {
                                JObject jo = JObject.Load(jr);

                                if (first)
                                {
                                    string vnum = (string)jo["FlightType"];
                                    string datetime = (string)jo["Date"];
                                    first = false;
                                }
                                else
                                {
                                    FlightEntry fe = new FlightEntry();
                                    fe.offsettime = (long)jo["T"];
                                    fe.zoom = (float)jo["Z"];
                                    fe.timetofly = (long)jo["FlyTime"];
                                    fe.timetopan = (long)jo["PanTime"];
                                    fe.message = (string)jo["Msg"];
                                    fe.timetozoom = (long)jo["ZTime"];

                                    JArray ja = (JArray)jo["Pos"];
                                    fe.pos.X = (float)ja[0];
                                    fe.pos.Y = (float)ja[1];
                                    fe.pos.Z = (float)ja[2];

                                    JArray jb = (JArray)jo["Dir"];
                                    fe.dir.X = (float)jb[0];
                                    fe.dir.Y = (float)jb[1];
                                    fe.dir.Z = (float)jb[2];

                                    newentries.Add(fe);
                                }
                            }
                        }
                    }
                }

                entries = newentries;
                return true;
            }
            catch   {   }

            return false;
        }

#if false
        public static void Test()
        {
            Vector3 _viewtargetpos = new Vector3(20, 20, 26000);

            Console.WriteLine("{0}", AzEl(_viewtargetpos, new Vector3(20, 20, 50000)));
            Console.WriteLine("{0}", AzEl(_viewtargetpos, new Vector3(20, 20, 00)));
            Console.WriteLine("{0}", AzEl(_viewtargetpos, new Vector3(20000, 20, 26000)));
            Console.WriteLine("{0}", AzEl(_viewtargetpos, new Vector3(-20000, 20, 26000)));
            Console.WriteLine("{0}", AzEl(_viewtargetpos, new Vector3(1000, 20, 0)));
            Console.WriteLine("{0}", AzEl(_viewtargetpos, new Vector3(-1000, 20, 0)));
            Console.WriteLine("{0}", AzEl(_viewtargetpos, new Vector3(1000, 20, 50000)));
            Console.WriteLine("{0}", AzEl(_viewtargetpos, new Vector3(-1000, 20, 50000)));
        }

        public static Vector2 AzEl(Vector3 curpos, Vector3 target)
        {
            Vector3 delta = Vector3.Subtract(target, curpos);
            Console.WriteLine("{0}->{1} d {2}", curpos, target, delta);

            float radius = delta.Length;

            float inclination = (float)Math.Acos(delta.Y / radius);
            float azimuth = (float)Math.Atan(delta.Z / delta.X);

            inclination *= (float)(180 / Math.PI);
            azimuth *= (float)(180 / Math.PI);

            if (delta.X < 0)      // atan wraps -90->+90, then -90 to +90 around the y axis, from the bottom.
                azimuth += 180;
            azimuth += 90;          // adjust to 0 at bottom, to 360

            azimuth = 180 - azimuth;
            return new Vector2(azimuth, inclination);
        }
#endif    
        }
}