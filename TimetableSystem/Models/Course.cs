using System;
using System.Collections.Generic;

namespace TimetableSystem.Models
{
    public partial class Course
    {
        public Course()
        {
            Timetables = new HashSet<Timetable>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
