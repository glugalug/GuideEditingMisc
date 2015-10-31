using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SchedulesDirectGrabber
{
    public class SDAccountManagement
    {
        public static void AddLineupToAccount(string lineup)
        {
            LineupSubscriptionChangeReponse response = JSONClient.GetJSONResponse<LineupSubscriptionChangeReponse>(
                UrlBuilder.BuildWithAPIPrefix("/lineups/" + lineup), null, SDTokenManager.token_manager.token, "PUT");
            if (!response.Succeeded())
            {
                throw new Exception("Failed to add lineup to account!");
            }
        }

        internal static void RemoveLineupFromAccount(string lineup)
        {
            LineupSubscriptionChangeReponse response = JSONClient.GetJSONResponse<LineupSubscriptionChangeReponse>(
                UrlBuilder.BuildWithAPIPrefix("/lineups/" + lineup), null, SDTokenManager.token_manager.token, "DELETE");
            if (!response.Succeeded())
            {
                throw new Exception("Failed to remove lineup from account!");
            }
        }
    }

    [DataContract]
    public class LineupSubscriptionChangeReponse
    {
        [DataMember(Name = "response", IsRequired = true)]
        public string response { get; set; }
        [DataMember(Name = "code")]
        public int code { get; set; }
        [DataMember(Name = "serverID")]
        public string serverID { get; set; }
        [DataMember(Name = "message")]
        public string message { get; set; }
        [DataMember(Name = "changesRemaining")]
        public int changesRemaining { get; set; }
        [DataMember(Name = "datetime")]
        public DateTime datetime { get; set; }

        public bool Succeeded() { return response == "OK"; }
    }

}
