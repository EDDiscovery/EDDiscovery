namespace UnitTest
{
    partial class UnitTestBindingsEditor
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
            this.bindingsEditor = new EliteDangerousCore.Bindings.BindingsEditor();
            this.SuspendLayout();
            // 
            // bindingsEditor
            // 
            this.bindingsEditor.ChangedBindings = null;
            this.bindingsEditor.ChangedDefault = null;
            this.bindingsEditor.DeviceInput = null;
            this.bindingsEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bindingsEditor.Location = new System.Drawing.Point(0, 0);
            this.bindingsEditor.Name = "bindingsEditor";
            this.bindingsEditor.Size = new System.Drawing.Size(1254, 737);
            this.bindingsEditor.TabIndex = 4;
            // 
            // UnitTestBindingsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bindingsEditor);
            this.Name = "UnitTestBindingsEditor";
            this.Size = new System.Drawing.Size(1254, 737);
            this.ResumeLayout(false);

        }

        #endregion

        private EliteDangerousCore.Bindings.BindingsEditor bindingsEditor;
    }
}
