using TimetableSystem.Models;

namespace TimetableSystem.Services
{
    public class TimeslotTypeService
    {
        public static List<TimeslotType> GetAllTimeslotType()
        {
            using (var context = new prn221Context())
            {
                return context.TimeslotTypes.ToList();
            }
        }
        public static TimeslotType GetTimeslotTypeByName(string name)
        {
            using (var context = new prn221Context())
            {
                return context.TimeslotTypes.FirstOrDefault(t => t.Name.ToLower().Equals(name.ToLower()));
            }
        }
        public static TimeslotType GetTimeslotTypeById(int id)
        {
            using (var context = new prn221Context())
            {
                return context.TimeslotTypes.FirstOrDefault(tts => tts.Id == id);
            }
        }

    }
}
