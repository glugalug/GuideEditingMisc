namespace ChannelEditingLib
{
    partial class ListingSelectionForm
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
            this.CustomListingsRadioButton = new System.Windows.Forms.RadioButton();
            this.AllListingsRadioButton = new System.Windows.Forms.RadioButton();
            this.WmisDevicesRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SortNumberRadioButton = new System.Windows.Forms.RadioButton();
            this.SortNameRadioButton = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ListingSelectionListBox = new System.Windows.Forms.ListBox();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.CancelSelectionButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.ButtonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CustomListingsRadioButton);
            this.groupBox1.Controls.Add(this.AllListingsRadioButton);
            this.groupBox1.Controls.Add(this.WmisDevicesRadioButton);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 109);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose listing from:";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // CustomListingsRadioButton
            // 
            this.CustomListingsRadioButton.AutoSize = true;
            this.CustomListingsRadioButton.Location = new System.Drawing.Point(8, 73);
            this.CustomListingsRadioButton.Name = "CustomListingsRadioButton";
            this.CustomListingsRadioButton.Size = new System.Drawing.Size(243, 17);
            this.CustomListingsRadioButton.TabIndex = 2;
            this.CustomListingsRadioButton.TabStop = true;
            this.CustomListingsRadioButton.Text = "Listings already associated with the channel(s)";
            this.CustomListingsRadioButton.UseVisualStyleBackColor = true;
            this.CustomListingsRadioButton.Visible = false;
            this.CustomListingsRadioButton.Click += new System.EventHandler(this.ListingSourceRadioClicked);
            // 
            // AllListingsRadioButton
            // 
            this.AllListingsRadioButton.AutoSize = true;
            this.AllListingsRadioButton.Location = new System.Drawing.Point(9, 48);
            this.AllListingsRadioButton.Name = "AllListingsRadioButton";
            this.AllListingsRadioButton.Size = new System.Drawing.Size(113, 17);
            this.AllListingsRadioButton.TabIndex = 1;
            this.AllListingsRadioButton.Text = "ALL known listings";
            this.AllListingsRadioButton.UseVisualStyleBackColor = true;
            this.AllListingsRadioButton.Click += new System.EventHandler(this.ListingSourceRadioClicked);
            // 
            // WmisDevicesRadioButton
            // 
            this.WmisDevicesRadioButton.AutoSize = true;
            this.WmisDevicesRadioButton.Checked = true;
            this.WmisDevicesRadioButton.Location = new System.Drawing.Point(10, 24);
            this.WmisDevicesRadioButton.Name = "WmisDevicesRadioButton";
            this.WmisDevicesRadioButton.Size = new System.Drawing.Size(178, 17);
            this.WmisDevicesRadioButton.TabIndex = 0;
            this.WmisDevicesRadioButton.TabStop = true;
            this.WmisDevicesRadioButton.Text = "Lineups associated with devices";
            this.WmisDevicesRadioButton.UseVisualStyleBackColor = true;
            this.WmisDevicesRadioButton.Click += new System.EventHandler(this.ListingSourceRadioClicked);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.SortNumberRadioButton);
            this.groupBox2.Controls.Add(this.SortNameRadioButton);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(298, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(290, 109);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sort by:";
            // 
            // SortNumberRadioButton
            // 
            this.SortNumberRadioButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SortNumberRadioButton.AutoSize = true;
            this.SortNumberRadioButton.Location = new System.Drawing.Point(91, 50);
            this.SortNumberRadioButton.Name = "SortNumberRadioButton";
            this.SortNumberRadioButton.Size = new System.Drawing.Size(62, 17);
            this.SortNumberRadioButton.TabIndex = 1;
            this.SortNumberRadioButton.Text = "Number";
            this.SortNumberRadioButton.UseVisualStyleBackColor = true;
            this.SortNumberRadioButton.Click += new System.EventHandler(this.SortRadioClicked);
            // 
            // SortNameRadioButton
            // 
            this.SortNameRadioButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SortNameRadioButton.AutoSize = true;
            this.SortNameRadioButton.Checked = true;
            this.SortNameRadioButton.Location = new System.Drawing.Point(91, 27);
            this.SortNameRadioButton.Name = "SortNameRadioButton";
            this.SortNameRadioButton.Size = new System.Drawing.Size(53, 17);
            this.SortNameRadioButton.TabIndex = 0;
            this.SortNameRadioButton.TabStop = true;
            this.SortNameRadioButton.Text = "Name";
            this.SortNameRadioButton.UseVisualStyleBackColor = true;
            this.SortNameRadioButton.Click += new System.EventHandler(this.SortRadioClicked);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ButtonPanel, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(591, 360);
            this.tableLayoutPanel1.TabIndex = 2;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // groupBox3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox3, 2);
            this.groupBox3.Controls.Add(this.ListingSelectionListBox);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(585, 209);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Select listing:";
            // 
            // ListingSelectionListBox
            // 
            this.ListingSelectionListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListingSelectionListBox.FormattingEnabled = true;
            this.ListingSelectionListBox.Location = new System.Drawing.Point(3, 16);
            this.ListingSelectionListBox.Name = "ListingSelectionListBox";
            this.ListingSelectionListBox.Size = new System.Drawing.Size(579, 186);
            this.ListingSelectionListBox.TabIndex = 0;
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Controls.Add(this.CancelSelectionButton);
            this.ButtonPanel.Controls.Add(this.OKButton);
            this.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonPanel.Location = new System.Drawing.Point(298, 333);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(290, 24);
            this.ButtonPanel.TabIndex = 3;
            // 
            // CancelSelectionButton
            // 
            this.CancelSelectionButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelSelectionButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.CancelSelectionButton.Location = new System.Drawing.Point(140, 0);
            this.CancelSelectionButton.Name = "CancelSelectionButton";
            this.CancelSelectionButton.Size = new System.Drawing.Size(75, 24);
            this.CancelSelectionButton.TabIndex = 1;
            this.CancelSelectionButton.Text = "Cancel";
            this.CancelSelectionButton.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.OKButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OKButton.Location = new System.Drawing.Point(215, 0);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 24);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // ListingSelectionForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelSelectionButton;
            this.ClientSize = new System.Drawing.Size(591, 360);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ListingSelectionForm";
            this.Text = "Select a listing to associate with the channel(s):";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ButtonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton CustomListingsRadioButton;
        private System.Windows.Forms.RadioButton AllListingsRadioButton;
        private System.Windows.Forms.RadioButton WmisDevicesRadioButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton SortNumberRadioButton;
        private System.Windows.Forms.RadioButton SortNameRadioButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox ListingSelectionListBox;
        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.Button CancelSelectionButton;
        private System.Windows.Forms.Button OKButton;
    }
}