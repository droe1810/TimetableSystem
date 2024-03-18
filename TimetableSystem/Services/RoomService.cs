using TimetableSystem.Models;

namespace TimetableSystem.Services
{
    public class RoomService
    {
        public static List<Room> GetAllRoom()
        {
            using (var context = new prn221Context())
            {
                return context.Rooms.ToList();
            }
        }
        public static Room GetRoomByName(string name)
        {
            using (var context = new prn221Context())
            {
                return context.Rooms.FirstOrDefault(r => r.Name.ToLower().Equals(name.ToLower()));
            }
        }

    }
}
