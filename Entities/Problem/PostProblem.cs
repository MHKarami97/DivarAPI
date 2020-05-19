using System;
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Problem
{
    public class PostProblem : BaseEntity
    {
        public int PostId { get; set; }
        public int ReasonId { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTimeOffset Time { get; set; }

        public Post.Post Post { get; set; }
        public ProblemReason Reason { get; set; }
    }

    public class PostProblemConfiguration : IEntityTypeConfiguration<PostProblem>
    {
        public void Configure(EntityTypeBuilder<PostProblem> builder)
        {
            builder.Property(p => p.PostId).IsRequired();

            builder.HasOne(p => p.Post)
                .WithMany(c => c.PostProblems)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(a => a.ReasonId).HasName("IX_PostProblem_ReasonId");
            builder.HasIndex(a => a.PostId).HasName("IX_PostProblem_PostId");
        }
    }
}