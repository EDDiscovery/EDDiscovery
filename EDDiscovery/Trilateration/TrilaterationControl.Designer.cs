namespace EDDiscovery
{
    partial class TrilaterationControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrilaterationControl));
            this.dataGridViewDistances = new System.Windows.Forms.DataGridView();
            this.ColumnSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCalculated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxSystemName = new System.Windows.Forms.TextBox();
            this.labelTargetSystem = new System.Windows.Forms.Label();
            this.labelCoordinates = new System.Windows.Forms.Label();
            this.textBoxCoordinateX = new System.Windows.Forms.TextBox();
            this.labelCoordinateX = new System.Windows.Forms.Label();
            this.labelCoordinateY = new System.Windows.Forms.Label();
            this.textBoxCoordinateY = new System.Windows.Forms.TextBox();
            this.labelCoordinateZ = new System.Windows.Forms.Label();
            this.textBoxCoordinateZ = new System.Windows.Forms.TextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.panelImplementation = new System.Windows.Forms.Panel();
            this.labelAlgorithm = new System.Windows.Forms.Label();
            this.radioButtonAlgorithmJs = new System.Windows.Forms.RadioButton();
            this.radioButtonAlgorithmCsharp = new System.Windows.Forms.RadioButton();
            this.toolTipAlgorithm = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridViewSuggestedSystems = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewClosestSystems = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumnClosestSystemsSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnClosestSystemsDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.richTextBox_History = new System.Windows.Forms.RichTextBox();
            this.labelSuggestedSystems = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSubmitDistances = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRemoveUnused = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemoveAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMap = new System.Windows.Forms.ToolStripButton();
            this.panel_controls = new System.Windows.Forms.Panel();
            this.labelstpos = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDistances)).BeginInit();
            this.panelImplementation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSuggestedSystems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClosestSystems)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panel_controls.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewDistances
            // 
            this.dataGridViewDistances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDistances.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSystem,
            this.ColumnDistance,
            this.ColumnCalculated,
            this.ColumnStatus});
            this.dataGridViewDistances.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridViewDistances.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewDistances.Name = "dataGridViewDistances";
            this.dataGridViewDistances.Size = new System.Drawing.Size(538, 354);
            this.dataGridViewDistances.TabIndex = 0;
            this.dataGridViewDistances.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDistances_CellClick);
            this.dataGridViewDistances.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDistances_CellEndEdit);
            this.dataGridViewDistances.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDistances_CellLeave);
            this.dataGridViewDistances.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewDistances_CellValidating);
            this.dataGridViewDistances.CurrentCellChanged += new System.EventHandler(this.dataGridViewDistances_CurrentCellChanged);
            this.dataGridViewDistances.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewDistances_EditingControlShowing);
            this.dataGridViewDistances.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewDistances_KeyDown);
            // 
            // ColumnSystem
            // 
            this.ColumnSystem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnSystem.FillWeight = 250F;
            this.ColumnSystem.HeaderText = "System";
            this.ColumnSystem.MinimumWidth = 75;
            this.ColumnSystem.Name = "ColumnSystem";
            // 
            // ColumnDistance
            // 
            this.ColumnDistance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnDistance.HeaderText = "Distance";
            this.ColumnDistance.MinimumWidth = 75;
            this.ColumnDistance.Name = "ColumnDistance";
            // 
            // ColumnCalculated
            // 
            this.ColumnCalculated.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnCalculated.HeaderText = "Calculated";
            this.ColumnCalculated.MinimumWidth = 75;
            this.ColumnCalculated.Name = "ColumnCalculated";
            this.ColumnCalculated.ReadOnly = true;
            // 
            // ColumnStatus
            // 
            this.ColumnStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnStatus.HeaderText = "Status";
            this.ColumnStatus.MinimumWidth = 75;
            this.ColumnStatus.Name = "ColumnStatus";
            this.ColumnStatus.ReadOnly = true;
            // 
            // textBoxSystemName
            // 
            this.textBoxSystemName.Location = new System.Drawing.Point(1, 41);
            this.textBoxSystemName.Name = "textBoxSystemName";
            this.textBoxSystemName.ReadOnly = true;
            this.textBoxSystemName.Size = new System.Drawing.Size(178, 20);
            this.textBoxSystemName.TabIndex = 1;
            // 
            // labelTargetSystem
            // 
            this.labelTargetSystem.AutoSize = true;
            this.labelTargetSystem.Location = new System.Drawing.Point(1, 25);
            this.labelTargetSystem.Name = "labelTargetSystem";
            this.labelTargetSystem.Size = new System.Drawing.Size(44, 13);
            this.labelTargetSystem.TabIndex = 2;
            this.labelTargetSystem.Text = "System:";
            // 
            // labelCoordinates
            // 
            this.labelCoordinates.AutoSize = true;
            this.labelCoordinates.Location = new System.Drawing.Point(199, 25);
            this.labelCoordinates.Name = "labelCoordinates";
            this.labelCoordinates.Size = new System.Drawing.Size(122, 13);
            this.labelCoordinates.TabIndex = 5;
            this.labelCoordinates.Text = "Trilaterated Coordinates:";
            // 
            // textBoxCoordinateX
            // 
            this.textBoxCoordinateX.Location = new System.Drawing.Point(200, 41);
            this.textBoxCoordinateX.Name = "textBoxCoordinateX";
            this.textBoxCoordinateX.ReadOnly = true;
            this.textBoxCoordinateX.Size = new System.Drawing.Size(50, 20);
            this.textBoxCoordinateX.TabIndex = 6;
            this.textBoxCoordinateX.Text = "?";
            this.textBoxCoordinateX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelCoordinateX
            // 
            this.labelCoordinateX.AutoSize = true;
            this.labelCoordinateX.Location = new System.Drawing.Point(185, 44);
            this.labelCoordinateX.Name = "labelCoordinateX";
            this.labelCoordinateX.Size = new System.Drawing.Size(17, 13);
            this.labelCoordinateX.TabIndex = 7;
            this.labelCoordinateX.Text = "X:";
            // 
            // labelCoordinateY
            // 
            this.labelCoordinateY.AutoSize = true;
            this.labelCoordinateY.Location = new System.Drawing.Point(252, 44);
            this.labelCoordinateY.Name = "labelCoordinateY";
            this.labelCoordinateY.Size = new System.Drawing.Size(17, 13);
            this.labelCoordinateY.TabIndex = 9;
            this.labelCoordinateY.Text = "Y:";
            // 
            // textBoxCoordinateY
            // 
            this.textBoxCoordinateY.Location = new System.Drawing.Point(269, 41);
            this.textBoxCoordinateY.Name = "textBoxCoordinateY";
            this.textBoxCoordinateY.ReadOnly = true;
            this.textBoxCoordinateY.Size = new System.Drawing.Size(50, 20);
            this.textBoxCoordinateY.TabIndex = 8;
            this.textBoxCoordinateY.Text = "?";
            this.textBoxCoordinateY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelCoordinateZ
            // 
            this.labelCoordinateZ.AutoSize = true;
            this.labelCoordinateZ.Location = new System.Drawing.Point(319, 44);
            this.labelCoordinateZ.Name = "labelCoordinateZ";
            this.labelCoordinateZ.Size = new System.Drawing.Size(17, 13);
            this.labelCoordinateZ.TabIndex = 11;
            this.labelCoordinateZ.Text = "Z:";
            // 
            // textBoxCoordinateZ
            // 
            this.textBoxCoordinateZ.Location = new System.Drawing.Point(336, 41);
            this.textBoxCoordinateZ.Name = "textBoxCoordinateZ";
            this.textBoxCoordinateZ.ReadOnly = true;
            this.textBoxCoordinateZ.Size = new System.Drawing.Size(50, 20);
            this.textBoxCoordinateZ.TabIndex = 10;
            this.textBoxCoordinateZ.Text = "?";
            this.textBoxCoordinateZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelStatus
            // 
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelStatus.Location = new System.Drawing.Point(122, 3);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(160, 19);
            this.labelStatus.TabIndex = 12;
            this.labelStatus.Text = "Awaiting Start";
            // 
            // panelImplementation
            // 
            this.panelImplementation.Controls.Add(this.labelAlgorithm);
            this.panelImplementation.Controls.Add(this.radioButtonAlgorithmJs);
            this.panelImplementation.Controls.Add(this.radioButtonAlgorithmCsharp);
            this.panelImplementation.Location = new System.Drawing.Point(411, 0);
            this.panelImplementation.Name = "panelImplementation";
            this.panelImplementation.Size = new System.Drawing.Size(49, 63);
            this.panelImplementation.TabIndex = 16;
            this.panelImplementation.Visible = false;
            // 
            // labelAlgorithm
            // 
            this.labelAlgorithm.AutoSize = true;
            this.labelAlgorithm.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelAlgorithm.Location = new System.Drawing.Point(0, 12);
            this.labelAlgorithm.Name = "labelAlgorithm";
            this.labelAlgorithm.Size = new System.Drawing.Size(48, 12);
            this.labelAlgorithm.TabIndex = 2;
            this.labelAlgorithm.Text = "Algorithm:";
            // 
            // radioButtonAlgorithmJs
            // 
            this.radioButtonAlgorithmJs.AutoSize = true;
            this.radioButtonAlgorithmJs.Location = new System.Drawing.Point(3, 27);
            this.radioButtonAlgorithmJs.Name = "radioButtonAlgorithmJs";
            this.radioButtonAlgorithmJs.Size = new System.Drawing.Size(37, 17);
            this.radioButtonAlgorithmJs.TabIndex = 1;
            this.radioButtonAlgorithmJs.Text = "JS";
            this.toolTipAlgorithm.SetToolTip(this.radioButtonAlgorithmJs, "Original algoritthm from ed-systems, written in Javascript (slower)");
            this.radioButtonAlgorithmJs.UseVisualStyleBackColor = true;
            this.radioButtonAlgorithmJs.CheckedChanged += new System.EventHandler(this.radioButtonAlgorithm_CheckedChanged);
            // 
            // radioButtonAlgorithmCsharp
            // 
            this.radioButtonAlgorithmCsharp.AutoSize = true;
            this.radioButtonAlgorithmCsharp.Checked = true;
            this.radioButtonAlgorithmCsharp.Location = new System.Drawing.Point(3, 44);
            this.radioButtonAlgorithmCsharp.Name = "radioButtonAlgorithmCsharp";
            this.radioButtonAlgorithmCsharp.Size = new System.Drawing.Size(39, 17);
            this.radioButtonAlgorithmCsharp.TabIndex = 0;
            this.radioButtonAlgorithmCsharp.TabStop = true;
            this.radioButtonAlgorithmCsharp.Text = "C#";
            this.toolTipAlgorithm.SetToolTip(this.radioButtonAlgorithmCsharp, "Algorithm from ed-systems rewritten to C# (fast, experimental)");
            this.radioButtonAlgorithmCsharp.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 99);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewSuggestedSystems);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewDistances);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewClosestSystems);
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox_History);
            this.splitContainer1.Size = new System.Drawing.Size(924, 530);
            this.splitContainer1.SplitterDistance = 354;
            this.splitContainer1.TabIndex = 20;
            // 
            // dataGridViewSuggestedSystems
            // 
            this.dataGridViewSuggestedSystems.AllowUserToAddRows = false;
            this.dataGridViewSuggestedSystems.AllowUserToDeleteRows = false;
            this.dataGridViewSuggestedSystems.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSuggestedSystems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewSuggestedSystems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSuggestedSystems.ColumnHeadersVisible = false;
            this.dataGridViewSuggestedSystems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem});
            this.dataGridViewSuggestedSystems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSuggestedSystems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewSuggestedSystems.Location = new System.Drawing.Point(538, 0);
            this.dataGridViewSuggestedSystems.Name = "dataGridViewSuggestedSystems";
            this.dataGridViewSuggestedSystems.ReadOnly = true;
            this.dataGridViewSuggestedSystems.RowHeadersVisible = false;
            this.dataGridViewSuggestedSystems.Size = new System.Drawing.Size(386, 354);
            this.dataGridViewSuggestedSystems.TabIndex = 18;
            this.dataGridViewSuggestedSystems.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSuggestedSystems_CellClick);
            // 
            // dataGridViewTextBoxColumnSuggestedSystemsSystem
            // 
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem.HeaderText = "System";
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem.MinimumWidth = 100;
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem.Name = "dataGridViewTextBoxColumnSuggestedSystemsSystem";
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem.ReadOnly = true;
            // 
            // dataGridViewClosestSystems
            // 
            this.dataGridViewClosestSystems.AllowUserToAddRows = false;
            this.dataGridViewClosestSystems.AllowUserToDeleteRows = false;
            this.dataGridViewClosestSystems.AllowUserToResizeRows = false;
            this.dataGridViewClosestSystems.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewClosestSystems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewClosestSystems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewClosestSystems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumnClosestSystemsSystem,
            this.dataGridViewTextBoxColumnClosestSystemsDistance});
            this.dataGridViewClosestSystems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewClosestSystems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewClosestSystems.Location = new System.Drawing.Point(538, 0);
            this.dataGridViewClosestSystems.Name = "dataGridViewClosestSystems";
            this.dataGridViewClosestSystems.ReadOnly = true;
            this.dataGridViewClosestSystems.RowHeadersVisible = false;
            this.dataGridViewClosestSystems.Size = new System.Drawing.Size(386, 172);
            this.dataGridViewClosestSystems.TabIndex = 13;
            this.dataGridViewClosestSystems.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewClosestSystems_CellMouseClick);
            // 
            // dataGridViewTextBoxColumnClosestSystemsSystem
            // 
            this.dataGridViewTextBoxColumnClosestSystemsSystem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnClosestSystemsSystem.HeaderText = "Wanted System";
            this.dataGridViewTextBoxColumnClosestSystemsSystem.MinimumWidth = 100;
            this.dataGridViewTextBoxColumnClosestSystemsSystem.Name = "dataGridViewTextBoxColumnClosestSystemsSystem";
            this.dataGridViewTextBoxColumnClosestSystemsSystem.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumnClosestSystemsDistance
            // 
            this.dataGridViewTextBoxColumnClosestSystemsDistance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumnClosestSystemsDistance.HeaderText = "Distance";
            this.dataGridViewTextBoxColumnClosestSystemsDistance.MinimumWidth = 80;
            this.dataGridViewTextBoxColumnClosestSystemsDistance.Name = "dataGridViewTextBoxColumnClosestSystemsDistance";
            this.dataGridViewTextBoxColumnClosestSystemsDistance.ReadOnly = true;
            this.dataGridViewTextBoxColumnClosestSystemsDistance.Width = 80;
            // 
            // richTextBox_History
            // 
            this.richTextBox_History.Dock = System.Windows.Forms.DockStyle.Left;
            this.richTextBox_History.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_History.Name = "richTextBox_History";
            this.richTextBox_History.Size = new System.Drawing.Size(538, 172);
            this.richTextBox_History.TabIndex = 6;
            this.richTextBox_History.Text = "";
            // 
            // labelSuggestedSystems
            // 
            this.labelSuggestedSystems.AutoSize = true;
            this.labelSuggestedSystems.Location = new System.Drawing.Point(535, 50);
            this.labelSuggestedSystems.Name = "labelSuggestedSystems";
            this.labelSuggestedSystems.Size = new System.Drawing.Size(146, 13);
            this.labelSuggestedSystems.TabIndex = 19;
            this.labelSuggestedSystems.Text = "Suggested reference systems";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSubmitDistances,
            this.toolStripButtonNew,
            this.toolStripSeparator1,
            this.toolStripButtonRemoveUnused,
            this.toolStripButtonRemoveAll,
            this.toolStripButtonMap});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(924, 25);
            this.toolStrip1.TabIndex = 23;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonSubmitDistances
            // 
            this.toolStripButtonSubmitDistances.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSubmitDistances.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSubmitDistances.Image")));
            this.toolStripButtonSubmitDistances.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSubmitDistances.Name = "toolStripButtonSubmitDistances";
            this.toolStripButtonSubmitDistances.Size = new System.Drawing.Size(118, 22);
            this.toolStripButtonSubmitDistances.Text = "Submit Distances";
            this.toolStripButtonSubmitDistances.Click += new System.EventHandler(this.toolStripButtonSubmitDistances_Click);
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNew.Image")));
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(76, 22);
            this.toolStripButtonNew.Text = "Start new";
            this.toolStripButtonNew.ToolTipText = "Calculate coordinates for current system";
            this.toolStripButtonNew.Click += new System.EventHandler(this.toolStripButtonNew_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonRemoveUnused
            // 
            this.toolStripButtonRemoveUnused.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveUnused.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRemoveUnused.Image")));
            this.toolStripButtonRemoveUnused.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveUnused.Name = "toolStripButtonRemoveUnused";
            this.toolStripButtonRemoveUnused.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRemoveUnused.Text = "toolStripButtonRemoveUnused";
            this.toolStripButtonRemoveUnused.ToolTipText = "Remove unused";
            this.toolStripButtonRemoveUnused.Click += new System.EventHandler(this.toolStripButtonRemoveUnused_Click);
            // 
            // toolStripButtonRemoveAll
            // 
            this.toolStripButtonRemoveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRemoveAll.Image")));
            this.toolStripButtonRemoveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveAll.Name = "toolStripButtonRemoveAll";
            this.toolStripButtonRemoveAll.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRemoveAll.Text = "toolStripButton1";
            this.toolStripButtonRemoveAll.ToolTipText = "Remove all";
            this.toolStripButtonRemoveAll.Click += new System.EventHandler(this.toolStripButtonRemoveAll_Click);
            // 
            // toolStripButtonMap
            // 
            this.toolStripButtonMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMap.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMap.Image")));
            this.toolStripButtonMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMap.Name = "toolStripButtonMap";
            this.toolStripButtonMap.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonMap.Text = "3d map";
            this.toolStripButtonMap.ToolTipText = "Show 3d map";
            this.toolStripButtonMap.Click += new System.EventHandler(this.toolStripButtonMap_Click);
            // 
            // panel_controls
            // 
            this.panel_controls.Controls.Add(this.labelstpos);
            this.panel_controls.Controls.Add(this.labelSuggestedSystems);
            this.panel_controls.Controls.Add(this.textBoxSystemName);
            this.panel_controls.Controls.Add(this.labelTargetSystem);
            this.panel_controls.Controls.Add(this.labelCoordinates);
            this.panel_controls.Controls.Add(this.textBoxCoordinateX);
            this.panel_controls.Controls.Add(this.labelCoordinateX);
            this.panel_controls.Controls.Add(this.textBoxCoordinateY);
            this.panel_controls.Controls.Add(this.labelCoordinateY);
            this.panel_controls.Controls.Add(this.textBoxCoordinateZ);
            this.panel_controls.Controls.Add(this.labelCoordinateZ);
            this.panel_controls.Controls.Add(this.labelStatus);
            this.panel_controls.Controls.Add(this.panelImplementation);
            this.panel_controls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_controls.Location = new System.Drawing.Point(0, 25);
            this.panel_controls.Name = "panel_controls";
            this.panel_controls.Size = new System.Drawing.Size(924, 74);
            this.panel_controls.TabIndex = 24;
            // 
            // labelstpos
            // 
            this.labelstpos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelstpos.Location = new System.Drawing.Point(3, 3);
            this.labelstpos.Name = "labelstpos";
            this.labelstpos.Size = new System.Drawing.Size(90, 19);
            this.labelstpos.TabIndex = 20;
            this.labelstpos.Text = "Current Status:";
            // 
            // TrilaterationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel_controls);
            this.Controls.Add(this.toolStrip1);
            this.Name = "TrilaterationControl";
            this.Size = new System.Drawing.Size(924, 681);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDistances)).EndInit();
            this.panelImplementation.ResumeLayout(false);
            this.panelImplementation.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSuggestedSystems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClosestSystems)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel_controls.ResumeLayout(false);
            this.panel_controls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxSystemName;
        private System.Windows.Forms.Label labelTargetSystem;
        private System.Windows.Forms.Label labelCoordinates;
        private System.Windows.Forms.TextBox textBoxCoordinateX;
        private System.Windows.Forms.Label labelCoordinateX;
        private System.Windows.Forms.Label labelCoordinateY;
        private System.Windows.Forms.TextBox textBoxCoordinateY;
        private System.Windows.Forms.Label labelCoordinateZ;
        private System.Windows.Forms.TextBox textBoxCoordinateZ;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Panel panelImplementation;
        private System.Windows.Forms.RadioButton radioButtonAlgorithmJs;
        private System.Windows.Forms.RadioButton radioButtonAlgorithmCsharp;
        private System.Windows.Forms.Label labelAlgorithm;
        private System.Windows.Forms.ToolTip toolTipAlgorithm;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSubmitDistances;
        private System.Windows.Forms.Label labelSuggestedSystems;
        private System.Windows.Forms.DataGridView dataGridViewSuggestedSystems;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnSuggestedSystemsSystem;
        internal System.Windows.Forms.RichTextBox richTextBox_History;
        private System.Windows.Forms.DataGridView dataGridViewClosestSystems;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        protected System.Windows.Forms.DataGridView dataGridViewDistances;
        private System.Windows.Forms.ToolStripButton toolStripButtonMap;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveUnused;
        private System.Windows.Forms.Panel panel_controls;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnClosestSystemsSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnClosestSystemsDistance;
        private System.Windows.Forms.Label labelstpos;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCalculated;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStatus;
    }
}
