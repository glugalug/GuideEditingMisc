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
    public partial class SourceChannelManagementForm : Form
    {
        public SourceChannelManagementForm(MergedChannel merged_channel)
        {
            InitializeComponent();
            merged_channel_ = merged_channel;
            RefreshSubchannelsGrid();
            InitLineupCombos();
            SortAvailChannels();
        }

        private void InitLineupCombos()
        {
            Lineup[] lineups = ChannelEditing.GetLineups();
            SourceLineupCombo.Items.Clear();
            SourceLineupCombo.Items.AddRange(lineups);
            SourceLineupCombo.SelectedIndex = 0;
        }

        private void RefreshSubchannelsGrid()
        {
            subchannels_ = ChannelEditing.GetSubChannels(merged_channel_);
            SubChannelsGrid.RowCount = subchannels_.Count;
            SubChannelsGrid.Invalidate();
        }

        List<Channel> subchannels_;

        private MergedChannel merged_channel_;
        private Lineup selected_avail_lineup_;
        private List<Channel> avail_channels_;
        private Comparison<Channel> current_avail_sort_ = MainForm.CompareChannelNumbers;
        private bool reverse_avail_sort_ = false;

        #region "Grid Column Enumerations"
        private enum CurrentSourceGridColumn
        {
            Number,
            Type,
            TunerCount,
            Lineup,
            Callsign,
            Listing,
            Promote,
            Demote,
            Remove,
            InheritListing,
            InheritCallsign,
            Unknown
        }

        private enum AvailableSourceGridColumn
        {
            Number,
            Type,
            TunerCount,
            Callsign,
            AddFirst,
            AddLast,
            Listing,
            TuningParams,
            Unknown
        }

        private CurrentSourceGridColumn CurrentSourceColumnFromIndex(int col_index)
        {
            if (col_index == NumberCol.Index) return CurrentSourceGridColumn.Number;
            if (col_index == ChannelTypeCol.Index) return CurrentSourceGridColumn.Type;
            if (col_index == TunerCountCol.Index) return CurrentSourceGridColumn.TunerCount;
            if (col_index == LineupCol.Index) return CurrentSourceGridColumn.Lineup;
            if (col_index == CallsignCol.Index) return CurrentSourceGridColumn.Callsign;
            if (col_index == ListingCol.Index) return CurrentSourceGridColumn.Listing;
            if (col_index == PromoteCol.Index) return CurrentSourceGridColumn.Promote;
            if (col_index == DemoteCol.Index) return CurrentSourceGridColumn.Demote;
            if (col_index == RemoveCol.Index) return CurrentSourceGridColumn.Remove;
            if (col_index == InheritListingCol.Index) return CurrentSourceGridColumn.InheritListing;
            if (col_index == InheritCallsignCol.Index) return CurrentSourceGridColumn.InheritCallsign;
            return CurrentSourceGridColumn.Unknown;
        }

        private AvailableSourceGridColumn AvailableSourceColumnFromIndex(int col_index)
        {
            if (col_index == AvailNumberCol.Index) return AvailableSourceGridColumn.Number;
            if (col_index == AvailTypeCol.Index) return AvailableSourceGridColumn.Type;
            if (col_index == AvailTunerCountCol.Index) return AvailableSourceGridColumn.TunerCount;
            if (col_index == AvailCallsignCol.Index) return AvailableSourceGridColumn.Callsign;
            if (col_index == AddAsFirstCol.Index) return AvailableSourceGridColumn.AddFirst;
            if (col_index == AddAsLastCol.Index) return AvailableSourceGridColumn.AddLast;
            if (col_index == AvailListingCol.Index) return AvailableSourceGridColumn.Listing;
            if (col_index == TuningParamsCol.Index) return AvailableSourceGridColumn.TuningParams;
            return AvailableSourceGridColumn.Unknown;
        }
        #endregion


        private void SwapSubChannelRows(int index1, int index2)
        {
            Channel temp = subchannels_[index1];
            subchannels_[index1] = subchannels_[index2];
            subchannels_[index2] = temp;
            SubChannelsGrid.InvalidateRow(index1);
            SubChannelsGrid.InvalidateRow(index2);
            ChannelEditing.SetSubChannels(merged_channel_, subchannels_, true);
        }

        private void SubChannelsGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= subchannels_.Count) return;
            Channel subchannel = subchannels_[e.RowIndex];
            CurrentSourceGridColumn column = CurrentSourceColumnFromIndex(e.ColumnIndex);
            switch (column)
            {
                case CurrentSourceGridColumn.Callsign:
                    e.Value = subchannel.CallSign;
                    break;
                case CurrentSourceGridColumn.Lineup:
                    e.Value = subchannel.Lineup;
                    break;
                case CurrentSourceGridColumn.Listing:
                    e.Value = subchannel.Service;
                    break;
                case CurrentSourceGridColumn.Number:
                    e.Value = subchannel.DisplayChannelNumber;
                    break;
                case CurrentSourceGridColumn.TunerCount:
                    e.Value = ChannelEditing.GetTunerCount(subchannel);
                    break;
                case CurrentSourceGridColumn.Type:
                    e.Value = subchannel.ChannelType;
                    break;
            }
        }

        private void SourceLineupCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            selected_avail_lineup_ = (Lineup)SourceLineupCombo.SelectedItem;
            avail_channels_ = selected_avail_lineup_.GetChannels().ToList();
            AvailLineupGridView.RowCount = avail_channels_.Count();
            AvailLineupGridView.Invalidate();
        }

        private void AvailLineupGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= avail_channels_.Count()) return;
            Channel ch = avail_channels_[e.RowIndex];
            AvailableSourceGridColumn column = AvailableSourceColumnFromIndex(e.ColumnIndex);
            switch (column)
            {
                case AvailableSourceGridColumn.Callsign:
                    e.Value = ch.CallSign;
                    break;
                case AvailableSourceGridColumn.Listing:
                    e.Value = ch.Service;
                    break;
                case AvailableSourceGridColumn.Number:
                    e.Value = ch.DisplayChannelNumber;
                    break;
                case AvailableSourceGridColumn.TunerCount:
                    e.Value = ChannelEditing.GetTunerCount(ch);
                    break;
                case AvailableSourceGridColumn.Type:
                    e.Value = ch.ChannelType;
                    break;
                case AvailableSourceGridColumn.TuningParams:
                    if (ch.TuningInfos != null && !ch.TuningInfos.Empty)
                    {
                        e.Value = ChannelEditing.GetTuningParamsAsString(ch.TuningInfos.First);
                    }
                    else
                    {
                        e.Value = "No tuning infos";
                    }
                    break;
            }
        }

        private void RemoveSourceChannelButton_Click(object sender, EventArgs e)
        {
            int row_index = SubChannelsGrid.CurrentRow.Index;
            if (row_index < 0 || row_index >= subchannels_.Count) return;
            Channel subchannel = subchannels_[row_index];
            ChannelEditing.RemoveSubChannel(merged_channel_, subchannel, false);
            RefreshSubchannelsGrid();
        }



        private void SubChannelsGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= subchannels_.Count) return;
            Channel subchannel = subchannels_[e.RowIndex];
            CurrentSourceGridColumn column = CurrentSourceColumnFromIndex(e.ColumnIndex);
            switch (column)
            {
                case CurrentSourceGridColumn.Remove:
                    ChannelEditing.RemoveSubChannel(merged_channel_, subchannel, false);
                    RefreshSubchannelsGrid();
                    break;
                case CurrentSourceGridColumn.Promote:
                    if (e.RowIndex > 0)
                        SwapSubChannelRows(e.RowIndex, e.RowIndex - 1);
                    break;
                case CurrentSourceGridColumn.Demote:
                    if (e.RowIndex < subchannels_.Count - 1)
                        SwapSubChannelRows(e.RowIndex, e.RowIndex + 1);
                    break;
                case CurrentSourceGridColumn.InheritCallsign:
                    merged_channel_.CallSign = subchannel.CallSign;
                    merged_channel_.Update();
                    break;
                case CurrentSourceGridColumn.InheritListing:
                    merged_channel_.Service = subchannel.Service;
                    merged_channel_.Update();
                    break;
            }
        }
        private void AvailLineupGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= avail_channels_.Count()) return;
            Channel ch = avail_channels_[e.RowIndex];
            AvailableSourceGridColumn column = AvailableSourceColumnFromIndex(e.ColumnIndex);
            switch (column)
            {
                case AvailableSourceGridColumn.AddFirst:
                    ChannelEditing.AddScannedChannelToMergedChannel(ch, merged_channel_, true);
                    RefreshSubchannelsGrid();
                    break;
                case AvailableSourceGridColumn.AddLast:
                    ChannelEditing.AddScannedChannelToMergedChannel(ch, merged_channel_, false);
                    RefreshSubchannelsGrid();
                    break;
            }
        }

        private void SourceChannelCreationControl_ChannelAdded(object sender, EventArgs e)
        {
            RefreshSubchannelsGrid();
        }

        static int CompareCallsign(Channel ch1, Channel ch2)
        {
            return ch1.CallSign.CompareTo(ch2.CallSign);
        }

        private void AvailLineupGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Comparison<Channel> old_sort = current_avail_sort_;
            AvailableSourceGridColumn column = AvailableSourceColumnFromIndex(e.ColumnIndex);
            Comparison<Channel> new_sort = null;
            switch (column)
            {
                case AvailableSourceGridColumn.Number:
                    new_sort = MainForm.CompareChannelNumbers;
                    break;
                case AvailableSourceGridColumn.Type:
                    new_sort = MainForm.CompareChannelType;
                    break;
                case AvailableSourceGridColumn.TunerCount:
                    new_sort = MainForm.CompareTunerCount;
                    break;
                case AvailableSourceGridColumn.Callsign:
                    new_sort = CompareCallsign;
                    break;
                case AvailableSourceGridColumn.Listing:
                    new_sort = MainForm.CompareListing;
                    break;
                case AvailableSourceGridColumn.TuningParams:
                    new_sort = MainForm.CompareTuningParams;
                    break;
            }
            if (new_sort == null) return;
            reverse_avail_sort_ = (new_sort == old_sort) && !reverse_avail_sort_;
            current_avail_sort_ = new_sort;
            SortAvailChannels();
        }

        private void SortAvailChannels()
        {
            avail_channels_.Sort(current_avail_sort_);
            if (reverse_avail_sort_) avail_channels_.Reverse();
            AvailLineupGridView.Invalidate();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SubChannelsGrid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= subchannels_.Count) return;
            Channel subchannel = subchannels_[e.RowIndex];
            CurrentSourceGridColumn column = CurrentSourceColumnFromIndex(e.ColumnIndex);
            switch (column)
            {
                case CurrentSourceGridColumn.Callsign:
                    subchannel.CallSign = e.Value.ToString();
                    subchannel.Update();
                    break;
            }
        }
    }
}
