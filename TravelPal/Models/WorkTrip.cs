﻿using System;
using System.Collections.Generic;

namespace TravelPal.Models
{
    public class WorkTrip : Travel
    {
        public string MeetingDetails { get; set; }

        public WorkTrip(string meetingDetails, string destination, Country country, int travellers, DateTime startDate, DateTime endDate) : base(destination, country, travellers, startDate, endDate)
        {
            MeetingDetails = meetingDetails;
        }
        public WorkTrip(string meetingDetails, string destination, Country country, int travellers, DateTime startDate, DateTime endDate, List<IPackingListItem> packingList) : base(destination, country, travellers, startDate, endDate, packingList)
        {
            MeetingDetails = meetingDetails;
        }
        public override string GetInfo()
        {
            return $"{Destination}, {Country} (Work trip)";
        }
    }
}
