using System;
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
            //Ladda comboboxar med alternativ
            foreach (Enum country in Enum.GetValues(typeof(Country)))
            {
                cbCountry.Items.Add(country);
            }
            foreach (Enum travelType in Enum.GetValues(typeof(TravelType)))
            {
                cbTypeOfTravel.Items.Add(travelType);
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            TravelsWindow travelsWindow = new();
            travelsWindow.Show();

            Close();
        }

        private void cbTypeOfTravel_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
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
            bool isAllFixedFieldsValid = ValidateDestinationInput(txtDestination.Text);
            if (isAllFixedFieldsValid)
            {
                TravelType selectedTravelType = (TravelType)cbTypeOfTravel.SelectedItem;
                if (selectedTravelType == TravelType.Vacation)
                {
                    bool isAllInclusive = (bool)cxAllInclusive.IsChecked!;
                    Vacation newVacation = new(isAllInclusive, txtDestination.Text, (Country)cbCountry.SelectedItem, int.Parse(txtTravellers.Text), (DateTime)dpStartDate.SelectedDate!, (DateTime)dpEndDate.SelectedDate!);
                    TravelManager.AddTravel(newVacation);
                    ConfirmSuccessfullyRegisteredTravel(newVacation);
                    UpdateUi();
                }
                else if (selectedTravelType == TravelType.WorkTrip && ValidateMeetingDetails(txtMeetingDetails.Text))
                {
                    string meetingDetails = txtMeetingDetails.Text;
                    WorkTrip newWorkTrip = new(meetingDetails, txtDestination.Text, (Country)cbCountry.SelectedItem, int.Parse(txtTravellers.Text), (DateTime)dpStartDate.SelectedDate!, (DateTime)dpEndDate.SelectedDate!);
                    TravelManager.AddTravel(newWorkTrip);
                    ConfirmSuccessfullyRegisteredTravel(newWorkTrip);
                    UpdateUi();
                }
            }
        }
        private bool ValidateDestinationInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("No destination has been entered");
                return false;
            }
            return ValidateSelectedItemInComboBox(cbCountry.SelectedItem, cbCountry);
        }
        private bool ValidateSelectedItemInComboBox(object value, ComboBox selectedCombobox)
        {
            if (value == null)
            {
                MessageBox.Show($"No {selectedCombobox.Items[0].GetType().Name} has been selected");
                return false;
            }
            if (selectedCombobox == cbCountry)
            {
                return ValidateIntInput(txtTravellers.Text);
            }
            return true;
        }
        private bool ValidateIntInput(string input)
        {
            try
            {
                int travellers = int.Parse(input);
                if (travellers <= 0)
                {
                    throw new ArgumentException("Number of travellers must be at least 1");
                }
            }
            catch (FormatException e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            catch (OverflowException e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return ValidateSelectedDatesInDatePickers(dpStartDate.SelectedDate!, dpEndDate.SelectedDate!);
        }

        private bool ValidateSelectedDatesInDatePickers(object startDate, object endDate)
        {
            if (startDate == null)
            {
                MessageBox.Show("No start date has been entered");
                return false;
            }
            else if (endDate == null)
            {
                MessageBox.Show("No end date has been entered");
                return false;
            }
            return ValidateDates((DateTime)startDate, (DateTime)endDate);
        }
        private bool ValidateDates(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                MessageBox.Show("End date must come after start date");
                return false;
            }
            else if (startDate < DateTime.Today)
            {
                MessageBox.Show("Start date can't be before todays' date");
                return false;
            }
            return ValidateSelectedItemInComboBox(cbTypeOfTravel.SelectedItem, cbTypeOfTravel);
        }

        private bool ValidateMeetingDetails(string meetingDetails)
        {
            if (string.IsNullOrEmpty(meetingDetails))
            {
                MessageBox.Show("No meeting details has been entered");
                return false;
            }
            return true;
        }

        private void ConfirmSuccessfullyRegisteredTravel(Travel travel)
        {
            MessageBox.Show($"New travel to {travel.Destination} has been registered!");
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

        }
    }
}
