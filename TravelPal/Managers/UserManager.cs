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
            new User("user", "password", Country.USA),
            new Admin("admin", "password", Country.USA)
        };

        public static IUser SignedInUser { get; set; }

        public static bool ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("No username has been entered", "Error");
                return false;
            }
            return CheckIfUsernameExists(username);
        }

        public static bool CheckIfUsernameExists(string username)
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
                MessageBox.Show("Password needs to be 6 characters or more", "Error");
                return false;
            }
            return true;
        }
        public static bool CreateAndAddUser(string username, string password, Country location)
        {
            User newUser = new(username, password, location);
            return AddUser(newUser);
        }

        private static bool AddUser(IUser user)
        {
            try
            {
                Users.Add(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool SignInUser(string username, string password)
        {
            return true;
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
            MessageBox.Show("Invalid username and/or password", "Error");
            return false;
        }

    }
}
