using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;

namespace SchedulesDirectGrabber
{
    public class SDLineupSelection
    {
        public static SubscribedLineupsResponse GetSubscribedLineups()
        {
            return JSONClient.GetJSONResponse<SubscribedLineupsResponse>(UrlBuilder.BuildWithAPIPrefix("/lineups"),
                null, SDTokenManager.token_manager.token);
        }

        public static List<Country> GetCountryListFromUri(string uri)
        {
            Dictionary<string, Country[]> countries_by_region = JSONClient.GetJSONResponse<Dictionary<string, Country[]>>(
                UrlBuilder.BuildWithBasePrefix(uri), JSONClient.empty_request);
            List<Country> countries = new List<Country>();
            foreach (Country[] region_countries in countries_by_region.Values)
                countries.AddRange(region_countries);
            return countries;
        }

        public static List<Country> supported_countries { get
            {
                if (supported_countries_ == null)
                {
                    supported_countries_ = GetCountryListFromUri(String.Format("/{0}/available/countries",
                        Properties.Settings.Default.APIVersion));
                }
                return supported_countries_;
            }
        }

        public static List<SDLineup> GetLineupsForZip(SDTokenManager token_manager, string country, string zip)
        {
            string url = String.Format(UrlBuilder.BuildWithAPIPrefix("/headends?country={0}&postalcode={1}"), country, zip);
            List<HeadEnd> headends = JSONClient.GetJSONResponse<List<HeadEnd>>(url, JSONClient.empty_request, token_manager.token);
            List<SDLineup> lineups = new List<SDLineup>();
            foreach(HeadEnd headend in headends)
            {
                if (headend.lineups != null)
                {
                    foreach (SDLineup lineup in headend.lineups)
                    {
                        lineup.transport = headend.transport;
                    }
                    lineups.AddRange(headend.lineups.ToArray());
                }
            }
            return lineups;
        }

        static List<Country> supported_countries_ = null;
    }
    [DataContract]
    class CountryListUrlInfo
    {
        [DataMember(Name = "type")]
        public string type { get; set; }
        [DataMember(Name = "description")]
        public string description { get; set; }
        [DataMember(Name = "uri", IsRequired = true)]
        public string uri { get; set; }

        public string url { get { return UrlBuilder.BuildWithBasePrefix(uri); } }
    }

    [DataContract]
    public class Country
    {
        [DataMember(Name = "fullName")]
        public string fullName { get; set; }
        [DataMember(Name = "shortName")]
        public string shortName { get; set; }
        [DataMember(Name = "postalCodeExample")]
        public string postalCodeExample { get; set; }
        [DataMember(Name = "postalCode", IsRequired = true)]
        public string postalCode { get; set; }
        [DataMember(Name = "onePostalCode")]
        public bool onePostalCode { get; set; }

        public override string ToString()
        {
            return fullName;
        }

        public string postalCodeRegex
        {
            get
            {
                string regex = postalCode;
                if (string.IsNullOrEmpty(regex)) return string.Empty;
                if (regex[0] == '/') regex = '^' + regex.Substring(1);
                if (regex[regex.Length - 1] == '/') regex = regex.Substring(0, regex.Length - 1) + '$';
                return regex;
            }
        }
    }

    [DataContract]
    public class HeadEnd
    {
        [DataMember(Name = "headend")]
        public string headend { get; set; }
        [DataMember(Name = "transport")]
        public string transport { get; set; }
        [DataMember(Name = "location")]
        public string location { get; set; }
        [DataMember(Name = "lineups")]
        public SDLineup[] lineups { get; set; }
    }

    [DataContract]
    public class SDLineup
    {
        [DataMember(Name = "name")]
        public string name { get; set; }
        [DataMember(Name = "lineup", IsRequired = true)]
        public string lineup { get; set; }
        [DataMember(Name = "uri", IsRequired = true)]
        public string uri { get; set; }
        [DataMember(Name = "transport")]
        public string transport { get; set; }
        [DataMember(Name = "location")]
        public string location { get; set; }
        [DefaultValue(false)]
        [DataMember(Name = "isDeleted")]
        public bool isDeleted { get; set; }

        public override string ToString()
        {
            if (isDeleted) return "DELETED: " + name;
            return String.Format("{0} - {2} ({1})", name, lineup, transport);
        }
    }

    [DataContract]
    public class SubscribedLineupsResponse
    {
        [DataMember(Name = "code")]
        public int code { get; set; }
        [DataMember(Name = "serverID")]
        public string serverID { get; set; }
        [DataMember(Name = "datetime")]
        public DateTime datetime { get; set; }
        [DataMember(Name = "lineups")]
        public SDLineup[] lineups { get; set; }

        public SDLineup[] GetActiveLineups()
        {
            List<SDLineup> active_lineups = new List<SDLineup>();
            foreach (var lineup in lineups)
                active_lineups.Add(lineup);
            return active_lineups.ToArray();
        }
    }
}
