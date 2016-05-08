using ExtendedControls;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrilaterationControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.trilatContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToWantedSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelTargetSystem = new System.Windows.Forms.Label();
            this.labelCoordinates = new System.Windows.Forms.Label();
            this.labelCoordinateX = new System.Windows.Forms.Label();
            this.labelCoordinateY = new System.Windows.Forms.Label();
            this.labelCoordinateZ = new System.Windows.Forms.Label();
            this.panelImplementation = new System.Windows.Forms.Panel();
            this.labelAlgorithm = new System.Windows.Forms.Label();
            this.radioButtonAlgorithmJs = new ExtendedControls.RadioButtonCustom();
            this.radioButtonAlgorithmCsharp = new ExtendedControls.RadioButtonCustom();
            this.toolTipAlgorithm = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1 = new ExtendedControls.SplitContainerCustom();
            this.splitContainerTop = new ExtendedControls.SplitContainerCustom();
            this.dataViewScrollerPanelDistances = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom3 = new ExtendedControls.VScrollBarCustom();
            this.dataGridViewDistances = new System.Windows.Forms.DataGridView();
            this.ColumnSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCalculated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataViewScrollerPanelSuggestedReferences = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.dataGridViewSuggestedSystems = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainerBottom = new ExtendedControls.SplitContainerCustom();
            this.richTextBox_History = new ExtendedControls.RichTextBoxScroll();
            this.dataViewScrollerPanelWantedSystems = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom2 = new ExtendedControls.VScrollBarCustom();
            this.dataGridViewClosestSystems = new System.Windows.Forms.DataGridView();
            this.Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnClosestSystemsSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wantedContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeFromWantedSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSubmitDistances = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRemoveUnused = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemoveAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMap = new System.Windows.Forms.ToolStripButton();
            this.panel_controls = new System.Windows.Forms.Panel();
            this.labelstpos = new System.Windows.Forms.Label();
            this.textBox_status = new ExtendedControls.TextBoxBorder();
            this.textBoxSystemName = new ExtendedControls.TextBoxBorder();
            this.textBoxCoordinateX = new ExtendedControls.TextBoxBorder();
            this.textBoxCoordinateY = new ExtendedControls.TextBoxBorder();
            this.textBoxCoordinateZ = new ExtendedControls.TextBoxBorder();
            this.trilatContextMenu.SuspendLayout();
            this.panelImplementation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).BeginInit();
            this.splitContainerTop.Panel1.SuspendLayout();
            this.splitContainerTop.Panel2.SuspendLayout();
            this.splitContainerTop.SuspendLayout();
            this.dataViewScrollerPanelDistances.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDistances)).BeginInit();
            this.dataViewScrollerPanelSuggestedReferences.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSuggestedSystems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBottom)).BeginInit();
            this.splitContainerBottom.Panel1.SuspendLayout();
            this.splitContainerBottom.Panel2.SuspendLayout();
            this.splitContainerBottom.SuspendLayout();
            this.dataViewScrollerPanelWantedSystems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClosestSystems)).BeginInit();
            this.wantedContextMenu.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel_controls.SuspendLayout();
            this.SuspendLayout();
            // 
            // trilatContextMenu
            // 
            this.trilatContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToWantedSystemsToolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem});
            this.trilatContextMenu.Name = "trilatContextMenu";
            this.trilatContextMenu.Size = new System.Drawing.Size(198, 48);
            // 
            // addToWantedSystemsToolStripMenuItem
            // 
            this.addToWantedSystemsToolStripMenuItem.Name = "addToWantedSystemsToolStripMenuItem";
            this.addToWantedSystemsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.addToWantedSystemsToolStripMenuItem.Text = "Add to wanted systems";
            this.addToWantedSystemsToolStripMenuItem.Click += new System.EventHandler(this.addToWantedSystemsToolStripMenuItem_Click);
            // 
            // viewOnEDSMToolStripMenuItem
            // 
            this.viewOnEDSMToolStripMenuItem.Name = "viewOnEDSMToolStripMenuItem";
            this.viewOnEDSMToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.viewOnEDSMToolStripMenuItem.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem_Click);
            // 
            // labelTargetSystem
            // 
            this.labelTargetSystem.AutoSize = true;
            this.labelTargetSystem.Location = new System.Drawing.Point(3, 25);
            this.labelTargetSystem.Name = "labelTargetSystem";
            this.labelTargetSystem.Size = new System.Drawing.Size(44, 13);
            this.labelTargetSystem.TabIndex = 2;
            this.labelTargetSystem.Text = "System:";
            // 
            // labelCoordinates
            // 
            this.labelCoordinates.AutoSize = true;
            this.labelCoordinates.Location = new System.Drawing.Point(3, 47);
            this.labelCoordinates.Name = "labelCoordinates";
            this.labelCoordinates.Size = new System.Drawing.Size(122, 13);
            this.labelCoordinates.TabIndex = 5;
            this.labelCoordinates.Text = "Trilaterated Coordinates:";
            // 
            // labelCoordinateX
            // 
            this.labelCoordinateX.AutoSize = true;
            this.labelCoordinateX.Location = new System.Drawing.Point(148, 47);
            this.labelCoordinateX.Name = "labelCoordinateX";
            this.labelCoordinateX.Size = new System.Drawing.Size(17, 13);
            this.labelCoordinateX.TabIndex = 7;
            this.labelCoordinateX.Text = "X:";
            // 
            // labelCoordinateY
            // 
            this.labelCoordinateY.AutoSize = true;
            this.labelCoordinateY.Location = new System.Drawing.Point(228, 47);
            this.labelCoordinateY.Name = "labelCoordinateY";
            this.labelCoordinateY.Size = new System.Drawing.Size(17, 13);
            this.labelCoordinateY.TabIndex = 9;
            this.labelCoordinateY.Text = "Y:";
            // 
            // labelCoordinateZ
            // 
            this.labelCoordinateZ.AutoSize = true;
            this.labelCoordinateZ.Location = new System.Drawing.Point(309, 47);
            this.labelCoordinateZ.Name = "labelCoordinateZ";
            this.labelCoordinateZ.Size = new System.Drawing.Size(17, 13);
            this.labelCoordinateZ.TabIndex = 11;
            this.labelCoordinateZ.Text = "Z:";
            // 
            // panelImplementation
            // 
            this.panelImplementation.Controls.Add(this.labelAlgorithm);
            this.panelImplementation.Controls.Add(this.radioButtonAlgorithmJs);
            this.panelImplementation.Controls.Add(this.radioButtonAlgorithmCsharp);
            this.panelImplementation.Location = new System.Drawing.Point(467, 4);
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
            this.radioButtonAlgorithmJs.FontNerfReduction = 0.5F;
            this.radioButtonAlgorithmJs.Location = new System.Drawing.Point(3, 27);
            this.radioButtonAlgorithmJs.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.radioButtonAlgorithmJs.Name = "radioButtonAlgorithmJs";
            this.radioButtonAlgorithmJs.RadioButtonColor = System.Drawing.Color.Gray;
            this.radioButtonAlgorithmJs.RadioButtonInnerColor = System.Drawing.Color.White;
            this.radioButtonAlgorithmJs.SelectedColor = System.Drawing.Color.DarkBlue;
            this.radioButtonAlgorithmJs.SelectedColorRing = System.Drawing.Color.Black;
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
            this.radioButtonAlgorithmCsharp.FontNerfReduction = 0.5F;
            this.radioButtonAlgorithmCsharp.Location = new System.Drawing.Point(3, 44);
            this.radioButtonAlgorithmCsharp.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.radioButtonAlgorithmCsharp.Name = "radioButtonAlgorithmCsharp";
            this.radioButtonAlgorithmCsharp.RadioButtonColor = System.Drawing.Color.Gray;
            this.radioButtonAlgorithmCsharp.RadioButtonInnerColor = System.Drawing.Color.White;
            this.radioButtonAlgorithmCsharp.SelectedColor = System.Drawing.Color.DarkBlue;
            this.radioButtonAlgorithmCsharp.SelectedColorRing = System.Drawing.Color.Black;
            this.radioButtonAlgorithmCsharp.Size = new System.Drawing.Size(39, 17);
            this.radioButtonAlgorithmCsharp.TabIndex = 0;
            this.radioButtonAlgorithmCsharp.TabStop = true;
            this.radioButtonAlgorithmCsharp.Text = "C#";
            this.toolTipAlgorithm.SetToolTip(this.radioButtonAlgorithmCsharp, "Algorithm from ed-systems rewritten to C# (fast, experimental)");
            this.radioButtonAlgorithmCsharp.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 99);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainerTop);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainerBottom);
            this.splitContainer1.Size = new System.Drawing.Size(924, 582);
            this.splitContainer1.SplitterDistance = 247;
            this.splitContainer1.TabIndex = 20;
            // 
            // splitContainerTop
            // 
            this.splitContainerTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTop.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTop.Name = "splitContainerTop";
            // 
            // splitContainerTop.Panel1
            // 
            this.splitContainerTop.Panel1.Controls.Add(this.dataViewScrollerPanelDistances);
            // 
            // splitContainerTop.Panel2
            // 
            this.splitContainerTop.Panel2.Controls.Add(this.dataViewScrollerPanelSuggestedReferences);
            this.splitContainerTop.Size = new System.Drawing.Size(924, 247);
            this.splitContainerTop.SplitterDistance = 540;
            this.splitContainerTop.TabIndex = 3;
            this.splitContainerTop.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerTop_SplitterMoved);
            // 
            // dataViewScrollerPanelDistances
            // 
            this.dataViewScrollerPanelDistances.Controls.Add(this.vScrollBarCustom3);
            this.dataViewScrollerPanelDistances.Controls.Add(this.dataGridViewDistances);
            this.dataViewScrollerPanelDistances.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanelDistances.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanelDistances.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanelDistances.Name = "dataViewScrollerPanelDistances";
            this.dataViewScrollerPanelDistances.ScrollBarWidth = 20;
            this.dataViewScrollerPanelDistances.Size = new System.Drawing.Size(540, 247);
            this.dataViewScrollerPanelDistances.TabIndex = 22;
            this.dataViewScrollerPanelDistances.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom3
            // 
            this.vScrollBarCustom3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBarCustom3.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom3.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom3.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom3.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom3.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom3.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom3.HideScrollBar = true;
            this.vScrollBarCustom3.LargeChange = 1;
            this.vScrollBarCustom3.Location = new System.Drawing.Point(520, 21);
            this.vScrollBarCustom3.Maximum = 0;
            this.vScrollBarCustom3.Minimum = 0;
            this.vScrollBarCustom3.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom3.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom3.Name = "vScrollBarCustom3";
            this.vScrollBarCustom3.Size = new System.Drawing.Size(20, 226);
            this.vScrollBarCustom3.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom3.SmallChange = 1;
            this.vScrollBarCustom3.TabIndex = 1;
            this.vScrollBarCustom3.Text = "vScrollBarCustom3";
            this.vScrollBarCustom3.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom3.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom3.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom3.ThumbDrawAngle = 0F;
            this.vScrollBarCustom3.Value = 0;
            this.vScrollBarCustom3.ValueLimited = 0;
            // 
            // dataGridViewDistances
            // 
            this.dataGridViewDistances.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDistances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDistances.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSystem,
            this.ColumnDistance,
            this.ColumnCalculated,
            this.ColumnStatus});
            this.dataGridViewDistances.ContextMenuStrip = this.trilatContextMenu;
            this.dataGridViewDistances.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewDistances.Name = "dataGridViewDistances";
            this.dataGridViewDistances.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewDistances.Size = new System.Drawing.Size(520, 247);
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
            // dataViewScrollerPanelSuggestedReferences
            // 
            this.dataViewScrollerPanelSuggestedReferences.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanelSuggestedReferences.Controls.Add(this.dataGridViewSuggestedSystems);
            this.dataViewScrollerPanelSuggestedReferences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanelSuggestedReferences.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanelSuggestedReferences.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanelSuggestedReferences.Name = "dataViewScrollerPanelSuggestedReferences";
            this.dataViewScrollerPanelSuggestedReferences.ScrollBarWidth = 20;
            this.dataViewScrollerPanelSuggestedReferences.Size = new System.Drawing.Size(380, 247);
            this.dataViewScrollerPanelSuggestedReferences.TabIndex = 19;
            this.dataViewScrollerPanelSuggestedReferences.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom1
            // 
            this.vScrollBarCustom1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom1.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom1.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom1.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom1.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom1.HideScrollBar = true;
            this.vScrollBarCustom1.LargeChange = 0;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(360, 21);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 226);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 19;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -1;
            this.vScrollBarCustom1.ValueLimited = -1;
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
            this.dataGridViewSuggestedSystems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem});
            this.dataGridViewSuggestedSystems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewSuggestedSystems.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSuggestedSystems.Name = "dataGridViewSuggestedSystems";
            this.dataGridViewSuggestedSystems.ReadOnly = true;
            this.dataGridViewSuggestedSystems.RowHeadersVisible = false;
            this.dataGridViewSuggestedSystems.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewSuggestedSystems.Size = new System.Drawing.Size(360, 247);
            this.dataGridViewSuggestedSystems.TabIndex = 18;
            this.dataGridViewSuggestedSystems.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSuggestedSystems_CellClick);
            // 
            // dataGridViewTextBoxColumnSuggestedSystemsSystem
            // 
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem.HeaderText = "Suggested Reference Systems";
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem.MinimumWidth = 100;
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem.Name = "dataGridViewTextBoxColumnSuggestedSystemsSystem";
            this.dataGridViewTextBoxColumnSuggestedSystemsSystem.ReadOnly = true;
            // 
            // splitContainerBottom
            // 
            this.splitContainerBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerBottom.Location = new System.Drawing.Point(0, 0);
            this.splitContainerBottom.Name = "splitContainerBottom";
            // 
            // splitContainerBottom.Panel1
            // 
            this.splitContainerBottom.Panel1.Controls.Add(this.richTextBox_History);
            // 
            // splitContainerBottom.Panel2
            // 
            this.splitContainerBottom.Panel2.Controls.Add(this.dataViewScrollerPanelWantedSystems);
            this.splitContainerBottom.Size = new System.Drawing.Size(924, 331);
            this.splitContainerBottom.SplitterDistance = 537;
            this.splitContainerBottom.SplitterWidth = 6;
            this.splitContainerBottom.TabIndex = 2;
            this.splitContainerBottom.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerBottom_SplitterMoved);
            // 
            // richTextBox_History
            // 
            this.richTextBox_History.BorderColor = System.Drawing.Color.Transparent;
            this.richTextBox_History.BorderColorScaling = 0.5F;
            this.richTextBox_History.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_History.HideScrollBar = true;
            this.richTextBox_History.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_History.Name = "richTextBox_History";
            this.richTextBox_History.ScrollBarWidth = 20;
            this.richTextBox_History.ShowLineCount = false;
            this.richTextBox_History.Size = new System.Drawing.Size(537, 331);
            this.richTextBox_History.TabIndex = 6;
            // 
            // dataViewScrollerPanelWantedSystems
            // 
            this.dataViewScrollerPanelWantedSystems.Controls.Add(this.vScrollBarCustom2);
            this.dataViewScrollerPanelWantedSystems.Controls.Add(this.dataGridViewClosestSystems);
            this.dataViewScrollerPanelWantedSystems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanelWantedSystems.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanelWantedSystems.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanelWantedSystems.Name = "dataViewScrollerPanelWantedSystems";
            this.dataViewScrollerPanelWantedSystems.ScrollBarWidth = 20;
            this.dataViewScrollerPanelWantedSystems.Size = new System.Drawing.Size(381, 331);
            this.dataViewScrollerPanelWantedSystems.TabIndex = 14;
            this.dataViewScrollerPanelWantedSystems.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom2
            // 
            this.vScrollBarCustom2.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom2.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom2.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom2.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom2.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom2.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom2.HideScrollBar = true;
            this.vScrollBarCustom2.LargeChange = 0;
            this.vScrollBarCustom2.Location = new System.Drawing.Point(361, 21);
            this.vScrollBarCustom2.Maximum = -1;
            this.vScrollBarCustom2.Minimum = 0;
            this.vScrollBarCustom2.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom2.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom2.Name = "vScrollBarCustom2";
            this.vScrollBarCustom2.Size = new System.Drawing.Size(20, 310);
            this.vScrollBarCustom2.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom2.SmallChange = 1;
            this.vScrollBarCustom2.TabIndex = 14;
            this.vScrollBarCustom2.Text = "vScrollBarCustom2";
            this.vScrollBarCustom2.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom2.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom2.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom2.ThumbDrawAngle = 0F;
            this.vScrollBarCustom2.Value = -1;
            this.vScrollBarCustom2.ValueLimited = -1;
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
            this.Source,
            this.dataGridViewTextBoxColumnClosestSystemsSystem});
            this.dataGridViewClosestSystems.ContextMenuStrip = this.wantedContextMenu;
            this.dataGridViewClosestSystems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewClosestSystems.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewClosestSystems.Name = "dataGridViewClosestSystems";
            this.dataGridViewClosestSystems.ReadOnly = true;
            this.dataGridViewClosestSystems.RowHeadersVisible = false;
            this.dataGridViewClosestSystems.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewClosestSystems.Size = new System.Drawing.Size(361, 331);
            this.dataGridViewClosestSystems.TabIndex = 13;
            this.dataGridViewClosestSystems.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewClosestSystems_CellMouseClick);
            // 
            // Source
            // 
            this.Source.HeaderText = "Source";
            this.Source.Name = "Source";
            this.Source.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumnClosestSystemsSystem
            // 
            this.dataGridViewTextBoxColumnClosestSystemsSystem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnClosestSystemsSystem.HeaderText = "Wanted System";
            this.dataGridViewTextBoxColumnClosestSystemsSystem.MinimumWidth = 100;
            this.dataGridViewTextBoxColumnClosestSystemsSystem.Name = "dataGridViewTextBoxColumnClosestSystemsSystem";
            this.dataGridViewTextBoxColumnClosestSystemsSystem.ReadOnly = true;
            // 
            // wantedContextMenu
            // 
            this.wantedContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeFromWantedSystemsToolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem1});
            this.wantedContextMenu.Name = "wantedContextMenu";
            this.wantedContextMenu.Size = new System.Drawing.Size(234, 48);
            // 
            // removeFromWantedSystemsToolStripMenuItem
            // 
            this.removeFromWantedSystemsToolStripMenuItem.Name = "removeFromWantedSystemsToolStripMenuItem";
            this.removeFromWantedSystemsToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.removeFromWantedSystemsToolStripMenuItem.Text = "Remove from wanted systems";
            this.removeFromWantedSystemsToolStripMenuItem.Click += new System.EventHandler(this.removeFromWantedSystemsToolStripMenuItem_Click);
            // 
            // viewOnEDSMToolStripMenuItem1
            // 
            this.viewOnEDSMToolStripMenuItem1.Name = "viewOnEDSMToolStripMenuItem1";
            this.viewOnEDSMToolStripMenuItem1.Size = new System.Drawing.Size(233, 22);
            this.viewOnEDSMToolStripMenuItem1.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem1.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem1_Click);
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
            this.toolStripButtonSubmitDistances.Text = "&Submit Distances";
            this.toolStripButtonSubmitDistances.Click += new System.EventHandler(this.toolStripButtonSubmitDistances_Click);
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNew.Image")));
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(76, 22);
            this.toolStripButtonNew.Text = "Start &new";
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
            this.panel_controls.Controls.Add(this.textBox_status);
            this.panel_controls.Controls.Add(this.labelstpos);
            this.panel_controls.Controls.Add(this.textBoxSystemName);
            this.panel_controls.Controls.Add(this.labelTargetSystem);
            this.panel_controls.Controls.Add(this.labelCoordinates);
            this.panel_controls.Controls.Add(this.textBoxCoordinateX);
            this.panel_controls.Controls.Add(this.labelCoordinateX);
            this.panel_controls.Controls.Add(this.textBoxCoordinateY);
            this.panel_controls.Controls.Add(this.labelCoordinateY);
            this.panel_controls.Controls.Add(this.textBoxCoordinateZ);
            this.panel_controls.Controls.Add(this.labelCoordinateZ);
            this.panel_controls.Controls.Add(this.panelImplementation);
            this.panel_controls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_controls.Location = new System.Drawing.Point(0, 25);
            this.panel_controls.Name = "panel_controls";
            this.panel_controls.Size = new System.Drawing.Size(924, 74);
            this.panel_controls.TabIndex = 24;
            // 
            // textBox_status
            // 
            this.textBox_status.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_status.BorderColorScaling = 0.5F;
            this.textBox_status.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_status.Location = new System.Drawing.Point(102, 4);
            this.textBox_status.Name = "textBox_status";
            this.textBox_status.ReadOnly = true;
            this.textBox_status.Size = new System.Drawing.Size(167, 13);
            this.textBox_status.TabIndex = 21;
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
            // textBoxSystemName
            // 
            this.textBoxSystemName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxSystemName.BorderColorScaling = 0.5F;
            this.textBoxSystemName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxSystemName.Location = new System.Drawing.Point(72, 25);
            this.textBoxSystemName.Name = "textBoxSystemName";
            this.textBoxSystemName.ReadOnly = true;
            this.textBoxSystemName.Size = new System.Drawing.Size(178, 13);
            this.textBoxSystemName.TabIndex = 1;
            // 
            // textBoxCoordinateX
            // 
            this.textBoxCoordinateX.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxCoordinateX.BorderColorScaling = 0.5F;
            this.textBoxCoordinateX.Location = new System.Drawing.Point(165, 46);
            this.textBoxCoordinateX.Name = "textBoxCoordinateX";
            this.textBoxCoordinateX.ReadOnly = true;
            this.textBoxCoordinateX.Size = new System.Drawing.Size(60, 20);
            this.textBoxCoordinateX.TabIndex = 6;
            this.textBoxCoordinateX.Text = "?";
            this.textBoxCoordinateX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxCoordinateY
            // 
            this.textBoxCoordinateY.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxCoordinateY.BorderColorScaling = 0.5F;
            this.textBoxCoordinateY.Location = new System.Drawing.Point(245, 46);
            this.textBoxCoordinateY.Name = "textBoxCoordinateY";
            this.textBoxCoordinateY.ReadOnly = true;
            this.textBoxCoordinateY.Size = new System.Drawing.Size(60, 20);
            this.textBoxCoordinateY.TabIndex = 8;
            this.textBoxCoordinateY.Text = "?";
            this.textBoxCoordinateY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxCoordinateZ
            // 
            this.textBoxCoordinateZ.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxCoordinateZ.BorderColorScaling = 0.5F;
            this.textBoxCoordinateZ.Location = new System.Drawing.Point(325, 46);
            this.textBoxCoordinateZ.Name = "textBoxCoordinateZ";
            this.textBoxCoordinateZ.ReadOnly = true;
            this.textBoxCoordinateZ.Size = new System.Drawing.Size(60, 20);
            this.textBoxCoordinateZ.TabIndex = 10;
            this.textBoxCoordinateZ.Text = "?";
            this.textBoxCoordinateZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            this.trilatContextMenu.ResumeLayout(false);
            this.panelImplementation.ResumeLayout(false);
            this.panelImplementation.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainerTop.Panel1.ResumeLayout(false);
            this.splitContainerTop.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).EndInit();
            this.splitContainerTop.ResumeLayout(false);
            this.dataViewScrollerPanelDistances.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDistances)).EndInit();
            this.dataViewScrollerPanelSuggestedReferences.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSuggestedSystems)).EndInit();
            this.splitContainerBottom.Panel1.ResumeLayout(false);
            this.splitContainerBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBottom)).EndInit();
            this.splitContainerBottom.ResumeLayout(false);
            this.dataViewScrollerPanelWantedSystems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClosestSystems)).EndInit();
            this.wantedContextMenu.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel_controls.ResumeLayout(false);
            this.panel_controls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.TextBoxBorder textBoxSystemName;
        private System.Windows.Forms.Label labelTargetSystem;
        private System.Windows.Forms.Label labelCoordinates;
        private ExtendedControls.TextBoxBorder textBoxCoordinateX;
        private System.Windows.Forms.Label labelCoordinateX;
        private System.Windows.Forms.Label labelCoordinateY;
        private ExtendedControls.TextBoxBorder textBoxCoordinateY;
        private System.Windows.Forms.Label labelCoordinateZ;
        private ExtendedControls.TextBoxBorder textBoxCoordinateZ;
        private System.Windows.Forms.Panel panelImplementation;
        private ExtendedControls.RadioButtonCustom radioButtonAlgorithmJs;
        private ExtendedControls.RadioButtonCustom radioButtonAlgorithmCsharp;
        private System.Windows.Forms.Label labelAlgorithm;
        private System.Windows.Forms.ToolTip toolTipAlgorithm;
        private ExtendedControls.SplitContainerCustom splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSubmitDistances;
        private System.Windows.Forms.DataGridView dataGridViewSuggestedSystems;
        internal ExtendedControls.RichTextBoxScroll richTextBox_History;
        private System.Windows.Forms.DataGridView dataGridViewClosestSystems;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        protected System.Windows.Forms.DataGridView dataGridViewDistances;
        private System.Windows.Forms.ToolStripButton toolStripButtonMap;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveUnused;
        private System.Windows.Forms.Panel panel_controls;
        private System.Windows.Forms.Label labelstpos;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCalculated;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStatus;
        private ExtendedControls.TextBoxBorder textBox_status;
        private System.Windows.Forms.ContextMenuStrip trilatContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addToWantedSystemsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip wantedContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeFromWantedSystemsToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnClosestSystemsSystem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem1;
        private DataViewScrollerPanel dataViewScrollerPanelSuggestedReferences;
        private VScrollBarCustom vScrollBarCustom1;
        private DataViewScrollerPanel dataViewScrollerPanelWantedSystems;
        private VScrollBarCustom vScrollBarCustom2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnSuggestedSystemsSystem;
        private DataViewScrollerPanel dataViewScrollerPanelDistances;
        private VScrollBarCustom vScrollBarCustom3;
        private ExtendedControls.SplitContainerCustom splitContainerTop;
        private ExtendedControls.SplitContainerCustom splitContainerBottom;
    }
}
