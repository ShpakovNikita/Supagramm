using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCore;
using EntityCore.Constants;
using Microsoft.EntityFrameworkCore;
using AppManagers.Database;
using AppManagers.Extensions;
using AppManagers.ManagersImpl;

namespace AppManagers
{
    public class ManagersContainer : IManagersContainer
    {
        private readonly Context context;

        private PostManagerImpl postManager;
        private UserManagerImpl userManager;
        private CommentManagerImpl commentManager;
        private LikeManagerImpl likeManager;

        public ManagersContainer(Context dbContext)
        {
            context = dbContext;
        }

        public IPostManager GetPostManager()
        {
            if (postManager == null)
            {
                postManager = new PostManagerImpl(context);
            }

            return postManager;
        }

        public IUserManager GetUserManager()
        {
            if (userManager == null)
            {
                userManager = new UserManagerImpl(context);
            }

            return userManager;
        }

        public ICommentManager GetCommentManager()
        {
            if (commentManager == null)
            {
                commentManager = new CommentManagerImpl(context);
            }

            return commentManager;
        }

        public ILikeManager GetLikeManager()
        {
            if (likeManager == null)
            {
                likeManager = new LikeManagerImpl(context);
            }

            return likeManager;
        }
    }
}
