/*
 * Copyright 2023-2023 Robbyxp1 @ github.com
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

using GLOFC;
using GLOFC.GL4;
using GLOFC.Utils;
using OpenTK;
using QuickJSON;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace EDDiscovery.UserControls.Map3D
{
    public class ImageCache
    {
        public class ImageEntry
        {
            public bool Enabled { get; set; }
            public string Name { get; set; }                // name given to it, for preselected ones only
            public string ImagePathOrURL { get; set; }      // http:... or c:\ or Resource:<name>

            [JsonIgnore(JsonIgnoreAttribute.Operation.Include, "X", "Y", "Z")]
            public Vector3 Centre { get; set; }
            [JsonIgnore(JsonIgnoreAttribute.Operation.Include, "X", "Y")]
            public Vector2 Size { get; set; }
            [JsonIgnore(JsonIgnoreAttribute.Operation.Include, "X", "Y", "Z")]
            public Vector3 RotationDegrees { get; set; }
            public bool RotateToViewer { get; set; }
            public bool RotateElevation { get; set; }
            public float AlphaFadeScalar { get; set; }
            public float AlphaFadePosition { get; set; }

            [JsonIgnore]
            public Bitmap Bitmap { get; set; }              // when bitmap has been loaded, this is it. Use LoadBitmaps to set this up
            [JsonIgnore]
            public bool OwnBitmap { get; set; }             // if we own it, else the application owns it
            public ImageEntry()                             // need this for JSON -> object!
            { }

            public ImageEntry(string name, string path, bool enabled, Vector3 centre, Vector2 size, Vector3 rotation,
                                bool rotviewer = false, bool rotelevation = false, float alphafadescaler = 0, float alphafadepos = 1)
            {
                Name = name; ImagePathOrURL = path; Enabled = enabled; Centre = centre; Size = size; RotationDegrees = rotation;
                RotateToViewer = rotviewer; RotateElevation = rotelevation; AlphaFadeScalar = alphafadescaler; AlphaFadePosition = alphafadepos;
            }
            public bool LoadBitmap(string file, bool ownbmp = true)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"Load bitmap from {ImagePathOrURL} file {file} own {ownbmp}");
                    Bitmap = new Bitmap(file);
                    OwnBitmap = ownbmp;
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Load bitmap exception {ImagePathOrURL} : {ex}");
                    return false;
                }
            }
        }

        public int CountLoaded { get { return images.Count(x => x.Bitmap != null); } }

        public List<ImageEntry> GetImageList() { return new List<ImageEntry>(images); }

        public void SetImageList(List<ImageEntry> newlist) { images = newlist; }

        public ImageCache(GLItemsList items, GLRenderProgramSortedList rObjects)
        {
        }

        public void LoadFromString(string res)
        {
            images.Clear();
            var split = res.Split('\u2345');
            foreach (var s in split)
            {
                var entries = s.Split('\u2346');
                if (entries.Length == 10)
                {
                    Vector3? centre = entries[3].InvariantParseVector3();
                    Vector2? size = entries[4].InvariantParseVector2();
                    Vector3? rotationdeg = entries[5].InvariantParseVector3();
                    bool rotatetoviewer = entries[6].InvariantParseBool(false);
                    bool rotateelevation = entries[7].InvariantParseBool(false);
                    float alphafadescaler = entries[8].InvariantParseFloat(0);
                    float alphafadeposition = entries[9].InvariantParseFloat(0);

                    if (centre != null && size != null && rotationdeg != null)
                    {
                        images.Add(new ImageEntry(entries[0], entries[1], entries[2].InvariantParseBool(false), centre.Value, size.Value, rotationdeg.Value, rotatetoviewer, rotateelevation, alphafadescaler, alphafadeposition));
                    }

                }
            }
        }

        public string ImageStringList()
        {
            string res = "";
            for (int i = 0; i < images.Count; i++)
            {
                res = res.AppendPrePad(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}\u2346{1}\u2346{2}\u2346{3}\u2346{4}\u2346{5}\u2346{6}\u2346{7}\u2346{8}\u2346{9}",
                                images[i].Name,
                                images[i].ImagePathOrURL,
                                images[i].Enabled,
                                images[i].Centre.ToStringInvariant(),
                                images[i].Size.ToStringInvariant(),
                                images[i].RotationDegrees.ToStringInvariant(),
                                images[i].RotateToViewer.ToStringInvariant(),
                                images[i].RotateElevation.ToStringInvariant(),
                                images[i].AlphaFadeScalar.ToStringInvariant(),
                                images[i].AlphaFadePosition.ToStringInvariant()
                                ),
                                "\u2345");
            }

            return res;
        }

        /// <summary>
        /// Load given bitmaps
        /// </summary>
        /// <param name="resourceassembly">null, or resource assembly for Resource: </param>
        /// <param name="resourcepath">path to resource, something like TestOpenTk.Properties.Resources</param>
        public void LoadBitmaps(Action<ImageEntry> resources,
                                Action<ImageEntry> text,
                                Action<ImageEntry> httpload,
                                Action<ImageEntry> fileload,
                                Action<ImageEntry> imageload
            )
        {
            foreach (var ie in Enumerable.Reverse(images))
            {
                if (ie.OwnBitmap)
                    ie.Bitmap?.Dispose();
                ie.Bitmap = null;
                ie.OwnBitmap = false;

                if (ie.Enabled)
                {
                    try
                    {
                        if (ie.ImagePathOrURL.StartsWith("Resource:", System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (resources != null)
                            {
                                resources.Invoke(ie);
                            }
                        }
                        else if (ie.ImagePathOrURL.StartsWith("\""))
                        {
                            if (text != null)
                            {
                                text.Invoke(ie);
                            }
                        }
                        else if (ie.ImagePathOrURL.StartsWith("Image:", System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (imageload != null)
                            {
                                imageload.Invoke(ie);
                            }
                        }
                        else if (ie.ImagePathOrURL.StartsWith("http", System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (httpload != null)
                            {
                                httpload.Invoke(ie);
                            }
                        }
                        else if (File.Exists(ie.ImagePathOrURL))
                        {
                            if (ie.LoadBitmap(ie.ImagePathOrURL))
                            {
                                fileload.Invoke(ie);
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Imagecache unknown image location for {ie.ImagePathOrURL} ");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Imagecache image {ie.ImagePathOrURL} cannot load {ex}");
                    }
                }
            }
        }

        public void Add(ImageEntry img)
        {
            images.Add(img);
        }
        public void Remove(ImageEntry img)
        {
            images.Remove(img);
        }

        private List<ImageEntry> images = new List<ImageEntry>();
    }

}
//}
