/*
 * Copyright © 2017 EDDiscovery development team
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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Icons
{
    public class IconGroup<T> : IReadOnlyDictionary<T, Image>
    {
        protected Dictionary<T, Image> icons;

        public IconGroup(string basedir)
        {
            Init(basedir, Enum.GetValues(typeof(T)).OfType<T>());
        }

        protected void Init(string basedir, IEnumerable<T> keys)
        {
            icons = keys.ToDictionary(e => e, e => EDDiscovery.Icons.IconSet.GetIcon(basedir + "." + e.ToString()));
        }

        public Image this[T key] => icons[key];
        public IEnumerable<T> Keys => icons.Keys;
        public IEnumerable<Image> Values => icons.Values;
        public int Count => icons.Count;
        public bool ContainsKey(T key) => icons.ContainsKey(key);
        public IEnumerator<KeyValuePair<T, Image>> GetEnumerator() => icons.GetEnumerator();
        public bool TryGetValue(T key, out Image value) => icons.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => icons.GetEnumerator();
    }
}
