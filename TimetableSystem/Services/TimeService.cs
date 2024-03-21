using TimetableSystem.Models;

namespace TimetableSystem.Services
{
    public class TimeService
    {
        public static List<Time> GetAllTime()
        {
            using (var context = new prn221Context())
            {
                return context.Times.ToList();
            }
        }
    }
}
