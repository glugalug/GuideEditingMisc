using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LabeledValueControl
{
    public partial class LabeledValue : UserControl
    {
        public LabeledValue()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        [Description("Caption to show on label (left side) of control"),
         DefaultValue("Label:"), Browsable(true),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string LabelCaption
        {
            get { return LabelLabel.Text; }
            set { LabelLabel.Text = value; }
        }

        [Description("Caption to show as value (right side) of control"),
         DefaultValue(""), Browsable(true),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ValueCaption
        {
            get { return ValueLabel.Text; }
            set { ValueLabel.Text = value; }
        }
    }
}
