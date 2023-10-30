using System.Collections.Generic;
using System.Windows;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public static class UserManager
    {
        public static List<IUser> Users { get; set; } = new()
        {
            new Admin("admin", "password", Country.USA),
            new User
            {
                Username = "user",
                Password = "password",
                Location = Country.Sweden,
                //This ensures that the user has the same travels that are registered as "default-travels" in TravelManager.
                Travels = new List<Travel>
                {
                    TravelManager.Travels[0],
                    TravelManager.Travels[1]
                }
            }
        };

        public static IUser? SignedInUser { get; set; }

        //Methods for validating username and password when registering as a new customer.
        public static bool ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("No username has been entered", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (username.Length < 3)
            {
                MessageBox.Show("Username needs to be at least 3 characters long", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return CheckIfUsernameExists(username);
        }

        private static bool CheckIfUsernameExists(string username)
        {
            foreach (IUser user in Users)
            {
                if (user.Username == username)
                {
                    MessageBox.Show("Username is already in use, please choose another one", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true;
        }

        public static bool ValidatePassword(string password)
        {
            if (password.Length < 5)
            {
                MessageBox.Show("Password needs to be at least 5 characters long", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public static bool ComparePasswords(string enteredPassword, string confirmationPassword)
        {
            if (enteredPassword != confirmationPassword)
            {
                MessageBox.Show("Passwords are not identical, try again!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        public static IUser RegisterUser(string username, string password, Country country)
        {
            User newUser = new(username, password, country);
            Users.Add(newUser);
            return newUser;
        }

        public static bool SignIn(string username, string password)
        {
            foreach (IUser user in Users)
            {
                if (user.Username == username && user.Password == password)
                {
                    SignedInUser = user;
                    return true;
                }
            }
            MessageBox.Show("Invalid username and/or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
    }
}
