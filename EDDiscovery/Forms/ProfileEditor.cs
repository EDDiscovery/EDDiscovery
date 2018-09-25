﻿using Conditions;
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
        public List<EDDProfiles.Profile> Result;            // pass back result
        public int PowerOnIndex = -1;

        class Group
        {
            public Panel panel;
            public ExtendedControls.RichTextBoxScroll name;
            public ExtendedControls.ComboBoxCustom stdtrigger;
            public ExtendedControls.ButtonExt edittriggerbutton;
            public ExtendedControls.ButtonExt editbacktriggerbutton;
            public ExtendedControls.ButtonExt deletebutton;
            public ExtendedControls.CheckBoxCustom chkbox;
            public ConditionLists triggercondition;
            public ConditionLists backcondition;
            public int Id;

            public void Dispose()
            {
                name.Dispose();
                stdtrigger.Dispose();
                edittriggerbutton.Dispose();
                editbacktriggerbutton.Dispose();
                deletebutton.Dispose();
                chkbox.Dispose();
            }
        }

        List<Group> groups = new List<Group>();

        EDDProfiles profiles;

        public ProfileEditor()
        {
            InitializeComponent();
        }

        public void Init(EDDProfiles pr, Icon ic)
        {
            profiles = pr;
            Icon = ic;

            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            bool winborder = theme.ApplyToFormStandardFontSize(this);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;

            BaseUtils.Translator.Instance.Translate(this);

            SuspendLayout();
            foreach (EDDProfiles.Profile p in profiles.ProfileList)
            {
                AddProfile(p.Id, p.Name, p.TripCondition, p.BackCondition , p == pr.PowerOn );
            }

            PositionPanels();

            ResumeLayout();
        }

        int textheightmargin = 3;
        int panelleftmargin = 3;

        private void AddProfile(int id, string name, ConditionLists tripcondition , ConditionLists backcondition , bool poweron)
        {
            Group g = new Group();

            g.Id = id;

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
            g.stdtrigger.Items.Add("Custom".Tx(this));
            g.stdtrigger.Items.AddRange(EDDProfiles.StandardTriggers.Select((p1) => p1.Name));
            g.stdtrigger.SelectedIndex = EDDProfiles.FindTriggerIndex(tripcondition,backcondition) + 1;
            g.stdtrigger.SelectedIndexChanged += Stdtrigger_SelectedIndexChanged;
            g.stdtrigger.Tag = g;
            g.panel.Controls.Add(g.stdtrigger);

            g.edittriggerbutton = new ExtendedControls.ButtonExt();
            g.edittriggerbutton.Location = new Point(420, textheightmargin);
            g.edittriggerbutton.Size = new Size(100, 24);
            g.edittriggerbutton.Text = "Trigger".Tx(this);
            g.edittriggerbutton.Tag = g;
            g.edittriggerbutton.Click += EditTrigger_Click;
            g.panel.Controls.Add(g.edittriggerbutton);

            g.editbacktriggerbutton = new ExtendedControls.ButtonExt();
            g.editbacktriggerbutton.Location = new Point(530, textheightmargin);
            g.editbacktriggerbutton.Size = new Size(100, 24);
            g.editbacktriggerbutton.Text = "Back".Tx(this);
            g.editbacktriggerbutton.Tag = g;
            g.editbacktriggerbutton.Click += EditBack_Click;
            g.panel.Controls.Add(g.editbacktriggerbutton);

            g.chkbox = new ExtendedControls.CheckBoxCustom();
            g.chkbox.Location = new Point(640, textheightmargin);
            g.chkbox.Size = new Size(150, 24);
            g.chkbox.Text = "Default".Tx(this);
            g.chkbox.Tag = g;
            g.chkbox.Checked = poweron;
            g.chkbox.Click += Chkbox_Click;
            g.panel.Controls.Add(g.chkbox);

            g.deletebutton = new ExtendedControls.ButtonExt();
            g.deletebutton.Location = new Point(panelVScrollMain.Width-60, textheightmargin);
            g.deletebutton.Size = new Size(24, 24);
            g.deletebutton.Text = "X";
            g.deletebutton.Tag = g;
            g.deletebutton.Click += Deletebutton_Click;
            g.panel.Controls.Add(g.deletebutton);

            g.triggercondition = new ConditionLists(tripcondition);        // copy so we can edit
            g.backcondition = new ConditionLists(backcondition);

            EDDiscovery.EDDTheme.Instance.ApplyToControls(g.panel, label_index.Font);

            panelVScrollMain.Controls.Add(g.panel);
            groups.Add(g);
        }

        private void PositionPanels()
        {
            int vpos = 4;
            int panelwidth = Math.Max(panelVScrollMain.Width - panelVScrollMain.ScrollBarWidth, 10) - panelleftmargin * 2;

            foreach (Group g in groups)
            {
                g.panel.Location = new Point(panelleftmargin, vpos);
                g.panel.Size = new Size(panelwidth, 32);
                g.deletebutton.Visible = g.Id != EDDProfiles.DefaultId;
                vpos += g.panel.Height + 4;
            }

            buttonMore.Location = new Point(panelleftmargin, vpos);
        }


        bool disabletriggers = false;

        private void EditTrigger_Click(object sender, EventArgs e)
        {
            Group g = ((Control)sender).Tag as Group;
            ExtendedConditionsForms.ConditionFilterForm frm = new ExtendedConditionsForms.ConditionFilterForm();
            frm.InitCondition(string.Format("Edit Profile {0} Trigger".Tx(this,"TrigEdit"), g.name.Text), this.Icon, new List<string>(), g.triggercondition);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                g.triggercondition = frm.result;
                disabletriggers = true;
                g.stdtrigger.SelectedIndex = EDDProfiles.FindTriggerIndex(g.triggercondition, g.backcondition) + 1;
                disabletriggers = false;
            }
        }
        private void EditBack_Click(object sender, EventArgs e)
        {
            Group g = ((Control)sender).Tag as Group;
            ExtendedConditionsForms.ConditionFilterForm frm = new ExtendedConditionsForms.ConditionFilterForm();
            frm.InitCondition(string.Format("Edit Profile {0} Back Trigger".Tx(this,"BackEdit"), g.name.Text), this.Icon, new List<string>(), g.backcondition);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                g.backcondition = frm.result;
                disabletriggers = true;
                g.stdtrigger.SelectedIndex = EDDProfiles.FindTriggerIndex(g.triggercondition, g.backcondition) + 1;
                disabletriggers = false;
            }
        }

        private void Stdtrigger_SelectedIndexChanged(object sender, EventArgs e)
        {
            Group g = ((Control)sender).Tag as Group;
            ExtendedControls.ComboBoxCustom c = sender as ExtendedControls.ComboBoxCustom;

            if ( !disabletriggers && c.SelectedIndex>=1)
            {
                g.triggercondition = new ConditionLists(EDDProfiles.StandardTriggers[c.SelectedIndex - 1].TripCondition);
                g.backcondition = new ConditionLists(EDDProfiles.StandardTriggers[c.SelectedIndex - 1].BackCondition);
            }
        }

        private void buttonMore_Click(object sender, EventArgs e)
        {
            int pno = 1;
            while (groups.FindIndex(x => x.name.Text.Equals("Profile " + pno.ToStringInvariant())) != -1)
                pno++;

            string pname = "Profile " + pno.ToStringInvariant();

            SuspendLayout();        // add with an unknown ID as its new
            AddProfile(-1, pname, new ConditionLists(EDDProfiles.StandardTriggers[EDDProfiles.NoTriggerIndex].TripCondition),
                new ConditionLists(EDDProfiles.StandardTriggers[EDDProfiles.NoTriggerIndex].BackCondition) , false );
            PositionPanels();
            ResumeLayout();
        }

        private void buttonExtCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Result = new List<EDDProfiles.Profile>();
            foreach (Group g in groups)
            {
                Result.Add(new EDDProfiles.Profile(g.Id, g.name.Text, g.triggercondition.ToString(), g.backcondition.ToString()));
                if (g.chkbox.Checked)
                    PowerOnIndex = groups.IndexOf(g);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {
            Group g = ((Control)sender).Tag as Group;

            if (ExtendedControls.MessageBoxTheme.Show(this,
                        string.Format(("Do you wish to delete profile {0}?" + Environment.NewLine + "This will remove all the profile information and" + Environment.NewLine + "is not reversible!").Tx(this, "DeleteWarning"), g.name.Text), "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {

                SuspendLayout();
                panelVScrollMain.Controls.Remove(g.panel);
                g.Dispose();
                groups.Remove(g);
                PositionPanels();
                ResumeLayout();

                int pindex = groups.FindIndex(x => x.chkbox.Checked == true);
                if (pindex == -1)
                {
                    disablechk = true;
                    groups[0].chkbox.Checked = true;
                    disablechk = false;
                }
            }
        }

        bool disablechk = false;
        private void Chkbox_Click(object sender, EventArgs e)
        {
            if (!disablechk)
            {
                ExtendedControls.CheckBoxCustom c = sender as ExtendedControls.CheckBoxCustom;
                disablechk = true;
                foreach (Group g in groups)
                    g.chkbox.Checked = false;
                c.Checked = true;
                disablechk = false;
            }
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
