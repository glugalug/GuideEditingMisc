namespace GuideEditor2
{
    partial class SourceChannelManagementForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SubChannelsGrid = new System.Windows.Forms.DataGridView();
            this.ScannedSourcesTabControl = new System.Windows.Forms.TabControl();
            this.AvailableChannelsTabPage = new System.Windows.Forms.TabPage();
            this.AvailLineupGridView = new System.Windows.Forms.DataGridView();
            this.AvailNumberCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvailTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvailTunerCountCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvailCallsignCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddAsFirstCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.AddAsLastCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.AvailListingCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TuningParamsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SourceLineupCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CreateChannelTabPage = new System.Windows.Forms.TabPage();
            this.SourceChannelCreationControl = new GuideEditor2.ScannedChannelCreationControl();
            this.NumberCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TunerCountCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineupCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromoteCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DemoteCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ListingCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InheritListingCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.CallsignCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InheritCallsignCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.RemoveCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SubChannelsGrid)).BeginInit();
            this.ScannedSourcesTabControl.SuspendLayout();
            this.AvailableChannelsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AvailLineupGridView)).BeginInit();
            this.CreateChannelTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ScannedSourcesTabControl);
            this.splitContainer1.Size = new System.Drawing.Size(1033, 521);
            this.splitContainer1.SplitterDistance = 248;
            this.splitContainer1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SubChannelsGrid);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1033, 248);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source Channels used in this channel:";
            // 
            // SubChannelsGrid
            // 
            this.SubChannelsGrid.AllowUserToAddRows = false;
            this.SubChannelsGrid.AllowUserToDeleteRows = false;
            this.SubChannelsGrid.AllowUserToOrderColumns = true;
            this.SubChannelsGrid.AllowUserToResizeRows = false;
            this.SubChannelsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SubChannelsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SubChannelsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NumberCol,
            this.ChannelTypeCol,
            this.TunerCountCol,
            this.LineupCol,
            this.PromoteCol,
            this.DemoteCol,
            this.ListingCol,
            this.InheritListingCol,
            this.CallsignCol,
            this.InheritCallsignCol,
            this.RemoveCol});
            this.SubChannelsGrid.Location = new System.Drawing.Point(3, 16);
            this.SubChannelsGrid.Name = "SubChannelsGrid";
            this.SubChannelsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.SubChannelsGrid.Size = new System.Drawing.Size(1027, 226);
            this.SubChannelsGrid.TabIndex = 0;
            this.SubChannelsGrid.VirtualMode = true;
            this.SubChannelsGrid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.SubChannelsGrid_CellValueNeeded);
            this.SubChannelsGrid.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.SubChannelsGrid_CellValuePushed);
            this.SubChannelsGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SubChannelsGrid_CellContentClick);
            // 
            // ScannedSourcesTabControl
            // 
            this.ScannedSourcesTabControl.Controls.Add(this.AvailableChannelsTabPage);
            this.ScannedSourcesTabControl.Controls.Add(this.CreateChannelTabPage);
            this.ScannedSourcesTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScannedSourcesTabControl.Location = new System.Drawing.Point(0, 0);
            this.ScannedSourcesTabControl.Name = "ScannedSourcesTabControl";
            this.ScannedSourcesTabControl.SelectedIndex = 0;
            this.ScannedSourcesTabControl.Size = new System.Drawing.Size(1033, 269);
            this.ScannedSourcesTabControl.TabIndex = 0;
            // 
            // AvailableChannelsTabPage
            // 
            this.AvailableChannelsTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.AvailableChannelsTabPage.Controls.Add(this.AvailLineupGridView);
            this.AvailableChannelsTabPage.Controls.Add(this.SourceLineupCombo);
            this.AvailableChannelsTabPage.Controls.Add(this.label1);
            this.AvailableChannelsTabPage.Location = new System.Drawing.Point(4, 22);
            this.AvailableChannelsTabPage.Name = "AvailableChannelsTabPage";
            this.AvailableChannelsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.AvailableChannelsTabPage.Size = new System.Drawing.Size(1025, 243);
            this.AvailableChannelsTabPage.TabIndex = 0;
            this.AvailableChannelsTabPage.Text = "Available Channels";
            // 
            // AvailLineupGridView
            // 
            this.AvailLineupGridView.AllowUserToAddRows = false;
            this.AvailLineupGridView.AllowUserToDeleteRows = false;
            this.AvailLineupGridView.AllowUserToOrderColumns = true;
            this.AvailLineupGridView.AllowUserToResizeRows = false;
            this.AvailLineupGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AvailLineupGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AvailLineupGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AvailNumberCol,
            this.AvailTypeCol,
            this.AvailTunerCountCol,
            this.AvailCallsignCol,
            this.AddAsFirstCol,
            this.AddAsLastCol,
            this.AvailListingCol,
            this.TuningParamsCol});
            this.AvailLineupGridView.Location = new System.Drawing.Point(6, 33);
            this.AvailLineupGridView.Name = "AvailLineupGridView";
            this.AvailLineupGridView.ReadOnly = true;
            this.AvailLineupGridView.Size = new System.Drawing.Size(1016, 211);
            this.AvailLineupGridView.TabIndex = 2;
            this.AvailLineupGridView.VirtualMode = true;
            this.AvailLineupGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.AvailLineupGridView_ColumnHeaderMouseClick);
            this.AvailLineupGridView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.AvailLineupGridView_CellValueNeeded);
            this.AvailLineupGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AvailLineupGridView_CellContentClick);
            // 
            // AvailNumberCol
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.AvailNumberCol.DefaultCellStyle = dataGridViewCellStyle7;
            this.AvailNumberCol.HeaderText = "Number";
            this.AvailNumberCol.Name = "AvailNumberCol";
            this.AvailNumberCol.ReadOnly = true;
            // 
            // AvailTypeCol
            // 
            this.AvailTypeCol.HeaderText = "Channel Type";
            this.AvailTypeCol.Name = "AvailTypeCol";
            this.AvailTypeCol.ReadOnly = true;
            // 
            // AvailTunerCountCol
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.AvailTunerCountCol.DefaultCellStyle = dataGridViewCellStyle8;
            this.AvailTunerCountCol.HeaderText = "# of Tuners";
            this.AvailTunerCountCol.Name = "AvailTunerCountCol";
            this.AvailTunerCountCol.ReadOnly = true;
            // 
            // AvailCallsignCol
            // 
            this.AvailCallsignCol.HeaderText = "Callsign";
            this.AvailCallsignCol.Name = "AvailCallsignCol";
            this.AvailCallsignCol.ReadOnly = true;
            // 
            // AddAsFirstCol
            // 
            this.AddAsFirstCol.HeaderText = "Add as First Source";
            this.AddAsFirstCol.Name = "AddAsFirstCol";
            this.AddAsFirstCol.ReadOnly = true;
            this.AddAsFirstCol.Text = "Add as First";
            this.AddAsFirstCol.UseColumnTextForButtonValue = true;
            // 
            // AddAsLastCol
            // 
            this.AddAsLastCol.HeaderText = "Add as Last Source";
            this.AddAsLastCol.Name = "AddAsLastCol";
            this.AddAsLastCol.ReadOnly = true;
            this.AddAsLastCol.Text = "Add as Last";
            this.AddAsLastCol.UseColumnTextForButtonValue = true;
            // 
            // AvailListingCol
            // 
            this.AvailListingCol.HeaderText = "Listing";
            this.AvailListingCol.Name = "AvailListingCol";
            this.AvailListingCol.ReadOnly = true;
            this.AvailListingCol.Width = 250;
            // 
            // TuningParamsCol
            // 
            this.TuningParamsCol.HeaderText = "Tuning Params";
            this.TuningParamsCol.Name = "TuningParamsCol";
            this.TuningParamsCol.ReadOnly = true;
            // 
            // SourceLineupCombo
            // 
            this.SourceLineupCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SourceLineupCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SourceLineupCombo.FormattingEnabled = true;
            this.SourceLineupCombo.Location = new System.Drawing.Point(84, 6);
            this.SourceLineupCombo.Name = "SourceLineupCombo";
            this.SourceLineupCombo.Size = new System.Drawing.Size(935, 21);
            this.SourceLineupCombo.TabIndex = 1;
            this.SourceLineupCombo.SelectedIndexChanged += new System.EventHandler(this.SourceLineupCombo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source lineup:";
            // 
            // CreateChannelTabPage
            // 
            this.CreateChannelTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.CreateChannelTabPage.Controls.Add(this.SourceChannelCreationControl);
            this.CreateChannelTabPage.Location = new System.Drawing.Point(4, 22);
            this.CreateChannelTabPage.Name = "CreateChannelTabPage";
            this.CreateChannelTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.CreateChannelTabPage.Size = new System.Drawing.Size(1025, 243);
            this.CreateChannelTabPage.TabIndex = 1;
            this.CreateChannelTabPage.Text = "Create Source Channel";
            // 
            // SourceChannelCreationControl
            // 
            this.SourceChannelCreationControl.DestinationMergedChannel = null;
            this.SourceChannelCreationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourceChannelCreationControl.Location = new System.Drawing.Point(3, 3);
            this.SourceChannelCreationControl.Name = "SourceChannelCreationControl";
            this.SourceChannelCreationControl.Size = new System.Drawing.Size(1019, 237);
            this.SourceChannelCreationControl.TabIndex = 0;
            this.SourceChannelCreationControl.ChannelAdded += new System.EventHandler(this.SourceChannelCreationControl_ChannelAdded);
            // 
            // NumberCol
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.NumberCol.DefaultCellStyle = dataGridViewCellStyle5;
            this.NumberCol.HeaderText = "Number";
            this.NumberCol.Name = "NumberCol";
            this.NumberCol.ReadOnly = true;
            this.NumberCol.Width = 50;
            // 
            // ChannelTypeCol
            // 
            this.ChannelTypeCol.HeaderText = "Channel Type";
            this.ChannelTypeCol.Name = "ChannelTypeCol";
            this.ChannelTypeCol.ReadOnly = true;
            this.ChannelTypeCol.Width = 75;
            // 
            // TunerCountCol
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.TunerCountCol.DefaultCellStyle = dataGridViewCellStyle6;
            this.TunerCountCol.HeaderText = "# of tuners";
            this.TunerCountCol.Name = "TunerCountCol";
            this.TunerCountCol.ReadOnly = true;
            this.TunerCountCol.Width = 50;
            // 
            // LineupCol
            // 
            this.LineupCol.HeaderText = "Lineup";
            this.LineupCol.Name = "LineupCol";
            this.LineupCol.ReadOnly = true;
            this.LineupCol.Width = 200;
            // 
            // PromoteCol
            // 
            this.PromoteCol.HeaderText = "Promote";
            this.PromoteCol.Name = "PromoteCol";
            this.PromoteCol.ReadOnly = true;
            this.PromoteCol.Text = "↑ Promote ↑";
            this.PromoteCol.UseColumnTextForButtonValue = true;
            this.PromoteCol.Width = 50;
            // 
            // DemoteCol
            // 
            this.DemoteCol.HeaderText = "Demote";
            this.DemoteCol.Name = "DemoteCol";
            this.DemoteCol.ReadOnly = true;
            this.DemoteCol.Text = "↓ Demote ↓";
            this.DemoteCol.UseColumnTextForButtonValue = true;
            this.DemoteCol.Width = 50;
            // 
            // ListingCol
            // 
            this.ListingCol.HeaderText = "Listing";
            this.ListingCol.Name = "ListingCol";
            this.ListingCol.ReadOnly = true;
            // 
            // InheritListingCol
            // 
            this.InheritListingCol.HeaderText = "Inherit Listing";
            this.InheritListingCol.Name = "InheritListingCol";
            this.InheritListingCol.ReadOnly = true;
            this.InheritListingCol.Text = "Inherit";
            this.InheritListingCol.UseColumnTextForButtonValue = true;
            // 
            // CallsignCol
            // 
            this.CallsignCol.HeaderText = "Callsign";
            this.CallsignCol.Name = "CallsignCol";
            this.CallsignCol.Width = 75;
            // 
            // InheritCallsignCol
            // 
            this.InheritCallsignCol.HeaderText = "Inherit Callsign";
            this.InheritCallsignCol.Name = "InheritCallsignCol";
            this.InheritCallsignCol.ReadOnly = true;
            this.InheritCallsignCol.Text = "Inherit";
            this.InheritCallsignCol.UseColumnTextForButtonValue = true;
            // 
            // RemoveCol
            // 
            this.RemoveCol.HeaderText = "Remove";
            this.RemoveCol.Name = "RemoveCol";
            this.RemoveCol.ReadOnly = true;
            this.RemoveCol.Text = "Remove";
            this.RemoveCol.UseColumnTextForButtonValue = true;
            this.RemoveCol.Width = 50;
            // 
            // SourceChannelManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 521);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SourceChannelManagementForm";
            this.Text = "Source Channel Management";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SubChannelsGrid)).EndInit();
            this.ScannedSourcesTabControl.ResumeLayout(false);
            this.AvailableChannelsTabPage.ResumeLayout(false);
            this.AvailableChannelsTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AvailLineupGridView)).EndInit();
            this.CreateChannelTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView SubChannelsGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl ScannedSourcesTabControl;
        private System.Windows.Forms.TabPage AvailableChannelsTabPage;
        private System.Windows.Forms.TabPage CreateChannelTabPage;
        private System.Windows.Forms.ComboBox SourceLineupCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView AvailLineupGridView;
        private ScannedChannelCreationControl SourceChannelCreationControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailNumberCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailTypeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailTunerCountCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailCallsignCol;
        private System.Windows.Forms.DataGridViewButtonColumn AddAsFirstCol;
        private System.Windows.Forms.DataGridViewButtonColumn AddAsLastCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailListingCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TuningParamsCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumberCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelTypeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TunerCountCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineupCol;
        private System.Windows.Forms.DataGridViewButtonColumn PromoteCol;
        private System.Windows.Forms.DataGridViewButtonColumn DemoteCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ListingCol;
        private System.Windows.Forms.DataGridViewButtonColumn InheritListingCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CallsignCol;
        private System.Windows.Forms.DataGridViewButtonColumn InheritCallsignCol;
        private System.Windows.Forms.DataGridViewButtonColumn RemoveCol;

    }
}