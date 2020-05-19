using System;
using Entities.User;
using Entities.Common;
using Entities.Problem;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Post
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTimeOffset Time { get; set; }
        public int Type { get; set; }
        public long Price { get; set; }
        public bool IsConfirm { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }

        public int CategoryId { get; set; }
        public int StateId { get; set; }
        public int UserId { get; set; }

        public State.State State { get; set; }
        public Category Category { get; set; }
        public User.User User { get; set; }

        public ICollection<PostImage> Images { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
        public ICollection<PostProblem> PostProblems { get; set; }
        public ICollection<View> Views { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }

    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Time).IsRequired();
            builder.Property(p => p.Type).IsRequired();
            builder.Property(p => p.Text).IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.User)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(a => a.StateId).HasName("IX_Post_StateId");
            builder.HasIndex(a => a.CategoryId).HasName("IX_Post_CategoryId");
            builder.HasIndex(a => a.UserId).HasName("IX_Post_UserId");
        }
    }
}