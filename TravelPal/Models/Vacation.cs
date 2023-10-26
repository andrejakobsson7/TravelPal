using System;
using System.Collections.Generic;

namespace TravelPal.Models
{
    class Vacation : Travel
    {
        public bool AllInclusive { get; set; }

        public Vacation(bool isAllInclusive, string destination, Country country, int travellers, DateTime startDate, DateTime endDate) : base(destination, country, travellers, startDate, endDate)
        {
            AllInclusive = isAllInclusive;
        }
        public Vacation(bool isAllInclusive, string destination, Country country, int travellers, DateTime startDate, DateTime endDate, List<IPackingListItem> packingList) : base(destination, country, travellers, startDate, endDate, packingList)
        {
            AllInclusive = isAllInclusive;
        }
        public override string GetInfo()
        {
            return $"{Destination}, {Country} (Vacation)";
        }
    }
}
