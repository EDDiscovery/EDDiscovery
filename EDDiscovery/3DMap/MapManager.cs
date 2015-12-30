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
            _formMap.Prepare();
            _formMap.Show();
            _formMap.Focus();
        }
    }
}
