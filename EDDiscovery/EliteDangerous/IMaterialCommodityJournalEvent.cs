using EDDiscovery.DB;
using EDDiscovery2.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
{
    public interface IMaterialCommodityJournalEntry
    {
        void MaterialList(MaterialCommoditiesList mc, SQLiteConnectionUser conn);
    }
}
