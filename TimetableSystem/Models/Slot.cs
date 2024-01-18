using System;
using System.Collections.Generic;

namespace TimetableSystem.Models
{
    public partial class Slot
    {
        public Slot()
        {
            Timeslots = new HashSet<Timeslot>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public virtual ICollection<Timeslot> Timeslots { get; set; }
    }
}
