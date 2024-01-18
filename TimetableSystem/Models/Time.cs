using System;
using System.Collections.Generic;

namespace TimetableSystem.Models
{
    public partial class Time
    {
        public Time()
        {
            Timeslots = new HashSet<Timeslot>();
        }

        public int Id { get; set; }
        public string? Date { get; set; }

        public virtual ICollection<Timeslot> Timeslots { get; set; }
    }
}
