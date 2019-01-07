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
    public class UserManagerImpl : ManagerBase, IUserManager
    {
        public UserManagerImpl(Context context) : base(context)
        {

        }

        public int Create(User entity)
        {
            Database.Models.User user = entity.CastToDatabase();
            db.Users.Add(user);
            db.SaveChanges();
            entity.Id = user.Id;
            return user.Id;
        }

        public void Edit(User entity)
        {
            Database.Models.User user = entity.CastToDatabase();
            db.Users.Update(user);
            db.SaveChanges();
        }

        public User Get(int id)
        {
            Database.Models.User user = db.Users.FirstOrDefault(u => u.Id == id);
            return user.CastToEntity();
        }

        public void Delete(int id)
        {
            Database.Models.User user = db.Users.FirstOrDefault(u => u.Id == id);
            db.Users.Remove(user);
            db.SaveChanges();
        }


        public User GetByUsername(string username)
        {
            Database.Models.User user = db.Users.FirstOrDefault(u => u.Username == username);
            return user.CastToEntity();
        }

        public User GetByAuthData(string username, string password)
        {
            Database.Models.User user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            return user.CastToEntity();
        }

        public void Follow(int followerId, int userId)
        {
            if (!IsUserExists(userId))
            {
                return;
            }

            Database.Models.FollowingRelation existingRelation = db.FollowingRelations.FirstOrDefault(
                r => r.FollowerId == followerId && r.UserId == userId);

            if (followerId != userId && existingRelation == null)
            {
                Database.Models.FollowingRelation relation = new Database.Models.FollowingRelation();
                relation.FollowerId = followerId;
                relation.UserId = userId;

                db.FollowingRelations.Add(relation);
                db.SaveChanges();
            }
        }

        public void Unfollow(int followerId, int userId)
        {
            Database.Models.FollowingRelation existingRelation = db.FollowingRelations.FirstOrDefault(
                r => r.FollowerId == followerId && r.UserId == userId);

            if (existingRelation != null)
            {
                db.FollowingRelations.Remove(existingRelation);
                db.SaveChanges();
            }
        }

        public bool IsUserExists(int id)
        {
            Database.Models.User user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return false;
            }

            return true;
        }

        public List<User> GetFollowers(int id)
        {
            List<Database.Models.User> dbUsers = (from u in db.FollowingRelations
                                                  where u.UserId == id
                                                  orderby u.User.Username descending
                                                  select u.Follower).ToList();

            List<User> entityUsers = (from u in dbUsers
                                      select u.CastToEntity()).ToList();

            return entityUsers;
        }

        public List<User> GetFollowings(int id)
        {
            List<Database.Models.User> dbUsers = (from u in db.FollowingRelations
                                                  where u.FollowerId == id
                                                  orderby u.User.Username descending
                                                  select u.Follower).ToList();

            List<User> entityUsers = (from u in dbUsers
                                      select u.CastToEntity()).ToList();

            return entityUsers;
        }

        public bool IsAlreadyFollowing(int followerId, int userId)
        {
            Database.Models.FollowingRelation relation = db.FollowingRelations.FirstOrDefault(
                u => u.UserId == userId &&
                u.FollowerId == followerId);

            if (relation != null)
            {
                return true;
            }

            return false;
        }

        public List<User> GetUsers(int start, int end)
        {
            var query = (from u in db.Users
                         orderby u.Username descending
                         select u).Skip(start).Take(end - start);

            List<Database.Models.User> dbUsers = query.ToList();
            List<User> entityUsers = (from p in dbUsers
                                      select p.CastToEntity()).ToList();


            return entityUsers;
        }

        public List<User> GetUsers(int end = -1)
        {
            return GetUsers("", end);
        }

        public List<User> GetUsers(string filter)
        {
            return GetUsers(filter, -1);
        }

        public List<User> GetUsers(string filter, int start, int end)
        {
            if (end - start <= start)
            {
                return new List<User>();
            }

            var query = (from u in db.Users
                         where u.Username.ToUpper().Contains(filter.ToUpper())
                         orderby u.Username descending
                         select u).Skip(start).Take(end - start);

            List<Database.Models.User> dbUsers = query.ToList();
            List<User> entityUsers = (from p in dbUsers
                                      select p.CastToEntity()).ToList();


            return entityUsers;
        }

        public List<User> GetUsers(string filter, int end = -1)
        {
            if (end == -1)
            {
                end = db.Users.Count();
            }

            return GetUsers(filter, 0, end);
        }

        public List<User> GetUsers(User filter, int end = -1)
        {
            if (end == -1)
            {
                end = db.Users.Count();
            }

            List<Database.Models.User> dbUsers = (from u in db.Users
                                                  where u.Username.ToUpper().Contains(filter.Username.ToUpper()) &&
                                                  u.Firstname.ToUpper().Contains(filter.Firstname.ToUpper()) &&
                                                  u.Lastname.ToUpper().Contains(filter.Lastname.ToUpper()) &&
                                                  (u.Role == filter.Role)
                                                  select u).Take(end).ToList();

            List<User> entityUsers = (from u in dbUsers
                                      select u.CastToEntity()).ToList();

            return entityUsers;
        }

        public void TogglePromotion(int promoterId, int userId)
        {
            Database.Models.User promoter = db.Users.FirstOrDefault(u => u.Id == promoterId);
            if (promoter.Role != Roles.ADMIN)
                return;

            Database.Models.User user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                if (user.Role == Roles.ADMIN)
                {
                    user.Role = Roles.USER;
                }
                else
                {
                    user.Role = Roles.ADMIN;
                }

                db.Users.Update(user);
                db.SaveChanges();
            }
        }
    }
}
