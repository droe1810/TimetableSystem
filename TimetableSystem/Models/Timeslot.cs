using System;
using System.Collections.Generic;

namespace TimetableSystem.Models
{
    public partial class Timeslot
    {
        public int TimeId { get; set; }
        public int SlotId { get; set; }
        public int? TypeId { get; set; }

        public virtual Slot Slot { get; set; } = null!;
        public virtual Time Time { get; set; } = null!;
        public virtual TimeslotType? Type { get; set; }
    }
}
