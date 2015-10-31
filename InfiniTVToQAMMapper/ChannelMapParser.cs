using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using System.Xml;
using Microsoft.MediaCenter.TV.Tuning;
using Microsoft.MediaCenter.Guide;

namespace InfiniTVToQAMMapper
{
    internal class ChannelMapEntry: IComparable
    {
        private enum HtmlColumnIndex
        {
            Empty,
            ChannelNumber,
            Callsign,
            Modulation,
            Frequency,
            ProgramId,
            EIA
        };

        public ChannelMapEntry(int number, string callsign, ModulationType modulation, ChannelNumber physicalChannel)
        {
            number_ = number;
            callsign_ = callsign;
            modulation_ = modulation;
            physical_channel_ = physicalChannel;
        }
        public int Number { get { return number_; } }
        public string Callsign { get { return callsign_; } }
        public ModulationType Modulation { get { return modulation_; } }
        public ChannelNumber PhysicalChannel { get { return physical_channel_; } }
        public bool HasUnsupportedModulation() { return modulation_ == ModulationType.BDA_MOD_MAX; }
        public const ModulationType kUnsupportedModulation = ModulationType.BDA_MOD_MAX;
        private int number_;
        private string callsign_;
        private ModulationType modulation_;
        private ChannelNumber physical_channel_;

        int IComparable.CompareTo(object obj)
        {
            ChannelMapEntry other_entry = (ChannelMapEntry)obj;
            return number_.CompareTo(other_entry.number_);
        }
    }
    
    internal class ChannelMapParser
    {
        private static IPAddress GetInfiniTVIP()
        {
            Console.WriteLine("Finding InfiniTV network adapter");
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            NetworkInterface infiniTV_interface = null;
            foreach (NetworkInterface network_interface in interfaces)
            {
                if (network_interface.Description.Contains("InfiniTV")) {
                    infiniTV_interface = network_interface;
                    break;
                }
            }
            if (null == infiniTV_interface)
            {
                System.Windows.Forms.MessageBox.Show("No InfiniTV card found on this PC.  Networked tuners not yet supported.");
                return null;
            }
            IPInterfaceProperties ip_properties = infiniTV_interface.GetIPProperties();
            return ip_properties.DhcpServerAddresses.First();
        }

        internal static string DownloadXmlChannelMap()
        {
            IPAddress ip_address = GetInfiniTVIP();
            Console.WriteLine("Downloading XML channel map from InfiniTV");
            if (ip_address == null) return null;
            string request_url = string.Format(@"http://{0}/view_channel_map.cgi?page=0&xml=1", ip_address.ToString());
            return DownloadUrl(request_url);
        }

        internal static string DownloadUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream response_stream = response.GetResponseStream();
            byte[] buf = new byte[8192];
            StringBuilder sb = new StringBuilder();
            int count = 0;
            do
            {
                count = response_stream.Read(buf, 0, buf.Length);
                if (count != 0)
                    sb.Append(Encoding.ASCII.GetString(buf, 0, count));
            } while (count > 0);
            return sb.ToString();
        }

        internal static string DownloadHtmlChannelMap()
        {
            IPAddress ip_address = GetInfiniTVIP();
            Console.WriteLine("Downloading HTML channel map from InfiniTV");
            if (ip_address == null) return null;
            string request_url = string.Format("http://{0}/view_channel_map.cgi?page=0", ip_address.ToString());
            return DownloadUrl(request_url);
        }

        private enum HtmlColumnIndex
        {
            Empty,
            ChannelNumber,
            Callsign,
            Modulation,
            Frequency,
            ProgramId,
            EIA
        };

        private static XmlElement CreateSubElement(XmlElement parent, string subelement_name)
        {
            XmlElement sub_element = parent.OwnerDocument.CreateElement(subelement_name);
            parent.AppendChild(sub_element);
            return sub_element;
        }
        internal static XmlDocument HtmlChannelMapToXml(string html_channel_map)
        {
            Console.WriteLine("Converting channel map to XML");
            XmlDocument doc = new XmlDocument();
            XmlElement channels_elem = (XmlElement)doc.AppendChild(doc.CreateElement("channels"));
            string[] table_rows = html_channel_map.Split(new string[] {"<tr>"}, StringSplitOptions.None);
            // skip the first 2 "rows" for the stuff before the table and the headers.
            string[] col_separator_param = new string[] { "<td>" };
            for (int row_index = 2; row_index < table_rows.Length; ++row_index)
            {
                string row = table_rows[row_index];
                string[] columns = row.Split(col_separator_param, StringSplitOptions.None);
                // remove </td> and anything after it from each column, along with whitespace.
                for (int col_index = 0; col_index < columns.Length; ++col_index) {
                    string column = columns[col_index];
                    int column_end_pos = column.IndexOf("</td>");
                    if (column_end_pos >= 0)
                    {
                        column = column.Remove(column_end_pos);
                    }
                    columns[col_index] = column.Trim();
                }
                XmlElement channel_elem = CreateSubElement(channels_elem, "channel");
                XmlElement name_elem = CreateSubElement(channel_elem, "name");
                name_elem.InnerText = Convert.ToBase64String(Encoding.UTF8.GetBytes(columns[(int)HtmlColumnIndex.Callsign]));
                XmlElement number_elem = CreateSubElement(channel_elem, "number");
                number_elem.InnerText = columns[(int)HtmlColumnIndex.ChannelNumber];
                XmlElement modulation_elem = CreateSubElement(channel_elem, "modulation");
                modulation_elem.InnerText = columns[(int)HtmlColumnIndex.Modulation];
                XmlElement frequency_elem = CreateSubElement(channel_elem, "frequency");
                frequency_elem.InnerText = columns[(int)HtmlColumnIndex.Frequency];
                XmlElement eia_elem = CreateSubElement(channel_elem, "eia");
                eia_elem.InnerText = columns[(int)HtmlColumnIndex.EIA];
            }
            return doc;
        }

        static internal Dictionary<int, ChannelMapEntry> ParseXmlChannelMap(XmlDocument xml_channel_map)
        {
            Console.WriteLine("Parsing XML channel map");
            Dictionary<int, ChannelMapEntry> channel_map = new Dictionary<int, ChannelMapEntry>();
            XmlNodeList channel_nodes = xml_channel_map.SelectNodes("channels/channel");
            foreach (XmlNode channel_node in channel_nodes)
            {
                string callsign = Encoding.UTF8.GetString(
                    Convert.FromBase64String(channel_node.SelectSingleNode("name/text()").Value));
                int number = int.Parse(channel_node.SelectSingleNode("number/text()").Value);
                string modulation_string = channel_node.SelectSingleNode("modulation/text()").Value;
                ModulationType modulation;
                switch (modulation_string)
                {
                    case "QAM256":
                        modulation = ModulationType.BDA_MOD_256QAM;
                        break;
                    case "QAM64":
                        modulation = ModulationType.BDA_MOD_64QAM;
                        break;
                    case "8VSB":
                        modulation = ModulationType.BDA_MOD_8VSB;
                        break;
                    default:
                        modulation = ModulationType.BDA_MOD_MAX;
                        break;
                }
                string physical_channel_string = channel_node.SelectSingleNode("eia/text()").Value;
                string[] physical_channel_parts = physical_channel_string.Split(new char[] { '.' });
                ChannelNumber physical_channel = new ChannelNumber(
                    int.Parse(physical_channel_parts[0]), int.Parse(physical_channel_parts[1]));
                ChannelMapEntry entry = new ChannelMapEntry(number, callsign, modulation, physical_channel);
                channel_map[number] = entry;
            }
            return channel_map;
        }

        static private Dictionary<int, ChannelMapEntry> channel_map_ = null;
        static internal Dictionary<int, ChannelMapEntry> channel_map
        {
            get
            {
                if (null == channel_map_) channel_map_ = GetChannelMap();
                return channel_map_;
            }
        }

        static private Dictionary<int, ChannelMapEntry> GetChannelMap()
        {
            XmlDocument channel_map_xml = new XmlDocument();
            try
            {
                string xml_channel_map_string = DownloadXmlChannelMap();
                channel_map_xml.LoadXml(xml_channel_map_string);
                XmlNodeList channel_nodes = channel_map_xml.SelectNodes("channels/channel");
                if (channel_nodes.Count == 0) throw new Exception("No channels found");
            }
            catch
            {
                channel_map_xml = ChannelMapParser.HtmlChannelMapToXml(ChannelMapParser.DownloadHtmlChannelMap());
            }
            return ChannelMapParser.ParseXmlChannelMap(channel_map_xml);
        }

    }
}
