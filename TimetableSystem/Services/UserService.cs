using TimetableSystem.Models;

namespace TimetableSystem.Services
{
    public class UserService
    {
        public static List<User> GetAllTeacher()
        {
            using (var context = new prn221Context())
            {
                return context.Users.Where(u => u.RoleId == 2).ToList();
            }
        }
        public static User GetUserByName(string name)
        {
            using (var context = new prn221Context())
            {
                return context.Users.FirstOrDefault(u => u.Username.ToLower().Equals(name.ToLower()));
            }
        }
        public static User GetUserById(int id)
        {
            using (var context = new prn221Context())
            {
                return context.Users.FirstOrDefault(u => u.Id == id);
            }
        }
    }
}
