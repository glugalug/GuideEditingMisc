namespace LabeledValueControl
{
    partial class LabeledValue
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
            this.LabelLabel = new System.Windows.Forms.Label();
            this.ValueLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LabelLabel
            // 
            this.LabelLabel.AutoSize = true;
            this.LabelLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LabelLabel.Location = new System.Drawing.Point(0, 0);
            this.LabelLabel.Name = "LabelLabel";
            this.LabelLabel.Size = new System.Drawing.Size(36, 13);
            this.LabelLabel.TabIndex = 0;
            this.LabelLabel.Text = "Label:";
            // 
            // ValueLabel
            // 
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ValueLabel.Location = new System.Drawing.Point(245, 0);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(0, 13);
            this.ValueLabel.TabIndex = 1;
            // 
            // LabeledValue
            // 
            this.Controls.Add(this.ValueLabel);
            this.Controls.Add(this.LabelLabel);
            this.Name = "LabeledValue";
            this.Size = new System.Drawing.Size(245, 19);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelLabel;
        private System.Windows.Forms.Label ValueLabel;

    }
}
