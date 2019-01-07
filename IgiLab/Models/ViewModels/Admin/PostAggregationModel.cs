using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCore;

namespace IgiLab.Models.ViewModels.Admin
{
    public class PostAggregationModel
    {
        public List<Post> Posts { get; set; }
        public PostSearchModel PostSearchModel { get; set; }

        public PostAggregationModel()
        {
            Posts = new List<Post>();
        }
    }
}
