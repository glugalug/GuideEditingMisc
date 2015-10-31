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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            object_store_ = Microsoft.MediaCenter.Store.ObjectStore.DefaultSingleton;
            PopulateLineupsListBox();
            PopulateModulationComboBox();
            SelectFirstScannedLineup();
            PopulateListingComboBox();
        }

        private void PopulateListingComboBox()
        {
            ListingComboBox.Items.Clear();
            ListingComboBox.Items.Add("No listing");
            ListingComboBox.Items.AddRange(ChannelEditing.GetServices().ToArray());
        }

        private void PopulateLineupsListBox()
        {
            LineupsComboBox.Items.Clear();
            LineupsComboBox.Items.AddRange(ChannelEditing.GetLineups());
        }

        private void SelectFirstScannedLineup()
        {
            foreach (Lineup lineup in LineupsComboBox.Items)
            {
                if (!lineup.ScanDevices.Empty)
                {
                    LineupsComboBox.SelectedItem = lineup;
                    break;
                }
            }
        }

        private void PopulateUserChannelsListBox()
        {
            UserChannelsListBox.Items.Clear();
            if (selected_lineup_ == null) return;

            UserChannelsListBox.Items.AddRange(ChannelEditing.GetUserAddedChannelsForLineup(selected_lineup_).ToArray());
            UserChannelsListBox.Refresh();
        }
        private Microsoft.MediaCenter.Store.ObjectStore object_store_ = null;

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (object_store_ != null)
            {
                object_store_.Dispose();
                object_store_ = null;
            }
        }

        private void PopulateTuningInfosListBox()
        {
            ChannelTuningInfosListBox.Items.Clear();
            if (selected_channel_ == null) return;
            foreach (TuningInfo tuning_info in selected_channel_.TuningInfos)
            {
                ChannelTuningInfo channel_tuning_info = tuning_info as ChannelTuningInfo;
                ChannelTuningInfosListBox.Items.Add(new ChannelTuningInfoListBoxWrapper(channel_tuning_info));
            }
            if (!selected_channel_.TuningInfos.Empty)
            {
                ChannelTuningInfosListBox.SelectedIndex = 0;
            }
           // this.richTextBox1.AppendText(selected_channel_.TuningInfos.First.Device.DeviceType.TuningSpace.ToString());
        }

        private void SelectListingForChannel()
        {
            if (selected_channel_ == null || selected_channel_.Service == null)
            {
                ListingComboBox.SelectedIndex = 0;
            }
            else
            {
                ListingComboBox.SelectedItem = selected_channel_.Service;
            }
        }
        private void UserChannelsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateTuningInfosListBox();
            if (selected_channel_ == null) return;
            CallsignEdit.TextValue = selected_channel_.CallSign;
            ChannelNumberLabel.ValueCaption = selected_channel_.ChannelNumber.ToString();
            ChannelOriginalNumberLabel.ValueCaption = selected_channel_.OriginalNumber.ToString();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Lineup selected_lineup_
        { 
            get { return LineupsComboBox.SelectedItem as Lineup; }
        }

        private Channel selected_channel_
        {
            get { return UserChannelsListBox.SelectedItem as Channel; }
        }

        private void PopulateScanDevicesListbox()
        {
            ScanDevicesListBox.Items.Clear();
            Devices scan_devices = selected_lineup_.ScanDevices;
            foreach (Device device in scan_devices)
            {
                ScanDevicesListBox.Items.Add(new DeviceWrapper(device));
            }
        }

        private void PopulateWmisDevicesListbox()
        {
            WmisDevicesListBox.Items.Clear();
            Devices wmis_devices = selected_lineup_.WmisDevices;
            foreach (Device device in wmis_devices)
            {
                WmisDevicesListBox.Items.Add(new DeviceWrapper(device));
            }
        }

        private void PopulateModulationComboBox()
        {
            ModulationTypeComboBox.Items.Clear();
            foreach (ModulationType modulation_type in Enum.GetValues(typeof(ModulationType)))
            {
                ModulationTypeComboBox.Items.Add(new ModulationTypeWrapper(modulation_type));
            }
        }

        private void LineupsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selected_lineup_ == null) return;
            LineupTypesLabel.ValueCaption = selected_lineup_.LineupTypes;
            AlternateLineupTypesLabel.ValueCaption = selected_lineup_.AlternateLineupTypes;
            PopulateScanDevicesListbox();
            PopulateWmisDevicesListbox();
            DeviceGroupLabel.ValueCaption =
                (selected_lineup_.DeviceGroup != null) ? selected_lineup_.DeviceGroup.Name : "";
            PopulateUserChannelsListBox();
            if (UserChannelsListBox.Items.Count > 0)
            {
                UserChannelsListBox.SelectedIndex = 0;
            }

        }

        private void RemoveTuningInfoClick(object sender, EventArgs e)
        {
            if (selected_tuning_info_ == null) return;
            DialogResult dr = MessageBox.Show("Delete selected tuning info from the selected channel?", "Are you sure?", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    ChannelTuningInfo tuning_info = selected_tuning_info_;
                    selected_channel_.TuningInfos.RemoveAllMatching(tuning_info);
                    foreach (MergedChannel merged_channel in selected_channel_.ReferencingPrimaryChannels)
                    {
                        merged_channel.TuningInfos.RemoveAllMatching(tuning_info);
                    }
                    foreach (MergedChannel merged_channel in selected_channel_.ReferencingSecondaryChannels)
                    {
                        merged_channel.TuningInfos.RemoveAllMatching(tuning_info);
                    }
                }
                catch (Exception exc)
                {
                    ExceptionWithKeyValues key_valued_exception = new ExceptionWithKeyValues(exc);
                    key_valued_exception.AddKeyValue("tuning info", selected_tuning_info_);
                    key_valued_exception.AddKeyValue("channel", selected_channel_);
                    new ErrorReportingForm("Exception occured when deleting a tuning info", key_valued_exception);
                }
            }
        }

        private ChannelTuningInfo selected_tuning_info_
        {
            get
            {
                if (ChannelTuningInfosListBox.SelectedItem == null)
                    return null;
                return (ChannelTuningInfosListBox.SelectedItem as ChannelTuningInfoListBoxWrapper).channel_tuning_info;
            }
        }

        private void ChannelUpdateButton_Click(object sender, EventArgs e)
        {
            if (selected_channel_ == null) return;
            selected_channel_.CallSign = CallsignEdit.TextValue;
            UserChannelsListBox.Refresh();
        }

        private void SetSelectedModulation(ModulationType modulation_type)
        {
            foreach (Object o in ModulationTypeComboBox.Items)
            {
                if ((o as ModulationTypeWrapper).modulation_type == modulation_type)
                {
                    ModulationTypeComboBox.SelectedItem = o;
                }
            }
        }

        private void ChannelTuningInfosListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selected_tuning_info_ == null)
            {
                PhysicalNumberInput.Visible = false;
                SubChannelLabel.Visible = false;
                LogicalChannelInput.Visible = false;
                ModulationTypeComboBox.Visible = false;
            }
            else
            {
                PhysicalNumberInput.NumberValue = selected_tuning_info_.PhysicalNumber;
                SubChannelLabel.ValueCaption = selected_tuning_info_.SubNumber.ToString();
                LogicalChannelInput.NumberValue = selected_tuning_info_.LogicalNumber;
                SetSelectedModulation(selected_tuning_info_.ModulationType);
                PhysicalNumberInput.Visible = true;
                SubChannelLabel.Visible = true;
                LogicalChannelInput.Visible = true;
                ModulationTypeComboBox.Visible = true;
            }
        }

        private void UpdateTuningInfoButton_Click(object sender, EventArgs e)
        {
            if (selected_tuning_info_ == null) return;
            try
            {
                selected_tuning_info_.PhysicalNumber = Decimal.ToInt32(PhysicalNumberInput.NumberValue);
                selected_tuning_info_.LogicalNumber = decimal.ToInt32(LogicalChannelInput.NumberValue);
                selected_tuning_info_.ModulationType = (ModulationTypeComboBox.SelectedItem as ModulationTypeWrapper).modulation_type;
            }
            catch (Exception exc)
            {
                ExceptionWithKeyValues key_valued_exception = new ExceptionWithKeyValues(exc);
                key_valued_exception.AddKeyValue("tuning info", selected_tuning_info_);
                key_valued_exception.AddKeyValue("Physical number", PhysicalNumberInput.NumberValue);
                key_valued_exception.AddKeyValue("Logical number", LogicalChannelInput.NumberValue);
                key_valued_exception.AddKeyValue("Modulation type", ModulationTypeComboBox.SelectedItem);
                new ErrorReportingForm("Exception occured while updating tuning info properties", key_valued_exception);
            }
        }

        private void ChannelUpdateButton_Click_1(object sender, EventArgs e)
        {
            Service selected_listing = (ListingComboBox.SelectedIndex > 0) ? ListingComboBox.SelectedItem as Service : null;
            try
            {
                selected_channel_.CallSign = CallsignEdit.TextValue;
                if (selected_channel_.Service != selected_listing)
                {
                    DialogResult dr = MessageBox.Show("Update the listing of guide entries tied to this channel?",
                        "Listing selection for channel has been changed", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        foreach (MergedChannel merged_channel in selected_channel_.ReferencingPrimaryChannels)
                        {
                            merged_channel.Service = selected_listing;
                            merged_channel.Update();
                        }
                        foreach (MergedChannel merged_channel in selected_channel_.ReferencingSecondaryChannels)
                        {
                            merged_channel.Service = selected_listing;
                            merged_channel.Update();
                        }
                    }
                    selected_channel_.Service = ListingComboBox.SelectedItem as Service;
                }
            }
            catch (Exception exc)
            {
                ExceptionWithKeyValues key_valued_exception = new ExceptionWithKeyValues(exc);
                key_valued_exception.AddKeyValue("Channel", selected_channel_);
                key_valued_exception.AddKeyValue("Callsign", CallsignEdit.TextValue);
                key_valued_exception.AddKeyValue("Service", selected_listing);
                key_valued_exception.AddKeyValue("Selected Channel Service", selected_channel_.Service);
                key_valued_exception.AddKeyValue("Service is different", selected_listing != selected_channel_.Service);
                new ErrorReportingForm("Exception occured when trying to update channel properties.", key_valued_exception);
            }
        }

        private void AddChannelButton_Click(object sender, EventArgs e)
        {
            if (selected_lineup_ == null) return;
            AddChannelForm add_channel_form = new AddChannelForm();
            add_channel_form.lineup = selected_lineup_;
            add_channel_form.ShowDialog();
        }

        private void RemoveChannelButton_Click(object sender, EventArgs e)
        {
            if (selected_channel_ == null) return;
            DialogResult dr = MessageBox.Show("Are you sure you want to remove channel ("
                + selected_channel_.ToString() + ") from the lineup?", "Confirm deletion", MessageBoxButtons.YesNo);
            if (dr != DialogResult.Yes) return;
            try
            {
                ChannelEditing.DeleteChannel(selected_channel_);
            }
            catch (Exception exc)
            {
                ExceptionWithKeyValues key_valued_exception = new ExceptionWithKeyValues(exc);
                key_valued_exception.AddKeyValue("Channel", selected_channel_);
                new ErrorReportingForm("Exception occured when deleting a channel.", key_valued_exception);
            }
            PopulateUserChannelsListBox();
        }
    }

    class ChannelTuningInfoListBoxWrapper
    {
        public ChannelTuningInfoListBoxWrapper(ChannelTuningInfo ti)
        {
            channel_tuning_info_ = ti;
        }
        public ChannelTuningInfo channel_tuning_info
        {
            get { return channel_tuning_info_; }
        }
        private ChannelTuningInfo channel_tuning_info_;
        public override string ToString()
        {
            return channel_tuning_info_.Device.Name + " (" + channel_tuning_info_.ModulationType.ToString() + ")";
        }
    }

    class DeviceWrapper
    {
        public DeviceWrapper(Device device)
        {
            device_ = device;
        }
        private Device device_;

        public Device device
        {
            get { return device_; }
        }
        public override string ToString()
        {
            return device_.Name;
        }
    }

    class ChannelListBoxWrapper
    {
        public ChannelListBoxWrapper(Channel ch)
        {
            channel_ = ch;
        }
        private Channel channel_;
        public Channel channel
        {
            get { return channel_; }
        }
        public override string ToString()
        {
            return channel_.CallSign + 
                " (" + channel_.DisplayChannelNumber + ") [" 
                + channel_.ChannelNumber.Number + "-" + channel_.ChannelNumber.SubNumber + "]";
        }
    }

    class ModulationTypeWrapper
    {
        public ModulationTypeWrapper(Microsoft.MediaCenter.TV.Tuning.ModulationType modulation_type)
        {
            modulation_type_ = modulation_type;
        }

        private Microsoft.MediaCenter.TV.Tuning.ModulationType modulation_type_;

        public Microsoft.MediaCenter.TV.Tuning.ModulationType modulation_type
        {
            get { return modulation_type_; }
        }

        public override string ToString()
        {
            return ChannelEditing.ModulationTypeName(modulation_type_);
        }
    }
}
