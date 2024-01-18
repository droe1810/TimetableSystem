using System;
using System.Collections.Generic;

namespace TimetableSystem.Models
{
    public partial class TimeslotType
    {
        public TimeslotType()
        {
            Timeslots = new HashSet<Timeslot>();
            Timetables = new HashSet<Timetable>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Timeslot> Timeslots { get; set; }
        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
