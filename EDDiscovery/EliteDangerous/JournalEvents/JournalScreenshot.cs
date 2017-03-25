﻿/*
 * Copyright © 2016 EDDiscovery development team
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
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: screenshot
    [JournalEntryType(JournalTypeEnum.Screenshot)]
    public class JournalScreenshot : JournalEntry
    {
        public JournalScreenshot(JObject evt ) : base(evt, JournalTypeEnum.Screenshot)
        {
            Filename = evt["Filename"].Str();
            Width = evt["Width"].Int();
            Height = evt["Height"].Int();
            System = evt["System"].Str();
            Body = evt["Body"].Str();
        }
        public string Filename { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string System { get; set; }
        public string Body { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.screenshot; } }

        public override void FillInformation(out string summary, out string info, out string detailed)  //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("At " , Body , "<in " , System , "File:", Filename, "Width:", Width , "Height:", Height);
            detailed = "";
        }

        public void SetConvertedFilename(string input_filename, string output_filename, int width, int height)
        {
            JObject jo = GetJson();
            jo["EDDInputFile"] = input_filename;
            jo["EDDOutputFile"] = output_filename;
            jo["EDDOutputWidth"] = width;
            jo["EDDOutputHeight"] = height;
            UpdateJson(jo);
        }

        public static JournalScreenshot GetScreenshot(string filename, int width, int height, DateTime timestamp, string sysname, int cmdrid)
        {
            JournalScreenshot ss = null;
            string body = null;

            if (cmdrid >= 0)
            {
                JournalEntry je = JournalEntry.GetLast(cmdrid, timestamp + TimeSpan.FromSeconds(2), e =>
                    e is JournalScreenshot ||
                    e is JournalSupercruiseEntry ||
                    e is JournalSupercruiseExit ||
                    e is JournalLocation ||
                    e is JournalFSDJump);

                if (je is JournalScreenshot && (sysname == null || sysname == ((JournalScreenshot)je).System) && Math.Abs(timestamp.Subtract(je.EventTimeUTC).TotalSeconds) < 2)
                {
                    ss = je as JournalScreenshot;
                    body = ss.Body;
                    sysname = ss.System;
                }
                if (je is JournalSupercruiseExit)
                {
                    body = ((JournalSupercruiseExit)je).Body;
                    sysname = ((JournalSupercruiseExit)je).StarSystem;
                }
                else if (je is JournalLocation)
                {
                    body = ((JournalLocation)je).Body;
                    sysname = ((JournalLocation)je).StarSystem;
                }
            }

            if (ss == null)
            {
                JObject jo = JObject.FromObject(new
                {
                    timestamp = timestamp.ToUniversalTime().ToString("s") + "Z",
                    @event = "Screenshot",
                    Filename = filename,
                    Width = width,
                    Height = height,
                    System = sysname,
                    Body = body
                });

                ss = JournalEntry.CreateJournalEntry(jo.ToString()) as JournalScreenshot;
                ss.Add(jo);
            }

            return ss;
        }
    }
}
