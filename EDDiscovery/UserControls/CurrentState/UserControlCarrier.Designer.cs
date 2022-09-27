namespace EDDiscovery.UserControls
{
    partial class UserControlCarrier
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
            ExtendedControls.TabStyleSquare tabStyleSquare1 = new ExtendedControls.TabStyleSquare();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extTabControlOverall = new ExtendedControls.ExtTabControl();
            this.tabPageOverall = new System.Windows.Forms.TabPage();
            this.labelStarSystem = new System.Windows.Forms.Label();
            this.labelCallSign = new System.Windows.Forms.Label();
            this.labelNotoriusAccess = new System.Windows.Forms.Label();
            this.labelDockingAccess = new System.Windows.Forms.Label();
            this.labelFuel = new System.Windows.Forms.Label();
            this.labelBalance = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.tabPageIterinery = new System.Windows.Forms.TabPage();
            this.tabPageFinances = new System.Windows.Forms.TabPage();
            this.tabPageServices = new System.Windows.Forms.TabPage();
            this.tabPageCargo = new System.Windows.Forms.TabPage();
            this.tabPageShips = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPageMarket = new System.Windows.Forms.TabPage();
            this.extTabControlOverall.SuspendLayout();
            this.tabPageOverall.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // extTabControlOverall
            // 
            this.extTabControlOverall.AllowDragReorder = false;
            this.extTabControlOverall.Controls.Add(this.tabPageOverall);
            this.extTabControlOverall.Controls.Add(this.tabPageIterinery);
            this.extTabControlOverall.Controls.Add(this.tabPageFinances);
            this.extTabControlOverall.Controls.Add(this.tabPageServices);
            this.extTabControlOverall.Controls.Add(this.tabPageCargo);
            this.extTabControlOverall.Controls.Add(this.tabPageShips);
            this.extTabControlOverall.Controls.Add(this.tabPage1);
            this.extTabControlOverall.Controls.Add(this.tabPageMarket);
            this.extTabControlOverall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extTabControlOverall.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extTabControlOverall.Location = new System.Drawing.Point(0, 0);
            this.extTabControlOverall.Name = "extTabControlOverall";
            this.extTabControlOverall.SelectedIndex = 0;
            this.extTabControlOverall.Size = new System.Drawing.Size(853, 572);
            this.extTabControlOverall.TabColorScaling = 0.5F;
            this.extTabControlOverall.TabControlBorderBrightColor = System.Drawing.Color.LightGray;
            this.extTabControlOverall.TabControlBorderColor = System.Drawing.Color.DarkGray;
            this.extTabControlOverall.TabDisabledScaling = 0.5F;
            this.extTabControlOverall.TabIndex = 0;
            this.extTabControlOverall.TabMouseOverColor = System.Drawing.Color.White;
            this.extTabControlOverall.TabNotSelectedBorderColor = System.Drawing.Color.Gray;
            this.extTabControlOverall.TabNotSelectedColor = System.Drawing.Color.Gray;
            this.extTabControlOverall.TabOpaque = 100F;
            this.extTabControlOverall.TabSelectedColor = System.Drawing.Color.LightGray;
            this.extTabControlOverall.TabStyle = tabStyleSquare1;
            this.extTabControlOverall.TextNotSelectedColor = System.Drawing.SystemColors.ControlText;
            this.extTabControlOverall.TextSelectedColor = System.Drawing.SystemColors.ControlText;
            this.extTabControlOverall.SelectedIndexChanged += new System.EventHandler(this.extTabControl1_SelectedIndexChanged);
            // 
            // tabPageOverall
            // 
            this.tabPageOverall.Controls.Add(this.labelStarSystem);
            this.tabPageOverall.Controls.Add(this.labelCallSign);
            this.tabPageOverall.Controls.Add(this.labelNotoriusAccess);
            this.tabPageOverall.Controls.Add(this.labelDockingAccess);
            this.tabPageOverall.Controls.Add(this.labelFuel);
            this.tabPageOverall.Controls.Add(this.labelBalance);
            this.tabPageOverall.Controls.Add(this.labelName);
            this.tabPageOverall.Location = new System.Drawing.Point(4, 22);
            this.tabPageOverall.Name = "tabPageOverall";
            this.tabPageOverall.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOverall.Size = new System.Drawing.Size(845, 546);
            this.tabPageOverall.TabIndex = 0;
            this.tabPageOverall.Text = "Overall";
            this.tabPageOverall.UseVisualStyleBackColor = true;
            // 
            // labelStarSystem
            // 
            this.labelStarSystem.AutoSize = true;
            this.labelStarSystem.Location = new System.Drawing.Point(6, 77);
            this.labelStarSystem.Name = "labelStarSystem";
            this.labelStarSystem.Size = new System.Drawing.Size(66, 13);
            this.labelStarSystem.TabIndex = 0;
            this.labelStarSystem.Text = "Star System:";
            // 
            // labelCallSign
            // 
            this.labelCallSign.AutoSize = true;
            this.labelCallSign.Location = new System.Drawing.Point(6, 47);
            this.labelCallSign.Name = "labelCallSign";
            this.labelCallSign.Size = new System.Drawing.Size(46, 13);
            this.labelCallSign.TabIndex = 0;
            this.labelCallSign.Text = "Callsign:";
            // 
            // labelNotoriusAccess
            // 
            this.labelNotoriusAccess.AutoSize = true;
            this.labelNotoriusAccess.Location = new System.Drawing.Point(657, 426);
            this.labelNotoriusAccess.Name = "labelNotoriusAccess";
            this.labelNotoriusAccess.Size = new System.Drawing.Size(93, 13);
            this.labelNotoriusAccess.TabIndex = 0;
            this.labelNotoriusAccess.Text = "Notorious Access:";
            // 
            // labelDockingAccess
            // 
            this.labelDockingAccess.AutoSize = true;
            this.labelDockingAccess.Location = new System.Drawing.Point(657, 400);
            this.labelDockingAccess.Name = "labelDockingAccess";
            this.labelDockingAccess.Size = new System.Drawing.Size(88, 13);
            this.labelDockingAccess.TabIndex = 0;
            this.labelDockingAccess.Text = "Docking Access:";
            // 
            // labelFuel
            // 
            this.labelFuel.AutoSize = true;
            this.labelFuel.Location = new System.Drawing.Point(715, 47);
            this.labelFuel.Name = "labelFuel";
            this.labelFuel.Size = new System.Drawing.Size(30, 13);
            this.labelFuel.TabIndex = 0;
            this.labelFuel.Text = "Fuel:";
            // 
            // labelBalance
            // 
            this.labelBalance.AutoSize = true;
            this.labelBalance.Location = new System.Drawing.Point(715, 18);
            this.labelBalance.Name = "labelBalance";
            this.labelBalance.Size = new System.Drawing.Size(49, 13);
            this.labelBalance.TabIndex = 0;
            this.labelBalance.Text = "Balance:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(6, 18);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name:";
            // 
            // tabPageIterinery
            // 
            this.tabPageIterinery.Location = new System.Drawing.Point(4, 22);
            this.tabPageIterinery.Name = "tabPageIterinery";
            this.tabPageIterinery.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIterinery.Size = new System.Drawing.Size(845, 546);
            this.tabPageIterinery.TabIndex = 1;
            this.tabPageIterinery.Text = "Iterinery";
            this.tabPageIterinery.UseVisualStyleBackColor = true;
            this.tabPageIterinery.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // tabPageFinances
            // 
            this.tabPageFinances.Location = new System.Drawing.Point(4, 22);
            this.tabPageFinances.Name = "tabPageFinances";
            this.tabPageFinances.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFinances.Size = new System.Drawing.Size(845, 546);
            this.tabPageFinances.TabIndex = 2;
            this.tabPageFinances.Text = "Finances";
            this.tabPageFinances.UseVisualStyleBackColor = true;
            // 
            // tabPageServices
            // 
            this.tabPageServices.Location = new System.Drawing.Point(4, 22);
            this.tabPageServices.Name = "tabPageServices";
            this.tabPageServices.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageServices.Size = new System.Drawing.Size(845, 546);
            this.tabPageServices.TabIndex = 3;
            this.tabPageServices.Text = "Services";
            this.tabPageServices.UseVisualStyleBackColor = true;
            // 
            // tabPageCargo
            // 
            this.tabPageCargo.Location = new System.Drawing.Point(4, 22);
            this.tabPageCargo.Name = "tabPageCargo";
            this.tabPageCargo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCargo.Size = new System.Drawing.Size(845, 546);
            this.tabPageCargo.TabIndex = 4;
            this.tabPageCargo.Text = "Cargo";
            this.tabPageCargo.UseVisualStyleBackColor = true;
            // 
            // tabPageShips
            // 
            this.tabPageShips.Location = new System.Drawing.Point(4, 22);
            this.tabPageShips.Name = "tabPageShips";
            this.tabPageShips.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageShips.Size = new System.Drawing.Size(845, 546);
            this.tabPageShips.TabIndex = 5;
            this.tabPageShips.Text = "Ships";
            this.tabPageShips.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(845, 546);
            this.tabPage1.TabIndex = 6;
            this.tabPage1.Text = "Modules";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPageMarket
            // 
            this.tabPageMarket.Location = new System.Drawing.Point(4, 22);
            this.tabPageMarket.Name = "tabPageMarket";
            this.tabPageMarket.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMarket.Size = new System.Drawing.Size(845, 546);
            this.tabPageMarket.TabIndex = 7;
            this.tabPageMarket.Text = "Market";
            this.tabPageMarket.UseVisualStyleBackColor = true;
            // 
            // UserControlCarrier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.extTabControlOverall);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserControlCarrier";
            this.Size = new System.Drawing.Size(853, 572);
            this.extTabControlOverall.ResumeLayout(false);
            this.tabPageOverall.ResumeLayout(false);
            this.tabPageOverall.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtTabControl extTabControlOverall;
        private System.Windows.Forms.TabPage tabPageOverall;
        private System.Windows.Forms.Label labelStarSystem;
        private System.Windows.Forms.Label labelCallSign;
        private System.Windows.Forms.Label labelNotoriusAccess;
        private System.Windows.Forms.Label labelDockingAccess;
        private System.Windows.Forms.Label labelFuel;
        private System.Windows.Forms.Label labelBalance;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TabPage tabPageIterinery;
        private System.Windows.Forms.TabPage tabPageFinances;
        private System.Windows.Forms.TabPage tabPageServices;
        private System.Windows.Forms.TabPage tabPageCargo;
        private System.Windows.Forms.TabPage tabPageShips;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPageMarket;
    }
}
