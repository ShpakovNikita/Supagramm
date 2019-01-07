﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntityCore
{
    public class Like : Entity
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}