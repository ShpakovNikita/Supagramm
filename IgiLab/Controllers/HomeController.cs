using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using EntityCore;
using AppManagers;
using IgiLab.Models.ViewModels;
using NLog;

namespace IgiLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly IManagersContainer managers;
        Logger logger;

        public HomeController(IManagersContainer container)
        {
            managers = container;
            logger = NLog.LogManager.GetCurrentClassLogger();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            int id = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
            
            User user = managers.GetUserManager().Get(id);
            return RedirectToAction("Profile", "User", new { id = user.Id });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
