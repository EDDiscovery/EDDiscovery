using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.DB
{
    public interface ISystemBase
    {
        long id { get; set; }
        long id_edsm { get; set; }

        string name { get; set; }
        double x { get; set; }
        double y { get; set; }
        double z { get; set; }
        DateTime UpdateDate { get; set; }
        bool HasCoordinate { get; }
        int gridid { get; set; }
        int randomid { get; set; }
    }

    public interface ISystemEDDB
    {
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
        bool HasEDDBInformation { get; }
    }

    // Definition of the core interface so we can swap out an "offline" version during testing
    public interface ISystem : ISystemBase, ISystemEDDB
    {
        int cr { get; set; }
        string CommanderCreate { get; set; }
        DateTime CreateDate { get; set; }
        string CommanderUpdate { get; set; }
        EDDiscovery.DB.SystemStatusEnum status { get; set; }        // Who made this entry, where did the info come from?
        string SystemNote { get; set; }
    }
}
