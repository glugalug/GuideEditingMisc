using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;

namespace SchedulesDirectGrabber
{
    using System.Xml;
    using System.Xml.Schema;
    using MpaaRating = Microsoft.MediaCenter.Guide.MpaaRating;

    // TODO: 
    // 1. Make the cache hold dbPrograms not SDPrograms
    // 2. Write both DBProgram and SDProgram (for debugging) to DB.
    // 3. Implement IEnumerator<MXFProgram>
    public class ProgramCache
    {
        public static ProgramCache instance { get { return instance_; } }
        private static ProgramCache instance_ = new ProgramCache();

        private ProgramCache() { }

        private Dictionary<string, DBProgram> programCache_ = new Dictionary<string, DBProgram>();

        private Dictionary<string, int> programIdIndex_ = new Dictionary<string, int>();

        public string GetProgramIndexByKey(string key)
        {
            if (!programIdIndex_.ContainsKey(key))
            {
                programIdIndex_[key] = programIdIndex_.Count + 1;
            }
            return programIdIndex_[key].ToString();
        }

        internal DBProgram GetProgrambyId(string programId)
        {
            return programCache_[programId];
        }
      
        public IEnumerable<MXFProgram> GetMXFPrograms()
        {
            foreach (var program in programCache_.Values)
                yield return new MXFProgram(program);
        }

        public IEnumerable<DBProgram> GetProgramsByIDWithMD5(IDictionary<string, string> md5ByProgramId)
        {
            // Fetch what we haven't already cached from DB.
            HashSet<string> idsNeeded = new HashSet<string>();
            foreach(string id in md5ByProgramId.Keys)
                if (!programCache_.ContainsKey(id)) idsNeeded.Add(id);
            var programsFromDB = DBManager.instance.GetProgramsByIds(idsNeeded);
            foreach (var kv in programsFromDB) programCache_[kv.Key] = kv.Value;

            Console.WriteLine("Determining what programs to download based on md5s in the schedule");
            // Fetch any programs that are either still not cached or have wrong md5.
            idsNeeded.Clear();
            foreach(var kv in md5ByProgramId)
            {
                string programID = kv.Key;
                if (!programCache_.ContainsKey(programID) || programCache_[programID].md5 != kv.Value)
                    idsNeeded.Add(programID);
            }
            var downloaded = DownloadPrograms(idsNeeded);
            foreach(var program in downloaded)
                programCache_[program.programID] = program;

            return programCache_.Values;
        }

        private const int kMaxProgramIdsPerRequest = 5000;
        private List<DBProgram> DownloadPrograms(IEnumerable<string> programIDs)
        {
            const int kBatchSize = 5000;
            List<string> currentBatch = new List<string>();
            List<DBProgram> fetchedPrograms = new List<DBProgram>();

            Func<int> FetchCurrentBatch = new Func<int>(() =>
            {
                Console.WriteLine("Downloading program info from SchedulesDirect");
                List<object> response = JSONClient.GetJSONResponse<List<object>>(
                    UrlBuilder.BuildWithAPIPrefix("/programs"), currentBatch, SDTokenManager.token_manager.token);
                
                List<DBProgram> programsThisBatch = new List<DBProgram>();
                // Only include programs fetched successfully, so any errors do not replace good data.
                List<SDProgram> successfulResponses = new List<SDProgram>();
                foreach(object genericJson in response)
                {
                    SDProgram sdProgram = null;
                    try
                    {
                        sdProgram = JSONClient.Deserialize<SDProgram>(genericJson.ToString());
                    } catch (Exception exc)
                    {
                        Console.WriteLine("Failed to deserialize program JSON: {0}", genericJson);
                        Misc.OutputException(exc);
                        continue;
                    }
                    if (sdProgram.code > 0)
                    {
                        Console.WriteLine("Failed to download updated program info for programID {0}, response {1}, message: {2}",
                            sdProgram.programID, sdProgram.response, sdProgram.message);
                        continue;
                    }
                    successfulResponses.Add(sdProgram);
                    var dbProgram = new DBProgram(sdProgram);
                    fetchedPrograms.Add(dbProgram);
                    programsThisBatch.Add(dbProgram);
                }
                DBManager.instance.SaveRawSDProgramResponses(successfulResponses);
                DBManager.instance.SaveProgramData(programsThisBatch);
                currentBatch.Clear();
                return 0;
            });

            foreach(string id in programIDs)
            {
                currentBatch.Add(id);
                if (currentBatch.Count >= kBatchSize) FetchCurrentBatch();
            }
            if (currentBatch.Count > 0) FetchCurrentBatch();
            return fetchedPrograms;
        }

        public enum EntityType
        {
            Show,
            Episode,
            Sports,
            Movie,
            Invalid,
        }

        public enum ShowType
        {
            Unknown,
            Series,
            PaidProgramming,
            Serial,
            Special,
            Miniseries,
            FeatureFilm,
            SportsNonEvent,
            SportsEvent,
            TVMovie,
            ShortFilm
        }

        [DataContract]
        public class DBProgram
        {
            // Default constructor needed for deserialization from DB
            public DBProgram() { }
            public DBProgram(SDProgram sdProgram)
            {
                programID = sdProgram.programID;
                // First title from SD used as primary, longest and shortest kept if different to use
                // in SeriesInfo.
                title = sdProgram.GetFirstTitle();
                string sTitle = sdProgram.GetShortTitle();
                if (sTitle != null && sTitle != title)
                {
                    shortTitle = sTitle;
                }
                string lTitle = sdProgram.GetLongTitle();
                if (lTitle != null && lTitle != title)
                {
                    longTitle = lTitle;
                }

                episodeTitle = sdProgram.episodeTitle150;

                // There may be many SchedulesDirect descriptions, but WMC supports 1 short & 1 long, so we
                // use the first 100 char description and first 1000 char description.
                var sdDescriptions = sdProgram.descriptions;
                if (sdDescriptions != null)
                {
                    if (sdDescriptions.description1000s?.Count > 0)
                        description = sdDescriptions.description1000s[0].description;
                    if (sdDescriptions.description100s?.Count > 0)
                        shortDescription = sdDescriptions.description100s[0].description;
                }

                language = sdProgram.GetLanguage();

                originalAirDate = sdProgram.originalAirDate;

                seasonNumber = sdProgram.GetSeasonNumber();
                episodeNumber = sdProgram.GetEpisodeNumber();

                keyWords = sdProgram.keyWords;

                showType = sdProgram.showType;
                entityType = sdProgram.entityType;

                if (sdProgram?.movie?.qualityRating?.Length > 0)
                {
                    var qualityRating = sdProgram.movie.qualityRating[0];
                    double rating = double.Parse(qualityRating.rating);
                    double minRating = double.Parse(qualityRating.minRating);
                    double maxRating = double.Parse(qualityRating.maxRating);
                    double unitRating = (1.0 * (rating - minRating)) / (maxRating - minRating);
                    halfStars = 1 + (int)(unitRating * 7 + 0.5);
                }

                mpaaRating = sdProgram.GetMpaaRating();

                if (sdProgram.genres?.Length > 0)
                    genres = new HashSet<string>(sdProgram.genres);

                if (sdProgram.contentAdvisory?.Length > 0)
                    contentAdvisory = new HashSet<string>(sdProgram.contentAdvisory);

                hasImages = sdProgram.hasImageArtwork;

                List<SDProgram.SDCastMember> castMembers = new List<SDProgram.SDCastMember>();
                if (sdProgram.cast != null) castMembers.AddRange(sdProgram.cast);
                if (sdProgram.crew != null) castMembers.AddRange(sdProgram.crew);
                if (castMembers.Count > 0)
                {
                    castAndCrew = new List<DBCastMember>();
                    foreach (var castMember in castMembers)
                    {
                        castAndCrew.Add(new DBCastMember(castMember));
                    }
                }

                md5 = sdProgram.md5;
            }

            [DataMember(Name = "programID", IsRequired = true)]
            public string programID { get; set; }

            // First title from SD used as primary, longest and shortest kept if different to use
            // in SeriesInfo.
            [DataMember(Name = "title")]
            public string title { get; set; }
            [DataMember(Name = "shortTitle")]
            public string shortTitle { get; set; }
            [DataMember(Name = "longTitle")]
            public string longTitle { get; set; }

            [DataMember(Name = "episodeTitle")]
            public string episodeTitle { get; set; }

            // There may be many SchedulesDirect descriptions, but WMC supports 1 short & 1 long, so we
            // use the first 100 char description and first 1000 char description.
            [DataMember(Name ="description")]
            public string description { get; set; }
            [DataMember(Name ="shortDescription")]
            public string shortDescription { get; set; }

            [DataMember(Name = "language")]
            public string language { get; set; }

            [DataMember(Name = "originalAirDate")]
            public DateTime originalAirDate { get; set; }

            [DataMember(Name ="seasonNumber")]
            public int seasonNumber { get; set; }
            [DataMember(Name ="episodeNumber")]
            public int episodeNumber { get; set; }

            [DataMember(Name = "keyWords")]
            public Dictionary<string, string[]> keyWords { get; set; }

            [DataMember(Name = "entityType")]
            public EntityType entityType { get; set; }

            [DataMember(Name = "showType")]
            public ShowType showType { get; set; }

            [DataMember(Name = "halfStars")]
            public int halfStars { get; set; }

            [DataMember(Name = "mpaaRating")]
            public MpaaRating mpaaRating { get; set; }

            [DataMember(Name = "genres", EmitDefaultValue =false)]
            public HashSet<string> genres { get; set; }

            [DataMember(Name ="contentAdvisory", EmitDefaultValue =false)]
            public HashSet<string> contentAdvisory { get; set; }

            [DataMember(Name ="hasImages", EmitDefaultValue =false)]
            public bool hasImages { get; set; }

            [DataMember(Name ="castAndCrew", EmitDefaultValue =false)]
            public List<DBCastMember> castAndCrew { get; set; }

            [DataMember(Name = "md5", IsRequired =true)]
            public string md5 { get; set; }

            public bool IsSeries()
            {
                switch (showType)
                {
                    case ShowType.Miniseries:
                    case ShowType.Serial:
                    case ShowType.Series:
                        return true;
                }
                return entityType == EntityType.Episode;
            }

            [DataContract]
            public class DBCastMember
            {
                public DBCastMember() { }
                static Dictionary<string, RoleType> roleSubstringMapping_ = new Dictionary<string, RoleType>
                {
                    {"Actor", RoleType.Actor },
                    {"Guest", RoleType.GuestActor },
                    {"Director", RoleType.Director },
                    {"Writer", RoleType.Writer },
                    {"Host", RoleType.Host }
                };
                internal DBCastMember(SDProgram.SDCastMember sdCastMember)
                {
                    id = string.Format("{0}|{1}", sdCastMember.personID ?? string.Empty, sdCastMember.nameID ?? string.Empty);
                    name = sdCastMember.name;
                    if (string.IsNullOrEmpty(id)) id = name;
                    roleType = RoleType.Producer;  // Default until we figure out more appropriate role
                    foreach (var kv in roleSubstringMapping_)
                        if (sdCastMember.role.Contains(kv.Key))
                            roleType = kv.Value;
                    billingOrder = int.Parse(sdCastMember.billingOrder);
                    PersonCache.instance.AddPerson(id, name);
                }
                [DataMember(Name ="id")]
                public string id { get; set; }
                [DataMember(Name ="name", IsRequired =true)]
                public string name { get; set; }
                [DataMember(Name ="roleType")]
                public RoleType roleType { get; set; }
                [DataMember(Name = "billingOrder", IsRequired = true)]
                public int billingOrder { get; set; }

                public enum RoleType
                {
                    Actor,
                    GuestActor,
                    Host,
                    Writer,
                    Producer,
                    Director
                }
            }
        }

        internal bool IsProgramLoaded(string programID)
        {
            return programCache_.ContainsKey(programID);
        }

        public class MXFProgram
        {
            const string kProgramIdMXFPrefix = "!Program!GSD";
            public MXFProgram() { throw new NotImplementedException(); }
            public MXFProgram(DBProgram dbProgram)
            {
                dbProgram_ = dbProgram;
                if (dbProgram.keyWords != null)
                {
                    List<string> keywordIds = new List<string>();
                    var keywordsCache = KeywordCache.instance.keywords;
                    foreach (var keywordGroup in dbProgram.keyWords)
                    {
                        keywordsCache.AddKeywords(keywordGroup.Key, keywordGroup.Value);
                        keywordIds.Add(keywordsCache.GetTopLevelKeywordId(keywordGroup.Key));
                        foreach(string secondLevelKeyword in keywordGroup.Value)
                        {
                            keywordIds.Add(keywordsCache.GetSecondLevelKeywordId(keywordGroup.Key, secondLevelKeyword));
                        }
                    }
                    keywords = string.Join(",", keywordIds);
                }
                if (isSeries)
                {
                    series = SeriesInfoCache.instance.GetMxfSeriesIdBySdId(key_);
                }

                if (dbProgram.hasImages)
                {
                    string imageId = ImageCache.GetSDImageIDByProgramID(key_);
                    if (ImageCache.instance.isImageLoaded(imageId))
                        guideImage = ImageCache.instance.FindOrCreateMXFImageId(imageId);
                }
            }

            private string key_ { get { return dbProgram_.programID; } }

            private IEnumerable<DBProgram.DBCastMember> castAndCrew_ { get { return dbProgram_.castAndCrew; } }
            private readonly DBProgram dbProgram_;

            private bool GenreContains(string genre)
            {
                return dbProgram_.genres != null && dbProgram_.genres.Contains(genre);
            }

            private bool ContentAdvisoryContains(string contentAdvisory)
            {
                return dbProgram_.contentAdvisory != null && dbProgram_.contentAdvisory.Contains(contentAdvisory);
            }

            public class CastAndCrewSerializer : IXmlSerializable
            {
                public CastAndCrewSerializer() { }
                internal CastAndCrewSerializer(IEnumerable<DBProgram.DBCastMember> castAndCrew)
                {
                    castAndCrew_ = castAndCrew;
                }

                private IEnumerable<DBProgram.DBCastMember> castAndCrew_;

                XmlSchema IXmlSerializable.GetSchema()
                {
                    return null;
                }

                void IXmlSerializable.ReadXml(XmlReader reader)
                {
                    throw new NotImplementedException();
                }

                private string GetElementNameForRole(DBProgram.DBCastMember.RoleType role)
                {
                    return role.ToString() + "Role";
                }
                void IXmlSerializable.WriteXml(XmlWriter writer)
                {
                    if (castAndCrew_ == null) return;
                    foreach(var castMember in castAndCrew_)
                    {
                        writer.WriteStartElement(GetElementNameForRole(castMember.roleType));
                        writer.WriteAttributeString("person", PersonCache.instance.GetMXFPersonIdForKey(castMember.id));
                        writer.WriteStartAttribute("rank");
                        writer.WriteValue(castMember.billingOrder);
                        writer.WriteEndAttribute();
                        writer.WriteEndElement();
                    }
                }
            }

            [DataMember(IsRequired = true), XmlAttribute("id")]
            public string mxfId {
                get { return ProgramCache.instance.GetProgramIndexByKey(key_); }
                set { throw new NotImplementedException(); }
            }

            [DataMember(IsRequired = true), XmlAttribute("uid")]
            public string uid {
                get { return kProgramIdMXFPrefix + key_; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue = false), XmlAttribute("title")]
            public string title {
                get { return dbProgram_.title; }
                set { throw new NotImplementedException(); }
            }
            [DataMember(EmitDefaultValue = false), XmlAttribute("episodeTitle")]
            public string episodeTitle {
                get { return dbProgram_.episodeTitle; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue = false), XmlAttribute("description")]
            public string description {
                get { return string.IsNullOrEmpty(dbProgram_.description) ? dbProgram_.shortDescription : dbProgram_.description; }
                set { throw new NotImplementedException(); }
            }
            [DataMember(EmitDefaultValue = false), XmlAttribute("shortDescription")]
            public string shortDescription
            {
                get { return dbProgram_.shortDescription; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue = false), XmlAttribute("language")]
            public string language {
                get { return dbProgram_.language; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue = false), XmlAttribute("year"), DefaultValue(0)]
            public int year {
                get { return dbProgram_.originalAirDate != default(DateTime) ? dbProgram_.originalAirDate.Year : 0; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue = false), XmlAttribute("seasonNumber"), DefaultValue(0)]
            public int seasonNumber {
                get { return dbProgram_.seasonNumber; }
                set { throw new NotImplementedException(); }
            }
            [DataMember(EmitDefaultValue = false), XmlAttribute("episodeNumber"), DefaultValue(0)]
            public int episodeNumber
            {
                get { return dbProgram_.episodeNumber; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue = false), XmlAttribute("originalAirDate")]
            public string originalAirDate {
                get {
                    return (dbProgram_.originalAirDate != default(DateTime)) 
                        ? dbProgram_.originalAirDate.ToLocalTime().ToString("o") : null;
                }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue =false), XmlAttribute("keywords")]
            public string keywords { get; set; }

            // season ID not here as season info (other than #) is not part of current schedulesdirect data.
            [DataMember(EmitDefaultValue = false), XmlAttribute("season")]
            public string season { get { return null; } set { throw new NotImplementedException(); } }

            [DataMember(EmitDefaultValue =false), XmlAttribute("series")]
            public string series { get; set; }

            [DataMember(EmitDefaultValue =false), XmlAttribute("mpaaRating"), DefaultValue(0)]
            public int mpaaRating {
                get { return (int)dbProgram_.mpaaRating; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue =false), XmlAttribute("isMovie"), DefaultValue(false)]
            public bool isMovie {
                get { return dbProgram_.entityType == EntityType.Movie; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue = false), XmlAttribute("isMiniseries"), DefaultValue(false)]
            public bool isMiniseries {
                get {
                    return ScheduleCache.instance.IsProgramIdMiniseries(key_);
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            // No idea what this is, for now always false.
            [DataMember(EmitDefaultValue =false), XmlAttribute("isLimitedSeries"), DefaultValue(false)]
            public bool isLimitedSeries {
                get { return false; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue = false), XmlAttribute("isPaidProgramming"), DefaultValue(false)]
            public bool isPaidProgramming {
                get { return dbProgram_.showType == ShowType.PaidProgramming; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue = false), XmlAttribute("isSerial"), DefaultValue(false)]
            public bool isSerial {
                get { return dbProgram_.showType == ShowType.Serial; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue = false), XmlAttribute("isSeries"), DefaultValue(false)]
            public bool isSeries {
                get { return dbProgram_.IsSeries(); }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue =false), XmlAttribute("isShortFilm"), DefaultValue(false)]
            public bool isShortFilm {
                get { return dbProgram_.showType == ShowType.ShortFilm; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue =false), XmlAttribute("isSpecial"), DefaultValue(false)]
            public bool isSpecial {
                get { return dbProgram_.showType == ShowType.Special; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue = false), XmlAttribute("isSports"), DefaultValue(false)]
            public bool isSports {
                get { return dbProgram_.entityType == EntityType.Sports; }
                set { throw new NotImplementedException(); }
            }

            [DataMember(EmitDefaultValue =false), XmlAttribute("isNews"), DefaultValue(false)]
            public bool isNews { get { return GenreContains("News"); } set { throw new NotImplementedException(); } }

            [DataMember(EmitDefaultValue =false), XmlAttribute("isKids"), DefaultValue(false)]
            public bool isKids { get { return GenreContains("Children"); } set { throw new NotImplementedException(); } }

            [DataMember(EmitDefaultValue = false), XmlAttribute("isReality"), DefaultValue(false)]
            public bool isReality { get { return GenreContains("Reality"); } set { throw new NotImplementedException(); } }

            [DataMember(EmitDefaultValue =false), XmlAttribute("hasAdult"), DefaultValue(false)]
            public bool hasAdult {
                get { return ContentAdvisoryContains("Adult Situations"); }
                set { throw new NotImplementedException(); }
            }
            [DataMember(EmitDefaultValue = false), XmlAttribute("hasStrongSexualContent"), DefaultValue(false)]
            public bool hasStrongSexualContent {
                get { return ContentAdvisoryContains("Strong Sexual Content"); }
                set { throw new NotImplementedException(); }
            }
            [DataMember(EmitDefaultValue =false), XmlAttribute("hasLanguage"), DefaultValue(false)]
            public bool hasLanguage {
                get { return ContentAdvisoryContains("Adult Language"); }
                set { throw new NotImplementedException(); }
            }
            [DataMember(EmitDefaultValue = false), XmlAttribute("hasViolence"), DefaultValue(false)]
            public bool hasViolence {
                get { return ContentAdvisoryContains("Violence"); }
                set { throw new NotImplementedException(); }
            }
            [DataMember(EmitDefaultValue =false), XmlAttribute("hasGraphicViolence"), DefaultValue(false)]
            public bool hasGraphicViolence {
                get { return ContentAdvisoryContains("Graphic Violence"); }
                set { throw new NotImplementedException(); }
            }        
            [DataMember(EmitDefaultValue = false), XmlAttribute("hasBriefNudity"), DefaultValue(false)]
            public bool hasBriefNudity {
                get { return ContentAdvisoryContains("Brief Nudity"); }
                set { throw new NotImplementedException(); }
            }
            [DataMember(EmitDefaultValue =false), XmlAttribute("hasNudity"), DefaultValue(false)]
            public bool hasNudity {
                get { return ContentAdvisoryContains("Nudity"); }
                set { throw new NotImplementedException(); }
            }
            [DataMember(EmitDefaultValue =false), XmlAttribute("hasMildViolence"), DefaultValue(false)]
            public bool hasMildViolence {
                get { return ContentAdvisoryContains("Mild Violence"); }
                set { throw new NotImplementedException(); }
            }
            [DataMember(EmitDefaultValue =false), XmlAttribute("hasRape"), DefaultValue(false)]
            public bool hasRape {
                get { return ContentAdvisoryContains("Rape"); }
                set { throw new NotImplementedException(); }
            }
            [DataMember(EmitDefaultValue = false), XmlAttribute("hasGraphicLanguage"), DefaultValue(false)]
            public bool hasGraphicLanguage {
                get { return ContentAdvisoryContains("Graphic Language"); }
                set { throw new NotImplementedException(); }
            }
            [DataMember(EmitDefaultValue =false), XmlAttribute("guideImage")]
            public string guideImage { get; set; }

            [XmlAnyElement]
            public CastAndCrewSerializer castAndCrew {
                get { return new CastAndCrewSerializer(castAndCrew_); }
                set { throw new NotImplementedException(); }
            }
        }

        [DataContract]
        public class SDProgram
        {
            [DataMember(Name = "programID", IsRequired = true)]
            public string programID { get; set; }
            #region error info
            [DataMember(Name = "code")]
            public int code { get; set; }
            [DataMember(Name = "response")]
            public string response { get; set; }
            [DataMember(Name = "serverID")]
            public string serverID { get; set; }
            [DataMember(Name = "datetime")]
            public string datetime { get; set; }
            [DataMember(Name = "message")]
            public string message { get; set; }
            #endregion
            [DataMember(Name = "titles", IsRequired = true)]  // program title(s)
            public SDProgramTitle[] titles { get; set; }
            public string GetShortTitle()
            {
                if (titles.Length == 0) return null;
                string shortTitle = titles[0].title120;
                foreach (SDProgramTitle programTitle in titles)
                {
                    string title120 = programTitle.title120;
                    if (title120.Length < shortTitle.Length)
                    {
                        shortTitle = title120;
                    }
                }
                return shortTitle;
            }
            public string GetLongTitle()
            {
                if (titles.Length == 0) return null;
                string longTitle = string.Empty;
                foreach (SDProgramTitle programTitle in titles)
                {
                    string title120 = programTitle.title120;
                    if (title120.Length > longTitle.Length)
                    {
                        longTitle = title120;
                    }
                }
                return longTitle;
            }
            public string GetFirstTitle()
            {
                if (titles.Length == 0) return null;
                return titles[0].title120;
            }
            [DataMember(Name = "episodeTitle150")] // episode title
            public string episodeTitle150 { get; set; }
            [DataMember(Name = "eventDetails")]
            public SDProgramEventDetails eventDetails { get; set; }
            [DataMember(Name = "descriptions")] // program descriptions
            public SDProgramDescriptions descriptions { get; set; }
            public string GetLanguage()
            {
                // Using description language as a proxy for program language until we hear there is a better way.
                if (descriptions?.description100s?.Count > 0)
                {
                    return descriptions.description100s[0].descriptionLanguage;
                }
                if (descriptions?.description1000s?.Count > 0)
                {
                    return descriptions.description1000s[0].descriptionLanguage;
                }
                return null;
            }

            [DataMember(Name = "originalAirDate")]  // YYYY-MM-DD, optional
            public DateTime originalAirDate { get; set; }
            [DataMember(Name = "genres")]
            public string[] genres { get; set; }
            [DataMember(Name = "holiday")]
            public string holiday { get; set; }
            [DataMember(Name = "animation")]
            public string animation { get; set; }
            [DataMember(Name = "officialURL")]
            public string officialURL { get; set; }
            [DataMember(Name = "keyWords")]
            public Dictionary<string, string[]> keyWords { get; set; }
            //public SDProgramKeywords keyWords { get; set; }
            // array of dictionaries; dictionary key is provider.  I have no idea why this is in such a wierd format.
            [DataMember(Name = "metadata")]
            public Dictionary<string, SDSeasonAndEpisodeNum>[] metadata { get; set; }
            // Helper functions to parse the strange metadata format above
            public int GetSeasonNumber()  // Returns 0 if unknown
            {
                if (metadata == null) return 0;
                foreach(var metadataDictionary in metadata)
                {
                    foreach(SDSeasonAndEpisodeNum seasonAndEpisode in metadataDictionary.Values)
                        return seasonAndEpisode.season;
                }
                return 0;
            }
            public int GetEpisodeNumber() // Returns 0 if unknown
            {
                if (metadata == null) return 0;
                foreach (var metadataDictionary in metadata)
                {
                    foreach (SDSeasonAndEpisodeNum seasonAndEpisode in metadataDictionary.Values)
                        return seasonAndEpisode.episode;
                }
                return 0;
            }

            // Supposed to be IsRequired but a few shows are missing it.
            [DataMember(Name = "entityType")]
            public string entityTypeString
            {
                get { return entityType_.ToString(); }
                set
                {
                    if (!Enum.TryParse<EntityType>(value, out entityType_))
                    {
                        Console.WriteLine("Bad entityType: {0}", value);
                        entityType_ = EntityType.Invalid;
                    }
                }
            }  // Must be one of Show | Episode | Sports | Movie
            [IgnoreDataMember]
            public EntityType entityType
            {
                get { return entityType_; }
                set { entityType_ = value; }
            }
            [IgnoreDataMember]
            private EntityType entityType_;

            public bool IsSeries()
            {
                switch(showType)
                {
                    case ShowType.Miniseries:
                    case ShowType.Serial:
                    case ShowType.Series:
                        return true;
                }
                return entityType == EntityType.Episode;
            }
            [DataMember(Name = "showType")]  // type of program, optional
            public string showTypeString
            {
                get { return showType_.ToString(); }
                set
                {
                    if (!Enum.TryParse<ShowType>(value.Replace(" ","").Replace("-",""), true, out showType_))
                    {
                        Console.WriteLine("Unrecognized ShowType: {0}", value);
                        showType_ = ShowType.Unknown;
                    }
                }
            }
            [IgnoreDataMember]
            public ShowType showType { get { return showType_; } set { showType_ = value; } }
            [IgnoreDataMember]
            private ShowType showType_;
            [DataMember(Name = "audience")]
            public string audience { get; set; }
            [DataMember(Name = "contentAdvisory")]
            public string[] contentAdvisory { get; set; }
            [DataMember(Name = "contentRating")]
            public SDContentRating[] contentRating { get; set; }
            public MpaaRating GetMpaaRating()
            {
                if (contentRating == null) return MpaaRating.Unknown;
                // TODO: add option to support rating systems other than US.
                foreach(SDContentRating rating in contentRating)
                {
                    if (rating.body == "Motion Picture Association of America")
                    {
                        MpaaRating result;
                        if (Enum.TryParse<MpaaRating>(rating.code.Replace("-", ""), out result))
                        {
                            return result;
                        }
                        Console.WriteLine("Failed to parse MPAA rating: {0}", rating.code);
                    }
                }
                return MpaaRating.Unknown;
            }

            [DataMember(Name = "movie")]
            public SDMovie movie { get; set; }
            [DataMember(Name = "cast")]
            public SDCastMember[] cast { get; set; }
            [DataMember(Name = "crew")]
            public SDCastMember[] crew { get; set; }
            [DataMember(Name = "recommendations")]  // Array of similar programs!
            public SDRecommendation[] recommendations { get; set; }
            [DataMember(Name = "hasImageArtwork")]
            public bool hasImageArtwork { get; set; }
            [DataMember(Name = "md5", IsRequired = true)]  // md5 hash value of the JSON
            public string md5 { get; set; }
            [DataContract]
            public class SDRecommendation
            {
                [DataMember(Name = "programID", IsRequired = true)]
                public string programID { get; set; }
                [DataMember(Name = "title120", IsRequired = true)]
                public string title120 { get; set; }
            }

            [DataContract]
            public class SDCastMember
            {
                [DataMember(Name = "personID")]  // used to retrieve images.
                public string personID { get; set; }
                [DataMember(Name = "nameID")]
                public string nameID { get; set; }
                [DataMember(Name = "name", IsRequired = true)]
                public string name { get; set; }
                [DataMember(Name = "role", IsRequired = true)]
                public string role { get; set; }
                [DataMember(Name = "characterName")]
                public string characterName { get; set; }
                [DataMember(Name = "billingOrder", IsRequired = true)]
                public string billingOrder { get; set; }
            }

            [DataContract]
            public class SDMovie
            {
                [DataMember(Name = "year")]
                public string year { get; set; }  // YYYY
                [DataMember(Name = "duration")]  // duration in seconds
                public int duration { get; set; }
                [DataMember(Name = "qualityRating")]
                public SDMovieQualityRating[] qualityRating { get; set; }
            }

            [DataContract]
            public class SDMovieQualityRating
            {
                [DataMember(Name = "ratingsBody", IsRequired = true)]
                public string ratingsBody { get; set; }
                [DataMember(Name = "rating", IsRequired = true)]
                public string rating { get; set; }
                [DataMember(Name = "minRating")]
                public string minRating { get; set; }
                [DataMember(Name = "maxRating")]
                public string maxRating { get; set; }
                [DataMember(Name = "increment")]
                public string increment { get; set; }
            }

            [DataContract]
            public class SDContentRating
            {
                [DataMember(Name = "body", IsRequired = true)]
                public string body { get; set; }
                [DataMember(Name = "code", IsRequired = true)]
                public string code { get; set; }
            }

            [DataContract]
            public class SDSeasonAndEpisodeNum
            {
                [DataMember(Name = "season", IsRequired = true)]
                public int season { get; set; }
                [DataMember(Name = "episode")]
                public int episode { get; set; }
            }

            [DataContract]
            public class SDProgramDescriptions
            {
                [DataMember(Name = "description100")] // short descriptions
                public List<SDProgramDescription> description100s { get; set; }
                [DataMember(Name = "description1000")]  // long descriptions
                public List<SDProgramDescription> description1000s { get; set; }
            }

            [DataContract]
            public class SDProgramDescription
            {
                [DataMember(Name = "descriptionLanguage", IsRequired = true)]
                public string descriptionLanguage { get; set; }
                [DataMember(Name = "description", IsRequired = true)]
                public string description { get; set; }
            }

            [DataContract]
            public class SDProgramEventDetails
            {
                [DataMember(Name = "subType")]
                public string subType { get; set; }
                [DataMember(Name = "venue100")] // Event location
                public string venue100 { get; set; }
                [DataMember(Name = "teams")] // Teams that are playing
                public SDEventTeam[] teams { get; set; }
                [DataMember(Name = "gameDate")]
                public DateTime gameDate { get; set; }
            }

            [DataContract]
            public class SDEventTeam
            {
                [DataMember(Name = "name", IsRequired = true)]  // team name
                public string name { get; set; }
                [DataMember(Name = "isHome")]  // Is this the home team?
                public bool isHome { get; set; }
                [DataMember(Name = "gameDate")]  // YYYY-MM-DD
                public string gameDate { get; set; }
            }

            [DataContract]
            public class SDProgramTitle
            {
                [DataMember(Name = "title120", IsRequired = true)]
                public string title120 { get; set; }
            }
        }
    }
}
