using System;
using System.Collections.Generic;

namespace TravelPal.Models
{
    class Vacation : Travel
    {
        public bool AllInclusive { get; set; }

        //Konstruktor utan packlista
        public Vacation(bool isAllInclusive, string destination, Country country, int travellers, DateTime startDate, DateTime endDate) : base(destination, country, travellers, startDate, endDate)
        {
            AllInclusive = isAllInclusive;
        }

        //Konstruktor med packlista
        public Vacation(bool isAllInclusive, string destination, Country country, int travellers, DateTime startDate, DateTime endDate, List<IPackingListItem> packingList) : base(destination, country, travellers, startDate, endDate, packingList)
        {
            AllInclusive = isAllInclusive;
        }

        //Konstruktor när man lägger till resa manuellt.
        public Vacation(bool isAllInclusive, string destination, Country country, int travellers, DateTime startDate, DateTime endDate, int id) : base(destination, country, travellers, startDate, endDate, id)
        {
            AllInclusive = isAllInclusive;
        }
        public override string GetInfo()
        {
            return $"{Destination}, {Country} (Vacation)";
        }
    }
}
