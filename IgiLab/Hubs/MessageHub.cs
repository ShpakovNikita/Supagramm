using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using IgiLab.Models;
using IgiLab.Constants;
using IgiLab.Constants.Enums;
using IgiLab.Hubs;

namespace IgiLab.Hubs
{
    public class MessageHub : Hub
    {
        //TODO: this action on create post
        public const string POST_ACTION = "Post";

        public const string FOLLOW_ACTION = "Follow";
        public const string HUB_URL = "/MessageHub";

        public MessageHub()
        {
        }
    }

    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
