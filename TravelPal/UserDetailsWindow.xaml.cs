using System;
using System.Windows;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        public UserDetailsWindow()
        {
            InitializeComponent();
            FillComboBoxes();
            ReadInAllUserInformation();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            TravelsWindow travelsWindow = new();
            travelsWindow.Show();

            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Land behöver inte valideras, eftersom att man inte kan välja "null".
            bool isUsernameChanged = false;
            bool isPasswordChanged = false;
            bool isCountryChanged = false;

            if (txtNewUsername.Text != txtCurrentUsername.Text)
            {
                isUsernameChanged = UserManager.UpdateUsername(UserManager.SignedInUser!, txtNewUsername.Text);
            }
            if (pbNewPassword.Password != "")
            {
                isPasswordChanged = UserManager.UpdatePassword(UserManager.SignedInUser!, pbNewPassword.Password, pbConfirmPassword.Password);
            }
            if (cbNewCountry.SelectedIndex != cbCurrentCountry.SelectedIndex)
            {
                isCountryChanged = UserManager.UpdateCountry(UserManager.SignedInUser!, (Country)cbNewCountry.SelectedItem);

                //Följande kod säkerställer att rätt typ av "Passport"(required / ej) läggs till på användarens resa när man ändrat sin location.
                //Admin har inga resor så behöver inte göra något med det.
                if (UserManager.SignedInUser!.GetType() == typeof(User))
                {
                    User signedInCustomer = (User)UserManager.SignedInUser;
                    foreach (Travel travel in signedInCustomer.Travels)
                    {
                        travel.PackingList![0] = TravelManager.AddDefaultPackingListItem(UserManager.SignedInUser.Location, travel.Country);
                    }
                }
            }
            if (isUsernameChanged || isPasswordChanged || isCountryChanged)
            {
                ConfirmAndCloseUserDetailsWindow();
            }
        }

        private void FillComboBoxes()
        {
            foreach (Enum country in Enum.GetValues(typeof(Country)))
            {
                cbCurrentCountry.Items.Add(country);
                cbNewCountry.Items.Add(country);
            }
        }

        private void ReadInAllUserInformation()
        {
            txtCurrentUsername.Text = UserManager.SignedInUser!.Username;
            txtNewUsername.Text = UserManager.SignedInUser.Username;
            cbCurrentCountry.SelectedItem = UserManager.SignedInUser.Location;
            cbNewCountry.SelectedItem = UserManager.SignedInUser.Location;
        }

        private void ConfirmAndCloseUserDetailsWindow()
        {
            MessageBox.Show("Changes successfully saved. Close this window to return to overview-page");
            TravelsWindow travelsWindow = new();
            travelsWindow.Show();

            Close();
        }
    }
}
