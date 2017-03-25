﻿/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
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

                EDDiscovery.Forms.MessageBoxTheme.Show(String.Format("Please create a route on the route tab"), TITLE);
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
                        writer.Write(MakeValueCsvFriendly(totalDist.ToString("0.00"),false));
                        writer.WriteLine();
                    }
                }
                return true;
            }
            catch (IOException )
            {
                EDDiscovery.Forms.MessageBoxTheme.Show(String.Format("Is file {0} open?", filename), TITLE,
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
    }
}
