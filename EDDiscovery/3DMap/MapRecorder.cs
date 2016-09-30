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
using System.Linq;
using System.Drawing;

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

        public class FlightEntry
        {
            public long offsettime;         // delta time ms, 0 = imm, -1 wait for all slews to stop.
            public static long WaitForComplete { get { return -1; } }

            public Vector3 pos;             // .x=nan no move
            public long timetofly;          //0 = imm
            public bool IsPosSet { get { return !float.IsNaN(pos.X); } }

            public Vector3 dir;             // .x=nan no dir
            public long timetopan;          //0 = imm
            public bool IsDirSet { get { return !float.IsNaN(dir.X); } }

            public float zoom;              // .x=nan no zoom
            public long timetozoom;         //0 = imm
            public bool IsZoomSet { get { return zoom>0; } }

            public string message;          // null/empty no message
            public long messagetime;        // 0 = 3s
            public bool IsMessageSet { get { return message != null && message.Length > 0; } }

            public static Vector3 NullVector { get { return new Vector3(float.NaN, 0, 0); } }
            public static float NullZoom { get { return 0; } }
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
                lastpos = FlightEntry.NullVector;
                lastdir = FlightEntry.NullVector;
                lastzoom = FlightEntry.NullZoom;
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

                    RecordEntry(curtime,(pos != lastpos) ? pos : FlightEntry.NullVector, 
                                        (d != lastdir) ? d : FlightEntry.NullVector, 
                                        (z != lastzoom) ? z : FlightEntry.NullZoom , 
                                        curtime - lasttime, 0, 0, 0, "",0);


                    //Console.WriteLine("At {0} store {1} {2} {3}", entries[entries.Count - 1].offsettime, lastpos, lastdir, lastzoom);
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
        public void RecordStep(Vector3 pos, Vector3 d, float z, long etime, long timetofly, long timetopan, long timetozoom, 
                            string msg, long mtime , bool waitcomplete, bool displaymessagewhencomplete )
        {
            if (record)
            {
                long curtime = timer.ElapsedMilliseconds;

                RecordEntry(curtime, pos, d, z, etime, timetofly, timetopan, timetozoom,
                            (displaymessagewhencomplete) ? "" : msg, (displaymessagewhencomplete) ? 0 : mtime);

                if ( waitcomplete )
                {
                    RecordEntry(curtime, FlightEntry.NullVector, FlightEntry.NullVector, FlightEntry.NullZoom, FlightEntry.WaitForComplete, 0
                                , 0, 0, (!displaymessagewhencomplete) ? "" : msg, (!displaymessagewhencomplete) ? 0 : mtime);
                    //Console.WriteLine("At {0} store {1} {2} {3}", entries[entries.Count - 1].offsettime, lastpos, lastdir, lastzoom);
                }

            }
        }

        public void RecordStepDialog(Vector3 pos, Vector3 dir, float zoom)
        {
            if (Recording)
            {
                RecordStep frm = new RecordStep();
                frm.Init(pos, dir, zoom);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RecordStep(frm.Pos, frm.Dir, frm.Zoom, frm.Elapsed, frm.FlyTime, frm.PanTime, frm.ZoomTime, frm.Msg, frm.MsgTime, frm.WaitForComplete, frm.DisplayMessageWhenComplete);
                }
            }
        }

        public bool PlayBack(out FlightEntry fe , bool inslews )
        {
            fe = null;

            if (!pause && playbackpos >= 0)
            {
                if (playbackpos == entries.Count)
                    StopPlayBack();
                else
                {
                    long offsettime = entries[playbackpos].offsettime;
                    long ms = timer.ElapsedMilliseconds;
                    long targettime = offsettime + lasttime;

                    if (offsettime == FlightEntry.WaitForComplete)                     // <0 means wait for slews
                    {
                        if ( !inslews )
                        {
                            lasttime = ms;
                            fe = entries[playbackpos++];
                            return true;
                        }
                    }
                    else if ( ms >= targettime )
                    {
                        lasttime = targettime;
                        fe = entries[playbackpos++];
                        return true;
                    }
                }
            }

            return false;
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
                dlg.Filter = "Flight files (*.flight)|*.flight|All files (*.*)|*.*";

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

            dlg.Filter = "Flight files (*.flight)|*.flight|All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (!ReadFromFile(dlg.FileName))
                    MessageBox.Show("Failed to load flight " + dlg.FileName + ". Check file path and file contents");
            }
        }

        public delegate void loadaction(Object sender, EventArgs e);

        public void UpdateStoredVideosToolButton( ToolStripDropDownButton tsb , loadaction ab ,Bitmap image )
        {
            List<ToolStripMenuItem> removelist = new List<ToolStripMenuItem>();

            foreach(ToolStripMenuItem tsmi in tsb.DropDownItems )
            {
                if (tsmi.Tag != null)
                    removelist.Add(tsmi);
            }

            foreach (ToolStripMenuItem tsmi in removelist)
            {
                tsb.DropDownItems.Remove(tsmi);
                tsmi.Dispose();
            }

            string flightdir =  Path.Combine(Tools.GetAppDataDirectory(), "Flights");
            DirectoryInfo dirInfo = new DirectoryInfo(flightdir);

            if (dirInfo.Exists)
            {
                try
                {
                    var sortedfiles = dirInfo.EnumerateFiles("*.flight", SearchOption.AllDirectories).OrderByDescending(x => x.LastWriteTime).Take(15).ToList();

                    foreach (FileInfo file in sortedfiles)
                    {
                        ToolStripMenuItem tsmi = new ToolStripMenuItem();
                        tsmi.Text = "Load " + Path.GetFileNameWithoutExtension(file.FullName);
                        tsmi.Size = new Size(195, 22);
                        tsmi.Click += new System.EventHandler(ab);
                        tsmi.Tag = file.FullName;
                        tsmi.Image = image;
                        tsb.DropDownItems.Add(tsmi);
                    }
                }
                catch
                {
                }
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

                        for (int i = 0; i < entries.Count; i++)
                        {
                            jr.WriteStartObject();
                            jr.WritePropertyName("T"); jr.WriteValue(entries[i].offsettime);

                            if (entries[i].IsPosSet)
                            {
                                jr.WritePropertyName("Pos"); jr.WriteStartArray(); jr.WriteValue(entries[i].pos.X);
                                jr.WriteValue(entries[i].pos.Y); jr.WriteValue(entries[i].pos.Z); jr.WriteEndArray();

                                if (entries[i].timetofly != 0)
                                {
                                    jr.WritePropertyName("FlyTime"); jr.WriteValue(entries[i].timetofly);
                                }
                            }

                            if (entries[i].IsDirSet)
                            {
                                jr.WritePropertyName("Dir"); jr.WriteStartArray(); jr.WriteValue(entries[i].dir.X);
                                jr.WriteValue(entries[i].dir.Y); jr.WriteValue(entries[i].dir.Z); jr.WriteEndArray();

                                if (entries[i].timetopan != 0)
                                {
                                    jr.WritePropertyName("PanTime"); jr.WriteValue(entries[i].timetopan);
                                }
                            }

                            if (entries[i].IsZoomSet)
                            {
                                jr.WritePropertyName("Z"); jr.WriteValue(entries[i].zoom);

                                if (entries[i].timetozoom != 0)
                                {
                                    jr.WritePropertyName("ZTime"); jr.WriteValue(entries[i].timetozoom);
                                }
                            }

                            if ( entries[i].IsMessageSet )
                            {
                                jr.WritePropertyName("Msg"); jr.WriteValue(entries[i].message);

                                if (entries[i].messagetime != 0)
                                {
                                    jr.WritePropertyName("MsgTime"); jr.WriteValue(entries[i].messagetime);
                                }
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
                                        fe.pos = new Vector3((float)ja[0], (float)ja[1], (float)ja[2]);
                                        fe.timetofly = ReadLong(jo, "FlyTime", 0);
                                    }
                                    else
                                        fe.pos = FlightEntry.NullVector;

                                    JArray jb = (JArray)jo["Dir"];

                                    if (jb != null)
                                    {
                                        fe.dir = new Vector3((float)jb[0], (float)jb[1], (float)jb[2]);
                                        fe.timetopan = ReadLong(jo, "PanTime", 0);
                                    }
                                    else
                                        fe.dir = FlightEntry.NullVector;

                                    JToken jtmt = jo["Z"];

                                    if (jtmt != null)
                                    {
                                        fe.zoom = (float)jo["Z"];
                                        fe.timetozoom = ReadLong(jo, "ZTime", 0);
                                    }
                                    else
                                        fe.zoom = FlightEntry.NullZoom;

                                    fe.message = (string)jo["Msg"];
                                    fe.messagetime = ReadLong(jo, "MsgTime", 0);

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