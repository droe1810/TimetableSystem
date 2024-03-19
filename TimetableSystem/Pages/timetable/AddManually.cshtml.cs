using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TimetableSystem.Models;
using TimetableSystem.Services;

namespace TimetableSystem.Pages.timetable
{
    public class AddManuallyModel : PageModel
    {
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

        public IActionResult OnPostAdd(int classidadd, int courseidadd, int roomidadd, int teacheridadd, int timeslottypeidadd)
        {
            Timetable expectedTT = new Timetable();
            expectedTT.Class = ClassService.GetClassById(classidadd);
            expectedTT.Course = CourseService.GetCourseById(courseidadd);
            expectedTT.Room = RoomService.GetRoomById(roomidadd);
            expectedTT.Teacher = UserService.GetUserById(teacheridadd);
            expectedTT.TimeslotType = TimeslotTypeService.GetTimeslotTypeById(timeslottypeidadd);

            List<Timetable> listCheck = TimetableService.GetAllTimetable();
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
                expectedTT.ClassId = expectedTT.Class.Id;
                expectedTT.CourseId = expectedTT.Course.Id;
                expectedTT.RoomId = expectedTT.Room.Id;
                expectedTT.TeacherId = expectedTT.Teacher.Id;
                expectedTT.TimeslotTypeId = expectedTT.TimeslotType.Id;

                Timetable temp = new Timetable();
                temp.Class = expectedTT.Class;
                temp.Course = expectedTT.Course;
                temp.Room = expectedTT.Room;
                temp.TimeslotType = expectedTT.TimeslotType;
                temp.Teacher = expectedTT.Teacher;

                expectedTT.Class = null;
                expectedTT.Course = null;
                expectedTT.Room = null;
                expectedTT.Teacher = null;
                expectedTT.TimeslotType = null;

                TimetableService.AddTimetable(expectedTT);

                expectedTT.Class = temp.Class;
                expectedTT.Course = temp.Course;
                expectedTT.Room = temp.Room;
                expectedTT.TimeslotType = temp.TimeslotType;
                expectedTT.Teacher = temp.Teacher;

                expectedTT.Note = "Add success";
            }
            else
            {
                int noteLegnth = expectedTT.Note.Length;
                expectedTT.Note = expectedTT.Note.Remove(noteLegnth - 1, 1);
            }

            ViewData["expectedTT"] = expectedTT;

            getData();
            return Page();
        }

    }
}
