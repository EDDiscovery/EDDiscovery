/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace OpenTKUtils
{
    public interface IDrawingPrimative      // a single object to draw (can of course be complex set of images/lines)
    {
        PrimitiveType Type { get; }
        float Size { get; set; }
        Color Color { get; set; }
        void Draw(GLControl control);
    }

    public interface IData3DCollection
    {
        string Name { get; set; }
        void DrawAll(GLControl control);
    }

    // Class holding a list of IDrawingPrimitives. Class conforms to the interface IData3DCollection

    public class Data3DCollection<T> : IData3DCollection where T : IDrawingPrimative
    {
        public string Name { get; set; }
        public IList<T> Primatives { get; protected set; }
        public Color Color;
        public float Size;

        public bool Visible;

        protected Data3DCollection(string name, Color color, float pointsize)
        {
            Name = name;
            this.Color = color;
            Size = pointsize;
            Primatives = new List<T>();
            Visible = true;
        }

        public virtual void Add(T primative)
        {
            primative.Color = this.Color;
            primative.Size = Size;
            Primatives.Add(primative);
        }

        public virtual void DrawAll(GLControl control)
        {
            if (!Visible) return;

            if (control.InvokeRequired)
            {
                control.Invoke(new Action<GLControl>(this.DrawAll), control);
            }
            else
            {
                foreach (var primative in Primatives)
                {
                    primative.Draw(control);
                }
            }
        }

        public static Data3DCollection<T> Create(string name, Color color, float pointsize)
        {
            if (typeof(T) == typeof(PointData))
            {
                return new PointDataCollection(name, color, pointsize) as Data3DCollection<T>;
            }
            else if (typeof(T) == typeof(LineData))
            {
                return new LineDataCollection(name, color, pointsize) as Data3DCollection<T>;
            }
            else if (typeof(T) == typeof(TexturedQuadData))
            {
                return new TexturedQuadDataCollection(name, color, pointsize) as Data3DCollection<T>;
            }
            else
            {
                return new Data3DCollection<T>(name, color, pointsize);
            }
        }
    }


}
