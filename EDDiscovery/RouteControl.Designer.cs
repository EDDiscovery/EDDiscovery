namespace EDDiscovery
{
    partial class RouteControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RouteControl));
            this.richTextBox_routeresult = new ExtendedControls.RichTextBoxScroll();
            this.textBox_From = new ExtendedControls.TextBoxBorder();
            this.label3 = new System.Windows.Forms.Label();
            this.button_Route = new ExtendedControls.ButtonExt();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_To = new ExtendedControls.TextBoxBorder();
            this.textBox_Range = new ExtendedControls.TextBoxBorder();
            this.textBoxCurrent = new ExtendedControls.TextBoxBorder();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_FromX = new ExtendedControls.TextBoxBorder();
            this.textBox_FromY = new ExtendedControls.TextBoxBorder();
            this.textBox_FromZ = new ExtendedControls.TextBoxBorder();
            this.textBox_ToX = new ExtendedControls.TextBoxBorder();
            this.textBox_ToY = new ExtendedControls.TextBoxBorder();
            this.textBox_ToZ = new ExtendedControls.TextBoxBorder();
            this.comboBoxRoutingMetric = new ExtendedControls.ComboBoxCustom();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_Distance = new ExtendedControls.TextBoxBorder();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox_Entries = new ExtendedControls.GroupBoxCustom();
            this.cmd3DMap = new ExtendedControls.ButtonExt();
            this.groupBox_Entries.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox_routeresult
            // 
            this.richTextBox_routeresult.BorderColor = System.Drawing.Color.Transparent;
            this.richTextBox_routeresult.BorderColorScaling = 0.5F;
            this.richTextBox_routeresult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_routeresult.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox_routeresult.Location = new System.Drawing.Point(0, 105);
            this.richTextBox_routeresult.Name = "richTextBox_routeresult";
            this.richTextBox_routeresult.ScrollBarWidth = 20;
            this.richTextBox_routeresult.ShowLineCount = false;
            this.richTextBox_routeresult.Size = new System.Drawing.Size(897, 324);
            this.richTextBox_routeresult.TabIndex = 11;
            // 
            // textBox_From
            // 
            this.textBox_From.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_From.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_From.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_From.Location = new System.Drawing.Point(57, 19);
            this.textBox_From.Name = "textBox_From";
            this.textBox_From.ReadOnly = true;
            this.textBox_From.Size = new System.Drawing.Size(234, 20);
            this.textBox_From.TabIndex = 0;
            this.textBox_From.TextChanged += new System.EventHandler(this.textBox_From_TextChanged);
            this.textBox_From.Enter += new System.EventHandler(this.textBox_From_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "From";
            // 
            // button_Route
            // 
            this.button_Route.BorderColorScaling = 1.25F;
            this.button_Route.ButtonColorScaling = 0.5F;
            this.button_Route.ButtonDisabledScaling = 0.5F;
            this.button_Route.Enabled = false;
            this.button_Route.Location = new System.Drawing.Point(745, 19);
            this.button_Route.Name = "button_Route";
            this.button_Route.Size = new System.Drawing.Size(111, 27);
            this.button_Route.TabIndex = 10;
            this.button_Route.Text = "Find route";
            this.button_Route.UseVisualStyleBackColor = true;
            this.button_Route.Click += new System.EventHandler(this.button_Route_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(566, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Max jump";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(300, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "To";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(687, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "ly";
            // 
            // textBox_To
            // 
            this.textBox_To.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_To.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_To.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_To.Location = new System.Drawing.Point(326, 19);
            this.textBox_To.Name = "textBox_To";
            this.textBox_To.ReadOnly = true;
            this.textBox_To.Size = new System.Drawing.Size(234, 20);
            this.textBox_To.TabIndex = 1;
            this.textBox_To.TextChanged += new System.EventHandler(this.textBox_To_TextChanged);
            this.textBox_To.Enter += new System.EventHandler(this.textBox_To_Enter);
            // 
            // textBox_Range
            // 
            this.textBox_Range.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_Range.Location = new System.Drawing.Point(624, 19);
            this.textBox_Range.Name = "textBox_Range";
            this.textBox_Range.ReadOnly = true;
            this.textBox_Range.Size = new System.Drawing.Size(57, 20);
            this.textBox_Range.TabIndex = 2;
            this.textBox_Range.Click += new System.EventHandler(this.textBox_Clicked);
            this.textBox_Range.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Range_KeyPress);
            // 
            // textBoxCurrent
            // 
            this.textBoxCurrent.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxCurrent.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxCurrent.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxCurrent.Location = new System.Drawing.Point(57, 76);
            this.textBoxCurrent.Name = "textBoxCurrent";
            this.textBoxCurrent.ReadOnly = true;
            this.textBoxCurrent.Size = new System.Drawing.Size(125, 20);
            this.textBoxCurrent.TabIndex = 12;
            this.textBoxCurrent.TabStop = false;
            this.textBoxCurrent.DoubleClick += new System.EventHandler(this.textBoxCurrent_DoubleClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Current";
            // 
            // textBox_FromX
            // 
            this.textBox_FromX.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_FromX.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_FromX.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_FromX.Location = new System.Drawing.Point(57, 48);
            this.textBox_FromX.Name = "textBox_FromX";
            this.textBox_FromX.ReadOnly = true;
            this.textBox_FromX.Size = new System.Drawing.Size(72, 20);
            this.textBox_FromX.TabIndex = 3;
            this.textBox_FromX.Click += new System.EventHandler(this.textBox_Clicked);
            this.textBox_FromX.TextChanged += new System.EventHandler(this.textBox_XYZ_From_Changed);
            this.textBox_FromX.Enter += new System.EventHandler(this.textBox_FromXYZ_Enter);
            // 
            // textBox_FromY
            // 
            this.textBox_FromY.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_FromY.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_FromY.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_FromY.Location = new System.Drawing.Point(135, 48);
            this.textBox_FromY.Name = "textBox_FromY";
            this.textBox_FromY.ReadOnly = true;
            this.textBox_FromY.Size = new System.Drawing.Size(72, 20);
            this.textBox_FromY.TabIndex = 4;
            this.textBox_FromY.Click += new System.EventHandler(this.textBox_Clicked);
            this.textBox_FromY.TextChanged += new System.EventHandler(this.textBox_XYZ_From_Changed);
            this.textBox_FromY.Enter += new System.EventHandler(this.textBox_FromXYZ_Enter);
            // 
            // textBox_FromZ
            // 
            this.textBox_FromZ.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_FromZ.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_FromZ.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_FromZ.Location = new System.Drawing.Point(213, 48);
            this.textBox_FromZ.Name = "textBox_FromZ";
            this.textBox_FromZ.ReadOnly = true;
            this.textBox_FromZ.Size = new System.Drawing.Size(72, 20);
            this.textBox_FromZ.TabIndex = 5;
            this.textBox_FromZ.Click += new System.EventHandler(this.textBox_Clicked);
            this.textBox_FromZ.TextChanged += new System.EventHandler(this.textBox_XYZ_From_Changed);
            this.textBox_FromZ.Enter += new System.EventHandler(this.textBox_FromXYZ_Enter);
            // 
            // textBox_ToX
            // 
            this.textBox_ToX.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_ToX.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_ToX.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_ToX.Location = new System.Drawing.Point(325, 47);
            this.textBox_ToX.Name = "textBox_ToX";
            this.textBox_ToX.ReadOnly = true;
            this.textBox_ToX.Size = new System.Drawing.Size(72, 20);
            this.textBox_ToX.TabIndex = 6;
            this.textBox_ToX.Click += new System.EventHandler(this.textBox_Clicked);
            this.textBox_ToX.TextChanged += new System.EventHandler(this.textBox_XYZ_To_Changed);
            this.textBox_ToX.Enter += new System.EventHandler(this.textBox_ToXYZ_Enter);
            // 
            // textBox_ToY
            // 
            this.textBox_ToY.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_ToY.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_ToY.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_ToY.Location = new System.Drawing.Point(403, 47);
            this.textBox_ToY.Name = "textBox_ToY";
            this.textBox_ToY.ReadOnly = true;
            this.textBox_ToY.Size = new System.Drawing.Size(72, 20);
            this.textBox_ToY.TabIndex = 7;
            this.textBox_ToY.Click += new System.EventHandler(this.textBox_Clicked);
            this.textBox_ToY.TextChanged += new System.EventHandler(this.textBox_XYZ_To_Changed);
            this.textBox_ToY.Enter += new System.EventHandler(this.textBox_ToXYZ_Enter);
            // 
            // textBox_ToZ
            // 
            this.textBox_ToZ.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_ToZ.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_ToZ.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_ToZ.Location = new System.Drawing.Point(481, 47);
            this.textBox_ToZ.Name = "textBox_ToZ";
            this.textBox_ToZ.ReadOnly = true;
            this.textBox_ToZ.Size = new System.Drawing.Size(72, 20);
            this.textBox_ToZ.TabIndex = 8;
            this.textBox_ToZ.Click += new System.EventHandler(this.textBox_Clicked);
            this.textBox_ToZ.TextChanged += new System.EventHandler(this.textBox_XYZ_To_Changed);
            this.textBox_ToZ.Enter += new System.EventHandler(this.textBox_ToXYZ_Enter);
            // 
            // comboBoxRoutingMetric
            // 
            this.comboBoxRoutingMetric.ArrowWidth = 1;
            this.comboBoxRoutingMetric.BorderColor = System.Drawing.Color.Red;
            this.comboBoxRoutingMetric.ButtonColorScaling = 0.5F;
            this.comboBoxRoutingMetric.DataSource = null;
            this.comboBoxRoutingMetric.DisplayMember = null;
            this.comboBoxRoutingMetric.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxRoutingMetric.DropDownHeight = 200;
            this.comboBoxRoutingMetric.Enabled = false;
            this.comboBoxRoutingMetric.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxRoutingMetric.ItemHeight = 20;
            this.comboBoxRoutingMetric.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("comboBoxRoutingMetric.Items")));
            this.comboBoxRoutingMetric.Location = new System.Drawing.Point(326, 76);
            this.comboBoxRoutingMetric.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxRoutingMetric.Name = "comboBoxRoutingMetric";
            this.comboBoxRoutingMetric.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxRoutingMetric.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxRoutingMetric.ScrollBarWidth = 16;
            this.comboBoxRoutingMetric.SelectedIndex = -1;
            this.comboBoxRoutingMetric.SelectedItem = null;
            this.comboBoxRoutingMetric.Size = new System.Drawing.Size(234, 21);
            this.comboBoxRoutingMetric.TabIndex = 9;
            this.comboBoxRoutingMetric.ValueMember = null;
            this.comboBoxRoutingMetric.SelectedIndexChanged += new ExtendedControls.ComboBoxCustom.OnSelectedIndexChanged(this.comboBoxRoutingMetric_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(244, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Routing Metric";
            // 
            // textBox_Distance
            // 
            this.textBox_Distance.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_Distance.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_Distance.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_Distance.Location = new System.Drawing.Point(624, 47);
            this.textBox_Distance.Name = "textBox_Distance";
            this.textBox_Distance.ReadOnly = true;
            this.textBox_Distance.Size = new System.Drawing.Size(57, 20);
            this.textBox_Distance.TabIndex = 8;
            this.textBox_Distance.TabStop = false;
            this.textBox_Distance.Click += new System.EventHandler(this.textBox_Clicked);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(569, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Distance";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(687, 49);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "ly";
            // 
            // groupBox_Entries
            // 
            this.groupBox_Entries.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBox_Entries.BackColorScaling = 0.5F;
            this.groupBox_Entries.BorderColor = System.Drawing.Color.LightGray;
            this.groupBox_Entries.BorderColorScaling = 0.5F;
            this.groupBox_Entries.Controls.Add(this.cmd3DMap);
            this.groupBox_Entries.Controls.Add(this.textBox_From);
            this.groupBox_Entries.Controls.Add(this.textBox_Range);
            this.groupBox_Entries.Controls.Add(this.textBox_To);
            this.groupBox_Entries.Controls.Add(this.label1);
            this.groupBox_Entries.Controls.Add(this.label9);
            this.groupBox_Entries.Controls.Add(this.textBox_Distance);
            this.groupBox_Entries.Controls.Add(this.label4);
            this.groupBox_Entries.Controls.Add(this.textBox_ToZ);
            this.groupBox_Entries.Controls.Add(this.label2);
            this.groupBox_Entries.Controls.Add(this.textBox_ToY);
            this.groupBox_Entries.Controls.Add(this.label7);
            this.groupBox_Entries.Controls.Add(this.textBox_ToX);
            this.groupBox_Entries.Controls.Add(this.label6);
            this.groupBox_Entries.Controls.Add(this.textBox_FromZ);
            this.groupBox_Entries.Controls.Add(this.button_Route);
            this.groupBox_Entries.Controls.Add(this.textBox_FromY);
            this.groupBox_Entries.Controls.Add(this.label3);
            this.groupBox_Entries.Controls.Add(this.textBox_FromX);
            this.groupBox_Entries.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Entries.FillClientAreaWithAlternateColor = false;
            this.groupBox_Entries.Location = new System.Drawing.Point(0, 0);
            this.groupBox_Entries.Name = "groupBox_Entries";
            this.groupBox_Entries.Size = new System.Drawing.Size(897, 105);
            this.groupBox_Entries.TabIndex = 24;
            this.groupBox_Entries.TabStop = false;
            this.groupBox_Entries.TextPadding = 0;
            this.groupBox_Entries.TextStartPosition = -1;
            // 
            // cmd3DMap
            // 
            this.cmd3DMap.Location = new System.Drawing.Point(745, 66);
            this.cmd3DMap.Name = "cmd3DMap";
            this.cmd3DMap.Size = new System.Drawing.Size(111, 26);
            this.cmd3DMap.TabIndex = 24;
            this.cmd3DMap.Text = "3D Map";
            this.cmd3DMap.UseVisualStyleBackColor = true;
            this.cmd3DMap.Click += new System.EventHandler(this.cmd3DMap_Click);
            // 
            // RouteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxRoutingMetric);
            this.Controls.Add(this.textBoxCurrent);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.richTextBox_routeresult);
            this.Controls.Add(this.groupBox_Entries);
            this.Name = "RouteControl";
            this.Size = new System.Drawing.Size(897, 429);
            this.groupBox_Entries.ResumeLayout(false);
            this.groupBox_Entries.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.RichTextBoxScroll richTextBox_routeresult;
        internal ExtendedControls.TextBoxBorder textBox_From;
        private System.Windows.Forms.Label label3;
        private ExtendedControls.ButtonExt button_Route;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        internal ExtendedControls.TextBoxBorder textBox_To;
        internal ExtendedControls.TextBoxBorder textBoxCurrent;
        private System.Windows.Forms.Label label5;
        internal ExtendedControls.TextBoxBorder textBox_Range;
        internal ExtendedControls.TextBoxBorder textBox_FromX;
        internal ExtendedControls.TextBoxBorder textBox_FromY;
        internal ExtendedControls.TextBoxBorder textBox_FromZ;
        internal ExtendedControls.TextBoxBorder textBox_ToX;
        internal ExtendedControls.TextBoxBorder textBox_ToY;
        internal ExtendedControls.TextBoxBorder textBox_ToZ;
        private ExtendedControls.ComboBoxCustom comboBoxRoutingMetric;
        private System.Windows.Forms.Label label6;
        internal ExtendedControls.TextBoxBorder textBox_Distance;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private ExtendedControls.GroupBoxCustom groupBox_Entries;
        private ExtendedControls.ButtonExt cmd3DMap;
    }
}
