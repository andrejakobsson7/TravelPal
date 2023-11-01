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
            bool isValidCountry = ValidateSelectedItemInComboBox();
            if (isValidCountry)
            {
                IUser newUser = UserManager.RegisterUser(txtUsername.Text, pbPassword.Password, (Country)cbCountry.SelectedItem);
                if (newUser != null)
                {
                    ConfirmAndCloseRegisterWindow(newUser.Username);
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
                MessageBox.Show("No country has been selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void ConfirmAndCloseRegisterWindow(string username)
        {
            MessageBox.Show($"{username} has been successfully registered as a new customer! You will now be redirected to login page", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            MainWindow mainWindow = new();
            mainWindow.Show();

            Close();
        }
    }
}
