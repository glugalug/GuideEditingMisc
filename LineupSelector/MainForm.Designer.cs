namespace LineupSelector
{
    partial class MainForm
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
            this.MergedLineupComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.WMILineupComboBox = new System.Windows.Forms.ComboBox();
            this.SyncOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.DebugTextBox = new System.Windows.Forms.TextBox();
            this.SyncButton = new System.Windows.Forms.Button();
            this.ExtraChannelOptionsComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ExistingChannelOptionsComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.MissingChannelOptionsComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ScannedLineupComboBox = new System.Windows.Forms.ComboBox();
            this.SyncOptionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Merged lineup used by your guide:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MergedLineupComboBox
            // 
            this.MergedLineupComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MergedLineupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MergedLineupComboBox.FormattingEnabled = true;
            this.MergedLineupComboBox.Location = new System.Drawing.Point(178, 6);
            this.MergedLineupComboBox.Name = "MergedLineupComboBox";
            this.MergedLineupComboBox.Size = new System.Drawing.Size(513, 21);
            this.MergedLineupComboBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "WMI Lineup to sync to:";
            // 
            // WMILineupComboBox
            // 
            this.WMILineupComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.WMILineupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WMILineupComboBox.FormattingEnabled = true;
            this.WMILineupComboBox.Location = new System.Drawing.Point(178, 40);
            this.WMILineupComboBox.Name = "WMILineupComboBox";
            this.WMILineupComboBox.Size = new System.Drawing.Size(513, 21);
            this.WMILineupComboBox.TabIndex = 3;
            // 
            // SyncOptionsGroupBox
            // 
            this.SyncOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SyncOptionsGroupBox.Controls.Add(this.DebugTextBox);
            this.SyncOptionsGroupBox.Controls.Add(this.SyncButton);
            this.SyncOptionsGroupBox.Controls.Add(this.ExtraChannelOptionsComboBox);
            this.SyncOptionsGroupBox.Controls.Add(this.label5);
            this.SyncOptionsGroupBox.Controls.Add(this.ExistingChannelOptionsComboBox);
            this.SyncOptionsGroupBox.Controls.Add(this.label4);
            this.SyncOptionsGroupBox.Controls.Add(this.MissingChannelOptionsComboBox);
            this.SyncOptionsGroupBox.Controls.Add(this.label3);
            this.SyncOptionsGroupBox.Location = new System.Drawing.Point(6, 106);
            this.SyncOptionsGroupBox.Name = "SyncOptionsGroupBox";
            this.SyncOptionsGroupBox.Size = new System.Drawing.Size(685, 325);
            this.SyncOptionsGroupBox.TabIndex = 4;
            this.SyncOptionsGroupBox.TabStop = false;
            this.SyncOptionsGroupBox.Text = "Sync Options";
            // 
            // DebugTextBox
            // 
            this.DebugTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DebugTextBox.Location = new System.Drawing.Point(9, 115);
            this.DebugTextBox.Multiline = true;
            this.DebugTextBox.Name = "DebugTextBox";
            this.DebugTextBox.ReadOnly = true;
            this.DebugTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DebugTextBox.Size = new System.Drawing.Size(670, 161);
            this.DebugTextBox.TabIndex = 7;
            // 
            // SyncButton
            // 
            this.SyncButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SyncButton.Location = new System.Drawing.Point(604, 296);
            this.SyncButton.Name = "SyncButton";
            this.SyncButton.Size = new System.Drawing.Size(75, 23);
            this.SyncButton.TabIndex = 6;
            this.SyncButton.Text = "Sync Now";
            this.SyncButton.UseVisualStyleBackColor = true;
            this.SyncButton.Click += new System.EventHandler(this.SyncButton_Click);
            // 
            // ExtraChannelOptionsComboBox
            // 
            this.ExtraChannelOptionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ExtraChannelOptionsComboBox.FormattingEnabled = true;
            this.ExtraChannelOptionsComboBox.Items.AddRange(new object[] {
            "Keep",
            "Remove"});
            this.ExtraChannelOptionsComboBox.Location = new System.Drawing.Point(250, 76);
            this.ExtraChannelOptionsComboBox.Name = "ExtraChannelOptionsComboBox";
            this.ExtraChannelOptionsComboBox.Size = new System.Drawing.Size(121, 21);
            this.ExtraChannelOptionsComboBox.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(235, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Existing Channel numbers not in the WMI lineup:";
            // 
            // ExistingChannelOptionsComboBox
            // 
            this.ExistingChannelOptionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ExistingChannelOptionsComboBox.FormattingEnabled = true;
            this.ExistingChannelOptionsComboBox.Items.AddRange(new object[] {
            "Do Nothing",
            "Replace selected listing with the one from WMI lineup"});
            this.ExistingChannelOptionsComboBox.Location = new System.Drawing.Point(250, 42);
            this.ExistingChannelOptionsComboBox.Name = "ExistingChannelOptionsComboBox";
            this.ExistingChannelOptionsComboBox.Size = new System.Drawing.Size(429, 21);
            this.ExistingChannelOptionsComboBox.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(238, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Channel numbers already existing in both lineups:";
            // 
            // MissingChannelOptionsComboBox
            // 
            this.MissingChannelOptionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MissingChannelOptionsComboBox.FormattingEnabled = true;
            this.MissingChannelOptionsComboBox.Items.AddRange(new object[] {
            "Do Nothing",
            "Add new channels using listing from WMI lineup."});
            this.MissingChannelOptionsComboBox.Location = new System.Drawing.Point(337, 13);
            this.MissingChannelOptionsComboBox.Name = "MissingChannelOptionsComboBox";
            this.MissingChannelOptionsComboBox.Size = new System.Drawing.Size(342, 21);
            this.MissingChannelOptionsComboBox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(308, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Channels in the WMI lineup that are missing from merged lineup:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(285, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Scanned Lineup to use tuners from when adding channels:";
            // 
            // ScannedLineupComboBox
            // 
            this.ScannedLineupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScannedLineupComboBox.FormattingEnabled = true;
            this.ScannedLineupComboBox.Location = new System.Drawing.Point(294, 73);
            this.ScannedLineupComboBox.Name = "ScannedLineupComboBox";
            this.ScannedLineupComboBox.Size = new System.Drawing.Size(397, 21);
            this.ScannedLineupComboBox.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 443);
            this.Controls.Add(this.ScannedLineupComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SyncOptionsGroupBox);
            this.Controls.Add(this.WMILineupComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MergedLineupComboBox);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Lineup Selector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.SyncOptionsGroupBox.ResumeLayout(false);
            this.SyncOptionsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox MergedLineupComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox WMILineupComboBox;
        private System.Windows.Forms.GroupBox SyncOptionsGroupBox;
        private System.Windows.Forms.ComboBox MissingChannelOptionsComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ExistingChannelOptionsComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button SyncButton;
        private System.Windows.Forms.ComboBox ExtraChannelOptionsComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox DebugTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ScannedLineupComboBox;
    }
}

