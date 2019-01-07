using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IgiLab.Models;
using System.ComponentModel.DataAnnotations;
using EntityCore;

namespace IgiLab.Models.ViewModels
{
    public class PostViewComponentModel
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public string UserAvatarPath { get; set; }
        public DateTime Date { get; set; }
        public int Likes { get; set; }
        public bool Liked { get; set; }
        public List<CommentExtendedModel> Comments { get; set; }

        public bool CanDelete { get; set; }

        [Required(ErrorMessage = "Empty Comment!")]
        public string CommentForm { get; set; }

        public PostViewComponentModel() { }

        public PostViewComponentModel(Post post, User user)
        {
            Id = post.Id;
            ImagePath = post.ImagePath;
            Description = post.Description;
            Username = user.Username;
            UserAvatarPath = user.AvatarPath;
            Date = post.Date;
            Likes = post.Likes.Count();
        }
    }
}
