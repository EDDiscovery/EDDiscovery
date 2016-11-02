using System;

namespace EDDiscovery2.DB.InMemory
{
    // For when you need a minimal version and don't want to mess up the database. 
    // Useful for creation of test doubles
    public class SystemClassBase : ISystemBase
    {
        public long id { get; set; }
        public long id_edsm { get; set; }
        public string name { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public DateTime UpdateDate { get; set; }
        public int gridid { get; set; }
        public int randomid { get; set; }

        public bool HasCoordinate
        {
            get
            {
                return (!double.IsNaN(x));
            }
        }
    }

    public class SystemClass : SystemClassBase, ISystem
    {
        public int cr { get; set; }
        public string CommanderCreate { get; set; }
        public DateTime CreateDate { get; set; }
        public string CommanderUpdate { get; set; }
        public EDDiscovery.DB.SystemStatusEnum status { get; set; }
        public string SystemNote { get; set; }

        public long id_eddb { get; set; }
        public string faction { get; set; }
        public long population { get; set; }
        public EDGovernment government { get; set; }
        public EDAllegiance allegiance { get; set; }
        public EDState state { get; set; }
        public EDSecurity security { get; set; }
        public EDEconomy primary_economy { get; set; }
        public int needs_permit { get; set; }
        public int eddb_updated_at { get; set; }

        public bool HasEDDBInformation
        {
            get
            {
                return population != 0 || government != EDGovernment.Unknown || needs_permit != 0 || allegiance != EDAllegiance.Unknown ||
                       state != EDState.Unknown || security != EDSecurity.Unknown || primary_economy != EDEconomy.Unknown || (faction != null && faction.Length>0); 
            }
        }
    }
}
