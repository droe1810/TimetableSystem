using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TimetableSystem.Hubs;
using TimetableSystem.Models;
using TimetableSystem.Services;

namespace TimetableSystem.Pages.timetable
{
    public class AddManually2Model : PageModel
    {
        private readonly IHubContext<DocumentHub> _hubContext;

        public AddManually2Model(IHubContext<DocumentHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public void getData()
        {
            List<Room> listRoom = RoomService.GetAllRoom();
            ViewData["listRoom"] = listRoom;

            List<User> listTeacher = UserService.GetAllTeacher();
            ViewData["listTeacher"] = listTeacher;

            List<Course> listCourse = CourseService.GetAllCourse();
            ViewData["listCourse"] = listCourse;

            List<TimeslotType> listTimeslotType = TimeslotTypeService.GetAllTimeslotType();
            ViewData["listTimeslotType"] = listTimeslotType;

            List<Class> listClass = ClassService.GetAllClass();
            ViewData["listClass"] = listClass;
        }

        public IActionResult OnGet()
        {
            getData();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToWishList(string listTimetableDisplay, int classidadd2, int courseidadd2, int roomidadd2, int teacheridadd2, int timeslottypeidadd2)
        {
            Timetable expectedTT = new Timetable();

            expectedTT.Class = ClassService.GetClassById(classidadd2);
            expectedTT.Course = CourseService.GetCourseById(courseidadd2);
            expectedTT.Room = RoomService.GetRoomById(roomidadd2);
            expectedTT.Teacher = UserService.GetUserById(teacheridadd2);
            expectedTT.TimeslotType = TimeslotTypeService.GetTimeslotTypeById(timeslottypeidadd2);

            List<Timetable> listCheck = TimetableService.GetAllTimetable();
            List<Timetable> listTimetableToDisplay = JsonConvert.DeserializeObject<List<Timetable>>(listTimetableDisplay);

            if (listTimetableToDisplay != null && listTimetableToDisplay.Count >0)
            {
                foreach (var item in listTimetableToDisplay)
                {
                    if(item.Note == null || item.Note.Length == 0)
                    {
                        listCheck.Add(item);
                    }
                }
            }
            else
            {
                listTimetableToDisplay = new List<Timetable>();
            }

            foreach (var itemCheck in listCheck)
            {
                if (expectedTT.Teacher.Id == itemCheck.Teacher.Id && expectedTT.TimeslotType.Id == itemCheck.TimeslotType.Id)
                {
                    expectedTT.Note += $" {expectedTT.Teacher.Username} has been teaching in timeslot {expectedTT.TimeslotType.Name} -";
                }
                if (expectedTT.Class.Id == itemCheck.Class.Id && expectedTT.TimeslotType.Id == itemCheck.TimeslotType.Id)
                {
                    expectedTT.Note += $" {expectedTT.Class.Name} has been studing in timeslot {expectedTT.TimeslotType.Name} -";
                }
                if (expectedTT.Room.Id == itemCheck.Room.Id && expectedTT.TimeslotType.Id == itemCheck.TimeslotType.Id)
                {
                    expectedTT.Note += $" {expectedTT.Room.Name} has been booking in timeslot {expectedTT.TimeslotType.Name} -";
                }
                if (expectedTT.Class.Id == itemCheck.Class.Id && expectedTT.Course.Id == itemCheck.Course.Id)
                {
                    expectedTT.Note += $" {expectedTT.Class.Name} has taken the course {expectedTT.Course.Code} before -";
                }
            }

            if (expectedTT.Note == null || expectedTT.Note.Equals(""))
            {
                listCheck.Add(expectedTT);
            }
            else
            {
                int noteLegnth = expectedTT.Note.Length;
                expectedTT.Note = expectedTT.Note.Remove(noteLegnth - 1, 1);
            }

            ViewData["expectedTT"] = expectedTT;
            listTimetableToDisplay.Add(expectedTT);
            ViewData["listTimetable"] = listTimetableToDisplay;
            await _hubContext.Clients.All.SendAsync("ReloadDocuments");

            getData();
            return Page();
        }

        public async Task<IActionResult> OnPostSave(string listTimetableDisplay)
        {
            if (!string.IsNullOrEmpty(listTimetableDisplay))
            {
                List<Timetable> listDisplay = JsonConvert.DeserializeObject<List<Timetable>>(listTimetableDisplay);
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
