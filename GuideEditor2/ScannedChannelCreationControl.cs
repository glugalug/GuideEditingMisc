using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.MediaCenter.Guide;
using ChannelEditingLib;
using Microsoft.MediaCenter.TV.Tuning;

namespace GuideEditor2
{
    public partial class ScannedChannelCreationControl : UserControl
    {
        public class ChannelAddedEventArgs : EventArgs
        {
            public ChannelAddedEventArgs(EventArgs inner_args, MergedChannel merged_channel, Channel scanned_channel)
            {
                merged_channel_ = merged_channel;
                scanned_channel_ = scanned_channel;
                inner_args_ = inner_args;
            }

            public MergedChannel MergedChannel
                { get { return merged_channel_; } }

            public Channel ScannedChannel
                { get { return scanned_channel_; } }

            public EventArgs InnerArgs
                { get { return inner_args_; } }

            private EventArgs inner_args_;
            private MergedChannel merged_channel_;
            private Channel scanned_channel_;

        }

        public ScannedChannelCreationControl()
        {
            InitializeComponent();
            PopulateModulationCombo();
            InitLineupComboBox();
            PopulateListingsCombo();
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        public event EventHandler ChannelAdded;

        private void PopulateModulationCombo()
        {
            ModulationCombo.Items.Clear();
            foreach (ModulationType modulation_type in Enum.GetValues(typeof(ModulationType)))
                ModulationCombo.Items.Add(new ModulationTypeWrapper(modulation_type));
        }

        private void PopulateListingsCombo()
        {
            object old_selected_item = ListingSelectionCombo.SelectedItem;
            ListingSelectionCombo.Items.Clear();
            ListingSelectionCombo.Items.Add("No listing (new empty listing)");
            if (ListingsWithDevicesRadio.Checked)
            {
                wmis_services.Sort(ChannelService.CompareByService);
                ListingSelectionCombo.Items.AddRange(wmis_services.ToArray());
            }
            else if (AllListingsRadio.Checked)
                ListingSelectionCombo.Items.AddRange(all_services.ToArray());
            if (old_selected_item != null && ListingSelectionCombo.Items.Contains(old_selected_item))
                ListingSelectionCombo.SelectedItem = old_selected_item;
            else ListingSelectionCombo.SelectedIndex = 0;
        }

        public MergedChannel DestinationMergedChannel
        {
            get { return destination_merged_channel_; }
            set
            {
                destination_merged_channel_ = value;
                DisplayRelevantPanel();
            }
        }

        private void DisplayRelevantPanel()
        {
            bool has_destination_channel = (destination_merged_channel_ != null);
            ListingSelectionGroupBox.Visible = !has_destination_channel;
            GuideChannelNumberPanel.Visible = !has_destination_channel;
            MergePositionGroupBox.Visible = has_destination_channel;
        }

        private void InitLineupComboBox()
        {
            Lineup[] lineups = ChannelEditing.GetLineups();
            // include only lineups with scanned devices here, as there are no tuners to add to otherwise.
            LineupSelectionComboBox.Items.Clear();
            foreach (Lineup lineup in lineups)
                if (!lineup.ScanDevices.Empty)
                    LineupSelectionComboBox.Items.Add(lineup);
            if (LineupSelectionComboBox.Items.Count > 0)
                LineupSelectionComboBox.SelectedIndex = 0;
        }

        private MergedChannel destination_merged_channel_ = null;
        private List<ChannelService> wmis_services = ChannelEditing.GetChannelServicesForWmiLineups().ToList();
        private List<Service> all_services = ChannelEditing.GetServices();

        private Lineup SelectedLineup { get { return (Lineup)LineupSelectionComboBox.SelectedItem; } }

        private class DeviceWrapper
        {
            public DeviceWrapper(Device d)
            {
                device_ = d;
            }
            public Device Device { get { return device_; } }
            public override string ToString()
            {
                return device_.Name;
            }
            private Device device_;
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

        private void LineupSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Device> channel_tuning_info_devices =
                ChannelEditing.GetLineupScannedDevicesByTuningInfoType(
                    SelectedLineup, ChannelEditing.TuningInfoType.ChannelTuningInfo);
            List<Device> dvb_devices =
                ChannelEditing.GetLineupScannedDevicesByTuningInfoType(
                    SelectedLineup, ChannelEditing.TuningInfoType.DvbTuningInfo);
            List<Device> dvblink_devices =
                ChannelEditing.GetLineupScannedDevicesByTuningInfoType(
                    SelectedLineup, ChannelEditing.TuningInfoType.DvbLink);

            bool supports_channeltuninginfo = channel_tuning_info_devices.Count != 0;
            bool supports_dvbtuning = dvb_devices.Count != 0;
            bool has_dvblink = dvblink_devices.Count != 0;

            SetTabVisibility(ChannelTuningInfoPage, supports_channeltuninginfo);
            SetTabVisibility(DVBTuningInfoPage, supports_dvbtuning);
            SetTabVisibility(DVBLinkPage, has_dvblink);

            TunerSelectionListBox.Items.Clear();
            foreach (Device d in channel_tuning_info_devices)
                TunerSelectionListBox.Items.Add(new DeviceWrapper(d));
            for (int index = 0; index < channel_tuning_info_devices.Count; ++index)
                TunerSelectionListBox.SetItemChecked(index, true);

            DvbTunersCheckedListBox.Items.Clear();
            foreach (Device d in dvb_devices)
                DvbTunersCheckedListBox.Items.Add(new DeviceWrapper(d));
            for (int index = 0; index < dvb_devices.Count; ++index)
                DvbTunersCheckedListBox.SetItemChecked(index, true);

            DVBLinkTunerCheckedListBox.Items.Clear();
            foreach (Device d in dvblink_devices)
                DVBLinkTunerCheckedListBox.Items.Add(new DeviceWrapper(d));
            for (int index = 0; index < dvblink_devices.Count; ++index)
                DVBLinkTunerCheckedListBox.SetItemChecked(index, true);

            PopulateDvbCombos();

            // set modulation to match the first channel we find in the lineup.
            foreach (Channel ch in SelectedLineup.GetChannels())
                if (ch.TuningInfos != null && !ch.TuningInfos.Empty)
                    foreach (TuningInfo ti in ch.TuningInfos)
                    {
                        if (!(ti is ChannelTuningInfo)) continue; // DvbTuningInfos not handled!!!
                        ModulationType mod_type = ((ChannelTuningInfo)ti).ModulationType;
                        for (int index = 0; index < ModulationCombo.Items.Count; ++index)
                        {
                            if (((ModulationTypeWrapper)ModulationCombo.Items[index]).modulation_type == mod_type)
                            {
                                ModulationCombo.SelectedIndex = index;
                                break;
                            }
                        }
                        break;
                    }
        }

        private void SetTabVisibility(TabPage page, bool visibility)
        {
            bool tab_currently_visible = ChannelTypeTabControl.TabPages.Contains(page);
            if (tab_currently_visible == visibility) return;
            if (visibility)
            {
                ChannelTypeTabControl.TabPages.Add(page);
            }
            else
            {
                ChannelTypeTabControl.TabPages.Remove(page);
            }
        }

        private void ListingsWithDevicesRadio_CheckedChanged(object sender, EventArgs e)
        {
            PopulateListingsCombo();
        }


        private List<Device> GetCheckedDevices(CheckedListBox device_listbox)
        {
            List<Device> checked_devices = new List<Device>();
            foreach (object o in device_listbox.CheckedItems)
                checked_devices.Add(((DeviceWrapper)o).Device);
            return checked_devices;
        }

        private List<Device> GetCheckedDevices()
        {
            if (ChannelTypeTabControl.SelectedTab == ChannelTuningInfoPage) return GetCheckedDevices(TunerSelectionListBox);
            if (ChannelTypeTabControl.SelectedTab == DVBTuningInfoPage) return GetCheckedDevices(DvbTunersCheckedListBox);
            if (ChannelTypeTabControl.SelectedTab == DVBLinkPage) return GetCheckedDevices(DVBLinkTunerCheckedListBox);
            throw new Exception("Unknown tab in GetCheckedDevices");
        }

        private void CreateChannelButton_Click(object sender, EventArgs e)
        {
            if (CallsignInput.Text.Length == 0)
            {
                MessageBox.Show("Callsign is required.");
                return;
            }
            Service new_channel_service = null;
            if (destination_merged_channel_ != null)
            {
                new_channel_service = destination_merged_channel_.Service;
            }
            else
            {
                if (ListingSelectionCombo.SelectedItem is Service)
                    new_channel_service = ListingSelectionCombo.SelectedItem as Service;
                else if (ListingSelectionCombo.SelectedItem is ChannelService)
                    new_channel_service = (ListingSelectionCombo.SelectedItem as ChannelService).Service;
            }

            Channel new_channel = null;
            if (ChannelTypeTabControl.SelectedTab == ChannelTuningInfoPage)
            {
                ModulationType mod_type = ((ModulationTypeWrapper)ModulationCombo.SelectedItem).modulation_type;

                try
                {
                    new_channel = ChannelEditing.AddUserChannelToLineupWithoutMerge(
                        SelectedLineup, CallsignInput.Text, (int)ChannelNumberInput.Value, (int)SubchannelInput.Value,
                        mod_type, new_channel_service, GetCheckedDevices());
                }
                catch (Exception exc)
                {
                    AddInputsToException(exc);
                    new ErrorReporter.ErrorReportingForm("Exception occured when trying to add the scanned channel.", exc);
                    return;
                }
            }
            else if (ChannelTypeTabControl.SelectedTab == DVBTuningInfoPage)
            {
                try
                {
                    new_channel = ChannelEditing.AddDvbUserChannelToLineupWithoutMerge(
                        SelectedLineup, CallsignInput.Text, (int)ChannelNumberInput.Value, (int)SubchannelInput.Value,
                        int.Parse(DvbONIDCombo.Text), int.Parse(DvbTSIDCombo.Text), int.Parse(DvbSIDCombo.Text),
                        int.Parse(DvbNIDCombo.Text), int.Parse(DvbFrequencyCombo.Text), (int)DvbLCNInput.Value,
                        new_channel_service, GetCheckedDevices());
                }
                catch (Exception exc)
                {
                    AddInputsToException(exc);
                    new ErrorReporter.ErrorReportingForm("Exception occured when trying to add the DVB scanned channel.", exc);
                    return;
                }
            }
            else if (ChannelTypeTabControl.SelectedTab == DVBLinkPage)
            {
                try
                {
                    int dvblink_channel_key = (int)DVBLinkChannelNumInput.Value;
                    int frequency = dvblink_channel_key * 10000;
                    new_channel = ChannelEditing.AddDvbUserChannelToLineupWithoutMerge(
                        SelectedLineup, CallsignInput.Text, (int)ChannelNumberInput.Value, (int)SubchannelInput.Value,
                        dvblink_channel_key, dvblink_channel_key, dvblink_channel_key, dvblink_channel_key,
                        frequency, (int)DVBLinkLCNInput.Value, new_channel_service, GetCheckedDevices());

                }
                catch (Exception exc)
                {
                    AddInputsToException(exc);
                    new ErrorReporter.ErrorReportingForm("Exception occured when trying to add the DVBLink scanned channel.", exc);
                    return;
                }
            }

            try
            {
                MergedChannel updated_merged_channel = null;
                if (destination_merged_channel_ != null)
                {
                    ChannelEditing.AddScannedChannelToMergedChannel(new_channel, destination_merged_channel_, AddAsFirstRadio.Checked);
                    updated_merged_channel = destination_merged_channel_;
                }
                else if (CreateNewChannelCheckbox.Checked)
                {
                    updated_merged_channel = ChannelEditing.CreateMergedChannelFromChannels(new List<Channel>(new Channel[] { new_channel }),
                        (int)GuideNumberInput.Value, (int)GuideSubNumberInput.Value);
                }
                /* if (ChannelAdded != null)
                {
                    ChannelAddedEventArgs args = new ChannelAddedEventArgs(e, updated_merged_channel, new_channel);
                    ChannelAdded(sender, args);
                } */
                guide_channel_entered_ = false;
            }
            catch (Exception exc)
            {
                if (new_channel != null)
                {
                    exc.Data.Add("new_channel number", new_channel.Number);
                    exc.Data.Add("new_channel subnumber", new_channel.SubNumber);
                    if (new_channel.TuningInfos != null)
                    {
                        exc.Data.Add("new_channel tuninginfo count", new_channel.TuningInfos.Count());
                    }
                    else
                    {
                        exc.Data.Add("new_channel tuning infos", "NULL");
                    }
                }
                else
                {
                    exc.Data.Add("new_channel", "NULL");
                }
                AddInputsToException(exc);
                new ErrorReporter.ErrorReportingForm("Exception occured when trying to put the newly created scanned channel in a merged channel.", exc);
                return;
            }
        }

        private void AddInputsToException(Exception exc)
        {
            exc.Data.Add("Channel Type Tab", ChannelTypeTabControl.SelectedTab.Name);
            exc.Data.Add("Guide number", GuideNumberInput.Value);
            exc.Data.Add("Guide subnumber", GuideSubNumberInput.Value);
            exc.Data.Add("tuned #", ChannelNumberInput.Value);
            exc.Data.Add("tuned subnumber", SubchannelInput.Value);
            exc.Data.Add("Modulation", ModulationCombo.Text);
            exc.Data.Add("DvbFrequency", DvbFrequencyCombo.Text);
            exc.Data.Add("DvbLCN", DvbLCNInput.Value);
            exc.Data.Add("DVBLink Channel #", DVBLinkChannelNumInput.Value);
            exc.Data.Add("DVBLink LCN", DVBLinkLCNInput.Value);
            exc.Data.Add("DvbNid", DvbNIDCombo.Text);
            exc.Data.Add("DvbONid", DvbONIDCombo.Text);
            exc.Data.Add("DvbSid", DvbSIDCombo.Text);
            exc.Data.Add("DvbTSid", DvbTSIDCombo.Text);
            foreach (object o in GetCheckedDevices())
                exc.Data.Add(o, "Checked tuner");
        }

        private void ChannelNumberInput_ValueChanged(object sender, EventArgs e)
        {
            if (!guide_channel_entered_)
                GuideNumberInput.Value = ChannelNumberInput.Value;
        }

        private void SubchannelInput_ValueChanged(object sender, EventArgs e)
        {
            if (!guide_channel_entered_) 
                GuideSubNumberInput.Value = SubchannelInput.Value;
        }

        private void VerifyComboEntry_Integer(object sender, CancelEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            int combo_val;
            bool parse_success = int.TryParse(combo.Text, out combo_val);
            if (!parse_success)
            {
                e.Cancel = true;
            }
            if (combo.Text != combo_val.ToString())
                combo.Text = combo_val.ToString();
        }

        #region "Dvb Property enumerators for building combo boxes"
        private delegate int DvbTuningInfoPropertyGetter(DvbTuningInfo dti);

        private int GetDvbLcn(DvbTuningInfo dti)
        {
            return dti.Lcn;
        }
        private int GetDvbFrequency(DvbTuningInfo dti)
        {
            return dti.Frequency;
        }
        private int GetDvbTsid(DvbTuningInfo dti)
        {
            return dti.Tsid;
        }
        private int GetDvbSid(DvbTuningInfo dti)
        {
            return dti.Sid;
        }
        private int GetDvbOnid(DvbTuningInfo dti)
        {
            return dti.Onid;
        }
        private int GetDvbNid(DvbTuningInfo dti)
        {
            return dti.Nid;
        }

        static private List<int> GetLineupDvbPropertyValues(Lineup lineup, DvbTuningInfoPropertyGetter property_getter)
        {
            List<int> values = new List<int>();
            foreach (Channel ch in lineup.GetChannels())
                if (ch.TuningInfos != null)
                    foreach (TuningInfo ti in ch.TuningInfos)
                        if (ti is DvbTuningInfo)
                            values.Add(property_getter((DvbTuningInfo)ti));
            values = values.Distinct().ToList();
            values.Sort();
            return values;
        }
        #endregion

        void PopulateDvbCombo(ComboBox combo, DvbTuningInfoPropertyGetter property_getter)
        {
            combo.Items.Clear();
            foreach (int property_value in GetLineupDvbPropertyValues(SelectedLineup, property_getter))
                combo.Items.Add(property_value);
        }

        void PopulateDvbCombos()
        {
            PopulateDvbCombo(DvbFrequencyCombo, GetDvbFrequency);
            PopulateDvbCombo(DvbTSIDCombo, GetDvbTsid);
            PopulateDvbCombo(DvbSIDCombo, GetDvbSid);
            PopulateDvbCombo(DvbONIDCombo, GetDvbOnid);
            PopulateDvbCombo(DvbNIDCombo, GetDvbNid);
        }

        private void GuideNumberInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
                guide_channel_entered_ = true;
        }

        private void GuideSubNumberInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
                guide_channel_entered_ = true;

        }

        private bool guide_channel_entered_; // flag to indicate whether guide channel # should auto-update with tuning channel #.
    }
}
