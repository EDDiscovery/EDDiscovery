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

        public void Prepare(ISystem historysel, string homesys, ISystem centersys, float zoom,
                            AutoCompleteStringCollection sysname, List<VisitedSystemsClass> visited)
        {
            _formMap.Prepare(historysel, homesys, centersys, zoom, sysname, visited);
        }

        public void Prepare(VisitedSystemsClass historysel, string homesys, ISystem centersys, float zoom,
                            AutoCompleteStringCollection sysname, List<VisitedSystemsClass> visited)
        {
            _formMap.Prepare(historysel, homesys, centersys, zoom, sysname, visited);
        }

        public void Prepare(string historysel, string homesys, string centersys, float zoom,
                            AutoCompleteStringCollection sysname , List<VisitedSystemsClass> visited )
        {
            _formMap.Prepare(historysel, homesys, centersys, zoom, sysname, visited);
        }

        public void SetPlanned(List<SystemClass> plannedr)
        {
            _formMap.SetPlannedRoute(plannedr);
        }

        public void UpdateVisited(List<VisitedSystemsClass> visited)
        {
            _formMap.UpdateVisitedSystems(visited);
        }

        public void UpdateHistorySystem(string historysel)
        {
            _formMap.UpdateHistorySystem(historysel);
        }

        public void UpdateHistorySystem(VisitedSystemsClass historysel)
        {
            _formMap.UpdateHistorySystem(historysel);
        }

        public bool MoveToSystem(VisitedSystemsClass system)
        {
            return _formMap.SetCenterSystemTo(system);
        }

        public bool MoveToSystem(string sysname)
        {
            return _formMap.SetCenterSystemTo(sysname);
        }

        public void UpdateNote()
        {
            _formMap.UpdateNote();
        }

        public void UpdateBookmarksGMO(bool gototarget )
        {
            _formMap.UpdateBookmarksGMO(gototarget);
        }

        public void Show()
        {
            _formMap.TopMost = EDDiscoveryForm.EDDConfig.KeepOnTop;
            // TODO: set Opacity to match EDDiscoveryForm
            _formMap.Show();
            _formMap.Focus();
        }
    }
}
