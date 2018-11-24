/*
 * Copyright © 2016-2018 EDDiscovery development team
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

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.SendText)]
    public class JournalSendText : JournalEntry
    {
        public JournalSendText(JObject evt) : base(evt, JournalTypeEnum.SendText)
        {
            To = evt["To"].Str();
            To_Localised = JournalFieldNaming.CheckLocalisation(evt["To_Localised"].Str(),To);
            Message = evt["Message"].Str();
        }

        public string To { get; set; }
        public string To_Localised { get; set; }
        public string Message { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("To:".Tx(this), To_Localised, "Msg:".Tx(this), Message);
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.ReceiveText)]
    public class JournalReceiveText : JournalEntry
    {
        public JournalReceiveText(JObject evt) : base(evt, JournalTypeEnum.ReceiveText)
        {
            From = evt["From"].Str();
            FromLocalised = evt["From_Localised"].Str().Alt(From);
            Message = evt["Message"].Str();
            MessageLocalised = JournalFieldNaming.CheckLocalisation(evt["Message_Localised"].Str(), Message);
            Channel = evt["Channel"].Str();
        }

        public string From { get; set; }
        public string FromLocalised { get; set; }
        public string Message { get; set; }
        public string MessageLocalised { get; set; }
        public string Channel { get; set; }         // wing/local/voicechat/friend/player/npc : 3.3 adds squadron/starsystem

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("From:".Tx(this), FromLocalised, "Msg:".Tx(this), MessageLocalised, "Channel:".Tx(this), Channel);
            detailed = "";
        }
    }

}
