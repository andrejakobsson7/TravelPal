using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public static class TravelManager
    {
        private static int TravelId;
        public static List<Travel> Travels { get; set; } = new()
        {
            new Vacation
            {
                AllInclusive = true,
                Destination = "New York",
                Country = Country.USA,
                Travellers = 2,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(3),
                PackingList = new List<IPackingListItem>
                {
                    new TravelDocument("Passport", true)
                }
            },
            new WorkTrip
            {
                MeetingDetails = "Internal discussions",
                Destination = "Stockholm",
                Country = Country.Sweden,
                Travellers = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                PackingList = new List<IPackingListItem>
                {
                    new OtherItem("Computer", 1)

                }
            }
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
        public static int GetNextId()
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
        private static bool RemoveFromUserTravelList(Travel travelToRemove)
        {
            //Denna snurra gör följande:
            //Går igenom användare för användare i 'databasen' (statiska listan i denna klass)
            //Om användaren är av typen "User", så går vi vidare och kollar igenom resorna denna user har i sin lista.
            //Vi jämför ID-numret för varje resa i userns lista mot ID-numret för den resa som skall tas bort.
            //Om ID-numret stämmer överens, tar vi bort den från användarens lista.
            foreach (IUser user in UserManager.Users)
            {
                if (user.GetType() == typeof(User))
                {
                    User userToCheck = (User)user;
                    for (int i = 0; i < userToCheck.Travels.Count; i++)
                        if (userToCheck.Travels[i].Id == travelToRemove.Id)
                        {
                            userToCheck.Travels.RemoveAt(i);
                            return true;
                        }
                }
            }
            return false;
        }

        //Hur kan jag göra denna metod mer generell?
        public static bool ValidateStringInput(string input, object sender)
        {
            if (string.IsNullOrEmpty(input) && sender is Button)
            {
                Button btn = (Button)sender;
                if (btn.Name == "btnAdd" || btn.Name == "btnSave")
                {
                    MessageBox.Show("No destination has been entered");
                    return false;
                }
                else if (btn.Name == "btnAddItemToPackingList")
                {
                    MessageBox.Show("No item has been entered");
                    return false;
                }
            }
            return true;
        }

        public static bool ValidateSelectedItemInComboBox(object value, ComboBox selectedCombobox)
        {
            if (value == null)
            {
                //Felmeddelandet är generiskt och hämtar typnamnet för första enum i listan som comboboxen avser och displayar i felmeddelandet.
                MessageBox.Show($"No {selectedCombobox.Items[0].GetType().Name} has been selected");
                return false;
            }
            return true;
        }

        public static bool ValidateIntInput(string input)
        {
            try
            {
                int convertedInput = int.Parse(input);
                if (convertedInput <= 0)
                {
                    throw new ArgumentOutOfRangeException("Number must be at least 1");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show($"{input} is not a valid number");
                return false;
            }
            catch (OverflowException)
            {
                MessageBox.Show($"{input} is a too large or small number");
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }
        public static bool ValidateSelectedDatesInDatePickers(object? startDate, object? endDate)
        {
            if (startDate == null)
            {
                MessageBox.Show("No start date has been entered");
                return false;
            }
            else if (endDate == null)
            {
                MessageBox.Show("No end date has been entered");
                return false;
            }
            return ValidateDates((DateTime)startDate, (DateTime)endDate);
        }
        private static bool ValidateDates(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                MessageBox.Show("End date must be after start date");
                return false;
            }
            else if (startDate < DateTime.Today)
            {
                MessageBox.Show("Start date can't be before todays' date");
                return false;
            }
            return true;
        }
        public static bool ValidateMeetingDetails(string meetingDetails)
        {
            if (string.IsNullOrEmpty(meetingDetails))
            {
                MessageBox.Show("No meeting details has been entered");
                return false;
            }
            return true;
        }

        public static List<IPackingListItem> GetPackingList(ListBox packingList)
        {
            List<IPackingListItem> userPackingList = new();
            foreach (ListBoxItem item in packingList.Items)
            {
                IPackingListItem packingListItem = (IPackingListItem)item.Tag;
                userPackingList.Add(packingListItem);
            }
            return userPackingList;
        }

        public static void AddItemToPackingList(IPackingListItem document, ListBox packingList)
        {
            ListBoxItem item = new();
            item.Tag = document;
            item.Content = document.GetInfo();
            packingList.Items.Add(item);
        }
        public static void ConfirmSuccessfullyRegisteredTravel(Travel travel)
        {
            MessageBox.Show($"New travel to {travel.Destination} has been registered!");
        }

    }
}
