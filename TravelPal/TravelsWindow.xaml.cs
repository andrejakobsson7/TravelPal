using System.Windows;
using System.Windows.Controls;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for TravelsWindow.xaml
    /// </summary>
    public partial class TravelsWindow : Window
    {
        public TravelsWindow()
        {
            InitializeComponent();
            lblWelcomeUser.Content = $"Welcome {UserManager.SignedInUser!.Username}";
            UpdateUi();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();

            UserManager.SignedInUser = null;

            Close();
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TravelPal is your one-stop shop for booking and administrating travels all over the world.\nOn this page you can:\n- Book a new travel, by clicking 'Add travel'.\n- Get to know more about a specific travel, by clicking on the particular travel and then clicking 'See details'\n- Cancel travel, by clicking on the particular travel and then the button 'Remove'", "About TravelPal", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTravelWindow addTravelWindow = new();
            addTravelWindow.Show();

            Close();
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            bool isValidChoice = ValidateTripHasBeenSelected();
            if (isValidChoice)
            {
                ListBoxItem selectedItem = (ListBoxItem)lstTravels.SelectedItem;
                TravelManager.SelectedTravel = (Travel)selectedItem.Tag;
                TravelDetailsWindow travelDetailsWindow = new();
                travelDetailsWindow.Show();

                Close();
            }
        }

        private bool ValidateTripHasBeenSelected()
        {
            if (lstTravels.SelectedIndex < 0)
            {
                MessageBox.Show("No travel has been selected");
                return false;
            }
            return true;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            bool isValidChoice = ValidateTripHasBeenSelected();
            if (isValidChoice)
            {
                ListBoxItem selectedItem = (ListBoxItem)lstTravels.SelectedItem;
                Travel selectedTravel = (Travel)selectedItem.Tag;
                MessageBoxResult response = MessageBox.Show($"Please confirm that you want to remove travel to {selectedTravel.Destination}.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (response == MessageBoxResult.Yes)
                {
                    TravelManager.RemoveTravel(selectedTravel);
                    MessageBox.Show("Travel was successfully removed", "Confirmation");
                    UpdateUi();
                }
            }
        }
        private void UpdateUi()
        {
            lstTravels.Items.Clear();
            if (UserManager.SignedInUser!.GetType() == typeof(User))
            {
                User loggedInUser = (User)UserManager.SignedInUser;
                foreach (Travel travel in loggedInUser.Travels)
                {
                    ListBoxItem item = new();
                    item.Tag = travel;
                    item.Content = travel.GetInfo();
                    lstTravels.Items.Add(item);
                }
            }
            else if (UserManager.SignedInUser.GetType() == typeof(Admin))
            {
                lblTravels.Content = "All registered travels";
                foreach (Travel travel in TravelManager.Travels)
                {
                    ListBoxItem item = new();
                    item.Tag = travel;
                    item.Content = travel.GetInfo();
                    lstTravels.Items.Add(item);
                }
            }

        }
    }
}
