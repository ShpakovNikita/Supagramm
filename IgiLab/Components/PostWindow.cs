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
using IgiLab.Models.ViewModels;
using IgiLab.Models;
using NLog;
using AppManagers;
using EntityCore;


namespace IgiLab.Components
{
    public class PostWindow : ViewComponent
    {
        private readonly IManagersContainer managers;
        private Logger logger;

        public PostWindow(IManagersContainer container)
        {
            managers = container;
            logger = NLog.LogManager.GetCurrentClassLogger();
        }

        public IViewComponentResult Invoke(int id)
        {
            Post post = managers.GetPostManager().Get(id);
            User user = managers.GetUserManager().Get(post.OwnerId);

            PostViewComponentModel model = new PostViewComponentModel(post, user);

            int currentUserId = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
            model.Liked = !(post.Likes.Find(l => l.UserId == currentUserId) == null);

            logger.Debug($"Is current user liked post? {model.Liked}");

            List<Comment> comments = managers.GetCommentManager().GetCommentsByPost(id);
            List<CommentExtendedModel> extComments = CastToExtendedModel(comments);
            model.Comments = extComments;

            // TODO: real rights CanDelete
            model.CanDelete = currentUserId == post.OwnerId;

            return View(model);
        }

        private List<CommentExtendedModel> CastToExtendedModel(List<Comment> comments)
        {
            List<CommentExtendedModel> resultList = new List<CommentExtendedModel>();

            foreach (Comment comment in comments)
            {
                CommentExtendedModel model = new CommentExtendedModel(comment);
                model.PosterUsername = managers.GetUserManager().Get(model.CommenterId).Username;
                resultList.Add(model);
            }
            return resultList;
        }
    }
}
