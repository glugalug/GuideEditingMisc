using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.TV.Tuning;
using ChannelEditingLib;

namespace InfiniTVToQAMMapper
{
    enum QAMChannelStatus
    {
        IncorrectQAM, // QAM tuning infos exist in this merged channel but they are incorrect.
        ConflictingCCFirst, // Another CableCARD channel with different EIA info is merged with this one, this one is first in the merged channel.
        ConflictingCCNotFirst, // Another CableCARD channel with different EIA info is merged with this one, this one is NOT first in the merged channel.
        NoCCChannelWithQAM, // CableCARD channel for this channel map entry does not exist, but ClearQAM does!
        NoCCChannelOrQAM, // CableCARD channel for this channel map entry does not exist!
        ProperlyMergedWithExtraneousCCard, // QAM tuning infos are in the merged channel, along with CableCARD tuners using EIA improperly.
        OTAOnly,  // QAM tuning info(s) for this channel don't exist, but probably should as it is OTA
        ExistingDupeCCard, // QAM tuning infos matching this channel exist but are merged with another CableCARD channel.
        ExistingUnmerged, // QAM tuning info(s) for this channel exist but are not merged with it.
        NotExisting, // QAM tuning info(s) for this channel do not yet exist.
        ProperlyMerged, // QAM tuning info(s) with the correct parameters already exist in the merged channel.
        StatusValueCount
    };

    enum CorrectiveAction
    {
        None,
        ReplaceIncorrectQAM,
        RemoveIncorrectQAM,
        RemoveOtherCCChannel,
        RemoveThisCCChannel,
        MergeCCChannelToQAMWithNumberUpdate,
        CreateCCChannel,
        CreateMergedCCAndQAMChannels,
        CreateUnmergedCCAndQAMChannels,
        RemoveBogusCCChannel,
        CreateQAMAndMerge,
        CreateQAMWithoutMerge,
        MergeQAM,
    };

    public delegate void StatusChangedEventHandler(object sender, EventArgs e);

    class ChannelMapEntryWrapper : IComparable, ListingSelectionListener
    {
        public static ChannelMapEntryWrapper CreateOrGetWrapperForEntry(ChannelMapEntry entry)
        {
            int number = entry.Number;
            if (wrappers_by_num_.ContainsKey(number))
            {
                ChannelMapEntryWrapper wrapper = wrappers_by_num_[number];
                if (!entry.Equals(wrapper.MapEntry))
                    throw new Exception("Conflicting channel map entry already exists!");
                return wrapper;
            }
            else
            {
                ChannelMapEntryWrapper wrapper = new ChannelMapEntryWrapper(entry);
                wrappers_by_num_.Add(number, wrapper);
                int qam_key = ChannelNumberToInt(entry.PhysicalChannel);
                if (!wrappers_by_qam_key_.ContainsKey(qam_key))
                {
                    wrappers_by_qam_key_.Add(qam_key, new List<ChannelMapEntryWrapper>());
                }
                wrappers_by_qam_key_[qam_key].Add(wrapper);
                return wrapper;
            }
        }

        public string SerializeForDebug()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Channel #: {0} Callsign: {1} Physical #: {2}\n",
                channel_map_entry_.Number, channel_map_entry_.Callsign, channel_map_entry_.PhysicalChannel);
            sb.AppendFormat("Modulation: {0}, QAMStatus: \n", channel_map_entry_.Modulation, qam_status_);
            return sb.ToString();
        }

        public void UpdateStatus()
        {
            merged_channels_ = null;
            listings_ = null;
            QAMChannelStatus old_status = qam_status_;
            qam_status_ = DetermineQAMChannelStatus(channel_map_entry_, out exists_as_ota_);
            if (qam_status_ != old_status)
            {
                CorrectiveAction old_corrective_action = selected_action_;
                selected_action_ = CorrectiveAction.None;
                NotifyStatusChanged(new StatusChangedEventArgs(old_corrective_action, old_status, qam_status_));
            }
        }

        private static void UpdateStatusByQAMKey(int key)
        {
            if (!wrappers_by_qam_key_.ContainsKey(key)) return;
            foreach (ChannelMapEntryWrapper wrapper in wrappers_by_qam_key_[key])
                wrapper.UpdateStatus();
        }

        private static MergedChannel GetFirstMergedChannelByCCNum(int channel_num)
        {
            int channel_key = ChannelNumberToInt(channel_num, 0);
            foreach (Channel ch in ccard_channels_by_num[channel_key])
            {
                List<MergedChannel> referencing_channels = ChannelEditing.GetReferencingMergedChannels(ch);
                if (referencing_channels.Count > 0)
                    return referencing_channels.First();
            }
            return null;
        }

        private static IEnumerable<MergedChannel> GetMergedChannelsByCCNum(int channel_num)
        {
            int key = ChannelNumberToInt(channel_num, 0);
            List<MergedChannel> merged_channels = new List<MergedChannel>();
            foreach (Channel ch in ccard_channels_by_num[key])
                merged_channels.AddRange(ChannelEditing.GetReferencingMergedChannels(ch));
            return merged_channels.Distinct();
        }

        private static IEnumerable<MergedChannel> GetMergedChannelsByQAM(ChannelNumber physical_number)
        {
            int key = ChannelNumberToInt(physical_number);
            List<MergedChannel> merged_channels = new List<MergedChannel>();
            foreach (Channel ch in qam_channels_by_num[key])
                merged_channels.AddRange(ChannelEditing.GetReferencingMergedChannels(ch));
            return merged_channels.Distinct();
        }

        private static MergedChannel GetFirstMergedQAMChannel(ChannelNumber physical_number)
        {
            int channel_key = ChannelNumberToInt(physical_number);
            foreach (Channel ch in qam_channels_by_num[channel_key])
            {
                List<MergedChannel> referencing_channels = ChannelEditing.GetReferencingMergedChannels(ch);
                if (referencing_channels.Count > 0)
                    return referencing_channels.First();
            }
            return null;
        }

        #region "RunSelectedAction handlers"
        private void Action_CreateCChannel()
        {
            ChannelEditing.AddUserChannelAndMergedChannelInLineup(
                cablecard_lineup,
                channel_map_entry_.Callsign, channel_map_entry_.Number, 0, ModulationType.BDA_MOD_NOT_DEFINED,
                channel_map_entry_.Number, 0, selected_listing_);
        }

        private void Action_CreateMergedCCAndQAMChannels()
        {
            Channel qam_channel = ChannelEditing.AddUserChannelToLineupWithoutMerge(
                qam_lineup, channel_map_entry_.Callsign,
                channel_map_entry_.PhysicalChannel.Number, channel_map_entry_.PhysicalChannel.SubNumber, channel_map_entry_.Modulation,
                selected_listing_);
            Channel cc_channel = ChannelEditing.AddUserChannelToLineupWithoutMerge(
                cablecard_lineup, channel_map_entry_.Callsign,
                channel_map_entry_.Number, 0, ModulationType.BDA_MOD_NOT_DEFINED, selected_listing);
            ChannelEditing.CreateMergedChannelFromChannels(
                new List<Channel>(new Channel[] { qam_channel, cc_channel }), channel_map_entry_.Number, 0);
        }

        private void Action_CreateQAMAndMerge()
        {
            MergedChannel merged_channel = GetFirstMergedChannelByCCNum(channel_map_entry_.Number);
            Channel qam_channel = ChannelEditing.AddUserChannelToLineupWithoutMerge(
                qam_lineup, channel_map_entry_.Callsign,
                channel_map_entry_.PhysicalChannel.Number, channel_map_entry_.PhysicalChannel.SubNumber, channel_map_entry_.Modulation,
                selected_listing_);
            ChannelEditing.AddScannedChannelToMergedChannel(qam_channel, merged_channel, true);
        }

        private void Action_CreateQAMWithoutMerge()
        {
            ChannelEditing.AddUserChannelAndMergedChannelInLineup(
                qam_lineup, channel_map_entry_.Callsign,
                channel_map_entry_.PhysicalChannel.Number, channel_map_entry_.PhysicalChannel.SubNumber, channel_map_entry_.Modulation,
                channel_map_entry_.Number, 0,
                selected_listing_);
        }

        private void Action_CreateUnmergedCCAndQAMChannels()
        {
            ChannelEditing.AddUserChannelAndMergedChannelInLineup(
                cablecard_lineup,
                channel_map_entry_.Callsign, channel_map_entry_.Number, 0, ModulationType.BDA_MOD_NOT_DEFINED,
                channel_map_entry_.Number, 0, selected_listing_);
            ChannelEditing.AddUserChannelAndMergedChannelInLineup(
                qam_lineup, channel_map_entry_.Callsign,
                channel_map_entry_.PhysicalChannel.Number, channel_map_entry_.PhysicalChannel.SubNumber, channel_map_entry_.Modulation,
                channel_map_entry_.Number, 0,
                selected_listing_);
        }

        private void Action_MergeCCChannelToQAMWithNumberUpdate()
        {
            MergedChannel merged_channel = GetFirstMergedQAMChannel(channel_map_entry_.PhysicalChannel);
            Channel cc_channel = ChannelEditing.AddUserChannelToLineupWithoutMerge(
                cablecard_lineup,
                channel_map_entry_.Callsign, channel_map_entry_.Number, 0, ModulationType.BDA_MOD_NOT_DEFINED,
                selected_listing_);
            ChannelEditing.AddScannedChannelToMergedChannel(cc_channel, merged_channel, false);
            merged_channel.Number = channel_map_entry_.Number;
            merged_channel.SubNumber = 0;
            merged_channel.Update();
        }

        private void Action_MergeQAM()
        {
            MergedChannel qam_channel = GetFirstMergedQAMChannel(channel_map_entry_.PhysicalChannel);
            MergedChannel cc_channel = GetFirstMergedChannelByCCNum(channel_map_entry_.Number);
            ChannelEditing.CombineMergedChannels(cc_channel, qam_channel, true, true);
        }

        private void Action_RemoveBogusCCChannel()
        {
            ChannelEditing.FindAndRemoveCableCARDEIAChannel(cablecard_lineup, channel_map_entry_.PhysicalChannel);
        }

        private void Action_RemoveIncorrectQAM()
        {
            // loop through the merged channels with this number to find any with incorrect qam and remove it.
            IEnumerable<MergedChannel> merged_channels = GetMergedChannelsByCCNum(channel_map_entry_.Number);
            foreach (MergedChannel mc in merged_channels)
            {
                List<Channel> subchannels = ChannelEditing.GetSubChannels(mc);
                foreach (Channel subchannel in subchannels)
                {
                    if (subchannel.TuningInfos.Empty)
                    {
                        ChannelEditing.DeleteChannel(subchannel);
                        continue;
                    }
                    ChannelTuningInfo ti = subchannel.TuningInfos.First as ChannelTuningInfo;
                    if (ti == null) continue; // skip any TuningInfos that are not ChannelTuningInfo
                    if (!IsTuningInfoOnQAMTuner(ti)) continue;
                    if (ChannelNumberToInt(ti.PhysicalNumber, ti.SubNumber) != ChannelNumberToInt(channel_map_entry_.PhysicalChannel) ||
                        ti.ModulationType != channel_map_entry_.Modulation)
                    {
                        ChannelEditing.RemoveSubChannel(mc, subchannel);
                    }
                }
                foreach (TuningInfo ti in mc.TuningInfos)
                {
                    ChannelTuningInfo cti = ti as ChannelTuningInfo;
                    if (cti == null) continue; // skip any TuningInfos that are not ChannelTuningInfo
                    if (!IsTuningInfoOnQAMTuner(cti)) continue;
                    if (ChannelNumberToInt(cti.PhysicalNumber, cti.SubNumber) != ChannelNumberToInt(channel_map_entry_.PhysicalChannel) ||
                        cti.ModulationType != channel_map_entry_.Modulation)
                        mc.TuningInfos.RemoveAllMatching(ti);
                }
                mc.Update();
            }
        }

        private void Action_RemoveOtherCCChannel()
        {
            IEnumerable<MergedChannel> merged_channels = GetMergedChannelsByCCNum(channel_map_entry_.Number);
            foreach (MergedChannel mc in merged_channels)
            {
                // Check that both matching and non-matching tuning infos exist here.  OTherwise skip.
                bool match_found = false;
                bool nonmatch_found = false;
                foreach (TuningInfo ti in mc.TuningInfos)
                {
                    ChannelTuningInfo cti = ti as ChannelTuningInfo;
                    if (cti == null) continue;  // skip any TuningInfos that are not ChannelTuningInfo
                    if (!IsTuningInfoOnCCardTuner(cti)) continue;
                    if (cti.PhysicalNumber == channel_map_entry_.Number && cti.SubNumber == 0) match_found = true;
                    else nonmatch_found = true;
                    if (match_found && nonmatch_found) break;
                }
                if (!(match_found && nonmatch_found)) continue;

                List<Channel> subchannels = ChannelEditing.GetSubChannels(mc);
                foreach (Channel subchannel in subchannels)
                {
                    if (subchannel.TuningInfos.Empty)
                    {
                        ChannelEditing.DeleteChannel(subchannel);
                        continue;
                    }
                    ChannelTuningInfo ti = subchannel.TuningInfos.First as ChannelTuningInfo;
                    if (ti == null) continue;  // skip any TuningInfos that are not ChannelTuningInfo
                    if (!IsTuningInfoOnCCardTuner(ti)) continue;
                    if (ti.PhysicalNumber != channel_map_entry_.Number || ti.SubNumber != 0)
                        ChannelEditing.RemoveSubChannel(mc, subchannel);
                }
                foreach (TuningInfo ti in mc.TuningInfos)
                {
                    ChannelTuningInfo cti = ti as ChannelTuningInfo;
                    if (cti == null) continue;  // skip any TuningInfos that are not ChannelTuningInfo
                    if (!IsTuningInfoOnCCardTuner(cti)) continue;
                    if (cti.PhysicalNumber != channel_map_entry_.Number || cti.SubNumber != 0)
                        mc.TuningInfos.RemoveAllMatching(ti);
                }
                mc.Update();
                // abort the loop once we have updated at least 1 channel.
                //Chances are these are in pairs with channels needing "REmoveThisCCChannel"
                break;
            }
        }

        private void Action_RemoveThisCCChannel()
        {
            IEnumerable<MergedChannel> merged_channels = GetMergedChannelsByCCNum(channel_map_entry_.Number);
            foreach (MergedChannel mc in merged_channels)
            {
                // Check that both matching and non-matching tuning infos exist here.  OTherwise skip.
                bool match_found = false;
                bool nonmatch_found = false;
                int first_nonmatched_channel_num = 0;
                foreach (TuningInfo ti in mc.TuningInfos)
                {
                    ChannelTuningInfo cti = ti as ChannelTuningInfo;
                    if (cti == null) continue;  // skip any TuningInfos that are not ChannelTuningInfo
                    if (!IsTuningInfoOnCCardTuner(cti)) continue;
                    if (cti.PhysicalNumber == channel_map_entry_.Number && cti.SubNumber == 0)
                    {
                        match_found = true;
                    }
                    else
                    {
                        nonmatch_found = true;
                        first_nonmatched_channel_num = cti.PhysicalNumber;
                    }
                    if (match_found && nonmatch_found) break;
                }
                if (!(match_found && nonmatch_found)) continue;

                List<Channel> subchannels = ChannelEditing.GetSubChannels(mc);
                foreach (Channel subchannel in subchannels)
                {
                    if (subchannel.TuningInfos.Empty)
                    {
                        ChannelEditing.DeleteChannel(subchannel);
                        continue;
                    }
                    ChannelTuningInfo ti = subchannel.TuningInfos.First as ChannelTuningInfo;
                    if (ti == null) continue;  // skip any TuningInfos that are not ChannelTuningInfo
                    if (!IsTuningInfoOnCCardTuner(ti)) continue;
                    if (ti.PhysicalNumber == channel_map_entry_.Number && ti.SubNumber == 0)
                        ChannelEditing.RemoveSubChannel(mc, subchannel);
                }
                foreach (TuningInfo ti in mc.TuningInfos)
                {
                    ChannelTuningInfo cti = ti as ChannelTuningInfo;
                    if (cti == null) continue;  // skip any TuningInfos that are not ChannelTuningInfo
                    if (!IsTuningInfoOnCCardTuner(cti)) continue;
                    if (cti.PhysicalNumber == channel_map_entry_.Number && cti.SubNumber == 0)
                        mc.TuningInfos.RemoveAllMatching(ti);
                }
                // update the channel number to match the new cablecard channel left behind.
                mc.Number = first_nonmatched_channel_num;
                mc.SubNumber = 0;
                mc.Update();
                // abort the loop once we have updated at least 1 channel.
                //Chances are these are in pairs with channels needing "REmoveOtherCCChannel"
                break;
            }  // for each mc in merged_channels
        }

        private void Action_ReplaceIncorrectQAM()
        {
            Action_RemoveIncorrectQAM();
            UpdateStatus();
            switch (qam_status_)
            {
                case QAMChannelStatus.NotExisting:
                    Action_CreateQAMAndMerge();
                    break;
                case QAMChannelStatus.ExistingUnmerged:
                    Action_MergeQAM();
                    break;
                case QAMChannelStatus.ProperlyMerged:
                    break;
                default:
                    Console.WriteLine("Unexpected status after removing incorrect QAM in Action_ReplaceIncorrectQAM");
                    break;
            }
        }
        #endregion

        public void RunSelectedAction()
        {
            switch (selected_action_)
            {
                case CorrectiveAction.CreateCCChannel:
                    Action_CreateCChannel();
                    break;
                case CorrectiveAction.CreateMergedCCAndQAMChannels:
                    Action_CreateMergedCCAndQAMChannels();
                    break;
                case CorrectiveAction.CreateQAMAndMerge:
                    Action_CreateQAMAndMerge();
                    break;
                case CorrectiveAction.CreateQAMWithoutMerge:
                    Action_CreateQAMWithoutMerge();
                    break;
                case CorrectiveAction.CreateUnmergedCCAndQAMChannels:
                    Action_CreateUnmergedCCAndQAMChannels();
                    break;
                case CorrectiveAction.MergeCCChannelToQAMWithNumberUpdate:
                    Action_MergeCCChannelToQAMWithNumberUpdate();
                    break;
                case CorrectiveAction.MergeQAM:
                    Action_MergeQAM();
                    break;
                case CorrectiveAction.None:
                    return;
                case CorrectiveAction.RemoveBogusCCChannel:
                    Action_RemoveBogusCCChannel();
                    break;
                case CorrectiveAction.RemoveIncorrectQAM:
                    Action_RemoveIncorrectQAM();
                    break;
                case CorrectiveAction.RemoveOtherCCChannel:
                    Action_RemoveOtherCCChannel();
                    break;
                case CorrectiveAction.RemoveThisCCChannel:
                    Action_RemoveThisCCChannel();
                    break;
                case CorrectiveAction.ReplaceIncorrectQAM:
                    Action_ReplaceIncorrectQAM();
                    break;
            }
            // update the channel dictionaries each time we make a change.
            // woefully inefficient, but I'm too lazy to just update the appropriate map entries for now.
            UpdateChannelDictionaries();
            UpdateStatusByQAMKey(ChannelNumberToInt(channel_map_entry_.PhysicalChannel));
        }

        public event EventHandler StatusChanged;

        public class StatusChangedEventArgs : EventArgs
        {
            public StatusChangedEventArgs(
                CorrectiveAction old_corrective_action,
                QAMChannelStatus old_status, QAMChannelStatus new_status) : base()
            {
                old_corrective_action_ = old_corrective_action;
                old_status_ = old_status;
                new_status_ = new_status;
            }
            CorrectiveAction old_corrective_action { get { return old_corrective_action_; } }
            QAMChannelStatus old_status { get { return old_status_; } }
            QAMChannelStatus new_status { get { return new_status_; } }
            CorrectiveAction old_corrective_action_;
            QAMChannelStatus old_status_;
            QAMChannelStatus new_status_;
        }

        private void NotifyStatusChanged(EventArgs e)
        {
            if (StatusChanged != null)
                StatusChanged(this, e);
        }

        public static void ClearSelectedActions()
        {
            foreach (ChannelMapEntryWrapper wrapper in wrappers_by_num_.Values)
                wrapper.selected_action_ = CorrectiveAction.None;
        }

        public static void SelectDefaultActions()
        {
            foreach (ChannelMapEntryWrapper wrapper in wrappers_by_num_.Values)
                wrapper.selected_action_ = wrapper.GetDefaultAction();
        }

        public ChannelMapEntry MapEntry { get { return channel_map_entry_; } }
        public QAMChannelStatus QAMStatus { get { return qam_status_; } }
        public bool ExistsAsOTA { get { return exists_as_ota_; } }
        public CorrectiveAction selected_action
        {
            get { return selected_action_; }
            set
            {
                if (GetCorrectiveActionsForStatus(qam_status_).Contains(value))
                {
                    selected_action_ = value;
                }
                else
                {
                    throw new Exception(string.Format("Action {0} can not be applied to channel map entry with status {1}", value, qam_status_));
                }
            }
        }

        public Service selected_listing
        {
            get { return selected_listing_; }
            set { 
                selected_listing_ = value;
                if (value != null)
                    this.listings_ = new Service[] { value }.ToList();
            }
        }

        public CorrectiveAction GetDefaultAction()
        {
            CorrectiveAction action = GetDefaultActionByStatus(qam_status_);
            if (!channel_map_entry_.HasUnsupportedModulation()) return action;
            // can't create "qam" channel if modulation is unsupported.
            switch (action)
            {
                case CorrectiveAction.CreateQAMAndMerge:
                case CorrectiveAction.CreateQAMWithoutMerge:
                case CorrectiveAction.MergeQAM: // makes no sense if we can't even identify it.
                case CorrectiveAction.MergeCCChannelToQAMWithNumberUpdate:
                    return CorrectiveAction.None;
                case CorrectiveAction.CreateMergedCCAndQAMChannels:
                case CorrectiveAction.CreateUnmergedCCAndQAMChannels:
                    return CorrectiveAction.CreateCCChannel;
                case CorrectiveAction.ReplaceIncorrectQAM:
                    return CorrectiveAction.RemoveIncorrectQAM;
            }
            return action;
        }

        public IEnumerable<CorrectiveAction> GetCorrectiveActions()
        {
            CorrectiveAction[] status_actions = GetCorrectiveActionsForStatus(qam_status_);
            if (!channel_map_entry_.HasUnsupportedModulation())
                return status_actions;
            List<CorrectiveAction> viable_actions = new List<CorrectiveAction>();
            foreach (CorrectiveAction action in status_actions)
            {
                switch (action)
                {
                    case CorrectiveAction.CreateCCChannel:
                    case CorrectiveAction.None:
                    case CorrectiveAction.RemoveBogusCCChannel:
                    case CorrectiveAction.RemoveIncorrectQAM:
                    case CorrectiveAction.RemoveOtherCCChannel:
                    case CorrectiveAction.RemoveThisCCChannel:
                        viable_actions.Add(action);
                        break;
                }
            }
            return viable_actions;
        }
        private static CorrectiveAction GetDefaultActionByStatus(QAMChannelStatus status)
        {
            switch (status)
            {
                case QAMChannelStatus.IncorrectQAM:
                    return CorrectiveAction.ReplaceIncorrectQAM;
                case QAMChannelStatus.ConflictingCCFirst:
                    return CorrectiveAction.RemoveOtherCCChannel;
                case QAMChannelStatus.ConflictingCCNotFirst:
                    return CorrectiveAction.RemoveThisCCChannel;
                case QAMChannelStatus.NoCCChannelWithQAM:
                    return CorrectiveAction.MergeCCChannelToQAMWithNumberUpdate;
                case QAMChannelStatus.NoCCChannelOrQAM:
                    return CorrectiveAction.CreateCCChannel;
                case QAMChannelStatus.ProperlyMergedWithExtraneousCCard:
                    return CorrectiveAction.RemoveBogusCCChannel;
                case QAMChannelStatus.OTAOnly:
                    return CorrectiveAction.CreateQAMAndMerge;
                case QAMChannelStatus.ExistingDupeCCard:
                case QAMChannelStatus.ExistingUnmerged:
                    return CorrectiveAction.MergeQAM;
                case QAMChannelStatus.NotExisting:
                case QAMChannelStatus.ProperlyMerged:
                    return CorrectiveAction.None;
                default:
                    throw new Exception("Unknown status value in GetDefaultActionByStatus!");
            }
        }

        private static CorrectiveAction[] GetCorrectiveActionsForStatus(QAMChannelStatus status) {
            if (actions_by_status_ == null)
            {
                actions_by_status_ = new CorrectiveAction[(int)QAMChannelStatus.StatusValueCount][];
                actions_by_status_[(int)QAMChannelStatus.IncorrectQAM] = new CorrectiveAction[] {
                        CorrectiveAction.None,
                        CorrectiveAction.ReplaceIncorrectQAM,
                        CorrectiveAction.RemoveIncorrectQAM };
                actions_by_status_[(int)QAMChannelStatus.ConflictingCCFirst] = new CorrectiveAction[] {
                        CorrectiveAction.None,
                        CorrectiveAction.RemoveOtherCCChannel,
                        CorrectiveAction.RemoveThisCCChannel };
                // same options whether this channel has the 1st tuner info or not, may as well make it same actual array.
                actions_by_status_[(int)QAMChannelStatus.ConflictingCCNotFirst] = actions_by_status_[(int)QAMChannelStatus.ConflictingCCFirst];
                actions_by_status_[(int)QAMChannelStatus.NoCCChannelWithQAM] = new CorrectiveAction[] {
                        CorrectiveAction.None,
                        CorrectiveAction.CreateCCChannel,
                        CorrectiveAction.MergeCCChannelToQAMWithNumberUpdate };
                actions_by_status_[(int)QAMChannelStatus.NoCCChannelOrQAM] = new CorrectiveAction[] {
                        CorrectiveAction.None,
                        CorrectiveAction.CreateCCChannel,
                        CorrectiveAction.CreateMergedCCAndQAMChannels,
                        CorrectiveAction.CreateUnmergedCCAndQAMChannels };
                actions_by_status_[(int)QAMChannelStatus.ProperlyMergedWithExtraneousCCard] = new CorrectiveAction[] {
                        CorrectiveAction.None,
                        CorrectiveAction.RemoveBogusCCChannel };
                actions_by_status_[(int)QAMChannelStatus.OTAOnly] = new CorrectiveAction[] {
                        CorrectiveAction.None,
                        CorrectiveAction.CreateQAMAndMerge,
                        CorrectiveAction.CreateQAMWithoutMerge };
                actions_by_status_[(int)QAMChannelStatus.ExistingDupeCCard] = new CorrectiveAction[] {
                        CorrectiveAction.None,
                        CorrectiveAction.MergeQAM };
                actions_by_status_[(int)QAMChannelStatus.ExistingUnmerged] = actions_by_status_[(int)QAMChannelStatus.ExistingDupeCCard];
                actions_by_status_[(int)QAMChannelStatus.NotExisting] = actions_by_status_[(int)QAMChannelStatus.OTAOnly];
                actions_by_status_[(int)QAMChannelStatus.ProperlyMerged] = new CorrectiveAction[] { CorrectiveAction.None };
            }
            return actions_by_status_[(int)status];
        }
        public static string GetCorrectiveActionDescription(CorrectiveAction action)
        {
            switch (action)
            {
                case CorrectiveAction.CreateCCChannel:
                    return "Create CableCARD™ channel matching this channel map entry";
                case CorrectiveAction.CreateMergedCCAndQAMChannels:
                    return "Create both CableCARD™ and QAM channels matching this channel map entry, and merge them.";
                case CorrectiveAction.CreateQAMAndMerge:
                    return "Create QAM channel and merge it with the existing CableCARD™ channel.";
                case CorrectiveAction.CreateQAMWithoutMerge:
                    return "Create ClearQAM channel, but don't merge it with the existing CableCARD™ channel.";
                case CorrectiveAction.CreateUnmergedCCAndQAMChannels:
                    return "Create CableCARD™ and ClearQAM channels matching this channel map entry, but don't merge them";
                case CorrectiveAction.MergeCCChannelToQAMWithNumberUpdate:
                    return "Create CableCARD™ channel and merge it to the existing QAM channel matching this channel map entry, updating the number in the guide to match the CableCARD™ channel number";
                case CorrectiveAction.MergeQAM:
                    return "Merge Existing ClearQAM channel with this CableCARD™ channel";
                case CorrectiveAction.RemoveBogusCCChannel:
                    return "Remove CableCARD sources incorrectly using EIA (QAM) channel number";
                case CorrectiveAction.RemoveIncorrectQAM:
                    return "Remove merged ClearQAM channel with incorrect EIA number";
                case CorrectiveAction.RemoveOtherCCChannel:
                    return "Remove the other CableCARD channel merged with this one that doesn't have the same EIA number in the channel map.";
                case CorrectiveAction.RemoveThisCCChannel:
                    return "Remove this CableCARD channel, leaving the one that is merged with it which doesn't have the same EIA number in the channel map";
                case CorrectiveAction.ReplaceIncorrectQAM:
                    return "Replace merged ClearQAM channel with incorrect EIA numbers with corrected one";
                default:
                    return action.ToString();
            }
        }

        #region "Comparison functions for changing the grid sort"
        public static int DefaultComparison(ChannelMapEntryWrapper x, ChannelMapEntryWrapper y)
        {
            int status_result = CompareStatus(x, y);
            if (status_result != 0)
                return status_result;
            return CompareNumber(x, y);
        }
        public static int CompareStatus(ChannelMapEntryWrapper x, ChannelMapEntryWrapper y)
        {
            return (int)x.QAMStatus - (int)y.QAMStatus;
        }
        public static int CompareNumber(ChannelMapEntryWrapper x, ChannelMapEntryWrapper y)
        {
            return x.MapEntry.Number - y.MapEntry.Number;
        }
        public static int CompareCallsign(ChannelMapEntryWrapper x, ChannelMapEntryWrapper y)
        {
            return x.MapEntry.Callsign.CompareTo(y.MapEntry.Callsign);
        }
        public static int ComparePhysicalChannel(ChannelMapEntryWrapper x, ChannelMapEntryWrapper y)
        {
            ChannelNumber x_chan = x.MapEntry.PhysicalChannel;
            ChannelNumber y_chan = y.MapEntry.PhysicalChannel;
            int val = x_chan.Number - y_chan.Number;
            if (val != 0) return val;
            return x_chan.SubNumber - y_chan.SubNumber;
        }
        public static int CompareOTA(ChannelMapEntryWrapper x, ChannelMapEntryWrapper y)
        {
            return -y.ExistsAsOTA.CompareTo(x.ExistsAsOTA);
        }
        public static int CompareListing(ChannelMapEntryWrapper x, ChannelMapEntryWrapper y)
        {
            bool x_single_listing = x.listings.Count == 1;
            bool y_single_listing = y.listings.Count == 1;
            if (x_single_listing != y_single_listing) return y_single_listing.CompareTo(x_single_listing);
            if (x.listings.Count != y.listings.Count) return y.listings.Count - x.listings.Count;
            if (x_single_listing)
            {
                return x.listings[0].Name.CompareTo(y.listings[0].Name);
            }
            return 0;
        }
        #endregion

        public List<MergedChannel> merged_channels
        {
            get
            {
                if (null == merged_channels_) merged_channels_ = GetMergedChannels();
                return merged_channels_;
            }
        }
        public List<Service> listings
        {
            get
            {
                if (null == listings_) listings_ = GetListings();
                return listings_;
            }
        }

        private ChannelMapEntryWrapper(ChannelMapEntry entry)
        {
            channel_map_entry_ = entry;
            qam_status_ = DetermineQAMChannelStatus(entry, out exists_as_ota_);
            selected_action_ = CorrectiveAction.None;
        }

        private ChannelMapEntry channel_map_entry_;
        private QAMChannelStatus qam_status_;
        private bool exists_as_ota_;
        private List<MergedChannel> merged_channels_;
        private List<Service> listings_;
        private CorrectiveAction selected_action_ = CorrectiveAction.None;
        private Service selected_listing_ = null;

        #region "private static variables used by utility functions"
        static private Dictionary<int, ChannelMapEntryWrapper> wrappers_by_num_ = new Dictionary<int, ChannelMapEntryWrapper>();
        static private Dictionary<int, List<ChannelMapEntryWrapper>> wrappers_by_qam_key_ =
            new Dictionary<int, List<ChannelMapEntryWrapper>>();
        static private Dictionary<int, List<Channel>> ccard_channels_by_num_ = null;
        static private Dictionary<int, List<Channel>> qam_channels_by_num_ = null;

        static private HashSet<string> ota_callsigns_ = null;

        static private Lineup cablecard_lineup_ = null;
        static private Lineup qam_lineup_ = null;
        private static CorrectiveAction[][] actions_by_status_;
        #endregion

        #region "Utility Functions used to initialize this object"
        private List<MergedChannel> GetMergedChannels()
        {
            int cc_key = ChannelNumberToInt(channel_map_entry_.Number, 0);
            int qam_key = ChannelNumberToInt(channel_map_entry_.PhysicalChannel.Number, channel_map_entry_.PhysicalChannel.SubNumber);
            List<Channel> channels = new List<Channel>();
            if (ccard_channels_by_num.ContainsKey(cc_key))
                channels.AddRange(ccard_channels_by_num[cc_key]);
            if (qam_channels_by_num.ContainsKey(qam_key))
                channels.AddRange(qam_channels_by_num[qam_key]);
            List<MergedChannel> merged_channels = new List<MergedChannel>();
            foreach (Channel ch in channels)
                foreach (MergedChannel mc in ChannelEditing.GetReferencingMergedChannels(ch))
                    if (!merged_channels.Contains(mc))
                        merged_channels.Add(mc);
            return merged_channels;
        }

        private List<Service> GetListings()
        {
            List<Service> temp_listings = new List<Service>();
            foreach (MergedChannel mc in merged_channels)
                if (mc.Service != null && !temp_listings.Contains(mc.Service))
                    temp_listings.Add(mc.Service);
            return temp_listings;
        }


        static private bool FindScannedLineups(out Lineup cablecard_lineup, out Lineup qam_lineup)
        {
            cablecard_lineup = null;
            qam_lineup = null;
            Lineup[] lineups = ChannelEditing.GetLineups();
            foreach (Lineup lineup in lineups)
            {
                switch (lineup.Name)
                {
                    case "Scanned (Digital Cable (CableCARD™))":
                        if (lineup.ScanDevices.Count() > 0)
                            cablecard_lineup = lineup;
                        break;
                    case "Scanned (Digital Cable (ClearQAM))":
                        if (lineup.ScanDevices.Count() > 0)
                            qam_lineup = lineup;
                        break;
                }
            }
            return (cablecard_lineup != null) && (qam_lineup != null);
        }

        static private Dictionary<int, List<Channel>> ccard_channels_by_num
        {
            get
            {
                if (null == ccard_channels_by_num_) UpdateChannelDictionaries();
                return ccard_channels_by_num_;
            }
        }

        static private Dictionary<int, List<Channel>> qam_channels_by_num
        {
            get
            {
                if (null == qam_channels_by_num_) UpdateChannelDictionaries();
                return qam_channels_by_num_;
            }
        }

        // to make map lookup more efficient
        static private int ChannelNumberToInt(int channel_number, int subnumber)
        {
            return (channel_number | (subnumber << 16));
        }

        static private int ChannelNumberToInt(ChannelNumber channel_number)
        {
            return ChannelNumberToInt(channel_number.Number, channel_number.SubNumber);
        }

        static private void FillDictionaryFromLineup(Lineup lineup, Dictionary<int, List<Channel>> channels_by_num)
        {
            foreach (Channel ch in lineup.GetChannels())
            {
                if (ch.TuningInfos != null && ch.TuningInfos.First != null)
                {
                    ChannelTuningInfo channel_tuning_info = ch.TuningInfos.First as ChannelTuningInfo;
                    if (channel_tuning_info == null) continue;  // skip any TuningInfos that are not ChannelTuningInfo
                    if (channel_tuning_info.IsEncrypted) continue; // don't include encrypted QAM.
                    int key = ChannelNumberToInt(channel_tuning_info.PhysicalNumber, channel_tuning_info.SubNumber);
                    if (!channels_by_num.ContainsKey(key)) channels_by_num.Add(key, new List<Channel>());
                    channels_by_num[key].Add(ch);
                }
            }
        }

        static private void UpdateChannelDictionaries()
        {
            Dictionary<int, List<Channel>> ccard_channels = new Dictionary<int,List<Channel>>();
            FillDictionaryFromLineup(cablecard_lineup, ccard_channels);
            ccard_channels_by_num_ = ccard_channels;
            Dictionary<int, List<Channel>> qam_channels = new Dictionary<int, List<Channel>>();
            FillDictionaryFromLineup(qam_lineup, qam_channels);
            qam_channels_by_num_ = qam_channels;
        }

        static private Channel FindCCardChannelForMapEntry(ChannelMapEntry entry, Lineup cablecard_lineup)
        {
            int key = ChannelNumberToInt(entry.Number, 0);
            if (ccard_channels_by_num.ContainsKey(key))
                return ccard_channels_by_num[key].First();
            return null;
        }

        static private bool QAMTuningInfoMatchesMapEntry(ChannelTuningInfo tuning_info, ChannelMapEntry entry)
        {
            return (entry.PhysicalChannel.Number == tuning_info.PhysicalNumber &&
                entry.PhysicalChannel.SubNumber == tuning_info.SubNumber &&
                entry.Modulation == tuning_info.ModulationType);
        }

        static private List<Channel> FindMatchingQAMChannels(ChannelMapEntry entry, Lineup qam_lineup)
        {
            // if the channel has an unsupported modulation, skip it.
            if (entry.HasUnsupportedModulation()) return new List<Channel>();
            int key = ChannelNumberToInt(entry.PhysicalChannel.Number, entry.PhysicalChannel.SubNumber);
            if (qam_channels_by_num.ContainsKey(key))
            {
                List<Channel> matching_channels = qam_channels_by_num[key];
                // remove any where the modulation doesn't match.
                bool has_unmatched_modulation = false;
                foreach (Channel ch in matching_channels)
                {
                    if (ch.TuningInfos == null || ch.TuningInfos.Empty ||
                        ((ChannelTuningInfo)ch.TuningInfos.First).ModulationType != entry.Modulation)
                    {
                        has_unmatched_modulation = true;
                        break;
                    }
                }
                if (!has_unmatched_modulation) return matching_channels;
                List<Channel> filtered_matching_channels = new List<Channel>();
                foreach (Channel ch in matching_channels)
                    if (ch.TuningInfos != null && !ch.TuningInfos.Empty &&
                        ((ChannelTuningInfo)ch.TuningInfos.First).ModulationType == entry.Modulation)
                    {
                        filtered_matching_channels.Add(ch);
                    }
                return filtered_matching_channels;
            }
            return null;
        }

        static private bool IsTuningInfoOnCCardTuner(ChannelTuningInfo tuning_info)
        {
            if (tuning_info.Device == null) return false;
            string guid = tuning_info.Device.StoredObjectGuid;
            foreach (Device device in cablecard_lineup.ScanDevices)
            {
                if (device.StoredObjectGuid == guid)
                    return true;
            }
            return false;
        }

        static private bool IsTuningInfoOnQAMTuner(ChannelTuningInfo tuning_info)
        {
            if (tuning_info.Device == null) return false;
            string guid = tuning_info.Device.StoredObjectGuid;
            foreach (Device device in qam_lineup.ScanDevices)
            {
                if (device.StoredObjectGuid == guid)
                    return true;
            }
            return false;
        }

        static private HashSet<string> ota_callsigns
        {
            get
            {
                if (null == ota_callsigns_) InitOTACallsigns();
                return ota_callsigns_;
            }
        }

        static private void InitOTACallsigns()
        {
            ota_callsigns_ = new HashSet<string>();
            foreach (Lineup lineup in ChannelEditing.GetLineups())
            {
                if (lineup.Name == "Digital Terrestrial Lineup")
                {
                    foreach (Channel ch in lineup.GetChannels())
                        if (ch.Service != null)
                        {
                            string callsign = ch.Service.CallSign;
                            if (!ota_callsigns.Contains(callsign))
                                ota_callsigns_.Add(callsign);
                        }
                }
            }

        }

        static private void InitLineups()
        {
            if (!FindScannedLineups(out cablecard_lineup_, out qam_lineup_))
            {
                StringBuilder sb = new StringBuilder();
                if (null == cablecard_lineup_) sb.Append("Failed to find CableCARD scanned lineup.\n");
                if (null == qam_lineup_) sb.Append("Failed to find ClearQAM scanned lineup.\n");
                sb.Append("The following lineups were found in your system:\n");
                foreach (Lineup lineup in ChannelEditing.GetLineups())
                    sb.Append(lineup.Name + "\n");
                throw new Exception(sb.ToString());
            }
        }
        static private Lineup cablecard_lineup
        {
            get
            {
                if (null == cablecard_lineup_)
                    InitLineups();
                return cablecard_lineup_;
            }
        }
        static private Lineup qam_lineup
        {
            get
            {
                if (null == qam_lineup_)
                    InitLineups();
                return qam_lineup_;
            }
        }

        static private bool ChannelNumbersEquivalent(ChannelNumber cn1, ChannelNumber cn2)
        {
            return cn1.Number == cn2.Number && cn1.SubNumber == cn2.SubNumber;
        }
        static private QAMChannelStatus DetermineQAMChannelStatus(ChannelMapEntry entry, out bool exists_as_ota)
        {
            exists_as_ota = false;
            Channel cablecard_channel = FindCCardChannelForMapEntry(entry, cablecard_lineup);
            List<Channel> matching_qam_channels = FindMatchingQAMChannels(entry, qam_lineup);
            List<MergedChannel> referencing_merged_channels =
                (null == cablecard_channel) ? null : ChannelEditing.GetReferencingMergedChannels(cablecard_channel);

            if (cablecard_channel != null && cablecard_channel.Service != null)
                exists_as_ota = ota_callsigns.Contains(cablecard_channel.Service.CallSign) || ota_callsigns.Contains(cablecard_channel.Service.CallSign + "DT");

            if (null == cablecard_channel || referencing_merged_channels.Count == 0)
            {
                if (null == matching_qam_channels)
                {
                    return QAMChannelStatus.NoCCChannelOrQAM;
                }
                else
                {
                    return QAMChannelStatus.NoCCChannelWithQAM;
                }
            }

            // If we get here, the CableCARD channel at least exists.  Check if it is merged with other CableCARD channels on
            // different QAM channels.  If so, that needs to be fixed before ClearQAM channel state can be worried about.
            foreach (MergedChannel mc in referencing_merged_channels)
            {
                bool this_channel_first = false;
                foreach (TuningInfo ti in mc.TuningInfos)
                {
                    ChannelTuningInfo cti = ti as ChannelTuningInfo;
                    if (cti == null) continue; // skip any TuningInfos that are not ChannelTuningInfo
                    if (IsTuningInfoOnCCardTuner(cti))
                    {
                        int channel_number = cti.PhysicalNumber;
                        if (ChannelMapParser.channel_map.ContainsKey(channel_number) &&
                            ChannelNumbersEquivalent(entry.PhysicalChannel, ChannelMapParser.channel_map[channel_number].PhysicalChannel))
                        {
                            this_channel_first = true;
                        }
                        else
                        {
                            return (this_channel_first) ? QAMChannelStatus.ConflictingCCFirst : QAMChannelStatus.ConflictingCCNotFirst;
                        }
                    }
                }
            }

            if (null == matching_qam_channels)
            {
                foreach (MergedChannel mc in referencing_merged_channels)
                    foreach (TuningInfo ti in mc.TuningInfos)
                    {
                        ChannelTuningInfo cti = ti as ChannelTuningInfo;
                        if (cti == null) continue; // skip any TuningInfos that are not ChannelTuningInfo
                        if (cti.Device == null) continue;
                        // check for non-CableCARD tuning infos in the referencing merged channel
                        if (!IsTuningInfoOnCCardTuner(cti))
                            return QAMChannelStatus.IncorrectQAM;
                    }
                return (exists_as_ota) ? QAMChannelStatus.OTAOnly : QAMChannelStatus.NotExisting;
            }

            // if we get this far, both the cablecard and the QAM channel exist.  Check to see if they are merged together.
            bool found_matching_qam = false;
            bool has_CCard_EIA_merged = false;
            foreach (MergedChannel mc in referencing_merged_channels)
            {
                foreach (TuningInfo ti in mc.TuningInfos)
                {
                    // ignore WMI tuning infos or non-ChannelTuningInfos
                    if (ti.Device == null) continue;
                    ChannelTuningInfo channel_tuning_info = ti as ChannelTuningInfo;
                    if (channel_tuning_info == null) continue;  // skip any TuningInfos that are not ChannelTuningInfo
                    if (IsTuningInfoOnCCardTuner(channel_tuning_info)) {
                        if (channel_tuning_info.SubNumber != 0)
                            has_CCard_EIA_merged = true;
                        else
                            continue;
                    }
                    else if (IsTuningInfoOnQAMTuner(channel_tuning_info))
                    {
                        if (QAMTuningInfoMatchesMapEntry(channel_tuning_info, entry))
                        {
                            found_matching_qam = true;
                        }
                        else
                        {
                            // If there is a non-matching channel merged, that takes priority over whether the
                            // matching QAM channel is also there.
                            return QAMChannelStatus.IncorrectQAM;
                        }
                    }
                    else
                    { // unknown tuner type - ignore for now
                    }
                }
            }
            if (found_matching_qam)
            {
                return (has_CCard_EIA_merged) ?
                    QAMChannelStatus.ProperlyMergedWithExtraneousCCard :
                    QAMChannelStatus.ProperlyMerged;
            }

            // QAM and CCard channels both exist, but are not merged together.  Check if the QAM channel is merged
            // with another CableCARD channel.
            List<MergedChannel> qam_merged_channels = new List<MergedChannel>();
            foreach (Channel ch in matching_qam_channels)
            {
                qam_merged_channels.AddRange(ChannelEditing.GetReferencingMergedChannels(ch));
            }
            foreach (MergedChannel mc in qam_merged_channels)
            {
                foreach (TuningInfo ti in mc.TuningInfos)
                {
                    ChannelTuningInfo cti = ti as ChannelTuningInfo;
                    if (cti == null) continue;  // skip any TuningInfos that are not ChannelTuningInfo
                    if (IsTuningInfoOnCCardTuner(cti))
                        return QAMChannelStatus.ExistingDupeCCard;
                }
            }
            return QAMChannelStatus.ExistingUnmerged;
        }
        #endregion

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            return DefaultComparison(this, (ChannelMapEntryWrapper)obj);
        }

        #endregion

        #region ListingSelectionListener Members

        void ListingSelectionListener.HandleListingSelected(Service listing)
        {
            selected_listing = listing;
            foreach (MergedChannel mc in merged_channels)
            {
                // verify the channel REALLY has a matching tuning info before we update the listing!
                foreach (TuningInfo ti in mc.TuningInfos)
                {
                    ChannelTuningInfo cti = ti as ChannelTuningInfo;
                    if (cti == null) continue; // skip any TuningInfos that are not ChannelTuningInfo
                    if (QAMTuningInfoMatchesMapEntry(cti, channel_map_entry_))
                    {
                        mc.Service = listing;
                        mc.Update();
                        break;
                    }
                    if (IsTuningInfoOnCCardTuner(cti) && cti.PhysicalNumber == channel_map_entry_.Number)
                    {
                        mc.Service = listing;
                        mc.Update();
                        break;
                    }
                }
            }
            listings_.Clear();
            listings_.Add(listing);
        }

        bool ListingSelectionListener.HasCustomServices()
        {
            return (listings.Count > 1);
        }

        IEnumerable<Service> ListingSelectionListener.GetCustomServices()
        {
            return listings;
        }

        #endregion
    }
}
