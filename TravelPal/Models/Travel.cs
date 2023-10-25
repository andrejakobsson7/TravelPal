using System;
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

        public Travel(string destination, Country country, int travellers, DateTime startDate, DateTime endDate)
        {
            Destination = destination;
            Country = country;
            Travellers = travellers;
            StartDate = startDate;
            EndDate = endDate;
            TravelDays = CalculateTravelDays(startDate, endDate);
            Id = TravelManager.GetId();
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
