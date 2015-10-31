namespace CableCARDUserChannelTagger
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
            this.UserChannelsListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LineupComboBox = new System.Windows.Forms.ComboBox();
            this.TaggingOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CallsignTagInput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AppendRadioButton = new System.Windows.Forms.RadioButton();
            this.ReplaceRadioButton = new System.Windows.Forms.RadioButton();
            this.PrependRadioButton = new System.Windows.Forms.RadioButton();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.AddOrphanButton = new System.Windows.Forms.Button();
            this.TaggingOptionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User channels found in selected lineup:";
            // 
            // UserChannelsListBox
            // 
            this.UserChannelsListBox.FormattingEnabled = true;
            this.UserChannelsListBox.Location = new System.Drawing.Point(12, 64);
            this.UserChannelsListBox.Name = "UserChannelsListBox";
            this.UserChannelsListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.UserChannelsListBox.Size = new System.Drawing.Size(329, 186);
            this.UserChannelsListBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Lineup:";
            // 
            // LineupComboBox
            // 
            this.LineupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LineupComboBox.FormattingEnabled = true;
            this.LineupComboBox.Location = new System.Drawing.Point(60, 16);
            this.LineupComboBox.Name = "LineupComboBox";
            this.LineupComboBox.Size = new System.Drawing.Size(281, 21);
            this.LineupComboBox.TabIndex = 3;
            this.LineupComboBox.SelectedIndexChanged += new System.EventHandler(this.LineupComboBox_SelectedIndexChanged);
            // 
            // TaggingOptionsGroupBox
            // 
            this.TaggingOptionsGroupBox.Controls.Add(this.label4);
            this.TaggingOptionsGroupBox.Controls.Add(this.CallsignTagInput);
            this.TaggingOptionsGroupBox.Controls.Add(this.label3);
            this.TaggingOptionsGroupBox.Controls.Add(this.AppendRadioButton);
            this.TaggingOptionsGroupBox.Controls.Add(this.ReplaceRadioButton);
            this.TaggingOptionsGroupBox.Controls.Add(this.PrependRadioButton);
            this.TaggingOptionsGroupBox.Location = new System.Drawing.Point(347, 64);
            this.TaggingOptionsGroupBox.Name = "TaggingOptionsGroupBox";
            this.TaggingOptionsGroupBox.Size = new System.Drawing.Size(253, 134);
            this.TaggingOptionsGroupBox.TabIndex = 4;
            this.TaggingOptionsGroupBox.TabStop = false;
            this.TaggingOptionsGroupBox.Text = "Tagging Style";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(0, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(247, 33);
            this.label4.TabIndex = 5;
            this.label4.Text = "to easily spot them in the Add Missing Channels screen";
            // 
            // CallsignTagInput
            // 
            this.CallsignTagInput.Location = new System.Drawing.Point(6, 71);
            this.CallsignTagInput.Name = "CallsignTagInput";
            this.CallsignTagInput.Size = new System.Drawing.Size(187, 20);
            this.CallsignTagInput.TabIndex = 4;
            this.CallsignTagInput.Text = "_CCARD_";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 29);
            this.label3.TabIndex = 3;
            this.label3.Text = "Callsigns for user channels in this lineup with";
            // 
            // AppendRadioButton
            // 
            this.AppendRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AppendRadioButton.AutoSize = true;
            this.AppendRadioButton.Location = new System.Drawing.Point(185, 19);
            this.AppendRadioButton.Name = "AppendRadioButton";
            this.AppendRadioButton.Size = new System.Drawing.Size(62, 17);
            this.AppendRadioButton.TabIndex = 2;
            this.AppendRadioButton.TabStop = true;
            this.AppendRadioButton.Text = "Append";
            this.AppendRadioButton.UseVisualStyleBackColor = true;
            // 
            // ReplaceRadioButton
            // 
            this.ReplaceRadioButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ReplaceRadioButton.AutoSize = true;
            this.ReplaceRadioButton.Location = new System.Drawing.Point(94, 19);
            this.ReplaceRadioButton.Name = "ReplaceRadioButton";
            this.ReplaceRadioButton.Size = new System.Drawing.Size(65, 17);
            this.ReplaceRadioButton.TabIndex = 1;
            this.ReplaceRadioButton.TabStop = true;
            this.ReplaceRadioButton.Text = "Replace";
            this.ReplaceRadioButton.UseVisualStyleBackColor = true;
            // 
            // PrependRadioButton
            // 
            this.PrependRadioButton.AutoSize = true;
            this.PrependRadioButton.Checked = true;
            this.PrependRadioButton.Location = new System.Drawing.Point(6, 19);
            this.PrependRadioButton.Name = "PrependRadioButton";
            this.PrependRadioButton.Size = new System.Drawing.Size(65, 17);
            this.PrependRadioButton.TabIndex = 0;
            this.PrependRadioButton.TabStop = true;
            this.PrependRadioButton.Text = "Prepend";
            this.PrependRadioButton.UseVisualStyleBackColor = true;
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(353, 215);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(241, 23);
            this.UpdateButton.TabIndex = 5;
            this.UpdateButton.Text = "Update Call Signs";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // AddOrphanButton
            // 
            this.AddOrphanButton.Location = new System.Drawing.Point(402, 19);
            this.AddOrphanButton.Name = "AddOrphanButton";
            this.AddOrphanButton.Size = new System.Drawing.Size(170, 23);
            this.AddOrphanButton.TabIndex = 6;
            this.AddOrphanButton.Text = "Add Orphaned Channels";
            this.AddOrphanButton.UseVisualStyleBackColor = true;
            this.AddOrphanButton.Click += new System.EventHandler(this.AddOrphanButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 265);
            this.Controls.Add(this.AddOrphanButton);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.TaggingOptionsGroupBox);
            this.Controls.Add(this.LineupComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.UserChannelsListBox);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "CableCARD Lineup User Channel Tagger";
            this.TaggingOptionsGroupBox.ResumeLayout(false);
            this.TaggingOptionsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox UserChannelsListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox LineupComboBox;
        private System.Windows.Forms.GroupBox TaggingOptionsGroupBox;
        private System.Windows.Forms.RadioButton AppendRadioButton;
        private System.Windows.Forms.RadioButton ReplaceRadioButton;
        private System.Windows.Forms.RadioButton PrependRadioButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox CallsignTagInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Button AddOrphanButton;
    }
}

