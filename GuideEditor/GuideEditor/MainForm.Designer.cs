namespace GuideEditor
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AddChannelButton = new System.Windows.Forms.Button();
            this.ListingComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.RemoveChannelButton = new System.Windows.Forms.Button();
            this.ChannelOriginalNumberLabel = new LabeledValueControl.LabeledValue();
            this.ChannelNumberLabel = new LabeledValueControl.LabeledValue();
            this.CallsignEdit = new LabeledValueControl.LabeledEditBox();
            this.ChannelUpdateButton = new System.Windows.Forms.Button();
            this.UserChannelsListBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ModulationTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LogicalChannelInput = new LabeledValueControl.LabeledNumberInput();
            this.SubChannelLabel = new LabeledValueControl.LabeledValue();
            this.UpdateTuningInfoButton = new System.Windows.Forms.Button();
            this.PhysicalNumberInput = new LabeledValueControl.LabeledNumberInput();
            this.RemoveTuningInfoButton = new System.Windows.Forms.Button();
            this.ChannelTuningInfosListBox = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.DeviceGroupLabel = new LabeledValueControl.LabeledValue();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.WmisDevicesListBox = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ScanDevicesListBox = new System.Windows.Forms.ListBox();
            this.AlternateLineupTypesLabel = new LabeledValueControl.LabeledValue();
            this.LineupTypesLabel = new LabeledValueControl.LabeledValue();
            this.LineupsComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.AddChannelButton);
            this.groupBox1.Controls.Add(this.ListingComboBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.RemoveChannelButton);
            this.groupBox1.Controls.Add(this.ChannelOriginalNumberLabel);
            this.groupBox1.Controls.Add(this.ChannelNumberLabel);
            this.groupBox1.Controls.Add(this.CallsignEdit);
            this.groupBox1.Controls.Add(this.ChannelUpdateButton);
            this.groupBox1.Controls.Add(this.UserChannelsListBox);
            this.groupBox1.Location = new System.Drawing.Point(0, 165);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(314, 393);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Added Channels";
            // 
            // AddChannelButton
            // 
            this.AddChannelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddChannelButton.Location = new System.Drawing.Point(3, 361);
            this.AddChannelButton.Name = "AddChannelButton";
            this.AddChannelButton.Size = new System.Drawing.Size(305, 23);
            this.AddChannelButton.TabIndex = 14;
            this.AddChannelButton.Text = "Add New Channel to Lineup";
            this.AddChannelButton.UseVisualStyleBackColor = true;
            this.AddChannelButton.Click += new System.EventHandler(this.AddChannelButton_Click);
            // 
            // ListingComboBox
            // 
            this.ListingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ListingComboBox.FormattingEnabled = true;
            this.ListingComboBox.Location = new System.Drawing.Point(139, 115);
            this.ListingComboBox.Name = "ListingComboBox";
            this.ListingComboBox.Size = new System.Drawing.Size(169, 21);
            this.ListingComboBox.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(136, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Listing:";
            // 
            // RemoveChannelButton
            // 
            this.RemoveChannelButton.Location = new System.Drawing.Point(3, 330);
            this.RemoveChannelButton.Name = "RemoveChannelButton";
            this.RemoveChannelButton.Size = new System.Drawing.Size(127, 25);
            this.RemoveChannelButton.TabIndex = 11;
            this.RemoveChannelButton.Text = "Remove Channel";
            this.RemoveChannelButton.UseVisualStyleBackColor = true;
            this.RemoveChannelButton.Click += new System.EventHandler(this.RemoveChannelButton_Click);
            // 
            // ChannelOriginalNumberLabel
            // 
            this.ChannelOriginalNumberLabel.LabelCaption = "Original Number:";
            this.ChannelOriginalNumberLabel.Location = new System.Drawing.Point(139, 77);
            this.ChannelOriginalNumberLabel.Name = "ChannelOriginalNumberLabel";
            this.ChannelOriginalNumberLabel.Size = new System.Drawing.Size(169, 19);
            this.ChannelOriginalNumberLabel.TabIndex = 10;
            // 
            // ChannelNumberLabel
            // 
            this.ChannelNumberLabel.LabelCaption = "Number:";
            this.ChannelNumberLabel.Location = new System.Drawing.Point(139, 52);
            this.ChannelNumberLabel.Name = "ChannelNumberLabel";
            this.ChannelNumberLabel.Size = new System.Drawing.Size(169, 19);
            this.ChannelNumberLabel.TabIndex = 9;
            this.ChannelNumberLabel.ValueCaption = "0";
            // 
            // CallsignEdit
            // 
            this.CallsignEdit.LabelCaption = "Callsign:";
            this.CallsignEdit.Location = new System.Drawing.Point(139, 19);
            this.CallsignEdit.Name = "CallsignEdit";
            this.CallsignEdit.Size = new System.Drawing.Size(169, 27);
            this.CallsignEdit.TabIndex = 7;
            // 
            // ChannelUpdateButton
            // 
            this.ChannelUpdateButton.Location = new System.Drawing.Point(139, 142);
            this.ChannelUpdateButton.Name = "ChannelUpdateButton";
            this.ChannelUpdateButton.Size = new System.Drawing.Size(169, 28);
            this.ChannelUpdateButton.TabIndex = 8;
            this.ChannelUpdateButton.Text = "Update channel properties";
            this.ChannelUpdateButton.UseVisualStyleBackColor = true;
            this.ChannelUpdateButton.Click += new System.EventHandler(this.ChannelUpdateButton_Click_1);
            // 
            // UserChannelsListBox
            // 
            this.UserChannelsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.UserChannelsListBox.FormattingEnabled = true;
            this.UserChannelsListBox.Location = new System.Drawing.Point(3, 16);
            this.UserChannelsListBox.Name = "UserChannelsListBox";
            this.UserChannelsListBox.Size = new System.Drawing.Size(127, 303);
            this.UserChannelsListBox.TabIndex = 0;
            this.UserChannelsListBox.SelectedIndexChanged += new System.EventHandler(this.UserChannelsListBox_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.ModulationTypeComboBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.LogicalChannelInput);
            this.groupBox2.Controls.Add(this.SubChannelLabel);
            this.groupBox2.Controls.Add(this.UpdateTuningInfoButton);
            this.groupBox2.Controls.Add(this.PhysicalNumberInput);
            this.groupBox2.Controls.Add(this.RemoveTuningInfoButton);
            this.groupBox2.Controls.Add(this.ChannelTuningInfosListBox);
            this.groupBox2.Location = new System.Drawing.Point(320, 165);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(380, 393);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tuning Infos";
            // 
            // ModulationTypeComboBox
            // 
            this.ModulationTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModulationTypeComboBox.FormattingEnabled = true;
            this.ModulationTypeComboBox.Location = new System.Drawing.Point(235, 91);
            this.ModulationTypeComboBox.Name = "ModulationTypeComboBox";
            this.ModulationTypeComboBox.Size = new System.Drawing.Size(139, 21);
            this.ModulationTypeComboBox.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Modulation:";
            // 
            // LogicalChannelInput
            // 
            this.LogicalChannelInput.LabelCaption = "Logical Channel #";
            this.LogicalChannelInput.Location = new System.Drawing.Point(167, 16);
            this.LogicalChannelInput.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.LogicalChannelInput.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.LogicalChannelInput.Name = "LogicalChannelInput";
            this.LogicalChannelInput.NumberValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.LogicalChannelInput.Size = new System.Drawing.Size(207, 28);
            this.LogicalChannelInput.TabIndex = 6;
            // 
            // SubChannelLabel
            // 
            this.SubChannelLabel.LabelCaption = "SubNumber (program ID):";
            this.SubChannelLabel.Location = new System.Drawing.Point(167, 77);
            this.SubChannelLabel.Name = "SubChannelLabel";
            this.SubChannelLabel.Size = new System.Drawing.Size(207, 19);
            this.SubChannelLabel.TabIndex = 5;
            // 
            // UpdateTuningInfoButton
            // 
            this.UpdateTuningInfoButton.Location = new System.Drawing.Point(167, 361);
            this.UpdateTuningInfoButton.Name = "UpdateTuningInfoButton";
            this.UpdateTuningInfoButton.Size = new System.Drawing.Size(207, 23);
            this.UpdateTuningInfoButton.TabIndex = 4;
            this.UpdateTuningInfoButton.Text = "Update TuningInfo Properties";
            this.UpdateTuningInfoButton.UseVisualStyleBackColor = true;
            this.UpdateTuningInfoButton.Click += new System.EventHandler(this.UpdateTuningInfoButton_Click);
            // 
            // PhysicalNumberInput
            // 
            this.PhysicalNumberInput.LabelCaption = "Physical Channel #:";
            this.PhysicalNumberInput.Location = new System.Drawing.Point(167, 43);
            this.PhysicalNumberInput.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.PhysicalNumberInput.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.PhysicalNumberInput.Name = "PhysicalNumberInput";
            this.PhysicalNumberInput.NumberValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.PhysicalNumberInput.Size = new System.Drawing.Size(207, 28);
            this.PhysicalNumberInput.TabIndex = 2;
            // 
            // RemoveTuningInfoButton
            // 
            this.RemoveTuningInfoButton.Location = new System.Drawing.Point(6, 361);
            this.RemoveTuningInfoButton.Name = "RemoveTuningInfoButton";
            this.RemoveTuningInfoButton.Size = new System.Drawing.Size(155, 23);
            this.RemoveTuningInfoButton.TabIndex = 1;
            this.RemoveTuningInfoButton.Text = "Remove selected tuner";
            this.RemoveTuningInfoButton.UseVisualStyleBackColor = true;
            this.RemoveTuningInfoButton.Click += new System.EventHandler(this.RemoveTuningInfoClick);
            // 
            // ChannelTuningInfosListBox
            // 
            this.ChannelTuningInfosListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.ChannelTuningInfosListBox.FormattingEnabled = true;
            this.ChannelTuningInfosListBox.Location = new System.Drawing.Point(6, 19);
            this.ChannelTuningInfosListBox.Name = "ChannelTuningInfosListBox";
            this.ChannelTuningInfosListBox.Size = new System.Drawing.Size(155, 329);
            this.ChannelTuningInfosListBox.TabIndex = 0;
            this.ChannelTuningInfosListBox.SelectedIndexChanged += new System.EventHandler(this.ChannelTuningInfosListBox_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(861, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.DeviceGroupLabel);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.AlternateLineupTypesLabel);
            this.groupBox3.Controls.Add(this.LineupTypesLabel);
            this.groupBox3.Controls.Add(this.LineupsComboBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 24);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(861, 135);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Lineup Info";
            // 
            // DeviceGroupLabel
            // 
            this.DeviceGroupLabel.LabelCaption = "Device group:";
            this.DeviceGroupLabel.Location = new System.Drawing.Point(9, 90);
            this.DeviceGroupLabel.Name = "DeviceGroupLabel";
            this.DeviceGroupLabel.Size = new System.Drawing.Size(305, 19);
            this.DeviceGroupLabel.TabIndex = 6;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.WmisDevicesListBox);
            this.groupBox5.Location = new System.Drawing.Point(594, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(261, 116);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "WmisDevices";
            // 
            // WmisDevicesListBox
            // 
            this.WmisDevicesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WmisDevicesListBox.FormattingEnabled = true;
            this.WmisDevicesListBox.Location = new System.Drawing.Point(3, 16);
            this.WmisDevicesListBox.Name = "WmisDevicesListBox";
            this.WmisDevicesListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.WmisDevicesListBox.Size = new System.Drawing.Size(255, 95);
            this.WmisDevicesListBox.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.ScanDevicesListBox);
            this.groupBox4.Location = new System.Drawing.Point(320, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(271, 116);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Scan Devices";
            // 
            // ScanDevicesListBox
            // 
            this.ScanDevicesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScanDevicesListBox.FormattingEnabled = true;
            this.ScanDevicesListBox.Location = new System.Drawing.Point(3, 16);
            this.ScanDevicesListBox.Name = "ScanDevicesListBox";
            this.ScanDevicesListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.ScanDevicesListBox.Size = new System.Drawing.Size(265, 95);
            this.ScanDevicesListBox.TabIndex = 0;
            // 
            // AlternateLineupTypesLabel
            // 
            this.AlternateLineupTypesLabel.LabelCaption = "Alternate lineup types:";
            this.AlternateLineupTypesLabel.Location = new System.Drawing.Point(9, 65);
            this.AlternateLineupTypesLabel.Name = "AlternateLineupTypesLabel";
            this.AlternateLineupTypesLabel.Size = new System.Drawing.Size(305, 19);
            this.AlternateLineupTypesLabel.TabIndex = 3;
            // 
            // LineupTypesLabel
            // 
            this.LineupTypesLabel.LabelCaption = "Types:";
            this.LineupTypesLabel.Location = new System.Drawing.Point(9, 40);
            this.LineupTypesLabel.Name = "LineupTypesLabel";
            this.LineupTypesLabel.Size = new System.Drawing.Size(305, 19);
            this.LineupTypesLabel.TabIndex = 2;
            // 
            // LineupsComboBox
            // 
            this.LineupsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LineupsComboBox.FormattingEnabled = true;
            this.LineupsComboBox.Location = new System.Drawing.Point(113, 13);
            this.LineupsComboBox.Name = "LineupsComboBox";
            this.LineupsComboBox.Size = new System.Drawing.Size(201, 21);
            this.LineupsComboBox.TabIndex = 1;
            this.LineupsComboBox.SelectedIndexChanged += new System.EventHandler(this.LineupsComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select listing to edit:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 563);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Glugglug\'s Channel Editor";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox UserChannelsListBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox ChannelTuningInfosListBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox LineupsComboBox;
        private System.Windows.Forms.Label label1;
        private LabeledValueControl.LabeledValue LineupTypesLabel;
        private LabeledValueControl.LabeledValue AlternateLineupTypesLabel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox ScanDevicesListBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListBox WmisDevicesListBox;
        private LabeledValueControl.LabeledValue DeviceGroupLabel;
        private System.Windows.Forms.Button RemoveTuningInfoButton;
        private LabeledValueControl.LabeledValue ChannelOriginalNumberLabel;
        private LabeledValueControl.LabeledValue ChannelNumberLabel;
        private LabeledValueControl.LabeledEditBox CallsignEdit;
        private System.Windows.Forms.Button ChannelUpdateButton;
        private LabeledValueControl.LabeledNumberInput PhysicalNumberInput;
        private System.Windows.Forms.Button UpdateTuningInfoButton;
        private LabeledValueControl.LabeledValue SubChannelLabel;
        private LabeledValueControl.LabeledNumberInput LogicalChannelInput;
        private System.Windows.Forms.Button RemoveChannelButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ModulationTypeComboBox;
        private System.Windows.Forms.ComboBox ListingComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button AddChannelButton;
    }
}

