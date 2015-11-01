using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.MediaCenter.Guide;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;

namespace SchedulesDirectGrabber
{
    public class ScheduleCache
    {
        private static ScheduleCache instance_ = new ScheduleCache();
        internal static ScheduleCache instance { get { return instance_; } }

        internal bool IsProgramIdMiniseries(string programId)
        {
            if (!entriesByProgramId_.ContainsKey(programId)) throw new Exception("Unrecognized program ID!");
            return entriesByProgramId_[programId][0].multipart != null;
        }
        internal IEnumerable<ProgramCache.DBProgram> GetAllProgramsForStations(ISet<string> stationIDs)
        {
            var schedulesByStation = GetSchedulesByStation(stationIDs);
            BuildIndexByProgramId();
            Console.WriteLine("Building list of program MD5s from current cached schedules");
            Dictionary<string, string> md5sByProgramID = new Dictionary<string, string>();
            foreach (var schedule in schedulesByStation.Values)
                foreach (var program in schedule.Values)
                    md5sByProgramID[program.programID] = program.md5;
            HashSet<string> audioProperties = new HashSet<string>();
            foreach (var schedule in schedulesByStation.Values)
                foreach (var programEntry in schedule.Values)
                    if (programEntry.audioProperties != null)
                        audioProperties.UnionWith(programEntry.audioProperties);
            foreach(string audioProperty in audioProperties)
            {
                Console.WriteLine(audioProperty);
            }

            return ProgramCache.instance.GetProgramsByIDWithMD5(md5sByProgramID);
        }

        private IDictionary<string, SortedDictionary<DateTime, SDStationScheduleProgramEntry>>
            GetSchedulesByStation(ISet<string> stationIDs)
        {
            // Only retrieve stations not already cached.
            HashSet<string> uncachedStationIDs = new HashSet<string>();
            foreach(string stationID in stationIDs)
            {
                if (!cachedSchedule_.ContainsKey(stationID))
                {
                    uncachedStationIDs.Add(stationID);
                    cachedSchedule_[stationID] = new SortedDictionary<DateTime, SDStationScheduleProgramEntry>();
                }
            }
            var stationSchedules = DBManager.instance.GetStationSchedules(uncachedStationIDs);
            foreach(var stationSchedule in stationSchedules)
            {
                foreach(var stationDaySchedule in stationSchedule.Value)
                    foreach(var scheduleEntry in stationDaySchedule.Value.programs)
                        cachedSchedule_[stationSchedule.Key][scheduleEntry.airDateTime] = scheduleEntry;
            }
            Console.WriteLine("Reading schedule MD5s from SchedulesDirect to determine what to download");
            var md5Responses = GetStationMD5Responses(uncachedStationIDs);
            Console.WriteLine("Building list of schedules to fetch");
            Dictionary<string, List<string>> daysNeedingUpdate = new Dictionary<string, List<string>>();
            foreach(var stationResponse in md5Responses)
            {
                var station = stationResponse.Key;
                if (!stationSchedules.ContainsKey(station))
                {
                    stationSchedules[station] = new Dictionary<string, SDStationScheduleResponse>();
                }
                var dailySchedule = stationSchedules[station];
                foreach (var dayResponse in stationResponse.Value)
                {
                    string day = dayResponse.Key;
                    if (!dailySchedule.ContainsKey(day) || dailySchedule[day].metadata.md5 != dayResponse.Value.md5)
                    {
                        if (!daysNeedingUpdate.ContainsKey(station)) daysNeedingUpdate[station] = new List<string>();
                        daysNeedingUpdate[station].Add(day);
                    }
                }
            }
            List<SDStationScheduleResponse> stationScheduleResponses = GetStationScheduleResponses(daysNeedingUpdate);
            DBManager.instance.SaveSchedules(stationScheduleResponses);
            Console.WriteLine("Updating cache with schedules from DB and download.");
            foreach(SDStationScheduleResponse response in stationScheduleResponses)
            {
                string station = response.stationID;
                DateTime day_start = DateTime.Parse(response.metadata.startDate + "T00:00:00Z");
                DateTime day_end = day_start.AddDays(1);
                if (!cachedSchedule_.ContainsKey(station))
                    cachedSchedule_[station] = new SortedDictionary<DateTime, SDStationScheduleProgramEntry>();
                // Remove and replace programs whose start date is in the range of response day.
                var stationSchedule = cachedSchedule_[station];
                var keysToRemove = stationSchedule.Keys.Where(k => ((day_start <= k) && (k < day_end))).ToArray();
                foreach(var key in keysToRemove)
                {
                    stationSchedule.Remove(key);
                }
                foreach(var entry in response.programs)
                {
                    stationSchedule[entry.airDateTime] = entry;
                }
            }

            return cachedSchedule_;
        }

        private Dictionary<string, SortedDictionary<DateTime, SDStationScheduleProgramEntry>> cachedSchedule_ =
            new Dictionary<string, SortedDictionary<DateTime, SDStationScheduleProgramEntry>>();

        private Dictionary<string, List<SDStationScheduleProgramEntry>> entriesByProgramId_ =
            new Dictionary<string, List<SDStationScheduleProgramEntry>>();

        private void BuildIndexByProgramId()
        {
            Console.WriteLine("Indexing schedule by programId");
            foreach(var dateAndEntry in cachedSchedule_.Values)
            {
                foreach(var entry in dateAndEntry.Values)
                {
                    string programId = entry.programID;
                    if (!entriesByProgramId_.ContainsKey(programId))
                        entriesByProgramId_[programId] = new List<SDStationScheduleProgramEntry>();
                    entriesByProgramId_[programId].Add(entry);
                }
            }
        }

        private List<SDStationScheduleResponse> GetStationScheduleResponses(IDictionary<string, List<string>> daysByStationID)
        {
            Console.WriteLine("Downloading station schedules from SchedulesDirect");
            const int kBatchSize = 5000;
            List<SDScheduleStationRequest> request = new List<SDScheduleStationRequest>();
            List<SDStationScheduleResponse> responses = new List<SDStationScheduleResponse>();

            Func<int> DoBatch = new Func<int>(() => {
                var batchResponses = JSONClient.GetJSONResponse<List<SDStationScheduleResponse>>(UrlBuilder.BuildWithAPIPrefix("/schedules"),
                request, SDTokenManager.token_manager.token);
                // Some of the reponses may be errors!  Loop through and exclude these so we don't replace good data!
                foreach(var response in batchResponses)
                {
                    if (response.code > 0)
                    {
                        Console.WriteLine(
                            "Portions of the schedule for station ID {0} failed to download, SchedulesDirect response code: {1} - {2}",
                            response.stationID, response.code, response.reponse);
                        continue;
                    }
                    responses.Add(response);
                }
                request.Clear();
                return 0;
            });

            foreach(var stationIDAndDays in daysByStationID)
            {
                request.Add(new SDScheduleStationRequest(stationIDAndDays.Key, stationIDAndDays.Value));
                if (request.Count > kBatchSize) DoBatch();
            }
            if (request.Count > 0) DoBatch();
            return responses;
        }

        private IDictionary<string, Dictionary<string, SDStationMD5Response>> GetStationMD5Responses(IEnumerable<string> stationIDs)
        {
            // TODO: batch by 5000
            const int kBatchSize = 5000;
            List<Dictionary<string, string>> request = new List<Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, SDStationMD5Response>> stationMD5Responses =
                new Dictionary<string, Dictionary<string, SDStationMD5Response>>();
            Func<int> DoBatch = new Func<int>(() => {
                //var batchResponse = JSONClient.GetJSONResponse<Dictionary<string, Dictionary<string, SDStationMD5Response>>>(
                var batchResponse = JSONClient.GetJSONResponse<Dictionary<string, object>>(
                UrlBuilder.BuildWithAPIPrefix("/schedules/md5"), request, SDTokenManager.token_manager.token);
                foreach (var keyval in batchResponse)
                {
                    string stationID = keyval.Key;
                    try
                    {
                        Dictionary<string, SDStationMD5Response> dailyResponses =
                            JSONClient.Deserialize<Dictionary<string, SDStationMD5Response>>(keyval.Value.ToString());

                        stationMD5Responses[stationID] = dailyResponses;
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Failed to deserialize schedule MD5s for station {0}, JSON: {1}", stationID, keyval.Value);
                        Misc.OutputException(exc);
                    }
                }
                request.Clear();
                return 0;
            });
            foreach (string stationID in stationIDs)
            {
                request.Add(new Dictionary<string, string>() { { "stationID", stationID } });
                if (request.Count >= kBatchSize)
                {
                    DoBatch();
                }
            }
            if (request.Count > 0) DoBatch();
            return stationMD5Responses;
        }

        public class ScheduleSerializer : IXmlSerializable
        {
            public ScheduleSerializer() { }

            private ScheduleCache scheduleCache { get { return ScheduleCache.instance; } }
            private StationCache stationCache { get { return StationCache.instance; } }

            XmlSchema IXmlSerializable.GetSchema()
            {
                return null;
            }

            void IXmlSerializable.ReadXml(XmlReader reader)
            {   // Class is write-only for generating MXF
                throw new NotImplementedException();
            }

            void IXmlSerializable.WriteXml(XmlWriter writer)
            {
                // Overwrite seriealized element name for serialized MXFScheduleEntries.
/*                XmlElementAttribute myElementAttribute = new XmlElementAttribute();
                myElementAttribute.ElementName = "ScheduleEntry";
                XmlAttributes myAttributes = new XmlAttributes();
                myAttributes.XmlElements.Add(myElementAttribute);
                XmlAttributeOverrides myOverrides = new XmlAttributeOverrides();
                myOverrides.Add(typeof(ScheduleSerializer), myAttributes); */
                XmlSerializer entrySerializer = new XmlSerializer(typeof(MXFScheduleEntry)); 
                foreach (var stationAndSchedules in scheduleCache.GetSchedulesByStation(new HashSet<string>(stationCache.GetStationIds())))
                {
                    string stationId = stationAndSchedules.Key;
                    string mxfServiceId = stationCache.GetServiceIdByStationId(stationId);
                    var schedule = stationAndSchedules.Value;
                    if (schedule.Count == 0) continue;
                    writer.WriteStartElement("ScheduleEntries");
                    writer.WriteAttributeString("service", mxfServiceId);
                    DateTime previousEndTime = schedule.Keys.First();
                    bool isFirstElement = true;
                    foreach (var dateAndEntry in schedule)
                    {
                        DateTime startTime = dateAndEntry.Key;
                        DateTime endTime = startTime.AddSeconds(dateAndEntry.Value.duration);
                        // Check for gap between the start of this entry and the end of the previous one.
                        bool foundGap = startTime != previousEndTime;
                        if (!isFirstElement && foundGap)
                        {
                            writer.WriteEndElement();  // close ScheduleEntries element
                            writer.WriteStartElement("ScheduleEntries");  // start next group
                            writer.WriteAttributeString("service", mxfServiceId);
                        }
                        entrySerializer.Serialize(writer, new MXFScheduleEntry(dateAndEntry.Value, foundGap || isFirstElement));
                        previousEndTime = endTime;
                        isFirstElement = false;
                    }
                    writer.WriteEndElement();  // Close final ScheduleEntries element.
                }
            }
        }

        [DataContract]
        private class SDScheduleStationRequest
        {
            public SDScheduleStationRequest(string stationID)
            {
                this.stationID = stationID;
            }
            public SDScheduleStationRequest(string stationID, List<string> dates)
            {
                this.stationID = stationID;
                this.dates = dates;
            }

            [DataMember(Name = "stationID", IsRequired = true)]
            public string stationID { get; set; }
            [DataMember(Name = "date")]
            public List<string> dates { get; set; }
        }

        [DataContract]
        private class SDStationMD5Response
        {
            [DataMember(Name ="code", IsRequired =true)]
            public int code { get; set; }
            [DataMember(Name ="message")]
            public string message { get; set; }
            [DataMember(Name ="lastModified", IsRequired =true)]
            public DateTime lastModified { get; set; }
            [DataMember(Name ="md5", IsRequired =true)]
            public string md5 { get; set; }
        }

        [DataContract]
        internal class SDStationScheduleResponse
        {
            [DataMember(Name = "stationID", IsRequired = true)]
            public string stationID { get; set; }
            [DataMember(Name = "serverID")]
            public string serverID { get; set; }
            [DataMember(Name = "code")]  // Indicates an error
            public int code { get; set; }
            [DataMember(Name = "response")] // better error description
            public string reponse { get; set; }
            [DataMember(Name = "programs")]
            public SDStationScheduleProgramEntry[] programs { get; set; }
            [DataMember(Name = "metadata")]
            internal SDStationScheduleResponseMetadata metadata { get; set; }
            [DataContract]
            internal class SDStationScheduleResponseMetadata
            {
                [DataMember(Name = "modified")]
                public string modified { get; set; }
                [DataMember(Name = "md5")]
                public string md5 { get; set; }
                [DataMember(Name = "startDate")]
                public string startDate { get; set; }
                [DataMember(Name = "code")]  // Indicates an error.  
                public int code { get; set; }

                public DateTime GetDate() { return DateTime.Parse(startDate).Date; }
            }

        }

        // TODO: Create new Schedule DB format to decouple this from the SchedulesDirect native format.
        [DataContract]
        public class SDStationScheduleProgramEntry
        {
            [DataMember(Name = "programID", IsRequired = true)]
            public string programID { get; set; }
            [DataMember(Name = "airDateTime", IsRequired = true)]
            public DateTime airDateTime { get; set; }
            [DataMember(Name = "duration", IsRequired = true)]
            public int duration { get; set; }
            // md5 of the associated program
            [DataMember(Name = "md5", IsRequired = true)]
            public string md5 { get; set; }
            [DataMember(Name = "new")]
            public bool isNew { get; set; }
            [DataMember(Name = "cableInTheClassRoom")]
            public bool cableInTheClassRoom { get; set; }
            [DataMember(Name = "catchup")]
            public bool catchup { get; set; }
            [DataMember(Name = "continued")]
            public bool continued { get; set; }
            [DataMember(Name = "educational")]
            public bool educational { get; set; }
            [DataMember(Name = "joinedInProgress")]
            public bool joinedInProgress { get; set; }
            [DataMember(Name = "leftInProgress")]
            public bool leftInProgress { get; set; }
            [DataMember(Name = "premiere")]  // only for miniseries & movies.  Not the same as series premiere.
            public bool moviePremiere { get; set; }
            [DataMember(Name = "programBreak")]
            public bool programBreak { get; set; }
            [DataMember(Name = "repeat")]  // repeat of an encore presentation.  Not used for typical series.
            public bool encoreRepeat { get; set; }
            [DataMember(Name = "signed")]  // Has on screen sign language translation
            public bool signed { get; set; }
            [DataMember(Name = "subjectToBlackout")]
            public bool subjectToBlackout { get; set; }
            [DataMember(Name = "timeApproximate")]
            public bool timeApproximate { get; set; }
            [DataMember(Name = "liveTapeDelay")]  // enum values of "Live", "Tape", or "Delay"
            public string liveTapeDelay { get; set; }
            // enum values of "Season Premiere", "Season Finale", "Series Premiere", or "Series Finale"
            [DataMember(Name = "isPremiereOrFinale")]
            public string isPremiereOrFinale { get; set; }
            [DataMember(Name = "ratings")]
            public List<ScheduleEntryRating> ratings { get; set; }
            [DataMember(Name = "multipart")]  // for miniseries
            public ScheduleEntryMultiPartinfo multipart { get; set; }
            [DataMember(Name = "audioProperties")]
            public HashSet<string> audioProperties { get; set; }
            [DataMember(Name = "videoProperties")]
            public HashSet<string> videoProperties { get; set; }
            [DataMember(Name = "parentalAdvisory")]
            public string parentalAdvisory { get; set; }

            [DataContract]
            public class ScheduleEntryRating
            {
                [DataMember(Name = "body", IsRequired = true)]
                public string body { get; set; }
                [DataMember(Name = "code", IsRequired = true)]
                public string code { get; set; }
            }

            [DataContract]
            public class ScheduleEntryMultiPartinfo
            {
                [DataMember(Name = "partNumber", IsRequired = true)]
                public int partNumber { get; set; }
                [DataMember(Name = "totalParts", IsRequired = true)]
                public int totalParts { get; set; }
            }

            public bool IsDelay() { return liveTapeDelay == "Delay"; }

            private bool HasAudioProp(string propertyName)
            {
                return audioProperties != null && audioProperties.Contains(propertyName);
            }
            private bool HasVideoProp(string propertyName)
            {
                return videoProperties != null && videoProperties.Contains(propertyName);
            }
            // Determine audio format enum for MXF from audioProperties
            public AudioFormat WMCAudioFormat()
            {
                if (HasAudioProp("surround")) return AudioFormat.THX;
                if (HasAudioProp("DD 5.1") || HasAudioProp("DD")) return AudioFormat.DolbyDigital;
                if (HasAudioProp("Dolby")) return AudioFormat.Dolby;
                if (HasAudioProp("stereo")) return AudioFormat.Stereo;
                return AudioFormat.NotSpecified;
            }

            public bool Is3D() { return HasVideoProp("3d"); }
            public bool IsCC() { return HasAudioProp("cc"); }
            public bool IsDVS() { return HasAudioProp("dvs"); }
            public bool IsEnhanced() { return HasVideoProp("enhanced"); }
            public bool IsFinale() { return isPremiereOrFinale != null && isPremiereOrFinale.Contains("Finale"); }
            public bool IsHD() { return HasVideoProp("hdtv"); }
            public bool IsLetterBox() { return HasVideoProp("letterbox"); }
            public bool IsLive() { return liveTapeDelay == "Live"; }
            public bool IsPremiere() { return isPremiereOrFinale != null && isPremiereOrFinale.Contains("Premiere"); }
            public bool IsRepeat() { return !isNew; }
            public bool IsSubtitled() { return HasVideoProp("subtitled"); }
            public bool IsTape() { return liveTapeDelay == "Tape"; }
        }

        [XmlRoot("ScheduleEntry")]
        public class MXFScheduleEntry
        {
            public MXFScheduleEntry() { }
            public MXFScheduleEntry(SDStationScheduleProgramEntry sdProgramEntry, bool isFirstInGroup)
            {
                sdProgramEntry_ = sdProgramEntry;
                isFirstInGroup_ = isFirstInGroup;
            }

            [XmlAttribute("program")]
            public string program
            {
                get { return ProgramCache.instance.GetProgramIndexByKey(sdProgramEntry_.programID); }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("startTime"), DataMember(EmitDefaultValue =false)]
            public string startTime  // Using a string rather than a date time so it can be sometimes not emitted.
            {
                get
                {
                    if (isFirstInGroup_) return sdProgramEntry_.airDateTime.ToString("o");
                    return null;
                }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("duration"), DataMember(EmitDefaultValue = false)]
            public int duration
            {
                get { return sdProgramEntry_.duration; }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("isCC"), DataMember(EmitDefaultValue =false), DefaultValue(false)]
            public bool isCC
            {
                get { return sdProgramEntry_.IsCC(); }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("audioFormat"), DataMember(EmitDefaultValue = false), DefaultValue(0)]
            public int audioFormat
            {
                get { return (int)sdProgramEntry_.WMCAudioFormat(); }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("isLive"), DataMember(EmitDefaultValue = false), DefaultValue(false)]
            public bool isLive
            {
                get { return sdProgramEntry_.IsLive(); }
                set { throw new NotImplementedException(); }
            }

            private ProgramCache.DBProgram GetProgram()
            {
                return ProgramCache.instance.GetProgrambyId(sdProgramEntry_.programID);
            }
            [XmlAttribute("isLiveSports"), DataMember(EmitDefaultValue =false), DefaultValue(false)]
            public bool isLiveSports
            {
                get { return isLive && GetProgram().entityType == ProgramCache.EntityType.Sports; }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("isTape"), DataMember(EmitDefaultValue =false), DefaultValue(false)]
            public bool isTape
            {
                get { return sdProgramEntry_.IsTape(); }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("isDelay"), DataMember(EmitDefaultValue =false), DefaultValue(false)]
            public bool isDelay
            {
                get { return sdProgramEntry_.IsDelay(); }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("isSubtitled"), DataMember(EmitDefaultValue = false), DefaultValue(false)]
            public bool isSubtitled
            {
                get { return sdProgramEntry_.IsSubtitled(); }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("isPremiere"), DataMember(EmitDefaultValue = false), DefaultValue(false)]
            public bool isPremiere
            {
                get { return sdProgramEntry_.IsPremiere(); }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("isFinale"), DataMember(EmitDefaultValue = false), DefaultValue(false)]
            public bool isFinale
            {
                get { return sdProgramEntry_.IsFinale(); }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("isInProgress"), DataMember(EmitDefaultValue = false), DefaultValue(false)]
            public bool isInProgress
            {
                get { return sdProgramEntry_.joinedInProgress; }
                set { throw new NotImplementedException(); }
            }

            // Can't find this property in SchedulesDirect data.  Interestingly it isn't in the MXF export from Rovi either.
            [XmlAttribute("isSap"), DataMember(EmitDefaultValue =false), DefaultValue(false)]
            public bool isSap
            {
                get { return false; }  
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("isBlackout"), DataMember(EmitDefaultValue =false), DefaultValue(false)]
            public bool isBlackout
            {
                get { return sdProgramEntry_.subjectToBlackout; }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("isEnhanced"), DataMember(EmitDefaultValue = false), DefaultValue(false)]
            public bool isEnhanced
            {
                get { return sdProgramEntry_.IsEnhanced(); }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("is3D"), DataMember(EmitDefaultValue =false), DefaultValue(false)]
            public bool is3D
            {
                get { return sdProgramEntry_.Is3D(); }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("isLetterbox"), DataMember(EmitDefaultValue =false), DefaultValue(false)]
            public bool isLetterbox
            {
                get { return sdProgramEntry_.IsLetterBox(); }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("isHdtv"), DataMember(EmitDefaultValue =false), DefaultValue(false)]
            public bool isHdtv
            {
                get { return sdProgramEntry_.IsHD(); }
                set { throw new NotImplementedException(); }
            }

            // Not found in either SchedulesDirect data or the MXF exported from Rovi guide
            [XmlAttribute("isHdtvSimulcast"), DataMember(EmitDefaultValue =false), DefaultValue(false)]
            public bool isHdtvSimulcast
            {
                get { return false; }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("isDvs"), DataMember(EmitDefaultValue =false), DefaultValue(false)]
            public bool isDvs
            {
                get { return sdProgramEntry_.IsDVS(); }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("part"), DataMember(EmitDefaultValue =false), DefaultValue(0)]
            public int part
            {
                get { return sdProgramEntry_.multipart?.partNumber ?? 0; }
                set { throw new NotImplementedException(); }
            }

            [XmlAttribute("parts"), DataMember(EmitDefaultValue =false), DefaultValue(0)]
            public int parts
            {
                get { return sdProgramEntry_.multipart?.totalParts ?? 0; }
                set { throw new NotImplementedException(); }
            }

            private SDStationScheduleProgramEntry sdProgramEntry_;
            bool isFirstInGroup_;
        }
    }
}
