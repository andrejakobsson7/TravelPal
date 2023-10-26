using System;
using System.Collections.Generic;
using System.Windows;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public static class UserManager
    {
        public static List<IUser> Users { get; set; } = new()
        {
            new User
            {
                Username = "user",
                Password = "password",
                Location = Country.Sweden,
                Travels = new List<Travel>
                {
                    new Vacation(true, "New York", Country.USA, 2, DateTime.Today, DateTime.Today.AddDays(3), 1),
                    new WorkTrip("Internal discussions", "Stockholm", Country.Sweden, 1, DateTime.Today, DateTime.Today.AddDays(2), 2)
                }
            },
            new Admin("admin", "password", Country.USA)
        };

        public static IUser? SignedInUser { get; set; }

        //Methods for validating username and password when registering as a new customer.
        public static bool ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("No username has been entered", "Error");
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
                    MessageBox.Show("Username is already in use, please choose another one", "Error");
                    return false;
                }
            }
            return true;
        }

        public static bool ValidatePassword(string password)
        {
            if (password.Length < 6)
            {
                MessageBox.Show("Password needs to be at least 6 characters long", "Error");
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

        //Method used when signing in from MainWindow.
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
            MessageBox.Show("Invalid username and/or password", "Error");
            return false;
        }
    }
}
