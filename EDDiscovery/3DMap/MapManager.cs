using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery;

namespace EDDiscovery2._3DMap
{
    public class MapManager
    {
        private FormMap _formMap;

        public AutoCompleteStringCollection SystemNames { get; set; }
        public List<SystemPosition> VisitedSystems { get; set;  }

        public void Prepare()
        {
            _formMap = new FormMap(SystemNames);
        }

        public void Show()
        {
            if (isReady()) {
                _formMap.visitedSystems = VisitedSystems;
                _formMap.Show();
                _formMap.Focus();
            }
        }

        private bool isReady()
        {
            return SystemNames != null;
        }
    }
}
