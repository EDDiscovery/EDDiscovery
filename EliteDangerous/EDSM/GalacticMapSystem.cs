using EliteDangerousCore;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore.EDSM
{
    public class GalacticMapSystem : SystemClass
    {
        public GalacticMapObject GalMapObject { get; set; }

        public GalacticMapSystem(ISystem sys, GalacticMapObject gmo)
        {
            this.Allegiance = sys.Allegiance;
            this.CommanderCreate = sys.CommanderCreate;
            this.CommanderUpdate = sys.CommanderUpdate;
            this.CreateDate = sys.CreateDate;
            this.EDDBUpdatedAt = sys.EDDBUpdatedAt;
            this.Faction = sys.Faction;
            this.Government = sys.Government;
            this.GridID = sys.GridID;
            this.EDDBID = sys.EDDBID;
            this.EDSMID = sys.EDSMID;
            this.Name = sys.Name;
            this.NeedsPermit = sys.NeedsPermit;
            this.Population = sys.Population;
            this.PrimaryEconomy = sys.PrimaryEconomy;
            this.RandomID = sys.RandomID;
            this.Security = sys.Security;
            this.State = sys.State;
            this.status = sys.status;
            this.UpdateDate = sys.UpdateDate;
            this.X = sys.X;
            this.Y = sys.Y;
            this.Z = sys.Z;
            this.GalMapObject = gmo;
        }

        public GalacticMapSystem(GalacticMapObject gmo)
        {
            this.Name = gmo.galMapSearch;
            this.X = gmo.points[0].X;
            this.Y = gmo.points[0].Y;
            this.Z = gmo.points[0].Z;
            this.GalMapObject = gmo;
        }
    }
}
