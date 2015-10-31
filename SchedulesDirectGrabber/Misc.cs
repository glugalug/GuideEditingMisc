using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SchedulesDirectGrabber
{
    class Misc
    {
        public static void OutputException(Exception e)
        {
            Console.WriteLine(e.ToString());
            Console.WriteLine("Exception type: {0}, message: {1}", e.GetType().Name, e.Message);
            Console.WriteLine("Stack trace: {0}", e.StackTrace);
            if (e.Data != null)
            {
                Console.WriteLine("Exception data:");
                foreach(var key in e.Data.Keys)
                {
                    Console.WriteLine("data[{0}]=={1}", key, e.Data[key]);
                }
            }
            if (e.InnerException != null)
            {
                Console.Write("Inner exception: ");
                OutputException(e.InnerException);
            }
        }

        public static Properties.Settings default_settings { get { return Properties.Settings.Default; } }

        public static Image LoadImageFromURL(string url)
        {
            try {
                using (WebClient wc = new WebClient())
                {
                    byte[] bytes = wc.DownloadData(url);
                    MemoryStream ms = new MemoryStream(bytes);
                    Image img = Image.FromStream(ms);
                    return img;
                }
            } catch  {
                return null;
            }
        }

        public static string LimitString(string s, int maxLength)
        {
            return (s== null) ? null : (s.Length > maxLength) ? s.Substring(0, maxLength) : s;
        }
    }

    public static class DictionaryExtensions
    {
        public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dic,
            Func<TValue, bool> predicate)
        {
            var keys = dic.Keys.Where(k => predicate(dic[k])).ToList();
            foreach (var key in keys)
            {
                dic.Remove(key);
            }
        }
   }
}
