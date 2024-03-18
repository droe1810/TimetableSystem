using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using TimetableSystem.Models;
using System.Text.Json;
using TimetableSystem.Services;
using Newtonsoft.Json;




namespace TimetableSystem.Pages.timetable
{
    public class ImportModel : PageModel
    {



        public void OnGet()
        {
        }



        public IActionResult OnPostImportFile(IFormFile file)
        {
            List<Timetable> listTimetableToDisplay = new List<Timetable>();
            List<Timetable> listTimetableToCheck = BaseService.GetAllTimetable();
            if (file != null && file.Length > 0)
            {
                using (var r = new StreamReader(file.OpenReadStream()))
                {
                    string json = r.ReadToEnd();
                    List<TimetableJson> listTimetableJson = System.Text.Json.JsonSerializer.Deserialize<List<TimetableJson>>(json);
                    foreach (var itemJson in listTimetableJson)
                    {
                        Timetable item = new Timetable();
                        item.Course = BaseService.GetCourseByCode(itemJson.CourseCode);
                        item.Room = BaseService.GetRoomByName(itemJson.RoomName);
                        item.Class = BaseService.GetClassByName(itemJson.ClassName);
                        item.Teacher = BaseService.GetUserByName(itemJson.TeacherName);
                        item.TimeslotType = BaseService.GetTimeslotTypeByName(itemJson.TimeslotTypeName);


                        foreach (var itemCheck in listTimetableToCheck)
                        {
                            if (item.Teacher.Id == itemCheck.Teacher.Id && item.TimeslotType.Id == itemCheck.TimeslotType.Id)
                            {
                                item.Note += " Teacher has been teaching in another timeslot -";
                            }
                            if (item.Class.Id == itemCheck.Class.Id && item.TimeslotType.Id == itemCheck.TimeslotType.Id)
                            {
                                item.Note += " Class has been studing in another timeslot -";
                            }
                            if (item.Room.Id == itemCheck.Room.Id && item.TimeslotType.Id == itemCheck.TimeslotType.Id)
                            {
                                item.Note += " Room has been booking in another timeslot -";
                            }
                            if (item.Class.Id == itemCheck.Class.Id && item.Course.Id == itemCheck.Course.Id)
                            {
                                item.Note += " Class has taken the course before -";
                            }

                        }
                        if (item.Note == null || item.Note.Equals(""))
                        {
                            listTimetableToCheck.Add(item);
                        }
                        else
                        {
                            int noteLegnth = item.Note.Length;
                            item.Note = item.Note.Remove(noteLegnth - 1, 1);
                        }
                        listTimetableToDisplay.Add(item);
                        ViewData["listTimetable"] = listTimetableToDisplay;

                    }
                }

            }
            return Page();


        }

        public IActionResult OnPostSave(string listTimetableDisplay)
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

                foreach(var itemToSave in listToSave)
                {
                    Timetable item = new Timetable();
                    item.CourseId = itemToSave.Course.Id;
                    item.RoomId = itemToSave.Room.Id;
                    item.ClassId = itemToSave.Class.Id; 
                    item.TeacherId = itemToSave.Teacher.Id;
                    item.TimeslotTypeId = itemToSave.TimeslotType.Id;
                    
                    BaseService.AddTimetable(item);

                }
            }
            return Page();
        }
    }
}
