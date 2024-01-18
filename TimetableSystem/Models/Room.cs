using System;
using System.Collections.Generic;

namespace TimetableSystem.Models
{
    public partial class Room
    {
        public Room()
        {
            Timetables = new HashSet<Timetable>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
