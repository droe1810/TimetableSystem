using System;
using System.Collections.Generic;

namespace TimetableSystem.Models
{
    public partial class Class
    {
        public Class()
        {
            Timetables = new HashSet<Timetable>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
