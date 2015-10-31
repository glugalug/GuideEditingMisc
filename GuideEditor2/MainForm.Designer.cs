namespace GuideEditor2
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MergedLineupViewGridPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MergedLineupSelectionPanel = new System.Windows.Forms.Panel();
            this.MergedLineupComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MergedLineupGridPanel = new System.Windows.Forms.Panel();
            this.MergedLineupGridView = new System.Windows.Forms.DataGridView();
            this.LogoCol = new System.Windows.Forms.DataGridViewImageColumn();
            this.ListingProviderCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelNumberCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubNumberCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelTypeCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ListingCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.TunerCountCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserBlockedStateCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.SourceChannelsCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.VisibilityCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.EncryptedCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.HDCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.InBandCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RemoveChannelsButton = new System.Windows.Forms.Button();
            this.MergeChannelsButton = new System.Windows.Forms.Button();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.MergedLineupsPage = new System.Windows.Forms.TabPage();
            this.ScannedLineupsPage = new System.Windows.Forms.TabPage();
            this.ScannedTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ScannedLineupSelectionPanel = new System.Windows.Forms.Panel();
            this.ScannedLineupCombo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ScannedChannelsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ScannedLineupGridView = new System.Windows.Forms.DataGridView();
            this.ScannedLogoCol = new System.Windows.Forms.DataGridViewImageColumn();
            this.ScannedNumberCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScannedTypeCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ScannedTunerCountCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScannedListingCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ScannedRemoveCol = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ScannedEncryptedCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TuningParamsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScannedCallsignCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scannedChannelCreationControl1 = new GuideEditor2.ScannedChannelCreationControl();
            this.MergedLineupViewGridPanel.SuspendLayout();
            this.MergedLineupSelectionPanel.SuspendLayout();
            this.MergedLineupGridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MergedLineupGridView)).BeginInit();
            this.MainTabControl.SuspendLayout();
            this.MergedLineupsPage.SuspendLayout();
            this.ScannedLineupsPage.SuspendLayout();
            this.ScannedTableLayoutPanel.SuspendLayout();
            this.ScannedLineupSelectionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScannedChannelsSplitContainer)).BeginInit();
            this.ScannedChannelsSplitContainer.Panel1.SuspendLayout();
            this.ScannedChannelsSplitContainer.Panel2.SuspendLayout();
            this.ScannedChannelsSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScannedLineupGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // MergedLineupViewGridPanel
            // 
            this.MergedLineupViewGridPanel.ColumnCount = 4;
            this.MergedLineupViewGridPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.MergedLineupViewGridPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.MergedLineupViewGridPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.MergedLineupViewGridPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.MergedLineupViewGridPanel.Controls.Add(this.MergedLineupSelectionPanel, 0, 0);
            this.MergedLineupViewGridPanel.Controls.Add(this.MergedLineupGridPanel, 0, 1);
            this.MergedLineupViewGridPanel.Controls.Add(this.RemoveChannelsButton, 0, 2);
            this.MergedLineupViewGridPanel.Controls.Add(this.MergeChannelsButton, 1, 2);
            this.MergedLineupViewGridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MergedLineupViewGridPanel.Location = new System.Drawing.Point(3, 3);
            this.MergedLineupViewGridPanel.Name = "MergedLineupViewGridPanel";
            this.MergedLineupViewGridPanel.RowCount = 3;
            this.MergedLineupViewGridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.MergedLineupViewGridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MergedLineupViewGridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.MergedLineupViewGridPanel.Size = new System.Drawing.Size(995, 647);
            this.MergedLineupViewGridPanel.TabIndex = 0;
            // 
            // MergedLineupSelectionPanel
            // 
            this.MergedLineupViewGridPanel.SetColumnSpan(this.MergedLineupSelectionPanel, 3);
            this.MergedLineupSelectionPanel.Controls.Add(this.MergedLineupComboBox);
            this.MergedLineupSelectionPanel.Controls.Add(this.label1);
            this.MergedLineupSelectionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MergedLineupSelectionPanel.Location = new System.Drawing.Point(3, 3);
            this.MergedLineupSelectionPanel.Name = "MergedLineupSelectionPanel";
            this.MergedLineupSelectionPanel.Size = new System.Drawing.Size(738, 29);
            this.MergedLineupSelectionPanel.TabIndex = 0;
            // 
            // MergedLineupComboBox
            // 
            this.MergedLineupComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MergedLineupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MergedLineupComboBox.FormattingEnabled = true;
            this.MergedLineupComboBox.Location = new System.Drawing.Point(85, 4);
            this.MergedLineupComboBox.Name = "MergedLineupComboBox";
            this.MergedLineupComboBox.Size = new System.Drawing.Size(652, 21);
            this.MergedLineupComboBox.TabIndex = 1;
            this.MergedLineupComboBox.SelectedValueChanged += new System.EventHandler(this.MergedLineupComboBox_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Merged Lineup:";
            // 
            // MergedLineupGridPanel
            // 
            this.MergedLineupViewGridPanel.SetColumnSpan(this.MergedLineupGridPanel, 4);
            this.MergedLineupGridPanel.Controls.Add(this.MergedLineupGridView);
            this.MergedLineupGridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MergedLineupGridPanel.Location = new System.Drawing.Point(3, 38);
            this.MergedLineupGridPanel.Name = "MergedLineupGridPanel";
            this.MergedLineupGridPanel.Size = new System.Drawing.Size(989, 576);
            this.MergedLineupGridPanel.TabIndex = 1;
            // 
            // MergedLineupGridView
            // 
            this.MergedLineupGridView.AllowUserToAddRows = false;
            this.MergedLineupGridView.AllowUserToDeleteRows = false;
            this.MergedLineupGridView.AllowUserToOrderColumns = true;
            this.MergedLineupGridView.AllowUserToResizeRows = false;
            this.MergedLineupGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MergedLineupGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LogoCol,
            this.ListingProviderCol,
            this.ChannelNumberCol,
            this.SubNumberCol,
            this.ChannelTypeCol,
            this.ListingCol,
            this.TunerCountCol,
            this.UserBlockedStateCol,
            this.SourceChannelsCol,
            this.VisibilityCol,
            this.EncryptedCol,
            this.HDCol,
            this.InBandCol});
            this.MergedLineupGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MergedLineupGridView.Location = new System.Drawing.Point(0, 0);
            this.MergedLineupGridView.Name = "MergedLineupGridView";
            this.MergedLineupGridView.Size = new System.Drawing.Size(989, 576);
            this.MergedLineupGridView.TabIndex = 0;
            this.MergedLineupGridView.VirtualMode = true;
            this.MergedLineupGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MergedLineupGridView_CellContentClick);
            this.MergedLineupGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.MergedLineupGridView_CellValidating);
            this.MergedLineupGridView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.MergedLineupGridView_CellValueNeeded);
            this.MergedLineupGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MergedLineupGridView_ColumnHeaderMouseClick);
            this.MergedLineupGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.MergedLineupGridView_DataError);
            this.MergedLineupGridView.SelectionChanged += new System.EventHandler(this.MergedLineupGridView_SelectionChanged);
            this.MergedLineupGridView.Validating += new System.ComponentModel.CancelEventHandler(this.MergedLineupGridView_Validating);
            // 
            // LogoCol
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.LogoCol.DefaultCellStyle = dataGridViewCellStyle17;
            this.LogoCol.HeaderText = "Logo";
            this.LogoCol.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.LogoCol.Name = "LogoCol";
            this.LogoCol.ReadOnly = true;
            this.LogoCol.Width = 50;
            // 
            // ListingProviderCol
            // 
            this.ListingProviderCol.HeaderText = "Listing Provider";
            this.ListingProviderCol.Name = "ListingProviderCol";
            this.ListingProviderCol.ReadOnly = true;
            // 
            // ChannelNumberCol
            // 
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle18.Format = "N0";
            dataGridViewCellStyle18.NullValue = null;
            this.ChannelNumberCol.DefaultCellStyle = dataGridViewCellStyle18;
            this.ChannelNumberCol.HeaderText = "Channel #";
            this.ChannelNumberCol.Name = "ChannelNumberCol";
            this.ChannelNumberCol.Width = 60;
            // 
            // SubNumberCol
            // 
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle19.Format = "N0";
            dataGridViewCellStyle19.NullValue = null;
            this.SubNumberCol.DefaultCellStyle = dataGridViewCellStyle19;
            this.SubNumberCol.HeaderText = "Sub #";
            this.SubNumberCol.Name = "SubNumberCol";
            this.SubNumberCol.Width = 60;
            // 
            // ChannelTypeCol
            // 
            this.ChannelTypeCol.HeaderText = "Type";
            this.ChannelTypeCol.Name = "ChannelTypeCol";
            // 
            // ListingCol
            // 
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ListingCol.DefaultCellStyle = dataGridViewCellStyle20;
            this.ListingCol.HeaderText = "Listing";
            this.ListingCol.Name = "ListingCol";
            this.ListingCol.Width = 200;
            // 
            // TunerCountCol
            // 
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.TunerCountCol.DefaultCellStyle = dataGridViewCellStyle21;
            this.TunerCountCol.HeaderText = "Tuner Count";
            this.TunerCountCol.Name = "TunerCountCol";
            this.TunerCountCol.ReadOnly = true;
            this.TunerCountCol.Width = 50;
            // 
            // UserBlockedStateCol
            // 
            this.UserBlockedStateCol.HeaderText = "User Blocked State";
            this.UserBlockedStateCol.Name = "UserBlockedStateCol";
            // 
            // SourceChannelsCol
            // 
            this.SourceChannelsCol.HeaderText = "Source Channels";
            this.SourceChannelsCol.Name = "SourceChannelsCol";
            this.SourceChannelsCol.Width = 200;
            // 
            // VisibilityCol
            // 
            this.VisibilityCol.HeaderText = "Visibility";
            this.VisibilityCol.Name = "VisibilityCol";
            this.VisibilityCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.VisibilityCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // EncryptedCol
            // 
            this.EncryptedCol.HeaderText = "Encrypted";
            this.EncryptedCol.Name = "EncryptedCol";
            this.EncryptedCol.ReadOnly = true;
            this.EncryptedCol.Width = 60;
            // 
            // HDCol
            // 
            this.HDCol.HeaderText = "HD";
            this.HDCol.Name = "HDCol";
            this.HDCol.Width = 30;
            // 
            // InBandCol
            // 
            this.InBandCol.HeaderText = "In-band";
            this.InBandCol.Name = "InBandCol";
            this.InBandCol.Width = 60;
            // 
            // RemoveChannelsButton
            // 
            this.RemoveChannelsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveChannelsButton.Location = new System.Drawing.Point(3, 620);
            this.RemoveChannelsButton.Name = "RemoveChannelsButton";
            this.RemoveChannelsButton.Size = new System.Drawing.Size(242, 24);
            this.RemoveChannelsButton.TabIndex = 2;
            this.RemoveChannelsButton.Text = "Remove Selected Channels";
            this.RemoveChannelsButton.UseVisualStyleBackColor = true;
            this.RemoveChannelsButton.Click += new System.EventHandler(this.RemoveChannelsButton_Click);
            // 
            // MergeChannelsButton
            // 
            this.MergeChannelsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MergeChannelsButton.Location = new System.Drawing.Point(251, 620);
            this.MergeChannelsButton.Name = "MergeChannelsButton";
            this.MergeChannelsButton.Size = new System.Drawing.Size(242, 24);
            this.MergeChannelsButton.TabIndex = 3;
            this.MergeChannelsButton.Text = "Merge Selected Channels";
            this.MergeChannelsButton.UseVisualStyleBackColor = true;
            this.MergeChannelsButton.Click += new System.EventHandler(this.MergeChannelsButton_Click);
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.MergedLineupsPage);
            this.MainTabControl.Controls.Add(this.ScannedLineupsPage);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(1009, 679);
            this.MainTabControl.TabIndex = 1;
            // 
            // MergedLineupsPage
            // 
            this.MergedLineupsPage.Controls.Add(this.MergedLineupViewGridPanel);
            this.MergedLineupsPage.Location = new System.Drawing.Point(4, 22);
            this.MergedLineupsPage.Name = "MergedLineupsPage";
            this.MergedLineupsPage.Padding = new System.Windows.Forms.Padding(3);
            this.MergedLineupsPage.Size = new System.Drawing.Size(1001, 653);
            this.MergedLineupsPage.TabIndex = 0;
            this.MergedLineupsPage.Text = "Merged Lineup(s)";
            this.MergedLineupsPage.UseVisualStyleBackColor = true;
            // 
            // ScannedLineupsPage
            // 
            this.ScannedLineupsPage.BackColor = System.Drawing.SystemColors.Control;
            this.ScannedLineupsPage.Controls.Add(this.ScannedTableLayoutPanel);
            this.ScannedLineupsPage.Location = new System.Drawing.Point(4, 22);
            this.ScannedLineupsPage.Name = "ScannedLineupsPage";
            this.ScannedLineupsPage.Padding = new System.Windows.Forms.Padding(3);
            this.ScannedLineupsPage.Size = new System.Drawing.Size(1001, 653);
            this.ScannedLineupsPage.TabIndex = 1;
            this.ScannedLineupsPage.Text = "Scanned/Wmis Lineups";
            // 
            // ScannedTableLayoutPanel
            // 
            this.ScannedTableLayoutPanel.ColumnCount = 4;
            this.ScannedTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ScannedTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ScannedTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ScannedTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ScannedTableLayoutPanel.Controls.Add(this.ScannedLineupSelectionPanel, 0, 0);
            this.ScannedTableLayoutPanel.Controls.Add(this.ScannedChannelsSplitContainer, 0, 1);
            this.ScannedTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScannedTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.ScannedTableLayoutPanel.Name = "ScannedTableLayoutPanel";
            this.ScannedTableLayoutPanel.RowCount = 2;
            this.ScannedTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.ScannedTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ScannedTableLayoutPanel.Size = new System.Drawing.Size(995, 647);
            this.ScannedTableLayoutPanel.TabIndex = 0;
            // 
            // ScannedLineupSelectionPanel
            // 
            this.ScannedTableLayoutPanel.SetColumnSpan(this.ScannedLineupSelectionPanel, 3);
            this.ScannedLineupSelectionPanel.Controls.Add(this.ScannedLineupCombo);
            this.ScannedLineupSelectionPanel.Controls.Add(this.label2);
            this.ScannedLineupSelectionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScannedLineupSelectionPanel.Location = new System.Drawing.Point(3, 3);
            this.ScannedLineupSelectionPanel.Name = "ScannedLineupSelectionPanel";
            this.ScannedLineupSelectionPanel.Size = new System.Drawing.Size(738, 29);
            this.ScannedLineupSelectionPanel.TabIndex = 0;
            // 
            // ScannedLineupCombo
            // 
            this.ScannedLineupCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScannedLineupCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScannedLineupCombo.FormattingEnabled = true;
            this.ScannedLineupCombo.Location = new System.Drawing.Point(89, 5);
            this.ScannedLineupCombo.Name = "ScannedLineupCombo";
            this.ScannedLineupCombo.Size = new System.Drawing.Size(646, 21);
            this.ScannedLineupCombo.TabIndex = 1;
            this.ScannedLineupCombo.SelectedValueChanged += new System.EventHandler(this.ScannedLineupCombo_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Select a lineup:";
            // 
            // ScannedChannelsSplitContainer
            // 
            this.ScannedTableLayoutPanel.SetColumnSpan(this.ScannedChannelsSplitContainer, 4);
            this.ScannedChannelsSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScannedChannelsSplitContainer.Location = new System.Drawing.Point(3, 38);
            this.ScannedChannelsSplitContainer.Name = "ScannedChannelsSplitContainer";
            this.ScannedChannelsSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // ScannedChannelsSplitContainer.Panel1
            // 
            this.ScannedChannelsSplitContainer.Panel1.Controls.Add(this.ScannedLineupGridView);
            // 
            // ScannedChannelsSplitContainer.Panel2
            // 
            this.ScannedChannelsSplitContainer.Panel2.Controls.Add(this.scannedChannelCreationControl1);
            this.ScannedChannelsSplitContainer.Size = new System.Drawing.Size(989, 606);
            this.ScannedChannelsSplitContainer.SplitterDistance = 326;
            this.ScannedChannelsSplitContainer.TabIndex = 1;
            // 
            // ScannedLineupGridView
            // 
            this.ScannedLineupGridView.AllowUserToAddRows = false;
            this.ScannedLineupGridView.AllowUserToDeleteRows = false;
            this.ScannedLineupGridView.AllowUserToOrderColumns = true;
            this.ScannedLineupGridView.AllowUserToResizeRows = false;
            this.ScannedLineupGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScannedLineupGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ScannedLineupGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ScannedLogoCol,
            this.ScannedNumberCol,
            this.ScannedTypeCol,
            this.ScannedTunerCountCol,
            this.ScannedListingCol,
            this.ScannedRemoveCol,
            this.ScannedEncryptedCol,
            this.TuningParamsCol,
            this.ScannedCallsignCol});
            this.ScannedLineupGridView.Location = new System.Drawing.Point(2, 4);
            this.ScannedLineupGridView.Name = "ScannedLineupGridView";
            this.ScannedLineupGridView.Size = new System.Drawing.Size(984, 320);
            this.ScannedLineupGridView.TabIndex = 0;
            this.ScannedLineupGridView.VirtualMode = true;
            this.ScannedLineupGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ScannedLineupGridView_CellContentClick);
            this.ScannedLineupGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.ScannedLineupGridView_CellValidating);
            this.ScannedLineupGridView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.ScannedLineupGridView_CellValueNeeded);
            this.ScannedLineupGridView.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.ScannedLineupGridView_CellValuePushed);
            this.ScannedLineupGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ScannedLineupGridView_ColumnHeaderMouseClick);
            this.ScannedLineupGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.ScannedLineupGridView_DataError);
            // 
            // ScannedLogoCol
            // 
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle22.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle22.NullValue")));
            this.ScannedLogoCol.DefaultCellStyle = dataGridViewCellStyle22;
            this.ScannedLogoCol.HeaderText = "Logo";
            this.ScannedLogoCol.Name = "ScannedLogoCol";
            this.ScannedLogoCol.ReadOnly = true;
            this.ScannedLogoCol.Visible = false;
            // 
            // ScannedNumberCol
            // 
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ScannedNumberCol.DefaultCellStyle = dataGridViewCellStyle23;
            this.ScannedNumberCol.HeaderText = "Number";
            this.ScannedNumberCol.Name = "ScannedNumberCol";
            this.ScannedNumberCol.ReadOnly = true;
            // 
            // ScannedTypeCol
            // 
            this.ScannedTypeCol.HeaderText = "Channel Type";
            this.ScannedTypeCol.Name = "ScannedTypeCol";
            // 
            // ScannedTunerCountCol
            // 
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ScannedTunerCountCol.DefaultCellStyle = dataGridViewCellStyle24;
            this.ScannedTunerCountCol.HeaderText = "Tuner Count";
            this.ScannedTunerCountCol.Name = "ScannedTunerCountCol";
            this.ScannedTunerCountCol.ReadOnly = true;
            // 
            // ScannedListingCol
            // 
            this.ScannedListingCol.HeaderText = "Listing";
            this.ScannedListingCol.Name = "ScannedListingCol";
            this.ScannedListingCol.ReadOnly = true;
            this.ScannedListingCol.Width = 250;
            // 
            // ScannedRemoveCol
            // 
            this.ScannedRemoveCol.HeaderText = "Remove Channel";
            this.ScannedRemoveCol.Name = "ScannedRemoveCol";
            this.ScannedRemoveCol.ReadOnly = true;
            this.ScannedRemoveCol.Text = "Remove";
            this.ScannedRemoveCol.UseColumnTextForButtonValue = true;
            // 
            // ScannedEncryptedCol
            // 
            this.ScannedEncryptedCol.HeaderText = "Encrypted";
            this.ScannedEncryptedCol.Name = "ScannedEncryptedCol";
            this.ScannedEncryptedCol.ReadOnly = true;
            this.ScannedEncryptedCol.Width = 75;
            // 
            // TuningParamsCol
            // 
            this.TuningParamsCol.HeaderText = "Tuning Params";
            this.TuningParamsCol.Name = "TuningParamsCol";
            this.TuningParamsCol.ReadOnly = true;
            // 
            // ScannedCallsignCol
            // 
            this.ScannedCallsignCol.HeaderText = "Callsign";
            this.ScannedCallsignCol.Name = "ScannedCallsignCol";
            // 
            // scannedChannelCreationControl1
            // 
            this.scannedChannelCreationControl1.DestinationMergedChannel = null;
            this.scannedChannelCreationControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scannedChannelCreationControl1.Location = new System.Drawing.Point(0, 0);
            this.scannedChannelCreationControl1.Name = "scannedChannelCreationControl1";
            this.scannedChannelCreationControl1.Size = new System.Drawing.Size(989, 276);
            this.scannedChannelCreationControl1.TabIndex = 0;
            this.scannedChannelCreationControl1.ChannelAdded += new System.EventHandler(this.scannedChannelCreationControl1_ChannelAdded);
            this.scannedChannelCreationControl1.Load += new System.EventHandler(this.scannedChannelCreationControl1_Load);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 679);
            this.Controls.Add(this.MainTabControl);
            this.Name = "MainForm";
            this.Text = "Guide Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.MergedLineupViewGridPanel.ResumeLayout(false);
            this.MergedLineupSelectionPanel.ResumeLayout(false);
            this.MergedLineupSelectionPanel.PerformLayout();
            this.MergedLineupGridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MergedLineupGridView)).EndInit();
            this.MainTabControl.ResumeLayout(false);
            this.MergedLineupsPage.ResumeLayout(false);
            this.ScannedLineupsPage.ResumeLayout(false);
            this.ScannedTableLayoutPanel.ResumeLayout(false);
            this.ScannedLineupSelectionPanel.ResumeLayout(false);
            this.ScannedLineupSelectionPanel.PerformLayout();
            this.ScannedChannelsSplitContainer.Panel1.ResumeLayout(false);
            this.ScannedChannelsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScannedChannelsSplitContainer)).EndInit();
            this.ScannedChannelsSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScannedLineupGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MergedLineupViewGridPanel;
        private System.Windows.Forms.Panel MergedLineupSelectionPanel;
        private System.Windows.Forms.ComboBox MergedLineupComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel MergedLineupGridPanel;
        private System.Windows.Forms.DataGridView MergedLineupGridView;
        private System.Windows.Forms.Button RemoveChannelsButton;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage MergedLineupsPage;
        private System.Windows.Forms.TabPage ScannedLineupsPage;
        private System.Windows.Forms.Button MergeChannelsButton;
        private System.Windows.Forms.TableLayoutPanel ScannedTableLayoutPanel;
        private System.Windows.Forms.Panel ScannedLineupSelectionPanel;
        private System.Windows.Forms.ComboBox ScannedLineupCombo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer ScannedChannelsSplitContainer;
        private System.Windows.Forms.DataGridView ScannedLineupGridView;
        private ScannedChannelCreationControl scannedChannelCreationControl1;
        private System.Windows.Forms.DataGridViewImageColumn ScannedLogoCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScannedNumberCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn ScannedTypeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScannedTunerCountCol;
        private System.Windows.Forms.DataGridViewButtonColumn ScannedListingCol;
        private System.Windows.Forms.DataGridViewButtonColumn ScannedRemoveCol;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ScannedEncryptedCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TuningParamsCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScannedCallsignCol;
        private System.Windows.Forms.DataGridViewImageColumn LogoCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ListingProviderCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelNumberCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubNumberCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn ChannelTypeCol;
        private System.Windows.Forms.DataGridViewButtonColumn ListingCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TunerCountCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn UserBlockedStateCol;
        private System.Windows.Forms.DataGridViewButtonColumn SourceChannelsCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn VisibilityCol;
        private System.Windows.Forms.DataGridViewCheckBoxColumn EncryptedCol;
        private System.Windows.Forms.DataGridViewCheckBoxColumn HDCol;
        private System.Windows.Forms.DataGridViewCheckBoxColumn InBandCol;
    }
}

