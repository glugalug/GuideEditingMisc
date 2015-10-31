namespace GuideEditor
{
    partial class AddChannelForm
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
            this.CancelAddingButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ModulationListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SubChannelInput = new LabeledValueControl.LabeledNumberInput();
            this.ChannelNumberInput = new LabeledValueControl.LabeledNumberInput();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.GuideSubchannelInput = new LabeledValueControl.LabeledNumberInput();
            this.GuideChannelNumberInput = new LabeledValueControl.LabeledNumberInput();
            this.CallsignInput = new LabeledValueControl.LabeledEditBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ListingComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // CancelAddingButton
            // 
            this.CancelAddingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelAddingButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelAddingButton.Location = new System.Drawing.Point(497, 262);
            this.CancelAddingButton.Name = "CancelAddingButton";
            this.CancelAddingButton.Size = new System.Drawing.Size(75, 23);
            this.CancelAddingButton.TabIndex = 0;
            this.CancelAddingButton.Text = "Cancel";
            this.CancelAddingButton.UseVisualStyleBackColor = true;
            // 
            // AddButton
            // 
            this.AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.AddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddButton.Location = new System.Drawing.Point(374, 262);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(117, 23);
            this.AddButton.TabIndex = 1;
            this.AddButton.Text = "Add Channel";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ModulationListBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.SubChannelInput);
            this.groupBox1.Controls.Add(this.ChannelNumberInput);
            this.groupBox1.Location = new System.Drawing.Point(12, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(564, 136);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Physical Channel";
            // 
            // ModulationListBox
            // 
            this.ModulationListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ModulationListBox.FormattingEnabled = true;
            this.ModulationListBox.Location = new System.Drawing.Point(413, 20);
            this.ModulationListBox.Name = "ModulationListBox";
            this.ModulationListBox.Size = new System.Drawing.Size(145, 108);
            this.ModulationListBox.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(342, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Modulation:";
            // 
            // SubChannelInput
            // 
            this.SubChannelInput.LabelCaption = "SubChannel:";
            this.SubChannelInput.Location = new System.Drawing.Point(189, 20);
            this.SubChannelInput.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.SubChannelInput.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SubChannelInput.Name = "SubChannelInput";
            this.SubChannelInput.NumberValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SubChannelInput.Size = new System.Drawing.Size(150, 28);
            this.SubChannelInput.TabIndex = 8;
            this.SubChannelInput.ValueChanged += new System.EventHandler(this.SubChannelInput_ValueChanged);
            // 
            // ChannelNumberInput
            // 
            this.ChannelNumberInput.LabelCaption = "Channel number:";
            this.ChannelNumberInput.Location = new System.Drawing.Point(14, 20);
            this.ChannelNumberInput.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ChannelNumberInput.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ChannelNumberInput.Name = "ChannelNumberInput";
            this.ChannelNumberInput.NumberValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ChannelNumberInput.Size = new System.Drawing.Size(169, 28);
            this.ChannelNumberInput.TabIndex = 7;
            this.ChannelNumberInput.ValueChanged += new System.EventHandler(this.ChannelNumberInput_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.GuideSubchannelInput);
            this.groupBox2.Controls.Add(this.GuideChannelNumberInput);
            this.groupBox2.Location = new System.Drawing.Point(12, 187);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(564, 58);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Guide Channel Number";
            // 
            // GuideSubchannelInput
            // 
            this.GuideSubchannelInput.LabelCaption = "SubChannel:";
            this.GuideSubchannelInput.Location = new System.Drawing.Point(189, 19);
            this.GuideSubchannelInput.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.GuideSubchannelInput.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.GuideSubchannelInput.Name = "GuideSubchannelInput";
            this.GuideSubchannelInput.NumberValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.GuideSubchannelInput.Size = new System.Drawing.Size(150, 28);
            this.GuideSubchannelInput.TabIndex = 1;
            // 
            // GuideChannelNumberInput
            // 
            this.GuideChannelNumberInput.LabelCaption = "Channel number:";
            this.GuideChannelNumberInput.Location = new System.Drawing.Point(6, 19);
            this.GuideChannelNumberInput.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.GuideChannelNumberInput.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.GuideChannelNumberInput.Name = "GuideChannelNumberInput";
            this.GuideChannelNumberInput.NumberValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.GuideChannelNumberInput.Size = new System.Drawing.Size(177, 28);
            this.GuideChannelNumberInput.TabIndex = 0;
            // 
            // CallsignInput
            // 
            this.CallsignInput.LabelCaption = "Callsign:";
            this.CallsignInput.Location = new System.Drawing.Point(12, 12);
            this.CallsignInput.Name = "CallsignInput";
            this.CallsignInput.Size = new System.Drawing.Size(224, 27);
            this.CallsignInput.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Listing:";
            // 
            // ListingComboBox
            // 
            this.ListingComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ListingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ListingComboBox.FormattingEnabled = true;
            this.ListingComboBox.Location = new System.Drawing.Point(285, 16);
            this.ListingComboBox.Name = "ListingComboBox";
            this.ListingComboBox.Size = new System.Drawing.Size(290, 21);
            this.ListingComboBox.TabIndex = 10;
            // 
            // AddChannelForm
            // 
            this.AcceptButton = this.AddButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelAddingButton;
            this.ClientSize = new System.Drawing.Size(584, 297);
            this.Controls.Add(this.ListingComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CallsignInput);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.CancelAddingButton);
            this.Name = "AddChannelForm";
            this.Text = "AddChannelForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelAddingButton;
        private System.Windows.Forms.Button AddButton;
        private LabeledValueControl.LabeledEditBox CallsignInput;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox ModulationListBox;
        private System.Windows.Forms.Label label1;
        private LabeledValueControl.LabeledNumberInput SubChannelInput;
        private LabeledValueControl.LabeledNumberInput ChannelNumberInput;
        private System.Windows.Forms.GroupBox groupBox2;
        private LabeledValueControl.LabeledNumberInput GuideChannelNumberInput;
        private LabeledValueControl.LabeledNumberInput GuideSubchannelInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ListingComboBox;
    }
}