using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCore;
using AppManagers.Database.Models;

namespace AppManagers.Extensions
{
    public static class CommentExtensions
    {
        public static EntityCore.Comment CastToEntity(this Database.Models.Comment dbComment)
        {
            if (dbComment == null)
            {
                return null;
            }

            EntityCore.Comment entityComment = new EntityCore.Comment();
            entityComment.Id = dbComment.Id;
            entityComment.Message = dbComment.Message;
            entityComment.Date = dbComment.Date;
            entityComment.CommenterId = dbComment.CommenterId;
            entityComment.PostId = dbComment.PostId;

            return entityComment;
        }

        public static Database.Models.Comment CastToDatabase(this EntityCore.Comment entityComment)
        {
            if (entityComment == null)
            {
                return null;
            }

            Database.Models.Comment dbComment = new Database.Models.Comment();
            dbComment.Id = entityComment.Id;
            dbComment.Message = entityComment.Message;
            dbComment.Date = entityComment.Date;
            dbComment.CommenterId = entityComment.CommenterId;
            dbComment.PostId = entityComment.PostId;

            return dbComment;
        }
    }
}
