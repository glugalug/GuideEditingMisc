using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LabeledValueControl
{
    public partial class LabeledNumberInput : UserControl
    {
        public LabeledNumberInput()
        {
            InitializeComponent();
        }
        [Description("Caption to show on label (left side) of control"),
         DefaultValue("Label:"), Browsable(true),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string LabelCaption
        {
            get { return LabelLabel.Text; }
            set
            {
                LabelLabel.Text = value;
            }
        }

        [Description("Value in the edit box"),
         DefaultValue(0), Browsable(true),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public decimal NumberValue
        {
            get { return numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }
        [DefaultValue(65535), Browsable(true),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public decimal Maximum
        {
            get { return numericUpDown1.Maximum; }
            set { numericUpDown1.Maximum = value;}
        }

        [DefaultValue(0), Browsable(true),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public decimal Minimum
        {
            get { return numericUpDown1.Minimum; }
            set { numericUpDown1.Minimum = value; }
        }

        [DefaultValue(0), Browsable(true),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DecimalPlaces
        {
            get { return numericUpDown1.DecimalPlaces; }
            set { numericUpDown1.DecimalPlaces = value; }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler ValueChanged;

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            }
        }
    }
}
