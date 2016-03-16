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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textBox_From = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_Route = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_To = new System.Windows.Forms.TextBox();
            this.textBox_Range = new System.Windows.Forms.TextBox();
            this.textBoxCurrent = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_FromX = new System.Windows.Forms.TextBox();
            this.textBox_FromY = new System.Windows.Forms.TextBox();
            this.textBox_FromZ = new System.Windows.Forms.TextBox();
            this.textBox_ToX = new System.Windows.Forms.TextBox();
            this.textBox_ToY = new System.Windows.Forms.TextBox();
            this.textBox_ToZ = new System.Windows.Forms.TextBox();
            this.comboBoxRoutingMetric = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_Distance = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(3, 94);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(849, 332);
            this.richTextBox1.TabIndex = 11;
            this.richTextBox1.Text = "";
            // 
            // textBox_From
            // 
            this.textBox_From.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_From.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_From.Location = new System.Drawing.Point(53, 3);
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
            this.label3.Location = new System.Drawing.Point(4, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "From";
            // 
            // button_Route
            // 
            this.button_Route.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Route.Enabled = false;
            this.button_Route.Location = new System.Drawing.Point(741, 0);
            this.button_Route.Name = "button_Route";
            this.button_Route.Size = new System.Drawing.Size(140, 27);
            this.button_Route.TabIndex = 10;
            this.button_Route.Text = "Find route";
            this.button_Route.UseVisualStyleBackColor = true;
            this.button_Route.Click += new System.EventHandler(this.button_Route_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(562, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Max jump";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(296, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "To";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(683, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "ly";
            // 
            // textBox_To
            // 
            this.textBox_To.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_To.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_To.Location = new System.Drawing.Point(322, 2);
            this.textBox_To.Name = "textBox_To";
            this.textBox_To.ReadOnly = true;
            this.textBox_To.Size = new System.Drawing.Size(234, 20);
            this.textBox_To.TabIndex = 1;
            this.textBox_To.TextChanged += new System.EventHandler(this.textBox_To_TextChanged);
            this.textBox_To.Enter += new System.EventHandler(this.textBox_To_Enter);
            // 
            // textBox_Range
            // 
            this.textBox_Range.Location = new System.Drawing.Point(620, 2);
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
            this.textBoxCurrent.Location = new System.Drawing.Point(53, 55);
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
            this.label5.Location = new System.Drawing.Point(3, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Current";
            // 
            // textBox_FromX
            // 
            this.textBox_FromX.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_FromX.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_FromX.Location = new System.Drawing.Point(53, 29);
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
            this.textBox_FromY.Location = new System.Drawing.Point(131, 29);
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
            this.textBox_FromZ.Location = new System.Drawing.Point(209, 29);
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
            this.textBox_ToX.Location = new System.Drawing.Point(321, 28);
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
            this.textBox_ToY.Location = new System.Drawing.Point(399, 28);
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
            this.textBox_ToZ.Location = new System.Drawing.Point(477, 28);
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
            this.comboBoxRoutingMetric.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRoutingMetric.Enabled = false;
            this.comboBoxRoutingMetric.FormattingEnabled = true;
            this.comboBoxRoutingMetric.Location = new System.Drawing.Point(322, 55);
            this.comboBoxRoutingMetric.Name = "comboBoxRoutingMetric";
            this.comboBoxRoutingMetric.Size = new System.Drawing.Size(234, 21);
            this.comboBoxRoutingMetric.TabIndex = 9;
            this.comboBoxRoutingMetric.SelectedIndexChanged += new System.EventHandler(this.comboBoxRoutingMetric_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(240, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Routing Metric";
            // 
            // textBox_Distance
            // 
            this.textBox_Distance.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_Distance.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_Distance.Location = new System.Drawing.Point(620, 28);
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
            this.label7.Location = new System.Drawing.Point(565, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Distance";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(683, 35);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "ly";
            // 
            // RouteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxRoutingMetric);
            this.Controls.Add(this.textBoxCurrent);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBox_Distance);
            this.Controls.Add(this.textBox_ToZ);
            this.Controls.Add(this.textBox_ToY);
            this.Controls.Add(this.textBox_ToX);
            this.Controls.Add(this.textBox_FromZ);
            this.Controls.Add(this.textBox_FromY);
            this.Controls.Add(this.textBox_FromX);
            this.Controls.Add(this.textBox_From);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_Route);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_To);
            this.Controls.Add(this.textBox_Range);
            this.Name = "RouteControl";
            this.Size = new System.Drawing.Size(897, 429);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        internal System.Windows.Forms.TextBox textBox_From;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_Route;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox textBox_To;
        internal System.Windows.Forms.TextBox textBoxCurrent;
        private System.Windows.Forms.Label label5;
        internal System.Windows.Forms.TextBox textBox_Range;
        internal System.Windows.Forms.TextBox textBox_FromX;
        internal System.Windows.Forms.TextBox textBox_FromY;
        internal System.Windows.Forms.TextBox textBox_FromZ;
        internal System.Windows.Forms.TextBox textBox_ToX;
        internal System.Windows.Forms.TextBox textBox_ToY;
        internal System.Windows.Forms.TextBox textBox_ToZ;
        private System.Windows.Forms.ComboBox comboBoxRoutingMetric;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TextBox textBox_Distance;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
    }
}
