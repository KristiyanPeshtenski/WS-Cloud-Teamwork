﻿namespace FacebookSystem.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using FacebookSystem.Models;
    using FacebookSystem.Data.Migrations;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class FacebookDbContext : IdentityDbContext<ApplicationUser>, IFacebookDbContext
    {
        public FacebookDbContext()
            :base("FacebookDbConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FacebookDbContext, Configuration>());
        }

        public virtual IDbSet<Post> Posts { get; set; }

        public virtual IDbSet<Group> Groups { get; set; }
        
        public virtual IDbSet<Comment> Comments { get; set; }
        
        public virtual IDbSet<Notification> Notifications { get; set; }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany<ApplicationUser>(u => u.Friends)
                .WithMany()
                .Map(uu =>
                {
                    uu.MapLeftKey("UserId");
                    uu.MapRightKey("FriendId");
                    uu.ToTable("UserFriends");
                });
            
            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.CommentOwner)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.CommentOwnerId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(n => n.Notifications)
                .WithOptional()
                .WillCascadeOnDelete(false);
        }
    }
}
