using TimetableSystem.Models;

namespace TimetableSystem.Services
{
    public class CourseService
    {
        public static List<Course> GetAllCourse()
        {
            using (var context = new prn221Context())
            {
                return context.Courses.ToList();
            }
        }
        public static Course GetCourseByCode(string code)
        {
            using (var context = new prn221Context())
            {
                return context.Courses.FirstOrDefault(c => c.Code.ToLower().Equals(code.ToLower()));
            }
        }

        public static Course GetCourseById(int id)
        {
            using (var context = new prn221Context())
            {
                return context.Courses.FirstOrDefault(c => c.Id == id);
            }
        }
    }
}
