namespace GuideEditor2
{
    partial class ChannelMergingForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PrimaryChannelSelectionPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.DestinationChannelComboBox = new System.Windows.Forms.ComboBox();
            this.ButtonsPanel = new System.Windows.Forms.Panel();
            this.MergeButton = new System.Windows.Forms.Button();
            this.CancelMergeButton = new System.Windows.Forms.Button();
            this.SortingPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.RemoveChannelsCheckbox = new System.Windows.Forms.CheckBox();
            this.ChannelSortingListBox = new System.Windows.Forms.ListBox();
            this.UpButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.PrimaryChannelSelectionPanel.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            this.SortingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.PrimaryChannelSelectionPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ButtonsPanel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.SortingPanel, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(771, 263);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // PrimaryChannelSelectionPanel
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.PrimaryChannelSelectionPanel, 2);
            this.PrimaryChannelSelectionPanel.Controls.Add(this.DestinationChannelComboBox);
            this.PrimaryChannelSelectionPanel.Controls.Add(this.label1);
            this.PrimaryChannelSelectionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PrimaryChannelSelectionPanel.Location = new System.Drawing.Point(3, 3);
            this.PrimaryChannelSelectionPanel.Name = "PrimaryChannelSelectionPanel";
            this.PrimaryChannelSelectionPanel.Size = new System.Drawing.Size(765, 29);
            this.PrimaryChannelSelectionPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(401, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select channel to merge into (will retain number, callsign, and listing of this c" +
                "hannel):";
            // 
            // DestinationChannelComboBox
            // 
            this.DestinationChannelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DestinationChannelComboBox.FormattingEnabled = true;
            this.DestinationChannelComboBox.Location = new System.Drawing.Point(410, 3);
            this.DestinationChannelComboBox.Name = "DestinationChannelComboBox";
            this.DestinationChannelComboBox.Size = new System.Drawing.Size(352, 21);
            this.DestinationChannelComboBox.TabIndex = 1;
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.CancelMergeButton);
            this.ButtonsPanel.Controls.Add(this.MergeButton);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.Location = new System.Drawing.Point(388, 231);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(380, 29);
            this.ButtonsPanel.TabIndex = 1;
            // 
            // MergeButton
            // 
            this.MergeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.MergeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.MergeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MergeButton.Location = new System.Drawing.Point(305, 0);
            this.MergeButton.Name = "MergeButton";
            this.MergeButton.Size = new System.Drawing.Size(75, 29);
            this.MergeButton.TabIndex = 0;
            this.MergeButton.Text = "Merge";
            this.MergeButton.UseVisualStyleBackColor = true;
            this.MergeButton.Click += new System.EventHandler(this.MergeButton_Click);
            // 
            // CancelMergeButton
            // 
            this.CancelMergeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelMergeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.CancelMergeButton.Location = new System.Drawing.Point(230, 0);
            this.CancelMergeButton.Name = "CancelMergeButton";
            this.CancelMergeButton.Size = new System.Drawing.Size(75, 29);
            this.CancelMergeButton.TabIndex = 1;
            this.CancelMergeButton.Text = "Cancel";
            this.CancelMergeButton.UseVisualStyleBackColor = true;
            // 
            // SortingPanel
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.SortingPanel, 2);
            this.SortingPanel.Controls.Add(this.DownButton);
            this.SortingPanel.Controls.Add(this.UpButton);
            this.SortingPanel.Controls.Add(this.ChannelSortingListBox);
            this.SortingPanel.Controls.Add(this.RemoveChannelsCheckbox);
            this.SortingPanel.Controls.Add(this.label2);
            this.SortingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SortingPanel.Location = new System.Drawing.Point(3, 38);
            this.SortingPanel.Name = "SortingPanel";
            this.SortingPanel.Size = new System.Drawing.Size(765, 187);
            this.SortingPanel.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(441, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Sort the channels to be merged in the order you would like the associated tuners " +
                "to be used:";
            // 
            // RemoveChannelsCheckbox
            // 
            this.RemoveChannelsCheckbox.AutoSize = true;
            this.RemoveChannelsCheckbox.Location = new System.Drawing.Point(6, 3);
            this.RemoveChannelsCheckbox.Name = "RemoveChannelsCheckbox";
            this.RemoveChannelsCheckbox.Size = new System.Drawing.Size(182, 17);
            this.RemoveChannelsCheckbox.TabIndex = 1;
            this.RemoveChannelsCheckbox.Text = "Remove channels after merging?";
            this.RemoveChannelsCheckbox.UseVisualStyleBackColor = true;
            // 
            // ChannelSortingListBox
            // 
            this.ChannelSortingListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelSortingListBox.FormattingEnabled = true;
            this.ChannelSortingListBox.Location = new System.Drawing.Point(9, 49);
            this.ChannelSortingListBox.Name = "ChannelSortingListBox";
            this.ChannelSortingListBox.Size = new System.Drawing.Size(714, 121);
            this.ChannelSortingListBox.TabIndex = 2;
            // 
            // UpButton
            // 
            this.UpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpButton.Location = new System.Drawing.Point(729, 49);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(27, 44);
            this.UpButton.TabIndex = 3;
            this.UpButton.Text = "↑";
            this.UpButton.UseVisualStyleBackColor = true;
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // DownButton
            // 
            this.DownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownButton.Location = new System.Drawing.Point(729, 120);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(27, 50);
            this.DownButton.TabIndex = 4;
            this.DownButton.Text = "↓";
            this.DownButton.UseVisualStyleBackColor = true;
            this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // ChannelMergingForm
            // 
            this.AcceptButton = this.MergeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelMergeButton;
            this.ClientSize = new System.Drawing.Size(771, 263);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ChannelMergingForm";
            this.Text = "Combine merged channels";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.PrimaryChannelSelectionPanel.ResumeLayout(false);
            this.PrimaryChannelSelectionPanel.PerformLayout();
            this.ButtonsPanel.ResumeLayout(false);
            this.SortingPanel.ResumeLayout(false);
            this.SortingPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel PrimaryChannelSelectionPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox DestinationChannelComboBox;
        private System.Windows.Forms.Panel ButtonsPanel;
        private System.Windows.Forms.Button CancelMergeButton;
        private System.Windows.Forms.Button MergeButton;
        private System.Windows.Forms.Panel SortingPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.ListBox ChannelSortingListBox;
        private System.Windows.Forms.CheckBox RemoveChannelsCheckbox;
        private System.Windows.Forms.Button DownButton;
    }
}