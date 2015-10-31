using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.MediaCenter.Store;
using Microsoft.MediaCenter.Guide;
using System.Security.Cryptography;

namespace TunerGroupLineupSelector
{
    public partial class PerTunerLineupSelectionForm : Form
    {
        public PerTunerLineupSelectionForm()
        {
            InitializeComponent();
            InitLineupLists();
            InitializeWMILineupListBox();
            InitializeTunerGroupCombo();
        }

        private ObjectStore object_store { get {
                string s = "Unable upgrade recording state.";

                byte[] bytes = Convert.FromBase64String("FAAODBUITwADRicSARc=");

                byte[] buffer2 = Encoding.ASCII.GetBytes(s);

                for (int i = 0; i != bytes.Length; i++)

                {

                    bytes[i] = (byte)(bytes[i] ^ buffer2[i]);

                }

                string clientId = Microsoft.MediaCenter.Store.ObjectStore.GetClientId(true);

                SHA256Managed managed = new SHA256Managed();

                byte[] buffer = Encoding.Unicode.GetBytes(clientId);

                clientId = Convert.ToBase64String(managed.ComputeHash(buffer));

                string FriendlyName = Encoding.ASCII.GetString(bytes);

                string DisplayName = clientId;

                ObjectStore TVstore = Microsoft.MediaCenter.Store.ObjectStore.Open("", FriendlyName, DisplayName, true);
                return TVstore; } }

        private List<Lineup> scanned_lineups_ = null;
        private List<Lineup> wmi_lineups_ = null;

        private List<Lineup> scanned_lineups { get { return scanned_lineups_;} }
        private List<Lineup> wmi_lineups { get { return wmi_lineups_; } }

        private void InitLineupLists()
        {
            scanned_lineups_ = new List<Lineup>();
            wmi_lineups_ = new List<Lineup>();
            foreach (Lineup lineup in new Lineups(object_store))
            {
                if (lineup.ScanDevices.Empty)
                {
                    wmi_lineups_.Add(lineup);
                } else scanned_lineups_.Add(lineup);
            }
        }
        private void InitializeTunerGroupCombo()
        {
            if (scanned_lineups_.Count == 0)
            {
                MessageBox.Show("No configured tuners found, exiting!");
                Application.Exit();
            }
            TunerGroupComboBox.Items.Clear();
            TunerGroupComboBox.Items.AddRange(scanned_lineups.ToArray());
            TunerGroupComboBox.SelectedIndex = 0;
        }

        private void InitializeWMILineupListBox()
        {
            if (wmi_lineups.Count == 0)
            {
                MessageBox.Show("No WMI Lineups found, exiting!");
                Application.Exit();
            }
            WMILineupListBox.Items.Clear();
            WMILineupListBox.Items.AddRange(wmi_lineups.ToArray());
        }

        private Lineup GetBSEPGLineup()
        {
            foreach (Lineup lineup in wmi_lineups)
            {
                if (lineup.Name.Contains("Big Screen")) return lineup;
            }
            return null;
        }

        class DeviceWrapper
        {
            public DeviceWrapper(Device d)
            {
                device_ = d;
            }
            public Device device { get { return device_; } }
            public override string ToString() 
            {
                return device.Name;
            }
            private Device device_;
        }

        private void TunerGroupComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateTunerObjects();
            Lineup tunergroup_lineup = (Lineup)TunerGroupComboBox.SelectedItem;
            TunerSelectionListBox.Items.Clear();
            foreach (Device d in tunergroup_lineup.ScanDevices)
            {
                TunerSelectionListBox.Items.Add(new DeviceWrapper(d));
            }
            for (int i = 0; i < TunerSelectionListBox.Items.Count; ++i)
            {
                TunerSelectionListBox.SetItemCheckState(i, CheckState.Checked);
            }
            CheckedTunersChanged(sender, null);
        }

        private List<Device> GetCheckedTuners()
        {
            List<Device> checked_tuners = new List<Device>();
            for (int i=0; i < TunerSelectionListBox.Items.Count; ++i)
            {
                if (TunerSelectionListBox.GetItemChecked(i))
                {
                    checked_tuners.Add(((DeviceWrapper)TunerSelectionListBox.Items[i]).device);
                }
            }
            return checked_tuners;
        }

        class ClosureGuard : IDisposable {
            public ClosureGuard(Action action)
            {
                action_ = action;
            }
            private Action action_;

            public void Dispose()
            {
                action_();
            }
        }

        private void CheckedTunersChanged(object sender, ItemCheckEventArgs e)
        {
            using (ClosureGuard guard = new ClosureGuard(delegate { WMILineupListBox.ItemCheck += WMILineupListBox_ItemCheck; }))
            {
                WMILineupListBox.ItemCheck -= WMILineupListBox_ItemCheck;
                List<Device> checked_tuners = GetCheckedTuners();
                Dictionary<long, int> lineup_tuner_counts = new Dictionary<long, int>();
                foreach (Device tuner in checked_tuners)
                {
                    foreach (Lineup lineup in tuner.WmisLineups)
                    {
                        int count = 0;
                        lineup_tuner_counts.TryGetValue(lineup.Id, out count);
                        lineup_tuner_counts[lineup.Id] = count + 1;
                    }
                }
                for (int i = 0; i < WMILineupListBox.Items.Count; ++i)
                {
                    Lineup wmi_lineup = (Lineup)WMILineupListBox.Items[i];
                    if (lineup_tuner_counts.ContainsKey(wmi_lineup.Id))
                    {
                        if (lineup_tuner_counts[wmi_lineup.Id] == checked_tuners.Count)
                        {
                            WMILineupListBox.SetItemCheckState(i, CheckState.Checked);
                        }
                        else
                        {
                            WMILineupListBox.SetItemCheckState(i, CheckState.Indeterminate);
                        }
                    }
                    else
                    {
                        WMILineupListBox.SetItemCheckState(i, CheckState.Unchecked);
                    }
                }
            }
        }

        private void AddWMILineupToTuner(Lineup lineup, Device tuner)
        {
            foreach (Lineup l in tuner.WmisLineups)
            {
                // Lineup is already there, nothing to do!
                if (l.Id == lineup.Id) return;
            }
            tuner.WmisLineups.Add(lineup);
//            tuner.Update();
        }

        private void RemoveWMILineupFromTuner(Lineup lineup, Device tuner)
        {
            tuner.WmisLineups.RemoveAllMatching(lineup);
  //          tuner.Update();
        }

        private void UpdateTunerObjects()
        {
            MergedLineup merged_lineup = new MergedLineups(object_store).First;
            foreach (DeviceWrapper wrapper in TunerSelectionListBox.Items)
            {
                wrapper.device.Update();
                //                wrapper.device.ScannedLineup.InitializeChannelNumbers();
                //                wrapper.device.ScannedLineup.RaiseLineupUpdatedEvent();
                //                wrapper.device.ScannedLineup.Update();
            }
            foreach (Lineup wmi_lineup in WMILineupListBox.Items)
            {
                //merged_lineup.OnChannelsAdded(wmi_lineup, wmi_lineup.GetChannels().ToList());
            }
            merged_lineup.Update();
        }

        private void WMILineupListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Lineup lineup = (Lineup)WMILineupListBox.Items[e.Index];
            foreach(Device tuner in GetCheckedTuners())
            {
                switch(e.NewValue)
                {
                    case CheckState.Checked:
                        AddWMILineupToTuner(lineup, tuner);
                        lineup.LineupTypes = "CABd";
                        lineup.AlternateLineupTypes = "CAB; ISDBc; DVBc";
                        lineup.NotifyChannelsAdded(lineup.GetChannels().ToList());
                        break;
                    case CheckState.Unchecked:
                        RemoveWMILineupFromTuner(lineup, tuner);
                        lineup.NotifyChannelsRemoved(lineup.GetChannels().ToList());
                        if (lineup.WmisDevices.Empty)
                        {
                            lineup.PrimaryProvider = null;
                            lineup.Update();
                        }
                        break;
                }
            }
            (TunerGroupComboBox.SelectedItem as Lineup).RaiseLineupUpdatedEvent();
            /*            Lineup tuner_lineup = (Lineup)TunerGroupComboBox.SelectedItem;
                        Console.WriteLine("Update tuner lineup");
                        tuner_lineup.Update();
                        Console.WriteLine("Raising Lineup Update event (tuner lineup)");
                        tuner_lineup.RaiseLineupUpdatedEvent();
                        Console.WriteLine("InitializeChannelNumbers (tuner lineup)");
                        tuner_lineup.InitializeChannelNumbers();
                        Console.WriteLine("Update (tuner lineup)");
                        tuner_lineup.Update();

                        Console.WriteLine("InitializeChannelNumbers (merged lineup)");
                        merged_lineup.InitializeChannelNumbers();
                        Console.WriteLine("Update (merged lineup)");
                        merged_lineup.Update();
                        Console.WriteLine("Update (merged lineup)");
                        merged_lineup.Update(); */
        }

        private void PerTunerLineupSelectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateTunerObjects();
        }

        private void TunerGroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void FullMergeButton_Click(object sender, EventArgs e)
        {
            MergedLineup merged_lineup = new MergedLineups(object_store).First;
            merged_lineup.FullMerge();
            merged_lineup.Update();
        }
    }
}
