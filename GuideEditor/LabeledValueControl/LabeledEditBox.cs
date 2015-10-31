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
    public partial class LabeledEditBox : UserControl
    {
        public LabeledEditBox()
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
              TextInput.Left = LabelLabel.Right + 15;
              TextInput.Width = Width - TextInput.Left - 4;
            }
        }

        [Description("Value in the edit text box"),
         DefaultValue(""), Browsable(true),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string TextValue
        {
            get { return TextInput.Text; }
            set
            { 
                TextInput.Text = value;
                if (TextValueChanged != null)
                    TextValueChanged(this, new EventArgs());
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        public event EventHandler TextValueChanged;

    }
}
