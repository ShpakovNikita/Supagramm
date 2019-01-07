using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppManagers.Database.Models
{
    // Not in use now
    public class FollowingRelation
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public int? UserId { get; set; }

        [ForeignKey("FollowerId")]
        public User Follower { get; set; }
        public int? FollowerId { get; set; }
    }
}
