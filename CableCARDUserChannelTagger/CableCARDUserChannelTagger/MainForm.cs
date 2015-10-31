using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.Store;

namespace CableCARDUserChannelTagger
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            PopulateLineupsComboBox();
            SelectCableCARDScannedLineup();
        }

        private void PopulateLineupsComboBox()
        {
            LineupComboBox.Items.Clear();
            LineupComboBox.Items.AddRange(new Lineups(object_store_).ToArray());
        }

        private List<Channel> GetUserChannelsFromLineup(Lineup lineup)
        {
            List<Channel> user_channels = new List<Channel>();
            foreach (Channel ch in lineup.GetChannels())
            {
                if (ch.ChannelType == ChannelType.UserAdded)
                {
                    user_channels.Add(ch);
                }
            }
            return user_channels;
        }

        private void SelectCableCARDScannedLineup()
        {
            foreach (object o in LineupComboBox.Items)
            {
                if ((o as Lineup).Name == "Scanned (Digital Cable (CableCARD™))")
                {
                    LineupComboBox.SelectedItem = o;
                    return;
                }
            }
        }

        private ObjectStore object_store_ = ObjectStore.DefaultSingleton;

        private void LineupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserChannelsListBox.Items.Clear();
            if (LineupComboBox.SelectedItem == null) return;
            List<Channel> user_channels = GetUserChannelsFromLineup(LineupComboBox.SelectedItem as Lineup);
            UserChannelsListBox.Items.AddRange(user_channels.ToArray());
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            foreach (Object o in UserChannelsListBox.Items)
            {
                Channel ch = o as Channel;
                if (PrependRadioButton.Checked)
                {
                    ch.CallSign = CallsignTagInput.Text + ch.CallSign;
                }
                else if (AppendRadioButton.Checked)
                {
                    ch.CallSign = ch.CallSign + CallsignTagInput.Text;
                }
                else if (ReplaceRadioButton.Checked)
                {
                    ch.CallSign = CallsignTagInput.Text;
                }
                ch.Update();
            }
            MessageBox.Show("Done updating callsigns.  Program will now Exit");
            this.Close();
        }

        private void AddOrphanButton_Click(object sender, EventArgs e)
        {
            foreach (Channel ch in new Channels(object_store_).ToArray())
            {
                if (ch.Lineup == null && ch.ChannelType == ChannelType.UserAdded)
                {
                    (LineupComboBox.SelectedItem as Lineup).AddChannel(ch);
                }
            }
        }
    }
}
