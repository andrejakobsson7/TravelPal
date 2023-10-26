using System;
using System.Windows;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            AddCountriesToCombobox();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();

            Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            bool isValidUsername = UserManager.ValidateUsername(txtUsername.Text);
            bool isValidPassword = UserManager.ValidatePassword(pbPassword.Password);
            bool isValidCountry = ValidateSelectedItemInComboBox();
            if (isValidUsername && isValidPassword && isValidCountry)
            {
                IUser newUser = UserManager.RegisterUser(txtUsername.Text, pbPassword.Password, (Country)cbCountry.SelectedItem);
                MessageBoxResult answer = MessageBox.Show($"{newUser.Username} has been successfully registered! Click 'OK' to go back to login page", "Confirmation", MessageBoxButton.OKCancel);
                if (answer == MessageBoxResult.OK)
                {
                    MainWindow mainWindow = new();
                    mainWindow.Show();
                    Close();
                }
            }
        }
        private void AddCountriesToCombobox()
        {
            foreach (Enum country in Enum.GetValues(typeof(Country)))
            {
                cbCountry.Items.Add(country);
            }
        }
        private bool ValidateSelectedItemInComboBox()
        {
            if (cbCountry.SelectedIndex < 0)
            {
                MessageBox.Show("No location has been selected", "Error");
                return false;
            }
            return true;
        }
    }
}
