﻿using System;
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Post
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; }
        public DateTimeOffset Time { get; set; }
        public int PostId { get; set; }
        public int CreatorId { get; set; }
        public int AnswererId { get; set; }

        public Post Post { get; set; }
        public User.User User { get; set; }
        public User.User UserAnswer { get; set; }
    }

    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(p => p.Text).IsRequired().HasMaxLength(1000);
            builder.Property(p => p.Time).IsRequired();
            builder.Property(p => p.PostId).IsRequired();
            builder.Property(p => p.CreatorId).IsRequired();

            builder.HasOne(p => p.Post)
                .WithMany(c => c.Comments)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.UserAnswer)
                .WithMany(c => c.AnswererComments)
                .HasForeignKey(p => p.AnswererId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.User)
                .WithMany(c => c.StarterComments)
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(a => a.PostId).HasName("IX_Comment_PostId");
            builder.HasIndex(a => a.CreatorId).HasName("IX_Comment_CreatorId");
            builder.HasIndex(a => a.AnswererId).HasName("IX_Comment_AnswererId");
        }
    }
}
