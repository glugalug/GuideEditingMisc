using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.Store;
using Microsoft.MediaCenter.TV.Tuning;
using System.Security.Cryptography;

namespace ChannelEditingLib
{
    public class ChannelEditing
    {
        private static bool blocking_background_threads_ = false;

        // Hoping this is no longer needed with admin hack 
        // private static Object lock_obj_ = new object();
        public static ObjectStore object_store
        {
            get
            {
                if (object_store_ == null)
                {
                    /* Hoping this is no longer needed with hack below.
                    lock (lock_obj_)
                    {
                        if (!blocking_background_threads_)
                        {
                            ObjectStore.WaitForThenBlockBackgroundThreads(0x7fffffff);
                            // block background threads that crash when we add a merged channel.
                            // very hacky, but what else to do?
                            blocking_background_threads_ = true;
                        }
                    }*/
                    // Crazy hack to get administrative ObjectStore connection from this thread:
                    // https://social.msdn.microsoft.com/Forums/en-US/ea979075-f602-475d-b485-3a4f787dcb70/new-media-center-addin-x64-microsoftmediacenterguidesubscribed?forum=netfx64bit
                    byte[] bytes = Convert.FromBase64String("FAAODBUITwADRicSARc=");
                    byte[] buffer2 = Encoding.ASCII.GetBytes("Unable upgrade recording state.");
                    for (int i = 0; i != bytes.Length; i++)
                    {
                        bytes[i] = (byte)(bytes[i] ^ buffer2[i]);
                    }
                    string FriendlyName = Encoding.ASCII.GetString(bytes);
                    string clientId = Microsoft.MediaCenter.Store.ObjectStore.GetClientId(true);
                    byte[] buffer = Encoding.Unicode.GetBytes(clientId);
                    string DisplayName = Convert.ToBase64String(new SHA256Managed().ComputeHash(buffer));
                    object_store_ = Microsoft.MediaCenter.Store.ObjectStore.Open("", FriendlyName, DisplayName, true);
                }
                return object_store_;
            }
        }
        private static ObjectStore object_store_;

        public enum TunerType
        {
            CableCARD,
            QAM,
            ATSC,
            Analog,
            DVBT,
            DVBS,
            DVBC,
            Aux,
            Unknown
        }

        public static TunerType GetTunerTypeForLineup(Lineup lineup)
        {
            string name = lineup.Name.ToLower().Replace("-","");
            if (name.Contains("cablecard")) return TunerType.CableCARD;
            if (name.Contains("qam")) return TunerType.QAM;
            if (name.Contains("atsc")) return TunerType.ATSC;
            if (name.Contains("dvbc")) return TunerType.DVBC;
            if (name.Contains("dvbs")) return TunerType.DVBS;
            if (name.Contains("dvb")) return TunerType.DVBT;
            if (name.Contains("analog") || name.Contains("ntsc") || name.Contains("pal")) return TunerType.Analog;
            if (name.Contains("aux")) return TunerType.Aux;
            return TunerType.Unknown;
        }

        public class ToStringComparer<T> : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return x.ToString().CompareTo(y.ToString());
            }
        }

        public static void ReleaseHandles()
        {
            lineups_ = null;
        }
        private static Lineup[] lineups_;

        public static Lineup[] GetLineups()
        {
            if (null == lineups_)
            {
                lineups_ = new Lineups(object_store).ToArray();
            }
            return lineups_;
        }

        public static List<Lineup> GetScannedLineups()
        {
            List<Lineup> scanned_lineups = new List<Lineup>();
            foreach(Lineup lineup in GetLineups())
                if (lineup.ScanDevices.Count() > 0) scanned_lineups.Add(lineup);
            return scanned_lineups;
        }

        public static List<Service> GetServices()
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

        // cleanup invalid/orphaned objects left from previous app runs in the development process.
        public static void CleanupCrud()
        {
            using (Channels full_channel_list = new Channels(object_store))
            {
                foreach (Channel ch in full_channel_list)
                {
                    if (ch.ChannelType == ChannelType.UserAdded)
                    {
                        if (ch.Lineup == null || ch.TuningInfos == null || ch.TuningInfos.Empty)
                        {
                            ch.CallSign = "DELE";
                            ch.Visibility = ChannelVisibility.NotTunable;
                            foreach(MergedChannel mc in ch.ReferencingSecondaryChannels)
                                mc.RemoveSecondaryChannel(ch);
                            foreach(MergedChannel mc in ch.ReferencingPrimaryChannels)
                            {
                                mc.PrimaryChannel = null;
                            }
                            using (TuningInfos tuning_infos = new TuningInfos(object_store))
                            {
                                foreach(TuningInfo ti in tuning_infos)
                                {
                                    if (ti.Channels.Contains(ch))
                                        ti.Channels.RemoveAllMatching(ch);
                                }
                            }
                            ch.Service = null;
                            try {
                                full_channel_list.RemoveAllMatching(ch);
                            } catch {}
                            try {
                                new Lineups(object_store).First.DeleteUserAddedChannel(ch);
                            } catch {}
                            try
                            {
                                Lineup oldLineup = ch.TuningInfos.First.Device.ScannedLineup;
                                ch.TuningInfos.Clear();
                                oldLineup.DeleteUserAddedChannel(ch);
                            } catch {
                            }
                            ch.UserBlockedState = UserBlockedState.Blocked;
                        }
                    }
                }
            }
            using (MergedLineups merged_lineups = new MergedLineups(object_store))
                foreach (MergedLineup merged_lineup in merged_lineups)
                    foreach (Channel ch in merged_lineup.GetChannels())
                        if (ch.TuningInfos.Empty && ch.Number < 9000)
                            merged_lineup.RemoveChannel(ch);
        }

        public static List<Channel> GetUserAddedChannels()
        {
            List<Channel> user_added_channels = new List<Channel>();
            using (Channels full_channel_list = new Channels(object_store))
                foreach (Channel ch in full_channel_list)
                    if (ch.ChannelType == ChannelType.UserAdded)
                        user_added_channels.Add(ch);
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

        public static void FindAndRemoveCableCARDEIAChannel(Lineup cablecard_lineup, ChannelNumber EIA)
        {
            foreach (Channel ch in cablecard_lineup.GetChannels())
            {
                if (ch.TuningInfos != null && !ch.TuningInfos.Empty)
                {
                    ChannelTuningInfo tuning_info = (ChannelTuningInfo)ch.TuningInfos.First;
                    if (tuning_info.PhysicalNumber == EIA.Number && tuning_info.SubNumber == EIA.SubNumber)
                        DeleteChannel(ch);
                }
            }
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
            // A lot of these tuning space names for different modulations are guesses,
            // I am not actually familiar with most of them.  I think all that matters though
            // for the purpose of adding channels is whether or not they have subchannels,
            // and the tuning space name does not appear to be persisted to the db once
            // the process exits.
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
                    return "Digital Cable";
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

        public static Channel AddUserChannelToLineupWithoutMerge(
            Lineup lineup, string callsign,
            int channel_number, int subchannel, ModulationType modulation_type,
            Service service)
        {
            return AddUserChannelToLineupWithoutMerge(lineup, callsign, channel_number, subchannel, modulation_type, service, null);
        }

        private static List<TuningInfo> CreateTuningInfosForChannel(
            int channel_number, int subchannel,
            ModulationType modulation_type, IEnumerable<Device> devices)
        {
            List<TuningInfo> tuning_infos = new List<TuningInfo>();
            foreach (Device d in devices)
            {
                string old_tuningspace_name = d.DeviceType.TuningSpaceName;
                if (string.IsNullOrEmpty(old_tuningspace_name))
                {
                    // hacky.  Why is it null for all my devices currently?
                    string temp_tuningspace_name = GetTuningSpaceNameForModulation(modulation_type);
                    // What really seems to matter looking at
                    // ChannelTuningInfo(Device device, SerializationFormat format, byte[] serializedTuneRequest, TuneRequest tr) : base(device, format, serializedTuneRequest, tr)
                    // in .NET Reflector is whether the tuning space has subnumbers or not.  So if we can't figure out
                    // what TuningSpace makes sense, pick one based on if subchannel is 0.
                    if (string.IsNullOrEmpty(temp_tuningspace_name))
                    {
                        temp_tuningspace_name = ((subchannel != 0) ? "Digital Cable" : "Cable");
                    }
                    d.DeviceType.TuningSpaceName = temp_tuningspace_name;
                }
                ChannelTuningInfo channel_tuning_info = new ChannelTuningInfo(d, channel_number, subchannel, modulation_type);
                tuning_infos.Add(channel_tuning_info);
            }
            return tuning_infos;
        }

        private static List<TuningInfo> CreateDvbTuningInfosForChannel(
            int onid, int tsid, int sid, int nid, int frequency, int lcn,
            IEnumerable<Device> devices)
        {
            List<TuningInfo> tuning_infos = new List<TuningInfo>();
            foreach (Device d in devices)
            {
                DvbTuningInfo dvb_tuning_info = new DvbTuningInfo(d, onid, tsid, sid, nid, frequency, lcn);
                tuning_infos.Add(dvb_tuning_info);
            }
            return tuning_infos;
        }

        private static Channel AddUserChannelToLineupWithoutMerge(
            Lineup lineup, string callsign, int channel_number, int subchannel,
            List<TuningInfo> tuning_infos,
            Service service, ChannelType channel_type=ChannelType.UserAdded)
        {
            Channel ch = new Channel();
            ch.CallSign = callsign;
            ch.ChannelType = channel_type;
            ch.Number = channel_number;
            ch.OriginalNumber = channel_number;
            ch.SubNumber = subchannel;
            ch.OriginalSubNumber = subchannel;
            ch.Visibility = ChannelVisibility.Available;
            lineup.AddChannel(ch);
            foreach (ChannelTuningInfo channel_tuning_info in tuning_infos)
            {
                object_store.Add(channel_tuning_info); // doesn't seem to be needed, but Reflector shows GuideTool does it...
                ch.TuningInfos.Add(channel_tuning_info);
            }
            if (service == null)
            {
                service = new Service(callsign, callsign);
                service.ServiceType = ServiceType.Unknown;
                object_store.Add(service);
            }
            ch.Service = service;
            ch.Update();
            return ch;
        }

        public static Channel AddUserChannelToLineupWithoutMerge(
            Lineup lineup, string callsign,
            int channel_number, int subchannel, ModulationType modulation_type,
            Service service, IEnumerable<Device> devices, ChannelType channel_type=ChannelType.UserAdded)
        {
            if (devices == null)
            {
                List<Device> device_list = new List<Device>();
                foreach (Device d in lineup.ScanDevices)
                  device_list.Add(d);
                devices = device_list;
            }
            List<TuningInfo> tuning_infos = CreateTuningInfosForChannel(channel_number, subchannel, modulation_type, devices);
            Channel ch = AddUserChannelToLineupWithoutMerge(lineup, callsign, channel_number, subchannel, tuning_infos, service, channel_type);
            return ch;
        }

        public static Channel AddDvbUserChannelToLineupWithoutMerge(
            Lineup lineup, string callsign, int channel_number, int subchannel,
            int onid, int tsid, int sid, int nid, int frequency, int lcn,
            Service service, IEnumerable<Device> devices, ChannelType channel_type=ChannelType.UserAdded)
        {
            if (devices == null)
            {
                List<Device> device_list = new List<Device>();
                foreach (Device d in lineup.ScanDevices)
                    //if (d.IsDvbTuningInfoSupported)
                        device_list.Add(d);
                devices = device_list;
            }
            List<TuningInfo> tuning_infos = CreateDvbTuningInfosForChannel(onid, tsid, sid, nid, frequency, lcn, devices);
            Channel ch = AddUserChannelToLineupWithoutMerge(lineup, callsign, channel_number, subchannel, tuning_infos, service, channel_type);
            return ch;
        }
        
        private static MergedLineup GetMergedLineupFromScannedLineup(Lineup scanned_lineup)
        {
            MergedLineup merged_lineup = scanned_lineup.PrimaryProvider;
            if (null == merged_lineup)
            {
                merged_lineup = scanned_lineup.SecondaryProvider;
                if (null == merged_lineup)
                {
                    merged_lineup = new MergedLineups(object_store).First;
                    if (merged_lineup.GetChannels().Length == 0)
                    {
                        foreach (MergedLineup ml in new MergedLineups(object_store))
                        {
                            if (ml.GetChannels().Length > 0)
                            {
                                merged_lineup = ml;
                                break;
                            }
                        }
                    }
                }
            }
            return merged_lineup;
        }

        public static MergedChannel CreateMergedChannelFromChannels(
            List<Channel> channels, int guide_channel_number, int guide_subchannel, ChannelType channel_type=ChannelType.UserAdded)
        {
            try
            {
                Channel primary_channel = channels[0];
                List<Channel> secondary_channels = channels.GetRange(1, channels.Count - 1);
                MergedLineup merged_lineup = GetMergedLineupFromScannedLineup(primary_channel.Lineup);
                MergedChannel merged_channel = new MergedChannel();
                merged_channel.CallSign = primary_channel.CallSign;
                merged_channel.Number = guide_channel_number;
                merged_channel.OriginalNumber = guide_channel_number;
                merged_channel.SubNumber = guide_subchannel;
                merged_channel.OriginalSubNumber = guide_subchannel;
                merged_channel.ChannelType = channel_type;
                merged_channel.PrimaryChannel = primary_channel;
                merged_channel.Service = primary_channel.Service;
                merged_channel.Lineup = merged_lineup;
                new MergedChannels(object_store).Add(merged_channel);
                foreach (TuningInfo tuning_info in primary_channel.TuningInfos)
                    merged_channel.TuningInfos.Add(tuning_info);
                foreach (Channel secondary_channel in secondary_channels)
                {
                    merged_channel.SecondaryChannels.Add(secondary_channel);
                    foreach (TuningInfo tuning_info in secondary_channel.TuningInfos)
                        merged_channel.TuningInfos.Add(tuning_info);
                }
                merged_channel.GetUIdValue(true);
                merged_channel.Update();
                try
                {
                    merged_lineup.AddChannel(merged_channel);
                }
                catch (Exception exc)
                {
                    Console.WriteLine("Caught exception thrown by MergedLineup.AddChannel {0} Message: {1}", exc.ToString(), exc.Message);
                }
                return merged_channel;
            }
            catch (Exception exc)
            {
                if (channels != null)
                {
                    exc.Data.Add("CreateMergedChannelFromChannels_channels.Count", channels.Count);
                    for (int index = 0; index < channels.Count; ++index)
                    {
                        string key_start = string.Format("CreateMergedChannelFromChannels_channels[{0}]", index);
                        Channel channel = channels[index];
                        if (channel != null)
                        {
                            exc.Data.Add(key_start + ".Number", channel.Number);
                            exc.Data.Add(key_start + ".SubNumber", channel.SubNumber);
                        }
                        else
                        {
                            exc.Data.Add(key_start, "NULL");
                        }
                    }
                    if (channels.Count > 0 && channels[0] != null)
                    {
                        if (channels[0].Lineup != null)
                        {
                            exc.Data.Add("CreateMergedChannelFromChannels_channels[0].Lineup", channels[0].Lineup);
                            if (channels[0].Lineup.PrimaryProvider != null)
                            {
                                exc.Data.Add("CreateMergedChannelFromChannels_channels[0].Lineup.PrimaryProvider", channels[0].Lineup.PrimaryProvider);
                            }
                            else
                            {
                                exc.Data.Add("CreateMergedChannelFromChannels_channels[0].Lineup.PrimaryProvider", "NULL");
                            }
                        }
                        else
                        {
                            exc.Data.Add("CreateMergedChannelFromChannels_channels[0].Lineup", "NULL");
                        }
                    }
                }
                else
                {
                    exc.Data.Add("CreateMergedChannelFromChannels_channels", "NULL");
                }
                throw;
            }
        }

        public static void CombineMergedChannels(MergedChannel dest_channel, MergedChannel src_channel, bool src_is_primary, bool remove_src_channel)
        {
            List<TuningInfo> tuning_infos = new List<TuningInfo>();
            tuning_infos.AddRange(dest_channel.TuningInfos.AsEnumerable());

            Channel old_primary = dest_channel.PrimaryChannel;
            foreach (Channel secondary in src_channel.SecondaryChannels)
            {
                dest_channel.SecondaryChannels.Add(secondary);
            }
            if (src_is_primary)
            {
                tuning_infos.InsertRange(0, src_channel.TuningInfos.AsEnumerable());
                dest_channel.PrimaryChannel = src_channel.PrimaryChannel;
                if (old_primary != null)
                    dest_channel.SecondaryChannels.Add(old_primary);
            }
            else
            {
                tuning_infos.AddRange(src_channel.TuningInfos.AsEnumerable());
                if (src_channel.PrimaryChannel != null)
                    dest_channel.SecondaryChannels.Add(src_channel.PrimaryChannel);
            }

            dest_channel.TuningInfos.Clear();
            foreach (TuningInfo ti in tuning_infos)
                dest_channel.TuningInfos.Add(ti);

            dest_channel.Update();

            if (remove_src_channel)
            {
                src_channel.TuningInfos.Clear();
                foreach (Channel ch in src_channel.SecondaryChannels)
                    src_channel.SecondaryChannels.RemoveAllMatching(ch);
                src_channel.PrimaryChannel = null;
                src_channel.Lineup.RemoveChannel(src_channel);
            }
        }

        public static void AddScannedChannelToMergedChannel(Channel scanned_channel, MergedChannel merged_channel, bool add_as_primary)
        {
            if (add_as_primary)
            {
                List<TuningInfo> tuning_infos = merged_channel.TuningInfos.ToList();
                Channel old_primary_channel = merged_channel.PrimaryChannel;
                merged_channel.PrimaryChannel = scanned_channel;
                merged_channel.SecondaryChannels.Add(old_primary_channel);

                tuning_infos.InsertRange(0, scanned_channel.TuningInfos);
                merged_channel.TuningInfos.Clear();
                foreach (TuningInfo ti in tuning_infos)
                    merged_channel.TuningInfos.Add(ti);
            }
            else
            {
                merged_channel.SecondaryChannels.Add(scanned_channel);
                foreach (TuningInfo ti in scanned_channel.TuningInfos)
                    merged_channel.TuningInfos.Add(ti);
            }
            merged_channel.Update();
        }

        public static void AddUserChannelAndMergedChannelInLineup(Lineup lineup, string callsign,
            int channel_number, int subchannel,
            ModulationType modulation_type,
            int guide_channel_number, int guide_subchannel,
            Service service)
        {
            Channel scanned_channel = AddUserChannelToLineupWithoutMerge(lineup, callsign, channel_number, subchannel, modulation_type, service);
            CreateMergedChannelFromChannels(new List<Channel>(new Channel[] {scanned_channel}), guide_channel_number, guide_subchannel);
            return;
        }

        public static List<MergedChannel> GetReferencingMergedChannels(Channel ch)
        {
            List<MergedChannel> merged_channels = new List<MergedChannel>();
            // ignore any UserHidden channels as these remnants from merge operations
            // don't really show up in the guide, and screw up analysis of what
            // listings are used.
            if (ch.ReferencingPrimaryChannels != null)
                foreach (MergedChannel mc in ch.ReferencingPrimaryChannels)
                    if (mc.ChannelType != ChannelType.UserHidden)
                        merged_channels.Add(mc);
            if (ch.ReferencingSecondaryChannels != null)
                foreach (MergedChannel mc in ch.ReferencingSecondaryChannels)
                    if (mc.ChannelType != ChannelType.UserHidden)
                        merged_channels.Add(mc);
            return merged_channels;
        }

        public static void RemoveSubChannel(MergedChannel merged, Channel subchannel)
        {
            RemoveSubChannel(merged, subchannel, true);
        }

        public static void RemoveSubChannel(MergedChannel merged, Channel subchannel, bool delete_if_last_reference)
        {
            foreach (TuningInfo ti in subchannel.TuningInfos)
                merged.TuningInfos.RemoveAllMatching(ti);
            if (merged.PrimaryChannel == subchannel)
            {
                if (!merged.SecondaryChannels.Empty)
                {
                    merged.PrimaryChannel = merged.SecondaryChannels.First;
                    merged.SecondaryChannels.RemoveAllMatching(merged.PrimaryChannel);
                }
            }
            else
            {
                merged.SecondaryChannels.RemoveAllMatching(subchannel);
            }
            merged.Update();
            if (delete_if_last_reference && GetReferencingMergedChannels(subchannel).Count == 0)
                DeleteChannel(subchannel);
        }

        public static List<Channel> GetSubChannels(MergedChannel mc)
        {
            List<Channel> subchannels = new List<Channel>();
            if (mc.PrimaryChannel != null) subchannels.Add(mc.PrimaryChannel);
            subchannels.AddRange(mc.SecondaryChannels.AsEnumerable());
            return subchannels;
        }

        public static void SetSubChannels(MergedChannel mc, List<Channel> subchannels, bool update_tuning_infos)
        {
            if (mc.PrimaryChannel != subchannels[0])
                mc.PrimaryChannel = subchannels[0];
            foreach (Channel ch in mc.SecondaryChannels)
                mc.SecondaryChannels.RemoveAllMatching(ch);
            for (int index = 1; index < subchannels.Count; ++index)
                mc.SecondaryChannels.Add(subchannels[index]);
            if (update_tuning_infos)
            {
                mc.TuningInfos.Clear();
                foreach (Channel ch in subchannels)
                    foreach (TuningInfo ti in ch.TuningInfos)
                        if (ti.Device != null)
                            mc.TuningInfos.Add(ti);
            }
            mc.Update();
        }

        private static List<Lineup> GetLineupsWithWmisDevices()
        {
            List<Lineup> wmis_lineups = new List<Lineup>();
            foreach (Lineup lineup in GetLineups())
                if (!lineup.WmisDevices.Empty)
                    wmis_lineups.Add(lineup);
            return wmis_lineups;
        }

        public static IEnumerable<ChannelService> GetChannelServicesForWmiLineups()
        {
            List<ChannelService> channel_services = new List<ChannelService>();
            foreach (Lineup lineup in GetLineupsWithWmisDevices())
                foreach (Channel ch in lineup.GetChannels())
                    channel_services.Add(new ChannelService(ch.Service, ch.ChannelNumber));
            return channel_services.Distinct();
        }

        public static int GetTunerCount(Channel ch)
        {
            int tuner_count = 0;
            if (ch.TuningInfos == null) return 0;
            foreach (TuningInfo ti in ch.TuningInfos)
            {
                if (ti.Device != null)
                    ++tuner_count;
            }
            return tuner_count;
        }

/*        public static bool ScannedDevicesSupportChannelTuningInfo(Lineup lineup)
        {
            if (lineup.ScanDevices == null)
                return false;

            foreach (Device d in lineup.ScanDevices)
                if (d.IsChannelTuningInfoSupported)
                    return true;

            return false;
        }

        public static bool ScannedDevicesSupportDvbTuningInfo(Lineup lineup)
        {
            if (lineup.ScanDevices == null)
                return false;

            foreach (Device d in lineup.ScanDevices)
                if (d.IsDvbTuningInfoSupported)
                    return true;

            return false;
        }

        public static bool IsScannedDVBLinkTunerInLineup(Lineup lineup)
        {
            if (lineup.ScanDevices == null)
                return false;

            foreach (Device d in lineup.ScanDevices)
                if (d.IsDvbTuningInfoSupported && d.Name.ToLower().Contains("dvblink"))
                    return true;

            return false;
        } */

        public enum TuningInfoType
        {
            ChannelTuningInfo,
            DvbTuningInfo,
            DvbLink
        }

        public static List<Device> GetLineupScannedDevicesByTuningInfoType(Lineup lineup, TuningInfoType tuning_info_type)
        {
            List<Device> devices = new List<Device>();
            if (lineup.ScanDevices == null)
                return devices;

            foreach (Device d in lineup.ScanDevices)
            {
                switch (tuning_info_type)
                {
                    case TuningInfoType.ChannelTuningInfo:
                        // ignore the flags returned by IsChannelTuningInfoSupported and IsDvbTuningInfoSupported
                        // because some tuners (ATI) incorrectly return false.
                        //if (d.IsChannelTuningInfoSupported)
                            devices.Add(d);
                        break;
                    case TuningInfoType.DvbTuningInfo:
                        //if (d.IsDvbTuningInfoSupported)
                            devices.Add(d);
                        break;
                    case TuningInfoType.DvbLink:
                        //if ((d.IsDvbTuningInfoSupported) && (d.Name.ToLower().Contains("dvblink")))
                        if (d.Name.ToLower().Contains("dvblink"))
                            devices.Add(d);
                        break;
                }
            }

            return devices;
        }

        public static string GetTuningParamsAsString(TuningInfo tuning_info)
        {
            ChannelTuningInfo channel_tuning_info = tuning_info as ChannelTuningInfo;
            DvbTuningInfo dvb_tuning_info = tuning_info as DvbTuningInfo;
            StringTuningInfo string_tuning_info = tuning_info as StringTuningInfo;
            if (channel_tuning_info != null)
            {
                if (channel_tuning_info.SubNumber != -1)
                    return string.Format("{0}.{1} ModulationType: {2}", channel_tuning_info.PhysicalNumber, channel_tuning_info.SubNumber, channel_tuning_info.ModulationType);
                else
                    return string.Format("{0} ModulationType: {1}", channel_tuning_info.PhysicalNumber, channel_tuning_info.ModulationType);
            }
            else if (dvb_tuning_info != null)
            {
                return string.Format("Freq: {0} Lcn: {1} Nid: {2} ONid: {3}, Sid: {4}, TSid: {5} SignalQuality: {6}", dvb_tuning_info.Frequency, dvb_tuning_info.Lcn, dvb_tuning_info.Nid, dvb_tuning_info.Onid, dvb_tuning_info.Sid, dvb_tuning_info.Tsid, dvb_tuning_info.SignalQuality);
            }
            else if (string_tuning_info != null)
            {
                return string.Format("TuningString: {0}", string_tuning_info.TuningString);
            }
            return tuning_info.ToString();
        }

    }  // class ChannelEditing

    public class ChannelService
    {
        public ChannelService(Service service, ChannelNumber number)
        {
            service_ = service;
            number_ = number;
        }

        public Service Service { get { return service_; } }
        public ChannelNumber Number { get { return number_; } }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (number_.Number != 0 || number_.SubNumber != 0)
                sb.Append(number_.ToString() + " - ");
            sb.Append(service_);
            return sb.ToString();
        }

        #region "Comparison Functions"
        public static int CompareByService(ChannelService cs1, ChannelService cs2)
        {
            int val = cs1.service_.ToString().CompareTo(cs2.service_.ToString());
            if (val != 0) return val;
            return CompareNumbers(cs1.number_, cs2.number_);
        }
        public static int CompareByNumber(ChannelService cs1, ChannelService cs2)
        {
            int val = CompareNumbers(cs1.number_, cs2.number_);
            if (val != 0) return val;
            return cs1.service_.ToString().CompareTo(cs2.service_.ToString());
        }

        private static int CompareNumbers(ChannelNumber cn1, ChannelNumber cn2)
        {
            int val = cn1.Number - cn2.Number;
            if (val != 0) return val;
            return cn1.SubNumber - cn2.SubNumber;
        }
        #endregion

        private Service service_;
        private ChannelNumber number_;
    }
}  // namespace ChannelEditingLib
