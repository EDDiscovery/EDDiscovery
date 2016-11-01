using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //    When written: when a text message is sent to another player
    //Parameters:
    //•	To
    //•	Message

    public class JournalSendText : JournalEntry
    {
        public JournalSendText(JObject evt) : base(evt, JournalTypeEnum.SendText)
        {
            To = JSONHelper.GetStringDef(evt["To"]);
            Message = JSONHelper.GetStringDef(evt["Message"]);
        }
        public string To { get; set; }
        public string Message { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.textsent; } }

    }
}
