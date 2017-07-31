using EliteDangerousCore;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EDSM
{
    public class GalacticMapSystem : SystemClassDB
    {
        public GalacticMapObject GalMapObject { get; set; }

        public GalacticMapSystem(ISystem sys, GalacticMapObject gmo)
        {
            this.allegiance = sys.allegiance;
            this.CommanderCreate = sys.CommanderCreate;
            this.CommanderUpdate = sys.CommanderUpdate;
            this.cr = sys.cr;
            this.CreateDate = sys.CreateDate;
            this.eddb_updated_at = sys.eddb_updated_at;
            this.faction = sys.faction;
            this.government = sys.government;
            this.gridid = sys.gridid;
            this.id_eddb = sys.id_eddb;
            this.id_edsm = sys.id_edsm;
            this.name = sys.name;
            this.needs_permit = sys.needs_permit;
            this.population = sys.population;
            this.primary_economy = sys.primary_economy;
            this.randomid = sys.randomid;
            this.security = sys.security;
            this.state = sys.state;
            this.status = sys.status;
            this.UpdateDate = sys.UpdateDate;
            this.x = sys.x;
            this.y = sys.y;
            this.z = sys.z;
            this.GalMapObject = gmo;
        }

        public GalacticMapSystem(GalacticMapObject gmo)
        {
            this.name = gmo.galMapSearch;
            this.x = gmo.points[0].X;
            this.y = gmo.points[0].Y;
            this.z = gmo.points[0].Z;
            this.GalMapObject = gmo;
        }
    }
}
