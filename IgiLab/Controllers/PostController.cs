using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using IgiLab.Models;
using IgiLab.Constants;
using IgiLab.Models.ViewModels;
using IgiLab.Components;
using IgiLab.Constants.Enums;
using AppManagers;
using EntityCore;
using NLog;

namespace IgiLab.Controllers
{
    public class PostController : Controller
    {
        Logger logger;
        IManagersContainer managers;

        public PostController(IManagersContainer container)
        {
            managers = container;
            logger = NLog.LogManager.GetCurrentClassLogger();
        }
        
        public IActionResult Index()
        {
            List<Post> posts = managers.GetPostManager().GetPosts(Pagination.TAKE_MIN);
            return View(posts);
        }

        public IActionResult Post(int id)
        {
            return View(id);
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult PostWindowViewComponent(int id)
        {
            return ViewComponent("PostWindow", new { id = id});
        }
        
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormFile photo, CreatePostModel model)
        {
            logger.Debug("Create action entering...");
            try
            {
                string ext = Path.GetExtension(photo.FileName);
                logger.Debug($"Full file extension is {ext}");

                string fullPath, filePath;

                do
                {
                    filePath = string.Format(@"{0}{1}", Guid.NewGuid(), ext);
                    fullPath = Path.Combine(Pathes.RESOURCES_DIR, filePath);

                } while (Directory.Exists(fullPath));

                logger.Debug($"Full file path is {fullPath}");

                using (var stream = new FileStream(fullPath, FileMode.OpenOrCreate))
                {
                    // async made!
                    photo.CopyTo(stream);
                }

                string relativeToResourcesPath = Path.Combine(Pathes.RESOURCES_FOLDER, filePath);
                int id = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
                Post post = new Post { Description = model.Description, OwnerId = id, ImagePath = relativeToResourcesPath };
                
                managers.GetPostManager().Create(post);


                //TODO: get followers and send them info about post
                /*
                string signalReceiverId = user.Id.ToString();
                IClientProxy proxy = hubContext.Clients.User(signalReceiverId);
                proxy.SendAsync(MessageHub.FOLLOW_ACTION, User.Identity.Name, currentUserId).Wait();
                */

                logger.Debug($"Redirecting to profile of {id} user");
                return RedirectToAction("Profile", "User", new { id = id});
            }
            catch (Exception e)
            {
                logger.Warn("Somethig gone wrong:");
                logger.Warn(e.Message);
                return View();
            }
        }
        
        public IActionResult Edit(int id)
        {
            return View();
        }
        
        [Authorize]
        public IActionResult Delete(int id)
        {
            int currentUserId = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
            managers.GetPostManager().Delete(currentUserId, id);

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}