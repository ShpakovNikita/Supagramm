using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using IgiLab.Models;
using IgiLab.Models.ViewModels;
using IgiLab.Constants;
using Microsoft.AspNetCore.Http;
using EntityCore;
using AppManagers;
using NLog;

namespace IgiLab.Controllers
{
    public class LikeController : Controller
    {
        Logger logger;
        private readonly IManagersContainer managers;

        public LikeController(IManagersContainer container)
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            managers = container;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("api/[controller]/{id}")]
        [HttpPost]
        public string Post(int id)
        {
            int currentUserId = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
            Post post = managers.GetPostManager().Get(id);
            Like currLike = managers.GetLikeManager().Get(currentUserId, id);

            if (User.Identity.IsAuthenticated && currLike == null)
            {
                Like like = new Like { PostId = post.Id, UserId = currentUserId };
                managers.GetLikeManager().Create(like);
            }
            else if (User.Identity.IsAuthenticated)
            {
                managers.GetLikeManager().Delete(currentUserId, id);
            }

            return post.Likes.Count().ToString();
        }
    }
}