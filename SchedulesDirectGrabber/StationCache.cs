using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace SchedulesDirectGrabber
{
    using MXFChannel = ConfigManager.MXFLineup.MXFChannel;

    public class StationCache
    {
        public static StationCache instance { get { return instance_; } }
        private static StationCache instance_ = new StationCache();

        public void Clear()
        {
            stationInfoByStationId_.Clear();
            stationIdsByChannelNumber_.Clear();
            serviceIdByStationId_.Clear();
            affiliates_.Clear();
        }

        private Dictionary<string, StationInfo> stationInfoByStationId_ = new Dictionary<string, StationInfo>();
        private Dictionary<ChannelNumberConfig, HashSet<string>> stationIdsByChannelNumber_ =
            new Dictionary<ChannelNumberConfig, HashSet<string>>();
        private Dictionary<string, int> serviceIdByStationId_ = new Dictionary<string, int>();
        public HashSet<string> affiliates_ = new HashSet<string>();
        private ConfigManager.SDGrabberConfig config { get { return ConfigManager.config; } }

        internal string GetServiceIdByStationId(string stationId)
        {
            if (!stationInfoByStationId_.ContainsKey(stationId))
                throw new Exception("stationId not loaded:" + stationId);
            if (!serviceIdByStationId_.ContainsKey(stationId))
            {
                serviceIdByStationId_[stationId] = serviceIdByStationId_.Count + 1;
            }
            return "s" + serviceIdByStationId_[stationId].ToString();
        }

        internal bool IsStationIdLoaded(string stationId)
        {
            return stationInfoByStationId_.ContainsKey(stationId);
        }

        const string kAffiliateNamePrefix = "!Affiliate!";

        private string GetIdForAffiliate(string affiliate)
        {
            return kAffiliateNamePrefix + affiliate;
        }
        private string AddAffiliateAndGetId(string affiliate)
        {
            affiliates_.Add(affiliate);
            return GetIdForAffiliate(affiliate);
        }

        internal void PopulateFromConfig()
        {
            Console.WriteLine("Donwloading channel lineups");
            Clear();
            foreach(var lineup in config.lineups)
            {
                foreach(var station in lineup.GetDownloadedStations())
                {
                    string stationId = station.stationID;
                    if (!stationInfoByStationId_.ContainsKey(stationId))
                        stationInfoByStationId_[stationId] = new StationInfo(station);
                    var stationInfo = stationInfoByStationId_[stationId];
                    var newChannelNumbers = lineup.EffectiveStationChannelNumbers(stationId);
                    stationInfo.AddChannelNumbers(newChannelNumbers);
                    foreach(var channelNumber in newChannelNumbers)
                    {
                        if (!stationIdsByChannelNumber_.ContainsKey(channelNumber))
                            stationIdsByChannelNumber_[channelNumber] = new HashSet<string>();
                        stationIdsByChannelNumber_[channelNumber].Add(stationId);
                    }
                    stationInfo.AddTuningConfigs(lineup.EffectivePhysicalChannelNumbers(stationId));
                }
            }
        }

        internal IEnumerable<MXFService> GetMXFServices()
        {
            foreach(var stationInfo in stationInfoByStationId_.Values)
            {
                yield return stationInfo.mxfService;
            }
        }

        public IEnumerable<MXFChannel> GetMXFChannels()
        {
            foreach(var stationIdAndInfo in stationInfoByStationId_)
            {
                string stationId = stationIdAndInfo.Key;
                StationInfo stationInfo = stationIdAndInfo.Value;
                var channelNumbers = stationInfo.guideChannelNumbers;
                foreach (var channelNumber in channelNumbers)
                {
                    yield return new MXFChannel(channelNumber, stationInfo.sdStation);
                }
                if (channelNumbers.Count == 0)
                {
                    yield return new MXFChannel(null, stationInfo.sdStation);
                }
            }
        }

        internal IEnumerable<MXFAffiliate> GetMXFAffiliates()
        {
            foreach(var affiliate in affiliates_)
            {
                yield return new MXFAffiliate(affiliate, GetIdForAffiliate(affiliate));
            }
        }

        internal IEnumerable<string> GetStationIds()
        {
            return stationInfoByStationId_.Keys;
        }

        class StationInfo
        {
            internal StationInfo(SDStation station) { sdStation_ = station; }

            internal void AddChannelNumbers(IEnumerable<ChannelNumberConfig> channelNumbers)
            {
                foreach(var channelNumber in channelNumbers)
                {
                    guideChannelNumbers_.Add(channelNumber);
                }
            }
            internal void AddTuningConfigs(IEnumerable<TuningConfig> tuningConfigs)
            {
                tuningConfigs_.AddRange(tuningConfigs);
            }

            internal ISet<ChannelNumberConfig> guideChannelNumbers { get { return guideChannelNumbers_; } }
            internal IEnumerable<TuningConfig> tuningConfigs { get{ return tuningConfigs_; } }

            public SDStation sdStation { get { return sdStation_; } }
            public MXFService mxfService { get { return new MXFService(sdStation); } }

            private SDStation sdStation_;
            private HashSet<ChannelNumberConfig> guideChannelNumbers_ = new HashSet<ChannelNumberConfig>();
            private List<TuningConfig> tuningConfigs_ = new List<TuningConfig>();
        }
        private StationCache() { }

        [DataContract]
        public class SDStation
        {
            [DataMember(Name = "stationID", IsRequired = true)]
            public string stationID { get; set; }
            [DataMember(Name = "name")]
            public string name { get; set; }
            [DataMember(Name = "callsign")]
            public string callsign { get; set; }
            [DataMember(Name = "affiliate")]
            public string affiliate { get; set; }
            [DataMember(Name = "broadcastLanguage")]
            public List<string> broadcastLanguage { get; set; }
            [DataMember(Name = "descriptionLanguage")]
            public List<string> descriptionLanguage { get; set; }
            [DataMember(Name = "broadcaster")]
            public Broadcaster broadcaster { get; set; }
            [DataMember(Name = "logo")]
            public Logo logo { get; set; }
            [DataMember(Name = "isCommercialFree")]
            public bool isCommercialFree { get; set; }
            [DataMember(Name = "metadata")]
            public Metadata metadata { get; set; }

            [DataContract]
            public class Broadcaster
            {
                [DataMember(Name = "city")]
                public string city { get; set; }
                [DataMember(Name = "state")]
                public string state { get; set; }
                [DataMember(Name = "postalcode")]
                public string postalcode { get; set; }
                [DataMember(Name = "country")]
                public string country { get; set; }
            }

            [DataContract]
            public class Logo
            {
                [DataMember(Name = "URL")]
                public string URL { get; set; }
                [DataMember(Name = "height")]
                public int height { get; set; }
                [DataMember(Name = "width")]
                public int width { get; set; }
                [DataMember(Name = "md5")]
                public string md5 { get; set; }
            }

            [DataContract]
            public class Metadata
            {
                [DataMember(Name = "lineup")]
                public string lineup { get; set; }
                [DataMember(Name = "modified")]
                public DateTime modified { get; set; }
                [DataMember(Name = "transport")]
                public string transport { get; set; }
                [DataMember(Name = "modulation")]
                public string modulation { get; set; }
            }
        }

        public class MXFService
        {
            public MXFService() { }
            internal MXFService(SDStation sdStation)
            {
                string stationId = sdStation.stationID;
                id = StationCache.instance.GetServiceIdByStationId(stationId);
                uid = "!Service!GSD" + stationId;
                name = sdStation.name;
                callSign = sdStation.callsign;
                if (!string.IsNullOrEmpty(sdStation.affiliate))
                {
                    affiliate = StationCache.instance.AddAffiliateAndGetId(sdStation.affiliate);
                }
                if (sdStation.logo != null)
                {
                    logoImage = ImageCache.instance.FindOrCreateMXFImageId(sdStation.logo.URL);
                }
            }

            [XmlAttribute("id")]
            public string id { get; set; }

            [XmlAttribute("uid")]
            public string uid { get; set; }

            [XmlAttribute("name")]
            public string name { get; set; }

            [XmlAttribute("callSign")]
            public string callSign { get; set; }

            [XmlAttribute("affiliate")]
            public string affiliate { get; set; }

            [XmlAttribute("logoImage")]
            public string logoImage { get; set; }
        }


        public class MXFAffiliate
        {
            public MXFAffiliate() { }

            public MXFAffiliate(string name, string uid)
            {
                this.name = name;
                this.uid = uid;
            }

            [XmlAttribute("name")]
            public string name { get; set; }

            [XmlAttribute("uid")]
            public string uid { get; set; }

            [XmlAttribute("logoImage")]
            public string logoImage { get; set; }
        }

    }
}
