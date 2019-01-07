using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppManagers
{
    public interface IManagersContainer
    {
        IPostManager GetPostManager();
        IUserManager GetUserManager();
        ICommentManager GetCommentManager();
        ILikeManager GetLikeManager();
    }
}
