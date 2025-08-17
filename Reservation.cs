using System;
using System.Collections.Generic;
using System.Linq;
using SPMS;

namespace SPMS
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public User User { get; set; }
        public ParkingSlot Slot { get; set; }
        public string Status { get; set; } // "Active" or "Cancelled"
    }
}