using TimetableSystem.Models;

namespace TimetableSystem.Services
{
    public class ClassService
    {
        public static List<Class> GetAllClass()
        {
            List<Class> classes = new List<Class>();
            using (var context = new prn221Context())
            {
                classes = context.Classes.ToList();
            }
            return classes;
        }
        public static Class GetClassByName(string name)
        {
            using (var context = new prn221Context())
            {
                return context.Classes.FirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower()));
            }
        }
    }
}
