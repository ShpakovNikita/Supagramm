using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityCore;
using AppManagers.Database.Models;

namespace AppManagers.Extensions
{
    public static class UserExtensions
    {
        public static EntityCore.User CastToEntity(this Database.Models.User dbUser)
        {
            if (dbUser == null)
            {
                return null;
            }

            EntityCore.User entityUser = new EntityCore.User();
            entityUser.Id = dbUser.Id;
            entityUser.Email = dbUser.Email;
            entityUser.Password = dbUser.Password;
            entityUser.Username = dbUser.Username;
            entityUser.Lastname = dbUser.Lastname;
            entityUser.Firstname = dbUser.Firstname;
            entityUser.Lastname = dbUser.Lastname;
            entityUser.Role = dbUser.Role;
            entityUser.AvatarPath = dbUser.AvatarPath;
            
            return entityUser;
        }
        
        public static Database.Models.User CastToDatabase(this EntityCore.User entityUser)
        {
            if (entityUser == null)
            {
                return null;
            }

            Database.Models.User dbUser = new Database.Models.User();
            dbUser.Id = entityUser.Id;
            dbUser.Email = entityUser.Email;
            dbUser.Password = entityUser.Password;
            dbUser.Username = entityUser.Username;
            dbUser.Lastname = entityUser.Lastname;
            dbUser.Firstname = entityUser.Firstname;
            dbUser.Lastname = entityUser.Lastname;
            dbUser.Role = entityUser.Role;
            dbUser.AvatarPath = entityUser.AvatarPath;

            return dbUser;
        }
    }
}
