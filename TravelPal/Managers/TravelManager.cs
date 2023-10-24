using System.Collections.Generic;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public static class TravelManager
    {
        public static List<Travel> Travels { get; set; } = new();

        public static void AddTravel(Travel travel)
        {
            Travels.Add(travel);
            User signedInCustomer = (User)UserManager.SignedInUser;
            signedInCustomer.Travels.Add(travel);
        }
    }
}
