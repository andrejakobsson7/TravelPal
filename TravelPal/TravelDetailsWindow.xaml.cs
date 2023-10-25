using System;
using System.Windows;
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
            //Läs in informationen till alla fält och visa ut den.
            txtDestination.Text = travelToDisplay.Destination;
            txtCountry.Text = travelToDisplay.Country.ToString();
            txtTravellers.Text = travelToDisplay.Travellers.ToString();
            txtStartDate.Text = DateOnly.FromDateTime(travelToDisplay.StartDate).ToString();
            txtEndDate.Text = DateOnly.FromDateTime(travelToDisplay.EndDate).ToString();
            txtTravelDays.Text = travelToDisplay.TravelDays.ToString();
            txtTypeOfTravel.Text = travelToDisplay.GetType().Name.ToString();
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
    }
}
