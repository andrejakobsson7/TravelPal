using System.Collections.Generic;

namespace TravelPal.Models
{
    public class User : IUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Country Location { get; set; }
        public List<Travel> Travels { get; set; }

        public User(string username, string password, Country location)
        {
            Username = username;
            Password = password;
            Location = location;
            Travels = new List<Travel>();
        }
        public User(string username, string password, Country location, List<Travel> travels)
        {
            Username = username;
            Password = password;
            Location = location;
            Travels = travels;
        }
    }
}
