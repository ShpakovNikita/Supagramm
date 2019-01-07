using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCore;

namespace IgiLab.Models.ViewModels.Admin
{
    public class PostSearchModel
    {
        public string DescriptionFilter { get; set; }
        public string OwnerUsername { get; set; }

        public DateTime Date { get; set; }

        public void EmptifyNulls()
        {
            if (String.IsNullOrEmpty(DescriptionFilter))
            {
                DescriptionFilter = "";
            }

            if (String.IsNullOrEmpty(OwnerUsername))
            {
                OwnerUsername = "";
            }

            if (Date == null)
            {
                Date = DateTime.MinValue;
            }
        }

        public Post CastToPost()
        {
            Post post = new Post();

            post.Description = DescriptionFilter;
            post.Date = Date;

            return post;
        }
    }
}
