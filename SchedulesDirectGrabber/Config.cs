﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.Store;
using ChannelEditingLib;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace SchedulesDirectGrabber
{
    using SDStation = StationCache.SDStation;

    public class ConfigManager
    {
        public static ConfigManager instance { get { return instance_; } }
        public static SDGrabberConfig config { get { return instance.config_; } }

        static private ConfigManager instance_ = new ConfigManager();

        private ConfigManager() : this(Properties.Settings.Default.ConfigPath) { }
        private ConfigManager(string config_path)
        {
            this.config_path_ = config_path;
            config_ = new SDGrabberConfig();
            try {
                using (Stream input_stream = File.OpenRead(config_path_)) {
                    using (StreamReader reader = new StreamReader(input_stream))
                    {
                        config_ = JSONClient.Deserialize<SDGrabberConfig>(reader.ReadToEnd());
                    }
                }
                if (config_.lineups == null) config_.lineups = new List<LineupConfig>();
                if (config_.scanned_lineups == null) config_.scanned_lineups = new List<ScannedLineupConfig>();
                config_.ValidateScannedLineupConfigs();
            } catch (Exception ex)
            {
                Misc.OutputException(ex);
            }
        }

        public void SaveConfig()
        {
            using (Stream output_stream = File.OpenWrite(config_path_))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JSONClient.Serialize(config_));
                output_stream.Write(bytes, 0, bytes.Length);
                output_stream.Flush();
                output_stream.Close();
            }
        }

        [DataContract]
        public class SDGrabberConfig
        {
            public SDGrabberConfig()
            {
                lineups = new List<LineupConfig>();
                scanned_lineups = new List<ScannedLineupConfig>();
                imageSelection = new ImageSelectionOptions();
            }

            [DataMember(Name ="username")]
            public string username { get; set; }
            [DataMember(Name ="pwhash")]
            public string pwhash { get; set; }

            [DataMember(Name ="lineups")]
            public List<LineupConfig> lineups { get; set; }

            internal IEnumerable<MXFLineup> GetMXFLineups()
            {
                yield return new MXFLineup();
            }

            [DataMember(Name ="scannedLineups")]
            public List<ScannedLineupConfig> scanned_lineups { get; set; }

            [DataMember(Name = "imageSelection", EmitDefaultValue = true)]
            public ImageSelectionOptions imageSelection { get; set; }

            [DataContract]
            public class ImageSelectionOptions
            {
                public ImageSelectionOptions()
                {
                    selectionAlgorithm = kDefaultSelectionAlgorithm;
                    preferredShowImageWidth = kDefaultPreferredShowImageWidth;
                    preferredShowImageHeight = kDefaultPreferredShowImageHeight;
                    preferredShowImagePixels = kDefaultPreferredShowImagePixels;
                }
                public enum SelectionAlgorithm
                {
                    closestPixels,
                    closestHeight,
                    closestWidth
                }

                [DataMember(Name = "selectionAlgorithm", EmitDefaultValue = true)]
                [DefaultValue(kDefaultSelectionAlgorithm)]
                public SelectionAlgorithm selectionAlgorithm { get; set; }

                [DataMember(Name = "preferredShowImageWidth", EmitDefaultValue = true)]
                [DefaultValue(kDefaultPreferredShowImageWidth)]
                public int preferredShowImageWidth { get; set; }

                [DataMember(Name = "preferredShowImageHeight", EmitDefaultValue = true)]
                [DefaultValue(kDefaultPreferredShowImageHeight)]
                public int preferredShowImageHeight { get; set; }

                [DataMember(Name = "preferredShowImagePixels", EmitDefaultValue = true)]
                [DefaultValue(kDefaultPreferredShowImagePixels)]
                public int preferredShowImagePixels { get; set; }

                private const SelectionAlgorithm kDefaultSelectionAlgorithm = SelectionAlgorithm.closestPixels;
                private const int kDefaultPreferredShowImageWidth = 300;
                private const int kDefaultPreferredShowImageHeight = 225;
                private const int kDefaultPreferredShowImagePixels = kDefaultPreferredShowImageWidth * kDefaultPreferredShowImageHeight;
            }

            public bool ContainsLineup(string sdLineupId)
            {
                if (lineups == null) return false;
                foreach (LineupConfig lineup in lineups)
                    if (lineup.sdLineup.lineup == sdLineupId) return true;
                return false;
            }

            // TODO: This stuff blows up if TV setup is rerun and the list of scanned lineups changes.
            // Detect this and reset that part of the config!
            public LineupConfig GetLineupConfigByID(string id)
            {
                if (lineups == null) return null;
                foreach (var lineup in lineups)
                    if (lineup.sdLineup.lineup == id) return lineup;
                return null;
            }

            public List<Lineup> GetWMCScannedLineupsBySDLineupID(string sd_lineup_id)
            {
                var wmc_lineups = new Lineups(ChannelEditing.object_store);
                List<Lineup> result = new List<Lineup>();
                foreach (var scanned_lineup in scanned_lineups)
                    if (scanned_lineup.sd_lineup_id == sd_lineup_id)
                        result.Add(wmc_lineups[scanned_lineup.id]);
                return result;
            }

            internal void ValidateScannedLineupConfigs()
            {
                for (int index = scanned_lineups.Count - 1; index >= 0; --index)
                {
                    var scanned_lineup = scanned_lineups[index];
                    if (scanned_lineup.FindOrFixScannedLineupAssociation() == null)
                    {
                        MessageBox.Show(string.Format(
                            "Could not find a scanned lineup matching the config with id {0} and name {1}, removing it!",
                            scanned_lineup.id, scanned_lineup.name));
                        scanned_lineups.RemoveAt(index);
                    }
                }
            }
        }

        // Config associated with a SchedulesDirect lineup
        [DataContract]
        public class LineupConfig
        {
            public LineupConfig()
            {
                stationOverrides = new Dictionary<string, StationOverrideConfig>();
            }

            [DataMember(Name = "sdLineup", IsRequired = true)]
            public SDLineup sdLineup { get; set; }
            // Config overrides keyed by station ID
            [DataMember(Name = "stationOverrides")]
            public Dictionary<string, StationOverrideConfig> stationOverrides { get; set; }

            public StationOverrideConfig GetStationOverrides(string stationId)
            {
                if (!sdChannelList_.GetStationsByID().ContainsKey(stationId))
                {
                    throw new Exception("stationId not in lineup");
                }
                if (!stationOverrides.ContainsKey(stationId))
                {
                    stationOverrides[stationId] = new StationOverrideConfig();
                }
                return stationOverrides[stationId];
            }

            internal void PruneStationOverrides()
            {
                List<string> stationIds = stationOverrides.Keys.ToList();
                foreach(var stationId in stationIds)
                {
                    if (stationOverrides[stationId].IsPrunable())
                        stationOverrides.Remove(stationId);                    
                }
            }

            public IEnumerable<SDStation> GetDownloadedStations()
            {
                foreach (var station in sdChannelList.stations)
                {
                    if (stationOverrides.ContainsKey(station.stationID) && stationOverrides[station.stationID].excludeFromDownload)
                    {
                        continue;
                    }
                    yield return station;
                }
            }

            private TuningConfig GetDefaultTuningConfigForChannelAndScannedLineup(SDChannel ch, Lineup lineup)
            {
                var tunerType = ChannelEditing.GetTunerTypeForLineup(lineup);
                List<long> tunerIDs = new List<long>();
                foreach (Device device in lineup.ScanDevices)
                    tunerIDs.Add(device.Id);
                TuningConfig tuning_config = new TuningConfig();
                tuning_config.tunerIDs = tunerIDs;
                switch (tunerType)
                {
                    case ChannelEditing.TunerType.ATSC:
                        if (ch.atscMajor > 1 && ch.atscMinor > 0)
                        {
                            tuning_config.number = ch.atscMajor;
                            tuning_config.subNumber = ch.atscMinor;
                        }
                        else return null; // Tuner not compatible with channel
                        break;
                    case ChannelEditing.TunerType.Aux:
                    case ChannelEditing.TunerType.CableCARD:
                        if (ch.guideChannelMinor > 0) return null;  // not compatible
                        tuning_config.number = ch.guideChannelMajor;
                        break;
                    case ChannelEditing.TunerType.QAM:
                        if (ch.QAMMajor * ch.QAMMinor == 0) return null; // not compatible
                        tuning_config.number = ch.QAMMajor;
                        tuning_config.subNumber = ch.QAMMinor;
                        break;
                    case ChannelEditing.TunerType.Analog:
                        if (ch.uhfVhf > 1) { tuning_config.number = ch.uhfVhf; }
                        else if (ch.guideChannelMinor == 0) { tuning_config.number = ch.guideChannelMajor; }
                        else return null;  // not compatible
                        break;
                    case ChannelEditing.TunerType.DVBT:
                    case ChannelEditing.TunerType.DVBS:
                    case ChannelEditing.TunerType.DVBC:
                        if (ch.frequencyHz > 0)
                        {
                            tuning_config.deliverySystem = ch.deliverySystem;
                            tuning_config.fec = ch.fec;
                            tuning_config.frequencyHz = ch.frequencyHz;
                            tuning_config.modulationSystem = ch.modulationSystem;
                            tuning_config.networkID = ch.networkID;
                            tuning_config.polarization = ch.polarization;
                            tuning_config.serviceID = ch.serviceID;
                            tuning_config.symbolrate = ch.symbolrate;
                            tuning_config.transportID = ch.transportID;
                        }
                        else return null;  // not compatible
                        break;
                    default:
                        Console.WriteLine("Channel not added with scanned lineup {0} because the tuner type is unrecognized", lineup.Name);
                        return null;
                }  // end switch statement
                return tuning_config;
            }

            [IgnoreDataMember]
            public List<Lineup> wmcScannedLineups
            {
                get { return wmcScannedLineups_; }
                set
                {
                    wmcScannedLineups_ = value;
                    defaultStationTuningParams_ = null;  // reset cache of default scanned lineups.
                }
            }
            [IgnoreDataMember]
            private List<Lineup> wmcScannedLineups_ = null;

            // Default tuning configs keyed by station ID
            [IgnoreDataMember]
            public Dictionary<string, List<TuningConfig>> defaultStationTuningParams
            {
                get
                {
                    if (wmcScannedLineups_ == null)
                    {
                        throw new Exception("WMC scanned lineups must be specified before defaultStationTuningParams can be calculated.");
                    }
                    if (defaultStationTuningParams_ == null)
                    {
                        defaultStationTuningParams_ = new Dictionary<string, List<TuningConfig>>();
                        var channelsBystation = sdChannelList.GetChannelsByStation();
                        foreach (string station in channelsBystation.Keys)
                        {
                            defaultStationTuningParams_[station] = new List<TuningConfig>();
                            foreach(var channel in channelsBystation[station])
                            {
                                foreach(var wmcLineup in wmcScannedLineups_)
                                {
                                    var tuning_config = GetDefaultTuningConfigForChannelAndScannedLineup(channel, wmcLineup);
                                    if (tuning_config != null) defaultStationTuningParams_[station].Add(tuning_config);
                                }
                            }
                        }
                    }
                    return defaultStationTuningParams_;
                }
            }

            [IgnoreDataMember]
            private Dictionary<string, List<TuningConfig>> defaultStationTuningParams_ = null;

            // Default channel numbers keyed by station ID
            [IgnoreDataMember]
            public Dictionary<string, List<ChannelNumberConfig>> defaultStationChannelNumbers {
                get
                {
                    if (defaultStationChannelNumbers_ == null)
                    {
                        defaultStationChannelNumbers_ = new Dictionary<string, List<ChannelNumberConfig>>();
                        var channelsBystation = sdChannelList.GetChannelsByStation();
                        foreach(string station in channelsBystation.Keys)
                        {
                            defaultStationChannelNumbers_[station] = new List<ChannelNumberConfig>();
                            foreach (var channel in channelsBystation[station])
                            {
                                ChannelNumber number = channel.GetChannelNumber();
                                if (number != null) defaultStationChannelNumbers_[station].Add(new ChannelNumberConfig(number));
                            }
                        }
                    }
                    return defaultStationChannelNumbers_;
                }
            }
            [IgnoreDataMember]
            public IEnumerable<string> stationIDs { get { return defaultStationChannelNumbers.Keys; } }

            public List<ChannelNumberConfig> EffectiveStationChannelNumbers(string stationID)
            {
                if (stationOverrides.ContainsKey(stationID))
                {
                    var overrideConfig = stationOverrides[stationID];
                    if (overrideConfig.excludeFromGuide) return new List<ChannelNumberConfig>();
                    if (overrideConfig.guideChannelNumbers != null)
                        return overrideConfig.guideChannelNumbers;
                }
                return defaultStationChannelNumbers[stationID];
            }

            public List<TuningConfig> EffectivePhysicalChannelNumbers(string stationID)
            {
                if (stationOverrides.ContainsKey(stationID))
                {
                    var overrideConfig = stationOverrides[stationID];
                    if (overrideConfig.excludeFromGuide) return new List<TuningConfig>();
                    if (overrideConfig.tuningOverrides != null)
                        return overrideConfig.tuningOverrides;
                }
                if (wmcScannedLineups?.Count > 0)
                    return defaultStationTuningParams[stationID];
                return new List<TuningConfig>();
            }

            public SDStation GetStationByID(string stationID)
            {
                return sdChannelList.GetStationsByID()[stationID];
            }

            public bool ExcludedFromDownload(string stationID)
            {
                if (!stationOverrides.ContainsKey(stationID)) return false;
                return stationOverrides[stationID].excludeFromDownload;
            }

            public bool ExcludedFromGuide(string stationID)
            {
                if (!stationOverrides.ContainsKey(stationID)) return false;
                return stationOverrides[stationID].excludeFromGuide;
            }

            [IgnoreDataMember]
            private Dictionary<string, List<ChannelNumberConfig>> defaultStationChannelNumbers_= null;

            [IgnoreDataMember]
            private SDChannelList sdChannelList {
                get
                {
                    if (sdChannelList_ == null)
                    {
                        sdChannelList_ = SDChannelReader.GetChannelListByLineupUri(sdLineup.uri);
                    }
                    return sdChannelList_;
                }
            }

            [IgnoreDataMember]
            private SDChannelList sdChannelList_ = null;

            public override string ToString()
            {
                return sdLineup.ToString();
            }

            internal void SetExcludedFromDownload(string stationID, bool exclude)
            {
                GetStationOverrides(stationID).excludeFromDownload = exclude;
            }

            internal void SetExcludedFromGuide(string stationID, bool exclude)
            {
                GetStationOverrides(stationID).excludeFromGuide = exclude;
            }
        }

        [DataContract]
        public class StationOverrideConfig
        {
            // Used to match the overrides with channels, rather than the channel # that may or may not
            // be in the map.
            [DataMember(Name = "sdServiceId", IsRequired =true)]
            public string sdServiceId { get; set; }
            [DataMember(Name = "excludeFromDownload")]
            public bool excludeFromDownload
            {
                get { return excludeFromDownload_; }
                set { excludeFromDownload_ = value; excludeFromGuide_ = excludeFromGuide_ || value; }
            }
            private bool excludeFromDownload_;
            [DataMember(Name = "excludeFromGuide")]
            public bool excludeFromGuide {
                get { return excludeFromGuide_; }
                set { excludeFromGuide_ = value;  excludeFromDownload_ = excludeFromDownload_ && value; }
            }
            private bool excludeFromGuide_;
            // Note that the default values for these override members are **null**. 
            // The list should only be created if explicitly overridden, in which case
            // an empty List means the channel is either not tunable, or has no channel numbers to
            // put in the guide.
            [DataMember(Name ="guideChannelNumbers")]
            public List<ChannelNumberConfig> guideChannelNumbers { get; set; }
            [DataMember(Name = "tuningOverrides")]
            public List<TuningConfig> tuningOverrides { get; set; }

            internal bool IsPrunable()
            {
                return !excludeFromDownload && !excludeFromGuide && guideChannelNumbers == null && tuningOverrides == null;
            }
        }

        [DataContract]
        public class ScannedLineupConfig
        {
            public ScannedLineupConfig()
            {
                addAssociation = true;
                removeOtherAssociations = true;
            }

            [DataMember(Name = "id", IsRequired = true)]
            public long id { get; set; }
            [DataMember(Name = "name", IsRequired = true, EmitDefaultValue =true)]
            public string name { get; set; }
            [DataMember(Name = "sdLineupId")]
            public string sd_lineup_id;
            [DataMember(Name = "addAssociation"), DefaultValue(true)]
            public bool addAssociation;
            [DataMember(Name = "removeOtherAssociations"), DefaultValue(true)]
            public bool removeOtherAssociations;

            public enum ExistingChannelCleanupOption
            {
                RemoveAutomaticallyAddedChannels,
                RemoveAllExistingChannels,
                DoNotRemoveExistingChannels
            }

            [DataMember(Name = "existingChannelCleanup"),
             DefaultValue(ExistingChannelCleanupOption.RemoveAutomaticallyAddedChannels)]
            public ExistingChannelCleanupOption existingChannelCleanup { get; set; }

            internal Lineup FindOrFixScannedLineupAssociation()
            {
                var wmc_lineups = new Lineups(ChannelEditing.object_store);
                foreach(Lineup lineup in wmc_lineups)
                    if (lineup.Id == id) return lineup;
                foreach(Lineup lineup in wmc_lineups)
                    if (lineup.Name == name)
                    {
                        name = lineup.Name;
                        MessageBox.Show("Scanned lineup with ID matching config not found, but matched by name.  Was TV Setup Rerun?");
                        return lineup;
                    }
                return null;
            }
        }

        private SDGrabberConfig config_;
        private string config_path_;

        public class MXFLineup
        {
            public MXFLineup() { }

            [XmlIgnore]
            internal const string mxfId = "l1";

            [XmlAttribute("id")]
            public string id {
                get { return mxfId; }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("uid")]
            public string uid
            {
                get { return "!Lineup!" + GetLineupIdSuffix(); }
                set { throw new NotImplementedException(); }
            }

            public static string GetLineupIdSuffix()
            {
                return "GSD";
            }

            [XmlAttribute("name")]
            public string name
            {
                get { return "SchedulesDirect lineup via Glugglug's SchedulesDirect Grabber"; }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("primaryProvider")]
            public string primaryProvider
            {
                get { return "!MCLineup!MainLineup"; }
                set { throw new NotImplementedException(); }
            }

            // This is the only lowercased element name in the MXF format.  Typo in the documentation?
            [XmlArray("channels"), XmlArrayItem("Channel")]
            public MXFData.EnumerableSerializationWrapper<MXFChannel> channels
            {
                get { return new MXFData.EnumerableSerializationWrapper<MXFChannel>(StationCache.instance.GetMXFChannels()); }
                set { throw new NotImplementedException(); }
            }
            public class MXFChannel
            {
                public MXFChannel() { }
                internal MXFChannel(ChannelNumberConfig channelNumberConfig, SDStation station)
                {
                    channelNumberConfig_ = channelNumberConfig;
                    station_ = station;
                }

                [XmlAttribute("uid")]
                public string uid
                {
                    get
                    {
                        return (channelNumberConfig_ != null)
                          ? string.Format("!Channel!{0}{1}_{2}_{3}",
                              MXFLineup.GetLineupIdSuffix(), station_.stationID, number, subNumber)
                          : string.Format("!Channel!{0}{1}", MXFLineup.GetLineupIdSuffix(), station_.stationID);
                    }
                    set { throw new NotImplementedException(); }
                }

                [XmlAttribute("lineup")]
                public string lineup
                {
                    get { return MXFLineup.mxfId; }
                    set { throw new NotImplementedException(); }
                }

                [XmlAttribute("service")]
                public string service
                {
                    get { return StationCache.instance.GetServiceIdByStationId(station_.stationID); }
                    set { throw new NotImplementedException(); }
                }

                [XmlAttribute("matchName"), DataMember(EmitDefaultValue = false)]
                public string matchName
                {
                    get { return string.Empty; }  // TODO: the MXF format doc says DVBT:onid:tsid:sid or DVBS:sat:freq:onid:tsid:sid
                                                  // can be used for DVB
                    set { throw new NotImplementedException(); }
                }

                [XmlAttribute("number"), DataMember(EmitDefaultValue = false), DefaultValue(-1)]
                public int number
                {
                    get { return (int)(channelNumberConfig_?.number ?? -1); }
                    set { throw new NotImplementedException(); }
                }

                [XmlAttribute("subNumber"), DataMember(EmitDefaultValue = false), DefaultValue(0)]
                public int subNumber
                {
                    get { return (int)(channelNumberConfig_?.subNumber ?? 0); }
                    set { throw new NotImplementedException(); }
                }

                private ChannelNumberConfig channelNumberConfig_;
                private SDStation station_;
            }
        }
    }
    [DataContract]
    public class ChannelNumberConfig : IComparable, IComparable<ChannelNumberConfig>
    {
        public ChannelNumberConfig() { }
        public ChannelNumberConfig(ChannelNumber channel_number)
        {
            number = channel_number.Number;
            subNumber = channel_number.SubNumber;
        }
        [DataMember(Name = "number", IsRequired = true)]
        public int number { get; set; }
        [DataMember(Name = "subNumber")]
        public int subNumber { get; set; }

        int IComparable<ChannelNumberConfig>.CompareTo(ChannelNumberConfig other)
        {
            if (this.number != other.number) return this.number - other.number;
            return this.subNumber - other.subNumber;
        }

        int IComparable.CompareTo(object obj)
        {
            ChannelNumberConfig other = obj as ChannelNumberConfig;
            if (other == null) throw new NotImplementedException();
            return ((IComparable<ChannelNumberConfig>)this).CompareTo(other);
        }

        public static int CompareChannels(ChannelNumberConfig left, ChannelNumberConfig right)
        {
            return (left as IComparable<ChannelNumberConfig>).CompareTo(right);
        }
        public override string ToString()
        {
            return (subNumber > 0) ? string.Format("{0}-{1}", number, subNumber) : number.ToString();
        }

        // OVerride Equals and GetHashCode so that ChannelNumbers can work properly in sets or as dictionary keys.
        public override bool Equals(object obj)
        {
            ChannelNumberConfig other = obj as ChannelNumberConfig;
            if (other == null) return false;
            return other.number == this.number && other.subNumber == this.subNumber;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

    [DataContract]
    public class TuningConfig
    {
        [DataMember(Name = "tunerIDs", IsRequired = true)]
        public List<long> tunerIDs { get; set; }
        [DataMember(Name = "number")]
        public int number { get; set; }
        [DataMember(Name = "subNumber")]
        public int subNumber { get; set; }
        #region DVB
        [DataMember(Name = "frequencyHz")]
        public long frequencyHz { get; set; }
        [DataMember(Name = "serviceID")]
        public int serviceID { get; set; }
        [DataMember(Name = "networkID")]
        public int networkID { get; set; }
        [DataMember(Name = "transportID")]
        public int transportID { get; set; }
        #region DVB-C/DVB-S
        [DataMember(Name = "deliverySystem")]
        public string deliverySystem { get; set; }
        [DataMember(Name = "modulationSystem")]
        public string modulationSystem { get; set; }
        [DataMember(Name = "symbolrate")]
        public int symbolrate { get; set; }
        // additional for DVB-S
        [DataMember(Name = "polarization")]
        public string polarization { get; set; }
        [DataMember(Name = "fec")]
        public string fec { get; set; }
        #endregion
        #endregion
        public static IDictionary<long, string> GetTunerNamesByID()
        {
            if (tunerNamesByID_ == null)
            {
                tunerNamesByID_ = new Dictionary<long, string>();
                foreach (Device d in new Devices(ChannelEditing.object_store))
                {
                    Device device = d as Device;
                    tunerNamesByID_[device.Id] = device.Name;
                }
            }
            return tunerNamesByID_;
        }

        bool HasDVBParams()
        {
            return serviceID > 0 || networkID > 0 || transportID > 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (number > 0) sb.Append(number.ToString());
            if (subNumber > 0)
            {
                sb.Append('-');
                sb.Append(subNumber.ToString());
            }
            if (HasDVBParams())
            {
                sb.Append(string.Format(" DVB: frequency: {0} service: {1} networkID: {2} transportID: {3}",
                    frequencyHz, serviceID, networkID, transportID));
            }
            List<string> tunerNames = new List<string>();
            var tunerNamesByID = GetTunerNamesByID();
            foreach (long id in tunerIDs)
            {
                tunerNames.Add(tunerNamesByID[id]);
            }
            sb.Append(" Tuners: ");
            sb.Append(string.Join(",", tunerNames));
            return sb.ToString();
        }
        private static Dictionary<long, string> tunerNamesByID_ = null;
    }
}