using Entities.Common;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.More
{
    public class Help : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int? ParentHelpId { get; set; }

        public Help ParentHelp { get; set; }

        public ICollection<Help> ChildHelps { get; set; }
    }

    public class HelpConfiguration : IEntityTypeConfiguration<Help>
    {
        public void Configure(EntityTypeBuilder<Help> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).HasMaxLength(1000);

            builder.HasOne(p => p.ParentHelp)
                .WithMany(c => c.ChildHelps)
                .HasForeignKey(p => p.ParentHelpId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}