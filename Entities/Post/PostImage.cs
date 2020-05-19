using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Post
{
    public class PostImage : BaseEntity
    {
        public int PostId { get; set; }
        public string Image { get; set; }

        public Post Post { get; set; }
    }

    public class PostImageConfiguration : IEntityTypeConfiguration<PostImage>
    {
        public void Configure(EntityTypeBuilder<PostImage> builder)
        {
            builder.Property(p => p.PostId).IsRequired();
            builder.Property(p => p.Image).IsRequired().HasMaxLength(200);

            builder.HasOne(p => p.Post)
                .WithMany(c => c.Images)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(a => a.PostId).HasName("IX_PostImage_PostId");
        }
    }
}