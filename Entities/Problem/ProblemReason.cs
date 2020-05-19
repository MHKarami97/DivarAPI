using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Problem
{
    public class ProblemReason : BaseEntity
    {
        public string Reason { get; set; }
    }

    public class ProblemReasonConfiguration : IEntityTypeConfiguration<ProblemReason>
    {
        public void Configure(EntityTypeBuilder<ProblemReason> builder)
        {
            builder.Property(p => p.Reason).IsRequired().HasMaxLength(100);
        }
    }
}