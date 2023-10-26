using System;
using System.Collections.Generic;

namespace TravelPal.Models
{
    public class WorkTrip : Travel
    {
        public string MeetingDetails { get; set; }

        //Konstruktor utan packlista

        public WorkTrip(string meetingDetails, string destination, Country country, int travellers, DateTime startDate, DateTime endDate) : base(destination, country, travellers, startDate, endDate)
        {
            MeetingDetails = meetingDetails;
        }
        //Konstruktor med packlista
        public WorkTrip(string meetingDetails, string destination, Country country, int travellers, DateTime startDate, DateTime endDate, List<IPackingListItem> packingList) : base(destination, country, travellers, startDate, endDate, packingList)
        {
            MeetingDetails = meetingDetails;
        }
        //Konstruktor när man lägger till resa manuellt.
        public WorkTrip(string meetingDetails, string destination, Country country, int travellers, DateTime startDate, DateTime endDate, int id) : base(destination, country, travellers, startDate, endDate, id)
        {
            MeetingDetails = meetingDetails;
        }

        public override string GetInfo()
        {
            return $"{Destination}, {Country} (Work trip)";
        }
    }
}
