using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.MediaCenter.Guide;

namespace ChannelEditingLib
{
    public sealed partial class ListingSelectionForm : Form, ListingSelectionListener
    {
        public ListingSelectionForm(ListingSelectionListener selection_listener)
            : this(selection_listener, null)
        {
        }

        public ListingSelectionForm(Channel channel_changing_listing)
            : this(null, channel_changing_listing)
        {
        }

        public ListingSelectionForm(ListingSelectionListener selection_listener, Channel channel_changing_listing)
        {
            InitializeComponent();
            channel_changing_listing_ = channel_changing_listing;
            selection_listener_ = (selection_listener != null) ? selection_listener : this;
            if (selection_listener_.HasCustomServices())
                custom_services = selection_listener_.GetCustomServices().ToList();
            CustomListingsRadioButton.Visible = (custom_services.Count > 0);
            SortServices();
            UpdateListBox();
        }

        private void SortServices()
        {
            Comparison<ChannelService> comparison = null;
            if (SortNameRadioButton.Checked)
            {
                comparison = ChannelService.CompareByService;
            }
            else
            {
                comparison = ChannelService.CompareByNumber;
            }
            wmis_services.Sort(comparison);
        }

        private void UpdateListBox()
        {
            object old_selected_item = ListingSelectionListBox.SelectedItem;
            ListingSelectionListBox.Items.Clear();
            if (WmisDevicesRadioButton.Checked)
                ListingSelectionListBox.Items.AddRange(wmis_services.ToArray());
            else if (AllListingsRadioButton.Checked)
                ListingSelectionListBox.Items.AddRange(all_services.ToArray());
            else if (CustomListingsRadioButton.Checked)
                ListingSelectionListBox.Items.AddRange(custom_services.ToArray());
            if (old_selected_item != null && ListingSelectionListBox.Items.Contains(old_selected_item))
                ListingSelectionListBox.SelectedItem = old_selected_item;
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private ListingSelectionListener selection_listener_;
        private List<ChannelService> wmis_services = ChannelEditing.GetChannelServicesForWmiLineups().ToList();
        private List<Service> all_services = ChannelEditing.GetServices();
        private List<Service> custom_services = new List<Service>();

        private void ListingSourceRadioClicked(object sender, EventArgs e)
        {
            UpdateListBox();
        }

        private void SortRadioClicked(object sender, EventArgs e)
        {
            SortServices();
            UpdateListBox();
        }

        private Service GetSelectedService()
        {
            object selected_item = ListingSelectionListBox.SelectedItem;
            if (selected_item is Service)
                return (Service)ListingSelectionListBox.SelectedItem;
            if (selected_item is ChannelService)
                return (selected_item as ChannelService).Service;
            return null;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (selection_listener_ != null)
            {
                Service selected_service = GetSelectedService();
                try
                {
                    if (selected_service != null)
                        selection_listener_.HandleListingSelected(GetSelectedService());
                }
                catch (Exception exc)
                {
                    new ErrorReporter.ErrorReportingForm("Exception occured while attempting to update listing.", exc);
                }
            }
        }

        #region ListingSelectionListener Members
        Channel channel_changing_listing_ = null;
        void ListingSelectionListener.HandleListingSelected(Service listing)
        {
            if (channel_changing_listing_ == null) return;
            channel_changing_listing_.Service = listing;
            channel_changing_listing_.Update();
        }

        bool ListingSelectionListener.HasCustomServices()
        {
            return false;
        }

        IEnumerable<Service> ListingSelectionListener.GetCustomServices()
        {
            return new List<Service>();
        }

        #endregion
    }

    public interface ListingSelectionListener
    {
        void HandleListingSelected(Service listing);
        bool HasCustomServices();
        IEnumerable<Service> GetCustomServices();
    }
}
