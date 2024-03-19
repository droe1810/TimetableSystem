using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TimetableSystem.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TimetableSystem.Services;

namespace TimetableSystem.Pages.timetable
{
    public class IndexModel : PageModel
    {

        public IActionResult OnGet()
        {
            getData();
            List<Timetable> listTimetable = TimetableService.GetAllTimetable();
            ViewData["listTimetable"] = listTimetable;

            return Page();
        }

        public IActionResult OnPostFilter(int classid, int courseid, int roomid, int teacherid, int timeslottypeid) {
            getData();

            List<Timetable> listTimetable = TimetableService.FilterListTimetable(classid, courseid, roomid, teacherid, timeslottypeid);
            ViewData["listTimetable"] = listTimetable;

            return Page();
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


        public IActionResult OnPostExportToJson()
        {
            List<Timetable> listTimetable = TimetableService.GetAllTimetable();
            List<TimetableJson> listTimetableJson = new List<TimetableJson>();
            foreach (var item in listTimetable)
            {
                TimetableJson itemJson = new TimetableJson();
                itemJson.ClassName = item.Class.Name;
                itemJson.RoomName = item.Room.Name;
                itemJson.CourseCode = item.Course.Code;
                itemJson.TimeslotTypeName = item.TimeslotType.Name;
                itemJson.TeacherName = item.Teacher.Username;
                listTimetableJson.Add(itemJson);
            }

            string json = JsonConvert.SerializeObject(listTimetableJson);

            string fileName = "timetable.json";

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "exports", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                using (StreamWriter sw = System.IO.File.CreateText(filePath))
                {
                    sw.Close();
                }
            }

            System.IO.File.WriteAllText(filePath, json);

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/json", fileName);
        }

        public IActionResult OnGetEdit(int timetableid)
        {
            return Redirect($"/timetable/Edit?timetableid={timetableid}");
        }

        public IActionResult OnGetDelete(int timetableid)
        {
            TimetableService.DeleteTimetable(timetableid);
            getData();
            List<Timetable> listTimetable = TimetableService.GetAllTimetable();
            ViewData["listTimetable"] = listTimetable;
            return Page();
        }
    }
}
