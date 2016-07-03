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

        public MapManager(bool nowindowreposition,TravelHistoryControl tc)
        {
            _formMap = new FormMap();
            _formMap.travelHistoryControl = tc;
            _formMap.noWindowReposition = nowindowreposition;
        }

        public bool Is3DMapsRunning { get { return _formMap.Is3DMapsRunning; } }
        
        public void Prepare(string historysel, string homesys, string centersys, float zoom,
                            AutoCompleteStringCollection sysname , List<VisitedSystemsClass> visited )
        {
            _formMap.Prepare(historysel, homesys, centersys, zoom, sysname, visited);
        }

        public void SetPlanned(List<SystemClass> plannedr)
        {
            _formMap.SetPlannedRoute(plannedr);
        }

        public void SetReferenceSystems(List<SystemClass> trir)
        {
            _formMap.SetReferenceSystems(trir);
        }

        public void UpdateVisited(List<VisitedSystemsClass> visited)
        {
            _formMap.UpdateVisitedSystems(visited);
        }

        public void UpdateHistorySystem(string historysel)
        {
            _formMap.UpdateHistorySystem(historysel);
        }

        public bool MoveToSystem(string sysname)
        {
            return _formMap.SetCenterSystemTo(sysname, true);
        }

        public void UpdateNote()
        {
            _formMap.UpdateNote();
        }

        public void Show()
        { 
            _formMap.Show();
            _formMap.Focus();
        }
    }
}
