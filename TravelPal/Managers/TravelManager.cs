using System;
using System.Collections.Generic;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public static class TravelManager
    {
        public static int TravelId;
        public static List<Travel> Travels { get; set; } = new()
        {
            new Vacation(true, "New York", Country.USA, 2, DateTime.Today, DateTime.Today.AddDays(3)),
            new WorkTrip("Internal discussions", "Stockholm", Country.Sweden, 1, DateTime.Today, DateTime.Today.AddDays(2))
        };

        public static void AddTravel(Travel travel)
        {
            Travels.Add(travel);
            User signedInCustomer = (User)UserManager.SignedInUser;
            signedInCustomer.Travels.Add(travel);
        }
        public static void RemoveTravel(Travel travelToRemove)
        {
            Travels.Remove(travelToRemove);
            if (UserManager.SignedInUser.GetType() == typeof(User))
            {
                User signedInCustomer = (User)UserManager.SignedInUser;
                signedInCustomer.Travels.Remove(travelToRemove);
            }
            else if (UserManager.SignedInUser.GetType() == typeof(Admin))
            {
                //Hitta resan med hjälp av ID-numret.
                //Gå igenom alla users listor med resor och leta efter ID-numret och ta bort det när det hittas, så att det både tas bort från 'databasen',
                //d.v.s. den statiska listan i denna klassen och användarens egna lista.
                bool hasFoundTravelId = false;
                foreach (IUser user in UserManager.Users)
                {
                    while (!hasFoundTravelId)
                    {
                        if (user.GetType() == typeof(User))
                        {
                            User userToCheck = (User)user;
                            for (int i = 0; i < userToCheck.Travels.Count; i++)
                                if (userToCheck.Travels[i].Id == travelToRemove.Id)
                                {
                                    userToCheck.Travels.RemoveAt(i);
                                    hasFoundTravelId = true;
                                    break;
                                }
                        }
                    }
                }
            }
        }
        public static int GetId()
        {
            return TravelId += 1;
        }
    }
}
