using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TimetableSystem.Models;

namespace TimetableSystem.Pages.timetable
{
    public class IndexModel : PageModel
    {
        private readonly prn221Context _context;

        public IndexModel()
        {
            _context = new prn221Context();
        }

        public IActionResult OnGet()
        {
            List<Timetable> listTimetable = _context.Timetables
                .Include(x => x.Class)
                .Include(x => x.Course)
                .Include(x => x.Room)
                .Include(x => x.Teacher)
                .Include(x => x.TimeslotType)
                .ToList();
            ViewData["listTimetable"] = listTimetable;
            return Page();
        }
    }
}
