using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using TimetableSystem.Models;
using System.Text.Json;
using TimetableSystem.Services;



namespace TimetableSystem.Pages.timetable
{
    public class ImportModel : PageModel
    {

        public List<Timetable> list = new List<Timetable>();


        public void OnGet()
        {
        }



        public IActionResult OnPostImportFile(IFormFile file)
        {
            List<Timetable> listTimetable = new List<Timetable>();
            if (file != null && file.Length > 0)
            {
                using (var r = new StreamReader(file.OpenReadStream()))
                {
                    string json = r.ReadToEnd();
                    List<TimetableJson> listTimetableJson = System.Text.Json.JsonSerializer.Deserialize<List<TimetableJson>>(json);
                    foreach (var itemJson in listTimetableJson)
                    {
                        Timetable item = new Timetable();
                        item.Id = itemJson.Id;
                        item.CourseId = BaseService.GetCourseByCode(itemJson.CourseCode).Id;
                        item.RoomId = BaseService.GetRoomByName(itemJson.RoomName).Id;
                        item.ClassId = BaseService.GetClassByName(itemJson.ClassName).Id;
                        item.TeacherId = BaseService.GetUserByName(itemJson.TeacherName).Id;
                        item.TimeslotTypeId = BaseService.GetTimeslotTypeByName(itemJson.TimeslotTypeName).Id;

                        listTimetable.Add(item);
                    }
                }
               
            }
            ViewData["listTimetable"] = listTimetable;

            return Page();
        }

    }
}
