using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SchedulesDirectGrabber
{
    [DataContract, XmlRoot("MXF")]
    public class MXF
    {
        public class EnumerableSerializationWrapper<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> wrapped_;

            public EnumerableSerializationWrapper() { throw new NotImplementedException(); }

            public EnumerableSerializationWrapper(IEnumerable<T> wrapped)
            {
                wrapped_ = wrapped;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return wrapped_.GetEnumerator();
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                return wrapped_.GetEnumerator();
            }

            public void Add(T foo) { throw new NotImplementedException(); }
        }

        private const string providerID = "provider1";
        // Skipping silly Assembly names for now, can't see how they can really be needed.
        [DataMember, XmlArray("Providers"), XmlArrayItem(ElementName ="Provider")]
        public Provider[] provider = new Provider[1] { new Provider() };

        [DataMember, XmlElement("With")]
        public ProviderData providerData = new ProviderData();

        [DataContract]
        public class Provider
        {
            [DataMember, XmlAttribute("id")]
            public string id = providerID;

            [DataMember, XmlAttribute("name")]
            public string name = "GlugglugsSchedulesDirectGrabber";

            [DataMember, XmlAttribute("displayName")]
            public string displayName = "Glugglug's SchedulesDirect Grabber";

            [DataMember, XmlAttribute("copyright")]
            public string copyright = "";
        }

        [DataContract]
        public class ProviderData
        {
            public ProviderData()
            {
                // Loop through all the enumerations top level first to get everything generated before serialization.
                foreach (var lineup in lineups) { }
                foreach (var service in services) { }
                foreach (var affiliate in affiliates) { }
                foreach (var program in programs) { }
                foreach (var keyword in keywords) { }
                foreach (var keywordGroup in keywordGroups) { }
                foreach (var person in people) { }
                foreach (var seriesInfo in seriesInfos) { }
                foreach (var guideImage in guideImages) { }
                // foreach (var season in seasons) { } we don't actually have seasons
            }

            [DataMember, XmlAttribute("provider")]
            public string id = providerID;

            [DataMember, XmlArray("Keywords"), XmlArrayItem("Keyword")]
            public EnumerableSerializationWrapper<KeywordCache.MXFKeyword> keywords {
                get { return new EnumerableSerializationWrapper<KeywordCache.MXFKeyword>(
                        KeywordCache.instance.keywords.keywords); }
                set { throw new NotImplementedException(); }
            }

            [DataMember, XmlArray("KeywordGroups"), XmlArrayItem("KeywordGroup")]
            public EnumerableSerializationWrapper<KeywordCache.MXFKeywordGroup> keywordGroups {
                get { return new EnumerableSerializationWrapper<KeywordCache.MXFKeywordGroup>(
                        KeywordCache.instance.keywordGroups.keywordGroups); }
                set { throw new NotImplementedException(); }
            }

            [DataMember, XmlArray("People"), XmlArrayItem("Person")]
            public EnumerableSerializationWrapper<PersonCache.MXFPerson> people =
                new EnumerableSerializationWrapper<PersonCache.MXFPerson>(PersonCache.instance.GetPersonEnumeration());

            [XmlArray("GuideImages"), XmlArrayItem("GuideImage")]
            public EnumerableSerializationWrapper<ImageCache.MXFGuideImage> guideImages =
                new EnumerableSerializationWrapper<ImageCache.MXFGuideImage>(ImageCache.instance.GetMXFGuideImages());

            [DataMember, XmlArray("SeriesInfos"), XmlArrayItem("SeriesInfo")]
            public EnumerableSerializationWrapper<SeriesInfoCache.MXFSeriesInfo> seriesInfos =
                new EnumerableSerializationWrapper<SeriesInfoCache.MXFSeriesInfo>(
                    SeriesInfoCache.instance.GetMXFSeriesEnumeration());

            [DataMember, XmlElement(ElementName = "Seasons")]
            public Seasons seasons = new Seasons();

            [DataMember, XmlArray("Programs"), XmlArrayItem("Program")]
            public EnumerableSerializationWrapper<ProgramCache.MXFProgram> programs =
                new EnumerableSerializationWrapper<ProgramCache.MXFProgram>(
                    ProgramCache.instance.GetMXFPrograms());

            [XmlArray("Affiliates"), XmlArrayItem("Affiliate")]
            public EnumerableSerializationWrapper<StationCache.MXFAffiliate> affiliates =
                new EnumerableSerializationWrapper<StationCache.MXFAffiliate>(StationCache.instance.GetMXFAffiliates());

            [XmlArray("Services"), XmlArrayItem("Service")]
            public EnumerableSerializationWrapper<StationCache.MXFService> services =
                new EnumerableSerializationWrapper<StationCache.MXFService>(StationCache.instance.GetMXFServices());

            [XmlAnyElement]
            public ScheduleCache.ScheduleSerializer scheduleSerializer = new ScheduleCache.ScheduleSerializer();

            [XmlArray("Lineups"), XmlArrayItem("Lineup")]
            public EnumerableSerializationWrapper<ConfigManager.MXFLineup> lineups =
                new EnumerableSerializationWrapper<ConfigManager.MXFLineup>(ConfigManager.config.GetMXFLineups());

            [DataContract]
            public class Seasons
            {  // We don't actually geneate seasons data, so this is just empty.
            }

        }
    }
    
}