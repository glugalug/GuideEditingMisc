using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.Store;
using Microsoft.MediaCenter.TV.Tuning;

namespace GuideEditor
{
    class ChannelEditingLib
    {
        public static ObjectStore OpenObjectStore()
        {
            return ObjectStore.Open();
        }

        public class ToStringComparer<T> : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return x.ToString().CompareTo(y.ToString());
            }
        }

        public static List<Lineup> GetLineups(ObjectStore object_store)
        {
            return new Lineups(object_store).ToList();
        }

        public static List<Service> GetServices(ObjectStore object_store)
        {
            List<Service> service_list = new Services(object_store).ToList();
            service_list.Sort(new ToStringComparer<Service>());
            return service_list;
        }

        public static List<Channel> GetUserAddedChannelsForLineup(Lineup lineup)
        {
            List<Channel> user_channels = new List<Channel>();
            foreach (Channel ch in lineup.GetChannels())
            {
                if (ChannelType.UserAdded == ch.ChannelType)
                {
                    user_channels.Add(ch);
                }
            }
            return user_channels;
        }

        public static List<Channel> GetUserAddedChannels(ObjectStore object_store)
        {
            /*using (MergedChannels merged_channels = new MergedChannels(object_store))
            {
                for (MergedChannel merged_channel in merged_channels)
                {
                    for (TuningInfo tuning_info in merged_channel.TuningInfos)
                    {
                        if (tuning_info.Channels.First.Lineup == null)
                        {
                            merged_channel.TuningInfos.RemoveAllMatching(tuning_info);
                        }
                    }
                }
            }*/
            List<Channel> user_added_channels = new List<Channel>();
            Channels full_channel_list = new Channels(object_store);
            foreach (Channel ch in full_channel_list)
            {
                if (ch.ChannelType == ChannelType.UserAdded)
                {
                    if (ch.Lineup == null)
                    {
                        ch.Visibility = ChannelVisibility.NotTunable;
                        foreach (MergedChannel mc in ch.ReferencingSecondaryChannels)
                        {
                            mc.RemoveSecondaryChannel(ch);
                        }
                        foreach(MergedChannel mc in ch.ReferencingPrimaryChannels)
                        {
                            mc.PrimaryChannel = null;
                        }
                        foreach (TuningInfo ti in new TuningInfos(object_store))
                        {
                            if (ti.Channels.Contains(ch))
                            {
                                ti.Channels.RemoveAllMatching(ch);
                            }
                        }
                        ch.Service = null;
                        try
                        {
                            new Channels(object_store).RemoveAllMatching(ch);
                        }
                        catch
                        {
                        }
                        try
                        {
                            new Lineups(object_store).First.DeleteUserAddedChannel(ch);
                        }
                        catch { }
                        try
                        {
                            Lineup oldLineup = ch.TuningInfos.First.Device.ScannedLineup;
                            ch.TuningInfos.Clear();
                            oldLineup.DeleteUserAddedChannel(ch);
                        } catch {
                        }
                        try
                        {
                            new Channels(object_store).RemoveAllMatching(ch);
                        }
                        catch
                        {
                        }
                        ch.UserBlockedState = UserBlockedState.Blocked;
                        ch.CallSign = "DELE";
                    }
                    if (ch.TuningInfos.Empty)
                    {
                        ch.CallSign = "DELE";
                    }
                    user_added_channels.Add(ch);
                }
            }
            return user_added_channels;
        }

        public static void DeleteChannel(Channel channel)
        {
            foreach (TuningInfo tuning_info in channel.TuningInfos)
            {
                if (tuning_info.Channels != null)
                {
                    foreach (Channel ch in tuning_info.Channels)
                    {
                        if (ch.TuningInfos != null)
                            ch.TuningInfos.RemoveAllMatching(tuning_info);
                        ch.Update();
                    }
                }
            }
            foreach (MergedChannel merged_channel in channel.ReferencingSecondaryChannels)
            {
                merged_channel.RemoveSecondaryChannel(channel);
                if (merged_channel.TuningInfos == null || merged_channel.TuningInfos.Empty)
                {
                    if (merged_channel.Lineup != null)
                        merged_channel.Lineup.RemoveChannel(merged_channel);
                }
            }
            foreach (MergedChannel merged_channel in channel.ReferencingPrimaryChannels)
            {
                merged_channel.PrimaryChannel = null;
                if (merged_channel.TuningInfos == null || merged_channel.TuningInfos.Empty)
                {
                    if (merged_channel.Lineup != null)
                        merged_channel.Lineup.RemoveChannel(merged_channel);
                }
            }
            channel.TuningInfos.Clear();
            channel.Update();
            channel.Lineup.RemoveChannel(channel);
        }

        public static string ModulationTypeName(ModulationType modulation_type)
        {
            return modulation_type.ToString().Substring(8); // skip the BDA_MOD_ part.
        }

        public static bool TryGetLineupModulation(Lineup lineup, out ModulationType modulation_type)
        {
            foreach(Channel ch in lineup.GetChannels())
            {
                foreach (TuningInfo tuning_info in ch.TuningInfos)
                {
                    modulation_type = (tuning_info as ChannelTuningInfo).ModulationType;
                    return true;
                }
            }
            modulation_type = ModulationType.BDA_MOD_NOT_SET;
            return false;
        }

        public static string GetTuningSpaceNameForModulation(ModulationType modulation_type)
        {
            switch (modulation_type)
            {
                case ModulationType.BDA_MOD_1024QAM:
                case ModulationType.BDA_MOD_112QAM:
                case ModulationType.BDA_MOD_128QAM:
                case ModulationType.BDA_MOD_160QAM:
                case ModulationType.BDA_MOD_16QAM:
                case ModulationType.BDA_MOD_192QAM:
                case ModulationType.BDA_MOD_224QAM:
                case ModulationType.BDA_MOD_256QAM:
                case ModulationType.BDA_MOD_320QAM:
                case ModulationType.BDA_MOD_32QAM:
                case ModulationType.BDA_MOD_384QAM:
                case ModulationType.BDA_MOD_448QAM:
                case ModulationType.BDA_MOD_512QAM:
                case ModulationType.BDA_MOD_640QAM:
                case ModulationType.BDA_MOD_64QAM:
                case ModulationType.BDA_MOD_768QAM:
                case ModulationType.BDA_MOD_80QAM:
                case ModulationType.BDA_MOD_896QAM:
                case ModulationType.BDA_MOD_96QAM:
                    return "ClearQAM";
                case ModulationType.BDA_MOD_16APSK:
                case ModulationType.BDA_MOD_32APSK:
                case ModulationType.BDA_MOD_8PSK:
                case ModulationType.BDA_MOD_DIRECTV:
                case ModulationType.BDA_MOD_BPSK:
                case ModulationType.BDA_MOD_NBC_8PSK:
                case ModulationType.BDA_MOD_NBC_QPSK:
                case ModulationType.BDA_MOD_QPSK:
                case ModulationType.BDA_MOD_OQPSK:
                    return "DVB-S";
                case ModulationType.BDA_MOD_16VSB:
                    return "ATSCCable";
                case ModulationType.BDA_MOD_8VSB:
                    return "ATSC";
                case ModulationType.BDA_MOD_ANALOG_AMPLITUDE:
                    return "Antenna";
                case ModulationType.BDA_MOD_ANALOG_FREQUENCY:
                    return "FM Radio";
                case ModulationType.BDA_MOD_ISDB_S_TMCC:
                    return "ISDB-S";
                case ModulationType.BDA_MOD_ISDB_T_TMCC:
                    return "ISDB-T";
                case ModulationType.BDA_MOD_RF:
                    return "Cable";
                default:
                    return null;
            }
        }

        public static void AddUserChannelInLineup(Lineup lineup, string callsign, int channel_number, int subchannel, ModulationType modulation_type)
        {
            List<ChannelTuningInfo> tuning_infos = new List<ChannelTuningInfo>();
            foreach (Device device in lineup.ScanDevices)
            {
                string old_tuningspace_name = device.DeviceType.TuningSpaceName;
                if (string.IsNullOrEmpty(old_tuningspace_name))
                {
                    // hacky.  For now I'm just trying to get it to work with ClearQAM.  Ultimately I guess this would be a case
                    // statement picking a value based on modulation_type.  Why is it null for all my devices currently?
                    string temp_tuningspace_name = GetTuningSpaceNameForModulation(modulation_type);
                    // What really seems to matter looking at
                    // ChannelTuningInfo(Device device, SerializationFormat format, byte[] serializedTuneRequest, TuneRequest tr) : base(device, format, serializedTuneRequest, tr)
                    // in .NET Reflector is whether the tuning space has subnumbers or not.  So if we can't figure out
                    // what TuningSpace makes sense, pick one based on if subchannel is 0.
                    if (string.IsNullOrEmpty(temp_tuningspace_name))
                    {
                        temp_tuningspace_name = ((subchannel != 0) ? "ClearQAM" : "Cable");
                    }
                    device.DeviceType.TuningSpaceName = temp_tuningspace_name;
                }
                try
                {
                    ChannelTuningInfo channel_tuning_info = new ChannelTuningInfo(device, channel_number, subchannel, modulation_type);
                    tuning_infos.Add(channel_tuning_info);
                }
                finally
                {
                    device.DeviceType.TuningSpaceName = old_tuningspace_name;
                }
            }
            Channel ch = new Channel();
            ch.CallSign = callsign;
            ch.ChannelType = ChannelType.UserAdded;
            ch.Number = channel_number;
            ch.SubNumber = subchannel;
            lineup.AddChannel(ch);
            foreach (ChannelTuningInfo channel_tuning_info in tuning_infos)
            {
                ch.TuningInfos.Add(channel_tuning_info);
            }
            ch.Update();
            MergedChannel merged_channel = new MergedChannel();
            merged_channel.Number = channel_number;
            merged_channel.SubNumber = subchannel;
            merged_channel.CallSign = callsign;
            merged_channel.ChannelType = ChannelType.UserAdded;
            merged_channel.FullMerge(ch);
            MergedLineup merged_lineup = (lineup.PrimaryProvider != null) ? lineup.PrimaryProvider : lineup.SecondaryProvider;
            if (merged_lineup == null) merged_lineup = new MergedLineups(lineup.ObjectStore).First;
            List<Channel> new_channels_list = new List<Channel>();
            new_channels_list.Add(ch);
            bool old_keepAllPrimary = merged_lineup.LineupMergeRule.KeepAllPrimary;
            bool old_keepAllSecondary = merged_lineup.LineupMergeRule.KeepAllSecondary;
            merged_lineup.LineupMergeRule.KeepAllPrimary = true;
            merged_lineup.LineupMergeRule.KeepAllSecondary = true;
            try
            {
                lineup.NotifyChannelAdded(ch);
//                merged_lineup.OnChannelsAdded(lineup, new_channels_list);
            }
            finally
            {
                merged_lineup.LineupMergeRule.KeepAllPrimary = old_keepAllPrimary;
                merged_lineup.LineupMergeRule.KeepAllSecondary = old_keepAllSecondary;
            } 
        }
    }
}
