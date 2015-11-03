using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.IO;
using Microsoft.MediaCenter.Store.MXF;
using ChannelEditingLib;
namespace SchedulesDirectGrabber
{
    // xmlns:sql="urn:schemas-microsoft-com:XML-sql"
    [DataContract, XmlRoot("MXF")]
    public class MXFData
    {
        internal static void BuildMxf()
        {
            StationCache.instance.PopulateFromConfig();
            IEnumerable<ProgramCache.DBProgram> programs =
                ScheduleCache.instance.GetAllProgramsForStations(new HashSet<string>(StationCache.instance.GetStationIds()));
            SeriesInfoCache.instance.FetchSeriesInfosForPrograms(programs);
            ImageCache.instance.GetAllImagesForPrograms(programs);
            using (XmlTextWriter writer = new XmlTextWriter("mxf.xml", System.Text.Encoding.UTF8))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(MXFData));
                writer.Formatting = Formatting.Indented;
                xmlSerializer.Serialize(writer, new MXFData());
                writer.Close();
            }
        }

        internal static void WriteMxfToStream(StreamWriter writer)
        {
            StationCache.instance.PopulateFromConfig();
            IEnumerable<ProgramCache.DBProgram> programs =
                ScheduleCache.instance.GetAllProgramsForStations(new HashSet<string>(StationCache.instance.GetStationIds()));
            SeriesInfoCache.instance.FetchSeriesInfosForPrograms(programs);
            ImageCache.instance.GetAllImagesForPrograms(programs);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(MXFData));
            xmlSerializer.Serialize(writer, new MXFData());
        }

        internal static void ImportMxf()
        {
            BuildMxf();
            MxfImporter.Import(new StreamReader("mxf.xml").BaseStream, ChannelEditing.object_store, MxfImportProgressCallback);
            //ChannelEditing.object_store.Import("mxf.xml", MxfImportProgressCallback);
        }

        private static bool MxfImportProgressCallback(int amountCompleted)
        {
            Console.WriteLine("MXF Import {0}% completed", 0.1 * amountCompleted);
            return true;
        }

        // Turns out the static assembly section really is required!  LoadXml quickly exits without doing anything if it is missing!
        public class AssemblySerializer : IXmlSerializable
        {
            XmlSchema IXmlSerializable.GetSchema()
            {
                return null;
            }

            void IXmlSerializable.ReadXml(XmlReader reader)
            {
                throw new NotImplementedException();
            }

            void IXmlSerializable.WriteXml(XmlWriter writer)
            {
                writer.WriteRaw(@"  <Assembly name=""mcepg"" version=""6.0.6000.0"" cultureInfo="""" publicKey=""0024000004800000940000000602000000240000525341310004000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC3652BF5C308055DA9"">
    <NameSpace name = ""Microsoft.MediaCenter.Guide"" >
      <Type name = ""Lineup"" />
      <Type name = ""Channel"" parentFieldName = ""lineup"" />
      <Type name = ""Service"" />
      <Type name = ""ScheduleEntry"" groupName = ""ScheduleEntries"" />
      <Type name = ""Program"" />
      <Type name = ""Keyword"" />
      <Type name = ""KeywordGroup"" />
      <Type name = ""Person"" groupName = ""People"" />
      <Type name = ""ActorRole"" parentFieldName = ""program"" />
      <Type name = ""DirectorRole"" parentFieldName = ""program"" />
      <Type name = ""WriterRole"" parentFieldName = ""program"" />
      <Type name = ""HostRole"" parentFieldName = ""program"" />
      <Type name = ""GuestActorRole"" parentFieldName = ""program"" />
      <Type name = ""ProducerRole"" parentFieldName = ""program"" />
      <Type name = ""GuideImage"" />
      <Type name = ""Affiliate"" />
      <Type name = ""SeriesInfo"" />
      <Type name = ""Season"" />
    </NameSpace >
  </Assembly >
  <Assembly name = ""mcstore"" version = ""6.0.6000.0"" cultureInfo = """" publicKey = ""0024000004800000940000000602000000240000525341310004000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC3652BF5C308055DA9"" >
    <NameSpace name = ""Microsoft.MediaCenter.Store"" >
      <Type name = ""Provider"" />
      <Type name = ""UId"" parentFieldName = ""target"" />
    </NameSpace >
  </Assembly > ");
            }
        }

        [XmlAnyElement]
        public AssemblySerializer sillyAssemblySection = new AssemblySerializer();

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
            public string copyright = "copyright placeholder";
        }

        [DataContract]
        public class ProviderData
        {
            public ProviderData()
            {
                // Loop through all the enumerations top level first to get everything generated before serialization.
               // foreach (var lineup in lineups) { }
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

            [XmlArray("GuideImages"), XmlArrayItem("GuideImage")]
            public EnumerableSerializationWrapper<ImageCache.MXFGuideImage> guideImages =
                new EnumerableSerializationWrapper<ImageCache.MXFGuideImage>(ImageCache.instance.GetMXFGuideImages());

            [DataMember, XmlArray("People"), XmlArrayItem("Person")]
            public EnumerableSerializationWrapper<PersonCache.MXFPerson> people =
                new EnumerableSerializationWrapper<PersonCache.MXFPerson>(PersonCache.instance.GetPersonEnumeration());

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