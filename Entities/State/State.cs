using Entities.Common;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.State
{
    public class State : BaseEntity
    {
        public string Name { get; set; }
        public int? ParentStateId { get; set; }

        public State ParentState { get; set; }

        public ICollection<State> ChildStates { get; set; }

        public ICollection<Post.Post> Posts { get; set; }
    }

    public class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);

            builder.HasOne(p => p.ParentState)
                .WithMany(c => c.ChildStates)
                .HasForeignKey(p => p.ParentStateId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(a => a.ParentStateId).HasName("IX_State_ParentStateId");
        }
    }
}