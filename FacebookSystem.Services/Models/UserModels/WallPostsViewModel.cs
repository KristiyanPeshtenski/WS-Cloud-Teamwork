﻿namespace FacebookSystem.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FacebookSystem.Models;
    using FacebookSystem.Services.Models.CommentModels;
    using FacebookSystem.Services.Models.GroupModels;

    public class WallPostsViewModel
    {
        public int Id { get; set; }

        public UserViewModelPreview Author { get; set; }

        public UserViewModelPreview WallOwner { get; set; }

        public string PostContent { get; set; }

        public DateTime Date { get; set; }

        public int LikesCount { get; set; }

        public bool Liked { get; set; }

        public int TotalCommentsCount { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static WallPostsViewModel Create(Post p, ApplicationUser currentUser)
        {
            return new WallPostsViewModel()
            {
                Id = p.Id,
                Author = UserViewModelPreview.Create(p.Owner, currentUser),
                WallOwner = UserViewModelPreview.Create(p.Owner, currentUser),
                PostContent = p.Content,
                Date = p.CreatedOn,
                LikesCount = p.Likes.Count,
                Liked = p.Likes
                    .Any(l => l.UserId == currentUser.Id),
                TotalCommentsCount = p.Comments.Count,
                Comments = p.Comments
                    .Take(3)
                    .Select(c => CommentViewModel.CreateCommentAndUser(c, currentUser))
            };
        }
    }
}