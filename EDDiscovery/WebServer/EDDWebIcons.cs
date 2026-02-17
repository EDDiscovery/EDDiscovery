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
using BaseUtils.WebServer;
using System.Drawing;
using System.Net;

namespace EDDiscovery.WebServer
{
    class EDDIconNodes : IHTTPNode
    {
        public NodeResponse Response(string partialpath, HttpListenerRequest request)
        {
            System.Diagnostics.Debug.WriteLine("Serve icon " + partialpath);

            if (partialpath.Contains(".png"))
            {
                string nopng = partialpath.Replace(".png", "");

                Bitmap img;


                if (nopng.Contains("."))       // if path, use it
                    img = BaseUtils.Icons.IconSet.GetImage(nopng) as Bitmap;
                else if (BaseUtils.Icons.IconSet.Contains("Highres." + nopng))  // no path, may be a hires one - check first for preference
                    img = BaseUtils.Icons.IconSet.GetImage("Highres." + nopng) as Bitmap;
                else if (BaseUtils.Icons.IconSet.Contains("Journal." + nopng))  // no path, may be a journal one
                    img = BaseUtils.Icons.IconSet.GetImage("Journal." + nopng) as Bitmap;
                else if (BaseUtils.Icons.IconSet.Contains("General." + nopng))  // no path, may be a general one
                    img = BaseUtils.Icons.IconSet.GetImage("General." + nopng) as Bitmap;
                else
                    img = BaseUtils.Icons.IconSet.GetImage(nopng) as Bitmap;

                //try       // debug only
                //{
                Bitmap bmpclone = img.Clone() as Bitmap;
                var cnv = bmpclone.ConvertTo(System.Drawing.Imaging.ImageFormat.Png);   // this converts to png and returns the raw PNG bytes..
                return new NodeResponse(cnv, "image/png");
                //}
                //catch (Exception ex)
                //{
                //System.Diagnostics.Debug.WriteLine("..Convert exception " + ex);
                //return null;
                //}
            }

            return null;
        }
    }
}



