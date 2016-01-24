namespace EDDiscovery2.PlanetSystems
{
    partial class PlanetsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlanetsForm));
            this.textBoxSystemName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddPlanet = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.panelPlanets = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.checkedListBox4 = new System.Windows.Forms.CheckedListBox();
            this.label12 = new System.Windows.Forms.Label();
            this.checkedListBox3 = new System.Windows.Forms.CheckedListBox();
            this.label11 = new System.Windows.Forms.Label();
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.comboBoxTerrainDifficulty = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxRadius = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxArrivalPoint = new System.Windows.Forms.TextBox();
            this.comboBoxVulcanism = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxAtmosphere = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxGravity = new System.Windows.Forms.TextBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.toolStripButtonAddStar = new System.Windows.Forms.ToolStripButton();
            this.panelStar = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnRowType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panelPlanets.SuspendLayout();
            this.panelStar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxSystemName
            // 
            this.textBoxSystemName.CausesValidation = false;
            this.textBoxSystemName.Location = new System.Drawing.Point(69, 12);
            this.textBoxSystemName.Name = "textBoxSystemName";
            this.textBoxSystemName.Size = new System.Drawing.Size(192, 20);
            this.textBoxSystemName.TabIndex = 0;
            this.textBoxSystemName.TextChanged += new System.EventHandler(this.textBoxSystemName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "System";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(278, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Current";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(72, 13);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(103, 20);
            this.textBoxName.TabIndex = 4;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 25);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(923, 203);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Location = new System.Drawing.Point(15, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(923, 228);
            this.panel1.TabIndex = 6;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddPlanet,
            this.toolStripButtonSave,
            this.toolStripButtonAddStar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(923, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonAddPlanet
            // 
            this.toolStripButtonAddPlanet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddPlanet.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddPlanet.Image")));
            this.toolStripButtonAddPlanet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddPlanet.Name = "toolStripButtonAddPlanet";
            this.toolStripButtonAddPlanet.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAddPlanet.Text = "Add planet";
            this.toolStripButtonAddPlanet.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(35, 22);
            this.toolStripButtonSave.Text = "Save";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // panelPlanets
            // 
            this.panelPlanets.Controls.Add(this.panelStar);
            this.panelPlanets.Controls.Add(this.label13);
            this.panelPlanets.Controls.Add(this.checkedListBox4);
            this.panelPlanets.Controls.Add(this.label12);
            this.panelPlanets.Controls.Add(this.checkedListBox3);
            this.panelPlanets.Controls.Add(this.label11);
            this.panelPlanets.Controls.Add(this.checkedListBox2);
            this.panelPlanets.Controls.Add(this.label10);
            this.panelPlanets.Controls.Add(this.checkedListBox1);
            this.panelPlanets.Controls.Add(this.comboBoxTerrainDifficulty);
            this.panelPlanets.Controls.Add(this.label9);
            this.panelPlanets.Controls.Add(this.checkBox1);
            this.panelPlanets.Controls.Add(this.label8);
            this.panelPlanets.Controls.Add(this.textBoxRadius);
            this.panelPlanets.Controls.Add(this.label7);
            this.panelPlanets.Controls.Add(this.textBoxArrivalPoint);
            this.panelPlanets.Controls.Add(this.comboBoxVulcanism);
            this.panelPlanets.Controls.Add(this.label6);
            this.panelPlanets.Controls.Add(this.comboBoxAtmosphere);
            this.panelPlanets.Controls.Add(this.label5);
            this.panelPlanets.Controls.Add(this.label4);
            this.panelPlanets.Controls.Add(this.textBoxGravity);
            this.panelPlanets.Controls.Add(this.comboBoxType);
            this.panelPlanets.Controls.Add(this.label3);
            this.panelPlanets.Controls.Add(this.label2);
            this.panelPlanets.Controls.Add(this.textBoxName);
            this.panelPlanets.Location = new System.Drawing.Point(15, 272);
            this.panelPlanets.Name = "panelPlanets";
            this.panelPlanets.Size = new System.Drawing.Size(923, 144);
            this.panelPlanets.TabIndex = 7;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(791, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "Very rare";
            // 
            // checkedListBox4
            // 
            this.checkedListBox4.BackColor = System.Drawing.Color.Salmon;
            this.checkedListBox4.CheckOnClick = true;
            this.checkedListBox4.FormattingEnabled = true;
            this.checkedListBox4.Items.AddRange(new object[] {
            "Antimony",
            "Polonium",
            "Ruthenium",
            "Technetium",
            "Tellurium",
            "Yttrium"});
            this.checkedListBox4.Location = new System.Drawing.Point(794, 16);
            this.checkedListBox4.Name = "checkedListBox4";
            this.checkedListBox4.Size = new System.Drawing.Size(93, 124);
            this.checkedListBox4.TabIndex = 27;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(692, -1);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(30, 13);
            this.label12.TabIndex = 26;
            this.label12.Text = "Rare";
            // 
            // checkedListBox3
            // 
            this.checkedListBox3.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.checkedListBox3.CheckOnClick = true;
            this.checkedListBox3.FormattingEnabled = true;
            this.checkedListBox3.Items.AddRange(new object[] {
            "Cadmium",
            "Mercury",
            "Molybdenum",
            "Niobium",
            "Tin",
            "Tungsten"});
            this.checkedListBox3.Location = new System.Drawing.Point(695, 15);
            this.checkedListBox3.Name = "checkedListBox3";
            this.checkedListBox3.Size = new System.Drawing.Size(93, 124);
            this.checkedListBox3.TabIndex = 25;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(593, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Common";
            // 
            // checkedListBox2
            // 
            this.checkedListBox2.BackColor = System.Drawing.Color.LightGreen;
            this.checkedListBox2.CheckOnClick = true;
            this.checkedListBox2.FormattingEnabled = true;
            this.checkedListBox2.Items.AddRange(new object[] {
            "Arsenic",
            "Chromium",
            "Germanium",
            "Manganese",
            "Selenium",
            "Vanadium",
            "Zinc",
            "Zirconium"});
            this.checkedListBox2.Location = new System.Drawing.Point(596, 15);
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.Size = new System.Drawing.Size(93, 124);
            this.checkedListBox2.TabIndex = 23;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(494, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Very common";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.Color.LightBlue;
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "Carbon",
            "Iron",
            "Nickel",
            "Phosphorus",
            "Sulphur"});
            this.checkedListBox1.Location = new System.Drawing.Point(497, 16);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(93, 124);
            this.checkedListBox1.TabIndex = 21;
            // 
            // comboBoxTerrainDifficulty
            // 
            this.comboBoxTerrainDifficulty.FormattingEnabled = true;
            this.comboBoxTerrainDifficulty.Location = new System.Drawing.Point(90, 92);
            this.comboBoxTerrainDifficulty.Name = "comboBoxTerrainDifficulty";
            this.comboBoxTerrainDifficulty.Size = new System.Drawing.Size(85, 21);
            this.comboBoxTerrainDifficulty.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Terrain difficulty";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(400, 41);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(91, 17);
            this.checkBox1.TabIndex = 18;
            this.checkBox1.Text = "Terraformable";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(181, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Radius(km)";
            // 
            // textBoxRadius
            // 
            this.textBoxRadius.Location = new System.Drawing.Point(242, 66);
            this.textBoxRadius.Name = "textBoxRadius";
            this.textBoxRadius.Size = new System.Drawing.Size(103, 20);
            this.textBoxRadius.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Arrival point";
            // 
            // textBoxArrivalPoint
            // 
            this.textBoxArrivalPoint.Location = new System.Drawing.Point(72, 66);
            this.textBoxArrivalPoint.Name = "textBoxArrivalPoint";
            this.textBoxArrivalPoint.Size = new System.Drawing.Size(103, 20);
            this.textBoxArrivalPoint.TabIndex = 14;
            // 
            // comboBoxVulcanism
            // 
            this.comboBoxVulcanism.FormattingEnabled = true;
            this.comboBoxVulcanism.Location = new System.Drawing.Point(242, 39);
            this.comboBoxVulcanism.Name = "comboBoxVulcanism";
            this.comboBoxVulcanism.Size = new System.Drawing.Size(107, 21);
            this.comboBoxVulcanism.TabIndex = 13;
            this.comboBoxVulcanism.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(181, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Vulcanism";
            // 
            // comboBoxAtmosphere
            // 
            this.comboBoxAtmosphere.FormattingEnabled = true;
            this.comboBoxAtmosphere.Location = new System.Drawing.Point(72, 39);
            this.comboBoxAtmosphere.Name = "comboBoxAtmosphere";
            this.comboBoxAtmosphere.Size = new System.Drawing.Size(103, 21);
            this.comboBoxAtmosphere.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Atmosphere";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(381, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Gravity";
            // 
            // textBoxGravity
            // 
            this.textBoxGravity.Location = new System.Drawing.Point(422, 12);
            this.textBoxGravity.Name = "textBoxGravity";
            this.textBoxGravity.Size = new System.Drawing.Size(58, 20);
            this.textBoxGravity.TabIndex = 8;
            this.textBoxGravity.TextChanged += new System.EventHandler(this.textBoxGravity_TextChanged);
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(242, 12);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(107, 21);
            this.comboBoxType.TabIndex = 7;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(181, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Name";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(394, 3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(495, 25);
            this.label14.TabIndex = 8;
            this.label14.Text = "Testing only!!  Storing function will come soon(TM)";
            // 
            // toolStripButtonAddStar
            // 
            this.toolStripButtonAddStar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddStar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddStar.Image")));
            this.toolStripButtonAddStar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddStar.Name = "toolStripButtonAddStar";
            this.toolStripButtonAddStar.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAddStar.Text = "Add Star";
            // 
            // panelStar
            // 
            this.panelStar.Controls.Add(this.dataGridView1);
            this.panelStar.Controls.Add(this.label17);
            this.panelStar.Controls.Add(this.textBox7);
            this.panelStar.Controls.Add(this.label18);
            this.panelStar.Controls.Add(this.textBox8);
            this.panelStar.Controls.Add(this.label19);
            this.panelStar.Controls.Add(this.textBox9);
            this.panelStar.Controls.Add(this.label16);
            this.panelStar.Controls.Add(this.textBox6);
            this.panelStar.Controls.Add(this.textBox5);
            this.panelStar.Controls.Add(this.label15);
            this.panelStar.Controls.Add(this.textBox3);
            this.panelStar.Controls.Add(this.label20);
            this.panelStar.Controls.Add(this.textBox1);
            this.panelStar.Controls.Add(this.label21);
            this.panelStar.Controls.Add(this.textBox2);
            this.panelStar.Controls.Add(this.comboBox4);
            this.panelStar.Controls.Add(this.label25);
            this.panelStar.Controls.Add(this.label26);
            this.panelStar.Controls.Add(this.textBox4);
            this.panelStar.Location = new System.Drawing.Point(0, 0);
            this.panelStar.Name = "panelStar";
            this.panelStar.Size = new System.Drawing.Size(923, 144);
            this.panelStar.TabIndex = 9;
            this.panelStar.Paint += new System.Windows.Forms.PaintEventHandler(this.panelStar_Paint);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(573, 98);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(62, 13);
            this.label20.TabIndex = 17;
            this.label20.Text = "Solar radius";
            this.label20.Click += new System.EventHandler(this.label20_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(636, 95);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(107, 20);
            this.textBox1.TabIndex = 16;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(734, 19);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(69, 13);
            this.label21.TabIndex = 15;
            this.label21.Text = "Solar masses";
            this.label21.Click += new System.EventHandler(this.label21_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(803, 16);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(103, 20);
            this.textBox2.TabIndex = 14;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(72, 39);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(107, 21);
            this.comboBox4.TabIndex = 7;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(28, 43);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(31, 13);
            this.label25.TabIndex = 6;
            this.label25.Text = "Type";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(28, 19);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(35, 13);
            this.label26.TabIndex = 5;
            this.label26.Text = "Name";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(72, 13);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(103, 20);
            this.textBox4.TabIndex = 4;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(28, 72);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 13);
            this.label15.TabIndex = 22;
            this.label15.Text = "Class";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(71, 69);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(58, 20);
            this.textBox3.TabIndex = 21;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(135, 69);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(58, 20);
            this.textBox5.TabIndex = 23;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(753, 100);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(70, 13);
            this.label16.TabIndex = 25;
            this.label16.Text = "Surface temp";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(829, 95);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(107, 20);
            this.textBox6.TabIndex = 24;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(752, 127);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(70, 13);
            this.label17.TabIndex = 31;
            this.label17.Text = "Surface temp";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(828, 122);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(107, 20);
            this.textBox7.TabIndex = 30;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(572, 125);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(32, 13);
            this.label18.TabIndex = 29;
            this.label18.Text = "Orbit ";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(635, 122);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(107, 20);
            this.textBox8.TabIndex = 28;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(733, 46);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(61, 13);
            this.label19.TabIndex = 27;
            this.label19.Text = "Orbit period";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(802, 43);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(103, 20);
            this.textBox9.TabIndex = 26;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnRowType,
            this.Column2,
            this.ColumnUnit});
            this.dataGridView1.Location = new System.Drawing.Point(247, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(320, 142);
            this.dataGridView1.TabIndex = 32;
            // 
            // ColumnRowType
            // 
            this.ColumnRowType.HeaderText = "Column1";
            this.ColumnRowType.Name = "ColumnRowType";
            this.ColumnRowType.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            // 
            // ColumnUnit
            // 
            this.ColumnUnit.HeaderText = "Column3";
            this.ColumnUnit.Name = "ColumnUnit";
            // 
            // PlanetsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 419);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.panelPlanets);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSystemName);
            this.Name = "PlanetsForm";
            this.Text = "PlanetsForm";
            this.Load += new System.EventHandler(this.PlanetsForm_Load);
            this.Shown += new System.EventHandler(this.PlanetsForm_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelPlanets.ResumeLayout(false);
            this.panelPlanets.PerformLayout();
            this.panelStar.ResumeLayout(false);
            this.panelStar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSystemName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddPlanet;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.Panel panelPlanets;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxAtmosphere;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxGravity;
        private System.Windows.Forms.ComboBox comboBoxTerrainDifficulty;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxRadius;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxArrivalPoint;
        private System.Windows.Forms.ComboBox comboBoxVulcanism;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckedListBox checkedListBox4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckedListBox checkedListBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckedListBox checkedListBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddStar;
        private System.Windows.Forms.Panel panelStar;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnRowType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnUnit;
    }
}