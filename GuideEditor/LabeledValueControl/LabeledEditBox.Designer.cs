namespace LabeledValueControl
{
    partial class LabeledEditBox
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
            this.TextInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LabelLabel
            // 
            this.LabelLabel.AutoSize = true;
            this.LabelLabel.Location = new System.Drawing.Point(3, 7);
            this.LabelLabel.Name = "LabelLabel";
            this.LabelLabel.Size = new System.Drawing.Size(42, 13);
            this.LabelLabel.TabIndex = 0;
            this.LabelLabel.Text = "Label:  ";
            this.LabelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextInput
            // 
            this.TextInput.Location = new System.Drawing.Point(41, 4);
            this.TextInput.Name = "TextInput";
            this.TextInput.Size = new System.Drawing.Size(418, 20);
            this.TextInput.TabIndex = 1;
            // 
            // LabeledEditBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TextInput);
            this.Controls.Add(this.LabelLabel);
            this.Name = "LabeledEditBox";
            this.Size = new System.Drawing.Size(224, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelLabel;
        private System.Windows.Forms.TextBox TextInput;
    }
}
