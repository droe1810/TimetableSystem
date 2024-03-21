using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TimetableSystem.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TimetableSystem.Services;
using Microsoft.AspNetCore.SignalR;
using TimetableSystem.Hubs;

namespace TimetableSystem.Pages.admin
{
    public class IndexModel : PageModel
    {
        private readonly IHubContext<DocumentHub> _hubContext;
        private readonly prn221Context _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public List<Timetable> Timetables { get; set; } = null!;
        public List<Class> Classes { get; set; } = null!;
        public List<Course> Courses { get; set; } = null!;
        public List<Room> Rooms { get; set; } = null!;
        public List<User> Teachers { get; set; } = null!;
        public List<TimeslotType> Timeslottypes { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }

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

        private int _totalItem, _pageSize, _startIndex;
        public int TotalPage { get; set; }

        public IndexModel(IHubContext<DocumentHub> hubContext, prn221Context dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult OnGet()
        {
            string userJson = _httpContextAccessor.HttpContext.Session.GetString("currentUser");
            if (userJson == null)
            {
                return Redirect("/AccessDenied");
            }
            else
            {
                User u = System.Text.Json.JsonSerializer.Deserialize<User>(userJson);
                if (!Authentication.IsAdmin(u))
                {
                    return Redirect("/AccessDenied");
                }
                else
                {
                    GetDataPagging();
                    return Page();
                }
            }
        }

        public IActionResult OnPostFilter()
        {
            string userJson = _httpContextAccessor.HttpContext.Session.GetString("currentUser");
            if (userJson == null)
            {
                return Redirect("/AccessDenied");
            }
            else
            {
                User u = System.Text.Json.JsonSerializer.Deserialize<User>(userJson);
                if (!Authentication.IsAdmin(u))
                {
                    return Redirect("/AccessDenied");
                }
                else
                {
                    GetDataPagging();
                    return Page();
                }
            }
                  
        }
        private void GetDataPagging()
        {
            Rooms = RoomService.GetAllRoom();
            Teachers = UserService.GetAllTeacher();
            Courses = CourseService.GetAllCourse();
            Timeslottypes = TimeslotTypeService.GetAllTimeslotType();
            Classes = ClassService.GetAllClass();

            var result = _dbContext.Timetables.
            Where(tt => Classid == 0 || tt.ClassId == Classid)
            .Where(tt => Courseid == 0 || tt.CourseId == Courseid)
            .Where(tt => Roomid == 0 || tt.RoomId == Roomid)
            .Where(tt => Teacherid == 0 || tt.TeacherId == Teacherid)
            .Where(tt => Timeslottypeid == 0 || tt.TimeslotTypeId == Timeslottypeid)
            .Include(tt => tt.Class).Include(tt => tt.Course).Include(tt => tt.Room).Include(tt => tt.Teacher).Include(tt => tt.TimeslotType)
            .OrderBy(tt => tt.Id);

            if (PageIndex < 1) PageIndex = 1;
            _totalItem = result.Count();
            _pageSize = 5;
            TotalPage = (int)Math.Ceiling((double)_totalItem / _pageSize);

            if (TotalPage > 0)
            {
                if (PageIndex > TotalPage) PageIndex = TotalPage;
                //_startIndex = (PageIndex - 1) * _pageSize + 1;

                Timetables = result.Skip((PageIndex - 1) * _pageSize)
                    .Take(_pageSize)
                    .ToList();
            }
        }

        public IActionResult OnPostExportToJson()
        {
            string userJson = _httpContextAccessor.HttpContext.Session.GetString("currentUser");
            if (userJson == null)
            {
                return Redirect("/AccessDenied");
            }
            else
            {
                User u = System.Text.Json.JsonSerializer.Deserialize<User>(userJson);
                if (!Authentication.IsAdmin(u))
                {
                    return Redirect("/AccessDenied");
                }
                else
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
            }
        }

        public IActionResult OnGetEdit(int timetableid)
        {
            string userJson = _httpContextAccessor.HttpContext.Session.GetString("currentUser");
            if (userJson == null)
            {
                return Redirect("/AccessDenied");
            }
            else
            {
                User u = System.Text.Json.JsonSerializer.Deserialize<User>(userJson);
                if (!Authentication.IsAdmin(u))
                {
                    return Redirect("/AccessDenied");
                }
                else
                {
                    return Redirect($"/admin/Edit?timetableid={timetableid}");
                }
            }
        }
        public async Task<IActionResult> OnGetDelete(int timetableid)
        {
            string userJson = _httpContextAccessor.HttpContext.Session.GetString("currentUser");
            if (userJson == null)
            {
                return Redirect("/AccessDenied");
            }
            else
            {
                User u = System.Text.Json.JsonSerializer.Deserialize<User>(userJson);
                if (!Authentication.IsAdmin(u))
                {
                    return Redirect("/AccessDenied");
                }
                else
                {
                    TimetableService.DeleteTimetable(timetableid);
                    GetDataPagging();
                    await _hubContext.Clients.All.SendAsync("ReloadDocuments");

                    return Page();
                }
            }     
        }
    }
}
