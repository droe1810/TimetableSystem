using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimetableSystem.Models
{
    public partial class Timetable
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int CourseId { get; set; }
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public int TimeslotTypeId { get; set; }

        [NotMapped]
        public string Note { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;
        public virtual Room Room { get; set; } = null!;
        public virtual User Teacher { get; set; } = null!;
        public virtual TimeslotType TimeslotType { get; set; } = null!;
    }
}
