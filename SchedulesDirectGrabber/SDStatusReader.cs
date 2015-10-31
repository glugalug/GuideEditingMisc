using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SchedulesDirectGrabber
{
    public class SDStatusReader
    {
        public SDStatusReader(SDTokenManager token_manager)
        {
            token_manager_ = token_manager;
        }

        public StatusResponse GetSchedulesDirectStatus()
        {
            StatusResponse status_response = JSONClient.GetJSONResponse<StatusResponse>(kStatusRequestUrl, new JSONClient.EmptyRequest(),
                token_manager_.token);
            return status_response;
        }

        private static string kStatusRequestUrl = UrlBuilder.BuildWithAPIPrefix("/status");
        private SDTokenManager token_manager_;
    }

    [DataContract]
    public class AccountStatus
    {
        [DataMember(Name = "expires")]
        public string expires { get; set; }
        [DataMember(Name = "messages")]
        public string[] messages { get; set; }
        [DataMember(Name = "maxLineups")]
        public int maxLineups { get; set; }
        [DataMember(Name = "nextSuggestedConnectTime")]
        public DateTime nextSuggestedConnectTime { get; set; }
    }
    [DataContract]
    public class LineupStatus
    {
        [DataMember(Name = "lineup")]
        public string lineup { get; set; }
        // Must be a string, not a DateTime because modified time of "Z" for deleted lineups will fail to parse.
        [DataMember(Name = "modified")]  
        public string modified { get; set; }
        [DataMember(Name = "uri")]
        public string uri { get; set; }
        [DataMember(Name = "isDeleted")]
        public bool isDeleted { get; set; }
    }

    [DataContract]
    public class SystemStatus
    {
        [DataMember(Name = "date")]
        public DateTime date { get; set; }
        [DataMember(Name = "status")]
        public string status { get; set; }  // "Online" or "Offline"
        [DataMember(Name = "details")]
        public string details { get; set; }
        [DataMember(Name = "message")]
        public string message { get; set; }
    }

    [DataContract]
    public class StatusResponse
    {
        [DataMember(Name = "account")]
        public AccountStatus account { get; set; }
        [DataMember(Name = "lineups")]
        public LineupStatus[] lineups { get; set; }
        [DataMember(Name = "lastDataUpdate")]
        public string lastDataUpdate { get; set; }
        [DataMember(Name = "notifications")]
        public string[] notifications { get; set; }
        [DataMember(Name = "systemStatus")]
        public SystemStatus[] systemStatus { get; set; }
        [DataMember(Name = "serverID")]
        public string serverID { get; set; }
        [DataMember(Name = "code")]
        public int code { get; set; }

        public bool IsOnline()
        {
            foreach (var status in systemStatus)
            {
                if (status.status == "Online") return true;
            }
            return false;
        }

        public List<LineupStatus> GetActiveLineups()
        {
            List<LineupStatus> active_lineups = new List<LineupStatus>();
            foreach (var lineup_status in lineups)
            {
                if (!lineup_status.isDeleted) active_lineups.Add(lineup_status);
            }
            return active_lineups;
        }
    }
}
