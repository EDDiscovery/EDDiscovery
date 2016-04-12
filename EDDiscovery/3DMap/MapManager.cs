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

        public void Show()
        {
            Show(true);
        }

        public void Show(bool CenterFromSettings)
        {
            Show(CenterFromSettings, -1);
        }

        public void Show(bool CenterFromSettings, float OverrideZoom)
        {
            _formMap.Prepare(CenterFromSettings, OverrideZoom);
            _formMap.Show();
            _formMap.Focus();
        }

    }
}
