using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public class CommandArgs
    {
        private string[] args;
        private int pos;

        public CommandArgs(string[] a)
        {
            args = a;
            pos = 0;
        }

        public CommandArgs(CommandArgs other)
        {
            args = other.args;
            pos = other.pos;
        }

        public string Next { get { return (pos < args.Length) ? args[pos++] : null; } }
        public int Int { get { return (pos < args.Length) ? args[pos++].InvariantParseInt(0) : 0; } }
        public string this[int v] { get { int left = args.Length - pos; return (v < left) ? args[pos + v] : null; } }
        public int Left { get { return args.Length - pos; } }
        public void Remove() { if (pos < args.Length) pos++; }
    }
}
