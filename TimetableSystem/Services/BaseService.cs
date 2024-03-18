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

        public static List<Class> GetAllClass()
        {
            List<Class> classes = new List<Class>();
            using (var context = new prn221Context())
            {
                classes = context.Classes.ToList();
            }
            return classes; 
        }

        public static List<Room> GetAllRoom()
        {
            using (var context = new prn221Context())
            {
                return context.Rooms.ToList();
            }
        }

        public static List<User> GetAllTeacher()
        {
            using (var context = new prn221Context())
            {
                return context.Users.Where(u => u.RoleId == 2).ToList();
            }
        }

        public static List<Course> GetAllCourse()
        {
            using (var context = new prn221Context())
            {
                return context.Courses.ToList();
            }
        }

        public static List<TimeslotType> GetAllTimeslotType()
        {
            using (var context = new prn221Context())
            {
                return context.TimeslotTypes.ToList();
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
