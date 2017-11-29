using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedControls
{
    public interface IIconPackControl
    {
        string BaseName { get; }
        void ReplaceImages(IconPackImageReplacer replacer);
    }

    public delegate void IconPackImageReplacer(Action<Image> setter, string name);
}
