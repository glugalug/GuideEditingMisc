using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace SchedulesDirectGrabber
{
    class JSONClient
    {
        public static T GetJSONResponse<T>(string url, object json_request = null, string token=null,
            string command = null, string output_debug_file=null) where T : new()
        {
            HttpWebResponse response = GetHttpResponse(url, json_request, token, command);
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string response_string = reader.ReadToEnd();
                if (output_debug_file != null)
                    File.WriteAllText(output_debug_file, response_string);
                try
                {
                    return JsonConvert.DeserializeObject<T>(response_string, deserializer_settings);
                } catch(Exception ex)
                {
                    Console.WriteLine("Failed to parse JSON response: " + response_string);
                    Misc.OutputException(ex);
                    throw;
                }
            }
        }

        public static IDictionary<string, object> GetGenericJSONResponse(string url, object json_request, string token=null)
        {
            HttpWebResponse response = GetHttpResponse(url, json_request, token);
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string response_string = reader.ReadToEnd();
                return (IDictionary<string, object>)generic_serializer.Deserialize<dynamic>(response_string);
            }
        }

        public static Dictionary<string, T> GetJSONResponseDictionary<T>(string url, object json_request, string token= null)
        {
            IDictionary<string, object> generic_response = GetGenericJSONResponse(url, json_request, token);
            Dictionary<string, T> result = new Dictionary<string, T>();
            foreach(string key in generic_response.Keys)
            {
                if (generic_response[key] != null)
                    result[key] = Deserialize<T>(Serialize(generic_response[key]));
                else result[key] = default(T);
            }
            return result;
        }

        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, deserializer_settings);
        }

        private static JsonSerializerSettings deserializer_settings_ = null;
        private static JsonSerializerSettings deserializer_settings {
            get
            {
                if (deserializer_settings_ == null)
                {
                    deserializer_settings_ = new JsonSerializerSettings();
                    deserializer_settings_.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
                    deserializer_settings_.NullValueHandling = NullValueHandling.Ignore;
                    deserializer_settings_.ObjectCreationHandling = ObjectCreationHandling.Reuse;
                    deserializer_settings_.MissingMemberHandling = MissingMemberHandling.Error;
                }
                return deserializer_settings_;
            }
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, deserializer_settings);
        }

        public static HttpWebResponse GetHttpResponse(string url, object json_request, string token = null,
            string command=null)
        {
            HttpWebRequest http_request = BuildRequest(url, json_request, token, command);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)http_request.GetResponse();
            }
            catch (WebException we)
            {
                Misc.OutputException(we);
                Console.WriteLine("WebException response: " + we.Response);
                if (we.Response is HttpWebResponse)
                {
                    HttpWebResponse hrw = we.Response as HttpWebResponse;
                    Console.WriteLine("Status Code: " + hrw.StatusCode);
                }
                throw new Exception("WebException returned from JSON request, response:" + we.Response, we);
            }
            catch (Exception e)
            {
                Misc.OutputException(e);
                throw new Exception("Unexpected error returned from JSON request", e);
            }
            return response;
        }

        private static HttpWebRequest BuildRequest(string url, object json_request, string token = null, string command=null)
        {
            HttpWebRequest http_request = (HttpWebRequest)WebRequest.Create(url);
            http_request.UserAgent = kUserAgent;
            http_request.AutomaticDecompression = DecompressionMethods.Deflate;
            if (!string.IsNullOrEmpty(token)) http_request.Headers.Add("token", token);
            if (json_request == null || json_request is EmptyRequest)
            {
                http_request.Method = "GET";
            }
            else
            {
                http_request.Method = "POST";
                byte[] bytes = Encoding.UTF8.GetBytes(Serialize(json_request));
                http_request.ContentLength = bytes.Length;
                var request_stream = http_request.GetRequestStream();
                request_stream.Write(bytes, 0, bytes.Length);
                request_stream.Close();
            }
            if (command != null) http_request.Method = command;
            return http_request;
        }

        [DataContract]
        public class EmptyRequest { }

        public static EmptyRequest empty_request = new EmptyRequest();

        public static void DisplayJSON(object json_object)
        {
            Console.WriteLine(Serialize(json_object));
        }

        private static JavaScriptSerializer generic_serializer = new JavaScriptSerializer();

        private const string kUserAgent = "Glugglug's Media Center SchedulesDirect Importer";
    }

}
