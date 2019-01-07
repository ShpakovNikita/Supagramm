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

namespace AppManagers.ManagersImpl
{
    public class LikeManagerImpl : ManagerBase, ILikeManager
    {
        public LikeManagerImpl(Context context) : base(context)
        {

        }

        public int Create(Like entity)
        {
            Database.Models.Like like = entity.CastToDatabase();
            db.Likes.Add(like);
            db.SaveChanges();
            entity.Id = like.Id;
            return like.Id;
        }

        public void Edit(Like entity)
        {
            Database.Models.Like like = entity.CastToDatabase();
            db.Likes.Update(like);
            db.SaveChanges();
        }

        public Like Get(int id)
        {
            Database.Models.Like like = db.Likes.FirstOrDefault(l => l.Id == id);
            return like.CastToEntity();
        }

        public void Delete(int id)
        {
            Database.Models.Like like = db.Likes.FirstOrDefault(l => l.Id == id);
            db.Likes.Remove(like);
            db.SaveChanges();
        }

        public void Delete(int likerId, int postId)
        {
            Database.Models.Like like = db.Likes.FirstOrDefault(l => l.PostId == postId && l.UserId == likerId);
            db.Likes.Remove(like);
            db.SaveChanges();
        }

        public Like Get(int userId, int postId)
        {
            Database.Models.Like like = db.Likes.FirstOrDefault(l => l.PostId == postId && l.UserId == userId);
            return like.CastToEntity();
        }
    }
}
