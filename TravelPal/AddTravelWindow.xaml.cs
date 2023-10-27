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
                List<IPackingListItem> userPackingList = GetPackingList();
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
            //bool isAllFixedFieldsValid = ValidateStringInput(txtDestination.Text, (Button)sender);
            //if (isAllFixedFieldsValid)
            //{
            //    List<IPackingListItem> userPackingList = GetPackingList();
            //    TravelType selectedTravelType = (TravelType)cbTypeOfTravel.SelectedItem;
            //    if (selectedTravelType == TravelType.Vacation)
            //    {
            //        bool isAllInclusive = (bool)cxAllInclusive.IsChecked!;
            //        Vacation newVacation = new(isAllInclusive, txtDestination.Text, (Country)cbCountry.SelectedItem, int.Parse(txtTravellers.Text), (DateTime)dpStartDate.SelectedDate!, (DateTime)dpEndDate.SelectedDate!, userPackingList);
            //        TravelManager.AddTravel(newVacation);
            //        ConfirmSuccessfullyRegisteredTravel(newVacation);
            //        UpdateUi();
            //    }
            //    else if (selectedTravelType == TravelType.WorkTrip && ValidateMeetingDetails(txtMeetingDetails.Text))
            //    {
            //        string meetingDetails = txtMeetingDetails.Text;
            //        WorkTrip newWorkTrip = new(meetingDetails, txtDestination.Text, (Country)cbCountry.SelectedItem, int.Parse(txtTravellers.Text), (DateTime)dpStartDate.SelectedDate!, (DateTime)dpEndDate.SelectedDate!, userPackingList);
            //        TravelManager.AddTravel(newWorkTrip);
            //        ConfirmSuccessfullyRegisteredTravel(newWorkTrip);
            //        UpdateUi();
            //    }
            //}
        }
        //Nedanstående metod används både för att validera så att man skrivit in något i fältet "Destination" och "Item" när man lägger till i sin packningslista,
        //därav skickar man med vilken knapp man kommer från till metoden, så att vi kan displaya ett bra felmeddelande.
        //private bool ValidateStringInput(string input, object sender)
        //{
        //    Button btn = (Button)sender;
        //    if (string.IsNullOrEmpty(input) && btn.Name == "btnAdd")
        //    {
        //        MessageBox.Show("No destination has been entered");
        //        return false;
        //    }
        //    else if (string.IsNullOrEmpty(input) && btn.Name == "btnAddItemToPackingList")
        //    {
        //        MessageBox.Show("No item has been entered");
        //        return false;
        //    }
        //    if (btn.Name == "btnAdd")
        //    {
        //        return ValidateSelectedItemInComboBox(cbCountry.SelectedItem, cbCountry);
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        ////Nedanstående metod används både för att validera att man valt något i combobox 1 (Land) och combobox 2 (resetyp).
        ////För att kunna ha metoden gemensam behöver man skicka med vilken combobox man kommer från och på så vis kan vi hantera felen på rätt sätt och slussa vidare till nästa kontrollstation.
        //private bool ValidateSelectedItemInComboBox(object value, ComboBox selectedCombobox)
        //{
        //    if (value == null)
        //    {
        //        //Felmeddelandet är generiskt och hämtar typnamnet för första enum i listan som comboboxen avser och displayar i felmeddelandet.
        //        MessageBox.Show($"No {selectedCombobox.Items[0].GetType().Name} has been selected");
        //        return false;
        //    }
        //    if (selectedCombobox == cbCountry)
        //    {
        //        return ValidateIntInput(txtTravellers.Text);
        //    }
        //    return true;
        //}
        //private bool ValidateIntInput(string input)
        //{
        //    try
        //    {
        //        int travellers = int.Parse(input);
        //        if (travellers <= 0)
        //        {
        //            throw new ArgumentOutOfRangeException("Number of travellers must be at least 1");
        //        }
        //    }
        //    catch (FormatException)
        //    {
        //        MessageBox.Show($"{input} is not a valid number of travellers");
        //        return false;
        //    }
        //    catch (OverflowException)
        //    {
        //        MessageBox.Show($"{input} is a too large or small number of travellers");
        //        return false;
        //    }
        //    catch (ArgumentOutOfRangeException e)
        //    {
        //        MessageBox.Show(e.Message);
        //        return false;
        //    }
        //    return ValidateSelectedDatesInDatePickers(dpStartDate.SelectedDate, dpEndDate.SelectedDate);
        //}

        //private bool ValidateSelectedDatesInDatePickers(object? startDate, object? endDate)
        //{
        //    if (startDate == null)
        //    {
        //        MessageBox.Show("No start date has been entered");
        //        return false;
        //    }
        //    else if (endDate == null)
        //    {
        //        MessageBox.Show("No end date has been entered");
        //        return false;
        //    }
        //    return ValidateDates((DateTime)startDate, (DateTime)endDate);
        //}
        //private bool ValidateDates(DateTime startDate, DateTime endDate)
        //{
        //    if (startDate > endDate)
        //    {
        //        MessageBox.Show("End date be after start date");
        //        return false;
        //    }
        //    else if (startDate < DateTime.Today)
        //    {
        //        MessageBox.Show("Start date can't be before todays' date");
        //        return false;
        //    }
        //    return ValidateSelectedItemInComboBox(cbTypeOfTravel.SelectedItem, cbTypeOfTravel);
        //}

        //private bool ValidateMeetingDetails(string meetingDetails)
        //{
        //    if (string.IsNullOrEmpty(meetingDetails))
        //    {
        //        MessageBox.Show("No meeting details has been entered");
        //        return false;
        //    }
        //    return true;
        //}

        //private void ConfirmSuccessfullyRegisteredTravel(Travel travel)
        //{
        //    MessageBox.Show($"New travel to {travel.Destination} has been registered!");
        //}

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

        private bool ValidateQuantityInput(string input)
        {
            try
            {
                int quantity = int.Parse(input);
                if (quantity <= 0)
                {
                    throw new ArgumentException("Quantity must be at least 1");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show($"{input} is not a valid quantity");
                return false;
            }
            catch (OverflowException)
            {
                MessageBox.Show($"{input} is a too large or small quantity");
                return false;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        private void btnAddItemToPackingList_Click(object sender, RoutedEventArgs e)
        {
            bool isValidItem = TravelManager.ValidateStringInput(txtItem.Text, (Button)sender);
            if (isValidItem)
            {
                if (cxTravelDocument.IsChecked == true)
                {
                    TravelDocument newTravelDocument = new(txtItem.Text, (bool)cxTravelDocumentRequired.IsChecked!);
                    AddItemToPackingList(newTravelDocument);
                    UpdatePackingSection();
                }
                else
                {
                    bool isValidQuantity = TravelManager.ValidateIntInput(txtQuantity.Text);
                    if (isValidQuantity)
                    {
                        OtherItem newOtherItem = new(txtItem.Text, int.Parse(txtQuantity.Text));
                        AddItemToPackingList(newOtherItem);
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

        private void AddItemToPackingList(IPackingListItem document)
        {
            ListBoxItem item = new();
            item.Tag = document;
            item.Content = document.GetInfo();
            lstPackingList.Items.Add(item);
        }

        private List<IPackingListItem> GetPackingList()
        {
            List<IPackingListItem> userPackingList = new();
            foreach (ListBoxItem item in lstPackingList.Items)
            {
                IPackingListItem packingListItem = (IPackingListItem)item.Tag;
                userPackingList.Add(packingListItem);
            }
            return userPackingList;
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
