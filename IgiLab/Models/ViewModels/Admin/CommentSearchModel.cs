using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCore;

namespace IgiLab.Models.ViewModels.Admin
{
    public class CommentSearchModel
    {
        public string MessageFilter { get; set; }
        public DateTime Date { get; set; }
        public string CommenterUsername { get; set; }

        public void EmptifyNulls()
        {
            if (String.IsNullOrEmpty(MessageFilter))
            {
                MessageFilter = "";
            }

            if (String.IsNullOrEmpty(CommenterUsername))
            {
                CommenterUsername = "";
            }

            if (Date == null)
            {
                Date = DateTime.MinValue;
            }
        }

        public Comment CastToComment()
        {
            Comment comment = new Comment();

            comment.Message = MessageFilter;
            comment.Date = Date;

            return comment;
        }
    }
}
