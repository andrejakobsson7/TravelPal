using System;
using System.Windows;
using TravelPal.Managers;

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

                //Vid ändring av location ändrar inte systemet något "default pass" i användarens resor - det får man göra själv.
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
