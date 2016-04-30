using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing;

namespace ExtendedControls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripComboBoxCustom : System.Windows.Forms.ToolStripControlHost
    {
        public ComboBoxCustom ComboBox { get { return (ComboBoxCustom)base.Control; } }

        public ToolStripComboBoxCustom() : base(new ComboBoxCustom())
        {
        }

        public Color MouseOverBackgroundColor { get { return ComboBox.MouseOverBackgroundColor; } set { ComboBox.MouseOverBackgroundColor = value; } }
        public Color BorderColor { get { return ComboBox.BorderColor; } set { ComboBox.BorderColor = value; } }
        public Color DropDownBackgroundColor { get { return ComboBox.DropDownBackgroundColor; } set { ComboBox.DropDownBackgroundColor = value; } }
        public Color ScrollBarColor { get { return ComboBox.ScrollBarColor; } set { ComboBox.ScrollBarColor = value; } }
        public Color ScrollBarButtonColor { get { return ComboBox.ScrollBarButtonColor; } set { ComboBox.ScrollBarButtonColor = value; } }

        public FlatStyle FlatStyle { get { return ComboBox.FlatStyle; } set { ComboBox.FlatStyle = value; } }
        public int DropDownHeight { get { return ComboBox.DropDownHeight; } set { ComboBox.DropDownHeight = value; } }
        public int ArrowWidth { get { return ComboBox.ArrowWidth; } set { ComboBox.ArrowWidth = value; } }
        public float ButtonColorScaling { get { return ComboBox.ButtonColorScaling; } set { ComboBox.ButtonColorScaling = value; } }
        public int ScrollBarWidth { get { return ComboBox.ScrollBarWidth; } set { ComboBox.ScrollBarWidth = value; } }
        public int ItemHeight { get { return ComboBox.ItemHeight; } set { ComboBox.ItemHeight = value; } }
        public int SelectedIndex { get { return ComboBox.SelectedIndex; } set { ComboBox.SelectedIndex = value; } }
        public List<string> Items { get { return ComboBox.Items; } set { ComboBox.Items = value; } }
        public object DataSource { get { return ComboBox.DataSource; } set { ComboBox.DataSource = value; } }
        public string DisplayMember { get { return ComboBox.DisplayMember; } set { ComboBox.DisplayMember = value; } }
        public string ValueMember { get { return ComboBox.ValueMember; } set { ComboBox.ValueMember = value; } }
        public object SelectedItem { get { return ComboBox.SelectedItem; } set { ComboBox.SelectedItem = value; } }
        public object SelectedValue { get { return ComboBox.SelectedValue; } }

        public event ComboBoxCustom.OnSelectedIndexChanged SelectedIndexChanged { add { ComboBox.SelectedIndexChanged += value; } remove { ComboBox.SelectedIndexChanged -= value; } }
    }
}
