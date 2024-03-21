using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TimetableSystem.Models;
using TimetableSystem.Services;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using TimetableSystem.Hubs;

namespace TimetableSystem.Pages.teacher
{
    public class ViewModel : PageModel
    {
        private readonly prn221Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<DocumentHub> _hubContext;

        public ViewModel(IHttpContextAccessor httpContextAccessor, prn221Context context, IHubContext<DocumentHub> hubContext)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }
        public List<Time> Times { get; set; }
        public List<Slot> Slots { get; set; }
        public string[][] TimetableTimeSlot { get; set; }

        public IActionResult OnGet()
        {
            string userJson = _httpContextAccessor.HttpContext.Session.GetString("currentUser");
            if (userJson != null)
            {
                User u = JsonSerializer.Deserialize<User>(userJson);
                if(Authentication.IsTeacher(u))
                {
                    Times = TimeService.GetAllTime();
                    Slots = SlotService.GetAllSlot();

                    TimetableTimeSlot = new string[Times.Count][];
                    for (int i = 0; i < Times.Count; i++)
                    {
                        TimetableTimeSlot[i] = new string[Slots.Count];
                    }

                    for (int i = 0; i < Times.Count; i++)
                    {
                        for (int j = 0; j < Slots.Count; j++)
                        {
                            Timetable t = GetTimetableOfTeacherByTimeIdAndSlotId(u.Id, Times[i].Id, Slots[j].Id);
                            TimetableTimeSlot[i][j] = GetActivity(t);
                        }
                    }
                }
                else
                {
                    return Redirect("/AccessDenied");
                }

            }
            else
            {
                return Redirect("/AccessDenied");
            }
            return Page();
        }

        public Timetable GetTimetableOfTeacherByTimeIdAndSlotId(int teacherId, int timeId, int slotId)
        {
            var timetable = _context.Timetables
                .FromSqlRaw($"SELECT TOP 1 * FROM Timetable WHERE timeslotTypeId IN (SELECT typeId FROM Timeslot WHERE timeId = {timeId} AND slotId = {slotId}) AND teacherId = {teacherId}")
                .FirstOrDefault();

            if (timetable == null)
            {
                return null;
            }
            timetable.Class = ClassService.GetClassById(timetable.ClassId);
            timetable.Course = CourseService.GetCourseById(timetable.CourseId);
            timetable.Room = RoomService.GetRoomById(timetable.RoomId);

            return timetable;
        }

        private string GetActivity(Timetable t)
        {
            if (t == null)
            {
                return " ";
            }
            //string activity = $"{t.Class.Name}</br>{t.Course.Code}</br>{t.Room.Name}";
            string activity = $"{t.Class.Name} - {t.Course.Code} - {t.Room.Name}";

            return activity;
        }
    }
}