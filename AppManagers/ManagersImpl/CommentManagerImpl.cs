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
    public class CommentManagerImpl : ManagerBase, ICommentManager
    {
        public CommentManagerImpl(Context context) : base(context)
        {

        }

        public int Create(Comment entity)
        {
            Database.Models.Comment comment = entity.CastToDatabase();
            comment.Date = DateTime.Now;
            db.Comments.Add(comment);
            db.SaveChanges();
            entity.Id = comment.Id;
            return comment.Id;
        }

        public void Edit(Comment entity)
        {
            Database.Models.Comment like = entity.CastToDatabase();
            db.Comments.Update(like);
            db.SaveChanges();
        }

        public Comment Get(int id)
        {
            Database.Models.Comment comment = db.Comments.FirstOrDefault(l => l.Id == id);
            return comment.CastToEntity();
        }

        public void Delete(int id)
        {
            Database.Models.Comment comment = db.Comments.FirstOrDefault(l => l.Id == id);
            db.Comments.Remove(comment);
            db.SaveChanges();
        }

        public List<Comment> GetComments(int start, int end)
        {
            if (end - start <= start)
            {
                return new List<Comment>();
            }

            var query = (from c in db.Comments
                         select c).Skip(start).Take(end - start);

            List<Database.Models.Comment> dbComments = query.ToList();
            List<Comment> entityComments = (from c in dbComments
                                            select c.CastToEntity()).ToList();

            return entityComments;
        }

        public List<Comment> GetComments(int end = -1)
        {
            if (end == -1)
            {
                end = db.Comments.Count();
            }

            return GetComments(0, end);
        }

        public List<Comment> GetComments(Comment filter, string username, int end)
        {
            List<Database.Models.Comment> dbComments = (from c in db.Comments
                                                        join u in db.Users on c.CommenterId equals u.Id
                                                        where u.Username.ToUpper().Contains(username.ToUpper()) &&
                                                        c.Message.ToUpper().Contains(filter.Message.ToUpper()) &&
                                                        (c.Date >= filter.Date)
                                                        select c).Take(end).ToList();

            List<Comment> entityComments = (from c in dbComments
                                            select c.CastToEntity()).ToList();

            return entityComments;
        }

        public List<Comment> GetCommentsByPost(int postId)
        {
            List<Database.Models.Comment> dbComments = (from c in db.Comments
                                                        where c.PostId == postId
                                                        select c).ToList();

            List<Comment> entityComments = (from c in dbComments
                                            select c.CastToEntity()).ToList();

            return entityComments;
        }
    }
}
