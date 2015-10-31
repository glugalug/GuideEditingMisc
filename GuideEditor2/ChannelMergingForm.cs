using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.MediaCenter.Guide;
using ChannelEditingLib;

namespace GuideEditor2
{
    public partial class ChannelMergingForm : Form
    {
        public ChannelMergingForm(List<MergedChannel> channels_to_merge)
        {
            InitializeComponent();
            channels_to_merge_ = channels_to_merge;
            InitListAndComboBoxes();
        }

        private void InitListAndComboBoxes()
        {
            DestinationChannelComboBox.Items.Clear();
            ChannelSortingListBox.Items.Clear();
            foreach (MergedChannel mc in channels_to_merge_)
            {
                MergedChannelComboBoxWrapper wrapper = new MergedChannelComboBoxWrapper(mc);
                DestinationChannelComboBox.Items.Add(wrapper);
                ChannelSortingListBox.Items.Add(wrapper);
            }
            DestinationChannelComboBox.SelectedIndex = 0;
        }

        private class MergedChannelComboBoxWrapper
        {
            public MergedChannelComboBoxWrapper(MergedChannel channel)
            {
                channel_ = channel;
            }

            public MergedChannel channel { get { return channel_; } }

            public override string ToString()
            {
                return string.Format("{0} {1}", channel_.DisplayChannelNumber, channel_.Service);
            }
            private MergedChannel channel_;
        }

        private List<MergedChannel> channels_to_merge_;

        private void SwapChannelIndices(int index1, int index2)
        {
            ListBox.ObjectCollection items = ChannelSortingListBox.Items;
            object temp = items[index1];
            items[index1] = items[index2];
            items[index2] = temp;
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            int selected_index = ChannelSortingListBox.SelectedIndex;
            if (selected_index > 1)
            {
                SwapChannelIndices(selected_index, selected_index - 1);
                ChannelSortingListBox.SelectedIndex = selected_index - 1;
            }
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            int selected_index = ChannelSortingListBox.SelectedIndex;
            if (selected_index >= 0 && selected_index < channels_to_merge_.Count - 1)
            {
                SwapChannelIndices(selected_index, selected_index + 1);
                ChannelSortingListBox.SelectedIndex = selected_index + 1;
            }
        }

        private void MergeButton_Click(object sender, EventArgs e)
        {
            MergedChannel dest_channel = ((MergedChannelComboBoxWrapper)DestinationChannelComboBox.SelectedItem).channel;
            List<MergedChannel> merge_order = new List<MergedChannel>();
            foreach (object item in ChannelSortingListBox.Items)
                merge_order.Add(((MergedChannelComboBoxWrapper)item).channel);
            bool past_dest_channel = false;
            foreach (MergedChannel mc in merge_order)
            {
                if (mc == dest_channel)
                {
                    past_dest_channel = true;
                    continue;
                }
                ChannelEditing.CombineMergedChannels(dest_channel, mc, !past_dest_channel, RemoveChannelsCheckbox.Checked);
            }
        }

    }
}
