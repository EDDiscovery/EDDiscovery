/*
 * Copyright © 2018 EDDiscovery development team
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

namespace BaseUtils
{
    public class CommandArgs
    {
        private string[] args;
        private int pos;

        public CommandArgs(string[] a, int index = 0)
        {
            args = a;
            pos = index;
        }

        public CommandArgs(CommandArgs other)
        {
            args = other.args;
            pos = other.pos;
        }

        public string Peek { get { return (pos < args.Length) ? args[pos] : null; } }

        public string Next() { return (pos < args.Length) ? args[pos++] : null; }
        public string NextEmpty() { return (pos < args.Length) ? args[pos++] : ""; }
        public int Int() { return (pos < args.Length) ? args[pos++].InvariantParseInt(0) : 0; }
        public string Rest(string sep = " ") { return string.Join(sep, args, pos, args.Length - pos); }

        public string this[int v] { get { int left = args.Length - pos; return (v < left) ? args[pos + v] : null; } }
        public bool More { get { return args.Length > pos; } }
        public int Left { get { return args.Length - pos; } }
        public void Remove() { if (pos < args.Length) pos++; }
    }
}
