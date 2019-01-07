using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntityCore
{
    public class Comment : Entity
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public int CommenterId { get; set; }
        public int PostId { get; set; }
    }
}