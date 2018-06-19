using Conditions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class ProfileEditor : ExtendedControls.DraggableForm
    {
        class Group
        {
            public Panel panel;
            public ExtendedControls.RichTextBoxScroll name;
            public ExtendedControls.ComboBoxCustom stdtrigger;
            public ExtendedControls.ButtonExt editbutton;
            public ExtendedControls.ButtonExt deletebutton;
            public ConditionLists condition;
        }

        List<Group> groups = new List<Group>();

        Profiles profiles;

        public ProfileEditor()
        {
            InitializeComponent();
        }

        public void Init(Profiles pr, Icon ic)
        {
            profiles = pr;
            Icon = ic;

            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            bool winborder = theme.ApplyToFormStandardFontSize(this);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;

            SuspendLayout();
            foreach (Profiles.Profile p in profiles.ProfileList)
            {
                AddProfile(p.Name, p.Condition );
            }

            PositionPanels();

            ResumeLayout();
        }

        int textheightmargin = 3;
        int panelleftmargin = 3;

        private void AddProfile(string name, ConditionLists condition )
        {
            Group g = new Group();
            g.panel = new Panel();
            g.panel.BorderStyle = BorderStyle.FixedSingle;
            g.panel.Tag = g;

            g.name = new ExtendedControls.RichTextBoxScroll();
            g.name.Location = new Point(4, textheightmargin);      // 8 spacing, allow 8*4 to indent
            g.name.Size = new Size(200, 24);
            g.name.Text = name;
            g.panel.Controls.Add(g.name);

            g.stdtrigger = new ExtendedControls.ComboBoxCustom();
            g.stdtrigger.Location = new Point(210, textheightmargin);      // 8 spacing, allow 8*4 to indent
            g.stdtrigger.Size = new Size(200, 24);
            g.stdtrigger.Items.Add("Custom");
            g.stdtrigger.Items.AddRange(Profiles.StandardTriggers.Select((p1) => p1.Name));
            g.stdtrigger.SelectedIndex = Profiles.FindTriggerIndex(condition) + 1;
            g.stdtrigger.SelectedIndexChanged += Stdtrigger_SelectedIndexChanged;
            g.stdtrigger.Tag = g;
            g.panel.Controls.Add(g.stdtrigger);

            g.editbutton = new ExtendedControls.ButtonExt();
            g.editbutton.Location = new Point(420, textheightmargin);
            g.editbutton.Size = new Size(60, 24);
            g.editbutton.Text = "Edit";
            g.editbutton.Tag = g;
            g.editbutton.Click += Editbutton_Click;
            g.panel.Controls.Add(g.editbutton);

            g.deletebutton = new ExtendedControls.ButtonExt();
            g.deletebutton.Location = new Point(500, textheightmargin);
            g.deletebutton.Size = new Size(24, 24);
            g.deletebutton.Text = "X";
            g.deletebutton.Tag = g;
            g.deletebutton.Click += Deletebutton_Click;
            g.panel.Controls.Add(g.deletebutton);

            g.condition = new ConditionLists(condition);        // copy so we can edit

            EDDiscovery.EDDTheme.Instance.ApplyToControls(g.panel, label_index.Font);

            panelVScrollMain.Controls.Add(g.panel);
            groups.Add(g);
        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        bool disabletriggers = false;

        private void Editbutton_Click(object sender, EventArgs e)
        {
            Group g = ((Control)sender).Tag as Group;
            ConditionFilterForm frm = new ConditionFilterForm();
            frm.InitCondition("Edit Profile " + g.name.Text, this.Icon, new List<string>(), g.condition );
            if ( frm.ShowDialog() == DialogResult.OK )
            {
                g.condition = frm.result;
                disabletriggers = true;
                g.stdtrigger.SelectedIndex = Profiles.FindTriggerIndex(g.condition) + 1;
                disabletriggers = false;
            }
        }

        private void Stdtrigger_SelectedIndexChanged(object sender, EventArgs e)
        {
            Group g = ((Control)sender).Tag as Group;
            ExtendedControls.ComboBoxCustom c = sender as ExtendedControls.ComboBoxCustom;

            if ( !disabletriggers && c.SelectedIndex>=1)
            {
                g.condition = new ConditionLists(Profiles.StandardTriggers[c.SelectedIndex - 1].Condition);
            }
        }

        private void PositionPanels()
        {
            int vpos = 4;
            int panelwidth = Math.Max(panelVScrollMain.Width - panelVScrollMain.ScrollBarWidth, 10) - panelleftmargin * 2;

            foreach ( Group g in groups )
            {
                g.panel.Location = new Point(panelleftmargin, vpos);
                g.panel.Size = new Size(panelwidth, 32);
                vpos += g.panel.Height + 4;
            }

            buttonMore.Location = new Point(panelleftmargin, vpos);
        }

        private void buttonMore_Click(object sender, EventArgs e)
        {
            int pno = 1;
            while (groups.FindIndex(x => x.name.Text.Equals("Profile " + pno.ToStringInvariant())) != -1)
                pno++;

            string pname = "Profile " + pno.ToStringInvariant();

            SuspendLayout();
            AddProfile(pname, new ConditionLists( Profiles.StandardTriggers[Profiles.NoTriggerIndex].Condition ));
            PositionPanels();
            ResumeLayout();
        }

        private void buttonExtCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label_index_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void label_index_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
