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
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //    When written: when a text message is sent to another player
    //Parameters:
    //•	To
    //•	Message
    [JournalEntryType(JournalTypeEnum.SendText)]
    public class JournalSendText : JournalEntry
    {
        public JournalSendText(JObject evt) : base(evt, JournalTypeEnum.SendText)
        {
            To = JSONHelper.GetStringDef(evt["To"]);
            To_Localised = JSONHelper.GetStringDef(evt["To_Localised"]);
            Message = JSONHelper.GetStringDef(evt["Message"]);
        }
        public string To { get; set; }
        public string To_Localised { get; set; }
        public string Message { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.sendtext; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("To:", To_Localised.Alt(To), "Msg:", Message);
            detailed = "";
        }
    }
}
