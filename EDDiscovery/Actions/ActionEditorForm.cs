using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public partial class ActionEditorForm : Form
    {
        ActionFile actionfile;
        List<Tuple<string, string>> events;
        List<string> grouptypenames;

        const int panelxmargin = 3;
        const int panelymargin = 1;
        const int conditionhoff = 26;

        class Group
        {
            public Panel panel;
            public ExtendedControls.ComboBoxCustom grouptype;
        }

        List<Group> groups;

        public ActionEditorForm()
        {
            InitializeComponent();
        }

        public void Init(ActionFile file, List<Tuple<string, string>> ev, List<string> g )
        {
            actionfile = file;
            events = ev;
            grouptypenames = g;
            LoadConditions();
        }

        private void LoadConditions()
        {
            ConditionLists c = actionfile.actionfieldfilter;
            groups = new List<Group>();

            for (int i = 0; i < c.Count; i++)
            {
                Condition cd = c.Get(i);
                Group g = CreateGroup(cd);
                groups.Add(g);
            }

            PositionGroups();
        }

        private Group CreateGroup(Condition cd)
        {
            Group g = new Group();

            g.panel = new Panel();
            g.panel.SuspendLayout();

            g.grouptype = new ExtendedControls.ComboBoxCustom();
            g.grouptype.Items.AddRange(grouptypenames);
            g.grouptype.Location = new Point(panelxmargin, panelymargin);
            g.grouptype.Size = new Size(150, 24);
            g.grouptype.DropDownHeight = 400;
            if (cd != null)
                g.grouptype.SelectedItem = GetGroupName(cd.action);

            //g.grouptype.SelectedIndexChanged += Evlist_SelectedIndexChanged;
            g.grouptype.Tag = g;
            g.panel.Controls.Add(g.grouptype);
            return g;
        }

        private void PositionGroups()
        {
            int y = panelymargin;
            int panelwidth = Math.Max(panelVScroll.Width - panelVScroll.ScrollBarWidth, 10);

            for (int i = 0; i < groups.Count; i++)
            {
                Group g = groups[i];

                int farx = 100;

                g.panel.Location = new Point(panelxmargin, y + panelVScroll.ScrollOffset);
                g.panel.Size = new Size(Math.Max(panelwidth - panelxmargin * 2, farx), panelymargin + conditionhoff);

                y += g.panel.Height + 2;
            }
        }

        private string GetGroupName(string a)
        {
            string name = events.Find(x => x.Item1 == a).Item2;
            if (name == null)
                name = "Unknown";
            return name;
        }
    }
}
