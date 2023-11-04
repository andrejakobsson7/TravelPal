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
        public TravelDetailsWindow()
        {
            InitializeComponent();
            FillComboBoxes();
            ReadInAllTravelInformation();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            TravelsWindow travelsWindow = new();
            travelsWindow.Show();

            Close();
        }

        //Följande metod används första gången när man öppnar upp "Travel Details Window".
        private void AddAllItemsToPackingList(Travel travel)
        {
            foreach (IPackingListItem packItem in travel.PackingList!)
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
            dpStartDate.IsEnabled = true;
            dpEndDate.IsEnabled = true;
            txtTypeOfTravel.IsReadOnly = false;
            lstPackingList.IsEnabled = true;
            txtMeetingDetails.IsReadOnly = false;
            cbCountry.IsEnabled = true;
            cbTypeOfTravel.IsEnabled = true;
            cxAllInclusive.IsEnabled = true;
            lblAddnewItem.Visibility = Visibility.Visible;
            txtItem.Visibility = Visibility.Visible;
            lblItem.Visibility = Visibility.Visible;
            cxTravelDocument.Visibility = Visibility.Visible;
            lblQuantity.Visibility = Visibility.Visible;
            txtQuantity.Visibility = Visibility.Visible;
            btnAddItemToPackingList.Visibility = Visibility.Visible;
            btnRemoveItemFromPackingList.Visibility = Visibility.Visible;
        }

        private void ReadInAllTravelInformation()
        {
            //Läs in informationen till alla fält och visa ut den.
            txtDestination.Text = TravelManager.SelectedTravel!.Destination;
            txtCountry.Text = TravelManager.SelectedTravel.Country.ToString();
            txtTravellers.Text = TravelManager.SelectedTravel.Travellers.ToString();
            dpStartDate.SelectedDate = TravelManager.SelectedTravel.StartDate;
            dpEndDate.SelectedDate = TravelManager.SelectedTravel.EndDate;
            cbCountry.SelectedItem = TravelManager.SelectedTravel.Country;
            cbTypeOfTravel.SelectedItem = TravelManager.SelectedTravel.GetType().Name;
            txtTravelDays.Text = TravelManager.SelectedTravel.TravelDays.ToString();
            txtTypeOfTravel.Text = TravelManager.SelectedTravel.GetType().Name.ToString();
            AddAllItemsToPackingList(TravelManager.SelectedTravel);
            if (TravelManager.SelectedTravel.GetType() == typeof(Vacation))
            {
                Vacation selectedVacation = (Vacation)TravelManager.SelectedTravel;
                cxAllInclusive.IsChecked = selectedVacation.AllInclusive;
            }
            else if (TravelManager.SelectedTravel.GetType() == typeof(WorkTrip))
            {
                WorkTrip selectedWorkTrip = (WorkTrip)TravelManager.SelectedTravel;
                txtMeetingDetails.Text = selectedWorkTrip.MeetingDetails;
            }
        }
        private void FillComboBoxes()
        {
            foreach (Enum country in Enum.GetValues(typeof(Country)))
            {
                cbCountry.Items.Add(country);
            }

            //Fick inte rätt på att populera comboboxen med vald resetyp om det inte konverterades till strängar.
            foreach (Enum travelType in Enum.GetValues(typeof(TravelType)))
            {
                cbTypeOfTravel.Items.Add(travelType.ToString());
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Behöver inte validera att man faktiskt valt något i comboboxarna eftersom att det är förifyllt i början och man inte kan komma tillbaka till "null".
            if (TravelManager.ValidateStringInput(txtDestination.Text, (Button)sender) &&
                TravelManager.ValidateIntInput(txtTravellers.Text) &&
                TravelManager.ValidateSelectedDatesInDatePickers(dpStartDate.SelectedDate, dpEndDate.SelectedDate)
                )
            {
                List<IPackingListItem> userPackingList = TravelManager.GetPackingList(lstPackingList);
                TravelType selectedTravelType = FindTravelType((string)cbTypeOfTravel.SelectedItem);

                //Behöver sedan skapa en ny resa eftersom att jag inte kan komma åt och uppdatera vilken typ av resa det är.
                //Uppdaterar både resan i 'databasen' och användarens lista.
                //ID-numret sätts till samma som man klickat sig in på, och sedan ersätter vi resan med den nya.
                if (selectedTravelType == TravelType.Vacation)
                {
                    Vacation updatedVacation = new(TravelManager.SelectedTravel!.Id, (bool)cxAllInclusive.IsChecked!, txtDestination.Text, (Country)cbCountry.SelectedItem, int.Parse(txtTravellers.Text), (DateTime)dpStartDate.SelectedDate!, (DateTime)dpEndDate.SelectedDate!, userPackingList);
                    TravelManager.ReplaceTravelInTravelList(TravelManager.SelectedTravel, updatedVacation);
                    TravelManager.SelectedTravel = null;
                    ConfirmAndCloseTravelDetailsWindow();
                }
                else if (selectedTravelType == TravelType.WorkTrip && TravelManager.ValidateMeetingDetails(txtMeetingDetails.Text))
                {
                    WorkTrip updatedWorkTrip = new(TravelManager.SelectedTravel!.Id, txtMeetingDetails.Text, txtDestination.Text, (Country)cbCountry.SelectedItem, int.Parse(txtTravellers.Text), (DateTime)dpStartDate.SelectedDate!, (DateTime)dpEndDate.SelectedDate!, userPackingList);
                    TravelManager.ReplaceTravelInTravelList(TravelManager.SelectedTravel, updatedWorkTrip);
                    TravelManager.SelectedTravel = null;
                    ConfirmAndCloseTravelDetailsWindow();
                }
            }
        }

        private void ConfirmAndCloseTravelDetailsWindow()
        {
            MessageBox.Show("Travel was successfully updated. Close this window to return to overview-page", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            TravelsWindow travelsWindow = new();
            travelsWindow.Show();

            Close();
        }

        //Fick inte rätt på att populera comboboxen med valt värde om det inte var strängar.
        //Därför behöver jag konvertera tillbaka till TravelType när vi skapar ny, uppdaterad resa.

        private TravelType FindTravelType(string travelType)
        {
            foreach (Enum trvType in Enum.GetValues(typeof(TravelType)))
            {
                if (trvType.ToString() == travelType)
                {
                    return (TravelType)trvType;
                }
            }
            return 0;
        }

        private void cbTypeOfTravel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Börja med att återställa till "default-läge" för att ha höjd för om man togglar.
            lblWorkTripOrVacation.Content = "";
            cxAllInclusive.Visibility = Visibility.Hidden;
            cxAllInclusive.IsChecked = false;
            txtMeetingDetails.Visibility = Visibility.Hidden;
            txtMeetingDetails.Text = "";
            if (cbTypeOfTravel.SelectedIndex >= 0)
            {
                TravelType selectedTravelType = FindTravelType((string)cbTypeOfTravel.SelectedItem);
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

        private void cbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
             * Vid ändring av resa kan jag inte erbjuda samma funktionalitet som när man lägger till en ny resa gällande att lägga till korrekt pass (required/ej) i packlistan,
            eftersom att jag inte har tillgång till användaren som skapat resans location.
            Om jag hade lagt in User på varje Travel så hade det varit möjligt */
        }

        private void dpStartDate_DateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == (DatePicker)sender)
            {
                DatePicker dp = (DatePicker)sender;
                if (dp.IsEnabled)
                {
                    txtTravelDays.Text = ((DateTime)dpEndDate.SelectedDate! - (DateTime)dpStartDate.SelectedDate!).TotalDays.ToString();
                }
            }
        }

        private void dpEndDate_DateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == (DatePicker)sender)
            {
                DatePicker dp = (DatePicker)sender;
                if (dp.IsEnabled)
                {
                    txtTravelDays.Text = ((DateTime)dpEndDate.SelectedDate! - (DateTime)dpStartDate.SelectedDate!).TotalDays.ToString();
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

        private void btnRemoveItemFromPackingList_Click(object sender, RoutedEventArgs e)
        {
            bool isValidItem = ValidateItemHasBeenSelected();
            if (isValidItem)
            {
                lstPackingList.Items.Remove(lstPackingList.SelectedItem);
            }
        }
        private bool ValidateItemHasBeenSelected()
        {
            if (lstPackingList.SelectedIndex < 0)
            {
                MessageBox.Show("No item has been selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
