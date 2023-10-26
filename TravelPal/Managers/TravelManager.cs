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
            new Vacation(true, "New York", Country.USA, 2, DateTime.Today, DateTime.Today.AddDays(3), 1),
            new WorkTrip("Internal discussions", "Stockholm", Country.Sweden, 1, DateTime.Today, DateTime.Today.AddDays(2), 2)
        };

        public static void AddTravel(Travel travel)
        {
            Travels.Add(travel);
            if (UserManager.SignedInUser!.GetType() == typeof(User))
            {
                User signedInCustomer = (User)UserManager.SignedInUser;
                signedInCustomer.Travels.Add(travel);
            }
        }
        public static void RemoveTravel(Travel travelToRemove)
        {
            Travels.Remove(travelToRemove);
            if (UserManager.SignedInUser!.GetType() == typeof(User))
            {
                User signedInCustomer = (User)UserManager.SignedInUser;
                signedInCustomer.Travels.Remove(travelToRemove);
            }
            else if (UserManager.SignedInUser.GetType() == typeof(Admin))
            {
                RemoveFromUserTravelList(travelToRemove);
            }
        }
        public static int GetId()
        {
            return TravelId += 1;
        }

        public static IPackingListItem AddDefaultPackingListItem(Country departureCountry, Country arrivalCountry)
        {
            bool isDepartureCountryEuropean = CheckIfCountryIsEuropean(departureCountry);
            bool isArrivalCountryEuropean = CheckIfCountryIsEuropean(arrivalCountry);

            if (!isDepartureCountryEuropean || isDepartureCountryEuropean && !isArrivalCountryEuropean)
            {
                return new TravelDocument("Passport", true);
            }
            else
            {
                return new TravelDocument("Passport", false);
            }
        }

        public static bool CheckIfCountryIsEuropean(Country country)
        {
            foreach (Enum europeanCountry in Enum.GetValues(typeof(EuropeanCountry)))
            {
                if (country.ToString() == europeanCountry.ToString())
                {
                    return true;
                }
            }
            return false;
        }
        private static void RemoveFromUserTravelList(Travel travelToRemove)
        {
            //Denna snurra gör följande:
            //Går igenom användare för användare i 'databasen' (statiska listan i denna klass)
            //Om användaren är av typen "User", så går vi vidare och kollar igenom resorna denna user har i sin lista.
            //Vi jämför ID-numret för varje resa i userns lista mot ID-numret för den resa som skall tas bort.
            //Om ID-numret stämmer överens, tar vi bort den från användarens lista.
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
}
