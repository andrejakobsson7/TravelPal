using System.Windows;
using TravelPal.Managers;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new();
            registerWindow.Show();

            Close();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            bool isSuccessfulLogin = UserManager.SignIn(txtUsername.Text, pbPassword.Password);
            if (isSuccessfulLogin)
            {
                TravelsWindow travelsWindow = new();
                travelsWindow.Show();
                Close();
            }
        }
    }
}
