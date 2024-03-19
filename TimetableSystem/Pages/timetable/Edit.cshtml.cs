using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TimetableSystem.Models;
using TimetableSystem.Services;

namespace TimetableSystem.Pages.timetable
{
    public class EditModel : PageModel
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
        public IActionResult OnGet(int timetableid)
        {
            Timetable tt = TimetableService.GetTimetableById(timetableid);
            ViewData["timetable"] = tt;

            getData();

            return Page();
        }

        public IActionResult OnPostEdit(int timetableid, int classidedit, int courseidedit, int roomidedit, int teacheridedit, int timeslottypeidedit)
        {
            Timetable oldTt = TimetableService.GetTimetableById(timetableid);

            Timetable expectedTT = new Timetable();
            expectedTT.Class = ClassService.GetClassById(classidedit);
            expectedTT.Course = CourseService.GetCourseById(courseidedit);
            expectedTT.Room = RoomService.GetRoomById(roomidedit);
            expectedTT.Teacher = UserService.GetUserById(teacheridedit);
            expectedTT.TimeslotType = TimeslotTypeService.GetTimeslotTypeById(timeslottypeidedit);


            if (oldTt.Class.Id == classidedit && oldTt.Course.Id == courseidedit && oldTt.Room.Id == roomidedit && oldTt.Teacher.Id == teacheridedit && oldTt.TimeslotType.Id == timeslottypeidedit)
            {
                oldTt.Note += " No Data change ";
            }
            else
            {
                List<Timetable> listCheck = TimetableService.GetAllTimetable();
                listCheck.RemoveAll(tt => tt.Id == oldTt.Id);
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
            }


            if (expectedTT.Note == null || expectedTT.Note.Equals(""))
            {
                expectedTT.Id = timetableid;
                expectedTT.ClassId = expectedTT.Class.Id;
                expectedTT.CourseId = expectedTT.Course.Id;
                expectedTT.RoomId = expectedTT.Room.Id;
                expectedTT.TeacherId = expectedTT.Teacher.Id;
                expectedTT.TimeslotTypeId = expectedTT.TimeslotType.Id;
                TimetableService.EditTimetable(expectedTT);
                expectedTT.Note = "Edit success";
            }
            else
            {
                int noteLegnth = expectedTT.Note.Length;
                expectedTT.Note = expectedTT.Note.Remove(noteLegnth - 1, 1);
            }
          
            ViewData["expectedTT"] = expectedTT;
            ViewData["timetable"] = oldTt;

            getData();
            return Page();
        }
    }
}
