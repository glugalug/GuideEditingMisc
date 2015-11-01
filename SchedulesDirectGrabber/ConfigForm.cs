using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using ChannelEditingLib;
using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.Store.MXF;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace SchedulesDirectGrabber
{
    using System.Xml.Schema;
    using LineupConfig = ConfigManager.LineupConfig;
    using ScannedLineupConfig = ConfigManager.ScannedLineupConfig;
    using SDStation = StationCache.SDStation;

    public partial class ConfigForm : Form
    {
        private SDTokenManager token_manager_;

        private SDLineup[] subscribed_lineups_ = null;

        public ConfigForm(SDTokenManager token_manager, StatusResponse sd_status)
        {
            InitializeComponent();
            token_manager_ = token_manager;
            sd_status_ = sd_status;
            InitCountryComboBox();
            UpdateSubscribedLineups();
            InitScannedLineupsComboBox();
        }

        private void UpdateSubscribedLineups()
        {
            subscribed_lineups_ = SDLineupSelection.GetSubscribedLineups().GetActiveLineups();
            UpdateLineupSelectionListBoxes();
        }

        private void InitCountryComboBox()
        {
            CountryComboBox.Items.Clear();
            CountryComboBox.Items.AddRange(SDLineupSelection.GetCountryListFromUri("/20141201/available/countries").ToArray());
            CountryComboBox.SelectedIndex = 0;
        }

        private void UpdateSelectedLineupsListBoxAndChannelSettingsCombo()
        {
            SelectedLineupsListBox.Items.Clear();
            SelectedLineupsListBox.Items.AddRange(config.lineups.ToArray());
            ChannelSettingsLineupSelectionCombo.Items.Clear();
            ChannelSettingsLineupSelectionCombo.Items.AddRange(config.lineups.ToArray());
        }
        private void UpdateLineupSelectionListBoxes()
        {
            UpdateAvailableLineupsListBox();
            UpdateSelectedLineupsListBoxAndChannelSettingsCombo();
        }

        private void InitScannedLineupsComboBox()
        {
            ScannedLineupSelectionComboBox.Items.Clear();
            ScannedLineupSelectionComboBox.Items.AddRange(ChannelEditing.GetScannedLineups().ToArray());
            if (ScannedLineupSelectionComboBox.Items.Count > 0)
                ScannedLineupSelectionComboBox.SelectedIndex = 0;
        }

        private bool updatingSDLineupAssociationsCombo_ = false;
        
        private Lineup selected_scanned_lineup { get { return ScannedLineupSelectionComboBox.SelectedItem as Lineup; } }

        private ScannedLineupConfig selected_scanned_lineup_config
        {
            get
            {
                foreach(var scanned_lineup_config in config.scanned_lineups)
                {
                    if (scanned_lineup_config.id == selected_scanned_lineup.Id)
                    {
                        return scanned_lineup_config;
                    }
                }
                return null;
            }
        }
        private void UpdateSDLineupAssociationsComboBox()
        {
            try
            {
                updatingSDLineupAssociationsCombo_ = true;
                AssociatedSDLineupComboBox.Items.Clear();
                AssociatedSDLineupComboBox.Items.Add("None");
                AssociatedSDLineupComboBox.Items.AddRange(config.lineups.ToArray());
                if (selected_scanned_lineup_config==null)
                {
                    AssociatedSDLineupComboBox.SelectedIndex = 0;
                    return;
                }
                string sdlineup_id = selected_scanned_lineup_config.sd_lineup_id;
                foreach(object sdLineup in AssociatedSDLineupComboBox.Items)
                {
                    if (sdlineup_id == (sdLineup as SDLineup)?.lineup)
                        AssociatedSDLineupComboBox.SelectedItem = sdLineup;
                }
            } finally
            {
                updatingSDLineupAssociationsCombo_ = false;
            }
        }

        private void AssociatedSDLineupComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (updatingSDLineupAssociationsCombo_) return;
            var scanned_lineup_config = FindOrCreateConfigForSelectedScannedLineup();
            var sd_lineup = (AssociatedSDLineupComboBox.SelectedItem as LineupConfig)?.sdLineup;
            scanned_lineup_config.sd_lineup_id = sd_lineup.lineup;
            AssociateSchedulesDirectLineupCheckbox.Enabled = sd_lineup != null;
            RemoveAssociationCheckBox.Enabled = sd_lineup != null;
            ResetConfigWMILineupCache();
        }

        private ScannedLineupConfig FindOrCreateConfigForSelectedScannedLineup()
        {
            if (selected_scanned_lineup_config != null) return selected_scanned_lineup_config;
            ScannedLineupConfig scanned_lineup_config = new ScannedLineupConfig();
            scanned_lineup_config.id = selected_scanned_lineup.Id;
            config.scanned_lineups.Add(scanned_lineup_config);
            return scanned_lineup_config;
        }

        bool updating_tuner_association_checkboxes_ = false;
        private readonly StatusResponse sd_status_;

        private void TunerAssociationCheckBoxCheckChanged(object sender, EventArgs e)
        {
            if (updating_tuner_association_checkboxes_) return;
            var scanned_lineup_config = FindOrCreateConfigForSelectedScannedLineup();
            if (scanned_lineup_config == null) return;
            scanned_lineup_config.addAssociation = AssociateSchedulesDirectLineupCheckbox.Checked;
            scanned_lineup_config.removeOtherAssociations = RemoveAssociationCheckBox.Checked;
        }

        private void UpdateTunerAssociationCheckboxes()
        {
            try
            {
                updating_tuner_association_checkboxes_ = true;
                var scanned_lineup_config = FindOrCreateConfigForSelectedScannedLineup();
                if (scanned_lineup_config == null) return;
                AssociateSchedulesDirectLineupCheckbox.Checked = scanned_lineup_config.addAssociation;
                RemoveAssociationCheckBox.Checked = scanned_lineup_config.removeOtherAssociations;
            }
            finally
            {
                updating_tuner_association_checkboxes_ = false;
            }
        }

        void UpdateAvailableLineupsListBox()
        {
            AvailableLineupsListBox.Items.Clear();
            foreach(var lineup in subscribed_lineups_)
            {
                if (!config.ContainsLineup(lineup.lineup)) AvailableLineupsListBox.Items.Add(lineup);
            }
        }

        private LineupConfig selected_lineup_for_removal {
            get { return SelectedLineupsListBox.SelectedItem as LineupConfig; }
        }
        private void SelectedLineupsListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            RemoveSelectedLineupButton.Enabled = selected_lineup_for_removal != null;

            if (selected_lineup_for_removal != null) {
                var channel_list = SDChannelReader.GetChannelListByLineupUri(selected_lineup_for_removal.sdLineup.uri);
                JSONClient.DisplayJSON(channel_list);
                string serialized = JSONClient.Serialize(channel_list);
                System.IO.File.WriteAllBytes("channel_list.json", Encoding.UTF8.GetBytes(serialized));
            }
        }

        private SDLineup selected_available_lineup {
            get {return AvailableLineupsListBox.SelectedItem as SDLineup;}
        }

        private void AvailableLineupsListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateAddLineupForDownloadButtonEnabled();
        }

        ConfigManager.SDGrabberConfig config { get { return ConfigManager.config; } }

        private void AddLineupForDownloadButton_Click(object sender, EventArgs e)
        {
            if (config.lineups == null) { config.lineups = new List<LineupConfig>(); }
            LineupConfig lineup_config = new LineupConfig();
            lineup_config.sdLineup = selected_available_lineup;
            config.lineups.Add(lineup_config);
            UpdateLineupSelectionListBoxes();
            AddLineupForDownloadButton.Enabled = false;
            UpdateSDLineupAssociationsComboBox();
        }

        private void RemoveSelectedLineupButton_Click(object sender, EventArgs e)
        {
            LineupConfig lineup_config = selected_lineup_for_removal;
            config.lineups.Remove(lineup_config);
            UpdateLineupSelectionListBoxes();
            RemoveSelectedLineupButton.Enabled = false;
            UpdateAddLineupForDownloadButtonEnabled();
            UpdateSDLineupAssociationsComboBox();
        }

        private void UpdateAddLineupForDownloadButtonEnabled()
        {
            AddLineupForDownloadButton.Enabled = selected_available_lineup != null &&
                !config.ContainsLineup(selected_available_lineup.lineup);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ConfigManager.instance.SaveConfig();
        }

        private void ScannedLineupSelectionComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ScannedLineupConfigurationGroupBox.Text =
                string.Format("Configuration For Lineup \"{0}\"", ScannedLineupSelectionComboBox.SelectedItem?.ToString());
            UpdateSDLineupAssociationsComboBox();
            UpdateTunerAssociationCheckboxes();
            UpdateExistingChannelCleanupComboBox();
        }

        private void UpdateExistingChannelCleanupComboBox()
        {
            ExistingChannelCleanupComboBox.SelectedIndex = (int)selected_scanned_lineup_config.existingChannelCleanup;
        }

        private Country selected_country { get { return CountryComboBox.SelectedItem as Country; } }
        private void CountryComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ZipTextBox.Text = selected_country.postalCodeExample;
            LineupSearchButton.Enabled = Regex.IsMatch(ZipTextBox.Text, selected_country.postalCodeRegex);
        }

        private void LineupSearchButton_Click(object sender, EventArgs e)
        {
            LineupSearchResultListBox.Items.Clear();
            LineupSearchResultListBox.Items.AddRange(SDLineupSelection.GetLineupsForZip(
                token_manager_, selected_country.shortName, ZipTextBox.Text).ToArray());
        }

        private void SubscribeButton_Click(object sender, EventArgs e)
        {
            try {
                SDAccountManagement.AddLineupToAccount((LineupSearchResultListBox.SelectedItem as SDLineup).lineup);
                UpdateSubscribedLineups();
            } catch(Exception ex)
            {
                Misc.OutputException(ex);
                MessageBox.Show("Failed to add lineup.");
            }

        }

        private void UnsubscribeButton_Click(object sender, EventArgs e)
        {
            try
            {
                SDAccountManagement.RemoveLineupFromAccount(selected_available_lineup.lineup);
                UpdateSubscribedLineups();
            } catch(Exception ex)
            {
                Misc.OutputException(ex);
                MessageBox.Show("Failed to remove lineup.");
            }
        }

        private LineupConfig channelSettingsLineup {
            get { return ChannelSettingsLineupSelectionCombo.SelectedItem as LineupConfig; }
        }

        class StationComparer : IComparer<string>
        {
            private readonly Func<LineupConfig, string, string, int> func_;
            private readonly LineupConfig lineupConfig_;

            public StationComparer(LineupConfig lineupConfig, Func<LineupConfig, string, string, int> func)
            {
                lineupConfig_ = lineupConfig;
                func_ = func;
            }
            int IComparer<string>.Compare(string x, string y)
            {
                return func_(lineupConfig_, x, y);
            }
        }
        private List<string> stationIDs_ = null;

        #region station comparison functions
        private static int CompareChannelNumberLists(List<ChannelNumberConfig> left, List<ChannelNumberConfig> right) {
            left.Sort();
            right.Sort();
            int minSize = Math.Min(left.Count, right.Count);
            for (int index = 0; index < minSize; ++index)
            {
                int comparisonResult = (left[index] as IComparable<ChannelNumberConfig>).CompareTo(right[index]);
                if (comparisonResult != 0) return comparisonResult;
            }
            return left.Count.CompareTo(right.Count);
        }

        private static int CompareStationCallsigns(LineupConfig lineupConfig, string left, string right)
        {
            SDStation leftStation = lineupConfig.GetStationByID(left);
            SDStation rightStation = lineupConfig.GetStationByID(right);
            return leftStation.callsign.CompareTo(rightStation.callsign);
        }
        private static int CompareDefaultGuideChannels(LineupConfig lineupConfig, string left, string right)
        {
            List<ChannelNumberConfig> leftChannels = lineupConfig.defaultStationChannelNumbers[left];
            List<ChannelNumberConfig> rightChannels = lineupConfig.defaultStationChannelNumbers[right];
            return CompareChannelNumberLists(leftChannels, rightChannels);
        }
        private static int CompareDownload(LineupConfig lineupConfig, string left, string right)
        {
            // Reverse the comparison order, because the value in the config setting is actually exclude not include.
            return lineupConfig.ExcludedFromDownload(right).CompareTo(lineupConfig.ExcludedFromDownload(right));
        }
        private static int CompareEffectiveGuideChannels(LineupConfig lineupConfig, string left, string right)
        {
            List<ChannelNumberConfig> leftChannels = lineupConfig.EffectiveStationChannelNumbers(left);
            List<ChannelNumberConfig> rightChannels = lineupConfig.EffectiveStationChannelNumbers(right);
            return CompareChannelNumberLists(leftChannels, rightChannels);
        }
        private static int CompareIncludeInGuide(LineupConfig lineupConfig, string left, string right)
        {
            // Reverse the comparison order, because the value in the config setting is actually exclude not include.
            return lineupConfig.ExcludedFromGuide(right).CompareTo(lineupConfig.ExcludedFromGuide(right));
        }
        private static int CompareStationIDs(LineupConfig lineupConfig, string left, string right)
        {
            return left.CompareTo(right);
        }
        private static int CompareStationNames(LineupConfig lineupConfig, string left, string right)
        {
            SDStation leftStation = lineupConfig.GetStationByID(left);
            SDStation rightStation = lineupConfig.GetStationByID(right);
            return leftStation.name.CompareTo(rightStation.name);
        }
        #endregion
        private static Dictionary<ChannelSettingsColumn, Func<LineupConfig, string, string, int>> stationColumnComparisonFunctions =
            new Dictionary<ChannelSettingsColumn, Func<LineupConfig, string, string, int>> {
                {ChannelSettingsColumn.Callsign, CompareStationCallsigns },
                {ChannelSettingsColumn.DefaultGuideChannels, CompareDefaultGuideChannels},
                {ChannelSettingsColumn.Download, CompareDownload },
                {ChannelSettingsColumn.EffectiveGuideChannels, CompareEffectiveGuideChannels },
                {ChannelSettingsColumn.IncludeInGuide, CompareIncludeInGuide },
                {ChannelSettingsColumn.StationID, CompareStationIDs },
                {ChannelSettingsColumn.StationName, CompareStationNames }
            };

        void SortChannelIDs()
        {
            if (!stationColumnComparisonFunctions.ContainsKey(channelSettingsSortColumn)) return;
            stationIDs_.Sort(new StationComparer(channelSettingsLineup, stationColumnComparisonFunctions[channelSettingsSortColumn]));
            if (channelSettingsSortReversed) stationIDs_.Reverse();
        }

        private ChannelSettingsColumn channelSettingsSortColumn = ChannelSettingsColumn.Callsign;
        private bool channelSettingsSortReversed = false;
        private void ResetConfigWMILineupCache()
        {
            foreach(var configLineup in config.lineups)
            {
                configLineup.wmcScannedLineups = config.GetWMCScannedLineupsBySDLineupID(configLineup.sdLineup.lineup);
            }
        }
        private void ChannelSettingsLineupSelectionCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            ResetConfigWMILineupCache();
            stationIDs_ = channelSettingsLineup?.stationIDs.ToList();
            SortChannelIDs();
            ChannelSettingsDataGridView.RowCount = stationIDs_.Count;
            ChannelSettingsDataGridView.Invalidate();
        }

        private enum ChannelSettingsColumn
        {
            Logo,
            StationID,
            StationName,
            Callsign,
            DefaultGuideChannels,
            EffectiveGuideChannels,
            GuideChannelOverride,
            DefaultPhysicalChannels,
            EffectivePhysicalChannels,
            PhysicalChannelOverride,
            Download,
            IncludeInGuide,
            Unknown
        }

        private ChannelSettingsColumn GetColumnByIndex(int index)
        {
            if (index == LogoCol.Index) return ChannelSettingsColumn.Logo;
            if (index == StationIDCol.Index) return ChannelSettingsColumn.StationID;
            if (index == StationNameCol.Index) return ChannelSettingsColumn.StationName;
            if (index == CallSignCol.Index) return ChannelSettingsColumn.Callsign;
            if (index == DefaultGuideChannelsCol.Index) return ChannelSettingsColumn.DefaultGuideChannels;
            if (index == EffectiveGuideChannelsCol.Index) return ChannelSettingsColumn.EffectiveGuideChannels;
            if (index == DefaultPhysicalChannelsCol.Index) return ChannelSettingsColumn.DefaultPhysicalChannels;
            if (index == EffectivePhysicalChannelsCol.Index) return ChannelSettingsColumn.EffectivePhysicalChannels;
            if (index == DownloadCol.Index) return ChannelSettingsColumn.Download;
            if (index == IncludeInGuideCol.Index) return ChannelSettingsColumn.IncludeInGuide;
            return ChannelSettingsColumn.Unknown;
        }

        private void ChannelSettingsDataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= stationIDs_.Count()) return;
            ChannelSettingsColumn column = GetColumnByIndex(e.ColumnIndex);
            string stationID = stationIDs_[e.RowIndex];
            SDStation station = channelSettingsLineup.GetStationByID(stationID);
            switch(column)
            {
                case ChannelSettingsColumn.Callsign:
                    e.Value = station.callsign;
                    break;
                case ChannelSettingsColumn.DefaultGuideChannels:
                    var defaultChannelNumbers = channelSettingsLineup.defaultStationChannelNumbers[stationID];
                    e.Value = string.Join("\n", defaultChannelNumbers);
                    break;
                case ChannelSettingsColumn.DefaultPhysicalChannels:
                    var defaultPhysicalChannels = channelSettingsLineup.defaultStationTuningParams[stationID];
                    e.Value = string.Join("\n", defaultPhysicalChannels);
                    break;
                case ChannelSettingsColumn.Download:
                    e.Value = !channelSettingsLineup.ExcludedFromDownload(stationID);
                    break;
                case ChannelSettingsColumn.EffectiveGuideChannels:
                    var effectiveChannelNumbers = channelSettingsLineup.EffectiveStationChannelNumbers(stationID);
                    e.Value = string.Join("\n", effectiveChannelNumbers);
                    break;
                case ChannelSettingsColumn.EffectivePhysicalChannels:
                    var effectivePhysicalChannels = channelSettingsLineup.EffectivePhysicalChannelNumbers(stationID);
                    e.Value = string.Join("\n", effectivePhysicalChannels);
                    break;
                case ChannelSettingsColumn.IncludeInGuide:
                    e.Value = !channelSettingsLineup.ExcludedFromGuide(stationID);
                    break;
                case ChannelSettingsColumn.Logo:
                    string url = station.logo?.URL;
                    Image image = null;
                    if (url != null)
                    {
                        image = Misc.LoadImageFromURL(url);
                    }
                    e.Value = (image != null) ? image : new Bitmap(1, 1);
                    break;
                case ChannelSettingsColumn.PhysicalChannelOverride:
                case ChannelSettingsColumn.GuideChannelOverride:
                    e.Value = "Override";
                    break;
                case ChannelSettingsColumn.StationID:
                    e.Value = stationID;
                    break;
                case ChannelSettingsColumn.StationName:
                    e.Value = station.name;
                    break;
            }
        }

        private void FetchButton_Click(object sender, EventArgs e)
        {
            MXF.BuildMxf();
        }

        private void ChannelSettingsDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ChannelSettingsColumn column = GetColumnByIndex(e.ColumnIndex);
            channelSettingsSortReversed = !channelSettingsSortReversed && (column == channelSettingsSortColumn);
            channelSettingsSortColumn = column;
            SortChannelIDs();
            ChannelSettingsDataGridView.Invalidate();
        }

        private void MXFExportButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                DialogResult dr = dialog.ShowDialog();
                if (dr != DialogResult.OK) return;
                //ChannelEditing.object_store.Export("MXFExport", dialog.FileName);
                using (XmlTextWriter writer = new XmlTextWriter(new StreamWriter(dialog.FileName)))
                {
                    MxfExporter.Export(ChannelEditing.object_store, writer, false);
                } 
            }
        }

        private void ChannelSettingsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= stationIDs_.Count()) return;
            ChannelSettingsColumn column = GetColumnByIndex(e.ColumnIndex);
            string stationID = stationIDs_[e.RowIndex];
            SDStation station = channelSettingsLineup.GetStationByID(stationID);
            switch (column)
            {
                case ChannelSettingsColumn.Download:
                    channelSettingsLineup.SetExcludedFromDownload(stationID, !channelSettingsLineup.ExcludedFromDownload(stationID));
                    ChannelSettingsDataGridView.InvalidateRow(e.RowIndex);
                    break;
                case ChannelSettingsColumn.IncludeInGuide:
                    channelSettingsLineup.SetExcludedFromGuide(stationID, !channelSettingsLineup.ExcludedFromGuide(stationID));
                    ChannelSettingsDataGridView.InvalidateRow(e.RowIndex);
                    break;
            }

        }

        private void ExistingChannelCleanupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selected_scanned_lineup_config.existingChannelCleanup =
                (ScannedLineupConfig.ExistingChannelCleanupOption)ExistingChannelCleanupComboBox.SelectedIndex;
        }
    }
}
