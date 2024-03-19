using Microsoft.EntityFrameworkCore;
using TimetableSystem.Models;

namespace TimetableSystem.Services
{
    public class TimetableService
    {
        public static List<Timetable> GetAllTimetable()
        {
            using (var context = new prn221Context())
            {
                return context.Timetables
                    .Include(x => x.Class)
                    .Include(x => x.Course)
                    .Include(x => x.Room)
                    .Include(x => x.Teacher)
                    .Include(x => x.TimeslotType)
                    .ToList();
            }
        }

        public static Timetable GetTimetableById(int id)
        {
            using (var context = new prn221Context())
            {
                return context.Timetables
                    .Include(x => x.Class)
                    .Include(x => x.Course)
                    .Include(x => x.Room)
                    .Include(x => x.Teacher)
                    .Include(x => x.TimeslotType)
                    .FirstOrDefault(i => i.Id == id);
            }
        }

        public static List<Timetable> FilterListTimetable(int classid, int courseid, int roomid, int teacherid, int timeslottypeid)
        {
            List<Timetable> list = new List<Timetable>();

            using (var context = new prn221Context())
            {
                var query = context.Timetables.AsQueryable();

                if (classid != 0)
                {
                    query = query.Where(t => t.ClassId == classid);
                }
                if (courseid != 0)
                {
                    query = query.Where(t => t.CourseId == courseid);
                }
                if (roomid != 0)
                {
                    query = query.Where(t => t.RoomId == roomid);
                }
                if (teacherid != 0)
                {
                    query = query.Where(t => t.TeacherId == teacherid);
                }
                if (timeslottypeid != 0)
                {
                    query = query.Where(t => t.TimeslotTypeId == timeslottypeid);
                }

                query = query.Include(t => t.Class)
                             .Include(t => t.Course)
                             .Include(t => t.Room)
                             .Include(t => t.Teacher)
                             .Include(t => t.TimeslotType);

                list = query.ToList();
            }

            return list;
        }

        public static void AddTimetable(Timetable timetable)
        {
            using (var context = new prn221Context())
            {
                context.Timetables.Add(timetable);
                context.SaveChanges();
            }
        }

        public static void EditTimetable(Timetable newTimetable)
        {
            Timetable oldTimetable = new Timetable();
            using (var context = new prn221Context())
            {
                oldTimetable = context.Timetables.FirstOrDefault(t => t.Id == newTimetable.Id);
                oldTimetable.RoomId = newTimetable.RoomId;
                oldTimetable.CourseId = newTimetable.CourseId;
                oldTimetable.TimeslotTypeId = newTimetable.TimeslotTypeId;
                oldTimetable.TeacherId = newTimetable.TeacherId;
                oldTimetable.ClassId = newTimetable.ClassId;

                context.SaveChanges();
            }
        }

        public static void DeleteTimetable(int id)
        {
            using (var context = new prn221Context())
            {
                var timetable = GetTimetableById(id);
                if (timetable != null)
                {
                    context.Timetables.Remove(timetable);
                    context.SaveChanges();
                }
            }
        }
    }
}
