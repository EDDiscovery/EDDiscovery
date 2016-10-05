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
            From = Tools.GetStringDef(evt["From"]);
            Message = Tools.GetStringDef(evt["Message"]);
        }
        public string From { get; set; }
        public string Message { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.textreceived; } }

    }
}
