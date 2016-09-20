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

        public void Prepare(ISystem historysel, string homesys, ISystem centersys, float zoom, List<HistoryEntry> visited)
        {
            _formMap.Prepare(historysel, homesys, centersys, zoom, visited);
        }

        public void SetPlanned(List<SystemClass> plannedr)
        {
            _formMap.SetPlannedRoute(plannedr);
        }

        public void UpdateSystemList(List<HistoryEntry> visited)
        {
            _formMap.UpdateSystemList(visited);
        }

        public void UpdateHistorySystem(ISystem historysel)
        {
            _formMap.UpdateHistorySystem(historysel);
        }

        public bool MoveToSystem(ISystem system)
        {
            return _formMap.SetCenterSystemTo(system);
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
