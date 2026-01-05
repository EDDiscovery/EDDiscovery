/*
 * Copyright © 2019-2021 EDDiscovery development team
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

using BaseUtils;
using QuickJSON;
using BaseUtils.WebServer;
using EliteDangerousCore;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace EDDiscovery.WebServer
{
    class EDDScanDisplay : IHTTPNode
    {
        private EDDiscoveryForm discoveryform;
        private Server server;

        public EDDScanDisplay(EDDiscoveryForm f, Server serverp)
        {
            discoveryform = f;
            server = serverp;
        }

        public JToken Notify()       // a full refresh of journal history
        {
            JObject response = new JObject();
            response["responsetype"] = "systemmapchanged";
            return response;
        }

        public NodeResponse Response(string partialpath, HttpListenerRequest request)
        {
            System.Diagnostics.Debug.WriteLine("Serve Scan Display " + partialpath);
            //foreach (var k in request.QueryString.AllKeys)   System.Diagnostics.Debug.WriteLine("Key {0} = {1}", k, request.QueryString[k]);

            int entry = (request.QueryString["entry"] ?? "-1").InvariantParseInt(-1);
            bool checkEDSM = (request.QueryString["EDSM"] ?? "false").InvariantParseBool(false);
            bool checkSPANSH = (request.QueryString["SPANSH"] ?? "false").InvariantParseBool(false);

            Bitmap img = null;
            JObject response = new JObject();
            response["responsetype"] = "scandisplayobjects";
            JArray objectlist = new JArray();

            var hl = discoveryform.History;
            if (hl.Count > 0)
            {
                if (entry < 0 || entry >= hl.Count)
                    entry = hl.Count - 1;

                // seen instances of exceptions accessing icons in different threads.  so push up to discovery form. need to investigate.

                discoveryform.Invoke((MethodInvoker)delegate
                {
                    var lookup = checkEDSM ? (checkSPANSH ? EliteDangerousCore.WebExternalDataLookup.SpanshThenEDSM : WebExternalDataLookup.EDSM) :
                                checkSPANSH ? EliteDangerousCore.WebExternalDataLookup.Spansh : EliteDangerousCore.WebExternalDataLookup.None;

                    var sn = hl.StarScan2.FindSystemSynchronous(hl.EntryOrder()[entry].System, lookup);

                    if (sn != null)
                    {
                        int starsize = (request.QueryString["starsize"] ?? "48").InvariantParseInt(48);
                        int width = (request.QueryString["width"] ?? "800").InvariantParseInt(800);
                        var sd = new EliteDangerousCore.StarScan2.SystemDisplay();
                        sd.ShowMoons = (request.QueryString["showmoons"] ?? "true").InvariantParseBool(true);
                        sd.ShowOverlays = (request.QueryString["showbodyicons"] ?? "true").InvariantParseBool(true);
                        sd.ShowMaterials = (request.QueryString["showmaterials"] ?? "true").InvariantParseBool(true);
                        sd.ShowAllG = (request.QueryString["showgravity"] ?? "true").InvariantParseBool(true);
                        sd.ShowPlanetMass = (request.QueryString["showplanetmass"] ?? "false").InvariantParseBool(false);
                        sd.ShowStarMass = (request.QueryString["showstarmass"] ?? "false").InvariantParseBool(false);
                        sd.ShowStarAge = (request.QueryString["showstarage"] ?? "true").InvariantParseBool(true);
                        sd.ShowHabZone = (request.QueryString["showhabzone"] ?? "true").InvariantParseBool(true);
                        sd.ShowStarClasses = (request.QueryString["showstarclass"] ?? "true").InvariantParseBool(true);
                        sd.ShowPlanetClasses = (request.QueryString["showplanetclass"] ?? "true").InvariantParseBool(true);
                        sd.ShowDist = (request.QueryString["showdistance"] ?? "true").InvariantParseBool(true);
                        sd.ValueLimit = (request.QueryString["valuelimit"] ?? "50000").InvariantParseInt(50000);
                        sd.ShowWebBodies = checkEDSM;
                        sd.SetSize(starsize);
                        sd.Font = new Font("MS Sans Serif", 8.25f);
                        sd.FontLarge = new Font("MS Sans Serif", 10f);
                        sd.FontUnderlined = new Font("MS Sans Serif", 8.25f, FontStyle.Underline);
                        ExtendedControls.ExtPictureBox imagebox = new ExtendedControls.ExtPictureBox();
                        sd.DrawSystemRender(imagebox, width, sn, null, null);
                        //imagebox.AddTextAutoSize(new Point(10, 10), new Size(1000, 48), "Generated on " + DateTime.UtcNow.ToString(), new Font("MS Sans Serif", 8.25f), Color.Red, Color.Black, 0);
                        imagebox.Render();

                        foreach (ExtendedControls.ImageElement.Element e in imagebox)
                        {
                            if (e.ToolTipText.HasChars())
                            {
                                //     System.Diagnostics.Debug.WriteLine("{0} = {1}", e.Location, e.ToolTipText);
                                objectlist.Add(new JObject() { ["left"] = e.Location.X, ["top"] = e.Location.Y, ["right"] = e.Bounds.Right, ["bottom"] = e.Bounds.Bottom, ["text"] = e.ToolTipText });
                            }
                        }

                        img = imagebox.Image.Clone() as Bitmap;
                        imagebox.Dispose();
                    }
                });

            }
            else
            {
                discoveryform.Invoke((MethodInvoker)delegate
                {
                    img = BaseUtils.Icons.IconSet.GetIcon("Bodies.Unknown") as Bitmap;
                });
            }

            response["objectlist"] = objectlist;
            server.SendWebSockets(response, false); // refresh history

            Bitmap bmpclone = img.Clone() as Bitmap;
            var cnv = bmpclone.ConvertTo(System.Drawing.Imaging.ImageFormat.Png);   // this converts to png and returns the raw PNG bytes..
            WebHeaderCollection wc = new WebHeaderCollection();                     // indicate don't cache this, this is a temp image
            wc[HttpRequestHeader.CacheControl] = "no-store";
            return new NodeResponse(cnv, "image/png", wc);
        }
    }
}



