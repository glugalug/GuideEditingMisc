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

namespace InfiniTVToQAMMapper
{
    public partial class MainForm : Form
    {
        private List<ChannelMapEntryWrapper> channel_map_ = null;

        public MainForm()
        {
            InitializeComponent();
            InitChannelMapGrid();
        }

        private Comparison<ChannelMapEntryWrapper> sort_order_ = ChannelMapEntryWrapper.DefaultComparison;
        private bool is_sort_reversed_ = false;

        private void SortGrid()
        {
            channel_map_.Sort(sort_order_);
            if (is_sort_reversed_) channel_map_.Reverse();
            for (int index = 0; index < channel_map_.Count; ++index)
            {
                DataGridViewComboBoxCell cell = ChannelMapGrid.Rows[index].Cells[ActionCol.Index] as DataGridViewComboBoxCell;
                cell.Value = null;
                cell.Items.Clear();
                foreach (CorrectiveAction action in channel_map_[index].GetCorrectiveActions())
                    cell.Items.Add(action);
                cell.Value = channel_map_[index].selected_action;
            }
            ChannelMapGrid.Invalidate();
        }

        private void InitChannelMapGrid()
        {
            Dictionary<int, ChannelMapEntry>.ValueCollection map_entries = ChannelMapParser.channel_map.Values;
            Console.WriteLine("Cross referencing channel map with EPG data and checking status of equivalent QAM channels");
            channel_map_ = new List<ChannelMapEntryWrapper>();
            foreach (ChannelMapEntry entry in ChannelMapParser.channel_map.Values)
            {
                Console.Write("{0} ", entry.Number);
                channel_map_.Add(ChannelMapEntryWrapper.CreateOrGetWrapperForEntry(entry));
            }
            foreach (ChannelMapEntryWrapper wrapper in channel_map_)
                wrapper.StatusChanged += new EventHandler(OnChannelMapEntryWrapperStatusChanged);
            ChannelMapEntryWrapper.SelectDefaultActions();
            ChannelMapGrid.RowCount = channel_map_.Count;
            SortGrid();
        }

        void OnChannelMapEntryWrapperStatusChanged(object sender, EventArgs e)
        {
            ChannelMapEntryWrapper wrapper = (ChannelMapEntryWrapper)sender;
            int index = channel_map_.IndexOf(wrapper);
            ChannelMapGrid.InvalidateRow(index);
        }

        enum ColumnEnum
        {
            Number,
            Callsign,
            Physical,
            Status,
            OTA,
            Listing,
            ChangeListing,
            Action,
            Run,
            Unknown
        };

        private ColumnEnum ColumnIndexToEnum(int col_index)
        {
            if (col_index == NumberColumn.Index) return ColumnEnum.Number;
            if (col_index == CallsignColumn.Index) return ColumnEnum.Callsign;
            if (col_index == PhysicalColumn.Index) return ColumnEnum.Physical;
            if (col_index == StatusCol.Index) return ColumnEnum.Status;
            if (col_index == OTAExistsCol.Index) return ColumnEnum.OTA;
            if (col_index == ListingCol.Index) return ColumnEnum.Listing;
            if (col_index == ChangeListingCol.Index) return ColumnEnum.ChangeListing;
            if (col_index == ActionCol.Index) return ColumnEnum.Action;
            if (col_index == RunCol.Index) return ColumnEnum.Run;
            return ColumnEnum.Unknown;
        }

        private void ChannelMapGrid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            ChannelMapEntryWrapper entry_wrapper = channel_map_[e.RowIndex];
            ChannelMapEntry entry = entry_wrapper.MapEntry;
            switch (ColumnIndexToEnum(e.ColumnIndex))
            {
                case ColumnEnum.Number:
                    e.Value = entry.Number;
                    break;
                case ColumnEnum.Callsign:
                    e.Value = entry.Callsign;
                    break;
                case ColumnEnum.Physical:
                    e.Value = entry.PhysicalChannel;
                    break;
                case ColumnEnum.Status:
                    e.Value = entry_wrapper.QAMStatus;
                    break;
                case ColumnEnum.OTA:
                    e.Value = entry_wrapper.ExistsAsOTA;
                    break;
                case ColumnEnum.Listing:
                    {
                        List<Service> listings = entry_wrapper.listings;
                        switch (listings.Count)
                        {
                            case 1:
                                e.Value = listings[0];
                                break;
                            case 0:
                                e.Value = "None selected";
                                break;
                            default:
                                e.Value = "Multiple selected!!";
                                break;
                        }
                    }
                    break;
                case ColumnEnum.Action:
                    e.Value = entry_wrapper.selected_action;
                    break;
            }
        }

        private void ChannelMapGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void ChannelMapGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            switch (ColumnIndexToEnum(e.ColumnIndex))
            {
                case ColumnEnum.Action:
                    channel_map_[e.RowIndex].selected_action = (CorrectiveAction)Enum.Parse(typeof(CorrectiveAction), e.FormattedValue.ToString());
                    break;
            }

        }

        private void ChannelMapGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= channel_map_.Count) return;
            ChannelMapEntryWrapper entry_wrapper = channel_map_[e.RowIndex];
            switch (ColumnIndexToEnum(e.ColumnIndex)) {
                case ColumnEnum.Run:
                    // GO button was clicked.  Figure out what entry it was, and apply the selected action.
                    try
                    {
                        entry_wrapper.RunSelectedAction();
                    }
                    catch (Exception exc)
                    {
                        StringBuilder err_description =  new StringBuilder();
                        err_description.AppendLine("An error occurred running selected action for channel: ");
                        err_description.AppendLine(entry_wrapper.SerializeForDebug());
                        new ErrorReporter.ErrorReportingForm(err_description.ToString(), exc);
                    }
                    break;
                case ColumnEnum.ChangeListing:
                    DialogResult dr = new ListingSelectionForm(entry_wrapper).ShowDialog();
                    ChannelMapGrid.InvalidateRow(e.RowIndex);
                    break;
            }
        }

        private void NoActionButton_Click(object sender, EventArgs e)
        {
            ChannelMapEntryWrapper.ClearSelectedActions();
            ChannelMapGrid.Invalidate(true);
        }

        private void RecommendedActionButton_Click(object sender, EventArgs e)
        {
            ChannelMapEntryWrapper.SelectDefaultActions();
            ChannelMapGrid.Invalidate(true);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            channel_map_.Sort(ChannelMapEntryWrapper.DefaultComparison);
            StringBuilder error_summary = new StringBuilder();
            bool errors_occurred = false;
            foreach (ChannelMapEntryWrapper wrapper in channel_map_)
            {
                try
                {
                    wrapper.RunSelectedAction();
                }
                catch (Exception exc)
                {
                    errors_occurred = true;
                    error_summary.AppendLine("An error occured while running selected actions on all channels:");
                    error_summary.AppendLine(wrapper.SerializeForDebug());
                    error_summary.AppendLine(ErrorReporter.ErrorReportingForm.SerializeException(exc));
                    error_summary.AppendLine("");
                }
            }
            if (errors_occurred)
            {
                new ErrorReporter.ErrorReportingForm("Error(s) occured while running selected actions on all channels:", null);
            }
            foreach (ChannelMapEntryWrapper wrapper in channel_map_)
                wrapper.UpdateStatus();
            SortGrid();
        }

        private void ChannelMapGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void ChannelMapGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Comparison<ChannelMapEntryWrapper> new_sort_order = null;

            switch (ColumnIndexToEnum(e.ColumnIndex)) {
                case ColumnEnum.Number:
                    new_sort_order = ChannelMapEntryWrapper.CompareNumber;
                    break;
                case ColumnEnum.Callsign:
                    new_sort_order = ChannelMapEntryWrapper.CompareCallsign;
                    break;
                case ColumnEnum.Physical:
                    new_sort_order = ChannelMapEntryWrapper.ComparePhysicalChannel;
                    break;
                case ColumnEnum.Status:
                    new_sort_order = ChannelMapEntryWrapper.DefaultComparison;
                    break;
                case ColumnEnum.OTA:
                    new_sort_order = ChannelMapEntryWrapper.CompareOTA;
                    break;
                case ColumnEnum.Listing:
                    new_sort_order = ChannelMapEntryWrapper.CompareListing;
                    break;
            }
            if (new_sort_order == null) return;

            if (new_sort_order == sort_order_) is_sort_reversed_ = !is_sort_reversed_;
            else is_sort_reversed_ = false;

            sort_order_ = new_sort_order;
            SortGrid();
        }

        private void ChannelMapGrid_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= channel_map_.Count) return;
            ChannelMapEntryWrapper entry_wrapper = channel_map_[e.RowIndex];
            switch (ColumnIndexToEnum(e.ColumnIndex))
            {
                case ColumnEnum.Status:
                    switch (entry_wrapper.QAMStatus)
                    {
                        case QAMChannelStatus.ConflictingCCFirst:
                            e.ToolTipText = "Another CableCARD™ channel with different EIA info is merged with this one, but the sources for this one are earlier in the merged channel.";
                            break;
                        case QAMChannelStatus.ConflictingCCNotFirst:
                            e.ToolTipText = "Another CableCARD™ channel with different EIA info is merged with this one, and the sources for the other channel are earlier in the merged channel.";
                            break;
                        case QAMChannelStatus.ExistingDupeCCard:
                            e.ToolTipText = "QAM source(s) matching this channel exist but are merged with another CableCARD™ channel.";
                            break;
                        case QAMChannelStatus.ExistingUnmerged:
                            e.ToolTipText = "QAM source(s) matching this channel exist but are not merged with this or other CableCARD™ channels.";
                            break;
                        case QAMChannelStatus.IncorrectQAM:
                            e.ToolTipText = "A QAM channel is merged with this one, but it does not match the QAM channel number and program ID for this channel from the channel map.";
                            break;
                        case QAMChannelStatus.NoCCChannelOrQAM:
                            e.ToolTipText = "The CableCARD™ channel does not exist in your guide, and neither does a corresponding QAM channel.";
                            break;
                        case QAMChannelStatus.NoCCChannelWithQAM:
                            e.ToolTipText = "The CableCARD™ channel does not exist in your guide, but a QAM channel with matching EIA number does.";
                            break;
                        case QAMChannelStatus.NotExisting:
                            e.ToolTipText = "QAM sources for this channel do not yet exist in your guide.  This is expected if the channel is encrypted.";
                            break;
                        case QAMChannelStatus.OTAOnly:
                            e.ToolTipText = "QAM sources for this channel do not exist in your guide, and a channel with matching callsign exists in your local OTA lineup, so it should probably be there as ClearQAM.";
                            break;
                        case QAMChannelStatus.ProperlyMerged:
                            e.ToolTipText = "QAM channel for this CableCARD™ channel already exists and is properly merged with it.";
                            break;
                        case QAMChannelStatus.ProperlyMergedWithExtraneousCCard:
                            e.ToolTipText = "Both a matching QAM channel and CableCARD™ sources improperly using the EIA channel number are merged with this channel.  These erroneous CableCARD™ sources should be removed as they will occupy a tuner until reboot if MCE tries to use them.";
                            break;
                    }
                    break;
                case ColumnEnum.Action:
                    e.ToolTipText = ChannelMapEntryWrapper.GetCorrectiveActionDescription(entry_wrapper.selected_action);
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ChannelEditing.ReleaseHandles();
        }
    }
}
