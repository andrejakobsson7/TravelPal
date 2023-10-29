using System;
using System.Collections.Generic;

namespace TravelPal.Models
{
    class Vacation : Travel
    {
        public bool AllInclusive { get; set; }

        //Konstruktor med packlista
        public Vacation(bool isAllInclusive, string destination, Country country, int travellers, DateTime startDate, DateTime endDate, List<IPackingListItem> packingList) : base(destination, country, travellers, startDate, endDate, packingList)
        {
            AllInclusive = isAllInclusive;
        }
        public Vacation(int id, bool isAllInclusive, string destination, Country country, int travellers, DateTime startDate, DateTime endDate, List<IPackingListItem> packingList) : base(id, destination, country, travellers, startDate, endDate, packingList)
        {
            AllInclusive = isAllInclusive;
        }
        public Vacation()
        {

        }

        //Konstruktor när man lägger till resa manuellt.
        public override string GetInfo()
        {
            return $"{Destination}, {Country} (Vacation)";
        }
    }
}
