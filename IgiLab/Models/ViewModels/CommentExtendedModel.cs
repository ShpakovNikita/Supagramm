using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCore;

namespace IgiLab.Models.ViewModels
{
    public class CommentExtendedModel
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public int CommenterId { get; set; }
        public string PosterUsername { get; set; }
        public int PostId { get; set; }

        public CommentExtendedModel(Comment comment)
        {
            Message = comment.Message;
            Date = comment.Date;
            CommenterId = comment.CommenterId;
            PostId = comment.PostId;
        }
    }
}
