using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for AddTravelWindow.xaml
    /// </summary>
    public partial class AddTravelWindow : Window
    {
        public AddTravelWindow()
        {
            InitializeComponent();
            FillComboBoxes();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            TravelsWindow travelsWindow = new();
            travelsWindow.Show();

            Close();
        }

        private void cbTypeOfTravel_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //Börja med att återställa till "default-läge" för att ha höjd för om man togglar.
            lblWorkTripOrVacation.Content = "";
            cxAllInclusive.Visibility = Visibility.Hidden;
            txtMeetingDetails.Visibility = Visibility.Hidden;
            if (cbTypeOfTravel.SelectedIndex >= 0)
            {
                TravelType selectedTravelType = (TravelType)cbTypeOfTravel.SelectedItem;
                if (selectedTravelType == TravelType.Vacation)
                {
                    lblWorkTripOrVacation.Content = "All Inclusive";
                    cxAllInclusive.Visibility = Visibility.Visible;
                }
                else if (selectedTravelType == TravelType.WorkTrip)
                {
                    lblWorkTripOrVacation.Content = "Meeting details";
                    txtMeetingDetails.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (TravelManager.ValidateStringInput(txtDestination.Text, (Button)sender) &&
                TravelManager.ValidateSelectedItemInComboBox(cbCountry.SelectedItem, cbCountry) &&
                TravelManager.ValidateIntInput(txtTravellers.Text) &&
                TravelManager.ValidateSelectedDatesInDatePickers(dpStartDate.SelectedDate, dpEndDate.SelectedDate) &&
                TravelManager.ValidateSelectedItemInComboBox(cbTypeOfTravel.SelectedItem, cbTypeOfTravel))
            {
                List<IPackingListItem> userPackingList = TravelManager.GetPackingList(lstPackingList);
                TravelType selectedTravelType = (TravelType)cbTypeOfTravel.SelectedItem;
                if (selectedTravelType == TravelType.Vacation)
                {
                    bool isAllInclusive = (bool)cxAllInclusive.IsChecked!;
                    Vacation newVacation = new(isAllInclusive, txtDestination.Text, (Country)cbCountry.SelectedItem, int.Parse(txtTravellers.Text), (DateTime)dpStartDate.SelectedDate!, (DateTime)dpEndDate.SelectedDate!, userPackingList);
                    TravelManager.AddTravel(newVacation);
                    TravelManager.ConfirmSuccessfullyRegisteredTravel(newVacation);
                    UpdateUi();
                }
                else if (selectedTravelType == TravelType.WorkTrip && TravelManager.ValidateMeetingDetails(txtMeetingDetails.Text))
                {
                    string meetingDetails = txtMeetingDetails.Text;
                    WorkTrip newWorkTrip = new(meetingDetails, txtDestination.Text, (Country)cbCountry.SelectedItem, int.Parse(txtTravellers.Text), (DateTime)dpStartDate.SelectedDate!, (DateTime)dpEndDate.SelectedDate!, userPackingList);
                    TravelManager.AddTravel(newWorkTrip);
                    TravelManager.ConfirmSuccessfullyRegisteredTravel(newWorkTrip);
                    UpdateUi();
                }
            }
        }
        private void UpdateUi()
        {
            txtDestination.Text = "";
            cbCountry.SelectedIndex = -1;
            txtTravellers.Text = "";
            dpStartDate.SelectedDate = null;
            dpEndDate.SelectedDate = null;
            cbTypeOfTravel.SelectedIndex = -1;
            cxAllInclusive.IsChecked = false;
            txtMeetingDetails.Text = "";
            lstPackingList.Items.Clear();
            UpdatePackingSection();
        }
        private void UpdatePackingSection()
        {
            txtItem.Text = "";
            cxTravelDocument.IsChecked = false;
            cxTravelDocumentRequired.Visibility = Visibility.Hidden;
            cxTravelDocumentRequired.IsChecked = false;
            lblQuantity.Visibility = Visibility.Visible;
            txtQuantity.Visibility = Visibility.Visible;
            txtQuantity.Text = "";
        }

        private void FillComboBoxes()
        {
            foreach (Enum country in Enum.GetValues(typeof(Country)))
            {
                cbCountry.Items.Add(country);
            }
            foreach (Enum travelType in Enum.GetValues(typeof(TravelType)))
            {
                cbTypeOfTravel.Items.Add(travelType);
            }
        }

        private void btnAddItemToPackingList_Click(object sender, RoutedEventArgs e)
        {
            bool isValidItem = TravelManager.ValidateStringInput(txtItem.Text, (Button)sender);
            if (isValidItem)
            {
                if (cxTravelDocument.IsChecked == true)
                {
                    TravelDocument newTravelDocument = new(txtItem.Text, (bool)cxTravelDocumentRequired.IsChecked!);
                    TravelManager.AddItemToPackingList(newTravelDocument, lstPackingList);
                    UpdatePackingSection();
                }
                else
                {
                    bool isValidQuantity = TravelManager.ValidateIntInput(txtQuantity.Text);
                    if (isValidQuantity)
                    {
                        OtherItem newOtherItem = new(txtItem.Text, int.Parse(txtQuantity.Text));
                        TravelManager.AddItemToPackingList(newOtherItem, lstPackingList);
                        UpdatePackingSection();
                    }
                }
            }

        }

        private void cxTravelDocument_Checked(object sender, RoutedEventArgs e)
        {
            cxTravelDocumentRequired.Visibility = Visibility.Visible;
            cxTravelDocumentRequired.IsChecked = false;
            lblQuantity.Visibility = Visibility.Hidden;
            txtQuantity.Visibility = Visibility.Hidden;
        }

        private void cxTravelDocument_Unchecked(object sender, RoutedEventArgs e)
        {
            cxTravelDocumentRequired.Visibility = Visibility.Hidden;
            cxTravelDocumentRequired.IsChecked = false;
            lblQuantity.Visibility = Visibility.Visible;
            txtQuantity.Visibility = Visibility.Visible;
        }

        private void cbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCountry.SelectedIndex >= 0)
            {
                lstPackingList.Items.Clear();
                IPackingListItem defaultItem = TravelManager.AddDefaultPackingListItem(UserManager.SignedInUser!.Location, (Country)cbCountry.SelectedItem);
                ListBoxItem item = new();
                item.Tag = defaultItem;
                item.Content = defaultItem.GetInfo();
                lstPackingList.Items.Add(item);
            }
        }
    }
}
