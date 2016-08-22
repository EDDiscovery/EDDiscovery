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
        //public bool Recorded { get { return record == false && entries != null; } }
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
            public long messagetime;
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

        public void Clear()
        {
            StopRecord();
            StopPlayBack();
            entries = null;
            timer.Stop();
        }

        public void StartRecord( bool step )
        {
            StopPlayBack();

            if (entries == null)
            {
                entries = new List<FlightEntry>();
                lastpos = new Vector3(0, 0, float.MaxValue);
                lastdir = new Vector3(0, 0, float.MaxValue);
                lastzoom = float.MaxValue;
            }

            record = true;
            pause = false;
            recordstep = step;

            timer = new Stopwatch();
            timer.Start();
            lasttime = 0;
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

        public void StartPlayBack()
        {
            if (entries != null)
            {
                StopRecord();

                playbackpos = 0;
                pause = false;
                timer = new Stopwatch();
                timer.Start();
                lasttime = 0;
            }
        }

        public void StopPlayBack()
        {
            if (playbackpos >= 0)
            {
                playbackpos = -1;
                timer.Stop();
            }
        }

        public void TogglePlayBack()
        {
            if (playbackpos < 0)
                StartPlayBack();
            else
                StopPlayBack();
        }

        public void Record(Vector3 pos, Vector3 d, float z)
        {
            if (playbackpos<0 && record && !recordstep && !pause)
            {
                if (pos != lastpos || d != lastdir || z != lastzoom)
                {
                    FlightEntry fe = new FlightEntry();
                    long curtime = timer.ElapsedMilliseconds;
                    RecordEntry(curtime, pos, d, z, curtime - lasttime, 0, 0, 0, "",0);
                    Console.WriteLine("At {0} store {1} {2} {3}", entries[entries.Count - 1].offsettime, lastpos, lastdir, lastzoom);
                }
            }
        }

        public void RecordEntry(long curtime, Vector3 pos, Vector3 d, float z, long etime, long timetofly, long timetopan, long timetozoom, string msg , long mtime)
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
            fe.messagetime = mtime;
            entries.Add(fe);

            lasttime = curtime;                     // keep these up to date in case we are doing a normal record, or swap back to it
            lastdir = d;
            lastpos = pos;
            lastzoom = z;
        }

        // can do this when paused or in any record mode.
        public void RecordStep(Vector3 pos, Vector3 d, float z, long etime, long timetofly, long timetopan, long timetozoom, string msg, long mtime , long holdhere )
        {
            if (record)
            {
                long curtime = timer.ElapsedMilliseconds;

                bool holding = (holdhere > 0);

                RecordEntry(curtime, pos, d, z, etime, timetofly, timetopan, timetozoom, (holding) ? "" : msg , (holding) ? 0 : mtime );
                Console.WriteLine("At {0} store {1} {2} {3}", entries[entries.Count - 1].offsettime, lastpos, lastdir, lastzoom);

                if ( holding )
                {
                    RecordEntry(curtime, pos, d, z, holdhere, 0,0,0, msg, mtime);
                    Console.WriteLine("At {0} store {1} {2} {3}", entries[entries.Count - 1].offsettime, lastpos, lastdir, lastzoom);
                }

            }
        }

        public bool PlayBack(out Vector3 newpos, out long newpostime, 
                             out Vector3 newdir , out long newdirtime, 
                             out float newzoom , out long newzoomtime,
                             out string message , out long newmessagetime )
        {
            newdir = newpos = new Vector3(0, 0, 0);
            newpostime = newdirtime = newzoomtime = newmessagetime = 0;
            newzoom = 0;
            message = null;

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
                        newmessagetime = entries[playbackpos].messagetime;

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
                    RecordStep(pos, dir, zoom, frm.Elapsed, frm.Fly, frm.Pan, frm.Zoom, frm.Msg, frm.Msg.Length>0 ? frm.MsgTime : 0, frm.HoldHere);
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

                        Vector3 prevpos = new Vector3(0, 0, float.MaxValue);
                        Vector3 prevdir = new Vector3(0, 0, float.MaxValue);
                        float prevzoom = 0;

                        for (int i = 0; i < entries.Count; i++)
                        {
                            jr.WriteStartObject();
                            jr.WritePropertyName("T"); jr.WriteValue(entries[i].offsettime);

                            if (prevpos != entries[i].pos)
                            {
                                jr.WritePropertyName("Pos"); jr.WriteStartArray(); jr.WriteValue(entries[i].pos.X);
                                jr.WriteValue(entries[i].pos.Y); jr.WriteValue(entries[i].pos.Z); jr.WriteEndArray();
                                prevpos = entries[i].pos;
                            }

                            if (prevdir != entries[i].dir)
                            {
                                jr.WritePropertyName("Dir"); jr.WriteStartArray(); jr.WriteValue(entries[i].dir.X);
                                jr.WriteValue(entries[i].dir.Y); jr.WriteValue(entries[i].dir.Z); jr.WriteEndArray();
                                prevdir = entries[i].dir;
                            }

                            if (prevzoom != entries[i].zoom)
                            {
                                jr.WritePropertyName("Z"); jr.WriteValue(entries[i].zoom);
                                prevzoom = entries[i].zoom;
                            }

                            if (entries[i].timetofly != 0)
                            {
                                jr.WritePropertyName("FlyTime"); jr.WriteValue(entries[i].timetofly);
                            }

                            if (entries[i].timetopan != 0)
                            {
                                jr.WritePropertyName("PanTime"); jr.WriteValue(entries[i].timetopan);
                            }

                            if (entries[i].timetozoom != 0)
                            {
                                jr.WritePropertyName("ZTime"); jr.WriteValue(entries[i].timetozoom);
                            }

                            if (entries[i].message != null && entries[i].message.Length > 0)
                            {
                                jr.WritePropertyName("Msg"); jr.WriteValue(entries[i].message);
                            }

                            if (entries[i].messagetime != 0)
                            {
                                jr.WritePropertyName("MsgTime"); jr.WriteValue(entries[i].messagetime);
                            }

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

                        Vector3 prevpos = new Vector3(0, 0, 0);
                        Vector3 prevdir = new Vector3(0, 0, 0);
                        float prevzoom = 0;

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
                                    JArray ja = (JArray)jo["Pos"];

                                    if (ja != null)
                                    {
                                        fe.pos.X = (float)ja[0];
                                        fe.pos.Y = (float)ja[1];
                                        fe.pos.Z = (float)ja[2];
                                        prevpos = fe.pos;
                                    }
                                    else
                                        fe.pos = prevpos;

                                    JArray jb = (JArray)jo["Dir"];

                                    if (jb != null)
                                    {
                                        fe.dir.X = (float)jb[0];
                                        fe.dir.Y = (float)jb[1];
                                        fe.dir.Z = (float)jb[2];
                                        prevdir = fe.dir;
                                    }
                                    else
                                        fe.dir = prevdir;

                                    fe.zoom = prevzoom = ReadFloat(jo, "Z", prevzoom);

                                    fe.timetofly = ReadLong(jo, "FlyTime", 0);
                                    fe.timetopan = ReadLong(jo, "PanTime", 0);
                                    fe.timetozoom = ReadLong(jo, "ZTime", 0);
                                    fe.messagetime = ReadLong(jo, "MsgTime", 0);
                                    fe.message = (string)jo["Msg"];

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

        long ReadLong(JObject jo, string t, long def)
        {
            JToken jtmt = jo[t];
            return (jtmt != null) ? (long)jo[t] : def;
        }

        float ReadFloat(JObject jo, string t, float def)
        {
            JToken jtmt = jo[t];
            return (jtmt != null) ? (float)jo[t] : def;
        }
    }
}