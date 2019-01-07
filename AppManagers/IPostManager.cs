using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCore;

namespace AppManagers
{
    public interface IPostManager
    {
        int Create(Post entity);
        void Delete(int deleterId, int postId);
        void Edit(Post entity);
        Post Get(int id);

        // Get posts in range, excluding the end element
        List<Post> GetPosts(int start, int end);
        List<Post> GetPosts(int end = -1);
        List<Post> GetPostsByUserId(int userId);
        List<Post> GetPosts(Post filter, string username, int end);
    }
}
