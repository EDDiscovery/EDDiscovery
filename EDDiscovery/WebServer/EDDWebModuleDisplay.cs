/*
 * Copyright 2026-2026 EDDiscovery development team
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
 */

using BaseUtils;
using QuickJSON;
using BaseUtils.WebServer;
using EliteDangerousCore;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace EDDiscovery.WebServer
{
    class EDDModuleDisplay : IHTTPNode
    {
        private EDDiscoveryForm discoveryform;
        private Server server;

        public EDDModuleDisplay(EDDiscoveryForm f, Server serverp)
        {
            discoveryform = f;
            server = serverp;
        }

        public JToken Notify()       // a full refresh of journal history
        {
            JObject response = new JObject();
            response["responsetype"] = "moduleschanged";
            return response;
        }

        public NodeResponse Response(string partialpath, HttpListenerRequest request)
        {
            System.Diagnostics.Debug.WriteLine("Serve Modules Display " + partialpath);

            var hl = discoveryform.History.LastOrDefault;
            var instance = hl?.ShipInformation;
            var ship = instance?.GetShipProperties();
            Bitmap bmp = null;
            JArray objectlist = new JArray();

            if (instance!=null)
            {
                discoveryform.Invoke((MethodInvoker)delegate
                {
                    ShipModuleDisplay smd = new ShipModuleDisplay();
                    smd.TextForeColor = Color.DarkOrange;
                    smd.BoxBackColor1 = Color.FromArgb(255, 64, 64, 64);
                    smd.BoxBackColor2 = Color.FromArgb(255, 48, 48, 48);
                    smd.BoxBorderColor = Color.FromArgb(255, 128,128, 128);
                    smd.Font = new System.Drawing.Font("Arial", 8);
                    smd.FontLarge = new System.Drawing.Font("Arial", 12);

                    int width = (request.QueryString["width"] ?? "800").InvariantParseInt(800);
                    string color = request.QueryString["textcolor"] ?? "#ff8000";
                    smd.TextForeColor = color.ColorFromNameOrValues();

                    var il = smd.CreateImages(ship, instance, new Point(4,8), width, instance.Name, true,false);
                    bmp = il.Paint(Color.Transparent);

                    foreach (ExtendedControls.ImageElement.Element e in il)
                    {
                        if (e.ToolTipText.HasChars())
                        {
                            //     System.Diagnostics.Debug.WriteLine("{0} = {1}", e.Location, e.ToolTipText);
                            objectlist.Add(new JObject() { ["left"] = e.Location.X, ["top"] = e.Location.Y, ["right"] = e.Bounds.Right, ["bottom"] = e.Bounds.Bottom, ["text"] = e.ToolTipText });
                        }
                    }
                });
            }
            else
            {
                discoveryform.Invoke((MethodInvoker)delegate
                {
                    bmp = BaseUtils.Icons.IconSet.GetImage("Controls.Stop") as Bitmap;
                });
            }

            // send thru the web socket a scan display objects prompt giving a new set of objects

            JObject response = new JObject();
            response["responsetype"] = "moduledisplayobjects";
            response["objectlist"] = objectlist;
            server.SendWebSockets(response, false); // refresh history

            var cnv = bmp.ConvertTo(System.Drawing.Imaging.ImageFormat.Png);   // this converts to png and returns the raw PNG bytes..
            WebHeaderCollection wc = new WebHeaderCollection();                     // indicate don't cache this, this is a temp image
            wc[HttpRequestHeader.CacheControl] = "no-store";

            bmp.Dispose();
            return new NodeResponse(cnv, "image/png", wc);
        }
    }
}



