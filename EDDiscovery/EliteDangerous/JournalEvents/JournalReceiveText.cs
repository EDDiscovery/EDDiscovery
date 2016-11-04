using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //    When written: when a text message is received from another player
    //Parameters:
    //•	From
    //•	Message


    public class JournalReceiveText : JournalEntry
    {
        public JournalReceiveText(JObject evt) : base(evt, JournalTypeEnum.ReceiveText)
        {
            From = JSONHelper.GetStringDef(evt["From"]);
            FromLocalised = JSONHelper.GetStringDef(evt["From_Localised"]);
            Message = JSONHelper.GetStringDef(evt["Message"]);
            MessageLocalised = JSONHelper.GetStringDef(evt["Message_Localised"]);
            Channel = JSONHelper.GetStringDef(evt["Channel"]);

        }
        public string From { get; set; }
        public string FromLocalised { get; set; }
        public string Message { get; set; }
        public string MessageLocalised { get; set; }
        public string Channel { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.textreceived; } }

    }
}
