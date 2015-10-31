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
using Microsoft.MediaCenter.Store;

namespace GuideEditor2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitMergedLineupsComboBox();
            InitChannelTypesComboBox();
            InitVisibilitiesComboBox();
            InitUserBlockedStateComboBox();
            InitScannedLineupsCombo();
        }

        private void InitScannedLineupsCombo()
        {
            ScannedLineupCombo.Items.Clear();
            ScannedLineupCombo.Items.AddRange(new Lineups(ChannelEditing.object_store).ToArray());
            ScannedLineupCombo.SelectedIndex = 0;
        }

        private void InitChannelTypesComboBox()
        {
            ChannelTypeCol.Items.Clear();
            ChannelTypeCol.Items.AddRange(Enum.GetNames(typeof(ChannelType)));
            ScannedTypeCol.Items.Clear();
            ScannedTypeCol.Items.AddRange(Enum.GetNames(typeof(ChannelType)));
        }

        private void InitVisibilitiesComboBox()
        {
            VisibilityCol.Items.Clear();
            VisibilityCol.Items.AddRange(Enum.GetNames(typeof(ChannelVisibility)));
        }

        private void InitUserBlockedStateComboBox()
        {
            UserBlockedStateCol.Items.Clear();
            UserBlockedStateCol.Items.AddRange(Enum.GetNames(typeof(UserBlockedState)));
        }

        private void InitMergedLineupsComboBox()
        {
            MergedLineupComboBox.Items.Clear();
            MergedLineupComboBox.Items.AddRange(new MergedLineups(ChannelEditing.object_store).ToArray());
            MergedLineupComboBox.SelectedIndex = 0;
        }

        private void AppendSourceChannel(Channel ch, StringBuilder sb)
        {
            sb.AppendFormat("{0}[{1}]", ch.ChannelNumber, (ch.ChannelType == ChannelType.Wmis) ? "WMI" : string.Format("{0} devices", ChannelEditing.GetTunerCount(ch)));
        }
        private string SerializeSourceChannelList(MergedChannel mc)
        {
            StringBuilder sb = new StringBuilder();
            if (mc.PrimaryChannel != null)
                AppendSourceChannel(mc.PrimaryChannel, sb);
            foreach (Channel ch in mc.SecondaryChannels)
            {
                sb.Append(", ");
                AppendSourceChannel(ch, sb);
            }
            return sb.ToString();
        }

        #region "Channel sorting functions"
        internal static int CompareChannelNumbers(Channel ch1, Channel ch2)
        {
            if (ch1.Number != ch2.Number)
                return ch1.Number - ch2.Number;
            return ch1.SubNumber - ch2.SubNumber;
        }

        internal static int CompareChannelType(Channel ch1, Channel ch2)
        {
            return ch1.ChannelType.CompareTo(ch2.ChannelType);
        }

        internal static int CompareListing(Channel ch1, Channel ch2)
        {
            return ch1.Service.ToString().CompareTo(ch2.Service.ToString());
        }

        internal static int CompareTuningParams(Channel ch1, Channel ch2)
        {
            bool ch1_has_tuning_info = ch1.TuningInfos != null && !ch1.TuningInfos.Empty;
            bool ch2_has_tuning_info = ch2.TuningInfos != null & !ch2.TuningInfos.Empty;
            if (ch1_has_tuning_info && ch2_has_tuning_info)
            {
                return ChannelEditing.GetTuningParamsAsString(ch1.TuningInfos.First).CompareTo(
                    ChannelEditing.GetTuningParamsAsString(ch2.TuningInfos.First));
            }
            else
            {
                return ch1_has_tuning_info.CompareTo(ch2_has_tuning_info);
            }
        }

        internal static int CompareCallsigns(Channel ch1, Channel ch2)
        {
            return ch1.CallSign.CompareTo(ch2.CallSign);
        }

        internal static int CompareTunerCount(Channel ch1, Channel ch2)
        {
            return ChannelEditing.GetTunerCount(ch1) - ChannelEditing.GetTunerCount(ch2);
        }

        internal static int CompareVisibility(Channel ch1, Channel ch2)
        {
            return ch1.Visibility.CompareTo(ch2.Visibility);
        }

        internal static int CompareUserBlockedState(Channel ch1, Channel ch2)
        {
            return ch1.UserBlockedState.CompareTo(ch2.UserBlockedState);
        }

        internal static int CompareEncrypted(Channel ch1, Channel ch2)
        {
            return ch1.IsEncrypted.CompareTo(ch2.IsEncrypted);
        }
        internal static bool IsScannedEncrypted(Channel ch)
        {
            if (ch.IsEncrypted) return true;
            if (ch.ReferencingPrimaryChannels.Empty) return false;
            return ch.ReferencingPrimaryChannels.First.IsEncrypted;
        }
        internal static int CompareScannedEncrypted(Channel ch1, Channel ch2)
        {
            return IsScannedEncrypted(ch1).CompareTo(IsScannedEncrypted(ch2));
        }
        internal static int CompareHD(Channel ch1, Channel ch2)
        {
            return IsChannelHD(ch1).CompareTo(IsChannelHD(ch2));
        }
        internal static int CompareInBand(MergedChannel ch1, MergedChannel ch2)
        {
            return ch2.IgnoreInbandSchedule.CompareTo(ch1.IgnoreInbandSchedule);
        }
        #endregion

        private static bool IsChannelHD(Channel ch)
        {
            if (ch.Service == null) return false;
            return ch.Service.IsHDCapable;
        }

        #region "Column Enumeration for making grid event handling a little easier"
        private enum MergedChannelGridColumn
        {
            Logo,
            Number,
            SubNumber,
            Type,
            Listing,
            TunerCount,
            SourceChannels,
            Visibility,
            UserBlockedState,
            Encrypted,
            HD,
            InBand,
            Unknown,
            ListingProvider
        }

        private MergedChannelGridColumn MergedChannelColumnToEnum(int column)
        {
            if (column == LogoCol.Index) return MergedChannelGridColumn.Logo;
            if (column == ChannelNumberCol.Index) return MergedChannelGridColumn.Number;
            if (column == SubNumberCol.Index) return MergedChannelGridColumn.SubNumber;
            if (column == ChannelTypeCol.Index) return MergedChannelGridColumn.Type;
            if (column == ListingCol.Index) return MergedChannelGridColumn.Listing;
            if (column == ListingProviderCol.Index) return MergedChannelGridColumn.ListingProvider;
            if (column == TunerCountCol.Index) return MergedChannelGridColumn.TunerCount;
            if (column == SourceChannelsCol.Index) return MergedChannelGridColumn.SourceChannels;
            if (column == VisibilityCol.Index) return MergedChannelGridColumn.Visibility;
            if (column == UserBlockedStateCol.Index) return MergedChannelGridColumn.UserBlockedState;
            if (column == EncryptedCol.Index) return MergedChannelGridColumn.Encrypted;
            if (column == HDCol.Index) return MergedChannelGridColumn.HD;
            if (column == InBandCol.Index) return MergedChannelGridColumn.InBand;
            return MergedChannelGridColumn.Unknown;
        }

        private enum ScannedGridColumn
        {
            Logo,
            Number,
            Type,
            TunerCount,
            Listing,
            Remove,
            Encrypted,
            TuningParams,
            Callsign,
            Unknown
        }

        private ScannedGridColumn ScannedColumnToEnum(int column)
        {
            if (column == ScannedLogoCol.Index) return ScannedGridColumn.Logo;
            if (column == ScannedNumberCol.Index) return ScannedGridColumn.Number;
            if (column == ScannedTypeCol.Index) return ScannedGridColumn.Type;
            if (column == ScannedTunerCountCol.Index) return ScannedGridColumn.TunerCount;
            if (column == ScannedListingCol.Index) return ScannedGridColumn.Listing;
            if (column == ScannedRemoveCol.Index) return ScannedGridColumn.Remove;
            if (column == ScannedEncryptedCol.Index) return ScannedGridColumn.Encrypted;
            if (column == TuningParamsCol.Index) return ScannedGridColumn.TuningParams;
            if (column == ScannedCallsignCol.Index) return ScannedGridColumn.Callsign;
            return ScannedGridColumn.Unknown;
        }
        #endregion

        private IEnumerable<int> GetSelectedRowIndices()
        {
            List<int> selected_row_indices = new List<int>();
            if (MergedLineupGridView.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in MergedLineupGridView.SelectedRows)
                {
                    selected_row_indices.Add(row.Index);
                }
                return selected_row_indices;
            }
            DataGridViewSelectedCellCollection selected_cells = MergedLineupGridView.SelectedCells;
            foreach (DataGridViewCell cell in selected_cells)
            {
                selected_row_indices.Add(cell.RowIndex);
            }
            return selected_row_indices.Distinct();
        }

        private void SortChannels()
        {
            merged_channels_.Sort(current_sort_);
            if (reverse_sort_)
                merged_channels_.Reverse();
        }

        private void SortScannedChannels()
        {
            scanned_channels_.Sort(current_scanned_sort_);
            if (reverse_scanned_sort_)
                scanned_channels_.Reverse();
        }

        private void SelectAndLoadLineup(MergedLineup lineup)
        {
            merged_lineup_ = lineup;
            Channel[] channels = merged_lineup_.GetChannels();
            int channel_count = channels.Length;
            merged_channels_ = new List<MergedChannel>();
            foreach (Channel ch in channels.Distinct())
                merged_channels_.Add((MergedChannel)ch);
            SortChannels();
            MergedLineupGridView.RowCount = merged_channels_.Count;
            MergedLineupGridView.Invalidate();
        }

        private void SelectAndLoadScannedLineup(Lineup lineup)
        {
            scanned_lineup_ = lineup;
            Channel[] channels = scanned_lineup_.GetChannels();
            int channel_count = channels.Length;
            scanned_channels_ = new List<Channel>();
            scanned_channels_.AddRange(channels.Distinct());
            SortScannedChannels();
            ScannedLineupGridView.RowCount = scanned_channels_.Count;
            ScannedLineupGridView.Invalidate();
        }

        #region "Private data"
        private MergedLineup merged_lineup_ = null;
        private Lineup scanned_lineup_ = null;
        private List<MergedChannel> merged_channels_ = null;
        private List<Channel> scanned_channels_ = null;
        private Comparison<Channel> current_scanned_sort_ = CompareChannelNumbers;
        private Comparison<MergedChannel> current_sort_ = CompareChannelNumbers;
        private bool reverse_sort_ = false;
        private bool reverse_scanned_sort_ = false;
        #endregion

        #region "event handlers"
        private void MergedLineupComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            SelectAndLoadLineup((MergedLineup)MergedLineupComboBox.SelectedItem);
        }

        private Image GetChannelLogo(Channel ch)
        {
            GuideImage logoImage = ch.LogoImage;
            if (logoImage == null && ch.Service != null) logoImage = ch.Service.LogoImage;
            Bitmap bitmap = null;
            if (logoImage != null)
            {
                string filename = logoImage.ImageUrl.Substring(7); // skip the "file://" part.
                bitmap = new Bitmap(filename);
            }
            if (bitmap == null) bitmap = new Bitmap(5, 5); // blank image if we can't really load it.
            return bitmap;
        }

        private void MergedLineupGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= merged_channels_.Count) return;
            MergedChannel ch = merged_channels_[e.RowIndex];
            MergedChannelGridColumn column = MergedChannelColumnToEnum(e.ColumnIndex);
            switch (column)
            {
                case MergedChannelGridColumn.Logo:
                    try
                    {
                        e.Value = GetChannelLogo(ch);
                    }
                    catch
                    {
                        e.Value = new Bitmap(5, 5);
                    }
                    break;
                case MergedChannelGridColumn.Number:
                    e.Value = ch.Number;
                    break;
                case MergedChannelGridColumn.SubNumber:
                    if (ch.SubNumber != 0) e.Value = ch.SubNumber;
                    else e.Value = null;
                    break;
                case MergedChannelGridColumn.Listing:
                    e.Value = ch.Service;
                    break;
                case MergedChannelGridColumn.ListingProvider:
                    e.Value = ch.Service?.Provider?.Name;
                    break;
                case MergedChannelGridColumn.Type:
                    e.Value = ch.ChannelType.ToString();
                    break;
                case MergedChannelGridColumn.TunerCount:
                    e.Value = ChannelEditing.GetTunerCount(ch);
                    break;
                case MergedChannelGridColumn.Visibility:
                    e.Value = ch.Visibility.ToString();
                    break;
                case MergedChannelGridColumn.UserBlockedState:
                    e.Value = ch.UserBlockedState.ToString();
                    break;
                case MergedChannelGridColumn.SourceChannels:
                    e.Value = SerializeSourceChannelList(ch);
                    break;
                case MergedChannelGridColumn.Encrypted:
                    e.Value = ch.IsEncrypted;
                    break;
                case MergedChannelGridColumn.HD:
                    e.Value = IsChannelHD(ch);
                    break;
                case MergedChannelGridColumn.InBand:
                    e.Value = !ch.IgnoreInbandSchedule;
                    break;
            }
        }

        private void MergedLineupGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void MergedLineupGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Comparison<MergedChannel> old_sort = current_sort_;
            Comparison<MergedChannel> new_sort = null;
            MergedChannelGridColumn column = MergedChannelColumnToEnum(e.ColumnIndex);
            switch (column)
            {
                case MergedChannelGridColumn.Number:
                case MergedChannelGridColumn.SubNumber:
                    new_sort = CompareChannelNumbers;
                    break;
                case MergedChannelGridColumn.Listing:
                    new_sort = CompareListing;
                    break;
                case MergedChannelGridColumn.Type:
                    new_sort = CompareChannelType;
                    break;
                case MergedChannelGridColumn.TunerCount:
                    new_sort = CompareTunerCount;
                    break;
                case MergedChannelGridColumn.Visibility:
                    new_sort = CompareVisibility;
                    break;
                case MergedChannelGridColumn.UserBlockedState:
                    new_sort = CompareUserBlockedState;
                    break;
                case MergedChannelGridColumn.Encrypted:
                    new_sort = CompareEncrypted;
                    break;
                case MergedChannelGridColumn.HD:
                    new_sort = CompareHD;
                    break;
                case MergedChannelGridColumn.InBand:
                    new_sort = CompareInBand;
                    break;
            }
            if (new_sort == null) return;
            reverse_sort_ = (new_sort == old_sort) && !reverse_sort_;
            current_sort_ = new_sort;
            SortChannels();
            MergedLineupGridView.Invalidate();
        }

        private void RemoveChannelsButton_Click(object sender, EventArgs e)
        {
            IEnumerable<int> row_indices = GetSelectedRowIndices();
            foreach (int row_index in row_indices)
            {
                MergedChannel ch = merged_channels_[row_index];
                try
                {
                    ch.Lineup.RemoveChannel(ch);
                }
                catch (Exception exc)
                {
                    new ErrorReporter.ErrorReportingForm("Error occurred while attempting to remove a channel.", exc);
                }
            }

            SelectAndLoadLineup(merged_lineup_); // force a refresh.
        }
        #endregion

        private void MergedLineupGridView_Validating(object sender, CancelEventArgs e)
        {
            Console.WriteLine("foo");
        }

        private void MergedLineupGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= merged_channels_.Count) return;
            MergedChannel ch = merged_channels_[e.RowIndex];
            MergedChannelGridColumn column = MergedChannelColumnToEnum(e.ColumnIndex);
            switch (column)
            {
                case MergedChannelGridColumn.Number:
                    try
                    {
                        int num = int.Parse(e.FormattedValue.ToString());
                        if (ch.Number == num) return;
                        ch.Number = num;
                        ch.Update();
                    }
                    catch { e.Cancel = true; }
                    break;
                case MergedChannelGridColumn.SubNumber:
                    try
                    {
                        string formatted_value = e.FormattedValue.ToString();
                        int num = (formatted_value.Length != 0) ? int.Parse(formatted_value) : 0;
                        if (ch.SubNumber == num) return;
                        ch.SubNumber = num;
                        ch.Update();
                    }
                    catch { e.Cancel = true; }
                    break;
                case MergedChannelGridColumn.Type:
                    try
                    {
                        ChannelType channel_type = (ChannelType)Enum.Parse(typeof(ChannelType), e.FormattedValue.ToString());
                        if (ch.ChannelType == channel_type) return;
                        ch.ChannelType = channel_type;
                        ch.Update();
                    }
                    catch{ e.Cancel = true; }
                    break;
                case MergedChannelGridColumn.UserBlockedState:
                    try
                    {
                        UserBlockedState user_blocked_state = (UserBlockedState)Enum.Parse(typeof(UserBlockedState), e.FormattedValue.ToString());
                        if (ch.UserBlockedState == user_blocked_state) return;
                        ch.UserBlockedState = user_blocked_state;
                        ch.Update();
                    }
                    catch{ e.Cancel = true; }
                    break;
                case MergedChannelGridColumn.Visibility:
                    try
                    {
                        ChannelVisibility visibility = (ChannelVisibility)Enum.Parse(typeof(ChannelVisibility), e.FormattedValue.ToString());
                        if (ch.Visibility == visibility) return;
                        ch.Visibility = visibility;
                        ch.Update();
                    }
                    catch { e.Cancel = true; }
                    break;
            }
        }

        private void MergedLineupGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= merged_channels_.Count) return;
            MergedChannel ch = merged_channels_[e.RowIndex];
            MergedChannelGridColumn column = MergedChannelColumnToEnum(e.ColumnIndex);
            switch (column)
            {
                case MergedChannelGridColumn.Listing:
                    new ListingSelectionForm(ch).ShowDialog();
                    MergedLineupGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);
                    break;
                case MergedChannelGridColumn.SourceChannels:
                    new SourceChannelManagementForm(ch).ShowDialog();
                    MergedLineupGridView.InvalidateRow(e.RowIndex);
                    break;
                case MergedChannelGridColumn.HD:
                    if (ch.Service != null)
                    {
                        ch.Service.IsHDCapable = !ch.Service.IsHDCapable;
                        ch.Service.Update();
                        MergedLineupGridView.InvalidateColumn(e.ColumnIndex);
                    }
                    break;
                case MergedChannelGridColumn.InBand:
                    ch.IgnoreInbandSchedule = !ch.IgnoreInbandSchedule;
                    ch.Update();
                    MergedLineupGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);
                    break;
            }
        }

        private void MergedLineupGridView_SelectionChanged(object sender, EventArgs e)
        {
            IEnumerable<int> selected_row_indices = GetSelectedRowIndices();
            int row_count = selected_row_indices.Count();
            RemoveChannelsButton.Enabled = row_count != 0;
            MergeChannelsButton.Enabled = row_count > 1;
        }

        private void MergeChannelsButton_Click(object sender, EventArgs e)
        {
            List<MergedChannel> channels_to_merge = new List<MergedChannel>();
            IEnumerable<int> selected_row_indices = GetSelectedRowIndices();
            foreach (int row_index in selected_row_indices)
                channels_to_merge.Add(merged_channels_[row_index]);
            new ChannelMergingForm(channels_to_merge).ShowDialog();

            SelectAndLoadLineup(merged_lineup_); // force a refresh.
        }

        private void ScannedLineupCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            Lineup selected_lineup = ScannedLineupCombo.SelectedItem as Lineup;
            if (selected_lineup != null)
                SelectAndLoadScannedLineup(selected_lineup);
        }

        private void ScannedLineupGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= scanned_channels_.Count) return;
            Channel ch = scanned_channels_[e.RowIndex];
            ScannedGridColumn column = ScannedColumnToEnum(e.ColumnIndex);
            switch (column)
            {
                case ScannedGridColumn.Logo:
                    try
                    {
                        e.Value = GetChannelLogo(ch);
                    }
                    catch
                    {
                        e.Value = new Bitmap(5, 5);
                    }
                    break;
                case ScannedGridColumn.Number:
                    e.Value = ch.DisplayChannelNumber;
                    break;
                case ScannedGridColumn.Listing:
                    e.Value = ch.Service.Name + ":" + ch.Service.Provider.Name;
                    break;
                case ScannedGridColumn.Type:
                    e.Value = ch.ChannelType.ToString();
                    break;
                case ScannedGridColumn.TunerCount:
                    e.Value = ChannelEditing.GetTunerCount(ch);
                    break;
                case ScannedGridColumn.Encrypted:
                    e.Value = IsScannedEncrypted(ch);
                    break;
                case ScannedGridColumn.TuningParams:
                    if (ch.TuningInfos != null && !ch.TuningInfos.Empty)
                        e.Value = ChannelEditing.GetTuningParamsAsString(ch.TuningInfos.First);
                    else
                        e.Value = "No tuning infos";
                    break;
                case ScannedGridColumn.Callsign:
                    e.Value = ch.CallSign;
                    break;
            }
        }

        private void ScannedLineupGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= scanned_channels_.Count) return;
            Channel ch = scanned_channels_[e.RowIndex];
            ScannedGridColumn column = ScannedColumnToEnum(e.ColumnIndex);
            switch (column)
            {
                case ScannedGridColumn.Listing:
                    new ListingSelectionForm(ch).ShowDialog();
                    ScannedLineupGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);
                    break;
                case ScannedGridColumn.Remove:
                    try
                    {
                        ChannelEditing.DeleteChannel(ch);
                    }
                    catch (Exception exc)
                    {
                        new ErrorReporter.ErrorReportingForm("Exception occured when attempting to remove channel.", exc);
                    }
                    SelectAndLoadScannedLineup(scanned_lineup_);
                    break;
            }
        }

        private void ScannedLineupGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Comparison<Channel> old_sort = current_scanned_sort_;
            Comparison<Channel> new_sort = null;
            ScannedGridColumn column = ScannedColumnToEnum(e.ColumnIndex);
            switch (column)
            {
                case ScannedGridColumn.Number:
                    new_sort = CompareChannelNumbers;
                    break;
                case ScannedGridColumn.Listing:
                    new_sort = CompareListing;
                    break;
                case ScannedGridColumn.Type:
                    new_sort = CompareChannelType;
                    break;
                case ScannedGridColumn.TunerCount:
                    new_sort = CompareTunerCount;
                    break;
                case ScannedGridColumn.Encrypted:
                    new_sort = CompareScannedEncrypted;
                    break;
                case ScannedGridColumn.TuningParams:
                    new_sort = CompareTuningParams;
                    break;
                case ScannedGridColumn.Callsign:
                    new_sort = CompareCallsigns;
                    break;
            }
            if (new_sort == null) return;
            reverse_scanned_sort_ = (new_sort == old_sort) && !reverse_scanned_sort_;
            current_scanned_sort_ = new_sort;
            SortScannedChannels();
            ScannedLineupGridView.Invalidate();
        }

        private void ScannedLineupGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void ScannedLineupGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= scanned_channels_.Count) return;
            Channel ch = scanned_channels_[e.RowIndex];
            ScannedGridColumn column = ScannedColumnToEnum(e.ColumnIndex);
            switch (column)
            {
                case ScannedGridColumn.Type:
                    try
                    {
                        ChannelType channel_type = (ChannelType)Enum.Parse(typeof(ChannelType), e.FormattedValue.ToString());
                        if (ch.ChannelType == channel_type) return;
                        ch.ChannelType = channel_type;
                        ch.Update();
                    }
                    catch { e.Cancel = true; }
                    break;
            }
        }

        private void scannedChannelCreationControl1_ChannelAdded(object sender, EventArgs e)
        {
            ScannedChannelCreationControl.ChannelAddedEventArgs channel_args = e as ScannedChannelCreationControl.ChannelAddedEventArgs;
            if (channel_args.MergedChannel != null)
            {
                SelectAndLoadLineup((MergedLineup)MergedLineupComboBox.SelectedItem);
                SelectMergedChannel(channel_args.MergedChannel);
            }
            Lineup selected_scanned_lineup = channel_args.ScannedChannel.Lineup;
            SelectAndLoadScannedLineup(selected_scanned_lineup);
            SelectScannedChannel(channel_args.ScannedChannel);
        }

        private void SelectScannedChannel(Channel scanned_channel)
        {
            int scanned_channel_index = -1;
            for (int index = 0; index < scanned_channels_.Count; ++index)
            {
                if (scanned_channels_[index].StoredObjectGuid == scanned_channel.StoredObjectGuid)
                {
                    scanned_channel_index = index;
                    break;
                }
            }
            if (scanned_channel_index >= 0)
            {
                ScannedLineupGridView.ClearSelection();
                ScannedLineupGridView.CurrentCell = ScannedLineupGridView.Rows[scanned_channel_index].Cells[0];
                foreach (DataGridViewCell cell in ScannedLineupGridView.Rows[scanned_channel_index].Cells)
                    cell.Selected = true;
                if (ScannedLineupGridView.FirstDisplayedScrollingRowIndex > scanned_channel_index)
                {
                    ScannedLineupGridView.FirstDisplayedScrollingRowIndex = scanned_channel_index;
                }
                else
                {
                    if (ScannedLineupGridView.DisplayedRowCount(false) + ScannedLineupGridView.FirstDisplayedScrollingRowIndex <= scanned_channel_index)
                        ScannedLineupGridView.FirstDisplayedScrollingRowIndex = scanned_channel_index - ScannedLineupGridView.DisplayedRowCount(false) + 1;
                }
            }
        }

        private void SelectMergedChannel(MergedChannel merged_channel)
        {
            int merged_channel_index = -1;
            for (int index = 0; index < merged_channels_.Count; ++index)
            {
                if (merged_channels_[index].StoredObjectGuid == merged_channel.StoredObjectGuid)
                {
                    merged_channel_index = index;
                    break;
                }
            }
            if (merged_channel_index >= 0)
            {
                MergedLineupGridView.ClearSelection();
                MergedLineupGridView.CurrentCell = MergedLineupGridView.Rows[merged_channel_index].Cells[0];
                foreach (DataGridViewCell cell in MergedLineupGridView.Rows[merged_channel_index].Cells)
                    cell.Selected = true;
                if (MergedLineupGridView.FirstDisplayedScrollingRowIndex > merged_channel_index)
                {
                    MergedLineupGridView.FirstDisplayedScrollingRowIndex = merged_channel_index;
                }
                else
                {
                    if (MergedLineupGridView.DisplayedRowCount(false) + MergedLineupGridView.FirstDisplayedScrollingRowIndex <= merged_channel_index)
                        MergedLineupGridView.FirstDisplayedScrollingRowIndex = merged_channel_index - MergedLineupGridView.DisplayedRowCount(false) + 1;
                }
            }
        }

        private void ScannedLineupGridView_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= scanned_channels_.Count) return;
            Channel ch = scanned_channels_[e.RowIndex];
            ScannedGridColumn column = ScannedColumnToEnum(e.ColumnIndex);
            switch (column)
            {
                case ScannedGridColumn.Callsign:
                    ch.CallSign = e.Value.ToString();
                    ch.Update();
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ChannelEditing.ReleaseHandles();
            // force terminate because of silly guide reindexing thread hanging.
            // Environment.Exit(0);
        }

        private void scannedChannelCreationControl1_Load(object sender, EventArgs e)
        {

        }
    }  // MainForm
}  // namespace
