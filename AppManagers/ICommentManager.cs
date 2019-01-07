using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCore;

namespace AppManagers
{
    public interface ICommentManager
    {
        int Create(Comment entity);
        void Delete(int id);
        void Edit(Comment entity);
        Comment Get(int id);

        List<Comment> GetComments(int end = -1);
        List<Comment> GetComments(int start, int end);
        List<Comment> GetComments(Comment comment, string username, int end);
        List<Comment> GetCommentsByPost(int postId);
    }
}
