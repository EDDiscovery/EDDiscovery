/*
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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalScreenshot : JournalEntry
    {
        public JournalScreenshot(JObject evt ) : base(evt, JournalTypeEnum.Screenshot)
        {
            Filename = JSONHelper.GetStringDef(evt["Filename"]);
            Width = JSONHelper.GetInt(evt["Width"]);
            Height = JSONHelper.GetInt(evt["Height"]);
            System = JSONHelper.GetStringDef(evt["System"]);
            Body = JSONHelper.GetStringDef(evt["Body"]);
        }
        public string Filename { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string System { get; set; }
        public string Body { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.screenshot; } }

        public void SetConvertedFilename(string input_filename, string output_filename, int width, int height)
        {
            this.jEventData["EDDInputFile"] = input_filename;
            this.jEventData["EDDOutputFile"] = output_filename;
            this.jEventData["EDDOutputWidth"] = width;
            this.jEventData["EDDOutputHeight"] = height;
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

                if (je is JournalScreenshot && (sysname == null || sysname == ((JournalScreenshot)je).System))
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
                ss.Add();
            }

            return ss;
        }
    }
}
