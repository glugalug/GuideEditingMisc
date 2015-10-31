using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChannelEditingLib;
using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.TV.Tuning;

namespace LineupSelector
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitMergedLineupsComboBox();
            InitWMILineupsComboBox();
            InitScannedLineupsComboBox();
            InitOptionsCombos();
            GatherDebugInfo();
        }

        private Lineup selected_wmi_lineup
        {
            get {return (Lineup)WMILineupComboBox.SelectedItem;}
        }

        private void GatherDebugInfo()
        {
        }

        private void AppendDebugLine(string line)
        {
            DebugTextBox.AppendText(line + "\r\n");
        }

        private Lineup selected_scanned_lineup { get { return (Lineup)ScannedLineupComboBox.SelectedItem; } }

        private MergedLineup selected_merged_lineup
        {
            get { return (MergedLineup)MergedLineupComboBox.SelectedItem; }
        }

        enum MissingChannelOptions
        {
            SkipMissingChannels,
            AddMissingChannels
        };
        enum ExistingChannelOptions
        {
            SkipExistingChannels,
            ReplaceListing,
        };
        enum ExtraChannelOptions
        {
            KeepExtraChannels,
            RemoveExtraChannels
        };

        private void InitOptionsCombos()
        {
            MissingChannelOptionsComboBox.SelectedIndex = (int)MissingChannelOptions.AddMissingChannels;
            ExistingChannelOptionsComboBox.SelectedIndex = (int)ExistingChannelOptions.ReplaceListing;
            ExtraChannelOptionsComboBox.SelectedIndex = (int)ExtraChannelOptions.KeepExtraChannels;
        }

        private void InitWMILineupsComboBox()
        {
            WMILineupComboBox.Items.Clear();
            foreach (Lineup lineup in new Lineups(ChannelEditing.object_store).ToArray())
            {
                if (lineup.ScanDevices.Count() == 0 && lineup.GetChannels().Count() > 0)
                {
                    WMILineupComboBox.Items.Add(lineup);
                }
            }
            if (WMILineupComboBox.Items.Count == 0)
            {
                AppendDebugLine("No WMI lineups found!");
                return;
            }
            WMILineupComboBox.SelectedIndex = 0;
            foreach (Lineup lineup in WMILineupComboBox.Items)
            {
                if (lineup.Name.Contains("Big Screen"))
                {
                    WMILineupComboBox.SelectedItem = lineup;
                }
            }
        }

        private void InitScannedLineupsComboBox()
        {
            ScannedLineupComboBox.Items.Clear();
            foreach (Lineup lineup in new Lineups(ChannelEditing.object_store).ToArray())
            {
                if (lineup.ScanDevices.Count() > 0)
                {
                    ScannedLineupComboBox.Items.Add(lineup);
                }
            }
            if (ScannedLineupComboBox.Items.Count == 0)
            {
                AppendDebugLine("No scanned lineups found to use tuners from!");
                return;
            }
            ScannedLineupComboBox.SelectedIndex = 0;
            foreach (Lineup lineup in ScannedLineupComboBox.Items)
            {
                if (lineup.Name.Contains("Scanned (Digital Cable (CableCARD™))") && !lineup.Name.Contains("Deleted")) {
                    ScannedLineupComboBox.SelectedItem = lineup;
                }
            }
        }


        private void InitMergedLineupsComboBox()
        {
            MergedLineupComboBox.Items.Clear();
            MergedLineupComboBox.Items.AddRange(new MergedLineups(ChannelEditing.object_store).ToArray());
            if (MergedLineupComboBox.Items.Count == 0) {
                MessageBox.Show("No merged lineups found to modify, exiting!");
                Application.Exit();
            }
            MergedLineupComboBox.SelectedIndex = 0;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ChannelEditing.ReleaseHandles();
        }

        private void SyncButton_Click(object sender, EventArgs e)
        {
            
            foreach (Channel ch in selected_wmi_lineup.GetChannels())
            {
                ChannelNumber channel_number = ch.ChannelNumber;
                Channel merged_channel = selected_merged_lineup.GetChannelFromNumber(channel_number.Number, channel_number.SubNumber);
                if (merged_channel == null)
                { // missing channel
                    switch ((MissingChannelOptions)MissingChannelOptionsComboBox.SelectedIndex)
                    {
                        case MissingChannelOptions.AddMissingChannels:
                            AppendDebugLine("Adding channel " + channel_number.ToString() + " callsign: " + ch.CallSign);
                            Channel user_channel = ChannelEditing.AddUserChannelToLineupWithoutMerge(
                                selected_scanned_lineup, ch.CallSign, channel_number.Number, channel_number.SubNumber,
                                ModulationType.BDA_MOD_NOT_SET, ch.Service, selected_scanned_lineup.ScanDevices, ChannelType.CalculatedScanned);
                            user_channel.Update();
                            MergedChannel new_merged_channel = ChannelEditing.CreateMergedChannelFromChannels(
                                new List<Channel>(new Channel[] { user_channel }),
                                channel_number.Number, channel_number.SubNumber, ChannelType.AutoMapped);
                            new_merged_channel.Update();
                            break;
                        case MissingChannelOptions.SkipMissingChannels:
                            break;
                    }
                }
                else
                { // existing channel
                    switch ((ExistingChannelOptions)ExistingChannelOptionsComboBox.SelectedIndex)
                    {
                        case ExistingChannelOptions.ReplaceListing:
                            if (merged_channel.Service.IsSameAs(ch.Service))
                            {
                                AppendDebugLine("Channel " + channel_number.ToString() + " already good!");
                                break;
                            }
                            AppendDebugLine("Replacing listing on channel " + channel_number.ToString() +
                                " old callsign: " + merged_channel.CallSign + " new callsign: " + ch.CallSign);
                            merged_channel.Service = ch.Service;
                            merged_channel.Update();
                            break;
                        case ExistingChannelOptions.SkipExistingChannels:
                            break;
                    }
                }
            }

            foreach (Channel ch in selected_merged_lineup.GetChannels().ToArray())
            {
                ChannelNumber channel_number = ch.ChannelNumber;
                Channel wmi_channel = selected_wmi_lineup.GetChannelFromNumber(channel_number.Number, channel_number.SubNumber);
                if (channel_number == null)
                { // extra channel
                    switch ((ExtraChannelOptions)ExtraChannelOptionsComboBox.SelectedIndex)
                    {
                        case ExtraChannelOptions.KeepExtraChannels:
                            break;
                        case ExtraChannelOptions.RemoveExtraChannels:
                            AppendDebugLine("Removing Extra channel " + channel_number.ToString() + " callsign: " + ch.CallSign);
                            ChannelEditing.DeleteChannel(ch);
                            break;
                    }
                }
            }

        }

    }
}
