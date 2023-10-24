using System;

namespace TravelPal.Models
{
    public class WorkTrip : Travel
    {
        public string MeetingDetails { get; set; }

        public WorkTrip(string meetingDetails, string destination, Country country, int travellers, DateTime startDate, DateTime endDate) : base(destination, country, travellers, startDate, endDate)
        {
            MeetingDetails = meetingDetails;
        }
        public override string GetInfo()
        {
            return $"Work trip to: {Destination}, {Country}";
        }
    }
}
