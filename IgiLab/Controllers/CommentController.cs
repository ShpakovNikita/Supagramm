using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IgiLab.Models;
using NLog;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using IgiLab.Models.ViewModels;
using IgiLab.Constants;
using Microsoft.AspNetCore.Http;
using AppManagers;
using EntityCore;

namespace IgiLab.Controllers
{
    public class CommentController : Controller
    {
        private readonly IManagersContainer managers;
        Logger logger;

        public CommentController(IManagersContainer container)
        {
            managers = container;
            logger = NLog.LogManager.GetCurrentClassLogger();
        }

        // POST api/comment
        [HttpPost]
        [Route("api/[controller]")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PostViewComponentModel model)
        {      
            if (model == null)
            {
                logger.Debug("Bad request!");
                return BadRequest();
            }

            if (String.IsNullOrEmpty(model.CommentForm))
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }

            int currentUserId = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);

            Comment comment = new Comment();
            comment.Message = model.CommentForm;
            comment.CommenterId = currentUserId;
            comment.PostId = model.Id;

            managers.GetCommentManager().Create(comment);
            logger.Debug($"Comment {comment.Message} added to post {comment.PostId}");

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}