using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TimetableSystem.Pages.common
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("currentUser");

            // HttpContext.Session.Clear();

            return RedirectToPage("/Index");
        }
    }
}
