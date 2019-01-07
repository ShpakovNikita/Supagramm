using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCore;

namespace AppManagers
{
    public interface IUserManager
    {
        int Create(User entity);
        void Delete(int id);
        void Edit(User entity);
        User Get(int id);
        
        void Follow(int followerId, int userId);
        void Unfollow(int followerId, int userId);
        bool IsUserExists(int id);
        List<User> GetFollowers(int id);
        List<User> GetFollowings(int id);
        bool IsAlreadyFollowing(int followerId, int userId);
        User GetByAuthData(string username, string password);
        User GetByUsername(string username);

        // Get posts in range, excluding the end element
        List<User> GetUsers(int start, int end);
        List<User> GetUsers(int end = -1);
        List<User> GetUsers(string filter, int start, int end);
        List<User> GetUsers(string filter, int end = -1);
        List<User> GetUsers(User userFilter, int end = -1);
        void TogglePromotion(int promoterId, int userId);
    }
}
