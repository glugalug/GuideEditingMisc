using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulesDirectGrabber
{
    public class UrlBuilder
    {
        private static readonly string kApiVersion = Properties.Settings.Default.APIVersion;
        private const string kUrlBase = "https://json.schedulesdirect.org";

        public static string BuildWithAPIPrefix(string uri) {
            return String.Format("{0}/{1}{2}", kUrlBase, kApiVersion, uri);
        }

        public static string BuildWithBasePrefix(string uri)
        {
            return kUrlBase + uri;
        }
    }
}
