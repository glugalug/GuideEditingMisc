using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SchedulesDirectGrabber
{
    using DBProgram = ProgramCache.DBProgram;

    public class SeriesInfoCache
    {
        public static SeriesInfoCache instance { get { return instance_; } }

        public DBSeriesInfo GetSeriesInfoBySeriesID(string seriesID)
        {
            if (!seriesInfoById_.ContainsKey(seriesID)) throw new Exception("Series ID not loaded.");
            return seriesInfoById_[seriesID];
        }

        public IEnumerable<DBSeriesInfo> FetchSeriesInfosForPrograms(IEnumerable<DBProgram> programs)
        {
            // First index the programs by series ID, with a sample program for each series.
            Dictionary<string, DBProgram> sampleProgramsBySeriesID = new Dictionary<string, DBProgram>();
            foreach(DBProgram program in programs)
            {
                // Only fetch seriesInfo for series.
                if (!program.IsSeries()) continue;
                // Skip any series we already have cached.
                string seriesId = SeriesIDByProgramID(program.programID);
                if (seriesInfoById_.ContainsKey(seriesId)) continue;
                sampleProgramsBySeriesID[seriesId] = program;
            }

            // Fetch any series already in the DB.
            var dbSeriesInfos = DBManager.instance.GetSeriesInfoByIDs(new HashSet<string>(sampleProgramsBySeriesID.Keys));
            foreach(var dbSeriesInfo in dbSeriesInfos)
            {
                seriesInfoById_[dbSeriesInfo.Key] = dbSeriesInfo.Value;
                sampleProgramsBySeriesID.Remove(dbSeriesInfo.Key);
            }

            // Create seriesInfos from the sample programs
            foreach(var kv in sampleProgramsBySeriesID)
                seriesInfoById_[kv.Key] = new DBSeriesInfo(kv.Value);

            // Download additional generic series info from SchedulesDirect and add it to our cache for these series.
            Dictionary<string, SDGenericProgramDescription> downloadedProgramDescriptions =
                DownloadSeriesByIDs(sampleProgramsBySeriesID.Keys);

            // Loop through and add any successfully downloaded.
            foreach(var kv in downloadedProgramDescriptions)
            {
                SDGenericProgramDescription genericDescription = kv.Value;
                seriesInfoById_[SeriesIDByProgramID(kv.Key)].AddGenericDescription(genericDescription);
            }

            List<DBSeriesInfo> updatedSeries = new List<DBSeriesInfo>();
            foreach(string key in sampleProgramsBySeriesID.Keys)
            {
                updatedSeries.Add(seriesInfoById_[key]);
            }
            DBManager.instance.SaveSeriesInfos(updatedSeries);
            return seriesInfoById_.Values;
        }

        private static Dictionary<string, SDGenericProgramDescription> DownloadSeriesByIDs(IEnumerable<string> seriesIds)
        {
            const int kBatchSize = 500;
            Console.WriteLine("Downloading series infos from SchedulesDirect");
            Dictionary<string, SDGenericProgramDescription> downloadedGenericDescriptions =
                new Dictionary<string, SDGenericProgramDescription>();
            List<string> currentBatch = new List<string>();
            Func<int> DownloadBatch = new Func<int>(() => {
                var batchRepsonse = JSONClient.GetJSONResponse<Dictionary<string, SDGenericProgramDescription>>(
                    UrlBuilder.BuildWithAPIPrefix("/metadata/description/"),
                    currentBatch, SDTokenManager.token_manager.token);
                foreach(var kv in batchRepsonse)
                {
                    var genericDescription = kv.Value;
                    if (genericDescription.code > 0)
                    {
                        Console.WriteLine("Failed to download generic description for program ID: {0}\ncode: {1}\nmessage: {2}",
                            kv.Key, genericDescription.code, genericDescription.message);
                        continue;
                    }
                    downloadedGenericDescriptions[kv.Key] = genericDescription;
                }
                currentBatch.Clear();
                return 0;
            });
            foreach(string seriesId in seriesIds)
            {
                currentBatch.Add(seriesId);
                if (currentBatch.Count >= kBatchSize) DownloadBatch();
            }
            if (currentBatch.Count > 0) DownloadBatch();
            return downloadedGenericDescriptions;
        }

        public static string SeriesIDByProgramID(string programID)
        {
            return string.Format("EP{0}0001", programID.Substring(2,8));
        }

        private Dictionary<string, DBSeriesInfo> seriesInfoById_ = new Dictionary<string, DBSeriesInfo>();

        private static SeriesInfoCache instance_ = new SeriesInfoCache();

        private Dictionary<string, int> mxfSeriesIdIndex_ = new Dictionary<string, int>();

        string GetMxfSeriesIdBySDSeriesID(string sdSeriesId)
        {
            if (!mxfSeriesIdIndex_.ContainsKey(sdSeriesId))
            {
                mxfSeriesIdIndex_[sdSeriesId] = mxfSeriesIdIndex_.Count + 1;
            }
            return "si" + mxfSeriesIdIndex_[sdSeriesId].ToString();
        }

        [DataContract]
        public class DBSeriesInfo
        {
            public DBSeriesInfo() { }
            public DBSeriesInfo(DBProgram program)
            {
                id = SeriesIDByProgramID(program.programID);
                title = program.longTitle ?? program.title;
                shortTitle = program.shortTitle;
                hasGuideImage = program.hasImages;
                description = program.description;
                shortDescription = program.shortDescription;
            }

            public void AddGenericDescription(SDGenericProgramDescription genericDescription)
            {
                description = genericDescription.description1000;
                shortDescription = genericDescription.description100;
            }

            [DataMember(Name = "id", IsRequired = true)]
            public string id { get; set; }

            [DataMember(Name = "title", IsRequired = true)]
            public string title { get; set; }

            [DataMember(Name = "shortTitle")]
            public string shortTitle { get; set; }

            [DataMember(Name = "description")]
            public string description { get; set; }

            [DataMember(Name = "shortDescription")]
            public string shortDescription { get; set; }

            [DataMember(Name ="hasGuideImage")]
            public bool hasGuideImage { get; set; }
        }

        public IEnumerable<MXFSeriesInfo> GetMXFSeriesEnumeration()
        {
            foreach(var series in seriesInfoById_.Values)
            {
                yield return new MXFSeriesInfo(series);
            }
        }

        [DataContract]
        public class SDGenericProgramDescription
        {
            [DataMember(Name ="code", IsRequired =true)]
            public int code { get; set; }

            [DataMember(Name ="message")]
            public string message { get; set; }

            [DataMember(Name ="description100")]
            public string description100 { get; set; }

            [DataMember(Name ="description1000")]
            public string description1000 { get; set; }

            public bool IsOK() { return code == 0; }
        }

        [DataContract(Name ="SeriesInfo")]
        public class MXFSeriesInfo
        {
            const string kSeriesIdMXFPrefix = "!Series!GSD";
            public MXFSeriesInfo() { throw new NotImplementedException(); }
            public MXFSeriesInfo(DBSeriesInfo dbSeriesInfo)
            {
                dbSeriesInfo_ = dbSeriesInfo;
            }

            // Should be of the form EP12345678, 10 characters.
            [DataMember(IsRequired = true), XmlAttribute("id")]
            public string mxfId {
                get { return SeriesInfoCache.instance.GetMxfSeriesIdBySDSeriesID(dbSeriesInfo_.id); }
                set { throw new NotImplementedException(); }
            }

            [DataMember(IsRequired =true), XmlAttribute("uid")]
            public string uid { get { return kSeriesIdMXFPrefix + dbSeriesInfo_.id; } set { throw new NotImplementedException(); } }

            [DataMember(EmitDefaultValue = false), XmlAttribute( "title")]
            public string title
            {
                get
                {
                    return Misc.LimitString(string.IsNullOrEmpty(dbSeriesInfo_.title) 
                        ? dbSeriesInfo_.shortTitle : dbSeriesInfo_.title, 512);
                }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue = false), XmlAttribute("shortTitle")]
            public string shortTitle
            {
                get
                {
                    return Misc.LimitString(string.IsNullOrEmpty(dbSeriesInfo_.shortTitle)
                        ? dbSeriesInfo_.title : dbSeriesInfo_.shortTitle, 512);
                }
                set { throw new NotImplementedException(); }
            }


            [DataMember(EmitDefaultValue = false), XmlAttribute("description")]
            public string description
            {
                get
                {
                    return Misc.LimitString(string.IsNullOrEmpty(dbSeriesInfo_.description)
                        ? dbSeriesInfo_.shortDescription : dbSeriesInfo_.description, 512);
                }
                set { throw new NotImplementedException(); }
            }
            
            [DataMember(EmitDefaultValue = false), XmlAttribute("shortDescription")]
            public string shortDescription
            {
                get
                {
                    return Misc.LimitString(string.IsNullOrEmpty(dbSeriesInfo_.shortDescription)
                        ? dbSeriesInfo_.description : dbSeriesInfo_.shortDescription, 512);
                }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue = false), XmlAttribute("startAirDate")]
            public DateTime startAirDate { get; set; }

            [DataMember(EmitDefaultValue = false), XmlAttribute("endAirDate")]
            public DateTime endAirDate { get; set; }

            [DataMember(EmitDefaultValue = false), XmlAttribute("guideImage")]
            public string guideImage
            {
                get
                {
                    if (!dbSeriesInfo_.hasGuideImage) return null;
                    return "IM" + ImageCache.GetSDImageIDByProgramID(mxfId);
                }
                set { throw new NotImplementedException(); }
            }
            private DBSeriesInfo dbSeriesInfo_;
        }
    }
}
