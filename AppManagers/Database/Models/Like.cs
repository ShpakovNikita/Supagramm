﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppManagers.Database.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}