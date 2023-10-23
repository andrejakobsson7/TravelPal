using System;
using System.Windows;
using TravelPal.Managers;

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
            //Ladda combobox med länder från Country. Den innehåller alla länder (även europeiska).

            foreach (Enum country in Enum.GetValues(typeof(Country)))
            {
                cbLocation.Items.Add(country);
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();

            Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            //Läs och validera input
            if (cbLocation.SelectedIndex < 0)
            {
                MessageBox.Show("No location has been selected", "Error");
            }
            else
            {
                bool isValidUsername = UserManager.ValidateUsername(txtUsername.Text);
                bool isValidPassword = UserManager.ValidatePassword(pbPassword.Password);
                if (isValidUsername && isValidPassword)
                {
                    bool isSuccessfullyRegistered = UserManager.CreateAndAddUser(txtUsername.Text, pbPassword.Password, (Country)cbLocation.SelectedItem);
                    if (isSuccessfullyRegistered)
                    {
                        MessageBoxResult answer = MessageBox.Show($"User has been successfully registered! Click 'OK' to go back to login page", "Confirmation", MessageBoxButton.OKCancel);
                        if (answer == MessageBoxResult.OK)
                        {
                            MainWindow mainWindow = new();
                            mainWindow.Show();

                            Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong. Try again later!", "Error");
                    }
                }
            }
        }
    }
}
