namespace UnitTest
{
    partial class UnitTests
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
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.bindingsEditor = new EliteDangerousCore.BindingsEditor();
            this.panelTest = new System.Windows.Forms.Panel();
            this.panelTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Left;
            this.richTextBoxLog.Font = new System.Drawing.Font("Cascadia Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLog.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(1250, 74);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.Text = "";
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Location = new System.Drawing.Point(1397, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(80, 24);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // bindingsEditor
            // 
            this.bindingsEditor.ChangedBindings = null;
            this.bindingsEditor.ChangedDefault = null;
            this.bindingsEditor.DeviceInput = null;
            this.bindingsEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bindingsEditor.Location = new System.Drawing.Point(0, 74);
            this.bindingsEditor.Name = "bindingsEditor";
            this.bindingsEditor.Size = new System.Drawing.Size(1505, 919);
            this.bindingsEditor.TabIndex = 3;
            // 
            // panelTest
            // 
            this.panelTest.Controls.Add(this.richTextBoxLog);
            this.panelTest.Controls.Add(this.buttonStart);
            this.panelTest.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTest.Location = new System.Drawing.Point(0, 0);
            this.panelTest.Name = "panelTest";
            this.panelTest.Size = new System.Drawing.Size(1505, 74);
            this.panelTest.TabIndex = 4;
            // 
            // UnitTests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1505, 993);
            this.Controls.Add(this.bindingsEditor);
            this.Controls.Add(this.panelTest);
            this.Name = "UnitTests";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Unit Test";
            this.panelTest.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonStart;
        private EliteDangerousCore.BindingsEditor bindingsEditor;
        private System.Windows.Forms.Panel panelTest;
    }
}

