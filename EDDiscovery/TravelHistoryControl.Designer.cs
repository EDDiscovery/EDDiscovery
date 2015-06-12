namespace EDDiscovery
{
    partial class TravelHistoryControl
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            this.richTextBox_History = new System.Windows.Forms.RichTextBox();
            this.button_RefreshHistory = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBoxHistoryWindow = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ColumnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.buttonMap = new System.Windows.Forms.Button();
            this.textBoxSystem = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.richTextBoxNote = new System.Windows.Forms.RichTextBox();
            this.textBoxDistText = new System.Windows.Forms.TextBox();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.textBoxDistance = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPrevSystem = new System.Windows.Forms.TextBox();
            this.label_Z = new System.Windows.Forms.Label();
            this.textBoxZ = new System.Windows.Forms.TextBox();
            this.labelDistEnter = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxCmdrName = new System.Windows.Forms.TextBox();
            this.buttonSync = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxVisits = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox_History
            // 
            this.richTextBox_History.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_History.Location = new System.Drawing.Point(21, 3);
            this.richTextBox_History.Name = "richTextBox_History";
            this.richTextBox_History.Size = new System.Drawing.Size(499, 70);
            this.richTextBox_History.TabIndex = 6;
            this.richTextBox_History.Text = "";
            // 
            // button_RefreshHistory
            // 
            this.button_RefreshHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_RefreshHistory.Location = new System.Drawing.Point(437, 77);
            this.button_RefreshHistory.Name = "button_RefreshHistory";
            this.button_RefreshHistory.Size = new System.Drawing.Size(83, 23);
            this.button_RefreshHistory.TabIndex = 5;
            this.button_RefreshHistory.Text = "Refresh";
            this.button_RefreshHistory.UseVisualStyleBackColor = true;
            this.button_RefreshHistory.Click += new System.EventHandler(this.button_RefreshHistory_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Travel history";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTime,
            this.ColumnSystem,
            this.ColumnDistance,
            this.ColumnNote});
            this.dataGridView1.Location = new System.Drawing.Point(21, 106);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(499, 471);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_ColumnHeaderMouseClick);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // ColumnTime
            // 
            this.ColumnTime.HeaderText = "Time";
            this.ColumnTime.Name = "ColumnTime";
            this.ColumnTime.ReadOnly = true;
            this.ColumnTime.Width = 140;
            // 
            // ColumnSystem
            // 
            this.ColumnSystem.HeaderText = "System";
            this.ColumnSystem.Name = "ColumnSystem";
            this.ColumnSystem.ReadOnly = true;
            this.ColumnSystem.Width = 200;
            // 
            // ColumnDistance
            // 
            this.ColumnDistance.HeaderText = "Distance";
            this.ColumnDistance.Name = "ColumnDistance";
            this.ColumnDistance.ReadOnly = true;
            this.ColumnDistance.Width = 70;
            // 
            // ColumnNote
            // 
            this.ColumnNote.HeaderText = "Note";
            this.ColumnNote.Name = "ColumnNote";
            this.ColumnNote.ReadOnly = true;
            this.ColumnNote.Width = 250;
            // 
            // comboBoxHistoryWindow
            // 
            this.comboBoxHistoryWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxHistoryWindow.FormattingEnabled = true;
            this.comboBoxHistoryWindow.Items.AddRange(new object[] {
            "24 hours",
            "Week",
            "Month",
            "All"});
            this.comboBoxHistoryWindow.Location = new System.Drawing.Point(242, 78);
            this.comboBoxHistoryWindow.Name = "comboBoxHistoryWindow";
            this.comboBoxHistoryWindow.Size = new System.Drawing.Size(121, 21);
            this.comboBoxHistoryWindow.TabIndex = 9;
            this.comboBoxHistoryWindow.SelectedIndexChanged += new System.EventHandler(this.comboBoxHistoryWindow_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Show history for last";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnName,
            this.columnDist});
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup1.Name = "Name";
            this.listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
            this.listView1.Location = new System.Drawing.Point(528, 360);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(267, 218);
            this.listView1.TabIndex = 11;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // ColumnName
            // 
            this.ColumnName.Text = "Name";
            this.ColumnName.Width = 200;
            // 
            // columnDist
            // 
            this.columnDist.Text = "Dist";
            this.columnDist.Width = 90;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(527, 344);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(206, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Closest systems  (from last known position)";
            // 
            // buttonMap
            // 
            this.buttonMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMap.Location = new System.Drawing.Point(716, 76);
            this.buttonMap.Name = "buttonMap";
            this.buttonMap.Size = new System.Drawing.Size(83, 23);
            this.buttonMap.TabIndex = 14;
            this.buttonMap.Text = "3D star map";
            this.buttonMap.UseVisualStyleBackColor = true;
            this.buttonMap.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxSystem
            // 
            this.textBoxSystem.Location = new System.Drawing.Point(50, 6);
            this.textBoxSystem.Name = "textBoxSystem";
            this.textBoxSystem.ReadOnly = true;
            this.textBoxSystem.Size = new System.Drawing.Size(203, 20);
            this.textBoxSystem.TabIndex = 15;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.textBoxVisits);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.richTextBoxNote);
            this.panel1.Controls.Add(this.textBoxDistText);
            this.panel1.Controls.Add(this.buttonUpdate);
            this.panel1.Controls.Add(this.textBoxDistance);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.textBoxPrevSystem);
            this.panel1.Controls.Add(this.label_Z);
            this.panel1.Controls.Add(this.textBoxZ);
            this.panel1.Controls.Add(this.labelDistEnter);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.textBoxY);
            this.panel1.Controls.Add(this.textBoxX);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBoxSystem);
            this.panel1.Location = new System.Drawing.Point(524, 106);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(270, 235);
            this.panel1.TabIndex = 16;
            // 
            // richTextBoxNote
            // 
            this.richTextBoxNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxNote.Location = new System.Drawing.Point(6, 176);
            this.richTextBoxNote.Name = "richTextBoxNote";
            this.richTextBoxNote.Size = new System.Drawing.Size(261, 56);
            this.richTextBoxNote.TabIndex = 27;
            this.richTextBoxNote.Text = "";
            this.richTextBoxNote.Leave += new System.EventHandler(this.richTextBoxNote_Leave);
            // 
            // textBoxDistText
            // 
            this.textBoxDistText.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxDistText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDistText.ForeColor = System.Drawing.Color.Red;
            this.textBoxDistText.Location = new System.Drawing.Point(4, 128);
            this.textBoxDistText.Multiline = true;
            this.textBoxDistText.Name = "textBoxDistText";
            this.textBoxDistText.ReadOnly = true;
            this.textBoxDistText.Size = new System.Drawing.Size(263, 28);
            this.textBoxDistText.TabIndex = 26;
            this.textBoxDistText.Text = "Important!!  Use galaxy map to get distance with 2 decimals. Ex 17.44     ";
            this.textBoxDistText.TextChanged += new System.EventHandler(this.textBox7_TextChanged);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(6, 103);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(69, 22);
            this.buttonUpdate.TabIndex = 24;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // textBoxDistance
            // 
            this.textBoxDistance.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxDistance.Location = new System.Drawing.Point(81, 105);
            this.textBoxDistance.Name = "textBoxDistance";
            this.textBoxDistance.Size = new System.Drawing.Size(186, 20);
            this.textBoxDistance.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Distance from";
            // 
            // textBoxPrevSystem
            // 
            this.textBoxPrevSystem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPrevSystem.Location = new System.Drawing.Point(81, 88);
            this.textBoxPrevSystem.Name = "textBoxPrevSystem";
            this.textBoxPrevSystem.ReadOnly = true;
            this.textBoxPrevSystem.Size = new System.Drawing.Size(186, 13);
            this.textBoxPrevSystem.TabIndex = 21;
            // 
            // label_Z
            // 
            this.label_Z.AutoSize = true;
            this.label_Z.Location = new System.Drawing.Point(3, 64);
            this.label_Z.Name = "label_Z";
            this.label_Z.Size = new System.Drawing.Size(14, 13);
            this.label_Z.TabIndex = 20;
            this.label_Z.Text = "Z";
            // 
            // textBoxZ
            // 
            this.textBoxZ.Location = new System.Drawing.Point(50, 61);
            this.textBoxZ.Name = "textBoxZ";
            this.textBoxZ.ReadOnly = true;
            this.textBoxZ.Size = new System.Drawing.Size(71, 20);
            this.textBoxZ.TabIndex = 19;
            // 
            // labelDistEnter
            // 
            this.labelDistEnter.AutoSize = true;
            this.labelDistEnter.ForeColor = System.Drawing.Color.Black;
            this.labelDistEnter.Location = new System.Drawing.Point(3, 46);
            this.labelDistEnter.Name = "labelDistEnter";
            this.labelDistEnter.Size = new System.Drawing.Size(14, 13);
            this.labelDistEnter.TabIndex = 18;
            this.labelDistEnter.Text = "Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "X";
            // 
            // textBoxY
            // 
            this.textBoxY.Location = new System.Drawing.Point(50, 43);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.ReadOnly = true;
            this.textBoxY.Size = new System.Drawing.Size(71, 20);
            this.textBoxY.TabIndex = 17;
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(50, 25);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.ReadOnly = true;
            this.textBoxX.Size = new System.Drawing.Size(71, 20);
            this.textBoxX.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "System";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(526, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 20);
            this.label6.TabIndex = 17;
            this.label6.Text = "Commander";
            // 
            // textBoxCmdrName
            // 
            this.textBoxCmdrName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCmdrName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCmdrName.Location = new System.Drawing.Point(631, 2);
            this.textBoxCmdrName.Name = "textBoxCmdrName";
            this.textBoxCmdrName.Size = new System.Drawing.Size(167, 26);
            this.textBoxCmdrName.TabIndex = 18;
            this.textBoxCmdrName.Leave += new System.EventHandler(this.textBoxCmdrName_Leave);
            // 
            // buttonSync
            // 
            this.buttonSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSync.Location = new System.Drawing.Point(631, 34);
            this.buttonSync.Name = "buttonSync";
            this.buttonSync.Size = new System.Drawing.Size(164, 23);
            this.buttonSync.TabIndex = 19;
            this.buttonSync.Text = "Send Distances to EDSC";
            this.buttonSync.UseVisualStyleBackColor = true;
            this.buttonSync.Click += new System.EventHandler(this.buttonSync_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Note";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(145, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Visits";
            // 
            // textBoxVisits
            // 
            this.textBoxVisits.Location = new System.Drawing.Point(182, 25);
            this.textBoxVisits.Name = "textBoxVisits";
            this.textBoxVisits.ReadOnly = true;
            this.textBoxVisits.Size = new System.Drawing.Size(71, 20);
            this.textBoxVisits.TabIndex = 29;
            // 
            // TravelHistoryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSync);
            this.Controls.Add(this.textBoxCmdrName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonMap);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxHistoryWindow);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox_History);
            this.Controls.Add(this.button_RefreshHistory);
            this.Name = "TravelHistoryControl";
            this.Size = new System.Drawing.Size(820, 586);
            this.Load += new System.EventHandler(this.TravelHistoryControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_RefreshHistory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox comboBoxHistoryWindow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ColumnName;
        private System.Windows.Forms.ColumnHeader columnDist;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonMap;
        internal System.Windows.Forms.RichTextBox richTextBox_History;
        private System.Windows.Forms.TextBox textBoxSystem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxDistance;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPrevSystem;
        private System.Windows.Forms.Label label_Z;
        private System.Windows.Forms.TextBox textBoxZ;
        private System.Windows.Forms.Label labelDistEnter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.TextBox textBoxDistText;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TextBox textBoxCmdrName;
        private System.Windows.Forms.RichTextBox richTextBoxNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNote;
        private System.Windows.Forms.Button buttonSync;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxVisits;
        private System.Windows.Forms.Label label8;
    }
}
