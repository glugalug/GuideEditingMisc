namespace XMLGuideImporter
{
    partial class ImportConfigForm
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
            this.GrabXMLButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.GrabberBrowseButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.XMLInputGroupBox = new System.Windows.Forms.GroupBox();
            this.ParseSettingGroupBox = new System.Windows.Forms.GroupBox();
            this.ParseSettingsSplit = new System.Windows.Forms.SplitContainer();
            this.ParseSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.SaveParsingSettingsButton = new System.Windows.Forms.Button();
            this.ServiceNameXPathTestButton = new System.Windows.Forms.Button();
            this.ServiceNameXPathRevertButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.CallsignTestButton = new System.Windows.Forms.Button();
            this.CallsignRevertButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.ChannelNumberTestButton = new System.Windows.Forms.Button();
            this.ChannelNumberRevertButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.ParsePreviewGroupBox = new System.Windows.Forms.GroupBox();
            this.TestMatchingListBox = new System.Windows.Forms.ListBox();
            this.MXFGenerationGroupBox = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.MXFBenerationTestButton = new System.Windows.Forms.Button();
            this.MXFPreviewText = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.AffiliateXPathTestButton = new System.Windows.Forms.Button();
            this.AffiliateXPathRevertButton = new System.Windows.Forms.Button();
            this.MxfLineupNameTextBox = new System.Windows.Forms.TextBox();
            this.MxfLineupUidTextBox = new System.Windows.Forms.TextBox();
            this.AffiliateTextBox = new System.Windows.Forms.TextBox();
            this.ServiceNameXPathTextBox = new System.Windows.Forms.TextBox();
            this.CallsignXPathTextBox = new System.Windows.Forms.TextBox();
            this.ChannelNumberXPathTextBox = new System.Windows.Forms.TextBox();
            this.GrabberArgumentsTextBox = new System.Windows.Forms.TextBox();
            this.CommandLineTextBox = new System.Windows.Forms.TextBox();
            this.XMLTVPathTextBox = new System.Windows.Forms.TextBox();
            this.XMLInputGroupBox.SuspendLayout();
            this.ParseSettingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParseSettingsSplit)).BeginInit();
            this.ParseSettingsSplit.Panel1.SuspendLayout();
            this.ParseSettingsSplit.Panel2.SuspendLayout();
            this.ParseSettingsSplit.SuspendLayout();
            this.ParseSettingsGroupBox.SuspendLayout();
            this.ParsePreviewGroupBox.SuspendLayout();
            this.MXFGenerationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "XML Grabber:";
            // 
            // GrabXMLButton
            // 
            this.GrabXMLButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GrabXMLButton.Location = new System.Drawing.Point(817, 17);
            this.GrabXMLButton.Name = "GrabXMLButton";
            this.GrabXMLButton.Size = new System.Drawing.Size(75, 23);
            this.GrabXMLButton.TabIndex = 2;
            this.GrabXMLButton.Text = "Run Now";
            this.GrabXMLButton.UseVisualStyleBackColor = true;
            this.GrabXMLButton.Click += new System.EventHandler(this.GrabXMLButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "XMLTV Path:";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.Location = new System.Drawing.Point(737, 49);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 5;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadButton.Location = new System.Drawing.Point(817, 49);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 6;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // GrabberBrowseButton
            // 
            this.GrabberBrowseButton.Location = new System.Drawing.Point(228, 17);
            this.GrabberBrowseButton.Name = "GrabberBrowseButton";
            this.GrabberBrowseButton.Size = new System.Drawing.Size(58, 23);
            this.GrabberBrowseButton.TabIndex = 7;
            this.GrabberBrowseButton.Text = "Browse";
            this.GrabberBrowseButton.UseVisualStyleBackColor = true;
            this.GrabberBrowseButton.Click += new System.EventHandler(this.GrabberBrowseButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(292, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Arguments:";
            // 
            // XMLInputGroupBox
            // 
            this.XMLInputGroupBox.Controls.Add(this.GrabberArgumentsTextBox);
            this.XMLInputGroupBox.Controls.Add(this.label1);
            this.XMLInputGroupBox.Controls.Add(this.label3);
            this.XMLInputGroupBox.Controls.Add(this.CommandLineTextBox);
            this.XMLInputGroupBox.Controls.Add(this.GrabberBrowseButton);
            this.XMLInputGroupBox.Controls.Add(this.GrabXMLButton);
            this.XMLInputGroupBox.Controls.Add(this.LoadButton);
            this.XMLInputGroupBox.Controls.Add(this.label2);
            this.XMLInputGroupBox.Controls.Add(this.BrowseButton);
            this.XMLInputGroupBox.Controls.Add(this.XMLTVPathTextBox);
            this.XMLInputGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.XMLInputGroupBox.Location = new System.Drawing.Point(0, 0);
            this.XMLInputGroupBox.Name = "XMLInputGroupBox";
            this.XMLInputGroupBox.Size = new System.Drawing.Size(900, 83);
            this.XMLInputGroupBox.TabIndex = 10;
            this.XMLInputGroupBox.TabStop = false;
            this.XMLInputGroupBox.Text = "XML Input";
            // 
            // ParseSettingGroupBox
            // 
            this.ParseSettingGroupBox.Controls.Add(this.ParseSettingsSplit);
            this.ParseSettingGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ParseSettingGroupBox.Location = new System.Drawing.Point(0, 83);
            this.ParseSettingGroupBox.Name = "ParseSettingGroupBox";
            this.ParseSettingGroupBox.Size = new System.Drawing.Size(900, 200);
            this.ParseSettingGroupBox.TabIndex = 11;
            this.ParseSettingGroupBox.TabStop = false;
            this.ParseSettingGroupBox.Text = "Parsing";
            // 
            // ParseSettingsSplit
            // 
            this.ParseSettingsSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParseSettingsSplit.Location = new System.Drawing.Point(3, 16);
            this.ParseSettingsSplit.Name = "ParseSettingsSplit";
            // 
            // ParseSettingsSplit.Panel1
            // 
            this.ParseSettingsSplit.Panel1.Controls.Add(this.ParseSettingsGroupBox);
            // 
            // ParseSettingsSplit.Panel2
            // 
            this.ParseSettingsSplit.Panel2.Controls.Add(this.ParsePreviewGroupBox);
            this.ParseSettingsSplit.Size = new System.Drawing.Size(894, 181);
            this.ParseSettingsSplit.SplitterDistance = 568;
            this.ParseSettingsSplit.TabIndex = 0;
            // 
            // ParseSettingsGroupBox
            // 
            this.ParseSettingsGroupBox.Controls.Add(this.AffiliateXPathTestButton);
            this.ParseSettingsGroupBox.Controls.Add(this.AffiliateXPathRevertButton);
            this.ParseSettingsGroupBox.Controls.Add(this.AffiliateTextBox);
            this.ParseSettingsGroupBox.Controls.Add(this.label9);
            this.ParseSettingsGroupBox.Controls.Add(this.SaveParsingSettingsButton);
            this.ParseSettingsGroupBox.Controls.Add(this.ServiceNameXPathTestButton);
            this.ParseSettingsGroupBox.Controls.Add(this.ServiceNameXPathRevertButton);
            this.ParseSettingsGroupBox.Controls.Add(this.ServiceNameXPathTextBox);
            this.ParseSettingsGroupBox.Controls.Add(this.label6);
            this.ParseSettingsGroupBox.Controls.Add(this.CallsignTestButton);
            this.ParseSettingsGroupBox.Controls.Add(this.CallsignRevertButton);
            this.ParseSettingsGroupBox.Controls.Add(this.CallsignXPathTextBox);
            this.ParseSettingsGroupBox.Controls.Add(this.label5);
            this.ParseSettingsGroupBox.Controls.Add(this.ChannelNumberTestButton);
            this.ParseSettingsGroupBox.Controls.Add(this.ChannelNumberRevertButton);
            this.ParseSettingsGroupBox.Controls.Add(this.ChannelNumberXPathTextBox);
            this.ParseSettingsGroupBox.Controls.Add(this.label4);
            this.ParseSettingsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParseSettingsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ParseSettingsGroupBox.Name = "ParseSettingsGroupBox";
            this.ParseSettingsGroupBox.Size = new System.Drawing.Size(568, 181);
            this.ParseSettingsGroupBox.TabIndex = 0;
            this.ParseSettingsGroupBox.TabStop = false;
            this.ParseSettingsGroupBox.Text = "Settings";
            // 
            // SaveParsingSettingsButton
            // 
            this.SaveParsingSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveParsingSettingsButton.Location = new System.Drawing.Point(408, 152);
            this.SaveParsingSettingsButton.Name = "SaveParsingSettingsButton";
            this.SaveParsingSettingsButton.Size = new System.Drawing.Size(154, 23);
            this.SaveParsingSettingsButton.TabIndex = 13;
            this.SaveParsingSettingsButton.Text = "Save Parsing Settings";
            this.SaveParsingSettingsButton.UseVisualStyleBackColor = true;
            this.SaveParsingSettingsButton.Click += new System.EventHandler(this.SaveParsingSettingsButton_Click);
            // 
            // ServiceNameXPathTestButton
            // 
            this.ServiceNameXPathTestButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ServiceNameXPathTestButton.Location = new System.Drawing.Point(489, 87);
            this.ServiceNameXPathTestButton.Name = "ServiceNameXPathTestButton";
            this.ServiceNameXPathTestButton.Size = new System.Drawing.Size(75, 23);
            this.ServiceNameXPathTestButton.TabIndex = 12;
            this.ServiceNameXPathTestButton.Text = "Test";
            this.ServiceNameXPathTestButton.UseVisualStyleBackColor = true;
            this.ServiceNameXPathTestButton.Click += new System.EventHandler(this.ServiceNameXPathTestButton_Click);
            // 
            // ServiceNameXPathRevertButton
            // 
            this.ServiceNameXPathRevertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ServiceNameXPathRevertButton.Location = new System.Drawing.Point(408, 87);
            this.ServiceNameXPathRevertButton.Name = "ServiceNameXPathRevertButton";
            this.ServiceNameXPathRevertButton.Size = new System.Drawing.Size(75, 23);
            this.ServiceNameXPathRevertButton.TabIndex = 11;
            this.ServiceNameXPathRevertButton.Text = "Revert";
            this.ServiceNameXPathRevertButton.UseVisualStyleBackColor = true;
            this.ServiceNameXPathRevertButton.Click += new System.EventHandler(this.ServiceNameXPathRevertButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Service name xpath:";
            // 
            // CallsignTestButton
            // 
            this.CallsignTestButton.Location = new System.Drawing.Point(489, 53);
            this.CallsignTestButton.Name = "CallsignTestButton";
            this.CallsignTestButton.Size = new System.Drawing.Size(75, 23);
            this.CallsignTestButton.TabIndex = 8;
            this.CallsignTestButton.Text = "Test";
            this.CallsignTestButton.UseVisualStyleBackColor = true;
            this.CallsignTestButton.Click += new System.EventHandler(this.CallsignTestButton_Click);
            // 
            // CallsignRevertButton
            // 
            this.CallsignRevertButton.Location = new System.Drawing.Point(408, 53);
            this.CallsignRevertButton.Name = "CallsignRevertButton";
            this.CallsignRevertButton.Size = new System.Drawing.Size(75, 23);
            this.CallsignRevertButton.TabIndex = 7;
            this.CallsignRevertButton.Text = "Revert";
            this.CallsignRevertButton.UseVisualStyleBackColor = true;
            this.CallsignRevertButton.Click += new System.EventHandler(this.CallsignRevertButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Callsign xpath:";
            // 
            // ChannelNumberTestButton
            // 
            this.ChannelNumberTestButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelNumberTestButton.Location = new System.Drawing.Point(489, 27);
            this.ChannelNumberTestButton.Name = "ChannelNumberTestButton";
            this.ChannelNumberTestButton.Size = new System.Drawing.Size(75, 23);
            this.ChannelNumberTestButton.TabIndex = 4;
            this.ChannelNumberTestButton.Text = "Test";
            this.ChannelNumberTestButton.UseVisualStyleBackColor = true;
            this.ChannelNumberTestButton.Click += new System.EventHandler(this.ChannelNumberTestButton_Click);
            // 
            // ChannelNumberRevertButton
            // 
            this.ChannelNumberRevertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelNumberRevertButton.Location = new System.Drawing.Point(408, 27);
            this.ChannelNumberRevertButton.Name = "ChannelNumberRevertButton";
            this.ChannelNumberRevertButton.Size = new System.Drawing.Size(75, 23);
            this.ChannelNumberRevertButton.TabIndex = 3;
            this.ChannelNumberRevertButton.Text = "Revert";
            this.ChannelNumberRevertButton.UseVisualStyleBackColor = true;
            this.ChannelNumberRevertButton.Click += new System.EventHandler(this.ChannelNumberRevertButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Channel Number xpath:";
            // 
            // ParsePreviewGroupBox
            // 
            this.ParsePreviewGroupBox.Controls.Add(this.TestMatchingListBox);
            this.ParsePreviewGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParsePreviewGroupBox.Location = new System.Drawing.Point(0, 0);
            this.ParsePreviewGroupBox.Name = "ParsePreviewGroupBox";
            this.ParsePreviewGroupBox.Size = new System.Drawing.Size(322, 181);
            this.ParsePreviewGroupBox.TabIndex = 0;
            this.ParsePreviewGroupBox.TabStop = false;
            this.ParsePreviewGroupBox.Text = "Test Matching Preview";
            // 
            // TestMatchingListBox
            // 
            this.TestMatchingListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TestMatchingListBox.FormattingEnabled = true;
            this.TestMatchingListBox.Location = new System.Drawing.Point(3, 16);
            this.TestMatchingListBox.Name = "TestMatchingListBox";
            this.TestMatchingListBox.Size = new System.Drawing.Size(316, 162);
            this.TestMatchingListBox.TabIndex = 0;
            // 
            // MXFGenerationGroupBox
            // 
            this.MXFGenerationGroupBox.Controls.Add(this.MxfLineupNameTextBox);
            this.MXFGenerationGroupBox.Controls.Add(this.label8);
            this.MXFGenerationGroupBox.Controls.Add(this.MxfLineupUidTextBox);
            this.MXFGenerationGroupBox.Controls.Add(this.label7);
            this.MXFGenerationGroupBox.Controls.Add(this.MXFBenerationTestButton);
            this.MXFGenerationGroupBox.Controls.Add(this.MXFPreviewText);
            this.MXFGenerationGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.MXFGenerationGroupBox.Location = new System.Drawing.Point(0, 283);
            this.MXFGenerationGroupBox.Name = "MXFGenerationGroupBox";
            this.MXFGenerationGroupBox.Size = new System.Drawing.Size(900, 490);
            this.MXFGenerationGroupBox.TabIndex = 12;
            this.MXFGenerationGroupBox.TabStop = false;
            this.MXFGenerationGroupBox.Text = "MXF Generation Settings";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Linup Name:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Lineup Unique Id";
            // 
            // MXFBenerationTestButton
            // 
            this.MXFBenerationTestButton.Location = new System.Drawing.Point(806, 117);
            this.MXFBenerationTestButton.Name = "MXFBenerationTestButton";
            this.MXFBenerationTestButton.Size = new System.Drawing.Size(75, 23);
            this.MXFBenerationTestButton.TabIndex = 14;
            this.MXFBenerationTestButton.Text = "Test";
            this.MXFBenerationTestButton.UseVisualStyleBackColor = true;
            this.MXFBenerationTestButton.Click += new System.EventHandler(this.MXFBenerationTestButton_Click);
            // 
            // MXFPreviewText
            // 
            this.MXFPreviewText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MXFPreviewText.Location = new System.Drawing.Point(12, 156);
            this.MXFPreviewText.Multiline = true;
            this.MXFPreviewText.Name = "MXFPreviewText";
            this.MXFPreviewText.ReadOnly = true;
            this.MXFPreviewText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.MXFPreviewText.Size = new System.Drawing.Size(869, 319);
            this.MXFPreviewText.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Affiliate xpath:";
            // 
            // AffiliateXPathTestButton
            // 
            this.AffiliateXPathTestButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AffiliateXPathTestButton.Location = new System.Drawing.Point(489, 121);
            this.AffiliateXPathTestButton.Name = "AffiliateXPathTestButton";
            this.AffiliateXPathTestButton.Size = new System.Drawing.Size(75, 23);
            this.AffiliateXPathTestButton.TabIndex = 17;
            this.AffiliateXPathTestButton.Text = "Test";
            this.AffiliateXPathTestButton.UseVisualStyleBackColor = true;
            this.AffiliateXPathTestButton.Click += new System.EventHandler(this.AffiliateXPathTestButton_Click);
            // 
            // AffiliateXPathRevertButton
            // 
            this.AffiliateXPathRevertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AffiliateXPathRevertButton.Location = new System.Drawing.Point(408, 121);
            this.AffiliateXPathRevertButton.Name = "AffiliateXPathRevertButton";
            this.AffiliateXPathRevertButton.Size = new System.Drawing.Size(75, 23);
            this.AffiliateXPathRevertButton.TabIndex = 16;
            this.AffiliateXPathRevertButton.Text = "Revert";
            this.AffiliateXPathRevertButton.UseVisualStyleBackColor = true;
            this.AffiliateXPathRevertButton.Click += new System.EventHandler(this.AffiliateXPathRevertButton_Click);
            // 
            // MxfLineupNameTextBox
            // 
            this.MxfLineupNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MxfLineupNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::XMLGuideImporter.Properties.Settings.Default, "MXFLineupName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.MxfLineupNameTextBox.Location = new System.Drawing.Point(82, 55);
            this.MxfLineupNameTextBox.Name = "MxfLineupNameTextBox";
            this.MxfLineupNameTextBox.Size = new System.Drawing.Size(799, 20);
            this.MxfLineupNameTextBox.TabIndex = 18;
            this.MxfLineupNameTextBox.Text = global::XMLGuideImporter.Properties.Settings.Default.MXFLineupName;
            // 
            // MxfLineupUidTextBox
            // 
            this.MxfLineupUidTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::XMLGuideImporter.Properties.Settings.Default, "MXFLineupUid", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.MxfLineupUidTextBox.Location = new System.Drawing.Point(103, 29);
            this.MxfLineupUidTextBox.Name = "MxfLineupUidTextBox";
            this.MxfLineupUidTextBox.Size = new System.Drawing.Size(778, 20);
            this.MxfLineupUidTextBox.TabIndex = 16;
            this.MxfLineupUidTextBox.Text = global::XMLGuideImporter.Properties.Settings.Default.MXFLineupUid;
            // 
            // AffiliateTextBox
            // 
            this.AffiliateTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AffiliateTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::XMLGuideImporter.Properties.Settings.Default, "AffiliateXPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AffiliateTextBox.Location = new System.Drawing.Point(88, 123);
            this.AffiliateTextBox.Name = "AffiliateTextBox";
            this.AffiliateTextBox.Size = new System.Drawing.Size(314, 20);
            this.AffiliateTextBox.TabIndex = 15;
            this.AffiliateTextBox.Text = global::XMLGuideImporter.Properties.Settings.Default.AffiliateXPath;
            // 
            // ServiceNameXPathTextBox
            // 
            this.ServiceNameXPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ServiceNameXPathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::XMLGuideImporter.Properties.Settings.Default, "ServiceNameXPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ServiceNameXPathTextBox.Location = new System.Drawing.Point(117, 87);
            this.ServiceNameXPathTextBox.Name = "ServiceNameXPathTextBox";
            this.ServiceNameXPathTextBox.Size = new System.Drawing.Size(285, 20);
            this.ServiceNameXPathTextBox.TabIndex = 10;
            this.ServiceNameXPathTextBox.Text = global::XMLGuideImporter.Properties.Settings.Default.ServiceNameXPath;
            // 
            // CallsignXPathTextBox
            // 
            this.CallsignXPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CallsignXPathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::XMLGuideImporter.Properties.Settings.Default, "CallsignXPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CallsignXPathTextBox.Location = new System.Drawing.Point(88, 55);
            this.CallsignXPathTextBox.Name = "CallsignXPathTextBox";
            this.CallsignXPathTextBox.Size = new System.Drawing.Size(314, 20);
            this.CallsignXPathTextBox.TabIndex = 6;
            this.CallsignXPathTextBox.Text = global::XMLGuideImporter.Properties.Settings.Default.CallsignXPath;
            // 
            // ChannelNumberXPathTextBox
            // 
            this.ChannelNumberXPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelNumberXPathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::XMLGuideImporter.Properties.Settings.Default, "ChannelNumberXPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ChannelNumberXPathTextBox.Location = new System.Drawing.Point(123, 27);
            this.ChannelNumberXPathTextBox.Name = "ChannelNumberXPathTextBox";
            this.ChannelNumberXPathTextBox.Size = new System.Drawing.Size(279, 20);
            this.ChannelNumberXPathTextBox.TabIndex = 2;
            this.ChannelNumberXPathTextBox.Text = global::XMLGuideImporter.Properties.Settings.Default.ChannelNumberXPath;
            // 
            // GrabberArgumentsTextBox
            // 
            this.GrabberArgumentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrabberArgumentsTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::XMLGuideImporter.Properties.Settings.Default, "GrabberArguments", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.GrabberArgumentsTextBox.Location = new System.Drawing.Point(358, 19);
            this.GrabberArgumentsTextBox.Name = "GrabberArgumentsTextBox";
            this.GrabberArgumentsTextBox.Size = new System.Drawing.Size(453, 20);
            this.GrabberArgumentsTextBox.TabIndex = 9;
            this.GrabberArgumentsTextBox.Text = global::XMLGuideImporter.Properties.Settings.Default.GrabberArguments;
            this.GrabberArgumentsTextBox.Leave += new System.EventHandler(this.GrabberArgumentsTextBox_Leave);
            // 
            // CommandLineTextBox
            // 
            this.CommandLineTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::XMLGuideImporter.Properties.Settings.Default, "GrabberCommandLine", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CommandLineTextBox.Location = new System.Drawing.Point(88, 19);
            this.CommandLineTextBox.Name = "CommandLineTextBox";
            this.CommandLineTextBox.Size = new System.Drawing.Size(134, 20);
            this.CommandLineTextBox.TabIndex = 1;
            this.CommandLineTextBox.Text = global::XMLGuideImporter.Properties.Settings.Default.GrabberCommandLine;
            this.CommandLineTextBox.Leave += new System.EventHandler(this.CommandLineTextBox_Leave);
            // 
            // XMLTVPathTextBox
            // 
            this.XMLTVPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.XMLTVPathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::XMLGuideImporter.Properties.Settings.Default, "XMLTVPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.XMLTVPathTextBox.Location = new System.Drawing.Point(88, 51);
            this.XMLTVPathTextBox.Name = "XMLTVPathTextBox";
            this.XMLTVPathTextBox.Size = new System.Drawing.Size(643, 20);
            this.XMLTVPathTextBox.TabIndex = 4;
            this.XMLTVPathTextBox.Text = global::XMLGuideImporter.Properties.Settings.Default.XMLTVPath;
            this.XMLTVPathTextBox.Leave += new System.EventHandler(this.XMLTVPathTextBox_Leave);
            // 
            // ImportConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 775);
            this.Controls.Add(this.MXFGenerationGroupBox);
            this.Controls.Add(this.ParseSettingGroupBox);
            this.Controls.Add(this.XMLInputGroupBox);
            this.Name = "ImportConfigForm";
            this.Text = "XML Import Configuration";
            this.XMLInputGroupBox.ResumeLayout(false);
            this.XMLInputGroupBox.PerformLayout();
            this.ParseSettingGroupBox.ResumeLayout(false);
            this.ParseSettingsSplit.Panel1.ResumeLayout(false);
            this.ParseSettingsSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ParseSettingsSplit)).EndInit();
            this.ParseSettingsSplit.ResumeLayout(false);
            this.ParseSettingsGroupBox.ResumeLayout(false);
            this.ParseSettingsGroupBox.PerformLayout();
            this.ParsePreviewGroupBox.ResumeLayout(false);
            this.MXFGenerationGroupBox.ResumeLayout(false);
            this.MXFGenerationGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox CommandLineTextBox;
        private System.Windows.Forms.Button GrabXMLButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox XMLTVPathTextBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Button GrabberBrowseButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox GrabberArgumentsTextBox;
        private System.Windows.Forms.GroupBox XMLInputGroupBox;
        private System.Windows.Forms.GroupBox ParseSettingGroupBox;
        private System.Windows.Forms.SplitContainer ParseSettingsSplit;
        private System.Windows.Forms.GroupBox ParseSettingsGroupBox;
        private System.Windows.Forms.GroupBox ParsePreviewGroupBox;
        private System.Windows.Forms.ListBox TestMatchingListBox;
        private System.Windows.Forms.TextBox ChannelNumberXPathTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button ChannelNumberTestButton;
        private System.Windows.Forms.Button ChannelNumberRevertButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button CallsignTestButton;
        private System.Windows.Forms.Button CallsignRevertButton;
        private System.Windows.Forms.TextBox CallsignXPathTextBox;
        private System.Windows.Forms.TextBox ServiceNameXPathTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ServiceNameXPathTestButton;
        private System.Windows.Forms.Button ServiceNameXPathRevertButton;
        private System.Windows.Forms.Button SaveParsingSettingsButton;
        private System.Windows.Forms.GroupBox MXFGenerationGroupBox;
        private System.Windows.Forms.Button MXFBenerationTestButton;
        private System.Windows.Forms.TextBox MXFPreviewText;
        private System.Windows.Forms.TextBox MxfLineupUidTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox MxfLineupNameTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button AffiliateXPathTestButton;
        private System.Windows.Forms.Button AffiliateXPathRevertButton;
        private System.Windows.Forms.TextBox AffiliateTextBox;
        private System.Windows.Forms.Label label9;
    }
}

