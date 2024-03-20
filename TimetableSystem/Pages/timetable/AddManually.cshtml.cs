using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TimetableSystem.Hubs;
using TimetableSystem.Models;
using TimetableSystem.Services;

namespace TimetableSystem.Pages.timetable
{
    public class AddManuallyModel : PageModel
    {
        private readonly IHubContext<DocumentHub> _hubContext;
        public List<Class> Classes { get; set; } = null!;
        public List<Course> Courses { get; set; } = null!;
        public List<Room> Rooms { get; set; } = null!;
        public List<User> Teachers { get; set; } = null!;
        public List<TimeslotType> Timeslottypes { get; set; } = null!;


        [BindProperty]
        public int Classid { get; set; }

        [BindProperty]
        public int Courseid { get; set; }

        [BindProperty]
        public int Roomid { get; set; }

        [BindProperty]
        public int Teacherid { get; set; }

        [BindProperty]
        public int Timeslottypeid { get; set; }

        [BindProperty]
        public Timetable expectedTt { get; set; }
        public AddManuallyModel(IHubContext<DocumentHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public void getData()
        {
            Rooms = RoomService.GetAllRoom();
            Teachers = UserService.GetAllTeacher();
            Courses = CourseService.GetAllCourse();
            Timeslottypes = TimeslotTypeService.GetAllTimeslotType();
            Classes = ClassService.GetAllClass();
        }

        public IActionResult OnGet()
        {
            getData();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToWishList(string listTimetableDisplayJson)
        {
            expectedTt.Class = ClassService.GetClassById(Classid);
            expectedTt.Course = CourseService.GetCourseById(Courseid);
            expectedTt.Room = RoomService.GetRoomById(Roomid);
            expectedTt.Teacher = UserService.GetUserById(Teacherid);
            expectedTt.TimeslotType = TimeslotTypeService.GetTimeslotTypeById(Timeslottypeid);

            List<Timetable> listCheck = TimetableService.GetAllTimetable();
            List<Timetable> listTimetableDisplay = JsonConvert.DeserializeObject<List<Timetable>>(listTimetableDisplayJson);
            
            if (listTimetableDisplay != null)
            {
                foreach (var item in listTimetableDisplay)
                {
                    if(item.Note == null || item.Note.Length == 0)
                    {
                        listCheck.Add(item);
                    }
                }
            }
            else
            {
                listTimetableDisplay = new List<Timetable>();
            }

            foreach (var itemCheck in listCheck)
            {
                if (expectedTt.Teacher.Id == itemCheck.Teacher.Id && expectedTt.TimeslotType.Id == itemCheck.TimeslotType.Id)
                {
                    expectedTt.Note += $" {expectedTt.Teacher.Username} has been teaching in timeslot {expectedTt.TimeslotType.Name} -";
                }
                if (expectedTt.Class.Id == itemCheck.Class.Id && expectedTt.TimeslotType.Id == itemCheck.TimeslotType.Id)
                {
                    expectedTt.Note += $" {expectedTt.Class.Name} has been studing in timeslot {expectedTt.TimeslotType.Name} -";
                }
                if (expectedTt.Room.Id == itemCheck.Room.Id && expectedTt.TimeslotType.Id == itemCheck.TimeslotType.Id)
                {
                    expectedTt.Note += $" {expectedTt.Room.Name} has been booking in timeslot {expectedTt.TimeslotType.Name} -";
                }
                if (expectedTt.Class.Id == itemCheck.Class.Id && expectedTt.Course.Id == itemCheck.Course.Id)
                {
                    expectedTt.Note += $" {expectedTt.Class.Name} has taken the course {expectedTt.Course.Code} before -";
                }
            }

            if (expectedTt.Note == null || expectedTt.Note.Equals(""))
            {
                listCheck.Add(expectedTt);
            }
            else
            {
                int noteLegnth = expectedTt.Note.Length;
                expectedTt.Note = expectedTt.Note.Remove(noteLegnth - 1, 1);
            }

            listTimetableDisplay.Add(expectedTt);
            ViewData["listTimetableDisplay"] = listTimetableDisplay;
            await _hubContext.Clients.All.SendAsync("ReloadDocuments");

            getData();
            return Page();
        }

        public async Task<IActionResult> OnPostSave(string listTimetableDisplayJson)
        {
            if (!string.IsNullOrEmpty(listTimetableDisplayJson))
            {
                List<Timetable> listDisplay = JsonConvert.DeserializeObject<List<Timetable>>(listTimetableDisplayJson);
                List<Timetable> listToSave = new List<Timetable>();

                foreach (var itemDisplay in listDisplay)
                {
                    if (itemDisplay.Note == null || itemDisplay.Note.Equals(""))
                    {
                        listToSave.Add(itemDisplay);
                    }
                }

                foreach (var itemToSave in listToSave)
                {
                    Timetable item = new Timetable();
                    item.CourseId = itemToSave.Course.Id;
                    item.RoomId = itemToSave.Room.Id;
                    item.ClassId = itemToSave.Class.Id;
                    item.TeacherId = itemToSave.Teacher.Id;
                    item.TimeslotTypeId = itemToSave.TimeslotType.Id;

                    TimetableService.AddTimetable(item);

                }
            }
            ViewData["Msg"] = "Add successfully";
            getData();
            await _hubContext.Clients.All.SendAsync("ReloadDocuments");
            return Page();
        }
    }
}
