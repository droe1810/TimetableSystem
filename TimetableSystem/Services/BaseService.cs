using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TimetableSystem.Models;

namespace TimetableSystem.Services
{
    public class BaseService
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
                oldTimetable = GetTimetableById (newTimetable.Id);
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

        public static Room GetRoomByName(string name)
        {
            using (var context = new prn221Context())
            {
                return context.Rooms.FirstOrDefault(r => r.Name.ToLower().Equals(name.ToLower()));
            }
        }


        public static TimeslotType GetTimeslotTypeByName(string name)
        {
            using (var context = new prn221Context())
            {
                return context.TimeslotTypes.FirstOrDefault(t => t.Name.ToLower().Equals(name.ToLower()));
            }
        }

        public static Course GetCourseByCode(string code)
        {
            using (var context = new prn221Context())
            {
                return context.Courses.FirstOrDefault(c => c.Code.ToLower().Equals(code.ToLower()));
            }
        }

        public static Class GetClassByName(string name)
        {
            using (var context = new prn221Context())
            {
                return context.Classes.FirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower()));
            }
        }

        public static User GetUserByName(string name)
        {
            using (var context = new prn221Context())
            {
                return context.Users.FirstOrDefault(u => u.Username.ToLower().Equals(name.ToLower()));
            }
        }

    }
}
