using TimetableSystem.Models;

namespace TimetableSystem.Services
{
    public class SlotService
    {
        public static List<Slot> GetAllSlot()
        {
            using (var context = new prn221Context())
            {
                return context.Slots.ToList();
            }
        }
    }
}
