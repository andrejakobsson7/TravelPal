using System.Windows;
using TravelPal.Managers;

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
            lblWelcomeUser.Content = $"Welcome {UserManager.SignedInUser.Username}";
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
    }
}
