using System;
using System.Collections.Generic;

namespace TimetableSystem.Models
{
    public partial class User
    {
        public User()
        {
            Timetables = new HashSet<Timetable>();
        }

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Timetable> Timetables { get; set; }

        public bool isAdmin(int? roleId)
        {
            if (roleId == null || roleId != 1)
            {
                return false;
            }
            else { return true; }
        }

        public bool isTeacher(int? roleId)
        {
            if (roleId == null || roleId != 2)
            {
                return false;
            }
            else { return true; }
        }
    }
}
