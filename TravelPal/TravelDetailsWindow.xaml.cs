using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for TravelDetailsWindow.xaml
    /// </summary>
    public partial class TravelDetailsWindow : Window
    {
        public TravelDetailsWindow(Travel travelToDisplay)
        {
            InitializeComponent();
            FillComboBoxes();
            //Läs in informationen till alla fält och visa ut den.
            txtDestination.Text = travelToDisplay.Destination;
            txtCountry.Text = travelToDisplay.Country.ToString();
            txtTravellers.Text = travelToDisplay.Travellers.ToString();
            txtStartDate.Text = DateOnly.FromDateTime(travelToDisplay.StartDate).ToString();
            txtEndDate.Text = DateOnly.FromDateTime(travelToDisplay.EndDate).ToString();
            dpStartDate.SelectedDate = travelToDisplay.StartDate;
            dpEndDate.SelectedDate = travelToDisplay.EndDate;
            cbCountry.SelectedItem = travelToDisplay.Country;
            cbTypeOfTravel.SelectedItem = travelToDisplay.GetType().Name;
            txtTravelDays.Text = travelToDisplay.TravelDays.ToString();
            txtTypeOfTravel.Text = travelToDisplay.GetType().Name.ToString();
            AddItemsToPackingList(travelToDisplay);
            if (travelToDisplay.GetType() == typeof(Vacation))
            {
                Vacation selectedVacation = (Vacation)travelToDisplay;
                lblWorkTripOrVacation.Content = "All Inclusive";
                cxAllInclusive.Visibility = Visibility.Visible;
                cxAllInclusive.IsChecked = selectedVacation.AllInclusive;
                if (!selectedVacation.AllInclusive)
                {
                    cxAllInclusive.Content = "No, it's not All Inclusive";
                }
            }
            else if (travelToDisplay.GetType() == typeof(WorkTrip))
            {
                WorkTrip selectedWorkTrip = (WorkTrip)travelToDisplay;
                lblWorkTripOrVacation.Content = "Meeting details";
                txtMeetingDetails.Visibility = Visibility.Visible;
                txtMeetingDetails.Text = selectedWorkTrip.MeetingDetails;
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            TravelsWindow travelsWindow = new();
            travelsWindow.Show();

            Close();
        }

        private void AddItemsToPackingList(Travel travel)
        {
            foreach (IPackingListItem packItem in travel.PackingList)
            {
                ListBoxItem item = new();
                item.Tag = packItem;
                item.Content = packItem.GetInfo();
                lstPackingList.Items.Add(item);

            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            MakeAllFieldsEditable();
            btnEdit.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Visible;
        }
        private void MakeAllFieldsEditable()
        {
            txtDestination.IsReadOnly = false;
            txtCountry.IsReadOnly = false;
            txtTravellers.IsReadOnly = false;
            txtStartDate.IsReadOnly = false;
            txtEndDate.IsReadOnly = false;
            txtTravelDays.IsReadOnly = false;
            txtTypeOfTravel.IsReadOnly = false;
            lstPackingList.IsEnabled = true;
            txtMeetingDetails.IsReadOnly = false;
            cbCountry.IsEnabled = true;
            cbTypeOfTravel.IsEnabled = true;
        }
        private void FillComboBoxes()
        {
            foreach (Enum country in Enum.GetValues(typeof(Country)))
            {
                cbCountry.Items.Add(country);
            }
            foreach (Enum travelType in Enum.GetValues(typeof(TravelType)))
            {
                cbTypeOfTravel.Items.Add(travelType.ToString());
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Kontrollera input, omvandla typeofTravel till Typeoftravel
            if (TravelManager.ValidateStringInput(txtDestination.Text, (Button)sender) &&
                TravelManager.ValidateSelectedItemInComboBox(cbCountry.SelectedItem, cbCountry) &&
                TravelManager.ValidateIntInput(txtTravellers.Text) &&
                TravelManager.ValidateSelectedDatesInDatePickers(dpStartDate.SelectedDate, dpEndDate.SelectedDate) &&
                TravelManager.ValidateSelectedItemInComboBox(cbTypeOfTravel.SelectedItem, cbTypeOfTravel))
            {
                List<IPackingListItem> userPackingList = TravelManager.GetPackingList(lstPackingList);
                TravelType selectedTravelType = (TravelType)cbTypeOfTravel.SelectedItem;
            }
        }
    }
}
