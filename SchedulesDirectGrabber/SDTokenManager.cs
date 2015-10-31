using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization;

namespace SchedulesDirectGrabber
{
    public class SDTokenManager
    {
       public SDTokenManager(string username, string pwhash)
        {
            username_ = username;
            pwhash_ = pwhash;
        }

        public static SDTokenManager token_manager { get { return token_manager_; } }
        private static SDTokenManager token_manager_;

        public static string HashPassword(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = SHA1.Create().ComputeHash(bytes);
            return HexStringFromBytes(hashBytes);
        }

        private static readonly string kTokenRequestUrl = UrlBuilder.BuildWithAPIPrefix("/token");

        public string token {  get { if (string.IsNullOrEmpty(token_)) return GetNewToken(); else return token_; } }

        public void RefreshToken() { GetNewToken(); }

        public double GetTokenHours() { return DateTime.Now.Subtract(last_updated_).TotalHours; }

        private string GetNewToken()
        {
            TokenRequest token_request = new TokenRequest(username_, pwhash_);
            TokenResponse token_response = JSONClient.GetJSONResponse<TokenResponse>(kTokenRequestUrl, token_request);
            switch(token_response.code)
            {
                case 0:
                    token_ = token_response.token;
                    Console.WriteLine("Successfully requested access token: {0}", token_);
                    last_updated_ = DateTime.Now;
                    token_manager_ = this;
                    return token_;
                case 3000:
                    throw new ServerDownException(token_response.response_code, token_response.message);
                default:
                    throw new Exception("Unrecognized error - bad password? response_code:" + token_response.response_code +
                        " message: " + token_response.message + " code: " + token_response.code);
            }
        }

        private static string HexStringFromBytes(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        [DataContract]
        private class TokenRequest
        {
            public TokenRequest(string username, string password_hash)
            {
                this.username = username;
                this.password_hash = password_hash;
            }
            [DataMember(Name = "username", EmitDefaultValue = false)]
            public string username { get; set; }

            [DataMember(Name = "password", EmitDefaultValue = false)]
            public string password_hash { get; set; }
        }

        [DataContract]
        private class TokenResponse
        {
            [DataMember(Name = "code", EmitDefaultValue = false)]
            public int code { get; set; }

            [DataMember(Name = "message", EmitDefaultValue = false)]
            public string message { get; set; }

            [DataMember(Name = "serverID", EmitDefaultValue = false)]
            public string server_id { get; set; }

            [DataMember(Name = "token", EmitDefaultValue = false)]
            public string token { get; set; }

            [DataMember(Name = "response", EmitDefaultValue = false)]
            public string response_code { get; set; }

            [DataMember(Name = "datetime", EmitDefaultValue = false)]
            public DateTime date_time { get; set; }
        }

        public class ServerDownException : Exception
        {
            public ServerDownException(string response, string message)
                : base("Server is currently down. response:" + response + " message: " + message)
            { }
        }

        private string username_;
        private string pwhash_;
        private string token_;
        private DateTime last_updated_;
    }
}
