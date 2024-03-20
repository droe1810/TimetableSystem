using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using TimetableSystem.Hubs;
using TimetableSystem.Models;
using TimetableSystem.Services;

namespace TimetableSystem.Pages.timetable
{
    public class EditModel : PageModel
    {
        private readonly IHubContext<DocumentHub> _hubContext;
        public List<Timetable> Timetables { get; set; } = null!;
        public List<Class> Classes { get; set; } = null!;
        public List<Course> Courses { get; set; } = null!;
        public List<Room> Rooms { get; set; } = null!;
        public List<User> Teachers { get; set; } = null!;
        public List<TimeslotType> Timeslottypes { get; set; } = null!;
        public Timetable oldTimetable { get; set; } = null!;

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

        public EditModel(IHubContext<DocumentHub> hubContext)
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
        public IActionResult OnGet(int timetableid)
        {
            oldTimetable = TimetableService.GetTimetableById(timetableid);
            Classid = oldTimetable.ClassId; 
            Courseid = oldTimetable.CourseId;
            Roomid = oldTimetable.RoomId; 
            Teacherid = oldTimetable.TeacherId;
            Timeslottypeid = oldTimetable.TimeslotTypeId;
            getData();

            return Page();
        }

        public async Task<IActionResult> OnPostEdit(int timetableid)
        {
            oldTimetable = TimetableService.GetTimetableById(timetableid);

            expectedTt.Class = ClassService.GetClassById(Classid);
            expectedTt.Course = CourseService.GetCourseById(Courseid);
            expectedTt.Room = RoomService.GetRoomById(Roomid);
            expectedTt.Teacher = UserService.GetUserById(Teacherid);
            expectedTt.TimeslotType = TimeslotTypeService.GetTimeslotTypeById(Timeslottypeid);

            if (expectedTt.ClassId == oldTimetable.ClassId && expectedTt.CourseId == oldTimetable.CourseId && expectedTt.RoomId == oldTimetable.RoomId && expectedTt.TeacherId == oldTimetable.TeacherId && expectedTt.TimeslotTypeId == oldTimetable.TimeslotTypeId)
            {
                expectedTt.Note += " No Data change ";
            }
            else
            {
                List<Timetable> listCheck = TimetableService.GetAllTimetable();
                listCheck.RemoveAll(tt => tt.Id == oldTimetable.Id);

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
            }


            if (expectedTt.Note == null || expectedTt.Note.Equals(""))
            {
                expectedTt.Id = timetableid;
                expectedTt.ClassId = expectedTt.Class.Id;
                expectedTt.CourseId = expectedTt.Course.Id;
                expectedTt.RoomId = expectedTt.Room.Id;
                expectedTt.TeacherId = expectedTt.Teacher.Id;
                expectedTt.TimeslotTypeId = expectedTt.TimeslotType.Id;
                TimetableService.EditTimetable(expectedTt);
                expectedTt.Note = "Edit success";
            }
            else
            {
                int noteLegnth = expectedTt.Note.Length;
                expectedTt.Note = expectedTt.Note.Remove(noteLegnth - 1, 1);
            }
        
            await _hubContext.Clients.All.SendAsync("ReloadDocuments");

            getData();
            return Page();
        }
    }
}
