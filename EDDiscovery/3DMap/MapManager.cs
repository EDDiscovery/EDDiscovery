using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;

namespace EDDiscovery2._3DMap
{
    public class MapManager
    {
        private FormMap _formMap;

        public MapManager(bool nowindowreposition)
        {
            _formMap = new FormMap();
            _formMap.Nowindowreposition = nowindowreposition;
        }

        public void Prepare(string historysel, string homesys, string centersys, float zoom,
                            AutoCompleteStringCollection sysname )
        {
            _formMap.Prepare(historysel, homesys, centersys, zoom, sysname);
        }

        public void SetPlanned(List<SystemClass> plannedr)
        {
            _formMap.SetPlannedRoute(plannedr);
        }

        public void SetReferenceSystems(List<SystemClass> trir)
        {
            _formMap.SetReferenceSystems(trir);
        }

        public void SetVisited(List<VisitedSystemsClass> visited)
        {
            _formMap.SetVisitedSystems(visited);
        }

        public void Show()
        { 
            _formMap.Show();
            _formMap.Focus();
        }
    }
}
