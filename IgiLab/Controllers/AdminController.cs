using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
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
using IgiLab.Models.ViewModels.Admin;
using IgiLab.Constants.Enums;
using Microsoft.AspNetCore.Http;
using EntityCore;
using AppManagers;

// Views for this controller are localized
namespace IgiLab.Controllers
{
    [Authorize(Policy = ClaimPolicies.ADMIN_REQUIRED)]
    public class AdminController : Controller
    {
        private readonly Logger logger;
        private readonly IManagersContainer managers;

        public AdminController(IManagersContainer container)
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            managers = container;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Users(UserSearchModel userSearchModel)
        {
            logger.Debug(userSearchModel);

            List<User> users;

            if (userSearchModel == null)
            {
                userSearchModel = new UserSearchModel();
                users = managers.GetUserManager().GetUsers(Pagination.TAKE_MIN);
            }
            else
            { 
                userSearchModel.EmptifyNulls();
                logger.Debug($"Username contains: '{userSearchModel.Username}', Lastname contains: '{userSearchModel.Lastname}', Firstname contains: '{userSearchModel.Firstname}', Role: '{userSearchModel.Role}'");

                users = managers.GetUserManager().GetUsers(userSearchModel.CastToUser(), Pagination.TAKE_MIN);
            }

            UserAggregationModel model = new UserAggregationModel();
            model.Users = users;
            model.UserSearchModel = userSearchModel;

            return View(model);
        }

        [HttpGet]
        public IActionResult Comments(CommentSearchModel commentSearchModel)
        {
            logger.Debug(commentSearchModel);

            List<Comment> comments = new List<Comment>();

            if (commentSearchModel == null)
            {
                commentSearchModel = new CommentSearchModel();
                comments = managers.GetCommentManager().GetComments(Pagination.TAKE_MIN);
            }
            else
            {
                commentSearchModel.EmptifyNulls();
                logger.Debug($"Username contains: '{commentSearchModel.CommenterUsername}', Message contains: '{commentSearchModel.MessageFilter}', from Date: '{commentSearchModel.Date}'");

                comments = managers.GetCommentManager().GetComments(commentSearchModel.CastToComment(), commentSearchModel.CommenterUsername, Pagination.TAKE_MIN);
            }

            CommentAggregationModel model = new CommentAggregationModel();
            model.Comments = comments;
            model.CommentSearchModel = commentSearchModel;

            return View(model);
        }

        [HttpGet]
        public IActionResult Posts(PostSearchModel postSearchModel)
        {
            logger.Debug(postSearchModel);

            List<Post> posts = new List<Post>();

            if (postSearchModel == null)
            {
                postSearchModel = new PostSearchModel();
                posts = managers.GetPostManager().GetPosts(Pagination.TAKE_MIN);
            }
            else
            {
                postSearchModel.EmptifyNulls();
                logger.Debug($"Username contains: '{postSearchModel.OwnerUsername}', Description contains: '{postSearchModel.DescriptionFilter}', from Date: '{postSearchModel.Date}'");

                posts = managers.GetPostManager().GetPosts(postSearchModel.CastToPost(), postSearchModel.OwnerUsername, Pagination.TAKE_MIN);
            }

            PostAggregationModel model = new PostAggregationModel();
            model.Posts = posts;
            model.PostSearchModel = postSearchModel;

            return View(model);
        }

        public IActionResult TogglePromotion(int id)
        {
            int currentUserId = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
            if (id == currentUserId)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }

            managers.GetUserManager().TogglePromotion(currentUserId, id);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult DeleteUser(int id)
        {
            int currentUserId = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
            if (id == currentUserId)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                managers.GetUserManager().Delete(id);
                return Redirect(Request.Headers["Referer"].ToString());
            }
        }

        public IActionResult DeletePost(int id)
        {
            int currentUserId = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
            managers.GetPostManager().Delete(currentUserId, id);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult DeleteComment(int id)
        {
            managers.GetCommentManager().Delete(id);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}