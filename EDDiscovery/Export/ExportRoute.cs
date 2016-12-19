using EDDiscovery.DB;
using EMK.LightGeometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Export
{
    class ExportRoute : ExportBase
    {
        private const string TITLE = "Export Route plan";
        private List<KeyValuePair<String, double>> data = new List<KeyValuePair<String, double>>();
        public override bool GetData(EDDiscoveryForm _discoveryForm)
        {
            if(_discoveryForm.RouteControl.RouteSystems==null
                || _discoveryForm.RouteControl.RouteSystems.Count==0)
            {

                MessageBox.Show(String.Format("Please create a route on the route tab"), TITLE);
                return false;
            }

            Point3D last = null;
            foreach (SystemClass s in _discoveryForm.RouteControl.RouteSystems)
            {
               Point3D pos = new Point3D(s.x, s.y, s.z);
                double dist = 0;
                if (last != null)
                {
                    dist = Point3D.DistanceBetween(pos, last);
                }
                last = pos;
                data.Add(new KeyValuePair<String, double>(s.name, dist));
            }
            return true; 
        }

        public override bool ToCSV(string filename)
        {
            try
            {

                using (StreamWriter writer = new StreamWriter(filename))
                {
                    if (IncludeHeader)
                    {
                        writer.Write("System" + delimiter);
                        writer.Write("Distance" + delimiter);
                        writer.Write("Total distance");
                        writer.WriteLine();
                    }
                    double totalDist = 0;
                    foreach (KeyValuePair<String, double>  item in data)
                    {
                        writer.Write(MakeValueCsvFriendly(item.Key));
                        writer.Write(MakeValueCsvFriendly(item.Value.ToString("0.00")));
                        totalDist += item.Value;
                        writer.Write(MakeValueCsvFriendly(totalDist.ToString("0.00")));
                        writer.WriteLine();
                    }
                }
                return true;
            }
            catch (IOException exx)
            {
                MessageBox.Show(String.Format("Is file {0} open?", filename), TITLE,
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
    }
}
