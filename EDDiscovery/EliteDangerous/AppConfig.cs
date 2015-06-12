using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EDDiscovery2
{
    public class EdNetwork
    {
        [XmlAttribute("VerboseLogging")]
        public int VerboseLogging { get; set; }
    }

    [System.Xml.Serialization.XmlRoot("AppConfig")]
    public class AppConfig
    {

        public EdNetwork Network { get; set; }
    }
}
