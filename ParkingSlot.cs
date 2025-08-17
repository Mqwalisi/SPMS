using System;
using System.Collections.Generic;
using System.Linq;
using SPMS;

namespace SPMS
{
    public enum SlotStatus { Free, Reserved, Occupied }

    public class ParkingSlot
    {
        public int SlotId { get; set; }
        public SlotStatus Status { get; set; }
    }
}