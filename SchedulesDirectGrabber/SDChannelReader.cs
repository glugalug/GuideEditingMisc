using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.MediaCenter.Guide;

namespace SchedulesDirectGrabber
{
    using SDStation = StationCache.SDStation;

    public class SDChannelReader
    {
        public static SDChannelList GetChannelListByLineupUri(string lineupuri)
        {
            return JSONClient.GetJSONResponse<SDChannelList>(
                UrlBuilder.BuildWithBasePrefix(lineupuri), JSONClient.empty_request,
                SDTokenManager.token_manager.token);
        }

    }

    [DataContract]
    public class SDChannelList
    {
        [DataMember(Name = "map", IsRequired = true)]
        public List<SDChannel> channel_map{ get; set; }
        [DataMember(Name = "stations", IsRequired = true)]
        public List<SDStation> stations { get; set; }
        [DataMember(Name = "metadata")]
        public Metadata metadata { get; set; }

        [DataContract]
        public class Metadata
        {
            [DataMember(Name ="lineup")]
            public string lineup { get; set; }
            [DataMember(Name ="modified")]
            public DateTime modified { get; set; }
            [DataMember(Name ="transport")]
            public string transport { get; set; }
            [DataMember(Name ="modulation")]
            public string modulation { get; set; }
        }

        [IgnoreDataMember]
        private SortedDictionary<string, List<SDChannel>> channels_by_station_ = null;

        public SortedDictionary<string, List<SDChannel>> GetChannelsByStation()
        {
            if (channels_by_station_ == null)
            {
                channels_by_station_ = new SortedDictionary<string, List<SDChannel>>();
                foreach (var channel in channel_map)
                {
                    if (!channels_by_station_.ContainsKey(channel.stationID))
                    {
                        channels_by_station_[channel.stationID] = new List<SDChannel>();
                    }
                    channels_by_station_[channel.stationID].Add(channel);
                }
            }
            return channels_by_station_;
        }

        [IgnoreDataMember]
        private SortedDictionary<string, SDChannel> channels_by_sort_key_ = null;
        public SortedDictionary<string, SDChannel> GetChannelsBySortKey()
        {
            if (channels_by_sort_key_ == null)
            {
                channels_by_sort_key_ = new SortedDictionary<string, SDChannel>();
                foreach (var channel in channel_map)
                    channels_by_sort_key_[channel.sort_key] = channel;
            }
            return channels_by_sort_key_;
        }
        private Dictionary<string, SDStation> stations_by_id_ = null;
        internal Dictionary<string, SDStation> GetStationsByID()
        {
            if (stations_by_id_ == null)
            {
                stations_by_id_ = new Dictionary<string, SDStation>();
                foreach (var station in stations)
                    stations_by_id_[station.stationID] = station;
            }
            return stations_by_id_;
        }
    }

    [DataContract]
    public class SDChannel
    {
        #region stationID and channel required for every SDChannel
        [DataMember(Name = "stationID", IsRequired = true)]
        public string stationID { get; set; }

        // "Required" in documentation, but missing for ATSC channels.
        [DataMember(Name = "channel", IsRequired = false)]
        public string channel {
            get { return (guideChannelMajor > 0) ? ChannelPartsToString(guideChannelMajor, guideChannelMinor) : rawChannelStr_; }
            set {
                rawChannelStr_ = value;
                try {
                    ParseChannelParts(value, out guideChannelMajor_, out guideChannelMinor_);
                } catch
                {
                    Console.WriteLine("Could not parse '{0}' as a channel number", value);
                }
            }
        }

        // channel number as read from JSON.
        string rawChannelStr_;

        public ChannelNumber GetChannelNumber() { return new ChannelNumber(guideChannelMajor, guideChannelMinor); }
        public int guideChannelMajor { get {
                return (logicalChannelMajor_ > 0)
                    ? logicalChannelMajor_
                    : (guideChannelMajor_ > 0)
                        ? guideChannelMajor_
                        : (atscMajor > 0) ? atscMajor : uhfVhf;
            }
        }
        [IgnoreDataMember]
        public int guideChannelMinor
        {
            get
            {
                return (logicalChannelMajor_ > 0)
                    ? logicalChannelMinor_
                    : (guideChannelMajor_ > 0)
                        ? guideChannelMinor_
                        : (atscMajor > 0) ? atscMinor : uhfVhf;
            }
        }
        [IgnoreDataMember]
        private int guideChannelMajor_;
        [IgnoreDataMember]
        private int guideChannelMinor_;
        #endregion

        #region OTA params
        // ATSC/NTSC params
        [DataMember(Name = "uhfVhf")]
        public int uhfVhf { get; set; }
        [DataMember(Name = "atscMajor")]
        public int atscMajor { get; set; }
        [IgnoreDataMember]
        public int atscTuningMajor { get { return (uhfVhf > 0) ? uhfVhf : atscMajor; } }
        [DataMember(Name = "atscMinor")]
        public int atscMinor { get; set; }
        /*[DataMember(Name = "channelMajor")]
        public int channelMajor { get; set; }
        [DataMember(Name = "channelMinor")]
        public int channelMinor { get; set; } */
        #endregion

        #region QAM
        [DataMember(Name = "providerCallsign")]
        public string providerCallsign { get; set; }
        [DataMember(Name = "matchType")]
        public string matchType { get; set; }
        [DataMember(Name ="providerChannel")]
        public string providerChannel {
            get { return ChannelPartsToString(providerChannelMajor_, providerChannelMinor_); }
            set { ParseChannelParts(value, out providerChannelMajor_, out providerChannelMinor_); }
        }

        [IgnoreDataMember]
        public int QAMMajor { get { return (providerChannelMinor_ > 0) ? providerChannelMajor_ : atscMajor; } }
        public int QAMMinor { get { return (providerChannelMinor_ > 0) ? providerChannelMinor_ : atscMinor; } }

        [IgnoreDataMember]
        private int providerChannelMajor_;
        [IgnoreDataMember]
        private int providerChannelMinor_;
        [DataMember(Name ="logicalChannelNumber")]
        public string logicalChannelNumber {
            get { return ChannelPartsToString(logicalChannelMajor_, logicalChannelMinor_); }
            set { ParseChannelParts(value, out logicalChannelMajor_, out logicalChannelMinor_); }
        }
        [IgnoreDataMember]
        private int logicalChannelMajor_;
        [IgnoreDataMember]
        private int logicalChannelMinor_;
        #endregion

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

        private string PadTo5Digits(int foo)
        {
            return foo.ToString("00000");
        }
        [IgnoreDataMember]
        public string sort_key  // key to sort by number then stationID.
        {
            get
            {
                return string.Format("{0}.{0}|{2}", PadTo5Digits(guideChannelMajor), PadTo5Digits(guideChannelMinor), stationID);
            }
        }

        #region helper functions
        private static char[] channelPartDelimiters_ = new char[] { '.', '_', '-' };
        void ParseChannelParts(string channelString, out int major, out int minor)
        {
            if (channelString == null)
            {
                major = minor = 0;
                return;
            }
            string[] channelParts = channelString.Split(channelPartDelimiters_);
            if (channelParts.Length > 2) throw new Exception("Channel parsing failure!");
            major = int.Parse(channelParts[0]);
            minor = (channelParts.Length > 1) ? int.Parse(channelParts[1]) : 0;
        }
        string ChannelPartsToString(int major, int minor)
        {
            return (major > 0)
                ? (minor > 0)
                    ? string.Format("{0}-{1}", major, minor)
                    : major.ToString()
                : null;
        }
        #endregion
    }

}
