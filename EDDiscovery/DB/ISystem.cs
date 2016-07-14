using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.DB
{
    // Definition of the core interface so we can swap out an "offline" version during testing
    public interface ISystem
    {
        long id { get; set; }
        long id_edsm { get; set; }

        string name { get; set; }
        string SearchName { get; set; }
        double x { get; set; }
        double y { get; set; }
        double z { get; set; }
        int cr { get; set; }
        string CommanderCreate { get; set; }
        DateTime CreateDate { get; set; }
        string CommanderUpdate { get; set; }
        DateTime UpdateDate { get; set; }
        EDDiscovery.DB.SystemStatusEnum status { get; set; }        // Who made this entry, where did the info come from?
        string SystemNote { get; set; }

        long id_eddb { get; set; }
        string faction { get; set; }
        long population { get; set; }
        EDGovernment government { get; set; }
        EDAllegiance allegiance { get; set; }
        EDState state { get; set; }
        EDSecurity security { get; set; }
        EDEconomy primary_economy { get; set; }
        int needs_permit { get; set; }
        int eddb_updated_at { get; set; }

        bool HasCoordinate{ get; }
        bool HasEDDBInformation { get; }
    }
}
