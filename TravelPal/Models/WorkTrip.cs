using System;
using System.Collections.Generic;

namespace TravelPal.Models
{
    public class WorkTrip : Travel
    {
        public string MeetingDetails { get; set; }

        //Konstruktor med packlista
        public WorkTrip(string meetingDetails, string destination, Country country, int travellers, DateTime startDate, DateTime endDate, List<IPackingListItem> packingList) : base(destination, country, travellers, startDate, endDate, packingList)
        {
            MeetingDetails = meetingDetails;
        }
        public WorkTrip(int id, string meetingDetails, string destination, Country country, int travellers, DateTime startDate, DateTime endDate, List<IPackingListItem> packingList) : base(id, destination, country, travellers, startDate, endDate, packingList)
        {
            MeetingDetails = meetingDetails;
        }

        public WorkTrip()
        {

        }

        public override string GetInfo()
        {
            return $"{Destination}, {Country} (Work trip)";
        }
    }
}
