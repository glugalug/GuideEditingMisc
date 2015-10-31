using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace XMLGuideImporter
{
    public partial class ImportConfigForm : Form
    {
        public ImportConfigForm()
        {
            InitializeComponent();
            if (XMLTVPathTextBox.Text != "") LoadXml();
        }

        private void GrabXMLButton_Click(object sender, EventArgs e)
        {
            GrabXML();
        }

        private bool GrabXML()
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = CommandLineTextBox.Text;
                p.StartInfo.Arguments = GrabberArgumentsTextBox.Text;
                p.Start();
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    ShowError("XML grab failed with error code " + p.ExitCode);
                    return false;
                }
            }
            return true;
        }

        private void GrabberBrowseButton_Click(object sender, EventArgs e)
        {
            string path = CommandLineTextBox.Text;
            if (ChooseFile(ref path))
            {
                CommandLineTextBox.Text = path;
                SaveXmlInputSettings();
            }
        }

        private Properties.Settings default_settings { get { return Properties.Settings.Default; } }
        private void SaveXmlInputSettings()
        {
            default_settings.GrabberCommandLine = CommandLineTextBox.Text;
            default_settings.GrabberArguments = GrabberArgumentsTextBox.Text;
            default_settings.XMLTVPath = XMLTVPathTextBox.Text;
            default_settings.Save();
        }

        private void SaveParsingSettings()
        {
            default_settings.ChannelNumberXPath = ChannelNumberXPathTextBox.Text;
            default_settings.CallsignXPath = CallsignXPathTextBox.Text;
            default_settings.ServiceNameXPath = ServiceNameXPathTextBox.Text;
            default_settings.AffiliateXPath = AffiliateTextBox.Text;
            default_settings.Save();
        }

        private bool ChooseFile(ref string path, string extension="")
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                if (extension=="")
                {
                    dialog.DefaultExt = extension;
                }
                dialog.FileName = path;
                dialog.CheckFileExists = true;
                DialogResult dr = dialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    path = dialog.FileName;
                    return true;
                }
            }
            return false;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            string path = XMLTVPathTextBox.Text;
            if (ChooseFile(ref path))
            {
                XMLTVPathTextBox.Text = path;
                SaveXmlInputSettings();
            }
        }

        private void LoadXml()
        {
            try
            {
                Console.Write("Loading XML from " + XMLTVPathTextBox.Text + " ... ");
                input_xml_.Load(XMLTVPathTextBox.Text);
                Console.WriteLine("Done");
            }
            catch (Exception exc)
            {
                ShowError("Failed to load XML from " + XMLTVPathTextBox.Text + " exception: " + DescribeException(exc));
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            LoadXml();
        }

        private XmlDocument input_xml_ = new XmlDocument();
        private void ShowError(string errormessage)
        {
            Console.WriteLine(errormessage);
            if (this.Visible)
            {
                MessageBox.Show(errormessage);
            }
        }

        private void GrabberArgumentsTextBox_Leave(object sender, EventArgs e)
        {
            SaveXmlInputSettings();
        }

        private void XMLTVPathTextBox_Leave(object sender, EventArgs e)
        {
            SaveXmlInputSettings();
        }

        private void CommandLineTextBox_Leave(object sender, EventArgs e)
        {
            SaveXmlInputSettings();
        }

        private XmlNodeList GetChannelNodes()
        {
            return input_xml_.DocumentElement.SelectNodes("channel");
        }

        private List<string> TestXPathOnChannels(string xpath)
        {
            XmlNodeList channel_nodes = GetChannelNodes();
            Console.WriteLine(channel_nodes.Count + " channel elements found");
            List<string> xpath_matches = new List<string>();
            try
            {
                foreach (XmlElement node in channel_nodes)
                {
                    string match = node.SelectSingleNode(xpath)?.Value;
                    if (match != null) xpath_matches.Add(match);
                }
            }
            catch (Exception exc)
            {
                ShowError("Error extracting values with xpath string " + xpath + " exception: " + DescribeException(exc));
            }
            return xpath_matches;
        }
        private string DescribeException(Exception exc)
        {
            return "Message: " + exc.Message + " StackTrace: " + exc.StackTrace;
        }
        private void ChannelNumberTestButton_Click(object sender, EventArgs e)
        {
            TestMatchingListBox.Items.Clear();
            TestMatchingListBox.Items.AddRange(TestXPathOnChannels(ChannelNumberXPathTextBox.Text).ToArray());
        }

        private void CallsignTestButton_Click(object sender, EventArgs e)
        {
            TestMatchingListBox.Items.Clear();
            TestMatchingListBox.Items.AddRange(TestXPathOnChannels(CallsignXPathTextBox.Text).ToArray());
        }

        private void ServiceNameXPathTestButton_Click(object sender, EventArgs e)
        {
            TestMatchingListBox.Items.Clear();
            TestMatchingListBox.Items.AddRange(TestXPathOnChannels(ServiceNameXPathTextBox.Text).ToArray());
        }

        private void ChannelNumberRevertButton_Click(object sender, EventArgs e)
        {
            ChannelNumberXPathTextBox.Text = default_settings.ChannelNumberXPath;
        }

        private void CallsignRevertButton_Click(object sender, EventArgs e)
        {
            CallsignRevertButton.Text = default_settings.CallsignXPath;
        }

        private void ServiceNameXPathRevertButton_Click(object sender, EventArgs e)
        {
            ServiceNameXPathRevertButton.Text = default_settings.ServiceNameXPath;
        }

        private XmlDocument mxf_document_ = new XmlDocument();

        private XmlElement CreateSubElement(XmlElement parent, string name)
        {
            return (XmlElement)parent.AppendChild(parent.OwnerDocument.CreateElement(name));
        }

        private string GetAttributeWithDefault(XmlElement element, string attribute, string default_value="")
        {
            if (element.HasAttribute(attribute)) return element.GetAttribute(attribute);
            return default_value;
        }

        private Dictionary<string, string> guide_image_ids_ = new Dictionary<string, string>();

        private void ClearGuideImageIndex()
        {
            guide_image_ids_.Clear();
        }

        private string GetIdForGuideImageUrlUpdatingXml(string url, ref XmlElement guide_images_element)
        {
            if (guide_image_ids_.ContainsKey(url)) return guide_image_ids_[url];
            int index = guide_image_ids_.Count + 1;
            string id = "g" + index;
            guide_image_ids_[url] = id;
            XmlElement guide_image_element = CreateSubElement(guide_images_element, "GuideImage");
            guide_images_element.SetAttribute("id", id);
            guide_images_element.SetAttribute("uid", "!Image!" + url);
            guide_images_element.SetAttribute("imageUrl", url);
            return id;
        }

        private void MXFBenerationTestButton_Click(object sender, EventArgs e)
        {
            const string kProviderId = "MyProviderId";
            const string kLineupId = "l1";
            string source_info = "Source: " + GetAttributeWithDefault(input_xml_.DocumentElement, "source-info-name") +
                " Generator" + GetAttributeWithDefault(input_xml_.DocumentElement, "generator-info-name");
            mxf_document_ = new XmlDocument();
            XmlElement document_element = (XmlElement)mxf_document_.AppendChild(mxf_document_.CreateElement("MXF"));
            XmlElement providers_element = CreateSubElement(document_element, "Providers");
            XmlElement provider_element = CreateSubElement(providers_element, "Provider");
            provider_element.SetAttribute("id", kProviderId);
            provider_element.SetAttribute("name", "glugglug's Guide Importer Provider "+ source_info);
            provider_element.SetAttribute("displayName", "glugglug's Guide Importer" + source_info);
            XmlElement with_element = CreateSubElement(document_element, "With");
            with_element.SetAttribute("provider", kProviderId);
            XmlElement keywords_element = CreateSubElement(with_element, "Keywords");
            XmlElement keyword_groups_element = CreateSubElement(with_element, "KeywordGroups");
            XmlElement guide_images_element = CreateSubElement(with_element, "GuideImages");
            XmlElement people_element = CreateSubElement(with_element, "People");
            XmlElement series_infos_element = CreateSubElement(with_element, "SeriesInfos");
            XmlElement seasons_element = CreateSubElement(with_element, "Seasons");
            XmlElement programs_element = CreateSubElement(with_element, "Programs");
            XmlElement affiliates_element = CreateSubElement(with_element, "Affiliates");
            XmlElement services_element = CreateSubElement(with_element, "Services");
            XmlElement schedule_entries_element = CreateSubElement(with_element, "ScheduleEntries");
            XmlElement lineups_element = CreateSubElement(with_element, "Lineups");
            XmlElement lineup_element = CreateSubElement(lineups_element, "Lineup");
            lineup_element.SetAttribute("id", kLineupId);
            lineup_element.SetAttribute("uid", "!Lineup!" + MxfLineupUidTextBox.Text);
            lineup_element.SetAttribute("name", MxfLineupNameTextBox.Text);
            lineup_element.SetAttribute("primaryProvider", "!MCLineup!MainLineup");
            XmlElement channels_element = CreateSubElement(lineup_element, "channels");
            XmlNodeList input_channel_nodes = GetChannelNodes();
            int service_index = 0;
            ClearGuideImageIndex();
            HashSet<string> affiliates = new HashSet<string>();
            foreach (XmlElement input_channel in input_channel_nodes)
            {
                ++service_index;
                string channel_number_str = input_channel.SelectSingleNode(ChannelNumberXPathTextBox.Text)?.Value;
                string[] channel_number_parts = channel_number_str.Split(new char[] { '-', '.', '_' });
                int channel_number = -1, channel_sub_number = 0;
                if (channel_number_parts.Count() == 0 || !int.TryParse(channel_number_parts[0], out channel_number))
                {
                    channel_number = -1;
                }
                if (channel_number_parts.Count() < 2 || !int.TryParse(channel_number_parts[1], out channel_sub_number))
                {
                    channel_sub_number = 0;
                }
                string callsign = input_channel.SelectSingleNode(CallsignXPathTextBox.Text)?.Value;
                string service_name = input_channel.SelectSingleNode(ServiceNameXPathTextBox.Text)?.Value;
                string affiliate = input_channel.SelectSingleNode(AffiliateTextBox.Text)?.Value;
                string service_id = "s" + service_index;
                string service_uid = "!Service!" + input_channel.GetAttribute("id");
                XmlElement channel_element = CreateSubElement(channels_element, "Channel");
                XmlElement service_element = CreateSubElement(services_element, "Service");
                service_element.SetAttribute("id", service_id);
                service_element.SetAttribute("uid", input_channel.GetAttribute("id"));
                service_element.SetAttribute("name", service_name);
                service_element.SetAttribute("callSign", callsign);
                XmlNode image_node = input_channel.SelectSingleNode("icon/@src");
                string image_url;
                if (image_node != null) { 
                    image_url = input_channel.SelectSingleNode("icon/@src")?.Value;
                    if (image_url?.Length > 0)
                    {
                        service_element.SetAttribute("logoImage", GetIdForGuideImageUrlUpdatingXml(image_url, ref guide_images_element));
                    }
                }
                if (affiliate?.Length > 0) {
                    string affiliate_uid = "!Affiliate!" + affiliate;
                    service_element.SetAttribute("affiliate", affiliate_uid);
                    if (!affiliates.Contains(affiliate))
                    {
                        XmlElement affiliate_element = CreateSubElement(affiliates_element, "Affiliate");
                        affiliate_element.SetAttribute("name", affiliate);
                        affiliate_element.SetAttribute("uid",  affiliate_uid);
                        affiliates.Add(affiliate);
                    }
                }
                channel_element.SetAttribute("uid", "!Channel!" + MxfLineupUidTextBox.Text + '!' + 
                    channel_number + '_' + channel_sub_number);
                channel_element.SetAttribute("lineup", kLineupId);
                channel_element.SetAttribute("service", service_id);
                if (channel_number >= 0) channel_element.SetAttribute("number", channel_number.ToString());
                if (channel_sub_number > 0) channel_element.SetAttribute("subNumber", channel_sub_number.ToString());

                // TODO: rest of channel, start with service element
            }

            XmlNodeList programme_nodes = document_element.SelectNodes("programme");
            foreach(XmlElement input_program_element in programme_nodes)
            {
                XmlElement schedule_entry_element = CreateSubElement(schedule_entries_element, "ScheduleEntry");

            }
            MXFPreviewText.Text = mxf_document_.OuterXml;
        }

        private void SaveParsingSettingsButton_Click(object sender, EventArgs e)
        {
            SaveParsingSettings();
        }

        private void AffiliateXPathRevertButton_Click(object sender, EventArgs e)
        {
            AffiliateTextBox.Text = default_settings.AffiliateXPath;
        }

        private void AffiliateXPathTestButton_Click(object sender, EventArgs e)
        {
            TestMatchingListBox.Items.Clear();
            TestMatchingListBox.Items.AddRange(TestXPathOnChannels(AffiliateTextBox.Text).ToArray());
        }
    }
}
