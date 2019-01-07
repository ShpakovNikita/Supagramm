using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace AppManagers.Database.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }

        public DateTime Date { get; set; }
        public List<Like> Likes { get; set; }
    }
}