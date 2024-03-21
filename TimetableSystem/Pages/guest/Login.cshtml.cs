using TimetableSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace TimetableSystem.Pages.guest
{
    public class LoginModel : PageModel
    {
        [BindProperty] public User user { get; set; }

        private readonly prn221Context _context;

        public LoginModel()
        {
            _context = new prn221Context();
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var authenticatedUser = _context.Users
                .FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

            if (authenticatedUser != null)
            {
                string userJson = JsonSerializer.Serialize(authenticatedUser);
                HttpContext.Session.SetString("currentUser", userJson);

                if(authenticatedUser.RoleId == 1)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    return RedirectToPage("/Index");
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }
        }
    }
}
