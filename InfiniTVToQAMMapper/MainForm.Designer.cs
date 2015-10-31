namespace InfiniTVToQAMMapper
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ChannelMapGrid = new System.Windows.Forms.DataGridView();
            this.NumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CallsignColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PhysicalColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OTAExistsCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ListingCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChangeListingCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ActionCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.RunCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.RunButton = new System.Windows.Forms.Button();
            this.NoActionButton = new System.Windows.Forms.Button();
            this.RecommendedActionButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChannelMapGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ChannelMapGrid);
            this.groupBox1.Location = new System.Drawing.Point(6, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(874, 548);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channel Map Channels";
            // 
            // ChannelMapGrid
            // 
            this.ChannelMapGrid.AllowUserToAddRows = false;
            this.ChannelMapGrid.AllowUserToDeleteRows = false;
            this.ChannelMapGrid.AllowUserToOrderColumns = true;
            this.ChannelMapGrid.AllowUserToResizeRows = false;
            this.ChannelMapGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.ChannelMapGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ChannelMapGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NumberColumn,
            this.CallsignColumn,
            this.PhysicalColumn,
            this.StatusCol,
            this.OTAExistsCol,
            this.ListingCol,
            this.ChangeListingCol,
            this.ActionCol,
            this.RunCol});
            this.ChannelMapGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChannelMapGrid.Location = new System.Drawing.Point(3, 16);
            this.ChannelMapGrid.MultiSelect = false;
            this.ChannelMapGrid.Name = "ChannelMapGrid";
            this.ChannelMapGrid.RowHeadersVisible = false;
            this.ChannelMapGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.ChannelMapGrid.Size = new System.Drawing.Size(868, 529);
            this.ChannelMapGrid.TabIndex = 0;
            this.ChannelMapGrid.VirtualMode = true;
            this.ChannelMapGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ChannelMapGrid_CellClick);
            this.ChannelMapGrid.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.ChannelMapGrid_CellToolTipTextNeeded);
            this.ChannelMapGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.ChannelMapGrid_CellValidating);
            this.ChannelMapGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.ChannelMapGrid_CellValueChanged);
            this.ChannelMapGrid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.ChannelMapGrid_CellValueNeeded);
            this.ChannelMapGrid.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ChannelMapGrid_ColumnHeaderMouseClick);
            this.ChannelMapGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.ChannelMapGrid_DataError);
            // 
            // NumberColumn
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.NumberColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.NumberColumn.Frozen = true;
            this.NumberColumn.HeaderText = "Channel #";
            this.NumberColumn.Name = "NumberColumn";
            this.NumberColumn.ReadOnly = true;
            this.NumberColumn.Width = 70;
            // 
            // CallsignColumn
            // 
            this.CallsignColumn.HeaderText = "Callsign";
            this.CallsignColumn.Name = "CallsignColumn";
            this.CallsignColumn.ReadOnly = true;
            // 
            // PhysicalColumn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.PhysicalColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.PhysicalColumn.HeaderText = "Physical Channel";
            this.PhysicalColumn.Name = "PhysicalColumn";
            this.PhysicalColumn.ReadOnly = true;
            this.PhysicalColumn.Width = 70;
            // 
            // StatusCol
            // 
            this.StatusCol.HeaderText = "Current Status";
            this.StatusCol.Name = "StatusCol";
            this.StatusCol.ReadOnly = true;
            // 
            // OTAExistsCol
            // 
            this.OTAExistsCol.HeaderText = "Exists As OTA";
            this.OTAExistsCol.Name = "OTAExistsCol";
            this.OTAExistsCol.ReadOnly = true;
            this.OTAExistsCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.OTAExistsCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.OTAExistsCol.Width = 70;
            // 
            // ListingCol
            // 
            this.ListingCol.HeaderText = "Listing";
            this.ListingCol.Name = "ListingCol";
            this.ListingCol.ReadOnly = true;
            this.ListingCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ChangeListingCol
            // 
            this.ChangeListingCol.HeaderText = "Change Listing";
            this.ChangeListingCol.Name = "ChangeListingCol";
            this.ChangeListingCol.Text = "Select Listing";
            this.ChangeListingCol.UseColumnTextForButtonValue = true;
            this.ChangeListingCol.Width = 85;
            // 
            // ActionCol
            // 
            this.ActionCol.HeaderText = "Action To Take";
            this.ActionCol.Name = "ActionCol";
            this.ActionCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ActionCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ActionCol.Width = 175;
            // 
            // RunCol
            // 
            this.RunCol.HeaderText = "Do Selected Action";
            this.RunCol.Name = "RunCol";
            this.RunCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.RunCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.RunCol.Text = "GO!";
            this.RunCol.UseColumnTextForButtonValue = true;
            this.RunCol.Width = 70;
            // 
            // RunButton
            // 
            this.RunButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RunButton.Location = new System.Drawing.Point(518, 565);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(365, 23);
            this.RunButton.TabIndex = 1;
            this.RunButton.Text = "Run Selected Actions on All Channels";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // NoActionButton
            // 
            this.NoActionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NoActionButton.Location = new System.Drawing.Point(6, 556);
            this.NoActionButton.Name = "NoActionButton";
            this.NoActionButton.Size = new System.Drawing.Size(114, 37);
            this.NoActionButton.TabIndex = 2;
            this.NoActionButton.Text = "Select No Action on All Channels";
            this.NoActionButton.UseVisualStyleBackColor = true;
            this.NoActionButton.Click += new System.EventHandler(this.NoActionButton_Click);
            // 
            // RecommendedActionButton
            // 
            this.RecommendedActionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RecommendedActionButton.Location = new System.Drawing.Point(122, 556);
            this.RecommendedActionButton.Name = "RecommendedActionButton";
            this.RecommendedActionButton.Size = new System.Drawing.Size(146, 37);
            this.RecommendedActionButton.TabIndex = 3;
            this.RecommendedActionButton.Text = "Select Recommended Action on All Channels";
            this.RecommendedActionButton.UseVisualStyleBackColor = true;
            this.RecommendedActionButton.Click += new System.EventHandler(this.RecommendedActionButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 594);
            this.Controls.Add(this.RecommendedActionButton);
            this.Controls.Add(this.NoActionButton);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "InfiniTV to ClearQAM Channel Mapper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ChannelMapGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView ChannelMapGrid;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button NoActionButton;
        private System.Windows.Forms.Button RecommendedActionButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CallsignColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PhysicalColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusCol;
        private System.Windows.Forms.DataGridViewCheckBoxColumn OTAExistsCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ListingCol;
        private System.Windows.Forms.DataGridViewButtonColumn ChangeListingCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn ActionCol;
        private System.Windows.Forms.DataGridViewButtonColumn RunCol;

    }
}

