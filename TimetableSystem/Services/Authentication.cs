using Microsoft.AspNetCore.Mvc;
using System;
using TimetableSystem.Models;

namespace TimetableSystem.Services
{
    public class Authentication
    {
        public static bool IsAdmin(User u)
        {
            int adminRole = 1;
            return u != null && u.RoleId == adminRole;
        }

        public static bool IsTeacher(User u)
        {
            int teacherRole = 2;
            return u != null && u.RoleId == teacherRole;
        }

        public static RedirectToPageResult AccessDenied()
        {
            return new RedirectToPageResult("/AccessDenied");
        }
    }
}
