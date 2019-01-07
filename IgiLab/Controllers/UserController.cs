using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using IgiLab.Constants.Enums;
using IgiLab.Models.ViewModels;
using Microsoft.AspNetCore.SignalR;
using IgiLab.Hubs;
using EntityCore;
using NLog;
using AppManagers;
using AppManagers.Database;

namespace IgiLab.Controllers
{
    public class UserController : Controller
    {
        private readonly IHubContext<MessageHub> hubContext;
        private readonly IManagersContainer managers;
        Logger logger;

        public UserController(IHubContext<MessageHub> messageHubContext, IManagersContainer container)
        {
            hubContext = messageHubContext;
            logger = NLog.LogManager.GetCurrentClassLogger();
            managers = container;
        }

        public IActionResult Profile(int id)
        {
            logger.Debug($"{((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value} is claims id for current user");

            User user = managers.GetUserManager().Get(id);
            if (user != null)
            {
                ProfileModel profile = new ProfileModel(user);
                profile.Posts = managers.GetPostManager().GetPostsByUserId(id);
                profile.Followers = managers.GetUserManager().GetFollowers(id).Count();
                profile.Following = managers.GetUserManager().GetFollowings(id).Count();
                profile.SubscriptionStatus = GetSubscriptionStatus(user);

                return View(profile);
            }

            return NotFound();
        }

        [Authorize]
        public IActionResult Subscribe(int id)
        {
            if (!managers.GetUserManager().IsUserExists(id))
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }

            int currentUserId = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
            managers.GetUserManager().Follow(currentUserId, id);

            string signalReceiverId = managers.GetUserManager().Get(id).Id.ToString();
            IClientProxy proxy = hubContext.Clients.User(signalReceiverId);
            proxy.SendAsync(MessageHub.FOLLOW_ACTION, User.Identity.Name, currentUserId).Wait();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Authorize]
        public IActionResult Unsubscribe(int id)
        {
            int currentUserId = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);

            managers.GetUserManager().Unfollow(currentUserId, id);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Following(int id)
        {
            // get following async
            List<User> followings = managers.GetUserManager().GetFollowings(id);
            return View(CastUserToSubscriptionViewModel(followings));
        }

        public IActionResult Followers(int id)
        {
            // get followiers async
            List<User> followers = managers.GetUserManager().GetFollowers(id);
            return View(CastUserToSubscriptionViewModel(followers));
        }

        private List<SubscriptionViewModel> CastUserToSubscriptionViewModel(List<User> users)
        {
            List<SubscriptionViewModel> models = (from u in users
                                                  select new SubscriptionViewModel(u) { SubscriptionStatus = GetSubscriptionStatus(u)}).ToList();

            return models;
        }

        private SubscriptionStatus GetSubscriptionStatus(User user)
        {
            var nameIdentifier = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifier == null)
            {
                return SubscriptionStatus.CannotSubscribe;
            }

            int currentUserId = Int32.Parse(nameIdentifier.Value);

            SubscriptionStatus subscriptionStatus = SubscriptionStatus.CanSubscribe;

            if (currentUserId == user.Id)
            {
                subscriptionStatus = SubscriptionStatus.CannotSubscribe;
            }
            else if (managers.GetUserManager().IsAlreadyFollowing(currentUserId, user.Id))
            {
                subscriptionStatus = SubscriptionStatus.AlreadyFollowing;
            }

            return subscriptionStatus;
        }

        [Route("api/[controller]")]
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return managers.GetUserManager().GetUsers();
        }

        [Route("api/[controller]/{filter}")]// GET api/user/Sha => [{Shaft}, {Shas}, ...]
        [HttpGet("{filter}")]
        public IEnumerable<User> Get(string filter)
        {
            return managers.GetUserManager().GetUsers(filter);
        }

        // POST api/users
        [Route("api/[controller]")]
        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            
            managers.GetUserManager().Create(user);
            return Ok(user);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}