namespace TunerGroupLineupSelector
{
    partial class PerTunerLineupSelectionForm
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
            this.TunerGroupBox = new System.Windows.Forms.GroupBox();
            this.TunerSelectionListBox = new System.Windows.Forms.CheckedListBox();
            this.TunerGroupComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.WMILineupSelectionGroup = new System.Windows.Forms.GroupBox();
            this.WMILineupListBox = new System.Windows.Forms.CheckedListBox();
            this.FullMergeButton = new System.Windows.Forms.Button();
            this.TunerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.WMILineupSelectionGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // TunerGroupBox
            // 
            this.TunerGroupBox.Controls.Add(this.TunerSelectionListBox);
            this.TunerGroupBox.Controls.Add(this.TunerGroupComboBox);
            this.TunerGroupBox.Controls.Add(this.label1);
            this.TunerGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TunerGroupBox.Location = new System.Drawing.Point(0, 0);
            this.TunerGroupBox.Name = "TunerGroupBox";
            this.TunerGroupBox.Size = new System.Drawing.Size(314, 617);
            this.TunerGroupBox.TabIndex = 0;
            this.TunerGroupBox.TabStop = false;
            this.TunerGroupBox.Text = "Tuner Selection";
            // 
            // TunerSelectionListBox
            // 
            this.TunerSelectionListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TunerSelectionListBox.CheckOnClick = true;
            this.TunerSelectionListBox.FormattingEnabled = true;
            this.TunerSelectionListBox.Location = new System.Drawing.Point(6, 51);
            this.TunerSelectionListBox.Name = "TunerSelectionListBox";
            this.TunerSelectionListBox.Size = new System.Drawing.Size(302, 559);
            this.TunerSelectionListBox.TabIndex = 2;
            this.TunerSelectionListBox.ThreeDCheckBoxes = true;
            this.TunerSelectionListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckedTunersChanged);
            // 
            // TunerGroupComboBox
            // 
            this.TunerGroupComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TunerGroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TunerGroupComboBox.FormattingEnabled = true;
            this.TunerGroupComboBox.Location = new System.Drawing.Point(109, 24);
            this.TunerGroupComboBox.Name = "TunerGroupComboBox";
            this.TunerGroupComboBox.Size = new System.Drawing.Size(199, 21);
            this.TunerGroupComboBox.TabIndex = 1;
            this.TunerGroupComboBox.SelectedIndexChanged += new System.EventHandler(this.TunerGroupComboBox_SelectedIndexChanged);
            this.TunerGroupComboBox.SelectedValueChanged += new System.EventHandler(this.TunerGroupComboBox_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select tuner group:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TunerGroupBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.WMILineupSelectionGroup);
            this.splitContainer1.Size = new System.Drawing.Size(942, 617);
            this.splitContainer1.SplitterDistance = 314;
            this.splitContainer1.TabIndex = 1;
            // 
            // WMILineupSelectionGroup
            // 
            this.WMILineupSelectionGroup.Controls.Add(this.FullMergeButton);
            this.WMILineupSelectionGroup.Controls.Add(this.WMILineupListBox);
            this.WMILineupSelectionGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WMILineupSelectionGroup.Location = new System.Drawing.Point(0, 0);
            this.WMILineupSelectionGroup.Name = "WMILineupSelectionGroup";
            this.WMILineupSelectionGroup.Size = new System.Drawing.Size(624, 617);
            this.WMILineupSelectionGroup.TabIndex = 0;
            this.WMILineupSelectionGroup.TabStop = false;
            this.WMILineupSelectionGroup.Text = "WMI Lineup Selection";
            // 
            // WMILineupListBox
            // 
            this.WMILineupListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WMILineupListBox.CheckOnClick = true;
            this.WMILineupListBox.FormattingEnabled = true;
            this.WMILineupListBox.Location = new System.Drawing.Point(0, 66);
            this.WMILineupListBox.Name = "WMILineupListBox";
            this.WMILineupListBox.Size = new System.Drawing.Size(618, 544);
            this.WMILineupListBox.TabIndex = 0;
            this.WMILineupListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.WMILineupListBox_ItemCheck);
            // 
            // FullMergeButton
            // 
            this.FullMergeButton.Location = new System.Drawing.Point(225, 38);
            this.FullMergeButton.Name = "FullMergeButton";
            this.FullMergeButton.Size = new System.Drawing.Size(75, 23);
            this.FullMergeButton.TabIndex = 1;
            this.FullMergeButton.Text = "Full Merge";
            this.FullMergeButton.UseVisualStyleBackColor = true;
            this.FullMergeButton.Click += new System.EventHandler(this.FullMergeButton_Click);
            // 
            // PerTunerLineupSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 617);
            this.Controls.Add(this.splitContainer1);
            this.Name = "PerTunerLineupSelectionForm";
            this.Text = "Tuner Lineup Selector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PerTunerLineupSelectionForm_FormClosing);
            this.TunerGroupBox.ResumeLayout(false);
            this.TunerGroupBox.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.WMILineupSelectionGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox TunerGroupBox;
        private System.Windows.Forms.ComboBox TunerGroupComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox TunerSelectionListBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox WMILineupSelectionGroup;
        private System.Windows.Forms.CheckedListBox WMILineupListBox;
        private System.Windows.Forms.Button FullMergeButton;
    }
}

