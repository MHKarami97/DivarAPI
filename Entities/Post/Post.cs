using System;
using Entities.User;
using Entities.Common;
using Entities.Problem;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sieve.Attributes;

namespace Entities.Post
{
    public class Post : BaseEntity
    {
        [Sieve(CanFilter = false, CanSort = false)]
        public string Title { get; set; }

        [Sieve(CanFilter = false, CanSort = false)]
        public string Text { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTimeOffset Time { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int Type { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public long Price { get; set; }

        [Sieve(CanFilter = false, CanSort = false)]
        public bool IsConfirm { get; set; }

        [Sieve(CanFilter = false, CanSort = false)]
        public string Phone { get; set; }

        [Sieve(CanFilter = false, CanSort = false)]
        public string Location { get; set; }

        [Sieve(CanFilter = true, CanSort = false)]
        public int CategoryId { get; set; }

        [Sieve(CanFilter = true, CanSort = false)]
        public int StateId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
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