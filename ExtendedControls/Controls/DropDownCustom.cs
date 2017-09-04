using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class DropDownCustom : Form
    {
        private ListControlCustom _listcontrol;

        public event EventHandler SelectedIndexChanged;
        public event KeyPressEventHandler KeyPressed;
        public event KeyEventHandler OtherKeyPressed;

        public List<string> Items { get { return _listcontrol.Items; } set { _listcontrol.Items = value; } }
        public Color MouseOverBackgroundColor { get { return _listcontrol.MouseOverBackgroundColor; } set { _listcontrol.MouseOverBackgroundColor = value; } }
        public int SelectedIndex { get { return _listcontrol.SelectedIndex; } set { _listcontrol.SelectedIndex = value; } }
        public Color SelectionBackColor { get { return _listcontrol.SelectionBackColor; } set { _listcontrol.SelectionBackColor = value; this.BackColor = value; } }
        public Color BorderColor { get { return _listcontrol.BorderColor; } set { _listcontrol.BorderColor = value; } }
        public Color ScrollBarColor { get { return _listcontrol.ScrollBarColor; } set { _listcontrol.ScrollBarColor = value; } }
        public Color ScrollBarButtonColor { get { return _listcontrol.ScrollBarButtonColor; } set { _listcontrol.ScrollBarButtonColor = value; } }
        public FlatStyle FlatStyle { get { return _listcontrol.FlatStyle; } set { _listcontrol.FlatStyle = value; } }
        public new Font Font { get { return base.Font; } set { base.Font = value; _listcontrol.Font = value; } }
        public bool FitToItemsHeight { get { return _listcontrol.FitToItemsHeight; } set { _listcontrol.FitToItemsHeight = value; } }
        public int ScrollBarWidth { get { return _listcontrol.ScrollBarWidth; } set { _listcontrol.ScrollBarWidth = value; } }
        public float GradientColorScaling { get { return _listcontrol.GradientColorScaling; } set { _listcontrol.GradientColorScaling = value; } }
        public int ItemHeight { get { return _listcontrol.ItemHeight; } set { _listcontrol.ItemHeight = value; } }

        private bool closeondeactivateselected;

        public DropDownCustom(string name = "", bool closeondeact = true)
        {
            closeondeactivateselected = closeondeact;

            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this._listcontrol = new ListControlCustom();
            this.Name = this._listcontrol.Name = name;
            this._listcontrol.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this._listcontrol.Dock = DockStyle.Fill;
            this._listcontrol.Visible = true;
            this._listcontrol.SelectedIndexChanged += _listcontrol_SelectedIndexChanged;
            this._listcontrol.KeyPressed += _listcontrol_KeyPressed;
            this._listcontrol.OtherKeyPressed += _listcontrol_OtherKeyPressed;
            this._listcontrol.Margin = new Padding(0);
            this.Padding = new Padding(0);
            this.Controls.Add(this._listcontrol);
        }

        public void KeyDownAction(KeyEventArgs e)
        {
            _listcontrol.KeyDownAction(e);
        }

        private void _listcontrol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (closeondeactivateselected)
                this.Close();

            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);

        }

        private void _listcontrol_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (KeyPressed != null)
                KeyPressed(this, e);
        }

        private void _listcontrol_OtherKeyPressed(object sender, KeyEventArgs e)
        {
            if (OtherKeyPressed != null)
                OtherKeyPressed(this, e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            if ( closeondeactivateselected)
                this.Close();
        }
    }

}
