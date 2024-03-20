using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using TimetableSystem.Models;
using System.Text.Json;
using TimetableSystem.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using TimetableSystem.Hubs;




namespace TimetableSystem.Pages.timetable
{
    public class ImportModel : PageModel
    {
        private readonly IHubContext<DocumentHub> _hubContext;

        [BindProperty]
        public List<Timetable> listTimetableDisplay { get; set; }
        public ImportModel(IHubContext<DocumentHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void OnGet()
        {
        }
        public IActionResult OnPostImportFile(IFormFile file)
        {
            listTimetableDisplay = new List<Timetable>();
            List<Timetable> listTimetableToCheck = TimetableService.GetAllTimetable();
            if (file != null && file.Length > 0)
            {
                using (var r = new StreamReader(file.OpenReadStream()))
                {
                    try {
                        string json = r.ReadToEnd();
                        List<TimetableJson> listTimetableJson = System.Text.Json.JsonSerializer.Deserialize<List<TimetableJson>>(json);
                        foreach (var itemJson in listTimetableJson)
                        {
                            Timetable item = new Timetable();
                            item.Course = CourseService.GetCourseByCode(itemJson.CourseCode);
                            item.Room = RoomService.GetRoomByName(itemJson.RoomName);
                            item.Class = ClassService.GetClassByName(itemJson.ClassName);
                            item.Teacher = UserService.GetUserByName(itemJson.TeacherName);
                            item.TimeslotType = TimeslotTypeService.GetTimeslotTypeByName(itemJson.TimeslotTypeName);

                            if(item.Course == null || item.Class == null || item.Room == null || item.Teacher == null || item.TimeslotType == null)
                            {
                                ViewData["Msg"] = "The file's data is invalid";
                                return Page();
                            }
                            else
                            {
                                foreach (var itemCheck in listTimetableToCheck)
                                {
                                    if (item.Teacher.Id == itemCheck.Teacher.Id && item.TimeslotType.Id == itemCheck.TimeslotType.Id)
                                    {
                                        item.Note += $" {item.Teacher.Username} has been teaching in timeslot {item.TimeslotType.Name} -";
                                    }
                                    if (item.Class.Id == itemCheck.Class.Id && item.TimeslotType.Id == itemCheck.TimeslotType.Id)
                                    {
                                        item.Note += $" {item.Class.Name} has been studing in timeslot {item.TimeslotType.Name} -";
                                    }
                                    if (item.Room.Id == itemCheck.Room.Id && item.TimeslotType.Id == itemCheck.TimeslotType.Id)
                                    {
                                        item.Note += $" {item.Room.Name} has been booking in timeslot {item.TimeslotType.Name} -";
                                    }
                                    if (item.Class.Id == itemCheck.Class.Id && item.Course.Id == itemCheck.Course.Id)
                                    {
                                        item.Note += $" {item.Class.Name} has taken the course {item.Course.Code} before -";
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
                                listTimetableDisplay.Add(item);
                            }
                        }
                    }
                    catch
                    {
                        ViewData["Msg"] = "File is not in the correct format";
                        return Page();
                    }   
                }
            }
            else
            {
                ViewData["Msg"] = "Please enter a file";
            }
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

                foreach(var itemToSave in listToSave)
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
            await _hubContext.Clients.All.SendAsync("ReloadDocuments");
            return Page();
        }
    }
}
