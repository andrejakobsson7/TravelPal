using System;

namespace TravelPal.Models
{
    class Vacation : Travel
    {
        public bool AllInclusive { get; set; }

        public Vacation(bool isAllInclusive, string destination, Country country, int travellers, DateTime startDate, DateTime endDate) : base(destination, country, travellers, startDate, endDate)
        {
            AllInclusive = isAllInclusive;
        }
        public override string GetInfo()
        {
            return $"Vacation to: {Destination}, {Country}";
        }
    }
}
