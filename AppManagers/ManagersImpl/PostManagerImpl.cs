using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppManagers;
using EntityCore;
using EntityCore.Constants;
using Microsoft.EntityFrameworkCore;
using AppManagers.Database;
using AppManagers.Extensions;

namespace AppManagers.ManagersImpl
{
    public class PostManagerImpl : ManagerBase, IPostManager
    {
        public PostManagerImpl(Context context) : base(context)
        {

        }

        public int Create(Post entity)
        {
            Database.Models.Post post = entity.CastToDatabase();
            post.Date = DateTime.Now;
            db.Posts.Add(post);
            db.SaveChanges();
            entity.Id = post.Id;
            return post.Id;
        }

        public void Delete(int deleterId, int postId)
        {
            Database.Models.User user = db.Users.FirstOrDefault(u => u.Id == deleterId);
            Database.Models.Post post = db.Posts.FirstOrDefault(p => p.Id == postId);

            if (user.Role == Roles.ADMIN || (post != null && postId == post.OwnerId))
            {
                // It is better not to call Delete method to save some time and not doing sql request
                db.Posts.Remove(post);
                db.SaveChanges();
            }
        }

        public void Edit(Post entity)
        {
            Database.Models.Post post = entity.CastToDatabase();
            db.Posts.Update(post);
            db.SaveChanges();
        }

        public Post Get(int id)
        {
            Database.Models.Post post = db.Posts.Include(p => p.Likes).FirstOrDefault(p => p.Id == id);
            return post.CastToEntity();
        }

        public List<Post> GetPostsByUserId(int userId)
        {
            List<Database.Models.Post> dbPosts = (from p in db.Posts
                                                  where p.OwnerId == userId
                                                  orderby p.Id descending
                                                  select p).ToList();
            List<Post> entityPosts = (from p in dbPosts
                                      select p.CastToEntity()).ToList();

            return entityPosts;
        }

        public List<Post> GetPosts(int start, int end)
        {
            if (end - start <= start)
            {
                return new List<Post>();
            }

            var query = (from p in db.Posts.Include(p => p.Likes)
                         orderby p.Likes.Count() descending
                         select p).Skip(start).Take(end - start);

            List<Database.Models.Post> dbPosts = query.ToList();
            List<Post> entityPosts = (from p in dbPosts
                                      select p.CastToEntity()).ToList();


            return entityPosts;
        }

        public List<Post> GetPosts(int end = -1)
        {
            if (end == -1)
            {
                end = db.Posts.Count();
            }

            return GetPosts(0, end);
        }

        public List<Post> GetPosts(Post filter, string username, int end)
        {
            List<Database.Models.Post> dbPosts = (from p in db.Posts
                                                  join u in db.Users on p.OwnerId equals u.Id
                                                  where u.Username.ToUpper().Contains(username.ToUpper()) &&
                                                  p.Description.ToUpper().Contains(filter.Description.ToUpper()) &&
                                                  (p.Date >= filter.Date)
                                                  select p).Take(end).ToList();

            List<Post> entityPosts = (from p in dbPosts
                                      select p.CastToEntity()).ToList();

            return entityPosts;
        }
    }
}
