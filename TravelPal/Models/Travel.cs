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
        public int TravelDays { get { return CalculateTravelDays(StartDate, EndDate); } }
        public List<IPackingListItem> PackingList { get; set; }

        public Travel(string destination, Country country, int travellers, DateTime startDate, DateTime endDate, List<IPackingListItem> packingList)
        {
            Id = TravelManager.GetNextId();
            Destination = destination;
            Country = country;
            Travellers = travellers;
            StartDate = startDate;
            EndDate = endDate;
            PackingList = packingList;
        }

        //Constructor som används när man uppdaterar en befintlig resa. För att säkerställa att resan får samma ID-nummer som den som den ersätter, skickar man med ID-numret.
        public Travel(int id, string destination, Country country, int travellers, DateTime startDate, DateTime endDate, List<IPackingListItem> packingList)
        {
            Id = id;
            Destination = destination;
            Country = country;
            Travellers = travellers;
            StartDate = startDate;
            EndDate = endDate;
            PackingList = packingList;
        }
        public Travel()
        {
            Id = TravelManager.GetNextId();
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
