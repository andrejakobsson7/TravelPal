using System;
using System.Windows;

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
            //Ladda combobox med valbara länder från Enums
            foreach (Enum country in Enum.GetValues(typeof(EuropeanCountry)))
            {
                cbLocation.Items.Add(country);
            }
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
    }
}
