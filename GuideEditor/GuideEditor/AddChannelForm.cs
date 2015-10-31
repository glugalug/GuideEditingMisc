using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.TV.Tuning;
using ErrorReporter;
using ChannelEditingLib;

namespace GuideEditor
{
    public partial class AddChannelForm : Form
    {
        public AddChannelForm()
        {
            InitializeComponent();
            PopulateModulationListBox();
            ListingComboBox.Items.Add("No listing");
            ListingComboBox.Items.AddRange(ChannelEditing.GetServices().ToArray());
            ListingComboBox.SelectedIndex = 0;
        }

        public Lineup lineup
        {
            get { return lineup_; }
            set
            {
                lineup_ = value;
                ModulationType modulation_type;
                if (ChannelEditing.TryGetLineupModulation(lineup_, out modulation_type))
                    SelectMatchingModulationType(modulation_type);
            }
        }

        private void PopulateModulationListBox()
        {
            ModulationListBox.Items.Clear();
            foreach (ModulationType modulation in Enum.GetValues(typeof(ModulationType)))
                ModulationListBox.Items.Add(new ModulationTypeWrapper(modulation));
        }

        private void SelectMatchingModulationType(ModulationType modulation_type)
        {
            foreach (object o in ModulationListBox.Items)
            {
                if ((o as ModulationTypeWrapper).modulation_type == modulation_type)
                {
                    ModulationListBox.SelectedItem = o;
                    return;
                }
            }
        }
        
        private Lineup lineup_;

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (ModulationListBox.SelectedItem == null)
            {
                MessageBox.Show("New channel must have a modulation type.");
                this.DialogResult = DialogResult.None;
                return;
            }

            if (CallsignInput.TextValue == "")
            {
                MessageBox.Show("Channel must have a callsign.");
                this.DialogResult = DialogResult.None;
                return;
            }

            try
            {
                ChannelEditing.AddUserChannelAndMergedChannelInLineup(
                    lineup_, CallsignInput.TextValue,
                    (int)ChannelNumberInput.NumberValue, (int)SubChannelInput.NumberValue,
                    (ModulationListBox.SelectedItem as ModulationTypeWrapper).modulation_type,
                    (int)GuideChannelNumberInput.NumberValue, (int)GuideSubchannelInput.NumberValue,
                    ListingComboBox.SelectedItem as Service);
            }
            catch (Exception exc)
            {
                ExceptionWithKeyValues key_valued_exception = new ExceptionWithKeyValues(exc);
                key_valued_exception.AddKeyValue("lineup", lineup_);
                key_valued_exception.AddKeyValue("Callsign", CallsignInput.TextValue);
                key_valued_exception.AddKeyValue("Subchannel", SubChannelInput.NumberValue);
                key_valued_exception.AddKeyValue("Modulation", ModulationListBox.SelectedItem);
                key_valued_exception.AddKeyValue("GuideNumber", GuideChannelNumberInput.NumberValue);
                key_valued_exception.AddKeyValue("GuideSubChannel", GuideSubchannelInput.NumberValue);
                key_valued_exception.AddKeyValue("Service", ListingComboBox.SelectedItem);
                new ErrorReportingForm("Exception occured when attempting to add a channel", key_valued_exception);
            }
            this.DialogResult = AddButton.DialogResult;
        }

        private void ChannelNumberInput_ValueChanged(object sender, EventArgs e)
        {
            GuideChannelNumberInput.NumberValue = ChannelNumberInput.NumberValue;
        }

        private void SubChannelInput_ValueChanged(object sender, EventArgs e)
        {
            GuideSubchannelInput.NumberValue = ChannelNumberInput.NumberValue;
        }
    }
}
