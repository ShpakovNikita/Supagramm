using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;
using System.IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using IgiLab.Models;
using IgiLab.Models.ViewModels;
using IgiLab.Constants;
using IgiLab.Constants.Enums;
using EntityCore.Constants;
using Microsoft.AspNetCore.Http;
using NLog;
using EntityCore;
using AppManagers;

namespace IgiLab.Controllers
{
    public class AccountController : Controller
    {
        Logger logger;
        private readonly IManagersContainer managers;

        public AccountController(IManagersContainer container)
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            managers = container;
        }

        [HttpGet]
        public IActionResult Login()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;

            if (isAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = managers.GetUserManager().GetByAuthData(model.Username, model.Password);
                if (user != null)
                {
                    await Authenticate(user); // authenticate

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Incorrect login or password");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;

            if (isAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            int currentUserId = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
            string currentUserRole = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Role).Value;

            if (!(currentUserId == id || currentUserRole == Roles.ADMIN))
            {
                return NotFound();
            }
            
            User user = managers.GetUserManager().Get(id);

            // we don't have to check user on null because if we are authorized then we definetely have this user in db 
            EditProfileModel model = new EditProfileModel { Firstname = user.Firstname,
                                                            Lastname = user.Lastname,
                                                            Password = user.Password };

            logger.Debug("{user.Firstname}, {user.Lastname}, {user.Username}");

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile avatar, EditProfileModel model)
        {
            int currentUserId = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
            User user = managers.GetUserManager().Get(currentUserId);

            logger.Debug(String.Format("Is changing avatar? {0}", avatar == null ? false: true));
            if (avatar != null)
            {
                string ext = Path.GetExtension(avatar.FileName);

                string fullPath, filePath;

                do
                {
                    filePath = string.Format(@"{0}{1}", Guid.NewGuid(), ext);
                    fullPath = Path.Combine(Pathes.RESOURCES_DIR, filePath);

                } while (Directory.Exists(fullPath));

                logger.Debug($"File full path is {fullPath}");

                using (var stream = new FileStream(fullPath, FileMode.OpenOrCreate))
                {
                    await avatar.CopyToAsync(stream);
                }

                string relativeToResourcesPath = Path.Combine(Pathes.RESOURCES_FOLDER, filePath);
                user.AvatarPath = relativeToResourcesPath;
            }

            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.Password = model.Password;

            managers.GetUserManager().Edit(user);
            return RedirectToAction("Profile", "User", new { id = user.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = managers.GetUserManager().GetByUsername(model.Username);
                if (user == null)
                {
                    string avatarRelativePath = Path.Combine(Pathes.RESOURCES_FOLDER, Pathes.DEFAULT_AVATAR_FILE);

                    user = new User { Email = model.Email,
                        Password = model.Password,
                        Firstname = model.Firstname,
                        Lastname = model.Lastname,
                        Username = model.Username,
                        AvatarPath = avatarRelativePath,
                        Role = Roles.DEFAULT_ROLE
                    };
                    managers.GetUserManager().Create(user);

                    Authenticate(user).Wait();

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Incorrect login or password");
            }
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            // creating one claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };
            // creating object ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // setting the authentication cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}