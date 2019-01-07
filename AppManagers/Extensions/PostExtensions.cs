using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCore;
using AppManagers.Database.Models;

namespace AppManagers.Extensions
{
    public static class PostExtensions
    {
        public static EntityCore.Post CastToEntity(this Database.Models.Post dbPost)
        {
            if (dbPost == null)
            {
                return null;
            }

            EntityCore.Post entityPost = new EntityCore.Post();
            entityPost.Id = dbPost.Id;
            entityPost.ImagePath = dbPost.ImagePath;
            entityPost.Description = dbPost.Description;
            entityPost.OwnerId = dbPost.OwnerId;
            entityPost.Date = dbPost.Date;

            entityPost.Likes = (from l in dbPost.Likes select l.CastToEntity(entityPost)).ToList();

            return entityPost;
        }

        public static Database.Models.Post CastToDatabase(this EntityCore.Post entityPost)
        {
            if (entityPost == null)
            {
                return null;
            }

            Database.Models.Post dbPost = new Database.Models.Post();
            dbPost.Id = entityPost.Id;
            dbPost.ImagePath = entityPost.ImagePath;
            dbPost.Description = entityPost.Description;
            dbPost.OwnerId = entityPost.OwnerId;
            dbPost.Date = entityPost.Date;

            return dbPost;
        }
    }
}
