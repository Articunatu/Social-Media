using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Persistence.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            // Set primary key
            builder.HasKey(c => c.Id);

            // Configure Parent-Child Relationship (Self-referencing)
            builder.HasMany(c => c.Replies)
                   .WithOne()
                   .HasForeignKey(c => c.ParentId)
                   .OnDelete(DeleteBehavior.Cascade);  // Delete all replies if parent is deleted

            // Optional: Set default values or constraints
            builder.Property(c => c.Content)
                   .IsRequired()
                   .HasMaxLength(1000); // Example constraint
        }
    }

}
