
namespace EDDiscovery.UserControls
{
    partial class EngineerStatusPanel
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
            this.panelOuter = new System.Windows.Forms.Panel();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewEngineering = new BaseUtils.DataGridViewColumnControl();
            this.vScrollBarCustomMC = new ExtendedControls.ExtScrollBar();
            this.engineerImage = new System.Windows.Forms.PictureBox();
            this.labelBaseName = new System.Windows.Forms.Label();
            this.labelEngineerStatus = new System.Windows.Forms.Label();
            this.labelPlanet = new System.Windows.Forms.Label();
            this.labelEngineerDistance = new System.Windows.Forms.Label();
            this.labelEngineerStarSystem = new System.Windows.Forms.Label();
            this.labelEngineerName = new System.Windows.Forms.Label();
            this.UpgradeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WantedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PercentageCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NotesCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecipeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EngineersCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelOuter.SuspendLayout();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEngineering)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.engineerImage)).BeginInit();
            this.SuspendLayout();
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.dataViewScrollerPanel);
            this.panelOuter.Controls.Add(this.engineerImage);
            this.panelOuter.Controls.Add(this.labelBaseName);
            this.panelOuter.Controls.Add(this.labelEngineerStatus);
            this.panelOuter.Controls.Add(this.labelPlanet);
            this.panelOuter.Controls.Add(this.labelEngineerDistance);
            this.panelOuter.Controls.Add(this.labelEngineerStarSystem);
            this.panelOuter.Controls.Add(this.labelEngineerName);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 0);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(870, 311);
            this.panelOuter.TabIndex = 0;
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewEngineering);
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustomMC);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(357, 0);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(511, 309);
            this.dataViewScrollerPanel.TabIndex = 2;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewEngineering
            // 
            this.dataGridViewEngineering.AllowDrop = true;
            this.dataGridViewEngineering.AllowUserToAddRows = false;
            this.dataGridViewEngineering.AllowUserToDeleteRows = false;
            this.dataGridViewEngineering.AllowUserToOrderColumns = true;
            this.dataGridViewEngineering.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewEngineering.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEngineering.ColumnReorder = true;
            this.dataGridViewEngineering.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UpgradeCol,
            this.ModuleCol,
            this.LevelCol,
            this.MaxCol,
            this.WantedCol,
            this.PercentageCol,
            this.NotesCol,
            this.RecipeCol,
            this.EngineersCol});
            this.dataGridViewEngineering.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewEngineering.Name = "dataGridViewEngineering";
            this.dataGridViewEngineering.RowHeaderMenuStrip = null;
            this.dataGridViewEngineering.RowHeadersVisible = false;
            this.dataGridViewEngineering.RowHeadersWidth = 25;
            this.dataGridViewEngineering.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewEngineering.SingleRowSelect = true;
            this.dataGridViewEngineering.Size = new System.Drawing.Size(495, 309);
            this.dataGridViewEngineering.TabIndex = 1;
            this.dataGridViewEngineering.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewEngineering_CellEndEdit);
            // 
            // vScrollBarCustomMC
            // 
            this.vScrollBarCustomMC.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustomMC.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustomMC.ArrowColorScaling = 0.5F;
            this.vScrollBarCustomMC.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustomMC.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustomMC.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustomMC.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustomMC.HideScrollBar = false;
            this.vScrollBarCustomMC.LargeChange = 0;
            this.vScrollBarCustomMC.Location = new System.Drawing.Point(495, 0);
            this.vScrollBarCustomMC.Maximum = -1;
            this.vScrollBarCustomMC.Minimum = 0;
            this.vScrollBarCustomMC.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomMC.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomMC.Name = "vScrollBarCustomMC";
            this.vScrollBarCustomMC.Size = new System.Drawing.Size(16, 309);
            this.vScrollBarCustomMC.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustomMC.SmallChange = 1;
            this.vScrollBarCustomMC.TabIndex = 0;
            this.vScrollBarCustomMC.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustomMC.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustomMC.ThumbColorScaling = 0.5F;
            this.vScrollBarCustomMC.ThumbDrawAngle = 0F;
            this.vScrollBarCustomMC.Value = -1;
            this.vScrollBarCustomMC.ValueLimited = -1;
            // 
            // engineerImage
            // 
            this.engineerImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.engineerImage.Location = new System.Drawing.Point(0, 0);
            this.engineerImage.Name = "engineerImage";
            this.engineerImage.Size = new System.Drawing.Size(258, 210);
            this.engineerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.engineerImage.TabIndex = 1;
            this.engineerImage.TabStop = false;
            // 
            // labelBaseName
            // 
            this.labelBaseName.AutoSize = true;
            this.labelBaseName.Location = new System.Drawing.Point(263, 83);
            this.labelBaseName.Name = "labelBaseName";
            this.labelBaseName.Size = new System.Drawing.Size(43, 13);
            this.labelBaseName.TabIndex = 0;
            this.labelBaseName.Text = "<code>";
            // 
            // labelEngineerStatus
            // 
            this.labelEngineerStatus.AutoSize = true;
            this.labelEngineerStatus.Location = new System.Drawing.Point(263, 103);
            this.labelEngineerStatus.Name = "labelEngineerStatus";
            this.labelEngineerStatus.Size = new System.Drawing.Size(43, 13);
            this.labelEngineerStatus.TabIndex = 0;
            this.labelEngineerStatus.Text = "<code>";
            // 
            // labelPlanet
            // 
            this.labelPlanet.AutoSize = true;
            this.labelPlanet.Location = new System.Drawing.Point(263, 63);
            this.labelPlanet.Name = "labelPlanet";
            this.labelPlanet.Size = new System.Drawing.Size(43, 13);
            this.labelPlanet.TabIndex = 0;
            this.labelPlanet.Text = "<code>";
            // 
            // labelEngineerDistance
            // 
            this.labelEngineerDistance.AutoSize = true;
            this.labelEngineerDistance.Location = new System.Drawing.Point(263, 43);
            this.labelEngineerDistance.Name = "labelEngineerDistance";
            this.labelEngineerDistance.Size = new System.Drawing.Size(43, 13);
            this.labelEngineerDistance.TabIndex = 0;
            this.labelEngineerDistance.Text = "<code>";
            // 
            // labelEngineerStarSystem
            // 
            this.labelEngineerStarSystem.AutoSize = true;
            this.labelEngineerStarSystem.Location = new System.Drawing.Point(263, 23);
            this.labelEngineerStarSystem.Name = "labelEngineerStarSystem";
            this.labelEngineerStarSystem.Size = new System.Drawing.Size(43, 13);
            this.labelEngineerStarSystem.TabIndex = 0;
            this.labelEngineerStarSystem.Text = "<code>";
            // 
            // labelEngineerName
            // 
            this.labelEngineerName.AutoSize = true;
            this.labelEngineerName.Location = new System.Drawing.Point(264, 3);
            this.labelEngineerName.Name = "labelEngineerName";
            this.labelEngineerName.Size = new System.Drawing.Size(43, 13);
            this.labelEngineerName.TabIndex = 0;
            this.labelEngineerName.Text = "<code>";
            // 
            // UpgradeCol
            // 
            this.UpgradeCol.HeaderText = "Upgrade/Mat";
            this.UpgradeCol.MinimumWidth = 50;
            this.UpgradeCol.Name = "UpgradeCol";
            this.UpgradeCol.ReadOnly = true;
            this.UpgradeCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ModuleCol
            // 
            this.ModuleCol.HeaderText = "Module";
            this.ModuleCol.MinimumWidth = 50;
            this.ModuleCol.Name = "ModuleCol";
            this.ModuleCol.ReadOnly = true;
            this.ModuleCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // LevelCol
            // 
            this.LevelCol.FillWeight = 25F;
            this.LevelCol.HeaderText = "Level";
            this.LevelCol.MinimumWidth = 50;
            this.LevelCol.Name = "LevelCol";
            this.LevelCol.ReadOnly = true;
            this.LevelCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MaxCol
            // 
            this.MaxCol.FillWeight = 25F;
            this.MaxCol.HeaderText = "Max";
            this.MaxCol.MinimumWidth = 50;
            this.MaxCol.Name = "MaxCol";
            this.MaxCol.ReadOnly = true;
            this.MaxCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // WantedCol
            // 
            this.WantedCol.FillWeight = 25F;
            this.WantedCol.HeaderText = "Wanted";
            this.WantedCol.MinimumWidth = 50;
            this.WantedCol.Name = "WantedCol";
            this.WantedCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PercentageCol
            // 
            this.PercentageCol.FillWeight = 25F;
            this.PercentageCol.HeaderText = "%";
            this.PercentageCol.Name = "PercentageCol";
            // 
            // NotesCol
            // 
            this.NotesCol.FillWeight = 150F;
            this.NotesCol.HeaderText = "Notes";
            this.NotesCol.MinimumWidth = 50;
            this.NotesCol.Name = "NotesCol";
            this.NotesCol.ReadOnly = true;
            this.NotesCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RecipeCol
            // 
            this.RecipeCol.FillWeight = 50F;
            this.RecipeCol.HeaderText = "Recipe";
            this.RecipeCol.MinimumWidth = 15;
            this.RecipeCol.Name = "RecipeCol";
            this.RecipeCol.ReadOnly = true;
            this.RecipeCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // EngineersCol
            // 
            this.EngineersCol.HeaderText = "Engineers";
            this.EngineersCol.MinimumWidth = 50;
            this.EngineersCol.Name = "EngineersCol";
            this.EngineersCol.ReadOnly = true;
            this.EngineersCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // EngineerStatusPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelOuter);
            this.Name = "EngineerStatusPanel";
            this.Size = new System.Drawing.Size(870, 311);
            this.panelOuter.ResumeLayout(false);
            this.panelOuter.PerformLayout();
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEngineering)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.engineerImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelOuter;
        private System.Windows.Forms.PictureBox engineerImage;
        private System.Windows.Forms.Label labelEngineerName;
        private System.Windows.Forms.Label labelEngineerStatus;
        private System.Windows.Forms.Label labelEngineerDistance;
        private System.Windows.Forms.Label labelEngineerStarSystem;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private BaseUtils.DataGridViewColumnControl dataGridViewEngineering;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        private System.Windows.Forms.Label labelBaseName;
        private System.Windows.Forms.Label labelPlanet;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpgradeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn WantedCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PercentageCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn NotesCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecipeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn EngineersCol;
    }
}
