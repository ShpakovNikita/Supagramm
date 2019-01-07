using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCore;

namespace IgiLab.Models.ViewModels.Admin
{
    public class CommentAggregationModel
    {
        public List<Comment> Comments { get; set; }
        public CommentSearchModel CommentSearchModel { get; set; }

        public CommentAggregationModel()
        {
            Comments = new List<Comment>();
        }
    }
}
