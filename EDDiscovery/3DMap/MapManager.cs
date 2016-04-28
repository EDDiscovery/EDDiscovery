using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery;
using EDDiscovery.DB;

namespace EDDiscovery2._3DMap
{
    public class MapManager
    {
        private FormMap _formMap;

        public MapManager()
        {
            _formMap = new FormMap();
        }

        public FormMap Instance
        {
            get
            {
                return _formMap;
            }
        }

        public void Show(string historysel, string homesys, string centersys, float zoom)
        {
            _formMap.Prepare(historysel, homesys, centersys, zoom);
            _formMap.Show();
            _formMap.Focus();
        }

        public void ShowPlanned(string historysel, string homesys, string centersys, float zoom, List<SystemClass> plannedr)
        {
            _formMap.Prepare(historysel, homesys, centersys, zoom);
            _formMap.SetPlannedRoute(plannedr);
            _formMap.Show();
            _formMap.Focus();
        }

        public void ShowTrilat(string historysel, string homesys, string centersys, float zoom, List<SystemClass> plannedr)
        {
            _formMap.Prepare(historysel, homesys, centersys, zoom);
            _formMap.SetReferenceSystems(plannedr);
            _formMap.Show();
            _formMap.Focus();
        }
    }
}
