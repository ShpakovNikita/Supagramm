using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCore;
using AppManagers.Database.Models;

namespace AppManagers.Extensions
{
    public static class LikeExtensions
    {
        public static EntityCore.Like CastToEntity(this Database.Models.Like dbLike, EntityCore.Post post = null, EntityCore.User user = null)
        {
            if (dbLike == null)
            {
                return null;
            }

            EntityCore.Like entityLike = new EntityCore.Like();
            entityLike.Id = dbLike.Id;
            entityLike.PostId = dbLike.PostId;
            entityLike.UserId = dbLike.UserId;
            entityLike.Post = post;
            entityLike.User = user;

            return entityLike;
        }

        public static Database.Models.Like CastToDatabase(this EntityCore.Like entityLike)
        {
            if (entityLike == null)
            {
                return null;
            }

            Database.Models.Like dbLike = new Database.Models.Like();
            dbLike.Id = entityLike.Id;
            dbLike.PostId = entityLike.PostId;
            dbLike.UserId = entityLike.UserId;

            return dbLike;
        }
    }
}
