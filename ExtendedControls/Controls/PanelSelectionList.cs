using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public partial class PanelSelectionList : Panel
    {
        public event EventHandler SelectedIndexChanged;

        public Color SelectionMarkColor { get; set; } = Color.Yellow;
        public int SelectionSize { get; set; } = 8;

        public List<string> Items { get { return ddc.Items; } set { ddc.Items = value; } }

        public override Color ForeColor { get { return base.ForeColor; } set { ddc.ForeColor = base.ForeColor = value; } }
        public bool FitToItemsHeight { get { return ddc.FitToItemsHeight; } set { ddc.FitToItemsHeight = value; } }
        public int ScrollBarWidth { get { return ddc.ScrollBarWidth; } set { ddc.ScrollBarWidth = value; } }
        public int DropDownWidth { get { return dropdownwidth; } set { dropdownwidth = value; } }
        public int DropDownHeight { get { return dropdownheight; } set { dropdownheight = value; } }
        public int ItemHeight { get { return ddc.ItemHeight; } set { ddc.ItemHeight = value; } }
        public Color BorderColor { get { return ddc.BorderColor; } set { ddc.BorderColor = value; } } 
        public FlatStyle FlatStyle { get { return ddc.FlatStyle; } set { ddc.FlatStyle = value; } }
        public Color SelectionBackColor { get { return ddc.SelectionBackColor; } set { ddc.SelectionBackColor = value; } }
        public float GradientColorScaling { get { return ddc.GradientColorScaling; } set { ddc.GradientColorScaling = value; } }
        public Color ScrollBarColor { get { return ddc.ScrollBarColor; } set { ddc.ScrollBarColor = value; } }
        public Color ScrollBarButtonColor { get { return ddc.ScrollBarButtonColor; } set { ddc.ScrollBarButtonColor = value; } }
        public Color MouseOverBackgroundColor { get { return ddc.MouseOverBackgroundColor; } set { ddc.MouseOverBackgroundColor = value; } }

        public int SelectedIndex { get { return ddc.SelectedIndex; } set { ddc.SelectedIndex = value; } }
        public string SelectedItem { get { return (ddc.SelectedIndex>=0) ? ddc.Items[ddc.SelectedIndex] : null; }  }

        private DropDownCustom ddc;
        int dropdownwidth = 200;
        int dropdownheight = 400;

        public PanelSelectionList()
        {
            InitializeComponent();
            ddc = new DropDownCustom("PSDD",false);
            ddc.SelectedIndexChanged += Ddc_SelectedIndexChanged;
            ddc.Activated += Ddc_DropDown;
            ddc.Deactivate += Ddc_Deactivate;
        }

        private void Ddc_DropDown(object sender, EventArgs e)
        {
            Point location = this.PointToScreen(new Point(Width-dropdownwidth, Height));
            ddc.Location = location;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = ClientRectangle;
            
            if (BorderColor != Color.Transparent)
            {
                rect.Height--;
                rect.Width--;
                using (Pen p1 = new Pen(BorderColor, 1.0F))
                    e.Graphics.DrawRectangle(p1, rect);
            }

            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

            using (SolidBrush b1 = new SolidBrush(SelectionMarkColor))
            {
                int x = rect.Right;
                int y = rect.Bottom;

                Point[] tri = new Point[] { new Point(x - SelectionSize, y), new Point(x, y - SelectionSize), new Point(x, y) };

                e.Graphics.FillPolygon(b1, tri);
            }
        }

        bool dropdown = false;

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if ( e.X>=Width-SelectionSize && e.Y>=Height-SelectionSize)
            {
                if (dropdown == false)
                {
                    int fittableitems = this.DropDownHeight / this.ItemHeight;
                    if (fittableitems == 0)
                        fittableitems = 5;

                    if (fittableitems > this.Items.Count())                             // no point doing more than we have..
                        fittableitems = this.Items.Count();

                    ddc.Size = new Size(this.DropDownWidth > 9 ? this.DropDownWidth : this.Width, fittableitems * this.ItemHeight + 4);
                    ddc.Show(FindForm());
                }
                else
                {
                    ddc.Hide();
                }

                dropdown = !dropdown;

            }
        }

        private void Ddc_Deactivate(object sender, EventArgs e)
        {
            dropdown = false;
            ddc.Hide();
        }

        private void Ddc_SelectedIndexChanged(object sender, EventArgs e)
        {
            dropdown = false;
            ddc.Hide();
            SelectedIndexChanged?.Invoke(this, e);
        }
    }
}
