using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCore;

namespace AppManagers
{
    public interface ILikeManager
    {
        int Create(Like entity);
        void Delete(int id);
        void Delete(int likerId, int postId);
        void Edit(Like entity);
        Like Get(int id);
        Like Get(int userId, int postId);
    }
}
