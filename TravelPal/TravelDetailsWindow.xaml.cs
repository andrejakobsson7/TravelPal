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
            //Läs in informationen till alla fält och visa ut den.
            txtDestination.Text = TravelManager.SelectedTravel.Destination;
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
                lblWorkTripOrVacation.Content = "All Inclusive";
                cxAllInclusive.Visibility = Visibility.Visible;
                cxAllInclusive.IsChecked = selectedVacation.AllInclusive;
            }
            else if (TravelManager.SelectedTravel.GetType() == typeof(WorkTrip))
            {
                WorkTrip selectedWorkTrip = (WorkTrip)TravelManager.SelectedTravel;
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

        private void AddAllItemsButTheFirstToPackingList(Travel travel)
        {
            for (int i = 1; i < travel.PackingList.Count; i++)
            {
                ListBoxItem item = new();
                item.Tag = travel.PackingList[i];
                item.Content = travel.PackingList[i].GetInfo();
                lstPackingList.Items.Insert(i, item);
            }
        }

        private void AddAllItemsToPackingList(Travel travel)
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
            dpStartDate.IsEnabled = true;
            dpEndDate.IsEnabled = true;
            txtTypeOfTravel.IsReadOnly = false;
            lstPackingList.IsEnabled = true;
            txtMeetingDetails.IsReadOnly = false;
            cbCountry.IsEnabled = true;
            cbTypeOfTravel.IsEnabled = true;
            cxAllInclusive.IsEnabled = true;
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
                //ID-numret sätts till samma som man klickat sig in på, och sedan tar vi bort resan som man klickat sig in från.
                if (selectedTravelType == TravelType.Vacation)
                {
                    bool isAllInclusive = (bool)cxAllInclusive.IsChecked!;
                    Vacation updatedVacation = new(TravelManager.SelectedTravel!.Id, isAllInclusive, txtDestination.Text, (Country)cbCountry.SelectedItem, int.Parse(txtTravellers.Text), (DateTime)dpStartDate.SelectedDate!, (DateTime)dpEndDate.SelectedDate!, userPackingList);
                    TravelManager.AddTravel(updatedVacation);
                    TravelManager.RemoveTravel(TravelManager.SelectedTravel!);
                    TravelManager.SelectedTravel = null;
                    ConfirmAndCloseTravelDetailsWindow();
                }
                else if (selectedTravelType == TravelType.WorkTrip && TravelManager.ValidateMeetingDetails(txtMeetingDetails.Text))
                {
                    string meetingDetails = txtMeetingDetails.Text;
                    WorkTrip updatedWorkTrip = new(TravelManager.SelectedTravel!.Id, meetingDetails, txtDestination.Text, (Country)cbCountry.SelectedItem, int.Parse(txtTravellers.Text), (DateTime)dpStartDate.SelectedDate!, (DateTime)dpEndDate.SelectedDate!, userPackingList);
                    TravelManager.AddTravel(updatedWorkTrip);
                    TravelManager.RemoveTravel(TravelManager.SelectedTravel!);
                    TravelManager.SelectedTravel = null;
                    ConfirmAndCloseTravelDetailsWindow();
                }
            }
        }

        private void ConfirmAndCloseTravelDetailsWindow()
        {
            MessageBox.Show("Travel was successfully updated. Close this window to return to overview-page");
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
            if (sender == (ComboBox)sender)
            {
                ComboBox cbb = (ComboBox)sender;
                //Detta säkerställer att följande kod bara körs när användaren själv har valt något nytt land i rullistan. 
                //När sidan startas är comboboxen inte "enabled".
                //Vi flyttar med packlistan (utom första saken i listan, som alltid är passet, för den räknas fram (om den är required eller ej) och läggs till som default item.

                if (cbb.IsEnabled)
                {
                    lstPackingList.Items.Clear();
                    IPackingListItem defaultItem = TravelManager.AddDefaultPackingListItem(UserManager.SignedInUser!.Location, (Country)cbCountry.SelectedItem);
                    ListBoxItem item = new();
                    item.Tag = defaultItem;
                    item.Content = defaultItem.GetInfo();
                    lstPackingList.Items.Insert(0, item);
                    AddAllItemsButTheFirstToPackingList(TravelManager.SelectedTravel!);
                }
            }
        }
    }
}
