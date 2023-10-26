using System;
using System.Collections.Generic;
using TravelPal.Managers;

namespace TravelPal.Models
{
    public class Travel
    {
        public int Id { get; set; }
        public string Destination { get; set; }
        public Country Country { get; set; }
        public int Travellers { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TravelDays { get; }
        public List<IPackingListItem> PackingList { get; set; }

        public Travel(string destination, Country country, int travellers, DateTime startDate, DateTime endDate)
        {
            Id = TravelManager.GetId();
            Destination = destination;
            Country = country;
            Travellers = travellers;
            StartDate = startDate;
            EndDate = endDate;
            TravelDays = CalculateTravelDays(startDate, endDate);
        }
        public Travel(string destination, Country country, int travellers, DateTime startDate, DateTime endDate, List<IPackingListItem> packingList)
        {
            Destination = destination;
            Country = country;
            Travellers = travellers;
            StartDate = startDate;
            EndDate = endDate;
            TravelDays = CalculateTravelDays(startDate, endDate);
            Id = TravelManager.GetId();
            PackingList = packingList;
        }

        //Konstruktorn nedanför används när vi lägger upp resor manuellt. Då lägger vi in ID-nummer själv,
        //men ökar ändå på TravelId med ett så att när man lägger till en resa "den vanliga vägen" så kan man inte få samma ID-nr.
        public Travel(string destination, Country country, int travellers, DateTime startDate, DateTime endDate, int id)
        {
            Id = id;
            Destination = destination;
            Country = country;
            Travellers = travellers;
            StartDate = startDate;
            EndDate = endDate;
            TravelDays = CalculateTravelDays(startDate, endDate);
            PackingList = new List<IPackingListItem>();
            TravelManager.TravelId += 1;
        }

        private int CalculateTravelDays(DateTime startDate, DateTime endDate)
        {
            return (int)(endDate - startDate).TotalDays;
        }

        public virtual string GetInfo()
        {
            return $"{Destination}, {Country}";
        }
    }
}
