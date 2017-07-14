using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public interface ThemeableForms
    {
        bool ApplyToForm(System.Windows.Forms.Form form, System.Drawing.Font fnt);
        void ApplyToControls(System.Windows.Forms.Control parent, System.Drawing.Font fnt = null);
        System.Drawing.Color TextBlockColor { get; set; }
        string FontName { get; set; }
        bool WindowsFrame { get; set; }
    }

    public static class ThemeAbleFormsInstance
    {
        static public ThemeableForms Instance { get; set; }
    }
}
