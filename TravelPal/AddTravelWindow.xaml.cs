using System;
using System.Windows;

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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //Läs input
            //Validera input
        }
    }
}
