/*
 * Copyright © 2016-2020 EDDiscovery development team
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

using EDDiscovery.Forms;
using BaseUtils.JSON;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery._3DMap
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
        private FormMap owner = null;

        private bool record = false;
        private bool pause = false;
        private bool recordstep = false;
        private Vector3 lastpos, lastdir;
        private float lastzoom;
        private long lasttime;

        private Stopwatch timer;

        int playbackpos = -1;

        public MapRecorder(FormMap form)
        {
            owner = form;
        }

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
                if (frm.ShowDialog(owner) == DialogResult.OK)
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
                dlg.InitialDirectory = EDDOptions.Instance.FlightsAppDirectory();
                dlg.DefaultExt = "flight";
                dlg.AddExtension = true;
                dlg.Filter = "Flight files (*.flight)|*.flight|All files (*.*)|*.*";

                if (dlg.ShowDialog(owner) == DialogResult.OK)
                {
                    if ( !SaveToFile(dlg.FileName) )
                        ExtendedControls.MessageBoxTheme.Show(owner, "Failed to save flight - check file path");
                }
            }
            else
                ExtendedControls.MessageBoxTheme.Show(owner, "No flight recorded");
        }

        public void LoadDialog()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = EDDOptions.Instance.FlightsAppDirectory();
            dlg.DefaultExt = "flight";
            dlg.AddExtension = true;

            dlg.Filter = "Flight files (*.flight)|*.flight|All files (*.*)|*.*";

            if (dlg.ShowDialog(owner) == DialogResult.OK)
            {
                if (!ReadFromFile(dlg.FileName))
                    ExtendedControls.MessageBoxTheme.Show(owner, "Failed to load flight " + dlg.FileName + ". Check file path and file contents");
            }
        }

        public delegate void loadaction(Object sender, EventArgs e);

        public void UpdateStoredVideosToolButton( ToolStripDropDownButton tsb , loadaction ab ,Image image )
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

            string flightdir = EDDOptions.Instance.FlightsAppDirectory();
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
                JArray outer = new JArray();

                JObject ftype = new JObject();
                ftype["FlightType"] = "V1";
                ftype["Date"] = DateTime.Now;

                outer.Add(ftype);

                for (int i = 0; i < entries.Count; i++)
                {
                    JObject entry = new JObject();
                    entry["T"] = entries[i].offsettime;

                    if (entries[i].IsPosSet)
                    {
                        entry["Pos"] = new JArray(entries[i].pos.X, entries[i].pos.Y, entries[i].pos.Z);

                        if (entries[i].timetofly != 0)
                        {
                            entry["FlyTime"] = entries[i].timetofly;
                        }
                    }

                    if (entries[i].IsDirSet)
                    {
                        entry["Dir"] = new JArray(entries[i].dir.X, entries[i].dir.Y, entries[i].dir.Z);

                        if (entries[i].timetopan != 0)
                        {
                            entry["PanTime"] = entries[i].timetopan;
                        }
                    }

                    if (entries[i].IsZoomSet)
                    {
                        entry["Z"] = entries[i].zoom;

                        if (entries[i].timetozoom != 0)
                        {
                            entry["ZTime"] = entries[i].timetozoom;
                        }
                    }

                    if (entries[i].IsMessageSet)
                    {
                        entry["Msg"] = entries[i].message;

                        if (entries[i].messagetime != 0)
                        {
                            entry["MsgTime"] = entries[i].messagetime;
                        }
                    }

                    outer.Add(entry);
                }

                File.WriteAllText(filename, outer.ToString(true));

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
                string json = File.ReadAllText(filename);
                JArray outer = JArray.Parse(json);

                if (outer != null)
                {
                    JObject ftype = outer[0].Object();
                    if (ftype != null)
                    {
                        for (int i = 1; i < outer.Count; i++)
                        {
                            JObject entry = outer[i].Object();

                            if (entry == null)
                                continue;

                            FlightEntry fe = new FlightEntry();
                            fe.offsettime = entry["T"].Long();

                            JArray ja = entry["Pos"].Array();

                            if (ja != null)
                            {
                                fe.pos = new Vector3((float)ja[0], (float)ja[1], (float)ja[2]);
                                fe.timetofly = entry["FlyTime"].Long();
                            }
                            else
                                fe.pos = FlightEntry.NullVector;

                            JArray jb = entry["Dir"].Array();

                            if (jb != null)
                            {
                                fe.dir = new Vector3((float)jb[0], (float)jb[1], (float)jb[2]);
                                fe.timetopan = entry["PanTime"].Long();
                            }
                            else
                                fe.dir = FlightEntry.NullVector;


                            JToken jtmt = entry["Z"];

                            if (jtmt != null)
                            {
                                fe.zoom = entry["Z"].Float();
                                fe.timetozoom = entry["ZTime"].Long();
                            }
                            else
                                fe.zoom = FlightEntry.NullZoom;

                            fe.message = entry["Msg"].Str();
                            fe.messagetime = entry["MsgTime"].Long();

                            newentries.Add(fe);
                        }
                    }
                }

                entries = newentries;
                return true;
            }
            catch   {   }

            return false;
        }

    }
}