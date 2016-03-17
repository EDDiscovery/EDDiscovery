using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2
{
    public class EDDTheme
    {
        private Color forecolor;
        private Color backcolor;
        private Color textcolor;
        private Color texthighlightcolor;
        private Color visitedsystemcolor;

        public Color ForeColor
        {
            get
            {
                return forecolor;
            }

            set
            {
                forecolor = value;
            }
        }

        public Color BackColor
        {
            get
            {
                return backcolor;
            }

            set
            {
                backcolor = value;
            }
        }

        public Color TextColor
        {
            get
            {
                return textcolor;
            }

            set
            {
                textcolor = value;
            }
        }

        public Color TextHighlightColor
        {
            get
            {
                return texthighlightcolor;
            }

            set
            {
                texthighlightcolor = value;
            }
        }

        public Color VisitedsystemColor
        {
            get
            {
                return visitedsystemcolor;
            }

            set
            {
                visitedsystemcolor = value;
            }
        }

        internal void SetThemeBlack()
        {
            forecolor = Color.IndianRed;
            backcolor = Color.Black;
            textcolor = Color.Orange;
            texthighlightcolor = Color.Red;
            visitedsystemcolor = Color.White;
        }

        public void ApplyTheme(Form form)
        {
            form.ForeColor = forecolor;
            form.BackColor = backcolor;


            foreach (Control c in form.Controls)
            {
                UpdateColorControls(c);
            }
        }

        public void UpdateColorControls(Control myControl)
        {
            try
            {
                myControl.BackColor = backcolor;
                myControl.ForeColor = forecolor;
            }
            catch { }

            if (myControl is DataGridView)
            {
                DataGridView MyDgv = (DataGridView)myControl;
                MyDgv.ColumnHeadersDefaultCellStyle.BackColor = backcolor;     // NOT WORKING
                MyDgv.ColumnHeadersDefaultCellStyle.ForeColor = forecolor;
                MyDgv.BackgroundColor = backcolor;
                MyDgv.DefaultCellStyle.BackColor = backcolor;
                MyDgv.DefaultCellStyle.ForeColor = forecolor;
            }

            foreach (Control subC in myControl.Controls)
            {
                UpdateColorControls(subC);
            }
        }

    }
}
