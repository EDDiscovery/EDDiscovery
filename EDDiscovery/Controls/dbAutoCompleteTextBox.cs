using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ExtendedControls
{
    class dbAutoCompleteTextBox : TextBoxBorder 
    {
        public dbAutoCompleteTextBox() : base()
        {
            TextChanged += TextChangeEventHandler;
        }

        protected void TextChangeEventHandler(object sender, EventArgs e)
        {
        }
    }
}
