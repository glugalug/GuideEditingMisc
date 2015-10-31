namespace ErrorReporter
{
    partial class ErrorReportingForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.GeneratedErrorDescriptionInput = new System.Windows.Forms.RichTextBox();
            this.SendReportButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "An error has occured.  Description of the error is as follows:";
            // 
            // GeneratedErrorDescriptionInput
            // 
            this.GeneratedErrorDescriptionInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GeneratedErrorDescriptionInput.Location = new System.Drawing.Point(12, 33);
            this.GeneratedErrorDescriptionInput.Name = "GeneratedErrorDescriptionInput";
            this.GeneratedErrorDescriptionInput.ReadOnly = true;
            this.GeneratedErrorDescriptionInput.Size = new System.Drawing.Size(882, 357);
            this.GeneratedErrorDescriptionInput.TabIndex = 1;
            this.GeneratedErrorDescriptionInput.Text = "";
            // 
            // SendReportButton
            // 
            this.SendReportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SendReportButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SendReportButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendReportButton.Location = new System.Drawing.Point(780, 397);
            this.SendReportButton.Name = "SendReportButton";
            this.SendReportButton.Size = new System.Drawing.Size(114, 37);
            this.SendReportButton.TabIndex = 2;
            this.SendReportButton.Text = "OK";
            this.SendReportButton.UseVisualStyleBackColor = true;
            this.SendReportButton.Click += new System.EventHandler(this.SendReportButton_Click);
            // 
            // ErrorReportingForm
            // 
            this.AcceptButton = this.SendReportButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 438);
            this.Controls.Add(this.SendReportButton);
            this.Controls.Add(this.GeneratedErrorDescriptionInput);
            this.Controls.Add(this.label1);
            this.Name = "ErrorReportingForm";
            this.Text = "Error Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox GeneratedErrorDescriptionInput;
        private System.Windows.Forms.Button SendReportButton;
    }
}